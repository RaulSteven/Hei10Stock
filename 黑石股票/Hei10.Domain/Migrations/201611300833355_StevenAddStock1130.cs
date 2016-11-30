namespace Hei10.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StevenAddStock1130 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Stocks",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Code = c.String(nullable: false, maxLength: 50),
                        StockMarketId = c.Long(nullable: false),
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
            
            CreateTable(
                "dbo.StockMarkets",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 20),
                        RequestUrl = c.String(nullable: false, maxLength: 250),
                        Remark = c.String(maxLength: 500),
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
            
            CreateTable(
                "dbo.StockRecords",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        StockId = c.Long(nullable: false),
                        OpenPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FormPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MaxPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MinPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MTR = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ATR = c.Decimal(nullable: false, precision: 18, scale: 2),
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
            DropTable("dbo.StockRecords");
            DropTable("dbo.StockMarkets");
            DropTable("dbo.Stocks");
        }
    }
}
