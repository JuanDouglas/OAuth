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
SET Text=using OAuth.Dal.Models;
SET Directory=%CD%\OAuth.Dal\OAuthContext.cs
setlocal

call :FindReplace "OAuth.Dal.Models" "OAuth.Dal" %Directory%
call :FindReplace "using System" "using OAuth.Dal.Models" %Directory%

exit /b 

:FindReplace <findstr> <replstr> <file>
set tmp="%temp%\tmp.txt"
If not exist %temp%\_.vbs call :MakeReplace
for /f "tokens=*" %%a in ('dir "%3" /s /b /a-d /on') do (
  for /f "usebackq" %%b in (`Findstr /mic:"%~1" "%%a"`) do (
    echo(&Echo Replacing "%~1" with "%~2" in file %%~nxa
    <%%a cscript //nologo %temp%\_.vbs "%~1" "%~2">%tmp%
    if exist %tmp% move /Y %tmp% "%%~dpnxa">nul
  )
)
del %temp%\_.vbs
exit /b

:MakeReplace
>%temp%\_.vbs echo with Wscript
>>%temp%\_.vbs echo set args=.arguments
>>%temp%\_.vbs echo .StdOut.Write _
>>%temp%\_.vbs echo Replace(.StdIn.ReadAll,args(0),args(1),1,-1,1)
>>%temp%\_.vbs echo end with

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