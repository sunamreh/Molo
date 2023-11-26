# Molo (Mobile Loans)
### Prerequisite
- RabbitMq
  - docker run -it -p 15672:15672 -p 5672:5672 rabbitmq:3-management
- Postgres
  - docker run --name molo-postgres -e POSTGRES_PASSWORD=postgres -d postgres
 
### Set Configuration on Api/Worker (appsettings.json)
    "MoloDbContext": "Hosthost.docker.internal:5432; Database=Molo; Username=postgres; Password=postgres",
    "RabbitMq": "amqp://guest:guest@host.docker.internal:5672/"

### Live Api
https://moloapi.azurewebsites.net/swagger/index.html

### Live USSD
https://moloussd.azurewebsites.net/swagger/index.html
