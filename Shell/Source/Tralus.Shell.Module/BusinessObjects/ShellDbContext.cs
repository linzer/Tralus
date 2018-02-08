using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using DevExpress.ExpressApp.EF.Updating;
using DevExpress.ExpressApp.Workflow.EF;using DevExpress.ExpressApp.Workflow.Versioning;
using DevExpress.Persistent.BaseImpl.EF;
using DevExpress.Workflow.EF;
using Tralus.Framework.BusinessModel.Entities;
using Tralus.Framework.Data;
using Tralus.Framework.Utility.ReflectionHelpers;
using Role = Tralus.Framework.BusinessModel.Entities.Role;
using User = Tralus.Framework.BusinessModel.Entities.User;

namespace Tralus.Shell.Module.BusinessObjects {
    public class ShellDbContext : FrameworkDbContext
    {
        static ShellDbContext()
        {
            Database.SetInitializer<ShellDbContext>(null);    
        }

        public ShellDbContext(String connectionString)
            : base(connectionString)
        {
        }
        public ShellDbContext(DbConnection connection)
            : base(connection)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.HasDefaultSchema("Tralus");

            //modelBuilder.Entity<User>()
            // .HasMany<Role>(u => u.Roles)
            // .WithMany(r => r.Users)
            // .Map((m) =>
            // {
            //     m.ToTable("UserRoles", "System")
            //         .MapLeftKey("UserId")
            //         .MapRightKey("RoleId");
            // });

            //modelBuilder.Entity<Role>()
            //   .HasMany<Role>(r => r.ChildRoles)
            //   .WithMany(r => r.ParentRoles)
            //   .Map((m) =>
            //   {
            //       m.ToTable("RoleRoles", "System")
            //           .MapLeftKey("ParentRoleId")
            //           .MapRightKey("ChildRoleId");
            //   });

            //modelBuilder.Entity<ReportDataV2>();
            //modelBuilder.Entity<Analysis>();

            //modelBuilder.Entity<EFWorkflowDefinition>();
            //modelBuilder.Entity<EFStartWorkflowRequest>();
            //modelBuilder.Entity<EFRunningWorkflowInstanceInfo>();
            //modelBuilder.Entity<EFWorkflowInstanceControlCommandRequest>();
            //modelBuilder.Entity<EFInstanceKey>();
            //modelBuilder.Entity<EFTrackingRecord>();
            //modelBuilder.Entity<EFWorkflowInstance>();
            //modelBuilder.Entity<EFUserActivityVersion>();
            //modelBuilder.Entity<ModelDifference>();
            //modelBuilder.Entity<ModelDifferenceAspect>();


            //base.OnModelCreating(modelBuilder);
            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(ass=>ass.FullName.Contains("BusinessModel"));

            var entityTypes =
               assemblies.SelectMany(ass=>ass.GetTypes())
                   .Where(e => e.IsSubclassOf(typeof(EntityBase)) && !e.IsAbstract);

            foreach (var entity in entityTypes)
            {
                modelBuilder.RegisterEntityType(entity);
            }
        }

        protected override IEnumerable<Type> GetEntityTypes()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(ass => ass.FullName.Contains("BusinessModel"));

            var entityTypes =
               assemblies.SelectMany(ass => ass.GetTypes())
                   .Where(e => e.IsSubclassOf(typeof(EntityBase)) && !e.IsAbstract)
                   .ToList();

            return entityTypes;
        }
        //public virtual DbSet<ReportDataV2> ReportDataV2 { get; set; }
        //public virtual DbSet<Analysis> Analysis { get; set; }
        //public virtual DbSet<EFWorkflowDefinition> EFWorkflowDefinition { get; set; }
        //public virtual DbSet<EFStartWorkflowRequest> EFStartWorkflowRequest { get; set; }
        //public virtual DbSet<EFRunningWorkflowInstanceInfo> EFRunningWorkflowInstanceInfo { get; set; }
        //public virtual DbSet<EFWorkflowInstanceControlCommandRequest> EFWorkflowInstanceControlCommandRequest { get; set; }
        //public virtual DbSet<EFInstanceKey> EFInstanceKey { get; set; }
        //public virtual DbSet<EFTrackingRecord> EFTrackingRecord { get; set; }
        //public virtual DbSet<EFWorkflowInstance> EFWorkflowInstance { get; set; }
        //public virtual DbSet<EFUserActivityVersion> EFUserActivityVersion { get; set; }
        //public virtual DbSet<ModelDifference> ModelDifference { get; set; }
        //public virtual DbSet<ModelDifferenceAspect> ModelDifferenceAspect { get; set; }
        //public virtual DbSet<User> User { get; set; }
    }
}