# Swagger API Documentation

## Overview

The Influencer Marketplace API includes comprehensive Swagger/OpenAPI documentation for all endpoints.

## Accessing Swagger UI

Once the application is running, you can access the Swagger UI at:

```
http://localhost:5000/swagger
```

Or if using HTTPS:

```
https://localhost:5001/swagger
```

## Features

### 1. **Interactive API Testing**
- Test all API endpoints directly from the browser
- View request/response schemas
- See example values for all parameters

### 2. **JWT Authentication**
- Click the "Authorize" button at the top of the Swagger UI
- Enter your JWT token in the format: `Bearer {your-token}`
- All protected endpoints will now include the authorization header

### 3. **Comprehensive Documentation**
- XML comments from controllers are automatically included
- Response codes and descriptions for each endpoint
- Parameter descriptions and examples
- Schema definitions for all DTOs

### 4. **API Information**
- API version and title
- Contact information
- License details

## Configuration

Swagger can be enabled/disabled via `appsettings.json`:

```json
{
  "EnableSwagger": true
}
```

By default, Swagger is enabled in Development environment and can be controlled via the `EnableSwagger` setting in other environments.

## Endpoints Documentation

### Authentication (`/api/auth`)
- **POST /api/auth/login** - Authenticate and get JWT token
- **POST /api/auth/register** - Register new user account
- **GET /api/auth/me** - Get current user information (requires authentication)

### Campaigns (`/api/campaign`)
- **GET /api/campaign/{id}** - Get campaign by ID
- **GET /api/campaign/brand** - Get all campaigns for authenticated brand (Brand role required)
- **GET /api/campaign/influencer** - Get all campaigns for authenticated influencer (Influencer role required)
- **GET /api/campaign/available** - Get available campaigns (Influencer role required)
- **POST /api/campaign** - Create new campaign (Brand role required)
- **PUT /api/campaign/{id}** - Update campaign (Brand role required)
- **DELETE /api/campaign/{id}** - Delete campaign (Brand role required)
- **POST /api/campaign/{id}/apply** - Apply to campaign (Influencer role required)
- **PUT /api/campaign/{id}/status** - Update campaign status (Brand role required)

### Influencer Profiles (`/api/influencerprofile`)
- Profile management endpoints for influencers

### Payments (`/api/payment`)
- Payment and escrow management endpoints

### Wallets (`/api/wallet`)
- Wallet balance and transaction endpoints

## Using Swagger UI

1. **Start the application**
   ```bash
   cd backend/src/API
   dotnet run
   ```

2. **Open Swagger UI**
   - Navigate to `http://localhost:5000/swagger` in your browser

3. **Authenticate**
   - Use the `/api/auth/login` endpoint to get a JWT token
   - Click "Authorize" button and enter: `Bearer {your-token}`
   - Click "Authorize" to save

4. **Test Endpoints**
   - Expand any endpoint to see details
   - Click "Try it out" to test the endpoint
   - Fill in required parameters
   - Click "Execute" to send the request
   - View the response below

## Swagger JSON

The OpenAPI specification is available at:

```
http://localhost:5000/swagger/v1/swagger.json
```

This can be imported into tools like Postman, Insomnia, or used for code generation.

## Customization

Swagger configuration is in `Program.cs`. You can customize:

- API information (title, description, version)
- Security schemes
- XML documentation inclusion
- Schema filters for examples
- UI appearance and behavior

## Notes

- XML documentation comments in controllers are automatically included
- Example values are provided for common fields via `SwaggerExampleSchemaFilter`
- JWT Bearer authentication is pre-configured
- All endpoints show proper HTTP status codes and response types

