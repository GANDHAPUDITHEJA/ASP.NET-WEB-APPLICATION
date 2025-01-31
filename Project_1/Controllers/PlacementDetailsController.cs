using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Project_1.Models;

namespace Project_1.Controllers
{
    public class PlacementDetailsController : Controller
    {
        private PlacementEntities db = new PlacementEntities();

        // GET: PlacementDetails
        public ActionResult Index()
        {
            return View(db.PlacementDetails.ToList());
        }

        // GET: PlacementDetails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PlacementDetail placementDetail = db.PlacementDetails.Find(id);
            if (placementDetail == null)
            {
                return HttpNotFound();
            }
            return View(placementDetail);
        }

        // GET: PlacementDetails/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PlacementDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PlacementId,AcademicYear,CompanyName,RegistrationStart,RegistrationEnd,Criteria")] PlacementDetail placementDetail)
        {
            if (ModelState.IsValid)
            {
                db.PlacementDetails.Add(placementDetail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(placementDetail);
        }

        // GET: PlacementDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PlacementDetail placementDetail = db.PlacementDetails.Find(id);
            if (placementDetail == null)
            {
                return HttpNotFound();
            }
            return View(placementDetail);
        }

        // POST: PlacementDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PlacementId,AcademicYear,CompanyName,RegistrationStart,RegistrationEnd,Criteria")] PlacementDetail placementDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(placementDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(placementDetail);
        }

        // GET: PlacementDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PlacementDetail placementDetail = db.PlacementDetails.Find(id);
            if (placementDetail == null)
            {
                return HttpNotFound();
            }
            return View(placementDetail);
        }

        // POST: PlacementDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PlacementDetail placementDetail = db.PlacementDetails.Find(id);
            db.PlacementDetails.Remove(placementDetail);
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
