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
    public partial class CartItemDetails : System.Web.UI.Page
    {
        BAL.PurchaseOrder.PurchaseOrder dp = new BAL.PurchaseOrder.PurchaseOrder();

        protected void Page_Load(object sender, EventArgs e)
        {
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;

            if (!Page.IsPostBack)
            {
                FillDistributorDetails();
                FillData();
               
            }
        }

        private void FillDistributorDetails()
        {
            string str1 = @"SELECT T1.* FROM MastParty AS T1 WHERE PartyId=" + Convert.ToInt32(Settings.Instance.DistributorID) + "";
            DataTable obj1 = new DataTable();
            obj1 = DbConnectionDAL.GetDataTable(CommandType.Text, str1);

            DataTable dt = new DataTable();
            string Query = "SELECT DISTINCT T.countryid,T.countryName,T.stateid,T.stateName,T.cityid,T.cityName FROM ViewGeo AS T WHERE T.cityid=" + Convert.ToInt32(obj1.Rows[0]["CityId"]) + "";
            dt = DbConnectionDAL.GetDataTable(CommandType.Text, Query);

           lblDistName.Text = Convert.ToString(obj1.Rows[0]["PartyName"]);

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
                        lblOutstanding.Text = Convert.ToString(Math.Abs(Convert.ToDecimal(dtFinal.Rows[0]["Outstanding"]))) + " Cr.";
                    }
                    if (Convert.ToDecimal(dtFinal.Rows[0]["Outstanding"]) > 0)
                    {
                        lblOutstanding.Text = Convert.ToString(dtFinal.Rows[0]["Outstanding"]) + " Dr.";
                    }
                    if (Convert.ToDecimal(dtFinal.Rows[0]["Outstanding"]) == 0M)
                    {
                        lblOutstanding.Text = "0.00";
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

        private void FillData()
        {
            string str = @"SELECT * FROM DistItemDetails AS T1 
                                INNER JOIN MastItem AS T2
                                ON T1.ItemID=T2.ItemId WHERE T1.DistID=" + Convert.ToInt32(Settings.Instance.DistributorID) + " Order by T1.ItemName";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            DataTable dtItem = new DataTable();
            dtItem.Columns.Add("ID");
            dtItem.Columns.Add("ItemID");
            dtItem.Columns.Add("Price");
            dtItem.Columns.Add("Unit");
            dtItem.Columns.Add("ItemName");
            dtItem.Columns.Add("Packing");
            dtItem.Columns.Add("Qty");
            dtItem.Columns.Add("PricePerUnit");
            dtItem.Columns.Add("Rate");
            dtItem.Columns.Add("Total");
            dtItem.Columns.Add("PriceGroup");
            dtItem.Columns.Add("Remarks");

            if(dt!=null && dt.Rows.Count>0)
            {
                for(int i=0;i<dt.Rows.Count;i++)
                {
                    dtItem.Rows.Add();
                    dtItem.Rows[dtItem.Rows.Count - 1]["ID"] = dt.Rows[i]["ID"];
                    dtItem.Rows[dtItem.Rows.Count - 1]["ItemID"] = dt.Rows[i]["ItemID"];
                    dtItem.Rows[dtItem.Rows.Count - 1]["Price"] = dt.Rows[i]["Price"];
                    dtItem.Rows[dtItem.Rows.Count - 1]["Unit"] = dt.Rows[i]["Unit"];
                    dtItem.Rows[dtItem.Rows.Count - 1]["ItemName"] ="(" + dt.Rows[i]["SyncId"].ToString() + ")" + " " + dt.Rows[i]["ItemName"].ToString() + " " + "(" + dt.Rows[i]["ItemCode"].ToString() + ")"; 
                    dtItem.Rows[dtItem.Rows.Count - 1]["Packing"] = dt.Rows[i]["Packing"];
                    dtItem.Rows[dtItem.Rows.Count - 1]["Qty"] = dt.Rows[i]["Qty"];
                    dtItem.Rows[dtItem.Rows.Count - 1]["PricePerUnit"] = Convert.ToString(dt.Rows[i]["Price"]) + "/" + Convert.ToString(dt.Rows[i]["Unit"]);
                    dtItem.Rows[dtItem.Rows.Count - 1]["Total"] = dt.Rows[i]["Total"];
                    dtItem.Rows[dtItem.Rows.Count - 1]["PriceGroup"] = dt.Rows[i]["PriceGroup"];
                    dtItem.Rows[dtItem.Rows.Count - 1]["Remarks"] = dt.Rows[i]["Remarks"];
                }

                if(dtItem!=null && dtItem.Rows.Count>0)
                {
                    gvCartItemDetails.DataSource = dtItem;
                    gvCartItemDetails.DataBind();

                    decimal decAmtTotal = 0M;
                    foreach (GridViewRow grow in gvCartItemDetails.Rows)
                    {
                        Label lblAmt = (Label)grow.FindControl("lblAmt");
                        if (lblAmt.Text != "")
                        {
                            decAmtTotal += Convert.ToDecimal(lblAmt.Text);
                        }
                    }
                    Label lblAmtTotal = (Label)gvCartItemDetails.FooterRow.FindControl("lblAmtTotal");
                    lblAmtTotal.Text = Convert.ToString(Math.Round(decAmtTotal, 2));
                   
                }                
            }
            else
            {
                gvCartItemDetails.DataSource = new DataTable();
                gvCartItemDetails.DataBind();
            }
           
        }

        protected void ImgDelete_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton ImgDelete = (ImageButton)sender;
            GridViewRow grow = (GridViewRow)ImgDelete.NamingContainer;
            Label lblCartID = (Label)grow.FindControl("lblCartID");

            string confirmValue = Request.Form["confirm_value"];
            if (confirmValue == "Yes")
            {
                int retdel = dp.deleteCartItem(Convert.ToInt32(lblCartID.Text.Trim()));
                if (retdel == 1)
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Deleted Successfully');", true);
                    FillData();
                }
            }

        }

        protected void btnCheckOut_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/PlaceOrder.aspx");
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/PurchaseOrderForDistributor.aspx");
        }

    
        protected void gvCartItemDetails_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvCartItemDetails.EditIndex = e.NewEditIndex;
            FillData();

        }

        protected void gvCartItemDetails_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int QtyCheck1 = 0;
                decimal decQtyCheck1 = 0M;              
                int RowId = e.RowIndex;
                int id = Convert.ToInt32(gvCartItemDetails.DataKeys[RowId].Value);
                TextBox txtQty = (TextBox)gvCartItemDetails.Rows[RowId].FindControl("txtQty");
                Label lblPrice = (Label)gvCartItemDetails.Rows[RowId].FindControl("lblPrice");
                int qty = Convert.ToInt32(txtQty.Text);
                Label lblPacking = (Label)gvCartItemDetails.Rows[RowId].FindControl("lblPacking");
                decimal decamt = Math.Round((Convert.ToDecimal(txtQty.Text.Trim()) * Convert.ToDecimal(lblPrice.Text.Trim())), 2);
                if (Convert.ToInt32(lblPacking.Text.Trim()) != 0)
                {
                    QtyCheck1 = Convert.ToInt32(Math.Round(Convert.ToDecimal(txtQty.Text.Trim()), 0)) / Convert.ToInt32(lblPacking.Text.Trim());
                    decQtyCheck1 = Convert.ToDecimal(txtQty.Text.Trim()) / Convert.ToDecimal(lblPacking.Text.Trim());
                }

                if ((QtyCheck1 > 0 && decQtyCheck1 > 0M && QtyCheck1 == decQtyCheck1) || Convert.ToInt32(lblPacking.Text.Trim()) == 0)
                {
                    string update = "update DistItemDetails set Qty = " + qty + ", Total= " + decamt + " where Id = " + id + "";
                    DbConnectionDAL.ExecuteNonQuery(CommandType.Text, update);
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record updated successfully');", true);
                    gvCartItemDetails.EditIndex = -1;
                    FillData();

                }              
                   
               
            }
            catch (Exception ex)
            {

            }

        }

        protected void gvCartItemDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvCartItemDetails.PageIndex = e.NewPageIndex;
            FillData();
        }

        protected void txtQty_TextChanged(object sender, EventArgs e)
        {
            int QtyCheck1 = 0;
            decimal decQtyCheck1 = 0M;

            TextBox txt = (TextBox)sender;
            GridViewRow gvCartItemDetails = (GridViewRow)txt.Parent.Parent;            
            TextBox txtQty = (TextBox)gvCartItemDetails.FindControl("txtQty");
            int qty = Convert.ToInt32(txtQty.Text);
            Label lblPrice = (Label)gvCartItemDetails.FindControl("lblPrice");
            Label lblPacking = (Label)gvCartItemDetails.FindControl("lblPacking");            
            //decimal decamt = Math.Round((Convert.ToDecimal(txtQty.Text.Trim()) * Convert.ToDecimal(lblPrice.Text.Trim())), 2);   

            if (Convert.ToInt32(lblPacking.Text.Trim()) != 0)
            {
                QtyCheck1 = Convert.ToInt32(Math.Round(Convert.ToDecimal(txtQty.Text.Trim()), 0)) / Convert.ToInt32(lblPacking.Text.Trim());
                decQtyCheck1 = Convert.ToDecimal(txtQty.Text.Trim()) / Convert.ToDecimal(lblPacking.Text.Trim());
            }

            if ((QtyCheck1 > 0 && decQtyCheck1 > 0M && QtyCheck1 == decQtyCheck1) || Convert.ToInt32(lblPacking.Text.Trim()) == 0)
            {
                decimal decamt = Math.Round((Convert.ToDecimal(txtQty.Text.Trim()) * Convert.ToDecimal(lblPrice.Text.Trim())), 2);
                Label lblAmt = (Label)gvCartItemDetails.FindControl("lblAmt");
                lblAmt.Text = Convert.ToString(decamt);
                //lblAmount.Text = Convert.ToString(decamt);
                //txtRemarks.Focus();

            }
            else
            {
                int PackingQty = Convert.ToInt32(lblPacking.Text.Trim());
                int OrdQty = Convert.ToInt32(txtQty.Text.Trim());
                int EnterQty = Convert.ToInt32(txtQty.Text.Trim()) - Convert.ToInt32(lblPacking.Text.Trim());

                if (EnterQty <= 0)
                {
                    txtQty.Text = Convert.ToString(lblPacking.Text.Trim());
                }
                else
                {
                    txtQty.Text = Convert.ToString((Convert.ToInt32(lblPacking.Text.Trim()) - Convert.ToInt32(txtQty.Text.Trim()) % Convert.ToInt32(lblPacking.Text.Trim())) + Convert.ToInt32(txtQty.Text.Trim()));

                }

                decimal decamt = Math.Round((Convert.ToDecimal(txtQty.Text.Trim()) * Convert.ToDecimal(lblPrice.Text.Trim())), 2);
                Label lblAmt = (Label)gvCartItemDetails.FindControl("lblAmt");
                lblAmt.Text = Convert.ToString(decamt);
                //lblAmount.Text = Convert.ToString(decamt);
               

            }

        }

      
    }
}