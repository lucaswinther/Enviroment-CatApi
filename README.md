# Environment-CatApi

![gif](https://github.com/lucaswinther/Enviroment-CatApi/blob/master/images/environment_architecture.png)


## Application
This application is utilized to get data from https://thecatapi.com/.

## Requirements

* Git Bash (download repository) - https://git-scm.com/download/win
* Docker for Windows Release 19 - https://docs.docker.com/docker-for-windows/install/
* Visual Code - https://code.visualstudio.com/download (optional)
* SQL Management Studio SSMS - https://docs.microsoft.com/pt-br/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver15 (optional)
* PostMan - https://www.postman.com/downloads/ (optional)

## How to Use

1º - Open powershell window

2º - ```git clone https://github.com/lucaswinther/Enviroment-CatApi.git```

3º - Run ```cd .\Enviroment-CatApi\ ```

4º - Run ```docker-compose up```

When you run docker-compose up, all the Environment will be created.

* Cat Web Api
* Elastic Search
* Kibana
* APM Server
* Sql server

![gif](https://github.com/lucaswinther/Enviroment-CatApi/blob/master/images/enviroment.png)


![gif](https://github.com/lucaswinther/Enviroment-CatApi/blob/master/images/compose-up.gif)



## Know possibles issues
Sometimes, when you run docker compose, the database has is not created automatically.
The turnaround is, you remove the container name sqlserver, and run it docker-compose again.

```
docker stop sqlserver
docker rm sqlserver
docker-compose up
```

![gif](https://github.com/lucaswinther/Enviroment-CatApi/blob/master/images/compose-up2.gif)


## API
You can use Swagger to check and use endpoints 

http://localhost:8085/swagger

![Image](https://github.com/lucaswinther/Enviroment-CatApi/blob/master/images/swagger.png)

## Endpoints

#### /api/v1/CatsBreeds/loadcats
Endpoint to load cats database

#### /api/v1/CatsBreeds/getallbreeds
Endpoint to get all breeads in database

#### /api/v1/CatsBreeds/getbreeds?IdOrName=
Endpoint to get a specific Breed using Id or name

#### /api/v1/CatsBreeds/getbreedsbytemperament?temperament=
Endpoint to get a specific Breed using temperament

#### /v1/CatsBreeds/getimageurlbycategory?category=
Endpoint to get a specific image using a category

#### /api/v1/CatsBreeds/getbreedsbyorigin?origin=
Endpoint to get a specific image using a origin


## Postman
You can use the Postman to testing the endpoints

![Image](https://github.com/lucaswinther/Enviroment-CatApi/blob/master/images/Postman.png)

![Image](https://github.com/lucaswinther/Enviroment-CatApi/blob/master/images/Postman2.png)


## Metrics
Using Elastic APM and Kibana, you can see logs of aplication and metrics about response time and errors

### http://localhost:5601/app/apm 

![Image](https://github.com/lucaswinther/Enviroment-CatApi/blob/master/images/ApiMonitoring.png)

And you can se informations about use of CPU and memory

![Postman](https://github.com/lucaswinther/Enviroment-CatApi/blob/master/images/ApiMonitoring2.png)


## Logs
Automaticly, the aplication will creade an index in ElasticSearch.
You can check then using discover tab in Kibana.

### http://localhost:5601/app/kibana#/discover

![Image](https://github.com/lucaswinther/Enviroment-CatApi/blob/master/images/ApiLogs.png)
