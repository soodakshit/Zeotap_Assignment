# Incident Tracker - Full Stack Application

A production-grade incident tracking system built with .NET 8, React, TypeScript, PostgreSQL, and Docker.

## 🏗️ Architecture

### Backend (.NET 8 Web API)
- **Framework**: ASP.NET Core 8.0
- **Database**: PostgreSQL with Entity Framework Core
- **Validation**: FluentValidation
- **API Documentation**: Swagger/OpenAPI

### Frontend (React + TypeScript)
- **Framework**: React 18 with TypeScript
- **Build Tool**: Vite
- **Styling**: Tailwind CSS
- **Routing**: React Router v6
- **HTTP Client**: Axios

### Infrastructure
- **Containerization**: Docker & Docker Compose
- **Database**: PostgreSQL 16
- **Web Server**: Nginx (for frontend)

## 📋 Features

### Core Functionality
✅ Create incidents with validation  
✅ Browse incidents with server-side pagination  
✅ Advanced filtering (search, service, severity, status)  
✅ Column sorting (ascending/descending)  
✅ View incident details  
✅ Update incident status and information  
✅ 200 pre-seeded sample incidents  

### Technical Highlights
- **Server-side pagination** with configurable page size
- **Debounced search** to reduce API calls
- **Database indexing** on frequently queried fields
- **Proper validation** on both client and server
- **RESTful API** design
- **Responsive UI** with loading states
- **Type-safe** TypeScript implementation
- **Parameterized queries** via Entity Framework Core

## 🚀 Quick Start

### Prerequisites
- Docker & Docker Compose
- (Optional) .NET 8 SDK for local development
- (Optional) Node.js 20+ for local frontend development

### Running with Docker (Recommended)

1. **Clone the repository**
```bash
git clone <your-repo-url>
cd incident-tracker
```

2. **Start all services**
```bash
docker-compose up --build
```

This will start:
- PostgreSQL database on `localhost:5432`
- .NET API on `http://localhost:5000`
- React frontend on `http://localhost:3000`

3. **Access the application**
- Open browser to `http://localhost:3000`
- API docs available at `http://localhost:5000/swagger`

4. **Stop services**
```bash
docker-compose down
```

To remove data volumes:
```bash
docker-compose down -v
```

## 🛠️ Local Development

### Backend Setup

1. **Navigate to backend directory**
```bash
cd backend
```

2. **Restore dependencies**
```bash
dotnet restore
```

3. **Update connection string** (if not using Docker)
Edit `appsettings.json` to point to your PostgreSQL instance.

4. **Run migrations**
```bash
dotnet ef database update
```

5. **Run the API**
```bash
dotnet run
```

API will be available at `http://localhost:5000`

### Frontend Setup

1. **Navigate to frontend directory**
```bash
cd frontend
```

2. **Install dependencies**
```bash
npm install
```

3. **Start development server**
```bash
npm run dev
```

Frontend will be available at `http://localhost:3000`

## 📡 API Overview

### Base URL
```
http://localhost:5000/api
```

### Endpoints

#### Get Incidents (with pagination, filtering, sorting)
```http
GET /api/incidents?page=1&pageSize=10&search=timeout&service=Auth Service&severity=SEV1&status=OPEN&sortBy=createdAt&sortOrder=desc
```

**Query Parameters:**
- `page` (default: 1) - Page number
- `pageSize` (default: 10, max: 100) - Items per page
- `search` - Search in title, service, owner, summary
- `service` - Filter by exact service name
- `severity` - Filter by severity (SEV1, SEV2, SEV3, SEV4)
- `status` - Filter by status (OPEN, MITIGATED, RESOLVED)
- `sortBy` - Sort column (title, service, severity, status, createdAt, updatedAt)
- `sortOrder` - Sort direction (asc, desc)

