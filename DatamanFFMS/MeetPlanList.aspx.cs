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

namespace AstralFFMS
{
    public partial class MeetPlanList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;

            txtmDate.Attributes.Add("readonly", "readonly");
            txttodate.Attributes.Add("readonly", "readonly");
            if(!IsPostBack)
            {
                fillUnderUsers();
                txtmDate.Text = Settings.GetUTCTime().AddMonths(-1).ToString("dd/MMM/yyyy"); //DateTime.Parse(DateTime.UtcNow.AddSeconds(19800).ToShortDateString()).ToString("dd/MMM/yyyy");
                txttodate.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");// DateTime.Parse(DateTime.UtcNow.AddSeconds(19800).ToShortDateString()).ToString("dd/MMM/yyyy");
            }
        }

        private void fillUnderUsers()
        {
            try
            {          
                    DataTable d = Settings.UnderUsers(Settings.Instance.SMID);
                    DataView dv = new DataView(d);
                    dv.RowFilter = "RoleType='AreaIncharge' or RoleType='CityHead' or RoleType='DistrictHead'";
                    ddlunderUser.DataSource = dv;
                    ddlunderUser.DataTextField = "SMName";
                    ddlunderUser.DataValueField = "SMId";
                    ddlunderUser.DataBind();
                    //Ankita - 20/may/2016- (For Optimization)
                    //string Role = "Select RoleType from Mastrole mr left join Mastsalesrep ms on mr.RoleId=ms.RoleId where ms.Smid in (" + Settings.Instance.SMID + ") ";
                    //DataTable dtRole = new DataTable();
                    //dtRole = DbConnectionDAL.GetDataTable(CommandType.Text, Role);
                    //string RoleTy = dtRole.Rows[0]["RoleType"].ToString();
                    string RoleTy = Settings.Instance.RoleType;
                    if (RoleTy == "CityHead" || RoleTy == "DistrictHead")
                    {
                        ddlunderUser.SelectedValue = Settings.Instance.SMID;
                    }
            }                
            
            catch
            { }             

            
        }
        private void fillMeetPLan()
        {
            try
            {
                string str = "";
                if (Convert.ToDateTime(txttodate.Text) >= Convert.ToDateTime(txtmDate.Text))
                {
                    if (txtmDate.Text != "" && txttodate.Text != "" && ddlstatus.SelectedIndex > 0)
                    {
                        str = @"select MI.Name as IndName,dbo.getPartName(m.MeetPlanId) as PartyName,M.AppAmount, M.FromTime,M.ToTime,M.MeetDate,M.MeetName,MastArea.AreaName,M.MeetPlanId,M.*
 from TransMeetPlanEntry M 
  left join MastArea on MastArea.AreaId=M.MeetLoaction left join MastParty MP on m.RetailerPartyID=MP.PartyId 
  	left join MastItemClass MI on m.IndId=MI.Id where M.AppStatus='" + ddlstatus.SelectedValue + "' and M.MeetDate>='" + Settings.dateformat1(txtmDate.Text) + "' and M.MeetDate<='" + Settings.dateformat1(txttodate.Text) + "' and M.SMId=" + ddlunderUser.SelectedValue + " order by M.MeetDate desc";
                    }
                    else if (txtmDate.Text != "" && ddlstatus.SelectedIndex > 0)
                    {
                        str = @"select MI.Name as IndName,dbo.getPartName(m.MeetPlanId) as PartyName,M.AppAmount, M.FromTime,M.ToTime,M.MeetDate,M.MeetName,MastArea.AreaName,M.MeetPlanId,M.*
 from TransMeetPlanEntry M 
  left join MastArea on MastArea.AreaId=M.MeetLoaction left join MastParty MP on m.RetailerPartyID=MP.PartyId 
  	left join MastItemClass MI on m.IndId=MI.Id where M.AppStatus='" + ddlstatus.SelectedValue + "' and  M.MeetDate>='" + Settings.dateformat1(txtmDate.Text) + "' and M.SMId=" + ddlunderUser.SelectedValue + " order by M.MeetDate desc";
                    }
                    else if (ddlstatus.SelectedIndex > 0)
                    {
                        str = @"select MI.Name as IndName,dbo.getPartName(m.MeetPlanId) as PartyName,M.AppAmount, M.FromTime,M.ToTime,M.MeetDate,M.MeetName,MastArea.AreaName,M.MeetPlanId,M.*
 from TransMeetPlanEntry M 
  left join MastArea on MastArea.AreaId=M.MeetLoaction left join MastParty MP on m.RetailerPartyID=MP.PartyId 
  	left join MastItemClass MI on m.IndId=MI.Id where M.AppStatus='" + ddlstatus.SelectedValue + "'  and M.SMId=" + ddlunderUser.SelectedValue + " order by M.MeetDate desc"; ;
                    }
                    else if (txtmDate.Text != "" && txttodate.Text != "")
                    {
                        str = @"select MI.Name as IndName,dbo.getPartName(m.MeetPlanId) as PartyName,M.AppAmount, M.FromTime,M.ToTime,M.MeetDate,M.MeetName,MastArea.AreaName,M.MeetPlanId,M.* from TransMeetPlanEntry M   left join MastArea on MastArea.AreaId=M.MeetLoaction left join MastParty MP on m.RetailerPartyID=MP.PartyId 
  	left join MastItemClass MI on m.IndId=MI.Id where M.MeetDate>='" + Settings.dateformat1(txtmDate.Text) + "' and M.MeetDate<='" + Settings.dateformat1(txttodate.Text) + "' and M.SMId=" + ddlunderUser.SelectedValue + " order by M.MeetDate desc";
                    }
                    else if (txtmDate.Text != "")
                    {
                        str = @"select MI.Name as IndName,dbo.getPartName(m.MeetPlanId) as PartyName,M.AppAmount, M.FromTime,M.ToTime,M.MeetDate,M.MeetName,MastArea.AreaName,M.MeetPlanId,M.* from TransMeetPlanEntry M   left join MastArea on MastArea.AreaId=M.MeetLoaction left join MastParty MP on m.RetailerPartyID=MP.PartyId 
  	left join MastItemClass MI on m.IndId=MI.Id where  M.MeetDate>='" + Settings.dateformat1(txtmDate.Text) + "' and M.SMId=" + ddlunderUser.SelectedValue + " order by M.MeetDate desc";
                    }
                    else if (ddlstatus.SelectedIndex > 0)
                    {
                        str = @"select MI.Name as IndName,dbo.getPartName(m.MeetPlanId) as PartyName,M.AppAmount, M.FromTime,M.ToTime,M.MeetDate,M.MeetName,MastArea.AreaName,M.MeetPlanId,M.* from TransMeetPlanEntry M   left join MastArea on MastArea.AreaId=M.MeetLoaction left join MastParty MP on m.RetailerPartyID=MP.PartyId 
  	left join MastItemClass MI on m.IndId=MI.Id where M.AppStatus='" + ddlstatus.SelectedValue + "'  and M.SMId=" + ddlunderUser.SelectedValue + " order by M.MeetDate desc";
                    }
                    else
                    {
                        str = @"select MI.Name as IndName,dbo.getPartName(m.MeetPlanId) as PartyName,M.AppAmount, M.FromTime,M.ToTime,M.MeetDate,M.MeetName,MastArea.AreaName,M.MeetPlanId,M.* from TransMeetPlanEntry M   left join MastArea on MastArea.AreaId=M.MeetLoaction left join MastParty MP on m.RetailerPartyID=MP.PartyId 
  	left join MastItemClass MI on m.IndId=MI.Id where  M.SMId=" + ddlunderUser.SelectedValue + " order by M.MeetDate desc";

                    }
                    DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                    if (dt.Rows.Count > 0)
                    {
                        rpt.DataSource = dt;
                        rpt.DataBind();
                    }
                    else
                    {
                        rpt.DataSource = null;
                        rpt.DataBind();
                    }
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('To Date Cannot be less than from date.');", true);
                    rpt.DataSource = null;
                    rpt.DataBind();
                }
            }
            catch { }
        }




        protected void rpt_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "MeetEdit")
            {

                GridView2.DataSource = null;
                GridView2.DataBind();
                string str = @"select g.ItemName as ProdctGroup,I.ItemName as ProdctName,c.Name as MatrialClass,s.Name as Segment,P.ClassID as MatrialClassId,p.ItemGrpId as ProdctGroupId,p.ItemId as ProdctID,p.SegmentID  from TransMeetPlanProduct p
             left join MastItemClass c on p.ClassID=c.Id  left join MastItemSegment s on p.SegmentID=s.Id  left join MastItem g on p.ItemGrpId=g.ItemId
             left join MastItem I on p.ItemId=I.ItemId  where p.MeetPlanId=" + e.CommandArgument.ToString();
                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                if (dt.Rows.Count > 0)
                {
                    GridView2.DataSource = dt;
                    GridView2.DataBind();
                }

                this.ModalPopupExtender1.Show();
            }
            else  if (e.CommandName == "MeetEdit1")
            {
                HiddenField h = (HiddenField)e.Item.FindControl("HiddenField2");
                if (h != null)
                {
                    if (h.Value == "Pending")
                    {
                        //Response.Redirect("/EditMeetPlan.aspx?MeetPlanID=" + e.CommandArgument.ToString());
                        Response.Redirect("/MeetPlanEntry.aspx?MeetPlanID=" + e.CommandArgument.ToString() + "&Mode=Edit");
                    }
                    else
                    {
                      
                    }
                }
            }
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            fillMeetPLan();
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {

        }
        protected void btnback_Click(object sender, EventArgs e)
        {
                Response.Redirect("~/MeetPlanEntry.aspx");

        }

    }
}