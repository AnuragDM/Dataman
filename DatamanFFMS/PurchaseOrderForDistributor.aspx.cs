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

namespace AstralFFMS
{
    public partial class PurchaseOrderForDistributor : System.Web.UI.Page
    {
        BAL.PurchaseOrder.PurchaseOrder dp = new BAL.PurchaseOrder.PurchaseOrder();

        protected void Page_Load(object sender, EventArgs e)
        {
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;

            if(!Page.IsPostBack)
            {
                FillDistributorDetails();
                string str1 = @"select Count(*) from DistItemDetails where DistID=" + Convert.ToInt32(Settings.Instance.DistributorID) + "";
                int val1 = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, str1));
                CartItemCount.InnerHtml = Convert.ToString(val1);
                //string pageName = Path.GetFileName(Request.Path);
                
                //    AddToCart.Enabled = Settings.Instance.CheckAddPermission(pageName, Convert.ToString(Session["user_name"]));
                //    AddToCart.CssClass = "btn btn-primary";
               
              
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

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> SearchItem(string prefixText)
        {//Ankita - 20/may/2016- (For Optimization)
            //string str = "select Top 500 * FROM MastItem where (ItemName like '%" + prefixText + "%' or SyncId like '%" + prefixText + "%' or ItemCode like '%" + prefixText + "%') and ItemType='Item' and Active=1";
            string str = "select Top 500 SyncId,ItemName,ItemCode,ItemId FROM MastItem where (ItemName like '%" + prefixText + "%' or SyncId like '%" + prefixText + "%' or ItemCode like '%" + prefixText + "%') and ItemType='Item' and Active=1";
            DataTable dt = new DataTable();

            dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            List<string> Items = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem("(" + dt.Rows[i]["SyncId"].ToString() + ")" + " " + dt.Rows[i]["ItemName"].ToString() + " " + "(" + dt.Rows[i]["ItemCode"].ToString() + ")", dt.Rows[i]["ItemId"].ToString());
                Items.Add(item);                
            }
            return Items;
        }

        protected void AddToCart_Click(object sender, EventArgs e)
        {
            if(txtItem.Text=="")
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Errormessage", "errormessage('Please select Product!');", true);
                return;
            }

