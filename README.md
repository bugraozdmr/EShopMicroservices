<p align=center>
  <br>
  <span>EShopMicroservices is microservice based asp web api</span>
  <br>
</p>

## Clone the Repo
```console
# clone the repo
$ git clone https://github.com/bugraozdmr/EShopMicroservices.git
```

## Dotnet Package Installation
```console
# change the working directory EShopMicroservices/src
$ cd EShopMicroservices/src

# install packages
$ dotnet restore
```

You can change the port numbers as you want to if any conflict in your computer happens.Also don't forget to change container volumes based on your operating system.
## Docker-Compose
```console
# change the working directory to EShopMicroservices/src
$ cd EShopMicroservices/src

# compose up
$ docker-compose down -v && docker-compose -p microservices up --build
```

Microservice based project 🎉