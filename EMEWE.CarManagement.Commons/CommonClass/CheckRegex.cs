using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace EMEWE.CarManagement.Commons.CommonClass
{
    /// <summary>
    /// 正则表达式验证
    /// </summary>
    public class CheckRegex
    {

        private static string reg = "";

        /// <summary>
        /// 验证用户名
        /// </summary>
        /// <param name="loginId">用户名</param>
        /// <returns></returns>
        public static bool RegexUser(string loginId, out string msg)
        {
            //正则表达式
            reg = @"^[A-Za-z0-9_]+$";
            //验证
            Regex regx = new Regex(reg);
            Match mt = regx.Match(loginId);
            msg = "用户名错误，用户名由数字、字母、下划线组成！";
            return !mt.Success;
        }

        /// <summary>
        /// 验证正整数
        /// </summary>
        /// <param name="ness">输入的数字</param>
        /// <returns></returns>
        public static bool RegexRightNess(string maths)
        {
            //正则表达式
            reg = @"^\+?[1-9][0-9]*$";
            //验证
            Regex regx = new Regex(reg);
            Match mt = regx.Match(maths);
            return !mt.Success;
        }

        /// <summary>
        /// 验证数字
        /// </summary>
        /// <param name="maths"></param>
        /// <returns></returns>
        public static bool RegexMath(string maths)
        {
            //正则表达式
            reg = @"^\+?[0-9][0-9]*$";
            //验证
            Regex regx = new Regex(reg);
            Match mt = regx.Match(maths);
            return !mt.Success;
        }

        /// <summary>
        /// 验证邮箱
        /// </summary>
        /// <param name="email">邮箱编号</param>
        /// <returns></returns>
        public static bool RegexEmail(string email)
        {
            //正则表达式
            reg = @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
            //验证
            Regex regx = new Regex(reg);
            Match mt = regx.Match(email);
            return !mt.Success;
        }

        /// <summary>
        /// 验证座机号码
        /// </summary>
        /// <param name="phone">座机号码</param>
        /// <returns></returns>
        public static bool RegexPhone(string phone)
        {
            //正则表达式
            reg = @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
            //验证
            Regex regx = new Regex(reg);
            Match mt = regx.Match(phone);
            return !mt.Success;
        }

        /// <summary>
        /// 验证中文字符
        /// </summary>
        /// <param name="ch">字符串</param>
        /// <returns></returns>
        public static bool RegexChinese(string ch)
        {
            //正则表达式
            reg = @"[\u4e00-\u9fa5] ";
            //验证
            Regex regx = new Regex(reg);
            Match mt = regx.Match(ch);
            return !mt.Success;

        }

        /// <summary>
        /// 验证手机号码
        /// </summary>
        /// <param name="telePhone"></param>
        /// <returns></returns>
        public static bool RegexTelePhone(string telePhone)
        {
            //正则表达式
            reg = @"^\+?[1-9][0-9]{10}";
            //验证
            Regex regx = new Regex(reg);
            Match mt = regx.Match(telePhone);
            return !mt.Success;
        }

        /// <summary>
        /// 验证小数
        /// </summary>
        /// <param name="decl"></param>
        /// <returns></returns>
        public static bool RegexDecelmal(string decl)
        {
            //正则表达式
            reg = @"^[0-9]+(.[0-9]{1,30})?$";
            //验证
            Regex regx = new Regex(reg);
            Match mt = regx.Match(decl);
            return !mt.Success;
            
        }
    }
}
