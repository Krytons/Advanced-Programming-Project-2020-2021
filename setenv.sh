#!/bin/bash
read -p "Enter your python executable name (i.e. 'python3'):" my_python
my_python=${my_python:-python3}
echo "#!/bin/bash" > .env.sh
echo "alias my_python='$my_python'" >> .env.sh
cd Django
echo "Setting up the Django .env, press ENTER on non-required fields to auto-fill"
read -p "Enter your EBAY_APP_ID (REQUIRED!):" ebayappid
ebayappid=${ebayappid:-NONE}
read -p "Enter your EBAY_GLOBAL_ID:" ebayglobalid
ebayglobalid=${ebayglobalid:-EBAY-IT}
read -p "Enter your EBAY_ITALY_CODE:" ebayitalycode
ebayitalycode=${ebayitalycode:-101}
read -p "Enter your PERIODIC_UPDATE (default: 300):" periodicupdate
periodicupdate=${periodicupdate:-300}
read -p "Enter your MONGO_HOST (default: localhost)" mongohost
mongohost=${mongohost:-localhost}
read -p "Enter your MONGO_PORT (default: 27017):" mongoport
mongoport=${mongoport:-27017}
read -p "Enter your MONGO_DB_NAME (default: apl_db):" mongodb_name
mongodb_name=${mongodb_name:-apl_db}
echo "EBAY_APP_ID=$ebayappid
EBAY_GLOBAL_ID=$ebayglobalid
EBAY_ITALY_CODE=$ebayitalycode
PERIODIC_UPDATE=$periodicupdate
MONGO_HOST=$mongohost
MONGO_PORT=$mongoport
MONGO_DB_NAME=$mongodb_name" > .env
cd ..
cd Xamarin/XamarinFrontEnd/Configuration
read -p "URL Ngrok:" ngrokUrl
ngrokUrl=${ngrokUrl:-localhost}
echo "{ \"ngrok\": \"$ngrokUrl\" }" > secrets.json
cd ../../..
echo "db_name='$mongodb_name'" >> .env.sh