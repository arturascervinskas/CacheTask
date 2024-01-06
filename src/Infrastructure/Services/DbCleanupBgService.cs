using Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services;
public class DbCleanupBgService : BackgroundService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<DbCleanupBgService> _logger;
    private readonly IServiceScopeFactory _scopeFactory;

    public DbCleanupBgService(IConfiguration configuration, ILogger<DbCleanupBgService> logger, IServiceScopeFactory scopeFactory)
    {
        _configuration = configuration;
        _logger = logger;
        _scopeFactory = scopeFactory;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var itemRepository = scope.ServiceProvider.GetRequiredService<IItemRepository>();

                    DateTime date = DateTime.UtcNow;

                    int rowsDeleted = await itemRepository.DeleteExpiredItems(date);

                    _logger.LogInformation("Database cleanup completed successfully. {RowsDeleted} rows deleted.", rowsDeleted);
                }

                var dbCleanupInterval = _configuration.GetValue<int>("DbCleanupInterval");

                await Task.Delay(TimeSpan.FromSeconds(dbCleanupInterval), stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during database cleanup.");
            }
        }
    }
}