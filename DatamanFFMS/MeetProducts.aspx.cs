using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLayer;
using DAL;
using BAL;
using System.IO;

namespace AstralFFMS
{

    public partial class MeetProducts : System.Web.UI.Page
    {
        string PageNames = "";
        string MeetPlanID = "";
        BAL.Meet.MeetPlanEntryBAL ME = new BAL.Meet.MeetPlanEntryBAL();
        protected void Page_Load(object sender, EventArgs e)
        {
            string pageName = Path.GetFileName(Request.Path);    
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;
            string PermAll = Settings.Instance.CheckPagePermissions(pageName, Convert.ToString(Session["user_name"]));
            string[] SplitPerm = PermAll.Split(',');
            if (Request.QueryString["Pagename"] != null)
            {
                PageNames = Request.QueryString["Pagename"].ToString();               
            }
            if (Request.QueryString["MeetPlanID"] != null)
            {               
                MeetPlanID = Request.QueryString["MeetPlanID"].ToString();
            }
            if (btnsave.Text == "Save")
            {
                btnsave.Enabled = Convert.ToBoolean(SplitPerm[1]);
              //  btnsave.Enabled = Settings.Instance.CheckAddPermission(pageName, Convert.ToString(Session["user_name"]));
                btnsave.CssClass = "btn btn-primary";
            }
            else
            {
                btnsave.Enabled = Convert.ToBoolean(SplitPerm[2]);
               // btnsave.Enabled = Settings.Instance.CheckEditPermission(pageName, Convert.ToString(Session["user_name"]));
                btnsave.CssClass = "btn btn-primary";
            }
            if (!IsPostBack)
            {
                fillMeetType();
                fillUnderUsers();
                FillClass();
                FillGroup();
                FillSegment();
               // fillInitialRecords();
                SetInitialRow();
                ddlmeet.Items.Insert(0, new ListItem("-- Select --", "0"));
            }

        }
        private void fillMeetType()
        {//Ankita - 20/may/2016- (For Optimization)
            //string query = "select * from MastMeetType order by Name";
            string query = "select Id,Name from MastMeetType order by Name";
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
                catch { }

            }
        }
      
        private void FillClass()
        {//Ankita - 20/may/2016- (For Optimization)
            //string str = @"select * from MastItemClass order by Name";
            string str = @"select Id,Name from MastItemClass order by Name";
            DataTable deptValueDt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (deptValueDt.Rows.Count > 0)
            {
                ddlclass.DataSource = deptValueDt;
                ddlclass.DataTextField = "Name";
                ddlclass.DataValueField = "Id";
                ddlclass.DataBind();
            }
            ddlclass.Items.Insert(0, new ListItem("-- Select --", "0"));

        }
        private void FillSegment()
        {//Ankita - 20/may/2016- (For Optimization)
            //string str = @"select * from MastItemSegment order by Name";
            string str = @"select Id,Name from MastItemSegment order by Name";
            DataTable deptValueDt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (deptValueDt.Rows.Count > 0)
            {
                ddlsegment.DataSource = deptValueDt;
                ddlsegment.DataTextField = "Name";
                ddlsegment.DataValueField = "Id";
                ddlsegment.DataBind();
            }
            ddlsegment.Items.Insert(0, new ListItem("-- Select --", "0"));
        }
        private void FillGroup()
        {//Ankita - 20/may/2016- (For Optimization)
            //string str = @"select * from MastItem where ItemType='MATERIALGROUP' and Active=1 order by ItemName";
            string str = @"select ItemId,ItemName from MastItem where ItemType='MATERIALGROUP' and Active=1 order by ItemName";
            DataTable deptValueDt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (deptValueDt.Rows.Count > 0)
            {
                ddlgroup.DataSource = deptValueDt;
                ddlgroup.DataTextField = "ItemName";
                ddlgroup.DataValueField = "ItemId";
                ddlgroup.DataBind();
            }
            ddlgroup.Items.Insert(0, new ListItem("-- Select --", "0"));
        }

        protected void GridView2_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int index = Convert.ToInt32(e.RowIndex);
            DataTable dt = (DataTable)ViewState["CurrentTable"];

