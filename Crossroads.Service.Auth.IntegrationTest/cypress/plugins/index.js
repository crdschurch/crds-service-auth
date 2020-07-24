const loadConfig = require('crds-cypress-config');
const waitOn = require('wait-on');

function getHealthCheckEndpoint(config){
  const match = config.baseUrl.match(/(https?)(:.*)/);
  const waitOnEndpoint = `${match[1]}-get${match[2]}/api/health/ready`;

  //Sanity check
  console.log(`Waiting for server  ${waitOnEndpoint}`);

  return waitOnEndpoint;
}

module.exports = (on, config) => {
  
  const waitOnEndpoint = getHealthCheckEndpoint(config);
  const waitOnOpts = {
    resources: [waitOnEndpoint],
    timeout: 90000
  };

  // 1. Waits for server to be available. This is necessary to avoid timeout when running on Docker in the CI pipeline.
  // 2. Loads Vault secrets configured in /config/vault_config into Cypress's config.env object
  return waitOn(waitOnOpts).then(() => loadConfig.loadConfigFromVault(config));
};