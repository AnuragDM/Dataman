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
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Web.UI.HtmlControls;

namespace FFMS
{
    public partial class ConfirmPurchaseOrder : System.Web.UI.Page
    {
        BAL.PurchaseOrder.PurchaseOrder dp = new BAL.PurchaseOrder.PurchaseOrder();
        string parameter = "";

        protected void Page_Load(object sender, EventArgs e)
        { 
            if (!Page.IsPostBack)
            {
                Session["ItemDataPO"] = null;
                LoadDetails();               
               
            }
        }

        private void LoadDetails()
        {
            try
            {
                string strTP = @"select * from TransPurchOrder AS T1 WHERE T1.POrdId=" + Convert.ToInt32(Session["POID"]) + "";
                DataTable dtTP = DbConnectionDAL.GetDataTable(CommandType.Text, strTP);

                if(dtTP.Rows[0]["OrderStatus"].ToString()!="P")
                {
                    btnOnHold.Attributes.Add("disabled", "disabled");
                    btnConfirm.Attributes.Add("disabled", "disabled");
                    Cancel.Attributes.Add("disabled", "disabled");
                }
                else
                {
                    btnOnHold.Enabled = true;
                    btnConfirm.Enabled = true;
                    Cancel.Enabled = true;
                }

                //string str2 = @"SELECT T1.Id,T1.Name FROM MastTransporter AS T1 WHERE T1.Id="+Convert.ToInt32(dtTP.Rows[0]["Transporter"])+"";
                //DataTable dt2 = DbConnectionDAL.GetDataTable(CommandType.Text, str2);

                //if(dt2!=null && dt2.Rows.Count>0)
                //{
                lblTransporter.Text = Convert.ToString(dtTP.Rows[0]["Transporter"]);
                //}

                string str1 = @"SELECT T1.* FROM MastParty AS T1 WHERE T1.PartyId=" + Convert.ToInt32(dtTP.Rows[0]["DistId"]) + "";
                DataTable obj1 = new DataTable();
                obj1 = DbConnectionDAL.GetDataTable(CommandType.Text, str1);               

                if(obj1.Rows[0]["Remark"]!=null)
                {
                    lblRemarks.Text = Convert.ToString(obj1.Rows[0]["Remark"]);
                }
                lblBillTo.Text = Convert.ToString(obj1.Rows[0]["PartyName"]);

                if (obj1.Rows[0]["CreditLimit"] != null || Convert.ToDecimal(obj1.Rows[0]["CreditLimit"]) != 0M)
                {
                    lblCreditLimit.Text = Convert.ToString(obj1.Rows[0]["CreditLimit"]);
                }
                else
                {
                    lblCreditLimit.Text = "0.00";
                }
                if (obj1.Rows[0]["Outstanding"] != null || Convert.ToDecimal(obj1.Rows[0]["Outstanding"]) != 0M)
                {
                    if (Convert.ToDecimal(obj1.Rows[0]["Outstanding"]) < 0)
                    {
                        lblOutstanding.Text = Convert.ToString(Math.Abs(Convert.ToDecimal(obj1.Rows[0]["Outstanding"]))) + " Cr.";
                    }
                    if (Convert.ToDecimal(obj1.Rows[0]["Outstanding"]) > 0)
                    {
                        lblOutstanding.Text = Convert.ToString(obj1.Rows[0]["Outstanding"]) + " Dr.";
                    }
                    if (Convert.ToDecimal(obj1.Rows[0]["Outstanding"]) == 0M)
                    {
                        lblOutstanding.Text = "0.00";
                    }

                }
                else
                {
                    lblOutstanding.Text = "0.00";
                }
                if (obj1.Rows[0]["OpenOrder"] != null || Convert.ToDecimal(obj1.Rows[0]["OpenOrder"]) != 0M)
                {
                    lblOpenOrders.Text = Convert.ToString(obj1.Rows[0]["OpenOrder"]);
                }
                else
                {
                    lblOpenOrders.Text = "0.00";
                }

                DataTable dt = new DataTable();
                string Query = "SELECT DISTINCT T.countryid,T.countryName,T.stateid,T.stateName,T.cityid,T.cityName FROM ViewGeo AS T WHERE T.cityid=" + Convert.ToInt32(obj1.Rows[0]["CityId"]) + "";
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

                dtFinal.Rows[0]["CITY"] = dt.Rows[0]["cityName"];
                dtFinal.Rows[0]["STATE"] = dt.Rows[0]["stateName"];
                dtFinal.Rows[0]["COUNTRY"] = dt.Rows[0]["countryName"];

                dtFinal.Rows[0]["Pin"] = obj1.Rows[0]["Pin"];
                dtFinal.Rows[0]["Mobile"] = obj1.Rows[0]["Mobile"];
                dtFinal.Rows[0]["Email"] = obj1.Rows[0]["Email"];


                if (dtFinal != null && dtFinal.Rows.Count > 0)
                {
                    lblBillToAdd.Text = dtFinal.Rows[0]["Address1"].ToString();

                    if (dtFinal.Rows[0]["Address2"].ToString() != "")
                    {
                        lblBillToAdd.Text = dtFinal.Rows[0]["Address1"].ToString() + ", " + dtFinal.Rows[0]["Address2"].ToString();
                    }
                    if (dtFinal.Rows[0]["CITY"].ToString() != "")
                    {
                        lblBillToAdd.Text = lblBillToAdd.Text + ", <br/>" + dtFinal.Rows[0]["CITY"].ToString();
                    }
                    if (dtFinal.Rows[0]["Pin"].ToString() != "")
                    {
                        lblBillToAdd.Text = lblBillToAdd.Text + "-" + dtFinal.Rows[0]["Pin"].ToString();
                    }
                    if (dtFinal.Rows[0]["Mobile"].ToString() != "")
                    {
                        lblBillToAdd.Text = lblBillToAdd.Text + ", <br/>" + dtFinal.Rows[0]["Mobile"].ToString();
                    }
                    if (dtFinal.Rows[0]["Email"].ToString() != "")
                    {
                        lblBillToAdd.Text = lblBillToAdd.Text + ", <br/>" + dtFinal.Rows[0]["Email"].ToString();
                    }

                   if(dtTP!=null && dtTP.Rows.Count>0)
                   {
                       lblOrderNo.Text = Convert.ToString(dtTP.Rows[0]["PODocId"]);
                       lblOrderDate.Text = (Convert.ToDateTime(dtTP.Rows[0]["CreatedDate"])).ToString("dd/MMM/yyyy");
                       lblCreatedDate.Text = (Settings.GetUTCTime()).ToString("dd/MMM/yyyy");

                       lblDispatchTo.Text = Convert.ToString(dtTP.Rows[0]["DispName"]);
                       lblDispatchToAdd.Text = Convert.ToString(dtTP.Rows[0]["DispAdd1"]);

                       if (Convert.ToString(dtTP.Rows[0]["DispAdd2"]) != "")
                       {
                           lblDispatchToAdd.Text = dtTP.Rows[0]["DispAdd1"].ToString() + ", " + dtTP.Rows[0]["DispAdd2"].ToString();
                       }

                       string strCity = @"select AreaName from MastArea AS T1 WHERE T1.AreaId=" + Convert.ToInt32(dtTP.Rows[0]["DispCity"]) + "";
                       DataTable dtCity = DbConnectionDAL.GetDataTable(CommandType.Text, strCity);

                       if(dtCity!=null && dtCity.Rows.Count>0)
                       {
                           lblDispatchToAdd.Text = lblDispatchToAdd.Text + ", <br/>" + dtCity.Rows[0]["AreaName"].ToString();
                       }
                       if (dtTP.Rows[0]["DispPin"].ToString() != "")
                       {
                           lblDispatchToAdd.Text = lblDispatchToAdd.Text + "-" + dtTP.Rows[0]["DispPin"].ToString();
                       }
                       if (dtTP.Rows[0]["DispPhone"].ToString() != "")
                       {
                           lblDispatchToAdd.Text = lblDispatchToAdd.Text + ", <br/>" + dtTP.Rows[0]["DispPhone"].ToString();
                       }
                       if (dtTP.Rows[0]["DispMobile"].ToString() != "")
                       {
                           lblDispatchToAdd.Text = lblDispatchToAdd.Text + ", " + dtTP.Rows[0]["DispMobile"].ToString();
                       }
                       if (dtTP.Rows[0]["DispEmail"].ToString() != "")
                       {
                           lblDispatchToAdd.Text = lblDispatchToAdd.Text + ", <br/>" + dtTP.Rows[0]["DispEmail"].ToString();
                       }
                   }

                   LoadGridData();

                   if (Convert.ToString(dtTP.Rows[0]["OrderStatus"]) == "P")
                   {
                       LoadSplitedData();
                   }
                   else
                   {
                       LoadExistingData();
                   }
                  
                }

            }
            catch (Exception ex)
            {

            }
        }

        private void LoadExistingData()
        {  //Ankita - 12/may/2016- (For Optimization)
            //            string str = @"SELECT T1.PCOrd1Id,T1.POrd1Id AS PO1ID,T1.ItemId AS ItemID,
            //                                        T2.ItemName,T2.Unit,T1.Qty AS  ConfQty,T2.MRP AS Rate,
            //                                        T1.Disc AS Discount,CASE WHEN T1.Remarks='&nbsp;' THEN '' ELSE  T1.Remarks END AS Remarks,T2.PriceGroup AS GroupCode ,
            //                                        0 AS Excise,T1.Qty*T2.MRP AS Amount,T1.Location AS LocationID,
            //                                        T3.ResCenName AS Location
            //                                        FROM TransPurchOrder1Conf AS T1 
            //                                        INNER JOIN MastItem AS T2
            //                                        ON T1.ItemId=T2.ItemId
            //                                        INNER JOIN MastResCentre AS T3
            //                                        ON T1.Location=T3.ResCenId
            //                                        WHERE T1.POrdId=" + Convert.ToInt32(Session["POID"]) + " ORDER BY T1.PCOrd1Id";
            string str = "select * from [View_OrderForApproval] WHERE POrdId=" + Convert.ToInt32(Session["POID"]) + " ORDER BY PCOrd1Id";

            //            string str = @"select T1.POrdId AS ID,T2.ItemId AS ItemID,T2.ItemName,T2.StdPack AS CartonQty,T1.Qty AS OrdQty,T2.MRP AS Rate,
            //                                T1.Qty*T2.MRP AS Amount,T2.Unit,T1.Remarks,T2.PriceGroup,0.00 AS Excise,T2.SyncId,T2.ItemCode
            //                              from TransPurchOrder1 AS T1 LEFT JOIN MastItem AS T2 
            //                             ON T1.ItemId=T2.ItemId 
            //                             WHERE T1.POrdId=" + Convert.ToInt32(Session["POID"]) + "";
            DataTable objData = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            Session["ItemDataPO"] = objData;
            decimal decAmtTotal = 0M;
            foreach (GridViewRow grow in gvDetails.Rows)
            {
                try
                {
                    Label lblAmt1 = (Label)grow.FindControl("lblOriAmount");
                    Label lblItemID = (Label)grow.FindControl("lblItemID");
                    TextBox txtDiscount = (TextBox)grow.FindControl("txtDiscount");
                    DropDownList ddlLocation = (DropDownList)grow.FindControl("ddlLocation");


                    DataView dv = new DataView(objData);
                    //     dv.RowFilter = "RoleName='Level 1'";
                    dv.RowFilter = "ItemID=" + Convert.ToInt32(lblItemID.Text.Trim()) + "";
                    DataTable dtPOItem = dv.ToTable();
                    txtDiscount.Text = Convert.ToString(dtPOItem.Rows[0]["Discount"]);
                    ddlLocation.SelectedValue = Convert.ToString(dtPOItem.Rows[0]["LocationID"]);
                    lblAmt1.Text = Convert.ToString(Convert.ToDecimal(lblAmt1.Text) - (Convert.ToDecimal(lblAmt1.Text) * Convert.ToDecimal(dtPOItem.Rows[0]["Discount"])) / 100);

                    ddlLocation.Enabled = false;

                    if (lblAmt1.Text != "")
                    {
                        decAmtTotal += Convert.ToDecimal(lblAmt1.Text);
                    }

                    if (Session["ItemDataPO"] != null)
                    {
                        DataTable dtLoc1 = (DataTable)Session["ItemDataPO"];

                        foreach (DataRow dr in dtLoc1.Rows) // search whole table
                        {
                            if (dr["ItemID"].ToString() == lblItemID.Text.Trim())
                            {
                                dr["Discount"] = txtDiscount.Text.Trim();
                                dr["Amount"] = lblAmt1.Text.Trim();
                            }
                        }

                        dtLoc1.AcceptChanges();
                        Session["ItemDataPO"] = dtLoc1;
                    }
                }
                catch (Exception ex) { ex.ToString(); }
            }
            Label lblAmtTotal = (Label)gvDetails.FooterRow.FindControl("lblAmtTotal");
            lblAmtTotal.Text = Convert.ToString(Math.Round(decAmtTotal, 2));



            //decimal decSplitAmtTotal = 0M;
            //DataTable dtSplit = (DataTable)Session["ItemDataPO"];

            //if (dtSplit != null && dtSplit.Rows.Count > 0)
            //{
            //    for (int i = 0; i < dtSplit.Rows.Count; i++)
            //    {
            //        decSplitAmtTotal += Convert.ToDecimal(dtSplit.Rows[i]["Amount"]);
            //    }
            //}
            lblAmountTotal.Text = Convert.ToString(Math.Round(decAmtTotal, 2));
            //BindLocationWiseData();

            BindLocationWiseData_Modified();

        }

        private void LoadSplitedData()
        {

            DataTable dtLoc1 = new DataTable();

            dtLoc1.Columns.Add("PO1ID");
            dtLoc1.Columns.Add("ItemID");
            dtLoc1.Columns.Add("ItemName");
            dtLoc1.Columns.Add("Unit");
            dtLoc1.Columns.Add("ConfQty");
            dtLoc1.Columns.Add("Rate");
            dtLoc1.Columns.Add("Discount");
            dtLoc1.Columns.Add("Remarks");
            dtLoc1.Columns.Add("GroupCode");
            dtLoc1.Columns.Add("Excise");
            dtLoc1.Columns.Add("Amount");
            dtLoc1.Columns.Add("LocationID");
            dtLoc1.Columns.Add("Location");

            foreach (GridViewRow gvrow in gvDetails.Rows)
            {
                DropDownList ddl = (DropDownList)gvrow.FindControl("ddlLocation");
                TextBox txtConfirmQty = (TextBox)gvrow.FindControl("txtConfirmQty");
                TextBox txtDiscount = (TextBox)gvrow.FindControl("txtDiscount");
                Label lblID = (Label)gvrow.FindControl("lblID");
                Label lblOrdQty = (Label)gvrow.FindControl("lblOrdQty");
                Label lblAmt = (Label)gvrow.FindControl("lblAmt");
                Label lblItemID = (Label)gvrow.FindControl("lblItemID");
                Label lblItemName = (Label)gvrow.FindControl("lblItemName");

                decimal ConfirmQty = Convert.ToDecimal(txtConfirmQty.Text.Trim());
                decimal decPacking = (Convert.ToDecimal(gvrow.Cells[2].Text.Trim()));


                if (ConfirmQty != 0M)
                {
                    dtLoc1.Rows.Add();
                    dtLoc1.Rows[dtLoc1.Rows.Count - 1]["PO1ID"] = lblID.Text.Trim();
                    dtLoc1.Rows[dtLoc1.Rows.Count - 1]["ItemID"] = lblItemID.Text.Trim();
                    dtLoc1.Rows[dtLoc1.Rows.Count - 1]["ItemName"] = lblItemName.Text.Trim();
                    dtLoc1.Rows[dtLoc1.Rows.Count - 1]["Unit"] = gvrow.Cells[1].Text.Trim();
                    dtLoc1.Rows[dtLoc1.Rows.Count - 1]["ConfQty"] = txtConfirmQty.Text.Trim();
                    dtLoc1.Rows[dtLoc1.Rows.Count - 1]["Rate"] = gvrow.Cells[5].Text.Trim();
                    dtLoc1.Rows[dtLoc1.Rows.Count - 1]["Discount"] = txtDiscount.Text.Trim();
                    dtLoc1.Rows[dtLoc1.Rows.Count - 1]["Remarks"] = gvrow.Cells[8].Text.Trim();
                    dtLoc1.Rows[dtLoc1.Rows.Count - 1]["GroupCode"] = gvrow.Cells[9].Text.Trim();
                    dtLoc1.Rows[dtLoc1.Rows.Count - 1]["Excise"] = 0;
                    dtLoc1.Rows[dtLoc1.Rows.Count - 1]["Amount"] = Math.Round((Convert.ToDecimal(txtConfirmQty.Text.Trim()) * (Convert.ToDecimal(gvrow.Cells[5].Text.Trim()))), 2);
                    dtLoc1.Rows[dtLoc1.Rows.Count - 1]["LocationID"] = ddl.SelectedItem.Value.Trim();
                    dtLoc1.Rows[dtLoc1.Rows.Count - 1]["Location"] = ddl.SelectedItem.Text.Trim();

                    lblOrdQty.Text = (Convert.ToInt32(lblOrdQty.Text) - ConfirmQty).ToString();
                    lblAmt.Text = Convert.ToString(Math.Round((Convert.ToDecimal(lblOrdQty.Text) * Convert.ToDecimal(gvrow.Cells[5].Text)), 2));
                }

            }

            decimal decAmtTotal = 0M;
            foreach (GridViewRow grow in gvDetails.Rows)
            {
                Label lblAmt1 = (Label)grow.FindControl("lblAmt");
                if (lblAmt1.Text != "")
                {
                    decAmtTotal += Convert.ToDecimal(lblAmt1.Text);
                }
            }
            Label lblAmtTotal = (Label)gvDetails.FooterRow.FindControl("lblAmtTotal");
            //lblAmtTotal.Text = Convert.ToString(Math.Round(decAmtTotal, 2));
            Session["ItemDataPO"] = dtLoc1;
            decimal decSplitAmtTotal = 0M;
            DataTable dtSplit = (DataTable)Session["ItemDataPO"];

            if (dtSplit != null && dtSplit.Rows.Count > 0)
            {
                for (int i = 0; i < dtSplit.Rows.Count; i++)
                {
                    decSplitAmtTotal += Convert.ToDecimal(dtSplit.Rows[i]["Amount"]);
                }
            }
            lblAmountTotal.Text = Convert.ToString(decSplitAmtTotal);
            //BindLocationWiseData();
            
            BindLocationWiseData_Modified();

           
        }

