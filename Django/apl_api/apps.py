from django.apps import AppConfig

class AplApiConfig(AppConfig):
    name = 'apl_api'
    def ready(self):
        from .scheduler import scheduler
        from apl_api.models import SequenceNumber
        from apl_api.serializers import SequenceNumberSerializer
        from django.core.exceptions import ObjectDoesNotExist
        scheduler.start()
        try:
            returned_sequence = SequenceNumber.objects.latest('created_at')
            print("Sequence number exists: no need to create one")
        except ObjectDoesNotExist:
            sequence = {
                'number' : 0
            }
            serializer = SequenceNumberSerializer(data=sequence)
            if serializer.is_valid():
                serializer.save()
                print("A new sequence number has been saved")


