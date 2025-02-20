using Microsoft.EntityFrameworkCore;

namespace ProvidersPlatform.UserServices.Services.Models;

public partial class JwtDbContext : DbContext
{
    public JwtDbContext()
    {
    }

    public JwtDbContext(DbContextOptions<JwtDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<RefreshTokenHistory> RefreshTokenHistories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder){}
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RefreshTokenHistory>(entity =>
        {
            entity.HasKey(e => e.IdRefreshToken).HasName("PK__RefreshT__25B20AACA082B2C8");

            entity.ToTable("RefreshTokenHistory");

            entity.Property(e => e.IdRefreshToken).HasColumnName("idRefreshToken");
            entity.Property(e => e.CreationDate)
                .HasColumnType("datetime")
                .HasColumnName("creationDate");
            entity.Property(e => e.ExpirationDate)
                .HasColumnType("datetime")
                .HasColumnName("expirationDate");
            entity.Property(e => e.IdUser).HasColumnName("idUser");
            entity.Property(e => e.IsActive)
                .HasComputedColumnSql("(case when [ExpirationDate]<getdate() then CONVERT([bit],(0)) else CONVERT([bit],(1)) end)", false)
                .HasColumnName("isActive");
            entity.Property(e => e.RefreshToken)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("refreshToken");
            entity.Property(e => e.Token)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("token");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