        private void LoadGridData()
        {
            try
            {

                string str = @"select T1.POrdId AS ID,T2.ItemId AS ItemID,T2.ItemName,T2.StdPack AS CartonQty,T1.Qty AS OrdQty,T2.MRP AS Rate,
                                T1.Qty*T2.MRP AS Amount,T2.Unit,T1.Remarks,T2.PriceGroup,0.00 AS Excise,T2.SyncId,T2.ItemCode
                              from TransPurchOrder1 AS T1 LEFT JOIN MastItem AS T2 
                             ON T1.ItemId=T2.ItemId 
                             WHERE T1.POrdId=" +Convert.ToInt32(Session["POID"])+"";
                DataTable objData = DbConnectionDAL.GetDataTable(CommandType.Text, str);

                DataTable dt = new DataTable();

                dt.Columns.Add("ID");
                dt.Columns.Add("ItemID");
                dt.Columns.Add("ItemName");
                dt.Columns.Add("CartonQty");
                dt.Columns.Add("OrdQty");
                dt.Columns.Add("Rate");
                dt.Columns.Add("Unit");
                dt.Columns.Add("Remarks");
                dt.Columns.Add("GroupCode");
                dt.Columns.Add("Excise");
                dt.Columns.Add("Amount");
                if (objData != null && objData.Rows.Count > 0)
                {
                    for (int i = 0; i < objData.Rows.Count; i++)
                    {
                        dt.Rows.Add();

                        dt.Rows[dt.Rows.Count - 1]["ID"] = objData.Rows[i]["ID"];
                        dt.Rows[dt.Rows.Count - 1]["ItemID"] = objData.Rows[i]["ItemID"];
                        dt.Rows[dt.Rows.Count - 1]["ItemName"] = "(" + objData.Rows[i]["SyncId"].ToString() + ")" + " " + objData.Rows[i]["ItemName"].ToString() + " " + "(" + objData.Rows[i]["ItemCode"].ToString() + ")"; 
                        dt.Rows[dt.Rows.Count - 1]["CartonQty"] = Convert.ToInt32(objData.Rows[i]["CartonQty"]);
                        dt.Rows[dt.Rows.Count - 1]["OrdQty"] = Convert.ToInt32(objData.Rows[i]["OrdQty"]);
                        dt.Rows[dt.Rows.Count - 1]["Rate"] = Math.Round(Convert.ToDecimal(objData.Rows[i]["Rate"]), 2);
                        dt.Rows[dt.Rows.Count - 1]["Amount"] = Math.Round(Convert.ToDecimal(objData.Rows[i]["Amount"]), 2);
                        dt.Rows[dt.Rows.Count - 1]["Remarks"] = objData.Rows[i]["Remarks"];
                        dt.Rows[dt.Rows.Count - 1]["GroupCode"] = objData.Rows[i]["PriceGroup"];
                        dt.Rows[dt.Rows.Count - 1]["Excise"] = objData.Rows[i]["Excise"];
                        dt.Rows[dt.Rows.Count - 1]["Unit"] = objData.Rows[i]["Unit"];
                    }
                }

                if (dt != null && dt.Rows.Count > 0)
                {
                    gvDetails.DataSource = dt;
                    gvDetails.DataBind();
                }


                foreach (GridViewRow gvrow in gvDetails.Rows)
                {
                    DropDownList ddlLocation = (DropDownList)gvrow.FindControl("ddlLocation");                

                    //var obj = (from r in context.MastDepots orderby r.Name select new { r.Id, r.Name }).ToList();

                    string strLoc = @"SELECT T1.ResCenId AS Id,T1.ResCenName AS Name FROM MastResCentre AS T1 WHERE T1.Active=1 ORDER BY T1.ResCenName";
                    DataTable obj = DbConnectionDAL.GetDataTable(CommandType.Text, strLoc);

                    string strLocID = @"SELECT T1.ResCenId AS Id,T1.ResCenName AS Name FROM MastResCentre AS T1 WHERE T1.Active=1 AND  Upper(T1.ResCenName)='ASTRAL FACTORY'";
                    DataTable dtID = DbConnectionDAL.GetDataTable(CommandType.Text, strLocID);

                    if (obj != null && obj.Rows.Count > 0)
                    {
                        ddlLocation.DataSource = obj;
                        ddlLocation.DataTextField = "Name";
                        ddlLocation.DataValueField = "Id";
                        ddlLocation.DataBind();
                        ddlLocation.Items.Insert(0, new ListItem("--Select--", "0"));

                        if(dtID !=null && dtID.Rows.Count>0)
                        {
                            ddlLocation.SelectedValue = Convert.ToString(dtID.Rows[0]["Id"]);
                        }
                    }                  
                }

                decimal decAmtTotal = 0M;
                foreach (GridViewRow grow in gvDetails.Rows)
                {
                    Label lblAmt = (Label)grow.FindControl("lblAmt");
                    if (lblAmt.Text != "")
                    {
                        decAmtTotal += Convert.ToDecimal(lblAmt.Text);
                    }
                }
                Label lblAmtTotal = (Label)gvDetails.FooterRow.FindControl("lblAmtTotal");
                lblAmtTotal.Text = Convert.ToString(Math.Round(decAmtTotal, 2));
            }
            catch (Exception ex)
            {

            }
        }

//        public bool AddP()
//        {
//            string pageName = Path.GetFileName(Request.Path);
//            bool addP = UP.CheckAddPermission(pageName, Convert.ToString(Session["user_name"]));
           
//            return addP;
//        }

//        public bool EditP()
//        {
//            string pageName = Path.GetFileName(Request.Path);
//            bool editP = UP.CheckEditPermission(pageName, Convert.ToString(Session["user_name"]));
//            return editP;
//        }
//        public bool DeleteP()
//        {
//            string pageName = Path.GetFileName(Request.Path);
//            bool deleteP = UP.CheckDeletePermission(pageName, Convert.ToString(Session["user_name"]));
//            return deleteP;
//        }

//        private void BindOrder()
//        {
//            try
//            {
//                //var objOrder = (from r in context.TransPurchOrders where (r.OrderStatus.Equals("H") || r.OrderStatus.Equals(null)) orderby r.POrdId select r).ToList();
//                var objOrder = (from r in context.TransPurchOrders where (r.OrderStatus.Equals(null)) orderby r.POrdId select r).ToList();

//                if (objOrder != null && objOrder.Count > 0)
//                {
//                    ddlOrder.DataSource = objOrder;
//                    ddlOrder.DataTextField = "PODocId";
//                    ddlOrder.DataValueField = "POrdId";
//                    ddlOrder.DataBind();
//                    ddlOrder.Items.Insert(0, new ListItem("--Select--", "0"));
//                }
//            }
//            catch (Exception)
//            {
               
//            }
//        }

//        private void LoadData()
//        {
//            try
//            {
//                var DistID = (from r in context.TransPurchOrders where r.POrdId.Equals(Convert.ToInt32(ddlOrder.SelectedItem.Value)) select r.DistId).ToList();

//                var obj = (from r in context.MastParties orderby r.PartyName where (r.PartyDist.Equals(1) & r.PartyId.Equals(Convert.ToInt32(DistID[0]))) select r).ToList();
//                if (obj != null && obj.Count > 0)
//                {
//                    lblBillTo.Text = obj[0].PartyName;
//                    lblRemarks.Text = obj[0].Remark;
//                    DataTable dt = new DataTable();
//                    var Query = "SELECT * FROM MastArea WHERE areaid IN(SELECT Maingrp FROM MastAreaGrp WHERE AreaId=" + Convert.ToInt32(obj[0].AreaId) + ")";
//                    dt = DAL.getFromDataTable(Query);

//                    if (obj[0].Address2 != "")
//                    {
//                        lblBillToAdd.Text = obj[0].Address1 + "," + obj[0].Address2;
//                    }
//                    if (dt != null && dt.Rows.Count > 0)
//                    {
//                        if (obj[0].Address2 != "")
//                        {
//                            DataRow[] drow = dt.Select("AreaType='CITY'");
//                            if (drow != null && drow.Length > 0)
//                            {
//                                lblBillToAdd.Text = obj[0].Address1 + "," + obj[0].Address2 + "," + drow[0]["AreaName"];
//                            }
//                        }
//                        if (obj[0].Address2 != "" && obj[0].Pin != "")
//                        {
//                            DataRow[] drow = dt.Select("AreaType='CITY'");
//                            DataRow[] drow1 = dt.Select("AreaType='STATE'");
//                            DataRow[] drow2 = dt.Select("AreaType='COUNTRY'");

//                            if (drow != null && drow.Length > 0)
//                            {
//                                lblBillToAdd.Text = obj[0].Address1 + "," + obj[0].Address2 + "," + drow[0]["AreaName"] + "-" + obj[0].Pin;
//                                if (drow1 != null && drow1.Length > 0)
//                                {
//                                    lblBillToAdd.Text = obj[0].Address1 + "," + obj[0].Address2 + "," + drow[0]["AreaName"] + "-" + obj[0].Pin + "," + drow1[0]["AreaName"];
//                                    if (drow2 != null && drow2.Length > 0)
//                                    {
//                                        lblBillToAdd.Text = obj[0].Address1 + "," + obj[0].Address2 + "," + drow[0]["AreaName"] + "-" + obj[0].Pin + "," + drow1[0]["AreaName"] + "," + drow2[0]["AreaName"];
//                                    }
//                                }
//                            }

//                        }
//                        if (obj[0].Address2 != "" && obj[0].Pin != "" && obj[0].Mobile != "")
//                        {
//                            DataRow[] drow = dt.Select("AreaType='CITY'");
//                            DataRow[] drow1 = dt.Select("AreaType='STATE'");
//                            DataRow[] drow2 = dt.Select("AreaType='COUNTRY'");

//                            if (drow != null && drow.Length > 0)
//                            {
//                                lblBillToAdd.Text = obj[0].Address1 + "," + obj[0].Address2 + "," + drow[0]["AreaName"] + "-" + obj[0].Pin + "," + obj[0].Mobile;
//                                if (drow1 != null && drow1.Length > 0)
//                                {
//                                    lblBillToAdd.Text = obj[0].Address1 + "," + obj[0].Address2 + "," + drow[0]["AreaName"] + "-" + obj[0].Pin + "," + drow1[0]["AreaName"] + "," + obj[0].Mobile;
//                                    if (drow2 != null && drow2.Length > 0)
//                                    {
//                                        lblBillToAdd.Text = obj[0].Address1 + "," + obj[0].Address2 + "," + drow[0]["AreaName"] + "-" + obj[0].Pin + "," + drow1[0]["AreaName"] + "," + drow2[0]["AreaName"] + "," + obj[0].Mobile;
//                                    }
//                                }
//                            }

//                        }
//                        if (obj[0].Address2 != "" && obj[0].Pin != "" && obj[0].Mobile != "" && obj[0].Email != "")
//                        {

//                            DataRow[] drow = dt.Select("AreaType='CITY'");
//                            DataRow[] drow1 = dt.Select("AreaType='STATE'");
//                            DataRow[] drow2 = dt.Select("AreaType='COUNTRY'");

//                            if (drow != null && drow.Length > 0)
//                            {
//                                lblBillToAdd.Text = obj[0].Address1 + "," + obj[0].Address2 + "," + drow[0]["AreaName"] + "-" + obj[0].Pin + "," + obj[0].Mobile + "|" + obj[0].Email;
//                                if (drow1 != null && drow1.Length > 0)
//                                {
//                                    lblBillToAdd.Text = obj[0].Address1 + "," + obj[0].Address2 + "," + drow[0]["AreaName"] + "-" + obj[0].Pin + "," + drow1[0]["AreaName"] + "," + obj[0].Mobile + "|" + obj[0].Email;
//                                    if (drow2 != null && drow2.Length > 0)
//                                    {
//                                        lblBillToAdd.Text = obj[0].Address1 + "," + obj[0].Address2 + "," + drow[0]["AreaName"] + "-" + obj[0].Pin + "," + drow1[0]["AreaName"] + "," + drow2[0]["AreaName"] + "," + obj[0].Mobile + "|" + obj[0].Email;
//                                    }
//                                }
//                            }

//                        }
//                    }
//                }

//                var objOrder = (from r in context.TransPurchOrders where r.POrdId.Equals(Convert.ToInt32(ddlOrder.SelectedItem.Value)) orderby r.POrdId select r).ToList();
//                if (objOrder != null && objOrder.Count > 0)
//                {

//                    lblCreatedDate.Text = objOrder[0].VDate.ToString("dd/M/yyyy");
//                    lblDispatchTo.Text = objOrder[0].DispName;

//                    if (objOrder[0].DispAdd1 != "" && objOrder[0].DispAdd2 != "" && objOrder[0].DispCity != "" && objOrder[0].DispPin != "" && objOrder[0].DispState != "" && objOrder[0].DispCountry != "" && objOrder[0].DispMobile != "" && objOrder[0].DispEmail!="")
//                    {
//                        var City = (from r in context.MastAreas where r.AreaId.Equals(Convert.ToInt32(objOrder[0].DispCity)) select r).ToList();
//                        var State = (from r in context.MastAreas where r.AreaId.Equals(Convert.ToInt32(objOrder[0].DispState)) select r).ToList();
//                        var Country = (from r in context.MastAreas where r.AreaId.Equals(Convert.ToInt32(objOrder[0].DispCountry)) select r).ToList();

//                        lblDispatchToAdd.Text = objOrder[0].DispAdd1 + "," + objOrder[0].DispAdd2 + "," + City[0] + "-" + objOrder[0].DispPin + "," + State[0] + "," + Country[0]+","+objOrder[0].DispMobile+"|"+objOrder[0].DispEmail;
//                    }

                   
//                }

//                LoadGridData();
//            }
//            catch (Exception ex)
//            {
               
//            }
//        }

     
//        private void LoadGridData()
//        {
//            try
//            {
//                var objData = (from r in context.TransPurchOrder1s
//                               join m in context.MastItems on r.ItemId equals m.ItemId
//                               where r.POrdId.Equals(Convert.ToInt32(ddlOrder.SelectedItem.Value.Trim()))
//                               select new
//                               {
//                                   ID = r.POrd1Id,
//                                   ItemID = r.ItemId,
//                                   ItemName = m.ItemName,
//                                   Qty = r.Qty,
//                                   Unit = m.Unit,
//                                   Rate = r.Rate,
//                                   CartonQty = m.StdPack,
//                                   Amount = (r.Qty * r.Rate)
//                               }).ToList();

//                DataTable dt = new DataTable();

//                dt.Columns.Add("ID");
//                dt.Columns.Add("ItemID");
//                dt.Columns.Add("ItemName");
//                dt.Columns.Add("CartonQty");
//                dt.Columns.Add("OrdQty");
//                dt.Columns.Add("Rate");
//                dt.Columns.Add("Amount");

//                if(objData!=null && objData.Count>0)
//                {
//                    for (int i = 0; i < objData.Count; i++)
//                    {
//                        dt.Rows.Add();
//                        dt.Rows[dt.Rows.Count - 1]["ID"] = objData[i].ID;
//                        dt.Rows[dt.Rows.Count - 1]["ItemID"] = objData[i].ItemID;
//                        dt.Rows[dt.Rows.Count - 1]["ItemName"] = objData[i].ItemName;
//                        dt.Rows[dt.Rows.Count - 1]["CartonQty"] = Convert.ToInt32(objData[i].CartonQty);
//                        dt.Rows[dt.Rows.Count - 1]["OrdQty"] = Convert.ToInt32(objData[i].Qty);
//                        dt.Rows[dt.Rows.Count - 1]["Rate"] = Math.Round(Convert.ToDecimal(objData[i].Rate), 2);
//                        dt.Rows[dt.Rows.Count - 1]["Amount"] = Math.Round(Convert.ToDecimal(objData[i].Amount), 2);
//                    }
//                }

//                if(dt!=null && dt.Rows.Count>0)
//                {
//                    gvDetails.DataSource = dt;
//                    gvDetails.DataBind();
//                }


//                foreach(GridViewRow gvrow in gvDetails.Rows)
//                {
//                    DropDownList ddlLocation = (DropDownList)gvrow.FindControl("ddlLocation");
//                    TextBox txtDiscount = (TextBox)gvrow.FindControl("txtDiscount");
//                    TextBox txtConfirmQty = (TextBox)gvrow.FindControl("txtDiscount");

//                    var obj = (from r in context.MastDepots orderby r.Name select new { r.Id,r.Name}).ToList();

//                    if (obj != null && obj.Count > 0)
//                    {
//                        ddlLocation.DataSource = obj;
//                        ddlLocation.DataTextField = "Name";
//                        ddlLocation.DataValueField = "Id";
//                        ddlLocation.DataBind();
//                        ddlLocation.Items.Insert(0, new ListItem("--Select--", "0"));
//                    }

//                    txtDiscount.Attributes.Add("pattern", "^[0-9]{1,3}(\\.[0-9]{0,2})?$");
//                    txtDiscount.ToolTip = "Maximum digits left side of decimal : " + 3 + " and Maximum digits right side of decimal : " + 2;

//                    txtConfirmQty.Attributes.Add("pattern", "^[0-9]{1,3}(\\.[0-9]{0,2})?$");
//                    txtConfirmQty.ToolTip = "Enter digits only";
//                }
             
                

             
//            }
//            catch (Exception ex)
//            {

//            }
//        }

//        protected void ddlOrder_SelectedIndexChanged(object sender, EventArgs e)
//        {
//            try
//            {
//                Session["ItemDataPO"] = null;
//                Session["Main"] = null;
//                LoadData();
//            }
//            catch (Exception ex)
//            {

//            }
//        }

//        [WebMethod]
//        public static string GetData()
//        {
//            string data = string.Empty;

//            try
//            {
//                DataTable dt = (DataTable)HttpContext.Current.Session["ItemDataPO"];

//                if (dt != null && dt.Rows.Count > 0)
//                {
//                    var lst = dt.AsEnumerable().Select(r => r.Table.Columns.Cast<DataColumn>().Select(c => new KeyValuePair<string, object>(c.ColumnName, r[c.Ordinal])).ToDictionary(z => z.Key, z => z.Value)).ToList();
//                    //now serialize it
//                    var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
//                    data = serializer.Serialize(lst);
//                }
//                else
//                {
//                    List<string> lst = new List<string>();
//                    JavaScriptSerializer serializer = new JavaScriptSerializer();
//                    data = serializer.Serialize(lst);
//                }

//                return data;
                
//            }
//            catch
//            {
//                return data;

//            }
//        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            GridViewRow gvrow = (GridViewRow)btn.NamingContainer;
            DropDownList ddl = (DropDownList)gvrow.FindControl("ddlLocation");
            TextBox txtConfirmQty = (TextBox)gvrow.FindControl("txtConfirmQty");
            TextBox txtDiscount = (TextBox)gvrow.FindControl("txtDiscount");           
            Label lblID = (Label)gvrow.FindControl("lblID");
            Label lblOrdQty = (Label)gvrow.FindControl("lblOrdQty");
            Label lblAmt = (Label)gvrow.FindControl("lblAmt");
            Label lblItemID = (Label)gvrow.FindControl("lblItemID");
            Label lblItemName = (Label)gvrow.FindControl("lblItemName");

             if (Convert.ToDecimal(lblOrdQty.Text.Trim())== 0M)
             {
                 txtConfirmQty.Focus();
                 System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Errormessage", "errormessage('All quantity is consumed!');", true);
                 return;
             }
            if(txtConfirmQty.Text=="")
            {
                txtConfirmQty.Focus();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Errormessage", "errormessage('Please enter confirm order quantity!');", true);
                return;
            }

            decimal ConfirmQty = Convert.ToDecimal(txtConfirmQty.Text.Trim());
            if (Convert.ToDecimal(txtConfirmQty.Text) > Convert.ToDecimal(lblOrdQty.Text.Trim()))
            {
                txtConfirmQty.Focus();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Errormessage", "errormessage('Confirm Qty should be less than or equal to Ordered Qty!');", true);
                return;
            }

            decimal decPacking = (Convert.ToDecimal(gvrow.Cells[2].Text.Trim()));
            int QtyCheck = 0;
            decimal decQtyCheck = 0M;

            if (Convert.ToInt32(decPacking) != 0)
            {
                QtyCheck = Convert.ToInt32(Math.Round(Convert.ToDecimal(txtConfirmQty.Text.Trim()), 0)) / Convert.ToInt32(decPacking);
                decQtyCheck = Convert.ToDecimal(txtConfirmQty.Text.Trim()) / decPacking;
            }

            if (decPacking != 0M)
            {
                if (!(QtyCheck > 0 && decQtyCheck > 0M && QtyCheck == decQtyCheck))
                {
                    txtConfirmQty.Focus();
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Errormessage", "errormessage('Please enter quantity in multiple of packing!');", true);
                    return;
                }
            }
            if(txtDiscount.Text=="0")
            {
                txtConfirmQty.Focus();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Errormessage", "errormessage('Please enter discount!');", true);
                return;
            }
            if (txtDiscount.Text.Trim()!="")
            {
                if (Convert.ToDecimal(txtDiscount.Text.Trim()) > 100M)
                {
                    txtConfirmQty.Focus();
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Errormessage", "errormessage('Please enter discount less than or equal to 100!');", true);
                    return;
                }
            }           
            if(ddl.SelectedItem.Value=="0")
            {
                ddl.Focus();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Errormessage", "errormessage('Please select location!');", true);
                return;
            }

                

            if (Session["ItemDataPO"] != null)
            {
                DataTable dtLoc1 = (DataTable)Session["ItemDataPO"];
                bool flag = true;
                if(dtLoc1!=null && dtLoc1.Rows.Count>0)
                {
                    for(int i=0;i<dtLoc1.Rows.Count;i++)
                    {
                        if ((Convert.ToInt32(dtLoc1.Rows[i]["ItemID"]) == Convert.ToInt32(lblItemID.Text)) && (Convert.ToInt32(dtLoc1.Rows[i]["LocationID"]) == Convert.ToInt32(ddl.SelectedItem.Value.Trim()))
                            && (Convert.ToString(dtLoc1.Rows[i]["GroupCode"]) == gvrow.Cells[9].Text.Trim()))
                        {
                            flag = false;
                            dtLoc1.Rows[i]["ConfQty"] = Convert.ToInt32(dtLoc1.Rows[i]["ConfQty"]) + Convert.ToInt32(txtConfirmQty.Text.Trim());
                            dtLoc1.Rows[i]["Amount"] = Math.Round((Convert.ToDecimal(dtLoc1.Rows[i]["Amount"]) + Math.Round((Convert.ToDecimal(txtConfirmQty.Text.Trim()) * (Convert.ToDecimal(gvrow.Cells[5].Text.Trim()))), 2)), 2);
                        }
                    }
                }

                if (flag)
                {
                    dtLoc1.Rows.Add();
                    dtLoc1.Rows[dtLoc1.Rows.Count - 1]["PO1ID"] = lblID.Text.Trim();
                    dtLoc1.Rows[dtLoc1.Rows.Count - 1]["ItemID"] = lblItemID.Text.Trim();
                    dtLoc1.Rows[dtLoc1.Rows.Count - 1]["ItemName"] = lblItemName.Text.Trim();
                    dtLoc1.Rows[dtLoc1.Rows.Count - 1]["Unit"] = gvrow.Cells[1].Text.Trim();
                    dtLoc1.Rows[dtLoc1.Rows.Count - 1]["ConfQty"] = txtConfirmQty.Text.Trim();
                    dtLoc1.Rows[dtLoc1.Rows.Count - 1]["Rate"] = gvrow.Cells[5].Text.Trim();
                    dtLoc1.Rows[dtLoc1.Rows.Count - 1]["Discount"] = txtDiscount.Text.Trim();
                    dtLoc1.Rows[dtLoc1.Rows.Count - 1]["Remarks"] = gvrow.Cells[8].Text.Trim();
                    dtLoc1.Rows[dtLoc1.Rows.Count - 1]["GroupCode"] = gvrow.Cells[9].Text.Trim();
                    dtLoc1.Rows[dtLoc1.Rows.Count - 1]["Excise"] = 0;
                    dtLoc1.Rows[dtLoc1.Rows.Count - 1]["Amount"] = Math.Round((Convert.ToDecimal(txtConfirmQty.Text.Trim()) * (Convert.ToDecimal(gvrow.Cells[5].Text.Trim()))), 2);
                    dtLoc1.Rows[dtLoc1.Rows.Count - 1]["LocationID"] = ddl.SelectedItem.Value.Trim();
                    dtLoc1.Rows[dtLoc1.Rows.Count - 1]["Location"] = ddl.SelectedItem.Text.Trim();
                }

                Session["ItemDataPO"] = dtLoc1;
            }
            else
            {


                DataTable dtLoc1 = new DataTable();
                dtLoc1.Columns.Add("PO1ID");
                dtLoc1.Columns.Add("ItemID");
                dtLoc1.Columns.Add("ItemName");
                dtLoc1.Columns.Add("Unit");
                dtLoc1.Columns.Add("ConfQty");
                dtLoc1.Columns.Add("Rate");
                dtLoc1.Columns.Add("Discount");
                dtLoc1.Columns.Add("Remarks");
                dtLoc1.Columns.Add("GroupCode");
                dtLoc1.Columns.Add("Excise");
                dtLoc1.Columns.Add("Amount");
                dtLoc1.Columns.Add("LocationID");
                dtLoc1.Columns.Add("Location");

                dtLoc1.Rows.Add();
                dtLoc1.Rows[dtLoc1.Rows.Count - 1]["PO1ID"] = lblID.Text.Trim();
                dtLoc1.Rows[dtLoc1.Rows.Count - 1]["ItemID"] = lblItemID.Text.Trim();
                dtLoc1.Rows[dtLoc1.Rows.Count - 1]["ItemName"] = lblItemName.Text.Trim();
                dtLoc1.Rows[dtLoc1.Rows.Count - 1]["Unit"] = gvrow.Cells[1].Text.Trim();
                dtLoc1.Rows[dtLoc1.Rows.Count - 1]["ConfQty"] = txtConfirmQty.Text.Trim();
                dtLoc1.Rows[dtLoc1.Rows.Count - 1]["Rate"] = gvrow.Cells[5].Text.Trim();
                dtLoc1.Rows[dtLoc1.Rows.Count - 1]["Discount"] = txtDiscount.Text.Trim();
                dtLoc1.Rows[dtLoc1.Rows.Count - 1]["Remarks"] = gvrow.Cells[8].Text.Trim();
                dtLoc1.Rows[dtLoc1.Rows.Count - 1]["GroupCode"] = gvrow.Cells[9].Text.Trim();
                dtLoc1.Rows[dtLoc1.Rows.Count - 1]["Excise"] = 0;
                dtLoc1.Rows[dtLoc1.Rows.Count - 1]["Amount"] = Math.Round((Convert.ToDecimal(txtConfirmQty.Text.Trim()) * (Convert.ToDecimal(gvrow.Cells[5].Text.Trim()))), 2);
                dtLoc1.Rows[dtLoc1.Rows.Count - 1]["LocationID"] = ddl.SelectedItem.Value.Trim();
                dtLoc1.Rows[dtLoc1.Rows.Count - 1]["Location"] = ddl.SelectedItem.Text.Trim();

                Session["ItemDataPO"] = dtLoc1;
            }

