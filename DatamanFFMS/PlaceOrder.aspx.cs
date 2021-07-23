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


namespace AstralFFMS
{
    public partial class PlaceOrder : System.Web.UI.Page
    {
        BAL.PurchaseOrder.PurchaseOrder dp = new BAL.PurchaseOrder.PurchaseOrder();
        string parameter = "";
        string VisitorsIPAddr = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            parameter = Request["__EVENTARGUMENT"];
            if (parameter != "")
            {
                ViewState["POID"] = parameter;
                ViewState["POIDDel"] = parameter;
                FillPOData(Convert.ToInt32(parameter));
                BindProjects();
                mainDiv.Style.Add("display", "block");
                rptmain.Style.Add("display", "none");
            }
            //string pageName = Path.GetFileName(Request.Path);
            //btnPlaceOrder.Enabled = Settings.Instance.CheckAddPermission(pageName, Convert.ToString(Session["user_name"]));
            //btnPlaceOrder.CssClass = "btn btn-primary";


            if (!Page.IsPostBack)
            {
                Session["ItemDataPlaceOrder"] = null;
                FillDistributorDetails();
                FillData();
                BindTransPorter();
                BindCity();
                FillDispatchToDetails();
                BindSchemes();
                BindProjects();

                Bindtaxform();
            }
        }
        private void Bindtaxform()
        {
            try
            {
                string str = @"SELECT Id,Form_Desc As Name FROM Taxforms ORDER BY Form_Desc";
                DataTable obj = new DataTable();

                obj = DbConnectionDAL.GetDataTable(CommandType.Text, str);

                if (obj != null && obj.Rows.Count > 0)
                {
                    ddltaxform.DataSource = obj;
                    ddltaxform.DataTextField = "Name";
                    ddltaxform.DataValueField = "Id";
                    ddltaxform.DataBind();

                }
                ddltaxform.Items.Insert(0, new ListItem("--Select--", "0"));

            }
            catch (Exception ex)
            {

            }
        }
        private void BindProjects()
        {
            try
            {
                string str = @"SELECT Id,Name FROM MastProject WHERE Active=1 ORDER BY Name";
                DataTable obj = new DataTable();

                obj = DbConnectionDAL.GetDataTable(CommandType.Text, str);

                if (obj != null && obj.Rows.Count > 0)
                {
                    ddlProject.DataSource = obj;
                    ddlProject.DataTextField = "Name";
                    ddlProject.DataValueField = "Id";
                    ddlProject.DataBind();

                }
                ddlProject.Items.Insert(0, new ListItem("--Select--", "0"));

            }
            catch (Exception ex)
            {

            }
        }


        private void BindSchemes()
        {
            try
            {
                string str = @"SELECT Id,Name FROM MastScheme WHERE Active=1";
                DataTable obj = new DataTable();

                obj = DbConnectionDAL.GetDataTable(CommandType.Text, str);

                if (obj != null && obj.Rows.Count > 0)
                {
                    ddlScheme.DataSource = obj;
                    ddlScheme.DataTextField = "Name";
                    ddlScheme.DataValueField = "Id";
                    ddlScheme.DataBind();

                }
                ddlScheme.Items.Insert(0, new ListItem("--Select--", "0"));

            }
            catch (Exception ex)
            {

            }
        }

        private void FillPOData(int POID)
        {
            string str = @"select * from TransPurchOrder T1 INNER JOIN MastParty T2 on T1.DistId=T2.PartyId where T1.POrdId=" + POID + "";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);

            if (dt != null && dt.Rows.Count > 0)
            {
                BindTransPorter();
                if (Convert.ToString(dt.Rows[0]["Transporter"]).Trim() != "--Select--")
                {
                    ddlTransporter.SelectedValue = Convert.ToString(dt.Rows[0]["Transporter"]);
                }
                else
                {
                    ddlTransporter.SelectedValue = "0";
                }
                lblOrderSt.Text = Convert.ToString(dt.Rows[0]["PODocId"]);
                lblDate.Text = (Settings.GetUTCTime()).ToString("dd/MMM/yyyy");
                //BindCountry();                
                DispName.Text = Convert.ToString(dt.Rows[0]["DispName"]);
                DispAdd1.Text = Convert.ToString(dt.Rows[0]["DispAdd1"]);
                DispAdd2.Text = Convert.ToString(dt.Rows[0]["DispAdd2"]);
                Pin.Text = Convert.ToString(dt.Rows[0]["DispPin"]);
                Phone.Text = Convert.ToString(dt.Rows[0]["DispPhone"]);
                Mobile.Text = Convert.ToString(dt.Rows[0]["DispMobile"]);
                Email.Text = Convert.ToString(dt.Rows[0]["DispEmail"]);
                txtCountry.Text = Convert.ToString(dt.Rows[0]["DispCountry"]);
                txtState.Text = Convert.ToString(dt.Rows[0]["DispState"]);
                BindCity();
                try
                {
                    ddlCity.SelectedValue = Convert.ToString(dt.Rows[0]["DispCity"]);
                }
                catch (Exception ex)
                {
                    ddlCity.SelectedValue = "0";
                }

                BindSchemes();
                try
                {
                    ddlScheme.SelectedValue = Convert.ToString(dt.Rows[0]["SchemeID"]);
                }
                catch (Exception ex)
                {
                    ddlScheme.SelectedValue = "0";
                }


                FillDetails(Convert.ToInt32(dt.Rows[0]["DistId"]));
                lblDate.Text = (Convert.ToDateTime(dt.Rows[0]["CreatedDate"])).ToString("dd/MMM/yyyy");
                Remarks.Text = Convert.ToString(dt.Rows[0]["Remarks"]);

                if (Convert.ToString(dt.Rows[0]["OrderStatus"]) == "P")
                {
                    CancelOrder.Visible = true;

                }
                else
                {
                    CancelOrder.Visible = false;

                }
                btnPlaceOrder.Attributes.Add("disabled", "disabled");

                if (Convert.ToString(dt.Rows[0]["ProjectType"]) == "N")
                {
                    ProjectType.Items[0].Selected = true;
                    ProjectDiv.Visible = false;
                    //txtProject.Text = "";
                    ddlProject.SelectedValue = "0";
                }
                if (Convert.ToString(dt.Rows[0]["ProjectType"]) == "P")
                {
                    string strProject = @"select T1.Name from MastProject T1 where T1.Id=" + Convert.ToInt32(dt.Rows[0]["ProjectID"]) + " ORDER BY T1.Name";
                    DataTable dtProject = DbConnectionDAL.GetDataTable(CommandType.Text, strProject);

                    ProjectType.Items[1].Selected = true;
                    ProjectDiv.Visible = true;
                    ddlProject.SelectedValue = Convert.ToString(dt.Rows[0]["ProjectID"]);
                    //txtProject.Text = Convert.ToString(dtProject.Rows[0]["Name"]);
                    //hfProjectID.Value = Convert.ToString(dt.Rows[0]["ProjectID"]);
                }

            }

