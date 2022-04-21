docker build -t optimizely-demo:latest .
docker rm -f "optimizely-demo"
docker run -d -p 8000:8000 -v C:\dev\Coffee:/app/coffee -e ASPNETCORE_ENVIRONMENT=Docker --name optimizely-demo optimizely-demo:latest