            txtConfirmQty.Text = "";
            ddl.SelectedValue = "0";
            //txtDiscount.Text = "";

            lblOrdQty.Text = (Convert.ToInt32(lblOrdQty.Text) - ConfirmQty).ToString();
            lblAmt.Text = Convert.ToString(Math.Round((Convert.ToDecimal(lblOrdQty.Text) * Convert.ToDecimal(gvrow.Cells[5].Text)), 2));

            decimal decAmtTotal = 0M;
            foreach (GridViewRow grow in gvDetails.Rows)
            {
                Label lblAmt1 = (Label)grow.FindControl("lblAmt");
                if (lblAmt.Text != "")
                {
                    decAmtTotal += Convert.ToDecimal(lblAmt1.Text);
                }
            }
            Label lblAmtTotal = (Label)gvDetails.FooterRow.FindControl("lblAmtTotal");
            lblAmtTotal.Text = Convert.ToString(Math.Round(decAmtTotal, 2));

            decimal decSplitAmtTotal = 0M;
            DataTable dtSplit = (DataTable)Session["ItemDataPO"];

            if(dtSplit!=null && dtSplit.Rows.Count>0)
            {
                for(int i=0;i<dtSplit.Rows.Count;i++)
                {
                    decSplitAmtTotal += Convert.ToDecimal(dtSplit.Rows[i]["Amount"]);
                }
            }
            lblAmountTotal.Text = Convert.ToString(decSplitAmtTotal);
            //BindLocationWiseData();
            BindLocationWiseData_Modified();
        }

        private void BindLocationWiseData_Modified()
        {
            string[] arr = { "GroupCode", "LocationID" };
            DataTable dt = (DataTable)Session["ItemDataPO"];

            DataTable dtUni = new DataTable();
            dtUni = dt.DefaultView.ToTable(true, arr);

            DataTable dtFinal = new DataTable();
            dtFinal.Columns.Add("LocationID");
            dtFinal.Columns.Add("GroupCode");

            if (dtUni != null && dtUni.Rows.Count > 0)
            {

                for (int i = 0; i < dtUni.Rows.Count; i++)
                {
                    dtFinal.Rows.Add();
                    dtFinal.Rows[dtFinal.Rows.Count - 1]["LocationID"] = Convert.ToInt32(dtUni.Rows[i]["LocationID"]);
                    dtFinal.Rows[dtFinal.Rows.Count - 1]["GroupCode"] = Convert.ToString(dtUni.Rows[i]["GroupCode"]);

                }
            }

            gvLocDataDetails.DataSource = dtFinal;
            gvLocDataDetails.DataBind();

         

        }

        protected void gvLocDataDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string[] arr = { "GroupCode", "LocationID" };
                DataTable dt = (DataTable)Session["ItemDataPO"];

                DataTable dtUni = new DataTable();
                dtUni = dt.DefaultView.ToTable(true, arr);

                if (dtUni != null && dtUni.Rows.Count > 0)
                {
                    GridView gvLoc = (GridView)e.Row.FindControl("gvLocwiseData");
                    Label lbllocName = (Label)e.Row.FindControl("lbllocName");
                    Label lblLocationID = (Label)e.Row.FindControl("lblLocationID");
                    Label lblGroupCode = (Label)e.Row.FindControl("lblGroupCode");
                  
                        DataTable dtLoc = new DataTable();
                        DataRow[] drow1 = dt.Select("LocationID=" + lblLocationID.Text.Trim() + " and GroupCode='" + lblGroupCode.Text.Trim()+"'");

                        DataView dv = new DataView(dt);
                        //     dv.RowFilter = "RoleName='Level 1'";
                        dv.RowFilter = "LocationID=" + lblLocationID.Text.Trim() + " and GroupCode='" + lblGroupCode.Text.Trim() + "'";

                        //if (drow1 != null && drow1.Length > 0)
                        //{
                            //dtLoc = drow1.CopyToDataTable();
                            dtLoc = dv.ToTable();
                            lbllocName.Text = dtLoc.Rows[0]["Location"].ToString() + "-" + dtLoc.Rows[0]["GroupCode"].ToString();
                            gvLoc.DataSource = dtLoc;
                            gvLoc.DataBind();

                            decimal decAmtTotal = 0M;
                            foreach (GridViewRow grow in gvLoc.Rows)
                            {
                                Label lblAmt = (Label)grow.FindControl("lblOriAmount");
                                //lblAmt.Text =Convert.ToString(Convert.ToDecimal(lblAmt.Text)-)

                                if (lblAmt.Text != "")
                                {
                                    decAmtTotal += Convert.ToDecimal(lblAmt.Text);
                                }
                            }
                            Label lblAmtTotal = (Label)gvLoc.FooterRow.FindControl("lblAmtTotal");
                            lblAmtTotal.Text = Convert.ToString(Math.Round(decAmtTotal, 2));
                       // }
                }
            }
                
        }

        private void BindLocationWiseData()
        {
            try
            {
                string str = @"SELECT T1.ResCenId AS Id,T1.ResCenName AS Name FROM MastResCentre AS T1 ORDER BY T1.ResCenName";
                DataTable dtLoc = DbConnectionDAL.GetDataTable(CommandType.Text, str);

                int intCount = 0;
                if (dtLoc != null && dtLoc.Rows.Count > 0)
                {

                    DataTable dt = (DataTable)Session["ItemDataPO"];
                    if (dt != null && dt.Rows.Count > 0)
                    {                       
                        DynamicRow.Visible = true;
                        if (intCount != dt.Rows.Count)
                        {
                            DataRow[] drow1 = dt.Select("LocationID=" + Convert.ToInt32(dtLoc.Rows[0]["Id"]) + "");
                            if (drow1 != null && drow1.Length > 0)
                            {
                                DataTable dtLoc1 = drow1.CopyToDataTable();
                                lblOrderLocation1.Text = dtLoc1.Rows[0]["Location"].ToString() + "-" + dtLoc1.Rows[0]["GroupCode"].ToString();
                                divLoc1.Visible = true;
                                gvDataLoc1.DataSource = dtLoc1;
                                gvDataLoc1.DataBind();
                                intCount += 1;
                            }
                        }
                        if (intCount != dt.Rows.Count)
                        {
                            DataRow[] drow2 = dt.Select("LocationID=" + Convert.ToInt32(dtLoc.Rows[1]["Id"]) + "");
                            if (drow2 != null && drow2.Length > 0)
                            {
                                DataTable dtLoc2 = drow2.CopyToDataTable();
                                lblOrderLocation2.Text = dtLoc2.Rows[0]["Location"].ToString() + "-" + dtLoc2.Rows[0]["GroupCode"].ToString();
                                divLoc2.Visible = true;
                                gvDataLoc2.DataSource = dtLoc2;
                                gvDataLoc2.DataBind();
                                intCount += 1;
                            }
                        }
                        if (intCount != dt.Rows.Count)
                        {
                            DataRow[] drow3 = dt.Select("LocationID=" + Convert.ToInt32(dtLoc.Rows[2]["Id"]) + "");
                            if (drow3 != null && drow3.Length > 0)
                            {
                                DataTable dtLoc3 = drow3.CopyToDataTable();
                                lblOrderLocation3.Text = dtLoc3.Rows[0]["Location"].ToString() + "-" + dtLoc3.Rows[0]["GroupCode"].ToString();
                                divLoc3.Visible = true;
                                gvDataLoc3.DataSource = dtLoc3;
                                gvDataLoc3.DataBind();
                                intCount += 1;
                            }
                        }
                        if (intCount != dt.Rows.Count)
                        {
                            DataRow[] drow4 = dt.Select("LocationID=" + Convert.ToInt32(dtLoc.Rows[3]["Id"]) + "");
                            if (drow4 != null && drow4.Length > 0)
                            {
                                DataTable dtLoc4 = drow4.CopyToDataTable();
                                lblOrderLocation4.Text = dtLoc4.Rows[0]["Location"].ToString() + "-" + dtLoc4.Rows[0]["GroupCode"].ToString();
                                divLoc4.Visible = true;
                                gvDataLoc4.DataSource = dtLoc4;
                                gvDataLoc4.DataBind();
                                intCount += 1;
                            }
                        }
                        if (intCount != dt.Rows.Count)
                        {
                            DataRow[] drow5 = dt.Select("LocationID=" + Convert.ToInt32(dtLoc.Rows[4]["Id"]) + "");
                            if (drow5 != null && drow5.Length > 0)
                            {
                                DataTable dtLoc5 = drow5.CopyToDataTable();
                                lblOrderLocation5.Text = dtLoc5.Rows[0]["Location"].ToString() + "-" + dtLoc5.Rows[0]["GroupCode"].ToString();
                                divLoc5.Visible = true;
                                gvDataLoc5.DataSource = dtLoc5;
                                gvDataLoc5.DataBind();
                                intCount += 1;
                            }
                        }

                        if (intCount != dt.Rows.Count)
                        {
                            DataRow[] drow6 = dt.Select("LocationID=" + Convert.ToInt32(dtLoc.Rows[5]["Id"]) + "");
                            if (drow6 != null && drow6.Length > 0)
                            {
                                DataTable dtLoc6 = drow6.CopyToDataTable();
                                lblOrderLocation6.Text = dtLoc6.Rows[0]["Location"].ToString() + "-" + dtLoc6.Rows[0]["GroupCode"].ToString();
                                divLoc6.Visible = true;
                                gvDataLoc6.DataSource = dtLoc6;
                                gvDataLoc6.DataBind();
                                intCount += 1;
                            }
                        }
                        if (intCount != dt.Rows.Count)
                        {
                            DataRow[] drow7 = dt.Select("LocationID=" + Convert.ToInt32(dtLoc.Rows[6]["Id"]) + "");
                            if (drow7 != null && drow7.Length > 0)
                            {
                                DataTable dtLoc7 = drow7.CopyToDataTable();
                                lblOrderLocation7.Text = dtLoc7.Rows[0]["Location"].ToString() + "-" + dtLoc7.Rows[0]["GroupCode"].ToString();
                                divLoc7.Visible = true;
                                gvDataLoc7.DataSource = dtLoc7;
                                gvDataLoc7.DataBind();
                                intCount += 1;
                            }
                        }
                        if (intCount != dt.Rows.Count)
                        {
                            DataRow[] drow8 = dt.Select("LocationID=" + Convert.ToInt32(dtLoc.Rows[7]["Id"]) + "");
                            if (drow8 != null && drow8.Length > 0)
                            {
                                DataTable dtLoc8 = drow8.CopyToDataTable();
                                lblOrderLocation8.Text = dtLoc8.Rows[0]["Location"].ToString() + "-" + dtLoc8.Rows[0]["GroupCode"].ToString();
                                divLoc8.Visible = true;
                                gvDataLoc8.DataSource = dtLoc8;
                                gvDataLoc8.DataBind();
                                intCount += 1;
                            }
                        }
                        if (intCount != dt.Rows.Count)
                        {
                            DataRow[] drow9 = dt.Select("LocationID=" + Convert.ToInt32(dtLoc.Rows[8]["Id"]) + "");
                            if (drow9 != null && drow9.Length > 0)
                            {
                                DataTable dtLoc9 = drow9.CopyToDataTable();
                                lblOrderLocation9.Text = dtLoc9.Rows[0]["Location"].ToString() + "-" + dtLoc9.Rows[0]["GroupCode"].ToString();
                                divLoc9.Visible = true;
                                gvDataLoc9.DataSource = dtLoc9;
                                gvDataLoc9.DataBind();
                                intCount += 1;
                            }
                        }

                        if (intCount != dt.Rows.Count)
                        {
                            DataRow[] drow10 = dt.Select("LocationID=" + Convert.ToInt32(dtLoc.Rows[9]["Id"]) + "");
                            if (drow10 != null && drow10.Length > 0)
                            {
                                DataTable dtLoc10 = drow10.CopyToDataTable();
                                lblOrderLocation10.Text = dtLoc10.Rows[0]["Location"].ToString() + "-" + dtLoc10.Rows[0]["GroupCode"].ToString();
                                divLoc10.Visible = true;
                                gvDataLoc10.DataSource = dtLoc10;
                                gvDataLoc10.DataBind();
                                intCount += 1;
                            }
                        }
                        if (intCount != dt.Rows.Count)
                        {
                            DataRow[] drow11 = dt.Select("LocationID=" + Convert.ToInt32(dtLoc.Rows[10]["Id"]) + "");
                            if (drow11 != null && drow11.Length > 0)
                            {
                                DataTable dtLoc11 = drow11.CopyToDataTable();
                                lblOrderLocation11.Text = dtLoc11.Rows[0]["Location"].ToString() + "-" + dtLoc11.Rows[0]["GroupCode"].ToString();
                                divLoc11.Visible = true;
                                gvDataLoc11.DataSource = dtLoc11;
                                gvDataLoc11.DataBind();
                                intCount += 1;
                            }
                        }
                        if (intCount != dt.Rows.Count)
                        {
                            DataRow[] drow12 = dt.Select("LocationID=" + Convert.ToInt32(dtLoc.Rows[11]["Id"]) + "");
                            if (drow12 != null && drow12.Length > 0)
                            {
                                DataTable dtLoc12 = drow12.CopyToDataTable();
                                lblOrderLocation12.Text = dtLoc12.Rows[0]["Location"].ToString() + "-" + dtLoc12.Rows[0]["GroupCode"].ToString();
                                divLoc12.Visible = true;
                                gvDataLoc12.DataSource = dtLoc12;
                                gvDataLoc12.DataBind();
                                intCount += 1;
                            }
                        }
                        if (intCount != dt.Rows.Count)
                        {
                            DataRow[] drow13 = dt.Select("LocationID=" + Convert.ToInt32(dtLoc.Rows[12]["Id"]) + "");
                            if (drow13 != null && drow13.Length > 0)
                            {
                                DataTable dtLoc13 = drow13.CopyToDataTable();
                                lblOrderLocation13.Text = dtLoc13.Rows[0]["Location"].ToString() + "-" + dtLoc13.Rows[0]["GroupCode"].ToString();
                                divLoc13.Visible = true;
                                gvDataLoc13.DataSource = dtLoc13;
                                gvDataLoc13.DataBind();
                                intCount += 1;
                            }
                        }
                        if (intCount != dt.Rows.Count)
                        {
                            DataRow[] drow14 = dt.Select("LocationID=" + Convert.ToInt32(dtLoc.Rows[13]["Id"]) + "");
                            if (drow14 != null && drow14.Length > 0)
                            {
                                DataTable dtLoc14 = drow14.CopyToDataTable();
                                lblOrderLocation14.Text = dtLoc14.Rows[0]["Location"].ToString() + "-" + dtLoc14.Rows[0]["GroupCode"].ToString();
                                divLoc14.Visible = true;
                                gvDataLoc14.DataSource = dtLoc14;
                                gvDataLoc14.DataBind();
                                intCount += 1;
                            }
                        }
                        if (intCount != dt.Rows.Count)
                        {
                            DataRow[] drow15 = dt.Select("LocationID=" + Convert.ToInt32(dtLoc.Rows[14]["Id"]) + "");
                            if (drow15 != null && drow15.Length > 0)
                            {
                                DataTable dtLoc15 = drow15.CopyToDataTable();
                                lblOrderLocation15.Text = dtLoc15.Rows[0]["Location"].ToString() + "-" + dtLoc15.Rows[0]["GroupCode"].ToString();
                                divLoc15.Visible = true;
                                gvDataLoc15.DataSource = dtLoc15;
                                gvDataLoc15.DataBind();
                                intCount += 1;
                            }
                        }
                        if (intCount != dt.Rows.Count)
                        {
                            DataRow[] drow16 = dt.Select("LocationID=" + Convert.ToInt32(dtLoc.Rows[15]["Id"]) + "");
                            if (drow16 != null && drow16.Length > 0)
                            {
                                DataTable dtLoc16 = drow16.CopyToDataTable();
                                lblOrderLocation16.Text = dtLoc16.Rows[0]["Location"].ToString() + "-" + dtLoc16.Rows[0]["GroupCode"].ToString();
                                divLoc16.Visible = true;
                                gvDataLoc16.DataSource = dtLoc16;
                                gvDataLoc16.DataBind();
                                intCount += 1;
                            }
                        }
                        if (intCount != dt.Rows.Count)
                        {
                            DataRow[] drow17 = dt.Select("LocationID=" + Convert.ToInt32(dtLoc.Rows[16]["Id"]) + "");
                            if (drow17 != null && drow17.Length > 0)
                            {
                                DataTable dtLoc17 = drow17.CopyToDataTable();
                                lblOrderLocation17.Text = dtLoc17.Rows[0]["Location"].ToString() + "-" + dtLoc17.Rows[0]["GroupCode"].ToString();
                                divLoc17.Visible = true;
                                gvDataLoc17.DataSource = dtLoc17;
                                gvDataLoc17.DataBind();
                                intCount += 1;
                            }
                        }
                        if (intCount != dt.Rows.Count)
                        {
                            DataRow[] drow18 = dt.Select("LocationID=" + Convert.ToInt32(dtLoc.Rows[17]["Id"]) + "");
                            if (drow18 != null && drow18.Length > 0)
                            {
                                DataTable dtLoc18 = drow18.CopyToDataTable();
                                lblOrderLocation18.Text = dtLoc18.Rows[0]["Location"].ToString() + "-" + dtLoc18.Rows[0]["GroupCode"].ToString();
                                divLoc18.Visible = true;
                                gvDataLoc18.DataSource = dtLoc18;
                                gvDataLoc18.DataBind();
                                intCount += 1;
                            }
                        }
                        if (intCount != dt.Rows.Count)
                        {
                            DataRow[] drow19 = dt.Select("LocationID=" + Convert.ToInt32(dtLoc.Rows[18]["Id"]) + "");
                            if (drow19 != null && drow19.Length > 0)
                            {
                                DataTable dtLoc19 = drow19.CopyToDataTable();
                                lblOrderLocation19.Text = dtLoc19.Rows[0]["Location"].ToString() + "-" + dtLoc19.Rows[0]["GroupCode"].ToString();
                                divLoc19.Visible = true;
                                gvDataLoc19.DataSource = dtLoc19;
                                gvDataLoc19.DataBind();
                                intCount += 1;
                            }
                        }
                        if (intCount != dt.Rows.Count)
                        {
                            DataRow[] drow20 = dt.Select("LocationID=" + Convert.ToInt32(dtLoc.Rows[19]["Id"]) + "");
                            if (drow20 != null && drow20.Length > 0)
                            {
                                DataTable dtLoc20 = drow20.CopyToDataTable();
                                lblOrderLocation20.Text = dtLoc20.Rows[0]["Location"].ToString() + "-" + dtLoc20.Rows[0]["GroupCode"].ToString();
                                divLoc20.Visible = true;
                                gvDataLoc20.DataSource = dtLoc20;
                                gvDataLoc20.DataBind();
                                intCount += 1;
                            }
                        }
                    }
                }


            }
            catch (Exception ex)
            {

            }
        }
       

