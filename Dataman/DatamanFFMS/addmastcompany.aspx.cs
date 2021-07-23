using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using BAL;
using System.Data;
using BusinessLayer;
using System.Web.Services;
using System.Text.RegularExpressions;

namespace AstralFFMS
{
    public partial class addmastcompany : System.Web.UI.Page
    {
        CRMBAL CB = new CRMBAL();
        string parameter = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            parameter = Request["__EVENTARGUMENT"];
            if ((parameter != ""))
            {
                Page.Form.DefaultButton = btnSave.UniqueID;
                this.Form.DefaultButton = this.btnSave.UniqueID;
                ViewState["id"] = parameter;
                FillControls(Convert.ToInt32(parameter));
                mainDiv.Style.Add("display", "block");
                rptmain.Style.Add("display", "none");
            }
            if (!Page.IsPostBack)
            {
                BindCountry();
            }
        }
        private void BindCountry()
        {
            string strC = "select AreaID,AreaName from mastarea where AreaType='Country' and Active='1' order by AreaName";
            fillDDLDirect(ddlcountry, strC, "AreaID", "AreaName", 1);
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
        [WebMethod]
        private static int SaveData1()
        {
            int val = 0;

            return val;
        }
        private void fillRepeter()
        {
            string str = @"select * from CRM_MastLeadCompany order by CompName";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            rpt.DataSource = dt;
            rpt.DataBind();
        }
        private void FillControls(int id)
        {
            try
            {
                string Qry = @"select * from CRM_MastLeadCompany where id=" + id;
                DataTable ValueDt = DbConnectionDAL.GetDataTable(CommandType.Text, Qry);
                if (ValueDt.Rows.Count > 0)
                {
                    Compname.Value = ValueDt.Rows[0]["Compname"].ToString();
                    Desc.Value = ValueDt.Rows[0]["CompDesc"].ToString();
                    Phone.Value = ValueDt.Rows[0]["CompPhone"].ToString();
                    Url.Value = ValueDt.Rows[0]["CompUrl"].ToString();
                    add.Value = ValueDt.Rows[0]["Compadd"].ToString();
                    City.Value = ValueDt.Rows[0]["City"].ToString();
                    State.Value = ValueDt.Rows[0]["State"].ToString();
                    Zip.Value = ValueDt.Rows[0]["Zip"].ToString();
                    ddlcountry.SelectedValue = ValueDt.Rows[0]["countryid"].ToString();
                    btnSave.Text = "Update";
                    btnDelete.Visible = true;
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while showing the records');", true);
            }
        }
        private void Insert()
        {
            //Compname.Value = ValueDt.Rows[0]["Compname"].ToString();
            //        Desc.Value = ValueDt.Rows[0]["CompDesc"].ToString();
            //        Phone.Value = ValueDt.Rows[0]["CompPhone"].ToString();
            //        Url.Value = ValueDt.Rows[0]["CompUrl"].ToString();
            //        add.Value = ValueDt.Rows[0]["Compadd"].ToString();
            //        City.Value = ValueDt.Rows[0]["City"].ToString();
            //        State.Value = ValueDt.Rows[0]["State"].ToString();
            //        Zip.Value = ValueDt.Rows[0]["Zip"].ToString();
            //        ddlcountry.SelectedValue = 
            //(int id, string CompName, string CompDesc, string CompPhone, string CompUrl, string CompAdd, string city, string State, int zip, int countryid,  string flag)
            int retval = CB.InsertMastleadcompany(0, Compname.Value, Desc.Value, Phone.Value, Url.Value, add.Value, City.Value, State.Value, Convert.ToInt32(Zip.Value), Convert.ToInt32(ddlcountry.SelectedValue.ToString()), "I");
            if (retval == -1)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error While Inserting Records');", true);

                ClearControls();
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Inserted Successfully');", true);
                ClearControls();

            }
        }
        private void Update()
        {
            //        int retval = CB.InsertMastContacsType(Convert.ToInt32(ViewState["id"]), ddlphonetype1.SelectedValue.ToString(), Contactstype.Value, Convert.ToInt32(Txtsort.Value), "U");
            int retval = CB.InsertMastleadcompany(Convert.ToInt32(ViewState["id"]), Compname.Value, Desc.Value, Phone.Value, Url.Value, add.Value, City.Value, State.Value, Convert.ToInt32(Zip.Value), Convert.ToInt32(ddlcountry.SelectedValue.ToString()), "U");
            if (retval == -1)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error While Updating Records');", true);
                ClearControls();
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Updated Successfully');", true);
                ClearControls();
                btnDelete.Visible = false;
                btnSave.Text = "Save";
            }
        }
        protected void btnFind_Click(object sender, EventArgs e)
        {
            fillRepeter();
            //ClearControls();
            btnDelete.Visible = false;
            btnSave.Text = "Save";
            mainDiv.Style.Add("display", "none");
            rptmain.Style.Add("display", "block");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if(ddlcountry.SelectedIndex ==0)
                {

                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please Select Country');", true);
                    return;
                }
                Compname.Focus();
                Desc.Focus();
                Phone.Focus();
                Url.Focus();
                add.Focus();
                City.Focus();
                State.Focus();
                Zip.Focus();
                if (btnSave.Text == "Update")
                {
                    Update();
                }
                else
                {
                    Insert();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while inserting the records');", true);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string confirmValue = Request.Form["confirm_value"];
            if (confirmValue == "Yes")
            {
                int retval = CB.InsertMastleadcompany(Convert.ToInt32(ViewState["id"]), Compname.Value, Desc.Value, Phone.Value, Url.Value, add.Value, City.Value, State.Value, Convert.ToInt32(Zip.Value), Convert.ToInt32(ddlcountry.SelectedValue.ToString()), "D");
                if (retval != -1)
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Deleted Successfully');", true);
                    ClearControls();
                    btnDelete.Visible = false;
                    btnSave.Text = "Save";
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record cannot be deleted as it is in use');", true);
                    ClearControls();
                }
            }
        }
        protected void ClearControls()
        {
            Compname.Value = "";
            Desc.Value = "";
            Phone.Value = "";
            Url.Value = "";
            add.Value = "";
            City.Value = "";
            State.Value = "";
            Zip.Value = "";
            BindCountry();

        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("addmastcompany.aspx");
        }

        protected void btnback_Click(object sender, EventArgs e)
        {
            mainDiv.Style.Add("display", "block");
            rptmain.Style.Add("display", "none");
        }
    }
}