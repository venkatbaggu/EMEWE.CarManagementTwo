using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using EMEWE.CarManagement.DAL;
using System.Data;
using EMEWE.CarManagement.Entity;
using System.Collections;
using EMEWE.CarManagement.Commons;

namespace EMEWE.CarManagement.Commons.CommonClass
{
    public static class ImageFile
    {
        /// <summary>
        /// WebClient上传文件至服务器
        /// </summary>
        /// <param name="fileNameFullPath">要上传的文件（全路径格式）</param>
        /// <param name="strUrlDirPath">Web服务器文件夹路径</param>
        /// <returns>True/False是否上传成功</returns>
        public static bool UpLoadFile(string fileNameFullPath, string strUrlDirPath)
        {
            bool rbool = false;
            try
            {
                string CreateFolderPath = @"" + strUrlDirPath;
                if (!Directory.Exists(CreateFolderPath))
                {
                    Directory.CreateDirectory(CreateFolderPath);
                }
                //得到要上传的文件文件名
                string fileName = fileNameFullPath.Substring(fileNameFullPath.LastIndexOf("\\") + 1);
                //新文件名由年月日时分秒及毫秒组成
                //得到文件扩展名
                string fileNameExt = fileName.Substring(fileName.LastIndexOf(".") + 1);
                if (!string.IsNullOrEmpty(fileNameExt))
                {
                    //保存在服务器上时，将文件改名
                    strUrlDirPath = strUrlDirPath + fileName;
                    // 创建WebClient实例
                    WebClient myWebClient = new WebClient();
                    myWebClient.Credentials = CredentialCache.DefaultCredentials;
                    // 将要上传的文件打开读进文件流
                    FileStream myFileStream = new FileStream(fileNameFullPath, FileMode.Open, FileAccess.Read);
                    BinaryReader myBinaryReader = new BinaryReader(myFileStream);

                    byte[] postArray = myBinaryReader.ReadBytes((int)myFileStream.Length);
                    //打开远程Web地址，将文件流写入
                    Stream postStream = myWebClient.OpenWrite(strUrlDirPath, "PUT");
                    if (postStream.CanWrite)
                    {
                        postStream.Write(postArray, 0, postArray.Length);
                        rbool = true;
                    }

                    postStream.Close();//关闭流
                    myWebClient.Dispose();
                    myFileStream.Close();
                    myBinaryReader.Close();


                }
            }
            catch (Exception ex)
            {
                CommonalityEntity.WriteTextLog("Web服务器文件目前不可写入，请检查Web服务器目录权限设置:" + ex.Message.ToString());
            }
            return rbool;
        }

        /// <summary>
        /// 下载服务器文件至客户端
        /// </summary>
        /// <param name="strUrlFilePath">要下载的Web服务器上的文件地址（全路径)
        /// <param name="Dir">下载到的目录（存放位置，本地机器文件夹）</param>
        /// <returns>True/False是否上传成功</returns>
        public static bool DownLoadFile(string strUrlFilePath, string strLocalDirPath)
        {

            // 创建WebClient实例
            WebClient client = new WebClient();

            //被下载的文件名
            string fileName = strUrlFilePath.Substring(strUrlFilePath.LastIndexOf("\\"));

            //另存为的绝对路径＋文件名
            string Path = strLocalDirPath + fileName;

            try
            {
                WebRequest myWebRequest = WebRequest.Create(strUrlFilePath);
            }
            catch
            {
                CommonalityEntity.WriteTextLog("文件下载失败：");
            }
            try
            {
                client.DownloadFile(strUrlFilePath, Path);
                return true;
            }
            catch
            {
                CommonalityEntity.WriteTextLog("文件下载失败：");
                return false;
            }

        }

