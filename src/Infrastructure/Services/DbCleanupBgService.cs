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
    private readonly int _dbCleanupInterval;

    public DbCleanupBgService(IConfiguration configuration, ILogger<DbCleanupBgService> logger, IServiceScopeFactory scopeFactory)
    {
        _configuration = configuration;
        _logger = logger;
        _scopeFactory = scopeFactory;
        _dbCleanupInterval = _configuration.GetValue<int>("DbCleanupInterval");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    IItemRepository itemRepository = scope.ServiceProvider.GetRequiredService<IItemRepository>();

                    DateTime date = DateTime.UtcNow;

                    int rowsDeleted = await itemRepository.DeleteExpiredItems(date);

                    _logger.LogInformation("Database cleanup completed successfully. {rowsDeleted} rows deleted.", rowsDeleted);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during database cleanup.");
            }

            await Task.Delay(TimeSpan.FromSeconds(_dbCleanupInterval), stoppingToken);
        }
    }
}