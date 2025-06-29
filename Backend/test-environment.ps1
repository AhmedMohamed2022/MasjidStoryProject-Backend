# Test Environment Variables Script
# This script tests if environment variables are set correctly

Write-Host "Testing MasjidStory Environment Variables..." -ForegroundColor Green
Write-Host ""

# Test JWT Configuration
if ($env:JWT_KEY) {
    Write-Host "✅ JWT_KEY is set" -ForegroundColor Green
} else {
    Write-Host "❌ JWT_KEY is not set" -ForegroundColor Red
}

if ($env:JWT_ISSUER) {
    Write-Host "✅ JWT_ISSUER is set: $env:JWT_ISSUER" -ForegroundColor Green
} else {
    Write-Host "❌ JWT_ISSUER is not set" -ForegroundColor Red
}

if ($env:JWT_AUDIENCE) {
    Write-Host "✅ JWT_AUDIENCE is set: $env:JWT_AUDIENCE" -ForegroundColor Green
} else {
    Write-Host "❌ JWT_AUDIENCE is not set" -ForegroundColor Red
}

# Test Admin Configuration
if ($env:ADMIN_EMAIL) {
    Write-Host "✅ ADMIN_EMAIL is set: $env:ADMIN_EMAIL" -ForegroundColor Green
} else {
    Write-Host "❌ ADMIN_EMAIL is not set" -ForegroundColor Red
}

if ($env:ADMIN_PASSWORD) {
    Write-Host "✅ ADMIN_PASSWORD is set" -ForegroundColor Green
} else {
    Write-Host "❌ ADMIN_PASSWORD is not set" -ForegroundColor Red
}

# Test Database Configuration
if ($env:DATABASE_CONNECTION_STRING) {
    Write-Host "✅ DATABASE_CONNECTION_STRING is set" -ForegroundColor Green
} else {
    Write-Host "❌ DATABASE_CONNECTION_STRING is not set" -ForegroundColor Red
}

Write-Host ""
Write-Host "If any variables are missing, run: .\set-environment-variables.ps1" -ForegroundColor Yellow 