            string str3 = @"select * from TransPurchOrder1 T1 
                            INNER JOIN MastItem T2
                            ON T1.ItemId=T2.ItemId where T1.POrdId=" + POID + "";

            DataTable dt1 = DbConnectionDAL.GetDataTable(CommandType.Text, str3);

            if (dt1 != null && dt1.Rows.Count > 0)
            {
                DataTable dtItem = new DataTable();
                dtItem.Columns.Add("ID");
                dtItem.Columns.Add("ItemID");
                dtItem.Columns.Add("ItemName");
                dtItem.Columns.Add("Qty");
                dtItem.Columns.Add("Unit");
                dtItem.Columns.Add("Total");
                dtItem.Columns.Add("Price");
                dtItem.Columns.Add("Remarks");
                dtItem.Columns.Add("Packing");
                dtItem.Columns.Add("PriceGroup");
                dtItem.Columns.Add("PricePerUnit");

                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    dtItem.Rows.Add();
                    dtItem.Rows[dtItem.Rows.Count - 1]["ID"] = 0;
                    dtItem.Rows[dtItem.Rows.Count - 1]["ItemID"] = dt1.Rows[i]["ItemId"];
                    dtItem.Rows[dtItem.Rows.Count - 1]["ItemName"] = "(" + dt1.Rows[i]["SyncId"].ToString() + ")" + " " + dt1.Rows[i]["ItemName"].ToString() + " " + "(" + dt1.Rows[i]["ItemCode"].ToString() + ")";
                    dtItem.Rows[dtItem.Rows.Count - 1]["Qty"] = dt1.Rows[i]["Qty"];
                    dtItem.Rows[dtItem.Rows.Count - 1]["Unit"] = dt1.Rows[i]["Unit"];
                    dtItem.Rows[dtItem.Rows.Count - 1]["Price"] = dt1.Rows[i]["Rate"];
                    dtItem.Rows[dtItem.Rows.Count - 1]["Total"] = Math.Round((Convert.ToDecimal(dt1.Rows[i]["Qty"]) * Convert.ToDecimal(dt1.Rows[i]["MRP"])), 2);
                    dtItem.Rows[dtItem.Rows.Count - 1]["Remarks"] = dt1.Rows[i]["Remarks"];
                    dtItem.Rows[dtItem.Rows.Count - 1]["Packing"] = dt1.Rows[i]["StdPack"];
                    dtItem.Rows[dtItem.Rows.Count - 1]["PriceGroup"] = dt1.Rows[i]["PriceGroup"];
                    dtItem.Rows[dtItem.Rows.Count - 1]["PricePerUnit"] = dt1.Rows[i]["Rate"] + "/" + dt1.Rows[i]["Unit"];
                }

                if (dtItem != null && dtItem.Rows.Count > 0)
                {
                    gvCartItemDetails.DataSource = dtItem;
                    gvCartItemDetails.DataBind();
                }

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

