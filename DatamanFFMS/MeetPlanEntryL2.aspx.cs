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
using System.IO;

namespace AstralFFMS
{
    public partial class MeetPlanEntryL2 : System.Web.UI.Page
    {
        BAL.Meet.MeetPlanEntryBAL ME = new BAL.Meet.MeetPlanEntryBAL();
        protected void Page_Load(object sender, EventArgs e)
        {
            txtMeetDate.Attributes.Add("readonly", "readonly");
            //Ankita - 20/may/2016- (For Optimization)
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;
            string PermAll = Settings.Instance.CheckPagePermissions(pageName, Convert.ToString(Session["user_name"]));
            string[] SplitPerm = PermAll.Split(',');
            CalendarExtender1.StartDate = DateTime.Now;
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

            // btnDelete.Enabled = Settings.Instance.CheckDeletePermission(pageName, Convert.ToString(Session["user_name"]));
            // btnDelete.CssClass = "btn btn-primary";
            if (!IsPostBack)
            {
                try
                {
                    fillUnderUsers();
                    txtMeetDate.Text = DateTime.Parse(DateTime.UtcNow.AddSeconds(19800).ToShortDateString()).ToString("dd/MMM/yyyy");
                    fillArea();
                    BindIndustry();
                    fillMeetType();
                    fillAreaType();
                    fillscheme();
                    fillBeat();
                    txtParty.ReadOnly = true;
                    BindDDlCity();
                }
                catch { }

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

        private void BindDDlCity()
        {
            ddlmeetCity.Items.Clear();
           
//            string str = @"select * from mastarea where areaid in (select underid from mastarea where areaId in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA'
//                          and PrimCode=" + ddlunderUser.SelectedValue + ")) and  areatype='city' and Active=1 order by AreaName";
            //Ankita - 20/may/2016- (For Optimization)
            string str = @"select AreaId,AreaName from mastarea where areaid in (select underid from mastarea where areaId in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA'
                          and PrimCode=" + ddlunderUser.SelectedValue + ")) and  areatype='city' and Active=1 order by AreaName";

            //            string str = @"select * from mastarea where areaid in (select areaid from MastAreaGrp where MainGrp
            //                        in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode=" + ddlunderUser.SelectedValue + ")) and areatype='City' and Active=1 order by AreaName ";

            DataTable obj = new DataTable();
            obj = DbConnectionDAL.GetDataTable(CommandType.Text, str);

            if (obj.Rows.Count > 0)
            {
                try
                {
                    ddlmeetCity.DataSource = obj;
                    ddlmeetCity.DataTextField = "AreaName";
                    ddlmeetCity.DataValueField = "AreaId";
                    ddlmeetCity.DataBind();

                    //Added AS Per UAT 07-12-2015
                    string cityIDStr = "";
                    foreach (DataRow dr in obj.Rows)
                    {
                        cityIDStr = cityIDStr + "," + Convert.ToString(dr["AreaId"]);
                        //        smIDStr +=string.Join(",",dtSMId.Rows[i]["SMId"].ToString());
                    }
                    cityIDStr = cityIDStr.TrimStart(',').TrimEnd(',');
                    hdfCityIdStr.Value = cityIDStr;
                    //End
                }
                catch { }
            }
            ddlmeetCity.Items.Insert(0, new ListItem("-- Select --", "0"));
        }

        private void BindIndustry()
        { //Ankita - 20/may/2016- (For Optimization)
            ddlindrustry.Items.Clear();
            //string str = "select * from MastItemClass order by Name";
            string str = "select Id,Name from MastItemClass order by Name";
            DataTable obj = new DataTable();
            obj = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (obj.Rows.Count > 0)
            {
                ddlindrustry.DataSource = obj;
                ddlindrustry.DataTextField = "Name";
                ddlindrustry.DataValueField = "Id";
                ddlindrustry.DataBind();
            }
            ddlindrustry.Items.Insert(0, new ListItem("-- Select --", "0"));
        }

