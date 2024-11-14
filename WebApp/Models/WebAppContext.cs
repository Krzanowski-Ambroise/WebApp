using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using WebApp.Areas.Identity.Data;

namespace WebApp.Models
{
    public partial class WebAppContext : DbContext
    {
        public WebAppContext()
        {
        }

        public WebAppContext(DbContextOptions<WebAppContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TCompany> TCompanies { get; set; }
        public virtual DbSet<TExerce> TExerces { get; set; }
        public virtual DbSet<TJobPost> TJobPosts { get; set; }
        public virtual DbSet<TJobPostRequirement> TJobPostRequirements { get; set; }
        public virtual DbSet<TPoste> TPostes { get; set; }
        public virtual DbSet<TRequirementType> TRequirementTypes { get; set; }
        public virtual DbSet<TSite> TSites { get; set; }
        public virtual DbSet<TTypeOfContract> TTypeOfContracts { get; set; }
        public virtual DbSet<TUserRequirement> TUserRequirements { get; set; }
        public virtual DbSet<UserCompanieAssociation> UserCompanieAssociations { get; set; }

        // Ajout d'ApplicationUser pour l'intégration avec T_Users
        public virtual DbSet<ApplicationUser> ApplicationUsers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlServer("Name=WebAppConnection");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuration pour ApplicationUser (T_Users)
            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable("T_Users"); // Spécifie que ApplicationUser correspond à T_Users

                entity.Property(e => e.Latitude).HasColumnType("float").IsRequired(false);
                entity.Property(e => e.Longitude).HasColumnType("float").IsRequired(false);

                entity.HasMany(e => e.TExerces)
                      .WithOne(d => d.User)
                      .HasForeignKey(d => d.UserId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("FK_T_Exerces_T_Users");

                entity.HasMany(e => e.TUserRequirements)
                      .WithOne(d => d.User)
                      .HasForeignKey(d => d.UserId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("FK_T_UserRequirements_Users");

                entity.HasMany(e => e.UserCompanieAssociations)
                      .WithOne(d => d.User)
                      .HasForeignKey(d => d.UserId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("FK_UserCompanieAssociations_Users");
            });

            // Configuration de la table TCompany
            modelBuilder.Entity<TCompany>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_T_Companies");
                entity.ToTable("T_Companies");
                entity.Property(e => e.CompanieName).HasMaxLength(255);
                entity.Property(e => e.Siret).HasMaxLength(255);
                entity.Property(e => e.Website).HasMaxLength(255);
            });

            // Configuration de la table TExerce
            modelBuilder.Entity<TExerce>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_T_Exerces");
                entity.ToTable("T_Exerces");
                entity.Property(e => e.UserId).HasMaxLength(450);

                entity.HasOne(d => d.Poste).WithMany(p => p.TExerces)
                      .HasForeignKey(d => d.PosteId)
                      .HasConstraintName("FK_T_Exerces_PosteId");

                entity.HasOne(d => d.Site).WithMany(p => p.TExerces)
                      .HasForeignKey(d => d.SiteId)
                      .HasConstraintName("FK_T_Exerces_SiteId");

                entity.HasOne(d => d.TypeOfContract).WithMany(p => p.TExerces)
                      .HasForeignKey(d => d.TypeOfContractId)
                      .HasConstraintName("FK_Exerces_TypeOfContracts");

                entity.HasOne(d => d.User)
                      .WithMany(u => u.TExerces)
                      .HasForeignKey(d => d.UserId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("FK_T_Exerces_T_Users");
            });

            // Configuration de la table TJobPost
            modelBuilder.Entity<TJobPost>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_T_JobPosts");
                entity.ToTable("T_JobPosts");
                entity.Property(e => e.Title).HasMaxLength(255);

                entity.HasOne(d => d.Companie).WithMany(p => p.TJobPosts)
                      .HasForeignKey(d => d.CompanieId)
                      .HasConstraintName("FK_T_JobPost_CompanieId");

                entity.HasOne(d => d.Poste).WithMany(p => p.TJobPosts)
                      .HasForeignKey(d => d.PosteId)
                      .HasConstraintName("FK_T_JobPost_PosteId");