        private void FillDetails(int DistID)
        {
            string str1 = @"SELECT T1.* FROM MastParty AS T1 WHERE T1.PartyId=" + DistID + "";
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

        private void FillDispatchToDetails()
        {
            chkDispatchTo.Checked = true;
            string str1 = @"SELECT T1.* FROM MastParty AS T1 WHERE T1.PartyId=" + Convert.ToInt32(Settings.Instance.DistributorID) + "";
            DataTable obj1 = new DataTable();
            obj1 = DbConnectionDAL.GetDataTable(CommandType.Text, str1);

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
            dtFinal.Rows[0]["CITY"] = dt.Rows[0]["cityid"];
            dtFinal.Rows[0]["STATE"] = dt.Rows[0]["stateName"];
            dtFinal.Rows[0]["COUNTRY"] = dt.Rows[0]["countryName"];
            dtFinal.Rows[0]["Pin"] = obj1.Rows[0]["Pin"];
            dtFinal.Rows[0]["Mobile"] = obj1.Rows[0]["Mobile"];
            dtFinal.Rows[0]["Email"] = obj1.Rows[0]["Email"];


            if (dtFinal != null && dtFinal.Rows.Count > 0)
            {
                DispName.Text = Convert.ToString(obj1.Rows[0]["PartyName"]);
                DispAdd1.Text = Convert.ToString(dtFinal.Rows[0]["Address1"]);
                DispAdd2.Text = Convert.ToString(dtFinal.Rows[0]["Address2"]);
                Pin.Text = Convert.ToString(dtFinal.Rows[0]["Pin"]);
                //Phone.Text = Convert.ToString(dtFinal.Rows[0]["Phone"]);
                Mobile.Text = Convert.ToString(dtFinal.Rows[0]["Mobile"]);
                Email.Text = Convert.ToString(dtFinal.Rows[0]["Email"]);
                BindCity();
                try
                {
                    ddlCity.SelectedValue = Convert.ToString(dtFinal.Rows[0]["CITY"]);
                }
                catch (Exception ex)
                {

                    ddlCity.SelectedValue = "0";
                }

                txtState.Text = Convert.ToString(dtFinal.Rows[0]["STATE"]);
                txtCountry.Text = Convert.ToString(dtFinal.Rows[0]["COUNTRY"]);

            }
        }

        private void FillDistributorDetails()
        {
            string str1 = @"SELECT T1.* FROM MastParty AS T1 WHERE T1.PartyId=" + Convert.ToInt32(Settings.Instance.DistributorID) + "";
            DataTable obj1 = new DataTable();
            obj1 = DbConnectionDAL.GetDataTable(CommandType.Text, str1);

            DataTable dt = new DataTable();
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
string Query = "SELECT DISTINCT T.countryid,T.countryName,T.stateid,T.stateName,T.cityid,T.cityName FROM ViewGeo AS T WHERE T.cityid=" + Convert.ToInt32(obj1.Rows[0]["CityId"]) + "";
            dt = DbConnectionDAL.GetDataTable(CommandType.Text, Query);

            lblDistName.Text = Convert.ToString(obj1.Rows[0]["PartyName"]);
            lblOrderSt.Text = "New";
            lblDate.Text = (Settings.GetUTCTime()).ToString("dd/MMM/yyyy");
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

        private void BindCity()
        {
            try
            {
                string str = @"SELECT DISTINCT cityid,cityName FROM ViewGeo  where 
                                    cityid!=0 and cityName!='' and CityId in 
                                    (SELECT cityid FROM MastParty WHERE PartyId=" + Convert.ToInt32(Settings.Instance.DistributorID) + " AND Active=1)";

                DataTable obj = new DataTable();

                obj = DbConnectionDAL.GetDataTable(CommandType.Text, str);

                if (obj != null && obj.Rows.Count > 0)
                {
                    ddlCity.DataSource = obj;
                    ddlCity.DataTextField = "cityName";
                    ddlCity.DataValueField = "cityid";
                    ddlCity.DataBind();

                }
                ddlCity.Items.Insert(0, new ListItem("--Select--", "0"));

            }
            catch (Exception ex)
            {

            }
        }

        private void BindTransPorter()
        {
            try
            {
                string str = @"SELECT T1.Name,T1.Id FROM MastTransporter AS T1 ORDER BY T1.Name";
                DataTable obj = new DataTable();
                obj = DbConnectionDAL.GetDataTable(CommandType.Text, str);

                if (obj != null && obj.Rows.Count > 0)
                {
                    ddlTransporter.DataSource = obj;
                    ddlTransporter.DataTextField = "Name";
                    ddlTransporter.DataValueField = "Name";
                    ddlTransporter.DataBind();
                    ddlTransporter.Items.Insert(0, new ListItem("--Select--", "0"));
                    ddlTransporter.Items.Insert(ddlTransporter.Items.Count, new ListItem("Other", "Other"));
                }
                else
                {
                    ddlTransporter.Items.Insert(0, new ListItem("--Select--", "0"));
                    ddlTransporter.Items.Insert(1, new ListItem("Other", "Other"));
                }

            }
            catch (Exception ex)
            {

            }
        }

        protected void ddlTransporter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlTransporter.SelectedItem.Value == "Other")
            {
                TP.Visible = true;
            }
            else
            {
                TP.Visible = false;
            }
        }

