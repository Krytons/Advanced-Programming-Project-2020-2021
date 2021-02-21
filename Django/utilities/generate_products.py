import requests, json, getpass


email = input('Enter your admin email: ')
password = getpass.getpass('Enter your password: ')
port = input('Enter Django server port number: ')
login_headers = {'content-type': 'application/json'}
login_pload = {
    "username" : email,
    "password" : password
}
response = requests.post('http://localhost:'+port+'/login', data=json.dumps(login_pload), headers=login_headers)
if response.status_code == 200:
    print("Login: success")
    token = response.json()["token"]
    with open('products.json', encoding="utf8") as json_file:
        data = json.load(json_file)
        for product in data["products"]:
            print("Searching for: " + product)
            ebay_headers = {
                'content-type': 'application/json',
                'authorization' : 'token {}'.format(token)
            }
            ebay_pload = {
                "search": product,
                "n_items": 1
            }
            ebay_first_response = requests.post('http://localhost:' + port + '/ebay_search', data=json.dumps(ebay_pload), headers=ebay_headers)
            if ebay_first_response.status_code == 200:
                ebay_product = ebay_first_response.json()
                product_to_insert = {
                    "item_id": ebay_product[0]['item_id'],
                    "title": ebay_product[0]['title'],
                    "subtitle": ebay_product[0]['subtitle'],
                    "category_id": ebay_product[0]['category_id'],
                    "category_name": ebay_product[0]['category_name'],
                    "gallery_url": ebay_product[0]['gallery_url'],
                    "view_url": ebay_product[0]['view_url'],
                    "shipping_cost": ebay_product[0]['shipping_cost'],
                    "price": ebay_product[0]['price'],
                    "condition_id": ebay_product[0]['condition_id'],
                    "condition_name": ebay_product[0]['condition_name']
                }
                ebay_second_response = requests.post('http://localhost:' + port + '/api/products/', data=json.dumps(product_to_insert), headers=ebay_headers)
                if ebay_second_response.status_code == 201:
                    print("Inserted")
                else:
                    print("Insert failed")
        json_file.close()
        print("Product insert is over")
else:
    print (response.json())

