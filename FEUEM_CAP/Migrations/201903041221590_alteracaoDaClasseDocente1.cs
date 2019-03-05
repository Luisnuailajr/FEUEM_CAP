namespace FEUEM_CAP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alteracaoDaClasseDocente1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Docente", "Nuit", c => c.Int(nullable: false));
            AlterColumn("dbo.Docente", "Nib", c => c.Int(nullable: false));
            AlterColumn("dbo.Docente", "NumeroConta", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Docente", "NumeroConta", c => c.Double(nullable: false));
            AlterColumn("dbo.Docente", "Nib", c => c.Double(nullable: false));
            AlterColumn("dbo.Docente", "Nuit", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
