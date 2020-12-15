import {requestAuthorization} from './requestAuthorizationHelper';

describe('Tests expired token response', function () {
  const expiredResponse = 'Lifetime validation failed. The token is expired.';

  it('Expired MP token', function () {
    const expiredMPToken = 'eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsIng1dCI6Ijkyc3c1bmhtbjBQS3N0T0k1YS1nVVZlUC1NWSIsImtpZCI6Ijkyc3c1bmhtbjBQS3N0T0k1YS1nVVZlUC1NWSJ9.eyJpc3MiOiJodHRwczovL2FkbWluaW50LmNyb3Nzcm9hZHMubmV0L21pbmlzdHJ5cGxhdGZvcm1hcGkvb2F1dGgiLCJhdWQiOiJodHRwczovL2FkbWluaW50LmNyb3Nzcm9hZHMubmV0L21pbmlzdHJ5cGxhdGZvcm1hcGkvb2F1dGgvcmVzb3VyY2VzIiwiZXhwIjoxNjA1ODA0ODA4LCJuYmYiOjE2MDU4MDMwMDgsImNsaWVudF9pZCI6IkNSRFMuQ29tbW9uIiwic2NvcGUiOlsiaHR0cDovL3d3dy50aGlua21pbmlzdHJ5LmNvbS9kYXRhcGxhdGZvcm0vc2NvcGVzL2FsbCIsIm9mZmxpbmVfYWNjZXNzIiwib3BlbmlkIl0sInN1YiI6IjhhMjkwMDkyLTM0NTktNGY1YS05NzdlLTVjYzdiMDJlNzM0MiIsImF1dGhfdGltZSI6MTYwNTgwMzAwOCwiaWRwIjoiaWRzcnYiLCJuYW1lIjoibXBjcmRzK2F1dG8rMkBnbWFpbC5jb20iLCJhbXIiOlsicGFzc3dvcmQiXX0.jB1o7a0ZRGRRFesKgJfyaAB_IPGUAxgr4HqBJ-qfArRyGhlqAwNF9ILvdgzuE7pU1E0KXVPKUXJYhZrfRfOp7rtTpFCthpW6Pbw3Gvc012mYx6Lmri8xnKjHp5JLWkGs4aW02A1uJP-pyupMofbRxwH9G0dDJv2w4qcmG1NibByXHBEm_Hf9--C7rVKa-kE3Ue-5UmZfnLaiF9LnxTW-R3OD3zCmiV9vb0hI4aqAdBNnPbDo6g8TxtYWeV5oLClojlbod04jUMfW7ZGsNIvWkeP-6euq6oKPQCyopioMOuQAKdEtXWRzZup2gCGyfit_LencJHFSR0qXpbFgnUHGtA';

    requestAuthorization(expiredMPToken, false)
      .then(response => {
        expect(response.body).to.contain(expiredResponse);
        expect(response.status).to.eq(403);
      });
  });

  //TODO Skipping this test until there is a reliable way to get valid expired Okta tokens.
  //  Expired tokens become invalid tokens when Okta changes its signing key and there is not an easy way to get a valid expired token programmatically.
  //  This test should be updated when the auth service has a way to provide expired tokens on request.
  it.skip('Expired Okta token', function () {
    const expiredOktaToken = 'eyJraWQiOiJ3eXlrOXRtS0VzSzdGNHlBSW5oX1gteGNJTlF6LXltc0VCbHNMUUExeWpFIiwiYWxnIjoiUlMyNTYifQ.eyJ2ZXIiOjEsImp0aSI6IkFULmhvWWlrZmdDRHZwa29hSEh3aUlRa2U0c3RlcWVldmYzS2VGWVRrSXVOZFEiLCJpc3MiOiJodHRwczovL2Nyb3Nzcm9hZHMub2t0YXByZXZpZXcuY29tL29hdXRoMi9kZWZhdWx0IiwiYXVkIjoiYXBpOi8vZGVmYXVsdCIsImlhdCI6MTU1NTYxNTM0OSwiZXhwIjoxNTU1NjE3MTQ5LCJjaWQiOiIwb2FrNzZncjltaUpJRklDSjBoNyIsInVpZCI6IjAwdWkzOG0xd21yeTZVeUNDMGg3Iiwic2NwIjpbIm9wZW5pZCJdLCJzdWIiOiJtcGNyZHMrYXV0bysyQGdtYWlsLmNvbSIsIm1wQ29udGFjdElkIjoiNzc3MjI0OCJ9.pwc00xXR1v616oAaQ6JudMjWpf3jpgrsTuJu1ztHksH_6mMUrFyf4jdJlBE0fKWI7D2mFLi_iFlbyynJeNlXBkIMvAFnWICOMB4k4bsk8tq_M07ZOZ42wRXbJbtqAWkmVDUDAK332LpphafMqEKc-7p9qK2sG43qWye7I2l6Ft9hYALjQoLzPATevDdOMLr_i2NX-nz6dDKINqTTqZHj15XA6NXiwb_BSn9aQjdJLg6DaH1lz6NyBCJKnWRKqG_sBbP6Qss3JbMDaOpT0KwcQ1VzpLLKAaZpj3oQNRFwWVkxj45_Tvn5lzFsJQGEP4i-3u3JawK-2A9nswukqXGRsQ';

    requestAuthorization(expiredOktaToken, false)
      .then(response => {
        expect(response.body).to.contain(expiredResponse);
        expect(response.status).to.eq(403);
      });
  });
});