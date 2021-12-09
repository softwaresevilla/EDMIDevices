# EDMIDevices API

This API is created to manage devices. These devices could be of three different types. These types are:
1. Electric meter: Id, SerialNumber, FirmwareVersion, State
2. Water meter: Id, SerialNumber, FirmwareVersion, State
3. Gateways: Id, SerialNumber, FirmwareVersion, State, Ip, Port

Id, SerialNumber, FirmwareVersion ara mandatory. SerialNumber is a unique field. It is not possible to register two devices with the same SerialNumber.

# Requirements

1. Docker
2. MongoDB
3. RabbitMQ

# Configuration

This API is ready to be executed using RabbitMQ and you can also decide not to enable it. Also, there are two different backend storage options. You can decide store the data using MongoDB or in memory storage.

To configure these options, you have to follow the following instructions.

### RabbitMQ

To enable or disable RabbitMQ backend, you have to change the QueueBackend property inside the appsettings.json

```
"QueueBackend": false
```

Enabling RabbitMQ, it is mandatory to fill the RabbitMQ options inside appsettings.json. To do this, create an object inside it, with the following properties.

```
"QueueSettings": {
    "Host": "localhost",
    "Port": 5672,
    "QueueName": "devices",
    "User": "guest",
    "Password": "guest"
 }
 ```
 
### Storage options

To decide the storage, you have to change the property Destination inside Storage in appsettings.json. The available options are Memory and MongoDB.

```
"Storage": {
    "Destination": "Memory"
}
```

If you decide to use MongoDB storage, it is necessary to fill the Database settings. Here there is an example. 

```
"DatabaseSettings": {
    "CollectionName": "Devices",
    "ConnectionString": "mongodb://localhost:27017",
    "DatabaseName": "EDMIDevices"
}
```

# Deployment

This API is ready to be deployed using Docker. To deploy the API using docker, you have to follow the following instructions.

1. Build the application executing the following command

```
docker build -t edmidevices .
```

Where edmidevices will be the name of the image generated. This image will be execute later.

2. Run the container

```
docker run -d -p 5001:80 --name edmidevicesapi edmidevices
```

Where edmidevicesapi will be the name of the running container, and edmidevices the name of the image previously generated.


# Console Project

This is a simple frontend app using console. The connection parameters are hardcoded to use the rabbitmq connection