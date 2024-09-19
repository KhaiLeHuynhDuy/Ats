### build docker image
## docker build -t aegona-ats-api -f ./Ats.Api/Dockerfile . 

### run docker container
## docker run -d -ti -p 44397:44397 -e ASPNETCORE_ENVIRONMENT=Production aegona-ats-api