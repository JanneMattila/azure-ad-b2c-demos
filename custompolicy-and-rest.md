# Custom policy & Rest API example

## Official documentation links

https://learn.microsoft.com/en-us/azure/active-directory-b2c/custom-policy-overview

https://learn.microsoft.com/en-us/azure/active-directory-b2c/api-connectors-overview?pivots=b2c-custom-policy

https://learn.microsoft.com/en-us/azure/active-directory-b2c/tutorial-create-user-flows?pivots=b2c-custom-policy

https://learn.microsoft.com/en-us/azure/active-directory-b2c/add-api-connector-token-enrichment?pivots=b2c-custom-policy

## Scenario

You want to call Rest API from your custom policy to add additional claims to the token.

Here is the sequence diagram of the scenario:

```mermaid
sequenceDiagram
    actor User
    participant Application
    participant Azure AD B2C
    participant Rest API
    User->>Application: Go to application<br/>as anonymous user
    Application->>User: Redirect to login
    User->>Azure AD B2C: Login
    Azure AD B2C-->>Rest API: Fetch additonal<br/>details
    Rest API-->>Azure AD B2C: Return "customColors"
    Azure AD B2C->>User: Redirect back to application
    User->>Application: Use application<br/>as logged in user
```

Rest API will return `customColors` string array.

Add new claim type to define this type:

```xml
<ClaimType Id="customColors">
  <DisplayName>Favorite colors</DisplayName>
  <DataType>stringCollection</DataType>
  <UserHelpText>Favorite colors.</UserHelpText>
</ClaimType>
```

Add Rest API configuration as technical profile but in this demo it's anonymous and **without required authentication settings**:

```xml
<TechnicalProfile Id="REST-LocalClaimExtender">
  <DisplayName>Use demo app for adding extra claim</DisplayName>
  <Protocol Name="Proprietary" Handler="Web.TPEngine.Providers.RestfulProvider, Web.TPEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" />
  <Metadata>
    <Item Key="ServiceUrl">https://blbpcnjx-5001.euw.devtunnels.ms/api/ClaimExtender</Item>
    <Item Key="AuthenticationType">None</Item>
    <Item Key="AllowInsecureAuthInProduction">true</Item>
    <Item Key="SendClaimsIn">Body</Item>
    <Item Key="DefaultUserMessageIfRequestFailed">Cannot process your super important validation request right now, please try again later.</Item>
  </Metadata>
  <InputClaims>
    <InputClaim ClaimTypeReferenceId="userPrincipalName" />
    <InputClaim ClaimTypeReferenceId="objectId" />
    <InputClaim ClaimTypeReferenceId="givenName" />
    <InputClaim ClaimTypeReferenceId="surName" />
  </InputClaims>
  <OutputClaims>
    <OutputClaim ClaimTypeReferenceId="customColors" />
  </OutputClaims>
  <UseTechnicalProfileForSessionManagement ReferenceId="SM-Noop" />
</TechnicalProfile>
```

Note: Above `ServiceUrl` is using VS Dev Tunnel for demonstration purposes.

Add orchestration step to your user journey to call this Rest API in the sign-in:

```xml
<OrchestrationStep Order="4" Type="ClaimsExchange">
  <ClaimsExchanges>
    <ClaimsExchange Id="RESTLocalClaimExtender" TechnicalProfileReferenceId="REST-LocalClaimExtender" />
  </ClaimsExchanges>
</OrchestrationStep>
```

Add output claim to your relaying party to return it to the application:

```xml
<OutputClaim ClaimTypeReferenceId="customColors" />
```

Above input claim definition in the Rest API application means that your app will get following payload:

```json
{
  "userPrincipalName": "5e5a77ac-6a6a-4ed7-8df2-e2792f1da401@<yourtenantname>.onmicrosoft.com",
  "objectId": "5e5a77ac-6a6a-4ed7-8df2-e2792f1da401",
  "givenName": "John",
  "surName": "Smith"
}
```

Let's implement Rest API which returns following static response:

```json
{
 "customColors": [ "red", "green", "blue" ]
}
```

You can now test this in your Azure AD B2C:

![Test Custom Policy in Azure AD B2C](https://github.com/JanneMattila/azure-ad-b2c-demos/assets/2357647/a0e5b040-e788-4bff-9049-1b1bb6f846ae)

Sign in using local account:

![Sign in to Azure AD B2C using local account](https://github.com/JanneMattila/azure-ad-b2c-demos/assets/2357647/6d7520d2-5b66-4b45-adba-1994be3ba4dc)

Breakpoint in Visual Studio shows the incoming claims:

![VS in Debug mode showing the incoming claims](https://github.com/JanneMattila/azure-ad-b2c-demos/assets/2357647/d46a9040-f167-4961-84cc-2609a3d28300)

Target application has received the token with the new `customColors` claim:

![jwt.ms showing token content](https://github.com/JanneMattila/azure-ad-b2c-demos/assets/2357647/68621ec0-5783-4c19-8530-e1c515800f83)

## Error scenarios

[Handling error messages](https://learn.microsoft.com/en-us/azure/active-directory-b2c/api-connectors-overview?pivots=b2c-custom-policy#handling-error-messages)

From that document:

> ... calling a REST API technical profile from a **validation technical profile**. 
> **Letting the user to correct the data on the page and run the validation again** upon page submission.

> If you reference a REST API technical profile directly from a **user journey**, 
> the user is **redirected back to the relying party application with the relevant error message**.

`AADB2C90075: Cannot process your super important validation request right now, please try again later.`

![Service down](https://github.com/JanneMattila/azure-ad-b2c-demos/assets/2357647/63f5d9f3-6022-4d6b-bec5-38ae1af78ad3)

In more generic connection error case you might see following error message:

![Connection error](https://github.com/JanneMattila/azure-ad-b2c-demos/assets/2357647/03138866-22d6-4543-af24-abee7cee9e20)
