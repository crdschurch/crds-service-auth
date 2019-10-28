const loadConfig = require('crds-cypress-config');
module.exports = (on, config) => {
  return loadConfig.loadConfigFromVault(config);
};