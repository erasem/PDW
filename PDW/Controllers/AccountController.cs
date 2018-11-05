using PDW.Models.ViewModel;
using System.Web.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PDW.Models.EntityManager;
using PDW.Security;

namespace PDW.Controllers
{
    public class AccountController : Controller
    {
        //retorna SignUp.cshtml
        public ActionResult SignUp()
        {
            return View();
        }

        //retorna 
        [HttpPost]
        public ActionResult SignUp(UserSignUpView USV)
        {
            if (ModelState.IsValid)
            {
                UserManager UM = new UserManager();
                if (!UM.IsLoginNameExist(USV.LoginName))
                {
                    UM.AddUserAccount(USV);
                    FormsAuthentication.SetAuthCookie(USV.FirstName, false);
                    return RedirectToAction("Welcome", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Login Name already taken.");
                }

            }
            return View();
        }

        public ActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogIn(UserLoginView ULV, string returnUrl)
        {
            if (ModelState.IsValid) 
            {
                UserManager UM = new UserManager();
                string password = UM.GetUserPassword(ULV.LoginName);

                if (string.IsNullOrEmpty(password))
                {
                    ModelState.AddModelError("", "The user login or password provided is incorrect");
                }
                else
                {
                    if (ULV.Password.Equals(password))
                    {
                        FormsAuthentication.SetAuthCookie(ULV.LoginName, false);
                        return RedirectToAction("Welcome", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "The password provided is incorrect");
                    }
                }
            }
            return View(ULV);
        }

        //remover o ticket de autenticação do browser
        [Authorize]
        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            //redirecionar para a pagina Index após o SignOut
            return RedirectToAction("Index", "Home");
        }

    }

    //controlador de redirect
    public class HomeController : Controller
    {
        //default redirect
        public ActionResult Index()
        {
            return View();
        }

        //redirect after login
        [Authorize]  //so permite ver a pagina se estiver autenticado
        public ActionResult Welcome()
        {
            return View();
        }
        //To support multiple role access, just add another role name by separating it with comma for example [AuthorizeRoles(“Admin”,”Manager”)]. 
        [AuthorizeRoles("Admin")]
        public ActionResult AdminOnly()
        {
            return View();
        }

        public ActionResult UnAuthorized()
        {
            return View();
        }
    }
}