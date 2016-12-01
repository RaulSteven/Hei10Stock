namespace Hei10.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StevenAddJobTask1201 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.JobTasks",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        TaskId = c.Guid(nullable: false),
                        TaskName = c.String(maxLength: 255),
                        TaskParam = c.String(),
                        CronExpressionString = c.String(maxLength: 200),
                        CronRemark = c.String(maxLength: 300),
                        Assembly = c.String(maxLength: 150),
                        Class = c.String(maxLength: 150),
                        RecentRunTime = c.DateTime(),
                        LastRunTime = c.DateTime(),
                        Remark = c.String(),
                        IsDeleteOldTask = c.Boolean(nullable: false),
                        CommonStatus = c.Int(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                        CreateUserName = c.String(maxLength: 50),
                        CreateUserId = c.Long(nullable: false),
                        UpdateTime = c.DateTime(nullable: false),
                        UpdateUserId = c.Long(nullable: false),
                        CreateIP = c.String(maxLength: 50),
                        UpdateIP = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.JobTasks");
        }
    }
}
