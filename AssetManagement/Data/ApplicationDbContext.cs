﻿using AssetManagement.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace AssetManagement.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
        public DbSet<Employee> Employees { get; set; }

        public DbSet<Asset> Assets { get; set; }

        public DbSet<AssetDetails> AssetDetails { get; set; }
    }
}
