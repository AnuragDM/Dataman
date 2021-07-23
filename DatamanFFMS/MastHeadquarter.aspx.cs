using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BAL;
using System.Reflection;
using BusinessLayer;
using System.IO;
using BAL.SalesPersons;

namespace AstralFFMS
{
    public partial class MastHeadquarter : System.Web.UI.Page
    {
        Headquarter CTB = new Headquarter();
        string parameter = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Form.DefaultButton = btnSave.UniqueID; 
            this.Form.DefaultButton = this.btnSave.UniqueID;
            parameter = Request["__EVENTARGUMENT"];
            if (parameter != "")
            {
                ViewState["Id"] = parameter;
                FillSocietyControls(Convert.ToInt32(parameter));
                mainDiv.Style.Add("display", "block");
                rptmain.Style.Add("display", "none");
            }
            
            if (!IsPostBack)
            { 
                string pageName = Path.GetFileName(Request.Path);
                string PermAll = Settings.Instance.CheckPagePermissions(pageName, Convert.ToString(Session["user_name"]));
                string[] SplitPerm = PermAll.Split(',');
                if (btnSave.Text == "Save")
                {
                    btnSave.Enabled = Convert.ToBoolean(SplitPerm[1]);
                    btnSave.CssClass = "btn btn-primary";
                }
                else
                {
                    btnSave.Enabled = Convert.ToBoolean(SplitPerm[2]);
                    btnSave.CssClass = "btn btn-primary";
                }
                btnDelete.Enabled = Convert.ToBoolean(SplitPerm[3]);
                btnDelete.CssClass = "btn btn-primary";
                btnDelete.Visible = false;
                HeadQuarterName.Focus();
                BindCountry();
                BindSociety();
               // BindState();
                //BindCity();
                fillRepeter();
                mainDiv.Style.Add("display", "none");
                rptmain.Style.Add("display", "block");
               
                if (Request.QueryString["Id"] != null)
                {
                  FillSocietyControls(Convert.ToInt32(Request.QueryString["Id"]));
                }
            }
        }

        private void BindSociety()
        {
            string strC = @"Select Id,HeadquarterName as Name From MastHeadquarter";
            fillDDLDirect(ddlHeadquarterlist, strC, "Id", "Name");
        }
        private void BindCountry()
        {
            string strC = "select ma.AreaID As Id,ma.AreaName As Name from mastarea ma where ma.AreaType='Country' and ma.Active='1' order by ma.AreaName";
            fillDDLDirect(ddlCountry, strC, "Id", "Name");
        }

        private void BindState(string CountryID, DropDownList DL)
        {
            //string strC = "select ma.AreaID As Id,ma.AreaName As Name from mastarea ma where ma.AreaType='State' and ma.Active='1' order by ma.AreaName";
            //fillDDLDirect(ddlState, strC, "Id", "Name");
            string strQ = "select Distinct(StateID) as Id,StateName as Name from ViewGeo VG where VG.CountryId=" + CountryID + " order by StateName";
            fillDDLDirect(DL, strQ, "Id", "Name");
        }

        private void BindCity(string StateID, DropDownList DL)
        {
            string strQ = "select Distinct(CityID) as Id,CityName as Name from ViewGeo VG where VG.StateId=" + StateID + " order by CityName";
            fillDDLDirect(DL, strQ, "Id", "Name");
            //string strQ = "select ma.AreaID As Id,ma.AreaName As Name from mastarea ma where ma.AreaType='City' and ma.Active='1' order by ma.AreaName";
           // fillDDLDirect(ddlCity, strQ, "Id", "Name");
        }

        public static void fillDDLDirect(DropDownList xddl, string xmQry, string xvalue, string xtext)
        {
            DataTable xdt = new DataTable();
            xdt = DbConnectionDAL.GetDataTable(CommandType.Text, xmQry);
            xddl.DataSource = xdt;
            xddl.DataTextField = xtext.Trim();
            xddl.DataValueField = xvalue.Trim();
            xddl.DataBind();

            if (xdt.Rows.Count >= 1)
                xddl.Items.Insert(0, new ListItem("--Select--", "0"));
            else if (xdt.Rows.Count == 0)
                xddl.Items.Insert(0, new ListItem("---", "0"));

            xdt.Dispose();
        }

