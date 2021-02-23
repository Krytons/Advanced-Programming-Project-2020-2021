CALL .env.cmd
REM #########################################################
REM # 1. start MongoDB + Django
START /b %my_mongo% --logpath mongod.log
CD Django
if NOT EXIST "apl_api\migrations\" (
    %my_python% -m pip install -r requirements.txt
    CD apl_api
    MKDIR migrations
    CD migrations
    FSUTIL file createnew __init__.py 0
    CD ../..
    %my_python% manage.py makemigrations
    %my_python% manage.py migrate
    REM # populate DB
    CD utilities
    mongoimport --db %db_name% --collection apl_api_appuser --drop --jsonArray --file usersdatabase.json
    mongoimport --db %db_name% --collection apl_api_product --drop --jsonArray --file productsdatabase.json
    mongoimport --db %db_name% --collection apl_api_observedproduct --drop --jsonArray --file observationsdatabase.json
    mongoimport --db %db_name% --collection apl_api_pricehistory --drop --jsonArray --file pricehistory.json
    mongo --eval 'db.getCollection("__schema__"^).update({name:"apl_api_appuser"},{$set:{"auto":{"field_names":["id"],"seq":130}}}^)' %db_name%
    mongo --eval 'db.getCollection("__schema__"^).update({name:"apl_api_product"},{$set:{"auto":{"field_names":["id"],"seq":193}}}^)' %db_name%
    mongo --eval 'db.getCollection("__schema__"^).update({name:"apl_api_observedproduct"},{$set:{"auto":{"field_names":["id"],"seq":486}}}^)' %db_name%
    mongo --eval 'db.getCollection("__schema__"^).update({name:"apl_api_pricehistory"},{$set:{"auto":{"field_names":["id"],"seq":45}}}^)' %db_name%
    CD ..
)
CD ..
%my_python% Django/manage.py runserver 0.0.0.0:8000 --noreload