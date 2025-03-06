using Microsoft.EntityFrameworkCore;
using ProjectBase.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBase.Data
{
    public class ProjectDbContext(DbContextOptions options) : DbContext(options)
    {
        public virtual DbSet<WeatherItem> WeatherItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WeatherItem>(entity =>
            {
                entity.HasKey(x => x.Id);
            });
        }
    }
}
