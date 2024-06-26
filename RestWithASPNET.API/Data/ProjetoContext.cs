﻿using Microsoft.EntityFrameworkCore;
using RestWithASPNET.API.models;

namespace RestWithASPNET.API.Data
{
    public class ProjetoContext : DbContext
    {
        public ProjetoContext(DbContextOptions<ProjetoContext> options) : base(options)
        {
            
        }

        public DbSet<Person> Persons { get; set; }
        public DbSet<Boss> Bosses { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProjetoContext).Assembly);
        }

    }
}