                entity.HasOne(d => d.Site).WithMany(p => p.TJobPosts)
                      .HasForeignKey(d => d.SiteId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("FK_T_JobPost_SiteId");

                entity.HasOne(d => d.TypeOfContract).WithMany(p => p.TJobPosts)
                      .HasForeignKey(d => d.TypeOfContractId)
                      .HasConstraintName("FK_JobPosts_TypeOfContracts");

                entity.HasMany(e => e.TJobPostRequirements)
                      .WithOne(r => r.JobPost)
                      .HasForeignKey(r => r.JobPostId)
                      .HasConstraintName("FK_T_JobPostRequirement_JobPostId");
            });

            // Configuration de la table TJobPostRequirement
            modelBuilder.Entity<TJobPostRequirement>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_T_JobPostRequirements");
                entity.ToTable("T_JobPostRequirements");

                entity.HasOne(d => d.JobPost).WithMany(p => p.TJobPostRequirements)
                      .HasForeignKey(d => d.JobPostId)
                      .HasConstraintName("FK_T_JobPostRequirement_JobPostId");

                entity.HasOne(d => d.RequirementType).WithMany(p => p.TJobPostRequirements)
                      .HasForeignKey(d => d.RequirementTypeId)
                      .HasConstraintName("FK_T_JobPostRequirement_RequirementTypeId");
            });

            // Configuration de la table TPoste
            modelBuilder.Entity<TPoste>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_T_Postes");
                entity.ToTable("T_Postes");
                entity.Property(e => e.Title).HasMaxLength(255);
            });

            // Configuration de la table TRequirementType
            modelBuilder.Entity<TRequirementType>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_T_RequirementTypes");
                entity.ToTable("T_RequirementTypes");
                entity.Property(e => e.TypeName).HasMaxLength(100);
            });

            // Configuration de la table TSite
            modelBuilder.Entity<TSite>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_T_Sites");
                entity.ToTable("T_Sites");
                entity.Property(e => e.City).HasMaxLength(255);
                entity.Property(e => e.Street).HasMaxLength(255);
                entity.Property(e => e.Title).HasMaxLength(255);

                entity.Property(e => e.Latitude).HasColumnType("float").IsRequired(false);
                entity.Property(e => e.Longitude).HasColumnType("float").IsRequired(false);

                entity.HasOne(d => d.Companie).WithMany(p => p.TSites)
                      .HasForeignKey(d => d.CompanieId)
                      .HasConstraintName("FK_T_Sites_CompanieId");

                // Configurer la relation avec TJobPost pour la suppression en cascade
                entity.HasMany(s => s.TJobPosts)
                      .WithOne(j => j.Site)
                      .HasForeignKey(j => j.SiteId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configuration de la table TTypeOfContract
            modelBuilder.Entity<TTypeOfContract>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_T_TypeOfContracts");
                entity.ToTable("T_TypeOfContracts");
                entity.Property(e => e.ContractName).HasMaxLength(100);
            });

            // Configuration de la table TUserRequirement
            modelBuilder.Entity<TUserRequirement>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_T_UserRequirements");
                entity.ToTable("T_UserRequirements");
                entity.Property(e => e.UserId).HasMaxLength(450);

                entity.HasOne(d => d.RequirementType).WithMany(p => p.TUserRequirements)
                      .HasForeignKey(d => d.RequirementTypeId)
                      .HasConstraintName("FK_T_UserRequirement_RequirementTypeId");

                entity.HasOne(d => d.User)
                      .WithMany(p => p.TUserRequirements)
                      .HasForeignKey(d => d.UserId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("FK_T_UserRequirements_Users");
            });

            // Configuration de la table UserCompanieAssociation
            modelBuilder.Entity<UserCompanieAssociation>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_UserCompanieAssociations");
                entity.Property(e => e.UserId).HasMaxLength(450);

                entity.HasOne(d => d.Company).WithMany(p => p.UserCompanieAssociations)
                      .HasForeignKey(d => d.CompanyId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("FK_UserCompanieAssociations_Companies");

                entity.HasOne(d => d.User).WithMany(p => p.UserCompanieAssociations)
                      .HasForeignKey(d => d.UserId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("FK_UserCompanieAssociations_Users");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
