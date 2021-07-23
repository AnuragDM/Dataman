using BusinessLayer;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using System.Web.Services;

namespace AstralFFMS
{
    public partial class CoupanDemandGenerate : System.Web.UI.Page
    {       
        int cnt = 0;
        protected void Page_Load(object sender, EventArgs e)
        {      
            if (!IsPostBack)
            {
                BindDDLMonth();
                listboxmonth.SelectedValue = System.DateTime.Now.Month.ToString();
                YearListBox.SelectedValue = System.DateTime.Now.Year.ToString();
                BindDistributor();                                  
                btnExport.Visible = true;
                string pageName = Path.GetFileName(Request.Path);
                btnExport.Enabled = Settings.Instance.CheckExportPermission(pageName, Convert.ToString(Session["user_name"]));
                btnExport.CssClass = "btn btn-primary";
            }
        }

        private void BindDistributor()
        {
            try
            {
                string str = @"SELECT mp.PartyId,(mp.PartyName + ' - ' + ma.AreaName) AS PartyName FROM MastParty mp LEFT JOIN mastarea ma ON mp.AreaId=ma.AreaId where mp.partydist=1 and mp.Active=1 ORDER BY mp.PartyName";
                fillDDLDirect(ListBox1, str, "PartyId", "PartyName", 1);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        public static void fillDDLDirect(DropDownList xddl, string xmQry, string xvalue, string xtext, int sele)
        {
            DataTable xdt = new DataTable();
            xdt = DbConnectionDAL.GetDataTable(CommandType.Text, xmQry);
            xddl.DataSource = xdt;
            xddl.DataTextField = xtext.Trim();
            xddl.DataValueField = xvalue.Trim();
            xddl.DataBind();
            if (sele == 1)
            {
                if (xdt.Rows.Count >= 1)
                    xddl.Items.Insert(0, new ListItem("--Select--", "0"));
                else if (xdt.Rows.Count == 0)
                    xddl.Items.Insert(0, new ListItem("---", "0"));
            }
            else if (sele == 2)
            {
                xddl.Items.Insert(0, new ListItem("--Others--", "0"));
            }
            xdt.Dispose();
        }

        private void BindDDLMonth()
        {
            try
            {
                for (int month = 1; month <= 12; month++)
                {
                    string monthName = DateTimeFormatInfo.CurrentInfo.GetMonthName(month);
                    //listboxmonth.Items.Add(new ListItem(monthName.Substring(0, 3), month.ToString().PadLeft(2, '0')));
                    listboxmonth.Items.Add(new ListItem(monthName.Substring(0, 3), month.ToString()));
                }
                for (int i = System.DateTime.Now.Year - 1; i <= (System.DateTime.Now.Year); i++)
                {
                    YearListBox.Items.Add(new ListItem(i.ToString()));
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }       

        protected void btnGo_Click(object sender, EventArgs e)
        {
            BindGrid();
        }    
   
        private void BindGrid()
        {
            try
            {
                string month = DateTimeFormatInfo.CurrentInfo.GetMonthName(Convert.ToInt32(listboxmonth.SelectedValue));
                string year = YearListBox.SelectedValue;
                string Query = "SELECT mc.RetailerName,tr.Amount AS SaleAmount,(SELECT SaleAmount FROM CoupanSetUp) AS CoupanValue,Round((tr.Amount/ (SELECT SaleAmount FROM CoupanSetUp)),0) AS CoupanQty,tr.DistId,tr.Month,tr.Year,mc.Id FROM TransRetailerMonthWiseSale tr LEFT JOIN mastparty mp ON tr.DistId=mp.PartyId LEFT JOIN MastCoupanRetailer mc ON tr.RetailerId=mc.Id WHERE partydist=1 and tr.DistId in (" + ListBox1.SelectedValue + ") and tr.Month='" + month + "' AND tr.Year='" + year + "' and mc.Id NOT IN (SELECT RetailerId FROM TransCoupanDemand WHERE Month='" + month + "' AND Year='" + year + "') ORDER BY mc.RetailerName";
                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, Query);
                if (dt.Rows.Count > 0)
                {
                    distreportrpt.DataSource = dt;
                    distreportrpt.DataBind();                   
                    btnExport.Visible = true;
                    btnGenerate.Visible = true;
                }
                else
                {
                    distreportrpt.DataSource = dt;
                    distreportrpt.DataBind();                   
                    btnExport.Visible = false;
                    btnGenerate.Visible = false;
                }

            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/DistLedgerReport.aspx");
        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            int exists = 0;
            string insertquery = "";
            string docID = Settings.GetDocID("CDMND", DateTime.Now);
            Settings.SetDocID("CDMND", docID);
            try
            {
                foreach (RepeaterItem item in distreportrpt.Items)
                {
                    HiddenField hdnDistId = (HiddenField)item.FindControl("hdnDistId");
                    HiddenField hdnMonth = (HiddenField)item.FindControl("hdnMonth");
                    HiddenField hdnYear = (HiddenField)item.FindControl("hdnYear");
                    HiddenField hdnRetailerId = (HiddenField)item.FindControl("hdnRetailerId");

                    Label lblRetailerName = item.FindControl("lblRetailerName") as Label;
                    Label lblSaleAmount = item.FindControl("lblSaleAmount") as Label;
                    Label lblCoupanValue = item.FindControl("lblCoupanValue") as Label;
                    Label lblCoupanQty = item.FindControl("lblCoupanQty") as Label;

                    insertquery = @"Insert into TransCoupanDemand (DemandId,DistId,Month,Year,RetailerId,SaleValue,CoupanValue,CoupanQty,Createddate) values ('" + docID + "'," + hdnDistId.Value + ",'" + hdnMonth.Value + "','" + hdnYear.Value + "'," + hdnRetailerId.Value + ",'" + Settings.DMDecimal(lblSaleAmount.Text) + "','" + Settings.DMDecimal(lblCoupanValue.Text) + "', '" + Settings.DMDecimal(lblCoupanQty.Text) + "',DateAdd(minute,330,getutcdate()))";
                    exists = Convert.ToInt32(DbConnectionDAL.ExecuteQuery(insertquery));

                }

                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Coupan Demand Generated Successfuly');", true);
                BindGrid();

            }
            catch (Exception ex)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Error", "errormessage('Record cannot be insert');", true);
                ex.ToString();
            }
        }    

        protected void btnExport_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("content-disposition", "attachment;filename=DistributorLedgerReport.csv");
            string headertext = "Distributor Name".TrimStart('"').TrimEnd('"') + "," + "Debit".TrimStart('"').TrimEnd('"') + "," + "Credit".TrimStart('"').TrimEnd('"') + "," + "Closing Balance".TrimStart('"').TrimEnd('"');

            StringBuilder sb = new StringBuilder();
            sb.Append(headertext);
            sb.AppendLine(System.Environment.NewLine);
            string dataText = string.Empty;
            //
            DataTable dtParams = new DataTable();
            dtParams.Columns.Add(new DataColumn("DistributorName", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Debit", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Credit", typeof(String)));
            dtParams.Columns.Add(new DataColumn("ClosingBalance", typeof(String)));

            foreach (RepeaterItem item in distreportrpt.Items)
            {
                DataRow dr = dtParams.NewRow();
                Label DistributorLabel = item.FindControl("DistributorLabel") as Label;
                dr["DistributorName"] = DistributorLabel.Text;
                Label drLabel = item.FindControl("drLabel") as Label;
                dr["Debit"] = drLabel.Text.ToString();
                Label CrLabel = item.FindControl("CrLabel") as Label;
                dr["Credit"] = CrLabel.Text.ToString();
                Label cBalanceLabel = item.FindControl("cBalanceLabel") as Label;
                dr["ClosingBalance"] = cBalanceLabel.Text.ToString();

                dtParams.Rows.Add(dr);
            }

            for (int j = 0; j < dtParams.Rows.Count; j++)
            {
                for (int k = 0; k < dtParams.Columns.Count; k++)
                {
                    if (dtParams.Rows[j][k].ToString().Contains(","))
                    {
                        sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k].ToString()) + ',');
                    }
                    else if (dtParams.Rows[j][k].ToString().Contains(System.Environment.NewLine))
                    {
                        sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k].ToString()) + ',');
                    }
                    else
                    {
                        sb.Append(dtParams.Rows[j][k].ToString() + ',');
                    }
                }
                sb.Append(Environment.NewLine);
            }
            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("content-disposition", "attachment;filename=DistributorLedgerReport.csv");
            Response.Write(sb.ToString());
            Response.End();           
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
        }      
               
    }
}