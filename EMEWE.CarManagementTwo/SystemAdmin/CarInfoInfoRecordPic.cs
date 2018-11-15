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
    public partial class CarInfoInfoRecordPic : Form
    {
        public CarInfoInfoRecordPic()
        {
            InitializeComponent();
        }

        private void CarInfoInfoRecordPic_Load(object sender, EventArgs e)
        {
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
            ThreadPool.QueueUserWorkItem(new WaitCallback(a), null);
            GetCarInOutinfo();

           
        }

        public delegate void dAdd();
        public dAdd myAdd;
        public void a(object obj)
        {
            myAdd = new dAdd(ShowImage);
            gbShowImage.Invoke(myAdd);
        }


        private void GetCarInOutinfo()
        {
            DataTable dt = LinQBaseDao.Query("select CarInfo_Name,CarType_Name,CarInfo_Operate,CustomerInfo_Name,StaffInfo_Name,StaffInfo_Identity,CarInOutInfoRecord_ICType,CarInOutInfoRecord_ICValue,CarInOutInfoRecord_UserName,CarInOutInfoRecord_Remark,CarInOutInfoRecord_Time,CarInOutInfoRecord_Abnormal from View_carInOutSatistics where  CarInOutInfoRecord_ID=" + CommonalityEntity.CarInOutInfoRecord_ID).Tables[0];
            if (dt.Rows.Count > 0)
            {
                txtCarName.Text = dt.Rows[0]["CarInfo_Name"].ToString();
                txtCarType_Name.Text = dt.Rows[0]["CarType_Name"].ToString();
                txtCur.Text = dt.Rows[0]["CustomerInfo_Name"].ToString();
                txtICtype.Text = dt.Rows[0]["CarInOutInfoRecord_ICType"].ToString();
                txtICValue.Text = dt.Rows[0]["CarInOutInfoRecord_ICValue"].ToString();
                txtStaffName.Text = dt.Rows[0]["StaffInfo_Name"].ToString();
                txtStaInds.Text = dt.Rows[0]["StaffInfo_Identity"].ToString();
                txtUser.Text = dt.Rows[0]["CarInOutInfoRecord_UserName"].ToString();
                txtPicAdd.Text = dt.Rows[0]["CarInOutInfoRecord_Remark"].ToString();
                txtOutTime.Text = dt.Rows[0]["CarInOutInfoRecord_Time"].ToString();
                txtUserName.Text = dt.Rows[0]["CarInfo_Operate"].ToString();
                lbException.Text = dt.Rows[0]["CarInOutInfoRecord_Abnormal"].ToString();
                dt = LinQBaseDao.Query("select  CarPic_Type,CarPic_Add,CarPic_Match  from CarPic where CarPic_CarInOutInfoRecord_ID=" + CommonalityEntity.CarInOutInfoRecord_ID).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    txtPicType.Text = dt.Rows[0]["CarPic_Type"].ToString();
                    txtISTrue.Text = dt.Rows[0]["CarPic_Match"].ToString();
                }

            }
        }

        /// <summary>
        /// 显示图片
        /// </summary>
        private void ShowImage()
        {
            string sql = "Select * from CarPic where CarPic_CarInOutInfoRecord_ID=" + CommonalityEntity.CarInOutInfoRecord_ID + "";
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
                    wight = (480 - 10 * (doucount + 1)) / Convert.ToInt32(doucount);
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
                    wight = (480 - 10 * (sss + 2)) / Convert.ToInt32(sss);

                }
                height = (430 - 10 * (doucount + 1)) / Convert.ToInt32(doucount);
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
                CommonalityEntity.WriteTextLog("pbInImageOne_MouseHover()");
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
               // CommonalityEntity.WriteTextLog("ShowD()");
            }
        }
        #endregion

    }
}
