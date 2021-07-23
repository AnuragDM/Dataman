using BusinessLayer;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Newtonsoft.Json;
using System.Web.Services;

namespace AstralFFMS
{
    public partial class DistributerOrderSummary : System.Web.UI.Page
    {
        string roleType = "";
        int cnt = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            txtfmDate.Attributes.Add("readonly", "readonly");
            txttodate.Attributes.Add("readonly", "readonly");
            if (!IsPostBack)
            {
                //List<DistributorsInvoice> distributorsinvoice = new List<DistributorsInvoice>();
                //distributorsinvoice.Add(new DistributorsInvoice());
                //distreportrpt.DataSource = distributorsinvoice;
                //distreportrpt.DataBind();
                roleType = Settings.Instance.RoleType;
                this.BindDistributorDDl("");
                txtfmDate.Text = Settings.GetUTCTime().AddMonths(-1).ToString("dd/MMM/yyyy");// System.DateTime.Now.ToShortDateString();
                txttodate.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");// System.DateTime.Now.ToShortDateString();
                //End
                btnExport.Visible = true;
                string pageName = Path.GetFileName(Request.Path);
                btnExport.Enabled = Settings.Instance.CheckExportPermission(pageName, Convert.ToString(Session["user_name"]));
                btnExport.CssClass = "btn btn-primary";
            }
        }

        public class DistributorsInvoice
        {
            public string VDate { get; set; }
            public string PartyId { get; set; }
            public string PartyName { get; set; }
            public string BranchName { get; set; }
            public string Amount { get; set; }            
        }

        [WebMethod(EnableSession = true)]
        public static string GetDistributorInvice(string Distid, string Fromdate, string Todate)
        {
            string Qrychk = " tdv1.VDate>='" + Settings.dateformat(Fromdate) + " 00:00' and tdv1.VDate<='" + Settings.dateformat(Todate) + " 23:59'";
            string query = @"Select distinct po.VDate, po.DistId,po.ItemId,po.Qty,po.Rate,po.Qty * po.Rate as Amount,mp.PartyName,mi.ItemName from TransPurchOrder1 as po inner join MastParty as mp on po.DistId=mp.PartyId inner join MastItem as  mi on po.ItemId=mi.ItemId Where po.DistId IN(" + Distid + ")   ORDER BY po.VDate desc ";
            DataTable dtInvoice = DbConnectionDAL.GetDataTable(CommandType.Text, query);
            return JsonConvert.SerializeObject(dtInvoice);

        }

