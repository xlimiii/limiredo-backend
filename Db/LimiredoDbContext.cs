using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace limiredo_backend.Db
{
    public class LimiredoDbContext : DbContext
    {
        public DbSet<Sound> Sounds { get; set; }
        public LimiredoDbContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //TODO
            }
        }

    }
}
