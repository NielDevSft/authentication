-- The database DBAuthentication is created by the POSTGRES_DB environment variable.
-- This script runs connected to that database as the postgres superuser.

-- Enable pgcrypto for SHA1 hashing
CREATE EXTENSION IF NOT EXISTS pgcrypto;

-- Create schema
CREATE SCHEMA IF NOT EXISTS app;

-- Create migrator user
DO $$
BEGIN
  IF NOT EXISTS (SELECT FROM pg_catalog.pg_roles WHERE rolname = 'migrator') THEN
    CREATE ROLE migrator LOGIN PASSWORD 'migrator123!';
  END IF;
END
$$;

-- Create AdmAuthentication user
DO $$
BEGIN
  IF NOT EXISTS (SELECT FROM pg_catalog.pg_roles WHERE rolname = 'AdmAuthentication') THEN
    CREATE ROLE "AdmAuthentication" LOGIN PASSWORD 'AuthCation@247';
  END IF;
END
$$;

GRANT ALL PRIVILEGES ON DATABASE "DBAuthentication" TO migrator;
GRANT ALL PRIVILEGES ON DATABASE "DBAuthentication" TO "AdmAuthentication";
GRANT USAGE, CREATE ON SCHEMA app TO migrator;
GRANT USAGE, CREATE ON SCHEMA app TO "AdmAuthentication";
ALTER DEFAULT PRIVILEGES IN SCHEMA app GRANT ALL ON TABLES TO migrator;
ALTER DEFAULT PRIVILEGES IN SCHEMA app GRANT ALL ON TABLES TO "AdmAuthentication";
ALTER DEFAULT PRIVILEGES IN SCHEMA app GRANT ALL ON SEQUENCES TO migrator;
ALTER DEFAULT PRIVILEGES IN SCHEMA app GRANT ALL ON SEQUENCES TO "AdmAuthentication";