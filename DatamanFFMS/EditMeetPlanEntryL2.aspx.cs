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
    public partial class EditMeetPlanEntryL2 : System.Web.UI.Page
    {
        BAL.Meet.MeetPlanEntryBAL ME = new BAL.Meet.MeetPlanEntryBAL();
        string MeetPlanID = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            txtMeetDate.Attributes.Add("readonly", "readonly");
            if (Request.QueryString["MeetPlanID"] != null)
            {
                MeetPlanID = Request.QueryString["MeetPlanID"].ToString();
            }
            if (!IsPostBack)
            {
                try
                {
                    fillUnderUsers();
                    txtMeetDate.Text = DateTime.Now.ToShortDateString();
                    fillArea();
                    BindIndustry();
                    fillMeetType();
                    fillAreaType();
                    fillscheme();
                    fillBeat();
                    txtParty.ReadOnly = true;

                    BindDDlCity();

                    if (Request.QueryString["MeetPlanID"] != null)
                    {
                        MeetPlanID = Request.QueryString["MeetPlanID"].ToString();
                        FillMeet();
                    }
                }
                catch
                {
                }
            }
        }

        private void fillDisName(int disID)
        {
            string str = "select * from MastParty where PartyId=" + disID;
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (dt.Rows.Count > 0)
            {
                txtParty.Text = dt.Rows[0]["PartyName"].ToString();
            }
            else
            {
                txtParty.Text = "";
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

        private void FillMeet()
        {
            try
            {
                string str = "select * from TransMeetPlanEntry where MeetPlanId=" + MeetPlanID;
                DataTable Meetdt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                if (Meetdt.Rows.Count > 0)
                {
                    ddlmeetType.SelectedValue = Meetdt.Rows[0]["MeetTypeId"].ToString();
                    ddlindrustry.SelectedValue = Meetdt.Rows[0]["IndId"].ToString();
                    Hidparty.Value = Meetdt.Rows[0]["RetailerPartyID"].ToString();
                    txtNoOfUsers.Text = Meetdt.Rows[0]["NoOfUser"].ToString();
                    txtVenue.Text = Meetdt.Rows[0]["Venue"].ToString();
                    txtComments.Text = Meetdt.Rows[0]["Comments"].ToString();
                    txttypeofGiftsforenduser.Text = Meetdt.Rows[0]["typeOfGiftEnduser"].ToString();
                    txtvalueforenduser.Text = Meetdt.Rows[0]["valueofEnduser"].ToString();
                    //   txttypeofGiftsforretailer.Text = Meetdt.Rows[0]["typeOfGiftRetailer"].ToString();
                    txtgiftqty.Text = Meetdt.Rows[0]["valueofRetailer"].ToString();
                    //    txtMeetDate.Text = string.Format("{0:dd/MMM/yyyy}", Meetdt.Rows[0]["MeetDate"].ToString());

                    //Added As Per UAT -  07-12-2015
                    if (Meetdt.Rows[0]["DistId"] != DBNull.Value)
                    {
                        int partyId = Convert.ToInt32(Meetdt.Rows[0]["DistId"]);
                        hfDistId.Value = partyId.ToString();
                        string strNew = "select  mp.*,ma.AreaName FROM MastParty mp left join MastArea ma on mp.CityId=ma.AreaId where mp.PartyId=" + partyId + " and mp.PartyDist=1";
                        DataTable dt = new DataTable();

                        dt = DbConnectionDAL.GetDataTable(CommandType.Text, strNew);
                        if (dt.Rows.Count > 0)
                        {
                            txtDist.Text = "(" + dt.Rows[0]["PartyName"].ToString() + ")" + " " + dt.Rows[0]["SyncId"].ToString() + " " + "(" + dt.Rows[0]["AreaName"].ToString() + ")";
                        }
                    }
                    else { txtDist.Text = string.Empty; }
                    //End

                    txtMeetDate.Text = DateTime.Parse(Meetdt.Rows[0]["MeetDate"].ToString()).ToString("dd/MMM/yyyy");
                    ddlmeetCity.SelectedValue = Meetdt.Rows[0]["MeetLoaction"].ToString();
                    txtApproxBudget.Text = Meetdt.Rows[0]["LambBudget"].ToString();
                    txtVenue.Text = Meetdt.Rows[0]["VenueId"].ToString();
                    txtDistributerSharing.Text = Meetdt.Rows[0]["ExpShareDist"].ToString();
                    txtastralSharing.Text = Meetdt.Rows[0]["ExpShareSelf"].ToString();
                    ddlscheme.SelectedValue = Meetdt.Rows[0]["SchId"].ToString();
                    txtNoofStaf.Text = Meetdt.Rows[0]["NoStaff"].ToString();
                    //  ddlUser.SelectedValue = Meetdt.Rows[0]["SMId"].ToString();
                    ddlunderUser.SelectedValue = Meetdt.Rows[0]["SMId"].ToString();
                    fillDisName(Convert.ToInt32(Meetdt.Rows[0]["RetailerPartyID"].ToString()));
                    ddlbeat5.SelectedValue = Meetdt.Rows[0]["AreaId"].ToString();
                    ddlmeetCity.SelectedValue = Meetdt.Rows[0]["MeetLoaction"].ToString();
                    ddlunderUser.Enabled = false;
                }
            }
            catch
            {

            }
        }

        private void BindIndustry()
        {
            ddlindrustry.Items.Clear();
            string str = "select * from MastItemClass";
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
            string str = @"select * from mastarea where UnderId in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA'
                           and PrimCode=" + ddlunderUser.SelectedValue + ")) and  areatype='beat' and Active=1 order by AreaName";

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
        }
        private void BindDDlCity()
        {
            ddlmeetCity.Items.Clear();
            string str = @"select * from mastarea where areaid in (select underid from mastarea where areaId in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA'
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
                }
                catch { }
            }
            ddlmeetCity.Items.Insert(0, new ListItem("-- Select --", "0"));
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

        private void fillPartyBeat()
        {
            string query = "select * from MastParty where BeatId=" + ddlbeat5.SelectedValue;
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, query);
            if (dt.Rows.Count > 0)
            {

            }
            else
            {

            }

        }

        //private void fillPartyViapartType()
        //{
        //    var query = from h in context.MastParties.Where(u => u.AreaId == Convert.ToInt32(ddlareaSearch.SelectedValue) && u.PartyType==Convert.ToInt32( ddlPartyType.SelectedValue))
        //                select h;
        //    DataTable dt = BusinessClass.LINQResultToDataTable(query);
        //    if (dt.Rows.Count > 0)
        //    {
        //        GridView1.DataSource = dt;
        //        GridView1.DataBind();

        //    }
        //    else
        //    {
        //        GridView1.DataSource = null;
        //        GridView1.DataBind();
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




        protected void btnsave_Click(object sender, EventArgs e)
        {
            try
            {
                string docID = Settings.GetDocID("MPLAN", DateTime.Now);
                Settings.SetDocID("MPLAN", docID);
                string DistributorpartyId = "0";
                // DistributorpartyId = hfCustomerId.Value;

                //Added As Per UAT 07-12-2015 
                if (txtDist.Text != string.Empty)
                {
                    DistributorpartyId = hfDistId.Value;
                }
                //End

                decimal s = Settings.DMDecimal(txtastralSharing.Text) + Settings.DMDecimal(txtDistributerSharing.Text);
                if (s != 100)
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Total sharing percentage should be 100 %');", true);
                }
                else
                {
                    int RetSave = 0;// ME.Update(Settings.DMInt64(MeetPlanID), docID, Settings.DMInt32(Settings.Instance.UserID), Settings.DMInt32(ddlunderUser.SelectedValue), Settings.DMInt32(ddlbeat5.SelectedValue), Settings.DMInt32(ddlmeetType.SelectedValue), Settings.DMInt32("0"), Settings.DMInt32(ddlindrustry.SelectedValue), Settings.DMInt32(DistributorpartyId), Settings.DMInt32(txtNoOfUsers.Text), "", "", txtVenue.Text, txtComments.Text, txttypeofGiftsforenduser.Text, Settings.DMDecimal(txtvalueforenduser.Text), "", Settings.DMDecimal(txtgiftqty.Text), txtMeetDate.Text, (ddlmeetType.SelectedItem.Text + " " + txtMeetDate.Text), ddlmeetCity.SelectedValue, Settings.DMDecimal(txtApproxBudget.Text), Settings.DMInt32("0"), txtVenue.Text, Settings.DMDecimal(txtDistributerSharing.Text), Settings.DMDecimal(txtastralSharing.Text), Settings.DMInt32(ddlscheme.SelectedValue), Settings.DMInt32(txtNoofStaf.Text), true, "Pending", Settings.DMInt32(Hidparty.Value));
                    if (RetSave > 0)
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Updated Successfully');", true);
                        // reset();
                    }
                    else
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record Cannot be Update');", true);

                    }
                }
            }
            catch (Exception ex) { }
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
            // txtdistName.Text = "";
            txtDistributerSharing.Text = "";
            txtastralSharing.Text = "";
            ddlindrustry.SelectedValue = "0";
            txtApproxBudget.Text = "";
            // ddlmeetLocation.SelectedValue = "0";

            //Added As Per UAT 07-12-2015
            txtDist.Text = string.Empty;
            //End

            ddlmeetType.SelectedValue = "0";
            ddlmeetType.SelectedValue = "0";
            txtNoOfUsers.Text = "";
            txtNoofStaf.Text = "";
            txttypeofGiftsforenduser.Text = "";
            //  txttypeofGiftsforretailer.Text = "";
            txtvalueforenduser.Text = "";
            // txtvalueforretailer.Text = "";
            txtVenue.Text = "";
            Hidparty.Value = "0";
            txtgiftqty.Text = "";
        }

        protected void Cancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/MeetPlanListL2.aspx");
        }

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> SearchParty(string prefixText, string contextKey)
        {
            string str = "select * FROM MastParty where (PartyName like '%" + prefixText + "%') and PartyDist=0 and BeatId=" + contextKey + "";
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

            string str = "select * FROM MastItem where (ItemName like '%" + prefixText + "%' or SyncId like '%" + prefixText + "%' or ItemCode like '%" + prefixText + "%') and ItemType='Item' and Active=1";
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
            if (ddlbeat5.SelectedIndex > 0)
            { txtParty.ReadOnly = false; }
            else
            {
                txtParty.ReadOnly = true;
                Hidparty.Value = "0";
            }
        }

        protected void btndelete_Click(object sender, EventArgs e)
        {
            try
            {
                string confirmValue = Request.Form["confirm_value"];
                if (confirmValue == "Yes")
                {
                    if (Request.QueryString["MeetPlanID"] != null)
                    {
                        string s = "delete from TransMeetPlanEntry where MeetPlanId=" + MeetPlanID;
                        DbConnectionDAL.ExecuteNonQuery(CommandType.Text, s);

                        string s1 = "delete from TransAddMeetUser where MeetId=" + MeetPlanID;
                        DbConnectionDAL.ExecuteNonQuery(CommandType.Text, s1);

                        string s2 = "delete from TransMeetPlanProduct where MeetPlanId=" + MeetPlanID;
                        DbConnectionDAL.ExecuteNonQuery(CommandType.Text, s2);
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record delete Successfully');", true);

                        HtmlMeta meta = new HtmlMeta();
                        meta.HttpEquiv = "Refresh";
                        meta.Content = "2;url=MeetPlanList.aspx";
                        // meta.Content = "5;url=Page2.aspx";
                        this.Page.Controls.Add(meta);
                        Response.Redirect("MeetPlanList.aspx");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record Cannot be delete');", true);
            }


        }

        //Added As Per UAT 07-12-2015
        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> SearchDist(string prefixText, string contextKey)
        {
            //string str = "select * FROM MastParty where (PartyName like '%" + prefixText + "%') and PartyDist=1 and Active=1 and CityId=" + Convert.ToInt32(contextKey) + "";
            string str = "select mp.*,ma.AreaName FROM MastParty  mp left join MastArea ma on mp.CityId=ma.AreaId where (mp.PartyName like '%" + prefixText + "%' or mp.SyncId like '%" + prefixText + "%' or ma.AreaName like '%" + prefixText + "%' ) and mp.PartyDist=1 and mp.Active=1 and mp.CityId in (" + contextKey + ")";

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
    }
}