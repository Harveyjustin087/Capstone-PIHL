using System;
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
        public virtual DbSet<GameTeam> GameTeams { get; set; }
        public virtual DbSet<GoalRecord> GoalRecords { get; set; }
        public virtual DbSet<Penalty> Penalties { get; set; }
        public virtual DbSet<PenaltyRecord> PenaltyRecords { get; set; }
        public virtual DbSet<Player> Players { get; set; }
        public virtual DbSet<Season> Seasons { get; set; }
        public virtual DbSet<Team> Teams { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS21;Database=PIHL;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<AspNetRole>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedName] IS NOT NULL)");

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetRoleClaim>(entity =>
            {
                entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

                entity.Property(e => e.RoleId).IsRequired();

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetUser>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedUserName] IS NOT NULL)");

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.FirstName).HasMaxLength(100);

                entity.Property(e => e.LastName).HasMaxLength(100);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserClaim>(entity =>
            {
                entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogin>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserRole>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasIndex(e => e.RoleId, "IX_AspNetUserRoles_RoleId");

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

                entity.Property(e => e.GameId)
                    .ValueGeneratedNever()
                    .HasColumnName("GameID");

                entity.Property(e => e.AwayTeamId).HasColumnName("AwayTeamID");

                entity.Property(e => e.GameDate).HasColumnType("date");

                entity.Property(e => e.HomeTeamId).HasColumnName("HomeTeamID");
            });

            modelBuilder.Entity<GameTeam>(entity =>
            {
                entity.ToTable("GameTeam");

                entity.Property(e => e.GameTeamId)
                    .ValueGeneratedNever()
                    .HasColumnName("GameTeamID");

                entity.Property(e => e.GameId).HasColumnName("GameID");

                entity.Property(e => e.TeamId).HasColumnName("TeamID");

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.GameTeams)
                    .HasForeignKey(d => d.GameId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__GameTeam__GameID__7B5B524B");

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.GameTeams)
                    .HasForeignKey(d => d.TeamId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__GameTeam__TeamID__7C4F7684");
            });

            modelBuilder.Entity<GoalRecord>(entity =>
            {
                entity.ToTable("GoalRecord");

                entity.Property(e => e.GoalRecordId)
                    .ValueGeneratedNever()
                    .HasColumnName("GoalRecordID");

                entity.Property(e => e.FirstAssistPlayerId).HasColumnName("FirstAssistPlayerID");

                entity.Property(e => e.GameId).HasColumnName("GameID");

                entity.Property(e => e.GameTime).HasColumnType("time(3)");

                entity.Property(e => e.ScoringPlayerId).HasColumnName("ScoringPlayerID");

                entity.Property(e => e.SecondAssistPlayerId).HasColumnName("SecondAssistPlayerID");

                entity.HasOne(d => d.FirstAssistPlayer)
                    .WithMany(p => p.GoalRecordFirstAssistPlayers)
                    .HasForeignKey(d => d.FirstAssistPlayerId)
                    .HasConstraintName("FK__GoalRecor__First__05D8E0BE");

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.GoalRecords)
                    .HasForeignKey(d => d.GameId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__GoalRecor__GameI__03F0984C");

                entity.HasOne(d => d.ScoringPlayer)
                    .WithMany(p => p.GoalRecordScoringPlayers)
                    .HasForeignKey(d => d.ScoringPlayerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__GoalRecor__Scori__04E4BC85");

                entity.HasOne(d => d.SecondAssistPlayer)
                    .WithMany(p => p.GoalRecordSecondAssistPlayers)
                    .HasForeignKey(d => d.SecondAssistPlayerId)
                    .HasConstraintName("FK__GoalRecor__Secon__06CD04F7");
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

                entity.Property(e => e.PenaltyRecordId)
                    .ValueGeneratedNever()
                    .HasColumnName("PenaltyRecordID");

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
                    .HasConstraintName("FK__PenaltyRe__GameI__17036CC0");

                entity.HasOne(d => d.Penalty)
                    .WithMany(p => p.PenaltyRecords)
                    .HasForeignKey(d => d.PenaltyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PenaltyRe__Penal__01142BA1");

                entity.HasOne(d => d.Player)
                    .WithMany(p => p.PenaltyRecords)
                    .HasForeignKey(d => d.PlayerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PenaltyRe__Playe__00200768");
            });

            modelBuilder.Entity<Player>(entity =>
            {
                entity.ToTable("Player");

                entity.Property(e => e.PlayerId)
                    .ValueGeneratedNever()
                    .HasColumnName("PlayerID");

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
                    .HasConstraintName("FK__Player__TeamID__76969D2E");
            });

            modelBuilder.Entity<Season>(entity =>
            {
                entity.ToTable("Season");

                entity.Property(e => e.SeasonId)
                    .ValueGeneratedNever()
                    .HasColumnName("SeasonID");

                entity.Property(e => e.EndYear).HasColumnType("date");

                entity.Property(e => e.StartYear).HasColumnType("date");
            });

            modelBuilder.Entity<Team>(entity =>
            {
                entity.ToTable("Team");

                entity.Property(e => e.TeamId)
                    .ValueGeneratedNever()
                    .HasColumnName("TeamID");

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
                    .HasConstraintName("FK__Team__SeasonID__73BA3083");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