        private void fillscheme()
        {//Ankita - 20/may/2016- (For Optimization)
            //string str = "select * from MastScheme order by Name";
            string str = "select Id,Name from MastScheme order by Name";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (dt.Rows.Count > 0)
            {
                ddlscheme.DataSource = dt;
                ddlscheme.DataTextField = "Name";
                ddlscheme.DataValueField = "Id";
                ddlscheme.DataBind();
            }
            ddlscheme.Items.Insert(0, new ListItem("-- Select --", "0"));
        }

        private void fillArea()
        {
            //string beatQuery2 = "select * from mastarea where areaid in (select areaid from MastAreaGrp where MainGrp in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode="
            //          +Settings.Instance.SMID + ")) and areatype='Area' order by AreaName";
            //DataTable dtBeat = DbConnectionDAL.GetDataTable(CommandType.Text, beatQuery2);
            //if (dtBeat.Rows.Count > 0)
            //{
            //    ddlarea5.DataSource = dtBeat;
            //    ddlarea5.DataTextField = "AreaName";
            //    ddlarea5.DataValueField = "AreaId";
            //    ddlarea5.DataBind();
            //}
            //ddlarea5.Items.Insert(0, new ListItem("-- Select Area --", "0"));
        }

        private void fillBeat()
        {
//            string str = @" select * from mastarea where UnderId in 
//(select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode=" + ddlunderUser.SelectedValue + ") and  areatype='beat' and Active=1 order by AreaName";
            //Ankita - 20/may/2016- (For Optimization)
            string str = @" select AreaId,AreaName from mastarea where UnderId in 
(select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode=" + ddlunderUser.SelectedValue + ") and  areatype='beat' and Active=1 order by AreaName";

            //            string str = @"select * from mastarea where areaid in (select areaid from MastAreaGrp where MainGrp
            //                        in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode=" + Settings.Instance.SMID + ")) and areatype='Beat' and Active=1 order by AreaName ";
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
            //string str = "select * from MastMeetAreaType";

            //DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            //if (dt.Rows.Count > 0)
            //{
            //    ddlmeetLocation.DataSource = dt;
            //    ddlmeetLocation.DataTextField = "Name";
            //    ddlmeetLocation.DataValueField = "Id";
            //    ddlmeetLocation.DataBind();
            //}
            //ddlmeetLocation.Items.Insert(0, new ListItem("-- Select Area Type --", "0"));
        }

