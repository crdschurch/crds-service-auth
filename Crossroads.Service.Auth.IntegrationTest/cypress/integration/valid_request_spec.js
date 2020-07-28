const { requestAuthorization } = require('./requestAuthorizationHelper');

function getMPUserToken(username, password) {
  return cy.request({
    method: 'POST',
    url: `${Cypress.env('CRDS_GATEWAY_BASE_URL')}/api/login`,
    body: {
      username,
      password
    }
  }).its('body.userToken');
}

function getOktaToken(username, password) {
  return cy.request({
    method: 'POST',
    url: `${Cypress.env('OKTA_OAUTH_BASE_URL')}/v1/token`,
    headers: { authorization: `${Cypress.env('OKTA_TOKEN_AUTH')}` },
    form: true,
    body: {
      grant_type: 'password',
      username,
      password,
      scope: 'openid'
    }
  }).its('body.access_token');
}

const password = Cypress.env('BEN_KENOBI_PW');
const username = 'mpcrds+auto+2@gmail.com';

const mpResponse = {
  'Authentication': {
    'Provider': 'mp'
  },
  'Authorization': {
    'MpRoles': {
      '39': 'All Platform Users'
    },
    'OktaRoles': null
  },
  'UserInfo': {
    'Mp': {
      'Email': 'mpcrds+auto+2@gmail.com',
      'ContactId': 7772248,
      'UserId': 4488274,
      'ParticipantId': 7654359,
      'HouseholdId': 5819396,
      'DonorId': 7745938,
      'CanImpersonate': false
    }
  }
};

const oktaResponse = {
  'Authentication': {
    'Provider': 'okta'
  },
  'Authorization': {
    'MpRoles': {
      '39': 'All Platform Users'
    },
    'OktaRoles': {}
  },
  'UserInfo': {
    'Mp': {
      'Email': 'mpcrds+auto+2@gmail.com',
      'ContactId': 7772248,
      'UserId': 4488274,
      'ParticipantId': 7654359,
      'HouseholdId': 5819396,
      'DonorId': 7745938,
      'CanImpersonate': false
    }
  }
};

describe('Tests authorization response content', function () {
  it('Verifies provider, roles and user info when authorized with MP token', function () {
    getMPUserToken(username, password)
      .then(requestAuthorization)
      .its('body')
      .then(body => {
        expect(body.Authentication).to.deep.equal(mpResponse.Authentication);
        expect(body.Authorization).to.deep.equal(mpResponse.Authorization, `Actual ${JSON.stringify(body.Authorization)} vs. Expected ${JSON.stringify(mpResponse.Authorization)}`);
        expect(body.UserInfo).to.deep.equal(mpResponse.UserInfo, `Actual ${JSON.stringify(body.UserInfo)} vs. Expected ${JSON.stringify(mpResponse.UserInfo)}`);
      });
  });

  it('Verifies provider, roles and user info when authorized with Okta token', function () {
    getOktaToken(username, password)
      .then(requestAuthorization)
      .its('body')
      .then(body => {
        expect(body.Authentication).to.deep.equal(oktaResponse.Authentication);
        expect(body.Authorization).to.deep.equal(oktaResponse.Authorization, `Actual ${JSON.stringify(body.Authorization)} vs. Expected ${JSON.stringify(oktaResponse.Authorization)}`);
        expect(body.UserInfo).to.deep.equal(oktaResponse.UserInfo, `Actual ${JSON.stringify(body.UserInfo)} vs. Expected ${JSON.stringify(oktaResponse.UserInfo)}`);
      });
  });
});
