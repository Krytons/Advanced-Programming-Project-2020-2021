from django.urls import path
from .routes.priceview import *
from .views import *
from .ebay import *
from rest_framework.authtoken.views import obtain_auth_token

urlpatterns = [
    path('', home, name='home'),
    path('ping', ping, name='ping'),
    path('register', register_user, name='register'),
    path('login', obtain_auth_token, name='login'),
    path('create_observation', create_observation, name='create_observation'),
    path('get_all_observation', get_all_observation, name='get_all_observation'),
    path('get_user_observation', get_user_observation, name='get_user_observation'),
    path('update_observation/<int:pk>', update_observation, name='update_observation'),
    path('delete_observation/<int:pk>', delete_observation, name='delete_observation'),
    path('ebay_search', ebay_search_products, name='ebay_search'),
    path('ebay_select', ebay_select_product, name='ebay_select'),
    path('ebay_update', ebay_update_observed_product_price, name="ebay_update"),
    path('price/create', create_price, name="price_create"),
    path('price/get_all', get_all_prices, name="price_get_all"),
    path('price/history/<int:pk>', get_product_history, name="price_history"),
    path('price/update/<int:pk>', update_price, name="price_update"),
    path('price/delete/<int:pk>', delete_price, name="price_delete")
]
