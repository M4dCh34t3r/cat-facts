using Core.Domain.Entities;
using Hangfire;
using Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Shared.Utils;

namespace BackgroundJobs;

public class UpsertJob(
    HttpClient httpClient,
    IConfiguration configuration,
    IUpsertRepository repository
) : IUpsertJob
{
    private readonly Serilog.ILogger _logger = Serilog.Log.ForContext<UpsertJob>();

    private readonly HttpClient _httpClient = httpClient;
    private readonly IConfiguration _configuration = configuration;
    private readonly IUpsertRepository _repository = repository;

    public async Task ExecuteAsync()
    {
        try
        {
            /// GUARANTEE: The configuration keys are validated via 'StartupUtil.HasMissingConfiguration' in 'Program.cs'
            string requestUri = _configuration.GetValue<string>("Misc:PublicAPIRequestUri")!;

            var response = await _httpClient.GetAsync(requestUri);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                if (JsonUtil.Deserialize<PublicFact>(json) is PublicFact publicFact)
                    BackgroundJob.Enqueue(() => _repository.UpsertAsync(publicFact, requestUri));
            }
            else
                _logger.Warning("Failed to fetch data from the API.");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An error occurred while fetching from the API.");
        }
    }
}
