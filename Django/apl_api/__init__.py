import environ

env = environ.Env()
environ.Env.read_env()
HOST = env.str('MONGO_HOST')
PORT = env.int('MONGO_PORT')
DATABASE = env.str('MONGO_DB_NAME')

from pymongo import MongoClient
connection = MongoClient(HOST, PORT) #Connect to mongodb
db = connection[DATABASE]
if 'django_apscheduler_djangojob' in db.list_collection_names():
    default_app_config = 'apl_api.apps.AplApiConfig'