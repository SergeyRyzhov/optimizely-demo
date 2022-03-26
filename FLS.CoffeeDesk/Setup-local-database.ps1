#param(
#     [Parameter(Mandatory=$true,
#    HelpMessage="Do you want to remove existing DBs (y/N)")]
#     [string]$PruneExistingDb
#)

$PruneExistingDb = $(Read-Host -Prompt 'Do you want to remove existing DBs (y/N)')

if (& dotnet tool list  --global | Select-String "episerver.net.cli") {
    Write-Host -f Yellow 'Skipping install of EPiServer.Net.Cli. It''s already installed'
}
else {
    dotnet tool install EPiServer.Net.Cli --global --add-source https://nuget.optimizely.com/feed/packages.svc/
}

$PROJECT_PATH = "./FLS.CoffeeDesk.csproj"
$SQL_SERVER = "."

$CMS_DATABASE_NAME = "cmsCoffeeDesk"
$COMMERCE_DATABASE_NAME = "commerceCoffeeDesk"
$DB_USER = "coffeeDeskUser"
$DB_PASSWORD = "Wd2vAkb6LkGGDZbV"

if ($PruneExistingDb -eq "y" -or $PruneExistingDb -eq "Y" ) {
    write-host -f Yellow "Removing existing DB"
    sqlcmd -S $SQL_SERVER -E -b -Q "IF EXISTS(SELECT * FROM sys.databases WHERE name = 'cmsCoffeeDesk') BEGIN alter database [cmsCoffeeDesk] set single_user with rollback immediate END"
    sqlcmd -S $SQL_SERVER -E -b -Q "IF EXISTS(SELECT * FROM sys.databases WHERE name = 'cmsCoffeeDesk') BEGIN DROP DATABASE [cmsCoffeeDesk] END"

    sqlcmd -S $SQL_SERVER -E -b -Q "IF EXISTS(SELECT * FROM sys.databases WHERE name = 'commerceCoffeeDesk') BEGIN alter database [commerceCoffeeDesk] set single_user with rollback immediate END"
    sqlcmd -S $SQL_SERVER -E -b -Q "IF EXISTS(SELECT * FROM sys.databases WHERE name = 'commerceCoffeeDesk') BEGIN DROP DATABASE [commerceCoffeeDesk] END"

    sqlcmd -S $SQL_SERVER -E -b -Q "IF EXISTS (SELECT * FROM sys.database_principals WHERE name = N'coffeeDeskUser') DROP USER [coffeeDeskUser]"
}

dotnet-episerver create-cms-database $PROJECT_PATH -S $SQL_SERVER -E -dn $CMS_DATABASE_NAME -du $DB_USER -dp $DB_PASSWORD
dotnet-episerver create-commerce-database $PROJECT_PATH -S $SQL_SERVER -E -dn $COMMERCE_DATABASE_NAME -du $DB_USER -dp $DB_PASSWORD

dotnet-episerver add-admin-user $PROJECT_PATH -u admin -p Episerver123! -e admin@example.com -c EcfSqlConnection