using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Context;

public partial class AppDbContext(DbContextOptions options) : DbContext(options)
{
    public virtual DbSet<Fact> Facts { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Latin1_General_CI_AI");

        modelBuilder.Entity<Fact>(f =>
        {
            f.Property(f => f.Id).HasDefaultValueSql("NEWID()");
            f.Property(f => f.InsertedAt).HasDefaultValueSql("GETUTCDATE()").ValueGeneratedOnAdd();
            f.Property(f => f.OccurrenceCount).HasDefaultValue(1);

            f.HasIndex(f => f.Text).IsUnique();
        });
    }

    /// <summary>
    /// Retrieves a list of migration identifiers based on their application status.
    /// </summary>
    /// <param name="pending">
    /// A boolean flag indicating which migrations to list:
    /// <list type="bullet">
    /// <item><term>true</term>: Lists migrations that have not yet been applied.</item>
    /// <item><term>false</term>: Lists migrations that have already been applied.</item>
    /// </list>
    /// </param>
    /// <returns>
    /// An array of strings containing migration IDs based on the specified <paramref name="pending"/> flag.
    /// </returns>
    /// <example>
    /// Example usage:
    /// <code>
    /// // List all pending migrations
    /// string[] pendingMigrations = ListaMigracoes(true);
    ///
    /// // List all applied migrations
    /// string[] appliedMigrations = ListaMigracoes(false);
    /// </code>
    /// </example>
    /// <remarks>
    /// This method uses extension methods to retrieve the migrations assembly and the migration history repository.
    /// It calculates the list of pending or applied migrations based on the existence and content of the migration history.
    /// </remarks>
    public string[] ListaMigracoes(bool pending)
    {
        var migrationsAssembly = this.GetService<IMigrationsAssembly>();
        var history = this.GetService<IHistoryRepository>();

        string[] applied = history.Exists()
            ? [.. history.GetAppliedMigrations().Select(r => r.MigrationId)]
            : [];

        return [.. pending ? migrationsAssembly.Migrations.Keys.Except(applied) : applied];
    }
}
