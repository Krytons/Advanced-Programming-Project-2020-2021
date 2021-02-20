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



---

## 4. Permissions

---

## 5. Views

---

## 6. Recommendation system communication