//        protected void btnOK_Click(object sender, EventArgs e)
//        {
//            DataTable dtMain1 =null;
//            if( HttpContext.Current.Session["Main"]!=null)
//            {
//                dtMain1 = (DataTable)HttpContext.Current.Session["Main"];               
//                int count = 0;
//                foreach (GridViewRow gvrow1 in gvDetails.Rows)
//                {
//                    Label lblOrdQty1 = (Label)gvrow1.FindControl("lblOrdQty");
//                    Label lblAmt1 = (Label)gvrow1.FindControl("lblAmt");

//                    lblOrdQty1.Text = Convert.ToString(dtMain1.Rows[count]["OrdQty"]);
//                    lblAmt1.Text = Convert.ToString(dtMain1.Rows[count]["Amount"]);
//                    count++;
//                }
//            }

//            Button btn = (Button)sender;
//            GridViewRow gvrow = (GridViewRow)btn.NamingContainer;
//            DropDownList ddl = (DropDownList)gvrow.FindControl("ddlLocation");
//            TextBox txtConfirmQty = (TextBox)gvrow.FindControl("txtConfirmQty");
//            TextBox txtDiscount = (TextBox)gvrow.FindControl("txtDiscount");
//            TextBox txtRemarks = (TextBox)gvrow.FindControl("txtRemarks");           
//            Label lblID = (Label)gvrow.FindControl("lblID");
//            Label lblOrdQty = (Label)gvrow.FindControl("lblOrdQty");
//            Label lblAmt = (Label)gvrow.FindControl("lblAmt");
//            Label lblItemID = (Label)gvrow.FindControl("lblItemID");          

//            if (dtMain1 != null)
//            {
//                DataRow[] drow1 = dtMain1.Select("ID=" + lblID.Text);
//                lblOrdQty.Text = Convert.ToString(drow1[0]["OrdQty"]);
//                lblAmt.Text = Convert.ToString(drow1[0]["Amount"]);
//            }
           
//            decimal OrdQty = Convert.ToDecimal(lblOrdQty.Text);
            
//            bool flag = true;

//            if (Convert.ToInt32(lblOrdQty.Text) != 0)
//            {
//                if (Session["ItemDataPO"] != null)
//                {
//                    DataTable dt = (DataTable)Session["ItemDataPO"];

//                    for (int i = 0; i < dt.Rows.Count; i++)
//                    {
//                        if (Convert.ToString(dt.Rows[i]["ItemName"]) == gvrow.Cells[0].Text && Convert.ToString(dt.Rows[i]["Location"]) == ddl.SelectedItem.Text)
//                        { 
//                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Errormessage", "errormessage('Item for selected location already exist please delete and then insert!');", true);
//                            txtConfirmQty.Text = "";
//                            ddl.SelectedValue = "0";
//                            flag = false;
//                            break;
//                        }
//                    }
//                }

//                if (flag==true)
//                {
//                    if (txtConfirmQty.Text != "" && txtConfirmQty.Text != "0")
//                    {
//                        if (Convert.ToDecimal(txtConfirmQty.Text) > 0)
//                        {

//                            decimal ConfirmQty = Convert.ToDecimal(txtConfirmQty.Text.Trim());
//                                if (Convert.ToDecimal(txtConfirmQty.Text) <= OrdQty)
//                                {
//                                    bool discFlag = true;
//                                    if (txtDiscount.Text != "")
//                                    {
//                                        if (Convert.ToDecimal(txtDiscount.Text) >= 0)
//                                        {
//                                            if (Convert.ToDecimal(txtDiscount.Text) <= 100)
//                                            {
//                                                discFlag = true;
//                                            }
//                                            else
//                                            {
//                                                discFlag = false;
//                                                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Errormessage", "errormessage('Please enter discount less than or equal to 100!');", true);
//                                            }
//                                        }
//                                        else
//                                        {
//                                            discFlag = false;
//                                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Errormessage", "errormessage('Please enter positive discount!');", true);
//                                        }
//                                    }

//                                    if (discFlag)
//                                    {
//                                        if (ddl.SelectedItem.Value != "0")
//                                        {
//                                            DataTable dtItem = null;
//                                            if (Session["ItemDataPO"] == null)
//                                            {
//                                                dtItem = new DataTable();
//                                                dtItem.Columns.Add("SNo");
//                                                dtItem.Columns.Add("ID");
//                                                dtItem.Columns.Add("ItemID");
//                                                dtItem.Columns.Add("ItemName");
//                                                dtItem.Columns.Add("CartonQty");
//                                                dtItem.Columns.Add("OrdQty");
//                                                dtItem.Columns.Add("Rate");
//                                                dtItem.Columns.Add("Discount");
//                                                dtItem.Columns.Add("Location");
//                                                dtItem.Columns.Add("LocationID");
//                                                dtItem.Columns.Add("Amount");
//                                                dtItem.Columns.Add("Remark");

//                                                dtItem.Rows.Add();
//                                                dtItem.Rows[dtItem.Rows.Count - 1]["SNo"] = dtItem.Rows.Count;
//                                                dtItem.Rows[dtItem.Rows.Count - 1]["ID"] = lblID.Text;
//                                                dtItem.Rows[dtItem.Rows.Count - 1]["ItemID"] = lblItemID.Text.Trim();
//                                                dtItem.Rows[dtItem.Rows.Count - 1]["ItemName"] = gvrow.Cells[0].Text;
//                                                dtItem.Rows[dtItem.Rows.Count - 1]["CartonQty"] = gvrow.Cells[1].Text;
//                                                dtItem.Rows[dtItem.Rows.Count - 1]["OrdQty"] = Convert.ToString(ConfirmQty);
//                                                dtItem.Rows[dtItem.Rows.Count - 1]["Rate"] = gvrow.Cells[3].Text;
//                                                dtItem.Rows[dtItem.Rows.Count - 1]["Discount"] = txtDiscount.Text;
//                                                dtItem.Rows[dtItem.Rows.Count - 1]["Location"] = ddl.SelectedItem.Text;
//                                                dtItem.Rows[dtItem.Rows.Count - 1]["LocationID"] = ddl.SelectedItem.Value;
//                                                dtItem.Rows[dtItem.Rows.Count - 1]["Amount"] = Math.Round((Convert.ToDecimal(ConfirmQty) * Convert.ToDecimal(gvrow.Cells[3].Text)), 2);
//                                                dtItem.Rows[dtItem.Rows.Count - 1]["Remark"] = txtRemarks.Text;


//                                                lblOrdQty.Text = (Convert.ToInt32(lblOrdQty.Text) - ConfirmQty).ToString();
//                                                lblAmt.Text = Convert.ToString(Math.Round((Convert.ToDecimal(lblOrdQty.Text) * Convert.ToDecimal(gvrow.Cells[3].Text)), 2));


//                                                if (txtDiscount.Text != "")
//                                                {
//                                                    dtItem.Rows[dtItem.Rows.Count - 1]["Amount"] = Math.Round((Convert.ToDecimal(dtItem.Rows[dtItem.Rows.Count - 1]["Amount"]) - (Convert.ToDecimal(dtItem.Rows[dtItem.Rows.Count - 1]["Amount"]) * Convert.ToDecimal(txtDiscount.Text) / 100)), 2);
//                                                    lblAmt.Text = Convert.ToString(Math.Round((Convert.ToDecimal(lblAmt.Text) - (Convert.ToDecimal(lblAmt.Text) * Convert.ToDecimal(txtDiscount.Text) / 100)), 2));
//                                                }

//                                                Session["ItemDataPO"] = dtItem;

//                                                txtConfirmQty.Text = "";
//                                                ddl.SelectedValue = "0";


//                                            }
//                                            else
//                                            {

//                                                dtItem = (DataTable)Session["ItemDataPO"];
//                                                dtItem.Rows.Add();
//                                                dtItem.Rows[dtItem.Rows.Count - 1]["SNo"] = dtItem.Rows.Count;
//                                                dtItem.Rows[dtItem.Rows.Count - 1]["ID"] = lblID.Text;
//                                                dtItem.Rows[dtItem.Rows.Count - 1]["ItemID"] = lblItemID.Text.Trim();
//                                                dtItem.Rows[dtItem.Rows.Count - 1]["ItemName"] = gvrow.Cells[0].Text;
//                                                dtItem.Rows[dtItem.Rows.Count - 1]["CartonQty"] = gvrow.Cells[1].Text;
//                                                dtItem.Rows[dtItem.Rows.Count - 1]["OrdQty"] = Convert.ToString(ConfirmQty);
//                                                dtItem.Rows[dtItem.Rows.Count - 1]["Rate"] = gvrow.Cells[3].Text;
//                                                dtItem.Rows[dtItem.Rows.Count - 1]["Discount"] = txtDiscount.Text;
//                                                dtItem.Rows[dtItem.Rows.Count - 1]["Location"] = ddl.SelectedItem.Text;
//                                                dtItem.Rows[dtItem.Rows.Count - 1]["LocationID"] = ddl.SelectedItem.Value;
//                                                dtItem.Rows[dtItem.Rows.Count - 1]["Amount"] = Math.Round((Convert.ToDecimal(ConfirmQty) * Convert.ToDecimal(gvrow.Cells[3].Text)), 2);
//                                                dtItem.Rows[dtItem.Rows.Count - 1]["Remark"] = txtRemarks.Text;


//                                                lblOrdQty.Text = (Convert.ToInt32(lblOrdQty.Text) - ConfirmQty).ToString();
//                                                lblAmt.Text = Convert.ToString(Math.Round((Convert.ToDecimal(lblOrdQty.Text) * Convert.ToDecimal(gvrow.Cells[3].Text)), 2));


//                                                if (txtDiscount.Text != "")
//                                                {
//                                                    dtItem.Rows[dtItem.Rows.Count - 1]["Amount"] = Math.Round((Convert.ToDecimal(dtItem.Rows[dtItem.Rows.Count - 1]["Amount"]) - (Convert.ToDecimal(dtItem.Rows[dtItem.Rows.Count - 1]["Amount"]) * Convert.ToDecimal(txtDiscount.Text) / 100)), 2);
//                                                    lblAmt.Text = Convert.ToString(Math.Round((Convert.ToDecimal(lblAmt.Text) - (Convert.ToDecimal(lblAmt.Text) * Convert.ToDecimal(txtDiscount.Text) / 100)), 2));
//                                                }


//                                                Session["ItemDataPO"] = dtItem;

//                                                txtConfirmQty.Text = "";
//                                                ddl.SelectedValue = "0";


//                                            }

//                                            DataTable dtMain = new DataTable();
//                                            dtMain.Columns.Add("ItemName");
//                                            dtMain.Columns.Add("CartonQty");
//                                            dtMain.Columns.Add("OrdQty");
//                                            dtMain.Columns.Add("Rate");
//                                            dtMain.Columns.Add("ID");
//                                            dtMain.Columns.Add("Amount");
//                                            dtMain.Columns.Add("Discount");
//                                            dtMain.Columns.Add("Remarks");

//                                            foreach (GridViewRow row in gvDetails.Rows)
//                                            {
//                                                Label amt = (Label)row.FindControl("lblAmt");
//                                                Label ID = (Label)row.FindControl("lblID");
//                                                Label Qty = (Label)row.FindControl("lblOrdQty");
//                                                TextBox Disc = (TextBox)row.FindControl("txtDiscount");
//                                                TextBox Remarks = (TextBox)row.FindControl("txtRemarks");

//                                                dtMain.Rows.Add();
//                                                if (ID.Text == lblID.Text)
//                                                {
//                                                    dtMain.Rows[dtMain.Rows.Count - 1]["OrdQty"] = lblOrdQty.Text;
//                                                    dtMain.Rows[dtMain.Rows.Count - 1]["Amount"] = lblAmt.Text;
//                                                }
//                                                else
//                                                {
//                                                    dtMain.Rows[dtMain.Rows.Count - 1]["OrdQty"] = Qty.Text;
//                                                    dtMain.Rows[dtMain.Rows.Count - 1]["Amount"] = amt.Text;
//                                                }

//                                                dtMain.Rows[dtMain.Rows.Count - 1]["ItemName"] = row.Cells[0].Text;
//                                                dtMain.Rows[dtMain.Rows.Count - 1]["CartonQty"] = row.Cells[1].Text;

//                                                dtMain.Rows[dtMain.Rows.Count - 1]["Rate"] = row.Cells[3].Text;
//                                                dtMain.Rows[dtMain.Rows.Count - 1]["ID"] = ID.Text;

//                                                dtMain.Rows[dtMain.Rows.Count - 1]["Discount"] = Disc.Text;
//                                                dtMain.Rows[dtMain.Rows.Count - 1]["Remarks"] = Remarks.Text;
//                                            }

//                                            Session["Main"] = dtMain;

//                                        }
//                                        else
//                                        {
//                                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Errormessage", "errormessage('Please select location!');", true);
//                                        }
//                                    }
//                                }///////////////////////////////////////////////////////
//                                else
//                                {
//                                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Errormessage", "errormessage('Confirm Qty should be less than or equal to Ordered Qty!');", true);
//                                }
                           
//                        }
//                        else
//                        {
//                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Errormessage", "errormessage('Please enter positive confirm order quantity!');", true);
//                        }
//                        //////////////////////////////////////////////////
//                    }
//                    else
//                    {
//                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Errormessage", "errormessage('Please enter confirm order quantity!');", true);
//                    }
//                }
//            }
//            else
//            {

//                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Errormessage", "errormessage('Ordered quantity is consumed!');", true);
//                txtConfirmQty.Text = "";
//                ddl.SelectedValue = "0";
//            }

//            if (Session["ItemDataPO"] != null)
//            {

//                Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "LoadFinalGrid();", true);
//            }
           
