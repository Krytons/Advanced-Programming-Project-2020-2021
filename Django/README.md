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
- **Inside "Django/apl_api" create a .env file with the following arguments:**
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


---

## 3. Models

---

## 4. Permissions

---

## 5. Views

