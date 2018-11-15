using System;
using System.Text;
using EMEWE.CarManagement.Commons.CommonClass;

namespace EMEWE.CarManagement.GetPhoto
{
    public class SDKCommon
    {
        /// <summary>
        /// DVR初始化结果
        /// </summary>
        public static bool m_bInitSDK = false;
        /// <summary>
        /// 实时预览返回值，-1为失败
        /// </summary>
        public static int m_lRealHandle = -1;
        /// <summary>
        /// 用户ID
        /// </summary>
        public static int m_lUserID = -1;

        #region 初始化DRV
        /// <summary>
        /// 初始化SDK，调用其他SDK函数的前提。
        /// </summary>
        /// <returns>TRUE表示成功，FALSE表示失败。</returns>
        public static bool InitSDK()
        {
            return (m_bInitSDK = CHCNetSDK.NET_DVR_Init());
        }
        #endregion

        #region 登录到硬盘录像机
        /// <summary>
        /// 登录硬盘录像机
        /// </summary>
        /// <param name="DVRIPAddress">IP地址</param>
        /// <param name="DVRPortNumber">端口号</param>
        /// <param name="DVRUserName">用户名</param>
        /// <param name="DVRPassword">密码</param>
        /// <returns>-1表示失败，其他值表示返回的用户ID值。
        /// 该用户ID具有唯一性，后续对设备的操作都需要通过此ID实现。
        /// 接口返回失败请调用NET_DVR_GetLastError获取错误码，
        /// 通过错误码判断出错原因。
        /// </returns>
        public static int SetLogin(string DVRIPAddress, short DVRPortNumber, string DVRUserName, string DVRPassword)
        {
            //设备信息
            CHCNetSDK.NET_DVR_DEVICEINFO_V30 DeviceInfo = new CHCNetSDK.NET_DVR_DEVICEINFO_V30();
            return (m_lUserID = CHCNetSDK.NET_DVR_Login_V30(DVRIPAddress, DVRPortNumber, DVRUserName, DVRPassword, ref DeviceInfo));
        }
        #endregion

      
        /// <summary>
        /// 拍照
        /// </summary>
        /// <param name="strfileName">保存路径</param>
        /// <param name="strPosition">门岗值</param>
        /// <param name="strDriveway">通道值</param>
        /// <param name="ilChannel">摄像头</param>
        /// <returns>返回完整路径为成功，返回“”为失败。</returns>
        public static string CaptureJPEGPicture(string strfileName, string strPosition, string strDriveway, int ilChannel)
        {
            string strSavePicFile = GetPicFileName(strfileName, strPosition, strDriveway, ilChannel);
            CHCNetSDK.NET_DVR_JPEGPARA jpegpara = new CHCNetSDK.NET_DVR_JPEGPARA
            {
                wPicQuality = 0,//图像质量
                wPicSize = 0//图像大小
            };
            bool result = CHCNetSDK.NET_DVR_CaptureJPEGPicture(m_lUserID, ilChannel, ref jpegpara, strSavePicFile);
            if (result)
            {
                return strSavePicFile;
            }
            else
            {
                return "";
            }

        }

        

        /// <summary>
        /// 拍照取名
        /// </summary>
        /// <param name="strfileName">保存路径</param>
        /// <param name="strPosition">门岗值</param>
        /// <param name="strDriveway">通道值</param>
        /// <param name="ilChannel">摄像头</param>
        /// <returns>拼接后的路径</returns>
        public static string GetPicFileName(string strfileName, string strPosition, string strDriveway, int ilChannel)
        {
            StringBuilder sb = new StringBuilder();
            string sPicFileName = CommonalityEntity.GetServersTime().ToString("yyMMddHHmmss") + strPosition + strDriveway + ilChannel.ToString();
            sb.Append(strfileName);
            sb.Append(sPicFileName);
            sb.Append(".jpg");
            return sb.ToString();
        }



        public static void RealDataCallBack(int lRealHandle, uint dwDataType, ref byte pBuffer, uint dwBufSize, IntPtr pUser)
        {
        }

        public static int RealPlay(CHCNetSDK.NET_DVR_CLIENTINFO lpClientInfo)
        {
            CHCNetSDK.REALDATACALLBACK RealData = new CHCNetSDK.REALDATACALLBACK(SDKCommon.RealDataCallBack);
            IntPtr pUser = new IntPtr();
            m_lRealHandle = CHCNetSDK.NET_DVR_RealPlay_V30(m_lUserID, ref lpClientInfo, RealData, pUser, 1);
            return m_lRealHandle;
        }

        #region 关闭SDK
        public static void CloseSDK()
        {
            if (m_lRealHandle >= 0)
            {
                CHCNetSDK.NET_DVR_StopRealPlay(m_lRealHandle);
            }
            if (m_lUserID >= 0)
            {
                CHCNetSDK.NET_DVR_Logout_V30(m_lUserID);
                //CHCNetSDK.NET_DVR_Logout(m_lUserID);
            }
            if (m_bInitSDK)
            {
                CHCNetSDK.NET_DVR_Cleanup();
            }
        }
        #endregion
    }
}

