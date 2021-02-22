SET my_python=py -3
REM #########################################################
REM # 1. start MongoDB + Django
START /b mongod --logpath mongod.log
CD Django
if NOT EXIST "apl_api\migrations\" (
    %my_python% -m pip install -r requirements.txt
    CD apl_api
    MKDIR migrations
    CD migrations
    fsutil file createnew __init__.py 0
    CD ../..
    %my_python% manage.py makemigrations
    %my_python% manage.py migrate
    REM # populate DB
    CD utilities
    mongoimport --db apl_db --collection apl_api_appuser --drop --jsonArray --file usersdatabase.json
    mongoimport --db apl_db --collection apl_api_product --drop --jsonArray --file productsdatabase.json
    mongoimport --db apl_db --collection apl_api_observedproduct --drop --jsonArray --file observationsdatabase.json
    mongoimport --db apl_db --collection apl_api_pricehistory --drop --jsonArray --file pricehistory.json
    mongo --eval 'db.getCollection("__schema__"^).update({name:"apl_api_appuser"},{$set:{"auto":{"field_names":["id"],"seq":130}}}^)' apl_db
    mongo --eval 'db.getCollection("__schema__"^).update({name:"apl_api_product"},{$set:{"auto":{"field_names":["id"],"seq":193}}}^)' apl_db
    mongo --eval 'db.getCollection("__schema__"^).update({name:"apl_api_observedproduct"},{$set:{"auto":{"field_names":["id"],"seq":486}}}^)' apl_db
    mongo --eval 'db.getCollection("__schema__"^).update({name:"apl_api_pricehistory"},{$set:{"auto":{"field_names":["id"],"seq":45}}}^)' apl_db
    CD ..
)
CD ..
%my_python% Django/manage.py runserver 0.0.0.0:8000 --noreload