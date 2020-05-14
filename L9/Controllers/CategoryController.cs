using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using L9.Models;
using Microsoft.AspNetCore.Mvc;

namespace L9.Controllers
{
    public class CategoryController : Controller
    {
        private readonly Ramadan2020Context ORM = null;

        public CategoryController(Ramadan2020Context _ORM)
        {
            ORM = _ORM;
        }



        public async Task<IActionResult> Detail(int Id)
        {
            //Category C =  ORM.Category.FirstOrDefault(abc => abc.Id == Id);

            bool IsExist = ORM.Category.Any(a => a.Id == Id);

            if(IsExist)
            {
                return View(await ORM.Category.FindAsync(Id));
            }
            else
            {
                ViewBag.Message = "Required category does not exist.";
                return View();
            }

            
        }

        public IActionResult Delete(int Id)
        {
            Category C = ORM.Category.Find(Id);

            if (C != null)
            {
                ORM.Category.Remove(C);
                ORM.SaveChanges();
                TempData["Message"] = C.Name + " deleted successfully!";
                return RedirectToAction("AllCategories");
            }

            return View();
        }



        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,ShortDescription,LongDescription")] Category C)
        {
            if (ModelState.IsValid)
            {
                C.Status = "Active";
                C.CreatedDate = DateTime.Now;
                C.CreatedBy = "Admin";


                ORM.Category.Add(C);

                await ORM.SaveChangesAsync();

                ViewBag.Message = C.Name + " Category successfully Saved.";

            }
            else
            {
                ViewBag.Message = C.Name + " unable to save, please try again.";
                return View();
            }

            return RedirectToAction("AllCategories");
        }


        public IActionResult AllCategories(string SearchQuery)
        {
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
            }

            if (string.IsNullOrEmpty(SearchQuery))
            {
                return View(ORM.Category.ToList<Category>());
            }
            else
            {
                return View(ORM.Category.Where(a=>a.Name.Contains(SearchQuery)).ToList<Category>());
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {            
            return View(await ORM.Category.FindAsync(Id));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Category C)
        {
            //diff betweeen find and any
         Category Found =    ORM.Category.Find(C.Id);

            bool IsExist = ORM.Category.Any(abc => abc.Id == C.Id);


            if(Found==null)
            {
                //built in fun in your controller base.
                return NotFound();
            }




            ORM.Category.Update(C);
            ORM.SaveChanges();

            return RedirectToAction("AllCategories");
        }


        public string LoadAd1()
        {

            System.Threading.Thread.Sleep(5000);

            string Ad1 = "<iframe width = '560' height = '315' src = 'https://www.youtube.com/embed/KW-bqPDTY2k' frameborder = '0' allow = 'accelerometer; autoplay; encrypted-media; gyroscope; picture-in-picture' allowfullscreen ></iframe>";

            return Ad1;
        }


        public string LoadAd2()
        {

            System.Threading.Thread.Sleep(10000);

            string Ad1 = "<iframe width = '560' height = '315' src = 'https://www.youtube.com/embed/KW-bqPDTY2k' frameborder = '0' allow = 'accelerometer; autoplay; encrypted-media; gyroscope; picture-in-picture' allowfullscreen ></iframe>";

            return Ad1;
        }


    }
}