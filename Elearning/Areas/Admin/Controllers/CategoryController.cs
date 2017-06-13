using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.EF;
using Model.Dao;
using System.Text.RegularExpressions;

namespace Elearning.Areas.Admin.Controllers
{
    public class CategoryController : BaseController
    {
        // GET: Admin/Category
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [HasCredential(RoleID = "ADD_CATEGORY")]
        public ActionResult Create()
        {
            /*SetViewBag();*/ //Khi chưa post thì vẫn còn hiện thị đc danh sách Category với ParentID = Null
            ViewBag.Categoryparent = new CategoryDao().Categoryparent();
            return View();
        }


        [HttpPost]
        [HasCredential(RoleID = "ADD_CATEGORY")]
        public ActionResult Create(CourseCategory category, string categoryidselected)
        {

            //chuyển đổi tiêu đề sáng slug
            category.MetaTitle = ConvertToUnSign(category.CategoryName);

            if (category.DisplayOrder == 0)
            {
                category.ParentID = null;
            }

            category.DisplayOrder = 1;
            category.Status = true;
            category.CreateDate = DateTime.Now;

            //Truyền CategoryID sang cho ParentID
            if (categoryidselected == "null")
            {
                category.ParentID = null;
            }
            else
            {
                category.ParentID = long.Parse(categoryidselected);
            }

            //Xử lý Insert dữ liệu vào Database
            if (ModelState.IsValid)
            {
                var dao = new CategoryDao();
                long id = dao.Insert(category);
                if (id > 0)
                {
                    return RedirectToAction("Create", "Category");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm Category thành công");
                }
            }

            return View();
        }

        //Hàm sửa Category
        [HttpGet]
        [HasCredential(RoleID = "EDIT_CATEGORY")]
        public ActionResult EditCategory(long id)
        {
            var category = new CourseCategoryDao().ViewDetail(id);
            ViewBag.EditCategoryparent = new CategoryDao().EditCategoryparent();
            return View(category);
        }

        //update category
        [HttpPost]
        [HasCredential(RoleID = "EDIT_CATEGORY")]
        public ActionResult EditCategory(CourseCategory category, string categoryidselected)
        {

            //Chuyển đổi tiêu đề sáng slug
            category.MetaTitle = ConvertToUnSign(category.CategoryName);

            //Truyền CategoryID sang cho ParentID thông qua input được gán từ View
            if (categoryidselected == "")
            {
                category.ParentID = null;
            }
            else
            {
                try
                {
                    category.ParentID = long.Parse(categoryidselected);
                }
                catch (Exception e)
                {

                }
            }

            if (ModelState.IsValid)
            {
                var dao = new CourseCategoryDao();

                var result = dao.Update(category);
                if (result)
                {
                    SetAlert("Sửa chuyên mục thành công", "success");
                    return RedirectToAction("Create", "Category");
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật chuyên mục không thành công");
                }
            }

            return View("EditCategory");
        }

        //Hàm chuyển đổi tiêu đề sáng slug
        public static string ConvertToUnSign(string text)
        {
            text = text.ToLower();
            for (int i = 33; i < 48; i++)
            {
                text = text.Replace(((char)i).ToString(), "");
            }

            for (int i = 58; i < 65; i++)
            {
                text = text.Replace(((char)i).ToString(), "");
            }

            for (int i = 91; i < 97; i++)
            {
                text = text.Replace(((char)i).ToString(), "");
            }
            for (int i = 123; i < 127; i++)
            {
                text = text.Replace(((char)i).ToString(), "");
            }
            text = text.Replace(" ", "-");
            Regex regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");
            string strFormD = text.Normalize(System.Text.NormalizationForm.FormD);
            return regex.Replace(strFormD, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }

        // view ListCategory; lấy và hiển thị ds các chuyên mục
        [HasCredential(RoleID = "VIEW_CATEGORY")]
        public ActionResult ListCategory(string searchString, int page = 1, int pageSize = 10)
        {

            var dao = new CategoryDao();
            var model = dao.ListAllCategory(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(model);

        }

        //Truyền CategoryID theo trường ứng với table để xử lý Dropdownlist
        //public void SetViewBag(long? selectedId = null)
        //{
        //    var dao = new CategoryDao();
        //    ViewBag.CategoryID = new SelectList(dao.ListParentAll(), "CategoryID", "CategoryName", selectedId);
        //}

        //thay đổi trạng thái Category bằng categoryControllerChangeStatus.js
        [HttpPost]
        [HasCredential(RoleID = "STATUS_CATEGORY")]
        public JsonResult ChangeStatusCategory(long id)
        {
            var result = new CategoryDao().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }

        //xóa category
        [HttpDelete]
        [HasCredential(RoleID = "DELETE_CATEGORY")]
        public ActionResult DeleteCategory(long id)
        {
            new CourseCategoryDao().DeleteCategory(id);
            return RedirectToAction("ListCategory", "Category");
        }
    }
}