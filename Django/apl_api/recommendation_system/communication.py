from rest_framework.decorators import api_view, permission_classes
from rest_framework.response import Response
from rest_framework import status

from apl_api.models import SequenceNumber, ObservedProduct, NewObservedProduct
from apl_api.serializers import SequenceNumberSerializer, ObservedProductSerializer, NewObservedProductSerializer
from rest_framework.permissions import IsAdminUser

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
    if request["sequence_number"] == last_sequence_number.number:
        new_observations = NewObservedProduct.objects.filter(sequence_number=request["sequence_number"])
        serializer = NewObservedProductSerializer(new_observations, many=True)
        return Response(serializer.data, status=status.HTTP_200_OK)
    else:
        Response({'response': 'Wrong sequence number', 'expected_seq_number': last_sequence_number.number}, status=status.HTTP_200_OK)