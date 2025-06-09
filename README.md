# CarDemoBackend

ASP.NET Core Web API backend for CarDemoApp with robust features.

## ✅ Features
- Authentication with xUnit tests
- Data management with EF Core
- Hash Salt .Net Bcrypt
- Robust error logging with Serilog to DB & logs folder
- Create SQL agent to clean db logs every 30 days or as per policy
- Exception handling

## 🛠️ Setup
1. Clone repo: `git clone [https://github.com/GJprocode/CarDemo.git]`
2. Install Microsoft visual studio 2022 & SQL Server/ MSSM; create `CarDemoDB`Schema and tables, SQL queries provides in SQL folder. 
3. Restore NuGet Packages: `dotnet restore`
4. Apply migrations: `dotnet ef database update`
5. Build: `dotnet build`
6. Clean: `dotnet clean`
7. Run: `dotnet run`

## 📌 Notes
- API runs at `http://localhost:5073`. Swagger will load. 
- Run tests: `dotnet test`.
- Update `appsettings.json` for DB connection or .env file
