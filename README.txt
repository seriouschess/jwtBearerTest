## React/Asp.Net3.1 JWT sample

To get started, pull the repository off github to a folder and add a file called appsettings.json to the directory.

Inside appsettings.json paste the following:

{
    "Logging": {
        "LogLevel": {
        "Default": "Information",
        "Microsoft": "Warning",
        "Microsoft.Hosting.Lifetime": "Information"
        }
      },
    "AllowedHosts": "*",
  
    "Jwt":{
      "PrivateKey":"12345678901234567090123456709012",
      "LifetimeInSeconds":3600,
      "Issuer":"someidentifier",
      "Audience":"someidentifier"
    }
}

Note: Any Private Key listed here must be at least 32 characters long for the app to work due to the nature of the decryption alogorithm.

Navigate to the project folder in the command line terminal and type the command "dotnet run".

Go to your browser and navigate to the url 127.0.0.1:5000/

The page should display a JWT followed by a line containing "success". 
Two endpoints were accessed to get this: one to authenticate a fake user and retrieve the token and the other using the token to authorise a second endpoint.



Frontend code acessing the endpoints is contained in clientapp -> src -> components -> jwt-bearer-client.js