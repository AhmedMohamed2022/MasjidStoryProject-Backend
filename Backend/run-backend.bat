@echo off
echo Setting up MasjidStory environment variables...

REM Set environment variables for the current session
set JWT_KEY=ssDOlAHOOcMkDK0lt3CKnzOnlrfSdWxzJzEZdPRvvIs
set JWT_ISSUER=MasjidStoryApp
set JWT_AUDIENCE=MasjidStoryUsers
set ADMIN_EMAIL=admin@masjidstory.com
set ADMIN_PASSWORD=SuperSecret123!
set ADMIN_FIRST_NAME=Super
set ADMIN_LAST_NAME=Admin
set DATABASE_CONNECTION_STRING=Server=.;Database=MasjidStoryDb;Trusted_Connection=True;TrustServerCertificate=True;

echo Environment variables set successfully!
echo.
echo JWT_KEY: %JWT_KEY%
echo ADMIN_EMAIL: %ADMIN_EMAIL%
echo DATABASE_CONNECTION_STRING: %DATABASE_CONNECTION_STRING%
echo.

REM Navigate to the MasjidStory directory and run the application
cd MasjidStory
echo Starting the backend application...

REM Run dotnet with the environment variables set in the current session
dotnet run

pause 