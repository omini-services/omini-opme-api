using Microsoft.EntityFrameworkCore;
using Omini.Opme.Domain.BusinessPartners;
using Omini.Opme.Domain.Repositories;
using Omini.Opme.Domain.Warehouse;
using Omini.Opme.Infrastructure.Contexts;

namespace Omini.Opme.Infrastructure.Repositories;

internal sealed class DatabaseRepository : IDatabaseRepository
{
    public readonly OpmeContext Db;

    public DatabaseRepository(OpmeContext db)
    {
        Db = db;
    }

    public async Task PrepareDatabase()
    {
        await PrepareTable("items.sql", "hospitals.sql");
    }

    private async Task PrepareTable(params string[] sqlFiles)
    {
        foreach (var sqlFile in sqlFiles)
        {
            string basePath = AppContext.BaseDirectory;
            string filePath = Path.Combine(basePath, "Seeds", sqlFile);

            var sql = File.ReadAllText(filePath);

            await Db.Database.ExecuteSqlRawAsync(sql);
        }
    }
}
