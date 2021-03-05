namespace DIYVCV.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class module_picture_migration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Modules", "ModuleHasPic", c => c.Boolean(nullable: false));
            AddColumn("dbo.Modules", "PicExtension", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Modules", "PicExtension");
            DropColumn("dbo.Modules", "ModuleHasPic");
        }
    }
}
