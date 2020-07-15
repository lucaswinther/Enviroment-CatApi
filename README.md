# Enviroment-CatApi

## Aplication
This aplication is utilized to get data from https://thecatapi.com/.

## Requirements
* Docker for Windows Release 19

## How to Use
```
docker-compose up
```

## API
You can use Swagger to check and use endpoints 

http://localhost/swagger

![Postman](images\swagger.png)

## Postman
You can use the Postman to testing the endpoints

![Postman](images\Postman.png)

![Postman](images\Postman2.png)


## Metrics
Using Elastic APM and Kibana, you can see logs of aplication and metrics about response time and errors

![Postman](images\ApiMonitoring.png)

And you can se informations about use of CPU and memory

![Postman](images\ApiMonitoring2.png)

## Logs
Automaticly, the aplication will creade an index in ElasticSearch.
You can check then using discover tab in Kibana.

![Postman](images\ApiLogs.png)