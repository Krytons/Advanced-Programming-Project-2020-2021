cd Django/apl_api
if [ -d "migrations" ]; then
    rm -r migrations
fi
mongod --fork --logpath mongod.log
mongo --eval 'db.dropDatabase()' apl_db
mongo --eval 'db.shutdownServer()' admin
cd ../..
sudo R < Rserver/terminate.R --no-save