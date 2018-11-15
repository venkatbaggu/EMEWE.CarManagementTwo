using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EMEWE.CarManagement.Commons.CommonClass;
using EMEWE.CarManagement.DAL;
using EMEWE.CarManagement.Entity;
using System.Threading;

namespace EMEWE.CarManagement.SystemAdmin
{
    public partial class CarInfoInOutRecordForm : Form
    {
        string where = " 1=1";
        public CarInfoInOutRecordForm()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CarInfoDetailForm_Load(object sender, EventArgs e)
        {
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;

            ThreadPool.QueueUserWorkItem(new WaitCallback(a), null);
            GetCarInfo();
            GetStaffInfo();
        }
        public delegate void dAdd();
        public dAdd myAdd;
        public void a(object obj)
        {
            myAdd = new dAdd(ShowImage);
            gbShowImage.Invoke(myAdd);
        }
        /// <summary>
        /// 绑定车辆信息
        /// </summary>
        private void GetCarInfo()
        {
            if (CommonalityEntity.CarInfo_ID == "")
            {
                MessageBox.Show("参数错误");
                this.Close();
            }
            else
            {
                string sql = "Select * from CarInfo where CarInfo_ID=" + CommonalityEntity.CarInfo_ID + "";
                CarInfo carinfo = new CarInfo();
                carinfo = CarInfoDAL.GetViewCarInfoName(sql).FirstOrDefault();
                if (carinfo.CarInfo_ID > 0)
                {

                    txtCarInfo_Carriage.Text = carinfo.CarInfo_Carriage;
                    txtCarInfo_Height.Text = carinfo.CarInfo_Height;
                    txtCarInfo_Type.Text = carinfo.CarInfo_Type;
                    txtCarInfo_Weight.Text = carinfo.CarInfo_Weight;
                    txtCarName.Text = carinfo.CarInfo_Name;
                    txtCustomerInfo_ADD.Text = carinfo.CarInfo_LevelWaste;
                    txtRemark.Text = carinfo.CarInfo_Remark;
                    txtUserName.Text = carinfo.CarInfo_Operate;
                    txtState.Text = carinfo.CarInfo_State;
                    string typeSql = "Select CarType_Name from CarType where CarType_ID=" + carinfo.CarInfo_CarType_ID + "";
                    object typeObj = null;
                    typeObj = LinQBaseDao.GetSingle(typeSql);
                    if (typeObj != null)
                    {
                        txtCarType_Name.Text = typeObj.ToString();
                    }
                    if (Convert.ToBoolean(carinfo.CarInfo_Bail))
                    {
                        chkCarInfo_Bail.Checked = true;
                    }
                    else
                    {
                        chkCarInfo_Bail.Checked = false;
                    }
                    sql = "select StaffInfo_Name,StaffInfo_Identity,StaffInfo_Phone ,CarType_Name,CustomerInfo_Name from View_CarState where CarInfo_ID=" + CommonalityEntity.CarInfo_ID;
                    DataTable dt = LinQBaseDao.Query(sql).Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        txtCur.Text = dt.Rows[0]["CustomerInfo_Name"].ToString();
                        txtCarType_Name.Text = dt.Rows[0]["CarType_Name"].ToString();
                        txtStaffName.Text = dt.Rows[0]["StaffInfo_Name"].ToString();
                        txtStaInds.Text = dt.Rows[0]["StaffInfo_Identity"].ToString();
                        txtStaPhone.Text = dt.Rows[0]["StaffInfo_Phone"].ToString();
                    }
                }
            }
        }
        /// <summary>
        /// 绑定驾驶员信息
        /// </summary>
        private void GetStaffInfo()
        {
            where = " CarInOutInfoRecord_CarInOutRecord_ID=" + CommonalityEntity.CarInoutid;
            GetGriddataviewLoad("");
        }
        #region 分页和加载DataGridView
        /// <summary>
        /// 取消全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tslNotSelects()
        {
            for (int i = 0; i < this.dgvStaffInfo.Rows.Count; i++)
            {
                dgvStaffInfo.Rows[i].Selected = false;
            }
        }
        /// <summary>
        /// 全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tslSelectAlls()
        {
            for (int i = 0; i < dgvStaffInfo.Rows.Count; i++)
            {
                this.dgvStaffInfo.Rows[i].Selected = true;
            }
        }
        /// <summary>
        /// 加载信息
        /// </summary>
        public void GetGriddataviewLoad(string strClickedItemName)
        {
            Page.BindBoundControl(dgvStaffInfo, strClickedItemName, tstbPageCurrent, tslPageCount, tslNMax, tscbxPageSize, "View_CarinoutStaffInfo", "*", "CarInOutInfoRecord_ID", "CarInOutInfoRecord_ID", 0, where, true);

        }

