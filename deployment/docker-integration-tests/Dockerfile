FROM cypress/included:4.11.0
WORKDIR /Crossroads.Service.Auth.IntegrationTest

COPY ./Crossroads.Service.Auth.IntegrationTest ./
ENV CI=1
RUN npm ci

RUN npx cypress verify

ENTRYPOINT [ "npm", "run", "test" ]