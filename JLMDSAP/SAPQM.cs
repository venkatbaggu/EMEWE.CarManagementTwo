using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Services.Description;
using JLMDSAP.sappwd;




namespace JLMDSAP
{
    public class SAPQM
    {
        public SAPQM()
        {
            //Common.GetDataSet();
        }


        /// <summary>
        /// 验证车牌号
        /// </summary>
        /// <param name="listdt"></param>
        /// <returns></returns>
        public DT_pd_info_upload_res SIbdinfouploadreq(DT_pd_info_upload_reqRow[] listdt)
        {
            DT_pd_info_upload_res dtc = null;
            try
            {
                BS_CARSYS_CQ_SI_pd_info_upload_req bsxx = new BS_CARSYS_CQ_SI_pd_info_upload_req();
                bsxx.Credentials = new System.Net.NetworkCredential("CQ_CARSYS", "1234nm,.");
                dtc = bsxx.SI_pd_info_upload_req(listdt);

            }
            catch (Exception ex)
            {
                Common.WriteTextLog("验证失败：" + ex.Message);
            }
            return dtc;
        }
    }
}
