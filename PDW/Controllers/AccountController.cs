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

        //apenas admins podem invocar este metodo, chamao GetUserDataView passando o loginName e retorna o resultado na PartialView
        [AuthorizeRoles("Admin")]
        public ActionResult ManageUserPartial(string status = "")
        {
            if (User.Identity.IsAuthenticated)
            {
                string loginName = User.Identity.Name;
                UserManager UM = new UserManager();
                UserDataView UDV = UM.GetUserDataView(loginName);

                string message = string.Empty;
                if (status.Equals("update"))
                    message = "Update Successful";
                else if (status.Equals("delete"))
                    message = "Delete Successful";

                ViewBag.Message = message;

                return PartialView(UDV);
            }

            return RedirectToAction("Index", "Home");
        }

        //coleciona informacao enviada da view para dar update 
        [AuthorizeRoles("Admin")]
        public ActionResult UpdateUserData(int userID, string loginName, string password, string firstName, string lastName, string gender, int roleID = 0)
        {
            UserProfileView UPV = new UserProfileView();
            UPV.SYSUserID = userID;
            UPV.LoginName = loginName;
            UPV.Password = password;
            UPV.FirstName = firstName;
            UPV.LastName = lastName;
            UPV.Gender = gender;

            if (roleID > 0)
                UPV.LOOKUPRoleID = roleID;

            UserManager UM = new UserManager();
            UM.UpdateUserAccount(UPV);

            return Json(new { success = true });
        }

        public ActionResult UnAuthorized()
        {
            return View();
        }
    }
}