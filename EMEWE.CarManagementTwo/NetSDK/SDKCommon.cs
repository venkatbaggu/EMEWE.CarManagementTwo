using System;
using System.Collections.Generic;
using System.Text;
using EMEWE.CarManagement.Commons.CommonClass;

namespace EMEWE.CarManagement.PreviewDemo
{
    /// <summary>
    /// 硬盘录像机预览和抓取图片公共方法
    /// </summary>
    public class SDKCommon
    {
        public static Int32 m_lUserID = -1;

        public static bool m_bInitSDK = false;
        public static Int32 m_lRealHandle = -1;


        /// <summary>
        /// 加载SDK
        /// </summary>
        public static bool InitSDK()
        {
            return m_bInitSDK = CHCNetSDK.NET_DVR_Init();

        }
        /// <summary>
        /// 关闭SDK
        /// </summary>
        public static void CloseSDK()
        {
            if (m_lRealHandle >= 0)
            {
                CHCNetSDK.NET_DVR_StopRealPlay(m_lRealHandle);
            }
            if (m_lUserID >= 0)
            {
                CHCNetSDK.NET_DVR_Logout_V30(m_lUserID);
            }
            if (m_bInitSDK == true)
            {
                CHCNetSDK.NET_DVR_Cleanup();
            }
        }


        /// <summary>
        /// 
        /// 登录硬盘录像机
        /// </summary>
        /// <param name="DVRIPAddress">Ip地址</param>
        /// <param name="DVRPortNumber">服务器端口号</param>
        /// <param name="DVRUserName">用户名</param>
        /// <param name="DVRPassword">密码</param>
        /// <returns></returns>
        public static int SetLogin(string DVRIPAddress, Int16 DVRPortNumber, string DVRUserName, string DVRPassword)
        {
            CHCNetSDK.NET_DVR_DEVICEINFO_V30 DeviceInfo = new CHCNetSDK.NET_DVR_DEVICEINFO_V30();
            m_lUserID = CHCNetSDK.NET_DVR_Login_V30(DVRIPAddress, DVRPortNumber, DVRUserName, DVRPassword, ref DeviceInfo);
            return m_lUserID;

        }


        #region 预览视频

        /// <summary>
        /// 设置硬盘录像机客户端设置
        /// </summary>
        /// <param name="ilChannel"></param>
        /// <returns></returns>
        public static CHCNetSDK.NET_DVR_CLIENTINFO GetRealPlay(int ilChannel, System.Windows.Forms.PictureBox RealPlayWnd)
        {
            CHCNetSDK.NET_DVR_CLIENTINFO lpClientInfo = new CHCNetSDK.NET_DVR_CLIENTINFO();
            lpClientInfo.hPlayWnd = RealPlayWnd.Handle;
            lpClientInfo.lChannel = ilChannel;
            lpClientInfo.lLinkMode = 0x0000;
            lpClientInfo.sMultiCastIP = "";
            return lpClientInfo;

        }
        /// <summary>
        /// 预览硬盘录像机视频
        /// </summary>
        /// <param name="lpClientInfo">预览参数 </param>
        /// <returns></returns>
        public static int RealPlay(int ilChannel, System.Windows.Forms.PictureBox RealPlayWnd)
        {
            CHCNetSDK.NET_DVR_CLIENTINFO lpClientInfo = GetRealPlay(ilChannel, RealPlayWnd);
            CHCNetSDK.REALDATACALLBACK RealData = new CHCNetSDK.REALDATACALLBACK(RealDataCallBack);
            IntPtr pUser = new IntPtr();
            m_lRealHandle = CHCNetSDK.NET_DVR_RealPlay_V30(m_lUserID, ref lpClientInfo, RealData, pUser, 1);
            return m_lRealHandle;

        }

