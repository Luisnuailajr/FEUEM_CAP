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
    public class CategoriaController : Controller
    {
        private FEUMePresenceContext db = new FEUMePresenceContext();

        // GET: Categoria
        public ActionResult Index()
        {
            return View(db.Categorias.ToList());
        }

        public JsonResult ListarCategorias(string searchPhrase, int current = 1, int rowCount = 5)
        {
            string chave = Request.Form.AllKeys.Where(k => k.StartsWith("sort")).First();
            string ordenacao = Request[chave];
            string campo = chave.Replace("sort[", String.Empty).Replace("]", String.Empty);


            IQueryable<Categoria> categorias = db.Categorias;

            int Total = categorias.Count();

            if (!String.IsNullOrWhiteSpace(searchPhrase))
            {
                categorias = categorias.Where("DescricaoCategoria.Contains(@0)", searchPhrase);
            }

            string campoOrdenacao = String.Format("{0} {1}", campo, ordenacao);
            var CategoriasPaginadas = categorias.OrderBy(campoOrdenacao).Skip((current - 1) * 5).Take(rowCount);
            return Json(new

            {
                rows = CategoriasPaginadas.ToList(),
                current = current,
                rowCount = rowCount,
                total = Total
            }, JsonRequestBehavior.AllowGet);

        }

        // GET: Categoria/DetalhesCurso/5
        public ActionResult DetalhesCategoria(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Categoria categoria = db.Categorias.Find(id);
            if (categoria == null)
            {
                return HttpNotFound();
            }
            return View(categoria);
        }

        // GET: Categoria/AdicionarDocente
        public ActionResult AdicionarCategoria()
        {
            return View();
        }

        // POST: Categoria/AdicionarDocente
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdicionarCategoria([Bind(Include = "CategoriaId,DescricaoCategoria")] Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                db.Categorias.Add(categoria);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(categoria);
        }

        // GET: Categoria/EditarCurso/5
        public ActionResult EditarCategoria(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Categoria categoria = db.Categorias.Find(id);
            if (categoria == null)
            {
                return HttpNotFound();
            }
            return View(categoria);
        }

        // POST: Categoria/EditarCurso/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarCategoria([Bind(Include = "CategoriaId,DescricaoCategoria")] Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                db.Entry(categoria).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(categoria);
        }

        // GET: Categoria/RemoverCurso/5
        public ActionResult RemoverCategoria(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Categoria categoria = db.Categorias.Find(id);
            if (categoria == null)
            {
                return HttpNotFound();
            }
            return View(categoria);
        }

        // POST: Categoria/RemoverCurso/5
        [HttpPost, ActionName("RemoverCurso")]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmarRemocao(int id)
        {
            Categoria categoria = db.Categorias.Find(id);
            db.Categorias.Remove(categoria);
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
