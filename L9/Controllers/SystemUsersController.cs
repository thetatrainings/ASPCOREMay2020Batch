using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using L9.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.Extensions.Hosting;
using HeyRed.Mime;
using System.Net.Mail;
using System.Net;

namespace L9.Controllers
{
    public class SystemUsersController : Controller
    {
        private readonly Ramadan2020Context ORM;
        private readonly IHostEnvironment ENV;
        public SystemUsersController(Ramadan2020Context context, IHostEnvironment _ENV)
        {
            ORM = context;
            ENV = _ENV;
        }

        // GET: SystemUsers
        public async Task<IActionResult> Index()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Role")))
            {
                return RedirectToAction("Login");
            }

            return View(await ORM.SystemUser.ToListAsync());
        }

        // GET: SystemUsers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var systemUser = await ORM.SystemUser
                .FirstOrDefaultAsync(m => m.Id == id);
            if (systemUser == null)
            {
                return NotFound();
            }

            return View(systemUser);
        }


        // GET: SystemUsers/Login
        public IActionResult Login()
        {
            return View();
        }


        // POST: SystemUsers/Login
        [HttpPost]
        public IActionResult Login(string Username, string Password)
        {

            SystemUser LoggedInUser = ORM.SystemUser.Where(a => a.Username == Username && a.Password == Password).FirstOrDefault();


            if (LoggedInUser == null)
            {
                //for custom error messaging
                //ViewBag.Message = "Invalid Details";


                ModelState.AddModelError("", "Invalid Details, Please try again.");
                return View();
            }

            HttpContext.Session.SetString("Role", LoggedInUser.Role);
            HttpContext.Session.SetString("DisplayName", LoggedInUser.DisplayName);


            return RedirectToAction("Index");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }


        // GET: SystemUsers/Create
        public IActionResult Create()
        {
            //page leve autho.
            //if (HttpContext.Session.GetString("Role") != "Admin")
            //{
            //    return RedirectToAction("UnAuthroizedAccess");
            //}


            return View();
        }

        public FileResult DownloadCV(string FN)
        {
            string FilePath = ENV.ContentRootPath + "\\wwwroot\\Docs\\CVs\\" + FN;
           string MimeType =  MimeGuesser.GuessMimeType(FilePath);
            //MIME Type

            //FileStream FS = new FileStream();
            return File("/Docs/CVs/"+FN,MimeType,Guid.NewGuid()+Path.GetExtension(FN));
        }



        public IActionResult UnAuthroizedAccess()
        {
            return View();
        }

        // POST: SystemUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SystemUser systemUser, IFormFile UCV)
        {
            if(UCV!=null)
            {

                //in bytes
                if(UCV.Length > 1000)
                {

                }
                string FolderPath = ENV.ContentRootPath + "\\wwwroot\\Docs\\CVs\\";
                string FileName = Guid.NewGuid() + Path.GetExtension(UCV.FileName);
                FileStream FS = new FileStream(FolderPath+ FileName, FileMode.Create); ;
                UCV.CopyTo(FS);
                systemUser.CV = FileName;
            }





            if (ModelState.IsValid)
            {
                ORM.Add(systemUser);
                await ORM.SaveChangesAsync();

                //send welcome email to user
                MailMessage oEmail = new MailMessage();
                
                oEmail.From = new MailAddress("YOUR_EMAIL@gmail.com", "Theta Students");

                oEmail.To.Add(systemUser.Email);
                //oEmail.CC.Add("usman@thetasolutions.co.uk");
                //oEmail.Bcc.Add("");

                oEmail.Subject = "Welcome to Theta Solutions";

                oEmail.Body = "<p><b>Welcome, " + systemUser.DisplayName + ",</b></p><br>" +

                    "Thank you for registering your account with Theta.<br>Please find below your account details and keep it safe<br><br>" +


                    "Username: "+systemUser.Username+"<br>"+
                    "Password: "+systemUser.Password+

                    "<br style='color:red;'>Regards,<br>" +
                    "Support Team";
                    
                    ;

                oEmail.IsBodyHtml = true;

                if (System.IO.File.Exists("/Docs/CVs/" + systemUser.CV))
                {
                    oEmail.Attachments.Add(new Attachment("/Docs/CVs/" + systemUser.CV));
                }


                SmtpClient oSMTP = new SmtpClient();
                oSMTP.Host = "smtp.gmail.com";
                oSMTP.Credentials = new System.Net.NetworkCredential("YOUR_EMAIL@gmail.com", "YOUR_PASSWORD");
                oSMTP.Port = 587; //25 465
                oSMTP.EnableSsl = true;

                try
                {
                    oSMTP.Send(oEmail);
                }
                catch(Exception Ex)
                {

                }


                //send sms

                string SMSAPIURL = "https://sendpk.com/api/sms.php?username=923037226603&password=pakistan&sender=Masking&mobile="+systemUser.Mobile+"&message=ThetaSolutions";


                WebRequest request = HttpWebRequest.Create(SMSAPIURL);
                WebResponse response = request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string urlText = reader.ReadToEnd(); // it takes the response from your url. now you can use as your need  
                
                if(urlText.Contains("OK"))
                {

                }






















                return RedirectToAction(nameof(Index));
            }
            return View(systemUser);
        }

        // GET: SystemUsers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var systemUser = await ORM.SystemUser.FindAsync(id);
            if (systemUser == null)
            {
                return NotFound();
            }
            return View(systemUser);
        }

        // POST: SystemUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Status,Role,DisplayName,Username,Password")] SystemUser systemUser)
        {
            if (id != systemUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    ORM.Update(systemUser);
                    await ORM.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SystemUserExists(systemUser.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(systemUser);
        }

        // GET: SystemUsers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var systemUser = await ORM.SystemUser
                .FirstOrDefaultAsync(m => m.Id == id);
            if (systemUser == null)
            {
                return NotFound();
            }

            return View(systemUser);
        }

        // POST: SystemUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var systemUser = await ORM.SystemUser.FindAsync(id);
            ORM.SystemUser.Remove(systemUser);
            await ORM.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SystemUserExists(int id)
        {
            return ORM.SystemUser.Any(e => e.Id == id);
        }
    }
}
