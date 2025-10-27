#!/bin/bash
set -e

# This script initializes the keycloak database in PostgreSQL
# It should be run once before starting Keycloak

psql -v ON_ERROR_STOP=1 --username "$POSTGRES_USER" --dbname "$POSTGRES_DB" <<-EOSQL
    -- Create keycloak database if it doesn't exist
    SELECT 'CREATE DATABASE keycloak'
    WHERE NOT EXISTS (SELECT FROM pg_database WHERE datname = 'keycloak')\gexec

    -- Grant privileges to postgres user
    GRANT ALL PRIVILEGES ON DATABASE keycloak TO postgres;
EOSQL

echo "Keycloak database initialized successfully"