**Response:**
```json
{
  "data": [
    {
      "id": "uuid",
      "title": "High latency detected in Auth Service",
      "service": "Auth Service",
      "severity": "SEV1",
      "status": "OPEN",
      "owner": "John Doe",
      "summary": "Users experiencing slow login times...",
      "createdAt": "2024-02-15T10:30:00Z",
      "updatedAt": "2024-02-15T10:30:00Z"
    }
  ],
  "page": 1,
  "pageSize": 10,
  "totalCount": 200,
  "totalPages": 20
}
```

#### Get Single Incident
```http
GET /api/incidents/{id}
```

#### Create Incident
```http
POST /api/incidents
Content-Type: application/json

{
  "title": "Database connection timeout",
  "service": "User Service",
  "severity": "SEV2",
  "status": "OPEN",
  "owner": "Jane Smith",
  "summary": "Multiple connection timeouts observed..."
}
```

#### Update Incident (Partial Update)
```http
PATCH /api/incidents/{id}
Content-Type: application/json

{
  "status": "RESOLVED",
  "summary": "Issue resolved by increasing connection pool size."
}
```

## 🗄️ Database Schema

### Incidents Table
| Column | Type | Description |
|--------|------|-------------|
| id | UUID | Primary key |
| title | VARCHAR(200) | Incident title (required) |
| service | VARCHAR(100) | Affected service (required) |
| severity | ENUM | SEV1, SEV2, SEV3, SEV4 (required) |
| status | ENUM | OPEN, MITIGATED, RESOLVED (required) |
| owner | VARCHAR(100) | Assigned owner (optional) |
| summary | VARCHAR(2000) | Detailed description (optional) |
| created_at | TIMESTAMP | Creation timestamp |
| updated_at | TIMESTAMP | Last update timestamp |

**Indexes:**
- Primary key on `id`
- Index on `service`
- Index on `severity`
- Index on `status`
- Index on `created_at`

## 🎨 Design Decisions & Tradeoffs

### Backend
1. **Entity Framework Core over Dapper**
   - ✅ Faster development with migrations
   - ✅ Type-safe LINQ queries
   - ✅ Built-in change tracking
   - ⚠️ Slightly more overhead than raw SQL

2. **FluentValidation**
   - ✅ Clean separation of validation logic
   - ✅ Reusable validation rules
   - ✅ Easy to test

3. **Enums for Severity and Status**
   - ✅ Type safety
   - ✅ Database-enforced constraints
   - ⚠️ Requires migration to add new values

4. **PATCH for Updates**
   - ✅ Allows partial updates
   - ✅ More flexible than PUT
   - ⚠️ Slightly more complex validation

### Frontend
1. **Vite over Create React App**
   - ✅ Much faster build times
   - ✅ Better development experience
   - ✅ Modern tooling

2. **Tailwind CSS**
   - ✅ Rapid UI development
   - ✅ Consistent design system
   - ✅ Small production bundle
   - ⚠️ Requires learning utility classes

3. **Debounced Search**
   - ✅ Reduces API calls
   - ✅ Better performance
   - ⚠️ 500ms delay may feel slightly laggy

4. **Client-side Routing**
   - ✅ Fast navigation
   - ✅ Better UX
   - ⚠️ Requires nginx configuration for SPA

### Infrastructure
1. **Docker Compose for Development**
   - ✅ Consistent environment
   - ✅ Easy setup
   - ✅ Includes all dependencies

2. **Nginx for Frontend**
   - ✅ Production-ready
   - ✅ Efficient static file serving
   - ✅ Easy SSL termination (if needed)

## 🚧 Improvements for Production

### High Priority
1. **Authentication & Authorization**
   - Add JWT/OAuth2 authentication
   - Implement role-based access control
   - Add user management

2. **Error Handling**
   - Global exception handler
   - Structured logging (Serilog)
   - Error tracking (e.g., Sentry)

3. **Testing**
   - Unit tests for business logic
   - Integration tests for API endpoints
   - E2E tests for critical flows

4. **API Security**
   - Rate limiting
   - CORS configuration review
   - Input sanitization
   - SQL injection prevention (already using EF Core)

