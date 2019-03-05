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
    public class NivelAcademicoController : Controller
    {
        private FEUMePresenceContext db = new FEUMePresenceContext();

        // GET: NivelAcademico
        public ActionResult Index()
        {
            return View(db.NivelAcademicoes.ToList());
        }

        public JsonResult ListarNiveisAcademicos(string searchPhrase, int current = 1, int rowCount = 5)
        {
            string chave = Request.Form.AllKeys.Where(k => k.StartsWith("sort")).First();
            string ordenacao = Request[chave];
            string campo = chave.Replace("sort[", String.Empty).Replace("]", String.Empty);


            IQueryable<NivelAcademico> niveis = db.NivelAcademicoes;

            int Total = niveis.Count();

            if (!String.IsNullOrWhiteSpace(searchPhrase))
            {
                niveis = niveis.Where("DescricaoNivelAcademico.Contains(@0)", searchPhrase);
            }

            string campoOrdenacao = String.Format("{0} {1}", campo, ordenacao);
            var niveisPaginados = niveis.OrderBy(campoOrdenacao).Skip((current - 1) * 5).Take(rowCount);
            return Json(new

            {
                rows = niveisPaginados.ToList(),
                current = current,
                rowCount = rowCount,
                total = Total
            }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult DetalhesNivelAcademico(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NivelAcademico nivelAcademico = db.NivelAcademicoes.Find(id);
            if (nivelAcademico == null)
            {
                return HttpNotFound();
            }
            return PartialView(nivelAcademico);
        }

        public ActionResult AdicionarNAcademico()
        {
            return PartialView();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdicionarNAcademico([Bind(Include = "NivelAcademicoId,DescricaoNivelAcademico")] NivelAcademico nivelAcademico)
        {
            if (ModelState.IsValid)
            {
                db.NivelAcademicoes.Add(nivelAcademico);
                db.SaveChanges();
                return Json(new {resultado = true, mensagem = "Nível académico gravado com sucesso"});
            }
            {
                IEnumerable<ModelError> erros = ModelState.Values.SelectMany(item => item.Errors);
                return Json(new { resultado = false, mensagem = erros });
            }
        }

        public ActionResult EditarNAcdemico(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NivelAcademico nivelAcademico = db.NivelAcademicoes.Find(id);
            if (nivelAcademico == null)
            {
                return HttpNotFound();
            }
            return PartialView(nivelAcademico);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarNAcdemico([Bind(Include = "NivelAcademicoId,DescricaoNivelAcademico")] NivelAcademico nivelAcademico)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nivelAcademico).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { resultado = true, mensagem = "Nível académico actualizado com sucesso" });
            }
            {
                IEnumerable<ModelError> erros = ModelState.Values.SelectMany(item => item.Errors);
                return Json(new { resultado = false, mensagem = erros });
            }
        }

        public ActionResult RemoverNAcademico(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NivelAcademico nivelAcademico = db.NivelAcademicoes.Find(id);
            if (nivelAcademico == null)
            {
                return HttpNotFound();
            }
            return PartialView(nivelAcademico);
        }

        [HttpPost, ActionName("RemoverNAcademico")]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmarRemocao(int id)
        {
            try
            {
                NivelAcademico nivelAcademico = db.NivelAcademicoes.Find(id);
                db.NivelAcademicoes.Remove(nivelAcademico);
                db.SaveChanges();
                return Json(new { resultado = true, mensagem = "Nível académico removido com sucesso" });
            }
            catch (Exception ex)
            {
                return Json(new { resultado = false, mensagem = ex });
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
