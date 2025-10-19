#!/bin/bash
# Script to initialize the database with migrations

set -e

# Configuration
CONFIGURATION="${CONFIGURATION:-Debug}"
TARGET_FRAMEWORK="${TARGET_FRAMEWORK:-net8.0}"

# Get the directory of this script
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
BACKEND_DIR="$(dirname "$SCRIPT_DIR")"
SRC_DIR="$BACKEND_DIR/src"
MIGRATIONS_DIR="$BACKEND_DIR/migrations"

echo "Building the solution (Configuration: $CONFIGURATION, Framework: $TARGET_FRAMEWORK)..."
cd "$BACKEND_DIR/.."
dotnet build --configuration "$CONFIGURATION"

echo "Copying migrations assembly..."
OUTPUT_DIR="$SRC_DIR/bin/$CONFIGURATION/$TARGET_FRAMEWORK"
MIGRATIONS_OUTPUT_DIR="$MIGRATIONS_DIR/bin/$CONFIGURATION/$TARGET_FRAMEWORK"

if [ ! -f "$MIGRATIONS_OUTPUT_DIR/TrailmarksApi.Migrations.dll" ]; then
    echo "Error: Migrations assembly not found at $MIGRATIONS_OUTPUT_DIR"
    exit 1
fi

cp "$MIGRATIONS_OUTPUT_DIR/TrailmarksApi.Migrations.dll" "$OUTPUT_DIR/"
cp "$MIGRATIONS_OUTPUT_DIR/TrailmarksApi.Migrations.pdb" "$OUTPUT_DIR/" 2>/dev/null || true

echo "Running database initialization..."
cd "$SRC_DIR"
dotnet "$OUTPUT_DIR/TrailmarksApi.dll" -DbInit

echo "Database initialized successfully!"
