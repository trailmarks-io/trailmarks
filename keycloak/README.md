# Keycloak Configuration

This directory contains Keycloak configuration files for the Trailmarks application.

## Files

### realm-export.json

Pre-configured Keycloak realm for Trailmarks with:
- **Realm Name**: `trailmarks`
- **Supported Languages**: English (default), German
- **Authentication**: OpenID Connect / OAuth2

**Clients:**
1. **trailmarks-frontend** - Public client for Angular SPA
   - Uses Authorization Code flow with PKCE (S256)
   - Valid redirect URIs: `http://localhost:4200/*`, `http://localhost/*`
   - Supports direct access grants for development

2. **trailmarks-backend** - Bearer-only client for API protection
   - Validates JWT tokens from frontend
   - Access token lifespan: 5 minutes

**Roles:**
- `user` - Regular user role (assigned by default)
- `moderator` - Content management role
- `admin` - Administrator role

**Features:**
- Self-registration enabled
- Password reset enabled
- Brute force protection enabled
- Remember me enabled
- Token claims: username, email, roles

### init-keycloak-db.sh

PostgreSQL initialization script that creates the `keycloak` database.
This script is automatically executed by the PostgreSQL container on first startup.

## Usage

The realm configuration is automatically imported when Keycloak starts for the first time.

### Accessing Keycloak

**Admin Console**: http://localhost:8180

**Default Credentials:**
- Username: `admin`
- Password: `admin`

**⚠️ Security Warning**: Change the default admin password before deploying to production!

### Creating Users

1. Login to Keycloak Admin Console
2. Select the `trailmarks` realm
3. Navigate to "Users" → "Add user"
4. Fill in user details and save
5. Set password in "Credentials" tab
6. Assign roles in "Role Mappings" tab

### OpenID Connect Endpoints

All endpoints are under the realm base URL: `http://localhost:8180/realms/trailmarks`

- **Discovery Document**: `http://localhost:8180/realms/trailmarks/.well-known/openid-configuration`
- **Authorization**: `http://localhost:8180/realms/trailmarks/protocol/openid-connect/auth`
- **Token**: `http://localhost:8180/realms/trailmarks/protocol/openid-connect/token`
- **Userinfo**: `http://localhost:8180/realms/trailmarks/protocol/openid-connect/userinfo`
- **Logout**: `http://localhost:8180/realms/trailmarks/protocol/openid-connect/logout`

### Modifying the Realm

To update the realm configuration:

1. Make changes in Keycloak Admin Console
2. Export the realm: Realm Settings → Action → Partial export
3. Replace `realm-export.json` with the exported file
4. Restart Keycloak: `docker-compose restart keycloak`

**Note**: Only use partial export to avoid including sensitive data.

## Production Deployment

Before deploying to production:

1. **Change admin password**: Set `KEYCLOAK_ADMIN_PASSWORD` via environment variable or `.env` file (do not hardcode in docker-compose.yml)
2. **Use production mode**: Change `start-dev` to `start` in Keycloak command
3. **Enable HTTPS**: Configure SSL certificates and set `KC_HOSTNAME_STRICT_HTTPS=true`
4. **Update redirect URIs**: Add production URLs to client configuration
5. **Enable email verification**: Configure SMTP settings and enable email verification
6. **Review token lifespans**: Adjust token expiration times based on security requirements
7. **Configure database**: Use dedicated credentials for Keycloak database user

## Documentation

For more information, see:
- [DOCKER.md](../DOCKER.md) - Complete Docker deployment guide with Keycloak section
- [Keycloak Documentation](https://www.keycloak.org/documentation)
- [OpenID Connect Specification](https://openid.net/specs/openid-connect-core-1_0.html)
