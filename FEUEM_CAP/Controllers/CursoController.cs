 using System;
using System.Collections.Generic;
 using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Net;
using System.Web.Mvc;
 using FEUEM_CAP.Context;
 using FEUEM_CAP.Models;

namespace FEUEM_CAP.Controllers
{
    public class CursoController : Controller
    {
        private FEUMePresenceContext db = new FEUMePresenceContext();


        // GET: Curso
        public ActionResult Index()
        {
            //var cursos = db.Cursos.Include(c => c.Departamento);

            return View();
        }

        public JsonResult ListarCursos(string searchPhrase, int current = 1, int rowCount = 5)
        {
            string chave = Request.Form.AllKeys.Where(k => k.StartsWith("sort")).First();
            string ordenacao = Request[chave];
            string campo = chave.Replace("sort[", String.Empty).Replace("]", String.Empty);


            var cursos = db.Cursos.Join(db.Departamentos, c => c.DepartamentoId, d => d.DepartamentoId,
                                       (c, d) => new
                                                {
                                                    c.CursoId,
                                                    c.NomeCurso,
                                                    c.Duracao,
                                                    c.AnoCriacao,
                                                    d.NomeDepartamento

                                                });

            /*db.Cursos.Include(c => c.Departamento);*/

            int Total = cursos.Count();

            if (!String.IsNullOrWhiteSpace(searchPhrase))
            {
                int anoCriacao = 0;
                int.TryParse(searchPhrase, out anoCriacao);

                int duracao = 0;
                int.TryParse(searchPhrase, out duracao);

                
                

                cursos = cursos.Where("NomeCurso.Contains(@0) or NomeDepartamento.Contains(@0) or AnoCriacao == @1 or Duracao == @1",
                    searchPhrase, anoCriacao, duracao);
            }

            string campoOrdenacao = String.Format("{0} {1}", campo, ordenacao);
            var CursosPaginados = cursos.OrderBy(campoOrdenacao).Skip((current - 1) * 5).Take(rowCount);
            return Json( new
           
                {
                    rows = CursosPaginados.ToList(),
                    current = current,
                    rowCount = rowCount,
                    total = Total
                }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult Listar()
        {

            //var query = from n in names
            //    join p in people on n equals p.Name into matching
            //    select new { Name = n, Count = matching.Count() };

            //from publisher in SampleData.Publishers
            //    join book in SampleData.Books on publisher equals book.Publisher
            //    select new { Publisher = publisher.Name, Book = book.Title };

            // < TOuter, TInner, TKey, TResult > (outer, inner, outerKeySelector, innerKeySelector, resultSelector, comparer);
            
            var cursosDepartamento = db.Cursos.Join(db.Departamentos, c => c.DepartamentoId, d => d.DepartamentoId, 
                                                    (c, d) => new
                                                    {
                                                        c.NomeCurso, c.Duracao, c.AnoCriacao,d.NomeDepartamento

                                                    });


           var cursos = db.Cursos.Include(c => c.Departamento);

            return Json(cursosDepartamento.ToList(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult ListarEdicao(string searchPhrase, int? id)
        {

            //var query = from n in names
            //    join p in people on n equals p.Name into matching
            //    select new { Name = n, Count = matching.Count() };

            //from publisher in SampleData.Publishers
            //    join book in SampleData.Books on publisher equals book.Publisher
            //    select new { Publisher = publisher.Name, Book = book.Title };

            // < TOuter, TInner, TKey, TResult > (outer, inner, outerKeySelector, innerKeySelector, resultSelector, comparer);

            var cursosDepartamento = db.Cursos.Join(db.Departamentos, c => c.DepartamentoId, d => d.DepartamentoId,
                (c, d) => new
                {
                    c.CursoId,
                    c.NomeCurso,
                    c.Duracao,
                    c.AnoCriacao,
                    d.NomeDepartamento

                });

            var BuscarPeloId = cursosDepartamento.Where( c => c.CursoId == id);

            var cursos = db.Cursos.Include(c => c.Departamento);

            return Json(BuscarPeloId.ToList(), JsonRequestBehavior.AllowGet);
        }
        // GET: Curso/DetalhesCurso/5
        public ActionResult DetalhesCurso(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            var curso = db.Cursos.Find(id);

            return PartialView(curso);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdicionarDepartamento([Bind(Include = "DepartamentoId,NomeDepartamento,Sigla")] Departamento departamento)
        {
            if (ModelState.IsValid)
            {
                db.Departamentos.Add(departamento);
                db.SaveChanges();

                return Json(new { resultado = true, mensagem = "Departamento gravado com sucesso!" });
            }

            {
                IEnumerable<ModelError> erros = ModelState.Values.SelectMany(item => item.Errors);

                return Json(new { resultado = false, mensagem = erros });

            }
        }

        // GET: Curso/AdicionarDocente
        public ActionResult AdicionarCurso()
        {
            ViewBag.DepartamentoId = new SelectList(db.Departamentos, "DepartamentoId", "NomeDepartamento");
            return PartialView();
        }

        // POST: Curso/AdicionarDocente
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdicionarCurso([Bind(Include = "CursoId,NomeCurso,Duracao,AnoCriacao,DepartamentoId")] Curso curso)
        {
            if (ModelState.IsValid)
            {
                db.Cursos.Add(curso);
                db.SaveChanges();

                return Json(new { resultado = true, mensagem = "Curso gravado com sucesso!" });
            }
            else

            {
                IEnumerable<ModelError> erros = ModelState.Values.SelectMany(item => item.Errors);

                return Json(new { resultado = false, mensagem = erros });

            }

        }

        // GET: Curso/EditarCurso/5
        public ActionResult EditarCurso(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Curso curso = db.Cursos.Find(id);
            if (curso == null)
            {
                return HttpNotFound();
            }
            ViewBag.DepartamentoId = new SelectList(db.Departamentos, "DepartamentoId", "NomeDepartamento", curso.DepartamentoId);
            return PartialView(curso);
        }

        // POST: Curso/EditarCurso/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarCurso([Bind(Include = "CursoId,NomeCurso,Duracao,AnoCriacao,DepartamentoId")] Curso curso)
        {
            if (ModelState.IsValid)
            {
                db.Entry(curso).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { resultado = true, mensagem = "Curso actualizado com sucesso!" });
            }

            {
                IEnumerable<ModelError> erros = ModelState.Values.SelectMany(item => item.Errors);

                return Json(new { resultado = false, mensagem = erros });

            }
        }

        // GET: Curso/RemoverCurso/5
        public ActionResult RemoverCurso(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Curso curso = db.Cursos.Find(id);
            if (curso == null)
            {
                return HttpNotFound();
            }
            return PartialView(curso);
        }

        // POST: Curso/RemoverCurso/5
        [HttpPost, ActionName("RemoverCurso")]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmarRemocao(int id)
        {
            try
            {
                Curso curso = db.Cursos.Find(id);
                db.Cursos.Remove(curso);
                db.SaveChanges();

                return Json(new { resultado = true, mensagem = "curso removido com sucesso!" });
            }
            catch (Exception ex)
            {
                return Json(new { resultado = false, mensagem = ex.Message });
            }
            
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