        private void BindDistributorDDl(string SMIDStr)
        {
            try
            {
                string distqry = @"select PartyId,PartyName from MastParty where smid in ((select maingrp from mastsalesrepgrp union SELECT smid FROM mastsalesrepgrp ))  and PartyDist=1 and Active=1 order by PartyName";                                 
                DataTable dtDist = DbConnectionDAL.GetDataTable(CommandType.Text, distqry);
                if (dtDist.Rows.Count > 0)
                {
                    ListBox1.DataSource = dtDist;
                    ListBox1.DataTextField = "PartyName";
                    ListBox1.DataValueField = "PartyId";
                    ListBox1.DataBind();
                }

                else
                {

                    ListBox1.Items.Clear();
                    ListBox1.DataBind();                    
                    distreportrpt.DataSource = null;
                    distreportrpt.DataBind();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            try
            {
                string smIDStr1 = "", Qrychk = "", Query = "";               
                foreach (ListItem item in ListBox1.Items)
                {
                    if (item.Selected)
                    {
                        smIDStr1 += item.Value + ",";
                    }
                }
                smIDStr1 = smIDStr1.TrimStart(',').TrimEnd(',');

                Qrychk = " po.VDate>='" + Settings.dateformat(txtfmDate.Text) + " 00:00' and po.VDate<='" + Settings.dateformat(txttodate.Text) + " 23:59'";
                if (smIDStr1 != "")
                {
                    if (txtfmDate.Text != string.Empty && txttodate.Text != string.Empty)
                    {
                        if (Convert.ToDateTime(txtfmDate.Text) <= Convert.ToDateTime(txttodate.Text))
                        {

                            string query = @"Select distinct po.VDate, po.DistId,po.ItemId,po.Qty,po.Rate,po.Qty * po.Rate as Amount,mp.PartyName,mi.ItemName from TransPurchOrder1 as po inner join MastParty as mp on po.DistId=mp.PartyId inner join MastItem as  mi on po.ItemId=mi.ItemId Where po.DistId IN(" + smIDStr1 + ") And "+Qrychk+"  ORDER BY po.VDate desc ";
                            DataTable dtDistInvRep = DbConnectionDAL.GetDataTable(CommandType.Text, query);
                            if (dtDistInvRep.Rows.Count > 0)
                            {
                                distreportrpt.DataSource = dtDistInvRep;
                                distreportrpt.DataBind();
                                btnExport.Visible = true;
                            }
                            else
                            {
                                distreportrpt.DataSource = dtDistInvRep;
                                distreportrpt.DataBind();
                                btnExport.Visible = false;
                            }
                        }
                        else
                        {
                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('To date cannot be less than From Date.');", true);
                        }
                    }
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select distributor');", true);
                    distreportrpt.DataSource = null;
                    distreportrpt.DataBind();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/DistributerOrderSummary.aspx");
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            var item = (RepeaterItem)btn.NamingContainer;
            HiddenField hdnVisItCode = (HiddenField)item.FindControl("HiddenField1");
            GetDetails(hdnVisItCode.Value);
        }

        private void GetDetails(string p)
        {
            try
            {
                string detQry = @"select t1.DistInvDocId as Invoice_no,t1.ItemId as Item_Id,i.ItemName as Item,t1.Qty as Qty,t1.Amount as Amount
                                from TransDistInv1 t1 left join MastItem i on i.ItemId=t1.ItemId where DistInvDocId='" + p + "'";
                DataTable dtdetQry = DbConnectionDAL.GetDataTable(CommandType.Text, detQry);
                if(dtdetQry.Rows.Count>0)
                {
                    detailDiv.Style.Add("display", "block");
                    detailDistrpt.DataSource = dtdetQry;
                    detailDistrpt.DataBind();
                }
                else
                {
                    detailDiv.Style.Add("display", "none");
                    detailDistrpt.DataSource = dtdetQry;
                    detailDistrpt.DataBind();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

 
        protected void btnExport_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("content-disposition", "attachment;filename=DistributorInvoiceReport.csv");
            string headertext = "Distributer".TrimStart('"').TrimEnd('"') + "," + "Item".TrimStart('"').TrimEnd('"') + "," + "Qty".TrimStart('"').TrimEnd('"') + "," + "Rate".TrimStart('"').TrimEnd('"') + "," + "Amount".TrimStart('"').TrimEnd('"');

            StringBuilder sb = new StringBuilder();
            sb.Append(headertext);
            sb.AppendLine(System.Environment.NewLine);
           
            string dataText = string.Empty;


            //
            DataTable dtParams = new DataTable();
            //dtParams.Columns.Add(new DataColumn("PartyId", typeof(String)));
            //dtParams.Columns.Add(new DataColumn("UnderId", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Distributer", typeof(String)));

            dtParams.Columns.Add(new DataColumn("Item", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Qty", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Rate", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Amount", typeof(String)));
            foreach (RepeaterItem item in distreportrpt.Items)
            {
                DataRow dr = dtParams.NewRow();
                Label Distributer = item.FindControl("PartyName") as Label;
                dr["Distributer"] = Distributer.Text;
                Label Item = item.FindControl("ItemName") as Label;
                dr["Item"] = Item.Text.ToString();
                Label Rate = item.FindControl("Rate") as Label;
                dr["Rate"] = Rate.Text.ToString();
                Label Qty = item.FindControl("Qty") as Label;
                dr["Qty"] = Qty.Text.ToString();
                Label Amount = item.FindControl("Amount") as Label;
                dr["Amount"] = Amount.Text.ToString();

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
            //Response.Clear();
            //Response.ContentType = "text/csv";
            //Response.AddHeader("content-disposition", "attachment;filename=NotVisitedOutletReport.csv");
            Response.Write(sb.ToString());
            Response.End();
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
        }
        
        protected void ExportCSV(object sender, EventArgs e)
        {
            string smIDStr1 = "", Qrychk = "", Query = "";
            DataTable dt = new DataTable();
            foreach (ListItem item in ListBox1.Items)
            {
                if (item.Selected)
                {
                    smIDStr1 += item.Value + ",";
                }
            }
            smIDStr1 = smIDStr1.TrimStart(',').TrimEnd(',');

            Qrychk = " tdv.VDate>='" + Settings.dateformat(txtfmDate.Text) + " 00:00' and tdv.VDate<='" + Settings.dateformat(txttodate.Text) + " 23:59'";
            if (smIDStr1 != "")
            {
                if (txtfmDate.Text != string.Empty && txttodate.Text != string.Empty)
                {
                    if (Convert.ToDateTime(txtfmDate.Text) <= Convert.ToDateTime(txttodate.Text))
                    {

                        //Query = "select t.DistInvDocId as Invoice_No,convert (varchar,t.VDate,106) as Invoice_Date ,t.BillAmount as Invoice_Amt, gr.Bilty_No as GrNo,convert(varchar,gr.Bilty_Date,103) as GrDate,gr.Transpoter_Name as GrTransporterName from TransDistInv t Left Join TransGRData gr on gr.DistId=t.DistId and t.DistInvDocId=gr.Invoice_No where " + Qrychk + " and t.DistId in (" + smIDStr1 + ") ";
                        Query = @"select tdv.DistId,tdv.DistInvDocId,mp.SyncId AS PartyId,mp.PartyName,Convert(varchar(15),CONVERT(date,tdv.VDate,103),106) AS VDate,tdv1.Amount,mrc.ResCenName as BranchName from TransDistInv tdv inner join TransDistInv1 tdv1 on tdv.DistInvDocId=tdv1.DistInvDocId LEFT JOIN mastparty mp ON mp.PartyId=tdv.DistId left join MastResCentre mrc on mrc.ResCenId=tdv1.LocationID where " + Qrychk + " AND mp.PartyDist=1 and tdv.DistId in (" + smIDStr1 + ") ORDER BY tdv.VDate desc ";

                       
                        dt = DbConnectionDAL.GetDataTable(CommandType.Text, Query);


                    }

                }
            }

            {


                //Build the CSV file data as a Comma separated string.
                string csv = string.Empty;

                foreach (DataColumn column in dt.Columns)
                {
                    //Add the Header row for CSV file.
                    csv += column.ColumnName + ',';
                }

                //Add new line.
                csv += "\r\n";

                foreach (DataRow row in dt.Rows)
                {
                    foreach (DataColumn column in dt.Columns)
                    {
                        //Add the Data rows.
                        csv += row[column.ColumnName].ToString().Replace(",", ";") + ',';
                    }

                    //Add new line.
                    csv += "\r\n";
                }

                //Download the CSV file.
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=DistInvoiceReport.csv");
                string headertext = "Distributor Name".TrimStart('"').TrimEnd('"') + "," + "Product Class".TrimStart('"').TrimEnd('"') + "," + "Product Group".TrimStart('"').TrimEnd('"') + "," + "Year".TrimStart('"').TrimEnd('"') + "," + "Jan".TrimStart('"').TrimEnd('"') + "," + "Feb".TrimStart('"').TrimEnd('"') + "," + "Mar".TrimStart('"').TrimEnd('"') + "," + "Apr".TrimStart('"').TrimEnd('"') + "," + "May".TrimStart('"').TrimEnd('"') + "," + "Jun".TrimStart('"').TrimEnd('"') + "," + "Jul".TrimStart('"').TrimEnd('"') + "," + "Aug".TrimStart('"').TrimEnd('"') + "," + "Sep".TrimStart('"').TrimEnd('"') + "," + "Oct".TrimStart('"').TrimEnd('"') + "," + "Nov".TrimStart('"').TrimEnd('"') + "," + "Dec".TrimStart('"').TrimEnd('"') + "," + "Grand Total".TrimStart('"').TrimEnd('"');
                StringBuilder sb = new StringBuilder();
                sb.Append(headertext);
                sb.AppendLine(System.Environment.NewLine);
                Response.Charset = "";
                Response.ContentType = "application/text";
                Response.Output.Write(csv);
                Response.Flush();
                Response.End();
            }
        }
    }
}