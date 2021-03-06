from django.core.exceptions import ObjectDoesNotExist
from django.db.utils import DatabaseError

# Create your views here.
from rest_framework.decorators import api_view, permission_classes
from rest_framework.response import Response
from rest_framework import status
from rest_framework.permissions import IsAuthenticated, IsAdminUser

from apl_api.models import ObservedProduct, Product, NewObservedProduct, SequenceNumber
from apl_api.serializers import ObservedProductSerializer, ProductSerializer, NewObservedProductSerializer


@api_view(['POST'])
@permission_classes([IsAuthenticated])
def create_observation(request):
    serializer = ObservedProductSerializer(data=request.data)
    if serializer.is_valid():
        if serializer.validated_data['creator'] != request.user:
            return Response({'response': 'You have no permissions to create an observed product for somebody '
                                         'else!'}, status=status.HTTP_401_UNAUTHORIZED)
        else:
            try:
                serializer.save()
                try:
                    returned_sequence = SequenceNumber.objects.latest('created_at')
                    try:
                        new_observation = NewObservedProduct.objects.get(user_id=request.user.id,
                                                                        product=serializer.validated_data["product"].id,
                                                                        sequence_number=returned_sequence.number)
                        new_observation.operation = True
                        new_observation.save()
                    except ObjectDoesNotExist:
                        new_observation = {
                            'user_id': request.user.id,
                            'product': serializer.validated_data["product"].id,
                            'sequence_number': returned_sequence.number,
                            'operation': True
                        }
                        new_observation_serializer = NewObservedProductSerializer(data=new_observation)
                        if new_observation_serializer.is_valid():
                            new_observation_serializer.save()
                except ObjectDoesNotExist:
                    print("Couldn't get a valid Sequence Number")
                return Response(serializer.data, status=status.HTTP_201_CREATED)
            except DatabaseError:
                return Response({'response': 'This element is already observed'}, status=status.HTTP_400_BAD_REQUEST)
    else:
        return Response(serializer.errors, status=status.HTTP_400_BAD_REQUEST)


@api_view(['GET'])
@permission_classes([IsAdminUser])
def get_all_observation(request):
    try:
        observations = ObservedProduct.objects.all()
        serializer = ObservedProductSerializer(observations, many=True)
        return Response(serializer.data, status=status.HTTP_200_OK)
    except ObjectDoesNotExist:
        return Response({'response': 'There are no observations at the moment'}, status=status.HTTP_400_BAD_REQUEST)


@api_view(['GET'])
@permission_classes([IsAuthenticated])
def get_user_observation(request):
    try:
        observations = ObservedProduct.objects.filter(creator=request.user.id)
        serializer = ObservedProductSerializer(observations, many=True)
        return Response(serializer.data, status=status.HTTP_200_OK)
    except ObjectDoesNotExist:
        return Response({'response': 'There are no observations for this user at the moment'},
                        status=status.HTTP_400_BAD_REQUEST)


@api_view(['GET'])
@permission_classes([IsAuthenticated])
def get_user_observation_data_by_id(request, pk):
    try:
        observation = ObservedProduct.objects.get(id=pk)
        if observation.creator.email == request.user.email:
            product = Product.objects.get(id=observation.product_id)
            product_serializer = ProductSerializer(product)
            data = {
                "product": product_serializer.data,
                "threshold_price": observation.threshold_price,
                "email": observation.creator.email
            }
            return Response(data, status=status.HTTP_200_OK)
        else:
            return Response({'response': 'You are not authorized'}, status=status.HTTP_400_BAD_REQUEST)
    except ObjectDoesNotExist:
        return Response({'response': 'There are no observations for this user at the moment'},
                        status=status.HTTP_400_BAD_REQUEST)


@api_view(['GET'])
@permission_classes([IsAuthenticated])
def get_complete_user_observation_data(request):
    try:
        observations = ObservedProduct.objects.filter(creator=request.user.id)
        data = []
        for observation in observations:
            product = Product.objects.get(id = observation.product_id)
            product_serializer =  ProductSerializer(product)
            data.append({
                "product" : product_serializer.data,
                "threshold_price" : observation.threshold_price,
                "email" : observation.creator.email
            })
        return Response(data, status=status.HTTP_200_OK)
    except ObjectDoesNotExist:
        return Response({'response': 'There are no observations for this user at the moment'},
                        status=status.HTTP_400_BAD_REQUEST)


