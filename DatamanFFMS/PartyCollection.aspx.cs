using DAL;
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

namespace AstralFFMS
{
    public partial class PartyCollection : System.Web.UI.Page
    {
        BAL.PartyCollection.PartyCollection dp = new BAL.PartyCollection.PartyCollection();
     
          int PartyId = 0;
          int AreaId = 0;
        string parameter = "";
        string VisitID = "0";
        string CityID = "0";
        String Level = "0";
        protected void Page_Load(object sender, EventArgs e)
        {
           
            parameter = Request["__EVENTARGUMENT"];
            txCheqDate.Attributes.Add("readonly", "readonly");
            txtdocumentdate.Attributes.Add("readonly", "readonly");
            CalendarExtender2.EndDate = DateTime.Now;
            
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
            if (Request.QueryString["VisitID"] != null)
            {
                VisitID = Request.QueryString["VisitID"].ToString();
                try
                { CalendarExtender2.StartDate = Convert.ToDateTime(Settings.GetVisitDate(Convert.ToInt32(VisitID))); }
                catch { }
            }
           
            if (Request.QueryString["CityID"] != null)
            {
                CityID = Request.QueryString["CityID"].ToString();
            }
            if (Request.QueryString["AreaId"] != null)
            {
                AreaId = Convert.ToInt32(Request.QueryString["AreaId"].ToString());
            }
            if (Request.QueryString["Level"] != null)
            {
                Level = Request.QueryString["Level"].ToString();
            }
            CalendarExtender2.EndDate = Convert.ToDateTime(Settings.GetVisitDate(Convert.ToInt32(VisitID)));
            if (!IsPostBack)
            {
                try
                {
                    //txtdocumentdate.Text = DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MMM/yyyy");
                    txtdocumentdate.Text = DateTime.Parse(Settings.GetVisitDate(Convert.ToInt32(VisitID))).ToString("dd/MMM/yyyy");
                    try
                    {
                        lblVisitDate5.Text = DateTime.Parse(Convert.ToDateTime(Settings.GetVisitDate(Convert.ToInt32(VisitID))).ToShortDateString()).ToString("dd/MMM/yyyy");
                    }
                    catch { }
                  //  lblVisitDate5.Text = string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Settings.GetVisitDate(Convert.ToInt32(VisitID))).ToShortDateString());
                    lblAreaName5.Text = Settings.Instance.AreaName;
                    lblBeatName5.Text = Settings.Instance.BeatName;

                }
                catch
                {

                }
                divdocid.Visible = false;
                btnDelete.Visible = false;
                mainDiv.Style.Add("display", "block");
                GetPartyData(PartyId);
                if (Request.QueryString["CollId"] != null)
                {
                    FillDeptControls(Convert.ToInt32(Request.QueryString["CollId"]));
                }
            }
        }
        //Ankita - 18/may/2016- (For Optimization)
        private void GetPartyData(int PartyId)
        {
            string str = "select PartyName,Address1,Address2,Mobile,Pin from MastParty where PartyId =" + Convert.ToInt32(PartyId);
            //string str = "select * from MastParty where PartyId =" + Convert.ToInt32(PartyId);
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
        private void fillRepeter()
        {

            string str = @"select * from Temp_TransCollection  DC inner join MastParty MP on DC.PartyId=MP.PartyId where  DC.SMId=" + Settings.Instance.DSRSMID + " and  DC.UserId=" + Settings.Instance.UserID + " and DC.PartyId=" + PartyId;
            DataTable depdt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            rpt.DataSource = depdt;
            rpt.DataBind();
        }
        private void FillDeptControls(int depId)
        {
            try
            {
                string str = @"select * from Temp_TransCollection DC left join MastParty MP on DC.PartyId=MP.PartyId where CollId=" + depId;
                DataTable deptValueDt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                if (deptValueDt.Rows.Count > 0)
                {
                    
                    txtRemark.Text = deptValueDt.Rows[0]["Remarks"].ToString();
                    txtCHDDNO.Text = deptValueDt.Rows[0]["Cheque_DDNo"].ToString();
                    txtbranch.Text = deptValueDt.Rows[0]["Branch"].ToString();
                    txtbank.Text = deptValueDt.Rows[0]["Bank"].ToString();
                    txtAmount.Text = deptValueDt.Rows[0]["Amount"].ToString();
                    RadioButtonList1.SelectedValue = deptValueDt.Rows[0]["Mode"].ToString();
                    txCheqDate.Text = string.Format("{0:dd/MM/yyyy}", deptValueDt.Rows[0]["Cheque_DD_Date"]);
                    txtdocumentdate.Text = string.Format("{0:dd/MM/yyyy}", deptValueDt.Rows[0]["PaymentDate"]);
                    btnSave.Text = "Update";
                    btnDelete.Visible = true;
                    lbldocno.Text = deptValueDt.Rows[0]["CollDocId"].ToString();
                    divdocid.Visible = true;
                    if(deptValueDt.Rows[0]["Mode"].ToString()=="Cash")
                    {

                    }
                  

                }

            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
          
            try
            {
                if (btnSave.Text == "Update")
                {
                    UpdateRecord();
                }
                else
                {
                    InsertRecord();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while inserting the records');", true);
            }
        }

        private void InsertRecord()
        {
            //if (checkDDCHQNo(txtCHDDNO.Text) == 0)
            //{
            //if (Convert.ToDateTime(txtdocumentdate.Text) <= DateTime.Now)
            //{
            if (RadioButtonList1.SelectedValue == "Cash")
            {
                txtCHDDNO.Text = "";
                txtbank.Text = "";
                txtbranch.Text = "";
            }
            string docID = Settings.GetDocID("PACOL", Convert.ToDateTime(txtdocumentdate.Text));
                Settings.SetDocID("PACOL", docID);
                string chdate = "";
                if (txCheqDate.Text != "")
                {
                    chdate = Convert.ToDateTime(txCheqDate.Text).ToShortDateString();
                }
                int retsave = dp.Insert(docID, Convert.ToInt32(Settings.Instance.UserID), Convert.ToInt32(PartyId), Convert.ToInt32(Settings.Instance.DSRSMID), RadioButtonList1.SelectedValue, Convert.ToDecimal(txtAmount.Text), Convert.ToDateTime(txtdocumentdate.Text), txtCHDDNO.Text, chdate, txtbank.Text, txtbranch.Text, txtRemark.Text, VisitID, Settings.GetVisitDate(Convert.ToInt32(VisitID)), AreaId);

                if (retsave != 0)
                {
                    string updateandroidid = "update temp_transcollection set android_id='" + docID + "' where colldocid='" + docID + "'";
                    DbConnectionDAL.ExecuteNonQuery(CommandType.Text, updateandroidid);
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Inserted Successfully -" + docID + "');", true);
                    ClearControls();
                    btnDelete.Visible = false;
                    divdocid.Visible = false;
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record Cannot be Insert');", true);
                }
            //}
            //else
            //{
            //    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Document Date Cannot be Greater than Current Date');", true);
            //}
        }
            //else
            //{
            //    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Cheque/DD No already Exists');", true);
            //}

        

        private int checkDDCHQNo(string No)
        {
            string str = "select count(*) from Temp_TransCollection where Cheque_DDNo='" + No + "'";
            int exists = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, str));
            return exists;
        }
        private int checkDDCHQNoUpdate(string No)
        {
            string str = "select count(*) from Temp_TransCollection where Cheque_DDNo='" + No + "' and CollId !=" + ViewState["CollId"] + "";
            int exists = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, str));
            return exists;
        }

