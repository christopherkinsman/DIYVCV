namespace DIYVCV.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialmigration : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Modulexcomponents", "ComponentId", "dbo.Components");
            DropForeignKey("dbo.Modulexcomponents", "ModuleId", "dbo.Modules");
            DropIndex("dbo.Modulexcomponents", new[] { "ModuleId" });
            DropIndex("dbo.Modulexcomponents", new[] { "ComponentId" });
            RenameColumn(table: "dbo.Modulexcomponents", name: "ComponentId", newName: "Component_ComponentId");
            RenameColumn(table: "dbo.Modulexcomponents", name: "ModuleId", newName: "Module_ModuleId");
            AlterColumn("dbo.Modulexcomponents", "Module_ModuleId", c => c.Int());
            AlterColumn("dbo.Modulexcomponents", "Component_ComponentId", c => c.Int());
            CreateIndex("dbo.Modulexcomponents", "Component_ComponentId");
            CreateIndex("dbo.Modulexcomponents", "Module_ModuleId");
            AddForeignKey("dbo.Modulexcomponents", "Component_ComponentId", "dbo.Components", "ComponentId");
            AddForeignKey("dbo.Modulexcomponents", "Module_ModuleId", "dbo.Modules", "ModuleId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Modulexcomponents", "Module_ModuleId", "dbo.Modules");
            DropForeignKey("dbo.Modulexcomponents", "Component_ComponentId", "dbo.Components");
            DropIndex("dbo.Modulexcomponents", new[] { "Module_ModuleId" });
            DropIndex("dbo.Modulexcomponents", new[] { "Component_ComponentId" });
            AlterColumn("dbo.Modulexcomponents", "Component_ComponentId", c => c.Int(nullable: false));
            AlterColumn("dbo.Modulexcomponents", "Module_ModuleId", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.Modulexcomponents", name: "Module_ModuleId", newName: "ModuleId");
            RenameColumn(table: "dbo.Modulexcomponents", name: "Component_ComponentId", newName: "ComponentId");
            CreateIndex("dbo.Modulexcomponents", "ComponentId");
            CreateIndex("dbo.Modulexcomponents", "ModuleId");
            AddForeignKey("dbo.Modulexcomponents", "ModuleId", "dbo.Modules", "ModuleId", cascadeDelete: true);
            AddForeignKey("dbo.Modulexcomponents", "ComponentId", "dbo.Components", "ComponentId", cascadeDelete: true);
        }
    }
}