        private void fillRepeter()
        {

            string stradd = "";
            if (ddlHeadquarterlist.SelectedValue != "0")
            {
                stradd = " where MB.Id=" + Convert.ToInt32(ddlHeadquarterlist.SelectedValue) + "";
            }

            string str = @"select MB.Id,MB.HeadquarterName as Name,MB.Address,MA.AreaName As CityName,MA1.AreaName As StateName,MA2.AreaName As CountryName,MB.Pincode,MB.CreatedDate,MB.UpdatedDate,MU.SMName as UpdatedBy from MastHeadquarter MB Left join MastArea MA on MA.AreaID=MB.City_Id Left join MastArea MA1 on MA1.AreaID=MB.State_Id Left join MastArea MA2 on MA2.AreaID=MB.Country_Id left join MastSalesRep MU on MU.smid=MB.updatedby  " + stradd + " order by MB.HeadquarterName";
            DataTable CityTypedt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (CityTypedt.Rows.Count > 0)
            {
                
                rpt.DataSource = CityTypedt;
                rpt.DataBind();
            }
            else
            {
                rpt.DataSource = null;
                rpt.DataBind();
            }
        }
        private void FillSocietyControls(int Id)
        {
            try
            {
                string citytypequery = @"select HeadquarterName as Name,Address,City_Id,State_Id,Country_Id,Pincode from MastHeadquarter where Id=" + Id;
                DataTable CityTypeValueDt = DbConnectionDAL.GetDataTable(CommandType.Text, citytypequery);
                if (CityTypeValueDt.Rows.Count > 0)
                {
                    HeadQuarterName.Value = CityTypeValueDt.Rows[0]["Name"].ToString();
                    Address.Value = CityTypeValueDt.Rows[0]["Address"].ToString();
                    ddlCountry.SelectedValue = CityTypeValueDt.Rows[0]["Country_Id"].ToString();
                    BindState(ddlCountry.SelectedValue, ddlState);
                    ddlState.SelectedValue = CityTypeValueDt.Rows[0]["State_Id"].ToString();
                    BindCity(ddlState.SelectedValue, ddlCity);
                    ddlCity.SelectedValue = CityTypeValueDt.Rows[0]["City_Id"].ToString();
                    txtPincode.Value = CityTypeValueDt.Rows[0]["Pincode"].ToString();
                    btnSave.Text = "Update";
                    btnDelete.Visible = true;
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while displaying the records');", true);
            }
        }

        protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlState.SelectedIndex > 0)
            {
                //BindBlock(ddlSocietyList.SelectedValue, ddlbuildinglist);
                BindCity(ddlState.SelectedValue, ddlCity);
            }

        }

        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCountry.SelectedIndex > 0)
            {
                //BindBlock(ddlSocietyList.SelectedValue, ddlbuildinglist);
                BindState(ddlCountry.SelectedValue, ddlState);
            }

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnSave.Text == "Update")
                {
                    Updatesociety();
                }
                else
                {
                    Insertsociety();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error While Inserting Records.');", true);
            }
        }

        private void Insertsociety()
        {
            int retsave = CTB.InsertUpdateBuilding(0, HeadQuarterName.Value, Address.Value, Convert.ToInt32(ddlCity.SelectedValue), Convert.ToInt32(Settings.Instance.SMID), Convert.ToInt32(ddlCountry.SelectedValue), Convert.ToInt32(ddlState.SelectedValue), Convert.ToInt32(txtPincode.Value));

            if (retsave == -1)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Duplicate Headquarter Name Exists');", true);
                HeadQuarterName.Focus();
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Inserted Successfully');", true);
                 ClearControls();
                 HeadQuarterName.Value = string.Empty;
                btnDelete.Visible = false;
                HeadQuarterName.Focus();
            }
        }
        private void Updatesociety()
        {
            int retsave = CTB.InsertUpdateBuilding(Convert.ToInt32(ViewState["Id"]), HeadQuarterName.Value, Address.Value, Convert.ToInt32(ddlCity.SelectedValue), Convert.ToInt32(Settings.Instance.SMID), Convert.ToInt32(ddlCountry.SelectedValue), Convert.ToInt32(ddlState.SelectedValue), Convert.ToInt32(txtPincode.Value));

            if (retsave == -1)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Duplicate Headquarter Name Exists');", true);
                HeadQuarterName.Focus();
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Updated Successfully');", true);
                ClearControls();
                HeadQuarterName.Value = string.Empty;
                btnDelete.Visible = false;
                HeadQuarterName.Focus();
                fillRepeter();
                mainDiv.Style.Add("display", "none");
                rptmain.Style.Add("display", "block");
            }
        }
        private void ClearControls()
        {
            ddlHeadquarterlist.SelectedIndex = 0;


            HeadQuarterName.Value = string.Empty;
            Address.Value = string.Empty;
            ddlCountry.SelectedIndex = 0;
            ddlState.Items.Clear();
            ddlCity.Items.Clear();
            txtPincode.Value = string.Empty;
            btnDelete.Visible = false;
            btnSave.Text = "Save";
            rpt.DataSource = null;
            rpt.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            fillRepeter();

        }
        protected void btnResetSearch_Click(object sender, EventArgs e)
        {
            rpt.DataSource = null;
            rpt.DataBind();
            ddlHeadquarterlist.SelectedValue = "0";
           // fillRepeter();
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearControls();
            Response.Redirect("~/MastHeadquarter.aspx");
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string confirmValue = Request.Form["confirm_value"];
            if (confirmValue == "Yes")
            {
                int retdel = CTB.delete(Convert.ToString(ViewState["Id"]));
                if (retdel > 0)
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Deleted Successfully');", true);
                    ClearControls();
                    fillRepeter();
                    mainDiv.Style.Add("display", "none");
                    rptmain.Style.Add("display", "block");
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record cannot be deleted as it is in use');", true);
                    ClearControls();
                    HeadQuarterName.Focus();
                }
            }   
        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {



            HeadQuarterName.Value = string.Empty;
            Address.Value = string.Empty;
            ddlCountry.SelectedIndex = 0;
            ddlState.Items.Clear();
            ddlCity.Items.Clear();
            txtPincode.Value = string.Empty;
            btnDelete.Visible = false;
            btnSave.Text = "Save"; 
            mainDiv.Style.Add("display", "block");
            rptmain.Style.Add("display", "none");
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            fillRepeter();
            HeadQuarterName.Value = string.Empty;
            btnDelete.Visible = false;
            btnSave.Text = "Save";
            mainDiv.Style.Add("display", "none");
            rptmain.Style.Add("display", "block");
        }
    }
}