        private void ClearControls()
        {
            txtRemark.Text = "";
            txtCHDDNO.Text = "";
            txtbranch.Text = "";
            txtbank.Text = "";
            txtAmount.Text = "";
            btnDelete.Visible = false;
            btnSave.Text = "Save";
            txCheqDate.Text = "";
            txtdocumentdate.Text = "";
            lbldocno.Text = "";
            divdocid.Visible = false;
        }

        private void UpdateRecord()
        {
            //if (checkDDCHQNoUpdate(txtCHDDNO.Text) == 0)
            //{
            if (RadioButtonList1.SelectedValue == "Cash")
            {
                txtCHDDNO.Text = "";
               
                txtbank.Text = "";
                txtbranch.Text = "";
                txCheqDate.Text = "";

            }
                string chdate = "";
                if (txCheqDate.Text != "")
                {
                    chdate = Convert.ToDateTime(txCheqDate.Text).ToShortDateString();
                }
                int retsave = dp.Update(Convert.ToInt32(ViewState["CollId"]), Convert.ToInt32(Settings.Instance.UserID), Convert.ToInt32(PartyId), Convert.ToInt32(Settings.Instance.DSRSMID), RadioButtonList1.SelectedValue, Convert.ToDecimal(txtAmount.Text), Convert.ToDateTime(txtdocumentdate.Text), txtCHDDNO.Text, chdate, txtbank.Text, txtbranch.Text, txtRemark.Text, Settings.Instance.VistID, Settings.GetVisitDate(Convert.ToInt32(VisitID)),AreaId);

                if (retsave == 1)
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Updated Successfully');", true);
                    btnDelete.Visible = false;
                    btnSave.Text = "Save";
                    ClearControls();
                    divdocid.Visible = false;
                }

                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Duplicate SyncId Exists');", true);
                }
            //}
            //else
            //{
            //    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Cheque/DD No already Exists');", true);
            //}
        }



        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearControls();
           // Response.Redirect("~/PartyCollection.aspx");
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
                    btnSave.Text = "Save";
                    ClearControls();

                }
            }
            else
            {
                // this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('You clicked NO!')", true);
            }


        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            mainDiv.Style.Add("display", "block");
            rptmain.Style.Add("display", "none");
        }

        protected void btnFind_Click(object sender, EventArgs e)
        {
            fillRepeter();
            btnDelete.Visible = false;
            btnSave.Text = "Save";
            mainDiv.Style.Add("display", "none");
            rptmain.Style.Add("display", "block");
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/PartyDashboard.aspx?PartyId=" + PartyId + "&AreaId=" + AreaId + "&CityID=" + CityID + "&VisitID=" + VisitID+"&Level="+Level);
        }

    }
}