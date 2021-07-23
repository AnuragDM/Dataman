using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BAL;
using System.IO;
using BusinessLayer;
namespace AstralFFMS
{
    public partial class EmployeeWiseConvLimit : System.Web.UI.Page
    {
        EmplCityConvAmtBAL EC = new EmplCityConvAmtBAL();
        static string pageName = "";
        protected void Page_Load(object sender, EventArgs e)
        {
             pageName = Path.GetFileName(Request.Path);           
             string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
             lblPageHeader.Text = Pageheader;
             //Ankita - 11/may/2016- (For Optimization)
                //btnadd.Enabled = Settings.Instance.CheckAddPermission(pageName, Convert.ToString(Session["user_name"]));
                //btnadd.CssClass = "btn btn-primary";
            
                if (!Page.IsPostBack)
                {//Ankita - 11/may/2016- (For Optimization)
                    btnadd.Enabled = Settings.Instance.CheckAddPermission(pageName, Convert.ToString(Session["user_name"]));
                    btnadd.CssClass = "btn btn-primary";
                    divrpt.Attributes.Add("style", "display:none");
                    BindCity();
                }
        }

        protected void btnadd_Click(object sender, EventArgs e)
        {
          int val= EC.Insert(Convert.ToInt32(hdnSMId.Value),Convert.ToInt32(ddlcity.SelectedValue),Convert.ToDecimal(convamt.Value));
          if (val < 0)
          {
              System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Duplicate Record Exists');", true);
              txtsalespersons.Focus();
          }
          else
          {
              System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Inserted Successfully');", true);
             ddlcity.SelectedIndex = 0; convamt.Value = "";
              BindData();
          }
         
        }

        protected void btnsearch_Click(object sender, EventArgs e)
        {
            BindData();
        }
        private void BindData()
        {
            try
            {
                DataTable dt = new DataTable();
                string addqry = "";
                if(ddlcity.SelectedIndex > 0)
                    addqry = " and me.cityId=" + ddlcity.SelectedValue + "";
                if (!string.IsNullOrEmpty(hdnSMId.Value))
                    addqry = " and me.SmId=" + hdnSMId.Value + "";

                string str = "select ma.displayname,msr.smname,me.ConveyanceAmt,me.Id from MastEmployeeCityConvLimit me inner join MastSalesRep msr on msr.smid=me.smid inner join MastArea ma on me.cityId=ma.areaId where ma.areatype='City'"+ addqry+" group by msr.smname,ma.displayname,me.ConveyanceAmt,me.Id order by ma.displayname,msr.smname";
                dt = DAL.DbConnectionDAL.GetDataTable(CommandType.Text, str);
                divrpt.Attributes.Remove("style");
                rpt.DataSource = dt; rpt.DataBind();
            }
            catch (Exception ex) { ex.ToString(); }
        }
        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> SearchSalesPerson(string prefixText)
        {
            string str = @"SELECT msr.SMName,msr.SMID FROM MastSalesRep msr where isnull(msr.AllowChangeCity,0)=1 and  msr.Active=1 AND msr.SMName LIKE '%" + prefixText + "%'  order by msr.SMName";
            
            DataTable dt = new DataTable();
            dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            List<string> salespersons = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string sp = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem("" + dt.Rows[i]["SMName"].ToString() , dt.Rows[i]["SMId"].ToString());
                salespersons.Add(sp);
            }
            return salespersons;
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
        public void BindCity()
        {
            // Nishu 31/07/2017
            //string str = "select AreaId,DisplayName from mastarea where areatype='CITY' and ACTIVE=1 order by AreaName";
            //fillDDLDirect(ddlcity, str, "AreaId", "DisplayName", 1);
            string str = "SELECT DISTINCT T.cityid,T.cityName+' - '+T.districtName+' - '+T.statename AS cityName FROM ViewGeo AS T WHERE T.cityid>0 and T.cityAct=1 ORDER BY cityName";
            fillDDLDirect(ddlcity, str, "cityid", "cityName", 1);
        }

        protected void lnkdelete_Click(object sender, EventArgs e)
        {
            try
            {
                string confirmValue = Request.Form["confirm_value"];
                if (confirmValue == "Yes")
                {
                    LinkButton btn = (LinkButton)sender;
                    var item = (RepeaterItem)btn.NamingContainer;
                    HiddenField hdfId = (HiddenField)item.FindControl("hdnId");
                    string delqry = "delete from MastEmployeeCityConvLimit where id=" + hdfId.Value + "";
                    DbConnectionDAL.ExecuteQuery(delqry);
                    BindData();
                }
            }
            catch (Exception ex) { ex.ToString(); }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            txtsalespersons.Text = "";
            ddlcity.SelectedValue = "0";
            convamt.Value = "";
            hdnSMId.Value = "";
        }

        protected void rpt_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                LinkButton lnkdelete = (LinkButton)e.Item.FindControl("lnkdelete");
                lnkdelete.Visible = Settings.Instance.CheckDeletePermission(pageName, Convert.ToString(Session["user_name"]));
            }
        }

    }
}