### Medium Priority
5. **Performance Optimization**
   - Redis caching layer
   - CDN for static assets
   - Database query optimization
   - Lazy loading for large lists

6. **Monitoring & Observability**
   - Application Performance Monitoring (APM)
   - Health check endpoints
   - Metrics collection (Prometheus)
   - Distributed tracing

7. **Enhanced Features**
   - Incident timeline/history
   - File attachments
   - Comments/discussions
   - Email notifications
   - Real-time updates (SignalR)
   - Export to CSV/PDF

8. **UI/UX Improvements**
   - Dark mode
   - Keyboard shortcuts
   - Bulk actions
   - Advanced filters (date range, custom queries)
   - Data visualization (charts, metrics)

### Nice to Have
9. **DevOps**
   - CI/CD pipeline
   - Kubernetes deployment
   - Infrastructure as Code (Terraform)
   - Database backup strategy

10. **Documentation**
    - API documentation (expand Swagger)
    - Architecture decision records (ADRs)
    - Runbooks for operations

## 📦 Project Structure

```
incident-tracker/
├── backend/
│   ├── Controllers/          # API endpoints
│   ├── Data/                 # DbContext and seeder
│   ├── DTOs/                 # Data transfer objects
│   ├── Models/               # Domain models
│   ├── Validators/           # FluentValidation rules
│   ├── Program.cs            # Application entry point
│   ├── Dockerfile
│   └── IncidentTracker.Api.csproj
├── frontend/
│   ├── src/
│   │   ├── components/       # Reusable components
│   │   ├── pages/            # Page components
│   │   ├── services/         # API client
│   │   ├── types/            # TypeScript types
│   │   ├── utils/            # Helper functions
│   │   ├── App.tsx           # Root component
│   │   └── main.tsx          # Entry point
│   ├── Dockerfile
│   ├── nginx.conf
│   └── package.json
├── docker-compose.yml
└── README.md
```

## 🧪 Testing the Application

### Manual Testing
1. **Create an Incident**
   - Click "New Incident" button
   - Fill in the form
   - Verify validation errors appear
   - Submit and verify creation

2. **Filter & Search**
   - Use search box (try: "timeout")
   - Filter by service, severity, status
   - Verify results update correctly

3. **Sort**
   - Click column headers
   - Verify ascending/descending order
   - Try sorting by different columns

4. **Pagination**
   - Navigate through pages
   - Change filters and verify pagination resets
   - Check total count accuracy

5. **Update Incident**
   - Click on an incident
   - Click "Edit"
   - Change status or other fields
   - Save and verify changes persist

### API Testing with cURL

**Get incidents:**
```bash
curl http://localhost:5000/api/incidents?page=1&pageSize=5
```

**Create incident:**
```bash
curl -X POST http://localhost:5000/api/incidents \
  -H "Content-Type: application/json" \
  -d '{
    "title": "Test Incident",
    "service": "Test Service",
    "severity": "SEV3",
    "status": "OPEN"
  }'
```

**Update incident:**
```bash
curl -X PATCH http://localhost:5000/api/incidents/{id} \
  -H "Content-Type: application/json" \
  -d '{"status": "RESOLVED"}'
```

## 🐛 Troubleshooting

### Database Connection Issues
- Ensure PostgreSQL is running: `docker-compose ps`
- Check connection string in `appsettings.json`
- Verify database exists: `docker-compose exec postgres psql -U postgres -l`

### Frontend Can't Connect to API
- Check if backend is running: `curl http://localhost:5000/api/incidents`
- Verify CORS settings in `Program.cs`
- Check browser console for errors

### Port Already in Use
- Stop conflicting services
- Or modify ports in `docker-compose.yml`

## 📝 License

This project is for educational/evaluation purposes.

## 👤 Author

Software Engineer Assignment Submission

---

**Estimated Development Time**: 8 hours  
**Tech Stack**: .NET 8, React 18, TypeScript, PostgreSQL, Docker, Tailwind CSS
