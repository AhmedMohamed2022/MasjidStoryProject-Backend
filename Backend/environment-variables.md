# Environment Variables Configuration

## Required Environment Variables

### JWT Configuration

```bash
JWT_KEY=your-super-secret-jwt-key-here-minimum-32-characters
JWT_ISSUER=MasjidStoryApp
JWT_AUDIENCE=MasjidStoryUsers
```

### Admin User Configuration

```bash
ADMIN_EMAIL=admin@masjidstory.com
ADMIN_PASSWORD=SuperSecret123!
ADMIN_FIRST_NAME=Super
ADMIN_LAST_NAME=Admin
```

### Database Connection (for production)

```bash
DATABASE_CONNECTION_STRING=Server=your-server;Database=MasjidStoryDb;User Id=your-username;Password=your-password;TrustServerCertificate=True;
```

### File Storage (for production)

```bash
# Azure Blob Storage
AZURE_STORAGE_CONNECTION_STRING=your-azure-storage-connection-string

# AWS S3
AWS_ACCESS_KEY_ID=your-aws-access-key
AWS_SECRET_ACCESS_KEY=your-aws-secret-key
AWS_S3_BUCKET_NAME=your-s3-bucket-name
```

## How to Set Environment Variables

### Development (Windows)

```cmd
set JWT_KEY=your-super-secret-jwt-key-here-minimum-32-characters
set ADMIN_EMAIL=admin@masjidstory.com
set ADMIN_PASSWORD=SuperSecret123!
```

### Development (Linux/Mac)

```bash
export JWT_KEY=your-super-secret-jwt-key-here-minimum-32-characters
export ADMIN_EMAIL=admin@masjidstory.com
export ADMIN_PASSWORD=SuperSecret123!
```

### Production (Azure App Service)

Set in Application Settings > Configuration > Application settings

### Production (AWS)

Set in Environment Variables section of your deployment configuration

## Security Notes

- Never commit environment variables to source control
- Use strong, unique passwords for production
- Rotate JWT keys regularly
- Use managed secrets services in production (Azure Key Vault, AWS Secrets Manager)
