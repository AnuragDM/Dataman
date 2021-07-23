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
    public partial class MeetPlanListL2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;
            txtmDate.Attributes.Add("readonly", "readonly");
            txttodate.Attributes.Add("readonly", "readonly");
            if (!IsPostBack)
            {
                fillUnderUsers();
                txtmDate.Text = Settings.GetUTCTime().AddMonths(-1).ToString("dd/MMM/yyyy");// DateTime.Parse(DateTime.UtcNow.ToShortDateString()).ToString("dd/MMM/yyyy");
                txttodate.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");// DateTime.Parse(DateTime.UtcNow.ToShortDateString()).ToString("dd/MMM/yyyy");
            }
        }

        private void fillUnderUsers()
        {
            DataTable d = Settings.UnderUsers(Settings.Instance.SMID);
            if (d.Rows.Count > 0)
            {
                try
                {
                    DataView dv = new DataView(d);
                    dv.RowFilter = "RoleType='CityHead'";
                    ddlunderUser.DataSource = dv;
                    ddlunderUser.DataTextField = "SMName";
                    ddlunderUser.DataValueField = "SMId";
                    ddlunderUser.DataBind();
                }
                catch { }

            }
        }


        private void fillMeetPLan()
        {
            try
            {
                string str = "";
                if (txtmDate.Text != "" && txttodate.Text != "" && ddlstatus.SelectedIndex > 0)
                {
                    str = @"select MI.Name as IndName,MP.PartyName, M.FromTime,M.ToTime,M.MeetDate,M.MeetName,MastArea.AreaName,M.MeetPlanId,M.*
 from TransMeetPlanEntry M 
  left join MastArea on MastArea.AreaId=M.MeetLoaction left join MastParty MP on m.RetailerPartyID=MP.PartyId 
  	left join MastItemClass MI on m.IndId=MI.Id where M.AppStatus='" + ddlstatus.SelectedValue + "' and M.MeetDate>='" + Settings.dateformat1(txtmDate.Text) + "' and M.MeetDate<='" + Settings.dateformat1(txttodate.Text) + "' and M.SMId=" + ddlunderUser.SelectedValue + " order by M.MeetDate desc";
                }
                else if (txtmDate.Text != "" && ddlstatus.SelectedIndex > 0)
                {
                    str = @"select MI.Name as IndName,MP.PartyName, M.FromTime,M.ToTime,M.MeetDate,M.MeetName,MastArea.AreaName,M.MeetPlanId,M.*
 from TransMeetPlanEntry M 
  left join MastArea on MastArea.AreaId=M.MeetLoaction left join MastParty MP on m.RetailerPartyID=MP.PartyId 
  	left join MastItemClass MI on m.IndId=MI.Id where M.AppStatus='" + ddlstatus.SelectedValue + "' and  M.MeetDate>='" + Settings.dateformat1(txtmDate.Text) + "' and M.SMId=" + ddlunderUser.SelectedValue + " order by M.MeetDate desc";
                }
                else if (ddlstatus.SelectedIndex > 0)
                {
                    str = @"select MI.Name as IndName,MP.PartyName, M.FromTime,M.ToTime,M.MeetDate,M.MeetName,MastArea.AreaName,M.MeetPlanId,M.*
 from TransMeetPlanEntry M 
  left join MastArea on MastArea.AreaId=M.MeetLoaction left join MastParty MP on m.RetailerPartyID=MP.PartyId 
  	left join MastItemClass MI on m.IndId=MI.Id where M.AppStatus='" + ddlstatus.SelectedValue + "'  and M.SMId=" + ddlunderUser.SelectedValue + " order by M.MeetDate desc"; ;
                }
                else if (txtmDate.Text != "" && txttodate.Text != "")
                {
                    str = @"select MI.Name as IndName,MP.PartyName, M.FromTime,M.ToTime,M.MeetDate,M.MeetName,MastArea.AreaName,M.MeetPlanId,M.* from TransMeetPlanEntry M   left join MastArea on MastArea.AreaId=M.MeetLoaction left join MastParty MP on m.RetailerPartyID=MP.PartyId 
  	left join MastItemClass MI on m.IndId=MI.Id where M.MeetDate>='" + Settings.dateformat1(txtmDate.Text) + "' and M.MeetDate<='" + Settings.dateformat1(txttodate.Text) + "' and M.SMId=" + ddlunderUser.SelectedValue + " order by M.MeetDate desc";
                }
                else if (txtmDate.Text != "")
                {
                    str = @"select MI.Name as IndName,MP.PartyName, M.FromTime,M.ToTime,M.MeetDate,M.MeetName,MastArea.AreaName,M.MeetPlanId,M.* from TransMeetPlanEntry M   left join MastArea on MastArea.AreaId=M.MeetLoaction left join MastParty MP on m.RetailerPartyID=MP.PartyId 
  	left join MastItemClass MI on m.IndId=MI.Id where  M.MeetDate>='" + Settings.dateformat1(txtmDate.Text) + "' and M.SMId=" + ddlunderUser.SelectedValue + " order by M.MeetDate desc";
                }
                else if (ddlstatus.SelectedIndex > 0)
                {
                    str = @"select MI.Name as IndName,MP.PartyName, M.FromTime,M.ToTime,M.MeetDate,M.MeetName,MastArea.AreaName,M.MeetPlanId,M.* from TransMeetPlanEntry M   left join MastArea on MastArea.AreaId=M.MeetLoaction left join MastParty MP on m.RetailerPartyID=MP.PartyId 
  	left join MastItemClass MI on m.IndId=MI.Id where M.AppStatus='" + ddlstatus.SelectedValue + "'  and M.SMId=" + ddlunderUser.SelectedValue + " order by M.MeetDate desc";
                }
                else
                {
                    str = @"select MI.Name as IndName,MP.PartyName, M.FromTime,M.ToTime,M.MeetDate,M.MeetName,MastArea.AreaName,M.MeetPlanId,M.* from TransMeetPlanEntry M   left join MastArea on MastArea.AreaId=M.MeetLoaction left join MastParty MP on m.RetailerPartyID=MP.PartyId 
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
            else if (e.CommandName == "MeetEdit1")
            {
                Response.Redirect("/EditMeetPlanEntryL2.aspx?MeetPlanID=" + e.CommandArgument.ToString());
            }
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            fillMeetPLan();
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {

        }
    }
}