using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace MyVault.WebApi.Scalar.DocumentTransformers;

public class BearerSecuritySchemeTransformer(IAuthenticationSchemeProvider authenticationSchemeProvider) : IOpenApiDocumentTransformer
{
    private readonly IAuthenticationSchemeProvider _authenticationSchemeProvider = authenticationSchemeProvider;
    private const string AuthorizationTagName = "Authorization";


    public async Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {
        var authenticationSchemes = await _authenticationSchemeProvider.GetAllSchemesAsync();

        if (authenticationSchemes.Any(scheme => scheme.HandlerType.IsEquivalentTo(typeof(JwtBearerHandler))))
        {
            var requirements = new Dictionary<string, OpenApiSecurityScheme>
            {
                ["Bearer"] = new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    In = ParameterLocation.Header,
                    BearerFormat = "JWT"
                }
            };

            document.Components ??= new OpenApiComponents();
            document.Components.SecuritySchemes = requirements;

            foreach (var operation in document.Paths.Values.SelectMany(x => x.Operations).Where(x => x.Value.Tags.Any(y => y.Name == AuthorizationTagName)))
            {
                operation.Value.Security.Add(new OpenApiSecurityRequirement
                {
                    [new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Id = "Bearer",
                            Type = ReferenceType.SecurityScheme
                        }
                    }] = Array.Empty<string>()
                });

                operation.Value.Tags.Remove(operation.Value.Tags.FirstOrDefault(x => x.Name == AuthorizationTagName));
            }

            if (document.Tags.Any(x => x.Name == AuthorizationTagName))
            {
                document.Tags.Remove(document.Tags.FirstOrDefault(x => x.Name == AuthorizationTagName));
            }
        }
    }
}
