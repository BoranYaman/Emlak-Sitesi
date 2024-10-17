using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using RealEstate.Entity;

namespace RealEstate.Models
{
    public class RealEstateContext:IdentityDbContext<AppUser,AppRole,string>
    {
        public RealEstateContext(DbContextOptions<RealEstateContext>options):base(options)
        {

        }
        public DbSet<House> Houses=> Set <House> ();
        public DbSet<House> HouseImages=> Set <House> ();
        public DbSet<Plot> Plots=> Set <Plot> ();
        public DbSet<Plot> PlotImages=> Set <Plot> ();

        



    } 
}