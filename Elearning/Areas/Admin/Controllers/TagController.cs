using Model.Dao;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Elearning.Areas.Admin.Controllers
{
    public class TagController : Controller
    {
        // GET: Admin/Tag
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SelectTag()
        {
            return View();
        }

        [HttpGet]
        [HasCredential(RoleID = "ADD_TAG")]
        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [HasCredential(RoleID = "ADD_TAG")]
        public ActionResult Create(Tag tag)
        {

            //Xử lý Insert dữ liệu vào Database
            if (ModelState.IsValid)
            {
                var dao = new TagDao();
                long id = dao.Insert(tag);
                if (id > 0)
                {
                    return RedirectToAction("Create", "Tag");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm thẻ thành công");
                }
            }

            return View();
        }

        //Hàm sửa Tag lấy thông tin Tag theo id
        [HttpGet]
        [HasCredential(RoleID = "EDIT_TAG")]
        public ActionResult EditTag(long id)
        {
            var tag = new TagDao().ViewDetail(id);
            return View(tag);
        }

        //Update Tags
        [HttpPost]
        [HasCredential(RoleID = "EDIT_TAG")]
        public ActionResult EditTag(Tag tag)
        {
            if (ModelState.IsValid)
            {
                var dao = new TagDao();

                var result = dao.Update(tag);
                if (result)
                {
                    return RedirectToAction("Create" , "Tag");
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật chương không thành công");
                }
            }

            return View("EditSection");
        }

        // view ListTag lấy và hiển thị ds các tag
        [HasCredential(RoleID = "VIEW_TAG")]
        public ActionResult ListTag(string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new TagDao();
            var model = dao.ListAllPaging(searchString, page, pageSize);
            ViewBag.SearchString = searchString;

            return View(model);
        }

        //xóa Tag bằng [HttpDelete]
        [HttpDelete]
        [HasCredential(RoleID = "DELETE_TAG")]
        public ActionResult DeleteTag(long id)
        {
            new TagDao().DeleteTag(id);
            return RedirectToAction("ListTag");
        }
    }
}