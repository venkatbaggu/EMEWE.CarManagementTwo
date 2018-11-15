using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EMEWE.CarManagement.Commons.CommonClass;
using EMEWE.CarManagement.GetPhoto;
using System.IO;

namespace EMEWE.CarManagement.SystemAdmin
{
    public partial class CutPic : Form
    {
        public CutPic()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 当前选中摄像头
        /// </summary>
        int channel = 0;

        /// <summary>
        /// 默认摄像头个数
        /// </summary>
        int channelnum = 1;
        /// <summary>
        /// 用户句柄
        /// </summary>
        private int m_lRealHandle = -1;
        /// <summary>
        /// 用户ID
        /// </summary>
        public static int m_lUserID = -1;

        public static CutPic ct = null;

        private void CutPic_Load(object sender, EventArgs e)
        {
            //InitSDK();
            //base.Hide();//
            //SetLogin();
            //GetRealPlay();
        }


        private void btnPhoto_Click(object sender, EventArgs e)
        {
            CapturePic(SystemClass.lChannel);
            this.Close();
        }

        /// <summary>
        /// 登录硬盘录像机(默认)
        /// </summary>
        public void SetLogin()
        {
            string DVRIPAddress = SystemClass.DVRIPAddress;
            short DVRPortNumber = SystemClass.DVRPortNumber;
            string DVRUserName = SystemClass.DVRUserName;
            string DVRPassword = SystemClass.DVRPassword;
            m_lUserID = SDKCommon.SetLogin(DVRIPAddress, DVRPortNumber, DVRUserName, DVRPassword);
            if (m_lUserID != -1)
            {
                //DebugInfo("登录硬盘录像机成功！");
            }
            else
            {
                //DebugInfo("登录硬盘录像机失败！");
                return;
            }
        }
        /// <summary>
        /// 初始化
        /// </summary>
        public void InitSDK()
        {
            if (!SDKCommon.InitSDK())
            {
                CommonalityEntity.WriteTextLog("硬盘录像机初始化失败！");
            }
        }
        /// <summary>      
        /// 实时视频信息
        /// </summary>   
        private void GetRealPlay()
        {
            try
            {

                CHCNetSDK.NET_DVR_PREVIEWINFO lpPreviewInfo = new CHCNetSDK.NET_DVR_PREVIEWINFO();
                lpPreviewInfo.hPlayWnd = pictureBox1.Handle;//预览窗口
                lpPreviewInfo.lChannel = SystemClass.lChannel;//预te览的设备通道
                lpPreviewInfo.dwStreamType = 0;//码流类型：0-主码流，1-子码流，2-码流3，3-码流4，以此类推
                lpPreviewInfo.dwLinkMode = 0;//连接方式：0- TCP方式，1- UDP方式，2- 多播方式，3- RTP方式，4-RTP/RTSP，5-RSTP/HTTP 
                lpPreviewInfo.bBlocked = true; //0- 非阻塞取流，1- 阻塞取流

                CHCNetSDK.REALDATACALLBACK RealData = new CHCNetSDK.REALDATACALLBACK(RealDataCallBack);//预览实时流回调函数
                IntPtr pUser = new IntPtr();//用户数据

                //打开预览 Start live view 
                m_lRealHandle = CHCNetSDK.NET_DVR_RealPlay_V40(m_lUserID, ref lpPreviewInfo, null/*RealData*/, pUser);
                if (m_lRealHandle < 0)
                {
                    uint iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                }
            }
            catch (Exception ex)
            {
                CommonalityEntity.WriteTextLog(ex.Message);
            }
        }
        public void RealDataCallBack(Int32 lRealHandle, UInt32 dwDataType, ref byte pBuffer, UInt32 dwBufSize, IntPtr pUser)
        {
        }


        public void CapturePic(int channel)
        {
            if (channel == 0)
            {
                return;
            }
            try
            {
                InitSDK();
                base.Hide();//
                SetLogin();
                string strDirectoryName = SystemClass.BaseFile + "Car" + SystemClass.PosistionValue + "\\";
                if (!Directory.Exists(strDirectoryName))
                {
                    Directory.CreateDirectory(strDirectoryName);
                }
                string strfileImage = SDKCommon.CaptureJPEGPicture(strDirectoryName, SystemClass.PosistionValue, SystemClass.DrivewayValue, channel);
            }
            catch
            {

            }
        }

        private void CutPic_FormClosing(object sender, FormClosingEventArgs e)
        {
            ct = null;
        }
    }
}
