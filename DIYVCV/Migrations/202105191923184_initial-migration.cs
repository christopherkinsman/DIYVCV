namespace DIYVCV.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialmigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Components",
                c => new
                    {
                        ComponentId = c.Int(nullable: false, identity: true),
                        ComponentName = c.String(),
                        ComponentValue = c.String(),
                        ModuleId = c.String(),
                        ComponentQuantity = c.String(),
                    })
                .PrimaryKey(t => t.ComponentId);
            
            CreateTable(
                "dbo.Modules",
                c => new
                    {
                        ModuleId = c.Int(nullable: false, identity: true),
                        ModuleName = c.String(),
                        ModuleBrand = c.String(),
                        ModuleCategory = c.String(),
                        ModuleDescription = c.String(),
                        ModuleLink = c.String(),
                        ModuleSchematic = c.String(),
                        ModuleHasPic = c.Boolean(nullable: false),
                        PicExtension = c.String(),
                    })
                .PrimaryKey(t => t.ModuleId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Modules");
            DropTable("dbo.Components");
        }
    }
}
