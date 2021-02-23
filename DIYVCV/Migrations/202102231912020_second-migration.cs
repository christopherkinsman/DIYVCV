namespace DIYVCV.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class secondmigration : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Modulexcomponents", "Component_ComponentId", "dbo.Components");
            DropForeignKey("dbo.Modulexcomponents", "Module_ModuleId", "dbo.Modules");
            DropIndex("dbo.Modulexcomponents", new[] { "Component_ComponentId" });
            DropIndex("dbo.Modulexcomponents", new[] { "Module_ModuleId" });
            AddColumn("dbo.Components", "ModuleId", c => c.String());
            AddColumn("dbo.Components", "ComponentQuantity", c => c.String());
            DropTable("dbo.Modulexcomponents");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Modulexcomponents",
                c => new
                    {
                        ModulesxcomponentsId = c.Int(nullable: false, identity: true),
                        modulesxcomponentsmoduleid = c.Int(nullable: false),
                        modulesxcomponentscomponentid = c.Int(nullable: false),
                        componentamount = c.String(),
                        Component_ComponentId = c.Int(),
                        Module_ModuleId = c.Int(),
                    })
                .PrimaryKey(t => t.ModulesxcomponentsId);
            
            DropColumn("dbo.Components", "ComponentQuantity");
            DropColumn("dbo.Components", "ModuleId");
            CreateIndex("dbo.Modulexcomponents", "Module_ModuleId");
            CreateIndex("dbo.Modulexcomponents", "Component_ComponentId");
            AddForeignKey("dbo.Modulexcomponents", "Module_ModuleId", "dbo.Modules", "ModuleId");
            AddForeignKey("dbo.Modulexcomponents", "Component_ComponentId", "dbo.Components", "ComponentId");
        }
    }
}
