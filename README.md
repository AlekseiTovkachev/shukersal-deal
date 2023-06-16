# shukersal-deal

The best online marketplace

## Database Connection Guide:

### Installation:

1. Download [Express SQL Server](https://go.microsoft.com/fwlink/p/?linkid=2216019&clcid=0x409&culture=en-us&country=us) from the microsoft website.
2. Select `Basic` installation.
3. Install in the default location (or any location you want), follow the install instructions.
4. **Optional:** Install SQL Server Management Studio (SSMS). You can connect to the database with the name: `.\SQLEXPRESS` and Windows Authentication.

### Running the Database:

#### Make sure the database is running:

1. Open `SQL Server 2022 Configuration Manager` from the start menu.
2. Under `SQL Server Services` you will see: `SQL Server (SQLEXPRESS)`, make sure it is running. You can stop/start it by right clicking on it.

#### Make sure the backend is configured correctly:

1. In `Program.cs` you will see the following:

   ```cs
   builder.Services.AddDbContext<MarketDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
   ```

2. In `appsettings.json` you will see the following:

   ```json
   "ConnectionStrings": {
       "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=master;Trusted_Connection=True;TrustServerCertificate=True"
     },
   ```

3. Migrate your app (see guide below).

### **Migrating the database schema:**

I highly suggest you check out [this guide](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations) to understand the concept of database migrations and when to apply them.

#### **<span style="color: orange;">The migration steps need to be done every time the models are changed.</span>**

1. Open the Developer Powershell (<kbd>Ctrl</kbd> + <kbd>`</kbd> in Visual Studio).
2. If you don't have the dotnet cli installed (usually on the first run), Install the dotnet cli tool by running:

   ```ps
   dotnet tool install --global dotnet-ef
   ```

3. Add migration by running (use any migration name you want, for example `Init`):

   ```ps
   dotnet ef migrations add <Migration Name>
   ```

4. Update the database by running:
   ```ps
   dotnet ef database update
   ```
   _Note: This process is somethimes called `migrate`._

### Deleting a migration:

- When working on migrations and getting errors, you may want to remove the latest migration. This is harmless if the database is empty, so feel free to run the command in development:

  ```ps
  dotnet ef migrations remove
  ```
#### Working with sql server inside Docker:
   run `docker-compose up -d`

#### Connecting through SSMS:
   server name: localhost, 1433
   SQL Server Authentication
   login: sa
   password: YourStrong@Passw0rd

If the app is working in Docker too - update one of the connection strings in appsettings.json so that ```Server= <your machine ip>``` (you can run `ipconfig` in terminal)
If running in https set the connection string in `Program.cs` to `DockerConnection2`