        /// <summary>
        /// 复制图片
        /// </summary>
        /// <param name="filepath1">原图片路径（全路径)</param>
        /// <param name="filepath2">新路径</param>
        public static void Copt(string filepath1, string filepath2)
        {
            FileInfo file = new FileInfo(filepath1);
            file.CopyTo(filepath2, true);
        }

        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="path">图片路径</param>
        public static void Delete(string path)
        {
            //if (!File.Exists(path))
            //{
            try
            {

                File.Delete(path);
            }
            catch
            {
                CommonalityEntity.WriteTextLog("删除图片异常");
            }

            //}
        }
        /// <summary>
        /// 移动图片
        /// </summary>
        /// <param name="path1">原图片路径（全路径)</param>
        /// <param name="path2">新路径(完成的路径包括新名称)</param>
        public static void MoveImage(string path1, string path2)
        {
            File.Move(path1, path2);
        }
        public static ArrayList listWarning = new ArrayList();//异常信息存放
        /// <summary>
        /// 获取图片名称信息
        /// </summary>
        /// <param name="doubleMin">图片有效时间</param>
        /// <param name="path">路径</param>
        /// <param name="isCheck">是否验证有效 true验证  false 不验证</param>
        /// <returns></returns>
        public static List<string> GetImage(string path, bool isCheck)
        {
            List<string> list = new List<string>();
            string[] image = null;
            image = Directory.GetFiles(path);
            var images = from m in image orderby m descending select m;
            if (isCheck)
            {
                //验证时间，门岗，通道，摄像机编号
            }
            else
            {
                foreach (var item in image)
                {
                    list.Add(item);//登记照片，将所有的登记拍照信息返回
                }
            }
            #region 注释  老版本
            //string[] images = null;
            //try
            //{
            //    if (strImagefile != "")
            //    {
            //        images = Directory.GetFiles(strImagefile);
            //        var image = from m in images
            //                    orderby m descending
            //                    select m;
            //        //根据通道编号得到该通道下的摄像机名称
            //        DataSet camera = LinQBaseDao.Query("select * from eh_Camera where Driveway_id=" + CommonalityEntity.Driveway_Id + "");
            //        foreach (DataRow dr in camera.Tables[0].Rows)
            //        {
            //            foreach (string pathStr in image)
            //            {
            //                string fileName = pathStr.Substring(pathStr.LastIndexOf('\\') + 1);//图片名称
            //                string fileKind = fileName.Substring(fileName.LastIndexOf('.') + 1);//图片后缀
            //                //验证是否为图片
            //                if (fileKind.ToLower() != "jpg" && fileKind.ToLower() != "jpeg" && fileKind.ToLower() != "png" && fileKind.ToLower() != "gif" && fileKind.ToLower() != "bmp")
            //                {
            //                    continue;
            //                }
            //                string mi = GetTime(fileName.ToString());//获取时间差

            //                if (double.Parse(mi) <= doubleMin)//判断是否过期
            //                {
            //                    if (fileName.Length == 21)//判断图片名称字符长度
            //                    {
            //                        string camera_addCard = dr["Camera_AddCard"].ToString();
            //                        if (dr["Camera_AddCard"].ToString() == fileName.Substring(16, 1))
            //                        {

            //                            list.Add(fileName);
            //                            break;

            //                        }
            //                    }
            //                    else if (fileName.Length == 22)
            //                    {
            //                        if (dr["Camera_AddCard"].ToString() == fileName.Substring(16, 2))
            //                        {
            //                            list.Add(fileName);
            //                            break;
            //                        }
            //                    }

            //                }
            //            }
            //            if (list.Count == camera.Tables[0].Rows.Count)
            //            {
            //                break;
            //            }
            //        }

            //    }
            //}
            //catch 
            //{
            //    CommonalityEntity.WriteTextLog("ImageFile GetImages()" + "");
            //}
            #endregion
            return list;
        }

