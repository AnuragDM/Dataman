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
    public partial class OrderEntry : System.Web.UI.Page
    {
        BAL.Order.OrderEntryBAL dp = new BAL.Order.OrderEntryBAL();
          int PartyId = 0;
          int AreaId = 0;
        string parameter = "";
         string VisitID = "";
         string CityID = "";
         string Level = "0"; string pageSalesName = "";

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
                PartyId =Convert.ToInt32(Request.QueryString["PartyId"].ToString());
            }
             if (Request.QueryString["VisitID"] != null)
            {
                VisitID = Request.QueryString["VisitID"].ToString();
            }
             if (Request.QueryString["CityID"] != null)
            {
                CityID = Request.QueryString["CityID"].ToString();
            }
            if (Request.QueryString["AreaId"] != null)
            {
                AreaId =Convert.ToInt32(Request.QueryString["AreaId"].ToString());
            }
            if (Request.QueryString["Level"] != null)
            {
                Level = Request.QueryString["Level"].ToString();
            }
            //Added
            if (Request.QueryString["PageView"] != null)
            {
                pageSalesName = Request.QueryString["PageView"].ToString();
            }
            //End
            if(!IsPostBack)
            {
                try
                {
                    try
                    {
                        lblVisitDate5.Text = DateTime.Parse(Convert.ToDateTime(Settings.GetVisitDate(Convert.ToInt32(VisitID))).ToShortDateString()).ToString("dd/MMM/yyyy");
                    }
                    catch { }
                   // lblVisitDate5.Text = string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Settings.GetVisitDate(Convert.ToInt32(VisitID))).ToShortDateString());
                    lblAreaName5.Text = Settings.Instance.AreaName;
                    lblBeatName5.Text = Settings.Instance.BeatName;
                }
                catch
                {
                }
                GetPartyData(PartyId);
                divdocid.Visible = false;
                btnDelete.Visible = false;
                mainDiv.Style.Add("display", "block");
            }
        }

        private void FillDeptControls(int OrdId)
        {
            try
            {
                string str = @"select * from Temp_TransOrder  where OrdId=" + OrdId;
                DataTable deptValueDt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                if (deptValueDt.Rows.Count > 0)
                {
                    txtTotalAmount.Text = deptValueDt.Rows[0]["OrderAmount"].ToString();
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


//        [WebMethod]
//        public static UserDetails[] AddRec(string ItemID, string meetplanID)
//        {
//            List<UserDetails> details = new List<UserDetails>();
           
//              DataTable   Sdt = new DataTable();
              
//                string str = @"select m.ItemId,i.ItemName from TransMeetPlanProduct m  inner join MastItem i
//                                         on m.ItemId=i.ItemId where m.MeetPlanId='" + meetplanID + "'";
//                Sdt = DbConnectionDAL.GetDataTable(CommandType.Text,str);
            

//            DataRow[] dr = Sdt.Select("ItemId=" + ItemID);
//            if (dr.Length > 0)
//            {
//            }
//            else
//            {
//                if (ItemID != "0")
//                {
//                    var query = from h in context.MastItems.Where(u => u.ItemId == Convert.ToInt32(ItemID))
//                                select new { h.ItemName };
//                    Sdt.Rows.Add();
//                    int count = Sdt.Rows.Count - 1;
//                    if (Sdt.Rows[count].RowState != DataRowState.Deleted)
//                    {
//                        Sdt.Rows[count]["ItemId"] = ItemID;
//                        Sdt.Rows[count]["ItemName"] = query.FirstOrDefault().ItemName;
//                    }
//                }
//            }
//            int i = 0;
//            foreach (DataRow dtrow in Sdt.Rows)
//            {
//                if (dtrow.RowState != DataRowState.Deleted)
//                {
//                    i = i + 1;
//                    UserDetails user = new UserDetails();
//                    user.Sr = Convert.ToString(i);
//                    user.ItemName = dtrow["ItemName"].ToString();
//                    user.ItemId = dtrow["ItemId"].ToString();
//                    details.Add(user);
//                }
//            }

//            return details.ToArray();
//        }
//        public class UserDetails
//        {
//            public string Sr { get; set; }
//            public string ItemName { get; set; }
//            public string ItemId { get; set; }
//        }

        private void GetPartyData(int PartyId)
        {//Ankita - 20/may/2016- (For Optimization)
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
            txtTotalAmount.Text = "0.00";
            btnDelete.Visible = false;
            btnsave.Text = "Save";
            divdocid.Visible =false;
        }
        private void fillRepeter()
        {
            // Nishu 01/06/2016
            //string str = @"select * from Temp_TransOrder where  VisId=" + VisitID + " and UserId=" + Settings.Instance.UserID + " and SmId=" + Settings.Instance.DSRSMID + " and PartyId=" + PartyId;
            string str = @"select * from Temp_TransOrder where  VisId=" + VisitID + " and SmId=" + Settings.Instance.DSRSMID + " and PartyId=" + PartyId;
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

                int RetSave = dp.InsertOrderEntry(Convert.ToInt64(VisitID), docID, Convert.ToInt32(Settings.Instance.UserID), Settings.GetVisitDate(Convert.ToInt32(VisitID)), Convert.ToInt32(Settings.Instance.DSRSMID), PartyId, AreaId, Remark.Text, Convert.ToDecimal(txtTotalAmount.Text));
                string updateandroidid = "update temp_transorder set android_id='" + docID + "' where orddocid='" + docID + "'";
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, updateandroidid);
                if (RetSave > 0)
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Inserted Successfully');", true);
                    this.clearcontrols();
                    btnDelete.Visible = false;
                    divdocid.Visible = false;


                    HtmlMeta meta = new HtmlMeta();
                    meta.HttpEquiv = "Refresh";
                    if (pageSalesName == "Secondary")
                    {
                        meta.Content = "2;url=PartyDashboard.aspx?PartyId=" + PartyId + "&AreaId=" + AreaId + "&CityID=" + CityID + "&VisitID=" + VisitID + "&Level=" + Level + "&PageView=" + pageSalesName;
                    }
                    else
                    {
                        meta.Content = "2;url=PartyDashboard.aspx?PartyId=" + PartyId + "&AreaId=" + AreaId + "&CityID=" + CityID + "&VisitID=" + VisitID + "&Level=" + Level;
                    }
                    this.Page.Controls.Add(meta);
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record Cannot be Insert');", true);
                }
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
                int RetSave = dp.UpdateOrderEntry(Convert.ToInt64(ViewState["CollId"]), Convert.ToInt64(VisitID), Convert.ToInt32(Settings.Instance.UserID), Settings.GetVisitDate(Convert.ToInt32(VisitID)), Convert.ToInt32(Settings.Instance.DSRSMID), PartyId, AreaId, Remark.Text, Convert.ToDecimal(txtTotalAmount.Text));
                if (RetSave > 0)
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Updated Successfully');", true);
                    this.clearcontrols();
                    btnDelete.Visible = false;
                    divdocid.Visible = false;
                    HtmlMeta meta = new HtmlMeta();
                    meta.HttpEquiv = "Refresh";
                    if (pageSalesName == "Secondary")
                    {
                        meta.Content = "2;url=PartyDashboard.aspx?PartyId=" + PartyId + "&AreaId=" + AreaId + "&CityID=" + CityID + "&VisitID=" + VisitID + "&Level=" + Level + "&PageView=" + pageSalesName;
                    }
                    else
                    {
                        meta.Content = "2;url=PartyDashboard.aspx?PartyId=" + PartyId + "&AreaId=" + AreaId + "&CityID=" + CityID + "&VisitID=" + VisitID + "&Level=" + Level;
                    }
                   
                    this.Page.Controls.Add(meta);
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record Cannot be Update');", true);
                }
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

       

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string confirmValue = Request.Form["confirm_value"];
            if (confirmValue == "Yes")
            {
                int retdel = dp.delete(Convert.ToString(ViewState["CollId"]));
                if (retdel == 1)
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Deleted Successfully');", true);
                    btnDelete.Visible = false;
                    btnsave.Text = "Save";
                    clearcontrols();

                    HtmlMeta meta = new HtmlMeta();
                    meta.HttpEquiv = "Refresh";
                    if (pageSalesName == "Secondary")
                    {
                        meta.Content = "2;url=PartyDashboard.aspx?PartyId=" + PartyId + "&AreaId=" + AreaId + "&CityID=" + CityID + "&VisitID=" + VisitID + "&Level=" + Level + "&PageView=" + pageSalesName;
                    }
                    else
                    {
                        meta.Content = "2;url=PartyDashboard.aspx?PartyId=" + PartyId + "&AreaId=" + AreaId + "&CityID=" + CityID + "&VisitID=" + VisitID + "&Level=" + Level;
                    }
                   
                    this.Page.Controls.Add(meta);

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

        protected void btnBack_Click1(object sender, EventArgs e)
        {
            if(pageSalesName=="Secondary")
            {
                Response.Redirect("~/PartyDashboard.aspx?PartyId=" + PartyId + "&AreaId=" + AreaId + "&CityID=" + CityID + "&VisitID=" + VisitID + "&Level=" + Level + "&PageView="+pageSalesName);
            }
            else
            {
                Response.Redirect("~/PartyDashboard.aspx?PartyId=" + PartyId + "&AreaId=" + AreaId + "&CityID=" + CityID + "&VisitID=" + VisitID + "&Level=" + Level);
            }
           
        }


    }
}