using System;
using API.Entitties;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class AppDbContext : DbContext
{
 public AppDbContext(DbContextOptions options):base (options)
 {
 }
  public DbSet<AppUser> Users { get; set; }
}
