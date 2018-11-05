using PDW.Models.DB;
using PDW.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PDW.Models.EntityManager
{
    public class UserManager
    {
        //Adicionar user à base de dados
        public void AddUserAccount(UserSignUpView user)
        {
            using (DemoDBEntities db = new DemoDBEntities())
            {
                SYSUser SU = new SYSUser();
                SU.LoginName = user.LoginName;
                SU.PasswordEncryptedText = user.Password;
                SU.RowCreatedSYSUserID = user.SYSUserID > 0 ? user.SYSUserID : 1;
                SU.RowModifiedSYSUserID = user.SYSUserID > 0 ? user.SYSUserID : 1;
                SU.RowCreatedDateTime = DateTime.Now;
                SU.RowModifiedDateTime = DateTime.Now;

                db.SYSUsers.Add(SU);
                db.SaveChanges();

                SYSUserProfile SUP = new SYSUserProfile();
                SUP.SYSUserID = SU.SYSUserID;
                SUP.FirstName = user.FirstName;
                SUP.LastName = user.LastName;
                SUP.Gender = user.Gender;
                SUP.RowCreatedSYSUserID = user.SYSUserID > 0 ? user.SYSUserID : 1;
                SUP.RowModifiedSYSUserID = user.SYSUserID > 0 ? user.SYSUserID : 1;
                SUP.RowCreatedDateTime = DateTime.Now;
                SUP.RowModifiedDateTime = DateTime.Now;

                db.SYSUserProfiles.Add(SUP);
                db.SaveChanges();

                if (user.LOOKUPRoleID > 0)
                    if (user.LOOKUPRoleID > 0)
                    {
                        SYSUserRole SUR = new SYSUserRole();
                        SUR.LOOKUPRoleID = user.LOOKUPRoleID;
                        SUR.SYSUserID = user.SYSUserID;
                        SUR.IsActive = true;
                        SUR.RowCreatedSYSUserID = user.SYSUserID > 0 ? user.SYSUserID : 1;
                        SUR.RowModifiedSYSUserID = user.SYSUserID > 0 ? user.SYSUserID : 1;
                        SUR.RowCreatedDateTime = DateTime.Now;
                        SUR.RowModifiedDateTime = DateTime.Now;

                        db.SYSUserRoles.Add(SUR);
                        db.SaveChanges();
                    }
            }
        }

        //verifica se existe na base de dados
        public bool IsLoginNameExist(string loginName)
        {
            using (DemoDBEntities db = new DemoDBEntities())
            {
                return db.SYSUsers.Where(o => o.LoginName.Equals(loginName)).Any();
            }
        }

        public string GetUserPassword(string loginName)
        {
            using(DemoDBEntities db = new DemoDBEntities())
            {
                var user = db.SYSUsers.Where(o => o.LoginName.ToLower().Equals(loginName));
                if (user.Any())
                {
                    return user.FirstOrDefault().PasswordEncryptedText;
                }
                else
                {
                    return string.Empty;
                }
                
            }
        }

        public static implicit operator ModelState(UserManager v)
        {
            throw new NotImplementedException();
        }
    }
}