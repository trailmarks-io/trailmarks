#!/bin/bash
# Script to initialize the database with migrations

set -e

# Get the directory of this script
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
BACKEND_DIR="$(dirname "$SCRIPT_DIR")"
SRC_DIR="$BACKEND_DIR/src"
MIGRATIONS_DIR="$BACKEND_DIR/migrations"

echo "Building the solution..."
cd "$BACKEND_DIR/.."
dotnet build

echo "Copying migrations assembly..."
OUTPUT_DIR="$SRC_DIR/bin/Debug/net8.0"
cp "$MIGRATIONS_DIR/bin/Debug/net8.0/TrailmarksApi.Migrations.dll" "$OUTPUT_DIR/"
cp "$MIGRATIONS_DIR/bin/Debug/net8.0/TrailmarksApi.Migrations.pdb" "$OUTPUT_DIR/" 2>/dev/null || true

echo "Running database initialization..."
cd "$SRC_DIR"
dotnet "$OUTPUT_DIR/TrailmarksApi.dll" -DbInit

echo "Database initialized successfully!"