        protected void chkDispatchTo_CheckedChanged(object sender, EventArgs e)
        {

            if (chkDispatchTo.Checked)
            {
                string str1 = @"SELECT T1.* FROM MastParty AS T1 WHERE T1.PartyId=" + Convert.ToInt32(Settings.Instance.DistributorID) + "";
                DataTable obj1 = new DataTable();
                obj1 = DbConnectionDAL.GetDataTable(CommandType.Text, str1);

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

                dtFinal.Rows[0]["CITY"] = dt.Rows[0]["cityid"];
                dtFinal.Rows[0]["STATE"] = dt.Rows[0]["stateName"];
                dtFinal.Rows[0]["COUNTRY"] = dt.Rows[0]["countryName"];

                dtFinal.Rows[0]["Pin"] = obj1.Rows[0]["Pin"];
                dtFinal.Rows[0]["Mobile"] = obj1.Rows[0]["Mobile"];
                dtFinal.Rows[0]["Email"] = obj1.Rows[0]["Email"];


                if (dtFinal != null && dtFinal.Rows.Count > 0)
                {
                    DispName.Text = Convert.ToString(obj1.Rows[0]["PartyName"]);
                    DispAdd1.Text = Convert.ToString(dtFinal.Rows[0]["Address1"]);
                    DispAdd2.Text = Convert.ToString(dtFinal.Rows[0]["Address2"]);
                    Pin.Text = Convert.ToString(dtFinal.Rows[0]["Pin"]);
                    //Phone.Text = Convert.ToString(dtFinal.Rows[0]["Phone"]);
                    Mobile.Text = Convert.ToString(dtFinal.Rows[0]["Mobile"]);
                    Email.Text = Convert.ToString(dtFinal.Rows[0]["Email"]);
                    BindCity();

                    try
                    {
                        ddlCity.SelectedValue = Convert.ToString(dtFinal.Rows[0]["CITY"]);
                    }
                    catch (Exception ex)
                    {

                        ddlCity.SelectedValue = "0";
                    }

                    txtState.Text = Convert.ToString(dtFinal.Rows[0]["STATE"]);
                    txtCountry.Text = Convert.ToString(dtFinal.Rows[0]["COUNTRY"]);

                }
            }
            else
            {
                DispName.Text = "";
                DispAdd1.Text = "";
                DispAdd2.Text = "";
                Pin.Text = "";
                //Phone.Text = "";
                Mobile.Text = "";
                Email.Text = "";
                //ddlCountry.SelectedValue = "0";
                //ddlState.SelectedValue = "0";
                txtState.Text = "";
                txtCountry.Text = "";
                ddlCity.SelectedValue = "0";
            }

        }

        public bool IsValidEmailAddress(string s)
        {
            if (string.IsNullOrEmpty(s))
                return false;
            else
            {
                var regex = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
                return regex.IsMatch(s) && !s.EndsWith(".");
            }
        }




