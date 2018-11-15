using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace EMEWE.CarManagement.Commons.CommonClass
{
    public class MyBarCode
    {
        private Dictionary<string, string> hash = new Dictionary<string, string>();
        private string _barcode;
        private int _barcodewidth;
        private bool _isfoot;
        private string _barcodetext;
        private static int i = 0;

        /// <summary>
        /// 条形码
        /// </summary>
        public string BarCode
        {
            get { return _barcode; }
            set { _barcode = value; }
        }

        /// <summary>
        /// 条形码文本
        /// </summary>
        public string BarCodeText
        {
            get { return _barcodetext; }
            set { _barcodetext = value; }
        }

        /// <summary>
        /// 条形码宽度
        /// </summary>
        public int BarCodeWidth
        {
            get { return _barcodewidth; }
            set { _barcodewidth = value; }
        }

        /// <summary>
        /// 初始化条形码表
        /// </summary>
        private void InitHashtable()
        {
            //hash.Add("0", "000110100");
            //hash.Add("1", "100100001");
            //hash.Add("2", "001100001");
            //hash.Add("3", "101100000");
            //hash.Add("4", "000110001");
            //hash.Add("5", "100110000");
            //hash.Add("6", "001110000");
            //hash.Add("7", "000100101");
            //hash.Add("8", "100100100");
            //hash.Add("9", "001100100");

            //hash.Add("0", "1110010");
            //hash.Add("1", "1100110");
            //hash.Add("2", "1101100");
            //hash.Add("3", "1000010");
            //hash.Add("4", "1011100");
            //hash.Add("5", "1001110");
            //hash.Add("6", "1010000");
            //hash.Add("7", "1000100");
            //hash.Add("8", "1001000");
            //hash.Add("9", "1110100");


            hash.Add("0", "101001101101");
            hash.Add("1", "110100101011");
            hash.Add("2", "101100101011");
            hash.Add("3", "110110010101");
            hash.Add("4", "101001101011");
            hash.Add("5", "110100110101");
            hash.Add("6","101100110101");
            hash.Add("7", "101001011011");
            hash.Add("8", "110100101101");
            hash.Add("9", "101100101101");

            //hash.Add("A", "100001001");
            //hash.Add("B", "001001001");
            //hash.Add("C", "101001000");
            //hash.Add("D", "000011001");
            //hash.Add("E", "100011000");
            //hash.Add("F", "001011000");
            //hash.Add("G", "000001101");
            //hash.Add("H", "100001100");
            //hash.Add("I", "001001101");
            //hash.Add("J", "000011100");
            //hash.Add("K", "100000011");
            //hash.Add("L", "001000011");
            //hash.Add("M", "101000010");
            //hash.Add("N", "000010011");
            //hash.Add("O", "100010010");
            //hash.Add("P", "001010010");
            //hash.Add("Q", "000000111");
            //hash.Add("R", "100000110");
            //hash.Add("S", "001000110");
            //hash.Add("T", "000010110");
            //hash.Add("U", "110000001");
            //hash.Add("V", "011000001");
            //hash.Add("W", "111000000");
            //hash.Add("X", "010010001");
            //hash.Add("Y", "110010000");
            //hash.Add("Z", "011010000");
            //hash.Add("-", "010000101");
            //hash.Add("%", "000101010");
            //hash.Add("$", "010101000");
            //hash.Add("*", "010010100");
        }

        /// <summary>
        /// 测算条形码长度
        /// </summary>
        /// <param name="BarCode"></param>
        /// <returns></returns>
        private int GetBarCodeWidth(string BarCode)
        {
            int iWidth = 0;
            foreach (char item in BarCode)
            {
                if (item.ToString().Equals("0"))
                {
                    iWidth += 1;
                }
                else
                {
                    iWidth += 2;
                }
            }
            return iWidth;
        }

        /// <summary>
        /// 生成条形码
        /// </summary>
        /// <param name="sCode"></param>
        /// <returns></returns>
        private string Encoder(string sCode)
        {
            string sResult = "";
            foreach (char item in sCode)
            {
                sResult += hash[item.ToString().ToUpper()] + "0";
            }
            return sResult;
        }

        /// <summary>
        /// 在位图上绘制条形码
        /// </summary>
        /// <returns></returns>
        public Bitmap DrawBarCode()
        {
            
            Bitmap bmp = new Bitmap(BarCodeWidth, 20);
            try
            {
                #region 打印条码1
                Graphics graph = Graphics.FromImage(bmp);
                graph.FillRectangle(Brushes.White, 0, 0, BarCodeWidth, 50);
                
                Font font = new Font("宋体", 12, FontStyle.Regular);
                int iDrwaLineY = 35;
                if (IsFoot)
                {
                    SizeF size = graph.MeasureString(BarCodeText, font);
                    int iPos = (BarCodeWidth - (int)size.Width) / 2;
                    graph.DrawString(BarCodeText, font, Brushes.Black, iPos, 36);//5+1+a+s+p+x
                }
                else
                {
                    iDrwaLineY = 50;
                }
                int iPosition = 0;


                string s = BarCode;

                for (int i = 0; i < s.Length; i++)
                {
                    if (s.Substring(i, 1).Equals("0"))
                    {
                        Pen pen = new Pen(Color.White, 1);
                        graph.DrawLine(pen, iPosition, 0, iPosition, iDrwaLineY);
                        iPosition += 1;
                    }
                    else
                    {
                        //graph.DrawLine(pen, iPosition, 0, iPosition, iDrwaLineY);
                        Pen pen = new Pen(Color.Black, 1);
                        graph.DrawLine(pen, iPosition + 1, 0, iPosition + 1, iDrwaLineY);
                        iPosition += 1;
                    }
                    i++;
                    //if (s.Substring(i, 1).Equals("0"))
                    //{
                    //    iPosition += 1;
                    //}
                    //else
                    //{
                    //    iPosition += 2;
                    //}
                }
                return bmp;
#endregion
                #region 打印条码2
                //打印内容 为 全部Form           
                //Image myFormImage;            
                //myFormImage = new Bitmap（this.Width， this.Height）;            
                //Graphics g = Graphics.FromImage（myFormImage）;            
                //g.CopyFromScreen（this.Location.X, this.Location.Y, 0, 0, this.Size）;         
                //e.Graphics.DrawImage（myFormImage,0, 0）;           
                //打印内容 为 局部的 this.groupBox1            
                       
                //打印内容 为 自定义文本内容           
                /*Font font = new Font（"宋体"， 12）;          
                 * Brush bru = Brushes.Blue;           
                 * for （int i = 1; i <= 5; i++）         
                 * {              
                 * e.Graphics.DrawString（"Hello world "， font， bru， i*20， i*20）;       
                 * }*/
                #endregion
            }
            catch 
            {
                CommonalityEntity.WriteTextLog("MyBarCode DrawBarCode()" );
            }
            return bmp;
        }

        /// <summary>
        /// 是否显示条形码文本
        /// </summary>
        public bool IsFoot
        {
            get { return _isfoot; }
            set { _isfoot = value; }
        }

        /// <summary>
        /// 获取绘制的条形码
        /// </summary>
        public Bitmap PreviewBarCode
        {
            get
            {
                return DrawBarCode();
            }
        }

        private MyBarCode()
        {
            //InitHashtable();
        }

        public MyBarCode(string sCode)
        {
            int sum_even = 0;//偶数位之和
            int sum_odd = 0;//奇数位之和

            for (int i = 0; i < 12; i++)
            {
                if (i % 2 == 0)
                {
                    sum_odd += int.Parse(sCode[i].ToString());
                }
                else
                {
                    sum_even += int.Parse(sCode[i].ToString());
                }
            }
            int checkcode = (10 - (sum_even * 3 + sum_odd) % 10) % 10;//校验码
            sCode += checkcode;
            InitHashtable();
            IsFoot = false;
            BarCodeText = sCode;
            BarCode = Encoder(sCode);
            BarCodeWidth = GetBarCodeWidth(BarCode);
        }
    }
}
