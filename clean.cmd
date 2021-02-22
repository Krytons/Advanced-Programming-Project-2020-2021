CD Django/apl_api
if EXIST "migrations" (
    RD /s /q ".\migrations"
)
START /b mongod --logpath mongod.log
mongo --eval 'db.dropDatabase()' apl_db
mongo --eval 'db.shutdownServer()' admin
CD ../..
R ^< Rserver/terminate.R --no-save