# MasjidStory Environment Variables Setup Script
# Run this script to set up environment variables for development

Write-Host "Setting up MasjidStory environment variables..." -ForegroundColor Green

# JWT Configuration
$env:JWT_KEY = "ssDOlAHOOcMkDK0lt3CKnzOnlrfSdWxzJzEZdPRvvIs"
$env:JWT_ISSUER = "MasjidStoryApp"
$env:JWT_AUDIENCE = "MasjidStoryUsers"

# Admin User Configuration
$env:ADMIN_EMAIL = "admin@masjidstory.com"
$env:ADMIN_PASSWORD = "SuperSecret123!"
$env:ADMIN_FIRST_NAME = "Super"
$env:ADMIN_LAST_NAME = "Admin"

# Database Connection (using local for development)
$env:DATABASE_CONNECTION_STRING = "Server=.;Database=MasjidStoryDb;Trusted_Connection=True;TrustServerCertificate=True;"

Write-Host "Environment variables set successfully!" -ForegroundColor Green
Write-Host ""
Write-Host "JWT_KEY: $env:JWT_KEY" -ForegroundColor Yellow
Write-Host "ADMIN_EMAIL: $env:ADMIN_EMAIL" -ForegroundColor Yellow
Write-Host "DATABASE_CONNECTION_STRING: $env:DATABASE_CONNECTION_STRING" -ForegroundColor Yellow
Write-Host ""
Write-Host "You can now run the application with: dotnet run" -ForegroundColor Cyan 