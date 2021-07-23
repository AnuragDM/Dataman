using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLayer;
using BAL;
using System.IO;

namespace AstralFFMS
{
    public partial class MeetAttendenceEntry : System.Web.UI.Page
    {
        BAL.Meet.MeetAttendance MA = new BAL.Meet.MeetAttendance();
        protected void Page_Load(object sender, EventArgs e)
        {
            string pageName = Path.GetFileName(Request.Path);          
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;

            if (btnsubmit.Text == "Submit")
            {
                btnsubmit.Enabled = Settings.Instance.CheckAddPermission(pageName, Convert.ToString(Session["user_name"]));
                btnsubmit.CssClass = "btn btn-primary";
            }
            else
            {
                btnsubmit.Enabled = Settings.Instance.CheckEditPermission(pageName, Convert.ToString(Session["user_name"]));
                btnsubmit.CssClass = "btn btn-primary";
            }
            if (!IsPostBack)
            {
                fillMeetType();
              // fillInitialRecords();
                ddlMeet.Items.Insert(0, new ListItem("-- Select --", "0"));
                btnsubmit.Visible = false;
            }

        }

        private void fillMeetType()
        {
            ddlmeetTye.Items.Clear();
            string str = @"select * from MastMeetType  order by Name";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (dt.Rows.Count > 0)
            {
                ddlmeetTye.DataSource = dt;
                ddlmeetTye.DataTextField = "Name";
                ddlmeetTye.DataValueField = "Id";
                ddlmeetTye.DataBind();
            }
            ddlmeetTye.Items.Insert(0, new ListItem("-- Select --", "0"));


        }

        private void fillInitialRecords()
        {
            ddlMeet.Items.Clear();
            string str = @"select * from TransMeetPlanEntry mp LEFT JOIN TransMeetExpense me  ON mp.MeetPlanId = me.MeetPlanId where mp.AppStatus='Approved' and mp.Smid in (SELECT ms.smid from mastsalesrep ms LEFT JOIN MastRole mr ON ms.RoleId= mr.RoleId WHERE ms.SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN ( " + Settings.Instance.SMID + ")) AND mr.RoleType IN ('AreaIncharge','CityHead','DistrictHead')) and mp.MeetTypeId=" + ddlmeetTye.SelectedValue + "  AND me.FinalApprovedAmount IS null order by mp.MeetDate desc";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (dt.Rows.Count > 0)
            {
                ddlMeet.DataSource = dt;
                ddlMeet.DataTextField = "MeetName";
                ddlMeet.DataValueField = "MeetPlanId";
                ddlMeet.DataBind();
            }
            ddlMeet.Items.Insert(0, new ListItem("-- Select --", "0"));
        }
        private void fillgrid()
        {
            try
            {
                string str = @"select * from TransAddMeetUser where MeetId=" + Settings.DMInt32(ddlMeet.SelectedValue)+" and [MeetActive]=1";
                            // left join MastIndustry I on p.IndId=I.IndId left join PartyType T on p.PartyType=T.PartyTypeId where l.MeetPlanId=" + Settings.DMInt32(ddlMeet.SelectedValue);
                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                if (dt.Rows.Count > 0)
                {
                    //GridView1.DataSource = dt;
                    //GridView1.DataBind();
                    rpt.DataSource = dt;
                    rpt.DataBind();
                    ddlMeet.Enabled = false;
                    btnsubmit.Visible = true;
                }
                else
                {
                    //GridView1.DataSource =null;
                    //GridView1.DataBind();
                    rpt.DataSource = dt;
                    rpt.DataBind();
                    ddlMeet.Enabled =true;
                    btnsubmit.Visible = false;
                }
            }
            catch (Exception ex)
            { }
        }

        protected void btnsubmit_Click(object sender, EventArgs e)
        {
            try {
                string str = "";
                for (int i = 0; i < rpt.Items.Count; i++)
                {
                    bool status =false;
                    CheckBox chk = (CheckBox)rpt.Items[i].FindControl("chkRow");
                    TextBox txtremark = (TextBox)rpt.Items[i].FindControl("txtremark");
                    HiddenField hid = (HiddenField)rpt.Items[i].FindControl("hidParty");

                    
                    if(chk.Checked)
                    { status = true; }
                    else { status = false; }
                    try
                    {
                        int update = MA.Insert(Settings.DMInt32(hid.Value),status, txtremark.Text);
                        if (update == 1)
                        {
                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Inserted Successfully');", true);
                           
                        }
                        else
                        {
                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record Cannot be Insert');", true);
                        }
                    }
                    catch
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record Cannot be Insert');", true);
                    }

                }
                ddlMeet.SelectedIndex = 0;
                //GridView1.DataSource = null;
                //GridView1.DataBind();
                rpt.DataSource = null;
                rpt.DataBind();
                ddlMeet.Enabled = true;
            
            }
            catch(Exception ex) { }
        }

        protected void btnsave_Click(object sender, EventArgs e)
        {
            try
            {
                this.fillgrid();
            }
            catch
            {

            }
        }

        protected void btncancel_Click(object sender, EventArgs e)
        {
            ddlMeet.SelectedIndex = 0;
            //GridView1.DataSource = null;
            //GridView1.DataBind();
            rpt.DataSource = null;
            rpt.DataBind();
            ddlMeet.Enabled = true;
            btnsubmit.Visible = false;
        }

        protected void ddlmeetTye_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(ddlmeetTye.SelectedIndex>0)
            {
                ddlMeet.Enabled = true;
                fillInitialRecords();
            }
            else
            {
                ddlMeet.Items.Clear();
                ddlMeet.Items.Insert(0, new ListItem("-- Select --", "0"));
            }
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

        protected void btnexport_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", string.Format("attachment;filename={0}", "MeetAttendenceEntry- " + ddlMeet.SelectedItem.Text + ".xls"));
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
       
    }
}