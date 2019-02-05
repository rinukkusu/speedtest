#!/bin/bash

# prepend env variables
env | egrep '^INFLUX' | cat - /tmp/speedtest.cron > /etc/cron.d/speedtest
chmod 0644 /etc/cron.d/speedtest
crontab /etc/cron.d/speedtest

# create log file
touch /var/log/cron.log

# run cron and tail on log file
cron && tail -f /var/log/cron.log
