﻿using OAuth.Dal.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace OAuth.Dal
{
    public partial class OAuthContext : DbContext
    {
        public OAuthContext()
        {
        }

        public OAuthContext(DbContextOptions<OAuthContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<AccountConfirmation> AccountConfirmations { get; set; }
        public virtual DbSet<Application> Applications { get; set; }
        public virtual DbSet<ApplicationAuthentication> ApplicationAuthentications { get; set; }
        public virtual DbSet<Authentication> Authentications { get; set; }
        public virtual DbSet<Authorization> Authorizations { get; set; }
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<CompanyCategory> CompanyCategories { get; set; }
        public virtual DbSet<FailAttemp> FailAttemps { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<Ip> Ips { get; set; }
        public virtual DbSet<LoginFirstStep> LoginFirstSteps { get; set; }
        public virtual DbSet<Personal> Personals { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=OAuth;Trusted_Connection=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Latin1_General_CI_AS");

            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("Account");

                entity.HasIndex(e => e.Email, "UQ__Account__A9D1053447DA3310")
                    .IsUnique();

                entity.HasIndex(e => e.Key, "UQ__Account__C41E02890CBCC700")
                    .IsUnique();

                entity.HasIndex(e => e.UserName, "UQ__Account__C9F284563B67C3D9")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Key)
                    .IsRequired()
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.ProfileImageId).HasColumnName("ProfileImageID");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.ZipCode)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.HasOne(d => d.ProfileImage)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.ProfileImageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Account__Profile__2B3F6F97");
            });

            modelBuilder.Entity<AccountConfirmation>(entity =>
            {
                entity.ToTable("AccountConfirmation");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Key)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.HasOne(d => d.AccountNavigation)
                    .WithMany(p => p.AccountConfirmations)
                    .HasForeignKey(d => d.Account)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AccountCo__Accou__5629CD9C");
            });

            modelBuilder.Entity<Application>(entity =>
            {
                entity.ToTable("Application");

                entity.HasIndex(e => e.Key, "UQ__Applicat__C41E028918AB6B68")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AuthorizeRedirect)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Key)
                    .IsRequired()
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.LoginRedirect)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.PrivateKey)
                    .IsRequired()
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.Site)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.IconNavigation)
                    .WithMany(p => p.Applications)
                    .HasForeignKey(d => d.Icon)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Applicatio__Icon__398D8EEE");

                entity.HasOne(d => d.OwnerNavigation)
                    .WithMany(p => p.Applications)
                    .HasForeignKey(d => d.Owner)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Applicati__Owner__38996AB5");
            });

            modelBuilder.Entity<ApplicationAuthentication>(entity =>
            {
                entity.ToTable("ApplicationAuthentication");

                entity.HasIndex(e => e.Token, "UQ__Applicat__1EB4F817E482DFCB")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Ipadress)
                    .IsRequired()
                    .HasMaxLength(89)
                    .IsUnicode(false)
                    .HasColumnName("IPAdress");

                entity.Property(e => e.Token)
                    .IsRequired()
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.UserAgent)
                    .IsRequired()
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("User-Agent");

                entity.HasOne(d => d.ApplicationNavigation)
                    .WithMany(p => p.ApplicationAuthentications)
                    .HasForeignKey(d => d.Application)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Applicati__Appli__4E88ABD4");

                entity.HasOne(d => d.AuthenticationNavigation)
                    .WithMany(p => p.ApplicationAuthentications)
                    .HasForeignKey(d => d.Authentication)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Applicati__Authe__5070F446");

                entity.HasOne(d => d.AuthorizationNavigation)
                    .WithMany(p => p.ApplicationAuthentications)
                    .HasForeignKey(d => d.Authorization)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Applicati__Autho__4F7CD00D");

                entity.HasOne(d => d.IpadressNavigation)
                    .WithMany(p => p.ApplicationAuthentications)
                    .HasPrincipalKey(p => p.Adress)
                    .HasForeignKey(d => d.Ipadress)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Applicati__IPAdr__4D94879B");
            });

            modelBuilder.Entity<Authentication>(entity =>
            {
                entity.ToTable("Authentication");

                entity.HasIndex(e => e.Token, "UQ__Authenti__1EB4F817BFDBDC2E")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Ipadress)
                    .IsRequired()
                    .HasMaxLength(89)
                    .IsUnicode(false)
                    .HasColumnName("IPAdress");

                entity.Property(e => e.Token)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.UserAgent)
                    .IsRequired()
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("User-Agent");

                entity.HasOne(d => d.IpadressNavigation)
                    .WithMany(p => p.Authentications)
                    .HasPrincipalKey(p => p.Adress)
                    .HasForeignKey(d => d.Ipadress)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Authentic__IPAdr__45F365D3");

                entity.HasOne(d => d.LoginFirstStepNavigation)
                    .WithMany(p => p.Authentications)
                    .HasForeignKey(d => d.LoginFirstStep)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Authentic__Login__44FF419A");
            });

            modelBuilder.Entity<Authorization>(entity =>
            {
                entity.ToTable("Authorization");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Key)
                    .IsRequired()
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.HasOne(d => d.ApplicationNavigation)
                    .WithMany(p => p.Authorizations)
                    .HasForeignKey(d => d.Application)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Authoriza__Appli__49C3F6B7");

                entity.HasOne(d => d.AuthenticationNavigation)
                    .WithMany(p => p.Authorizations)
                    .HasForeignKey(d => d.Authentication)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Authoriza__Authe__48CFD27E");
            });

            modelBuilder.Entity<Company>(entity =>
            {
                entity.ToTable("Company");

                entity.HasIndex(e => e.Cnpj, "UQ__Company__AA57D6B4562355FC")
                    .IsUnique();

                entity.HasIndex(e => e.Account, "UQ__Company__B0C3AC46F9169B29")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.Cnpj)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CNPJ");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.HasOne(d => d.AccountNavigation)
                    .WithOne(p => p.Company)
                    .HasForeignKey<Company>(d => d.Account)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Company__Account__300424B4");

                entity.HasOne(d => d.IconNavigation)
                    .WithMany(p => p.Companies)
                    .HasForeignKey(d => d.Icon)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Company__Icon__30F848ED");
            });

            modelBuilder.Entity<CompanyCategory>(entity =>
            {
                entity.ToTable("CompanyCategory");

                entity.Property(e => e.Id).HasColumnName("ID");
            });

            modelBuilder.Entity<FailAttemp>(entity =>
            {
                entity.ToTable("FailAttemp");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Ipadress)
                    .IsRequired()
                    .HasMaxLength(89)
                    .IsUnicode(false)
                    .HasColumnName("IPAdress");

                entity.HasOne(d => d.IpadressNavigation)
                    .WithMany(p => p.FailAttemps)
                    .HasPrincipalKey(p => p.Adress)
                    .HasForeignKey(d => d.Ipadress)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__FailAttem__IPAdr__534D60F1");
            });

            modelBuilder.Entity<Image>(entity =>
            {
                entity.ToTable("Image");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.FileName)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Ip>(entity =>
            {
                entity.ToTable("IP");

                entity.HasIndex(e => e.Adress, "UQ__IP__08F62FE5F121308B")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Adress)
                    .IsRequired()
                    .HasMaxLength(89)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<LoginFirstStep>(entity =>
            {
                entity.ToTable("LoginFirstStep");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Ipadress)
                    .IsRequired()
                    .HasMaxLength(89)
                    .IsUnicode(false)
                    .HasColumnName("IPAdress");

                entity.Property(e => e.Token)
                    .IsRequired()
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.HasOne(d => d.AccountNavigation)
                    .WithMany(p => p.LoginFirstSteps)
                    .HasForeignKey(d => d.Account)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__LoginFirs__Accou__403A8C7D");

                entity.HasOne(d => d.IpadressNavigation)
                    .WithMany(p => p.LoginFirstSteps)
                    .HasPrincipalKey(p => p.Adress)
                    .HasForeignKey(d => d.Ipadress)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__LoginFirs__IPAdr__3F466844");
            });

            modelBuilder.Entity<Personal>(entity =>
            {
                entity.ToTable("Personal");

                entity.HasIndex(e => e.Account, "UQ__Personal__B0C3AC4685509328")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.HasOne(d => d.AccountNavigation)
                    .WithOne(p => p.Personal)
                    .HasForeignKey<Personal>(d => d.Account)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Personal__Accoun__34C8D9D1");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
