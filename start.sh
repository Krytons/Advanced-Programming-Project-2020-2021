#!/bin/bash
source .env.sh
#########################################################
# 1. start MongoDB + Django
mongod --fork --logpath mongod.log
cd Django
if [ ! -d "apl_api/migrations" ]; then
    my_python -m pip install -r requirements.txt
    cd apl_api
    mkdir migrations
    cd migrations
    touch __init__.py
    cd ../..
    my_python manage.py makemigrations
    my_python manage.py migrate
    # populate DB
    cd utilities
    mongoimport --db "${db_name}" --collection apl_api_appuser --drop --jsonArray --file usersdatabase.json
    mongoimport --db "${db_name}" --collection apl_api_product --drop --jsonArray --file productsdatabase.json
    mongoimport --db "${db_name}" --collection apl_api_observedproduct --drop --jsonArray --file observationsdatabase.json
    mongoimport --db "${db_name}" --collection apl_api_pricehistory --drop --jsonArray --file pricehistory.json
    mongo --eval 'db.getCollection("__schema__").update({name:"apl_api_appuser"},{$set:{"auto":{"field_names":["id"],"seq":130}}})' "${db_name}"
    mongo --eval 'db.getCollection("__schema__").update({name:"apl_api_product"},{$set:{"auto":{"field_names":["id"],"seq":193}}})' "${db_name}"
    mongo --eval 'db.getCollection("__schema__").update({name:"apl_api_observedproduct"},{$set:{"auto":{"field_names":["id"],"seq":486}}})' "${db_name}"
    mongo --eval 'db.getCollection("__schema__").update({name:"apl_api_pricehistory"},{$set:{"auto":{"field_names":["id"],"seq":45}}})' "${db_name}"
    cd ..
fi
cd ..
my_python Django/manage.py runserver 0.0.0.0:8000 --noreload