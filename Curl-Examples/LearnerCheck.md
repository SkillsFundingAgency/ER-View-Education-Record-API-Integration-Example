# Learner Check - Curl Example

The following provides an example curl request to the learner check Education Record API. You must first complete the steps in [Authorisation](Authorisation.md) document to generate a bearer token for authorisation with the API.

You require the following values from DfE
1) ApiUrl (this example uses a learner check API)
2) AuthorisationToken
3) Data

`curl -s -o NUL -w "%{http_code}\n" --http1.1 -X POST "<ApiUrl>" -H "Authorization: Bearer <AuthorisationToken>" -H "Content-Type: application/json; charset=utf-8" -H "Accept: application/json" -H "User-Agent:" --data-binary "<Data>"`[^1][^2]

This example filters out the response to just the HTTP status code which can be used by some DfE APIs to indicate success or failure.

[^1]: Replace the tokens in <> with real values
[^2]: Data can be JSON payload e.g. {\"MobileNumber\":\"012345678910\",\"DateOfBirth\":\"2008-09-02T00:00:00\"}