        public static void RealDataCallBack(Int32 lRealHandle, UInt32 dwDataType, ref byte pBuffer, UInt32 dwBufSize, IntPtr pUser)
        {
        }
        #endregion
        #region 抓取图片
        /// <summary>
        /// 获取图片的存放位置
        /// </summary>
        /// <param name="strfileName">存放图片文件路径</param>
        /// <param name="strPosition">门岗序号</param>
        /// <param name="strDriveway">通道序号</param>
        /// <param name="ilChannel">摄像机序号</param>
        /// <returns>图片存储路径和图片名称图片格式</returns>
        public static string GetPicFileName(string strfileName, string strPosition, string strDriveway, Int32 ilChannel)
        {
            StringBuilder sb = new StringBuilder();
            string sPicFileName = CommonalityEntity.GetServersTime().ToString("yyMMddHHmmss") + strPosition + strDriveway + ilChannel.ToString();//图片名称
            sb.Append(strfileName + "\\");
            sb.Append(sPicFileName);
            sb.Append(".jpg");
            return sb.ToString();

        }
        static PreviewDemo.CHCNetSDK.NET_DVR_JPEGPARA jpegpara = new CHCNetSDK.NET_DVR_JPEGPARA();
        /// <summary>
        /// 抓取图片存放到指定路径
        /// </summary>
        /// <param name="strfileName">存放图片文件路径</param>
        /// <param name="strPosition">门岗序号</param>
        /// <param name="strDriveway">通道序号</param>
        /// <param name="ilChannel">摄像机序号</param>
        /// <returns></returns>
        public static void CaptureJPEGPicture(string strfileName, string strPosition, string strDriveway, Int32 ilChannel)
        {
            //string strSavePicFile = GetPicFileName(strfileName, strPosition, strDriveway, ilChannel);
            //jpegpara.wPicQuality =0;
            //jpegpara.wPicSize = 0;// "0-CIF";
            //CHCNetSDK.NET_DVR_CaptureJPEGPicture(m_lUserID, ilChannel, ref jpegpara, strSavePicFile);

            string strSavePicFile = GetPicFileName(strfileName, strPosition, strDriveway, ilChannel);
            CHCNetSDK.NET_DVR_JPEGPARA jpegpara = new CHCNetSDK.NET_DVR_JPEGPARA
            {
                wPicQuality = 0,//图像质量
                wPicSize = 0//图像大小
            };
            CHCNetSDK.NET_DVR_CaptureJPEGPicture(m_lUserID, ilChannel, ref jpegpara, strSavePicFile);

        }

        /// <summary>
        /// 预览抓取图片存放到指定路径
        /// </summary>
        /// <param name="RealPlayWnd">预览图片</param>
        /// <param name="strfileName">存放图片文件路径</param>
        /// <param name="strPosition">门岗序号</param>
        /// <param name="strDriveway">通道序号</param>
        /// <param name="ilChannel">摄像机序号</param>
        /// <returns></returns>
        public static void CaptureJPGPictureRealPlay(System.Windows.Forms.PictureBox RealPlayWnd, string strfileName, string strPosition, string strDriveway, Int32 ilChannel)
        {
            int ilrealHandle = RealPlay(ilChannel, RealPlayWnd);
            string strSavePicFile = GetPicFileName(strfileName, strPosition, strDriveway, ilChannel);
            CHCNetSDK.NET_DVR_CapturePicture(ilrealHandle, strSavePicFile);
        }


        /// <summary>
        /// 预览抓取图片存放到指定路径
        /// </summary>
        /// <param name="irealHandle">预览值</param>
        /// <param name="strfileName">存放图片文件路径</param>
        /// <param name="strPosition">门岗序号</param>
        /// <param name="strDriveway">通道序号</param>
        /// <param name="ilChannel">摄像机序号</param>
        /// <returns></returns>
        public static void CaptureJPGPictureRealPlay(int irealHandle, string strfileName, string strPosition, string strDriveway, Int32 ilChannel)
        {
            string strSavePicFile = GetPicFileName(strfileName, strPosition, strDriveway, ilChannel);
            CHCNetSDK.NET_DVR_CapturePicture(irealHandle, strSavePicFile);
        }
        #endregion
    }
}
