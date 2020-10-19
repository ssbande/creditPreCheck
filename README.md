# creditPreCheck

A qualifier pre-check for set of credit cards, based on Age and Income of the user.

#### Pre-requisites
- Install .NET Core 3.1 SDK or later
- Visual Studio / Visual Studio Code / Any other IDE of your choice

#### How to run ?
To run the tests and the web application, IDE is not a requirement. This can also be done from any terminal. The only thing that you would need to do is to clone this repo to your local computer.

**Build and run web app**

---------------

Navigate to the folder where repo is cloned.  
Run the following commands:  
    * `cd creditPreCheck`  
    * `dotnet build` - This will check for any errors and try to build the code  
    * `dotnet run` - This will seed the database and start the web app  
Open the browser and navigate to  
http://localhost:5000/  
https://localhost:5001/  

**Run Tests for the app**

---------------
Navigate to the folder where repo is cloned.  
Run the following commands:  
    * `cd creditPreCheck.Tests`  
    * `dotnet test`  
This will find and run all the tests currently available in the app. 

### Features 

---------------
- Built with **Code First** approach. 
- Runs on **SQLite DB**. When started the credit card data is seeded into the database and then utilised. All the data is in First Normal Form. 
- **Fully customisable**. All the cards will have their own age and income configurations which will be used to decide whether the card can be allotted to the user or not. 
- All the major actions and errors in the appication are logged on console as well as in files inside **Logs** folder
- **Caching**. The Application is having caching in it so that if some user is checking his/her validity for credit card the data will be fetched from the cache. Again, the time for which the cache would be referred is configurable in application 
- **Testing** The Application tests are added in a separate project from the app and are executing successfully.

### TODOs 
[] Write more tests  
