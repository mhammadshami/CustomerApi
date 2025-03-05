﻿using Microsoft.EntityFrameworkCore;
using Rest_Api.Models;

namespace Rest_Api.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Customer> Customers { get; set; }
    }
}
