namespace BackgroundJobs;

public interface IUpsertJob
{
    /// <summary>
    /// Executes the upsert job, fetching data from an external API and persisting it to the repository.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous execution of the upsert job.
    /// </returns>
    Task ExecuteAsync();
}
