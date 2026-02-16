# Quick Start Guide

## 🎯 Get Started in 3 Steps

### Step 1: Prerequisites
Make sure you have Docker and Docker Compose installed:
```bash
docker --version
docker-compose --version
```

### Step 2: Clone and Start
```bash
# Clone the repository
git clone <your-repo-url>
cd incident-tracker

# Start everything with one command
docker-compose up --build
```

Wait 30-60 seconds for all services to start and the database to be seeded.

### Step 3: Access the Application
- **Frontend**: http://localhost:3000
- **API**: http://localhost:5000
- **Swagger Docs**: http://localhost:5000/swagger

## 📊 What You'll See

The application starts pre-loaded with **200 sample incidents** representing various production scenarios:
- Different severity levels (SEV1 through SEV4)
- Multiple services (Auth Service, Payment Gateway, etc.)
- Various statuses (Open, Mitigated, Resolved)
- 90 days of historical data

## 🎮 Try These Features

1. **Browse Incidents**
   - View paginated list of all incidents
   - Click column headers to sort

2. **Search & Filter**
   - Search across titles, services, and descriptions
   - Filter by service, severity, or status
   - Combine multiple filters

3. **Create New Incident**
   - Click "New Incident" button
   - Fill in the required fields
   - See validation in action

4. **View Details**
   - Click any incident row
   - View complete details
   - Click "Edit" to update

5. **Update Status**
   - Edit an incident
   - Change status from OPEN → MITIGATED → RESOLVED
   - Updates are persisted to database

## 🛑 Stop the Application

Press `Ctrl+C` in the terminal, then:
```bash
docker-compose down
```

To completely remove all data:
```bash
docker-compose down -v
```

## 🔍 Common Issues

**Port already in use?**
- Stop other services on ports 3000, 5000, or 5432
- Or modify ports in `docker-compose.yml`

**Containers won't start?**
- Check Docker is running: `docker ps`
- Try: `docker-compose down -v && docker-compose up --build`

**Frontend shows connection errors?**
- Wait 30 seconds for backend to fully start
- Check backend logs: `docker-compose logs backend`

## 📖 Next Steps

See the main [README.md](README.md) for:
- Complete API documentation
- Architecture details
- Local development setup
- Design decisions and tradeoffs
- Production deployment considerations
