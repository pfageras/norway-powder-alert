#!/bin/bash

echo "🎿 Starting Norway Powder Alert..."
echo ""

# Check if Docker is installed
if ! command -v docker &> /dev/null; then
    echo "❌ Docker is not installed. Please install Docker first."
    echo "Visit: https://www.docker.com/get-started"
    exit 1
fi

# Check if docker-compose is available
if ! command -v docker-compose &> /dev/null; then
    echo "❌ docker-compose is not installed. Please install docker-compose first."
    exit 1
fi

echo "✅ Docker found!"
echo ""
echo "Building and starting the application..."
echo ""

docker-compose up --build -d

if [ $? -eq 0 ]; then
    echo ""
    echo "✅ Norway Powder Alert is now running!"
    echo ""
    echo "🌐 Open your browser and visit:"
    echo "   http://localhost:8080"
    echo ""
    echo "To view logs:"
    echo "   docker-compose logs -f"
    echo ""
    echo "To stop the application:"
    echo "   docker-compose down"
    echo ""
else
    echo ""
    echo "❌ Failed to start the application."
    echo "Check the error messages above."
    exit 1
fi
