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

##Dev notes to create similar project

Install project template
* dotnet new -i EPiServer.Net.Templates --nuget-source https://nuget.optimizely.com/feed/packages.svc/ --force

Install Optimizely CLI
* dotnet tool install EPiServer.Net.Cli --global --add-source https://nuget.optimizely.com/feed/packages.svc/

Add NuGet source
* dotnet nuget add source https://nuget.optimizely.com/feed/packages.svc/ -n optimizely

Create project via command or IDE
* dotnet new epicommerceempty -n FLS.CoffeeDesk
* Install EPiServer.CMS package via NuGet

Create databases
* dotnet-episerver create-cms-database
* dotnet-episerver create-commerce-database

Example
* dotnet-episerver create-cms-database .\FLS.CoffeeDesk\FLS.CoffeeDesk.csproj -S . -E -dn cmsCoffeeDesk -du coffeeDeskUser  -dp PaSSw0rd123!

Create administrator user
* dotnet-episerver add-admin-user .\FLS.CoffeeDesk\FLS.CoffeeDesk.csproj -u admin -p Episerver123! -e admin@example.com -c EcfSqlConnection

Setup site
* Navigate to http://localhost:8000/episerver/cms
* Login via admin / Episerver123!
* Go to Admin →Config → Manage websites - setup site
* Go to Admin →Access Rights - set up groups and rights to users
