using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using CsTest.Model;

namespace CsTest.Context
{
    public class  DbWorkerContext : DbContext
    {
        public DbSet<DbWorkerModel> Model { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=db.sql");
        }
    }
}