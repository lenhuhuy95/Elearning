using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;
using System.Web.Routing;
using Elearning.Common;

namespace Elearning
{
    public class HasCredentialAttribute : AuthorizeAttribute
    {
        public string RoleID { set; get; }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var session = (AdminLogin)HttpContext.Current.Session[Common.CommonConstants.ADMIN_SESSION];
            if (session == null)
            {
                return false;
            }

            List<string> privilegeLevels = this.GetCredentialByLoggedInUser(session.UserAdminName); // Call another method to get rights of the user from DB

            if (privilegeLevels.Contains(this.RoleID))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var session = (AdminLogin)HttpContext.Current.Session[Common.CommonConstants.ADMIN_SESSION];
            if (session == null)
            {
                filterContext.Result = new ViewResult
                {

                    ViewName = "~/Areas/Admin/Views/Login/Index.cshtml" //nếu chưa đăng nhập thì trả về trang login
                };
            }
            else
            {
                filterContext.Result = new ViewResult
                {

                    ViewName = "~/Areas/Admin/Views/Shared/LoiQuyen.cshtml" //lỗi không có quyền truy cập
                };
            }
        }
        private List<string> GetCredentialByLoggedInUser(string userName)
        {
            var credentials = (List<string>)HttpContext.Current.Session[Common.CommonConstants.SESSION_CREDENTIALS];
            return credentials;
        }
    }
}