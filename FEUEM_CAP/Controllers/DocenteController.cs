using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FEUEM_CAP.Context;
using FEUEM_CAP.Models;

namespace FEUEM_CAP.Controllers
{
    public class DocenteController : Controller
    {
        private FEUMePresenceContext db = new FEUMePresenceContext();

        // GET: Docente
        public ActionResult Index()
        {
            
            return View();
        }

        public JsonResult ListarDocentes(string searchPhrase, int current = 1, int rowCount = 5)
        {
            string chave = Request.Form.AllKeys.Where(k => k.StartsWith("sort")).First();
            string ordenacao = Request[chave];
            string campo = chave.Replace("sort[", String.Empty).Replace("]", String.Empty);


            var docentes = db.Docentes.Include(d => d.Categoria).Include(d => d.NivelAcademico);

            int Total = docentes.Count();

            if (!String.IsNullOrWhiteSpace(searchPhrase))
            {
                int numeroTelefone = 0;
                int.TryParse(searchPhrase, out numeroTelefone);

                int nuit = 0;
                int.TryParse(searchPhrase, out nuit);

                int nib = 0;
                int.TryParse(searchPhrase, out nib);

                int numConta = 0;
                int.TryParse(searchPhrase, out numConta);


                docentes = docentes.Where("NomeDocente.Contains(@0) or " +
                                          "ApelidoDocente.Contains(@0) or " +
                                          "ContactoEmail.Contains(@0) or " +
                                          "ContactoTelefone == @1 or nuit == @1 or " +
                                          "nib == @1 or NumeroConta == @1",
                    searchPhrase, numeroTelefone, nuit, nib, numConta);
            }
            string campoOrdenacao = String.Format("{0} {1}", campo, ordenacao);
            var docentesPaginados = docentes.OrderBy(campoOrdenacao).Skip((current - 1) * 5).Take(rowCount);
            return Json(new

            {
                rows = docentesPaginados.ToList(),
                current = current,
                rowCount = rowCount,
                total = Total
            }, JsonRequestBehavior.AllowGet);

        }

        
        public ActionResult DetalhesDocente(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Docente docente = db.Docentes.Find(id);
            if (docente == null)
            {
                return HttpNotFound();
            }
            return PartialView(docente);
        }

        
        public ActionResult AdicionarDocente()
        {
            ViewBag.CategoriaId = new SelectList(db.Categorias, "CategoriaId", "DescricaoCategoria");
            ViewBag.NivelAcademicoId = new SelectList(db.NivelAcademicoes, "NivelAcademicoId", "DescricaoNivelAcademico");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdicionarDocente([Bind(Include = "DocenteId,NomeDocente,ApelidoDocente,ContactoTelefone,ContactoEmail,Nuit,Nib,NumeroConta,CategoriaId,NivelAcademicoId")] Docente docente)
        {
            if (ModelState.IsValid)
            {
                db.Docentes.Add(docente);
                db.SaveChanges();
                return Json(new {resultado = true, mensagem="Docente gravado com sucesso"});
            }
            {
                IEnumerable<ModelError> erros = ModelState.Values.SelectMany(item => item.Errors);
                return Json(new { resultado = false, mensagem = erros });
            }
        }

        public ActionResult EditarDocente(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Docente docente = db.Docentes.Find(id);
            if (docente == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoriaId = new SelectList(db.Categorias, "CategoriaId", "DescricaoCategoria", docente.CategoriaId);
            ViewBag.NivelAcademicoId = new SelectList(db.NivelAcademicoes, "NivelAcademicoId", "DescricaoNivelAcademico", docente.NivelAcademicoId);
            return PartialView(docente);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarDocente([Bind(Include = "DocenteId,NomeDocente,ApelidoDocente,ContactoTelefone,ContactoEmail,Nuit,Nib,NumeroConta,CategoriaId,NivelAcademicoId")] Docente docente)
        {
            if (ModelState.IsValid)
            {
                db.Entry(docente).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { resultado = true, mensagem = "Docente actualizado com sucesso" });
            }
            {
                IEnumerable<ModelError> erros = ModelState.Values.SelectMany(item => item.Errors);
                return Json(new { resultado = false, mensagem = erros });
            }
        }

        
        public ActionResult RemoverDocente(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Docente docente = db.Docentes.Find(id);
            if (docente == null)
            {
                return HttpNotFound();
            }
            return PartialView(docente);
        }

        // POST: Docente/RemoverCurso/5
        [HttpPost, ActionName("RemoverDocente")]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmarRemocao(int id)
        {
            try
            {

            
            Docente docente = db.Docentes.Find(id);
            db.Docentes.Remove(docente);
            db.SaveChanges();
            return Json(new { resultado = true, mensagem = "Docente removido com sucesso" });
            }
            catch (Exception ex)
            {
                return Json(new { resultado = true, mensagem = ex });
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
