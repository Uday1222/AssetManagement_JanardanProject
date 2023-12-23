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

        public DbSet<Asset> Assets { get; set; }

        public DbSet<AssetDetails> AssetDetails { get; set; }
        
        public DbSet<User> Users { get; set; }  

        public DbSet<ItemTypes> ItemTypes { get; set; }

        public DbSet<Vendor> Vendor { get; set; }
    }
}
