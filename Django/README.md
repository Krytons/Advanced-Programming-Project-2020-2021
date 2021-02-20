# Django Backend 

<p align="center" style="font-size: 24px">
  <span> English </span> |
  <a href="https://github.com/Krytons/Progetto3ACarusoFallicaDSBD/blob/main/README.it.md">Italiano</a>
</p>

## Advanced Programming Languages 2020/2021
### Developed by:
- **Bartolomeo Caruso**
- **Giuseppe Fallica**
- **Gabriele Costanzo**

---

## 1. What we used
In order to develop this backend we have used the following elements:
- **Django web framework:** A high-level Python web framework that makes development clean, fast and scalable.
- **Django REST framework:** Powerful and flexible Django toolkit for building web APIs.
- **MongoDB:** General purpose, distributed, document-based database built form modern applications.
- **Djongo database mapper:** Extension of Django ORM, used to construct queries using an extremely friendly syntax.
- **Django-environ:** Package used to handle .env variables.
- **Django-apscheduler:** Django package that enables storing persistent jobs in the database.

---

## 2. Backend setup
To correctly setup and use this backend you must create a new virtual environment, then follow the steps below:
- **Start MongoDB:**
  ```bash
    $ sudo systemctl start mongod
  ```
- **Inside "Django" create a .env file with the following arguments:**
  ```dotenv
    EBAY_APP_ID=#Your ebay app id
    EBAY_GLOBAL_ID=#Ebay store id such as "EBAY-IT"
    EBAY_ITALY_CODE=101
    PERIODIC_UPDATE=#Period of "ebay price update" job, expressed in seconds 
    MONGO_HOST=#Your MongoDB host
    MONGO_PORT=#Your MongoDB port
    MONGO_DB_NAME=#Your MongoDB name
  ```
- **Open a terminal into "Django" project folder, and execute the following commands:**
  ```bash
    $ python3 manage.py makemigrations
    $ python3 manage.py migrate
  ```
- **Create a superuser using the terminal:**
  ```bash
    $ python3 manage.py createsuperuser
  ```
  
If your database is empty you can use "Django/utilities/insertscript" to insert all the products contained inside
 "Django/utilities/products.txt".
In order to use correctly our insert script, open a terminal inside "Django/utilities" and write the following command: 
```bash
$ python3 insertscript.py
```

---

## 3. Models

To store and manage all the useful information inside our MongoDB database we created some custom models
extending Django Model class.
By extending Django Model class, MongoDB tables will be generated using the commands "makemigration" and "migrate" as
described in "Django Setup" chapter.

For each model we have generated a proper serializer class, by extending Django Rest Framework ModelSerializer.
Serializers are used to provide a way of serializing and deserializing the model instances into representations such
as json.

We've declared the following models:
- **Products:** this model is used to store ebay's products info, such us title and price.

    Using ProductSerializer we are able to serialize a Product instance as shown down below: 
    ```JSON
      {
        "id": 1,
        "item_id": "402697978785",
        "title": "Nintendo DS Lite Nero con R4 + 40 Giochi Preinstallati ",
        "subtitle": "Leggero e portabile",
        "category_id": "139971",
        "category_name": "Console",
        "gallery_url": "https://thumbs2.ebaystatic.com/m/mQSTsb3LhaQHqyNC3jBuLJg/140.jpg",
        "view_url": "https://www.ebay.it/itm/Nintendo-DS-Lite-Nero-con-R4-40-Giochi-Preinstallati-/402697978785",
        "shipping_cost": "8.00",
        "price": "38.90",
        "condition_id": "3000",
        "condition_name": "Usato",
        "created_at": "2021-02-20T00:24:50.765+00:00",
        "updated_at": "2021-02-20T00:24:50.765+00:00"
      }
    ```
  
- **ObservedProduct:** this model is used to store an user's desired price for a certain product.

    Using ObservedProductSerializer we are able to serialize a ObservedProduct instance as shown down below: 
    ```JSON
      {
        "id": 1,
        "creator": 2,
        "product": 1,
        "threshold_price": 2.50
      }
    ```

- **PriceHistory:** this model is used to store a previous ebay's product price, in order to generate a price graph
 inside the user's app.

    Using PriceHistorySerializer we are able to serialize a PriceHistory instance as shown down below: 
    ```JSON
      {
        "id": 1,
        "product": 1,
        "old_price": "5.20",
        "price_time": "2021-02-14T23:43:14.635000Z"
      }
    ```