        private void ClearControls()
        {
            DispName.Text = "";
            DispAdd1.Text = "";
            DispAdd2.Text = "";
            Pin.Text = "";
            Phone.Text = "";
            Mobile.Text = "";
            Email.Text = "";
            txtCountry.Text = "";
            txtState.Text = "";
            ddlCity.SelectedValue = "0";
            Remarks.Text = "";
            BindTransPorter();
            TP.Visible = false;
            Transporter.Text = "";
            chkDispatchTo.Checked = false;
            ProjectType.Items[0].Selected = true;
            //txtProject.Text = "";
            ddlProject.SelectedValue = "0";
            ProjectDiv.Visible = false;
            gvCartItemDetails.DataSource = new DataTable();
            gvCartItemDetails.DataBind();
            ddlScheme.SelectedValue = "0";

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearControls();
        }


        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> SearchProject(string prefixText)
        { //Ankita - 18/may/2016- (For Optimization)
            //string str = "select * FROM MastProject where (Name like '%" + prefixText + "%') and Active=1 ORDER BY Name";
            string str = "select Id,Name FROM MastProject where (Name like '%" + prefixText + "%') and Active=1 ORDER BY Name";
            DataTable dt = new DataTable();
            dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            List<string> Projects = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(dt.Rows[i]["Name"].ToString(), dt.Rows[i]["Id"].ToString());
                Projects.Add(item);
            }
            return Projects;
        }

        protected void ddlCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            string str = @"SELECT DISTINCT T1.cityid,T1.cityName,T1.stateid,T1.stateName,T1.countryid,T1.countryName
                                                                            FROM ViewGeo AS T1 WHERE T1.cityid=" + Convert.ToInt32(ddlCity.SelectedItem.Value) + " and T1.cityid!=0 and T1.cityName!=''";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);

            if (dt != null && dt.Rows.Count > 0)
            {
                txtState.Text = Convert.ToString(dt.Rows[0]["stateName"]);
                txtCountry.Text = Convert.ToString(dt.Rows[0]["countryName"]);
            }
        }

        protected void ProjectType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ProjectType.Items[1].Selected)
            {
                ProjectDiv.Visible = true;
            }
            else
            {
                //txtProject.Text = "";
                ddlProject.SelectedValue = "0";
                ProjectDiv.Visible = false;
            }
        }

        private void FillData()
        {
            string str = @"SELECT * FROM DistItemDetails AS T1 
                                INNER JOIN MastItem AS T2
                                ON T1.ItemID=T2.ItemId WHERE T1.DistID =" + Convert.ToInt32(Settings.Instance.DistributorID) + " Order by T1.ItemName";
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

            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dtItem.Rows.Add();
                    dtItem.Rows[dtItem.Rows.Count - 1]["ID"] = dt.Rows[i]["ID"];
                    dtItem.Rows[dtItem.Rows.Count - 1]["ItemID"] = dt.Rows[i]["ItemID"];
                    dtItem.Rows[dtItem.Rows.Count - 1]["Price"] = dt.Rows[i]["Price"];
                    dtItem.Rows[dtItem.Rows.Count - 1]["Unit"] = dt.Rows[i]["Unit"];
                    dtItem.Rows[dtItem.Rows.Count - 1]["ItemName"] = "(" + dt.Rows[i]["SyncId"].ToString() + ")" + " " + dt.Rows[i]["ItemName"].ToString() + " " + "(" + dt.Rows[i]["ItemCode"].ToString() + ")";
                    dtItem.Rows[dtItem.Rows.Count - 1]["Packing"] = dt.Rows[i]["Packing"];
                    dtItem.Rows[dtItem.Rows.Count - 1]["Qty"] = dt.Rows[i]["Qty"];
                    dtItem.Rows[dtItem.Rows.Count - 1]["PricePerUnit"] = Convert.ToString(dt.Rows[i]["Price"]) + "/" + Convert.ToString(dt.Rows[i]["Unit"]);
                    dtItem.Rows[dtItem.Rows.Count - 1]["Total"] = dt.Rows[i]["Total"];
                    dtItem.Rows[dtItem.Rows.Count - 1]["PriceGroup"] = dt.Rows[i]["PriceGroup"];
                    dtItem.Rows[dtItem.Rows.Count - 1]["Remarks"] = dt.Rows[i]["Remarks"];
                }

                if (dtItem != null && dtItem.Rows.Count > 0)
                {
                    gvCartItemDetails.DataSource = dtItem;
                    gvCartItemDetails.DataBind();
                    Session["ItemDataPlaceOrder"] = dtItem;

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
                else
                {
                    gvCartItemDetails.DataSource = new DataTable();
                    gvCartItemDetails.DataBind();
                }
            }

        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/CartItemDetails.aspx");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

            DataTable dt = new DataTable();
            dt.Columns.Add("ItemID");
            dt.Columns.Add("ItemName");
            dt.Columns.Add("Qty");
            dt.Columns.Add("Unit");
            dt.Columns.Add("Rate");
            dt.Columns.Add("Amount");
            dt.Columns.Add("Remarks");
            dt.Columns.Add("StdPack");
            dt.Columns.Add("PriceGroup");

            if (Session["ItemDataPlaceOrder"] != null)
            {
                DataTable dtItem = (DataTable)Session["ItemDataPlaceOrder"];

                if (dtItem != null && dtItem.Rows.Count > 0)
                {
                    for (int i = 0; i < dtItem.Rows.Count; i++)
                    {
                        dt.Rows.Add();

                        dt.Rows[dt.Rows.Count - 1]["ItemID"] = dtItem.Rows[i]["ItemID"];
                        dt.Rows[dt.Rows.Count - 1]["ItemName"] = dtItem.Rows[i]["ItemName"];
                        dt.Rows[dt.Rows.Count - 1]["Qty"] = dtItem.Rows[i]["Qty"];
                        dt.Rows[dt.Rows.Count - 1]["Unit"] = dtItem.Rows[i]["Unit"];
                        dt.Rows[dt.Rows.Count - 1]["Rate"] = dtItem.Rows[i]["Price"];
                        dt.Rows[dt.Rows.Count - 1]["Amount"] = dtItem.Rows[i]["Total"];
                        dt.Rows[dt.Rows.Count - 1]["Remarks"] = dtItem.Rows[i]["Remarks"];
                        dt.Rows[dt.Rows.Count - 1]["StdPack"] = dtItem.Rows[i]["Packing"];
                        dt.Rows[dt.Rows.Count - 1]["PriceGroup"] = dtItem.Rows[i]["PriceGroup"];
                    }

                }
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please add Product in Cart.');", true);
                return;
            }

            //if (ddlTransporter.SelectedItem.Value == "0")
            //{
            //    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please select Transporter.');", true);
            //    return;
            //}

            if (ddlTransporter.SelectedValue == "Other")
            {
                if (Transporter.Text == "")
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please enter Transporter.');", true);
                    return;
                }
            }
            if (DispName.Text == "")
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please enter Dispatch Name.');", true);
                return;
            }
            if (DispAdd1.Text == "")
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please enter Address 1.');", true);
                return;
            }
            if (ddlCity.SelectedItem.Value == "0")
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please select City.');", true);
                return;
            }
            if (ddltaxform.SelectedItem.Value == "0")
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please select Agt from.');", true);
                return;
            }

            //if (Mobile.Text == "")
            //{
            //    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please enter Mobile No.');", true);
            //    return;
            //}

            //int MobLength = Mobile.Text.Length;

            //if (MobLength != 10)
            //{
            //    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please enter 10 digit Mobile No.');", true);
            //    return;
            //}

            if (Email.Text.Trim() != "")
            {
                if (!IsValidEmailAddress(Email.Text.Trim()))
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please enter valid email address.');", true);
                    return;
                }
            }

            DateTime Currdt = Settings.GetUTCTime();
            string docID = Settings.GetDocID("PORD", Currdt);
            Settings.SetDocID("DISTP", docID);

            int TransPorterID = 0;
            string TransP = "";

            if (ddlTransporter.SelectedItem.Value == "Other")
            {
                string strTP = @"SELECT T1.Name FROM MastTransporter AS T1 WHERE T1.Name='" + Transporter.Text.Trim() + "'";
                DataTable dtTP = DbConnectionDAL.GetDataTable(CommandType.Text, strTP);

                if (dtTP != null)
                {
                    if (dtTP.Rows.Count == 0)
                    {
                        TransPorterID = dp.InsertTP(Transporter.Text);
                        TransP = Transporter.Text;
                    }
                    else
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Transporter already exist.');", true);
                        return;
                    }
                }
            }
            else
            {
                TransPorterID = 0;
                TransP = ddlTransporter.SelectedItem.Text;
            }
            int retSave = 0;

            string PType = "";
            int ProjectID = 0;
            if (ProjectType.Items[0].Selected)
            {
                PType = "N";
            }
            else if (ProjectType.Items[1].Selected)
            {
                PType = "P";
                //ProjectID = Convert.ToInt32(hfProjectID.Value);
                ProjectID = Convert.ToInt32(ddlProject.SelectedItem.Value);
            }

            Label lblAmtTotal = (Label)gvCartItemDetails.FooterRow.FindControl("lblAmtTotal");
            decimal decAmtTotal = Convert.ToDecimal(lblAmtTotal.Text.Trim());

            int CityID = Convert.ToInt32(ViewState["CityID"]);
            if (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
            {
                VisitorsIPAddr = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            }
            else if (HttpContext.Current.Request.UserHostAddress.Length != 0)
            {
                VisitorsIPAddr = HttpContext.Current.Request.UserHostAddress;
            }
            //Label1.Text = "Your IP is: " + VisitorsIPAddr + "--" + HttpContext.Current.Request.UserHostName.ToString();

            retSave = dp.InsertPlaceOrder(Currdt, docID, Settings.Instance.UserID, Settings.Instance.SMID,
                Settings.Instance.DistributorID, TransPorterID, DispName.Text, DispAdd1.Text, DispAdd2.Text,
                txtCountry.Text.Trim(), txtState.Text.Trim(), ddlCity.SelectedItem.Value, Pin.Text,
                Phone.Text, Mobile.Text, Email.Text, dt, Remarks.Text, PType, ProjectID, decAmtTotal, CityID, TransP, Convert.ToInt32(ddlScheme.SelectedItem.Value.Trim()),VisitorsIPAddr,ddltaxform.SelectedItem.Text);

            if (retSave != 0)
            {
                SendEmailMessage(retSave, Currdt, docID, Settings.Instance.UserID, Settings.Instance.SMID,
                Settings.Instance.DistributorID, TransPorterID, DispName.Text, DispAdd1.Text, DispAdd2.Text,
                txtCountry.Text.Trim(), txtState.Text.Trim(), ddlCity.SelectedItem.Value, Pin.Text,
                Phone.Text, Mobile.Text, Email.Text, dt, Remarks.Text, PType, ProjectID, decAmtTotal, CityID, TransP, Convert.ToInt32(ddlScheme.SelectedItem.Value.Trim()));

                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Order Placed Successfully -" + docID + "');", true);
                ClearControls();
                Session["ItemDataPlaceOrder"] = null;
                HtmlMeta meta = new HtmlMeta();
                meta.HttpEquiv = "Refresh";
                meta.Content = "5;url=PlaceOrderList.aspx";
                this.Page.Controls.Add(meta);

            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record Cannot be Insert');", true);
            }

        }

        private void SendEmailMessage(int POID, DateTime Currdt, string docID, string UserID, string SMID, string CustID,
           int TransPorterID, string DispName, string DispAdd1, string DispAdd2, string Country, string State,
           string City, string Pin, string Phone, string Mobile, string Email, DataTable dtItem, string Remarks,
           string PType, int ProjectID, decimal decAmtTotal, int CityID, string TransPorter, int SchemeID)
        {
            string str = @"SELECT T1.SenderEmailId,T1.SenderPassword,T1.Port,T1.PurchaseEmailId,T1.PurchaseEmailCC,
                            T1.ChangePasswordEmailId,T1.ChangePasswordCC,T1.ForgetPasswordEmailId,T1.ForgetPasswordCC,
                            T1.OrderListEmailId,t1.OrderListEmailCC,T1.MailServer
                            FROM MastEnviro AS T1";

            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (dt != null && dt.Rows.Count > 0)
            {
                string strVar = @"SELECT * FROM MastEmailVariable WHERE EmailKey='NewOrderCreated'";
                DataTable dtVar = DbConnectionDAL.GetDataTable(CommandType.Text, strVar);
                string strEmailTemplate = @"SELECT * FROM EmailTemplate WHERE EmialKey='NewOrderCreated'";
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
                                strSubject = strSubject.Replace("{OrderId}", docID);
                            }
                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{OrderDate}")
                            {
                                strSubject = strSubject.Replace("{OrderDate}", Convert.ToString(Currdt));
                            }
                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{ItemGroupCode}")
                            {
                                //string strUID = @"SELECT T2.Underid FROM TransPurchOrder1 AS T1 LEFT JOIN MastItem AS T2 ON T1.ItemId=T2.ItemId WHERE T1.POrdId=" + POID + "";
                                //DataTable dtUID = DbConnectionDAL.GetDataTable(CommandType.Text, strUID);
                                //if (dtUID != null && dtUID.Rows.Count > 0)
                                //{
                                //    string strIG = @"SELECT * FROM MastItem AS T1 WHERE T1.ItemId=" + Convert.ToInt32(dtUID.Rows[0]["Underid"]) + "";
                                //    DataTable dtIG = DbConnectionDAL.GetDataTable(CommandType.Text, strIG);
                                //    strSubject = strSubject.Replace("{ItemGroupCode}", Convert.ToString(dtIG.Rows[0]["ItemCode"]));
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

                            string strDist = @"SELECT * FROM MastParty AS T WHERE T.PartyId=" + Convert.ToInt32(CustID) + "";
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
                                strSubject = strSubject.Replace("{Add1}", DispAdd1);
                            }
                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{Add2}")
                            {
                                strSubject = strSubject.Replace("{Add2}", DispAdd2);
                            }
                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{City}")
                            {
                                strSubject = strSubject.Replace("{City}", ddlCity.SelectedItem.Text.Trim());
                            }
                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{Pin}")
                            {
                                strSubject = strSubject.Replace("{Pin}", Pin);
                            }
                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{State}")
                            {
                                strSubject = strSubject.Replace("{State}", State);
                            }
                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{OrderAmount}")
                            {
                                strSubject = strSubject.Replace("{OrderAmount}", Convert.ToString(decAmtTotal));
                            }
                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{Remarks}")
                            {
                                strSubject = strSubject.Replace("{Remarks}", Remarks);
                            }
                        }

                        ///////////////////////////////////////////
                        if (strMailBody.Contains(Convert.ToString(dtVar.Rows[j]["Name"])))
                        {
                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{OrderId}")
                            {
                                strMailBody = strMailBody.Replace("{OrderId}", docID);
                            }
                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{OrderDate}")
                            {
                                strMailBody = strMailBody.Replace("{OrderDate}", Convert.ToString(Currdt));
                            }
                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{ItemGroupCode}")
                            {
                                //string strUID = @"SELECT T2.Underid FROM TransPurchOrder1 AS T1 LEFT JOIN MastItem AS T2 ON T1.ItemId=T2.ItemId WHERE T1.POrdId=" + POID + "";
                                //DataTable dtUID = DbConnectionDAL.GetDataTable(CommandType.Text, strUID);
                                //if (dtUID != null && dtUID.Rows.Count > 0)
                                //{
                                //    string strIG = @"SELECT * FROM MastItem AS T1 WHERE T1.ItemId=" + Convert.ToInt32(dtUID.Rows[0]["Underid"]) + "";
                                //    DataTable dtIG = DbConnectionDAL.GetDataTable(CommandType.Text, strIG);
                                //    strMailBody = strMailBody.Replace("{ItemGroupCode}", Convert.ToString(dtIG.Rows[0]["ItemCode"]));
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

                            string strDist = @"SELECT * FROM MastParty AS T WHERE T.PartyId=" + Convert.ToInt32(CustID) + "";
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
                                strMailBody = strMailBody.Replace("{Add1}", DispAdd1);
                            }
                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{Add2}")
                            {
                                strMailBody = strMailBody.Replace("{Add2}", DispAdd2);
                            }
                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{City}")
                            {
                                strMailBody = strMailBody.Replace("{City}", ddlCity.SelectedItem.Text.Trim());
                            }
                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{Pin}")
                            {
                                strMailBody = strMailBody.Replace("{Pin}", Pin);
                            }
                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{State}")
                            {
                                strMailBody = strMailBody.Replace("{State}", State);
                            }
                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{OrderAmount}")
                            {
                                strMailBody = strMailBody.Replace("{OrderAmount}", Convert.ToString(decAmtTotal));
                            }
                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{Remarks}")
                            {
                                strMailBody = strMailBody.Replace("{Remarks}", Remarks);
                            }
                        }
                    }
                }
                try
                {
                    MailMessage mail = new MailMessage();
                    mail.From = new MailAddress(Convert.ToString(dt.Rows[0]["SenderEmailId"]));
                    mail.Subject = strSubject;
                    mail.Body = strMailBody;
                    // mail.Attachments.Add(new Attachment(Server.MapPath("TextFileFolder/Dealer_Order.txt")));
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

                    string strDistEmailID = @"SELECT * FROM MastParty AS T WHERE T.PartyId=" + Convert.ToInt32(CustID) + "";
                    DataTable dtDistEmailID = DbConnectionDAL.GetDataTable(CommandType.Text, strDistEmailID);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtDistEmailID.Rows[0]["Email"])))
                    mail.To.Add(new MailAddress(Convert.ToString(dtDistEmailID.Rows[0]["Email"])));

                    string strSPEmailID = @"SELECT * FROM MastSalesRep WHERE SMId=" + Convert.ToInt32(dtDistEmailID.Rows[0]["SMID"]) + "";                 
                    DataTable dtSPEmailID = DbConnectionDAL.GetDataTable(CommandType.Text, strSPEmailID);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtSPEmailID.Rows[0]["Email"])))
                    mail.To.Add(new MailAddress(Convert.ToString(dtSPEmailID.Rows[0]["Email"])));


                    SmtpClient mailclient = new SmtpClient(Convert.ToString(dt.Rows[0]["MailServer"]), Convert.ToInt32(dt.Rows[0]["Port"]));
                    mailclient.EnableSsl = false;
                    mailclient.UseDefaultCredentials = false;
                    mailclient.Credentials = mailAuthenticaion;
                    mail.IsBodyHtml = true;
                    mailclient.Send(mail);

                }
                catch (Exception ex)
                {

                }


            }
        }

        protected void btnFind_Click(object sender, EventArgs e)
        {
            mainDiv.Style.Add("display", "none");
            rptmain.Style.Add("display", "block");
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            string str = "";
            string StatusVal = "0";

            if (ddlStatus.SelectedItem.Value != "0")
            {
                StatusVal = ddlStatus.SelectedItem.Value;
            }
            if (txtmDate.Text != "" && txttodate.Text != "")
            {
                str = @"SELECT T1.POrdId,CONVERT(date,T1.VDate,103),106) AS VDate,T1.PODocId,T2.PartyName,
                            CASE WHEN T1.OrderStatus='H' THEN 'Processed'
                                 WHEN T1.OrderStatus='R' THEN 'Processed'
                                 WHEN T1.OrderStatus='P' THEN 'Pending'
                                 WHEN T1.OrderStatus='M' THEN 'Processed'
                                 WHEN T1.OrderStatus='C' THEN 'Processed' END AS OrderStatus
                            from TransPurchOrder T1 inner join MastParty T2 
                            on T1.DistId=T2.PartyId where T2.Active=1 and T1.DistId = " + Convert.ToInt32(Settings.Instance.DistributorID) +
                            " Convert(VARCHAR(10),T1.VDate,101)>='" + Settings.dateformat1(txtmDate.Text) + "' and Convert(VARCHAR(10),T1.VDate,101)<='" + Settings.dateformat1(txttodate.Text) + "'" +
                            @" ORDER BY T1.PODocId ASC,T1.VDate desc";
                //str = @"select * from TransVisit where  SMId=" + ddlUndeUser.SelectedValue + " and VDate>='" + Settings.dateformat1(txtmDate.Text) + "' and VDate<='" + Settings.dateformat1(txttodate.Text) + "' order by VDate desc";
            }
            else if (txtmDate.Text != "")
            {
                //str = @"select * from TransVisit where  SMId=" + ddlUndeUser.SelectedValue + " and VDate>='" + Settings.dateformat1(txtmDate.Text) + "' order by VDate desc";

                str = @"SELECT T1.POrdId,CONVERT(date,T1.VDate,103),106) AS VDate,T1.PODocId,T2.PartyName,
                               CASE WHEN T1.OrderStatus='H' THEN 'Processed'
                                 WHEN T1.OrderStatus='R' THEN 'Processed'
                                 WHEN T1.OrderStatus='P' THEN 'Pending'
                                 WHEN T1.OrderStatus='M' THEN 'Processed'
                                 WHEN T1.OrderStatus='C' THEN 'Processed' END AS OrderStatus
                            from TransPurchOrder T1 inner join MastParty T2 
                            on T1.DistId=T2.PartyId where T2.Active=1 
                            and T1.DistId = " + Convert.ToInt32(Settings.Instance.DistributorID) +
                            " and Convert(VARCHAR(10),T1.VDate,101)>='" + Settings.dateformat1(txtmDate.Text) + "' ORDER BY T1.PODocId ASC,T1.VDate desc";
            }
            else
            {
                if (StatusVal == "0")
                {
                    str = @"SELECT T1.POrdId,CONVERT(date,T1.VDate,103),106) AS VDate,T1.PODocId,T2.PartyName,
                                 CASE WHEN T1.OrderStatus='H' THEN 'Processed'
                                 WHEN T1.OrderStatus='R' THEN 'Processed'
                                 WHEN T1.OrderStatus='P' THEN 'Pending'
                                 WHEN T1.OrderStatus='M' THEN 'Processed'
                                 WHEN T1.OrderStatus='C' THEN 'Processed' END AS OrderStatus
                            from TransPurchOrder T1 inner join MastParty T2 
                            on T1.DistId=T2.PartyId where T2.Active=1   and T1.DistId = " + Convert.ToInt32(Settings.Instance.DistributorID) +
                                @" and (T1.OrderStatus='" + StatusVal + "' OR " + StatusVal + "='0') ORDER BY T1.PODocId ASC,T1.VDate desc";
                }
                else
                {
                    str = @"SELECT T1.POrdId,CONVERT(date,T1.VDate,103),106) AS VDate,T1.PODocId,T2.PartyName,
                               CASE WHEN T1.OrderStatus='H' THEN 'Processed'
                                 WHEN T1.OrderStatus='R' THEN 'Processed'
                                 WHEN T1.OrderStatus='P' THEN 'Pending'
                                 WHEN T1.OrderStatus='M' THEN 'Processed'
                                 WHEN T1.OrderStatus='C' THEN 'Processed' END AS OrderStatus
                            from TransPurchOrder T1 inner join MastParty T2 
                            on T1.DistId=T2.PartyId where T2.Active=1   and T1.DistId = " + Convert.ToInt32(Settings.Instance.DistributorID) +
                                @" and (T1.OrderStatus='" + StatusVal + "') ORDER BY T1.PODocId ASC,T1.VDate desc";
                }
            }

            DataTable depdt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (depdt.Rows.Count > 0)
            {
                rpt.DataSource = depdt;
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
            mainDiv.Style.Add("display", "block");
            rptmain.Style.Add("display", "none");
        }

        protected void btnPOBack_Click(object sender, EventArgs e)
        {
            mainDiv.Style.Add("display", "block");
            rptmain.Style.Add("display", "none");
        }

        protected void CancelOrder_Click(object sender, EventArgs e)
        {
            //Cancel Order
            int retdel = dp.CancelOwnOrder_Dist(Convert.ToInt32(ViewState["POIDDel"]));
            if (retdel == 1)
            {

                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record " + lblOrderSt.Text + " Canceled Successfully');", true);

            }
        }

    }
}