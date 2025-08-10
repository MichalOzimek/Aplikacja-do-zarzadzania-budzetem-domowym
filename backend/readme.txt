1. Create a file appsettings.json and put the following into it replacing the following with your values (PasswordFromYourDatabase, PasswordFromYourDatabase, PortThatIsUsedByYourDatabase):
{
  "ConnectionStrings": {
    "ProjectSoftwareWorkshopDbConnectionString": "Server=localhost;Database=ProjectSoftwareWorkshop;User=nameOfTheUserFromYourDatabase;Password=PasswordFromYourDatabase;Port=PortThatIsUsedByYourDatabase"
  },
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning",
        "Microsoft.Hosting.Lifetime": "Information"
      }
    },
    "WriteTo": [
      {
        "Name": "File",
        "Args": {
          "path": "./logs/log-.txt",
          "rollingInterval": "Day"
        }
      },
      {
        "Name": "Seq",
        "Application": "ProjectSoftwareWorkshop",
        "Args": {
          "serverlUrl": "http://localhost:5341"
        }
      }
    ]
  },
  "AllowedHosts": "*"
}
2. Execute the following in the terminal:
    a) dotnet ef migrations add CreateTables
    b) dotnet ef database update
3. 