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
    public class DepartamentoController : Controller
    {
        private FEUMePresenceContext db = new FEUMePresenceContext();

        // GET: Departamento
        public ActionResult Index()
        {
            return View(db.Departamentos.ToList());
        }

        public JsonResult ListarDepartamentos(string searchPhrase, int current = 1, int rowCount = 5)
        {
            string chave = Request.Form.AllKeys.Where(k => k.StartsWith("sort")).First();
            string ordenacao = Request[chave];
            string campo = chave.Replace("sort[", String.Empty).Replace("]", String.Empty);


            IQueryable<Departamento> departamentos = db.Departamentos;

            int Total = departamentos.Count();

            if (!string.IsNullOrWhiteSpace(searchPhrase))
            {

                departamentos = departamentos.Where("NomeDepartamento.Contains(@0) Or Sigla.Contains(@0)",
                    searchPhrase);
            }

            string campoOrdenacao = String.Format("{0} {1}", campo, ordenacao);
            var deparatemntosPaginados = departamentos.OrderBy(campoOrdenacao).Skip((current - 1) * 5).Take(rowCount);
            return Json(new

            {
                rows = deparatemntosPaginados.ToList(),
                current = current,
                rowCount = rowCount,
                total = Total
            }, JsonRequestBehavior.AllowGet);
        }

        // GET: Departamento/DetalhesCurso/5
        public ActionResult DetalhesDepartamento(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Departamento departamento = db.Departamentos.Find(id);
            if (departamento == null)
            {
                return HttpNotFound();
            }
            return View(departamento);
        }

        // GET: Departamento/AdicionarDocente
        public ActionResult AdicionarDepartamento()
        {
            return PartialView();
        }

        // POST: Departamento/AdicionarDepartamento     
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: Departamento/EditarCurso/5
        public ActionResult EditarDepartamento(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Departamento departamento = db.Departamentos.Find(id);
            if (departamento == null)
            {
                return HttpNotFound();
            }
            return View(departamento);
        }

        // POST: Departamento/EditarCurso/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarDepartamento([Bind(Include = "DepartamentoId,NomeDepartamento,Sigla")] Departamento departamento)
        {
            if (ModelState.IsValid)
            {
                db.Entry(departamento).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(departamento);
        }

        // GET: Departamento/RemoverCurso/5
        public ActionResult RemoverDepartamento(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Departamento departamento = db.Departamentos.Find(id);
            if (departamento == null)
            {
                return HttpNotFound();
            }
            return View(departamento);
        }

        // POST: Departamento/RemoverCurso/5
        [HttpPost, ActionName("RemoverCurso")]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmarRemocao(int id)
        {
            Departamento departamento = db.Departamentos.Find(id);
            db.Departamentos.Remove(departamento);
            db.SaveChanges();
            return RedirectToAction("Index");
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
