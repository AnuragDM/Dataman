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
    public partial class DistributorPrint : System.Web.UI.Page
    {
        string distid = "";
        static string prevPage = String.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if(Request.QueryString["ID"]!=null)
            {
                distid = Request.QueryString["ID"].ToString();
                fillinitialrecord();
            }
            if (!IsPostBack)
            {
                prevPage = Request.UrlReferrer.ToString();
            }

        }
        private void fillinitialrecord()
        {
            string s = "select p.PartyName,d.PaymentDate,d.Mode,isnull(d.Cheque_DD_Date,'') AS Cheque_DD_Date, d.Cheque_DDNo,d.Amount,d.Bank,d.Branch from [DistributerCollection] d left join MastParty p on d.DistId=p.PartyId where d.CollId=" + distid;
              DataTable depdt = DbConnectionDAL.GetDataTable(CommandType.Text, s);
              if (depdt.Rows.Count > 0)
              {
                  lbldistName.Text = depdt.Rows[0]["PartyName"].ToString();
                  lblAmount.Text = depdt.Rows[0]["Amount"].ToString();
                  lblDate.Text = DateTime.Parse(Convert.ToDateTime(depdt.Rows[0]["PaymentDate"]).ToShortDateString()).ToString("dd/MMM/yyyy");
                  lblMode.Text = depdt.Rows[0]["Mode"].ToString();
                  if (Convert.ToString(depdt.Rows[0]["Cheque_DD_Date"]) != "")
                  {
                      lblCheqDate.Text = DateTime.Parse(Convert.ToDateTime(depdt.Rows[0]["Cheque_DD_Date"]).ToShortDateString()).ToString("dd/MMM/yyyy");// depdt.Rows[0]["Cheque_DD_Date"].ToString();
                  }
                  lbCheqNo.Text = depdt.Rows[0]["Cheque_DDNo"].ToString();
              }
              else
              {

              }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect(prevPage);
        }
    }
}