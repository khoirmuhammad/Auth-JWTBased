# Authentication .NET Core Using JSON Web Token (JWT)
In addition using cookie to secure our API, currently we have known about JWT. The first step is performing sign in as usual, once the credential has validated, then we will get generate string token. In the subsequent API calls, if we need to consume secure method / resource, we have to add the token in the header request. Here's is simply way to implement JWT in ASP NET Core Web API

1. First of all install the following packages from Nuget Package Manager
- Microsoft.AspNetCore.Authentication.Core
- Microsoft.AspNetCore.Authentication.JwtBearer
2. Add several data in appsettings.json, as per below code
```
"JwtConfig": {
    "Key": "abcdefghijklmnopqrstuvwz1234567890",
    "Issuer": "https://localhost:7035",
    "Audience": "https://localhost:7035"
  }
```
3. Configuring JWT authentication in the program.cs / startup.cs
```
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtConfig:Issuer"],
        ValidAudience = builder.Configuration["JwtConfig:Audience"],
        IssuerSigningKey = new
        SymmetricSecurityKey
        (Encoding.UTF8.GetBytes
        (builder.Configuration["JwtConfig:Key"]))
    };
});
```
The rest we able to see TokenService in respoitory code that will be used to generate a token

### Deal with expiration token
1. In my testing scenario in case we set with false value, then token won't be considered as expired in specific time. So we able to use previous token despite we have generated new token. By default NET Core will set to true
```
ValidateLifetime = true,
```
2. Determine our expiration time in token service generator. It can be in minutes, days etc. NET Core give recomendation to set in Utc time
```
var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = issuer,
                Audience = audience,
                Expires = DateTime.UtcNow.AddMinutes(EXPIRY_DURATION_MINUTES),
                SigningCredentials = credentials    
            };
```

Actually we can get expiration feature by only using those configuration. But in my testing scenario we get 5 minutes in expiration differences. For instance, we log in at 12:00 am, and we set expiration time to 10 minutes after log in. The reality token will be expired after 15 minutes from login, it means there are 5 minutes differences. In order to overcome with the issue we will set with this configuration in program.cs / startup.cs. So we will get expiration appropriate with our setting

```
ClockSkew = TimeSpan.Zero
```

In several arguments, many developer advice, sometimes the clockskew setting can make new issue. In case we are experiencing the issue, we can manipulate the expiration time. If we want to token expires after 1 hour, then we can set expiration to 55 minutes or let the token expires after 1 hour and 5 minutes. According our requirement

Reference : https://stackoverflow.com/questions/47153080/clock-skew-and-tokens
