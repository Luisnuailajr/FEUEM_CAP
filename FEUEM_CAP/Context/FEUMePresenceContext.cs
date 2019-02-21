using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using FEUEM_CAP.Models;

namespace FEUEM_CAP.Context
{
    public class FEUMePresenceContext : DbContext 
    {
        public DbSet<Curso> Cursos { get; set; }
        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<Disciplina> Disciplinas { get; set; }
        public DbSet<Docente> Docentes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Properties<string>().Configure(c => c.HasMaxLength(255));
        }

        public System.Data.Entity.DbSet<FEUEM_CAP.Models.Categoria> Categorias { get; set; }

        public System.Data.Entity.DbSet<NivelAcademico> NivelAcademicoes { get; set; }
    }

    
}