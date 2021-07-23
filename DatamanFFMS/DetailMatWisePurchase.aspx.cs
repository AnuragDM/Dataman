using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AstralFFMS
{
    public partial class DetailMatWisePurchase : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["DistId"] != null && Request.QueryString["UnderId"] != null)
                {
                    string year = Request.QueryString["Year"];
                    string Month = Request.QueryString["Month"];
                    GetDetailPurchaseData(Convert.ToInt32(Request.QueryString["DistId"]), Convert.ToInt32(Request.QueryString["UnderId"]),year,Month);
                }
            }
        }

        private void GetDetailPurchaseData(int distId, int itemId, string year, string Month)
        {
            try
            {
                string dertatquery = @"select mi.ItemName AS [Item],sum(qty) as Qty,sum(Net_Total) as Amount from TransDistInv1 t1 left outer join MastItem mi
                                 on mi.ItemId=t1.ItemId left outer join MastParty da on da.PartyId=t1.DistId WHERE year(t1.VDate)='" + year + "' and month(t1.VDate)='" + Month + "' and t1.DistId=" + distId + " AND mi.Underid=" + itemId + " and da.PartyDist=1 group by mi.ItemName order by mi.ItemName";
                DataTable matDetDt = DbConnectionDAL.GetDataTable(CommandType.Text, dertatquery);
                if (matDetDt.Rows.Count > 0)
                {
                    rptDetPurchase.DataSource = matDetDt;
                    rptDetPurchase.DataBind();
                }
                else
                {
                    rptDetPurchase.DataSource = matDetDt;
                    rptDetPurchase.DataBind();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Home.aspx");
        }
    }
}