

using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using Apps.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using Apps.Common;

namespace Apps.DAL
{
    /// <summary>
    /// 管理类的基类
    /// </summary>
    /// <typeparam name="T">模型类</typeparam>
    public abstract class BaseRepository<T> where T :class
    {
        /// <summary>
        /// 数据仓储类
        /// </summary>
        public DbContexts DbContext { get; set; }

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public BaseRepository():this(ContextFactory.CurrentContext())
        {
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbContext">数据上下文</param>
        public BaseRepository(DbContexts dbContext)
        {
            DbContext = dbContext;
        }

        /// <summary>
        /// 查找实体
        /// </summary>
        /// <param name="ID">实体主键值</param>
        /// <returns></returns>
        public T Find(int ID)
        {
            return DbContext.Set<T>().Find(ID);
        }

        #region 查找
        /// <summary>
        /// 查找实体
        /// </summary>
        /// <param name="where">查询Lambda表达式</param>
        /// <returns></returns>
        public virtual T Find(Expression<Func<T, bool>> where)
        {
            return DbContext.Set<T>().SingleOrDefault(where);
        }

        //查找实体列表
        #region FindList
        /// <summary>
        /// 查找实体列表
        /// </summary>
        /// <returns></returns>
        public virtual IQueryable<T> FindList()
        {
            return DbContext.Set<T>();
        }

        /// <summary>
        /// 查找实体列表
        /// </summary>
        /// <typeparam name="TKey">排序建类型</typeparam>
        /// <param name="order">排序键</param>
        /// <param name="asc">是否正序</param>
        /// <returns></returns>
        public virtual IQueryable<T> FindList<TKey>(Expression<Func<T, TKey>> order, bool asc)
        {
            return asc ? DbContext.Set<T>().OrderBy(order) : DbContext.Set<T>().OrderByDescending(order);
        }

        /// <summary>
        /// 查找实体列表
        /// </summary>
        /// <typeparam name="TKey">排序键类型</typeparam>
        /// <param name="order">排序键</param>
        /// <param name="asc">是否正序</param>
        /// <param name="number">获取的记录数量</param>
        /// <returns></returns>
        public virtual IQueryable<T> FindList<TKey>(Expression<Func<T, TKey>> order, bool asc, int number)
        {
            return asc ? DbContext.Set<T>().OrderBy(order).Take(number) : DbContext.Set<T>().OrderByDescending(order).Take(number);
        }

        /// <summary>
        /// 查找实体列表
        /// </summary>
        /// <param name="where">查询Lambda表达式</param>
        /// <returns></returns>
        public virtual IQueryable<T> FindList(Expression<Func<T, bool>> where)
        {
            return DbContext.Set<T>().Where(where);
        }

        /// <summary>
        /// 查找实体列表
        /// </summary>
        /// <param name="where">查询Lambda表达式</param>
        /// <param name="number">获取的记录数量</param>
        /// <returns></returns>
        public virtual IQueryable<T> FindList(Expression<Func<T, bool>> where, int number)
        {
            return DbContext.Set<T>().Where(where).Take(number);
        }

        /// <summary>
        /// 查找实体列表
        /// </summary>
        /// <typeparam name="TKey">排序键类型</typeparam>
        /// <param name="where">查询Lambda表达式</param>
        /// <param name="order">排序键</param>
        /// <param name="asc">是否正序</param>
        /// <returns></returns>
        public virtual IQueryable<T> FindList<TKey>(Expression<Func<T, bool>> where, Expression<Func<T, TKey>> order, bool asc)
        {
            return asc ? DbContext.Set<T>().Where(where).OrderBy(order) : DbContext.Set<T>().Where(where).OrderByDescending(order);
        }

        /// <summary>
        /// 查找实体列表
        /// </summary>
        /// <typeparam name="TKey">排序键类型</typeparam>
        /// <param name="where">查询Lambda表达式</param>
        /// <param name="order">排序键</param>
        /// <param name="asc">是否正序</param>
        /// <param name="number">获取的记录数量</param>
        /// <returns></returns>
        public virtual IQueryable<T> FindList<TKey>(Expression<Func<T, bool>> where, Expression<Func<T, TKey>> order, bool asc, int number)
        {
            return asc ? DbContext.Set<T>().Where(where).OrderBy(order).Take(number) : DbContext.Set<T>().Where(where).OrderByDescending(order).Take(number);
        }
        #endregion

        //查找实体分页列表

        /// <summary>
        /// 查找分页列表
        /// </summary>
        /// <param name="pageSize">每页记录数。必须大于1</param>
        /// <param name="pageIndex">页码。首页从1开始，页码必须大于1</param>
        /// <param name="totalNumber">总记录数</param>
        /// <returns></returns>
        public virtual IQueryable<T> FindPageList(int pageSize, int pageIndex, out int totalNumber)
        {
            if (pageIndex < 1) pageIndex = 1;
            if (pageSize < 1) pageSize = 10;
            IQueryable<T> _list = DbContext.Set<T>();
            totalNumber = _list.Count();
            return _list.Skip((pageIndex - 1) * pageIndex).Take(pageSize);
        }

        /// <summary>
        /// 查找分页列表
        /// </summary>
        /// <param name="pageSize">每页记录数。必须大于1</param>
        /// <param name="pageIndex">页码。首页从1开始，页码必须大于1</param>
        /// <param name="totalNumber">总记录数</param>
        /// <param name="order">排序键</param>
        /// <param name="asc">是否正序</param>
        /// <returns></returns>
        public virtual IQueryable<T> FindPageList<TKey>(int pageSize, int pageIndex, out int totalNumber, Expression<Func<T, TKey>> order, bool asc)
        {
            if (pageIndex < 1) pageIndex = 1;
            if (pageSize < 1) pageSize = 10;
            IQueryable<T> _list = DbContext.Set<T>();
            _list = asc ? _list.OrderBy(order) : _list.OrderByDescending(order);
            totalNumber = _list.Count();
            return _list.Skip((pageIndex - 1) * pageIndex).Take(pageSize);
        }

        /// <summary>
        /// 查找分页列表
        /// </summary>
        /// <param name="pageSize">每页记录数。必须大于1</param>
        /// <param name="pageIndex">页码。首页从1开始，页码必须大于1</param>
        /// <param name="totalNumber">总记录数</param>
        /// <param name="where">查询表达式</param>
        public virtual IQueryable<T> FindPageList(int pageSize, int pageIndex, out int totalNumber, Expression<Func<T, bool>> where)
        {
            if (pageIndex < 1) pageIndex = 1;
            if (pageSize < 1) pageSize = 10;
            IQueryable<T> _list = DbContext.Set<T>().Where(where);
            totalNumber = _list.Count();
            return _list.Skip((pageIndex - 1) * pageIndex).Take(pageSize);
        }

        /// <summary>
        /// 查找分页列表
        /// </summary>
        /// <param name="pageSize">每页记录数。必须大于1</param>
        /// <param name="pageIndex">页码。首页从1开始，页码必须大于1</param>
        /// <param name="totalNumber">总记录数</param>
        /// <param name="where">查询表达式</param>
        /// <param name="order">排序键</param>
        /// <param name="asc">是否正序</param>
        public virtual IQueryable<T> FindPageList<TKey>(int pageSize, int pageIndex, out int totalNumber, Expression<Func<T, bool>> where, Expression<Func<T, TKey>> order, bool asc)
        {
            if (pageIndex < 1) pageIndex = 1;
            if (pageSize < 1) pageSize = 10;
            IQueryable<T> _list = DbContext.Set<T>().Where(where);
            _list = asc ? _list.OrderBy(order) : _list.OrderByDescending(order);
            totalNumber = _list.Count();
            return _list.Skip((pageIndex - 1) * pageIndex).Take(pageSize);
        }

        public virtual IQueryable<T> FindPageList<S>(GridPager pager, Expression<Func<T, bool>> whereLambda, bool isAsc, Expression<Func<T, bool>> orderByLambda)
        {
            var queryable = FindList(whereLambda);
            pager.totalRows = queryable.Count();
            if (isAsc)
            {
                queryable = queryable.OrderBy(orderByLambda).Skip<T>(pager.rows * (pager.page - 1)).Take<T>(pager.rows);
            }
            else
            {
                queryable = queryable.OrderByDescending(orderByLambda).Skip<T>(pager.rows * (pager.page - 1)).Take<T>(pager.rows);
            }
            return queryable;
        }

        public virtual IQueryable<T> FindPageList(ref GridPager pager, Expression<Func<T, bool>> whereLambda)
        {
            var queryable = FindList(whereLambda);
            //启用通用列头过滤
            if (!string.IsNullOrWhiteSpace(pager.filterRules))
            {
                List<DataFilterModel> dataFilterList = JsonHandler.Deserialize<List<DataFilterModel>>(pager.filterRules).Where(f => !string.IsNullOrWhiteSpace(f.value)).ToList();
                queryable = LinqHelper.DataFilter<T>(queryable, dataFilterList);
            }
            pager.totalRows = queryable.Count();
            queryable = LinqHelper.SortingAndPaging(queryable, pager.sort, pager.order, pager.page, pager.rows);
            return queryable;
        }

#region 过滤查找-性能低下-慎用
        public virtual IQueryable<T> FindPageList(ref GridPager pager)
        {
            var queryData = FindList();
            //启用通用列头过滤
            if (!string.IsNullOrWhiteSpace(pager.filterRules))
            {
                List<DataFilterModel> dataFilterList = JsonHandler.Deserialize<List<DataFilterModel>>(pager.filterRules).Where(f => !string.IsNullOrWhiteSpace(f.value)).ToList();
                queryData = LinqHelper.DataFilter<T>(queryData, dataFilterList);
            }
            pager.totalRows = queryData.Count();
            //排序
            queryData = LinqHelper.SortingAndPaging(queryData, pager.sort, pager.order, pager.page, pager.rows);
            return queryData;
        }

        public virtual IQueryable<T> FindPageList(ref GridPager pager,string queryStr)
        {
            var queryData = FindList();
            //启用通用列头过滤
            if (!string.IsNullOrWhiteSpace(pager.filterRules))
            {
                List<DataFilterModel> dataFilterList = JsonHandler.Deserialize<List<DataFilterModel>>(pager.filterRules).Where(f => f.value.Contains(queryStr)).ToList();
                queryData = LinqHelper.DataFilter<T>(queryData, dataFilterList);
            }
            pager.totalRows = queryData.Count();
            //排序
            queryData = LinqHelper.SortingAndPaging(queryData, pager.sort, pager.order, pager.page, pager.rows);
            return queryData;
        }

        public virtual IQueryable<T> FindPageList(ref GridPager pager, string queryStr,Expression<Func<T, bool>> whereLambda)
        {
            var queryData = FindList(whereLambda);
            //启用通用列头过滤
            if (!string.IsNullOrWhiteSpace(pager.filterRules))
            {
                List<DataFilterModel> dataFilterList = JsonHandler.Deserialize<List<DataFilterModel>>(pager.filterRules).Where(f => f.value.Contains(queryStr)).ToList();
                queryData = LinqHelper.DataFilter<T>(queryData, dataFilterList);
            }
            pager.totalRows = queryData.Count();
            //排序
            queryData = LinqHelper.SortingAndPaging(queryData, pager.sort, pager.order, pager.page, pager.rows);
            return queryData;
        }
        #endregion

        #endregion

        #region 添加

        /// <summary>
        /// 添加实体【立即保存】
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>受影响的对象的数目</returns>
        public virtual Boolean Create(T entity)
        {
            return Add(entity, true) > 0 ;
        }

        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="isSave">是否立即保存</param>
        /// <returns>在“isSave”为True时返回受影响的对象的数目，为False时直接返回0</returns>
        public virtual int Add(T entity, bool isSave)
        {
            try
            {
                DbContext.Set<T>().Add(entity);
                return isSave ? DbContext.SaveChanges() : 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region 更新

        /// <summary>
        /// 更新实体【立即保存】
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>受影响的对象的数目</returns>
        public virtual Boolean Update(T entity)
        {
            return Update(entity, true) > 0;
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="isSave">是否立即保存</param>
        /// <returns>在“isSave”为True时返回受影响的对象的数目，为False时直接返回0</returns>
        public virtual int Update(T entity, bool isSave)
        {
            try
            {
                DbContext.Set<T>().Attach(entity);
                DbContext.Entry<T>(entity).State = EntityState.Modified;
                return isSave ? DbContext.SaveChanges() : 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 删除

        /// <summary>
        /// 删除实体【立即保存】
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>受影响的对象的数目</returns>
        public virtual int Delete(T entity)
        {
            return Delete(entity, true);
        }
        public virtual int Delete(int id)
        {
            var _entity = Find(id);
            if (_entity == null)
            {
                return 0;
            }
            else
            {
                return Delete(_entity, true);
            }
        }

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="isSave">是否立即保存</param>
        /// <returns>在“isSave”为True时返回受影响的对象的数目，为False时直接返回0</returns>
        public virtual int Delete(T entity, bool isSave)
        {
            DbContext.Set<T>().Attach(entity);
            DbContext.Entry<T>(entity).State = EntityState.Deleted;
            return isSave ? DbContext.SaveChanges() : 0;
        }

        /// <summary>
        /// 批量删除实体
        /// </summary>
        /// <param name="entities">实体集合</param>
        /// <returns>受影响的对象的数目</returns>
        public virtual int Delete(IEnumerable<T> entities)
        {
            DbContext.Set<T>().RemoveRange(entities);
            return DbContext.SaveChanges();
        }

        public virtual int Delete(params object[] keyValues)
        {
            int delCNT = 0;
            foreach (var item in keyValues)
            {
                T model = GetById(keyValues);
                if (model != null)
                {
                    delCNT = delCNT + Delete(model);
                }
            }
            return delCNT;
        }

        public virtual T GetById(params object[] keyValues)
        {
            return DbContext.Set<T>().Find(keyValues);
        }
        #endregion

        #region 记录数Count

        /// <summary>
        /// 记录数
        /// </summary>
        /// <param name="predicate">表达式</param>
        /// <returns></returns>
        public virtual int Count(Expression<Func<T, bool>> predicate)
        {
            return DbContext.Set<T>().Count(predicate);
        }
        #endregion


        /// <summary>
        /// 执行一条SQL语句
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public virtual int ExecuteSqlCommand(string sql)
        {
            return DbContext.Database.ExecuteSqlCommand(sql);
        }
        /// <summary>
        /// 异步执行一条SQL语句
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public virtual Task<int> ExecuteSqlCommandAsync(string sql)
        {
            return DbContext.Database.ExecuteSqlCommandAsync(sql);
        }

        public virtual DbRawSqlQuery<T> SqlQuery(string sql)
        {
            return DbContext.Database.SqlQuery<T>(sql);
        }
        /// <summary>
        /// 查询一条语句返回结果集
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public virtual DbRawSqlQuery<T> SqlQuery(string sql, params object[] paras)
        {
            return DbContext.Database.SqlQuery<T>(sql, paras);
        }

        public int SaveChanges()
        {
            return DbContext.SaveChanges();
        }
        //1、 Finalize只释放非托管资源；
        //2、 Dispose释放托管和非托管资源；
        //3、 重复调用Finalize和Dispose是没有问题的；
        //4、 Finalize和Dispose共享相同的资源释放策略，因此他们之间也是没有冲突的。
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing)
        {

            if (disposing)
            {
                DbContext.Dispose();
            }
        }
        //======================================================================================================

        public virtual IQueryable<T> PageList(IQueryable<T> entitys, int pageIndex, int pageSize)
        {
            return entitys.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }

        /// <summary>
        /// 记录是否存在
        /// </summary>
        /// <param name="predicate">表达式</param>
        /// <returns></returns>
        public virtual bool IsContains(Expression<Func<T, bool>> predicate)
        {
            return Count(predicate) > 0;
        }
    }

    public class Response
    {
        /// <summary>
        /// 返回代码. 0-失败，1-成功，其他-具体见方法返回值说明
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 返回消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 返回数据
        /// </summary>
        public dynamic Data { get; set; }

        public Response()
        {
            Code = 1;
        }
    }

    public class ajaxResponse
    {
        /// <summary>
        /// 返回代码. 0-失败，1-成功，其他-具体见方法返回值说明
        /// </summary>
        public string info { get; set; }
        /// <summary>
        /// 返回消息
        /// </summary>
        public string status { get; set; }
    }
}
