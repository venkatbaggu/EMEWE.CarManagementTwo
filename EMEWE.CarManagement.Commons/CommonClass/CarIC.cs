using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EMEWE.CarManagement.Commons.CommonClass
{
  public  class CarIC
    {
      /// <summary>
      /// IC卡号
      /// </summary>
        private string CarICNO;

        public string CarICNO1
        {
            get { return CarICNO; }
            set { CarICNO = value; }
        }
      /// <summary>
      /// 通道名称
      /// </summary>
        private string Driveway_Name;

        public string Driveway_Name1
        {
            get { return Driveway_Name; }
            set { Driveway_Name = value; }
        }
      /// <summary>
      /// 通道值
      /// </summary>
        private string Driveway_Value;

        public string Driveway_Value1
        {
            get { return Driveway_Value; }
            set { Driveway_Value = value; }
        }
      /// <summary>
      /// 车牌号
      /// </summary>
        private string CarNo;

        public string CarNo1
        {
            get { return CarNo; }
            set { CarNo = value; }
        }
    }
}
