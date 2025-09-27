# Influencer Marketing Platform

A full-stack SaaS platform connecting brands with influencers for marketing campaigns.

## Project Overview

The platform connects brands (vendors) with influencers (creators):
- Brands can search for influencers, create campaigns, deposit payments into escrow, and approve campaign delivery.
- Influencers can list their profile, accept/reject campaigns, deliver proof (post links/screenshots), and withdraw earnings.
- The system supports local wallet payments (stub for eSewa/Khalti integration), escrow management, and commission tracking.

## Tech Stack

### Frontend
- **Framework**: SvelteKit
- **Styling**: TailwindCSS
- **Features**:
  - Responsive UI
  - JWT Authentication
  - Brand dashboard
  - Influencer dashboard
  - Admin panel

### Backend
- **.NET 8 Web API**
- **ORM**: Dapper
- **Features**:
  - JWT Authentication & Role-based Authorization
  - Campaign workflow endpoints
  - Escrow simulation
  - Commission calculation
  - Logging & error handling middleware

### Database
- **PostgreSQL**
- **Key Entities**: Users, Roles, InfluencerProfiles, Campaigns, CampaignDeliverables, Payments, Wallets, Reviews, Notifications

### DevOps
- **Azure DevOps**
- **CI/CD Pipelines** for frontend and backend
- **Environments**: Dev, Staging, Prod

## Getting Started

### Prerequisites
- .NET 8 SDK
- Node.js (v16+)
- PostgreSQL
- Docker (optional)

### Backend Setup
```bash
cd backend
dotnet restore
dotnet build
dotnet run
```

### Frontend Setup
```bash
cd frontend
npm install
npm run dev
```

### Database Setup
```bash
cd backend
dotnet ef database update
```

## Project Structure

```
inf-market/
├── backend/                # .NET 8 Web API
│   ├── src/                # Source code
│   │   ├── API/            # API Controllers and middleware
│   │   ├── Core/           # Business logic and domain models
│   │   ├── Infrastructure/ # Data access and external services
│   │   └── Shared/         # Shared utilities and DTOs
│   └── tests/              # Unit and integration tests
├── frontend/               # SvelteKit application
│   ├── src/                # Source code
│   │   ├── lib/            # Shared components and utilities
│   │   ├── routes/         # Page routes
│   │   └── stores/         # State management
│   └── static/             # Static assets
├── database/               # Database scripts and migrations
└── devops/                 # CI/CD and deployment configurations
```

## License
[MIT](LICENSE)