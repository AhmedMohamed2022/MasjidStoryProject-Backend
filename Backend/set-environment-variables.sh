#!/bin/bash

# MasjidStory Environment Variables Setup Script
# Run this script to set up environment variables for development

echo "Setting up MasjidStory environment variables..."

# JWT Configuration
export JWT_KEY="ssDOlAHOOcMkDK0lt3CKnzOnlrfSdWxzJzEZdPRvvIs"
export JWT_ISSUER="MasjidStoryApp"
export JWT_AUDIENCE="MasjidStoryUsers"

# Admin User Configuration
export ADMIN_EMAIL="admin@masjidstory.com"
export ADMIN_PASSWORD="SuperSecret123!"
export ADMIN_FIRST_NAME="Super"
export ADMIN_LAST_NAME="Admin"

# Database Connection (using local for development)
export DATABASE_CONNECTION_STRING="Server=.;Database=MasjidStoryDb;Trusted_Connection=True;TrustServerCertificate=True;"

echo "Environment variables set successfully!"
echo ""
echo "JWT_KEY: $JWT_KEY"
echo "ADMIN_EMAIL: $ADMIN_EMAIL"
echo "DATABASE_CONNECTION_STRING: $DATABASE_CONNECTION_STRING"
echo ""
echo "You can now run the application with: dotnet run" 