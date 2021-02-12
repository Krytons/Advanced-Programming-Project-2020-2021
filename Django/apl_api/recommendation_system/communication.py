from rest_framework.decorators import api_view, permission_classes
from rest_framework.response import Response
from rest_framework import status

from apl_api.models import SequenceNumber, ObservedProduct, NewObservedProduct
from apl_api.serializers import SequenceNumberSerializer, ObservedProductSerializer, NewObservedProductSerializer
from rest_framework.permissions import IsAdminUser

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