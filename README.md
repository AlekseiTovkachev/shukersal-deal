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

#### Working with SQL Server inside Docker or a remote server:
To start the SQL Server instance inside Docker, run the following command: `docker-compose up -d`

#### Connecting through SSMS:
- Server name: `localhost, 1433`
- Authentication: SQL Server Authentication
- Login: `sa`
- Password: `YourStrong@Passw0rd`

If the application is also running inside Docker, update one of the connection strings in `appsettings.json` to use your machine's IP address. You can find your machine's IP address by running the `ipconfig` command in your terminal. For a remote server, specify the server's IP address in the connection string.

If the application is running over HTTPS, set the connection string in `Program.cs` to `DockerConnection2`.


## Boot Files Format

### Members

The boot file for members should be a JSON file named `members_bootfile.json` following the specified format. There are two possible roles: Administrator or Member.

```json
[
  {
    "Username": "Username",
    "Password": "Password",
    "Role": "Administrator"
  },
  {
    "Username": "U2",
    "Password": "password",
    "Role": "Member"
  },
  ...
]
```

### Stores

The boot file for stores should be a JSON file named `stores_bootfile.json` following the provided format. The file should contain two arrays: the first array should contain the usernames of the store founders, and the second array should contain store data.

```json
[
  ["Store_1_Founder_Name", "Store_2_Founder_Name", "Store_3_Founder_Name", ...],
  [
    {
      "name": "Store_1",
      "description": "Some description"
    },
    {
      "name": "Store_2",
      "description": "Some description"
    },
    {
      "name": "Store_3",
      "description": "Some description"
    },
    ...
  ]
]
```

### Products

The boot file for products should be a JSON file named `products_bootfile.json`. It should contain an array of product objects with the following properties:

```json
[
  {
    "name": "Apple",
    "description": "A string describing the product",
    "price": 20,
    "unitsInStock": 10,
    "categoryId": 11,
    "storename": "S"
  },
  {
    "name": "apple",
    "description": "A string describing the product",
    "price": 10,
    "unitsInStock": 10,
    "categoryId": 11,
    "storename": "Store2"
  },
  ...
]
```

### Managers

The boot file for managers should be a JSON file named `managers_bootfile.json`. It should contain an array of manager objects with the following properties:

```json
[
  {
    "BossMemberName": "john123",
    "NewManagerName": "jane456",
    "StoreName": "Store1",
    "Owner": true/false
  },
  {
    "BossMemberName": "jane456",
    "NewManagerName": "john123",
    "StoreName": "Store2",
    "Owner": true/false
  },
  ...
]
```
"Owner" field marks the type of Store Manager - owner or regular store manager.

Additionally, every store must be assigned to one of the members from the bootfile, and also only one of those members can become a store manager/owner
