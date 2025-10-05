// For Docker deployment, the API URL should point to the backend service
// In development, it uses localhost:8080
// In production with Docker, it should use the host machine's address
export const environment = {
  production: true,
  apiUrl: 'http://localhost:8080',
  otlpEndpoint: 'http://localhost:4318/v1/traces'
};
