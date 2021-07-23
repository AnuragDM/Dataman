using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using BusinessLayer;
using System.Data;

namespace AstralFFMS
{
    public partial class CoupanIssueRetailer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            txtIssueDate.Attributes.Add("readonly", "readonly");
            if (!IsPostBack)
            {
                txtIssueDate.Text = DateTime.Parse(DateTime.Now.ToUniversalTime().AddSeconds(19800).ToShortDateString()).ToString("dd/MMM/yyyy");
                CalendarExtender1.EndDate = Settings.GetUTCTime();
                BindCoupanScheme(); BindZone(); BindDistributor();               
            }
        }
        private void BindDistributor()
        {
            try
            {
                string str = "";
                if (Settings.Instance.RoleType == "Distributor")
                {
                    str = @"SELECT mp.PartyId,(mp.PartyName + ' - ' + ma.AreaName) AS PartyName FROM MastParty mp LEFT JOIN mastarea ma ON mp.AreaId=ma.AreaId where mp.partyid='" + Settings.Instance.DistributorID + "' and mp.partydist=1 and mp.Active=1 ORDER BY mp.PartyName";
                    fillDDLDirectDropdown(ddlDistributor, str, "PartyId", "PartyName");
                }
                else
                {
                    str = @"SELECT mp.PartyId,(mp.PartyName + ' - ' + ma.AreaName) AS PartyName FROM MastParty mp LEFT JOIN mastarea ma ON mp.AreaId=ma.AreaId where mp.partydist=1 and mp.Active=1 ORDER BY mp.PartyName";
                    fillDDLDirect(ddlDistributor, str, "PartyId", "PartyName", 1);
                }
               
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private void BindZone()
        {
            try
            {
                string str = "";
                if (Settings.Instance.RoleType == "Distributor")
                {
                    str = @"SELECT distinct Zone FROM Mastparty where Partyid='" + Settings.Instance.DistributorID + "' ORDER BY Zone";
                    fillDDLDirectDropdown(ddlZone, str, "Zone", "Zone");
                    BindPrefix(ddlZone.SelectedValue);
                }
                else
                {
                    str = @"SELECT distinct Zone FROM MastZone where Active=1 ORDER BY Zone";
                    fillDDLDirect(ddlZone, str, "Zone", "Zone", 1);
                }
                
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private void BindCoupanScheme()
        {
            try
            {
                string str = "";
                if (Settings.Instance.RoleType == "Distributor")
                {
                    str = @"SELECT SchemeId,SchemeName FROM MastCoupanScheme where Active=1 ORDER BY SchemeName";
                    fillDDLDirectDropdown(ddlScheme, str, "SchemeId", "SchemeName");
                }
                else
                {
                    str = @"SELECT SchemeId,SchemeName FROM MastCoupanScheme where Active=1 ORDER BY SchemeName";
                    fillDDLDirect(ddlScheme, str, "SchemeId", "SchemeName", 1);
                }
                
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private Int32 BindPrefix(string zone)
        {
            try
            {
                string str = "";
                if (Settings.Instance.RoleType == "Distributor")
                {
                    str = str = @"SELECT Prefix FROM MastZone where Active=1 and Zone='" + zone + "' ORDER BY Prefix";
                    fillDDLDirectDropdown(ddlPrefix, str, "Prefix", "Prefix");
                }
                else
                {
                    str = @"SELECT Prefix FROM MastZone where Active=1 and Zone='" + zone + "' ORDER BY Prefix";
                    fillDDLDirect(ddlPrefix, str, "Prefix", "Prefix", 1);
                }
               
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return 0;
        }

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> SearchItem(string prefixText)
        {
            string stradd = "";
            if(Settings.Instance.DistributorID != "0")
            {
                stradd = "and distid = " + Settings.Instance.DistributorID;
            }
            string str = "select RetailerName,Area,MobileNo FROM MastCoupanRetailer where (RetailerName like '%" + prefixText + "%' or MobileNo like '%" + prefixText + "%') " + stradd + "";       
            DataTable dt = new DataTable();
            dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            List<string> customers = new List<string>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(dt.Rows[i]["RetailerName"].ToString() + " - " + dt.Rows[i]["Area"].ToString() + " " + " - " + dt.Rows[i]["MobileNo"].ToString(), dt.Rows[i]["MobileNo"].ToString());
                //string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem("(" + dt.Rows[i]["RetailerName"].ToString() + ")" + " - " + dt.Rows[i]["Area"].ToString() + " " + "(" + dt.Rows[i]["MobileNo"].ToString() + ")", dt.Rows[i]["MobileNo"].ToString());
                customers.Add(item);                 
            }           
            return customers;
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            int exists = 0,insertexist = 0,retailerid = 0;
            string insertquery = "", insquery = "",  strretailer = "",retailername = "";       
   
            try
            {
                if (txtstartCoupan.Text != "")
                {
                    string start, end;
                    string prefix = ddlPrefix.SelectedItem.Text;
                    start = txtstartCoupan.Text;
                    end = txtEndCoupan.Text;

                    if (Convert.ToInt32(txtstartCoupan.Text) > Convert.ToInt32(txtEndCoupan.Text))
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Start coupan no. cannot be greater than from End Coupan no.');", true);
                        return;
                    }
                    string strcount = "SELECT count(*) FROM TransCoupan WHERE CoupanNo BETWEEN " + start + " AND " + end + " AND SchemeId=" + ddlScheme.SelectedValue + " and Zone = '" + ddlZone.SelectedValue + "' and prefix='" + prefix + "' and Distid=" + ddlDistributor.SelectedValue + "";
                    int count = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, strcount));
                    if (count == 0)
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('This Coupan range not available.');", true);
                        return;
                    }

                    string strcheck = "SELECT count(*) FROM TransCoupan WHERE CoupanNo between " + start + " and " + end + "  AND RetailerId is not NULL AND SchemeId=" + ddlScheme.SelectedValue + " and Zone = '" + ddlZone.SelectedValue + "' and prefix='" + prefix + "' and Distid=" + ddlDistributor.SelectedValue + "";

                    int countcheck = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, strcheck));
                    if (countcheck > 0)
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('This Coupan range already issued.');", true);
                        return;
                    }
                    string str = "SELECT count(*) FROM MastCoupanRetailer WHERE MobileNo='" + txtMobile.Text + "'";
                    int countRetailer = DbConnectionDAL.GetIntScalarVal(str);
                    if (countRetailer == 0)
                    {
                        insquery = @"INSERT INTO [dbo].[MastCoupanRetailer] ([RetailerName],[MobileNo],[Area],[DistId],[Createddate]) VALUES  ('" + txtSearch.Text.ToUpper() + "','" + txtMobile.Text + "','" + txtArea.Text.ToUpper() + "'," + ddlDistributor.SelectedValue + ",DateAdd(minute,330,getutcdate()))";
                        insertexist = Convert.ToInt32(DbConnectionDAL.ExecuteQuery(insquery));
                    }
                    strretailer = "SELECT * FROM MastCoupanRetailer WHERE MobileNo = '" + txtMobile.Text + "'";
                    DataTable dtMobile = DbConnectionDAL.GetDataTable(CommandType.Text, strretailer);
                    if (dtMobile.Rows.Count > 0)
                    {
                        retailername = dtMobile.Rows[0]["RetailerName"].ToString();
                        retailerid = Convert.ToInt32(dtMobile.Rows[0]["Id"]);
                    }                    

                    insertquery = @"Update TransCoupan set RetailerId = " + retailerid + ",RetailerName='" + retailername + "',RetailerIssuedate='" + txtIssueDate.Text + "' where SchemeId=" + ddlScheme.SelectedValue + " and Zone='" + ddlZone.SelectedValue + "' and Prefix='" + ddlPrefix.SelectedValue + "' and Distid=" + ddlDistributor.SelectedValue + " and CoupanNo between " + start + " and " + end + " ";
                    exists = Convert.ToInt32(DbConnectionDAL.ExecuteQuery(insertquery));
                    
                }                
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Save Successfully');", true);
                ClearControls();
            }
            catch (Exception ex)
            { ex.ToString(); }
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

        public static void fillDDLDirectDropdown(DropDownList xddl, string xmQry, string xvalue, string xtext)
        {
            DataTable xdt = new DataTable();
            xdt = DbConnectionDAL.GetDataTable(CommandType.Text, xmQry);
            xddl.DataSource = xdt;
            xddl.DataTextField = xtext.Trim();
            xddl.DataValueField = xvalue.Trim();
            xddl.DataBind();
            //if (sele == 1)
            //{
            //    if (xdt.Rows.Count >= 1)
            //        xddl.Items.Insert(0, new ListItem("--Select--", "0"));
            //    else if (xdt.Rows.Count == 0)
            //        xddl.Items.Insert(0, new ListItem("---", "0"));
            //}
            //else if (sele == 2)
            //{
            //    xddl.Items.Insert(0, new ListItem("--Others--", "0"));
            //}
            xdt.Dispose();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("CoupanIssueRetailer.aspx");
        }

        protected void ClearControls()
        {
            txtSearch.Text = "";
            txtMobile.Text = "";
            txtArea.Text = "";
            txtstartCoupan.Text = "";
            txtEndCoupan.Text = "";
            lblCoupan.Text = "";
        }      

        protected void ddlZone_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindPrefix(ddlZone.SelectedValue);
        }        

        protected void ddlPrefix_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindCoupanAvailability(ddlScheme.SelectedValue, ddlZone.SelectedValue, ddlPrefix.SelectedValue);
        }

        private Int32 BindCoupanAvailability(string scheme, string zone, string prefix)
        {
            try
            {
                string str = "SELECT count( * ) AS number FROM TransCoupan WHERE SchemeId=" + scheme + " and Zone = '" + zone + "' and prefix='" + prefix + "' AND DistId IS NULL";
                int number = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, str));
                lblCoupan.Text = number.ToString();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return 0;
        }

        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtSearch.Text != string.Empty)
                {
                    txtMobile.Text = string.Empty;
                    txtArea.Text = string.Empty;
                    string itemId = hfItemId.Value;
                    string getItemQry = @"select RetailerName,Area,MobileNo from MastCoupanRetailer where MobileNo='" + itemId + "'";
                    DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, getItemQry);
                    if (dt.Rows.Count > 0)
                    {
                        txtMobile.Text = dt.Rows[0]["MobileNo"].ToString();
                        txtArea.Text = dt.Rows[0]["Area"].ToString();
                    }
                    hfItemId.Value = "";
                }
                string name = txtSearch.Text;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
    }
}