        ///// <summary>
        ///// 获取存放临时图片文件夹名称
        ///// </summary>
        ///// <param name="strPostion">门岗</param>
        ///// <param name="strDrivewayname">通道</param>
        ///// <param name="state">in为进门，out为出门</param>
        ///// <returns>返回信息格式"~/cartwoin1_files/"</returns>
        //public static string GetStrImageFile()
        //{
        //    StringBuilder sb = new StringBuilder("");
        //    try
        //    {
        //        //初始化名称
        //        sb.Append(SystemClass.BaseFile + "car");
        //        //添加门岗
        //        sb.Append(SystemClass.PosistionValue);
        //        sb.Append(common.CurrentChannelNumber);
        //        #region 注释：不正确，读这个是错误的。
        //        // CardEntity ce = new CardEntity();
        //        //if (common.SerialnumberICbool)
        //        //{
        //        //    sb.Append(common.CurrentChannelNumber);
        //        //}
        //        //else
        //        //{
        //        //    //添加通道
        //        //    sb.Append(ce.DrivewayPort);
        //        //}
        //        #endregion

        //    }
        //    catch 
        //    {
        //        common.WriteTextLog("ImageFile GetStrImageFile():" + "");
        //    }
        //    return sb.ToString().Trim();

        //}

        /// <summary>
        /// 获取存放临时图片文件夹名称
        /// </summary>
        /// <param name="isCheck">登记路径or通行路径 true为通行图片路径 false为登记图片路径</param>
        /// <returns>返回信息格式"~/cartwoin1_files/"</returns>
        public static string GetStrImageFile(bool isCheck, string DIRV)
        {
            StringBuilder sb = new StringBuilder("");
            try
            {
                if (isCheck)
                {
                    //初始化名称
                    sb.Append(SystemClass.BaseFile + "Car");
                    //添加门岗
                    sb.Append(SystemClass.PosistionValue);
                    sb.Append(DIRV + "\\");
                }
                else
                {
                    sb.Append(SystemClass.BaseFile + "Car");
                    sb.Append(SystemClass.PosistionValue + "\\");
                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("ImageFile GetStrImageFile():");
            }
            return sb.ToString().Trim();

        }
        /// <summary>
        /// 当前系统时间
        /// </summary>
        public static DateTime t = CommonalityEntity.GetServersTime();

        /// <summary>
        /// 得到时间差
        /// </summary>
        /// <param name="d">需要获取的时间</param>
        /// <param name="length">字符长度</param>
        /// <returns></returns>
        public static string GetTime(string d)
        {
            string times = "";
            try
            {
                if (d.Length < 21)
                {
                    return "0";
                }
                string y = "20" + d.Substring(0, 2);
                string m = d.Substring(2, 2);
                string dd = d.Substring(4, 2);
                string h = d.Substring(6, 2);
                string mi = d.Substring(8, 2);
                string ss = d.Substring(10, 2);
                string time = y + "-" + m + "-" + dd + " " + h + ":" + mi + ":" + ss;
                DateTime dt = DateTime.Parse(time);
                t = dt;
                string sqltime = "select top 1 DATEDIFF(MINUTE,'" + dt + "',GETDATE())";
                var ds = LinQBaseDao.GetSingle(sqltime);
                times = ds.ToString();
            }
            catch
            {
                CommonalityEntity.WriteTextLog("ImageFile GetTime()");
            }
            return times;
        }


        /// <summary>
        /// 获取存放临时图片文件夹名称
        /// </summary>
        /// <param name="strPostion">门岗</param>
        /// <param name="strDrivewayname">通道</param>
        /// <param name="state">in为进门，out为出门</param>
        /// <returns>返回信息格式"~/cartwoin1_files/"</returns>
        public static string GetStrImageFile()
        {
            StringBuilder sb = new StringBuilder("");
            try
            {
                //初始化名称
                sb.Append(SystemClass.BaseFile + "car");
                //添加门岗
                sb.Append(SystemClass.PosistionValue);
                sb.Append(CommonalityEntity.CurrentChannelNumber);
            }
            catch
            {
                CommonalityEntity.WriteTextLog("ImageFile GetStrImageFile():");
            }
            return sb.ToString().Trim();

        }
    }
}
