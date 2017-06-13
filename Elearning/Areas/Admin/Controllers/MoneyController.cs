using Model.Dao;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Elearning.Areas.Admin.Controllers
{
    public class MoneyController : BaseController
    {
        // GET: Admin/Money
        public ActionResult Index()
        {
            return View();
        }

        //Hiển thị dánh sách các loại tiền
        [HasCredential(RoleID = "VIEW_MONEY")]
        public ActionResult ListMoney(string searchString, int page = 1, int pageSize = 10)
        {
            var model = new MoneyDao().ListAll(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(model);
        }

        //ChangeStatusMoney
        [HttpPost]
        public JsonResult ChangeStatusMoney(long id)
        {
            var result = new MoneyDao().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }

        //DeleteMoney
        [HttpDelete]
        [HasCredential(RoleID = "DELETE_MONEY")]
        public ActionResult DeleteMoney(long id)
        {
            new MoneyDao().DeleteMoney(id);
            return RedirectToAction("ListMoney", "Money");
        }

        [HttpGet]
        [HasCredential(RoleID = "ADD_MONEY")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [HasCredential(RoleID = "ADD_MONEY")]
        public ActionResult Create(Money money)
        {
            //Xử lý Insert dữ liệu vào Database
            if (ModelState.IsValid)
            {
                var dao = new MoneyDao();
                money.Status = true;
                long id = dao.Insert(money);
                if (id > 0)
                {
                    return RedirectToAction("ListMoney", "Money");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm thành công");
                }
            }
            return View();
        }

        [HttpGet]
        [HasCredential(RoleID = "EDIT_MONEY")]
        public ActionResult Edit(long id)
        {
            var model = new MoneyDao().ViewDetail(id);
            return View(model);
        }

        [HttpPost]
        [HasCredential(RoleID = "EDIT_MONEY")]
        public ActionResult Edit(Money money)
        {
            //Xử lý UploadImage
            
            if (ModelState.IsValid)
            {
                var dao = new MoneyDao();
                var result = dao.Update(money);
                if (result)
                {
                    SetAlert("Sửa thành công", "success");
                    return View("Create");
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật không thành công");
                }
            }
            return View("Create");
        }

    }
}