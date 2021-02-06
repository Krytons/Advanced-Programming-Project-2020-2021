from django.core.exceptions import ObjectDoesNotExist
from django.db.utils import DatabaseError

# Create your views here.
from rest_framework.decorators import api_view, permission_classes
from rest_framework.response import Response
from rest_framework import status
from rest_framework.permissions import IsAuthenticated, IsAdminUser

from apl_api.models import PriceHistory
from apl_api.serializers import PriceHistorySerializer


@api_view(['POST'])
@permission_classes([IsAdminUser])
def create_price(request):
    serializer = PriceHistorySerializer(data=request.data)
    if serializer.is_valid():
        try:
            serializer.save()
            return Response(serializer.data, status=status.HTTP_201_CREATED)
        except DatabaseError:
            return Response({'response': 'Duplicate price-date combination'}, status=status.HTTP_400_BAD_REQUEST)
    else:
        return Response(serializer.errors, status=status.HTTP_400_BAD_REQUEST)


@api_view(['GET'])
@permission_classes([IsAdminUser])
def get_all_prices(request):
    try:
        history = PriceHistory.objects.all()
        serializer = PriceHistorySerializer(history, many=True)
        return Response(serializer.data, status=status.HTTP_200_OK)
    except ObjectDoesNotExist:
        return Response({'response': 'There are no histories at the moment'}, status=status.HTTP_400_BAD_REQUEST)


@api_view(['GET'])
@permission_classes([IsAuthenticated])
def get_product_history(request,pk):
    try:
        history = PriceHistory.objects.filter(product=pk)
        serializer = PriceHistorySerializer(history, many=True)
        return Response(serializer.data, status=status.HTTP_200_OK)
    except ObjectDoesNotExist:
        return Response({'response': 'There are no histories for this product at the moment'},
                        status=status.HTTP_400_BAD_REQUEST)

@api_view(['PUT'])
@permission_classes([IsAdminUser])
def update_price(request, pk):
    try:
        price = PriceHistory.objects.get(id=pk)
        serializer = PriceHistorySerializer(instance=price, data=request.data)
        if serializer.is_valid():
            try:
                serializer.save()
                return Response(serializer.data, status=status.HTTP_200_OK)
            except DatabaseError:
                return Response({'response':'Duplicate price-date combination'}, status=status.HTTP_400_BAD_REQUEST)
        else:
            return Response(serializer.errors, status=status.HTTP_400_BAD_REQUEST)
    except ObjectDoesNotExist:
        return Response({'response':'This Price History does not exist'}, status=status.HTTP_400_BAD_REQUEST)


@api_view(['DELETE'])
@permission_classes([IsAdminUser])
def delete_price(request, pk):
    try:
        price = PriceHistory.objects.get(id=pk)
        price.delete()
        return Response({'response':'Observation successfully deleted'}, status=status.HTTP_200_OK)
    except ObjectDoesNotExist:
        return Response({'response':'This observation does not exist'}, status=status.HTTP_400_BAD_REQUEST)