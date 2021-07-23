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
using System.Text.RegularExpressions;
using System.Text;
using System.Web.UI.HtmlControls;


namespace AstralFFMS
{
    public partial class PurchaseOrderReportPrint : System.Web.UI.Page
    {
        BAL.PurchaseOrder.PurchaseOrder dp = new BAL.PurchaseOrder.PurchaseOrder();
        string parameter = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                FillData(Convert.ToInt32(Session["POIDPrint"]));
            }
        }

        private void FillDetails(int DistID)
        {
            string str1 = @"SELECT T1.* FROM MastParty AS T1 WHERE T1.PartyId=" + DistID + "";
            DataTable obj1 = new DataTable();
            obj1 = DbConnectionDAL.GetDataTable(CommandType.Text, str1);

            lblBillTo.Text = obj1.Rows[0]["PartyName"].ToString();
            lblDistCode.Text = obj1.Rows[0]["SyncId"].ToString();
            DataTable dt = new DataTable();
            string Query = "SELECT * FROM MastArea WHERE areaid IN(SELECT Maingrp FROM MastAreaGrp WHERE AreaId=" + Convert.ToInt32(obj1.Rows[0]["AreaId"]) + ")";
            dt = DbConnectionDAL.GetDataTable(CommandType.Text, Query);

            DataTable dtFinal = new DataTable();
            dtFinal.Columns.Add("Address1");
            dtFinal.Columns.Add("Address2");
            dtFinal.Columns.Add("CITY");
            dtFinal.Columns.Add("STATE");
            dtFinal.Columns.Add("COUNTRY");
            dtFinal.Columns.Add("Pin");
            dtFinal.Columns.Add("Mobile");
            dtFinal.Columns.Add("Email");
            dtFinal.Columns.Add("CreditLimit");
            dtFinal.Columns.Add("Outstanding");
            dtFinal.Columns.Add("OpenOrder");

            dtFinal.Rows.Add();

            dtFinal.Rows[0]["Address1"] = obj1.Rows[0]["Address1"];
            dtFinal.Rows[0]["Address2"] = obj1.Rows[0]["Address2"];

            dtFinal.Rows[0]["CreditLimit"] = obj1.Rows[0]["CreditLimit"];
            dtFinal.Rows[0]["Outstanding"] = obj1.Rows[0]["OutStanding"];
            dtFinal.Rows[0]["OpenOrder"] = obj1.Rows[0]["OpenOrder"];

            DataRow[] drow = dt.Select("AreaType='CITY'");
            if (drow != null && drow.Length > 0)
            {
                dtFinal.Rows[0]["CITY"] = drow[0]["AreaName"];
            }

            DataRow[] drow1 = dt.Select("AreaType='STATE'");

            if (drow != null && drow.Length > 0)
            {
                dtFinal.Rows[0]["STATE"] = drow1[0]["AreaName"];
            }

            DataRow[] drow2 = dt.Select("AreaType='COUNTRY'");

            if (drow != null && drow.Length > 0)
            {
                dtFinal.Rows[0]["COUNTRY"] = drow2[0]["AreaName"];
            }

            dtFinal.Rows[0]["Pin"] = obj1.Rows[0]["Pin"];
            dtFinal.Rows[0]["Mobile"] = obj1.Rows[0]["Mobile"];
            dtFinal.Rows[0]["Email"] = obj1.Rows[0]["Email"];

            if (dtFinal != null && dtFinal.Rows.Count > 0)
            {
                lblBillToAddress.Text = dtFinal.Rows[0]["Address1"].ToString();

                if (dtFinal.Rows[0]["Address2"].ToString() != "")
                {
                    lblBillToAddress.Text = dtFinal.Rows[0]["Address1"].ToString() + "," + dtFinal.Rows[0]["Address2"].ToString();
                }
                if (dtFinal.Rows[0]["CITY"].ToString() != "")
                {
                    lblBillToAddress.Text = lblBillToAddress.Text + ", " + dtFinal.Rows[0]["CITY"].ToString();
                }
                if (dtFinal.Rows[0]["Pin"].ToString() != "")
                {
                    lblBillToAddress.Text = lblBillToAddress.Text + "-" + dtFinal.Rows[0]["Pin"].ToString();
                }
                if (dtFinal.Rows[0]["Mobile"].ToString() != "")
                {
                    lblBillToAddress.Text = lblBillToAddress.Text + ", " + dtFinal.Rows[0]["Mobile"].ToString();
                }
                if (dtFinal.Rows[0]["Email"].ToString() != "")
                {
                    lblBillToAddress.Text = lblBillToAddress.Text + ", " + dtFinal.Rows[0]["Email"].ToString();
                }

            }
        }
        private void FillData(int POID)
        {
            string str = @"select * from TransPurchOrder T1 INNER JOIN MastParty T2 on T1.DistId=T2.PartyId where T1.POrdId=" + POID + "";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);

            string strTP = @"select * from TransPurchOrder AS T1 WHERE T1.POrdId=" + POID + "";
            DataTable dtTP = DbConnectionDAL.GetDataTable(CommandType.Text, strTP);

            if (dt != null && dt.Rows.Count > 0)
            {
                //lblStatus.Text = "";
                lblOrderDate.Text = Convert.ToDateTime(dt.Rows[0]["CreatedDate"]).ToString("dd/MMM/yyyy"); 
                FillDetails(Convert.ToInt32(dt.Rows[0]["DistId"]));
                if (dtTP != null && dtTP.Rows.Count > 0)
                {
                    lblDispatchTo.Text = Convert.ToString(dtTP.Rows[0]["DispName"]);
                    lblDispatchToAddress.Text = Convert.ToString(dtTP.Rows[0]["DispAdd1"]);

                    if (Convert.ToString(dtTP.Rows[0]["DispAdd2"]) != "")
                    {
                        lblDispatchToAddress.Text = dtTP.Rows[0]["DispAdd1"].ToString() + ", " + dtTP.Rows[0]["DispAdd2"].ToString();
                    }

                    string strCity = @"select AreaName from MastArea AS T1 WHERE T1.AreaId=" + Convert.ToInt32(dtTP.Rows[0]["DispCity"]) + "";
                    DataTable dtCity = DbConnectionDAL.GetDataTable(CommandType.Text, strCity);

                    if (dtCity != null && dtCity.Rows.Count > 0)
                    {
                        lblDispatchToAddress.Text = lblDispatchToAddress.Text + ", " + dtCity.Rows[0]["AreaName"].ToString();
                    }
                    if (dtTP.Rows[0]["DispPin"].ToString() != "")
                    {
                        lblDispatchToAddress.Text = lblDispatchToAddress.Text + "-" + dtTP.Rows[0]["DispPin"].ToString();
                    }
                    if (dtTP.Rows[0]["DispPhone"].ToString() != "")
                    {
                        lblDispatchToAddress.Text = lblDispatchToAddress.Text + ", " + dtTP.Rows[0]["DispPhone"].ToString();
                    }
                    if (dtTP.Rows[0]["DispMobile"].ToString() != "")
                    {
                        lblDispatchToAddress.Text = lblDispatchToAddress.Text + ", " + dtTP.Rows[0]["DispMobile"].ToString();
                    }
                    if (dtTP.Rows[0]["DispEmail"].ToString() != "")
                    {
                        lblDispatchToAddress.Text = lblDispatchToAddress.Text + ", " + dtTP.Rows[0]["DispEmail"].ToString();
                    }

                    lblOrderNo.Text = Convert.ToString(dtTP.Rows[0]["PODocId"]);

                    if (Convert.ToString(dtTP.Rows[0]["Transporter"]) != "--Select--")
                    {
                        lblTransporter.Text = Convert.ToString(dtTP.Rows[0]["Transporter"]);
                    }
                 

                }           

                string strUID = @"SELECT T2.Underid FROM TransPurchOrder1 AS T1 LEFT JOIN MastItem AS T2 ON T1.ItemId=T2.ItemId WHERE T1.POrdId=" +POID + "";
                DataTable dtUID = DbConnectionDAL.GetDataTable(CommandType.Text, strUID);
                if (dtUID != null && dtUID.Rows.Count > 0)
                {
                    string strIG = @"SELECT T1.ItemName FROM MastItem AS T1 WHERE T1.ItemId=" + Convert.ToInt32(dtUID.Rows[0]["Underid"]) + "";
                    DataTable dtIG = DbConnectionDAL.GetDataTable(CommandType.Text, strIG);

                    if (dtIG != null && dtIG.Rows.Count > 0)
                    {
                        lblItemGroup.Text = dtIG.Rows[0]["ItemName"].ToString();
                    }
                }

                lblCreated.Text = (Convert.ToDateTime(dt.Rows[0]["CreatedDate"])).ToString("dd/MMM/yyyy"); Convert.ToString(Convert.ToDateTime(dt.Rows[0]["CreatedDate"]).ToShortDateString());
                //lblModified.Text = (Convert.ToDateTime(dt.Rows[0]["VDate"])).ToString("dd/MMM/yyyy"); Convert.ToString(Convert.ToDateTime(dt.Rows[0]["VDate"]).ToShortDateString());
                lblRemarks.Text = Convert.ToString(dt.Rows[0]["Remarks"]);
                lblPrinted.Text = (Settings.GetUTCTime()).ToString("dd/MMM/yyyy");

                string str3 = @"select * from TransPurchOrder1 T1 
                            INNER JOIN MastItem T2
                            ON T1.ItemId=T2.ItemId where T1.POrdId=" + POID + "";

                DataTable dt1 = DbConnectionDAL.GetDataTable(CommandType.Text, str3);

                if (dt1 != null && dt1.Rows.Count > 0)
                {
                    DataTable dtItem = new DataTable();

                    dtItem.Columns.Add("ItemName");
                    dtItem.Columns.Add("Packing");
                    dtItem.Columns.Add("Qunatity");
                    dtItem.Columns.Add("PricePerUnit");
                    dtItem.Columns.Add("Discount");
                    dtItem.Columns.Add("Remarks");
                    dtItem.Columns.Add("Total");
                    decimal decAmtTotal = 0M;
                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        dtItem.Rows.Add();
                        dtItem.Rows[dtItem.Rows.Count - 1]["ItemName"] = "(" + dt1.Rows[i]["SyncId"].ToString() + ")" + " " + dt1.Rows[i]["ItemName"].ToString() + " " + "(" + dt1.Rows[i]["ItemCode"].ToString() + ")";
                        dtItem.Rows[dtItem.Rows.Count - 1]["Qunatity"] = dt1.Rows[i]["Qty"];
                        //dtItem.Rows[dtItem.Rows.Count - 1]["Unit"] = dt1.Rows[i]["Unit"];
                        //dtItem.Rows[dtItem.Rows.Count - 1]["Rate"] = dt1.Rows[i]["Rate"];
                        dtItem.Rows[dtItem.Rows.Count - 1]["PricePerUnit"] = dt1.Rows[i]["Rate"].ToString() + "/" + dt1.Rows[i]["Unit"].ToString();
                        dtItem.Rows[dtItem.Rows.Count - 1]["Total"] = Math.Round((Convert.ToDecimal(dt1.Rows[i]["Qty"]) * Convert.ToDecimal(dt1.Rows[i]["MRP"])), 2);
                        dtItem.Rows[dtItem.Rows.Count - 1]["Remarks"] = dt1.Rows[i]["Remarks"];
                        dtItem.Rows[dtItem.Rows.Count - 1]["Packing"] = dt1.Rows[i]["StdPack"];
                        dtItem.Rows[dtItem.Rows.Count - 1]["Discount"] = "0";
                        decAmtTotal += Convert.ToDecimal(dtItem.Rows[dtItem.Rows.Count - 1]["Total"]);
                    }

                    gvData.DataSource = dtItem;
                    gvData.DataBind();
                    lblTotal.Text = Convert.ToString(Math.Round(decAmtTotal, 2));
                }
            }
        }

        protected void btnPint_Click(object sender, EventArgs e)
        {
            btnPint.Visible = false;
            HttpContext.Current.Response.Write("<script>window.print();</script>");
          
            ////Session["ctrl"] = pnl1;            
            //ClientScript.RegisterStartupScript(this.GetType(), "onclick", "<script language=javascript>window.open('PurchaseOrderReportPrint.aspx','PrintMe','height=300px,width=300px,scrollbars=1');</script>");

            ////string str = "<script language=javascript>window.open('PurchaseOrderReportPrint.aspx','PrintMe','height=700px,width=600px,scrollbars=1');</script>";
            //PrintWebControl(pnl1, "");
            
        }


        public static void PrintWebControl(Control ctrl)
        {
            PrintWebControl(ctrl, string.Empty);
        }

        public static void PrintWebControl(Control ctrl, string Script)
        {
            StringWriter stringWrite = new StringWriter();
            System.Web.UI.HtmlTextWriter htmlWrite = new System.Web.UI.HtmlTextWriter(stringWrite);
            if (ctrl is WebControl)
            {
                Unit w = new Unit(100, UnitType.Percentage); ((WebControl)ctrl).Width = w;
            }
            Page pg = new Page();
            pg.EnableEventValidation = false;
            if (Script != string.Empty)
            {
                pg.ClientScript.RegisterStartupScript(pg.GetType(), "PrintJavaScript", Script);
            }
            HtmlForm frm = new HtmlForm();
            pg.Controls.Add(frm);
            frm.Attributes.Add("runat", "server");
            frm.Controls.Add(ctrl);
            pg.DesignerInitialize();
            pg.RenderControl(htmlWrite);
            string strHTML = stringWrite.ToString();
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Write(strHTML);
            HttpContext.Current.Response.Write("<script>window.print();</script>");
            HttpContext.Current.Response.End();
        }
    }
}