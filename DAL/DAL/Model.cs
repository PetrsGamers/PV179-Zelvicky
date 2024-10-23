namespace DAL;

using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Seeds;

public class ApplicationContext(DbContextOptions<ApplicationContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Album> Albums { get; set; }
    public DbSet<Cap> Caps { get; set; }
    public DbSet<Color> Colors { get; set; }
    public DbSet<Bottle> Bottles { get; set; }
    public DbSet<Producer> Producers { get; set; }
    public DbSet<Country> Countries { get; set; }

    private const string ConnectionString = "Host=localhost;Port=5432;Username=postgres;Password=password;Database=postgres";

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql(ConnectionString);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureRelationships(modelBuilder);
        ConfigureEntities(modelBuilder);
        SeedData(modelBuilder);
    }

    private static void ConfigureRelationships(ModelBuilder modelBuilder)
    {
        // User >o---|| Album
        modelBuilder.Entity<Album>()
            .HasOne(a => a.User)
            .WithMany(u => u.Albums)
            .HasForeignKey(a => a.UserId)
            .IsRequired();

        // Album >o---o< Cap
        modelBuilder.Entity<Album>()
            .HasMany(a => a.Caps)
            .WithMany(c => c.Albums);

        // Cap >o---|| Cap (self-referencing)
        modelBuilder.Entity<Cap>()
            .HasOne(c => c.IsEditFor)
            .WithMany(c => c.Edits)
            .HasForeignKey(c => c.IsEditForId);

        // Cap >o---o< Color (text and background colors)
        modelBuilder.Entity<Cap>()
            .HasMany(c => c.TextColors)
            .WithMany(c => c.CapTexts);

        modelBuilder.Entity<Cap>()
            .HasMany(c => c.BgColors)
            .WithMany(c => c.CapBackgrounds);

        // Cap >o---o< Bottle
        modelBuilder.Entity<Cap>()
            .HasMany(c => c.Bottles)
            .WithMany(b => b.Caps);

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

            entity.Property(u => u.Password)
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
                .IsRequired()
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

        var caps = CapSeed.Seed(modelBuilder, colors, albums, bottles);
    }
}