            dt.Rows[index].Delete();
            dt.AcceptChanges();
            ViewState["CurrentTable"] = dt;
            GridView2.DataSource = dt;
            GridView2.DataBind();
        }

        private void fillInitialRecords()
        {
            ddlmeet.Items.Clear();
            //Ankita - 20/may/2016- (For Optimization)
            //string str = @"select * from TransMeetPlanEntry where SmId=" + Settings.DMInt32(ddlunderUser.SelectedValue) + " and MeetTypeId="+ddlmeetType.SelectedValue+"  and [AppBy] is Null order by MeetDate desc";
            string str = @"select MeetPlanId,MeetName from TransMeetPlanEntry where SmId=" + Settings.DMInt32(ddlunderUser.SelectedValue) + " and MeetTypeId=" + ddlmeetType.SelectedValue + "  and [AppBy] is Null order by MeetDate desc";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (dt.Rows.Count > 0)
            {
                ddlmeet.DataSource = dt;
                ddlmeet.DataTextField = "MeetName";
                ddlmeet.DataValueField = "MeetPlanId";
                ddlmeet.DataBind();
            }
            ddlmeet.Items.Insert(0, new ListItem("-- Select --", "0"));
            fillProduct(ddlmeet.SelectedValue);
        }

        private void SetInitialRow()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;
            //dt.Columns.Add(new DataColumn("ProdctName", typeof(string)));
            //dt.Columns.Add(new DataColumn("ProdctID", typeof(string)));
            dt.Columns.Add(new DataColumn("ProdctGroupId", typeof(string)));
            dt.Columns.Add(new DataColumn("ProdctGroup", typeof(string)));
            dt.Columns.Add(new DataColumn("MatrialClassId", typeof(string)));
            dt.Columns.Add(new DataColumn("MatrialClass", typeof(string)));
            dt.Columns.Add(new DataColumn("SegmentId", typeof(string)));
            dt.Columns.Add(new DataColumn("Segment", typeof(string)));
            ViewState["CurrentTable"] = dt;
            GridView2.DataSource = dt;
            GridView2.DataBind();

        }
        private void AddNewRowToGrid()
        {
            int rowIndex = 0;
            if (ViewState["CurrentTable"] != null)
            {
                DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
                DataRow drCurrentRow = null;
                if (dtCurrentTable.Rows.Count > -1)
                {
                    for (int i = 0; i <= dtCurrentTable.Rows.Count; i++)
                    {
                        drCurrentRow = dtCurrentTable.NewRow();
                        //drCurrentRow["ProdctName"] = txtItem.Text;
                        //drCurrentRow["ProdctID"] = hfitemid.Value;
                        if (ddlgroup.SelectedIndex > 0)
                        {
                            drCurrentRow["ProdctGroup"] = ddlgroup.SelectedItem.Text;
                        }
                        else
                        {
                            drCurrentRow["ProdctGroup"] = "";
                        }
                        if (ddlclass.SelectedIndex > 0)
                        {
                            drCurrentRow["MatrialClass"] = ddlclass.SelectedItem.Text;
                        }
                        else
                        {
                            drCurrentRow["MatrialClass"] = "";
                        }
                        if (ddlsegment.SelectedIndex > 0)
                        {
                            drCurrentRow["Segment"] = ddlsegment.SelectedItem.Text;
                        }
                        else
                        {
                            drCurrentRow["Segment"] = "";
                        }

                        drCurrentRow["ProdctGroupId"] = ddlgroup.SelectedValue;
                        drCurrentRow["MatrialClassId"] = ddlclass.SelectedValue;
                        drCurrentRow["SegmentId"] = ddlsegment.SelectedValue;
                        rowIndex++;
                    }
                    dtCurrentTable.Rows.Add(drCurrentRow);
                    ViewState["CurrentTable"] = dtCurrentTable;
                    GridView2.DataSource = dtCurrentTable;
                    GridView2.DataBind();

                    ddlclass.SelectedIndex = 0;
                    ddlgroup.SelectedIndex = 0;
                    ddlsegment.SelectedIndex = 0;
                }
            }
            else
            {
                //  Response.Write("ViewState is null");
            }
            //Set Previous Data on Postbacks
            SetPreviousData();
        }

        private void SetPreviousData()
        {

            if (ViewState["CurrentTable"] != null)
            {
                DataTable dt = (DataTable)ViewState["CurrentTable"];
                GridView2.DataSource = dt;
                GridView2.DataBind();

            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (ViewState["CurrentTable"] != null)
                {
                    DataTable dt = (DataTable)ViewState["CurrentTable"];
                   

                    //DataRow[] dr = dt.Select("ProdctID=" + hfitemid.Value);
                    //if (dr.Length > 0)
                    //{
                    //    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "error", "errormessage('This Product is Already Added');", true);
                    //    return;
                    //}
                    //else
                    //{
                    if (ddlmeet.SelectedIndex > 0)
                    {
                        if (ddlsegment.SelectedIndex > 0 || ddlgroup.SelectedIndex > 0 || ddlclass.SelectedIndex > 0)
                        {
                            AddNewRowToGrid();
                            //hfitemid.Value = "0";
                            //txtItem.Text = "";
                        }
                        else
                        {
                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please select Material Group/Segment/Material Class');", true);

                        }
                    }
                    else
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please select the Meet');", true);
                    }
                    


                }

            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> SearchItem1(string prefixText)
        {//Ankita - 20/may/2016- (For Optimization)
            //string str = "select * FROM MastItem where (ItemName like '%" + prefixText + "%' or SyncId like '%" + prefixText + "%' or ItemCode like '%" + prefixText + "%') and ItemType='Item' and Active=1";
            string str = "select SyncId,ItemName,ItemCode,ItemId FROM MastItem where (ItemName like '%" + prefixText + "%' or SyncId like '%" + prefixText + "%' or ItemCode like '%" + prefixText + "%') and ItemType='Item' and Active=1";
            DataTable dt = new DataTable();
            dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            List<string> customers = new List<string>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem("(" + dt.Rows[i]["SyncId"].ToString() + ")" + " " + dt.Rows[i]["ItemName"].ToString() + " " + "(" + dt.Rows[i]["ItemCode"].ToString() + ")", dt.Rows[i]["ItemId"].ToString());
                customers.Add(item);
            }
            return customers;
        }

        private void InsertProduct(int meetPlanID, int srNo, int ItemGrpId, int ItemId, int ClassiD, int SegmentId)
        {
            try { int resaveProduct = ME.InsertMeetProduct(meetPlanID, srNo, ItemGrpId, ItemId, ClassiD, SegmentId); }
            catch (Exception ex) { }
        }

        protected void btnsave_Click(object sender, EventArgs e)
        {
            try
            {
                if (GridView2.Rows.Count > 0)
                {
                    string s = "delete from TransMeetPlanProduct where MeetPlanId=" + Settings.DMInt32(ddlmeet.SelectedValue);
                    DbConnectionDAL.ExecuteNonQuery(CommandType.Text, s);

                    for (int i = 0; i < GridView2.Rows.Count; i++)
                    {
                        try
                        {
                            HiddenField hid = (HiddenField)GridView2.Rows[i].FindControl("hidProduct");
                            Label lbl = (Label)GridView2.Rows[i].FindControl("lblPName");

                            HiddenField hidMaterialClass = (HiddenField)GridView2.Rows[i].FindControl("hidMaterialClass");
                            HiddenField hidSegment = (HiddenField)GridView2.Rows[i].FindControl("hidSegment");
                            HiddenField hidProductgroup = (HiddenField)GridView2.Rows[i].FindControl("hidProductgroup");

                            InsertProduct(Settings.DMInt32(ddlmeet.SelectedValue), i + 1, Settings.DMInt32(hidProductgroup.Value), Settings.DMInt32("0"), Settings.DMInt32(hidMaterialClass.Value), Settings.DMInt32(hidSegment.Value));
                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Inserted Successfully');", true);


                        }
                        catch
                        {

                        }
                    }
                    ddlmeet.SelectedIndex = 0;
                    ddlmeetType.SelectedValue = "0";
                    GridView2.DataSource = null;
                    GridView2.DataBind();
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Add atleast one product');", true);
                }
            }
            catch
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record Cannot be Insert');", true);
            }


        }

        private void fillProduct(string MeetId)
        {
            string str = @"select g.ItemName as ProdctGroup,I.ItemName as ProdctName,c.Name as MatrialClass,s.Name as Segment,P.ClassID as MatrialClassId,p.ItemGrpId as ProdctGroupId,p.ItemId as ProdctID,p.SegmentID  from TransMeetPlanProduct p
             left join MastItemClass c on p.ClassID=c.Id  left join MastItemSegment s on p.SegmentID=s.Id  left join MastItem g on p.ItemGrpId=g.ItemId
             left join MastItem I on p.ItemId=I.ItemId  where p.MeetPlanId=" + MeetId;
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (dt.Rows.Count > 0)
            {
                ViewState["CurrentTable"] = dt;
         //       GridView2.DataSource = dt;
         //       GridView2.DataBind();
            }
            else
            {
                SetInitialRow();
            }
        }

        protected void ddlmeet_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(ddlmeet.SelectedIndex>0)
            {
                fillProduct(ddlmeet.SelectedValue);
            }
            else
            {

            }
        }
        protected void ddlmeetType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlmeetType.SelectedIndex > 0)
            {
                fillInitialRecords();
            }
            else
            {
                ddlmeet.Items.Clear();
                ddlmeet.Items.Insert(0, new ListItem("-- Select --", "0"));
            }
        }

        protected void btncancel_Click(object sender, EventArgs e)
        {
            ddlunderUser.SelectedIndex = 0;
            ddlmeetType.SelectedIndex = 0;
            ddlmeet.Items.Clear();
            ddlmeet.Items.Insert(0, new ListItem("-- Select --", "0"));
            GridView2.DataSource = null;
            GridView2.DataBind();
            ViewState["CurrentTable"] =null;
            ddlsegment.SelectedIndex = 0;
            ddlgroup.SelectedIndex = 0;
            ddlclass.SelectedIndex = 0;
        }

        protected void btnback_Click(object sender, EventArgs e)
        {
            if (PageNames == "Meetplanentry")
            {
                Response.Redirect("~/MeetPlanEntry.aspx");
            }
            else
            {
                Response.Redirect("~/EditMeetPlan.aspx?MeetPlanID=" + MeetPlanID + "&Pagename=EditMeetplan");
            }
        }


    }
}