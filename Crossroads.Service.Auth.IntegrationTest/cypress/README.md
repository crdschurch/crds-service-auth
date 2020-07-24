# Integration Tests
## Quick Start
### Test live service (api-int.crossroads.net/auth)
0) Install Node.js
1) Load [environment variables](#environment-variables)
1) `cd ./Crossroads.Service.Auth.IntegrationTest`
2) `npm ci`
3) `npm run test` (headless mode) or `npx cypress open` (interactive mode)

### Test locally hosted service with Docker
0) Install Docker
1) Load all [environment variables](#environment-variables)
2) Run `docker-compose -f ./deployment/docker-integration-tests/docker-compose.yml up --build --abort-on-container-exit --exit-code-from integration_tests`

## Environment Variables

The following environment variables must be set for all scenarios
```bash
VAULT_ROLE_ID
VAULT_SECRET_ID
```
They can be set globally or locally as long as they're accessible in the terminal you're in. The Vault role being used must have access to the environment being tested.

When running tests in Docker this variable must also be set. This configures both the app and vault environments.
```bash
CRDS_ENV
```

## Changing Configurations

Cypress tests will run against the live `api-int.crossroads.net/auth` service by default, but can be configured to run against other live or locally hosted environments by setting the baseUrl and vaultEnv variables. 

You can either configure and run in one with one command:

`npx cypress open --config baseUrl=https://api-demo.crossroads.net/auth --env vaultEnv=demo`

Or set the environment variables in your terminal for Cypress to pick up automatically
```
# powershell
$env:CYPRESS_baseUrl="https://api-demo.crossroads.net/auth"
$env:CYPRESS_vaultEnv="demo"

# bash/sh
CYPRESS_baseUrl=https://api-demo.crossroads.net/auth
CYPRESS_vaultEnv=demo
```
Then start Cypress with `npx cypress open`, `npx cypress run` or `npm run test`.

If you're using `npx` to run Cypress, you can run in interactive mode with `npx cypress open` or in headless mode with `npx cypress run`.