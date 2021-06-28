using Microsoft.EntityFrameworkCore;
using OAuth.Dal.Models;

namespace OAuth.Dal
{
    public partial class OAuthContext : DbContext
    {
        public string ConnectionString { get; set; }
        public static bool Development { get; set; }
        public OAuthContext()
        {
            ConnectionString = Properties.Resources.ConnectionString;
            if (Development)
            {
                ConnectionString = Properties.Resources.DevelopmentConnectionString;
            }
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

                entity.HasIndex(e => e.Email, "UQ__Account__A9D105340490F2EB")
                    .IsUnique();

                entity.HasIndex(e => e.Key, "UQ__Account__C41E0289CD5DBAFE")
                    .IsUnique();

                entity.HasIndex(e => e.UserName, "UQ__Account__C9F28456B929DB11")
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
                    .HasMaxLength(25)
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
                    .HasConstraintName("FK__Account__Profile__2DE6D218");
            });

            modelBuilder.Entity<AccountDetail>(entity =>
            {
                entity.HasIndex(e => e.Account, "UQ__AccountD__B0C3AC467E05ABAE")
                    .IsUnique();

                entity.HasIndex(e => e.CpforCnpj, "UQ__AccountD__D47B2C135FFF3A9D")
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
                    .HasConstraintName("FK__AccountDe__Accou__32AB8735");
            });

            modelBuilder.Entity<Application>(entity =>
            {
                entity.ToTable("Application");

                entity.HasIndex(e => e.PrivateKey, "UQ__Applicat__9C560B9E539EEDD8")
                    .IsUnique();

                entity.HasIndex(e => e.Key, "UQ__Applicat__C41E0289D591A299")
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
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.IconNavigation)
                    .WithMany(p => p.Applications)
                    .HasForeignKey(d => d.Icon)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Applicatio__Icon__3864608B");

                entity.HasOne(d => d.OwnerNavigation)
                    .WithMany(p => p.Applications)
                    .HasForeignKey(d => d.Owner)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Applicati__Owner__37703C52");
            });

            modelBuilder.Entity<ApplicationAuthentication>(entity =>
            {
                entity.ToTable("ApplicationAuthentication");

                entity.HasIndex(e => e.Token, "UQ__Applicat__1EB4F81736580873")
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
                    .IsUnicode(false);

                entity.HasOne(d => d.ApplicationNavigation)
                    .WithMany(p => p.ApplicationAuthentications)
                    .HasForeignKey(d => d.Application)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Applicati__Appli__4D5F7D71");

                entity.HasOne(d => d.AuthenticationNavigation)
                    .WithMany(p => p.ApplicationAuthentications)
                    .HasForeignKey(d => d.Authentication)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Applicati__Authe__4F47C5E3");

                entity.HasOne(d => d.AuthorizationNavigation)
                    .WithMany(p => p.ApplicationAuthentications)
                    .HasForeignKey(d => d.Authorization)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Applicati__Autho__4E53A1AA");

                entity.HasOne(d => d.IpadressNavigation)
                    .WithMany(p => p.ApplicationAuthentications)
                    .HasPrincipalKey(p => p.Adress)
                    .HasForeignKey(d => d.Ipadress)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Applicati__IPAdr__4C6B5938");
            });

            modelBuilder.Entity<Authentication>(entity =>
            {
                entity.ToTable("Authentication");

                entity.HasIndex(e => e.Token, "UQ__Authenti__1EB4F817EE91FCCB")
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
                    .HasColumnName("UserAgent");

                entity.HasOne(d => d.IpadressNavigation)
                    .WithMany(p => p.Authentications)
                    .HasPrincipalKey(p => p.Adress)
                    .HasForeignKey(d => d.Ipadress)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Authentic__IPAdr__44CA3770");

                entity.HasOne(d => d.LoginFirstStepNavigation)
                    .WithMany(p => p.Authentications)
                    .HasForeignKey(d => d.LoginFirstStep)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Authentic__Login__43D61337");
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
                    .HasConstraintName("FK__Authoriza__Appli__489AC854");

                entity.HasOne(d => d.AuthenticationNavigation)
                    .WithMany(p => p.Authorizations)
                    .HasForeignKey(d => d.Authentication)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Authoriza__Authe__47A6A41B");
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
                    .HasConstraintName("FK__FailAttem__IPAdr__5224328E");
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

                entity.HasIndex(e => e.Adress, "UQ__IP__08F62FE55D3892D6")
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
                    .HasConstraintName("FK__LoginFirs__Accou__3F115E1A");

                entity.HasOne(d => d.IpadressNavigation)
                    .WithMany(p => p.LoginFirstSteps)
                    .HasPrincipalKey(p => p.Adress)
                    .HasForeignKey(d => d.Ipadress)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__LoginFirs__IPAdr__3E1D39E1");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
