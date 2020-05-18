using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using L9.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Migrations.Design;
using Microsoft.Extensions.Hosting;

namespace L9.Controllers
{
    public class CategoryController : Controller
    {
        private readonly Ramadan2020Context ORM = null;
        private readonly IHostEnvironment ENV = null;
        public CategoryController(Ramadan2020Context _ORM, IHostEnvironment _ENV)
        {
            ORM = _ORM;
            ENV = _ENV;
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


        //public IActionResult Login(string UName, string Pwd)
        //{
        //  User U =   ORM.SystemUsers.Where(abc.UName == UName && abc.Pwd == Pwd).firstordefault();

        //    if(U!=null)
        //        return
        //}


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,ShortDescription,LongDescription")] Category C, IFormFile CImage)
        {
            string FileName = "";

            if (CImage != null)
            {
                string FTPFolderPath = ENV.ContentRootPath + "\\wwwroot\\Images\\CategoryImages";

                string FileExt = Path.GetExtension(CImage.FileName);

                FileName = Guid.NewGuid() + FileExt;
                string FinalFilePath = FTPFolderPath + "\\" + FileName;





                FileStream FS = new FileStream(FinalFilePath, FileMode.Create);

                CImage.CopyTo(FS);

            }



           // CImage.CopyTo(new FileStream(ENV.ContentRootPath + "\\wwwroot\\Images\\CategoryImages\\" + Guid.NewGuid() + Path.GetExtension(CImage.FileName), FileMode.Create);






            if (ModelState.IsValid)
            {
                C.Status = "Active";
                C.CreatedDate = DateTime.Now;
                C.CreatedBy = "Admin";
                C.Image = FileName;

                ORM.Category.Add(C);

                await ORM.SaveChangesAsync();

                ViewBag.Message = C.Name + " Category successfully Saved.";


                HttpContext.Session.SetString("CName",C.Name);

               
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


        public string DeleteAjax(int id)
        {
            try
            {
                Category C = ORM.Category.Find(id);

                if (C != null)
                {
                    ORM.Category.Remove(C);
                    ORM.SaveChanges();
                    return "1";
                }
            }
            catch
            {
                return "0";
            }
            finally
            {

            }


            return "0";
        }


    }
}