using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace EMEWE.CarManagement.Commons.CommonClass
{
    public class DrawCode
    {
        public static Bitmap getEAN13(string s, int width, int height)
        {
            try
            {


                Bitmap bmp = new Bitmap(200, height);
                if (!Regex.IsMatch(s, @"^\d{12}$"))
                {
                    if (!Regex.IsMatch(s, @"^\d{13}$"))
                    {
                        return bmp;
                    }
                    else
                    {
                        s = s.Substring(0, 12);
                    }
                }

                int sum_even = 0;//偶数位之和
                int sum_odd = 0;//奇数位之和

                for (int i = 0; i < 12; i++)
                {
                    if (i % 2 == 0)
                    {
                        sum_odd += int.Parse(s[i].ToString());
                    }
                    else
                    {
                        sum_even += int.Parse(s[i].ToString());
                    }
                }

                int checkcode = (10 - (sum_even * 3 + sum_odd) % 10) % 10;//校验码

                s += checkcode;//变成13位

                // 000000000101左侧42个01010右侧35个校验7个101000000000
                // 6        101左侧6位 01010右侧5位 校验1位101000000000

                string result_bin = "";//二进制串
                result_bin += "000000000101";

                string type = ean13type(s[0]);
                for (int i = 1; i < 7; i++)
                {
                    result_bin += ean13(s[i], type[i - 1]);
                }
                result_bin += "01010";
                for (int i = 7; i < 13; i++)
                {
                    result_bin += ean13(s[i], 'C');
                }
                result_bin += "101000000000";

                int iDrwaLineY = height;
                using (Graphics graph = Graphics.FromImage(bmp))
                {
                    Color cl = new Color();
                    Pen pen = null;
                    graph.Clear(Color.White);
                    int k = 0;
                    foreach (char item in result_bin)
                    {
                        cl = item == '0' ? Color.White : Color.Black;
                        pen = new Pen(cl, 3);
                        pen.Alignment = PenAlignment.Right;
                        graph.DrawLine(pen, k, 0, k, iDrwaLineY);
                        k++;
                    }
                }
                return bmp;
            }
            catch (Exception)
            {
                return null;
            }
        }
        private static string ean13(char c, char type)
        {
            switch (type)
            {
                case 'A':
                    {
                        switch (c)
                        {
                            case '0': return "0001101";
                            case '1': return "0011001";
                            case '2': return "0010011";
                            case '3': return "0111101";//011101
                            case '4': return "0100011";
                            case '5': return "0110001";
                            case '6': return "0101111";
                            case '7': return "0111011";
                            case '8': return "0110111";
                            case '9': return "0001011";
                            default: return "Error!";
                        }
                    }
                case 'B':
                    {
                        switch (c)
                        {
                            case '0': return "0100111";
                            case '1': return "0110011";
                            case '2': return "0011011";
                            case '3': return "0100001";
                            case '4': return "0011101";
                            case '5': return "0111001";
                            case '6': return "0000101";//000101
                            case '7': return "0010001";
                            case '8': return "0001001";
                            case '9': return "0010111";
                            default: return "Error!";
                        }
                    }
                case 'C':
                    {
                        switch (c)
                        {
                            case '0': return "1110010";
                            case '1': return "1100110";
                            case '2': return "1101100";
                            case '3': return "1000010";
                            case '4': return "1011100";
                            case '5': return "1001110";
                            case '6': return "1010000";
                            case '7': return "1000100";
                            case '8': return "1001000";
                            case '9': return "1110100";
                            default: return "Error!";
                        }
                    }
                default: return "Error!";
            }
        }
        private static string ean13type(char c)
        {
            switch (c)
            {
                case '0': return "AAAAAA";
                case '1': return "AABABB";
                case '2': return "AABBAB";
                case '3': return "AABBBA";
                case '4': return "ABAABB";
                case '5': return "ABBAAB";
                case '6': return "ABBBAA";
                case '7': return "ABABAB";
                case '8': return "ABABBA";
                case '9': return "ABBABA";
                default: return "Error!";
            }
        }
    }
}
