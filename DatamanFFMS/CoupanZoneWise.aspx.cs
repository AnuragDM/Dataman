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
    public partial class CoupanZoneWise : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            txtIssueDate.Attributes.Add("readonly", "readonly");
            if (!IsPostBack)
            {
                txtIssueDate.Text = DateTime.Parse(DateTime.Now.ToUniversalTime().AddSeconds(19800).ToShortDateString()).ToString("dd/MMM/yyyy");
                CalendarExtender1.EndDate = Settings.GetUTCTime();
                BindZone(); BindCoupanScheme(); BindDistributor();
            }
        }

         private void BindDistributor()
        {
            try
            {
                string str = @"SELECT PartyId,PartyName FROM MastParty where partydist=1 and Active=1 ORDER BY PartyName";
                fillDDLDirect(ddlDistributor, str, "PartyId", "PartyName", 1);               
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
                string str = @"SELECT distinct Zone FROM MastZone where Active=1 ORDER BY Zone";
                fillDDLDirect(ddlZone, str, "Zone", "Zone", 1);               
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
                string str = @"SELECT SchemeId,SchemeName FROM MastCoupanScheme where Active=1 ORDER BY SchemeName";
                fillDDLDirect(ddlScheme, str, "SchemeId", "SchemeName", 1);                
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
                string str = @"SELECT Prefix FROM MastZone where Active=1 and Zone='" + zone + "' ORDER BY Prefix";
                fillDDLDirect(ddlPrefix, str, "Prefix", "Prefix", 1);                
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return 0;
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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int exists = 0;
            string insertquery = "";
            try
            {
                if (Convert.ToInt32(txtstartCoupan.Text) > Convert.ToInt32(txtEndCoupan.Text))
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Start coupan no. cannot be greater than from End Coupan no.');", true);
                    return;
                }
                if (txtstartCoupan.Text != "")
                {
                    string start, end;
                    string prefix = ddlPrefix.SelectedItem.Text;
                    start = txtstartCoupan.Text;
                    end = txtEndCoupan.Text;
                    //for (int i = Convert.ToInt32(start); i <= Convert.ToInt32(end); i++)
                    //{
                    //    string startwithprefix = prefix + i.ToString();
                    //    string strcheck = "SELECT count(*) FROM TransCoupan WHERE CoupanNo in ( '" + i + "') AND SchemeId=" + ddlScheme.SelectedValue + " and Zone = '" + ddlZone.SelectedValue + "' and prefix='" + prefix + "'";
                    //    int countcheck = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, strcheck));
                    //    if (countcheck > 0)
                    //    {
                    //        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('This Coupan range already issued.');", true);
                    //        return;
                    //    }
                    //}
                    string strcheck = "SELECT count(*) FROM (SELECT *, ROW_NUMBER() OVER (ORDER BY ID) AS RowNum FROM [TransCoupan]) AS MyDerivedTable WHERE MyDerivedTable.coupanno BETWEEN " + start + " AND " + end + " AND SchemeId=" + ddlScheme.SelectedValue + " and Zone = '" + ddlZone.SelectedValue + "' and prefix='" + prefix + "'";
                    int countcheck = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, strcheck));
                    if (countcheck > 0)
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('This Coupan range already issued.');", true);
                        return;
                    }
                    for (int i = Convert.ToInt32(start); i <= Convert.ToInt32(end); i++)
                    {
                        string addstr = prefix + i.ToString();
                        insertquery = @"Insert into TransCoupan (SchemeId,Zone,Prefix,CoupanNo,CoupanNoWithPrefix,CoupanIssueDate) values (" + ddlScheme.SelectedValue + ",'" + ddlZone.SelectedValue + "','" + ddlPrefix.SelectedValue + "','" + i + "', '" + addstr + "',DateAdd(minute,330,getutcdate()))";
                        exists = Convert.ToInt32(DbConnectionDAL.ExecuteQuery(insertquery));
                    }
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Save Successfully');", true);
                    ClearControls();
                }           
               
            }
            catch(Exception ex)
            { 
                ex.ToString(); 
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("CoupanZoneWise.aspx");
        }
        protected void ClearControls()
        {
          //  txtIssueDate.Text = DateTime.Parse(DateTime.Now.ToUniversalTime().AddSeconds(19800).ToShortDateString()).ToString("dd/MMM/yyyy");
            ddlZone.SelectedValue = "0";
            ddlScheme.SelectedValue = "0";
            ddlPrefix.SelectedValue = "0";
            ddlDistributor.SelectedValue = "0";
            txtstartCoupan.Text = "";
            txtEndCoupan.Text = "";             

        }   
       

        protected void ddlZone_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindPrefix(ddlZone.SelectedValue);
        }        
    }
}
   