using System.Text.Json.Nodes;
using DBZ.Presentation.DTOs;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace DBZ.Presentation.Swagger;

public sealed class SwaggerExampleSchemaFilter : ISchemaFilter
{
    public void Apply(IOpenApiSchema schema, SchemaFilterContext context)
    {
        if (schema is not OpenApiSchema openApiSchema)
        {
            return;
        }

        openApiSchema.Example = context.Type.Name switch
        {
            nameof(CreatePersonagemDTO) => JsonNode.Parse("""
                {
                  "name": "Goku",
                  "power": 80,
                  "race": "Saiyan",
                  "description": "Oi eu sou o Goku"
                }
                """),
            nameof(EditPersonagemDTO) => JsonNode.Parse("""
                {
                  "name": "Goku",
                  "power": 120,
                  "race": "Saiyan",
                  "description": "Muito forte"
                }
                """),
            nameof(TransformPersonagemDTO) => JsonNode.Parse("""
                {
                  "transformation": "Super Saiyan"
                }
                """),
            _ => openApiSchema.Example
        };
    }
}
