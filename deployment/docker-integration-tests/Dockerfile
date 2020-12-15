FROM cypress/included:5.6.0
WORKDIR /Crossroads.Service.Auth.IntegrationTest

COPY Crossroads.Service.Auth.IntegrationTest .

ENV CI=1
RUN npm ci --production && npx cypress verify

ENTRYPOINT [ "npx", "cypress", "run"]