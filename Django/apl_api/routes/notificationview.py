from django.core.exceptions import ObjectDoesNotExist
from django.db.utils import DatabaseError

# Create your views here.
from rest_framework.decorators import api_view, permission_classes
from rest_framework.response import Response
from rest_framework import status
from rest_framework.permissions import IsAuthenticated, IsAdminUser

from apl_api.models import Notification, ObservedProduct
from apl_api.serializers import NotificationSerializer, ObservedProductSerializer



@api_view(['GET'])
@permission_classes([IsAdminUser])
def get_all_notifications(request):
    try:
        observations = Notification.objects.all()
        serializer = NotificationSerializer(observations, many=True)
        return Response(serializer.data, status=status.HTTP_200_OK)
    except ObjectDoesNotExist:
        return Response({'response': 'There are no notifications at the moment'}, status=status.HTTP_400_BAD_REQUEST)


@api_view(['GET'])
@permission_classes([IsAuthenticated])
def get_user_notifications(request):
    try:
        observations = ObservedProduct.objects.filter(creator=request.user.id)
        return_notifications = []
        for observation in observations:
            notifications = Notification.objects.filter(observation=observation.id)
            notifications_serializer = NotificationSerializer(notifications, many=True)
            if notifications_serializer.data:
                for element in notifications_serializer.data:
                    return_notifications.append(element)
            for notification in notifications:
                if notification.status == "NOT-PULLED":
                    notification.status = "PULLED"
                    notification.save()
        return Response(return_notifications, status=status.HTTP_200_OK)
    except ObjectDoesNotExist:
        return Response({'response': 'There are no notifications for this user at the moment'},
                        status=status.HTTP_400_BAD_REQUEST)


@api_view(['GET'])
@permission_classes([IsAuthenticated])
def get_user_not_pulled_notifications(request):
    try:
        observations = ObservedProduct.objects.filter(creator=request.user.id)
        return_notifications = []
        for observation in observations:
            notifications = Notification.objects.filter(observation=observation.id, status="NOT-PULLED")
            notifications_serializer = NotificationSerializer(notifications, many=True)
            if notifications_serializer.data:
                for element in notifications_serializer.data:
                    return_notifications.append(element)
            for notification in notifications:
                    notification.status = "PULLED"
                    notification.save()
        return Response(return_notifications, status=status.HTTP_200_OK)
    except ObjectDoesNotExist:
        return Response({'response': 'There are no not pulled notifications for this user at the moment'},
                        status=status.HTTP_400_BAD_REQUEST)


@api_view(['PUT'])
@permission_classes([IsAuthenticated])
def update_notification(request, pk):
    try:
        notification = Notification.objects.get(id=pk)
        if notification.observation.creator.email == request.user.email:
                try:
                    observation_value = ObservedProduct.objects.get(id=request.data['observation'])
                    if request.user == observation_value.creator:
                        try:
                            serializer = NotificationSerializer(instance=notification, data=request.data)
                            if serializer.is_valid():
                                serializer.save()
                                return Response(serializer.data, status=status.HTTP_200_OK)
                            else:
                                return Response(serializer.errors, status=status.HTTP_400_BAD_REQUEST)
                        except DatabaseError:
                            return Response({'response':'Duplicate notification'}, status=status.HTTP_400_BAD_REQUEST)
                    else:
                        return Response({'response':'You are not authorized to use observations that belongs to '
                                                    'other users'},
                                        status=status.HTTP_400_BAD_REQUEST)
                except ObjectDoesNotExist:
                    return Response({'response': 'This observation does not exist'},
                            status=status.HTTP_400_BAD_REQUEST)
        else:
            return Response({'response': 'You have no permissions to update this notification'},
                            status=status.HTTP_401_UNAUTHORIZED)
    except ObjectDoesNotExist:
        return Response({'response':'This notification does not exist'}, status=status.HTTP_400_BAD_REQUEST)


@api_view(['DELETE'])
@permission_classes([IsAuthenticated])
def delete_notification(request, pk):
    try:
        notification = Notification.objects.get(id=pk)
        if notification.observation.creator.email == request.user.email:
            notification.delete()
            return Response({'response':'Notification successfully deleted'}, status=status.HTTP_200_OK)
        else:
            return Response({'You have no permissions to delete this notification'},
                            status=status.HTTP_401_UNAUTHORIZED)
    except ObjectDoesNotExist:
        return Response({'response':'This notification does not exist'}, status=status.HTTP_400_BAD_REQUEST)
