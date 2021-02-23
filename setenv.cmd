SET /p my_python=Enter your python executable name (i.e. "py -3"):
ECHO SET my_python=%my_python% > .env.cmd
CD Django
ECHO Setting up the Django .env, press ENTER on non-required fields to auto-fill
SET /p ebayappid=Enter your EBAY_APP_ID (REQUIRED!): || SET ebayappid=NONE
SET /p ebayglobalid=Enter your EBAY_GLOBAL_ID: || SET ebayglobalid=EBAY-IT
SET /p ebayitalycode=Enter your EBAY_ITALY_CODE: || SET ebayitalycode=101
SET /p periodicupdate=Enter your PERIODIC_UPDATE (default: 300): || SET periodicupdate=300
SET /p mongohost=Enter your MONGO_HOST (default: localhost): || SET mongohost=localhost
SET /p mongoport=Enter your MONGO_PORT (default: 27017): || SET mongoport=27017
SET /p mongodb_name=Enter your MONGO_DB_NAME (default: apl-db): || SET mongodb_name=apl-db


(
    ECHO EBAY_APP_ID=%ebayappid%
    ECHO EBAY_GLOBAL_ID=%ebayglobalid%
    ECHO EBAY_ITALY_CODE=%ebayitalycode%
    ECHO PERIODIC_UPDATE=%periodicupdate%
    ECHO MONGO_HOST=%mongohost%
    ECHO MONGO_PORT=%mongoport%
    ECHO MONGO_DB_NAME=%mongodb_name%
) > .env

CD ..
ECHO SET db_name=%mongodb_name% >> .env.cmd