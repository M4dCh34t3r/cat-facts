using BackgroundJobs;
using Shared.Utils;
using Hangfire;
using Infrastructure.Context;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;

string[] settingKeysToCheck =
[
    "ConnectionStrings:MSSQL",
    "Misc:PublicAPIRequestUri",
];
WebApplication app = BuildWebApp(args);
using (var scope = app.Services.CreateScope())
{
    var recurringJobManager = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();
    recurringJobManager.AddOrUpdate(
        "DefaultUpsertJob",
        () => scope.ServiceProvider.GetRequiredService<IUpsertJob>().ExecuteAsync(),
#if DEBUG
        Cron.Minutely()
#else
        Cron.Hourly()
#endif
    );
}
StartupUtil.RunApp(app, settingKeysToCheck);

static void AddServices(WebApplicationBuilder builder)
{
    builder.Services.AddDbContext<AppDbContext>(o =>
        o.UseSqlServer(builder.Configuration.GetConnectionString("MSSQL"))
    );

    builder.Services.AddHangfire(o =>
        o.UseSqlServerStorage(builder.Configuration.GetConnectionString("MSSQL"))
    );
    builder.Services.AddHangfireServer();

    builder.Services.AddHttpClient();

    builder
        .Services.AddScoped<IUpsertJob, UpsertJob>()
        .AddScoped<IUpsertRepository, UpsertRepository>();
}

static WebApplication BuildWebApp(string[] args)
{
    WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

    StartupUtil.AddSerilog(builder);
    StartupUtil.AddSentry(builder);

    AddServices(builder);

    return builder.Build();
}
