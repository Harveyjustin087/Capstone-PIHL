using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace PIHLSite.Models
{
    public partial class PIHLDBContext : DbContext
    {
        public PIHLDBContext()
        {
        }

        public PIHLDBContext(DbContextOptions<PIHLDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRole> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }
        public virtual DbSet<Game> Games { get; set; }
        public virtual DbSet<GoalRecord> GoalRecords { get; set; }
        public virtual DbSet<Penalty> Penalties { get; set; }
        public virtual DbSet<PenaltyRecord> PenaltyRecords { get; set; }
        public virtual DbSet<Player> Players { get; set; }
        public virtual DbSet<Season> Seasons { get; set; }
        public virtual DbSet<Team> Teams { get; set; }
        public IEnumerable<object> Users { get; internal set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS21;Database=PIHLDB;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<AspNetRole>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetRoleClaim>(entity =>
            {
                entity.Property(e => e.RoleId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetUser>(entity =>
            {
                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.FirstName).HasMaxLength(100);

                entity.Property(e => e.LastName).HasMaxLength(100);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserClaim>(entity =>
            {
                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogin>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserRole>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.RoleId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserToken>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserTokens)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<Game>(entity =>
            {
                entity.ToTable("Game");

                entity.Property(e => e.GameId).HasColumnName("GameID");

                entity.Property(e => e.AwayTeamId).HasColumnName("AwayTeamID");

                entity.Property(e => e.GameDate).HasColumnType("date");

                entity.Property(e => e.HomeTeamId).HasColumnName("HomeTeamID");

                entity.HasOne(d => d.AwayTeam)
                    .WithMany(p => p.GameAwayTeams)
                    .HasForeignKey(d => d.AwayTeamId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Game__AwayTeamID__2E1BDC42");

                entity.HasOne(d => d.HomeTeam)
                    .WithMany(p => p.GameHomeTeams)
                    .HasForeignKey(d => d.HomeTeamId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Game__HomeTeamID__2F10007B");
            });

            modelBuilder.Entity<GoalRecord>(entity =>
            {
                entity.ToTable("GoalRecord");

                entity.Property(e => e.GoalRecordId).HasColumnName("GoalRecordID");

                entity.Property(e => e.FirstAssistPlayerId).HasColumnName("FirstAssistPlayerID");

                entity.Property(e => e.GameId).HasColumnName("GameID");

                entity.Property(e => e.GameTime).HasColumnType("time(3)");

                entity.Property(e => e.ScoringPlayerId).HasColumnName("ScoringPlayerID");

                entity.Property(e => e.SecondAssistPlayerId).HasColumnName("SecondAssistPlayerID");

                entity.HasOne(d => d.FirstAssistPlayer)
                    .WithMany(p => p.GoalRecordFirstAssistPlayers)
                    .HasForeignKey(d => d.FirstAssistPlayerId)
                    .HasConstraintName("FK__GoalRecor__First__38996AB5");

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.GoalRecords)
                    .HasForeignKey(d => d.GameId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__GoalRecor__GameI__36B12243");

                entity.HasOne(d => d.ScoringPlayer)
                    .WithMany(p => p.GoalRecordScoringPlayers)
                    .HasForeignKey(d => d.ScoringPlayerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__GoalRecor__Scori__37A5467C");

                entity.HasOne(d => d.SecondAssistPlayer)
                    .WithMany(p => p.GoalRecordSecondAssistPlayers)
                    .HasForeignKey(d => d.SecondAssistPlayerId)
                    .HasConstraintName("FK__GoalRecor__Secon__398D8EEE");
            });

            modelBuilder.Entity<Penalty>(entity =>
            {
                entity.ToTable("Penalty");

                entity.Property(e => e.PenaltyId)
                    .ValueGeneratedNever()
                    .HasColumnName("PenaltyID");

                entity.Property(e => e.PenaltyCode)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.PenaltyDescription)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PenaltyRecord>(entity =>
            {
                entity.ToTable("PenaltyRecord");

                entity.Property(e => e.PenaltyRecordId).HasColumnName("PenaltyRecordID");

                entity.Property(e => e.GameId).HasColumnName("GameID");

                entity.Property(e => e.PenaltyId).HasColumnName("PenaltyID");

                entity.Property(e => e.Pim)
                    .HasColumnType("time(3)")
                    .HasColumnName("PIM");

                entity.Property(e => e.PlayerId).HasColumnName("PlayerID");

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.PenaltyRecords)
                    .HasForeignKey(d => d.GameId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PenaltyRe__GameI__31EC6D26");

                entity.HasOne(d => d.Penalty)
                    .WithMany(p => p.PenaltyRecords)
                    .HasForeignKey(d => d.PenaltyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PenaltyRe__Penal__33D4B598");

                entity.HasOne(d => d.Player)
                    .WithMany(p => p.PenaltyRecords)
                    .HasForeignKey(d => d.PlayerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PenaltyRe__Playe__32E0915F");
            });

            modelBuilder.Entity<Player>(entity =>
            {
                entity.ToTable("Player");

                entity.Property(e => e.PlayerId).HasColumnName("PlayerID");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Pimtotal)
                    .HasColumnType("time(3)")
                    .HasColumnName("PIMTotal");

                entity.Property(e => e.TeamId).HasColumnName("TeamID");

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.Players)
                    .HasForeignKey(d => d.TeamId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Player__TeamID__2B3F6F97");
            });

            modelBuilder.Entity<Season>(entity =>
            {
                entity.ToTable("Season");

                entity.Property(e => e.SeasonId).HasColumnName("SeasonID");

                entity.Property(e => e.EndYear).HasColumnType("date");

                entity.Property(e => e.StartYear).HasColumnType("date");
            });

            modelBuilder.Entity<Team>(entity =>
            {
                entity.ToTable("Team");

                entity.Property(e => e.TeamId).HasColumnName("TeamID");

                entity.Property(e => e.Company)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Otl).HasColumnName("OTL");

                entity.Property(e => e.SeasonId).HasColumnName("SeasonID");

                entity.HasOne(d => d.Season)
                    .WithMany(p => p.Teams)
                    .HasForeignKey(d => d.SeasonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Team__SeasonID__286302EC");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
