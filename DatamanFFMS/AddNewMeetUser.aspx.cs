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
using DAL;
using System.IO;

namespace AstralFFMS
{
    public partial class AddNewMeetUser : System.Web.UI.Page
    {
        int msg = 0;
        int uid = 0;
        int smID = 0;
        int dsrDays = 0;
        string VisitID = "0";

        BAL.Meet.AddMeetUsersBAL dp = new BAL.Meet.AddMeetUsersBAL();
        string parameter = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            string pageName = Path.GetFileName(Request.Path);         
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;

            if (btnSave.Text == "Save")
            {
                btnSave.Enabled = Settings.Instance.CheckAddPermission(pageName, Convert.ToString(Session["user_name"]));
                btnSave.CssClass = "btn btn-primary";
            }
            else
            {
                btnSave.Enabled = Settings.Instance.CheckEditPermission(pageName, Convert.ToString(Session["user_name"]));
                btnSave.CssClass = "btn btn-primary";
            }

            parameter = Request["__EVENTARGUMENT"];

            if (parameter != "")
            {
                ViewState["VisId"] = parameter;

               
                Settings.Instance.VistID = Convert.ToString(ViewState["VisId"]);
                FillDeptControls(Convert.ToInt32(parameter));
                mainDiv.Style.Add("display", "block");
                rptmain.Style.Add("display", "none");

            }


            if (!IsPostBack)
            {
                try
                {
                    fillUnderUsers();
                    mainDiv.Style.Add("display", "block");
                  //  BindDDlArea();
                    // fillMeet();
                    btnDelete.Visible = false;
                    ddlbeat.Items.Insert(0, new ListItem("-- Select --", "0"));
                    fillPartyType();

                      fillMeetType();
                    ddlmeetName.Items.Insert(0, new ListItem("-- Select --", "0"));
                }
                catch
                {
                }
            }
            
        }

