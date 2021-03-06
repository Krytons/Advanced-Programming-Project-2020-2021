from django.db import models
# Imports used to generate a custom user
from django.contrib.auth.models import AbstractBaseUser, PermissionsMixin, BaseUserManager
from django import forms
from django.utils.translation import ugettext_lazy as _
# Imports used to generate tokens
from django.conf import settings
from django.db.models.signals import post_save
from django.dispatch import receiver
from rest_framework.authtoken.models import Token
# Imports used to use mongoDB collections
from djongo import models
from django.core.validators import MaxValueValidator

# Create your models here
'''
class Price(models.Model):
    old_price = models.DecimalField(max_digits=6, decimal_places=2)
    price_time = models.DateTimeField()

    class Meta:
        abstract = True

    def __str__(self):
        return self.old_price

class Product(models.Model):
    id = models.AutoField(primary_key=True)
    item_id = models.CharField(max_length=20, null=False, unique=True)
    title = models.CharField(max_length=100, null=False)
    subtitle = models.CharField(max_length=100, blank=True)
    category_id = models.CharField(max_length=20, null=False)
    category_name = models.CharField(max_length=100, null=False)
    gallery_url = models.CharField(max_length=200, null=False)
    view_url = models.CharField(max_length=200, null=False)
    shipping_cost =  models.DecimalField(max_digits=6, decimal_places=2)
    price = models.DecimalField(max_digits=6, decimal_places=2)
    condition_id = models.CharField(max_length=20, null=False)
    condition_name = models.CharField(max_length=100, null=False)
    history = models.ArrayField(
        model_container=Price,
        null=True
    )
    created_at = models.DateTimeField(auto_now_add=True)
    updated_at = models.DateTimeField(auto_now=True)

    objects = models.DjongoManager()

    def __str__(self):
        return "Name: " + self.title + "Description: " + self.subtitle
'''

class Product(models.Model):
    id = models.AutoField(primary_key=True)
    item_id = models.CharField(max_length=20, null=False, unique=True)
    title = models.CharField(max_length=100, null=False)
    subtitle = models.CharField(max_length=100, blank=True)
    category_id = models.CharField(max_length=20, null=False)
    category_name = models.CharField(max_length=100, null=False)
    gallery_url = models.CharField(max_length=200, null=False)
    view_url = models.CharField(max_length=200, null=False)
    shipping_cost =  models.DecimalField(max_digits=6, decimal_places=2)
    price = models.DecimalField(max_digits=6, decimal_places=2)
    condition_id = models.CharField(max_length=20, null=False)
    condition_name = models.CharField(max_length=100, null=False)
    created_at = models.DateTimeField(auto_now_add=True)
    updated_at = models.DateTimeField(auto_now=True)

    def __str__(self):
        return "Name: " + self.title + "Description: " + self.subtitle

class CustomAccountManager(BaseUserManager):
    def create_user(self, email, name, surname, nickname, password, **other_fields):
        # Validate email
        if not email:
            raise ValueError('Provide a correct email address')
        if not nickname:
            raise ValueError('Provide a nickname')
        # Lowercasing email
        email = self.normalize_email(email)
        user = self.model(email=email, name=name, surname=surname, nickname=nickname, **other_fields)
        user.set_password(password)
        user.save()
        return user

    def create_superuser(self, email, name, surname, nickname, password, **other_fields):
        other_fields.setdefault('is_staff', True)
        other_fields.setdefault('is_superuser', True)
        # Validate other fields
        if other_fields.get('is_staff') is not True:
            raise ValueError('Superuser must have is_staff = True')
        if other_fields.get('is_superuser') is not True:
            raise ValueError('Superuser must have is_superuser = True')
        return self.create_user(email, name, surname, nickname, password, **other_fields)


class AppUser(AbstractBaseUser, PermissionsMixin):
    email = models.EmailField(_('email'), max_length=40, unique=True)
    name = models.CharField(max_length=40, null=False)
    surname = models.CharField(max_length=40, null=False)
    nickname = models.CharField(max_length=40, unique=True)
    is_staff = models.BooleanField(default=False)
    is_active = models.BooleanField(default=True)
    created_at = models.DateTimeField(auto_now_add=True)

    # Define the use of a custom account manager
    objects = CustomAccountManager()

    USERNAME_FIELD = 'email' #New primary key
    REQUIRED_FIELDS = ['nickname', 'name', 'surname'] #List of fields prompted when using createsuperuser

    def __str__(self):
        return "Nickname: " + self.nickname

    # For checking permissions: all admins have full permissions
    def has_perm(self, perm, obj=None):
        return self.is_staff

    # Easy mode: all users have permission to view this app
    def has_module_perms(self, app_label):
        return True

# This one will be called after a user has been saved
@receiver(post_save, sender=settings.AUTH_USER_MODEL)
def create_auth_token(sender, instance=None, created=False, **kwargs):
    if created: # Each time a user is created, a token will be generated
        Token.objects.create(user=instance)


class ObservedProduct(models.Model):
    creator = models.ForeignKey(AppUser, on_delete=models.CASCADE)
    product = models.ForeignKey(Product, on_delete=models.CASCADE)
    threshold_price = models.DecimalField(max_digits=6, decimal_places=2)

    class Meta:
        unique_together = ['creator', 'product']


class PriceHistory(models.Model):
    product = models.ForeignKey(Product, on_delete=models.CASCADE)
    old_price = models.DecimalField(max_digits=6, decimal_places=2)
    price_time = models.DateTimeField()

    class Meta:
        unique_together = ['product', 'price_time']
        ordering = ['price_time']


class Notification(models.Model):
    observation = models.ForeignKey(ObservedProduct, on_delete=models.CASCADE)
    notified_price = models.DecimalField(max_digits=6, decimal_places=2)
    created_at = models.DateTimeField(auto_now_add=True)
    status = models.CharField(max_length=40, null=False)

    class Meta:
        unique_together = ['observation', 'created_at']
        ordering = ['created_at']


class Recommendation(models.Model):
    user_id = models.ForeignKey(AppUser, on_delete=models.CASCADE)
    product_id = models.ForeignKey(Product, on_delete=models.CASCADE)
    created_at = models.DateTimeField(auto_now_add=True)

    class Meta:
        unique_together = ['user_id', 'product_id']
        ordering = ['created_at']


class SequenceNumber(models.Model):
    number = models.IntegerField(validators=[MaxValueValidator(999)])
    created_at = models.DateTimeField(auto_now_add=True)

    class Meta:
        unique_together = ['number', 'created_at']


class NewObservedProduct(models.Model):
    user_id = models.ForeignKey(AppUser, on_delete=models.CASCADE)
    product = models.ForeignKey(Product, on_delete=models.CASCADE)
    sequence_number = models.IntegerField(validators=[MaxValueValidator(999)])
    created_at = models.DateTimeField(auto_now_add=True)
    operation = models.BooleanField(default=True)

    class Meta:
        unique_together = ['user_id', 'product', 'sequence_number']
        ordering = ['created_at']
