# Create your views here.
from rest_framework.decorators import api_view, permission_classes
from rest_framework.response import Response
from rest_framework import status
from rest_framework.permissions import IsAuthenticated
from django.http import JsonResponse
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
        return Response(json_data, status=status.HTTP_200_OK)
    else:
        return Response({'response': 'Failed to research'}, status=status.HTTP_400_BAD_REQUEST)


@api_view(['POST'])
@permission_classes([IsAuthenticated])
def ebay_select_product(request):
    # Step 1: Search this product on ebay to check it's actual price and informations
    request_data = json.loads(request.body.decode('utf-8'))
    search_id = request_data["search_id"]

    # Step 2: If the product is already in our product DB then update actual price or add this price to the price list
    # Step 3: Return full product information to the final user
    return Response({'response': 'This API is under development'}, status=status.HTTP_204_NO_CONTENT)

@api_view(['POST'])
@permission_classes([IsAuthenticated])
def ebay_update_observed_product_price(request):
    # INFO: This API will be periodically called by the client-app to check the observed_products price
    # Step 1: For every observed product make a request to ebay to check the actual price
    # Step 2: Update products price list in our DB
    # Step 3: Notify the final user if the price drops under the threshold
    return Response({'response': 'This API is under development'}, status=status.HTTP_204_NO_CONTENT)
