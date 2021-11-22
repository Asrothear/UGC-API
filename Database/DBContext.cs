using Microsoft.EntityFrameworkCore;
using UGC_API.Config;
using UGC_API.Models.v1_0;
using System;
using System.Collections.Generic;
using System.Text;
using UGC_API.Database_Models;

namespace UGC_API.Database
{
    class DBContext : DbContext
    {
        public static ModelBuilder MoBuilder;
        public DBContext() { }
        public DBContext(DbContextOptions<DBContext> options) : base(options) { }

        public DbSet<DB_Config> DB_Config { get; set; }
        public virtual DbSet<DB_Carrier> Carrier { get; set; }
        public virtual DbSet<DB_Verify_Token> Verify_Token { get; set; }
        public virtual DbSet<DB_User> DB_Users { get; set; }
        public virtual DbSet<DB_Systeme> DB_Systemes { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //Lokal
                string connectionStr = $"server={Configs.Values.DB.Host};port={Configs.Values.DB.Port};user={Configs.Values.DB.User};password={Configs.Values.DB.Password};database={Configs.Values.DB.Database}";
                optionsBuilder.UseMySql(connectionStr, ServerVersion.AutoDetect(connectionStr),
                    mySqlOptionsAction: mysqlOptions =>
                    {
                        mysqlOptions.EnableRetryOnFailure();
                    });
                optionsBuilder.EnableSensitiveDataLogging();
                optionsBuilder.EnableDetailedErrors();
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            MoBuilder = modelBuilder;
            MoBuilder.Entity<DB_Config>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.ToTable("ugc_*config", Configs.Values.DB.Database);
                entity.HasIndex(e => e.id).HasDatabaseName("id");
                entity.Property(e => e.systems).HasColumnName("systems");
                entity.Property(e => e.events).HasColumnName("events");
                entity.Property(e => e.update_systems).HasColumnName("update_systems");
            });
            MoBuilder.Entity<DB_Carrier>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.ToTable("ugc_*carrier", Configs.Values.DB.Database);
                entity.HasIndex(e => e.id).HasDatabaseName("id");
                entity.Property(e => e.CarrierID).HasColumnName("CarrierID");
                entity.Property(e => e.Name).HasColumnName("Name");
                entity.Property(e => e.Callsign).HasColumnName("Callsign");
                entity.Property(e => e.System).HasColumnName("System");
                entity.Property(e => e.prev_System).HasColumnName("prev_System");
                entity.Property(e => e.DockingAccess).HasColumnName("DockingAccess");
                entity.Property(e => e.AllowNotorious).HasColumnName("AllowNotorious");
                entity.Property(e => e.FuelLevel).HasColumnName("FuelLevel");
                entity.Property(e => e.JumpRangeCurr).HasColumnName("JumpRangeCurr");
                entity.Property(e => e.JumpRangeMax).HasColumnName("JumpRangeMax");
                entity.Property(e => e.PendingDecommission).HasColumnName("PendingDecommission");
                entity.Property(e => e.SpaceUsage).HasColumnName("SpaceUsage");
                entity.Property(e => e.Finance).HasColumnName("Finance");
                entity.Property(e => e.Crew).HasColumnName("Crew");
                entity.Property(e => e.ShipPacks).HasColumnName("ShipPacks");
                entity.Property(e => e.ModulePacks).HasColumnName("ModulePacks");
                entity.Property(e => e.market).HasColumnName("market");
                entity.Property(e => e.Last_Update).HasColumnName("Last_Update");
            });
            MoBuilder.Entity<DB_Verify_Token>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.ToTable("ugc_*verify_token", Configs.Values.DB.Database);
                entity.HasIndex(e => e.id).HasDatabaseName("id");
                entity.Property(e => e.discord_id).HasColumnName("discord_id");
                entity.Property(e => e.discord_name).HasColumnName("discord_name");
                entity.Property(e => e.token).HasColumnName("token");
                entity.Property(e => e.used).HasColumnName("used");
                entity.Property(e => e.created_time).HasColumnName("created_time");
                entity.Property(e => e.used_time).HasColumnName("used_time");
            });
            MoBuilder.Entity<DB_User>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.ToTable("ugc_*user", Configs.Values.DB.Database);
                entity.HasIndex(e => e.id).HasDatabaseName("id");
                entity.Property(e => e.user).HasColumnName("user");
                entity.Property(e => e.uuid).HasColumnName("uuid");
                entity.Property(e => e.token).HasColumnName("token");
                entity.Property(e => e.last_pos).HasColumnName("last_pos");
                entity.Property(e => e.system).HasColumnName("system");
                entity.Property(e => e.docked).HasColumnName("docked");
                entity.Property(e => e.docked_faction).HasColumnName("docked_faction");
                entity.Property(e => e.last_docked).HasColumnName("last_docked");
                entity.Property(e => e.last_docked_faction).HasColumnName("last_docked_faction");
                entity.Property(e => e.last_data_insert).HasColumnName("last_data_insert");
                entity.Property(e => e.version_plugin_major).HasColumnName("version_plugin_major");
                entity.Property(e => e.version_plugin_minor).HasColumnName("version_plugin_minor");
                entity.Property(e => e.branch).HasColumnName("branch");
            });
            MoBuilder.Entity<DB_Systeme>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.ToTable($"ugc_systeme", Configs.Values.DB.Database);
                entity.HasIndex(e => e.id).HasDatabaseName("id");
                entity.Property(e => e.Timestamp).HasColumnName("Timestamp");
                entity.Property(e => e.last_update).HasColumnName("last_update");
                entity.Property(e => e.User_ID).HasColumnName("User_ID");
                entity.Property(e => e.System_ID).HasColumnName("System_ID");
                entity.Property(e => e.System_Name).HasColumnName("System_Name");
                entity.Property(e => e.Factions).HasColumnName("Factions");
            });
        }
    }
}