@api_view(['PUT'])
@permission_classes([IsAuthenticated])
def update_observation(request, pk):
    try:
        observation = ObservedProduct.objects.get(id=pk)
        if observation.creator.email == request.user.email:
            serializer = ObservedProductSerializer(instance=observation, data=request.data)
            if serializer.is_valid():
                if request.user == serializer.validated_data['creator']:
                    try:
                        serializer.save()
                        return Response(serializer.data, status=status.HTTP_200_OK)
                    except DatabaseError:
                        return Response({'response':'Duplicate observation'}, status=status.HTTP_400_BAD_REQUEST)
                else:
                    return Response({'response':'You are not authorized to modify your request with somebody else '
                                                'email'}, status=status.HTTP_400_BAD_REQUEST)
            else:
                return Response(serializer.errors, status=status.HTTP_400_BAD_REQUEST)
        else:
            return Response({'response': 'You have no permissions to update this observation'},
                            status=status.HTTP_401_UNAUTHORIZED)
    except ObjectDoesNotExist:
        return Response({'response':'This observation does not exist'}, status=status.HTTP_400_BAD_REQUEST)


@api_view(['PUT'])
@permission_classes([IsAuthenticated])
def update_observation_by_product_id(request, pk):
    try:
        observation = ObservedProduct.objects.get(product=pk, creator=request.user.id)
        if observation.creator.email == request.user.email:
            serializer = ObservedProductSerializer(instance=observation, data=request.data)
            if serializer.is_valid():
                if request.user == serializer.validated_data['creator']:
                    try:
                        serializer.save()
                        return Response(serializer.data, status=status.HTTP_200_OK)
                    except DatabaseError:
                        return Response({'response':'Duplicate observation'}, status=status.HTTP_400_BAD_REQUEST)
                else:
                    return Response({'response':'You are not authorized to modify your request with somebody else '
                                                'email'}, status=status.HTTP_400_BAD_REQUEST)
            else:
                return Response(serializer.errors, status=status.HTTP_400_BAD_REQUEST)
        else:
            return Response({'response': 'You have no permissions to update this observation'},
                            status=status.HTTP_401_UNAUTHORIZED)
    except ObjectDoesNotExist:
        return Response({'response':'This observation does not exist'}, status=status.HTTP_400_BAD_REQUEST)


@api_view(['DELETE'])
@permission_classes([IsAuthenticated])
def delete_observation(request, pk):
    try:
        observation = ObservedProduct.objects.get(id=pk)
        if observation.creator.email == request.user.email:
            returned_sequence = SequenceNumber.objects.latest('created_at')
            try:
                new_observation = NewObservedProduct.objects.get(user_id=request.user.id, product=observation.product.id,
                                                                 sequence_number=returned_sequence.number)
                new_observation.delete()
            except ObjectDoesNotExist:
                new_observation = {
                    'user_id': request.user.id,
                    'product': observation.product.id,
                    'sequence_number': returned_sequence.number,
                    'operation': False
                }
                new_observation_serializer = NewObservedProductSerializer(data=new_observation)
                if new_observation_serializer.is_valid():
                    new_observation_serializer.save()
            observation.delete()
            return Response({'response':'Observation successfully deleted'}, status=status.HTTP_200_OK)
        else:
            return Response({'You have no permissions to delete this observation'}, status=status.HTTP_401_UNAUTHORIZED)
    except ObjectDoesNotExist:
        return Response({'response':'This observation does not exist'}, status=status.HTTP_400_BAD_REQUEST)


@api_view(['DELETE'])
@permission_classes([IsAuthenticated])
def delete_observation_by_product_id(request, pk):
    try:
        print(pk)
        product = Product.objects.get(id=pk)
        print (product)
        observation = ObservedProduct.objects.get(product=product)
        if observation.creator.email == request.user.email:
            returned_sequence = SequenceNumber.objects.latest('created_at')
            try:
                new_observation = NewObservedProduct.objects.get(user_id=request.user.id, product=observation.product.id,
                                                                 sequence_number=returned_sequence.number)
                new_observation.delete()
            except ObjectDoesNotExist:
                new_observation = {
                    'user_id': request.user.id,
                    'product': product.id,
                    'sequence_number': returned_sequence.number,
                    'operation': False
                }
                new_observation_serializer = NewObservedProductSerializer(data=new_observation)
                if new_observation_serializer.is_valid():
                    new_observation_serializer.save()
            observation.delete()
            return Response({'response':'Observation successfully deleted'}, status=status.HTTP_200_OK)
        else:
            return Response({'You have no permissions to delete this observation'}, status=status.HTTP_401_UNAUTHORIZED)
    except ObjectDoesNotExist:
        return Response({'response':'This observation does not exist'}, status=status.HTTP_400_BAD_REQUEST)