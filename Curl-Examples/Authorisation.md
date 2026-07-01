# Authorisation with Microsoft Entra - Curl Example

All DfE Education Record APIs are protected with bearer token authorisation HTTP headers. The steps in this document must be completed before making any further Education Record API calls (see [LearnerCheck](LearnerCheck.md) for example bearer token usage).

The following provides an example curl request to Microsoft Entra for the purposes of receiving the authorisation token.

You require the following Microsoft Entra values from the DfE.
1) TenantId
2) ClientId
3) Secret
4) ResourceId

`curl -s -X POST "https://login.microsoftonline.com/<TenantId>/oauth2/token" -H "Content-Type: application/x-www-form-urlencoded" --data-urlencode "grant_type=client_credentials" --data-urlencode "client_id=<ClientId>" --data-urlencode "client_secret=<Secret>" --data-urlencode "resource=<ResourceId>" | powershell -NoProfile -Command "$input | ConvertFrom-Json | Select-Object -ExpandProperty access_token"`[^1][^2]

[^1]: Replace the tokens in <> with real values
[^2]: If powershell is unavailable replace with a suitable alternative or parse the output manually

The output of the above would be a long string beginning `ey` which when copy and pasted into [jwt.io](https://www.jwt.io/) decodes as a valid JSON Web Token (JWT). 