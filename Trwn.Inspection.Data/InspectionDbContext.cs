using Microsoft.EntityFrameworkCore;
using Trwn.Inspection.Models;

namespace Trwn.Inspection.Data;

public class InspectionDbContext : DbContext
{
    public InspectionDbContext(DbContextOptions<InspectionDbContext> options)
        : base(options)
    {
    }

    public DbSet<InspectionReport> InspectionReports => Set<InspectionReport>();
    public DbSet<InspectionOrderArticle> InspectionOrderArticles => Set<InspectionOrderArticle>();
    public DbSet<PhotoDocumentation> PhotoDocumentations => Set<PhotoDocumentation>();
    public DbSet<AuthSession> AuthSessions => Set<AuthSession>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<InspectionReport>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.ReportNo).IsUnique();
            entity.Property(e => e.Name).IsRequired();
            entity.Property(e => e.ReportNo).IsRequired();
            entity.Property(e => e.Client).IsRequired();
            entity.Property(e => e.ContractNo).IsRequired();
            entity.Property(e => e.ArticleName).IsRequired();
            entity.Property(e => e.Supplier).IsRequired();
            entity.Property(e => e.Factory).IsRequired();
            entity.Property(e => e.InspectionPlace).IsRequired();
            entity.Property(e => e.QualityMark).IsRequired();
            entity.Property(e => e.InspectionStandard).IsRequired();
            entity.Property(e => e.InspectionSampling).IsRequired();
            entity.Property(e => e.InspectionCartonNo).IsRequired();
            entity.Property(e => e.InspectorName).IsRequired();
            entity.Property(e => e.FactoryRepresentative).IsRequired();

            entity.HasMany(e => e.InspectionOrder)
                .WithOne()
                .HasForeignKey("InspectionReportId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.PhotoDocumentation)
                .WithOne()
                .HasForeignKey("InspectionReportId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.AuthSession)
                .WithMany()
                .HasForeignKey(e => e.AuthSessionId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<InspectionOrderArticle>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ArticleNumber).IsRequired();
        });

        modelBuilder.Entity<PhotoDocumentation>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Description).IsRequired();
            entity.Property(e => e.PicturePath).IsRequired();
        });

        modelBuilder.Entity<AuthSession>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Email).IsRequired();
            entity.Property(e => e.Code).IsRequired();
            entity.HasIndex(e => e.Code).IsUnique();
            entity.Property(e => e.AuthToken).HasMaxLength(4096);
        });
    }
}
