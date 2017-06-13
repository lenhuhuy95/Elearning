using Model.EF;
using Model.ViewModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class OrderDao
    {
        ElearningDbContext db = null;
        public OrderDao()
        {
            db = new ElearningDbContext();
        }
        public long Insert(Order order)
        {
            db.Orders.Add(order);
            db.SaveChanges();
            return order.OderID;
        }

        //lấy ds đơn hàng hoàn tất và search
        public IEnumerable<Order> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<Order> model = db.Orders;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Status == true && (x.ShipName.Contains(searchString) || x.ShipMobile.Contains(searchString) || x.ShipAddress.Contains(searchString)));
            }

            return model.OrderByDescending(x => x.OderID).Where(x => x.Status == true).ToPagedList(page, pageSize);
        }

        //lấy ds đơn hàng đang xử lý và search
        public IEnumerable<Order> ListAllPaging1(string searchString, int page, int pageSize)
        {
            IQueryable<Order> model = db.Orders;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Status == false && (x.ShipName.Contains(searchString) || x.ShipMobile.Contains(searchString) || x.ShipAddress.Contains(searchString)));
            }

            return model.OrderByDescending(x => x.OderID).Where(x => x.Status == false).ToPagedList(page, pageSize);
        }

        //lấy ds chi tiết đơn hàng hoàn tất và search
        public IEnumerable<OrderDetail> ListAllPagingDetail(string searchString, int page, int pageSize, long orderID)
        {
            IQueryable<OrderDetail> model = db.OrderDetails;

            model = model.Where(x => x.OderID == orderID);

            return model.OrderByDescending(x => x.OderID).ToPagedList(page, pageSize);
        }


        //thay đổi trạng thái của order
        public bool ChangeStatus(long id)
        {
            var order = db.Orders.Find(id);
            order.Status = !order.Status;
            db.SaveChanges();
            return order.Status.Value;
        }

        //lấy id user của order
        public long ViewIDUser(long id)
        {
            var order = db.Orders.Find(id);
            return order.CustomerID.Value;
        }

        //lấy list những course của order
        public List<OrderDetail> ListCourseOrderDetail(long id)
        {
            return db.OrderDetails.Where(x => x.OderID == id).OrderByDescending(x => x.OderID).ToList();
        }


        //trả ds học viên của khóa học
        public IEnumerable<DetailOrder_Course> ListOrderDetaiCourse(long id, string searchString, int page, int pageSize)
        {


            var ds = (from a in db.OrderDetails
                      join b in db.Courses
                      on a.CourseID equals b.CourseID
                      where a.OderID == id
                      select new DetailOrder_Course()
                      {
                          CourseID = a.CourseID,
                          OderID = a.OderID,
                          Cost = a.Cost,
                          CourseName = b.CourseName,
                         
                      }).ToList();

            return ds.ToPagedList(page, pageSize);
        }

    }
}
