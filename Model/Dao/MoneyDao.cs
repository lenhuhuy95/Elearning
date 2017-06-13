using Model.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class MoneyDao
    {
        ElearningDbContext db = null;
        public MoneyDao()
        {
            db = new ElearningDbContext();
        }

        public List<Money> ListAllMoney()
        {
            return db.Moneys.OrderBy(x => x.MoneyID).ToList();
        }

        public IEnumerable<Money> ListAll(string searchString, int page, int pageSize)
        {
            IQueryable<Money> model = db.Moneys;

            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Name.Contains(searchString) || x.Unit.Contains(searchString));
            }

            return model.OrderBy(x => x.MoneyID).ToPagedList(page, pageSize);
        }

        //thay đổi trạng thái 
        public bool ChangeStatus(long id)
        {
            var money = db.Moneys.Find(id);
            money.Status = !money.Status;
            db.SaveChanges();
            return money.Status.Value;
        }

        //DeleteMoney
        public bool DeleteMoney(long id)
        {
            try
            {
                var money = db.Moneys.Find(id);
                db.Moneys.Remove(money);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        //Insert
        public long Insert(Money entity)
        {
            db.Moneys.Add(entity);
            db.SaveChanges();
            return entity.MoneyID;
        }

        //ViewDetail
        public Money ViewDetail(long id)
        {
            return db.Moneys.Find(id);
        }
        //Update
        public bool Update(Money entity)
        {
            try
            {
                var money = db.Moneys.Find(entity.MoneyID);
                money.Name = entity.Name;
                money.Description = entity.Description;
                money.Unit = entity.Unit;
                money.ExchangeUSD = entity.ExchangeUSD;
                money.ModifiedDate = DateTime.Now;
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
