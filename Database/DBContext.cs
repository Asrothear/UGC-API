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
        public virtual DbSet<DB_ambiabar> ambiabar { get; set; }
        public virtual DbSet<DB_anahit> anahit { get; set; }
        public virtual DbSet<DB_angurongo> angurongo { get; set; }
        public virtual DbSet<DB_bpm_16204> bpm_16204 { get; set; }
        public virtual DbSet<DB_cernunnos> cernunnos { get; set; }
        public virtual DbSet<DB_crom_dubh> crom_dubh { get; set; }
        public virtual DbSet<DB_dall> dall { get; set; }
        public virtual DbSet<DB_delta_phoenicis> delta_phoenicis { get; set; }
        public virtual DbSet<DB_duronese> duronese { get; set; }
        public virtual DbSet<DB_hip_2747> hip_2747 { get; set; }
        public virtual DbSet<DB_hip_3603> hip_3603 { get; set; }
        public virtual DbSet<DB_hip_4764> hip_4764 { get; set; }
        public virtual DbSet<DB_hip_4964> hip_4964 { get; set; }
        public virtual DbSet<DB_hip_5099> hip_5099 { get; set; }
        public virtual DbSet<DB_hyperborea> hyperborea { get; set; }
        public virtual DbSet<DB_kartamayana> kartamayana { get; set; }
        public virtual DbSet<DB_khampti> khampti { get; set; }
        public virtual DbSet<DB_kharpulo> kharpulo { get; set; }
        public virtual DbSet<DB_kunggalerni> kunggalerni { get; set; }
        public virtual DbSet<DB_ltt_518> ltt_518 { get; set; }
        public virtual DbSet<DB_ltt_874> ltt_874 { get; set; }
        public virtual DbSet<DB_liu_di> liu_di { get; set; }
        public virtual DbSet<DB_maidubrigel> maidubrigel { get; set; }
        public virtual DbSet<DB_minanes> minanes { get; set; }
        public virtual DbSet<DB_nayanezgani> nayanezgani { get; set; }
        public virtual DbSet<DB_niflhel> niflhel { get; set; }
        public virtual DbSet<DB_nltt_2682> nltt_2682 { get; set; }
        public virtual DbSet<DB_paras> paras { get; set; }
        public virtual DbSet<DB_piperish> piperish { get; set; }
        public virtual DbSet<DB_rosmerta> rosmerta { get; set; }
        public virtual DbSet<DB_runo> runo { get; set; }
        public virtual DbSet<DB_sadhbh> sadhbh { get; set; }
        public virtual DbSet<DB_slatas> slatas { get; set; }
        public virtual DbSet<DB_tetela> tetela { get; set; }
        public virtual DbSet<DB_tocorii> tocorii { get; set; }
        public virtual DbSet<DB_wapiya> wapiya { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //Lokal
                string connectionStr = $"server={DatabaseConfig.Host};port={DatabaseConfig.Port};user={DatabaseConfig.User};password={DatabaseConfig.Password};database={DatabaseConfig.Database}";
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
                entity.ToTable("ugc_*config", DatabaseConfig.Database);
                entity.HasIndex(e => e.id).HasDatabaseName("id");
                entity.Property(e => e.systems).HasColumnName("systems");
                entity.Property(e => e.events).HasColumnName("events");
                entity.Property(e => e.update_systems).HasColumnName("update_systems");
            });
            MoBuilder.Entity<DB_Carrier>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.ToTable("ugc_*carrier", DatabaseConfig.Database);
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
                entity.ToTable("ugc_*verify_token", DatabaseConfig.Database);
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
                entity.ToTable("ugc_*user", DatabaseConfig.Database);
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
                entity.Property(e => e.version_plugin).HasColumnName("version_plugin");
                entity.Property(e => e.branch).HasColumnName("branch");
            });
            MoBuilder.Entity<DB_ambiabar>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.ToTable($"ugc_ambiabar", DatabaseConfig.Database);
                entity.HasIndex(e => e.id).HasDatabaseName("id");
                entity.Property(e => e.Timestamp).HasColumnName("Timestamp");
                entity.Property(e => e.last_update).HasColumnName("last_update");
                entity.Property(e => e.User_ID).HasColumnName("User_ID");
                entity.Property(e => e.System_ID).HasColumnName("System_ID");
                entity.Property(e => e.Faction_Name).HasColumnName("Faction_Name");
                entity.Property(e => e.Faction_State).HasColumnName("Faction_State");
                entity.Property(e => e.Faction_Government).HasColumnName("Faction_Government");
                entity.Property(e => e.Faction_Influence).HasColumnName("Faction_Influence");
                entity.Property(e => e.Faction_Influence_change).HasColumnName("Faction_Influence_change");
                entity.Property(e => e.Faction_Allegiance).HasColumnName("Faction_Allegiance");
                entity.Property(e => e.Faction_Happiness).HasColumnName("Faction_Happiness");
                entity.Property(e => e.Faction_ActiveState).HasColumnName("Faction_ActiveState");
                entity.Property(e => e.Faction_PendingState).HasColumnName("Faction_PendingState");
                entity.Property(e => e.Faction_RecoveringState).HasColumnName("Faction_RecoveringState");
                entity.Property(e => e.missions).HasColumnName("missions");
                entity.Property(e => e.explorer).HasColumnName("explorer");
                entity.Property(e => e.voucheer).HasColumnName("voucheer");
                entity.Property(e => e.trade).HasColumnName("trade");
            });
            MoBuilder.Entity<DB_anahit>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.ToTable($"ugc_anahit", DatabaseConfig.Database);
                entity.HasIndex(e => e.id).HasDatabaseName("id");
                entity.Property(e => e.Timestamp).HasColumnName("Timestamp");
                entity.Property(e => e.last_update).HasColumnName("last_update");
                entity.Property(e => e.User_ID).HasColumnName("User_ID");
                entity.Property(e => e.System_ID).HasColumnName("System_ID");
                entity.Property(e => e.Faction_Name).HasColumnName("Faction_Name");
                entity.Property(e => e.Faction_State).HasColumnName("Faction_State");
                entity.Property(e => e.Faction_Government).HasColumnName("Faction_Government");
                entity.Property(e => e.Faction_Influence).HasColumnName("Faction_Influence");
                entity.Property(e => e.Faction_Influence_change).HasColumnName("Faction_Influence_change");
                entity.Property(e => e.Faction_Allegiance).HasColumnName("Faction_Allegiance");
                entity.Property(e => e.Faction_Happiness).HasColumnName("Faction_Happiness");
                entity.Property(e => e.Faction_ActiveState).HasColumnName("Faction_ActiveState");
                entity.Property(e => e.Faction_PendingState).HasColumnName("Faction_PendingState");
                entity.Property(e => e.Faction_RecoveringState).HasColumnName("Faction_RecoveringState");
                entity.Property(e => e.missions).HasColumnName("missions");
                entity.Property(e => e.explorer).HasColumnName("explorer");
                entity.Property(e => e.voucheer).HasColumnName("voucheer");
                entity.Property(e => e.trade).HasColumnName("trade");
            });
            MoBuilder.Entity<DB_angurongo>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.ToTable($"ugc_angurongo", DatabaseConfig.Database);
                entity.HasIndex(e => e.id).HasDatabaseName("id");
                entity.Property(e => e.Timestamp).HasColumnName("Timestamp");
                entity.Property(e => e.last_update).HasColumnName("last_update");
                entity.Property(e => e.User_ID).HasColumnName("User_ID");
                entity.Property(e => e.System_ID).HasColumnName("System_ID");
                entity.Property(e => e.Faction_Name).HasColumnName("Faction_Name");
                entity.Property(e => e.Faction_State).HasColumnName("Faction_State");
                entity.Property(e => e.Faction_Government).HasColumnName("Faction_Government");
                entity.Property(e => e.Faction_Influence).HasColumnName("Faction_Influence");
                entity.Property(e => e.Faction_Influence_change).HasColumnName("Faction_Influence_change");
                entity.Property(e => e.Faction_Allegiance).HasColumnName("Faction_Allegiance");
                entity.Property(e => e.Faction_Happiness).HasColumnName("Faction_Happiness");
                entity.Property(e => e.Faction_ActiveState).HasColumnName("Faction_ActiveState");
                entity.Property(e => e.Faction_PendingState).HasColumnName("Faction_PendingState");
                entity.Property(e => e.Faction_RecoveringState).HasColumnName("Faction_RecoveringState");
                entity.Property(e => e.missions).HasColumnName("missions");
                entity.Property(e => e.explorer).HasColumnName("explorer");
                entity.Property(e => e.voucheer).HasColumnName("voucheer");
                entity.Property(e => e.trade).HasColumnName("trade");
            });
            MoBuilder.Entity<DB_bpm_16204>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.ToTable($"ugc_bpm 16204", DatabaseConfig.Database);
                entity.HasIndex(e => e.id).HasDatabaseName("id");
                entity.Property(e => e.Timestamp).HasColumnName("Timestamp");
                entity.Property(e => e.last_update).HasColumnName("last_update");
                entity.Property(e => e.User_ID).HasColumnName("User_ID");
                entity.Property(e => e.System_ID).HasColumnName("System_ID");
                entity.Property(e => e.Faction_Name).HasColumnName("Faction_Name");
                entity.Property(e => e.Faction_State).HasColumnName("Faction_State");
                entity.Property(e => e.Faction_Government).HasColumnName("Faction_Government");
                entity.Property(e => e.Faction_Influence).HasColumnName("Faction_Influence");
                entity.Property(e => e.Faction_Influence_change).HasColumnName("Faction_Influence_change");
                entity.Property(e => e.Faction_Allegiance).HasColumnName("Faction_Allegiance");
                entity.Property(e => e.Faction_Happiness).HasColumnName("Faction_Happiness");
                entity.Property(e => e.Faction_ActiveState).HasColumnName("Faction_ActiveState");
                entity.Property(e => e.Faction_PendingState).HasColumnName("Faction_PendingState");
                entity.Property(e => e.Faction_RecoveringState).HasColumnName("Faction_RecoveringState");
                entity.Property(e => e.missions).HasColumnName("missions");
                entity.Property(e => e.explorer).HasColumnName("explorer");
                entity.Property(e => e.voucheer).HasColumnName("voucheer");
                entity.Property(e => e.trade).HasColumnName("trade");
            });
            MoBuilder.Entity<DB_cernunnos>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.ToTable($"ugc_cernunnos", DatabaseConfig.Database);
                entity.HasIndex(e => e.id).HasDatabaseName("id");
                entity.Property(e => e.Timestamp).HasColumnName("Timestamp");
                entity.Property(e => e.last_update).HasColumnName("last_update");
                entity.Property(e => e.User_ID).HasColumnName("User_ID");
                entity.Property(e => e.System_ID).HasColumnName("System_ID");
                entity.Property(e => e.Faction_Name).HasColumnName("Faction_Name");
                entity.Property(e => e.Faction_State).HasColumnName("Faction_State");
                entity.Property(e => e.Faction_Government).HasColumnName("Faction_Government");
                entity.Property(e => e.Faction_Influence).HasColumnName("Faction_Influence");
                entity.Property(e => e.Faction_Influence_change).HasColumnName("Faction_Influence_change");
                entity.Property(e => e.Faction_Allegiance).HasColumnName("Faction_Allegiance");
                entity.Property(e => e.Faction_Happiness).HasColumnName("Faction_Happiness");
                entity.Property(e => e.Faction_ActiveState).HasColumnName("Faction_ActiveState");
                entity.Property(e => e.Faction_PendingState).HasColumnName("Faction_PendingState");
                entity.Property(e => e.Faction_RecoveringState).HasColumnName("Faction_RecoveringState");
                entity.Property(e => e.missions).HasColumnName("missions");
                entity.Property(e => e.explorer).HasColumnName("explorer");
                entity.Property(e => e.voucheer).HasColumnName("voucheer");
                entity.Property(e => e.trade).HasColumnName("trade");
            });
            MoBuilder.Entity<DB_crom_dubh>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.ToTable($"ugc_crom dubh", DatabaseConfig.Database);
                entity.HasIndex(e => e.id).HasDatabaseName("id");
                entity.Property(e => e.Timestamp).HasColumnName("Timestamp");
                entity.Property(e => e.last_update).HasColumnName("last_update");
                entity.Property(e => e.User_ID).HasColumnName("User_ID");
                entity.Property(e => e.System_ID).HasColumnName("System_ID");
                entity.Property(e => e.Faction_Name).HasColumnName("Faction_Name");
                entity.Property(e => e.Faction_State).HasColumnName("Faction_State");
                entity.Property(e => e.Faction_Government).HasColumnName("Faction_Government");
                entity.Property(e => e.Faction_Influence).HasColumnName("Faction_Influence");
                entity.Property(e => e.Faction_Influence_change).HasColumnName("Faction_Influence_change");
                entity.Property(e => e.Faction_Allegiance).HasColumnName("Faction_Allegiance");
                entity.Property(e => e.Faction_Happiness).HasColumnName("Faction_Happiness");
                entity.Property(e => e.Faction_ActiveState).HasColumnName("Faction_ActiveState");
                entity.Property(e => e.Faction_PendingState).HasColumnName("Faction_PendingState");
                entity.Property(e => e.Faction_RecoveringState).HasColumnName("Faction_RecoveringState");
                entity.Property(e => e.missions).HasColumnName("missions");
                entity.Property(e => e.explorer).HasColumnName("explorer");
                entity.Property(e => e.voucheer).HasColumnName("voucheer");
                entity.Property(e => e.trade).HasColumnName("trade");
            });
            MoBuilder.Entity<DB_dall>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.ToTable($"ugc_dall", DatabaseConfig.Database);
                entity.HasIndex(e => e.id).HasDatabaseName("id");
                entity.Property(e => e.Timestamp).HasColumnName("Timestamp");
                entity.Property(e => e.last_update).HasColumnName("last_update");
                entity.Property(e => e.User_ID).HasColumnName("User_ID");
                entity.Property(e => e.System_ID).HasColumnName("System_ID");
                entity.Property(e => e.Faction_Name).HasColumnName("Faction_Name");
                entity.Property(e => e.Faction_State).HasColumnName("Faction_State");
                entity.Property(e => e.Faction_Government).HasColumnName("Faction_Government");
                entity.Property(e => e.Faction_Influence).HasColumnName("Faction_Influence");
                entity.Property(e => e.Faction_Influence_change).HasColumnName("Faction_Influence_change");
                entity.Property(e => e.Faction_Allegiance).HasColumnName("Faction_Allegiance");
                entity.Property(e => e.Faction_Happiness).HasColumnName("Faction_Happiness");
                entity.Property(e => e.Faction_ActiveState).HasColumnName("Faction_ActiveState");
                entity.Property(e => e.Faction_PendingState).HasColumnName("Faction_PendingState");
                entity.Property(e => e.Faction_RecoveringState).HasColumnName("Faction_RecoveringState");
                entity.Property(e => e.missions).HasColumnName("missions");
                entity.Property(e => e.explorer).HasColumnName("explorer");
                entity.Property(e => e.voucheer).HasColumnName("voucheer");
                entity.Property(e => e.trade).HasColumnName("trade");
            });
            MoBuilder.Entity<DB_delta_phoenicis>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.ToTable($"ugc_delta phoenicis", DatabaseConfig.Database);
                entity.HasIndex(e => e.id).HasDatabaseName("id");
                entity.Property(e => e.Timestamp).HasColumnName("Timestamp");
                entity.Property(e => e.last_update).HasColumnName("last_update");
                entity.Property(e => e.User_ID).HasColumnName("User_ID");
                entity.Property(e => e.System_ID).HasColumnName("System_ID");
                entity.Property(e => e.Faction_Name).HasColumnName("Faction_Name");
                entity.Property(e => e.Faction_State).HasColumnName("Faction_State");
                entity.Property(e => e.Faction_Government).HasColumnName("Faction_Government");
                entity.Property(e => e.Faction_Influence).HasColumnName("Faction_Influence");
                entity.Property(e => e.Faction_Influence_change).HasColumnName("Faction_Influence_change");
                entity.Property(e => e.Faction_Allegiance).HasColumnName("Faction_Allegiance");
                entity.Property(e => e.Faction_Happiness).HasColumnName("Faction_Happiness");
                entity.Property(e => e.Faction_ActiveState).HasColumnName("Faction_ActiveState");
                entity.Property(e => e.Faction_PendingState).HasColumnName("Faction_PendingState");
                entity.Property(e => e.Faction_RecoveringState).HasColumnName("Faction_RecoveringState");
                entity.Property(e => e.missions).HasColumnName("missions");
                entity.Property(e => e.explorer).HasColumnName("explorer");
                entity.Property(e => e.voucheer).HasColumnName("voucheer");
                entity.Property(e => e.trade).HasColumnName("trade");
            });
            MoBuilder.Entity<DB_duronese>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.ToTable($"ugc_duronese", DatabaseConfig.Database);
                entity.HasIndex(e => e.id).HasDatabaseName("id");
                entity.Property(e => e.Timestamp).HasColumnName("Timestamp");
                entity.Property(e => e.last_update).HasColumnName("last_update");
                entity.Property(e => e.User_ID).HasColumnName("User_ID");
                entity.Property(e => e.System_ID).HasColumnName("System_ID");
                entity.Property(e => e.Faction_Name).HasColumnName("Faction_Name");
                entity.Property(e => e.Faction_State).HasColumnName("Faction_State");
                entity.Property(e => e.Faction_Government).HasColumnName("Faction_Government");
                entity.Property(e => e.Faction_Influence).HasColumnName("Faction_Influence");
                entity.Property(e => e.Faction_Influence_change).HasColumnName("Faction_Influence_change");
                entity.Property(e => e.Faction_Allegiance).HasColumnName("Faction_Allegiance");
                entity.Property(e => e.Faction_Happiness).HasColumnName("Faction_Happiness");
                entity.Property(e => e.Faction_ActiveState).HasColumnName("Faction_ActiveState");
                entity.Property(e => e.Faction_PendingState).HasColumnName("Faction_PendingState");
                entity.Property(e => e.Faction_RecoveringState).HasColumnName("Faction_RecoveringState");
                entity.Property(e => e.missions).HasColumnName("missions");
                entity.Property(e => e.explorer).HasColumnName("explorer");
                entity.Property(e => e.voucheer).HasColumnName("voucheer");
                entity.Property(e => e.trade).HasColumnName("trade");
            });
            MoBuilder.Entity<DB_hip_2747>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.ToTable($"ugc_hip 2747", DatabaseConfig.Database);
                entity.HasIndex(e => e.id).HasDatabaseName("id");
                entity.Property(e => e.Timestamp).HasColumnName("Timestamp");
                entity.Property(e => e.last_update).HasColumnName("last_update");
                entity.Property(e => e.User_ID).HasColumnName("User_ID");
                entity.Property(e => e.System_ID).HasColumnName("System_ID");
                entity.Property(e => e.Faction_Name).HasColumnName("Faction_Name");
                entity.Property(e => e.Faction_State).HasColumnName("Faction_State");
                entity.Property(e => e.Faction_Government).HasColumnName("Faction_Government");
                entity.Property(e => e.Faction_Influence).HasColumnName("Faction_Influence");
                entity.Property(e => e.Faction_Influence_change).HasColumnName("Faction_Influence_change");
                entity.Property(e => e.Faction_Allegiance).HasColumnName("Faction_Allegiance");
                entity.Property(e => e.Faction_Happiness).HasColumnName("Faction_Happiness");
                entity.Property(e => e.Faction_ActiveState).HasColumnName("Faction_ActiveState");
                entity.Property(e => e.Faction_PendingState).HasColumnName("Faction_PendingState");
                entity.Property(e => e.Faction_RecoveringState).HasColumnName("Faction_RecoveringState");
                entity.Property(e => e.missions).HasColumnName("missions");
                entity.Property(e => e.explorer).HasColumnName("explorer");
                entity.Property(e => e.voucheer).HasColumnName("voucheer");
                entity.Property(e => e.trade).HasColumnName("trade");
            });
            MoBuilder.Entity<DB_hip_3603>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.ToTable($"ugc_hip 3603", DatabaseConfig.Database);
                entity.HasIndex(e => e.id).HasDatabaseName("id");
                entity.Property(e => e.Timestamp).HasColumnName("Timestamp");
                entity.Property(e => e.last_update).HasColumnName("last_update");
                entity.Property(e => e.User_ID).HasColumnName("User_ID");
                entity.Property(e => e.System_ID).HasColumnName("System_ID");
                entity.Property(e => e.Faction_Name).HasColumnName("Faction_Name");
                entity.Property(e => e.Faction_State).HasColumnName("Faction_State");
                entity.Property(e => e.Faction_Government).HasColumnName("Faction_Government");
                entity.Property(e => e.Faction_Influence).HasColumnName("Faction_Influence");
                entity.Property(e => e.Faction_Influence_change).HasColumnName("Faction_Influence_change");
                entity.Property(e => e.Faction_Allegiance).HasColumnName("Faction_Allegiance");
                entity.Property(e => e.Faction_Happiness).HasColumnName("Faction_Happiness");
                entity.Property(e => e.Faction_ActiveState).HasColumnName("Faction_ActiveState");
                entity.Property(e => e.Faction_PendingState).HasColumnName("Faction_PendingState");
                entity.Property(e => e.Faction_RecoveringState).HasColumnName("Faction_RecoveringState");
                entity.Property(e => e.missions).HasColumnName("missions");
                entity.Property(e => e.explorer).HasColumnName("explorer");
                entity.Property(e => e.voucheer).HasColumnName("voucheer");
                entity.Property(e => e.trade).HasColumnName("trade");
            });
            MoBuilder.Entity<DB_hip_4764>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.ToTable($"ugc_hip 4764", DatabaseConfig.Database);
                entity.HasIndex(e => e.id).HasDatabaseName("id");
                entity.Property(e => e.Timestamp).HasColumnName("Timestamp");
                entity.Property(e => e.last_update).HasColumnName("last_update");
                entity.Property(e => e.User_ID).HasColumnName("User_ID");
                entity.Property(e => e.System_ID).HasColumnName("System_ID");
                entity.Property(e => e.Faction_Name).HasColumnName("Faction_Name");
                entity.Property(e => e.Faction_State).HasColumnName("Faction_State");
                entity.Property(e => e.Faction_Government).HasColumnName("Faction_Government");
                entity.Property(e => e.Faction_Influence).HasColumnName("Faction_Influence");
                entity.Property(e => e.Faction_Influence_change).HasColumnName("Faction_Influence_change");
                entity.Property(e => e.Faction_Allegiance).HasColumnName("Faction_Allegiance");
                entity.Property(e => e.Faction_Happiness).HasColumnName("Faction_Happiness");
                entity.Property(e => e.Faction_ActiveState).HasColumnName("Faction_ActiveState");
                entity.Property(e => e.Faction_PendingState).HasColumnName("Faction_PendingState");
                entity.Property(e => e.Faction_RecoveringState).HasColumnName("Faction_RecoveringState");
                entity.Property(e => e.missions).HasColumnName("missions");
                entity.Property(e => e.explorer).HasColumnName("explorer");
                entity.Property(e => e.voucheer).HasColumnName("voucheer");
                entity.Property(e => e.trade).HasColumnName("trade");
            });
            MoBuilder.Entity<DB_hip_4964>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.ToTable($"ugc_hip 4964", DatabaseConfig.Database);
                entity.HasIndex(e => e.id).HasDatabaseName("id");
                entity.Property(e => e.Timestamp).HasColumnName("Timestamp");
                entity.Property(e => e.last_update).HasColumnName("last_update");
                entity.Property(e => e.User_ID).HasColumnName("User_ID");
                entity.Property(e => e.System_ID).HasColumnName("System_ID");
                entity.Property(e => e.Faction_Name).HasColumnName("Faction_Name");
                entity.Property(e => e.Faction_State).HasColumnName("Faction_State");
                entity.Property(e => e.Faction_Government).HasColumnName("Faction_Government");
                entity.Property(e => e.Faction_Influence).HasColumnName("Faction_Influence");
                entity.Property(e => e.Faction_Influence_change).HasColumnName("Faction_Influence_change");
                entity.Property(e => e.Faction_Allegiance).HasColumnName("Faction_Allegiance");
                entity.Property(e => e.Faction_Happiness).HasColumnName("Faction_Happiness");
                entity.Property(e => e.Faction_ActiveState).HasColumnName("Faction_ActiveState");
                entity.Property(e => e.Faction_PendingState).HasColumnName("Faction_PendingState");
                entity.Property(e => e.Faction_RecoveringState).HasColumnName("Faction_RecoveringState");
                entity.Property(e => e.missions).HasColumnName("missions");
                entity.Property(e => e.explorer).HasColumnName("explorer");
                entity.Property(e => e.voucheer).HasColumnName("voucheer");
                entity.Property(e => e.trade).HasColumnName("trade");
            });
            MoBuilder.Entity<DB_hip_5099>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.ToTable($"ugc_hip 5099", DatabaseConfig.Database);
                entity.HasIndex(e => e.id).HasDatabaseName("id");
                entity.Property(e => e.Timestamp).HasColumnName("Timestamp");
                entity.Property(e => e.last_update).HasColumnName("last_update");
                entity.Property(e => e.User_ID).HasColumnName("User_ID");
                entity.Property(e => e.System_ID).HasColumnName("System_ID");
                entity.Property(e => e.Faction_Name).HasColumnName("Faction_Name");
                entity.Property(e => e.Faction_State).HasColumnName("Faction_State");
                entity.Property(e => e.Faction_Government).HasColumnName("Faction_Government");
                entity.Property(e => e.Faction_Influence).HasColumnName("Faction_Influence");
                entity.Property(e => e.Faction_Influence_change).HasColumnName("Faction_Influence_change");
                entity.Property(e => e.Faction_Allegiance).HasColumnName("Faction_Allegiance");
                entity.Property(e => e.Faction_Happiness).HasColumnName("Faction_Happiness");
                entity.Property(e => e.Faction_ActiveState).HasColumnName("Faction_ActiveState");
                entity.Property(e => e.Faction_PendingState).HasColumnName("Faction_PendingState");
                entity.Property(e => e.Faction_RecoveringState).HasColumnName("Faction_RecoveringState");
                entity.Property(e => e.missions).HasColumnName("missions");
                entity.Property(e => e.explorer).HasColumnName("explorer");
                entity.Property(e => e.voucheer).HasColumnName("voucheer");
                entity.Property(e => e.trade).HasColumnName("trade");
            });
            MoBuilder.Entity<DB_hyperborea>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.ToTable($"ugc_hyperborea", DatabaseConfig.Database);
                entity.HasIndex(e => e.id).HasDatabaseName("id");
                entity.Property(e => e.Timestamp).HasColumnName("Timestamp");
                entity.Property(e => e.last_update).HasColumnName("last_update");
                entity.Property(e => e.User_ID).HasColumnName("User_ID");
                entity.Property(e => e.System_ID).HasColumnName("System_ID");
                entity.Property(e => e.Faction_Name).HasColumnName("Faction_Name");
                entity.Property(e => e.Faction_State).HasColumnName("Faction_State");
                entity.Property(e => e.Faction_Government).HasColumnName("Faction_Government");
                entity.Property(e => e.Faction_Influence).HasColumnName("Faction_Influence");
                entity.Property(e => e.Faction_Influence_change).HasColumnName("Faction_Influence_change");
                entity.Property(e => e.Faction_Allegiance).HasColumnName("Faction_Allegiance");
                entity.Property(e => e.Faction_Happiness).HasColumnName("Faction_Happiness");
                entity.Property(e => e.Faction_ActiveState).HasColumnName("Faction_ActiveState");
                entity.Property(e => e.Faction_PendingState).HasColumnName("Faction_PendingState");
                entity.Property(e => e.Faction_RecoveringState).HasColumnName("Faction_RecoveringState");
                entity.Property(e => e.missions).HasColumnName("missions");
                entity.Property(e => e.explorer).HasColumnName("explorer");
                entity.Property(e => e.voucheer).HasColumnName("voucheer");
                entity.Property(e => e.trade).HasColumnName("trade");
            });
            MoBuilder.Entity<DB_kartamayana>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.ToTable($"ugc_kartamayana", DatabaseConfig.Database);
                entity.HasIndex(e => e.id).HasDatabaseName("id");
                entity.Property(e => e.Timestamp).HasColumnName("Timestamp");
                entity.Property(e => e.last_update).HasColumnName("last_update");
                entity.Property(e => e.User_ID).HasColumnName("User_ID");
                entity.Property(e => e.System_ID).HasColumnName("System_ID");
                entity.Property(e => e.Faction_Name).HasColumnName("Faction_Name");
                entity.Property(e => e.Faction_State).HasColumnName("Faction_State");
                entity.Property(e => e.Faction_Government).HasColumnName("Faction_Government");
                entity.Property(e => e.Faction_Influence).HasColumnName("Faction_Influence");
                entity.Property(e => e.Faction_Influence_change).HasColumnName("Faction_Influence_change");
                entity.Property(e => e.Faction_Allegiance).HasColumnName("Faction_Allegiance");
                entity.Property(e => e.Faction_Happiness).HasColumnName("Faction_Happiness");
                entity.Property(e => e.Faction_ActiveState).HasColumnName("Faction_ActiveState");
                entity.Property(e => e.Faction_PendingState).HasColumnName("Faction_PendingState");
                entity.Property(e => e.Faction_RecoveringState).HasColumnName("Faction_RecoveringState");
                entity.Property(e => e.missions).HasColumnName("missions");
                entity.Property(e => e.explorer).HasColumnName("explorer");
                entity.Property(e => e.voucheer).HasColumnName("voucheer");
                entity.Property(e => e.trade).HasColumnName("trade");
            });
            MoBuilder.Entity<DB_khampti>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.ToTable($"ugc_khampti", DatabaseConfig.Database);
                entity.HasIndex(e => e.id).HasDatabaseName("id");
                entity.Property(e => e.Timestamp).HasColumnName("Timestamp");
                entity.Property(e => e.last_update).HasColumnName("last_update");
                entity.Property(e => e.User_ID).HasColumnName("User_ID");
                entity.Property(e => e.System_ID).HasColumnName("System_ID");
                entity.Property(e => e.Faction_Name).HasColumnName("Faction_Name");
                entity.Property(e => e.Faction_State).HasColumnName("Faction_State");
                entity.Property(e => e.Faction_Government).HasColumnName("Faction_Government");
                entity.Property(e => e.Faction_Influence).HasColumnName("Faction_Influence");
                entity.Property(e => e.Faction_Influence_change).HasColumnName("Faction_Influence_change");
                entity.Property(e => e.Faction_Allegiance).HasColumnName("Faction_Allegiance");
                entity.Property(e => e.Faction_Happiness).HasColumnName("Faction_Happiness");
                entity.Property(e => e.Faction_ActiveState).HasColumnName("Faction_ActiveState");
                entity.Property(e => e.Faction_PendingState).HasColumnName("Faction_PendingState");
                entity.Property(e => e.Faction_RecoveringState).HasColumnName("Faction_RecoveringState");
                entity.Property(e => e.missions).HasColumnName("missions");
                entity.Property(e => e.explorer).HasColumnName("explorer");
                entity.Property(e => e.voucheer).HasColumnName("voucheer");
                entity.Property(e => e.trade).HasColumnName("trade");
            });
            MoBuilder.Entity<DB_kharpulo>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.ToTable($"ugc_kharpulo", DatabaseConfig.Database);
                entity.HasIndex(e => e.id).HasDatabaseName("id");
                entity.Property(e => e.Timestamp).HasColumnName("Timestamp");
                entity.Property(e => e.last_update).HasColumnName("last_update");
                entity.Property(e => e.User_ID).HasColumnName("User_ID");
                entity.Property(e => e.System_ID).HasColumnName("System_ID");
                entity.Property(e => e.Faction_Name).HasColumnName("Faction_Name");
                entity.Property(e => e.Faction_State).HasColumnName("Faction_State");
                entity.Property(e => e.Faction_Government).HasColumnName("Faction_Government");
                entity.Property(e => e.Faction_Influence).HasColumnName("Faction_Influence");
                entity.Property(e => e.Faction_Influence_change).HasColumnName("Faction_Influence_change");
                entity.Property(e => e.Faction_Allegiance).HasColumnName("Faction_Allegiance");
                entity.Property(e => e.Faction_Happiness).HasColumnName("Faction_Happiness");
                entity.Property(e => e.Faction_ActiveState).HasColumnName("Faction_ActiveState");
                entity.Property(e => e.Faction_PendingState).HasColumnName("Faction_PendingState");
                entity.Property(e => e.Faction_RecoveringState).HasColumnName("Faction_RecoveringState");
                entity.Property(e => e.missions).HasColumnName("missions");
                entity.Property(e => e.explorer).HasColumnName("explorer");
                entity.Property(e => e.voucheer).HasColumnName("voucheer");
                entity.Property(e => e.trade).HasColumnName("trade");
            });
            MoBuilder.Entity<DB_kunggalerni>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.ToTable($"ugc_kunggalerni", DatabaseConfig.Database);
                entity.HasIndex(e => e.id).HasDatabaseName("id");
                entity.Property(e => e.Timestamp).HasColumnName("Timestamp");
                entity.Property(e => e.last_update).HasColumnName("last_update");
                entity.Property(e => e.User_ID).HasColumnName("User_ID");
                entity.Property(e => e.System_ID).HasColumnName("System_ID");
                entity.Property(e => e.Faction_Name).HasColumnName("Faction_Name");
                entity.Property(e => e.Faction_State).HasColumnName("Faction_State");
                entity.Property(e => e.Faction_Government).HasColumnName("Faction_Government");
                entity.Property(e => e.Faction_Influence).HasColumnName("Faction_Influence");
                entity.Property(e => e.Faction_Influence_change).HasColumnName("Faction_Influence_change");
                entity.Property(e => e.Faction_Allegiance).HasColumnName("Faction_Allegiance");
                entity.Property(e => e.Faction_Happiness).HasColumnName("Faction_Happiness");
                entity.Property(e => e.Faction_ActiveState).HasColumnName("Faction_ActiveState");
                entity.Property(e => e.Faction_PendingState).HasColumnName("Faction_PendingState");
                entity.Property(e => e.Faction_RecoveringState).HasColumnName("Faction_RecoveringState");
                entity.Property(e => e.missions).HasColumnName("missions");
                entity.Property(e => e.explorer).HasColumnName("explorer");
                entity.Property(e => e.voucheer).HasColumnName("voucheer");
                entity.Property(e => e.trade).HasColumnName("trade");
            });
            MoBuilder.Entity<DB_liu_di>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.ToTable($"ugc_liu di", DatabaseConfig.Database);
                entity.HasIndex(e => e.id).HasDatabaseName("id");
                entity.Property(e => e.Timestamp).HasColumnName("Timestamp");
                entity.Property(e => e.last_update).HasColumnName("last_update");
                entity.Property(e => e.User_ID).HasColumnName("User_ID");
                entity.Property(e => e.System_ID).HasColumnName("System_ID");
                entity.Property(e => e.Faction_Name).HasColumnName("Faction_Name");
                entity.Property(e => e.Faction_State).HasColumnName("Faction_State");
                entity.Property(e => e.Faction_Government).HasColumnName("Faction_Government");
                entity.Property(e => e.Faction_Influence).HasColumnName("Faction_Influence");
                entity.Property(e => e.Faction_Influence_change).HasColumnName("Faction_Influence_change");
                entity.Property(e => e.Faction_Allegiance).HasColumnName("Faction_Allegiance");
                entity.Property(e => e.Faction_Happiness).HasColumnName("Faction_Happiness");
                entity.Property(e => e.Faction_ActiveState).HasColumnName("Faction_ActiveState");
                entity.Property(e => e.Faction_PendingState).HasColumnName("Faction_PendingState");
                entity.Property(e => e.Faction_RecoveringState).HasColumnName("Faction_RecoveringState");
                entity.Property(e => e.missions).HasColumnName("missions");
                entity.Property(e => e.explorer).HasColumnName("explorer");
                entity.Property(e => e.voucheer).HasColumnName("voucheer");
                entity.Property(e => e.trade).HasColumnName("trade");
            });
            MoBuilder.Entity<DB_ltt_518>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.ToTable($"ugc_ltt 518", DatabaseConfig.Database);
                entity.HasIndex(e => e.id).HasDatabaseName("id");
                entity.Property(e => e.Timestamp).HasColumnName("Timestamp");
                entity.Property(e => e.last_update).HasColumnName("last_update");
                entity.Property(e => e.User_ID).HasColumnName("User_ID");
                entity.Property(e => e.System_ID).HasColumnName("System_ID");
                entity.Property(e => e.Faction_Name).HasColumnName("Faction_Name");
                entity.Property(e => e.Faction_State).HasColumnName("Faction_State");
                entity.Property(e => e.Faction_Government).HasColumnName("Faction_Government");
                entity.Property(e => e.Faction_Influence).HasColumnName("Faction_Influence");
                entity.Property(e => e.Faction_Influence_change).HasColumnName("Faction_Influence_change");
                entity.Property(e => e.Faction_Allegiance).HasColumnName("Faction_Allegiance");
                entity.Property(e => e.Faction_Happiness).HasColumnName("Faction_Happiness");
                entity.Property(e => e.Faction_ActiveState).HasColumnName("Faction_ActiveState");
                entity.Property(e => e.Faction_PendingState).HasColumnName("Faction_PendingState");
                entity.Property(e => e.Faction_RecoveringState).HasColumnName("Faction_RecoveringState");
                entity.Property(e => e.missions).HasColumnName("missions");
                entity.Property(e => e.explorer).HasColumnName("explorer");
                entity.Property(e => e.voucheer).HasColumnName("voucheer");
                entity.Property(e => e.trade).HasColumnName("trade");
            });
            MoBuilder.Entity<DB_ltt_874>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.ToTable($"ugc_ltt 874", DatabaseConfig.Database);
                entity.HasIndex(e => e.id).HasDatabaseName("id");
                entity.Property(e => e.Timestamp).HasColumnName("Timestamp");
                entity.Property(e => e.last_update).HasColumnName("last_update");
                entity.Property(e => e.User_ID).HasColumnName("User_ID");
                entity.Property(e => e.System_ID).HasColumnName("System_ID");
                entity.Property(e => e.Faction_Name).HasColumnName("Faction_Name");
                entity.Property(e => e.Faction_State).HasColumnName("Faction_State");
                entity.Property(e => e.Faction_Government).HasColumnName("Faction_Government");
                entity.Property(e => e.Faction_Influence).HasColumnName("Faction_Influence");
                entity.Property(e => e.Faction_Influence_change).HasColumnName("Faction_Influence_change");
                entity.Property(e => e.Faction_Allegiance).HasColumnName("Faction_Allegiance");
                entity.Property(e => e.Faction_Happiness).HasColumnName("Faction_Happiness");
                entity.Property(e => e.Faction_ActiveState).HasColumnName("Faction_ActiveState");
                entity.Property(e => e.Faction_PendingState).HasColumnName("Faction_PendingState");
                entity.Property(e => e.Faction_RecoveringState).HasColumnName("Faction_RecoveringState");
                entity.Property(e => e.missions).HasColumnName("missions");
                entity.Property(e => e.explorer).HasColumnName("explorer");
                entity.Property(e => e.voucheer).HasColumnName("voucheer");
                entity.Property(e => e.trade).HasColumnName("trade");
            });
            MoBuilder.Entity<DB_maidubrigel>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.ToTable($"ugc_maidubrigel", DatabaseConfig.Database);
                entity.HasIndex(e => e.id).HasDatabaseName("id");
                entity.Property(e => e.Timestamp).HasColumnName("Timestamp");
                entity.Property(e => e.last_update).HasColumnName("last_update");
                entity.Property(e => e.User_ID).HasColumnName("User_ID");
                entity.Property(e => e.System_ID).HasColumnName("System_ID");
                entity.Property(e => e.Faction_Name).HasColumnName("Faction_Name");
                entity.Property(e => e.Faction_State).HasColumnName("Faction_State");
                entity.Property(e => e.Faction_Government).HasColumnName("Faction_Government");
                entity.Property(e => e.Faction_Influence).HasColumnName("Faction_Influence");
                entity.Property(e => e.Faction_Influence_change).HasColumnName("Faction_Influence_change");
                entity.Property(e => e.Faction_Allegiance).HasColumnName("Faction_Allegiance");
                entity.Property(e => e.Faction_Happiness).HasColumnName("Faction_Happiness");
                entity.Property(e => e.Faction_ActiveState).HasColumnName("Faction_ActiveState");
                entity.Property(e => e.Faction_PendingState).HasColumnName("Faction_PendingState");
                entity.Property(e => e.Faction_RecoveringState).HasColumnName("Faction_RecoveringState");
                entity.Property(e => e.missions).HasColumnName("missions");
                entity.Property(e => e.explorer).HasColumnName("explorer");
                entity.Property(e => e.voucheer).HasColumnName("voucheer");
                entity.Property(e => e.trade).HasColumnName("trade");
            });
            MoBuilder.Entity<DB_minanes>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.ToTable($"ugc_minanes", DatabaseConfig.Database);
                entity.HasIndex(e => e.id).HasDatabaseName("id");
                entity.Property(e => e.Timestamp).HasColumnName("Timestamp");
                entity.Property(e => e.last_update).HasColumnName("last_update");
                entity.Property(e => e.User_ID).HasColumnName("User_ID");
                entity.Property(e => e.System_ID).HasColumnName("System_ID");
                entity.Property(e => e.Faction_Name).HasColumnName("Faction_Name");
                entity.Property(e => e.Faction_State).HasColumnName("Faction_State");
                entity.Property(e => e.Faction_Government).HasColumnName("Faction_Government");
                entity.Property(e => e.Faction_Influence).HasColumnName("Faction_Influence");
                entity.Property(e => e.Faction_Influence_change).HasColumnName("Faction_Influence_change");
                entity.Property(e => e.Faction_Allegiance).HasColumnName("Faction_Allegiance");
                entity.Property(e => e.Faction_Happiness).HasColumnName("Faction_Happiness");
                entity.Property(e => e.Faction_ActiveState).HasColumnName("Faction_ActiveState");
                entity.Property(e => e.Faction_PendingState).HasColumnName("Faction_PendingState");
                entity.Property(e => e.Faction_RecoveringState).HasColumnName("Faction_RecoveringState");
                entity.Property(e => e.missions).HasColumnName("missions");
                entity.Property(e => e.explorer).HasColumnName("explorer");
                entity.Property(e => e.voucheer).HasColumnName("voucheer");
                entity.Property(e => e.trade).HasColumnName("trade");
            });
            MoBuilder.Entity<DB_nayanezgani>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.ToTable($"ugc_nayanezgani", DatabaseConfig.Database);
                entity.HasIndex(e => e.id).HasDatabaseName("id");
                entity.Property(e => e.Timestamp).HasColumnName("Timestamp");
                entity.Property(e => e.last_update).HasColumnName("last_update");
                entity.Property(e => e.User_ID).HasColumnName("User_ID");
                entity.Property(e => e.System_ID).HasColumnName("System_ID");
                entity.Property(e => e.Faction_Name).HasColumnName("Faction_Name");
                entity.Property(e => e.Faction_State).HasColumnName("Faction_State");
                entity.Property(e => e.Faction_Government).HasColumnName("Faction_Government");
                entity.Property(e => e.Faction_Influence).HasColumnName("Faction_Influence");
                entity.Property(e => e.Faction_Influence_change).HasColumnName("Faction_Influence_change");
                entity.Property(e => e.Faction_Allegiance).HasColumnName("Faction_Allegiance");
                entity.Property(e => e.Faction_Happiness).HasColumnName("Faction_Happiness");
                entity.Property(e => e.Faction_ActiveState).HasColumnName("Faction_ActiveState");
                entity.Property(e => e.Faction_PendingState).HasColumnName("Faction_PendingState");
                entity.Property(e => e.Faction_RecoveringState).HasColumnName("Faction_RecoveringState");
                entity.Property(e => e.missions).HasColumnName("missions");
                entity.Property(e => e.explorer).HasColumnName("explorer");
                entity.Property(e => e.voucheer).HasColumnName("voucheer");
                entity.Property(e => e.trade).HasColumnName("trade");
            });
            MoBuilder.Entity<DB_niflhel>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.ToTable($"ugc_niflhel", DatabaseConfig.Database);
                entity.HasIndex(e => e.id).HasDatabaseName("id");
                entity.Property(e => e.Timestamp).HasColumnName("Timestamp");
                entity.Property(e => e.last_update).HasColumnName("last_update");
                entity.Property(e => e.User_ID).HasColumnName("User_ID");
                entity.Property(e => e.System_ID).HasColumnName("System_ID");
                entity.Property(e => e.Faction_Name).HasColumnName("Faction_Name");
                entity.Property(e => e.Faction_State).HasColumnName("Faction_State");
                entity.Property(e => e.Faction_Government).HasColumnName("Faction_Government");
                entity.Property(e => e.Faction_Influence).HasColumnName("Faction_Influence");
                entity.Property(e => e.Faction_Influence_change).HasColumnName("Faction_Influence_change");
                entity.Property(e => e.Faction_Allegiance).HasColumnName("Faction_Allegiance");
                entity.Property(e => e.Faction_Happiness).HasColumnName("Faction_Happiness");
                entity.Property(e => e.Faction_ActiveState).HasColumnName("Faction_ActiveState");
                entity.Property(e => e.Faction_PendingState).HasColumnName("Faction_PendingState");
                entity.Property(e => e.Faction_RecoveringState).HasColumnName("Faction_RecoveringState");
                entity.Property(e => e.missions).HasColumnName("missions");
                entity.Property(e => e.explorer).HasColumnName("explorer");
                entity.Property(e => e.voucheer).HasColumnName("voucheer");
                entity.Property(e => e.trade).HasColumnName("trade");
            });
            MoBuilder.Entity<DB_nltt_2682>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.ToTable($"ugc_nltt 2682", DatabaseConfig.Database);
                entity.HasIndex(e => e.id).HasDatabaseName("id");
                entity.Property(e => e.Timestamp).HasColumnName("Timestamp");
                entity.Property(e => e.last_update).HasColumnName("last_update");
                entity.Property(e => e.User_ID).HasColumnName("User_ID");
                entity.Property(e => e.System_ID).HasColumnName("System_ID");
                entity.Property(e => e.Faction_Name).HasColumnName("Faction_Name");
                entity.Property(e => e.Faction_State).HasColumnName("Faction_State");
                entity.Property(e => e.Faction_Government).HasColumnName("Faction_Government");
                entity.Property(e => e.Faction_Influence).HasColumnName("Faction_Influence");
                entity.Property(e => e.Faction_Influence_change).HasColumnName("Faction_Influence_change");
                entity.Property(e => e.Faction_Allegiance).HasColumnName("Faction_Allegiance");
                entity.Property(e => e.Faction_Happiness).HasColumnName("Faction_Happiness");
                entity.Property(e => e.Faction_ActiveState).HasColumnName("Faction_ActiveState");
                entity.Property(e => e.Faction_PendingState).HasColumnName("Faction_PendingState");
                entity.Property(e => e.Faction_RecoveringState).HasColumnName("Faction_RecoveringState");
                entity.Property(e => e.missions).HasColumnName("missions");
                entity.Property(e => e.explorer).HasColumnName("explorer");
                entity.Property(e => e.voucheer).HasColumnName("voucheer");
                entity.Property(e => e.trade).HasColumnName("trade");
            });
            MoBuilder.Entity<DB_paras>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.ToTable($"ugc_paras", DatabaseConfig.Database);
                entity.HasIndex(e => e.id).HasDatabaseName("id");
                entity.Property(e => e.Timestamp).HasColumnName("Timestamp");
                entity.Property(e => e.last_update).HasColumnName("last_update");
                entity.Property(e => e.User_ID).HasColumnName("User_ID");
                entity.Property(e => e.System_ID).HasColumnName("System_ID");
                entity.Property(e => e.Faction_Name).HasColumnName("Faction_Name");
                entity.Property(e => e.Faction_State).HasColumnName("Faction_State");
                entity.Property(e => e.Faction_Government).HasColumnName("Faction_Government");
                entity.Property(e => e.Faction_Influence).HasColumnName("Faction_Influence");
                entity.Property(e => e.Faction_Influence_change).HasColumnName("Faction_Influence_change");
                entity.Property(e => e.Faction_Allegiance).HasColumnName("Faction_Allegiance");
                entity.Property(e => e.Faction_Happiness).HasColumnName("Faction_Happiness");
                entity.Property(e => e.Faction_ActiveState).HasColumnName("Faction_ActiveState");
                entity.Property(e => e.Faction_PendingState).HasColumnName("Faction_PendingState");
                entity.Property(e => e.Faction_RecoveringState).HasColumnName("Faction_RecoveringState");
                entity.Property(e => e.missions).HasColumnName("missions");
                entity.Property(e => e.explorer).HasColumnName("explorer");
                entity.Property(e => e.voucheer).HasColumnName("voucheer");
                entity.Property(e => e.trade).HasColumnName("trade");
            });
            MoBuilder.Entity<DB_piperish>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.ToTable($"ugc_piperish", DatabaseConfig.Database);
                entity.HasIndex(e => e.id).HasDatabaseName("id");
                entity.Property(e => e.Timestamp).HasColumnName("Timestamp");
                entity.Property(e => e.last_update).HasColumnName("last_update");
                entity.Property(e => e.User_ID).HasColumnName("User_ID");
                entity.Property(e => e.System_ID).HasColumnName("System_ID");
                entity.Property(e => e.Faction_Name).HasColumnName("Faction_Name");
                entity.Property(e => e.Faction_State).HasColumnName("Faction_State");
                entity.Property(e => e.Faction_Government).HasColumnName("Faction_Government");
                entity.Property(e => e.Faction_Influence).HasColumnName("Faction_Influence");
                entity.Property(e => e.Faction_Influence_change).HasColumnName("Faction_Influence_change");
                entity.Property(e => e.Faction_Allegiance).HasColumnName("Faction_Allegiance");
                entity.Property(e => e.Faction_Happiness).HasColumnName("Faction_Happiness");
                entity.Property(e => e.Faction_ActiveState).HasColumnName("Faction_ActiveState");
                entity.Property(e => e.Faction_PendingState).HasColumnName("Faction_PendingState");
                entity.Property(e => e.Faction_RecoveringState).HasColumnName("Faction_RecoveringState");
                entity.Property(e => e.missions).HasColumnName("missions");
                entity.Property(e => e.explorer).HasColumnName("explorer");
                entity.Property(e => e.voucheer).HasColumnName("voucheer");
                entity.Property(e => e.trade).HasColumnName("trade");
            });
            MoBuilder.Entity<DB_rosmerta>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.ToTable($"ugc_rosmerta", DatabaseConfig.Database);
                entity.HasIndex(e => e.id).HasDatabaseName("id");
                entity.Property(e => e.Timestamp).HasColumnName("Timestamp");
                entity.Property(e => e.last_update).HasColumnName("last_update");
                entity.Property(e => e.User_ID).HasColumnName("User_ID");
                entity.Property(e => e.System_ID).HasColumnName("System_ID");
                entity.Property(e => e.Faction_Name).HasColumnName("Faction_Name");
                entity.Property(e => e.Faction_State).HasColumnName("Faction_State");
                entity.Property(e => e.Faction_Government).HasColumnName("Faction_Government");
                entity.Property(e => e.Faction_Influence).HasColumnName("Faction_Influence");
                entity.Property(e => e.Faction_Influence_change).HasColumnName("Faction_Influence_change");
                entity.Property(e => e.Faction_Allegiance).HasColumnName("Faction_Allegiance");
                entity.Property(e => e.Faction_Happiness).HasColumnName("Faction_Happiness");
                entity.Property(e => e.Faction_ActiveState).HasColumnName("Faction_ActiveState");
                entity.Property(e => e.Faction_PendingState).HasColumnName("Faction_PendingState");
                entity.Property(e => e.Faction_RecoveringState).HasColumnName("Faction_RecoveringState");
                entity.Property(e => e.missions).HasColumnName("missions");
                entity.Property(e => e.explorer).HasColumnName("explorer");
                entity.Property(e => e.voucheer).HasColumnName("voucheer");
                entity.Property(e => e.trade).HasColumnName("trade");
            });
            MoBuilder.Entity<DB_runo>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.ToTable($"ugc_runo", DatabaseConfig.Database);
                entity.HasIndex(e => e.id).HasDatabaseName("id");
                entity.Property(e => e.Timestamp).HasColumnName("Timestamp");
                entity.Property(e => e.last_update).HasColumnName("last_update");
                entity.Property(e => e.User_ID).HasColumnName("User_ID");
                entity.Property(e => e.System_ID).HasColumnName("System_ID");
                entity.Property(e => e.Faction_Name).HasColumnName("Faction_Name");
                entity.Property(e => e.Faction_State).HasColumnName("Faction_State");
                entity.Property(e => e.Faction_Government).HasColumnName("Faction_Government");
                entity.Property(e => e.Faction_Influence).HasColumnName("Faction_Influence");
                entity.Property(e => e.Faction_Influence_change).HasColumnName("Faction_Influence_change");
                entity.Property(e => e.Faction_Allegiance).HasColumnName("Faction_Allegiance");
                entity.Property(e => e.Faction_Happiness).HasColumnName("Faction_Happiness");
                entity.Property(e => e.Faction_ActiveState).HasColumnName("Faction_ActiveState");
                entity.Property(e => e.Faction_PendingState).HasColumnName("Faction_PendingState");
                entity.Property(e => e.Faction_RecoveringState).HasColumnName("Faction_RecoveringState");
                entity.Property(e => e.missions).HasColumnName("missions");
                entity.Property(e => e.explorer).HasColumnName("explorer");
                entity.Property(e => e.voucheer).HasColumnName("voucheer");
                entity.Property(e => e.trade).HasColumnName("trade");
            });
            MoBuilder.Entity<DB_sadhbh>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.ToTable($"ugc_sadhbh", DatabaseConfig.Database);
                entity.HasIndex(e => e.id).HasDatabaseName("id");
                entity.Property(e => e.Timestamp).HasColumnName("Timestamp");
                entity.Property(e => e.last_update).HasColumnName("last_update");
                entity.Property(e => e.User_ID).HasColumnName("User_ID");
                entity.Property(e => e.System_ID).HasColumnName("System_ID");
                entity.Property(e => e.Faction_Name).HasColumnName("Faction_Name");
                entity.Property(e => e.Faction_State).HasColumnName("Faction_State");
                entity.Property(e => e.Faction_Government).HasColumnName("Faction_Government");
                entity.Property(e => e.Faction_Influence).HasColumnName("Faction_Influence");
                entity.Property(e => e.Faction_Influence_change).HasColumnName("Faction_Influence_change");
                entity.Property(e => e.Faction_Allegiance).HasColumnName("Faction_Allegiance");
                entity.Property(e => e.Faction_Happiness).HasColumnName("Faction_Happiness");
                entity.Property(e => e.Faction_ActiveState).HasColumnName("Faction_ActiveState");
                entity.Property(e => e.Faction_PendingState).HasColumnName("Faction_PendingState");
                entity.Property(e => e.Faction_RecoveringState).HasColumnName("Faction_RecoveringState");
                entity.Property(e => e.missions).HasColumnName("missions");
                entity.Property(e => e.explorer).HasColumnName("explorer");
                entity.Property(e => e.voucheer).HasColumnName("voucheer");
                entity.Property(e => e.trade).HasColumnName("trade");
            });
            MoBuilder.Entity<DB_slatas>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.ToTable($"ugc_slatas", DatabaseConfig.Database);
                entity.HasIndex(e => e.id).HasDatabaseName("id");
                entity.Property(e => e.Timestamp).HasColumnName("Timestamp");
                entity.Property(e => e.last_update).HasColumnName("last_update");
                entity.Property(e => e.User_ID).HasColumnName("User_ID");
                entity.Property(e => e.System_ID).HasColumnName("System_ID");
                entity.Property(e => e.Faction_Name).HasColumnName("Faction_Name");
                entity.Property(e => e.Faction_State).HasColumnName("Faction_State");
                entity.Property(e => e.Faction_Government).HasColumnName("Faction_Government");
                entity.Property(e => e.Faction_Influence).HasColumnName("Faction_Influence");
                entity.Property(e => e.Faction_Influence_change).HasColumnName("Faction_Influence_change");
                entity.Property(e => e.Faction_Allegiance).HasColumnName("Faction_Allegiance");
                entity.Property(e => e.Faction_Happiness).HasColumnName("Faction_Happiness");
                entity.Property(e => e.Faction_ActiveState).HasColumnName("Faction_ActiveState");
                entity.Property(e => e.Faction_PendingState).HasColumnName("Faction_PendingState");
                entity.Property(e => e.Faction_RecoveringState).HasColumnName("Faction_RecoveringState");
                entity.Property(e => e.missions).HasColumnName("missions");
                entity.Property(e => e.explorer).HasColumnName("explorer");
                entity.Property(e => e.voucheer).HasColumnName("voucheer");
                entity.Property(e => e.trade).HasColumnName("trade");
            });
            MoBuilder.Entity<DB_tetela>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.ToTable($"ugc_tetela", DatabaseConfig.Database);
                entity.HasIndex(e => e.id).HasDatabaseName("id");
                entity.Property(e => e.Timestamp).HasColumnName("Timestamp");
                entity.Property(e => e.last_update).HasColumnName("last_update");
                entity.Property(e => e.User_ID).HasColumnName("User_ID");
                entity.Property(e => e.System_ID).HasColumnName("System_ID");
                entity.Property(e => e.Faction_Name).HasColumnName("Faction_Name");
                entity.Property(e => e.Faction_State).HasColumnName("Faction_State");
                entity.Property(e => e.Faction_Government).HasColumnName("Faction_Government");
                entity.Property(e => e.Faction_Influence).HasColumnName("Faction_Influence");
                entity.Property(e => e.Faction_Influence_change).HasColumnName("Faction_Influence_change");
                entity.Property(e => e.Faction_Allegiance).HasColumnName("Faction_Allegiance");
                entity.Property(e => e.Faction_Happiness).HasColumnName("Faction_Happiness");
                entity.Property(e => e.Faction_ActiveState).HasColumnName("Faction_ActiveState");
                entity.Property(e => e.Faction_PendingState).HasColumnName("Faction_PendingState");
                entity.Property(e => e.Faction_RecoveringState).HasColumnName("Faction_RecoveringState");
                entity.Property(e => e.missions).HasColumnName("missions");
                entity.Property(e => e.explorer).HasColumnName("explorer");
                entity.Property(e => e.voucheer).HasColumnName("voucheer");
                entity.Property(e => e.trade).HasColumnName("trade");
            });
            MoBuilder.Entity<DB_tocorii>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.ToTable($"ugc_tocorii", DatabaseConfig.Database);
                entity.HasIndex(e => e.id).HasDatabaseName("id");
                entity.Property(e => e.Timestamp).HasColumnName("Timestamp");
                entity.Property(e => e.last_update).HasColumnName("last_update");
                entity.Property(e => e.User_ID).HasColumnName("User_ID");
                entity.Property(e => e.System_ID).HasColumnName("System_ID");
                entity.Property(e => e.Faction_Name).HasColumnName("Faction_Name");
                entity.Property(e => e.Faction_State).HasColumnName("Faction_State");
                entity.Property(e => e.Faction_Government).HasColumnName("Faction_Government");
                entity.Property(e => e.Faction_Influence).HasColumnName("Faction_Influence");
                entity.Property(e => e.Faction_Influence_change).HasColumnName("Faction_Influence_change");
                entity.Property(e => e.Faction_Allegiance).HasColumnName("Faction_Allegiance");
                entity.Property(e => e.Faction_Happiness).HasColumnName("Faction_Happiness");
                entity.Property(e => e.Faction_ActiveState).HasColumnName("Faction_ActiveState");
                entity.Property(e => e.Faction_PendingState).HasColumnName("Faction_PendingState");
                entity.Property(e => e.Faction_RecoveringState).HasColumnName("Faction_RecoveringState");
                entity.Property(e => e.missions).HasColumnName("missions");
                entity.Property(e => e.explorer).HasColumnName("explorer");
                entity.Property(e => e.voucheer).HasColumnName("voucheer");
                entity.Property(e => e.trade).HasColumnName("trade");
            });
            MoBuilder.Entity<DB_wapiya>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.ToTable($"ugc_wapiya", DatabaseConfig.Database);
                entity.HasIndex(e => e.id).HasDatabaseName("id");
                entity.Property(e => e.Timestamp).HasColumnName("Timestamp");
                entity.Property(e => e.last_update).HasColumnName("last_update");
                entity.Property(e => e.User_ID).HasColumnName("User_ID");
                entity.Property(e => e.System_ID).HasColumnName("System_ID");
                entity.Property(e => e.Faction_Name).HasColumnName("Faction_Name");
                entity.Property(e => e.Faction_State).HasColumnName("Faction_State");
                entity.Property(e => e.Faction_Government).HasColumnName("Faction_Government");
                entity.Property(e => e.Faction_Influence).HasColumnName("Faction_Influence");
                entity.Property(e => e.Faction_Influence_change).HasColumnName("Faction_Influence_change");
                entity.Property(e => e.Faction_Allegiance).HasColumnName("Faction_Allegiance");
                entity.Property(e => e.Faction_Happiness).HasColumnName("Faction_Happiness");
                entity.Property(e => e.Faction_ActiveState).HasColumnName("Faction_ActiveState");
                entity.Property(e => e.Faction_PendingState).HasColumnName("Faction_PendingState");
                entity.Property(e => e.Faction_RecoveringState).HasColumnName("Faction_RecoveringState");
                entity.Property(e => e.missions).HasColumnName("missions");
                entity.Property(e => e.explorer).HasColumnName("explorer");
                entity.Property(e => e.voucheer).HasColumnName("voucheer");
                entity.Property(e => e.trade).HasColumnName("trade");
            });
            MoBuilder.Entity<DB_ambiabar>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.ToTable($"ugc_ambiabar", DatabaseConfig.Database);
                entity.HasIndex(e => e.id).HasDatabaseName("id");
                entity.Property(e => e.Timestamp).HasColumnName("Timestamp");
                entity.Property(e => e.last_update).HasColumnName("last_update");
                entity.Property(e => e.User_ID).HasColumnName("User_ID");
                entity.Property(e => e.System_ID).HasColumnName("System_ID");
                entity.Property(e => e.Faction_Name).HasColumnName("Faction_Name");
                entity.Property(e => e.Faction_State).HasColumnName("Faction_State");
                entity.Property(e => e.Faction_Government).HasColumnName("Faction_Government");
                entity.Property(e => e.Faction_Influence).HasColumnName("Faction_Influence");
                entity.Property(e => e.Faction_Influence_change).HasColumnName("Faction_Influence_change");
                entity.Property(e => e.Faction_Allegiance).HasColumnName("Faction_Allegiance");
                entity.Property(e => e.Faction_Happiness).HasColumnName("Faction_Happiness");
                entity.Property(e => e.Faction_ActiveState).HasColumnName("Faction_ActiveState");
                entity.Property(e => e.Faction_PendingState).HasColumnName("Faction_PendingState");
                entity.Property(e => e.Faction_RecoveringState).HasColumnName("Faction_RecoveringState");
                entity.Property(e => e.missions).HasColumnName("missions");
                entity.Property(e => e.explorer).HasColumnName("explorer");
                entity.Property(e => e.voucheer).HasColumnName("voucheer");
                entity.Property(e => e.trade).HasColumnName("trade");
            });
            MoBuilder.Entity<DB_ambiabar>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.ToTable($"ugc_ambiabar", DatabaseConfig.Database);
                entity.HasIndex(e => e.id).HasDatabaseName("id");
                entity.Property(e => e.Timestamp).HasColumnName("Timestamp");
                entity.Property(e => e.last_update).HasColumnName("last_update");
                entity.Property(e => e.User_ID).HasColumnName("User_ID");
                entity.Property(e => e.System_ID).HasColumnName("System_ID");
                entity.Property(e => e.Faction_Name).HasColumnName("Faction_Name");
                entity.Property(e => e.Faction_State).HasColumnName("Faction_State");
                entity.Property(e => e.Faction_Government).HasColumnName("Faction_Government");
                entity.Property(e => e.Faction_Influence).HasColumnName("Faction_Influence");
                entity.Property(e => e.Faction_Influence_change).HasColumnName("Faction_Influence_change");
                entity.Property(e => e.Faction_Allegiance).HasColumnName("Faction_Allegiance");
                entity.Property(e => e.Faction_Happiness).HasColumnName("Faction_Happiness");
                entity.Property(e => e.Faction_ActiveState).HasColumnName("Faction_ActiveState");
                entity.Property(e => e.Faction_PendingState).HasColumnName("Faction_PendingState");
                entity.Property(e => e.Faction_RecoveringState).HasColumnName("Faction_RecoveringState");
                entity.Property(e => e.missions).HasColumnName("missions");
                entity.Property(e => e.explorer).HasColumnName("explorer");
                entity.Property(e => e.voucheer).HasColumnName("voucheer");
                entity.Property(e => e.trade).HasColumnName("trade");
            });
            MoBuilder.Entity<DB_ambiabar>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.ToTable($"ugc_ambiabar", DatabaseConfig.Database);
                entity.HasIndex(e => e.id).HasDatabaseName("id");
                entity.Property(e => e.Timestamp).HasColumnName("Timestamp");
                entity.Property(e => e.last_update).HasColumnName("last_update");
                entity.Property(e => e.User_ID).HasColumnName("User_ID");
                entity.Property(e => e.System_ID).HasColumnName("System_ID");
                entity.Property(e => e.Faction_Name).HasColumnName("Faction_Name");
                entity.Property(e => e.Faction_State).HasColumnName("Faction_State");
                entity.Property(e => e.Faction_Government).HasColumnName("Faction_Government");
                entity.Property(e => e.Faction_Influence).HasColumnName("Faction_Influence");
                entity.Property(e => e.Faction_Influence_change).HasColumnName("Faction_Influence_change");
                entity.Property(e => e.Faction_Allegiance).HasColumnName("Faction_Allegiance");
                entity.Property(e => e.Faction_Happiness).HasColumnName("Faction_Happiness");
                entity.Property(e => e.Faction_ActiveState).HasColumnName("Faction_ActiveState");
                entity.Property(e => e.Faction_PendingState).HasColumnName("Faction_PendingState");
                entity.Property(e => e.Faction_RecoveringState).HasColumnName("Faction_RecoveringState");
                entity.Property(e => e.missions).HasColumnName("missions");
                entity.Property(e => e.explorer).HasColumnName("explorer");
                entity.Property(e => e.voucheer).HasColumnName("voucheer");
                entity.Property(e => e.trade).HasColumnName("trade");
            });
            MoBuilder.Entity<DB_ambiabar>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.ToTable($"ugc_ambiabar", DatabaseConfig.Database);
                entity.HasIndex(e => e.id).HasDatabaseName("id");
                entity.Property(e => e.Timestamp).HasColumnName("Timestamp");
                entity.Property(e => e.last_update).HasColumnName("last_update");
                entity.Property(e => e.User_ID).HasColumnName("User_ID");
                entity.Property(e => e.System_ID).HasColumnName("System_ID");
                entity.Property(e => e.Faction_Name).HasColumnName("Faction_Name");
                entity.Property(e => e.Faction_State).HasColumnName("Faction_State");
                entity.Property(e => e.Faction_Government).HasColumnName("Faction_Government");
                entity.Property(e => e.Faction_Influence).HasColumnName("Faction_Influence");
                entity.Property(e => e.Faction_Influence_change).HasColumnName("Faction_Influence_change");
                entity.Property(e => e.Faction_Allegiance).HasColumnName("Faction_Allegiance");
                entity.Property(e => e.Faction_Happiness).HasColumnName("Faction_Happiness");
                entity.Property(e => e.Faction_ActiveState).HasColumnName("Faction_ActiveState");
                entity.Property(e => e.Faction_PendingState).HasColumnName("Faction_PendingState");
                entity.Property(e => e.Faction_RecoveringState).HasColumnName("Faction_RecoveringState");
                entity.Property(e => e.missions).HasColumnName("missions");
                entity.Property(e => e.explorer).HasColumnName("explorer");
                entity.Property(e => e.voucheer).HasColumnName("voucheer");
                entity.Property(e => e.trade).HasColumnName("trade");
            });
        }
    }
}
