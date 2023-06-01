﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using Microsoft.IdentityModel.Tokens;

namespace DevBlog.Api.IntTests.Framework;

public class BearerTokenBuilder
{
    private X509Certificate2 _signingCertificate;
    private string _issuer = "https://issuer.com";
    private string _audience = "https://subject.com";
    private TimeSpan _life = TimeSpan.FromHours(1);
    private DateTime _notBefore = DateTime.UtcNow;
    private readonly List<Claim> _claims = new();
    private readonly JwtSecurityTokenHandler _securityTokenHandler = new();

    public BearerTokenBuilder DefaultToken()
    {
        ForAudience(TestAuthorisationConstants.Audience);
        IssuedBy(TestAuthorisationConstants.Issuer);
        WithSigningCertificate(EmbeddedResourceReader.GetCertificate());
        return this;
    }

    public BearerTokenBuilder IssuedBy(string issuer)
    {
        if (string.IsNullOrEmpty(issuer))
        {
            throw new ArgumentException("Issued by cannot be null or empty", nameof(issuer));
        }

        _issuer = issuer;

        return this;
    }

    public BearerTokenBuilder ForAudience(string audience)
    {
        if (string.IsNullOrEmpty(audience))
        {
            throw new ArgumentException("Audience cannot be null or empty", nameof(audience));
        }

        _audience = audience;

        return this;
    }

    public BearerTokenBuilder ForSubject(string subject)
    {
        if (string.IsNullOrEmpty(subject))
        {
            throw new ArgumentException("Subject cannot be null or empty", nameof(subject));
        }

        if (_claims.FirstOrDefault(claim => claim.Type == "sub") == null)
        {
            _claims.Add(new Claim("sub", subject));
        }

        return this;
    }

    public BearerTokenBuilder WithSigningCertificate(X509Certificate2 certificate)
    {
        _signingCertificate = certificate ?? throw new ArgumentException("Certificate cannot be null or empty", nameof(certificate));

        return this;
    }

    public BearerTokenBuilder WithClaim(string claimType, string value)
    {
        if (string.IsNullOrEmpty(claimType))
        {
            throw new ArgumentException("Claim type cannot be null or empty", nameof(claimType));
        }

        if (value == null)
        {
            value = string.Empty;
        }

        _claims.Add(new Claim(claimType, value));

        return this;
    }


    public string BuildToken()
    {
        if (_signingCertificate == null)
        {
            throw new InvalidOperationException(
                "You must specify an X509 certificate to use for signing the JWT Token");
        }

        var signingCredentials = new SigningCredentials(new X509SecurityKey(_signingCertificate), SecurityAlgorithms.RsaSha256);

        var notBefore = _notBefore;
        var expires = notBefore.Add(_life);

        var identity = new ClaimsIdentity(_claims);

        var securityTokenDescriptor = new SecurityTokenDescriptor
        {
            Audience = _audience,
            Issuer = _issuer,
            NotBefore = notBefore,
            Expires = expires,
            SigningCredentials = signingCredentials,
            Subject = identity,
        };

        var token = _securityTokenHandler.CreateToken(securityTokenDescriptor);

        var encodedAccessToken = _securityTokenHandler.WriteToken(token);

        return encodedAccessToken;
    }
}