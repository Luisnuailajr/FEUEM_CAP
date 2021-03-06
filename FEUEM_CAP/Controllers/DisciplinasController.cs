﻿using System;
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
    public class DisciplinasController : Controller
    {
        private FEUMePresenceContext db = new FEUMePresenceContext();

        // GET: Disciplinas
        public ActionResult Index()
        {
            
            return View();
        }

        public JsonResult ListarDisciplinas(string searchPhrase, int current = 1, int rowCount = 5)
        {
            string chave = Request.Form.AllKeys.Where(k => k.StartsWith("sort")).First();
            string ordenacao = Request[chave];
            string campo = chave.Replace("sort[", String.Empty).Replace("]", String.Empty);

            var disciplinas = db.Disciplinas.Join(db.Cursos, d => d.CursoId, c => c.CursoId,
                (d, c) => new
                {
                    d.DisciplinaId,
                    d.NomeDisciplina,
                    d.Ano,
                    d.Semestre,
                    c.NomeCurso
                    
                });

            int Total = disciplinas.Count();

            if (!String.IsNullOrWhiteSpace(searchPhrase))
            {
                int ano = 0;
                int.TryParse(searchPhrase, out ano);

                int semestre = 0;
                int.TryParse(searchPhrase, out semestre);

                
                

                disciplinas = disciplinas.Where("NomeDisciplina.Contains(@0) or NomeCurso.Contains(@0) or Ano == @1 or Semestre == @1",
                    searchPhrase, ano, semestre);
            }

            string campoOrdenacao = String.Format("{0} {1}", campo, ordenacao);
            var disciplinasPaginadas = disciplinas.OrderBy(campoOrdenacao).Skip((current - 1) * 5).Take(rowCount);
            return Json( new
           
                {
                    rows = disciplinasPaginadas.ToList(),
                    current = current,
                    rowCount = rowCount,
                    total = Total
                }, JsonRequestBehavior.AllowGet);

        }

        // GET: Disciplinas/DetalhesCurso/5
        public ActionResult DetalhesDisciplina(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Disciplina disciplina = db.Disciplinas.Find(id);
            if (disciplina == null)
            {
                return HttpNotFound();
            }
            return PartialView(disciplina);
        }

        // GET: Disciplinas/AdicionarDocente
        public ActionResult AdicionarDisciplina()
        {
            ViewBag.CursoId = new SelectList(db.Cursos, "CursoId", "NomeCurso");
            return PartialView();
        }

        // POST: Disciplinas/AdicionarDocente
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdicionarDisciplina([Bind(Include = "DisciplinaId,NomeDisciplina,Ano,Semestre,CursoId")] Disciplina disciplina)
        {
            if (ModelState.IsValid)
            {
                db.Disciplinas.Add(disciplina);
                db.SaveChanges();

                return Json(new { resultado = true, mensagem = "Disciplina gravada com sucesso!" });
            }
            {
                IEnumerable<ModelError> erros = ModelState.Values.SelectMany(item => item.Errors);

                return Json(new { resultado = false, mensagem = erros });

            }

        }

        // GET: Disciplinas/EditarCurso/5
        public ActionResult EditarDisciplina(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Disciplina disciplina = db.Disciplinas.Find(id);
            if (disciplina == null)
            {
                return HttpNotFound();
            }
            ViewBag.CursoId = new SelectList(db.Cursos, "CursoId", "NomeCurso", disciplina.CursoId);
            return PartialView(disciplina);
        }

        // POST: Disciplinas/EditarCurso/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarDisciplina([Bind(Include = "DisciplinaId,NomeDisciplina,Ano,Semestre,CursoId")] Disciplina disciplina)
        {
            if (ModelState.IsValid)
            {
                db.Entry(disciplina).State = EntityState.Modified;
                db.SaveChanges();

                return Json(new { resultado = true, mensagem = "Disciplina actualizada com sucesso!" });
            }
            else

            {
                IEnumerable<ModelError> erros = ModelState.Values.SelectMany(item => item.Errors);

                return Json(new { resultado = false, mensagem = erros });

            }

        }

        // GET: Disciplinas/RemoverCurso/5
        public ActionResult RemoverDisciplina(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Disciplina disciplina = db.Disciplinas.Find(id);
            if (disciplina == null)
            {
                return HttpNotFound();
            }
            return PartialView(disciplina);
        }

        // POST: Disciplinas/RemoverCurso/5
        [HttpPost, ActionName("RemoverCurso")]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmarRemocao(int id)
        {

            try
            {
                Disciplina disciplina = db.Disciplinas.Find(id);
                db.Disciplinas.Remove(disciplina);
                db.SaveChanges();
                return Json(new { resultado = true, mensagem = "Disciplina removida com sucesso!" });
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
