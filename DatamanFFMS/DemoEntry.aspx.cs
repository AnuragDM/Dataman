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
using System.Web.Services;

namespace AstralFFMS
{
    public partial class DemoEntry : System.Web.UI.Page
    {
        BAL.Order.OrderEntryBAL dp = new BAL.Order.OrderEntryBAL();
        int PartyId = 0;
        int AreaId = 0;
        string parameter = "";
        private static DataTable Sdt;
        protected void Page_Load(object sender, EventArgs e)
        {
            parameter = Request["__EVENTARGUMENT"];

            if (parameter != "")
            {
                ViewState["CollId"] = parameter;
                FillDeptControls(Convert.ToInt32(parameter));
                mainDiv.Style.Add("display", "block");
                rptmain.Style.Add("display", "none");
            }
            if (Request.QueryString["PartyId"] != null)
            {
                PartyId = Convert.ToInt32(Request.QueryString["PartyId"].ToString());
            }
            if (!IsPostBack)
            {
                GetPartyData(PartyId);
                divdocid.Visible = false;
                btnDelete.Visible = false;
                mainDiv.Style.Add("display", "block");
                InitialFillGrid();
            }
        }
        private void InitialFillGrid()
        {
            List<UserDetails> details = new List<UserDetails>();
            Sdt=new DataTable();
            Sdt.Columns.Add("ItemId");
            Sdt.Columns.Add("ItemName");
           Sdt.Rows.Add();
           Sdt.Rows[0]["ItemId"]=0;
           Sdt.Rows[0]["ItemName"] = "";
            gvDetails.DataSource = Sdt;
            gvDetails.DataBind();
            gvDetails.Rows[0].Visible = false;
        }
        private void FillDeptControls(int OrdId)
        {
            try
            {//Ankita - 18/may/2016- (For Optimization)
                //string str = @"select * from Temp_TransOrder  where OrdId=" + OrdId;
                string str = @"select Remarks,OrdDocId from Temp_TransOrder  where OrdId=" + OrdId;
                DataTable deptValueDt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                if (deptValueDt.Rows.Count > 0)
                {
                    //txtTotalAmount.Text = deptValueDt.Rows[0]["OrderAmount"].ToString();
                    Remark.Text = deptValueDt.Rows[0]["Remarks"].ToString();
                    btnsave.Text = "Update";
                    btnDelete.Visible = true;
                    divdocid.Visible = true;
                    lbldocno.Text = deptValueDt.Rows[0]["OrdDocId"].ToString();
                }

            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }


        [WebMethod]
        public static UserDetails[] AddRec(string ItemID)
        {
            List<UserDetails> details = new List<UserDetails>();
            if (Sdt.Rows.Count == 0)
            {
                Sdt.Rows.Add();
            }
            

            if (Sdt.Rows.Count > 0)
            {
                if (Sdt.Rows[0]["ItemId"].ToString() == "0" || Sdt.Rows[0]["ItemId"].ToString() == "")
                {
                    Sdt.Rows[0].Delete();
                }
                DataRow[] dr = Sdt.Select("ItemId=" + ItemID);
                if (dr.Length > 0)
                {

                }

                if (ItemID != "0")
                {
                    string query = "select * from MastItem where ItemId=" + ItemID;
                    DataTable dt1 = new DataTable();
                    dt1 = DbConnectionDAL.GetDataTable(CommandType.Text, query);
                    Sdt.Rows.Add();
                    int count = Sdt.Rows.Count - 1;
                    if (Sdt.Rows[count].RowState != DataRowState.Deleted)
                    {
                        Sdt.Rows[count]["ItemId"] = ItemID;
                        Sdt.Rows[count]["ItemName"] = dt1.Rows[0]["ItemName"].ToString();
                    }
                }
                int i = 0;
                foreach (DataRow dtrow in Sdt.Rows)
                {
                    if (dtrow.RowState != DataRowState.Deleted)
                    {
                        i = i + 1;
                        UserDetails user = new UserDetails();
                        user.Sr = Convert.ToString(i);
                        user.ItemName = dtrow["ItemName"].ToString();
                        user.ItemId = dtrow["ItemId"].ToString();
                        details.Add(user);
                    }
                }
            }

            return details.ToArray();
        }
        public class UserDetails
        {
            public string Sr { get; set; }
            public string ItemName { get; set; }
            public string ItemId { get; set; }
        }


        [WebMethod]
        public static UserDetails[] DelRec(string ItemID)
        {

            int index = -1;
            if (ItemID != "")
            {
                DataRow[] dr1 = Sdt.Select("ItemId=" + ItemID);
                if (dr1.Count() > 0)
                {
                    index = Sdt.Rows.IndexOf(dr1[0]);
                }
                try
                {
                    Sdt.Rows[index].Delete();
                }
                catch
                {

                }
            }
                List<UserDetails> details = new List<UserDetails>();
                int i = 0;
                foreach (DataRow dtrow in Sdt.Rows)
                {
                    if (dtrow.RowState != DataRowState.Deleted)
                    {
                        i = i + 1;
                        UserDetails user = new UserDetails();
                        user.Sr = Convert.ToString(i);
                        user.ItemName = dtrow["ItemName"].ToString();
                        user.ItemId = dtrow["ItemId"].ToString();
                        details.Add(user);
                    }
                }
            
                return details.ToArray();
            }
        



        private void GetPartyData(int PartyId)
        {//Ankita - 18/may/2016- (For Optimization)
            //string str = "select * from MastParty where PartyId =" + Convert.ToInt32(PartyId);
            string str = "select PartyName,Address1,Address2,Mobile,Pin from MastParty where PartyId =" + Convert.ToInt32(PartyId);
            DataTable dt1 = new DataTable();
            dt1 = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (dt1.Rows.Count > 0)
            {
                partyName.Text = dt1.Rows[0]["PartyName"].ToString();
                address.Text = dt1.Rows[0]["Address1"].ToString() + "" + dt1.Rows[0]["Address2"].ToString();
                mobile.Text = dt1.Rows[0]["Mobile"].ToString();
                lblzipcode.Text = dt1.Rows[0]["Pin"].ToString();
            }
        }

        private void clearcontrols()
        {
            Remark.Text = "";
           // txtTotalAmount.Text = "0.00";
            btnDelete.Visible = false;
            btnsave.Text = "Save";
            divdocid.Visible = false;
        }
        private void fillRepeter()
        {

            string str = @"select * from Temp_TransOrder where UserId=" + Settings.Instance.UserID + " and PartyId=" + PartyId;
            DataTable depdt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            rpt.DataSource = depdt;
            rpt.DataBind();
        }

        private void InsertOrder()
        {
            try
            {
                string docID = Settings.GetDocID("ORDSN", DateTime.Now);
                Settings.SetDocID("ORDSN", docID);

                //int RetSave = dp.InsertOrderEntry(Convert.ToInt64(Settings.Instance.VistID), docID, Convert.ToInt32(Settings.Instance.UserID), Settings.GetVisitDate(Convert.ToInt32(Settings.Instance.VistID)), Convert.ToInt32(Settings.Instance.SMID), PartyId, AreaId, Remark.Text, Convert.ToDecimal(txtTotalAmount.Text));
                //if (RetSave > 0)
                //{
                //    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Inserted Successfully');", true);
                //    this.clearcontrols();
                //    btnDelete.Visible = false;
                //    divdocid.Visible = false;
                //}
                //else
                //{
                //    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record Cannot be Insert');", true);
                //}
            }
            catch (Exception ex)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record Cannot be Insert');", true);
                ex.ToString();
            }
        }

