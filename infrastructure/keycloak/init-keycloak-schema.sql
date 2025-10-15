-- Create Keycloak schema
CREATE SCHEMA IF NOT EXISTS keycloak;

-- Grant permissions to the postgres user
GRANT ALL PRIVILEGES ON SCHEMA keycloak TO postgres;
GRANT ALL PRIVILEGES ON ALL TABLES IN SCHEMA keycloak TO postgres;
GRANT ALL PRIVILEGES ON ALL SEQUENCES IN SCHEMA keycloak TO postgres;

-- Set default privileges for future objects
ALTER DEFAULT PRIVILEGES IN SCHEMA keycloak GRANT ALL PRIVILEGES ON TABLES TO postgres;
ALTER DEFAULT PRIVILEGES IN SCHEMA keycloak GRANT ALL PRIVILEGES ON SEQUENCES TO postgres;
