using DBZ.Application;
using DBZ.Infrastructure;
using DBZ.Presentation.Swagger;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Routing;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.Conventions.Add(new RouteTokenTransformerConvention(new LowercaseRouteTokenTransformer()));
});
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(entry => entry.Value?.Errors.Count > 0)
            .ToDictionary(
                entry => NormalizeModelStateKey(entry.Key),
                entry => entry.Value!.Errors
                    .Select(error => NormalizeModelStateError(entry.Key, error.ErrorMessage))
                    .Distinct()
                    .ToArray());

        errors.Remove("dto");

        return new BadRequestObjectResult(new ValidationProblemDetails(errors)
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "One or more validation errors occurred.",
            Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1"
        });
    };
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.EnableAnnotations();
    options.SchemaFilter<SwaggerExampleSchemaFilter>();
});
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

static string NormalizeModelStateKey(string key)
{
    return key.StartsWith("$.")
        ? key[2..]
        : key;
}

static string NormalizeModelStateError(string key, string errorMessage)
{
    if (key.Equals("$.power", StringComparison.OrdinalIgnoreCase)
        && errorMessage.Contains("System.Int32", StringComparison.OrdinalIgnoreCase))
    {
        return "O campo power deve ser um inteiro.";
    }

    return errorMessage;
}

public sealed class LowercaseRouteTokenTransformer : IOutboundParameterTransformer
{
    public string? TransformOutbound(object? value)
    {
        return value?.ToString()?.ToLowerInvariant();
    }
}
