@ECHO OFF
@echo Update Repository
git status 
git add .
git commit -m "Load save state"
git pull origin main 
@echo Create database
sqlcmd -S .\SQLEXPRESS -i database.sql
@echo Create tables
sqlcmd -S .\SQLEXPRESS -i tables.sql
@echo Insert Default values
sqlcmd -S .\SQLEXPRESS -i insert.sql
@echo Update Dal Context
IF EXIST %CD%\OAuth.Dal (
@echo Remove Previous
@del /s /q OAuth.Dal
@echo %CD%\OAuth.Dal removed
)
@echo Create Project
dotnet new classlib -n OAuth.Dal
@echo Add Nugets Packages
dotnet add OAuth.Dal package Microsoft.EntityFrameworkCore.SqlServer
dotnet add OAuth.Dal package Microsoft.EntityFrameworkCore.Design
@echo Create Context
dotnet ef dbcontext scaffold "Server=.\SQLEXPRESS;Database=OAuth;Trusted_Connection=true;" Microsoft.EntityFrameworkCore.SqlServer -o Models -p OAuth.Dal
@echo Move context 
move OAuth.Dal\Models\OAuthContext.cs OAuth.Dal
@echo Remove Surplus
rm %CD%\OAuth.Dal\Class1.cs
@echo Insert Defaults
sqlcmd -S .\SQLEXPRESS -i insert.sql
@echo Save Update
git status 
git add .
git commit -m "Update database"
@echo Run Service
dotnet run -p OAuth.Api


