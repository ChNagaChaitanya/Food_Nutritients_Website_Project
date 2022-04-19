using Microsoft.EntityFrameworkCore;
using DataGov_API_Intro_6.Models;
namespace DataGov_API_Intro_6.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :
            base(options)
        { }
        public DbSet<FoodRoot> FoodRoot { get; set; }
        public DbSet<Food_Item> Food_Items { get; set; }

        public DbSet<Food_Nutrient> Food_Nutrients { get; set; }
        public DbSet<Nutrient> Nutrients { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Food_Nutrient>().HasKey(fn => new { fn.fdcId, fn.number });
            modelBuilder.Entity<Food_Nutrient>().HasOne(fn => fn.food_item).WithMany(fn => fn.foodNutrients).HasForeignKey(f => f.fdcId);
            modelBuilder.Entity<Food_Nutrient>().HasOne(fn => fn.nutrient).WithMany(fn => fn.foodNutrients).HasForeignKey(n => n.number);
            base.OnModelCreating(modelBuilder);
            
            //base.OnModelCreating(modelBuilder);
        }


    }
}
