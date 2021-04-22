using JobManagerMVC_2.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace JobManagerMVC_2.Controllers
{
    public class JobController:Controller
    {
        public string GetAll()
        {
            string text = "";
            using (var db = new TaskDBContext())
            {
                List<Job> allJobs = db.Job.Include("InverseParent").ToList();
                text = JsonConvert.SerializeObject(allJobs, Formatting.Indented);
            }

            return text;
        }
        public string GetById(int id)
        {
            string text = "";
            using (var db = new TaskDBContext())
            {
                Job job = db.Job.FirstOrDefault(x => x.Id == id);
                text = JsonConvert.SerializeObject(job);
            }
            return text;
        }
        [HttpPost]
        public ActionResult Create( Job job)
        {
            if (ModelState.IsValid)
            {
                using (var db = new TaskDBContext())
                {
                    db.Job.Add(job);
                    db.SaveChanges();
                }

            }
            return RedirectToAction($"GetById/{job.Id}");
        }
        public ActionResult Delete(int id)
        {
            using(var db = new TaskDBContext())
            {
                Job job = db.Job.Find(id);
                foreach(var i in db.Job
                    .Where(x=>x.ParentId ==id))
                {
                    i.ParentId = null;
                }
                db.Job.Remove(job);
                db.SaveChanges();
                return RedirectToAction("GetAll");
            }
        }
        [HttpPost]
        public ActionResult Edit(Job job)
        {
            int id = job.Id;
            using(var db = new TaskDBContext())
            {
                if (ModelState.IsValid)
                {
                    db.Entry(job).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction($"getbyid/{job.Id}");
            }

        }

    }
}
