CALL .env.cmd
CD Django/apl_api
if EXIST "migrations" (
    RD /s /q ".\migrations"
)
START /b %my_mongo% --logpath mongod.log
mongo --eval 'db.dropDatabase()' %db_name%
mongo --eval 'db.shutdownServer()' admin
CD ../..
CD Rserver
R ^< terminate.R --no-save
DEL *.rds
DEL *.log
CD ..
DEL mongod.log*