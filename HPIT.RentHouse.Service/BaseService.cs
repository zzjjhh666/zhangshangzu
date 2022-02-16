using HPIT.RentHouse.Service.Entities;
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace HPIT.RentHouse.Service
{
    public class BaseService<T> where T : BaseEntity
    {
        private RentHouseEntity db;

        public BaseService(RentHouseEntity db)
        {
            this.db = db;
        }

        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public long Add(T entity)
        {
            db.Entry<T>(entity).State = EntityState.Added;
            db.SaveChanges();
            return entity.Id;
        }
        public bool AddBool(T entity)
        {
            db.Entry<T>(entity).State = EntityState.Added;
            return db.SaveChanges() > 0;
        }
        /// <summary>
        /// 删除实体（软删除）
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Delete(T entity)
        {
            db.Set<T>().Attach(entity);
            db.Entry<T>(entity).State = EntityState.Modified;
            entity.IsDeleted = true;
            return db.SaveChanges() > 0;
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Update(T entity)
        {
            db.Set<T>().Attach(entity);
            db.Entry<T>(entity).State = EntityState.Modified;
            return db.SaveChanges() > 0;
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        public IQueryable<T> GetList(Expression<Func<T, bool>> whereLambda)
        {
            return db.Set<T>().Where(whereLambda).Where(e => !e.IsDeleted);
        }

        /// <summary>
        /// 查询实体
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        public T Get(Expression<Func<T, bool>> whereLambda)
        {
            return GetList(whereLambda).FirstOrDefault();
        }

        /// <summary>
        /// 查询列表带排序
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="whereLambda"></param>
        /// <param name="orderBy"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        public IQueryable<T> GetList<TKey>(Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderBy, bool isAsc = true)
        {
            if (isAsc)
            {
                return GetList(whereLambda).OrderBy(orderBy);
            }
            else
            {
                return GetList(whereLambda).OrderByDescending(orderBy);
            }
        }

        /// <summary>
        /// 查询列表带分页
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="rowCount"></param>
        /// <param name="whereLambda"></param>
        /// <param name="orderBy"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        public IQueryable<T> GetPagedList<TKey>(int start, int pageSize, ref int rowCount, Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderBy, bool isAsc = true)
        {
            //获取总记录数
            rowCount = GetList(whereLambda).Count();

            //边界值判断
            start = start < 1 ? 0 : start;

            if (isAsc)
            {
                return GetList(whereLambda).OrderBy(orderBy).Skip(start).Take(pageSize);
            }
            else
            {
                return GetList(whereLambda).OrderByDescending(orderBy).Skip(start).Take(pageSize);
            }
        }
    }
}
