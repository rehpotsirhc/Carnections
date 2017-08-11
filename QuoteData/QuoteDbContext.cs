using Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuoteData
{
    public class QuoteDbContext : DbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<IQuote>().ToTable("QuoteHistory");
            modelBuilder.Entity<IQuote>().HasKey(d => d.Id);
            modelBuilder.Entity<IQuote>().Property(d => d.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<IQuote>().Property(d => d.CreatedDate).ValueGeneratedOnAdd();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }
    }
}
