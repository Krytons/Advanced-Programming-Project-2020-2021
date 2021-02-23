#!/bin/bash
source .env.sh
cd Django/apl_api
if [ -d "migrations" ]; then
    rm -r migrations
fi
mongod --fork --logpath mongod.log
mongo --eval 'db.dropDatabase()' "${db_name}"
mongo --eval 'db.shutdownServer()' admin
cd ../..
cd Rserver
sudo R < terminate.R --no-save
rm *.rds
rm *.log
cd ..
rm mongod.log*