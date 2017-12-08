using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Redgum.ServerMonitor.Web.ServerMonitor.Data
{
    public class MonitorDbContext : DbContext
    {
        public DbSet<ServerDataModel> Servers { get; set; }
        public DbSet<ServerDataDataModel> ServerData { get; set; }
        public DbSet<ServerDataAgregateDataModel> ServerDataAgregates { get; set; }
        public DbSet<SettingsDataModel> Settings { get; set; }

        public MonitorDbContext(DbContextOptions<MonitorDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // no index attribute?!?!?! wtf
            // oh well.. do it this lame way
            builder.Entity<ServerDataModel>().HasIndex(p => new { p.Name });
        }
    }


    public class ServerDataModel
    {
        public int Id { get; set; }
        public Guid Identifier { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(100)]
        public string Domain { get; set; }
        public DateTime UpdatedUtc { get; set; }
        public virtual ICollection<ServerDataAgregateDataModel> DataAggregate { get; set; }
        public virtual ICollection<ServerDataDataModel> Data { get; set; }



    }



    public class ServerDataDataModel
    {
        public int Id { get; set; }
        public Guid Identifier { get; set; }
        public virtual ServerDataModel Server { get; set; }
        [MaxLength(50)]
        public string DataKey { get; set; }
        public string Data { get; set; }
    }

    public class ServerDataAgregateDataModel
    {
        public int Id { get; set; }
        public Guid Identifier { get; set; }
        public virtual ServerDataModel Server { get; set; }
        [MaxLength(50)]
        public string DataKey { get; set; }
        public string Data { get; set; }
    }

    public class SettingsDataModel
    {
        public int Id { get; set; }
        public Guid Identifier { get; set; }
        [MaxLength(50)]
        public string DataKey { get; set; }
        public string Data { get; set; }
    }

}
