mongo --eval "db.shutdownServer()" admin
CD Rserver
Rscript terminate.R %cd%
CD ..