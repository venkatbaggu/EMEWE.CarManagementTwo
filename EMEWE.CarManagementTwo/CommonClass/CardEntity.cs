using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EMEWE.CarManagement.CommonClass
{
    public class CardEntity
    {
        private string cardTyep;
        /// <summary>
        /// 卡号类型
        /// 保安卡、员工卡、特殊卡
        /// </summary>
        public string CardTyep
        {
            get { return cardTyep; }
            set { cardTyep = value; }
        }
        private string cardNo;
        /// <summary>
        /// 卡号
        /// </summary>
        public string CardNo
        {
            get { return cardNo; }
            set { cardNo = value; }
        }
        private string cardName;
        /// <summary>
        /// 卡使用人
        /// </summary>
        public string CardName
        {
            get { return cardName; }
            set { cardName = value; }
        }
        private string carNo;
        /// <summary>
        /// 车卡、人卡、 车卡人卡
        /// </summary>
        public string CarNo
        {
            get { return carNo; }
            set { carNo = value; }
        }
        private int carId;
        /// <summary>
        /// 车辆登记编号 车辆和IC卡关联表 
        /// </summary>
        public int CarId
        {
            get { return carId; }
            set { carId = value; }
        }

        private string drivewayPort;
        /// <summary>
        /// 读卡器地址码
        /// </summary>

        public string Driveway_ReadCardPort
        {
            get { return drivewayPort; }
            set { drivewayPort = value; }
        }
        private string drivewayState;
        /// <summary>
        /// 通道状态：进厂、出厂
        /// </summary>
        public string DrivewayState
        {
            get { return drivewayState; }
            set { drivewayState = value; }
        }

        private string cardCar;
        /// <summary>
        /// 车牌号
        /// </summary>
        public string CardCar
        {
            get { return cardCar; }
            set { cardCar = value; }
        }
        private string cardUser;
        /// <summary>
        /// 
        /// </summary>
        public string CardUser
        {
            get { return CardUser; }
            set { cardUser = value; }
        }
    }
}