- **Notification:** this model is used to store all the notifications created for a certain user. A notification is
 generated when a product's price drops below an observation's price threshold.

    Using NotificationSerializer we are able to serialize a Notification instance as shown down below: 
    ```JSON
      {
        "observation": 1,
        "notified_price": 4.35,
        "created_at": "2021-02-14T21:13:16.625000Z",
        "status": "NOT-PULLED"
      } 
    ```

- **Recommendation:** this model is used to store all the recommended products for a certain user returned by our
 recommendation system.
 
    Using RecommendationSerializer we are able to serialize a Recommendation instance as shown down below: 
    ```JSON
      {
        "id": 1,
        "user_id_id": 2,
        "product_id": 1,
        "created_at": "2021-02-17T15:29:07.521+00:00"
      } 
    ```

- **SequenceNumber:** this model is used to store all the sequence number generated by our backend. Each sequence
 number identifies a certain amount of new observations that must be sent to our recommendation system. 
 
    Using SequenceNumberSerializer we are able to serialize a SequenceNumber instance as shown down below: 
    ```JSON
      {
        "id": 2,
        "number": 1,
        "created_at": "2021-02-16T15:46:22.092+00:00"
      } 
    ```

- **NewObservedProduct:** this model is used to store all the new observations that must be sent to our
 recommendation system. When our backend receives from the recommendation system a new sequence number that is the
  same as the expected one, all the previous new observations will be removed (Read "Recommendation system
   communication" chapter for more info). 
   
    Using NewObservedProductSerializer we are able to serialize a NewObservedProduct instance as shown down below: 
    ```JSON
      {
        "id": 1,
        "user_id_id": 2,
        "product_id": 1,
        "sequence_number": 0,
        "created_at": "2021-02-14T23:43:14.746+00:00"
      }
    ```

- **AppUser:** this model is used to store all user's information, such us email and password. For this particular
 model we have extended AbstractBaseUser class (Read "Permission" chapter for more info). 

    Using RegistrationSerializer we are able to serialize an AppUser instance (without showing password field) as shown
     down below: 
    ```JSON
      {
        "response": "Registration was successful",
        "email": "caruso.bartolomeo@virgilio.it",
        "name": "Bartolomeo",
        "surname": "Caruso",
        "nickname": "Krytons",
        "token": "1ab7376f60dcae04d2dfb0cea04ba15ab921d473"
      }
    ```

---

## 4. Auth and permissions
For our backend we defined two kind of AppUser:
- **Admin AppUser:** this AppUser has "is_staff = True". He can be created by using the command `$ python3 manage.py
 createsuperuser` inside Django backend terminal: this command will use `create_superuser` function defined inside
  CustomAccountManager class (extension of BaseUserManager class).
  
    An admin AppUser has `[IsAdminUser]` permission level, so he is able to do the following actions:
    - **Manage products** 
    - **Use Django admin panel**
    - **Manage Django jobs**
    - **Use insert script**

- **Normal AppUser:** this AppUser has "is_staff = False". He can be created by using the standard sign in procedure.
    
    A normal AppUser has `[IsAuthenticated]` permission level, so he is not able to use `[IsAdminUser]` views, but he
     is still able to do the following actions:
    - **Manage it's own observation**
    - **Manage it's own notification**
    - **Use ebay search services**

When an AppUser successfully logins, a Django Rest Framework token will be generated using the AppUser id.
In order to use our backend services, all the http requests must contain an **Authorization header** as shown
 down below:
```JSON
  {
    "authorization" : "Token 1ab7376f60dcae04d2dfb0cea04ba15ab921d473"
  }
```

Only login and sign in endpoints don't require an Authorization header.
    
---

## 5. Views
Our backend exposes different types of endpoints:
- **Ebay services endpoints:**
    - `POST /ebay_search`: this endpoint is used to do a search using ebay API, and return a list of products.
    
        An example of the required body for this endpoint is shown down below:
        ```JSON
          {
            "search": "Gameboy advance",
            "n_items": 10
          }
        ```
    - `POST /ebay_select`: this endpoint is used to generate an ObservedProduct and a NewObservedProduct by selecting a
     product previously returned by "/ebay_search" endpoint.
     
        An example of the required body for this endpoint is shown down below:
        ```JSON
          {
            "product": {
                "item_id": "133571359505",
                "title": "Apple iPhone 12 5G 64GB NUOVO Originale Smartphone iOS Black ",
                "subtitle": "PROMO-DISPONIBILITA' LIMITATA-PAGA ANCHE ALLA CONSEGNA",
                "category_id": "9355",
                "category_name": "Cellulari e smartphone",
                "gallery_url": "https://thumbs2.ebaystatic.com/m/mdHO0w-IinTVtloOXrtOFNg/140.jpg",
                "view_url": "https://www.ebay.it/itm/Apple-iPhone-12-5G-64GB-NUOVO-Originale-Smartphone-iOS-Black-/133571359505",
                "shipping_cost": "0.00",
                "price": 799.90,
                "condition_id": "1000",
                "condition_name": "Nuovo"
            },
            "threshold_price": "600.00",
            "email": "caruso.bartolomeo@virgilio.it"
        }
        ```
   
- **Notifications endpoints:**
    - `GET /notifications/get_all`: this endpoint is used by an admin to obtain all users notifications.
    
        A response example for this endpoint is shown down below:
        ```JSON
          {
            "id": 1,
            "observation": 2,
            "notified_price": "336.99",
            "status": "PULLED"
          }
        ```
    - `GET /notifications/user`: this endpoint is used to get all notifications of the user that makes the request.
    App user token is used to retrieve the user id required to understand which user's notification must be
     returned.
     If a notification has a "NOT-PULLED" status, it will be modified to "PULLED" after the use of this endpoint.   
    - `PUT /notifications/update/{notification_id}`: this endpoint is used to modify a notification. An App User can
     modify only it's own notifications.
     
        An example of the required body for this endpoint is shown down below:
        ```JSON
          {
            "observation": 1,
            "notified_price": "644.99",
            "status": "NOT-PULLED"
          }
        ```
    - `DEL /notifications/delete/{notification_id}`: this endpoint is used to delete a notification. An App User can
     delete only it's own notifications.
    - `GET /notifications/user/not_pulled`: this endpoint is used to retrieve all "NOT-PULLED" notifications of the
     user that makes the request.
     This endpoint is periodically used by our client application in order to obtain all the new notification for the
      logged user.

- **Observations endpoints:**
    - `POST /create_observation`: this endpoint is used to generate an ObservedProduct for a certain app user.
      
       An example of the required body for this endpoint is shown down below:
        ```JSON
          {
            "creator":"caruso.bartolomeo@virgilio.it",
            "product":"1",
            "threshold_price":"10"
          }
        ```
    - `GET /get_all_observation`: this endpoint is used by an admin to obtain all users ObservedProduct.      
    - `GET /get_user_observation`: this endpoint is used by an AppUser to obtain all its ObservedProduct. 
    - `GET /get_user_observation_data_by_id/{observation_id}`: this endpoint is used by an AppUser to obtain complete
     information about a certain own ObservedProduct.
     
        A response example for this endpoint is shown down below:
        ```JSON
          {
            "product": {
                "id": 2,
                "item_id": "133571359505",
                "title": "Apple iPhone 12 5G 64GB NUOVO Originale Smartphone iOS Black",
                "subtitle": "PROMO-DISPONIBILITA' LIMITATA-PAGA ANCHE ALLA CONSEGNA",
                "category_id": "9355",
                "category_name": "Cellulari e smartphone",
                "gallery_url": "https://thumbs2.ebaystatic.com/m/mdHO0w-IinTVtloOXrtOFNg/140.jpg",
                "view_url": "https://www.ebay.it/itm/Apple-iPhone-12-5G-64GB-NUOVO-Originale-Smartphone-iOS-Black-/133571359505",
                "shipping_cost": "0.00",
                "price": "789.90",
                "condition_id": "1000",
                "condition_name": "Nuovo",
                "created_at": "2021-02-14T23:52:20.895000Z",
                "updated_at": "2021-02-14T23:56:40.091000Z"
            },
            "threshold_price": "2000.00",
            "email": "giuseppe.fallica@gmail.com"
          }
        ```
    - `GET /get_complete_user_observation_data`: this endpoint is used by an AppUser to obtain complete information
     about all it's own ObservedProduct.
    - `PUT /update_observation/{observation_id}`: this endpoint is used to update an ObservedProduct of certain id.
        
        A example of the required body for this endpoint is shown down below:
         ```JSON
            {
                "creator":"caruso.bartolomeo@virgilio.it",
                "product":"1",
                "threshold_price":"179.00"
            }
         ```
        
        If an AppUser tries to update an ObservedProduct that belongs to another AppUser, an error message will be returned.
    - `DEL /delete_observation/{observation_id}`: this endpoint is used to delete an ObservedProduct of certain id. If an
     AppUser tries to delete an ObservedProduct that belongs to another AppUser, an error message will be returned.
    - `DEL /delete_observation_by_product_id/{product_id}`: this endpoint acts like the previous one, but it
     requires a product id. If an ObservedProduct that contains the product id and that belongs to the AppUser who makes the
      request is found, it will be deleted. 

- **Price history endpoints:**
    - `POST /price/create`: this endpoint is used to insert a price history inside the database.
    
        An example of the required body for this endpoint is shown down below:
        ```JSON
          {
            "product": 1,
            "old_price": "614.99",
            "price_time": "2021-02-08T17:33:14.280000Z"
          } 
        ```
    - `GET /price/get_all`: this endpoint is used to obtain all the price histories inside the database.
    - `GET /price/history/{price_history_id}`: this endpoint is used to obtain all price histories of a certain product.
    
        A response example for this endpoint is shown down below:
        ```JSON
        [{
            "id": 1,
            "product": 1,
            "old_price": "5.20",
            "price_time": "2021-02-14T23:43:14.635000Z"
        },
        {
            "id": 2,
            "product": 1,
            "old_price": "2.00",
            "price_time": "2021-02-14T23:44:06.269000Z"
        }]
        ```
    - `GET /price/history_by_ebay/{ebay_product_id}`: this endpoint acts like the previous one, but it requires an
     ebay product id.
    - `PUT /price/update/{price_history_id}`: this endpoint is used to update a price history of given id.
    
        An example of the required body for this endpoint is shown down below:
        ```JSON
          {
            "product": 1,
            "old_price": "6774.99",
            "price_time": "2021-02-08T17:33:14.280000Z"
          }      
        ```
    - `DEL /price/delete/{price_history_id}`: this endpoint is used to delete a price history of given id.

- **App users endpoints:**
    - `POST /register`: this endpoint is used to register a new AppUser.
    
        An example of the required body for this endpoint is shown down below:
        ```JSON
          {
            "email" : "gastani.frinzi@gmail.com",
            "name" : "Gastani",
            "surname" : "Frinzi",
            "nickname" : "Frigobar",
            "password" : "spaccoivasi",
            "password_confirm" : "spaccoivasi"
          }     
        ```
      
        If the registration is successful, a token will be returned:
        ```JSON
          {
            "token": "1ab7376f60dcae04d2dfb0cea04ba15ab921d473"
          }
        ```
    - `POST /login`: this endpoint is used to login an existing AppUser, and it will return a token like the previous
     endpoint.

---

## 6. Periodic price update
One of the aims of our project is to notify an user when the price of it's desired product drops below a threshold
 price.
In order to archive we have to check periodically the price of each product inside our database, using ebay API.

At first we looked for Celery, a flexible and reliable distributed system to process vast amounts of messages and
 supporting task scheduling.
 This approach required a broker (like RabbitMQ) to be used efficiently, and that would be an "overkill" solution for
  our project, so we decided to use `Django APScheduler`.
  
When our Django backend is started, a `price_update` job will be scheduled if it has not been scheduled before.
  
Inside the .env file the value `PERIODIC_UPDATE` is used to set the period to call price_update job: this one
 will call an "Ebay Shopping API" for each product inside the database to look for a price change.
 If the price of a product has been changed, the following actions will be executed:
  - The previous price value of the product will stored
  - The actual product price will be updated 
  - All the observations of this product will be retrieved, to check if the new price is below to one of the
   thresholds: if that happens a notification will be generated.

---

## 7. Recommendation system communication

- **Communication with recommendation system endpoints:**
- `POST /communication/send_all_new_observations`: this endpoint is used by our recommendation system to obtain
     all the NewObservedProduct.
     To correctly use this endpoint an admin AppUser token and a sequence number are required.
     
     An example of the required body for this endpoint is shown down below:
    ```JSON
        {
          "sequence_number" : 0
        }
    ```
  
    If Django backend is not waiting for the sequence number used by our recommendation system, an error that
     contains the expected sequence number will be returned:
     ```JSON
        {
            "response": "Wrong sequence number",
            "expected_seq_number": 1
        }
    ```
  
    If our backend reads the correct sequence number, all the NewObservedProduct with a previous value of
    sequence number will be deleted, and all the NewObservedProduct with the actual sequence number will be returned
     as shown below:
    ```JSON
        [{
            "id": 4,
            "user_id": 4,
            "product": 4,
            "sequence_number": 1,
            "created_at": "2021-02-17T23:38:02.942000Z"
        },
        {
            "id": 5,
            "user_id": 4,
            "product": 5,
            "sequence_number": 1,
            "created_at": "2021-02-17T23:38:30.874000Z"
        },
        {
            "id": 6,
            "user_id": 4,
            "product": 6,
            "sequence_number": 1,
            "created_at": "2021-02-17T23:44:08.162000Z"
        }]
    ``` 
- `POST /communication/add_recommendation`:
- `GET /communication/complete_recommendations_info`:
