from django.apps import AppConfig

class AplApiConfig(AppConfig):
    name = 'apl_api'
    def ready(self):
        from .scheduler import scheduler
        scheduler.start()