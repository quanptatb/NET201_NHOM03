using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBanThucAnNhanh.Models;
using Microsoft.EntityFrameworkCore;


namespace WebBanThucAnNhanh.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<TypeOfFastFood> TypeOfFastFoods { get; set; }
        public DbSet<FastFood> FastFoods { get; set; }
    }
}