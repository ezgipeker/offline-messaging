# offline-messaging
Simple offline messaging demo on .Net Core. 
## Getting Started
### Prerequisites
 - docker
### Installing
Clone repository:
```
https://github.com/ezgipeker/offline-messaging.git
```
Set container orchestration as startup project. Docker-compose.yml includes Api, Mssql, Redis, Elasticsearch and Kibana. So, you don't need any other setup. 
Run project.
By default, the compose file exposes the following ports:
- 1434: Mssql
- 9200: Elasticsearch
- 5601: Kibana
- 6379: Redis

For api, you can check the port with docker. 
```
docker ps
```
In container list, find offlinemessagingapi:dev and check port. 
If you go to localhost:[port], project will redirect you SwaggerUI.

In SwaggerUI, you can see api list and call apis. 
Some endpoints need authorization. For authorization, you need to register and login. 
user/login endpoint gives you token. In SwaggerUI, press the authorize button and write token. After that, you can use other endpoints.
All of the endpoints in SwaggerUI include sample request. You can use them.

### Test
Mssql container will be created once run the project. So, after ran the project once, you can run tests. 
Integration tests use mssql and service tests use in memory db. 