//        }

        protected void btnApply_Click(object sender, EventArgs e)
        {
                if (txtDiscount.Text != "")
                {
                    if (Convert.ToDecimal(txtDiscount.Text) > 0)
                    {
                        if (Convert.ToDecimal(txtDiscount.Text) < 100)
                        {
                            foreach (GridViewRow gvrow in gvDetails.Rows)
                            {
                                TextBox txtDisc = (TextBox)gvrow.FindControl("txtDiscount");
                                Label lblOrdQty = (Label)gvrow.FindControl("lblOrdQty");
                                Label lblOriQty = (Label)gvrow.FindControl("lblOriQty");
                                Label lblItemID = (Label)gvrow.FindControl("lblItemID");

                                txtDisc.Text = txtDiscount.Text.Trim();

                                Label lblAmt = (Label)gvrow.FindControl("lblAmt");
                                Label lblOriAmount = (Label)gvrow.FindControl("lblOriAmount");
                                decimal amt = Convert.ToDecimal(lblOrdQty.Text) * Convert.ToDecimal(gvrow.Cells[5].Text);
                                decimal ModdecAmt = amt - (amt * Convert.ToDecimal(txtDisc.Text) / 100);

                                decimal amt1 = Convert.ToDecimal(lblOriQty.Text) * Convert.ToDecimal(gvrow.Cells[5].Text);
                                decimal ModdecAmt1 = amt1 - (amt1 * Convert.ToDecimal(txtDisc.Text) / 100);

                                lblAmt.Text = Convert.ToString(ModdecAmt);
                                lblOriAmount.Text = Convert.ToString(ModdecAmt1);
                                decimal decAmtTotal = 0M;
                                foreach (GridViewRow grow in gvDetails.Rows)
                                {
                                    Label lblAmt1 = (Label)grow.FindControl("lblOriAmount");
                                    if (lblAmt.Text != "")
                                    {
                                        decAmtTotal += Convert.ToDecimal(lblAmt1.Text);
                                    }
                                }
                                Label lblAmtTotal = (Label)gvDetails.FooterRow.FindControl("lblAmtTotal");                               
                                lblAmtTotal.Text = Convert.ToString(Math.Round(decAmtTotal, 2));

                                if (Session["ItemDataPO"] != null)
                                {
                                    DataTable dtLoc1 = (DataTable)Session["ItemDataPO"];

                                    foreach (DataRow dr in dtLoc1.Rows) // search whole table
                                    {
                                        if (dr["ItemID"].ToString() == lblItemID.Text.Trim())
                                        {
                                            dr["Discount"] = txtDisc.Text.Trim();
                                            dr["Amount"] = lblOriAmount.Text.Trim();

                                        }
                                    }

                                    dtLoc1.AcceptChanges();
                                    Session["ItemDataPO"] = dtLoc1;
                                }
                            }

                            decimal decSplitAmtTotal = 0M;
                            DataTable dtSplit = (DataTable)Session["ItemDataPO"];

                            if (dtSplit != null && dtSplit.Rows.Count > 0)
                            {
                                for (int i = 0; i < dtSplit.Rows.Count; i++)
                                {
                                    decSplitAmtTotal += Convert.ToDecimal(dtSplit.Rows[i]["Amount"]);
                                }
                            }
                            lblAmountTotal.Text = Convert.ToString(decSplitAmtTotal);

                            BindLocationWiseData_Modified();
                        }
                        else
                        {
                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Errormessage", "errormessage('Please enter vaues less than or equal to 100!');", true);
                            return;                            
                        }

                    }
                    else
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Errormessage", "errormessage('Please enter positive discount!');", true);
                        return;          
                    }
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Errormessage", "errormessage('Please enter discount!');", true);
                    return;          
                }
           
        }      

        protected void txtDiscount_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;
       
            if (txt.Text != "")
            {
                    if (Convert.ToDecimal(txt.Text) >= 0)
                    {
                        if (Convert.ToDecimal(txt.Text) <= 100)
                        {
                            GridViewRow gvRow = (GridViewRow)txt.NamingContainer;
                            Label lblOriginalAmount = (Label)gvRow.FindControl("lblOriginalAmount");
                            Label lblOriQty = (Label)gvRow.FindControl("lblOriQty");
                            Label lblAmt = (Label)gvRow.FindControl("lblAmt");
                            Label lblOriAmount = (Label)gvRow.FindControl("lblOriAmount");
                            Label lblOrdQty = (Label)gvRow.FindControl("lblOrdQty");
                            Label lblID = (Label)gvRow.FindControl("lblID");
                            Label lblItemID = (Label)gvRow.FindControl("lblItemID");

                            decimal ModdecAmt = (Convert.ToDecimal(lblOrdQty.Text) * Convert.ToDecimal(gvRow.Cells[5].Text)) - (Convert.ToDecimal(lblOrdQty.Text) * Convert.ToDecimal(gvRow.Cells[5].Text) * Convert.ToDecimal(txt.Text) / 100);
                            decimal ModdecAmt1 = (Convert.ToDecimal(lblOriQty.Text) * Convert.ToDecimal(gvRow.Cells[5].Text)) - (Convert.ToDecimal(lblOriQty.Text) * Convert.ToDecimal(gvRow.Cells[5].Text) * Convert.ToDecimal(txt.Text) / 100);
                            lblAmt.Text = Convert.ToString(ModdecAmt);
                            lblOriAmount.Text = Convert.ToString(ModdecAmt1);
                            
                            decimal decAmtTotal = 0M;
                            foreach (GridViewRow grow in gvDetails.Rows)
                            {
                                Label lblAmt1 = (Label)grow.FindControl("lblOriAmount");
                                if (lblAmt1.Text != "")
                                {
                                    decAmtTotal += Convert.ToDecimal(lblAmt1.Text);
                                }
                            }
                            Label lblAmtTotal = (Label)gvDetails.FooterRow.FindControl("lblAmtTotal");
                            lblAmtTotal.Text = Convert.ToString(Math.Round(decAmtTotal, 2));

                            if (Session["ItemDataPO"] != null)
                            {
                                DataTable dtLoc1 = (DataTable)Session["ItemDataPO"];

                                foreach (DataRow dr in dtLoc1.Rows) // search whole table
                                {
                                    if (dr["ItemID"].ToString() == lblItemID.Text.Trim())
                                    {
                                        dr["Discount"] = txt.Text.Trim();
                                        dr["Amount"] = lblOriAmount.Text.Trim();

                                    }
                                }

                                dtLoc1.AcceptChanges();
                                Session["ItemDataPO"] = dtLoc1;
                            }

                            decimal decSplitAmtTotal = 0M;
                            DataTable dtSplit = (DataTable)Session["ItemDataPO"];

                            if (dtSplit != null && dtSplit.Rows.Count > 0)
                            {
                                for (int i = 0; i < dtSplit.Rows.Count; i++)
                                {
                                    decSplitAmtTotal += Convert.ToDecimal(dtSplit.Rows[i]["Amount"]);
                                }
                            }
                            lblAmountTotal.Text = Convert.ToString(decSplitAmtTotal);
                            BindLocationWiseData_Modified();
                        }
                        else
                        {
                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Errormessage", "errormessage('Please enter vaues less than or equal to 100!');", true);
                            return;
                        }
                    }
                    else
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Errormessage", "errormessage('Please enter positive discount!');", true);
                        return;
                    }               
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Errormessage", "errormessage('Please enter discount!');", true);
                return;
                
            }
        }       
       
     
        //private void HoldOrderUpdate()
        //{
        //    OpeartionDataContext context1 = new OpeartionDataContext();
        //    context1.Connection.Open();
        //    System.Data.Common.DbTransaction trans = context1.Connection.BeginTransaction();
        //    context1.Transaction = trans;
        //    DateTime dtCurrDate = BusinessClass.GetUTCTime();
        //    try
        //    {
        //        #region Update Status in TransPurchOrder

        //        TransPurchOrder objTransPO = context1.TransPurchOrders.Where(x => x.POrdId == Convert.ToInt32(ddlOrder.SelectedItem.Value)).Single<TransPurchOrder>();
        //        objTransPO.OrderStatus = ddlStatus.SelectedItem.Value;
        //        context1.SubmitChanges();

        //        #endregion

        //        #region Update Data in TransPurchOrderConf

        //        TransPurchOrderConf objTransPOConf = context1.TransPurchOrderConfs.Where(r => r.PCOrdId == Convert.ToInt32(Session["ConPOID"])).Single<TransPurchOrderConf>();
        //        objTransPOConf.OrderStatus = ddlStatus.SelectedItem.Value;
        //        context1.SubmitChanges();

        //        #endregion

        //        trans.Commit();

        //        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Order " + ddlOrder.SelectedItem.Text + " is on Hold.');", true);

        //        ClearFormData();
        //    }
        //    catch (Exception ex)
        //    {
        //        trans.Rollback();
        //    }
        //    finally
        //    {
        //        if (context1.Connection != null)
        //        {
        //            context1.Connection.Close();

        //        }
        //    }
        //}

        //private void ConfirmOrderUpdate()
        //{
        //    OpeartionDataContext context1 = new OpeartionDataContext();
        //    context1.Connection.Open();
        //    System.Data.Common.DbTransaction trans = context1.Connection.BeginTransaction();
        //    context1.Transaction = trans;
        //    DateTime dtCurrDate = BusinessClass.GetUTCTime();
        //    try
        //    {
        //        #region Update Status in TransPurchOrder

        //        TransPurchOrder objTransPO = context1.TransPurchOrders.Where(x => x.POrdId == Convert.ToInt32(ddlOrder.SelectedItem.Value)).Single<TransPurchOrder>();
        //        objTransPO.OrderStatus = ddlStatus.SelectedItem.Value;
        //        context1.SubmitChanges();

        //        #endregion

        //        #region Update Data in TransPurchOrderConf

        //        TransPurchOrderConf objTransPOConf = context1.TransPurchOrderConfs.Where(r => r.PCOrdId == Convert.ToInt32(Session["ConPOID"])).Single<TransPurchOrderConf>();
        //        objTransPOConf.OrderStatus = ddlStatus.SelectedItem.Value;
        //        context1.SubmitChanges();

        //        #endregion

        //        #region Insert Data in TransPurchOrder1Conf

        //        DataTable dtItem = (DataTable)Session["ItemDataPO"];

        //        var objData = (from r in context1.TransPurchOrders where r.POrdId.Equals(Convert.ToInt32(ddlOrder.SelectedItem.Value)) select r).ToList();
        //        for (int i = 0; i < dtItem.Rows.Count; i++)
        //        {
        //            TransPurchOrder1Conf objTransPOConf1 = new TransPurchOrder1Conf();

        //            objTransPOConf1.POrd1Id = Convert.ToInt32(dtItem.Rows[i]["ID"]);
        //            objTransPOConf1.POrdId = Convert.ToInt32(ddlOrder.SelectedItem.Value);
        //            objTransPOConf1.PODocId = ddlOrder.SelectedItem.Text + @"/" + (i + 1);
        //            objTransPOConf1.Sno = (i + 1);
        //            objTransPOConf1.VDate = dtCurrDate;
        //            objTransPOConf1.DistId = objData[0].DistId;
        //            objTransPOConf1.UserId = Convert.ToInt32(Settings.Instance.UserID);
        //            objTransPOConf1.ItemId = Convert.ToInt32(dtItem.Rows[i]["ItemID"]);
        //            objTransPOConf1.Qty = Convert.ToDecimal(dtItem.Rows[i]["OrdQty"]);
        //            objTransPOConf1.Disc = Convert.ToString(dtItem.Rows[i]["Discount"]) == "" ? 0 : Convert.ToDecimal(dtItem.Rows[i]["Discount"]);
        //            objTransPOConf1.Location = Convert.ToInt32(dtItem.Rows[i]["LocationID"]);
        //            objTransPOConf1.Remarks = Convert.ToString(dtItem.Rows[i]["Remark"]);
        //            objTransPOConf1.Rate = Convert.ToDecimal(dtItem.Rows[i]["Rate"]);

        //            context1.TransPurchOrder1Confs.InsertOnSubmit(objTransPOConf1);
        //            context1.SubmitChanges();

        //        }
        //        #endregion

        //        trans.Commit();

        //        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Order " + ddlOrder.SelectedItem.Text + " is Confirmed.');", true);

        //        ClearFormData();
        //    }
        //    catch (Exception ex)
        //    {
        //        trans.Rollback();
        //    }
        //    finally
        //    {
        //        if (context1.Connection != null)
        //        {
        //            context1.Connection.Close();

        //        }
        //    }
        //}

        //private void CancelOrderUpdate()
        //{
        //    OpeartionDataContext context1 = new OpeartionDataContext();
        //    context1.Connection.Open();
        //    System.Data.Common.DbTransaction trans = context1.Connection.BeginTransaction();
        //    context1.Transaction = trans;
        //    DateTime dtCurrDate = BusinessClass.GetUTCTime();
        //    try
        //    {
        //        #region Update Status in TransPurchOrder

        //        TransPurchOrder objTransPO = context1.TransPurchOrders.Where(x => x.POrdId == Convert.ToInt32(ddlOrder.SelectedItem.Value)).Single<TransPurchOrder>();
        //        objTransPO.OrderStatus = ddlStatus.SelectedItem.Value;
        //        context1.SubmitChanges();

        //        #endregion

        //        #region Update Data in TransPurchOrderConf

        //        TransPurchOrderConf objTransPOConf = context1.TransPurchOrderConfs.Where(r => r.PCOrdId == Convert.ToInt32(Session["ConPOID"])).Single<TransPurchOrderConf>();
        //        objTransPOConf.OrderStatus = ddlStatus.SelectedItem.Value;
        //        context1.SubmitChanges();

        //        #endregion

        //        trans.Commit();

        //        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Order " + ddlOrder.SelectedItem.Text + " is canceled.');", true);

        //        ClearFormData();
        //    }
        //    catch (Exception ex)
        //    {
        //        trans.Rollback();
        //    }
        //    finally
        //    {
        //        if (context1.Connection != null)
        //        {
        //            context1.Connection.Close();

        //        }
        //    }
        //}

        //private void HoldOrder()
        //{
        //    OpeartionDataContext context1 = new OpeartionDataContext();
        //    context1.Connection.Open();
        //    System.Data.Common.DbTransaction trans = context1.Connection.BeginTransaction();
        //    context1.Transaction = trans;
        //    DateTime dtCurrDate = BusinessClass.GetUTCTime();

        //    try
        //    {
        //        #region Update Status in TransPurchOrder

        //        TransPurchOrder objTransPO = context1.TransPurchOrders.Where(x => x.POrdId == Convert.ToInt32(ddlOrder.SelectedItem.Value)).Single<TransPurchOrder>();
        //        objTransPO.OrderStatus = ddlStatus.SelectedItem.Value;
        //        context1.SubmitChanges();

        //        #endregion

        //        #region Insert Data in TransPurchOrderConf

        //        var objData = (from r in context1.TransPurchOrders where r.POrdId.Equals(Convert.ToInt32(ddlOrder.SelectedItem.Value)) select r).ToList();

        //        TransPurchOrderConf objTransPOConf = new TransPurchOrderConf();
        //        objTransPOConf.POrdId = Convert.ToInt32(ddlOrder.SelectedItem.Value);
        //        objTransPOConf.PODocId = ddlOrder.SelectedItem.Text;
        //        objTransPOConf.VDate = dtCurrDate;
        //        objTransPOConf.UserId = Convert.ToInt32(Settings.Instance.UserID);
        //        objTransPOConf.SMId = objData[0].SMId;
        //        objTransPOConf.DistId = objData[0].DistId;
        //        objTransPOConf.Remarks = objData[0].Remarks;
        //        objTransPOConf.DispName = objData[0].DispName;
        //        objTransPOConf.DispAdd1 = objData[0].DispAdd1;
        //        objTransPOConf.DispAdd2 = objData[0].DispAdd2;
        //        objTransPOConf.DispCity = objData[0].DispCity;
        //        objTransPOConf.DispPin = objData[0].DispPin;
        //        objTransPOConf.DispState = objData[0].DispState;
        //        objTransPOConf.DispCountry = objData[0].DispCountry;
        //        objTransPOConf.DispPhone = objData[0].DispPhone;
        //        objTransPOConf.DispMobile = objData[0].DispMobile;
        //        objTransPOConf.DispEmail = objData[0].DispEmail;
        //        objTransPOConf.OrderStatus = ddlStatus.SelectedItem.Value;

        //        context1.TransPurchOrderConfs.InsertOnSubmit(objTransPOConf);
        //        context1.SubmitChanges();

        //        #endregion                


        //        trans.Commit();
        //        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Order " + ddlOrder.SelectedItem.Text + " is on Hold.');", true);
        //        ClearFormData();
        //    }
        //    catch (Exception ex)
        //    {
        //        trans.Rollback();
        //    }
        //    finally
        //    {
        //        if (context1.Connection != null)
        //        {
        //            context1.Connection.Close();

        //        }
        //    }
        //}

        private void ClearFormData()
        {
            gvDetails.DataSource = new DataTable();
            gvDetails.DataBind();
            //ddlStatus.SelectedValue = "0";
            txtDiscount.Text = "";
            lblBillTo.Text = "";
            lblDispatchTo.Text = "";
            lblBillToAdd.Text = "";
            lblDispatchToAdd.Text = "";
            lblRemarks.Text = "";
            //Session["ItemDataPO"] = null;
            //Session["Main"] = null;
            //BindOrder();
            //ddlOrder.Enabled = true;
            gvDetails.Enabled = true;

        }

//        private void ConfirmOrder()
//        {
//            try
//            {
//                OpeartionDataContext context1 = new OpeartionDataContext();
//                context1.Connection.Open();
//                System.Data.Common.DbTransaction trans = context1.Connection.BeginTransaction();
//                context1.Transaction = trans;
//                DateTime dtCurrDate = BusinessClass.GetUTCTime();

//                try
//                {
//                    #region Update Status in TransPurchOrder

//                    TransPurchOrder objTransPO = context1.TransPurchOrders.Where(x => x.POrdId == Convert.ToInt32(ddlOrder.SelectedItem.Value)).Single<TransPurchOrder>();
//                    objTransPO.OrderStatus = ddlStatus.SelectedItem.Value;
//                    context1.SubmitChanges();

//                    #endregion

//                    #region Insert Data in TransPurchOrderConf

//                    var objData = (from r in context1.TransPurchOrders where r.POrdId.Equals(Convert.ToInt32(ddlOrder.SelectedItem.Value)) select r).ToList();

//                    TransPurchOrderConf objTransPOConf = new TransPurchOrderConf();
//                    objTransPOConf.POrdId = Convert.ToInt32(ddlOrder.SelectedItem.Value);
//                    objTransPOConf.PODocId = ddlOrder.SelectedItem.Text;
//                    objTransPOConf.VDate = dtCurrDate;
//                    objTransPOConf.UserId = Convert.ToInt32(Settings.Instance.UserID);
//                    objTransPOConf.SMId = objData[0].SMId;
//                    objTransPOConf.DistId = objData[0].DistId;
//                    objTransPOConf.Remarks = objData[0].Remarks;
//                    objTransPOConf.DispName = objData[0].DispName;
//                    objTransPOConf.DispAdd1 = objData[0].DispAdd1;
//                    objTransPOConf.DispAdd2 = objData[0].DispAdd2;
//                    objTransPOConf.DispCity = objData[0].DispCity;
//                    objTransPOConf.DispPin = objData[0].DispPin;
//                    objTransPOConf.DispState = objData[0].DispState;
//                    objTransPOConf.DispCountry = objData[0].DispCountry;
//                    objTransPOConf.DispPhone = objData[0].DispPhone;
//                    objTransPOConf.DispMobile = objData[0].DispMobile;
//                    objTransPOConf.DispEmail = objData[0].DispEmail;
//                    objTransPOConf.OrderStatus = ddlStatus.SelectedItem.Value;

//                    context1.TransPurchOrderConfs.InsertOnSubmit(objTransPOConf);
//                    context1.SubmitChanges();

//                    #endregion

//                    #region Insert Data in TransPurchOrder1Conf

//                    DataTable dtItem = (DataTable)Session["ItemDataPO"];
                  

//                    for (int i = 0; i < dtItem.Rows.Count;i++)
//                    {
//                        TransPurchOrder1Conf objTransPOConf1 = new TransPurchOrder1Conf();

//                        objTransPOConf1.POrd1Id = Convert.ToInt32(dtItem.Rows[i]["ID"]);
//                        objTransPOConf1.POrdId = Convert.ToInt32(ddlOrder.SelectedItem.Value);
//                        objTransPOConf1.PODocId = ddlOrder.SelectedItem.Text + @"/" + (i + 1);
//                        objTransPOConf1.Sno = (i + 1);
//                        objTransPOConf1.VDate = dtCurrDate;
//                        objTransPOConf1.DistId = objData[0].DistId;
//                        objTransPOConf1.UserId = Convert.ToInt32(Settings.Instance.UserID);
//                        objTransPOConf1.ItemId = Convert.ToInt32(dtItem.Rows[i]["ItemID"]);
//                        objTransPOConf1.Qty = Convert.ToDecimal(dtItem.Rows[i]["OrdQty"]);
//                        objTransPOConf1.Disc = Convert.ToString(dtItem.Rows[i]["Discount"]) == "" ? 0 : Convert.ToDecimal(dtItem.Rows[i]["Discount"]);
//                        objTransPOConf1.Location = Convert.ToInt32(dtItem.Rows[i]["LocationID"]);
//                        objTransPOConf1.Remarks = Convert.ToString(dtItem.Rows[i]["Remark"]);
//                        objTransPOConf1.Rate = Convert.ToDecimal(dtItem.Rows[i]["Rate"]);

//                        context1.TransPurchOrder1Confs.InsertOnSubmit(objTransPOConf1);
//                        context1.SubmitChanges();

//                    }
//                    #endregion


//                   trans.Commit();
                
//                   System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Order " + ddlOrder.SelectedItem.Text + " is Confirmed.');", true);
//                    ClearFormData();
//                }
//                catch (Exception ex)
//                {
//                    trans.Rollback();
//                }
//                finally
//                {
//                    if (context1.Connection != null)
//                    {
//                        context1.Connection.Close();

//                    }
//                }

//            }
//            catch (Exception ex)
//            {

//            }
//        }

//        private void CancelOrder()
//        {
//            OpeartionDataContext context1 = new OpeartionDataContext();
//            context1.Connection.Open();
//            System.Data.Common.DbTransaction trans = context1.Connection.BeginTransaction();
//            context1.Transaction = trans;
//            DateTime dtCurrDate = BusinessClass.GetUTCTime();
//            try
//            {
//                #region Update Status in TransPurchOrder

//                TransPurchOrder objTransPO = context1.TransPurchOrders.Where(x => x.POrdId == Convert.ToInt32(ddlOrder.SelectedItem.Value)).Single<TransPurchOrder>();
//                objTransPO.OrderStatus = ddlStatus.SelectedItem.Value;
//                context1.SubmitChanges();

//                #endregion

//                #region Insert Data in TransPurchOrderConf

//                var objData = (from r in context1.TransPurchOrders where r.POrdId.Equals(Convert.ToInt32(ddlOrder.SelectedItem.Value)) select r).ToList();

//                TransPurchOrderConf objTransPOConf = new TransPurchOrderConf();
//                objTransPOConf.POrdId = Convert.ToInt32(ddlOrder.SelectedItem.Value);
//                objTransPOConf.PODocId = ddlOrder.SelectedItem.Text;
//                objTransPOConf.VDate = dtCurrDate;
//                objTransPOConf.UserId = Convert.ToInt32(Settings.Instance.UserID);
//                objTransPOConf.SMId = objData[0].SMId;
//                objTransPOConf.DistId = objData[0].DistId;
//                objTransPOConf.Remarks = objData[0].Remarks;
//                objTransPOConf.DispName = objData[0].DispName;
//                objTransPOConf.DispAdd1 = objData[0].DispAdd1;
//                objTransPOConf.DispAdd2 = objData[0].DispAdd2;
//                objTransPOConf.DispCity = objData[0].DispCity;
//                objTransPOConf.DispPin = objData[0].DispPin;
//                objTransPOConf.DispState = objData[0].DispState;
//                objTransPOConf.DispCountry = objData[0].DispCountry;
//                objTransPOConf.DispPhone = objData[0].DispPhone;
//                objTransPOConf.DispMobile = objData[0].DispMobile;
//                objTransPOConf.DispEmail = objData[0].DispEmail;
//                objTransPOConf.OrderStatus = ddlStatus.SelectedItem.Value;

//                context1.TransPurchOrderConfs.InsertOnSubmit(objTransPOConf);
//                context1.SubmitChanges();

//                #endregion                

//                trans.Commit();

//                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Order " + ddlOrder.SelectedItem.Text + " is canceled.');", true);

//                ClearFormData();
//            }
//            catch (Exception ex)
//            {
//                trans.Rollback();
//            }
//            finally
//            {
//                if (context1.Connection != null)
//                {
//                    context1.Connection.Close();

