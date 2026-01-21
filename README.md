Projekt AdZZiM
Baza danych

Serwer: .\SQLEXPRESS

Nazwa bazy: AdZZiM

Connection String: Server=.\SQLEXPRESS;Database=AdZZiM;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;

Administrator

Login: admin@adzzim.pl

Hasło: admin222

API Produkty

Pobieranie (GET): https://localhost:7105/api/productsapi

Usuwanie (DELETE):

Bash
curl -X DELETE https://localhost:7105/api/productsapi/30

Nuget:

    <PackageReference Include="Microsoft.AspNetCore.Identity.EntityFrameworkCore" Version="10.0.2" />
    <PackageReference Include="Microsoft.AspNetCore.Identity.UI" Version="10.0.2" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="10.0.2" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="10.0.2">
      <PrivateAssets>all</PrivateAssets>
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
    </PackageReference>
 
