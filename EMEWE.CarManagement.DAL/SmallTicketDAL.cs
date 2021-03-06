﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EMEWE.CarManagement.Entity;
using System.Linq.Expressions;

namespace EMEWE.CarManagement.DAL
{
    public class SmallTicketDAL
    {
        /// <summary>
        /// 按条件删除多条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dc"></param>
        /// <param name="tentitys"></param>
        /// <param name="fun"></param>
        public static bool DeleteToMany(Expression<Func<SmallTicket, bool>> fun)
        {
            bool rbool = true;
            try
            {

                LinQBaseDao.DeleteToMany<SmallTicket>(new DCCarManagementDataContext(), fun);

            }
            catch
            {
                rbool = false;
            }
            return rbool;

        }
    }
}
