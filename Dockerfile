FROM microsoft/dotnet:runtime

# Install needed packages
RUN apt-get update && apt-get -y install cron icu-devtools libssl-dev ca-certificates
RUN update-ca-certificates

# Add crontab file
ADD docker/crontab /tmp/speedtest.cron

ENV SPEEDTEST_HOME /speedtest
ENV INFLUXDB_URL http://localhost:8086

# Copy build output
COPY build/linux-x64-thin ${SPEEDTEST_HOME}

# Copy run script
COPY docker/run.sh run.sh
RUN chmod +x /run.sh

CMD ["/run.sh"]
