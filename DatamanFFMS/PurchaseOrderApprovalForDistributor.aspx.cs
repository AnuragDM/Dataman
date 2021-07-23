using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using DAL;

namespace AstralFFMS
{
    public partial class PurchaseOrderApprovalForDistributor : System.Web.UI.Page
    {
        BAL.PurchaseOrder.PurchaseOrder dp = new BAL.PurchaseOrder.PurchaseOrder();
        string parameter = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            parameter = Request["__EVENTARGUMENT"];
            if (parameter != "" && parameter != null)
            {
                Session["POID"] = Convert.ToInt32(parameter);
                Response.Redirect("~/ConfirmPurchaseOrderForDistributor.aspx");
            }
            if (!Page.IsPostBack)
            {
                ddlStatus.SelectedValue = "P";
                fillRepeter(); 
                txtmDate.Attributes.Add("ReadOnly", "true");
                txttodate.Attributes.Add("ReadOnly", "true");
            }

        
        }


        protected void btnGo_Click(object sender, EventArgs e)
        {
            string str = "";
            string StatusVal = "0";
            txtmDate.Attributes.Add("ReadOnly", "true");
            txttodate.Attributes.Add("ReadOnly", "true");


            if (ddlStatus.SelectedItem.Value != "0")
            {
                StatusVal = ddlStatus.SelectedItem.Value;
            }
            if (txtmDate.Text != "" && txttodate.Text != "")
            {
                str = @"SELECT T1.POrdId,T1.VDate,T1.PODocId,T2.PartyName,T3.AreaName,
                                CASE WHEN T1.OrderStatus='H' THEN 'On Hold'
                                 WHEN T1.OrderStatus='R' THEN 'CustomerCanceled'
                                 WHEN T1.OrderStatus='P' THEN 'Open'
                                 WHEN T1.OrderStatus='M' THEN 'Processed'
                                 WHEN T1.OrderStatus='C' THEN 'CompanyCanceled' END AS OrderStatus
                            from TransPurchOrder T1 Left join MastParty T2 
                            on T1.DistId=T2.PartyId  
                            LEFT JOIN MastArea AS T3
                            ON T2.CityId = T3.AreaId   where T2.Active=1 and T1.DistId = " + Convert.ToInt32(Settings.Instance.DistributorID) +
                            " and Convert(VARCHAR(10),T1.VDate,101)>='" + Settings.dateformat1(txtmDate.Text) + "' and Convert(VARCHAR(10),T1.VDate,101)<='" + Settings.dateformat1(txttodate.Text) + "'" +
                            @" ORDER BY T1.PODocId ASC,T1.VDate desc";
                //str = @"select * from TransVisit where  SMId=" + ddlUndeUser.SelectedValue + " and VDate>='" + Settings.dateformat1(txtmDate.Text) + "' and VDate<='" + Settings.dateformat1(txttodate.Text) + "' order by VDate desc";
            }
            else if (txtmDate.Text != "")
            {
                //str = @"select * from TransVisit where  SMId=" + ddlUndeUser.SelectedValue + " and VDate>='" + Settings.dateformat1(txtmDate.Text) + "' order by VDate desc";

                str = @"SELECT T1.POrdId,T1.VDate,T1.PODocId,T2.PartyName,T3.AreaName,
                                CASE WHEN T1.OrderStatus='H' THEN 'On Hold'
                                 WHEN T1.OrderStatus='R' THEN 'CustomerCanceled'
                                 WHEN T1.OrderStatus='P' THEN 'Open'
                                 WHEN T1.OrderStatus='M' THEN 'Processed'
                                 WHEN T1.OrderStatus='C' THEN 'CompanyCanceled' END AS OrderStatus
                            from TransPurchOrder T1 Left join MastParty T2 
                            on T1.DistId=T2.PartyId  
                            LEFT JOIN MastArea AS T3
                            ON T2.CityId = T3.AreaId where T2.Active=1 
                            and T1.DistId = " + Convert.ToInt32(Settings.Instance.DistributorID) +
                            " and Convert(VARCHAR(10),T1.VDate,101)>='" + Settings.dateformat1(txtmDate.Text) + "' ORDER BY T1.PODocId ASC,T1.VDate desc";
            }
            else
            {
                if (StatusVal == "A")
                {
                    StatusVal = "0";
                    str = @"SELECT T1.POrdId,T1.VDate,T1.PODocId,T2.PartyName,T3.AreaName,
                                CASE WHEN T1.OrderStatus='H' THEN 'On Hold'
                                 WHEN T1.OrderStatus='R' THEN 'CustomerCanceled'
                                 WHEN T1.OrderStatus='P' THEN 'Open'
                                 WHEN T1.OrderStatus='M' THEN 'Processed'
                                 WHEN T1.OrderStatus='C' THEN 'CompanyCanceled' END AS OrderStatus
                            from TransPurchOrder T1 Left join MastParty T2 
                            on T1.DistId=T2.PartyId  
                            LEFT JOIN MastArea AS T3
                            ON T2.CityId = T3.AreaId   where T2.Active=1   and T1.DistId = " + Convert.ToInt32(Settings.Instance.DistributorID) +
                                @" and (T1.OrderStatus='" + StatusVal + "' OR " + StatusVal + "='0') ORDER BY T1.PODocId ASC,T1.VDate desc";
                }
                else
                {
                    str = @"SELECT T1.POrdId,T1.VDate,T1.PODocId,T2.PartyName,T3.AreaName,
                                CASE WHEN T1.OrderStatus='H' THEN 'On Hold'
                                 WHEN T1.OrderStatus='R' THEN 'CustomerCanceled'
                                 WHEN T1.OrderStatus='P' THEN 'Open'
                                 WHEN T1.OrderStatus='M' THEN 'Processed'
                                 WHEN T1.OrderStatus='C' THEN 'CompanyCanceled' END AS OrderStatus
                            from TransPurchOrder T1 Left join MastParty T2 
                            on T1.DistId=T2.PartyId  
                            LEFT JOIN MastArea AS T3
                            ON T2.CityId = T3.AreaId where T2.Active=1 and T1.DistId = " + Convert.ToInt32(Settings.Instance.DistributorID) +
                                @" and (T1.OrderStatus='" + StatusVal + "') ORDER BY T1.PODocId ASC,T1.VDate desc";
                }
            }

            DataTable depdt = DbConnectionDAL.GetDataTable(CommandType.Text, str);

            if (depdt != null && depdt.Rows.Count > 0)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("POrdId");
                dt.Columns.Add("VDate");
                dt.Columns.Add("PODocId");
                dt.Columns.Add("IGandDispatchTo");
                dt.Columns.Add("OrderStatus");

                for (int i = 0; i < depdt.Rows.Count; i++)
                {
                    string strUID = @"SELECT T2.Underid FROM TransPurchOrder1 AS T1 LEFT JOIN MastItem AS T2 ON T1.ItemId=T2.ItemId WHERE T1.POrdId=" + Convert.ToInt32(depdt.Rows[i]["POrdId"]) + "";
                    DataTable dtUID = DbConnectionDAL.GetDataTable(CommandType.Text, strUID);
                    if (dtUID != null && dtUID.Rows.Count > 0)
                    {
                        string strIG = @"SELECT T1.ItemName FROM MastItem AS T1 WHERE T1.ItemId=" + Convert.ToInt32(dtUID.Rows[0]["Underid"]) + "";
                        DataTable dtIG = DbConnectionDAL.GetDataTable(CommandType.Text, strIG);

                        if (dtIG != null && dtIG.Rows.Count > 0)
                        {
                            dt.Rows.Add();
                            dt.Rows[dt.Rows.Count - 1]["POrdId"] = depdt.Rows[i]["POrdId"];
                            dt.Rows[dt.Rows.Count - 1]["VDate"] = depdt.Rows[i]["VDate"];
                            dt.Rows[dt.Rows.Count - 1]["PODocId"] = depdt.Rows[i]["PODocId"];
                            dt.Rows[dt.Rows.Count - 1]["IGandDispatchTo"] = dtIG.Rows[0]["ItemName"].ToString() + "-(" + depdt.Rows[i]["PartyName"] + " " + depdt.Rows[i]["AreaName"] + ")";
                            dt.Rows[dt.Rows.Count - 1]["OrderStatus"] = depdt.Rows[i]["OrderStatus"];
                        }
                    }
                }

                rpt.DataSource = dt;
                rpt.DataBind();
            }
            else
            {
                rpt.DataSource = null;
                rpt.DataBind();
            }
        }

        private void fillRepeter()
        {
            string str = @"SELECT T1.POrdId,T1.VDate,T1.PODocId,T2.PartyName,T3.AreaName,
                                CASE WHEN T1.OrderStatus='H' THEN 'On Hold'
                                 WHEN T1.OrderStatus='R' THEN 'CustomerCanceled'
                                 WHEN T1.OrderStatus='P' THEN 'Open'
                                 WHEN T1.OrderStatus='M' THEN 'Processed'
                                 WHEN T1.OrderStatus='C' THEN 'CompanyCanceled' END AS OrderStatus
                            from TransPurchOrder T1 Left join MastParty T2 
                            on T1.DistId=T2.PartyId  
                            LEFT JOIN MastArea AS T3
                            ON T2.CityId = T3.AreaId   where T2.Active=1   and T1.DistId = " + Convert.ToInt32(Settings.Instance.DistributorID) +
                                @" and (T1.OrderStatus='P') ORDER BY T1.PODocId ASC,T1.VDate desc";
            DataTable depdt = DbConnectionDAL.GetDataTable(CommandType.Text, str);

            if (depdt != null && depdt.Rows.Count > 0)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("POrdId");
                dt.Columns.Add("VDate");
                dt.Columns.Add("PODocId");
                dt.Columns.Add("IGandDispatchTo");
                dt.Columns.Add("OrderStatus");

                for (int i = 0; i < depdt.Rows.Count; i++)
                {
                    string strUID = @"SELECT T2.Underid FROM TransPurchOrder1 AS T1 LEFT JOIN MastItem AS T2 ON T1.ItemId=T2.ItemId WHERE T1.POrdId=" + Convert.ToInt32(depdt.Rows[i]["POrdId"]) + "";
                    DataTable dtUID = DbConnectionDAL.GetDataTable(CommandType.Text, strUID);
                    if (dtUID != null && dtUID.Rows.Count > 0)
                    {
                        string strIG = @"SELECT T1.ItemName FROM MastItem AS T1 WHERE T1.ItemId=" + Convert.ToInt32(dtUID.Rows[0]["Underid"]) + "";
                        DataTable dtIG = DbConnectionDAL.GetDataTable(CommandType.Text, strIG);

                        if (dtIG != null && dtIG.Rows.Count > 0)
                        {
                            dt.Rows.Add();
                            dt.Rows[dt.Rows.Count - 1]["POrdId"] = depdt.Rows[i]["POrdId"];
                            dt.Rows[dt.Rows.Count - 1]["VDate"] = depdt.Rows[i]["VDate"];
                            dt.Rows[dt.Rows.Count - 1]["PODocId"] = depdt.Rows[i]["PODocId"];
                            dt.Rows[dt.Rows.Count - 1]["IGandDispatchTo"] = dtIG.Rows[0]["ItemName"].ToString() + "-(" + depdt.Rows[i]["PartyName"] + " " + depdt.Rows[i]["AreaName"] + ")";
                            dt.Rows[dt.Rows.Count - 1]["OrderStatus"] = depdt.Rows[i]["OrderStatus"];
                        }
                    }
                }

                rpt.DataSource = dt;
                rpt.DataBind();
            }
            else
            {
                rpt.DataSource = null;
                rpt.DataBind();
            }
        }


        protected void rpt_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            //mainDiv.Style.Add("display", "block");
            rptmain.Style.Add("display", "none");
        }

    }
    

}