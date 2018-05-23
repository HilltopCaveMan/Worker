using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Monopy.PreceRateWage.Dal
{
    public class BaseDal<T> where T : class, new()
    {
        protected DbContext db = new Model.HHContext();

        public BaseDal()
        {
        }

        public int ExecuteSqlCommand(string sql)
        {
            return db.Database.ExecuteSqlCommand(sql);
        }

        public IQueryable<T> GetList()
        {
            return db.Set<T>().AsNoTracking();
        }

        public IQueryable<T> GetList(Expression<Func<T, bool>> whereLambda)
        {
            if (whereLambda == null)
            {
                return GetList();
            }
            return db.Set<T>().AsNoTracking().Where(whereLambda);
        }

        public IQueryable<T> GetList<S>(Expression<Func<T, bool>> whereLambda, Expression<Func<T, S>> orderLambda, bool isDesc)
        {
            if (isDesc)
            {
                return db.Set<T>().Where(whereLambda).OrderByDescending(orderLambda).AsNoTracking();
            }
            else
            {
                return db.Set<T>().Where(whereLambda).OrderBy(orderLambda).AsNoTracking();
            }
        }

        public IQueryable<T> GetList(Expression<Func<T, bool>> whereLambda, params IOrderByExpression<T>[] orderByExpressions)
        {
            var query = GetList(whereLambda);
            if (orderByExpressions == null)
                return query;
            IOrderedQueryable<T> output = null;

            foreach (var orderByExpression in orderByExpressions)
            {
                if (output == null)
                    output = orderByExpression.ApplyOrderBy(query);
                else
                    output = orderByExpression.ApplyThenBy(output);
            }

            return output.AsNoTracking() ?? query;
        }

        public IQueryable<T> GetList<S>(int pageSize, int pageIndex, bool isDesc, Expression<Func<T, S>> orderLambda)
        {
            if (isDesc)
            {
                return db.Set<T>().OrderByDescending(orderLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsNoTracking();
            }
            else
            {
                return db.Set<T>().OrderBy(orderLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsNoTracking();
            }
        }

        public IQueryable<T> GetList<S>(int pageSize, int pageIndex, bool isDesc, Expression<Func<T, S>> orderLambda, Expression<Func<T, bool>> whereLambda)
        {
            var temp = db.Set<T>().Where(whereLambda);
            if (isDesc)
            {
                return temp.OrderByDescending(orderLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsNoTracking();
            }
            else
            {
                return temp.OrderBy(orderLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsNoTracking();
            }
        }

        public T Get(Expression<Func<T, bool>> whereLambda)
        {
            return db.Set<T>().AsNoTracking().Where(whereLambda).FirstOrDefault();
        }

        public int Edit(T t)
        {
            //Type type = typeof(T); /*t.GetType();*/
            //StringBuilder sb = new StringBuilder();
            //sb.Append("表:" + type.Name + ";");
            //foreach (var item in type.GetProperties())
            //{
            //    sb.Append(item.Name + ":" + item.GetValue(t, null).ToString() + ";");
            //}
            //db.Set<LogTable>().Add(new LogTable { Id = Guid.NewGuid(), CreateTime = DateTime.Now, LogType = "Edit", LogInfo = sb.ToString() });
            db.Entry(t).State = EntityState.Modified;
            return db.SaveChanges();
        }

        public int Delete(T t)
        {
            db.Entry(t).State = EntityState.Deleted;
            return db.SaveChanges();
        }

        public int ExecuteSqlCommand(string whereStr, params object[] pars)
        {
            return db.Database.ExecuteSqlCommand(whereStr, pars);
        }

        public int Add(T t)
        {
            db.Set<T>().Add(t);
            return db.SaveChanges();
        }

        public int Add(List<T> list)
        {
            foreach (var item in list)
            {
                db.Set<T>().Add(item);
            }
            return db.SaveChanges();
        }
    }
}