            if (txtQty.Text == "")
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Errormessage", "errormessage('Please enter quantity!');", true);
                return;
            }


            int QtyCheck = 0;
            decimal decQtyCheck = 0M;

            if (Convert.ToInt32(lblPacking.Text.Trim()) != 0)
            {
                QtyCheck = Convert.ToInt32(Math.Round(Convert.ToDecimal(txtQty.Text.Trim()), 0)) / Convert.ToInt32(lblPacking.Text.Trim());
                decQtyCheck = Convert.ToDecimal(txtQty.Text.Trim()) / Convert.ToDecimal(lblPacking.Text.Trim());
            }

            if ((QtyCheck > 0 && decQtyCheck > 0M && QtyCheck == decQtyCheck) || Convert.ToInt32(lblPacking.Text.Trim()) == 0)
            {
                string str = @"select Count(*) from DistItemDetails where ItemId=" + Convert.ToInt32(hfItemId.Value.Trim()) + " and DistID="+Convert.ToInt32(Settings.Instance.DistributorID)+"";
                int val = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, str));

                if (val == 0)
                {
                    int retSave = 0;
                    retSave = dp.InsertItemForCart(Convert.ToInt32(hfItemId.Value), txtItem.Text.Trim(), Convert.ToInt32(lblPacking.Text.Trim()), Convert.ToInt32(txtQty.Text.Trim()),
                                                    Convert.ToDecimal(lblRate1.Text.Trim()), lblUnit1.Text.Trim(), Convert.ToDecimal(lblAmount.Text.Trim()),
                                                    lblPG.Text.Trim(), txtRemarks.Text.Trim(), Convert.ToInt32(Settings.Instance.UserID), Convert.ToInt32(Settings.Instance.SMID), Settings.GetUTCTime(), Convert.ToInt32(Settings.Instance.DistributorID));

                    if (retSave != 0)
                    {
                        string str1 = @"select Count(*) from DistItemDetails where DistID=" + Convert.ToInt32(Settings.Instance.DistributorID) + "";
                        int val1 = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, str1));
                        CartItemCount.InnerHtml = Convert.ToString(val1);

                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Product added to Cart');", true);
                        ClearControls();

                    }
                    else
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error Occured');", true);
                    }
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Product Already Exist');", true);
                    txtItem.Focus();
                }
             
                txtRemarks.Focus();

            }
            else
            {
                txtQty.Focus();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Errormessage", "errormessage('Please enter quantity in multiple of packing!');", true);
            }

           

        }

        private void ClearControls()
        {
            hfItemId.Value = "";
            txtItem.Text = "";
            lblPacking.Text = "";
            txtQty.Text = "";
            lblRate1.Text = "";
            lblUnit1.Text = "";
            lblAmount.Text = "";
            lblPG.Text = "";
            txtRemarks.Text = "";
            txtPricePerUnit.Text = "";
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearControls();
        }

        protected void txtItem_TextChanged(object sender, EventArgs e)
        {
            if (hfItemId.Value != "")
            {
                if (txtItem.Text != "")
                {

                    string str1 = @"SELECT T1.ItemId,T1.ItemName,T1.Unit,T1.MRP,T1.StdPack,T1.PriceGroup  
                                    FROM MastItem AS T1 WHERE T1.ItemId=" + Convert.ToInt32(hfItemId.Value) + "";
                    DataTable obj1 = new DataTable();
                    obj1 = DbConnectionDAL.GetDataTable(CommandType.Text, str1);

                    if (obj1 != null && obj1.Rows.Count > 0)
                    {
                        lblUnit1.Text = Convert.ToString(obj1.Rows[0]["Unit"]);
                        lblRate1.Text = Convert.ToString(obj1.Rows[0]["MRP"]);
                        lblPacking.Text = Convert.ToString(Math.Round((Convert.ToDecimal(obj1.Rows[0]["StdPack"])), 0));
                        lblPG.Text = Convert.ToString(obj1.Rows[0]["PriceGroup"]);
                        txtPricePerUnit.Text = Convert.ToString(obj1.Rows[0]["MRP"]) + "/" + Convert.ToString(obj1.Rows[0]["Unit"]);
                        txtQty.Text = "";
                        txtQty.Focus();
                    }

                }
                else
                {
                    hfItemId.Value = "";
                    txtItem.Focus();
                }
            }

        }

        protected void txtQty_TextChanged(object sender, EventArgs e)
        {           
            if (lblRate1.Text != "")
            {
                if (txtQty.Text != "")
                {
                    int QtyCheck = 0;
                    decimal decQtyCheck = 0M;

                    if (Convert.ToInt32(lblPacking.Text.Trim()) != 0)
                    {
                        QtyCheck = Convert.ToInt32(Math.Round(Convert.ToDecimal(txtQty.Text.Trim()), 0)) / Convert.ToInt32(lblPacking.Text.Trim());
                        decQtyCheck = Convert.ToDecimal(txtQty.Text.Trim()) / Convert.ToDecimal(lblPacking.Text.Trim());
                    }

                    if ((QtyCheck > 0 && decQtyCheck > 0M && QtyCheck == decQtyCheck) || Convert.ToInt32(lblPacking.Text.Trim()) == 0)
                    {
                        decimal decamt = Math.Round((Convert.ToDecimal(txtQty.Text.Trim()) * Convert.ToDecimal(lblRate1.Text.Trim())), 2);
                        lblAmount.Text = Convert.ToString(decamt);                      
                        txtRemarks.Focus();

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
                            //return (10 - toRound % 10) + toRound;
                        }

                        decimal decamt = Math.Round((Convert.ToDecimal(txtQty.Text.Trim()) * Convert.ToDecimal(lblRate1.Text.Trim())), 2);
                        lblAmount.Text = Convert.ToString(decamt);
                        txtRemarks.Focus();
                        //txtQty.Focus();
                        //System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Errormessage", "errormessage('Please enter quantity in multiple of packing!');", true);
                    }
                }
                else
                {
                    txtQty.Focus();
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Errormessage", "errormessage('Please enter quantity value!');", true);
                }
            }
            else
            {
                txtItem.Focus();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Errormessage", "errormessage('Please select Product!');", true);
            }

        }

        protected void btnFind_Click(object sender, EventArgs e)
        {
            Response.Redirect("CartItemDetails.aspx");
        }
    }
}