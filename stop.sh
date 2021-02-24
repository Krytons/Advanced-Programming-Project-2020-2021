#!/bin/bash
mongo --eval 'db.shutdownServer()' admin
cd Rserver
sudo Rscript terminate.R $PWD
cd ..