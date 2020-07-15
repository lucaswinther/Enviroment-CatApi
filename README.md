# Enviroment-CatApi

## Aplication
This aplication is utilized to get data from https://thecatapi.com/.

## Requirements
* Docker for Windows Release 19

## How to Use
```
docker-compose up
```

When you run docker-compose up, all the enviroment will be created.

* Cat Web Api
* Elastic Search
* Kibana
* Metric beat
* APM Server
* Sql server

![gif](https://github.com/lucaswinther/Enviroment-CatApi/blob/master/images/enviroment.png)


![gif](https://github.com/lucaswinther/Enviroment-CatApi/blob/master/images/compose-up.gif)



## Know possibles issues
Sometimes, when you run docker compose, the database has is not created automatically.
The turnaround is, you remove the container name sqlserver, and run it docker-compose again.

![gif](https://github.com/lucaswinther/Enviroment-CatApi/blob/master/images/compose-up2.gif)


## API
You can use Swagger to check and use endpoints 

http://localhost/swagger

![Image](https://github.com/lucaswinther/Enviroment-CatApi/blob/master/images/swagger.png)

## Postman
You can use the Postman to testing the endpoints

![Image](https://github.com/lucaswinther/Enviroment-CatApi/blob/master/images/Postman.png)

![Image](https://github.com/lucaswinther/Enviroment-CatApi/blob/master/images/Postman2.png)


## Metrics
Using Elastic APM and Kibana, you can see logs of aplication and metrics about response time and errors

![Image](https://github.com/lucaswinther/Enviroment-CatApi/blob/master/images/ApiMonitoring.png)

And you can se informations about use of CPU and memory

![Postman](https://github.com/lucaswinther/Enviroment-CatApi/blob/master/images/ApiMonitoring2.png)

## Logs
Automaticly, the aplication will creade an index in ElasticSearch.
You can check then using discover tab in Kibana.

![Image](https://github.com/lucaswinther/Enviroment-CatApi/blob/master/images/ApiLogs.png)
