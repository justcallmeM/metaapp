# Weather retrieval worker

Saves and displays current weather data by city name received from WeatherAPI every 30 seconds.

## Prerequisites

Before you begin, ensure you have met the following requirements:

* You have `Microsoft SQL Server Management Studio` installed
* You have created an empty database with the name `metaapp.Database` found in the project appsettings -> connectionstring

* Go to the solution
  * Right click metaapp.Database (Database project)
  * Select publish -> Edit... -> Browse -> Local -> Select your `MSSMS server` (MSSQLLocalDB) ->
    Select Database Name: metaapp.Database -> Test Connection (Test connection succeeded) -> Select OK ->
    Select Publish.
    * After a successfull publish, you should see [Weather] table and a couple SPs created. 

## Using Weather retrieval worker

To use Weather retrieval worker, follow these steps:

* Build the project
* Navigate to the metaapp.csproj executable, which is located in `metaapp\bin\Debug\net6.0`
* enter the following command:

```
metaapp.exe weather --city city1, city2,...,cityn.
```
- example

```
metaapp.exe weather --city Vilnius, Riga
```

## Contributors

* [Mindaugas Pikelis](https://www.linkedin.com/in/mindaugaspikelis/)

## Notes for the developer

- Write tests
- Read more about 
   * [Worker Services](https://learn.microsoft.com/en-us/dotnet/core/extensions/workers)
   * [Background tasks](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/host/hosted-services?view=aspnetcore-7.0&tabs=visual-studio)
   * [Exceptions & Error handling](https://learn.microsoft.com/en-us/dotnet/standard/exceptions/best-practices-for-exceptions)
   * [Logging](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/logging/loggermessage?view=aspnetcore-7.0)
   * [More fundamentals](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/?view=aspnetcore-7.0&tabs=windows)
   * [Authentication & Authorization](https://learn.microsoft.com/en-us/aspnet/core/security/?view=aspnetcore-7.0)
