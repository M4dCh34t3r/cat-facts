using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Templates;
using Serilog.Templates.Themes;

namespace Shared.Utils;

public static class StartupUtil
{
    private const string LoggerTemplate =
        "[{@t:HH:mm:ss.fff} "
        + "{#if SourceContext is not null}{SourceContext}"
        + "{#if Subcontext is not null}::{Subcontext}{#end} {#end}"
        + "{@l:u3}{#if ErrorTag is not null} {ErrorTag}{#end}"
        + "] {@m}"
        + "{#if @x is not null}{NewLine}{@x}{#end}"
        + "{#if Rest(true) <> {}}{NewLine}{Rest(true)}{#end}"
        + "{NewLine}{NewLine}";

    /// <summary>
    /// Configures and initializes Serilog as the logging provider for the application.
    /// </summary>
    /// <param name="builder">The <see cref="WebApplicationBuilder"/> used to configure the application.</param>
    /// <remarks>
    /// This method sets up a structured logging configuration with different log levels for various Microsoft components.
    /// In development mode, logs are written to the console with a specific template and theme.
    /// In production mode:
    /// <list type="bullet">
    /// <item><description>Logs are written to a file in the path specified by the "Paths:Logs" configuration key.</description></item>
    /// <item><description>Rolling intervals and file size limits are applied to log files.</description></item>
    /// <item><description>Integration with Sentry is configured for error-level events and above.</description></item>
    /// </list>
    /// </remarks>
    public static void AddSerilog(WebApplicationBuilder builder)
    {
        var isProduction = builder.Environment.IsProduction();

        var loggerConfig = new LoggerConfiguration()
            .Enrich.WithProperty("NewLine", Environment.NewLine)
            .Enrich.FromLogContext()
            .MinimumLevel.Is(LogEventLevel.Debug)
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Error)
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Query", LogEventLevel.Debug)
            .WriteTo.Console(new ExpressionTemplate(LoggerTemplate, theme: TemplateTheme.Code));
        ;

        if (isProduction)
        {
            var debugLog = builder.Configuration.GetValue<bool>("Misc:DebugLog");
            var minimumLevel = debugLog ? LogEventLevel.Debug : LogEventLevel.Information;
            var logPath = Path.Combine(builder.Configuration["Paths:Logs"]!, ".txt");

            loggerConfig
                .WriteTo.File(
                    new ExpressionTemplate(LoggerTemplate),
                    logPath,
                    restrictedToMinimumLevel: minimumLevel,
                    rollingInterval: RollingInterval.Day,
                    rollOnFileSizeLimit: true
                )
                .WriteTo.Sentry(
                    "",
                    minimumBreadcrumbLevel: LogEventLevel.Debug,
                    minimumEventLevel: LogEventLevel.Error
                );
        }

        Log.Logger = loggerConfig.CreateLogger();
        builder.Host.UseSerilog();
    }

    /// <summary>
    /// Configures Sentry for the application when running in a production environment.
    /// </summary>
    /// <param name="builder">The <see cref="WebApplicationBuilder"/> used to configure the application.</param>
    /// <remarks>
    /// Sentry is configured with a DSN from the application configuration under "Secrets:SentryDsn".
    /// If the DSN is not specified, Sentry will not be initialized.
    /// This method also sets default tags, environment, and trace sampling rate for Sentry in production mode.
    /// </remarks>
    public static void AddSentry(WebApplicationBuilder builder)
    {
        var logger = Log.ForContext("Subcontext", nameof(AddSentry));
        var config = builder.Configuration;

        if (builder.Environment.IsProduction())
        {
            if (config["Secrets:SentryDsn"] is string dsn)
            {
                logger.Debug("Starting Setry via {dsn}", dsn);
                builder.WebHost.UseSentry(o =>
                {
                    o.DefaultTags["instance_name"] = "Demo";
                    o.Environment = "production";
                    o.TracesSampleRate = 0.25;
                    o.Dsn = dsn;
                });
            }
            else
                logger.Debug("Sentry will not be used since the DSN wasn't specified");
        }
        else
            logger.Debug("Sentry will not be used since the app is in development mode");
    }

    /// <summary>
    /// Runs the provided <paramref name="app"/> while ensuring required settings are present in the configuration.
    /// If any required settings are missing, the application will not start. Logs errors in case of unexpected exceptions or host abortion.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> instance to run.</param>
    /// <param name="settingKeysToCheck">An array of configuration keys that must be present in the app configuration.</param>

    public static void RunApp(WebApplication app, string[] settingKeysToCheck)
    {
        try
        {
            if (HasMissingSetting(app.Configuration, settingKeysToCheck))
                return;

            app.Run();
        }
        catch (Exception ex)
        {
            if (
                ex is HostAbortedException hostAbortedEx
                && hostAbortedEx.StackTrace?.Contains("BuildWebApp") == true
            )
                Log.Warning(
                    "Host aborted during configuration. If started via \"dotnet-ef\", consider ignoring this warning"
                );
            else
                Log.Fatal(ex, "Unexpected error during server startup");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    /// <summary>
    /// Checks if any of the required configuration keys are missing or have invalid values.
    /// </summary>
    /// <param name="configuration">The configuration object to check.</param>
    /// <param name="keysToCheck">An array of configuration keys that must have valid values.</param>
    /// <returns>
    /// True if any of the specified keys are missing or contain whitespace; otherwise, false.
    /// </returns>
    private static bool HasMissingSetting(IConfiguration configuration, string[] keysToCheck)
    {
        foreach (var key in keysToCheck)
            if (string.IsNullOrWhiteSpace(configuration.GetValue<string>(key)))
            {
                Log.Information($"\"{key}\" needs to be specified in the app configuration");
                return true;
            }
        return false;
    }
}
