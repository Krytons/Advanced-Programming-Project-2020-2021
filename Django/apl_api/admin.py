from django.contrib import admin
from .models import *

# Register your models here.
admin.site.register(Product)
admin.site.register(AppUser)
admin.site.register(ObservedProduct)
admin.site.register(PriceHistory)
admin.site.register(Notification)
admin.site.register(Recommendation)
admin.site.register(SequenceNumber)
admin.site.register(NewObservedProduct)