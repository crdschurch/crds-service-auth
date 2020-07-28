export function requestAuthorization(authorization, failOnStatusCode = true) {
  return cy.request({
    method: 'GET',
    url: '/api/authorize',
    headers: { authorization },
    failOnStatusCode
  });
}