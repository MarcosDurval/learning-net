#!/usr/bin/env bash
set -euo pipefail

/opt/mssql/bin/sqlservr &
SQLSERVER_PID=$!

# Wait for SQL Server to accept connections
for i in {1..30}; do
  if /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "$SA_PASSWORD" -C -Q "SELECT 1" >/dev/null 2>&1; then
    break
  fi
  sleep 1
done

# Create database if it does not exist
/opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "$SA_PASSWORD" -C -Q "IF DB_ID('projectdbz') IS NULL CREATE DATABASE [projectdbz];"

wait "$SQLSERVER_PID"
