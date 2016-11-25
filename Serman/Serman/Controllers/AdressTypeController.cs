using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Serman.Models;

namespace Serman.Controllers
{
    public class AdressTypeController : Controller
    {
        private SERMANEntities db = new SERMANEntities();

      
        //
        // GET: /AdressType/
        public ActionResult Index()
        {            
            DateTime date =DateTime.Now;
            return Json(db.AdressTypes.Select(x => new
            {
                Id = x.AdressTypeId,
                Name = x.Name
            }).ToList()
             .Select(x => new { Data=x, Date = date, Tick =date.Ticks, Binary= date.ToBinary()}), JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /AdressType/Details/5

        public ActionResult Details(int id = 0)
        {
            AdressType adresstype = db.AdressTypes.Find(id);
            if (adresstype == null)
            {
                return HttpNotFound();
            }
            return Json(new { Id = adresstype.AdressTypeId, Name = adresstype.Name }, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /AdressType/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /AdressType/Create

        [HttpPost]
        public ActionResult Create(AdressType adresstype)
        {
            if (ModelState.IsValid)
            {
                db.AdressTypes.Add(adresstype);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(adresstype);
        }

        //
        // GET: /AdressType/Edit/5

        public ActionResult Edit(int id = 0)
        {
            AdressType adresstype = db.AdressTypes.Find(id);
            if (adresstype == null)
            {
                return HttpNotFound();
            }
            return View(adresstype);
        }

        //
        // POST: /AdressType/Edit/5

        [HttpPost]
        public ActionResult Edit(AdressType adresstype)
        {
            if (ModelState.IsValid)
            {
                db.Entry(adresstype).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(adresstype);
        }

        //
        // GET: /AdressType/Delete/5

        public ActionResult Delete(int id = 0)
        {
            AdressType adresstype = db.AdressTypes.Find(id);
            if (adresstype == null)
            {
                return HttpNotFound();
            }
            return View(adresstype);
        }

        //
        // POST: /AdressType/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            AdressType adresstype = db.AdressTypes.Find(id);
            db.AdressTypes.Remove(adresstype);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}