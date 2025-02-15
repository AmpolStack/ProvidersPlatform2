using Microsoft.EntityFrameworkCore;
using ProvidersPlatform.Shared.Models;

namespace ProvidersPlatform.Shared.Setup.Contexts;

public partial class ProvidersPlatformContext : DbContext
{
    public ProvidersPlatformContext()
    {
    }

    public ProvidersPlatformContext(DbContextOptions<ProvidersPlatformContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Active> Actives { get; set; }

    public virtual DbSet<Activity> Activities { get; set; }

    public virtual DbSet<Commentary> Commentaries { get; set; }

    public virtual DbSet<Contact> Contacts { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<Provider> Providers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;database=Providers_Platform;uid=root;pwd=2024", Microsoft.EntityFrameworkCore.ServerVersion.Parse("11.5.2-mariadb"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_uca1400_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Active>(entity =>
        {
            entity.HasKey(e => e.ActiveId).HasName("PRIMARY");

            entity.ToTable("active");

            entity.Property(e => e.ActiveId)
                .HasColumnType("int(11)")
                .HasColumnName("active_id");
            entity.Property(e => e.ActiveName)
                .HasMaxLength(150)
                .HasColumnName("active_name");
            entity.Property(e => e.ActiveType)
                .HasMaxLength(50)
                .HasColumnName("active_type");
            entity.Property(e => e.ProductCode)
                .HasColumnType("int(11)")
                .HasColumnName("product_code");
            entity.Property(e => e.ShippingMethod)
                .HasMaxLength(100)
                .HasColumnName("shipping_method");
        });

        modelBuilder.Entity<Activity>(entity =>
        {
            entity.HasKey(e => e.ActivityId).HasName("PRIMARY");

            entity.ToTable("activity");

            entity.Property(e => e.ActivityId)
                .HasColumnType("int(11)")
                .HasColumnName("activity_id");
            entity.Property(e => e.ActivityName)
                .HasMaxLength(150)
                .HasColumnName("activity_name");
            entity.Property(e => e.ActivityType)
                .HasMaxLength(50)
                .HasColumnName("activity_type");
            entity.Property(e => e.CiiuCode)
                .HasColumnType("int(11)")
                .HasColumnName("CIIU_code");
        });

        modelBuilder.Entity<Commentary>(entity =>
        {
            entity.HasKey(e => new { e.PostId, e.UserId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("commentary");

            entity.HasIndex(e => e.UserId, "user_id");

            entity.Property(e => e.PostId)
                .HasColumnType("int(11)")
                .HasColumnName("post_id");
            entity.Property(e => e.UserId)
                .HasColumnType("int(11)")
                .HasColumnName("user_id");
            entity.Property(e => e.Assessment)
                .HasColumnType("int(11)")
                .HasColumnName("assessment");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(200)
                .HasColumnName("image_Url");
            entity.Property(e => e.Text)
                .HasMaxLength(300)
                .HasColumnName("text");

            entity.HasOne(d => d.Post).WithMany(p => p.Commentaries)
                .HasForeignKey(d => d.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("commentary_ibfk_1");

            entity.HasOne(d => d.User).WithMany(p => p.Commentaries)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("commentary_ibfk_2");
        });

        modelBuilder.Entity<Contact>(entity =>
        {
            entity.HasKey(e => e.ContactId).HasName("PRIMARY");

            entity.ToTable("contact");

            entity.HasIndex(e => e.ProviderId, "provider_id");

            entity.Property(e => e.ContactId)
                .HasColumnType("int(11)")
                .HasColumnName("contact_id");
            entity.Property(e => e.ContactDescription)
                .HasMaxLength(100)
                .HasColumnName("contact_description");
            entity.Property(e => e.ContactType)
                .HasMaxLength(50)
                .HasColumnName("contact_type");
            entity.Property(e => e.ProviderId)
                .HasColumnType("int(11)")
                .HasColumnName("provider_id");
            entity.Property(e => e.Value)
                .HasMaxLength(100)
                .HasColumnName("value");

            entity.HasOne(d => d.Provider).WithMany(p => p.Contacts)
                .HasForeignKey(d => d.ProviderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("contact_ibfk_1");
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.PostId).HasName("PRIMARY");

            entity.ToTable("post");

            entity.HasIndex(e => e.ProviderId, "provider_id");

            entity.Property(e => e.PostId)
                .HasColumnType("int(11)")
                .HasColumnName("post_id");
            entity.Property(e => e.Description)
                .HasMaxLength(300)
                .HasColumnName("description");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(200)
                .HasColumnName("image_Url");
            entity.Property(e => e.ProviderId)
                .HasColumnType("int(11)")
                .HasColumnName("provider_id");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .HasColumnName("title");

            entity.HasOne(d => d.Provider).WithMany(p => p.Posts)
                .HasForeignKey(d => d.ProviderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("post_ibfk_1");
        });

        modelBuilder.Entity<Provider>(entity =>
{
    entity.HasKey(e => e.ProviderId).HasName("PRIMARY");

    entity.ToTable("provider");

    entity.HasIndex(e => e.UserId, "user_id");

    entity.Property(e => e.ProviderId)
        .HasColumnType("int(11)")
        .HasColumnName("provider_id");
    entity.Property(e => e.AssociationPrefix)
        .HasMaxLength(20)
        .HasColumnName("Association_prefix");
    entity.Property(e => e.EntityName)
        .HasMaxLength(100)
        .HasColumnName("entity_name");
    entity.Property(e => e.Nit)
        .HasColumnType("int(11)")
        .HasColumnName("NIT");
    entity.Property(e => e.UserId)
        .HasColumnType("int(11)")
        .HasColumnName("user_id");

    // Configuración de la relación 1:1 con User:
    entity.HasOne(d => d.User)
          .WithOne(p => p.Provider)
          .HasForeignKey<Provider>(d => d.UserId)
          .OnDelete(DeleteBehavior.ClientSetNull)
          .HasConstraintName("provider_ibfk_1");

    entity.HasMany(d => d.Actives).WithMany(p => p.Providers)
          .UsingEntity<Dictionary<string, object>>(
              "ProviderActive",
              r => r.HasOne<Active>().WithMany()
                  .HasForeignKey("ActiveId")
                  .OnDelete(DeleteBehavior.ClientSetNull)
                  .HasConstraintName("provider_active_ibfk_2"),
              l => l.HasOne<Provider>().WithMany()
                  .HasForeignKey("ProviderId")
                  .OnDelete(DeleteBehavior.ClientSetNull)
                  .HasConstraintName("provider_active_ibfk_1"),
              j =>
              {
                  j.HasKey("ProviderId", "ActiveId")
                      .HasName("PRIMARY")
                      .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                  j.ToTable("provider_active");
                  j.HasIndex(new[] { "ActiveId" }, "active_id");
                  j.IndexerProperty<int>("ProviderId")
                      .HasColumnType("int(11)")
                      .HasColumnName("provider_id");
                  j.IndexerProperty<int>("ActiveId")
                      .HasColumnType("int(11)")
                      .HasColumnName("active_id");
              });
});


        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity.ToTable("user");

            entity.Property(e => e.UserId)
                .HasColumnType("int(11)")
                .HasColumnName("user_id");
            entity.Property(e => e.Address)
                .HasMaxLength(120)
                .HasColumnName("address");
            entity.Property(e => e.Description)
                .HasMaxLength(200)
                .HasColumnName("description");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .HasColumnName("password");
            entity.Property(e => e.UserType)
                .HasMaxLength(20)
                .HasColumnName("user_type");

            // Configuración de la relación 1:1 con Provider
            entity.HasOne(u => u.Provider)
                .WithOne(p => p.User)
                .HasForeignKey<Provider>(p => p.UserId);
        });


        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