        private void fillMeetType()
        { //Ankita - 20/may/2016- (For Optimization)
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



        protected void btnAddparty_Click(object sender, EventArgs e)
        {

        }
        private void fillParty()
        {
            //var query = from h in context.MastParties.Where(u => u.AreaId == Convert.ToInt32(ddlareaSearch.SelectedValue))
            //            select h;
            //DataTable dt = BusinessClass.LINQResultToDataTable(query);
            //if (dt.Rows.Count > 0)
            //{
            //    GridView1.DataSource = dt;
            //    GridView1.DataBind();

            //}
            //else
            //{
            //    GridView1.DataSource = null;
            //    GridView1.DataBind();
            //}

        }

        //private void fillPartyBeat()
        //{
        //    string query = "select * from MastParty where BeatId=" + ddlbeat5.SelectedValue;
        //    DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, query);
        //    if (dt.Rows.Count > 0)
        //    {

        //    }
        //    else
        //    {

        //    }

        //}

        protected void GridView2_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int index = Convert.ToInt32(e.RowIndex);
            DataTable dt = (DataTable)ViewState["CurrentTable"];
            dt.Rows[index].Delete();
            dt.AcceptChanges();
            ViewState["CurrentTable"] = dt;
        }
        private int CheckMeet()
        {
            try
            {
                string str = "select count(*) from TransMeetPlanEntry where SMId=" + ddlunderUser.SelectedValue + " and MeetTypeId=" + ddlmeetType.SelectedValue + " and MeetDate='" + Settings.dateformat(txtMeetDate.Text) + "'";
                int exists = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, str));
                return exists;
            }
            catch
            {
                return 0;

            }
        }

        protected void btnsave_Click(object sender, EventArgs e)
        {
            btnsave.Enabled = false;
            try
            {
                decimal s = Settings.DMDecimal(txtastralSharing.Text) + Settings.DMDecimal(txtDistributerSharing.Text);
                if (s != 100)
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Total sharing percentage should be 100 %');", true);
                }
                else
                {
                    if (CheckMeet() > 0)
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('This meet already Exists');", true);
                    }
                    else
                    {
                        string docID = Settings.GetDocID("MPLAN", DateTime.Now);
                        Settings.SetDocID("MPLAN", docID);

                        string DistributorpartyId = "0";

                        //  DistributorpartyId =hfCustomerId.Value;

                        //Added As Per UAT 07-12-2015 
                        string distId1 = "";
                        if (txtDist.Text != string.Empty)
                        {
                            DistributorpartyId = hfDistId.Value;
                        }
                        //End

                        int RetSave = 0;// ME.Insert(docID, Settings.DMInt32(Settings.Instance.UserID), Settings.DMInt32(ddlunderUser.SelectedValue), Settings.DMInt32(ddlbeat5.SelectedValue), Settings.DMInt32(ddlmeetType.SelectedValue), Settings.DMInt32("0"), Settings.DMInt32(ddlindrustry.SelectedValue), Settings.DMInt32(DistributorpartyId), Settings.DMInt32(txtNoOfUsers.Text), "", "", txtVenue.Text, txtComments.Text, txttypeofGiftsforenduser.Text, Settings.DMDecimal(txtvalueforenduser.Text), "", Settings.DMInt32(txtgiftqty.Text), txtMeetDate.Text, (ddlmeetType.SelectedItem.Text + " " + txtMeetDate.Text), ddlmeetCity.SelectedValue, Settings.DMDecimal(txtApproxBudget.Text), Settings.DMInt32("0"), txtVenue.Text, Settings.DMDecimal(txtDistributerSharing.Text), Settings.DMDecimal(txtastralSharing.Text), Settings.DMInt32(ddlscheme.SelectedValue), Settings.DMInt32(txtNoofStaf.Text), true, "Pending", Settings.DMInt32(Hidparty.Value));
                        if (RetSave > 0)
                        {

                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Inserted Successfully');", true);
                            reset();
                        }
                        else
                        {
                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record Cannot be Insert');", true);

                        }
                    }
                }
            }
            catch (Exception ex) { }
            btnsave.Enabled = true;

        }

        private void InsertProduct(int meetPlanID, int srNo, int ItemGrpId, int ItemId, int ClassiD, int SegmentId)
        {
            try { int resaveProduct = ME.InsertMeetProduct(meetPlanID, srNo, ItemGrpId, ItemId, ClassiD, SegmentId); }
            catch (Exception ex) { }
        }

        private void InsertrMeetParty(int meetPlanID, int srNo, int PartyID, string MobileNo, string Address1)
        {
            try { int resaveParty = ME.InsertMeetParty(meetPlanID, srNo, PartyID, MobileNo, Address1, ""); }
            catch (Exception ex) { }
        }
        private void reset()
        {
            txtComments.Text = "";
            // hfCustomerId.Value = "0";
            //  txtdistName.Text="";
            txtDistributerSharing.Text = "";
            txtastralSharing.Text = "";
            ddlindrustry.SelectedValue = "0";
            txtApproxBudget.Text = "";
            txtgiftqty.Text = "";
            ddlmeetType.SelectedValue = "0";
            ddlmeetType.SelectedValue = "0";
            txtNoOfUsers.Text = "";
            txtNoofStaf.Text = "";
            txttypeofGiftsforenduser.Text = "";

            //Added As Per UAT 07-12-2015
            txtDist.Text = string.Empty;
            //End

            // txttypeofGiftsforretailer.Text = "";
            txtvalueforenduser.Text = "";
            // txtvalueforretailer.Text = "";
            txtVenue.Text = "";
            Hidparty.Value = "0";
            ddlunderUser.SelectedIndex = 0;
            ddlbeat5.SelectedIndex = 0;
            txtParty.Text = "";
            ddlscheme.SelectedIndex = 0;
            txtParty.ReadOnly = true;
            BindDDlCity();

        }

        protected void Cancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/MeetPlanEntry.aspx");
        }

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> SearchParty(string prefixText, string contextKey)
        { //Ankita - 20/may/2016- (For Optimization)
            //string str = "select * FROM MastParty where (PartyName like '%" + prefixText + "%') and PartyDist=0 and BeatId=" + contextKey + "";
            string str = "select PartyId,PartyName FROM MastParty where (PartyName like '%" + prefixText + "%') and PartyDist=0 and BeatId=" + contextKey + "";
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

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> SearchItem(string prefixText, string contextKey)
        {
            string str = @"SELECT T1.PartyId,T1.PartyName FROM MastParty AS T1 WHERE (PartyName like '%" + prefixText + "%') and  T1.PartyDist=1 AND T1.Active=1  and T1.CityId in (select AreaId from mastarea where areaid in (select areaid from MastAreaGrp where MainGrp " +
                  "in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode=" + Convert.ToInt32(Settings.Instance.SMID) + ")) and areatype='City' and Active=1) ORDER BY PartyName";
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


        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> SearchItem1(string prefixText)
        {
            //Ankita - 20/may/2016- (For Optimization)
            //string str = "select * FROM MastItem where (ItemName like '%" + prefixText + "%' or SyncId like '%" + prefixText + "%' or ItemCode like '%" + prefixText + "%') and ItemType='Item' and Active=1";
            string str = "select SyncId,ItemName,ItemCode,ItemId FROM MastItem where (ItemName like '%" + prefixText + "%' or SyncId like '%" + prefixText + "%' or ItemCode like '%" + prefixText + "%') and ItemType='Item' and Active=1";
            DataTable dt = new DataTable();

            dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);

            List<string> customers = new List<string>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem("(" + dt.Rows[i]["SyncId"].ToString() + ")" + " " + dt.Rows[i]["ItemName"].ToString() + " " + "(" + dt.Rows[i]["ItemCode"].ToString() + ")", dt.Rows[i]["ItemId"].ToString());
                customers.Add(item);
                //customers.Add("(" + dt.Rows[i]["SyncId"].ToString() + ")" + " " + dt.Rows[i]["ItemName"].ToString() + " " + "(" + dt.Rows[i]["ItemCode"].ToString() + ")");
            }
            return customers;
        }

        protected void ddlbeat5_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtParty.Text = "";
            if (ddlbeat5.SelectedIndex > 0)
            { txtParty.ReadOnly = false; }
            else
            {
                txtParty.ReadOnly = true;
                Hidparty.Value = "0";
            }
        }

        protected void ddlunderUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDDlCity();
        }

        //Added As Per UAT 07-12-2015
        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> SearchDist(string prefixText, string contextKey)
        {
            //string str = "select * FROM MastParty where (PartyName like '%" + prefixText + "%') and PartyDist=1 and Active=1 and CityId=" + Convert.ToInt32(contextKey) + "";
            //Ankita - 20/may/2016- (For Optimization)
            //string str = "select mp.*,ma.AreaName FROM MastParty  mp left join MastArea ma on mp.CityId=ma.AreaId where (mp.PartyName like '%" + prefixText + "%' or mp.SyncId like '%" + prefixText + "%' or ma.AreaName like '%" + prefixText + "%' ) and mp.PartyDist=1 and mp.Active=1 and mp.CityId in (" + contextKey + ")";
            string str = "select mp.PartyName,mp.SyncId,mp.PartyId,ma.AreaName FROM MastParty  mp left join MastArea ma on mp.CityId=ma.AreaId where (mp.PartyName like '%" + prefixText + "%' or mp.SyncId like '%" + prefixText + "%' or ma.AreaName like '%" + prefixText + "%' ) and mp.PartyDist=1 and mp.Active=1 and mp.CityId in (" + contextKey + ")";
            DataTable dt = new DataTable();
            dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            List<string> customers = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem("(" + dt.Rows[i]["PartyName"].ToString() + ")" + " " + dt.Rows[i]["SyncId"].ToString() + " " + "(" + dt.Rows[i]["AreaName"].ToString() + ")", dt.Rows[i]["PartyId"].ToString());
                customers.Add(item);
            }
            return customers;
        }

        protected void ddlmeetCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            hdfCityIdStr.Value = ddlmeetCity.SelectedValue;
        }
        //End
    }
}