//                }
//            }
//        }

        protected void gvDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvDetails.PageIndex = e.NewPageIndex;
            LoadGridData();
        }

        protected void Cancel_Click(object sender, EventArgs e)
        {
            //Cancel Order
            int retdel = dp.CancelOrder(Convert.ToInt32(Session["POID"]));
            if (retdel == 1)
            {
                DisabledFormData();
                SendEmailMessage("CompanyCancelled");
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record " + lblOrderNo.Text + " Canceled Successfully');", true);
                HtmlMeta meta = new HtmlMeta();
                meta.HttpEquiv = "Refresh";
                meta.Content = "10;url=PurchaseOrderApproval.aspx";
                this.Page.Controls.Add(meta);               
            }         
        }

        
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/PurchaseOrderApproval.aspx");
        }

        protected void btnOnHold_Click(object sender, EventArgs e)
        {
           
            //OnHold Order
            int retdel = dp.HoldOrder(Convert.ToInt32(Session["POID"]));
            if (retdel == 1)
            {
                DisabledFormData();
                SendEmailMessage("OrderOnHold");
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record " + lblOrderNo.Text + " is OnHold');", true);
                HtmlMeta meta = new HtmlMeta();
                meta.HttpEquiv = "Refresh";
                meta.Content = "5;url=PurchaseOrderApproval.aspx";
                this.Page.Controls.Add(meta);        
            }    
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            string str = "";
            Label lblAmtTotal = (Label)gvDetails.FooterRow.FindControl("lblAmtTotal");            
            DataTable dt = (DataTable)Session["ItemDataPO"];

            if(dt==null || dt.Rows.Count==0)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Errormessage", "errormessage('Please transfer product before confirmation!');", true);
                return;
            }
            string strTP = @"select * from TransPurchOrder AS T1 WHERE T1.POrdId=" + Convert.ToInt32(Session["POID"]) + "";
            DataTable dtTP = DbConnectionDAL.GetDataTable(CommandType.Text, strTP);

            int retdel = dp.ConfirmOrder(Convert.ToInt32(Session["POID"]), dt, dtTP,Settings.GetUTCTime(),Convert.ToInt32(Settings.Instance.UserID),Convert.ToInt32(Settings.Instance.SMID));
            if (retdel > 0)
            {
                DisabledFormData();               
                SendEmailMessage_Attachment("OrderProcessed");
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record " + lblOrderNo.Text + " Confirmed Successfully');", true);
                btnexport.Visible = true;
                //HtmlMeta meta = new HtmlMeta();
                //meta.HttpEquiv = "Refresh";
                //meta.Content = "5;url=PurchaseOrderApproval.aspx";
                //this.Page.Controls.Add(meta);                

//                    DataTable dtOrder1 = (DataTable)Session["ItemDataPO"];
//                    DataTable dtFinal1 = new DataTable();
//                    dtFinal1.Columns.Add("OrderNo");
//                    dtFinal1.Columns.Add("Company");
//                    dtFinal1.Columns.Add("Distr");
//                    dtFinal1.Columns.Add("DivCode");
//                    dtFinal1.Columns.Add("OrderType");
//                    dtFinal1.Columns.Add("CustomerID");
//                    dtFinal1.Columns.Add("CustomerID1");
//                    dtFinal1.Columns.Add("PartyOrder");
//                    dtFinal1.Columns.Add("PartyDate");
//                    dtFinal1.Columns.Add("Location");
//                    dtFinal1.Columns.Add("Desc");
//                    dtFinal1.Columns.Add("ItemCode");
//                    dtFinal1.Columns.Add("Qty");
//                    dtFinal1.Columns.Add("BlanlSp");
//                    dtFinal1.Columns.Add("Disc");
//                    //dtFinal1.Columns.Add("Num");

//                    string strDocID = Convert.ToString(dtTP.Rows[0]["PODocId"]); ;
//                    string strModstr = strDocID.Substring(10, 8);

//                    //string[] arr = { "GroupCode", "LocationID" };

//                    //DataTable dtUni1 = new DataTable();
//                    //dtUni1 = dtOrder1.DefaultView.ToTable(true, arr);
//                    //dtOrder1.Columns.Add("Num");

//                    //if (dtUni1 != null && dtUni1.Rows.Count > 0)
//                    //{
//                    //    for (int j = 0; j < dtUni1.Rows.Count; j++)
//                    //    {

//                    //        for (int k = 0; k < dtOrder1.Rows.Count; k++)
//                    //        {
//                    //            if ((Convert.ToInt32(dtOrder1.Rows[k]["LocationID"]) == Convert.ToInt32(dtUni1.Rows[j]["LocationID"]))
//                    //    && (Convert.ToString(dtOrder1.Rows[k]["GroupCode"]) == Convert.ToString(dtUni1.Rows[j]["GroupCode"])))
//                    //            {
//                    //                dtOrder1.Rows[k]["Num"] = j + 1;
//                    //            }
//                    //        }
//                    //    }
//                    //}

//                    for (int i = 0; i < dtOrder1.Rows.Count; i++)
//                    {
//                        string strLocDet = @"SELECT rescenid,rescenname,active,syncid,isnull(plantcode,0) AS plantcode,
//                                            planttype,isnull(ordertype,'') AS ordertype,isnull(divisioncode,0) divisioncode 
//                                            FROM MastResCentre WHERE ResCenId='" + Convert.ToInt32(dtOrder1.Rows[i]["LocationID"]) + "'";

//                        DataTable dtLocDet = DbConnectionDAL.GetDataTable(CommandType.Text, strLocDet);

//                        string strDist = @"SELECT * FROM MastParty AS T WHERE T.PartyId=" + Convert.ToInt32(dtTP.Rows[0]["DistId"]) + "";
//                        DataTable dtDist = DbConnectionDAL.GetDataTable(CommandType.Text, strDist);

//                        string strItemCode = @"SELECT * FROM MastItem WHERE ItemId='" + Convert.ToInt32(dtOrder1.Rows[i]["ItemID"]) + "'";
//                        DataTable dtItemCode = DbConnectionDAL.GetDataTable(CommandType.Text, strItemCode);

//                        dtFinal1.Rows.Add();

//                        dtFinal1.Rows[dtFinal1.Rows.Count - 1]["OrderNo"] = "P" + Convert.ToString(dtOrder1.Rows[i]["Num"]);
//                        dtFinal1.Rows[dtFinal1.Rows.Count - 1]["Company"] = "1000";
//                        dtFinal1.Rows[dtFinal1.Rows.Count - 1]["Distr"] = "10";
//                        dtFinal1.Rows[dtFinal1.Rows.Count - 1]["DivCode"] = Convert.ToString(dtLocDet.Rows[0]["divisioncode"]);
//                        dtFinal1.Rows[dtFinal1.Rows.Count - 1]["OrderType"] = Convert.ToString(dtLocDet.Rows[0]["ordertype"]);
//                        dtFinal1.Rows[dtFinal1.Rows.Count - 1]["CustomerID"] = Convert.ToString(dtDist.Rows[0]["SyncId"]);
//                        dtFinal1.Rows[dtFinal1.Rows.Count - 1]["CustomerID1"] = Convert.ToString(dtDist.Rows[0]["SyncId"]);
//                        dtFinal1.Rows[dtFinal1.Rows.Count - 1]["PartyOrder"] = strModstr + "-" + "P" + Convert.ToString(dtOrder1.Rows[i]["Num"]);
//                        dtFinal1.Rows[dtFinal1.Rows.Count - 1]["PartyDate"] = Convert.ToDateTime(dtTP.Rows[0]["CreatedDate"]).ToShortDateString();
//                        dtFinal1.Rows[dtFinal1.Rows.Count - 1]["Location"] = Convert.ToString(dtLocDet.Rows[0]["PlantCode"]);
//                        dtFinal1.Rows[dtFinal1.Rows.Count - 1]["Desc"] = Convert.ToString(dtItemCode.Rows[0]["ItemName"]);
//                        dtFinal1.Rows[dtFinal1.Rows.Count - 1]["ItemCode"] = Convert.ToString(dtItemCode.Rows[0]["SyncId"]);
//                        dtFinal1.Rows[dtFinal1.Rows.Count - 1]["Qty"] = dtOrder1.Rows[i]["ConfQty"];
//                        dtFinal1.Rows[dtFinal1.Rows.Count - 1]["BlanlSp"] = "";
//                        dtFinal1.Rows[dtFinal1.Rows.Count - 1]["Disc"] = dtOrder1.Rows[i]["Discount"];
//                        //dtFinal1.Rows[dtFinal1.Rows.Count - 1]["Num"] = dtOrder1.Rows[i]["Num"];
//                    }                 
                
                   
                   
                
//                    string strDocId = lblOrderNo.Text;
//                    strDocId = strDocId.Replace(" ", "_");

//                    //dtFinal1.DefaultView.Sort = "Num ASC";
                
//                    DataTable dtw1 = dtFinal1.DefaultView.ToTable();
                      
//                    //string strPath = Server.MapPath("ExpenseReport");

//                    //DataSet ds = new DataSet();
//                    //ds.Tables.Add(dtw1);
//                    //ExportToExecl(ds, "ExpenseReport");
//                    //HtmlMeta meta = new HtmlMeta();
//                    //meta.HttpEquiv = "Refresh";
//                    //meta.Content = "5;url=PurchaseOrderApproval.aspx";
//                    //this.Page.Controls.Add(meta);  
//                 string str1 = string.Empty;
//                    Response.ClearContent();
//        Response.Buffer = true;
//        Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "Customers.xls"));
//        Response.ContentType = "application/ms-excel";
//        string tab = "";
//        int i1;
//        //DataTable dt1 = dtw1;

//        foreach (DataRow dr in dtw1.Rows)
//        {
//            tab = "";
//            for (i1 = 0; i1 < dtw1.Columns.Count; i1++)
//            {
//                Response.Write(tab + dr[i1].ToString());
//                tab = "\t";
//            }
//            Response.Write("\n");
//        }
//        Response.End();

//                    //string tab = "";
//                    //int i1;
//                    //string attachment = "attachment; filename=" + strDocId + ".xls";
//                    //Response.ClearContent();
//                    //Response.AddHeader("content-disposition", attachment);
//                    //Response.ContentType = "application/vnd.ms-excel";

//                    //foreach (DataRow dr in dtw1.Rows)
//                    //{
//                    //    tab = "";
//                    //    for (i1 = 0; i1 < dtw1.Columns.Count; i1++)
//                    //    {
//                    //        Response.Write(tab + dr[i1].ToString());
//                    //        tab = "\t";
//                    //    }
//                    //    Response.Write("\n");

//                    //}

//                    //using (System.IO.StreamWriter OrderFile1 = new System.IO.StreamWriter(Server.MapPath("TextFileFolder/Dealer_Order_" + strDocId + ".xls"), false))
//                    //{
//                    //    var result = new StringBuilder();
//                    //    foreach (DataRow row in dtw1.Rows)
//                    //    {
//                    //        for (int i = 0; i < dtw1.Columns.Count - 1; i++)
//                    //        {
//                    //            result.Append(row[i].ToString().Trim());

//                    //            if (i == (dtw1.Columns.Count - 2))
//                    //            {
//                    //                result.AppendLine();
//                    //            }
//                    //            else
//                    //            {
//                    //                result.Append("\t");
//                    //            }

//                    //        }
//                    //    }

//                    //    OrderFile1.WriteLine(result.ToString());
//                    //    OrderFile1.Close();
//                    //}

//                    //string attachment = "attachment; filename=Dealer_Order.xls";
//                    //Response.ClearContent();
//                    //Response.AddHeader("content-disposition", attachment);
//                    //Response.ContentType = "application/vnd.ms-excel";
//                    //string tab = "";
//                    //int i1;

//                    //foreach (DataRow dr in dtw1.Rows)
//                    //{
//                    //    tab = "";
//                    //    for (i1 = 0; i1 < dtw1.Columns.Count; i1++)
//                    //    {
//                    //        Response.Write(tab + dr[i1].ToString());
//                    //        tab = "\t";
//                    //    }
//                    //    Response.Write("\n");

//                    //}

                
              
            }

        }

        public void ExportToExecl(object DS, string fileName)
        {
            fileName += ".xls";
            string worksheetName = fileName;               
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + fileName + "");
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";           
            StringWriter stringWriter = new StringWriter();
            HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWriter);

            //htmlWrite.WriteLine("<b><u><font size='4'><text-align='center'> " + "OLAM" + "</br>");
            //htmlWrite.WriteLine("<b><u><font size='3'><horizontalalign='center'> " + "CourseName" + "");
            DataGrid dataExportExcel = new DataGrid();
            //dataExportExcel.ItemDataBound += new DataGridItemEventHandler(dataExportExcel_ItemDataBound);
            dataExportExcel.DataSource = DS;
            dataExportExcel.DataBind();
            dataExportExcel.RenderControl(htmlWrite);
            
            
            StringBuilder sbResponseString = new StringBuilder();
            sbResponseString.Append("<html xmlns:v=\"urn:schemas-microsoft-com:vml\" xmlns:o=\"urn:schemas-microsoft-com:office:office\" xmlns:x=\"urn:schemas-microsoft-com:office:excel\" xmlns=\"http://www.w3.org/TR/REC-html40\"> <head><meta http-equiv=\"Content-Type\" content=\"text/html;charset=windows-1252\"><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>" + worksheetName + "</x:Name><x:WorksheetOptions><x:Panes></x:Panes></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--></head> <body>");
            sbResponseString.Append(stringWriter + "</body></html>");           
            HttpContext.Current.Response.Write(sbResponseString.ToString());
            HttpContext.Current.Response.End();
        }
       

        private void GenerateNotepadFile()
        {
            var result = new StringBuilder();

            //result = Convert.ToString("");
            //foreach (DataRow row in table.Rows)
            //{
            //    for (int i = 0; i < table.Columns.Count; i++)
            //    {
            //        result.Append(row[i].ToString());
            //        result.Append(i == table.Columns.Count - 1 ? "\n" : ",");
            //    }
            //    result.AppendLine();
            //}

            string strPath = Server.MapPath("");
            StreamWriter objWriter = new StreamWriter("C:\\test.txt", false);
            //objWriter.WriteLine(result.ToString());
            objWriter.WriteLine("abcdef");
            objWriter.Close();
        }

        private void DisabledFormData()
        {           
            txtDiscount.Attributes.Add("disabled", "disabled");
            btnApply.Attributes.Add("disabled", "disabled");
            btnConfirm.Attributes.Add("disabled", "disabled");
            Cancel.Attributes.Add("disabled", "disabled");
            btnOnHold.Attributes.Add("disabled", "disabled");

            foreach(GridViewRow gvrow in gvDetails.Rows)
            {
                DropDownList ddl = (DropDownList)gvrow.FindControl("ddlLocation");
                TextBox txtConfirmQty = (TextBox)gvrow.FindControl("txtConfirmQty");
                TextBox txtDisc = (TextBox)gvrow.FindControl("txtDiscount");
                Button btn = (Button)gvrow.FindControl("btnOK");

                ddl.Attributes.Add("disabled", "disabled");
                txtConfirmQty.Attributes.Add("disabled", "disabled");
                txtDisc.Attributes.Add("disabled", "disabled");
                btn.Attributes.Add("disabled", "disabled");
            }
        }

        protected void txtConfirmQty_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;
            GridViewRow gvrow = (GridViewRow)txt.NamingContainer;
            //Label lblPacking = (Label)gvrow.FindControl("lblPacking");
            decimal decPacking = (Convert.ToDecimal(gvrow.Cells[2].Text.Trim()));
            Label lblOriQty = (Label)gvrow.FindControl("lblOriQty");
            Label lblItemID = (Label)gvrow.FindControl("lblItemID");

            if (txt.Text == "")
            {
                txt.Focus();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Errormessage", "errormessage('Please enter confirm order quantity!');", true);
                return;
            }

            decimal ConfirmQty = Convert.ToDecimal(txt.Text.Trim());

            if(ConfirmQty!=0M)
            {
                txt.Text = lblOriQty.Text.Trim();
            }

            DataTable dtLocPO = (DataTable)Session["ItemDataPO"];

            if (ConfirmQty == 0M)
            {
                foreach (DataRow dr in dtLocPO.Rows) // search whole table
                {
                    if (dr["ItemID"].ToString() == lblItemID.Text.Trim())
                    {
                        dr.Delete();
                        break;
                    }
                }
                dtLocPO.AcceptChanges();
                Session["ItemDataPO"] = dtLocPO;

                decimal decSplitAmtTotal = 0M;
                DataTable dtSplit = (DataTable)Session["ItemDataPO"];

                if (dtSplit != null && dtSplit.Rows.Count > 0)
                {
                    for (int i = 0; i < dtSplit.Rows.Count; i++)
                    {
                        decSplitAmtTotal += Convert.ToDecimal(dtSplit.Rows[i]["Amount"]);
                    }
                }
                lblAmountTotal.Text = Convert.ToString(decSplitAmtTotal);
                //BindLocationWiseData();

                BindLocationWiseData_Modified();
            }
            else
            {
                LoadSplitedData();
            }            

            //if (Convert.ToDecimal(txt.Text) > Convert.ToDecimal(lblOrdQty.Text.Trim()))
            //{
            //    txt.Focus();
            //    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Errormessage", "errormessage('Confirm Qty should be less than or equal to Ordered Qty!');", true);
            //    return;
            //}

            //int QtyCheck = 0;
            //decimal decQtyCheck = 0M;

            //if (Convert.ToInt32(decPacking) != 0)
            //{
            //    QtyCheck = Convert.ToInt32(Math.Round(Convert.ToDecimal(txt.Text.Trim()), 0)) / Convert.ToInt32(decPacking);
            //    decQtyCheck = Convert.ToDecimal(txt.Text.Trim()) / decPacking;
            //}

            //if ((QtyCheck > 0 && decQtyCheck > 0M && QtyCheck == decQtyCheck) || Convert.ToInt32(decPacking)==0)
            //{
            //}
            //else
            //{
            //    txt.Focus();
            //    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Errormessage", "errormessage('Please enter quantity in multiple of packing!');", true);
            //}

        }

        private void SendEmailMessage(string EmailKey)
        {
            int POID = Convert.ToInt32(Session["POID"]);
            string strTP = @"select * from TransPurchOrder AS T1 WHERE T1.POrdId=" + POID + "";
            DataTable dtTP = DbConnectionDAL.GetDataTable(CommandType.Text, strTP);

            string str = @"SELECT T1.SenderEmailId,T1.SenderPassword,T1.Port,T1.PurchaseEmailId,T1.PurchaseEmailCC,
                            T1.ChangePasswordEmailId,T1.ChangePasswordCC,T1.ForgetPasswordEmailId,T1.ForgetPasswordCC,
                            T1.OrderListEmailId,T1.OrderListEmailCC,T1.MailServer
                            FROM MastEnviro AS T1";

            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (dt != null && dt.Rows.Count > 0)
            {
                string strVar = @"SELECT * FROM MastEmailVariable WHERE EmailKey='" + EmailKey + "'";
                DataTable dtVar = DbConnectionDAL.GetDataTable(CommandType.Text, strVar);
                string strEmailTemplate = @"SELECT * FROM EmailTemplate WHERE EmialKey='" + EmailKey + "'";
                DataTable dtEmailTemplate = DbConnectionDAL.GetDataTable(CommandType.Text, strEmailTemplate);

                string[] emailToAll = new string[20];
                string[] emailCCAll = new string[20];
                string emailNew = "";
                string emailCC = "";

                emailNew = dt.Rows[0]["PurchaseEmailId"].ToString();
                emailToAll = emailNew.Split(';');
                emailCC = dt.Rows[0]["PurchaseEmailCC"].ToString();
                emailCCAll = emailCC.Split(';');

                string strSubject = Convert.ToString(dtEmailTemplate.Rows[0]["Subject"]);
                string strMailBody = Convert.ToString(dtEmailTemplate.Rows[0]["TemplateValue"]);

                if (dtVar != null && dtVar.Rows.Count > 0)
                {
                    for (int j = 0; j < dtVar.Rows.Count; j++)
                    {
                        if (strSubject.Contains(Convert.ToString(dtVar.Rows[j]["Name"])))
                        {
                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{OrderId}")
                            {
                                strSubject = strSubject.Replace("{OrderId}", Convert.ToString(dtTP.Rows[0]["PODocId"]));
                            }
                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{OrderDate}")
                            {
                                strSubject = strSubject.Replace("{OrderDate}", Convert.ToDateTime(dtTP.Rows[0]["CreatedDate"]).ToShortDateString());
                            }
                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{ItemGroupCode}")
                            {
                                //string strUID = @"SELECT T2.Underid FROM TransPurchOrder1 AS T1 LEFT JOIN MastItem AS T2 ON T1.ItemId=T2.ItemId WHERE T1.POrdId=" + POID + "";
                                //DataTable dtUID = DbConnectionDAL.GetDataTable(CommandType.Text, strUID);
                                //if (dtUID != null && dtUID.Rows.Count > 0)
                                //{
                                //    string strIG = @"SELECT * FROM MastItem AS T1 WHERE T1.ItemId=" + Convert.ToInt32(dtUID.Rows[0]["Underid"]) + "";
                                //    DataTable dtIG = DbConnectionDAL.GetDataTable(CommandType.Text, strIG);
                                //    strSubject = strSubject.Replace("{ItemGroupCode}", Convert.ToString(dtIG.Rows[0]["PriceGroup"]));
                                //}
                                string strUID = @"SELECT isnull(T2.PriceGroup,'') AS PriceGroup,T2.ItemId,T1.POrd1Id FROM TransPurchOrder1 AS T1 
                                                    LEFT JOIN MastItem AS T2 ON T1.ItemId=T2.ItemId WHERE T1.POrdId=" + POID + " Order by T1.POrd1Id";
                                DataTable dtUID = DbConnectionDAL.GetDataTable(CommandType.Text, strUID);
                                strSubject = strSubject.Replace("{ItemGroupCode}", Convert.ToString(dtUID.Rows[dtUID.Rows.Count - 1]["PriceGroup"]));

                            }
                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{ItemGroupName}")
                            {
                                string strUID = @"SELECT T2.Underid FROM TransPurchOrder1 AS T1 LEFT JOIN MastItem AS T2 ON T1.ItemId=T2.ItemId WHERE T1.POrdId=" + POID + "";
                                DataTable dtUID = DbConnectionDAL.GetDataTable(CommandType.Text, strUID);
                                if (dtUID != null && dtUID.Rows.Count > 0)
                                {
                                    string strIG = @"SELECT * FROM MastItem AS T1 WHERE T1.ItemId=" + Convert.ToInt32(dtUID.Rows[0]["Underid"]) + "";
                                    DataTable dtIG = DbConnectionDAL.GetDataTable(CommandType.Text, strIG);
                                    strSubject = strSubject.Replace("{ItemGroupName}", Convert.ToString(dtIG.Rows[0]["ItemName"]));
                                }
                            }

                            string strDist = @"SELECT * FROM MastParty AS T WHERE T.PartyId=" + Convert.ToInt32(dtTP.Rows[0]["DistId"]) + "";
                            DataTable dtDist = DbConnectionDAL.GetDataTable(CommandType.Text, strDist);
                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{PartyCode}")
                            {
                                strSubject = strSubject.Replace("{PartyCode}", Convert.ToString(dtDist.Rows[0]["SyncId"]));
                            }
                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{PartyName}")
                            {
                                strSubject = strSubject.Replace("{PartyName}", Convert.ToString(dtDist.Rows[0]["PartyName"]));
                            }
                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{Add1}")
                            {
                                strSubject = strSubject.Replace("{Add1}", Convert.ToString(dtTP.Rows[0]["DispAdd1"]));
                            }
                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{Add2}")
                            {
                                strSubject = strSubject.Replace("{Add2}", Convert.ToString(dtTP.Rows[0]["DispAdd2"]));
                            }
                            string strCity = @"SELECT T.AreaName FROM MastArea AS T WHERE T.AreaId=" + Convert.ToInt32(dtTP.Rows[0]["DispCity"]) + "";

                            DataTable dtCity = DbConnectionDAL.GetDataTable(CommandType.Text, strCity);

                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{City}")
                            {
                                strSubject = strSubject.Replace("{City}", Convert.ToString(dtCity.Rows[0]["AreaName"]));
                            }
                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{Pin}")
                            {
                                strSubject = strSubject.Replace("{Pin}", Convert.ToString(dtTP.Rows[0]["DispPin"]));
                            }
                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{State}")
                            {
                                strSubject = strSubject.Replace("{State}", Convert.ToString(dtTP.Rows[0]["DispState"]));
                            }                            
                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{OrderAmount}")
                            {
                                strSubject = strSubject.Replace("{OrderAmount}", Convert.ToString(dtTP.Rows[0]["OrderValue"]));
                            }
                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{Remarks}")
                            {
                                strSubject = strSubject.Replace("{Remarks}", Convert.ToString(dtTP.Rows[0]["Remarks"]));
                            }
                        }

                        ///////////////////////////////////////////
                        if (strMailBody.Contains(Convert.ToString(dtVar.Rows[j]["Name"])))
                        {
                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{OrderId}")
                            {
                                strMailBody = strMailBody.Replace("{OrderId}", Convert.ToString(dtTP.Rows[0]["PODocId"]));
                            }
                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{OrderDate}")
                            {
                                strMailBody = strMailBody.Replace("{OrderDate}", Convert.ToDateTime(dtTP.Rows[0]["CreatedDate"]).ToShortDateString());
                            }
                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{ItemGroupCode}")
                            {
                                //string strUID = @"SELECT T2.Underid FROM TransPurchOrder1 AS T1 LEFT JOIN MastItem AS T2 ON T1.ItemId=T2.ItemId WHERE T1.POrdId=" + POID + "";
                                //DataTable dtUID = DbConnectionDAL.GetDataTable(CommandType.Text, strUID);
                                //if (dtUID != null && dtUID.Rows.Count > 0)
                                //{
                                //    string strIG = @"SELECT * FROM MastItem AS T1 WHERE T1.ItemId=" + Convert.ToInt32(dtUID.Rows[0]["Underid"]) + "";
                                //    DataTable dtIG = DbConnectionDAL.GetDataTable(CommandType.Text, strIG);
                                //    strMailBody = strMailBody.Replace("{ItemGroupCode}", Convert.ToString(dtIG.Rows[0]["PriceGroup"]));
                                //}

                                string strUID = @"SELECT isnull(T2.PriceGroup,'') AS PriceGroup,T2.ItemId,T1.POrd1Id FROM TransPurchOrder1 AS T1 
                                                    LEFT JOIN MastItem AS T2 ON T1.ItemId=T2.ItemId WHERE T1.POrdId=" + POID + " Order by T1.POrd1Id";
                                DataTable dtUID = DbConnectionDAL.GetDataTable(CommandType.Text, strUID);
                                strMailBody = strMailBody.Replace("{ItemGroupCode}", Convert.ToString(dtUID.Rows[dtUID.Rows.Count - 1]["PriceGroup"]));

                            }
                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{ItemGroupName}")
                            {
                                string strUID = @"SELECT T2.Underid FROM TransPurchOrder1 AS T1 LEFT JOIN MastItem AS T2 ON T1.ItemId=T2.ItemId WHERE T1.POrdId=" + POID + "";
                                DataTable dtUID = DbConnectionDAL.GetDataTable(CommandType.Text, strUID);
                                if (dtUID != null && dtUID.Rows.Count > 0)
                                {
                                    string strIG = @"SELECT * FROM MastItem AS T1 WHERE T1.ItemId=" + Convert.ToInt32(dtUID.Rows[0]["Underid"]) + "";
                                    DataTable dtIG = DbConnectionDAL.GetDataTable(CommandType.Text, strIG);
                                    strMailBody = strMailBody.Replace("{ItemGroupName}", Convert.ToString(dtIG.Rows[0]["ItemName"]));
                                }
                               
                            }

                            string strDist = @"SELECT * FROM MastParty AS T WHERE T.PartyId=" + Convert.ToInt32(dtTP.Rows[0]["DistId"]) + "";
                            DataTable dtDist = DbConnectionDAL.GetDataTable(CommandType.Text, strDist);
                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{PartyCode}")
                            {
                                strMailBody = strMailBody.Replace("{PartyCode}", Convert.ToString(dtDist.Rows[0]["SyncId"]));
                            }
                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{PartyName}")
                            {
                                strMailBody = strMailBody.Replace("{PartyName}", Convert.ToString(dtDist.Rows[0]["PartyName"]));
                            }
                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{Add1}")
                            {
                                strMailBody = strMailBody.Replace("{Add1}", Convert.ToString(dtTP.Rows[0]["DispAdd1"]));
                            }
                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{Add2}")
                            {
                                strMailBody = strMailBody.Replace("{Add2}", Convert.ToString(dtTP.Rows[0]["DispAdd2"]));
                            }
                            string strCity = @"SELECT T.AreaName FROM MastArea AS T WHERE T.AreaId=" + Convert.ToInt32(dtTP.Rows[0]["DispCity"]) + "";

                            DataTable dtCity = DbConnectionDAL.GetDataTable(CommandType.Text, strCity);

                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{City}")
                            {
                                strMailBody = strMailBody.Replace("{City}", Convert.ToString(dtCity.Rows[0]["AreaName"]));
                            }
                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{Pin}")
                            {
                                strMailBody = strMailBody.Replace("{Pin}", Convert.ToString(dtTP.Rows[0]["DispPin"]));
                            }
                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{State}")
                            {
                                strMailBody = strMailBody.Replace("{State}", Convert.ToString(dtTP.Rows[0]["DispState"]));
                            }
                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{OrderAmount}")
                            {
                                strMailBody = strMailBody.Replace("{OrderAmount}", Convert.ToString(dtTP.Rows[0]["OrderValue"]));
                            }
                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{Remarks}")
                            {
                                strMailBody = strMailBody.Replace("{Remarks}", Convert.ToString(dtTP.Rows[0]["Remarks"]));
                            }
                        }
                    }
                }

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(Convert.ToString(dt.Rows[0]["SenderEmailId"]));
                mail.Subject = strSubject;
                mail.Body = strMailBody;
                //mail.Attachments.Add(new Attachment(Server.MapPath("TextFileFolder/Dealer_Order.txt")));
                NetworkCredential mailAuthenticaion = new NetworkCredential(Convert.ToString(dt.Rows[0]["SenderEmailId"]), Convert.ToString(dt.Rows[0]["SenderPassword"]));

                if (emailToAll.Length > 0)
                {
                    for (int i = 0; i < emailToAll.Length; i++)
                    {
                        mail.To.Add(new MailAddress(emailToAll[i]));
                    }
                }
                if (emailCCAll.Length > 0)
                {
                    for (int j = 0; j < emailCCAll.Length; j++)
                    {
                        mail.CC.Add(new MailAddress(emailToAll[j]));
                    }
                }

                string strDistEmailID = @"SELECT * FROM MastParty AS T WHERE T.PartyId=" + Convert.ToInt32(dtTP.Rows[0]["DistId"]) + "";
                DataTable dtDistEmailID = DbConnectionDAL.GetDataTable(CommandType.Text, strDistEmailID);
                mail.To.Add(new MailAddress(Convert.ToString(dtDistEmailID.Rows[0]["Email"])));

                string strSPEmailID = @"SELECT * FROM MastSalesRep WHERE SMId=" + Convert.ToInt32(dtDistEmailID.Rows[0]["SMID"]) + "";
                //Ankita - 30/may/2016
               // DataTable dtSPEmailID = DbConnectionDAL.GetDataTable(CommandType.Text, strDistEmailID);
                DataTable dtSPEmailID = DbConnectionDAL.GetDataTable(CommandType.Text, strSPEmailID);
                mail.To.Add(new MailAddress(Convert.ToString(dtSPEmailID.Rows[0]["Email"])));

                SmtpClient mailclient = new SmtpClient(Convert.ToString(dt.Rows[0]["MailServer"]), Convert.ToInt32(dt.Rows[0]["Port"]));
                mailclient.EnableSsl = false;
                mailclient.UseDefaultCredentials = false;
                mailclient.Credentials = mailAuthenticaion;
                mail.IsBodyHtml = true;
                mailclient.Send(mail);
            }
        }

        private void SendEmailMessage_Attachment(string EmailKey)
        {
            int POID = Convert.ToInt32(Session["POID"]);
            string strTP = @"select * from TransPurchOrder AS T1 WHERE T1.POrdId=" + POID + "";
            DataTable dtTP = DbConnectionDAL.GetDataTable(CommandType.Text, strTP);

            string str = @"SELECT T1.SenderEmailId,T1.SenderPassword,T1.Port,T1.PurchaseEmailId,T1.PurchaseEmailCC,
                            T1.ChangePasswordEmailId,T1.ChangePasswordCC,T1.ForgetPasswordEmailId,T1.ForgetPasswordCC,
                            T1.OrderListEmailId,T1.OrderListEmailCC,T1.MailServer
                            FROM MastEnviro AS T1";

            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (dt != null && dt.Rows.Count > 0)
            {
                string strVar = @"SELECT * FROM MastEmailVariable WHERE EmailKey='" + EmailKey + "'";
                DataTable dtVar = DbConnectionDAL.GetDataTable(CommandType.Text, strVar);
                string strEmailTemplate = @"SELECT * FROM EmailTemplate WHERE EmialKey='" + EmailKey + "'";
                DataTable dtEmailTemplate = DbConnectionDAL.GetDataTable(CommandType.Text, strEmailTemplate);

                string[] emailToAll = new string[20];
                string[] emailCCAll = new string[20];
                string emailNew = "";
                string emailCC = "";

                emailNew = dt.Rows[0]["OrderListEmailId"].ToString();
                emailToAll = emailNew.Split(';');
                emailCC = dt.Rows[0]["OrderListEmailCC"].ToString();
                emailCCAll = emailCC.Split(';');

                string strSubject = Convert.ToString(dtEmailTemplate.Rows[0]["Subject"]);
                string strMailBody = Convert.ToString(dtEmailTemplate.Rows[0]["TemplateValue"]);

                if (dtVar != null && dtVar.Rows.Count > 0)
                {
                    for (int j = 0; j < dtVar.Rows.Count; j++)
                    {
                        if (strSubject.Contains(Convert.ToString(dtVar.Rows[j]["Name"])))
                        {
                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{OrderId}")
                            {
                                strSubject = strSubject.Replace("{OrderId}", Convert.ToString(dtTP.Rows[0]["PODocId"]));
                            }
                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{OrderDate}")
                            {
                                strSubject = strSubject.Replace("{OrderDate}", Convert.ToDateTime(dtTP.Rows[0]["CreatedDate"]).ToShortDateString());
                            }
                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{ItemGroupCode}")
                            {
                                //string strUID = @"SELECT T2.Underid,isnull(T2.PriceGroup,'') AS PriceGroup FROM TransPurchOrder1 AS T1 LEFT JOIN MastItem AS T2 ON T1.ItemId=T2.ItemId WHERE T1.POrdId=" + POID + "";
                                //DataTable dtUID = DbConnectionDAL.GetDataTable(CommandType.Text, strUID);
                                //if (dtUID != null && dtUID.Rows.Count > 0)
                                //{
                                //    string strIG = @"SELECT * FROM MastItem AS T1 WHERE T1.ItemId=" + Convert.ToInt32(dtUID.Rows[0]["Underid"]) + "";
                                //    DataTable dtIG = DbConnectionDAL.GetDataTable(CommandType.Text, strIG);
                                //    strSubject = strSubject.Replace("{ItemGroupCode}", Convert.ToString(dtIG.Rows[0]["PriceGroup"]));
                                //}

                                string strUID = @"SELECT isnull(T2.PriceGroup,'') AS PriceGroup,T2.ItemId,T1.POrd1Id FROM TransPurchOrder1 AS T1 
                                                    LEFT JOIN MastItem AS T2 ON T1.ItemId=T2.ItemId WHERE T1.POrdId=" + POID + " Order by T1.POrd1Id";
                                DataTable dtUID = DbConnectionDAL.GetDataTable(CommandType.Text, strUID);
                                strSubject = strSubject.Replace("{ItemGroupCode}", Convert.ToString(dtUID.Rows[dtUID.Rows.Count-1]["PriceGroup"]));

                            }
                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{ItemGroupName}")
                            {
                                string strUID = @"SELECT T2.Underid FROM TransPurchOrder1 AS T1 LEFT JOIN MastItem AS T2 ON T1.ItemId=T2.ItemId WHERE T1.POrdId=" + POID + "";
                                DataTable dtUID = DbConnectionDAL.GetDataTable(CommandType.Text, strUID);
                                if (dtUID != null && dtUID.Rows.Count > 0)
                                {
                                    string strIG = @"SELECT * FROM MastItem AS T1 WHERE T1.ItemId=" + Convert.ToInt32(dtUID.Rows[0]["Underid"]) + "";
                                    DataTable dtIG = DbConnectionDAL.GetDataTable(CommandType.Text, strIG);
                                    strSubject = strSubject.Replace("{ItemGroupName}", Convert.ToString(dtIG.Rows[0]["ItemName"]));
                                }
                            }

                            string strDist = @"SELECT * FROM MastParty AS T WHERE T.PartyId=" + Convert.ToInt32(dtTP.Rows[0]["DistId"]) + "";
                            DataTable dtDist = DbConnectionDAL.GetDataTable(CommandType.Text, strDist);
                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{PartyCode}")
                            {
                                strSubject = strSubject.Replace("{PartyCode}", Convert.ToString(dtDist.Rows[0]["SyncId"]));
                            }
                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{PartyName}")
                            {
                                strSubject = strSubject.Replace("{PartyName}", Convert.ToString(dtDist.Rows[0]["PartyName"]));
                            }
                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{Add1}")
                            {
                                strSubject = strSubject.Replace("{Add1}", Convert.ToString(dtTP.Rows[0]["DispAdd1"]));
                            }
                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{Add2}")
                            {
                                strSubject = strSubject.Replace("{Add2}", Convert.ToString(dtTP.Rows[0]["DispAdd2"]));
                            }
                            string strCity = @"SELECT T.AreaName FROM MastArea AS T WHERE T.AreaId=" + Convert.ToInt32(dtTP.Rows[0]["DispCity"]) + "";

                            DataTable dtCity = DbConnectionDAL.GetDataTable(CommandType.Text, strCity);

                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{City}")
                            {
                                strSubject = strSubject.Replace("{City}", Convert.ToString(dtCity.Rows[0]["AreaName"]));
                            }
                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{Pin}")
                            {
                                strSubject = strSubject.Replace("{Pin}", Convert.ToString(dtTP.Rows[0]["DispPin"]));
                            }
                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{State}")
                            {
                                strSubject = strSubject.Replace("{State}", Convert.ToString(dtTP.Rows[0]["DispState"]));
                            }
                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{OrderAmount}")
                            {
                                strSubject = strSubject.Replace("{OrderAmount}", Convert.ToString(dtTP.Rows[0]["OrderValue"]));
                            }
                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{Remarks}")
                            {
                                strSubject = strSubject.Replace("{Remarks}", Convert.ToString(dtTP.Rows[0]["Remarks"]));
                            }
                        }

                        ///////////////////////////////////////////
                        if (strMailBody.Contains(Convert.ToString(dtVar.Rows[j]["Name"])))
                        {
                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{OrderId}")
                            {
                                strMailBody = strMailBody.Replace("{OrderId}", Convert.ToString(dtTP.Rows[0]["PODocId"]));
                            }
                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{OrderDate}")
                            {
                                strMailBody = strMailBody.Replace("{OrderDate}", Convert.ToDateTime(dtTP.Rows[0]["CreatedDate"]).ToShortDateString());
                            }
                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{ItemGroupCode}")
                            {
                                //string strUID = @"SELECT T2.Underid FROM TransPurchOrder1 AS T1 LEFT JOIN MastItem AS T2 ON T1.ItemId=T2.ItemId WHERE T1.POrdId=" + POID + "";
                                //DataTable dtUID = DbConnectionDAL.GetDataTable(CommandType.Text, strUID);
                                //if (dtUID != null && dtUID.Rows.Count > 0)
                                //{
                                //    string strIG = @"SELECT * FROM MastItem AS T1 WHERE T1.ItemId=" + Convert.ToInt32(dtUID.Rows[0]["Underid"]) + "";
                                //    DataTable dtIG = DbConnectionDAL.GetDataTable(CommandType.Text, strIG);
                                //    strMailBody = strMailBody.Replace("{ItemGroupCode}", Convert.ToString(dtIG.Rows[dtIG.Rows.Count - 1]["PriceGroup"]));
                                //}

                                string strUID = @"SELECT isnull(T2.PriceGroup,'') AS PriceGroup,T2.ItemId,T1.POrd1Id FROM TransPurchOrder1 AS T1 
                                                    LEFT JOIN MastItem AS T2 ON T1.ItemId=T2.ItemId WHERE T1.POrdId=" + POID + " Order by T1.POrd1Id";
                                DataTable dtUID = DbConnectionDAL.GetDataTable(CommandType.Text, strUID);
                                strMailBody = strMailBody.Replace("{ItemGroupCode}", Convert.ToString(dtUID.Rows[dtUID.Rows.Count - 1]["PriceGroup"]));

                            }
                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{ItemGroupName}")
                            {
                                string strUID = @"SELECT T2.Underid FROM TransPurchOrder1 AS T1 LEFT JOIN MastItem AS T2 ON T1.ItemId=T2.ItemId WHERE T1.POrdId=" + POID + "";
                                DataTable dtUID = DbConnectionDAL.GetDataTable(CommandType.Text, strUID);
                                if (dtUID != null && dtUID.Rows.Count > 0)
                                {
                                    string strIG = @"SELECT * FROM MastItem AS T1 WHERE T1.ItemId=" + Convert.ToInt32(dtUID.Rows[0]["Underid"]) + "";
                                    DataTable dtIG = DbConnectionDAL.GetDataTable(CommandType.Text, strIG);
                                    strMailBody = strMailBody.Replace("{ItemGroupName}", Convert.ToString(dtIG.Rows[0]["ItemName"]));
                                }
                            }

                            string strDist = @"SELECT * FROM MastParty AS T WHERE T.PartyId=" + Convert.ToInt32(dtTP.Rows[0]["DistId"]) + "";
                            DataTable dtDist = DbConnectionDAL.GetDataTable(CommandType.Text, strDist);
                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{PartyCode}")
                            {
                                strMailBody = strMailBody.Replace("{PartyCode}", Convert.ToString(dtDist.Rows[0]["SyncId"]));
                            }
                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{PartyName}")
                            {
                                strMailBody = strMailBody.Replace("{PartyName}", Convert.ToString(dtDist.Rows[0]["PartyName"]));
                            }
                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{Add1}")
                            {
                                strMailBody = strMailBody.Replace("{Add1}", Convert.ToString(dtTP.Rows[0]["DispAdd1"]));
                            }
                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{Add2}")
                            {
                                strMailBody = strMailBody.Replace("{Add2}", Convert.ToString(dtTP.Rows[0]["DispAdd2"]));
                            }
                            string strCity = @"SELECT T.AreaName FROM MastArea AS T WHERE T.AreaId=" + Convert.ToInt32(dtTP.Rows[0]["DispCity"]) + "";

                            DataTable dtCity = DbConnectionDAL.GetDataTable(CommandType.Text, strCity);

                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{City}")
                            {
                                strMailBody = strMailBody.Replace("{City}", Convert.ToString(dtCity.Rows[0]["AreaName"]));
                            }
                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{Pin}")
                            {
                                strMailBody = strMailBody.Replace("{Pin}", Convert.ToString(dtTP.Rows[0]["DispPin"]));
                            }
                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{State}")
                            {
                                strMailBody = strMailBody.Replace("{State}", Convert.ToString(dtTP.Rows[0]["DispState"]));
                            }
                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{OrderAmount}")
                            {
                                strMailBody = strMailBody.Replace("{OrderAmount}", Convert.ToString(dtTP.Rows[0]["OrderValue"]));
                            }
                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{Remarks}")
                            {
                                strMailBody = strMailBody.Replace("{Remarks}", Convert.ToString(dtTP.Rows[0]["Remarks"]));
                            }
                        }
                    }
                }

                //string FilePathToDel = Server.MapPath("TextFileFolder/Dealer_Order.txt");

                //if (System.IO.File.Exists(FilePathToDel))
                //{
                //    System.IO.File.Delete(FilePathToDel);
                //}

                DataTable dtOrder = (DataTable)Session["ItemDataPO"];
                DataTable dtFinal = new DataTable();
                dtFinal.Columns.Add("OrderNo");
                dtFinal.Columns.Add("Company");
                dtFinal.Columns.Add("Distr");
                dtFinal.Columns.Add("DivCode");                
                dtFinal.Columns.Add("OrderType");
                dtFinal.Columns.Add("CustomerID");
                dtFinal.Columns.Add("CustomerID1");
                dtFinal.Columns.Add("PartyOrder");
                dtFinal.Columns.Add("PartyDate");
                dtFinal.Columns.Add("Location");
                dtFinal.Columns.Add("Desc");
                dtFinal.Columns.Add("ItemCode");
                dtFinal.Columns.Add("Qty");
                dtFinal.Columns.Add("BlanlSp");
                dtFinal.Columns.Add("Disc");
                dtFinal.Columns.Add("Num");

                string strDocID = Convert.ToString(dtTP.Rows[0]["PODocId"]); ;
                string strModstr = strDocID.Substring(10, 8);

                string[] arr = { "GroupCode", "LocationID" };

                DataTable dtUni = new DataTable();
                dtUni = dtOrder.DefaultView.ToTable(true, arr);
                dtOrder.Columns.Add("Num");

                if (dtUni != null && dtUni.Rows.Count > 0)
                {
                    for (int j = 0; j < dtUni.Rows.Count; j++)
                    {

                        for (int k = 0; k < dtOrder.Rows.Count; k++)
                        {
                            if ((Convert.ToInt32(dtOrder.Rows[k]["LocationID"]) == Convert.ToInt32(dtUni.Rows[j]["LocationID"]))
                    && (Convert.ToString(dtOrder.Rows[k]["GroupCode"]) == Convert.ToString(dtUni.Rows[j]["GroupCode"])))
                            {
                                dtOrder.Rows[k]["Num"] = j + 1;
                            }
                        }
                    }
                }

                for (int i = 0; i < dtOrder.Rows.Count; i++)
                {
                    string strLocDet = @"SELECT rescenid,rescenname,active,syncid,isnull(plantcode,0) AS plantcode,
                                            planttype,isnull(ordertype,'') AS ordertype,isnull(divisioncode,0) divisioncode 
                                            FROM MastResCentre WHERE ResCenId='" + Convert.ToInt32(dtOrder.Rows[i]["LocationID"]) + "'";

                    DataTable dtLocDet = DbConnectionDAL.GetDataTable(CommandType.Text, strLocDet);

                    string strDist = @"SELECT * FROM MastParty AS T WHERE T.PartyId=" + Convert.ToInt32(dtTP.Rows[0]["DistId"]) + "";
                    DataTable dtDist = DbConnectionDAL.GetDataTable(CommandType.Text, strDist);

                    string strItemCode = @"SELECT * FROM MastItem WHERE ItemId='" + Convert.ToInt32(dtOrder.Rows[i]["ItemID"]) + "'";
                    DataTable dtItemCode = DbConnectionDAL.GetDataTable(CommandType.Text, strItemCode);

                    dtFinal.Rows.Add();
                  
                    dtFinal.Rows[dtFinal.Rows.Count - 1]["OrderNo"] = "P" + Convert.ToString(dtOrder.Rows[i]["Num"]);
                    dtFinal.Rows[dtFinal.Rows.Count - 1]["Company"] = "1000";
                    dtFinal.Rows[dtFinal.Rows.Count - 1]["Distr"] = "10";
                    dtFinal.Rows[dtFinal.Rows.Count - 1]["DivCode"] = Convert.ToString(dtLocDet.Rows[0]["divisioncode"]);                    
                    dtFinal.Rows[dtFinal.Rows.Count - 1]["OrderType"] = Convert.ToString(dtLocDet.Rows[0]["ordertype"]);
                    dtFinal.Rows[dtFinal.Rows.Count - 1]["CustomerID"] = Convert.ToString(dtDist.Rows[0]["SyncId"]);
                    dtFinal.Rows[dtFinal.Rows.Count - 1]["CustomerID1"] = Convert.ToString(dtDist.Rows[0]["SyncId"]);
                    dtFinal.Rows[dtFinal.Rows.Count - 1]["PartyOrder"] = strModstr + "-" + "P" + Convert.ToString(dtOrder.Rows[i]["Num"]);
                    dtFinal.Rows[dtFinal.Rows.Count - 1]["PartyDate"] = Convert.ToDateTime(dtTP.Rows[0]["CreatedDate"]).ToShortDateString();
                    dtFinal.Rows[dtFinal.Rows.Count - 1]["Location"] = Convert.ToString(dtLocDet.Rows[0]["PlantCode"]);
                    dtFinal.Rows[dtFinal.Rows.Count - 1]["Desc"] = Convert.ToString(dtItemCode.Rows[0]["ItemName"]);
                    dtFinal.Rows[dtFinal.Rows.Count - 1]["ItemCode"] = Convert.ToString(dtItemCode.Rows[0]["SyncId"]);
                    dtFinal.Rows[dtFinal.Rows.Count - 1]["Qty"] = dtOrder.Rows[i]["ConfQty"];
                    dtFinal.Rows[dtFinal.Rows.Count - 1]["BlanlSp"] = "";
                    dtFinal.Rows[dtFinal.Rows.Count - 1]["Disc"] = dtOrder.Rows[i]["Discount"];
                    dtFinal.Rows[dtFinal.Rows.Count - 1]["Num"] = dtOrder.Rows[i]["Num"];
                }

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(Convert.ToString(dt.Rows[0]["SenderEmailId"]));
                mail.Subject = strSubject;
                mail.Body = strMailBody;
                //mail.Attachments.Add(new Attachment(Server.MapPath("TextFileFolder/Dealer_Order.txt")));
                NetworkCredential mailAuthenticaion = new NetworkCredential(Convert.ToString(dt.Rows[0]["SenderEmailId"]), Convert.ToString(dt.Rows[0]["SenderPassword"]));

                if (emailToAll.Length > 0)
                {
                    for (int i = 0; i < emailToAll.Length; i++)
                    {
                        mail.To.Add(new MailAddress(emailToAll[i]));
                    }
                }
                if (emailCCAll.Length > 0)
                {
                    for (int j = 0; j < emailCCAll.Length; j++)
                    {
                        mail.CC.Add(new MailAddress(emailToAll[j]));
                    }
                }

                string strDistEmailID = @"SELECT * FROM MastParty AS T WHERE T.PartyId=" + Convert.ToInt32(dtTP.Rows[0]["DistId"]) + "";
                DataTable dtDistEmailID = DbConnectionDAL.GetDataTable(CommandType.Text, strDistEmailID);

                string strSPEmailID = @"SELECT * FROM MastSalesRep WHERE SMId=" + Convert.ToInt32(dtDistEmailID.Rows[0]["SMID"]) + "";
                DataTable dtSPEmailID = DbConnectionDAL.GetDataTable(CommandType.Text, strDistEmailID);

                mail.To.Add(new MailAddress(Convert.ToString(dtSPEmailID.Rows[0]["Email"])));

                SmtpClient mailclient = new SmtpClient(Convert.ToString(dt.Rows[0]["MailServer"]), Convert.ToInt32(dt.Rows[0]["Port"]));
                mailclient.EnableSsl = false;
                mailclient.UseDefaultCredentials = false;
                mailclient.Credentials = mailAuthenticaion;
                mail.IsBodyHtml = true;
                mailclient.Send(mail);

                string strDocId = lblOrderNo.Text;
                strDocId = strDocId.Replace(" ", "_");

                dtFinal.DefaultView.Sort = "Num ASC";

                DataTable dtw = dtFinal.DefaultView.ToTable();

                using (System.IO.StreamWriter OrderFile = new System.IO.StreamWriter(Server.MapPath("TextFileFolder/Dealer_Order_" + strDocId + ".txt"), false))
                {
                    var result = new StringBuilder();
                    foreach (DataRow row in dtw.Rows)
                    {
                        for (int i = 0; i < dtw.Columns.Count-1; i++)
                        {
                            result.Append(row[i].ToString().Trim());

                            if (i == (dtw.Columns.Count - 2))
                            {
                                result.AppendLine();
                            }
                            else
                            {
                                result.Append("\t");
                            }

                        }
                    }

                    OrderFile.WriteLine(result.ToString());
                    OrderFile.Close();
                }

              
              
            }
        }


   protected string SetW(string mSTRING,int mLEN)
   {
       mSTRING = mSTRING.Trim().PadRight(2000);
       mSTRING=mSTRING.Substring(0, mLEN).Trim();
       //mSTRING.Trim();
       mSTRING = mSTRING.PadRight(mLEN - mSTRING.Length);
       return mSTRING;
   }
       
   protected string SetN(string mSTRING,int mLEN)
   {
       mSTRING.ToString();
       mSTRING = mSTRING.PadRight(2000);
       mSTRING =mSTRING.Substring(0, mLEN).Trim();
       //mSTRING.Trim();
       mSTRING =mSTRING.PadLeft(mLEN - mSTRING.Length);
       return mSTRING;
   }

   protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
   {
       DropDownList ddl1 = (DropDownList)sender;
       GridViewRow gvrow = (GridViewRow)ddl1.NamingContainer;
       DropDownList ddl = (DropDownList)gvrow.FindControl("ddlLocation");
       TextBox txtConfirmQty = (TextBox)gvrow.FindControl("txtConfirmQty");
       TextBox txtDiscount = (TextBox)gvrow.FindControl("txtDiscount");
       Label lblID = (Label)gvrow.FindControl("lblID");
       Label lblOrdQty = (Label)gvrow.FindControl("lblOrdQty");
       Label lblAmt = (Label)gvrow.FindControl("lblAmt");
       Label lblItemID = (Label)gvrow.FindControl("lblItemID");
       Label lblItemName = (Label)gvrow.FindControl("lblItemName");

       if (Convert.ToDecimal(lblOrdQty.Text.Trim()) == 0M)
       {
           if (Session["ItemDataPO"] != null)
           {
               DataTable dtLoc1 = (DataTable)Session["ItemDataPO"];

               foreach (DataRow dr in dtLoc1.Rows) // search whole table
               {
                   if (dr["ItemID"].ToString() == lblItemID.Text.Trim()) 
                   {
                       dr["LocationID"] = ddl1.SelectedItem.Value.Trim();
                       dr["Location"] = ddl1.SelectedItem.Text.Trim(); 
                     
                   }                 
               }

               dtLoc1.AcceptChanges();
               Session["ItemDataPO"] = dtLoc1;
           }
       }
       
       BindLocationWiseData_Modified();
   }

   protected void btnexport_Click(object sender, EventArgs e)
   {
       string strTP = @"select * from TransPurchOrder AS T1 WHERE T1.POrdId=" + Convert.ToInt32(Session["POID"]) + "";
       DataTable dtTP = DbConnectionDAL.GetDataTable(CommandType.Text, strTP);

       DataTable dtOrder1 = (DataTable)Session["ItemDataPO"];
       DataTable dtFinal1 = new DataTable();
       dtFinal1.Columns.Add("OrderNo");
       dtFinal1.Columns.Add("Company");
       dtFinal1.Columns.Add("Distr");
       dtFinal1.Columns.Add("DivCode");
       dtFinal1.Columns.Add("OrderType");
       dtFinal1.Columns.Add("CustomerID");
       dtFinal1.Columns.Add("CustomerID1");
       dtFinal1.Columns.Add("PartyOrder");
       dtFinal1.Columns.Add("PartyDate");
       dtFinal1.Columns.Add("Location");
       dtFinal1.Columns.Add("Desc");
       dtFinal1.Columns.Add("ItemCode");
       dtFinal1.Columns.Add("Qty");
       dtFinal1.Columns.Add("BlanlSp");
       dtFinal1.Columns.Add("Disc");
       //dtFinal1.Columns.Add("Num");

       string strDocID = Convert.ToString(dtTP.Rows[0]["PODocId"]); ;
       string strModstr = strDocID.Substring(10, 8);      

       for (int i = 0; i < dtOrder1.Rows.Count; i++)
       {
           string strLocDet = @"SELECT rescenid,rescenname,active,syncid,isnull(plantcode,0) AS plantcode,
                                            planttype,isnull(ordertype,'') AS ordertype,isnull(divisioncode,0) divisioncode 
                                            FROM MastResCentre WHERE ResCenId='" + Convert.ToInt32(dtOrder1.Rows[i]["LocationID"]) + "'";

           DataTable dtLocDet = DbConnectionDAL.GetDataTable(CommandType.Text, strLocDet);

           string strDist = @"SELECT * FROM MastParty AS T WHERE T.PartyId=" + Convert.ToInt32(dtTP.Rows[0]["DistId"]) + "";
           DataTable dtDist = DbConnectionDAL.GetDataTable(CommandType.Text, strDist);

           string strItemCode = @"SELECT * FROM MastItem WHERE ItemId='" + Convert.ToInt32(dtOrder1.Rows[i]["ItemID"]) + "'";
           DataTable dtItemCode = DbConnectionDAL.GetDataTable(CommandType.Text, strItemCode);

           dtFinal1.Rows.Add();

           dtFinal1.Rows[dtFinal1.Rows.Count - 1]["OrderNo"] = "P" + Convert.ToString(dtOrder1.Rows[i]["Num"]);
           dtFinal1.Rows[dtFinal1.Rows.Count - 1]["Company"] = "1000";
           dtFinal1.Rows[dtFinal1.Rows.Count - 1]["Distr"] = "10";
           dtFinal1.Rows[dtFinal1.Rows.Count - 1]["DivCode"] = Convert.ToString(dtLocDet.Rows[0]["divisioncode"]);
           dtFinal1.Rows[dtFinal1.Rows.Count - 1]["OrderType"] = Convert.ToString(dtLocDet.Rows[0]["ordertype"]);
           dtFinal1.Rows[dtFinal1.Rows.Count - 1]["CustomerID"] = Convert.ToString(dtDist.Rows[0]["SyncId"]);
           dtFinal1.Rows[dtFinal1.Rows.Count - 1]["CustomerID1"] = Convert.ToString(dtDist.Rows[0]["SyncId"]);
           dtFinal1.Rows[dtFinal1.Rows.Count - 1]["PartyOrder"] = strModstr + "-" + "P" + Convert.ToString(dtOrder1.Rows[i]["Num"]);
           dtFinal1.Rows[dtFinal1.Rows.Count - 1]["PartyDate"] = Convert.ToDateTime(dtTP.Rows[0]["CreatedDate"]).ToShortDateString();
           dtFinal1.Rows[dtFinal1.Rows.Count - 1]["Location"] = Convert.ToString(dtLocDet.Rows[0]["PlantCode"]);
           dtFinal1.Rows[dtFinal1.Rows.Count - 1]["Desc"] = Convert.ToString(dtItemCode.Rows[0]["ItemName"]);
           dtFinal1.Rows[dtFinal1.Rows.Count - 1]["ItemCode"] = Convert.ToString(dtItemCode.Rows[0]["SyncId"]);
           dtFinal1.Rows[dtFinal1.Rows.Count - 1]["Qty"] = dtOrder1.Rows[i]["ConfQty"];
           dtFinal1.Rows[dtFinal1.Rows.Count - 1]["BlanlSp"] = "";
           dtFinal1.Rows[dtFinal1.Rows.Count - 1]["Disc"] = dtOrder1.Rows[i]["Discount"];
           //dtFinal1.Rows[dtFinal1.Rows.Count - 1]["Num"] = dtOrder1.Rows[i]["Num"];
       }

       string strDocId = lblOrderNo.Text;
       strDocId = strDocId.Replace(" ", "_");
       string strFileName = "Dealer_Order_" + strDocId;
       //dtFinal1.DefaultView.Sort = "Num ASC";

       DataTable dtw1 = dtFinal1.DefaultView.ToTable();      
       string str1 = string.Empty;
       Response.ClearContent();
       Response.Buffer = true;
       Response.AddHeader("content-disposition", string.Format("attachment; filename=" + strFileName + ".xls"));
       Response.ContentType = "application/ms-excel";
       string tab = "";
       int i1;
       //DataTable dt1 = dtw1;

       foreach (DataRow dr in dtw1.Rows)
       {
           tab = "";
           for (i1 = 0; i1 < dtw1.Columns.Count; i1++)
           {
               Response.Write(tab + dr[i1].ToString());
               tab = "\t";
           }
           Response.Write("\n");
       }

       Response.End();    

   }

        
