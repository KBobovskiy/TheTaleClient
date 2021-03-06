using DataBaseContext.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace DataBaseContext
{
    public class SqliteContext : DbContext
    {
        public SqliteContext(DbContextOptions<SqliteContext> options) : base(options)
        {
        }

        public DbSet<Turn> Turns { get; set; }

        public DbSet<HeroInfoHistory> HeroInfoHistory { get; set; }

        public DbSet<LogEvent> LogEvents { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }
    }
}