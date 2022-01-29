using SchoolManagement.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SchoolManagement.Controllers
{
    public class EnrollmentsController : Controller
    {
        private SchoolEntities db = new SchoolEntities();

        // GET: Enrollments
        public async Task<ActionResult> Index()
        {
            var enrollments = db.Enrollment.Include(e => e.Course).Include(e => e.Student).Include(e => e.Lecturers);
            return View(await enrollments.ToListAsync());
        }

        public PartialViewResult _enrollmentPartial(int? courseId)
        {
            //if (courseId == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}

            var enrollments = db.Enrollment.Where(q => q.CourseID == courseId)
                .Include(e => e.Course)
                .Include(e => e.Student);
            return PartialView(enrollments.ToList());
        }

        // GET: Enrollments/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enrollment enrollment = await db.Enrollment.FindAsync(id);
            if (enrollment == null)
            {
                return HttpNotFound();
            }
            return View(enrollment);
        }

        // GET: Enrollments/Create
        public ActionResult Create()
        {
            ViewBag.CourseID = new SelectList(db.Course, "CourseID", "Title");
            ViewBag.StudentID = new SelectList(db.Student, "StudentID", "LastName");
            ViewBag.LecturerId = new SelectList(db.Lecturers, "Id", "FirstName");
            return View();
        }

        // POST: Enrollments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "EnrollmentID,Grade,CourseID,StudentID,LecturerId")] Enrollment enrollment)
        {
            if (ModelState.IsValid)
            {
                db.Enrollment.Add(enrollment);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CourseID = new SelectList(db.Course, "CourseID", "Title", enrollment.CourseID);
            ViewBag.StudentID = new SelectList(db.Student, "StudentID", "LastName", enrollment.StudentID);
            ViewBag.LecturerId = new SelectList(db.Lecturers, "Id", "FirstName", enrollment.LecturerId);
            return View(enrollment);
        }

        [HttpPost]
        public async Task<JsonResult> AddStudent([Bind(Include = "CourseID,StudentID")] Enrollment enrollment)
        {
            try
            {
                var isEnrolled = db.Enrollment.Any(q => q.CourseID == enrollment.CourseID && q.StudentID == enrollment.StudentID);
                if (ModelState.IsValid && !isEnrolled)
                {
                    db.Enrollment.Add(enrollment);
                    await db.SaveChangesAsync();
                    return Json(new { IsSuccess = true, Message = "Student Added Successfully" }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { IsSuccess = false, Message = "Student is alredy enrolled" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exc)
            {
                return Json(new { IsSuccess = false, Message = "Student Was Not Added Successfully. Please Contact Your Administrator: " + exc.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Enrollments/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enrollment enrollment = await db.Enrollment.FindAsync(id);
            if (enrollment == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseID = new SelectList(db.Course, "CourseID", "Title", enrollment.CourseID);
            ViewBag.StudentID = new SelectList(db.Student, "StudentID", "LastName", enrollment.StudentID);
            ViewBag.LecturerId = new SelectList(db.Lecturers, "Id", "FirstName", enrollment.LecturerId);
            return View(enrollment);
        }

        // POST: Enrollments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "EnrollmentID,Grade,CourseID,StudentID,LecturerId")] Enrollment enrollment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(enrollment).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CourseID = new SelectList(db.Course, "CourseID", "Title", enrollment.CourseID);
            ViewBag.StudentID = new SelectList(db.Student, "StudentID", "LastName", enrollment.StudentID);
            ViewBag.LecturerId = new SelectList(db.Lecturers, "Id", "FirstName", enrollment.LecturerId);
            return View(enrollment);
        }

        // GET: Enrollments/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enrollment enrollment = await db.Enrollment.FindAsync(id);
            if (enrollment == null)
            {
                return HttpNotFound();
            }
            return View(enrollment);
        }

        // POST: Enrollments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Enrollment enrollment = await db.Enrollment.FindAsync(id);
            db.Enrollment.Remove(enrollment);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        [HttpPost]
        public JsonResult GetStudents(string term)
        {
            var students = db.Student.Select(q => new { Name = q.FirstName + " " + q.LastName, Id = q.StudentID }).Where(q => q.Name.Contains(term));            
            return Json(students, JsonRequestBehavior.AllowGet);
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