        private void fillMeetType()
        {
            string query = "select * from MastMeetType order by Name ";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, query);
            if (dt.Rows.Count > 0)
            {
                ddlmeetType.DataSource = dt;
                ddlmeetType.DataTextField = "Name";
                ddlmeetType.DataValueField = "Id";
                ddlmeetType.DataBind();

                ddlmeettype1.DataSource = dt;
                ddlmeettype1.DataTextField = "Name";
                ddlmeettype1.DataValueField = "Id";
                ddlmeettype1.DataBind();


            }
            ddlmeetType.Items.Insert(0, new ListItem("-- Select --", "0"));
            ddlmeettype1.Items.Insert(0, new ListItem("-- Select --", "0"));
        }
        protected void ddlmeetType_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillInitialRecords();

        }
        protected void ddlunderUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlmeetType.SelectedIndex = 0;
            btnSave.Text = "Save";
        }
        protected void ddlmeetName_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strCity = string.Format("Select MeetLoaction From TransMeetPlanEntry Where MeetPlanId='{0}'", ddlmeetName.SelectedValue);
            string CityId =Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, strCity));
           // this.BindDDlBeat(CityId);
            this.BindDDlArea(CityId);
        }
        //private void fillUnderUsers()
        //{
        //    DataTable d = Settings.UnderUsers(Settings.Instance.SMID);
        //    if (d.Rows.Count > 0)
        //    {
        //        try
        //        {
        //            DataView dv = new DataView(d);
        //            dv.RowFilter = "RoleType='AreaIncharge' OR RoleType='CityHead'";
        //            ddlunderUser.DataSource = dv;
        //            ddlunderUser.DataTextField = "SMName";
        //            ddlunderUser.DataValueField = "SMId";
        //            ddlunderUser.DataBind();
        //        }
        //        catch { }

        //    }
        //}
        private void fillUnderUsers()
        {

            string Role = "Select RoleType from Mastrole mr left join Mastsalesrep ms on mr.RoleId=ms.RoleId where ms.Smid in (" + Settings.Instance.SMID + ") ";
            DataTable dtRole = new DataTable();
            dtRole = DbConnectionDAL.GetDataTable(CommandType.Text, Role);
            string RoleTy = dtRole.Rows[0]["RoleType"].ToString();
            try
            {
                if (RoleTy != "AreaIncharge")
                {
                    DataTable d = Settings.UnderUsers(Settings.Instance.SMID);
                    DataView dv = new DataView(d);
                    dv.RowFilter = "RoleType='CityHead' or RoleType='DistrictHead' or RoleType='AreaIncharge' and SMName<>'.'";
                    ddlunderUser.DataSource = dv;
                    ddlunderUser.DataTextField = "SMName";
                    ddlunderUser.DataValueField = "SMId";
                    ddlunderUser.DataBind();
                    ddlunderUser.SelectedValue = Settings.Instance.SMID;
                }
                else
                {
                    DataTable d = Settings.UnderUsers(Settings.Instance.SMID);
                    if (d.Rows.Count > 0)
                    {
                        DataView dv = new DataView(d);
                        dv.RowFilter = "RoleType='AreaIncharge'";
                        ddlunderUser.DataSource = dv;
                        ddlunderUser.DataTextField = "SMName";
                        ddlunderUser.DataValueField = "SMId";
                        ddlunderUser.DataBind();
                        ddlunderUser.SelectedValue = Settings.Instance.SMID;

                    }
                }
            }
            catch
            { }

        }

        private void fillInitialRecords()
        {
            ddlmeetName.Items.Clear();
         

            //string str = @"select * from TransMeetPlanEntry where [SMId]=" + Settings.DMInt32(ddlunderUser.SelectedValue) + " and MeetTypeId=" + Settings.DMInt32(ddlmeetType.SelectedValue) + "  order by MeetDate desc";
            string str = @"select mp.MeetPlanId,mp.MeetName from TransMeetPlanEntry mp LEFT JOIN TransMeetExpense me  ON mp.MeetPlanId = me.MeetPlanId where mp.AppStatus IN ('Pending','Approved') AND mp.SMId=" + Settings.DMInt32(ddlunderUser.SelectedValue) + " and mp.MeetTypeId=" + Settings.DMInt32(ddlmeetType.SelectedValue) + " AND me.FinalApprovedAmount IS null order by mp.MeetDate desc";
         
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (dt.Rows.Count > 0)
            {
                ddlmeetName.DataSource = dt;
                ddlmeetName.DataTextField = "MeetName";
                ddlmeetName.DataValueField = "MeetPlanId";
                ddlmeetName.DataBind();
            }
            ddlmeetName.Items.Insert(0, new ListItem("-- Select --", "0"));
           
        }

        private void fillInitialRecords1()
        {
            ddlmeet1.Items.Clear();
            string str = @"select mp.MeetPlanId,mp.MeetName from TransMeetPlanEntry mp LEFT JOIN TransMeetExpense me  ON mp.MeetPlanId = me.MeetPlanId where mp.AppStatus IN ('Pending','Approved') AND mp.SMId=" + Settings.DMInt32(ddlunderUser.SelectedValue) + " and mp.MeetTypeId=" + Settings.DMInt32(ddlmeettype1.SelectedValue) + " AND me.FinalApprovedAmount IS null order by mp.MeetDate desc";        
            //   string str = @"select * from TransMeetPlanEntry   where AppStatus='Approved' and SMId=" + ddlunderUser.SelectedValue + " and MeetTypeId=" + Settings.DMInt32(ddlmeetType.SelectedValue) + "  order by MeetDate desc";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (dt.Rows.Count > 0)
            {   
                ddlmeet1.DataSource = dt;
                ddlmeet1.DataTextField = "MeetName";
                ddlmeet1.DataValueField = "MeetPlanId";
                ddlmeet1.DataBind();
            }
         
            ddlmeet1.Items.Insert(0, new ListItem("-- Select --", "0"));
        }

        private void fillPartyType()
        {
            string s = "select * from PartyType";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, s);
            if (dt.Rows.Count > 0)
            {
                ddlpartyType.DataSource = dt;
                ddlpartyType.DataTextField = "PartyTypeName";
                ddlpartyType.DataValueField = "PartyTypeId";
                ddlpartyType.DataBind();
            }
            ddlpartyType.Items.Insert(0, new ListItem("-- Select --", "0"));
        }

        //private void fillMeet()
        //{
        //    string s = "select * from [TransMeetPlanEntry] where SMID="+Settings.DMInt32(Settings.Instance.SMID);
        //    DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, s);
        //    if(dt.Rows.Count>0)
        //    {
        //        ddlmeetName.DataSource = dt;
        //        ddlmeetName.DataTextField = "MeetName";
        //        ddlmeetName.DataValueField = "MeetPlanId";
        //        ddlmeetName.DataBind();

        //        ddlmeet1.DataSource = dt;
        //        ddlmeet1.DataTextField = "MeetName";
        //        ddlmeet1.DataValueField = "MeetPlanId";
        //        ddlmeet1.DataBind();
        //    }
        //    ddlmeetName.Items.Insert(0, new ListItem("-- Select --", "0"));
        //    ddlmeet1.Items.Insert(0, new ListItem("-- Select --", "0"));
        //}

        private void fillRepeter()
        {
            try
            {

                string str = @"  select A.*,M.AreaName,p.MeetName from [TransAddMeetUser] A left  join MastArea M on A.[AreaId]=M.AreaId
                             left join TransMeetPlanEntry P on A.[MeetId]=P.MeetPlanId where A.MeetId=" +Settings.DMInt32(ddlmeet1.SelectedValue);
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
            catch
            {

            }
        }

        private void BindDDlArea(string CityId)
        {
            ddlarea.Items.Clear();
            if (CityId != "")
            {
                string str = @"select * from mastarea where areaid in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA'
            and PrimCode=" + ddlunderUser.SelectedValue + ") And UnderId In(" + CityId + ")  and   areatype='Area' and Active=1 order by AreaName";
                // string str = @"select * from MastArea where AreaType='AREA'";
                DataTable obj = new DataTable();
                obj = DbConnectionDAL.GetDataTable(CommandType.Text, str);

                if (obj.Rows.Count > 0)
                {
                    try
                    {
                        ddlarea.DataSource = obj;
                        ddlarea.DataTextField = "AreaName";
                        ddlarea.DataValueField = "AreaId";
                        ddlarea.DataBind();
                    }
                    catch { }
                }
                ddlarea.Items.Insert(0, new ListItem("-- Select --", "0"));
            }
        }

        private void BindDDlBeat(string areaId)
        {
            ddlbeat.Items.Clear();
            string str = @"select * from MastArea where UnderId="+Settings.DMInt32(ddlarea.SelectedValue)+"  and Active=1 order by AreaName";
            DataTable obj = new DataTable();
            obj = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (obj.Rows.Count > 0)
            {
                try
                {
                    ddlbeat.DataSource = obj;
                    ddlbeat.DataTextField = "AreaName";
                    ddlbeat.DataValueField = "AreaId";
                    ddlbeat.DataBind();
                }
                catch { }
            }
            ddlbeat.Items.Insert(0, new ListItem("-- Select --", "0"));
        }

        private void FillDeptControls(int VisId)
        {
            try
            {
                string str = @"select * from TransAddMeetUser  where Id=" + VisId;
                DataTable deptValueDt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                if (deptValueDt.Rows.Count > 0)
                {
                    btnSave.Text = "Update";

                    //Added on 16-12-2015
                    string meetQry = @"select MeetTypeId from TransMeetPlanEntry where MeetPlanId=" + deptValueDt.Rows[0]["MeetId"].ToString() + "";
                    ddlmeetType.SelectedValue = DbConnectionDAL.GetScalarValue(CommandType.Text, meetQry).ToString();
                    fillInitialRecords();
                    //End

                    ddlmeetName.SelectedValue=deptValueDt.Rows[0]["MeetId"].ToString();

                    txtaddress.Text = deptValueDt.Rows[0]["Address"].ToString();
                    ddlarea.SelectedValue = deptValueDt.Rows[0]["AreaId"].ToString();

                 //  this.BindDDlBeat();
                    ddlbeat.SelectedValue = deptValueDt.Rows[0]["BeatId"].ToString();
                    txtContactperson.Text = deptValueDt.Rows[0]["ContactPersonName"].ToString();
                    txtemail.Text = deptValueDt.Rows[0]["EmailId"].ToString();
                    txtmobile.Text = deptValueDt.Rows[0]["MobileNo"].ToString();
                    txtPotential.Text = deptValueDt.Rows[0]["Potential"].ToString();
                    txtDOB.Text = deptValueDt.Rows[0]["DOB"].ToString();
                    btnDelete.Visible = true;
                    txtName.Text = deptValueDt.Rows[0]["Name"].ToString();
                    ddlpartyType.SelectedValue = deptValueDt.Rows[0]["PartyType"].ToString();
                }

            }
            catch (Exception ex)
            {
                ex.ToString();
                //  System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while inserting the records');", true);
            }
        }

        private int CheckMobileNoUpdate()
        {
            if (txtmobile.Text != "")
            {
                string str = "select count(*) from TransAddMeetUser where MeetId=" + ddlmeetName.SelectedValue + " and MobileNo='" + txtmobile.Text + "' and MobileNo!=9999999999 and Id !=" + ViewState["VisId"];
            int exists = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, str));
            return exists;
             }
            else
            {
                return 0;
            }
        }
        private int CheckEmailUpdate()
        {
            if (txtemail.Text != "")
            {
                string str = "select count(*) from TransAddMeetUser where MeetId=" + ddlmeetName.SelectedValue + " and EmailId='" + txtemail.Text + "' and Id !=" + ViewState["VisId"];
                int exists = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, str));
                return exists;
            }
            else
            {
                return 0;
            }
        }

        private int  CheckMobileNo()
        {
            if (txtmobile.Text != "")
            {
                string str = "select count(*) from TransAddMeetUser where MeetId=" + ddlmeetName.SelectedValue + " and MobileNo='" + txtmobile.Text + "' and MobileNo!=9999999999";
                int exists = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, str));
                return exists;

            }
            else
            {
                return 0;
            }
        }
        private int CheckEmail()
        {
            if (txtemail.Text != "")
            {
                string str = "select count(*) from TransAddMeetUser where MeetId=" + ddlmeetName.SelectedValue + " and EmailId='" + txtemail.Text+"'";
                int exists = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, str));
                return exists;
            }
            else
            {
                return 0;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            btnSave.Enabled = false;
            try
            {
                string strSql = @"select MeetDate from TransMeetPlanEntry where MeetPlanId=" +ddlmeetName.SelectedValue + "";
                DateTime MeetDate = Convert.ToDateTime(DbConnectionDAL.GetScalarValue(CommandType.Text, strSql));
              //  DateTime startdate = DateTime.ParseExact(MeetDate.Trim(), "dd/MM/yyyy", provider);
                if (MeetDate > DateTime.Today)
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('You can not upload meet user entry  before meet plan date');", true);
                    return;
                }
                else
                {
                    if (btnSave.Text == "Update")
                    {
                        if (CheckMobileNoUpdate() > 0)
                        {
                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('This Mobile No Already Exists');", true);
                        }
                        else if (CheckEmailUpdate() > 0)
                        {
                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('This Email Already Exists');", true);
                        }
                        else
                        {
                            UpdateRecord();
                        }
                    }
                    else
                    {
                        if (CheckMobileNo() > 0)
                        {
                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('This Mobile No Already Exists');", true);
                        }
                        else if (CheckEmail() > 0)
                        {
                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('This Email Already Exists');", true);
                        }
                        else
                        {
                            InsertRecord();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while inserting the records');", true);
            }
            btnSave.Enabled = true;
        }

        private void InsertRecord()
        {
          
            int retsave = dp.InsertMeetParty(Settings.DMInt32(ddlmeetName.SelectedValue), Settings.DMInt32(ddlarea.SelectedValue), Settings.DMInt32(ddlbeat.SelectedValue), txtaddress.Text, txtContactperson.Text, txtmobile.Text, txtemail.Text,txtName.Text,Settings.DMDecimal(txtPotential.Text),txtDOB.Text,Settings.DMInt32(ddlpartyType.SelectedValue));
            if (retsave != 0)
            {
                Settings.Instance.VistID = Convert.ToString(retsave);
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Inserted Successfully-" + retsave + "');", true);
                ClearControls();
                 btnDelete.Visible = false;
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record Cannot be Insert');", true);
            }

        }

        private void Reset()
        {
           
          
        }



        private void ClearControls()
        {
            btnDelete.Visible = false;
            btnSave.Text = "Save";
            ddlpartyType.SelectedIndex = 0;
            ddlmeetName.SelectedIndex = 0;
            ddlmeetType.SelectedIndex = 0;
            ddlmeet1.SelectedIndex = 0;
            ddlbeat.Items.Clear();
            ddlarea.SelectedIndex = 0;
            txtaddress.Text = "";
            txtContactperson.Text = "";
            txtemail.Text = "";
            txtmobile.Text = "";
            txtName.Text = "";
            txtPotential.Text = "";
        }

        private void UpdateRecord()
        {
            int retsave = dp.UpdateMeetParty(Convert.ToInt32(ViewState["VisId"]), Settings.DMInt32(ddlmeetName.SelectedValue), Settings.DMInt32(ddlarea.SelectedValue), Settings.DMInt32(ddlbeat.SelectedValue), txtaddress.Text, txtContactperson.Text, txtmobile.Text, txtemail.Text, txtName.Text, Settings.DMDecimal(txtPotential.Text), Convert.ToDateTime(txtDOB.Text), Settings.DMInt32(ddlpartyType.SelectedValue));
            if (retsave == 1)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Updated Successfully');", true);
                btnDelete.Visible = false;
                btnSave.Text = "Save";
                ClearControls();
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record cannoy be Update');", true);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {

            Response.Redirect("~/AddNewMeetUser.aspx");
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string confirmValue = Request.Form["confirm_value"];
            if (confirmValue == "Yes")
            {
                int retdel = dp.delete(Convert.ToString(ViewState["VisId"]));
                if (retdel == 1)
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Deleted Successfully');", true);
                    btnDelete.Visible = false;
                    btnSave.Text = "Save";
                    ClearControls();

                }
            }
            else
            {
                // this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('You clicked NO!')", true);
            }


        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            mainDiv.Style.Add("display", "block");
            rptmain.Style.Add("display", "none");
        }
        protected void btnFind_Click(object sender, EventArgs e)
        {
            rpt.DataSource = null;
            rpt.DataBind();
           // fillRepeter();
            btnDelete.Visible = false;
            btnSave.Text = "Save";
            mainDiv.Style.Add("display", "none");
            rptmain.Style.Add("display", "block");
        }

        protected void ddlarea_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlbeat.Items.Clear();
            if(ddlarea.SelectedIndex > 0)
            {
                this.BindDDlBeat(ddlbeat.SelectedValue);
            }
            else
            {
                ddlbeat.Items.Insert(0, new ListItem("-- Select --", "0"));
            }
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            this.fillRepeter();
        }

        protected void ddlmeettype1_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillInitialRecords1();
        }


        public IFormatProvider provider { get; set; }
    }
}









