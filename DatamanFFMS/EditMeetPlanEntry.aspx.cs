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
using System.Collections;

namespace AstralFFMS
{
    public partial class EditMeetPlanEntry : System.Web.UI.Page
    {
        string MeetId = "";
        BAL.Meet.MeetPlanEntryBAL ME = new BAL.Meet.MeetPlanEntryBAL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if(Request.QueryString["MeetId"]!=null)
            {
                MeetId = Request.QueryString["MeetId"].ToString();
            }
            if (!IsPostBack)
            {
                divBeat.Visible = false;
                fillCity();
                fillArea();
                BindIndustry();
                fillUsers();
                fillUserType();
                fillMeetType();
                fillSenior();
                fillAreaType();
                fillscheme();
                FillClass();
                FillSegment();
                FillGroup();
                SetInitialRow();
                FillMeet();
                fillProduct();
                fillInitialParty();
            }
        }
        private void FillMeet()
        {
            try
            {
                string str = "select * from TransMeetPlanEntry where MeetPlanId="+MeetId;
                DataTable Meetdt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                if (Meetdt.Rows.Count > 0)
                {
                    ddlarea5.SelectedValue = Meetdt.Rows[0]["AreaId"].ToString();
                    ddlmeetType.SelectedValue = Meetdt.Rows[0]["MeetTypeId"].ToString();
                    ddlUserType.SelectedValue = Meetdt.Rows[0]["Usertype"].ToString();
                    ddlindrustry.SelectedValue = Meetdt.Rows[0]["IndId"].ToString();
                    hfCustomerId.Value = Meetdt.Rows[0]["DistId"].ToString();
                    txtNoOfUsers.Text = Meetdt.Rows[0]["NoOfUser"].ToString();
                    basicExample.Value = Meetdt.Rows[0]["FromTime"].ToString();
                    basicExample1.Value = Meetdt.Rows[0]["ToTime"].ToString();
                    txtVenue.Text = Meetdt.Rows[0]["Venue"].ToString();
                    txtComments.Text = Meetdt.Rows[0]["Comments"].ToString();
                    txttypeofGiftsforenduser.Text = Meetdt.Rows[0]["typeOfGiftEnduser"].ToString();
                    txtvalueforenduser.Text = Meetdt.Rows[0]["valueofEnduser"].ToString();
                    txttypeofGiftsforretailer.Text = Meetdt.Rows[0]["typeOfGiftRetailer"].ToString();
                    txtvalueforretailer.Text = Meetdt.Rows[0]["valueofRetailer"].ToString();
                    txtMeetDate.Text =string.Format("{0:dd/MM/yyyy}", Meetdt.Rows[0]["MeetDate"].ToString());
                    txtMeetName.Text = Meetdt.Rows[0]["MeetName"].ToString();
                    ddlmeetLocation.SelectedValue = Meetdt.Rows[0]["MeetLoaction"].ToString();
                    txtApproxBudget.Text = Meetdt.Rows[0]["LambBudget"].ToString();
                    ddlsenior.SelectedValue = Meetdt.Rows[0]["SeniorId"].ToString();
                    txtVenue.Text = Meetdt.Rows[0]["VenueId"].ToString();
                    txtDistributerSharing.Text = Meetdt.Rows[0]["ExpShareDist"].ToString();
                    txtastralSharing.Text = Meetdt.Rows[0]["ExpShareSelf"].ToString();
                    ddlscheme.SelectedValue = Meetdt.Rows[0]["SchId"].ToString();
                    txtNoofStaf.Text = Meetdt.Rows[0]["NoStaff"].ToString();
                    ddlUser.SelectedValue = Meetdt.Rows[0]["SMId"].ToString();
                }
            }
            catch
            {

            }
        }
        private void fillProduct()
        {
            string str = @"select g.ItemName as ProdctGroup,I.ItemName as ProdctName,c.Name as MatrialClass,s.Name as Segment,P.ClassID as MatrialClassId,p.ItemGrpId as ProdctGroupId,p.ItemId as ProdctID,p.SegmentID  from TransMeetPlanProduct p
             left join MastItemClass c on p.ClassID=c.Id  left join MastItemSegment s on p.SegmentID=s.Id  left join MastItem g on p.ItemGrpId=g.ItemId
             left join MastItem I on p.ItemId=I.ItemId  where p.MeetPlanId=" + MeetId;
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if(dt.Rows.Count>0)
            {
                ViewState["CurrentTable"] = dt;
                GridView2.DataSource = dt;
                GridView2.DataBind();
            }
        }
        private void fillInitialParty()
        {
            string str = @" select l.Address1,l.MeetPlanId,l.MeetPlanPartyId,l.Mobile,l.PartyId,MastParty.PartyName from TransMeetPlanPartyList l left join MastParty
                            on MastParty.PartyId=l.PartyId  where l.MeetPlanId=" + MeetId;
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (dt.Rows.Count > 0)
            {
                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
        }

        private void BindIndustry()
        {
            ddlindrustry.Items.Clear();
            string str = "select * from MastIndustry";
            DataTable obj = new DataTable();
            obj = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (obj.Rows.Count > 0)
            {
                ddlindrustry.DataSource = obj;
                ddlindrustry.DataTextField = "IndName";
                ddlindrustry.DataValueField = "IndId";
                ddlindrustry.DataBind();
            }
            ddlindrustry.Items.Insert(0, new ListItem("-- Select --", "0"));
        }
        private void FillClass()
        {
            string str = @"select * from MastItemClass";
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
        {
            string str = @"select * from MastItemSegment";
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
        {
            string str = @"select * from MastItem where ItemType='MATERIALGROUP' and Active=1";
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
        private void fillscheme()
        {
            string str = "select * from MastScheme";

            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (dt.Rows.Count > 0)
            {
                ddlscheme.DataSource = dt;
                ddlscheme.DataTextField = "Name";
                ddlscheme.DataValueField = "Id";
                ddlscheme.DataBind();
            }
        }
        private void fillCity()
        {
            string beatQuery2 = "select * from mastarea where areaid in (select areaid from MastAreaGrp where MainGrp in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode="
                      + Settings.Instance.SMID + ")) and areatype='CITY' order by AreaName";
            DataTable dtBeat = DbConnectionDAL.GetDataTable(CommandType.Text, beatQuery2);
            if (dtBeat.Rows.Count > 0)
            {
                ddlcity.DataSource = dtBeat;
                ddlcity.DataTextField = "AreaName";
                ddlcity.DataValueField = "AreaId";
                ddlcity.DataBind();
            }
            ddlcity.Items.Insert(0, new ListItem("-- Select City --", "0"));
        }
        private void fillAreaViaCity()
        {
            string str = "select * from mastarea where UnderId=" + ddlcity.SelectedValue;
            DataTable dtBeat = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (dtBeat.Rows.Count > 0)
            {
                ddlareaSearch.DataSource = dtBeat;
                ddlareaSearch.DataTextField = "AreaName";
                ddlareaSearch.DataValueField = "AreaId";
                ddlareaSearch.DataBind();
            }
            ddlareaSearch.Items.Insert(0, new ListItem("-- Select Area --", "0"));
        }
        private void fillBeatViaArea()
        {
            string str = "select * from mastarea where UnderId=" + ddlareaSearch.SelectedValue;
            DataTable dtBeat = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (dtBeat.Rows.Count > 0)
            {
                ddlbeat.DataSource = dtBeat;
                ddlbeat.DataTextField = "AreaName";
                ddlbeat.DataValueField = "AreaId";
                ddlbeat.DataBind();
            }
            ddlbeat.Items.Insert(0, new ListItem("-- Select Beat --", "0"));
        }
        private void fillArea()
        {
            string beatQuery2 = "select * from mastarea where areaid in (select areaid from MastAreaGrp where MainGrp in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode="
                      + Settings.Instance.SMID + ")) and areatype='Area' order by AreaName";
            DataTable dtBeat = DbConnectionDAL.GetDataTable(CommandType.Text, beatQuery2);
            if (dtBeat.Rows.Count > 0)
            {
                ddlarea5.DataSource = dtBeat;
                ddlarea5.DataTextField = "AreaName";
                ddlarea5.DataValueField = "AreaId";
                ddlarea5.DataBind();
            }
            ddlarea5.Items.Insert(0, new ListItem("-- Select Area --", "0"));
        }
        private void fillBeat()
        {
            string str = "select * from mastarea where UnderId=" + ddlarea5.SelectedValue;
            DataTable dtBeat = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (dtBeat.Rows.Count > 0)
            {
                ddlbeat5.DataSource = dtBeat;
                ddlbeat5.DataTextField = "AreaName";
                ddlbeat5.DataValueField = "AreaId";
                ddlbeat5.DataBind();
            }
            ddlbeat5.Items.Insert(0, new ListItem("-- Select Beat --", "0"));
        }
        private void fillAreaType()
        {
            string str = "select * from MastMeetAreaType";

            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (dt.Rows.Count > 0)
            {
                ddlmeetLocation.DataSource = dt;
                ddlmeetLocation.DataTextField = "Name";
                ddlmeetLocation.DataValueField = "Id";
                ddlmeetLocation.DataBind();
            }
            ddlmeetLocation.Items.Insert(0, new ListItem("-- Select Area Type --", "0"));
        }
        private void fillSenior()
        {
            string str = "select * from MastSalesRep where smid in (select maingrp from MastSalesRepGrp where smid=" + Settings.Instance.SMID + ")";
            DataTable dt2 = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (dt2.Rows.Count > 0)
            {
                ddlsenior.DataSource = dt2;
                ddlsenior.DataTextField = "SMName";
                ddlsenior.DataValueField = "SMId";
                ddlsenior.DataBind();
            }
            ddlsenior.Items.Insert(0, new ListItem("-- Select --", "0"));
        }
        private void fillUserType()
        {
            string str = "select * from PartyType";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (dt.Rows.Count > 0)
            {
                ddlUserType.DataSource = dt;
                ddlUserType.DataTextField = "PartyTypeName";
                ddlUserType.DataValueField = "PartyTypeId";
                ddlUserType.DataBind();
            }
            ddlUserType.Items.Insert(0, new ListItem("-- Select User Name --", "0"));
        }
        private void fillMeetType()
        {
            string query = "select * from MastMeetType";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, query);
            if (dt.Rows.Count > 0)
            {
                ddlmeetType.DataSource = dt;
                ddlmeetType.DataTextField = "Name";
                ddlmeetType.DataValueField = "Id";
                ddlmeetType.DataBind();
            }
            ddlmeetType.Items.Insert(0, new ListItem("-- Select Name --", "0"));
        }
        private void fillUsers()
        {
            DataTable dt = Settings.UnderUsers(Settings.Instance.SMID);
            if (dt.Rows.Count > 0)
            {
                ddlUser.DataSource = dt;
                ddlUser.DataTextField = "SMName";
                ddlUser.DataValueField = "SMId";
                ddlUser.DataBind();
            }
            ddlUser.Items.Insert(0, new ListItem("-- Select User Name --", "0"));
        }

        private void SetInitialRow()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;
            dt.Columns.Add(new DataColumn("ProdctName", typeof(string)));
            dt.Columns.Add(new DataColumn("ProdctID", typeof(string)));
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
                        drCurrentRow["ProdctName"] = ddlgroup.SelectedItem.Text;
                        drCurrentRow["ProdctID"] = ddlgroup.SelectedValue;
                        drCurrentRow["ProdctGroup"] = ddlgroup.SelectedItem.Text;
                        drCurrentRow["MatrialClass"] = ddlclass.SelectedItem.Text;
                        drCurrentRow["Segment"] = ddlsegment.SelectedItem.Text;
                        drCurrentRow["ProdctGroupId"] = ddlgroup.SelectedValue;
                        drCurrentRow["MatrialClassId"] = ddlclass.SelectedValue;
                        drCurrentRow["SegmentId"] = ddlsegment.SelectedValue;
                        rowIndex++;
                    }
                    dtCurrentTable.Rows.Add(drCurrentRow);
                    ViewState["CurrentTable"] = dtCurrentTable;
                    GridView2.DataSource = dtCurrentTable;
                    GridView2.DataBind();
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
            ScriptManager.RegisterStartupScript(this, GetType(), "myFunction1", "myFunction1();", true);
            try
            {
                if (ViewState["CurrentTable"] != null)
                {
                    DataTable dt = (DataTable)ViewState["CurrentTable"];

                    //DataRow[] dr = dt.Select("ProdctID=" + ddlproduct.SelectedValue);
                    //if (dr.Length > 0)
                    //{
                    //    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "error", "errormessage('This Product is Already Added');", true);
                    //    return;
                    //}
                    //else
                    //{
                    AddNewRowToGrid();
                    // ddlproduct.SelectedIndex = 0;
                    //}

                }

            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        protected void btnAddparty_Click(object sender, EventArgs e)
        {

        }
        private void fillPartyBeat()
        {
            string query = "select * from MastParty where BeatId=" + ddlbeat.SelectedValue;
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, query);
            if (dt.Rows.Count > 0)
            {
                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
            else
            {
                GridView1.DataSource = null;
                GridView1.DataBind();
            }

        }
        protected void GridView2_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int index = Convert.ToInt32(e.RowIndex);
            DataTable dt = (DataTable)ViewState["CurrentTable"];
            dt.Rows[index].Delete();
            ViewState["CurrentTable"] = dt;
            GridView2.DataSource = dt;
            GridView2.DataBind();
        }
        protected void ddlcity_SelectedIndexChanged(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "myFunction", "myFunction();", true);

            if (ddlcity.SelectedIndex > 0)
            {
                ddlareaSearch.Items.Clear();
                fillAreaViaCity();
            }
        }
        protected void ddlareaSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "myFunction", "myFunction();", true);
            if (ddlareaSearch.SelectedIndex > 0)
            {
                ddlbeat.Items.Clear();
                fillBeatViaArea();

            }
        }
        protected void ddlbeat_SelectedIndexChanged(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "myFunction", "myFunction();", true);
            if (ddlbeat.SelectedIndex > 0)
            {
                
                fillPartyBeat();
            }
        }
        protected void checkAll_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox ChkBoxHeader = (CheckBox)GridView1.HeaderRow.FindControl("checkAll");
            foreach (GridViewRow row in GridView1.Rows)
            {
                CheckBox ChkBoxRows = (CheckBox)row.FindControl("CheckBox1");
                if (ChkBoxHeader.Checked == true)
                {
                    ChkBoxRows.Checked = true;
                }
                else
                {
                    ChkBoxRows.Checked = false;
                }
            }
        }
        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            

        }
        protected void btnsave_Click(object sender, EventArgs e)
        {
            try
            {
                string docID = Settings.GetDocID("MPLAN", DateTime.Now);
                Settings.SetDocID("MPLAN", docID);

                string DistributorpartyId = "0";
                if (ddlmeetType.SelectedItem.Text == "Counter Meet")
                {
                    DistributorpartyId = Hidparty.Value;
                }
                else
                {
                    DistributorpartyId = hfCustomerId.Value;
                }


                int RetSave = ME.Update(Settings.DMInt64(MeetId), docID, Settings.DMInt32(Settings.Instance.UserID), Settings.DMInt32(ddlUser.SelectedValue), Settings.DMInt32(ddlarea5.SelectedValue), Settings.DMInt32(ddlmeetType.SelectedValue), Settings.DMInt32(ddlUserType.SelectedValue), Settings.DMInt32(ddlindrustry.SelectedValue), Settings.DMInt32(DistributorpartyId), Settings.DMInt32(txtNoOfUsers.Text), basicExample.Value, basicExample1.Value, txtVenue.Text, txtComments.Text, txttypeofGiftsforenduser.Text, Settings.DMInt32(txtvalueforenduser.Text), txttypeofGiftsforretailer.Text, Settings.DMInt32(txtvalueforretailer.Text), txtMeetDate.Text, txtMeetName.Text, ddlmeetLocation.SelectedValue, Settings.DMDecimal(txtApproxBudget.Text), Settings.DMInt32(ddlsenior.SelectedValue), txtVenue.Text, Settings.DMDecimal(txtDistributerSharing.Text), Settings.DMDecimal(txtastralSharing.Text), Settings.DMInt32(ddlscheme.SelectedValue), Settings.DMInt32(txtNoofStaf.Text), true, "Pending");

                if (RetSave > 0)
                {
                    for (int i = 0; i < GridView2.Rows.Count; i++)
                    {
                        HiddenField hidseg = (HiddenField)GridView2.Rows[i].FindControl("hidSegment");
                        HiddenField hidclass = (HiddenField)GridView2.Rows[i].FindControl("hidMaterialClass");
                        HiddenField hidGroupId = (HiddenField)GridView2.Rows[i].FindControl("hidProductgroup");
                        HiddenField hidProductId = (HiddenField)GridView2.Rows[i].FindControl("hidProduct");

                        InsertProduct(RetSave, i + 1, Settings.DMInt32(hidGroupId.Value), Settings.DMInt32(hidProductId.Value), Settings.DMInt32(hidclass.Value), Settings.DMInt32(hidseg.Value));
                    }
                    int srno = 1;
                    for (int j = 0; j < GridView1.Rows.Count; j++)
                    {
                        CheckBox chk = (CheckBox)GridView1.Rows[j].FindControl("chkRow");

                        if (chk.Checked)
                        {
                            HiddenField hidParty = (HiddenField)GridView1.Rows[j].FindControl("hidParty");
                            InsertrMeetParty(RetSave, srno, Settings.DMInt32(hidParty.Value), GridView1.Rows[j].Cells[4].Text, GridView1.Rows[j].Cells[3].Text);
                            srno++;
                        }
                    }

                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Inserted Successfully');", true);
                    reset();
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record Cannot be Insert');", true);

                }
            }
            catch (Exception ex) { }
        }
        private void InsertProduct(int meetPlanID, int srNo, int ItemGrpId, int ItemId, int ClassiD, int SegmentId)
        {
            try {
                string s = "delete from TransMeetPlanProduct where MeetPlanId="+meetPlanID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, s);

                int resaveProduct = ME.InsertMeetProduct(meetPlanID, srNo, ItemGrpId, ItemId, ClassiD, SegmentId); }
            catch (Exception ex) { }
        }
        private void InsertrMeetParty(int meetPlanID, int srNo, int PartyID, string MobileNo, string Address1)
        {
            try {
                string s = "delete from [TransMeetPlanPartyList] where MeetPlanId=" + meetPlanID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, s);
                int resaveParty = ME.InsertMeetParty(meetPlanID, srNo, PartyID, MobileNo, Address1, ""); }
            catch (Exception ex) { }
        }
        private void reset()
        {
            ddlarea5.SelectedValue = "0";
            txtComments.Text = "";
            hfCustomerId.Value = "0";
            txtdistName.Text = "";
            txtDistributerSharing.Text = "";
            txtastralSharing.Text = "";
            ddlindrustry.SelectedValue = "0";
            txtApproxBudget.Text = "";
            ddlmeetLocation.SelectedValue = "0";
            ddlmeetType.SelectedValue = "0";
            ddlmeetType.SelectedValue = "0";
            txtNoOfUsers.Text = "";
            txtNoofStaf.Text = "";
            txttypeofGiftsforenduser.Text = "";
            txttypeofGiftsforretailer.Text = "";
            txtvalueforenduser.Text = "";
            txtvalueforretailer.Text = "";
            txtVenue.Text = "";
            ddlUserType.SelectedValue = "0";
            ddlUser.SelectedValue = "0";
            ddlsenior.SelectedValue = "0";
            ddlbeat.Items.Clear();
            ddlareaSearch.Items.Clear();
            GridView1.DataSource = null;
            GridView1.DataBind();
            GridView2.DataSource = null;
            GridView2.DataBind();
        }
        protected void Cancel_Click(object sender, EventArgs e)
        {
        }

        protected void ddlmeetType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlmeetType.SelectedItem.Text == "Counter Meet")
            {
                divBeat.Visible = true;
            }
            else
            {
                divBeat.Visible = false;
            }
        }
        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> SearchItem(string prefixText, string contextKey)
        {
            string str = "select * FROM MastParty where (PartyName like '%" + prefixText + "%') and PartyDist=1 and AreaId=" + contextKey + "";
            DataTable dt = new DataTable();
            dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            List<string> customers = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(dt.Rows[i]["PartyName"].ToString(), dt.Rows[i]["PartyId"].ToString());
                customers.Add(item);
            }
            return customers;
        }

    }
}