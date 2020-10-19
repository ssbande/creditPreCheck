# creditPreCheck

A qualifier pre-check for set of credit cards, based on Age and Income of the user.

### How to run 

To run the application, do the following after cloning the application
- Install .NET Core 3.1 SDK or later
- Execute below commands from the terminal: 

```
$ cd to the directory where you cloned the app
$ dotnet tool install --global dotnet-ef 
    if not already installed
$ dotnet ef migrations add InitialCreate
    This will create the schema of the database
$ dotnet build 
    This will try and build the application
$ dotnet run 
    This will seed the database and will start running the application
```

### Features 
- Built with **Code First** approach. 
- Runs on **SQLite DB**. When started the credit card data is seeded into the database and then utilised. All the data is in First Normal Form. 
- **Fully customisable**. All the cards will have their own age and income configurations which will be used to decide whether the card can be allotted to the user or not. 
- All the major actions and errors in the appication are logged on console as well as in files inside **Logs** folder
- **Caching**. The Application is having caching in it so that if some user is checking his/her validity for credit card the data will be fetched from the cache. Again, the time for which the cache would be referred is configurable in application 

### TODOs 
- Create a solution and create Test project
