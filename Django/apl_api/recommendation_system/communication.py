from json import JSONDecodeError

from django.core.exceptions import ObjectDoesNotExist
from rest_framework.decorators import api_view, permission_classes
from rest_framework.response import Response
from rest_framework import status

from apl_api.models import SequenceNumber, ObservedProduct, NewObservedProduct, Recommendation, Product
from apl_api.serializers import SequenceNumberSerializer, ObservedProductSerializer, NewObservedProductSerializer, \
    RecommendationSerializer, ProductSerializer
from rest_framework.permissions import IsAdminUser, IsAuthenticated

import json

@api_view(['POST'])
@permission_classes([IsAdminUser])
def send_all_observations(request):
    """
    This route must use numpy to return a product-user matrix, generated using all the observations
    """
    observations = ObservedProduct.objects.all();
    serializer = ObservedProductSerializer(observations, many=True)
    return Response(serializer.data, status=status.HTTP_200_OK)


@api_view(['POST'])
@permission_classes([IsAdminUser])
def send_all_new_observations(request):
    last_sequence_number = SequenceNumber.objects.latest('created_at')
    request_body = json.loads(request.body)
    if request_body["sequence_number"] == last_sequence_number.number:
        #Step 1: delete old observations from NewObservedProduct table
        if last_sequence_number.number != 0:
            old_value = last_sequence_number.number - 1
        else:
            old_value = 999
        NewObservedProduct.objects.filter(sequence_number=old_value).delete()
        #Step 2: generate a new sequence number
        if last_sequence_number.number != 999:
            new_value = last_sequence_number.number + 1
        else:
            new_value = 0
        new_sequence_number = {
            'number' : new_value
        }
        sequence_serializer = SequenceNumberSerializer(data=new_sequence_number)
        if sequence_serializer.is_valid():
            sequence_serializer.save()
        else:
            return Response(sequence_serializer.errors, status=status.HTTP_400_BAD_REQUEST)
        # Step 3: obtain all new observed product
        new_observations = NewObservedProduct.objects.filter(sequence_number=request_body["sequence_number"])
        serializer = NewObservedProductSerializer(new_observations, many=True)
        return Response(serializer.data, status=status.HTTP_200_OK)
    else:
        return Response({'response': 'Wrong sequence number', 'expected_seq_number': last_sequence_number.number},
                  status=status.HTTP_400_BAD_REQUEST)


@api_view(['POST'])
@permission_classes([IsAdminUser])
def add_recommendations(request):
    try:
        request_body = json.loads(request.body)
        for recommendation_received in request_body["recommendations"]:
            try:
                user_recommendations = Recommendation.objects.filter(user_id=recommendation_received["user_id"])
                user_product_list = []
                for recommendation in user_recommendations:
                    user_product_list.append(recommendation.product_id)
                actual_set = set(user_product_list)
                received_set = set(recommendation_received["products"])
                new_products_set = received_set.difference(actual_set)
                old_products_set = actual_set.difference(received_set)
                new_products_list = list(new_products_set)
                old_products_list = list(old_products_set)
                for new_product in new_products_list:
                    add_recommendation(new_product)
                for old_product in old_products_list:
                    remove_recommendation(old_product)
            except ObjectDoesNotExist:
                for new_product in recommendation_received["products"]:
                    add_recommendations(new_product)
        return Response({'response': 'procedure successfully completed'}, status=status.HTTP_200_OK)
    except JSONDecodeError:
        return Response({'response': 'received nothing'}, status=status.HTTP_400_BAD_REQUEST)


def add_recommendation(new_product):
    new_recommendation = {
        "user_id": 2,
        "product_id": new_product,
    }
    recommendation_serializer = RecommendationSerializer(data=new_recommendation)
    if recommendation_serializer.is_valid():
        recommendation_serializer.save()


def remove_recommendation(old_product):
    recommendation = Recommendation.objects.get(product_id=old_product)
    recommendation.delete()



@api_view(['GET'])
@permission_classes([IsAuthenticated])
def return_complete_recommendations_info(request):
    recommended_products = []
    try:
        recommendations = Recommendation.objects.filter(user_id=request.user.id)
        for recommendation in recommendations:
            product = Product.objects.get(id=recommendation.product_id.id)
            product_serializer = ProductSerializer(product)
            recommended_products.append(product_serializer.data)
        return Response(recommended_products, status=status.HTTP_200_OK)
    except ObjectDoesNotExist:
        return Response({'response': 'There are no recommendations for this user at the moment'},
                        status=status.HTTP_400_BAD_REQUEST)

'''
recommendations:
    [
        {  
            "user_id" : 1,
            "products" : [1,2,3,4]
        }
    ]
'''