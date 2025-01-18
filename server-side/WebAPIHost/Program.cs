using System.Text.Json.Serialization;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Shared.Utils;
using WebAPIHost.Services;

string[] settingKeysToCheck = ["ConnectionStrings:MSSQL", "Misc:PageSize"];
WebApplication app = BuildWebApp(args);

StartupUtil.RunApp(app, settingKeysToCheck);

static void AddServices(WebApplicationBuilder builder)
{
    builder
        .Services.AddControllers()
        .AddJsonOptions(o =>
        {
            o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("MSSQL"))
    );

    builder.Services.AddEndpointsApiExplorer();

    builder.Services.AddScoped<IFactService, FactService>();
}

static void AddSwagger(WebApplicationBuilder builder) =>
    builder.Services.AddSwaggerGen(o =>
    {
        o.SwaggerDoc("v1", new() { Title = "Assessment API - V1", Version = "v1" });

        string[] xmlFilePaths = [$"WebAPIHost.xml", "Core.Application.xml"];
        foreach (var xmlFilePath in xmlFilePaths)
        {
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilePath);
            if (File.Exists(xmlPath))
                o.IncludeXmlComments(xmlPath);
        }
    });

static WebApplication BuildWebApp(string[] args)
{
    WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

    StartupUtil.AddSerilog(builder);
    StartupUtil.AddSentry(builder);

    AddServices(builder);

    if (builder.Environment.IsDevelopment())
        AddSwagger(builder);

    WebApplication app = builder.Build();
    if (app.Environment.IsDevelopment())
    {
        app.UseSwaggerUI();
        app.UseSwagger();
    }
    else
    {
        app.UseDefaultFiles();
        app.UseStaticFiles();
    }
    app.MapControllers();
    return app;
}