//        protected void Delete_Click(object sender, EventArgs e)
//        {
//            Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "DeleteOrder();", true);
//            ClearFormData();
//        }

//        protected void Find_Click(object sender, EventArgs e)
//        {
//            btnSave.Text = "Update";
//            Delete.Visible = true;
//            JqxGriddiv.Visible = true;
//            InputWork.Visible = false;
//            BindGridData();

//        }

//        private void BindGridData()
//        {
//            try
//            {
//                DataTable dt = new DataTable();
//                var Query = @"SELECT T1.PCOrdId, T1.POrdId, T1.PODocId,
//                                         T1.UserId, T1.SMId,T3.PartyName AS DistName,T4.SMName,
//                                         T1.DistId,Convert(varchar(10),CONVERT(date,T2.VDate,106),103) AS VDate,
//                                         T1.OrderStatus AS Status,
//                                         CASE WHEN T1.OrderStatus='C' THEN 'Canceled'
//                                         WHEN T1.OrderStatus='H' THEN 'On Hold'
//                                         WHEN T1.OrderStatus='M' THEN 'Confirmed' END AS OrderStatus
//                                        FROM dbo.TransPurchOrderConf AS T1
//                                        LEFT JOIN TransPurchOrder AS T2
//                                        ON T1.POrdId=T2.POrdId
//                                        LEFT JOIN MastParty AS T3
//                                        ON (T1.DistId = T3.PartyId AND T3.PartyDist=1)
//                                        LEFT JOIN MastSalesRep AS T4
//                                        ON T1.SMId = T4.SMId
//                                        WHERE T1.UserId=" + Convert.ToInt32(Settings.Instance.UserID)+"";
//                dt = DAL.getFromDataTable(Query);

