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

namespace EMEWE.CarManagement.SystemAdmin
{
    public partial class CarImgInfoForm : Form
    {
        public CarImgInfoForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CarImgInfoForm_Load(object sender, EventArgs e)
        {
            ShowImage();
        }
        /// <summary>
        /// 显示图片
        /// </summary>
        private void ShowImage()
        {
            if (string.IsNullOrEmpty(CommonalityEntity.CarInfo_ID))
            {
                //为空，没有绑定车辆，显示单张图片
                string sql = "Select * from CarPic where CarPic_ID=" + CommonalityEntity.CarPic_ID + "";
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
                        wight = (720 - 10 * (doucount + 1)) / Convert.ToInt32(doucount);
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
                        wight = (720 - 10 * (sss + 2)) / Convert.ToInt32(sss);

                    }
                    height = (500 - 10 * (doucount + 1)) / Convert.ToInt32(doucount);
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
            else
            {
                #region 已匹配的照片
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
                        wight = (720 - 10 * (doucount + 1)) / Convert.ToInt32(doucount);
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
                        wight = (720 - 10 * (sss + 2)) / Convert.ToInt32(sss);

                    }
                    height = (500 - 10 * (doucount + 1)) / Convert.ToInt32(doucount);
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
                #endregion
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
                CommonalityEntity.WriteTextLog("pbInImageOne_MouseHover()" + "");
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
                //CommonalityEntity.WriteTextLog("ShowD()" );
            }
        }
        #endregion
    }
}