        EMEWE.CarManagement.Commons.CommonClass.PageControl Page = new EMEWE.CarManagement.Commons.CommonClass.PageControl();
        /// <summary>
        /// 选择每页条数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tscbxPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetGriddataviewLoad("");
        }
        /// <summary>
        /// 分页控件事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bdnInfo_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
        }
        #endregion
        /// <summary>
        /// 显示图片
        /// </summary>
        private void ShowImage()
        {
            string sql = "Select * from CarPic where CarPic_CarInfo_ID=" + CommonalityEntity.CarInfo_ID + "";
            List<CarPic> list = new List<CarPic>();
            list = LinQBaseDao.GetItemsForListing<CarPic>(sql).ToList();
            if (list.Count >= 1)
            {
                //得到图片的路径
                string path = SystemClass.SaveFile;
                ////获取文件夹下的图片信息
                //List<string> strList = ImageFile.GetImage(path, false);
                //将得到的图片信息绑定到页面
                int listcount = list.Count();
                double wight = 0, height = 0;
                int sss = 0, mmm = 0;
                double doucount = Math.Sqrt(listcount);
                if (Convert.ToInt32(doucount) == doucount)
                {
                    wight = (380 - 10 * (doucount + 1)) / Convert.ToInt32(doucount);
                    sss = Convert.ToInt32(doucount);
                }
                else
                {
                    if (doucount - (int)(doucount) == 0)
                    {
                        sss = Convert.ToInt32(doucount);
                    }
                    else
                    {
                        sss = (int)(doucount + 1);
                    }
                    wight = (380 - 10 * (sss + 2)) / Convert.ToInt32(sss);

                }
                height = (300 - 10 * (doucount + 1)) / Convert.ToInt32(doucount);
                mmm = Math.Abs((int)(listcount / sss) - ((double)listcount / (double)sss)) == 0 ? (listcount / sss) : (int)(((double)listcount / (double)sss)) + 1;
                int x = 0;
                int y = 0;
                gbShowImage.Controls.Clear();
                int k = 0;
                for (int i = 0; i < mmm; i++)
                {
                    y = Convert.ToInt32(10 * (i + 1) + i * height) + 10;
                    for (int m = 0; m < sss; m++)
                    {
                        if (k >= listcount)
                        {
                            return;
                        }
                        x = Convert.ToInt32(10 * (m + 1) + m * wight) + 5;
                        PictureBox pb = new PictureBox();
                        pb.Location = new Point(x, y);
                        pb.Width = Convert.ToInt32(wight);
                        pb.Height = Convert.ToInt32(height);
                        pb.SizeMode = pictureBox1.SizeMode;
                        pb.MouseHover += new System.EventHandler(this.pbInImageOne_MouseHover);
                        pb.MouseLeave += new System.EventHandler(this.pbInImageOne_MouseLeave);
                        pb.Name = "pictureBox" + list[k].CarPic_ID.ToString();
                        pb.Tag = path + list[k].CarPic_Add;
                        pb.ImageLocation = path + list[k].CarPic_Add;
                        this.gbShowImage.Controls.Add(pb);
                        k++;
                    }
                }
            }
        }
        #region 图片放大缩小事件
        /// <summary>
        /// 鼠标离开控件可见部分时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pbInImageOne_MouseLeave(object sender, EventArgs e)
        {
            ShowD();
        }
        /// <summary>
        /// 鼠标悬停一定时间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pbInImageOne_MouseHover(object sender, EventArgs e)
        {
            try
            {
                if (sender != null)
                {
                    PictureBox pic = sender as PictureBox;
                    if (pic.Tag != null)
                    {
                        ShowD(pic.Tag.ToString());
                    }
                }

            }
            catch 
            {
                CommonalityEntity.WriteTextLog("pbInImageOne_MouseHover()" );
            }
        }
        public Bitmap b = null;
        /// <summary>
        /// 显示
        /// </summary>
        /// <param name="FileName"></param>
        public void ShowD(string FileName)
        {
            groupBox7.BringToFront();
            b = new Bitmap(FileName);
            pictureBox1.Image = b;
            groupBox7.Visible = true;

        }
        /// <summary>
        /// 隐藏图片
        /// </summary>
        /// <param name="pb"></param>
        public void ShowD()
        {
            try
            {
                groupBox7.Visible = false;
                b.Dispose();
                //移至底层
                groupBox7.SendToBack();
            }
            catch 
            {
               // CommonalityEntity.WriteTextLog("ShowD()" );
            }
        }
        #endregion

        private void dgvStaffInfo_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            SolidBrush B = new SolidBrush(Color.Black);
            e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, B, e.RowBounds.Location.X + 20, e.RowBounds.Location.Y + 4);
        }
    }
}
