using Microsoft.EntityFrameworkCore;
using Omini.Opme.Be.Domain.Entities;

namespace Omini.Opme.Be.Application;

public interface IOpmeContext
{
    DbSet<Item> Items { get; set; }
}