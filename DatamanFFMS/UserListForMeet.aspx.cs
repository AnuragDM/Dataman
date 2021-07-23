using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLayer;
using System.IO;
using System.Drawing;

namespace AstralFFMS
{
    public partial class UserListForMeet : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;

            if (!IsPostBack)
            {
                fillUnderUsers();
                btnSave.Visible = false;
                fillPartyType();
                fillArea();
                fillMeetType();
              //  fillInitialRecords();
                ddlbeat.Items.Insert(0, new ListItem("-- Select --", "0"));
                ddlmeetName.Items.Insert(0, new ListItem("-- Select --", "0"));
                //string pageName = Path.GetFileName(Request.Path);
                btnexport.Enabled = Settings.Instance.CheckExportPermission(pageName, Convert.ToString(Session["user_name"]));
                btnexport.CssClass = "btn btn-primary";

            }
        }

        //private void fillInitialRecords()
        //{
        //    string str = @"select * from TransMeetPlanEntry where [SMId]=" + Settings.DMInt32(Settings.Instance.SMID) + " and  [AppBy] is Null order by MeetPlanId desc ";
        //    DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
        //    if (dt.Rows.Count > 0)
        //    {
        //        ddlmeetName.DataSource = dt;
        //        ddlmeetName.DataTextField = "MeetName";
        //        ddlmeetName.DataValueField = "MeetPlanId";
        //        ddlmeetName.DataBind();
        //    }
        //    ddlmeetName.Items.Insert(0, new ListItem("-- Select --", "0"));
        //}
        private void fillArea()
        {
            string s = @"select * from mastarea where areaid in (select areaid from MastAreaGrp where MainGrp
                        in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode=" + Settings.Instance.SMID + ")) and areatype='Area' and Active=1 order by AreaName ";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, s);
            if (dt.Rows.Count > 0)
            {
                ddlArea.DataSource = dt;
                ddlArea.DataTextField = "AreaName";
                ddlArea.DataValueField = "AreaId";
                ddlArea.DataBind();
            }
            ddlArea.Items.Insert(0, new ListItem("-- Select --", "0"));

        }
        private void fillbeat()
        {
            ddlbeat.Items.Clear();
            string s = @"select * from mastarea where [UnderId]=" + ddlArea.SelectedValue + "  and Active=1 order by AreaName ";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, s);
            if (dt.Rows.Count > 0)
            {
                ddlbeat.DataSource = dt;
                ddlbeat.DataTextField = "AreaName";
                ddlbeat.DataValueField = "AreaId";
                ddlbeat.DataBind();
            }
            ddlbeat.Items.Insert(0, new ListItem("-- Select --", "0"));

        }
        private void fillPartyType()
        {

            string s = @"select * from PartyType order by PartyTypeName ";
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
        private void fillrepeater()
        {
            string str = "";
            if (ddlArea.SelectedIndex > 0 && ddlbeat.SelectedIndex>0 && ddlpartyType.SelectedIndex > 0)
            {
                str = "select * from [TransAddMeetUser] l left join TransMeetPlanEntry E on l.MeetId=e.MeetPlanId  left join PartyType t on L.PartyType =t.PartyTypeId  where l.MeetId=" + Settings.DMInt32(ddlmeetName.SelectedValue) + " and l.[PartyType]=" + Settings.DMInt32(ddlpartyType.SelectedValue) + " and l.[BeatId]=" + Settings.DMInt32(ddlbeat.SelectedValue) + " and Name like '%"+txtName.Text+"%' and l.[AreaId]=" + ddlArea.SelectedValue;
            }
            else if (ddlArea.SelectedIndex > 0 && ddlbeat.SelectedIndex > 0)
            {
                str = "select * from [TransAddMeetUser] l left join TransMeetPlanEntry E on l.MeetId=e.MeetPlanId  left join PartyType t on L.PartyType =t.PartyTypeId  where l.MeetId=" + Settings.DMInt32(ddlmeetName.SelectedValue) + "and l.[BeatId]=" + Settings.DMInt32(ddlbeat.SelectedValue) + " and Name like '%" + txtName.Text + "%' and l.[AreaId]=" + ddlArea.SelectedValue;
            }

            else if (ddlArea.SelectedIndex > 0 && ddlpartyType.SelectedIndex > 0)
            {
                str = "select * from [TransAddMeetUser] l left join TransMeetPlanEntry E on l.MeetId=e.MeetPlanId  left join PartyType t on L.PartyType =t.PartyTypeId  where l.MeetId=" + Settings.DMInt32(ddlmeetName.SelectedValue) + " and l.[PartyType]=" + Settings.DMInt32(ddlpartyType.SelectedValue) + " and Name like '%" + txtName.Text + "%' and l.[AreaId]=" + ddlArea.SelectedValue;
            }
            else if (ddlArea.SelectedIndex > 0 && ddlpartyType.SelectedIndex >0)
            {
                str = "select * from [TransAddMeetUser] l left join TransMeetPlanEntry E on l.MeetId=e.MeetPlanId  left join PartyType t on L.PartyType =t.PartyTypeId  where l.MeetId=" + Settings.DMInt32(ddlmeetName.SelectedValue) + " and l.[PartyType]=" + Settings.DMInt32(ddlpartyType.SelectedValue) + " and Name like '%" + txtName.Text + "%' and l.[AreaId]=" + ddlArea.SelectedValue;
            }
            else if (ddlArea.SelectedIndex > 0 )
            {
                str = "select * from [TransAddMeetUser] l left join TransMeetPlanEntry E on l.MeetId=e.MeetPlanId  left join PartyType t on L.PartyType =t.PartyTypeId  where l.MeetId=" + Settings.DMInt32(ddlmeetName.SelectedValue) + " and Name like '%" + txtName.Text + "%' and l.[AreaId]=" + ddlArea.SelectedValue;
            }
            else if (ddlpartyType.SelectedIndex>0)
            {
                str = "select * from [TransAddMeetUser] l left join TransMeetPlanEntry E on l.MeetId=e.MeetPlanId  left join PartyType t on L.PartyType =t.PartyTypeId  where Name like '%" + txtName.Text + "%' and l.[PartyType]=" + Settings.DMInt32(ddlpartyType.SelectedValue) + " and l.MeetId=" + Settings.DMInt32(ddlmeetName.SelectedValue);
            }
            else  
            {
                str = "select * from [TransAddMeetUser] l left join TransMeetPlanEntry E on l.MeetId=e.MeetPlanId  left join PartyType t on L.PartyType =t.PartyTypeId  where Name like '%" + txtName.Text + "%' and l.MeetId=" + Settings.DMInt32(ddlmeetName.SelectedValue);
            }

            DataTable deptValueDt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (deptValueDt.Rows.Count > 0)
            {
                btnSave.Visible = true;
                //GridView1.DataSource = deptValueDt;
                //GridView1.DataBind();
                rpt.DataSource = deptValueDt;
                rpt.DataBind();
            }
            else
            {
                btnSave.Visible = false;
                //GridView1.DataSource = null;
                //GridView1.DataBind();
                //Added
                rpt.DataSource = null;
                rpt.DataBind();
                //End
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            fillrepeater();
        }

        protected void ddlArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlArea.SelectedIndex > 0)
            {
                this.fillbeat();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string str = "";
                for (int i = 0; i < rpt.Items.Count; i++)
                {
                    bool status = false;
                    CheckBox chk = (CheckBox)rpt.Items[i].FindControl("chkRow");
                    TextBox txtremark = (TextBox)rpt.Items[i].FindControl("txtremark");
                    HiddenField hid = (HiddenField)rpt.Items[i].FindControl("hidParty");
                    HiddenField hid1 = (HiddenField)rpt.Items[i].FindControl("HiddenField1");


                    if (chk.Checked)
                    { status = true; }
                    else { status = false; }
                    try
                    {

                        str = "update TransAddMeetUser set MeetActive='" + status + "' where Id=" + Settings.DMInt32(hid1.Value) + "";
                       DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
                        //int update = ;
                       
                           // System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Inserted Successfully');", true);
                         //  ddlArea.SelectedIndex = 0;
                         //  ddlbeat.SelectedIndex = 0;
                        //   ddlpartyType.SelectedIndex = 0;
                            //GridView1.DataSource = null;
                          //  GridView1.DataBind();
                          //  ddlmeetName.SelectedIndex = 0;
                       
                    }
                    catch
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record Cannot be Insert');", true);
                    }

                }
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Inserted Successfully');", true);
                ddlArea.SelectedIndex = 0;
                ddlbeat.SelectedIndex = 0;
                ddlpartyType.SelectedIndex = 0;
                //GridView1.DataSource = null;
                //GridView1.DataBind();
                //Added
                rpt.DataSource = null;
                rpt.DataBind();
                //End
                ddlmeetType.SelectedIndex = 0;

                ddlmeetName.Items.Clear();
                ddlmeetName.Items.Insert(0, new ListItem("-- Select --", "0"));
                //ddlmeetName.SelectedIndex = 0;

            }
            catch (Exception ex) { }
        }
        private void fillMeetType()
        {
            string query = "select * from MastMeetType order by Name";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, query);
            if (dt.Rows.Count > 0)
            {
                ddlmeetType.DataSource = dt;
                ddlmeetType.DataTextField = "Name";
                ddlmeetType.DataValueField = "Id";
                ddlmeetType.DataBind();
            }
            ddlmeetType.Items.Insert(0, new ListItem("-- Select --", "0"));
        }

        //private void fillUnderUsers()
        //{
        //    DataTable d = Settings.UnderUsers(Settings.Instance.SMID);
        //    if (d.Rows.Count > 0)
        //    {
        //        try
        //        {
        //            DataView dv = new DataView(d);
        //            dv.RowFilter = "RoleType='AreaIncharge'";
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
            DataTable d = Settings.UnderUsers(Settings.Instance.SMID);
            if (d.Rows.Count > 0)
            {
                try
                {
                    DataView dv = new DataView(d);
                    dv.RowFilter = "RoleType='AreaIncharge' OR RoleType='CityHead' OR RoleType='DistrictHead'";
                    ddlunderUser.DataSource = dv;
                    ddlunderUser.DataTextField = "SMName";
                    ddlunderUser.DataValueField = "SMId";
                    ddlunderUser.DataBind();
                    ddlunderUser.SelectedValue = Settings.Instance.SMID;
                }
                catch { }

            }
        }
        private void fillMeet()
        {
            ddlmeetName.Items.Clear();
            //string s = "select * from [TransMeetPlanEntry] where SMID=" + Settings.DMInt32(ddlunderUser.SelectedValue) + " and  [AppBy] is Null and   MeetTypeId=" + ddlmeetType.SelectedValue + " order by MeetPlanId desc";
            //string s = "select * from [TransMeetPlanEntry] where SMID=" + Settings.DMInt32(ddlunderUser.SelectedValue) + " and MeetTypeId=" + ddlmeetType.SelectedValue + " order by MeetPlanId desc";
            string s = "select * from [TransMeetPlanEntry] mp LEFT JOIN TransMeetExpense me  ON mp.MeetPlanId = me.MeetPlanId where mp.SMID=" + Settings.DMInt32(ddlunderUser.SelectedValue) + " and mp.MeetTypeId=" + ddlmeetType.SelectedValue + " AND me.FinalApprovedAmount IS null order by mp.MeetPlanId desc";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, s);
            if (dt.Rows.Count > 0)
            {
                ddlmeetName.DataSource = dt;
                ddlmeetName.DataTextField = "MeetName";
                ddlmeetName.DataValueField = "MeetPlanId";
                ddlmeetName.DataBind();
            }
            ddlmeetName.Items.Insert(0, new ListItem("-- Select --", "0"));

        }

        protected void ddlmeetType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlmeetType.SelectedIndex > 0)
            {
                ddlmeetName.Enabled = true;
                fillMeet();
            }
            else
            {
                ddlmeetName.Items.Clear();
                ddlmeetName.Items.Insert(0, new ListItem("-- Select --", "0"));
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
      

        protected void btnexport_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", string.Format("attachment;filename={0}", "MeetUserList- " + ddlmeetName.SelectedItem.Text + ".xls"));
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            rpt.RenderControl(hw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
         //   if (rpt.Items.Count > 0)
         //   {
         //       Response.ClearContent();
         //       Response.Buffer = true;
         //       Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "MeetUserList- "+ddlmeetName.SelectedItem.Text+".xls"));
         //       Response.ContentType = "application/ms-excel";
         //       StringWriter sw = new StringWriter();
         //       HtmlTextWriter htw = new HtmlTextWriter(sw);
               
         ////       GridView1.HeaderRow.Style.Add("background-color", "#FFFFFF");
         //       //Applying stlye to gridview header cells
         //       //for (int i = 0; i < GridView1.HeaderRow.Cells.Count; i++)
         //       //{
         //       //    GridView1.HeaderRow.Cells[i].Style.Add("background-color", "#df5015");
         //       //}


         //       rpt.RenderControl(htw);
         //       Response.Write(sw.ToString());
         //       Response.End();
         //   }
        }
        public override void VerifyRenderingInServerForm(Control control)
        {

        }

        protected void ddlunderUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlArea.SelectedIndex = 0;
            ddlbeat.SelectedIndex = 0;
            ddlpartyType.SelectedIndex = 0;
            //GridView1.DataSource = null;
            //GridView1.DataBind();
            //Added
            rpt.DataSource = null;
            rpt.DataBind();
            //End
            ddlmeetType.SelectedIndex = 0;

            ddlmeetName.Items.Clear();
            ddlmeetName.Items.Insert(0, new ListItem("-- Select --", "0"));

        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            ddlArea.SelectedIndex = 0;
            ddlbeat.SelectedIndex = 0;
            ddlpartyType.SelectedIndex = 0;
            //GridView1.DataSource = null;
            //GridView1.DataBind();
            rpt.DataSource = null;
            rpt.DataBind();
            ddlmeetType.SelectedIndex = 0;

            ddlmeetName.Items.Clear();
            ddlmeetName.Items.Insert(0, new ListItem("-- Select --", "0"));
        }

        protected void rpt_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

        }

        protected void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox btn = (CheckBox)sender;
            var item = (RepeaterItem)btn.NamingContainer;
            CheckBox chkAllBox = (CheckBox)item.FindControl("chkAll");
            for (int i = 0; i < rpt.Items.Count; i++)
            {
                CheckBox chk = (CheckBox)rpt.Items[i].FindControl("chkRow");
                if (chkAllBox.Checked)
                {
                    chk.Checked = true;
                }
                else
                {
                    chk.Checked = false;
                }
            }
        }
     
    }
}