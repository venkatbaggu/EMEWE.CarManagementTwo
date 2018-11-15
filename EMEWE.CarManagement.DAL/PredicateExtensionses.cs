using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace EMEWE.CarManagementDAL
{
    public static class PredicateExtensionses
    {
        public static Expression<Func<T, bool>> True<T>() { return f => true; }

        public static Expression<Func<T, bool>> False<T>() { return f => false; }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> exp_flow, Expression<Func<T, bool>> expression2)
        {

            var invokedExpression = System.Linq.Expressions.Expression.Invoke(expression2, exp_flow.Parameters.Cast<System.Linq.Expressions.Expression>());

            return System.Linq.Expressions.Expression.Lambda<Func<T, bool>>(System.Linq.Expressions.Expression.Or(exp_flow.Body, invokedExpression), exp_flow.Parameters);

        }
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> exp_flow, Expression<Func<T, bool>> expression2)
        {

            var invokedExpression = System.Linq.Expressions.Expression.Invoke(expression2, exp_flow.Parameters.Cast<System.Linq.Expressions.Expression>());

            return System.Linq.Expressions.Expression.Lambda<Func<T, bool>>(System.Linq.Expressions.Expression.And(exp_flow.Body, invokedExpression), exp_flow.Parameters);

        }

    }

    #region 注释
    //private void ListDataBind(int pageIndex)
    //{
    //    int rowCount = 0;
    //    int pageCount = 0;
    //    int pageSize = 30;
    //    Expression<Func<FeedBack, bool>> expr = PredicateExtensionses.True<FeedBack>();
    //    GetCondition(ref expr);
    //    var hs = from h in hm.AllFeedBacks.Where(expr) select h;//延迟加载,数据库没有任何操作
    //    if (pageIndex == 1)//如果是第一次取数据,需要获取符合条件的总记录条数
    //    {
    //        rowCount = hs.Count();//数据库进行一次Count操作
    //    }
    //    else//之后的记录条数,从分页控件持久态的属性中获取,省去一次Count查询
    //    {
    //        rowCount = PageNavigator1.RecordCount;
    //    }
    //    pageCount = rowCount > pageSize ? Convert.ToInt32((rowCount - 1) / pageSize) + 1 : 1;//通用分页算法
    //    if (pageIndex > pageCount)
    //    {
    //        pageIndex = pageCount;
    //    }
    //    var pageData = hs.Skip(pageSize * (pageIndex - 1)).Take(pageSize);//这里也是延迟加载,数据库此时不操作
    //    FeedBackManageList.DataSource = pageData;//这里才正式加载数据,仅仅向数据库发出请求30条记录SQL
    //    FeedBackManageList.DataBind();
    //    PageNavigator1.RecordCount = rowCount;//给分页控件一些数据
    //    PageNavigator1.PageCount = pageCount;//给分页控件一些数据
    //    PageNavigator1.PageIndex = pageIndex;//给分页控件一些数据
    //}
    //private void GetCondition(ref Expression<Func<FeedBack, bool>> expr)
    //{
    //    int isLock = Int32.Parse(ddlIsLock.SelectedValue);
    //    if (isLock > -1)
    //    {
    //        expr = expr.And(c => (c.IsLock == isLock));//一次组合
    //    }
    //    string keyword = tbxKeyword.Text.FilterInjectStr();
    //    if (!keyword.IsNullOrEmpty())
    //    {
    //        expr = expr.And(c => (c.HotelName.IndexOf(keyword) > -1)); //二次组合
    //    }
    //} 
    #endregion
}
