using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Inventario.Ferramentas.Domain.Entities;

namespace Inventario.Ferramentas.Infrastructure.Data.Context
{
    public class InventarioContext : DbContext
    {
        public InventarioContext(DbContextOptions<InventarioContext> options): base(options) { }
        public DbSet<Ferramenta> Ferramentas { get; set; }
        public DbSet<InventarioFerramentas> InventarioFerramentas { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Ferramenta>(e =>
            {
                e.HasKey(f => f.Id);
            });
            builder.Entity<InventarioFerramentas>(e =>
            {
                e.HasKey(f => f.IdFerramenta);
            });
        }

}
}
