# Create your views here.
from django.core.exceptions import ObjectDoesNotExist
from django.http import JsonResponse
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

APP_ID = ""
GLOBAL_ID = "EBAY-IT"

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
        'X-EBAY-SOA-OPERATION-NAME':"findItemsByKeywords"
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
        #The product exists: update it's own info
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

    observation_serializer = ObservedProductSerializer(data=observation)
    if observation_serializer.is_valid():
        if observation_serializer.validated_data['creator'] != request.user:
            return Response({'response': 'You have no permissions to create an observed product for somebody '
                                         'else!'}, status=status.HTTP_401_UNAUTHORIZED)
        else:
            try:
                observation_serializer.save()
                return Response(observation_serializer.data, status=status.HTTP_201_CREATED)
            except DatabaseError:
                return Response({'response': 'This element is already observed'}, status=status.HTTP_400_BAD_REQUEST)
    else:
        return Response(observation_serializer.errors, status=status.HTTP_400_BAD_REQUEST)


@api_view(['POST'])
@permission_classes([IsAuthenticated])
def ebay_update_observed_product_price(request):
    # INFO: This API will be periodically called by the client-app to check the observed_products price
    # Step 1: For every observed product make a request to ebay to check the actual price
    # Step 2: Update products price list in our DB
    # Step 3: Notify the final user if the price drops under the threshold
    return Response({'response': 'This API is under development'}, status=status.HTTP_204_NO_CONTENT)
