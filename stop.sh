#!/bin/bash
mongo --eval 'db.shutdownServer()' admin
sudo R < Rserver/terminate.R --no-save