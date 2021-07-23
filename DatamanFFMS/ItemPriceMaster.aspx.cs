using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Data;
using System.Data.SqlClient;
using BusinessLayer;
using DAL;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Text;

namespace AstralFFMS
{
    public partial class ItemPriceMaster : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                List<Distributors> distributors = new List<Distributors>();
                distributors.Add(new Distributors());
                rpt.DataSource = distributors;
                rpt.DataBind();
            }
        }

        public class Distributors
        {
            public string VDate { get; set; }
            public string Distributor { get; set; }
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string UpdatePriceList(int ItemPriceId, string MRP, string DP, string RP)
        {
            string str = "", msg = "", itemPriceId = "false";
            try
            {
                if (ItemPriceId == 0) return "ItemPriceId can not blank.";

                decimal Mrp = 0;
                if (!string.IsNullOrEmpty(MRP))
                { Mrp = Convert.ToDecimal(MRP); }
                decimal Dp = 0;
                if (!string.IsNullOrEmpty(DP))
                { Dp = Convert.ToDecimal(DP); }
                decimal Rp = 0;
                if (!string.IsNullOrEmpty(RP))
                { Rp = Convert.ToDecimal(RP); }

                str = "update PRICELIST set MRP=" + Mrp + ",RP=" + Rp + ",DP=" + Dp + ",createdDate=dateadd(ss,19800,getutcdate())  output inserted.id where Id=" + ItemPriceId;
                itemPriceId = Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, str));
                if (itemPriceId == "")
                {
                    msg = "False";
                }
            }
            catch (Exception ex)
            {
                msg = "false " + ex.Message.ToString();
            }
            return JsonConvert.SerializeObject(msg);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string FillPriceList()
        {
            string str = "";
            DataTable dt = new DataTable();
            try
            {
                str = "SELECT row_number() over( order by WEFDATE DESC,i.ItemName) as Sno, p.id,i.ItemName,i.SyncId, format(P.WEFDATE,'dd/MM/yyyy')  WEFDATE,P.MRP,P.DP,P.RP, isnull(P.PriceListApplicability,'') PriceListApplicability, case when ISNULL(p.PriceListApplicability,'')='Country' then mCNT.AreaName when ISNULL(p.PriceListApplicability,'')='State' then mCNT.SyncId when ISNULL(p.PriceListApplicability,'')='City' then mCITY.SyncId when ISNULL(p.PriceListApplicability,'')='Dist' then mDIST.SyncId else '' end Country_State_City_Dist_SyncId   FROM PRICELIST P LEFT JOIN MASTITEM I ON I.ITEMID=P.ITEMID left join MastArea mCNT on mCNT.AreaId=p.Country_State_City_Dist_id  left join MastArea mST on mST.AreaId=p.Country_State_City_Dist_id  left join MastArea mCITY on mCITY.AreaId=p.Country_State_City_Dist_id  left join MastParty mDIST on mDIST.PartyId=p.Country_State_City_Dist_id ORDER BY WEFDATE DESC,i.ItemName";
                dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            }
            catch (Exception ex)
            {
                dt = null;

            }
            return JsonConvert.SerializeObject(dt);
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetItemPrice(string ItemPriceId)
        {
            string str = "";
            DataTable dt = new DataTable();
            try
            {
                if (ItemPriceId != "")
                {
                    str = "SELECT row_number() over( order by WEFDATE DESC,i.ItemName) as Sno, p.id,i.ItemName,i.SyncId, format(P.WEFDATE,'dd/MM/yyyy')  WEFDATE,P.MRP,P.DP,P.RP, isnull(P.PriceListApplicability,'') PriceListApplicability, case when ISNULL(p.PriceListApplicability,'')='Country' then mCNT.AreaName when ISNULL(p.PriceListApplicability,'')='State' then mCNT.SyncId when ISNULL(p.PriceListApplicability,'')='City' then mCITY.SyncId when ISNULL(p.PriceListApplicability,'')='Dist' then mDIST.SyncId else '' end Country_State_City_Dist_SyncId   FROM PRICELIST P LEFT JOIN MASTITEM I ON I.ITEMID=P.ITEMID left join MastArea mCNT on mCNT.AreaId=p.Country_State_City_Dist_id  left join MastArea mST on mST.AreaId=p.Country_State_City_Dist_id  left join MastArea mCITY on mCITY.AreaId=p.Country_State_City_Dist_id  left join MastParty mDIST on mDIST.PartyId=p.Country_State_City_Dist_id where p.id=" + ItemPriceId + "  ORDER BY WEFDATE DESC,i.ItemName";
                    dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                }

            }
            catch (Exception ex)
            {
                dt = null;

            }
            return JsonConvert.SerializeObject(dt);
        }


    }
}