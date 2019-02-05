# speedtest

This is a very simple speedtesting tool for your network with InfluxDB integration.

## Usage

To just measure your speed it's `Speedtest [source]`:

```bash
Speedtest hetzner
```

Sources are defined in the `appsettings.json` file and currently available are: `inode`, `hetzner`, `belwue` and `tele2`.

## Get data into InfluxDB

To get your data into InfluxDB you have the following environment variables to configure the connection:

|Variable|Optional|Default Value|
|--|--|--|
|INFLUXDB_URL|false||
|INFLUXDB_USERNAME|true|root|
|INFLUXDB_PASSWORD|true|root|

Now just pipe the output of `Speedtest` into `Speedtest.InfluxDB`:

```bash
Speedtest hetzner | Speedtest.InfluxDB
```

## Build

To build the applications you will need .NET Core 2.2 installed, other than that it's just the following command after which you will find the standalone published binaries in the `build` folder:

```bash
./build.linux.sh
```

## Docker

```bash
sudo docker build --rm -t mylittlespeedtest .
sudo docker run -t -i -e INFLUXDB_URL='http://192.168.0.100:8086' mylittlespeedtest
```