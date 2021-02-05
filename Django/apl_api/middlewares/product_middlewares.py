import json
from apl_api.models import Product
from apl_api.serializers import ProductSerializer

def product_mapping(ebay_json):
    product_array = []
    for ebay_product in ebay_json["findItemsByKeywordsResponse"][0]["searchResult"][0]["item"]:
        new_product = Product()
        new_product.item_id = ebay_product["itemId"][0]
        new_product.title = ebay_product["title"][0]
        try:
            new_product.subtitle = ebay_product["subtitle"][0]
        except KeyError:
            new_product.subtitle = ""
        new_product.category_id = ebay_product["primaryCategory"][0]["categoryId"][0]
        new_product.category_name = ebay_product["primaryCategory"][0]["categoryName"][0]
        new_product.gallery_url = ebay_product["galleryURL"][0]
        new_product.view_url = ebay_product["viewItemURL"][0]
        new_product.shipping_cost = ebay_product["shippingInfo"][0]["shippingServiceCost"][0]["__value__"]
        new_product.price = ebay_product["sellingStatus"][0]["currentPrice"][0]["__value__"]
        new_product.condition_id = ebay_product["condition"][0]["conditionId"][0]
        new_product.condition_name = ebay_product["condition"][0]["conditionDisplayName"][0]
        product_array.append(new_product)
    serializer = ProductSerializer(product_array, many=True)
    return serializer.data