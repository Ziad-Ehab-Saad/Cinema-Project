using Cinema.Identity;
using Cinema.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Cinema.DataAccess
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Movie> movies { get; set; }
        public DbSet<Actor> actors { get; set; }
        public DbSet<ECinema> cinemas { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<ActorMovie> actorMovies { get; set; }
        public DbSet<Cart> carts { get; set; }
        public DbSet<CartItems> cartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItems> OrderItems { get; set; }
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ActorMovie>()
                .HasKey(am => new { am.ActorId, am.MovieId });

            modelBuilder.Entity<ActorMovie>()
                .HasOne(am => am.Actor)
                .WithMany(a => a.ActorMovies)
                .HasForeignKey(am => am.ActorId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ActorMovie>()
                .HasOne(am => am.Movie)
                .WithMany(m => m.ActorMovies)
                .HasForeignKey(am => am.MovieId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Cart>(
                entity =>
                {
                    entity.HasKey(e => e.Id);
                    entity.Property(e => e.CreatedAt).HasDefaultValueSql("getdate()");

                });
            modelBuilder.Entity<CartItems>()
                .HasOne(e => e.movie)
                .WithMany(e => e.CartItems)
                .HasForeignKey(e => e.MovieId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CartItems>()
                .HasOne(e => e.cart)
                .WithMany(e => e.CartItems)
                .HasForeignKey(e => e.CartId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Cart>()
            .HasOne(e => e.user)
            .WithOne(e => e.UserCart)
            .HasForeignKey<Cart>(e => e.UserId);



        }



    }
}