//                if(dt!=null && dt.Rows.Count>0)
//                {
//                    gvFinal.DataSource = dt;
//                    gvFinal.DataBind();

//                }
//            }
//            catch (Exception ex)
//            {
//            }
//        }

//        protected void Back_Click(object sender, EventArgs e)
//        {
//            JqxGriddiv.Visible = false;
//            InputWork.Visible = true;
//            btnSave.Text = "Save";
//            Delete.Visible = false;
//        }

//        protected void gvFinal_PageIndexChanging(object sender, GridViewPageEventArgs e)
//        {
//            gvDetails.PageIndex = e.NewPageIndex;
//            BindGridData();
//        }
    

//        private void LoadDataForEdit(int POID,string PODocID)
//        {
//            try
//            {
//                btnSave.Text = "Update";
//                Delete.Visible = true;
//                JqxGriddiv.Visible = false;
//                InputWork.Visible = true;
//                ddlOrder.DataSource = new DataTable();
//                ddlOrder.DataBind();

//                ddlOrder.Items.Insert(0, new ListItem(PODocID, Convert.ToString(POID)));
//                ddlOrder.SelectedValue = Convert.ToString(POID);
//                ddlOrder.Enabled = false;

//                LoadData();
//                LoadGridData();

//                if(ddlStatus.SelectedValue=="M")
//                {
//                    DataTable dt = new DataTable();
//                    var Query = @"SELECT T1.Sno AS SNo,T1.POrd1Id AS ID,T1.ItemId AS ItemID,T2.ItemName,T2.StdPack AS CartonQty,
//                                   T1.Qty AS OrdQty,T1.Disc AS Discount,T3.Name AS Location,T3.Id AS LocationID,T1.Rate AS Amount,
//                                   T1.Remarks AS Remark
//                                   FROM TransPurchOrder1Conf AS T1                 
//                                   LEFT JOIN MastItem AS T2
//                                   ON T1.ItemId=T2.ItemId 
//                                   LEFT JOIN MastDepot AS T3
//                                   ON T1.Location=T3.Id 
//                                   WHERE T1.POrdId="+Convert.ToInt32(ddlOrder.SelectedValue)+" ORDER BY T1.Sno";
//                    dt = DAL.getFromDataTable(Query);
//                    Session["ItemDataPO"] = dt;

//                    if (Session["ItemDataPO"] != null)
//                    {
//                        Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "LoadUpdateGrid();", true);                        
//                        gvDetails.Enabled = false;
                       
//                    }
//                }
//                else
//                {
//                    gvDetails.Enabled = true;
//                    Session["ItemDataPO"] = null;

//                }

//            }
//            catch (Exception)
//            {
               
//            }
//        }

//        protected void gvFinal_RowDataBound(object sender, GridViewRowEventArgs e)
//        {
//            if (e.Row.RowType == DataControlRowType.DataRow)
//            {
//                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvFinal, "Select$" + e.Row.RowIndex);
//                e.Row.Attributes["style"] = "cursor:pointer";
               
//            }
//        }
        

//        protected void gvFinal_SelectedIndexChanged(object sender, EventArgs e)
//        {
//            int index = gvFinal.SelectedRow.RowIndex;
//            Label ConPOID = (Label)gvFinal.SelectedRow.Cells[0].FindControl("lblPCOrdId");
//            Label POrdId = (Label)gvFinal.SelectedRow.Cells[0].FindControl("lblPOrdId");
//            Label Status = (Label)gvFinal.SelectedRow.Cells[0].FindControl("lblStatus");
//            string PODocId = gvFinal.SelectedRow.Cells[1].Text;           
//            ddlStatus.SelectedValue = Status.Text;
//            LoadDataForEdit(Convert.ToInt32(POrdId.Text.Trim()), PODocId);
//            Session["ConPOID"] = ConPOID.Text.Trim();
//            Session["PODocId"] = PODocId;
//            Session["POID"] = POrdId.Text.Trim();
//        }

//        [WebMethod]
//        public static string OrderDelete()
//        {
//            string strDocID = Convert.ToString(HttpContext.Current.Session["PODocId"]);
//            try
//            {
//                OpeartionDataContext context1 = new OpeartionDataContext();
//                context1.Connection.Open();
//                System.Data.Common.DbTransaction trans = context1.Connection.BeginTransaction();
//                context1.Transaction = trans;

//                try
//                {
//                    #region Update Status in TransPurchOrder

//                    TransPurchOrder objTransPO = context1.TransPurchOrders.Where(x => x.POrdId == Convert.ToInt32(HttpContext.Current.Session["POID"])).Single<TransPurchOrder>();
//                    objTransPO.OrderStatus = null;
//                    context1.SubmitChanges();

//                    #endregion

//                    List<TransPurchOrderConf> objTPDel = context1.TransPurchOrderConfs.Where(x => x.PCOrdId == Convert.ToInt32(HttpContext.Current.Session["ConPOID"])).ToList<TransPurchOrderConf>();
//                    context1.TransPurchOrderConfs.DeleteAllOnSubmit(objTPDel);
//                    context1.SubmitChanges();


//                    List<TransPurchOrder1Conf> objTPCDel = context1.TransPurchOrder1Confs.Where(x => x.POrdId == Convert.ToInt32(HttpContext.Current.Session["POID"])).ToList<TransPurchOrder1Conf>();
//                    context1.TransPurchOrder1Confs.DeleteAllOnSubmit(objTPCDel);
//                    context1.SubmitChanges();


//                    msg = 0;
//                    trans.Commit();

//                }
//                catch (Exception ex)
//                {

//                    trans.Rollback();
//                    strDocID = "";

//                }
//                finally
//                {
//                    if (context1.Connection != null)
//                    {
//                        context1.Connection.Close();
//                    }
//                }
//                msg = 0;
//            }
//            catch (Exception ex)
//            {

//                msg = 1;
//                strDocID = "";
//            }
//            return strDocID;
//        }

//        protected void btnGo_Click(object sender, EventArgs e)
//        {
//            DataTable dt = new DataTable();
//            string strParty = txtDistributor.Text.Trim();
//            string strDocID = txtOrderNo.Text.Trim();
//            string strDate = HiddenField1.Value.Trim();

//            var obj = context.SP_GetConfirmPurchaseOrderDetails(strParty, strDocID, strDate).ToList();

//            if (obj != null && obj.Count > 0)
//            {
//                gvFinal.DataSource = obj;
//                gvFinal.DataBind();

//            }
//            else
//            {
//                gvFinal.DataSource = new DataTable();
//                gvFinal.DataBind();
//            }
//        }
       
    }
}