        private void UpdateOrder()
        {
            try
            {
                //int RetSave = dp.UpdateOrderEntry(Convert.ToInt64(ViewState["CollId"]), Convert.ToInt64(Settings.Instance.VistID), Convert.ToInt32(Settings.Instance.UserID), Settings.GetVisitDate(Convert.ToInt32(Settings.Instance.VistID)), Convert.ToInt32(Settings.Instance.SMID), PartyId, AreaId, Remark.Text, Convert.ToDecimal(txtTotalAmount.Text));
                //if (RetSave > 0)
                //{
                //    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Inserted Successfully');", true);
                //    this.clearcontrols();
                //    btnDelete.Visible = false;
                //    divdocid.Visible = false;
                //}
                //else
                //{
                //    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record Cannot be Insert');", true);
                //}
            }
            catch (Exception ex)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record Cannot be Insert');", true);
                ex.ToString();
            }
        }

        protected void btnsave_Click(object sender, EventArgs e)
        {

            try
            {
                if (btnsave.Text == "Update")
                {
                    UpdateOrder();
                }
                else
                {
                    InsertOrder();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while inserting the records');", true);
            }
        }

        protected void btnreset_Click(object sender, EventArgs e)
        {
            clearcontrols();
        }

        protected void BtnCancel_Click(object sender, EventArgs e)
        {
            clearcontrols();
            Response.Redirect("~/DemoEntry1.aspx");
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string confirmValue = Request.Form["confirm_value"];
            if (confirmValue == "Yes")
            {
                int retdel = dp.delete(Request.QueryString["CollId"]);
                if (retdel == 1)
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Deleted Successfully');", true);
                    btnDelete.Visible = false;
                    btnsave.Text = "Save";
                    clearcontrols();

                }
            }
            else
            {
                // this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('You clicked NO!')", true);
            }


        }
        protected void btnFind_Click(object sender, EventArgs e)
        {
            fillRepeter();
            btnDelete.Visible = false;
            btnsave.Text = "Save";
            mainDiv.Style.Add("display", "none");
            rptmain.Style.Add("display", "block");
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            mainDiv.Style.Add("display", "block");
            rptmain.Style.Add("display", "none");
        }
        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> SearchItem(string prefixText)
        {

            string str = "select MI.ItemId,MI.ItemName,MI.ItemCode,MI.SyncId,MC.Name as Classname,MS.Name as SegmentName from MastItem MI left join MastItemClass MC on MI.ClassId=MC.Id left join MastItemSegment MS on MI.SegmentId=MS.Id  where (ItemName like '%" + prefixText + "%' or SyncId like '%" + prefixText + "%' or ItemCode like '%" + prefixText + "%' or MC.Name like '%" + prefixText + "%' or MS.Name like '%" + prefixText + "%' ) and ItemType='Item'";
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

    }
}