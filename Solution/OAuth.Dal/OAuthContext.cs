using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using OAuth.Dal.Models;

#nullable disable

namespace OAuth.Dal
{
    public partial class OAuthContext : DbContext
    {
        public static string ConnectionString { get; set; }
        public OAuthContext()
        {
        }

        public OAuthContext(DbContextOptions<OAuthContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<AccountDetail> AccountDetails { get; set; }
        public virtual DbSet<Application> Applications { get; set; }
        public virtual DbSet<ApplicationAuthentication> ApplicationAuthentications { get; set; }
        public virtual DbSet<Authentication> Authentications { get; set; }
        public virtual DbSet<Authorization> Authorizations { get; set; }
        public virtual DbSet<FailAttemp> FailAttemps { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<Ip> Ips { get; set; }
        public virtual DbSet<LoginFirstStep> LoginFirstSteps { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer(ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("Account");

                entity.HasIndex(e => e.Email, "UQ__Account__A9D1053458F1F84B")
                    .IsUnique();

                entity.HasIndex(e => e.Key, "UQ__Account__C41E0289828F343F")
                    .IsUnique();

                entity.HasIndex(e => e.UserName, "UQ__Account__C9F28456F4B7E541")
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

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ProfileImageId).HasColumnName("ProfileImageID");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.HasOne(d => d.ProfileImage)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.ProfileImageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Account__Profile__619B8048");
            });

            modelBuilder.Entity<AccountDetail>(entity =>
            {
                entity.HasIndex(e => e.Account, "UQ__AccountD__B0C3AC46768982D0")
                    .IsUnique();

                entity.HasIndex(e => e.CpforCnpj, "UQ__AccountD__D47B2C1320D59997")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CpforCnpj)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CPFOrCNPJ");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.AccountNavigation)
                    .WithOne(p => p.AccountDetail)
                    .HasForeignKey<AccountDetail>(d => d.Account)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AccountDe__Accou__66603565");
            });

            modelBuilder.Entity<Application>(entity =>
            {
                entity.ToTable("Application");

                entity.HasIndex(e => e.Key, "UQ__Applicat__C41E0289C8CA3B27")
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
                    .HasConstraintName("FK__Applicatio__Icon__6B24EA82");

                entity.HasOne(d => d.OwnerNavigation)
                    .WithMany(p => p.Applications)
                    .HasForeignKey(d => d.Owner)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Applicati__Owner__6A30C649");
            });

            modelBuilder.Entity<ApplicationAuthentication>(entity =>
            {
                entity.ToTable("ApplicationAuthentication");

                entity.HasIndex(e => e.Token, "UQ__Applicat__1EB4F81733EB9E06")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Ipadress)
                    .IsRequired()
                    .HasMaxLength(89)
                    .IsUnicode(false)
                    .HasColumnName("IPAdress");

                entity.Property(e => e.Token)
                    .IsRequired()
                    .HasMaxLength(86)
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
                    .HasConstraintName("FK__Applicati__Appli__00200768");

                entity.HasOne(d => d.AuthenticationNavigation)
                    .WithMany(p => p.ApplicationAuthentications)
                    .HasForeignKey(d => d.Authentication)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Applicati__Authe__02084FDA");

                entity.HasOne(d => d.AuthorizationNavigation)
                    .WithMany(p => p.ApplicationAuthentications)
                    .HasForeignKey(d => d.Authorization)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Applicati__Autho__01142BA1");

                entity.HasOne(d => d.IpadressNavigation)
                    .WithMany(p => p.ApplicationAuthentications)
                    .HasPrincipalKey(p => p.Adress)
                    .HasForeignKey(d => d.Ipadress)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Applicati__IPAdr__7F2BE32F");
            });

            modelBuilder.Entity<Authentication>(entity =>
            {
                entity.ToTable("Authentication");

                entity.HasIndex(e => e.Token, "UQ__Authenti__1EB4F81751EA8281")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Ipadress)
                    .IsRequired()
                    .HasMaxLength(89)
                    .IsUnicode(false)
                    .HasColumnName("IPAdress");

                entity.Property(e => e.Token)
                    .IsRequired()
                    .HasMaxLength(86)
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
                    .HasConstraintName("FK__Authentic__IPAdr__778AC167");

                entity.HasOne(d => d.LoginFirstStepNavigation)
                    .WithMany(p => p.Authentications)
                    .HasForeignKey(d => d.LoginFirstStep)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Authentic__Login__76969D2E");
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
                    .HasConstraintName("FK__Authoriza__Appli__7B5B524B");

                entity.HasOne(d => d.AuthenticationNavigation)
                    .WithMany(p => p.Authorizations)
                    .HasForeignKey(d => d.Authentication)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Authoriza__Authe__7A672E12");
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
                    .HasConstraintName("FK__FailAttem__IPAdr__04E4BC85");
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

                entity.HasIndex(e => e.Adress, "UQ__IP__08F62FE55FCA5AB0")
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
                    .HasConstraintName("FK__LoginFirs__Accou__71D1E811");

                entity.HasOne(d => d.IpadressNavigation)
                    .WithMany(p => p.LoginFirstSteps)
                    .HasPrincipalKey(p => p.Adress)
                    .HasForeignKey(d => d.Ipadress)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__LoginFirs__IPAdr__70DDC3D8");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
