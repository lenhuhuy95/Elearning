using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.Dao;
using Elearning.Models;
using Model.EF;

namespace Elearning.Controllers
{
    public class CartController : Controller
    {
        private const string CartSession = "CartSession"; //tạo key CartSession là hằng số  
        // GET: Cart
        public ActionResult Index()
        {
            ViewBag.MoneyUnit = new MoneyDao().ListAllMoney(); //Show dữ liệu cho tiền
            var cart = Session[CartSession];
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            return View(list);
        }

        public JsonResult DeleteAll()
        {
            Session[CartSession] = null;
            return Json(new
            {
                status = true
            });
        }

        public JsonResult Delete(long id)
        {
            var sessionCart = (List<CartItem>)Session[CartSession];
            sessionCart.RemoveAll(x => x.Course.CourseID == id);
            Session[CartSession] = sessionCart;
            return Json(new
            {
                status = true
            });
        }

        public ActionResult AddItem(long courseId)
        {
            var course = new CourseDao().ViewDetail(courseId);
            var cart = Session[CartSession];
            if (cart != null)
            {
                var list = (List<CartItem>)cart; //ép kiểu cart sang kiểu list
                if (list.Exists(x => x.Course.CourseID == courseId))
                {

                    foreach (var item in list)
                    {
                        if (item.Course.CourseID == courseId)
                        {
                            //item.Quantity += quantity; //đếm số lượng khóa học khi add vào cart
                        }
                    }
                }
                else
                {
                    //tạo mới đối tượng cart item
                    var item = new CartItem();
                    item.Course = course;
                   // item.Quantity = quantity;
                    list.Add(item);
                }
                //Gán vào session
                Session[CartSession] = list;
            }
            else //Trường hợp giỏ hàng chưa có
            {
                //tạo mới đối tượng cart item
                var item = new CartItem();
                item.Course = course;
                //item.Quantity = quantity;
                var list = new List<CartItem>();
                list.Add(item);
                //Gán vào session
                Session[CartSession] = list;
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Payment()
        {
            ViewBag.MoneyUnit = new MoneyDao().ListAllMoney(); //Show dữ liệu cho tiền
            var cart = Session[CartSession];
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            return View(list);
        }
        [HttpPost]
        public ActionResult Payment(string shipName, string mobile, string address, string email)
        {
            var session = (UserLogin)Session[Elearning.Common.CommonConstants.USER_SESSION];
            var order = new Order();
            order.CreateDate = DateTime.Now;
            order.CustomerID = session.UserID;
            order.ShipAddress = address;
            order.ShipMobile = mobile;
            order.ShipName = shipName;
            order.ShipEmail = email;
            order.Status = false;

            try
            {
                var id = new OrderDao().Insert(order);
                var cart = (List<CartItem>)Session[CartSession];
                var detailDao = new OrderDetailDao();
                foreach (var item in cart)
                {
                    var orderDetail = new OrderDetail();
                    orderDetail.CourseID = item.Course.CourseID;
                    orderDetail.OderID = id;
                    orderDetail.Cost = item.Course.Cost;
                    detailDao.Insert(orderDetail);

                }
            }
            catch (Exception ex)
            {
                //ghi log
                return Redirect("/loi-thanh-toan");
            }
            return Redirect("/hoan-thanh");
        }

        public ActionResult Success()
        {
            return View();
        }
    }
}