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


namespace AstralFFMS
{
    public partial class PurchaseOrderDistList : System.Web.UI.Page
    {
        BAL.PurchaseOrder.PurchaseOrder dp = new BAL.PurchaseOrder.PurchaseOrder();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                FillDistributorDetails();
                string str1 = @"select Count(*) from DistItemDetails where DistID=" + Convert.ToInt32(Settings.Instance.DistributorID) + "";
                int val1 = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, str1));
                CartItemCount.InnerHtml = Convert.ToString(val1);
                //string pageName = Path.GetFileName(Request.Path);

                //AddToCart.Enabled = Settings.Instance.CheckAddPermission(pageName, Convert.ToString(Session["user_name"]));
                //AddToCart.CssClass = "btn btn-primary";
                FillData();

            }
        }

        private void FillData()
        {
            string str1 = @"SELECT T.ItemId AS ItemID,T.ItemName ,
                                    CASE WHEN T.MRP IS NULL THEN 0 ELSE T.MRP END AS Price,
                                    CASE WHEN T.Unit IS NULL THEN '' ELSE T.Unit END AS Unit,
                                    CASE WHEN T.StdPack IS NULL THEN 0 ELSE T.StdPack END AS StdPack,
                                    CASE WHEN T.ItemCode IS NULL THEN '' ELSE T.ItemCode END AS ItemCode,
                                    CASE WHEN T.PriceGroup IS NULL THEN '' ELSE T.PriceGroup END AS PriceGroup,
                                    CASE WHEN T.SyncId IS NULL THEN '' ELSE T.SyncId END AS SyncId
                                    FROM MastItem AS T ORDER BY T.ItemName";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str1);

            if(dt!=null && dt.Rows.Count>0)
            {
                DataTable dtItem = new DataTable();
                dtItem.Columns.Add("ItemID");
                dtItem.Columns.Add("ItemName");
                dtItem.Columns.Add("Price");
                dtItem.Columns.Add("Unit");
                dtItem.Columns.Add("StdPack");
                dtItem.Columns.Add("ItemCode");
                dtItem.Columns.Add("PriceGroup");
                dtItem.Columns.Add("PricePerUnit");
               
                for(int i=0;i<dt.Rows.Count;i++)
                {
                    dtItem.Rows.Add();
                    dtItem.Rows[dtItem.Rows.Count - 1]["ItemID"] = dt.Rows[i]["ItemID"];
                    dtItem.Rows[dtItem.Rows.Count - 1]["ItemName"] = "(" + dt.Rows[i]["SyncId"].ToString() + ")" + " " + dt.Rows[i]["ItemName"].ToString() + " " + "(" + dt.Rows[i]["ItemCode"].ToString() + ")";
                    dtItem.Rows[dtItem.Rows.Count - 1]["Price"] = dt.Rows[i]["Price"];
                    dtItem.Rows[dtItem.Rows.Count - 1]["Unit"] = dt.Rows[i]["Unit"];
                    dtItem.Rows[dtItem.Rows.Count - 1]["StdPack"] = dt.Rows[i]["StdPack"];
                    dtItem.Rows[dtItem.Rows.Count - 1]["ItemCode"] = dt.Rows[i]["ItemCode"];
                    dtItem.Rows[dtItem.Rows.Count - 1]["PriceGroup"] = dt.Rows[i]["Price"] + "/" + dt.Rows[i]["Unit"];
                    dtItem.Rows[dtItem.Rows.Count - 1]["PricePerUnit"] = dt.Rows[i]["ItemID"];                   
                }

                gvCartItemDetails.DataSource = dtItem;
                gvCartItemDetails.DataBind();
            }
        }

        private void FillDistributorDetails()
        {
            string str1 = @"SELECT T1.* FROM MastParty AS T1 WHERE PartyId=" + Convert.ToInt32(Settings.Instance.DistributorID) + "";
            DataTable obj1 = new DataTable();
            obj1 = DbConnectionDAL.GetDataTable(CommandType.Text, str1);

            DataTable dt = new DataTable();
            string Query = "SELECT * FROM MastArea WHERE areaid IN(SELECT Maingrp FROM MastAreaGrp WHERE AreaId=" + Convert.ToInt32(obj1.Rows[0]["AreaId"]) + ")";
            dt = DbConnectionDAL.GetDataTable(CommandType.Text, Query);

            //lblDistName.Text = Convert.ToString(obj1.Rows[0]["PartyName"]);

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
                lblDistDetails.Text = dtFinal.Rows[0]["Address1"].ToString();

                if (dtFinal.Rows[0]["Address2"].ToString() != "")
                {
                    lblDistDetails.Text = dtFinal.Rows[0]["Address1"].ToString() + "," + dtFinal.Rows[0]["Address2"].ToString();
                }
                if (dtFinal.Rows[0]["CITY"].ToString() != "")
                {
                    lblDistDetails.Text = lblDistDetails.Text + ", " + dtFinal.Rows[0]["CITY"].ToString();
                }
                if (dtFinal.Rows[0]["Pin"].ToString() != "")
                {
                    lblDistDetails.Text = lblDistDetails.Text + "-" + dtFinal.Rows[0]["Pin"].ToString();
                }
                if (dtFinal.Rows[0]["Mobile"].ToString() != "")
                {
                    lblDistDetails.Text = lblDistDetails.Text + ", " + dtFinal.Rows[0]["Mobile"].ToString();
                }
                if (dtFinal.Rows[0]["Email"].ToString() != "")
                {
                    lblDistDetails.Text = lblDistDetails.Text + ", " + dtFinal.Rows[0]["Email"].ToString();
                }

                if (dtFinal.Rows[0]["CreditLimit"] != null || Convert.ToDecimal(dtFinal.Rows[0]["CreditLimit"]) != 0M)
                {
                    lblCreditLimit.Text = Convert.ToString(dtFinal.Rows[0]["CreditLimit"]);
                }
                else
                {
                    lblCreditLimit.Text = "0.00";
                }
                if (dtFinal.Rows[0]["Outstanding"] != null || Convert.ToDecimal(dtFinal.Rows[0]["Outstanding"]) != 0M)
                {
                    if (Convert.ToDecimal(dtFinal.Rows[0]["Outstanding"]) < 0)
                    {
                        lblOutstanding.Text = Convert.ToString(dtFinal.Rows[0]["Outstanding"]) + " Cr.";
                    }
                    if (Convert.ToDecimal(dtFinal.Rows[0]["Outstanding"]) > 0)
                    {
                        lblOutstanding.Text = Convert.ToString(dtFinal.Rows[0]["Outstanding"]) + " Dr.";
                    }

                }
                else
                {
                    lblOutstanding.Text = "0.00";
                }
                if (dtFinal.Rows[0]["OpenOrder"] != null || Convert.ToDecimal(dtFinal.Rows[0]["OpenOrder"]) != 0M)
                {
                    lblOpenOrders.Text = Convert.ToString(dtFinal.Rows[0]["OpenOrder"]);
                }
                else
                {
                    lblOpenOrders.Text = "0.00";
                }

            }
        }


        //[System.Web.Script.Services.ScriptMethod()]
        //[System.Web.Services.WebMethod]
        //public static List<string> SearchItem(string prefixText)
        //{
        //    string str = "select * FROM MastItem where (ItemName like '%" + prefixText + "%' or SyncId like '%" + prefixText + "%' or ItemCode like '%" + prefixText + "%') and ItemType='Item'";

        //    DataTable dt = new DataTable();

        //    dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
        //    List<string> Items = new List<string>();
        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem("(" + dt.Rows[i]["SyncId"].ToString() + ")" + " " + dt.Rows[i]["ItemName"].ToString() + " " + "(" + dt.Rows[i]["ItemCode"].ToString() + ")", dt.Rows[i]["ItemId"].ToString());
        //        Items.Add(item);
        //    }
        //    return Items;
        //}

        protected void AddToCart_Click(object sender, EventArgs e)
        {
            int retSave = 0;
            bool flag = true;

            foreach(GridViewRow gvrow in gvCartItemDetails.Rows)
            {
                CheckBox ckView = (CheckBox)gvrow.FindControl("ckView");
                Label lblItemID = (Label)gvrow.FindControl("lblItemID");
                Label lblPrice = (Label)gvrow.FindControl("lblPrice");
                Label lblUnit = (Label)gvrow.FindControl("lblUnit");
                Label lblItemName = (Label)gvrow.FindControl("lblItemName");
                Label lblPacking = (Label)gvrow.FindControl("lblPacking");
                Label lblAmt = (Label)gvrow.FindControl("lblAmt");
                Label lblPriceGroup = (Label)gvrow.FindControl("lblPriceGroup");
                TextBox txtQty = (TextBox)gvrow.FindControl("txtQty");

                if (ckView.Checked)
                {
                    flag = false;
                    if (txtQty.Text != "")
                    {
                        string str = @"select Count(*) from DistItemDetails where ItemId=" + Convert.ToInt32(lblItemID.Text.Trim()) + "";
                        int val = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, str));

                        if (val == 0)
                        {
                            retSave = dp.InsertItemForCart(Convert.ToInt32(lblItemID.Text.Trim()), lblItemName.Text.Trim(), Convert.ToInt32(lblPacking.Text.Trim()), Convert.ToInt32(txtQty.Text.Trim()),
                                             Convert.ToDecimal(lblPrice.Text.Trim()), lblUnit.Text.Trim(), Convert.ToDecimal(lblAmt.Text.Trim()),
                                             lblPriceGroup.Text.Trim(), "", Convert.ToInt32(Settings.Instance.UserID), Convert.ToInt32(Settings.Instance.SMID),
                                             Settings.GetUTCTime(), Convert.ToInt32(Settings.Instance.DistributorID));
                        }
                        else
                        {
                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Item Already Exist');", true);
                            return;
                        }
                    }
                    else
                    {
                        txtQty.Focus();
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Errormessage", "errormessage('Please enter quantity value!');", true);
                        return;
                    }
                }
               
            } 
            if(flag)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Errormessage", "errormessage('Please select atleast one item!');", true);
                return;
            }

            if (retSave != 0)
            {
                string str1 = @"select Count(*) from DistItemDetails where DistID=" + Convert.ToInt32(Settings.Instance.DistributorID) + "";
                int val1 = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, str1));
                CartItemCount.InnerHtml = Convert.ToString(val1);
                FillData();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Item added to Cart');", true);
             

            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error Occured');", true);
            }
        }

        //protected void AddToCart_Click(object sender, EventArgs e)
        //{
        //    if (txtItem.Text == "")
        //    {
        //        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Errormessage", "errormessage('Please select Item!');", true);
        //        return;
        //    }

        //    if (txtQty.Text == "")
        //    {
        //        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Errormessage", "errormessage('Please enter quantity!');", true);
        //        return;
        //    }


        //    int QtyCheck = 0;
        //    decimal decQtyCheck = 0M;

        //    if (Convert.ToInt32(lblPacking.Text.Trim()) != 0)
        //    {
        //        QtyCheck = Convert.ToInt32(Math.Round(Convert.ToDecimal(txtQty.Text.Trim()), 0)) / Convert.ToInt32(lblPacking.Text.Trim());
        //        decQtyCheck = Convert.ToDecimal(txtQty.Text.Trim()) / Convert.ToDecimal(lblPacking.Text.Trim());
        //    }

        //    if ((QtyCheck > 0 && decQtyCheck > 0M && QtyCheck == decQtyCheck) || Convert.ToInt32(lblPacking.Text.Trim()) == 0)
        //    {
        //        string str = @"select Count(*) from DistItemDetails where ItemId=" + Convert.ToInt32(hfItemId.Value.Trim()) + "";
        //        int val = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, str));

        //        if (val == 0)
        //        {
        //            int retSave = 0;
        //            retSave = dp.InsertItemForCart(Convert.ToInt32(hfItemId.Value), txtItem.Text.Trim(), Convert.ToInt32(lblPacking.Text.Trim()), Convert.ToInt32(txtQty.Text.Trim()),
        //                                            Convert.ToDecimal(lblRate1.Text.Trim()), lblUnit1.Text.Trim(), Convert.ToDecimal(lblAmount.Text.Trim()),
        //                                            lblPG.Text.Trim(), txtRemarks.Text.Trim(), Convert.ToInt32(Settings.Instance.UserID), Convert.ToInt32(Settings.Instance.SMID), Settings.GetUTCTime(), Convert.ToInt32(Settings.Instance.DistributorID));

        //            if (retSave != 0)
        //            {
        //                string str1 = @"select Count(*) from DistItemDetails where DistID=" + Convert.ToInt32(Settings.Instance.DistributorID) + "";
        //                int val1 = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, str1));
        //                CartItemCount.InnerHtml = Convert.ToString(val1);

        //                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Item added to Cart');", true);
        //                ClearControls();

        //            }
        //            else
        //            {
        //                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error Occured');", true);
        //            }
        //        }
        //        else
        //        {
        //            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Item Already Exist');", true);
        //            txtItem.Focus();
        //        }

        //        txtRemarks.Focus();

        //    }
        //    else
        //    {
        //        txtQty.Focus();
        //        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Errormessage", "errormessage('Please enter quantity in multiple of packing!');", true);
        //    }



        //}    

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/PurchaseOrderDistList.aspx");
        }

        protected void txtQty_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;
            GridViewRow gvrow = (GridViewRow)txt.NamingContainer;
            Label lblUnit = (Label)gvrow.FindControl("lblUnit");
            Label lblRate = (Label)gvrow.FindControl("lblPrice");
            Label lblAmount = (Label)gvrow.FindControl("lblAmt");
            Label lblPacking = (Label)gvrow.FindControl("lblPacking");

            if (txt.Text != "")
            {
                int QtyCheck = 0;
                decimal decQtyCheck = 0M;

                if (Convert.ToInt32(lblPacking.Text.Trim()) != 0)
                {
                    QtyCheck = Convert.ToInt32(Math.Round(Convert.ToDecimal(txt.Text.Trim()), 0)) / Convert.ToInt32(lblPacking.Text.Trim());
                    decQtyCheck = Convert.ToDecimal(txt.Text.Trim()) / Convert.ToDecimal(lblPacking.Text.Trim());
                }

                if ((QtyCheck > 0 && decQtyCheck > 0M && QtyCheck == decQtyCheck) || Convert.ToInt32(lblPacking.Text.Trim()) == 0)
                {
                    decimal decamt = Math.Round((Convert.ToDecimal(txt.Text.Trim()) * Convert.ToDecimal(lblRate.Text.Trim())), 2);
                    lblAmount.Text = Convert.ToString(decamt);

                    decimal decAmtTotal = 0M;
                    foreach (GridViewRow grow in gvCartItemDetails.Rows)
                    {
                        Label lblAmt = (Label)grow.FindControl("lblAmt");
                        TextBox txtQty = (TextBox)grow.FindControl("txtQty");

                        if (lblAmt.Text != "")
                        {
                            decAmtTotal += Convert.ToDecimal(lblAmt.Text);
                        }
                        //txtQty.Enabled = true;
                        txtQty.Attributes.Remove("readonly");
                     
                    }
                    Label lblAmtTotal = (Label)gvCartItemDetails.FooterRow.FindControl("lblAmtTotal");
                    lblAmtTotal.Text = Convert.ToString(Math.Round(decAmtTotal, 2));


                }
                else
                {
                    txt.Focus();

                    foreach (GridViewRow grow in gvCartItemDetails.Rows)
                    {
                        TextBox txtQty = (TextBox)grow.FindControl("txtQty");
                        if(grow!=gvrow)
                        {
                            txtQty.Attributes.Add("readonly", "readonly");
                        }
                        else
                        {
                            txt.Enabled = true;
                        }
                       
                    }
                   // txt.Enabled = true;
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Errormessage", "errormessage('Please enter quantity in multiple of packing!');", true);
                }
            }
            else
            {
                txt.Focus();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Errormessage", "errormessage('Please enter quantity value!');", true);
            }


        }
       
        protected void btnFind_Click(object sender, EventArgs e)
        {
            Response.Redirect("CartItemDetails.aspx");
        }

        protected void gvCartItemDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvCartItemDetails.PageIndex = e.NewPageIndex;
            FillData();
        }
    }
}