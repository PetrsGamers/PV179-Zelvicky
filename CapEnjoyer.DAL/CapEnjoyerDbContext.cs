namespace CapEnjoyer.DAL;

using Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Seeds;

public class CapEnjoyerDbContext(DbContextOptions<CapEnjoyerDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Album> Albums { get; set; }
    public DbSet<Cap> Caps { get; set; }
    public DbSet<Color> Colors { get; set; }
    public DbSet<Bottle> Bottles { get; set; }
    public DbSet<Producer> Producers { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<CapToAlbum> CapToAlbums { get; set; }
    public DbSet<CapToBottle> CapToBottles { get; set; }
    public DbSet<CapToBackgroundColor> CapToBackgroundColors { get; set; }
    public DbSet<CapToTextColor> CapToTextColors { get; set; }

    public DbSet<LocalIdentityUser> LocalIdentityUsers { get; set; }

    public DbSet<AuditLog> AuditLogs { get; set; }




    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureRelationships(modelBuilder);
        ConfigureEntities(modelBuilder);
        SeedData(modelBuilder);
        modelBuilder.Entity<IdentityUserClaim<string>>().HasKey(p => new { p.Id });
        modelBuilder.Entity<IdentityUserRole<string>>().HasKey(p => new { p.UserId, p.RoleId });
        modelBuilder.Entity<IdentityUserLogin<string>>().HasKey(p => new { p.UserId });
        modelBuilder.Entity<IdentityUserToken<string>>().HasKey(p => new { p.UserId });
        modelBuilder.Entity<IdentityRoleClaim<string>>().HasKey(p => new { p.Id });
        modelBuilder.Entity<IdentityRole>().HasKey(p => new { p.Id });

    }

    private static void ConfigureRelationships(ModelBuilder modelBuilder)
    {
        // User >o---|| Album
        modelBuilder.Entity<Album>()
            .HasOne(a => a.User)
            .WithMany(u => u.Albums)
            .HasForeignKey(a => a.UserId)
            .IsRequired();

        // Cap >o---|| Cap (self-referencing)
        modelBuilder.Entity<Cap>()
            .HasOne(c => c.IsEditFor)
            .WithMany(c => c.Edits)
            .HasForeignKey(c => c.IsEditForId);

        modelBuilder.Entity<Producer>()
            .HasOne(p => p.Country)
            .WithMany()
            .HasForeignKey(p => p.CountryId);

        // Bottle >o---|| Bottle
        modelBuilder.Entity<Bottle>()
            .HasOne(c => c.IsEditFor)
            .WithMany(c => c.Edits)
            .HasForeignKey(c => c.IsEditForId);

        // Producer >o---|| Producer
        modelBuilder.Entity<Bottle>()
            .HasOne(c => c.IsEditFor)
            .WithMany(c => c.Edits)
            .HasForeignKey(c => c.IsEditForId);

        // Producer >o---|| Country
        modelBuilder.Entity<Producer>()
            .HasOne(p => p.Country)
            .WithMany(c => c.Producers)
            .HasForeignKey(p => p.CountryId)
            .IsRequired();

        // CapToTextColor
        modelBuilder.Entity<CapToTextColor>()
            .HasKey(ct => new { ct.CapId, ct.TextColorId });

        modelBuilder.Entity<CapToTextColor>()
            .HasOne(ct => ct.Cap)
            .WithMany(c => c.TextColorLinks)
            .HasForeignKey(ct => ct.CapId);

        modelBuilder.Entity<CapToTextColor>()
            .HasOne(ct => ct.TextColor)
            .WithMany()
            .HasForeignKey(ct => ct.TextColorId);

        // CapToBackgroundColor
        modelBuilder.Entity<CapToBackgroundColor>()
            .HasKey(cb => new { cb.CapId, cb.BackgroundColorId });

        modelBuilder.Entity<CapToBackgroundColor>()
            .HasOne(cb => cb.Cap)
            .WithMany(c => c.BackgroundColorLinks)
            .HasForeignKey(cb => cb.CapId);

        modelBuilder.Entity<CapToBackgroundColor>()
            .HasOne(cb => cb.BackgroundColor)
            .WithMany()
            .HasForeignKey(cb => cb.BackgroundColorId);

        // CapToAlbum
        modelBuilder.Entity<CapToAlbum>()
            .HasKey(ca => new { ca.CapId, ca.AlbumId });

        modelBuilder.Entity<CapToAlbum>()
            .HasOne(ca => ca.Cap)
            .WithMany(c => c.AlbumLinks)
            .HasForeignKey(ca => ca.CapId);

        modelBuilder.Entity<CapToAlbum>()
            .HasOne(ca => ca.Album)
            .WithMany(a => a.CapLinks)
            .HasForeignKey(ca => ca.AlbumId);

        // CapToBottle
        modelBuilder.Entity<CapToBottle>()
            .HasKey(cb => new { cb.CapId, cb.BottleId });

        modelBuilder.Entity<CapToBottle>()
            .HasOne(cb => cb.Cap)
            .WithMany(c => c.BottleLinks)
            .HasForeignKey(cb => cb.CapId);

        modelBuilder.Entity<CapToBottle>()
            .HasOne(cb => cb.Bottle)
            .WithMany(b => b.CapLinks)
            .HasForeignKey(cb => cb.BottleId);
    }

    private static void ConfigureEntities(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(u => u.Username)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(u => u.Role)
                .IsRequired()
                .HasMaxLength(255);
        });

        modelBuilder.Entity<Album>(entity =>
        {
            entity.Property(a => a.Name)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(a => a.Description)
                .HasMaxLength(2047);

            entity.Property(a => a.Public)
                .IsRequired();
        });

        modelBuilder.Entity<Cap>(entity =>
        {
            entity.Property(c => c.TextOnCap)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(c => c.Description)
                .HasMaxLength(2047);

            entity.Property(c => c.CapPicture)
                .HasMaxLength(255);
        });

        modelBuilder.Entity<Color>(entity =>
        {
            entity.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(c => c.HexCode)
                .IsRequired()
                .HasMaxLength(255);
        });

        modelBuilder.Entity<Bottle>(entity =>
        {
            entity.Property(b => b.Name)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(b => b.Description)
                .HasMaxLength(2047);

            entity.Property(b => b.Voltage)
                .IsRequired();

            entity.Property(b => b.DrinkType)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(b => b.BottlePicture)
                .HasMaxLength(255);
        });

        modelBuilder.Entity<Producer>(entity =>
        {
            entity.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(p => p.City)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(p => p.Description)
                .HasMaxLength(2047);
        });

        modelBuilder.Entity<Country>(entity => entity.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(255));

        // enums to string conversion
        modelBuilder.Entity<User>()
            .Property(u => u.Role)
            .HasConversion<string>();

        modelBuilder.Entity<Bottle>()
            .Property(b => b.DrinkType)
            .HasConversion<string>();
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        var countries = CountrySeed.Seed(modelBuilder);
        var colors = ColorSeed.Seed(modelBuilder);
        var users = UserSeed.Seed(modelBuilder);
        var albums = AlbumSeed.Seed(modelBuilder, users);
        var producers = ProducerSeed.Seed(modelBuilder, countries);
        var bottles = BottleSeed.Seed(modelBuilder, producers);

        var caps = CapSeed.Seed(modelBuilder);
        CapToTextColorSeed.Seed(modelBuilder, caps, colors);
        CapToBackgroundColorSeed.Seed(modelBuilder, caps, colors);
        CapToAlbumSeed.Seed(modelBuilder, caps, albums);
        CapToBottleSeed.Seed(modelBuilder, caps, bottles);
    }
}
