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
            return PartialView(categoria);
        }

        
        public ActionResult AdicionarCategoria()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdicionarCategoria([Bind(Include = "CategoriaId,DescricaoCategoria")] Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                db.Categorias.Add(categoria);
                db.SaveChanges();
                return Json(new {resultado = true, mensagem = "Categoria gravada com sucesso"});
            }
            {
                IEnumerable<ModelError> erros = ModelState.Values.SelectMany(item => item.Errors);
                return Json(new { resultado = false, mensagem = erros});
            }
    }

        
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
            return PartialView(categoria);
        }
        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarCategoria([Bind(Include = "CategoriaId,DescricaoCategoria")] Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                db.Entry(categoria).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { resultado = true, mensagem = "Categoria actualizada com sucesso" });
            }
            {
                IEnumerable<ModelError> erros = ModelState.Values.SelectMany(item => item.Errors);
                return Json(new { resultado = false, mensagem = erros });
            }
        }

        
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
            return PartialView(categoria);
        }

        
        [HttpPost, ActionName("RemoverCategoria")]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmarRemocao(int id)
        {

            try
            {
                Categoria categoria = db.Categorias.Find(id);
                db.Categorias.Remove(categoria);
                db.SaveChanges();
                return Json(new {resultado = true, mensagem = "Categoria removida com sucesso"});
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
