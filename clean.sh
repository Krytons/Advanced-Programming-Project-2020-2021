cd Django/apl_api
if [ -d "migrations" ]; then
    rm -r migrations
fi
mongod --fork --logpath mongod.log
mongo --eval 'db.dropDatabase()' apl_db
mongo --eval 'db.shutdownServer()' admin
cd ../..
cd Rserver
sudo R < terminate.R --no-save
rm *.rds
rm *.log
cd ..
rm mongod.log*