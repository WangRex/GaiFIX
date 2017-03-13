namespace Apps.Models.Migrations
{
    using Sys;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Apps.Models.DbContexts>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(Apps.Models.DbContexts context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            context.SysUser.AddOrUpdate(
                //�����жϵĴ��ڵ��ֶ�
                u => u.UserName,
                new SysUser { UserName = "��������Ա", Password = "88888888",TrueName = "��������Ա", State = "True", DepId = "1", DepName = "��˾�쵼" }
                );
            context.SysModule.AddOrUpdate(
                m => m.Name,
                new SysModule { Name = "ϵͳ����", ParentId = "0", Enable = "True", Sort = "1", state = "True", Iconic = "fa fa-puzzle-piece" },
                new SysModule { Name = "�˵�����", ParentId = "1", Enable = "True", Sort = "1", state = "True", Iconic = "fa fa-puzzle-piece", Url = "/SysModule" }
                );
            context.SysStruct.AddOrUpdate(
                s => s.Name,
                new SysStruct { Name = "��˾�쵼", ParentId = "0", Sort = "1", Enable = "True", Remark = "��˾�쵼" }
                );
        }
    }
}
