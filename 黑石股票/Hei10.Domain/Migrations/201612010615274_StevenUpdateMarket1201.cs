namespace Hei10.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StevenUpdateMarket1201 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StockMarkets", "StockKey", c => c.String(nullable: false, maxLength: 10));
            AddColumn("dbo.StockMarkets", "OpenPriceKey", c => c.String(nullable: false, maxLength: 20));
            AddColumn("dbo.StockMarkets", "FormPriceKey", c => c.String(nullable: false, maxLength: 20));
            AddColumn("dbo.StockMarkets", "MaxPriceKey", c => c.String(nullable: false, maxLength: 20));
            AddColumn("dbo.StockMarkets", "MinPriceKey", c => c.String(nullable: false, maxLength: 20));
        }
        
        public override void Down()
        {
            DropColumn("dbo.StockMarkets", "MinPriceKey");
            DropColumn("dbo.StockMarkets", "MaxPriceKey");
            DropColumn("dbo.StockMarkets", "FormPriceKey");
            DropColumn("dbo.StockMarkets", "OpenPriceKey");
            DropColumn("dbo.StockMarkets", "StockKey");
        }
    }
}
