# FLS Coffee desk
### optimizely-demo

Demo project for Optimizely. Very basic eCommerce site just home page and catalog with products.

## Prerequisites
* .NET SDK 5

## Testing data
Most part of content is created via Faker library and one-time 'migration' code. 
You just need to have a folder with jpg/png images for one-time site and commerce content creation. 
* Folder path is specified in **appSettings.json**, key: **TestImagesFolderPath**

## Setup and run
* dotnet nuget add source https://nuget.optimizely.com/feed/packages.svc/ -n optimizely
* .\Setup-local-database.ps1
* dotnet restore
* dotnet run

