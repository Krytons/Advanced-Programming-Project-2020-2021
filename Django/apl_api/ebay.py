# Create your views here.
from django.core.exceptions import ObjectDoesNotExist
from django.http import JsonResponse
from djongo.sql2mongo import SQLDecodeError
from rest_framework.decorators import api_view, permission_classes
from rest_framework.response import Response
from rest_framework import status
from rest_framework.permissions import IsAuthenticated
from apl_api.models import *
from apl_api.serializers import *
from django.db.utils import DatabaseError
from apl_api.middlewares.product_middlewares import product_mapping

import json
import requests
import environ

env = environ.Env()
environ.Env.read_env()
APP_ID = env.str('EBAY_APP_ID')
GLOBAL_ID = env.str('EBAY_GLOBAL_ID')
STATE_CODE = env.str('EBAY_ITALY_CODE')

@api_view(['POST'])
@permission_classes([IsAuthenticated])
def ebay_search_products(request):
    # Step 1: Search on ebay everything like the request search word
    request_data = json.loads(request.body.decode('utf-8'))

    headers = {
        'Content-Type': 'application/json',
        'X-EBAY-SOA-GLOBAL-ID': GLOBAL_ID,
        'X-EBAY-SOA-SECURITY-APPNAME': APP_ID,
        'X-EBAY-SOA-REQUEST-DATA-FORMAT':"JSON",
        'X-EBAY-SOA-OPERATION-NAME':"findItemsByKeywords",
    }
    url = "https://svcs.ebay.com/services/search/FindingService/v1"
    data = {
        'keywords':request_data["search"],
        'paginationInput':{
            'entriesPerPage':request_data["n_items"]
        }
    }
    response = requests.post(url, data=json.dumps(data), headers=headers)
    # Step 2: Obtain a bunch of products from ebay: return those to the final user using ebay format
    if response.status_code == 200:
        json_data = json.loads(response.text)
        response_json = product_mapping(json_data)
        return Response(response_json, status=status.HTTP_200_OK)
        #return Response(json_data, status=status.HTTP_200_OK)
    else:
        return Response({'response': 'Failed to research'}, status=status.HTTP_400_BAD_REQUEST)


@api_view(['POST'])
@permission_classes([IsAuthenticated])
def ebay_select_product(request):
    # Step 1: Search this product on ebay to check it's actual price and information
    request_data = json.loads(request.body.decode('utf-8'))
    search_id = request_data["product"]["item_id"]
    threshold = request_data["threshold_price"]
    user_email = request_data["email"]

    try:
        product = Product.objects.get(item_id = search_id)
        #The product exists: update it's own info '{0:.2f}'.format(pi)
        if product.price != '{0:.2f}'.format(request_data["product"]["price"]):
            print("DIFFERENT PRICE")
            #Different price: it's time to register old price
            price = {
                "product" : product.id,
                "old_price" : product.price,
                "price_time" : product.updated_at
            }
            price_serializer = PriceHistorySerializer(data=price)
            if price_serializer.is_valid():
                price_serializer.save()
            else:
                return Response(price_serializer.errors, status=status.HTTP_400_BAD_REQUEST)
        serializer = ProductSerializer(instance=product, data=request_data["product"])
        if serializer.is_valid():
            serializer.save()
        else:
            return Response(serializer.errors, status=status.HTTP_400_BAD_REQUEST)
    except ObjectDoesNotExist:
        #Insert product into the database
        product_serializer = ProductSerializer(data=request_data["product"])
        if product_serializer.is_valid():
            product_serializer.save()
        else:
            return Response(product_serializer.errors, status=status.HTTP_400_BAD_REQUEST)

    #At this point we can insert our ObservedProduct
    product = Product.objects.get(item_id=search_id)
    observation = {
        'creator': user_email,
        'threshold_price' : threshold,
        'product': product.id
    }
    try:
        returned_sequence = SequenceNumber.objects.latest('created_at')
    except ObjectDoesNotExist:
        return Response('Observation generation error', status=status.HTTP_400_BAD_REQUEST)
    new_observation = {
        'user_id' : request.user.id,
        'product' : product.id,
        'sequence_number' : returned_sequence.number
    }
    observation_serializer = ObservedProductSerializer(data=observation)
    if observation_serializer.is_valid():
        if observation_serializer.validated_data['creator'] != request.user:
            return Response({'response': 'You have no permissions to create an observed product for somebody '
                                         'else!'}, status=status.HTTP_401_UNAUTHORIZED)
        else:
            try:
                observation_serializer.save()
                new_observation_serializer = NewObservedProductSerializer(data=new_observation)
                if new_observation_serializer.is_valid():
                    new_observation_serializer.save()
                    return Response(observation_serializer.data, status=status.HTTP_201_CREATED)
                else:
                    return Response(observation_serializer.data, status=status.HTTP_201_CREATED)
            except SQLDecodeError:
                return Response({'response': 'This element is already observed'}, status=status.HTTP_400_BAD_REQUEST)
    else:
        return Response(observation_serializer.errors, status=status.HTTP_400_BAD_REQUEST)


def ebay_update_observed_product_price():
    # For all products in the database
    products = Product.objects.all()
    for product in products:
        # Search on ebay for an update
        url = "https://open.api.ebay.com/shopping?callname=GetItemStatus&responseencoding=JSON&appid="+ APP_ID \
              +"&siteid=" + STATE_CODE + "&version=967&ItemID=" + product.item_id
        print(url)
        response = requests.get(url)
        #At this point the response is containing the updated product details in ebay format
        if response.status_code == 200:
            print("Ebay call OK")
            json_data = json.loads(response.text)
            #At this point we have a refined product: we can use this one to update our product
            ebay_price = format(json_data["Item"][0]["ConvertedCurrentPrice"]["Value"], '.2f')
            if product.price != ebay_price:
                print("Different Price")
                print(product.price)
                print(ebay_price)
                # Different price: it's time to register old price
                price = {
                    "product": product.id,
                    "old_price": product.price,
                    "price_time": product.updated_at
                }
                price_serializer = PriceHistorySerializer(data=price)
                if price_serializer.is_valid():
                    price_serializer.save()
                product.price = ebay_price
                product.save()
                #Different price means that there may be an user with a proper observation: analysis needed
                try:
                    observations = ObservedProduct.objects.filter(product=product.id)
                    for observation in observations:
                        print("An observation has been found")
                        if (float(ebay_price)) <= (float(observation.threshold_price)):
                            #Create a notification
                            print("Notification time")
                            notification = {
                                "observation" : observation.id,
                                "notified_price" : format(json_data["Item"][0]["ConvertedCurrentPrice"]["Value"], '.2f'),
                                "status" : "NOT-PULLED"
                            }
                            notification_serializer = NotificationSerializer(data=notification)
                            if notification_serializer.is_valid():
                                notification_serializer.save()
                except ObjectDoesNotExist:
                    print("No observations")
