using Microsoft.EntityFrameworkCore;
using Omini.Opme.Be.Domain.Entities;

namespace Omini.Opme.Be.Application;

public interface IOpmeContext
{
    DbSet<IdentityOpmeUser> IdentityOpmeUsers { get; set; }
    DbSet<Item> Items { get; set; }
}