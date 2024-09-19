### build docker image
## docker build -t aegona-ats-mvc -f ./Ats.Web/Dockerfile . 

### run docker container
## docker run -d -ti -p 5000:80 -e ASPNETCORE_ENVIRONMENT=Production aegona-ats-mvc