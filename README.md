# StoreManagement
Aplikacija za kreiranje porudzbina za proizvode na osnovu liste parova ID(Store Keeping Unit - Sku)<->Quantity. Test proizvodi se cuvaju u RAM memoriji.

# Environment
Koriscen .NET Core 3.1, pa je potreban ASP.NET Core Runtime (download link https://dotnet.microsoft.com/en-us/download/dotnet/3.1).

# Init, pull koda i pokretanje
1. git init
2. git pull https://github.com/milos-stolica/StoreManagement.git main
3. Navigirati se u folder StoreManagement.API i odraditi komandu dotnet run StoreManagement.API.csproj
4. Odradice se build i pokretanje aplikacije - server slusa na http://localhost:3000

# Kreiranje porudzbine (Postman ili neki slican tool)
Method POST
URL http://localhost:3000/api/orders
Body
[
    {
        "ProductSku": "123AB",
        "Quantity": 200
    },
    {
        "ProductSku":"012GH",
        "Quantity": 1000
    }
]

# Unit testovi u folderu StoreManagement.Tests
Pustiti kroz Visual Studio (vjerovatno ne moze stariji od 2019 - ja sam koristio Visual Studio Community 2019 - version 16.9) - Test -> Test explorer 

# U slucaju problema mozete mi pisati na mail
milos_stolica@hotmail.com
