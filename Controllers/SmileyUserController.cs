using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Data;
using System.Security.Claims;
using FYP_Smiley.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;
using System.Dynamic;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace FYP_Smiley.Controllers
{
    public class SmileyUserController : Controller
    {
        private const string AUTHSCHEME = "UserSecurity";
        private const string LOGIN_SQL =
           @"SELECT * FROM Smiley_User 
            WHERE user_id = '{0}' 
              AND Password = HASHBYTES('SHA1', '{1}')";

        private const string LASTLOGIN_SQL =
           @"UPDATE Smiley_User SET last_login=GETDATE() 
                        WHERE user_id='{0}'";

        private const string ROLE_COL = "role";
        private const string NAME_COL = "full_name";

        private const string REDIRECT_CNTR = "Home";
        private const string REDIRECT_ACTN = "Index";

        private const string LOGIN_VIEW = "Login";

        private AppDbContext _dbContext;

        public SmileyUserController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [Authorize]
        public IActionResult Index()
        {
            DbSet<SmileyUser> dbs = _dbContext.SmileyUser;
            List<SmileyUser> model = dbs.ToList();

            return View(model);
        }

        [Authorize]
        public IActionResult Create()
        {
            DbSet<SmileyUser> dbs = _dbContext.SmileyUser;
            var lstPokes = dbs.ToList();
            ViewData["pokes"] = new SelectList(lstPokes, "user_id", "full_Name");
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(SmileyUser smileyUser)
        {
            if (ModelState.IsValid)
            {
                DbSet<SmileyUser> dbs = _dbContext.SmileyUser;
                dbs.Add(smileyUser);

                if (_dbContext.SaveChanges() == 1)
                    TempData["Msg"] = "New Smiley User added!";
                else
                    TempData["Msg"] = "Failed to update database!";
            }
            else
            {
                TempData["Msg"] = "Invalid information entered";
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult Update(string id)
        {
            DbSet<SmileyUser> dbs = _dbContext.SmileyUser;
            SmileyUser tOrder = dbs.Where(mo => mo.UserId == id).FirstOrDefault();

            return View(tOrder);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Update(SmileyUser smileyUser)
        {
            if (ModelState.IsValid)
            {
                DbSet<SmileyUser> dbs = _dbContext.SmileyUser;
                SmileyUser tOrder = dbs.Where(mo => mo.UserId == smileyUser.UserId)
                                     .FirstOrDefault();

                if (tOrder != null)
                {
                    tOrder.UserId = smileyUser.UserId;
                    tOrder.FullName = smileyUser.FullName;
                    tOrder.Email = smileyUser.Email;
                    tOrder.Password = smileyUser.Password;
                    tOrder.Voicefile = smileyUser.Voicefile;
                    tOrder.Picfile = smileyUser.Picfile;
                    tOrder.Role = smileyUser.Role;
                    tOrder.PhoneNum = smileyUser.PhoneNum;

                    if (_dbContext.SaveChanges() == 1)
                        TempData["Msg"] = "Smiley User updated!";
                    else
                        TempData["Msg"] = "Failed to update database!";
                }
                else
                {
                    TempData["Msg"] = "Smiley User not found!";
                    return RedirectToAction("Index");
                }
            }

            else
            {
                TempData["Msg"] = "Invalid information entered";
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult Delete(string id)
        {
            DbSet<SmileyUser> dbs = _dbContext.SmileyUser;

            SmileyUser tOrder = dbs.Where(mo => mo.UserId == id).FirstOrDefault();

            if (tOrder != null)
            {
                dbs.Remove(tOrder);

                if (_dbContext.SaveChanges() == 1)
                    TempData["Msg"] = "Smiley User deleted!";
                else
                    TempData["Msg"] = "Failed to update database!";
            }
            else
            {
                TempData["Msg"] = "Smiley User not found!";
            }
            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            TempData["ReturnUrl"] = returnUrl;
            return View(LOGIN_VIEW);
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(LoginUser user)
        {
            if (!AuthenticateUser(user.UserId, user.Password, out ClaimsPrincipal principal))
            {
                ViewData["Message"] = "Incorrect User Id or Password";
                ViewData["MsgType"] = "warning";
                return View(LOGIN_VIEW);
            }
            else
            {
                HttpContext.SignInAsync(
                   AUTHSCHEME,
                   principal,
               new AuthenticationProperties
               {
                   IsPersistent = false
               });

                int num_affected = _dbContext.Database.ExecuteSqlInterpolated($"UPDATE Smiley_User SET last_login=GETDATE() WHERE user_id = {user.UserId}");

                if (TempData["returnUrl"] != null)
                {
                    string returnUrl = TempData["returnUrl"].ToString();
                    if (Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);
                }

                return RedirectToAction(REDIRECT_ACTN, REDIRECT_CNTR);
            }
        }

        [Authorize]
        public IActionResult Logoff(string returnUrl = null)
        {
            HttpContext.SignOutAsync(AUTHSCHEME);
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            return RedirectToAction(REDIRECT_ACTN, REDIRECT_CNTR);
        }

        [AllowAnonymous]
        public IActionResult Forbidden()
        {
            return View();
        }

        private bool AuthenticateUser(string uid, string pw, out ClaimsPrincipal principal)
        {

            DbSet<SmileyUser> dbs = _dbContext.SmileyUser;
            var pw_bytes = System.Text.Encoding.ASCII.GetBytes(pw);

            SmileyUser smileyUser = dbs.FromSqlInterpolated($"SELECT * FROM Smiley_User WHERE user_id = {uid} AND password = HASHBYTES('SHA1', {pw_bytes})").FirstOrDefault();

            principal = null;

            if (smileyUser != null)
            {
                principal =
                   new ClaimsPrincipal(
                      new ClaimsIdentity(
                         new Claim[] {
                        new Claim(ClaimTypes.NameIdentifier, smileyUser.UserId),
                        new Claim(ClaimTypes.Name, smileyUser.FullName),
                        new Claim(ClaimTypes.Role, smileyUser.Role)
                         }, "Basic"
                      )
                   );
                return true;
            }
            return false;
        }

        [Authorize]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult ChangePassword(PasswordUpdate pu)
        {
            var userid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var sql = String.Format($"UPDATE Smiley_User SET password = HASHBYTES('SHA1', '{pu.NewPassword}') WHERE user_id='{userid}' AND password = HASHBYTES('SHA1', '{pu.CurrentPassword}')");

            if (_dbContext.Database.ExecuteSqlCommand(sql) == 1)
                ViewData["Msg"] = "Password successfully updated!";
            else
                ViewData["Msg"] = "Failed to update password!";
            return View();
        }

        [Authorize]
        public JsonResult VerifyCurrentPassword(string CurrentPassword)
        {
            DbSet<SmileyUser> dbs = _dbContext.SmileyUser;
            var userid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var pw_bytes = System.Text.Encoding.ASCII.GetBytes(CurrentPassword);

            SmileyUser user = dbs.FromSqlInterpolated($"SELECT * FROM Smiley_User WHERE user_id = {userid} AND password = HASHBYTES('SHA1', {pw_bytes})").FirstOrDefault();

            if (user != null)
                return Json(true);
            else
                return Json(false);
        }

        [Authorize]
        public JsonResult VerifyNewPassword(string NewPassword)
        {
            DbSet<SmileyUser> dbs = _dbContext.SmileyUser;
            var userid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var pw_bytes = System.Text.Encoding.ASCII.GetBytes(NewPassword);

            SmileyUser user = dbs.FromSqlInterpolated($"SELECT * FROM Smiley_User WHERE user_id = {userid} AND password = HASHBYTES('SHA1', {pw_bytes})").FirstOrDefault();
            if (user == null)
                return Json(true);
            else
                return Json(false);
        }

        [Authorize]
        public IActionResult Print(int id)
        {
            DbSet<SmileyUser> dbs = _dbContext.SmileyUser;
            List<SmileyUser> model = dbs.ToList();

            if (model != null)
                return new ViewAsPdf(model)
                {
                    PageSize = Rotativa.AspNetCore.Options.Size.B5,
                    PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
                };
            else
            {
                TempData["Msg"] = "Record not found!";
                return RedirectToAction("Index");
            }

        }

    }
}
