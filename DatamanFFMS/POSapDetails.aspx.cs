using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DAL;
using BusinessLayer;

namespace AstralFFMS
{
    public partial class POSapDetails : System.Web.UI.Page
    {
        BAL.PurchaseOrder.PurchaseOrder dp = new BAL.PurchaseOrder.PurchaseOrder();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["PODocId"] != null)
            {
                string PODocId = Convert.ToString(Request.QueryString["PODocId"]);
                LoadDetails(PODocId);
                lblOrderSt.Text = PODocId;
            }

        }

        private void LoadDetails(string PODocId)
        {
            try
            {
                string str = @"SELECT T1.DistID,T1.LocationID,Convert(varchar(15),CONVERT(date,T1.VDate,103),106) AS VDate FROM PurchaseOrderImport AS T1 WHERE T1.PODocId='" + PODocId + "'";
                DataTable obj = new DataTable();
                obj = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                BindDistDetails(Convert.ToInt32(obj.Rows[0]["DistID"]));
                BindItemDetails(Convert.ToInt32(obj.Rows[0]["LocationID"]), PODocId);
                if (obj.Rows[0]["VDate"].ToString()!="")
                {
                    lblDate.Text = Convert.ToDateTime(Convert.ToString(obj.Rows[0]["VDate"])).ToString("dd/MMM/yyyy");
                }
                else
                {
                    lblDate.Text = "";
                }
              
            }
            catch (Exception ex)
            {
                
            }
        }

        private void BindItemDetails(int LocationID, string PODocId)
        {
            try
            {
                string str = @"SELECT T.ItemId AS ItemID,T.ItemName AS ItemName,T.StdPack,T1.Qty,
                                T1.PendingQty,T.Unit,T1.Rate,T1.ItemwiseTotal AS Amount,T.PriceGroup
                                FROM MastItem AS T
                                Right JOIN PurchaseOrderImport AS T1
                                ON T.ItemId=T1.ItemID
                                WHERE T1.PODocId='" + PODocId + "' and T1.LocationID=" + LocationID + "";
                DataTable obj = new DataTable();
                obj = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                if (obj != null && obj.Rows.Count > 0)
                {
                    gvDetails.DataSource = obj;
                    gvDetails.DataBind();

                    decimal decAmtTotal = 0M;
                    foreach (GridViewRow grow in gvDetails.Rows)
                    {
                        Label lblAmt = (Label)grow.FindControl("lblAmount");
                        if (lblAmt.Text != "")
                        {
                            decAmtTotal += Convert.ToDecimal(lblAmt.Text);
                        }
                    }
                    Label lblAmtTotal = (Label)gvDetails.FooterRow.FindControl("lblAmtTotal");
                    lblAmtTotal.Text = Convert.ToString(Math.Round(decAmtTotal, 2));
                }

                string str1 = @"SELECT T.PortalNo FROM PurchaseOrderImport AS T WHERE T.PODocId='"+PODocId+"'";
                DataTable obj1 = DbConnectionDAL.GetDataTable(CommandType.Text, str1);
                if(obj1!=null && obj1.Rows.Count>0)
                {
                    lblRefNo.Text = Convert.ToString(obj1.Rows[0]["PortalNo"]);
                }

                string str2 = @"SELECT T.ResCenName,T.PlantCode FROM MastResCentre AS T WHERE T.ResCenId="+LocationID+"";
                DataTable obj2 = DbConnectionDAL.GetDataTable(CommandType.Text, str2);
                if (obj2 != null && obj2.Rows.Count > 0)
                {
                    lblBranchCode.Text = Convert.ToString(obj2.Rows[0]["PlantCode"]);
                }
            }
            catch (Exception ex)
            {
               
            }
        }

        private void BindDistDetails(int DistID)
        {
                lblDistDetails.Text = "";
                lblCreditLimit.Text = "";
                lblOutstanding.Text = "";
                lblOpenOrders.Text = "";
                lblDate.Text = "";
                string str1 = @"SELECT T1.* FROM MastParty AS T1 WHERE T1.PartyId=" + DistID + "";
                DataTable obj1 = new DataTable();
                obj1 = DbConnectionDAL.GetDataTable(CommandType.Text, str1);
                ViewState["CityID"] = obj1.Rows[0]["CityId"];
                lblDistName.Text = Convert.ToString(obj1.Rows[0]["PartyName"]);

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
                    lblDistDetails.Text = dtFinal.Rows[0]["Address1"].ToString();

                    if (dtFinal.Rows[0]["Address2"].ToString() != "")
                    {
                        lblDistDetails.Text = dtFinal.Rows[0]["Address1"].ToString() + ", " + dtFinal.Rows[0]["Address2"].ToString();
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
    }
}