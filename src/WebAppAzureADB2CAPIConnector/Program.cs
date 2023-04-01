using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Runtime.CompilerServices;
using WebAppAzureADB2CAPIConnector.Data;
using WebAppAzureADB2CAPIConnector.Models;

[assembly: InternalsVisibleTo("WebAppAzureADB2CAPIConnectorTests")]

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<InvitationRepository>();
builder.Services.AddControllers();
builder.Services.AddOptions<ValidationOptions>()
    .Configure<IConfiguration>((settings, configuration) =>
    {
        configuration.GetSection("ValidationOptions").Bind(settings);
    });
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Azure AD B2C API Connector demo API",
        Description = "Playaround with Azure AD B2C API Connector demo with these APIs",
        TermsOfService = new Uri("https://api.contoso.com/terms"),
        License = new OpenApiLicense
        {
            Name = "Use under MIT",
            Url = new Uri("https://opensource.org/licenses/MIT"),
        }
    });

    // Set the comments path for the Swagger JSON and UI.
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

var app = builder.Build();
if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();

app.UseSwagger(c =>
{
    c.SerializeAsV2 = true;
});

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Azure AD B2C API Connector demo API");
});

app.MapControllers();

await app.RunAsync();
