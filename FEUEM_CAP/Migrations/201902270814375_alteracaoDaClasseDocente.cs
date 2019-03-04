namespace FEUEM_CAP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alteracaoDaClasseDocente : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categoria",
                c => new
                    {
                        CategoriaId = c.Int(nullable: false, identity: true),
                        DescricaoCategoria = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.CategoriaId);
            
            CreateTable(
                "dbo.Curso",
                c => new
                    {
                        CursoId = c.Int(nullable: false, identity: true),
                        NomeCurso = c.String(nullable: false, maxLength: 255),
                        Duracao = c.Byte(nullable: false),
                        AnoCriacao = c.Int(nullable: false),
                        DepartamentoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CursoId)
                .ForeignKey("dbo.Departamento", t => t.DepartamentoId, cascadeDelete: true)
                .Index(t => t.DepartamentoId);
            
            CreateTable(
                "dbo.Departamento",
                c => new
                    {
                        DepartamentoId = c.Int(nullable: false, identity: true),
                        NomeDepartamento = c.String(nullable: false, maxLength: 255),
                        Sigla = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.DepartamentoId);
            
            CreateTable(
                "dbo.Disciplina",
                c => new
                    {
                        DisciplinaId = c.Int(nullable: false, identity: true),
                        NomeDisciplina = c.String(nullable: false, maxLength: 255),
                        Ano = c.Byte(nullable: false),
                        Semestre = c.Byte(nullable: false),
                        CursoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DisciplinaId)
                .ForeignKey("dbo.Curso", t => t.CursoId, cascadeDelete: true)
                .Index(t => t.CursoId);
            
            CreateTable(
                "dbo.Docente",
                c => new
                    {
                        DocenteId = c.Int(nullable: false, identity: true),
                        NomeDocente = c.String(nullable: false, maxLength: 255),
                        ApelidoDocente = c.String(nullable: false, maxLength: 255),
                        ContactoTelefone = c.Int(nullable: false),
                        ContactoEmail = c.String(nullable: false, maxLength: 255),
                        Nuit = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Nib = c.Double(nullable: false),
                        NumeroConta = c.Double(nullable: false),
                        CategoriaId = c.Int(nullable: false),
                        NivelAcademicoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DocenteId)
                .ForeignKey("dbo.Categoria", t => t.CategoriaId, cascadeDelete: true)
                .ForeignKey("dbo.NivelAcademico", t => t.NivelAcademicoId, cascadeDelete: true)
                .Index(t => t.CategoriaId)
                .Index(t => t.NivelAcademicoId);
            
            CreateTable(
                "dbo.NivelAcademico",
                c => new
                    {
                        NivelAcademicoId = c.Int(nullable: false, identity: true),
                        DescricaoNivelAcademico = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.NivelAcademicoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Docente", "NivelAcademicoId", "dbo.NivelAcademico");
            DropForeignKey("dbo.Docente", "CategoriaId", "dbo.Categoria");
            DropForeignKey("dbo.Disciplina", "CursoId", "dbo.Curso");
            DropForeignKey("dbo.Curso", "DepartamentoId", "dbo.Departamento");
            DropIndex("dbo.Docente", new[] { "NivelAcademicoId" });
            DropIndex("dbo.Docente", new[] { "CategoriaId" });
            DropIndex("dbo.Disciplina", new[] { "CursoId" });
            DropIndex("dbo.Curso", new[] { "DepartamentoId" });
            DropTable("dbo.NivelAcademico");
            DropTable("dbo.Docente");
            DropTable("dbo.Disciplina");
            DropTable("dbo.Departamento");
            DropTable("dbo.Curso");
            DropTable("dbo.Categoria");
        }
    }
}
