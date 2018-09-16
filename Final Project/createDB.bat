
echo off

rem batch file to run a script to create a db
rem 8/29/2017

sqlcmd -S localhost -E -i projectDB.sql

rem sqlcmd -S localhost/sqlexpress -E -i equipmentDB.sql
rem sqlcmd -S localhost/mssqlserver -E -i equipmentDB.sql

ECHO .
ECHO if no error messages appear DB was created
PAUSE