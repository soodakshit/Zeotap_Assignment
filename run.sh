#!/bin/bash

echo "🚀 Starting Incident Tracker Application"
echo "========================================"
echo ""
echo "This will start:"
echo "  - PostgreSQL Database (port 5432)"
echo "  - .NET API Backend (port 5000)"
echo "  - React Frontend (port 3000)"
echo ""
echo "Building and starting containers..."
echo ""

docker-compose up --build

