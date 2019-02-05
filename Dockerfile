FROM ubuntu:16.04

# Install needed packages
RUN apt-get update && apt-get -y install cron icu-devtools libssl-dev ca-certificates
RUN update-ca-certificates

# Add crontab file
ADD docker/crontab /tmp/speedtest.cron

ENV SPEEDTEST_HOME /speedtest
ENV SPEEDTEST_SOURCE inode
ENV INFLUXDB_URL http://localhost:8086

# Copy build output
COPY build/linux-x64 ${SPEEDTEST_HOME}

# Copy speedtest script
COPY docker/speedtest.sh ${SPEEDTEST_HOME}/speedtest.sh
RUN chmod +x ${SPEEDTEST_HOME}/speedtest.sh

# Copy run script
COPY docker/run.sh run.sh
RUN chmod +x /run.sh

CMD ["/run.sh"]
