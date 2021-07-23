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
using System.IO;

namespace AstralFFMS
{
    public partial class DistributorCollection : System.Web.UI.Page
    {
        BAL.Distributer.DistributerCollection dp = new BAL.Distributer.DistributerCollection();
        string parameter = "";
        string VisitID = "0";
        string CityID = "0";
        string Level = "";
        string PartyId = "0";
        string Visid = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;

            parameter = Request["__EVENTARGUMENT"];
            txCheqDate.Attributes.Add("readonly", "readonly");
            txtdocumentdate.Attributes.Add("readonly", "readonly");
            txtmDate.Attributes.Add("readonly", "readonly");
            txttodate.Attributes.Add("readonly", "readonly");
            CalendarExtender2.EndDate = Settings.GetUTCTime();
            if (parameter != "")
            {
                ViewState["CollId"] = parameter;
                FillDeptControls(Convert.ToInt32(parameter));
                mainDiv.Style.Add("display", "block");
                rptmain.Style.Add("display", "none");
            }
            //Ankita - 19/may/2016- (For Optimization)
            //string pageName = Path.GetFileName(Request.Path);
            string PermAll = Settings.Instance.CheckPagePermissions(pageName, Convert.ToString(Session["user_name"]));
            string[] SplitPerm = PermAll.Split(',');
            if (btnSave.Text == "Save")
            {
                btnSave.Enabled = Convert.ToBoolean(SplitPerm[1]);
               // btnSave.Enabled = Settings.Instance.CheckAddPermission(pageName, Convert.ToString(Session["user_name"]));
                btnSave.CssClass = "btn btn-primary";
            }
            else
            {
                btnSave.Enabled = Convert.ToBoolean(SplitPerm[2]);
               // btnSave.Enabled = Settings.Instance.CheckEditPermission(pageName, Convert.ToString(Session["user_name"]));
                btnSave.CssClass = "btn btn-primary";
            }
            btnDelete.Enabled = Convert.ToBoolean(SplitPerm[3]);
           // btnDelete.Enabled = Settings.Instance.CheckDeletePermission(pageName, Convert.ToString(Session["user_name"]));
            btnDelete.CssClass = "btn btn-primary";
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
            if (Request.QueryString["Level"] != null)
            {
                Level = Request.QueryString["Level"].ToString();
            }
            if (Request.QueryString["PartyId"] != null)
            {
                PartyId = Request.QueryString["PartyId"].ToString();
            }
            CalendarExtender2.EndDate = Settings.GetUTCTime();
            //CalendarExtender2.EndDate = Convert.ToDateTime(Settings.GetVisitDate(Convert.ToInt32(VisitID)));
            
            if (!IsPostBack)
            {
                try
                {
                     txtdocumentdate.Text = DateTime.Parse(DateTime.Now.ToUniversalTime().AddSeconds(19800).ToShortDateString()).ToString("dd/MMM/yyyy");
                    //txtdocumentdate.Text = DateTime.Parse(Settings.GetVisitDate(Convert.ToInt32(VisitID))).ToString("dd/MMM/yyyy");

                    //txtmDate.Text = DateTime.Parse(DateTime.Now.ToUniversalTime().AddSeconds(19800).ToShortDateString()).ToString("dd/MMM/yyyy");
                    //txttodate.Text = DateTime.Parse(DateTime.Now.ToUniversalTime().AddSeconds(19800).ToShortDateString()).ToString("dd/MMM/yyyy");

                    txtmDate.Text = Settings.GetUTCTime().AddMonths(-1).ToString("dd/MMM/yyyy");// System.DateTime.Now.ToShortDateString();
                    txttodate.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");// System.DateTime.Now.ToShortDateString();

                    DataTable d = Settings.UnderUsers(Settings.Instance.SMID);
                    if (d.Rows.Count > 0)
                    {
                        ddlUndeUser.DataSource = d;
                        ddlUndeUser.DataTextField = "SMName";
                        ddlUndeUser.DataValueField = "SMId";
                        ddlUndeUser.DataBind();
                        DIVUnder.Visible = true;
                        ddlUndeUser.SelectedValue = Settings.Instance.SMID;
                    }
                if (Request.QueryString["DistID"] != null)
                {
                  
                    hfCustomerId.Value = Request.QueryString["DistID"].ToString();
                    txtdistName.Text = getdistName(Convert.ToInt32(hfCustomerId.Value));
                    txtdistName.ReadOnly = true;
                    Button1.Visible = true;
                    ddlUndeUser.SelectedValue = Settings.Instance.DSRSMID;
                    ddlUndeUser.Enabled = false;
                }
               else { Button1.Visible = false;
                   //ddlUndeUser.Enabled = false;
               }
                }
                catch{

                }
                divdocid.Visible = false;
                btnDelete.Visible = false;
                mainDiv.Style.Add("display", "block");
                if (Request.QueryString["CollId"] != null)
                {
                    FillDeptControls(Convert.ToInt32(Request.QueryString["CollId"]));
                }
               
            }
        }
        private string  getdistName(int id)
        {
            string st = "select PartyName from MastParty where PartyId=" + id;
            string paname =Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text,st));
            return paname;
        }
        private void fillRepeter()
        {

            //string str = @"select * from DistributerCollection DC inner join MastParty MP on DC.DistId=MP.PartyId where Active=1";
            //DataTable depdt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            //rpt.DataSource = depdt;
            //rpt.DataBind();
        }
        private void FillDeptControls(int depId)
        {
            try
            {
                string str = @"select * from DistributerCollection DC left join MastParty MP on DC.DistId=MP.PartyId where CollId=" + depId;
                DataTable deptValueDt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                if (deptValueDt.Rows.Count > 0)
                {
                    txtdistName.Text =deptValueDt.Rows[0]["PartyName"].ToString();
                    txtRemark.Text = deptValueDt.Rows[0]["Remarks"].ToString();
                    txtCHDDNO.Text = deptValueDt.Rows[0]["Cheque_DDNo"].ToString();
                    txtbranch.Text = deptValueDt.Rows[0]["Branch"].ToString();
                    txtbank.Text = deptValueDt.Rows[0]["Bank"].ToString();
                    txtAmount.Text = deptValueDt.Rows[0]["Amount"].ToString();
                    RadioButtonList1.SelectedValue = deptValueDt.Rows[0]["Mode"].ToString();
                    btnSave.Text = "Update";
                    btnDelete.Visible = true;
                    lbldocno.Text = deptValueDt.Rows[0]["CollDocId"].ToString();
                    divdocid.Visible  = true;
                    hfCustomerId.Value = deptValueDt.Rows[0]["DistId"].ToString();
                    ddlUndeUser.SelectedValue = deptValueDt.Rows[0]["SMID"].ToString();
                    txtdocumentdate.Text = DateTime.Parse(deptValueDt.Rows[0]["PaymentDate"].ToString()).ToString("dd/MMM/yyyy");
                    txCheqDate.Text = DateTime.Parse(deptValueDt.Rows[0]["Cheque_DD_Date"].ToString()).ToString("dd/MMM/yyyy");
          
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
           string  DistributerID= hfCustomerId.Value;
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
            DateTime comdate = DateTime.Now.ToUniversalTime().AddSeconds(19800);
            if (Request.QueryString["DistID"] != null)
            {
                comdate = Convert.ToDateTime(Settings.GetVisitDate(Convert.ToInt32(VisitID)));
            }
            if (Convert.ToDateTime(txtdocumentdate.Text) <= Settings.GetUTCTime())
            {
                string docID = Settings.GetDocID("DISTP", Convert.ToDateTime(txtdocumentdate.Text));
                Settings.SetDocID("DISTP", docID);
                string chdate = "";
                if (txCheqDate.Text != "")
                {
                    chdate = Convert.ToDateTime(txCheqDate.Text).ToShortDateString();
                }
                if (RadioButtonList1.SelectedValue == "Cash")
                {
                    txtCHDDNO.Text = "";
                    chdate = "";
                    txtbank.Text = "";
                    txtbranch.Text = "";
                }
                string str = @"Select VisId From Transvisit Where Smid = '" + ddlUndeUser.SelectedValue + "' and Vdate = '" + Settings.dateformat1(txtdocumentdate.Text) +"'";
                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                if (dt.Rows.Count > 0)
                {
                    VisitID = dt.Rows[0]["VisId"].ToString();


                    int retsave = dp.Insert(docID, Convert.ToInt32(Settings.Instance.UserID), Convert.ToInt32(hfCustomerId.Value), Convert.ToInt32(ddlUndeUser.SelectedValue), RadioButtonList1.SelectedValue, Convert.ToDecimal(txtAmount.Text), Convert.ToDateTime(txtdocumentdate.Text), txtCHDDNO.Text, chdate, txtbank.Text, txtbranch.Text, txtRemark.Text, VisitID, Settings.GetVisitDate(Convert.ToInt32(VisitID)));
                    string updateandroidid = "update distributercollection set android_id='" + docID + "' where colldocid='" + docID + "'";
                    DbConnectionDAL.ExecuteNonQuery(CommandType.Text, updateandroidid);
                    if (retsave != 0)
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Inserted Successfully -" + docID + "');", true);
                        ClearControls();
                        btnDelete.Visible = false;
                        divdocid.Visible = false;

                        //if (Request.QueryString["DistID"] != null)
                        //{
                        //    HtmlMeta meta = new HtmlMeta();
                        //    meta.HttpEquiv = "Refresh";
                        //    meta.Content = "3;url=DSREntryForm1.aspx?CityID=" + CityID + "&VisitID=" + VisitID;
                        //    this.Page.Controls.Add(meta);
                        //}
                    }
                    else
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record Cannot be Insert');", true);
                    }
                }
                else 
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please Fill The DSR First');", true);
                }
                
            }
            else
            {
                if (Request.QueryString["DistID"] != null)
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Document Date Cannot be Greater than Visit Date');", true);
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Document Date Cannot be Greater than Current Date');", true);
                }
            }
            // }
            //else
            //{
            //    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Cheque/DD No already Exists');", true);
            //}
        }
               
        private int  checkDDCHQNo(string No )
        {
            string str = "select count(*) from DistributerCollection where Cheque_DDNo='"+No+"'";
            int exists=Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, str));
            return exists;
        }
        private int checkDDCHQNoUpdate(string No)
        {
            string str = "select count(*) from DistributerCollection where Cheque_DDNo='" + No + "' and CollId !=" + ViewState["CollId"]+ "";
            int exists = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, str));
            return exists;
        }

        private void ClearControls()
        {
            txtRemark.Text = "";
          //  txtdistName.Text = "";
           // hfCustomerId.Value = "";
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
          //  txtmDate.Text = "";
           // txttodate.Text = "";
        }

        private void UpdateRecord()
        {
            //if (checkDDCHQNoUpdate(txtCHDDNO.Text) == 0)
            //{
           
            if (Convert.ToDateTime(txtdocumentdate.Text) <= Settings.GetUTCTime())
                {
                string chdate = "";
                if (txCheqDate.Text != "")
                {
                    chdate = Convert.ToDateTime(txCheqDate.Text).ToShortDateString();
                }
                if(RadioButtonList1.SelectedValue=="Cash")
                {
                    txtCHDDNO.Text="";
                    chdate=""; 
                    txtbank.Text="";
                    txtbranch.Text = "";
                }
                 string str = @"Select VisId From Transvisit Where Smid = '" + ddlUndeUser.SelectedValue + "' and Vdate = '" + Settings.dateformat1(txtdocumentdate.Text) +"'";
                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                if (dt.Rows.Count > 0)
                {
                    VisitID = dt.Rows[0]["VisId"].ToString();
                int retsave = dp.Update(Convert.ToInt32(ViewState["CollId"]), Convert.ToInt32(Settings.Instance.UserID), Convert.ToInt32(hfCustomerId.Value), Convert.ToInt32(ddlUndeUser.SelectedValue), RadioButtonList1.SelectedValue, Convert.ToDecimal(txtAmount.Text), Convert.ToDateTime(txtdocumentdate.Text), txtCHDDNO.Text, chdate, txtbank.Text, txtbranch.Text, txtRemark.Text, VisitID, Settings.GetVisitDate(Convert.ToInt32(VisitID)));
                if (retsave == 1)
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Updated Successfully');", true);
                    btnDelete.Visible = false;
                    btnSave.Text = "Save";
                    ClearControls();
                    divdocid.Visible = false;
                }
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please Fill The DSR First');", true);
                }
                }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Document Date Cannot be Greater than Current Date');", true);
            }

            //    else
            //    {
            //        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Duplicate SyncId Exists');", true);
            //    }
            //}
            //else
            //{
            //    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Cheque/DD No already Exists');", true);
            //}
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (Level == "1")
            {
                txtRemark.Text = "";
                txtCHDDNO.Text = "";
                txtbranch.Text = "";
                txtbank.Text = "";
                txtAmount.Text = "";
                btnDelete.Visible = false;
                txCheqDate.Text = "";
                txtdocumentdate.Text = "";
                lbldocno.Text = "";
                divdocid.Visible = false;
                btnSave.Text = "Save";
                RadioButtonList1.SelectedValue = "Cheque";
                //foreach (ListItem items in RadioButtonList1.Items)
                //{
                //    if (items.Text == "DD")
                //    {

                //        items.Selected = true;
                //    }
                //}
               // txtmDate.Text = "";
               // txttodate.Text = "";
            }
            else if (Level == "2")
            {
                txtRemark.Text = "";
                txtCHDDNO.Text = "";
                txtbranch.Text = "";
                txtbank.Text = "";
                txtAmount.Text = "";
                btnDelete.Visible = false;
                txCheqDate.Text = "";
                txtdocumentdate.Text = "";
                lbldocno.Text = "";
                divdocid.Visible = false;
              //  txtmDate.Text = "";
               // txttodate.Text = "";
            }
            else if (Level == "3")
            {
                txtRemark.Text = "";
                txtCHDDNO.Text = "";
                txtbranch.Text = "";
                txtbank.Text = "";
                txtAmount.Text = "";
                btnDelete.Visible = false;
                txCheqDate.Text = "";
                txtdocumentdate.Text = "";
                lbldocno.Text = "";
                divdocid.Visible = false;
              //  txtmDate.Text = "";
              //  txttodate.Text = "";
            }
            else
            {
                ClearControls();
                Response.Redirect("~/DistributorCollection.aspx");
            }
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
            string str = "";
          //  txtmDate.Text = "";
          //  txttodate.Text = "";
          //  fillRepeter();           
            rpt.DataSource = null;
            rpt.DataBind();

            ////txtdocumentdate.Text = DateTime.Parse(Settings.GetVisitDate(Convert.ToInt32(VisitID))).ToString("dd/MMM/yyyy");
            //str = @"select * from DistributerCollection DC inner join MastParty MP on DC.DistId=MP.PartyId where Active=1 and DC.SMId=" + ddlUndeUser.SelectedValue + " and Paymentdate='" + Settings.dateformat1(txtdocumentdate.Text) + "' ";
            //DataTable depdt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            //if (depdt.Rows.Count > 0)
            //{
            //    rpt.DataSource = depdt;
            //    rpt.DataBind();
            //}
            //else
            //{
            //    rpt.DataSource = null;
            //    rpt.DataBind();
            //}
            btnDelete.Visible = false;
            btnSave.Text = "Save";
            mainDiv.Style.Add("display", "none");
            rptmain.Style.Add("display", "block");
        }

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> SearchItem(string prefixText, string contextKey)
        {
            //Ankita - 19/may/2016- (For Optimization)
//            string str = @"select T1.*,T2.AreaName FROM MastParty AS T1 INNER JOIN MastArea AS T2
//                            ON T1.CityId=T2.AreaId  where ((T2.AreaName like '%" + prefixText + "%') OR (T1.PartyName like '%" + prefixText + "%') OR (T1.SyncId like '%" + prefixText + "%')) and T1.PartyDist=1 and T1.Active=1  " +
//                        " and T1.CityId in (select AreaId from mastarea where areaid in (select underid from mastarea where areaId " +
//                " in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode=" + Convert.ToInt32(contextKey) + ")) and areatype='City' and Active=1) ORDER BY PartyName";
            string citystr = "";
            string cityQry = @"  select AreaId from mastarea where areaid in (select distinct underid from mastarea where areaid in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + contextKey + ")))) and Active=1 ) and areatype='City' and Active=1 order by AreaName";
            DataTable dtCity = DbConnectionDAL.GetDataTable(CommandType.Text, cityQry);
            for (int i = 0; i < dtCity.Rows.Count; i++)
            {
                citystr += dtCity.Rows[i]["AreaId"] + ",";
            }
            citystr = citystr.TrimStart(',').TrimEnd(',');

            string str = @"select T1.PartyName,T1.SyncId,T1.PartyId,T2.AreaName FROM MastParty AS T1 INNER JOIN MastArea AS T2 ON T1.CityId=T2.AreaId  where ((T2.AreaName like '%" + prefixText + "%') OR (T1.PartyName like '%" + prefixText + "%') OR (T1.SyncId like '%" + prefixText + "%')) and T1.PartyDist=1 and T1.Active=1  " +
                         " and T1.CityId in (" + citystr + ") and smid in ((select maingrp from mastsalesrepgrp where smid in (" + contextKey + ") union SELECT smid FROM mastsalesrepgrp WHERE  maingrp in (" + contextKey + ")))  and T1.PartyDist=1 and T1.Active=1 order by T1.PartyName";

            //string str = @"SELECT T1.PartyId,T1.PartyName,T1.SyncId FROM MastParty AS T1 left join MastArea MA on  T1.AreaId=MA.AreaId WHERE (PartyName like '%" + prefixText + "%'  or  T1.SyncId like '%" + prefixText + "%' or MA.AreaName like '%" + prefixText + "%' ) and  T1.PartyDist=1 AND T1.Active=1  and T1.CityId in (select AreaId from mastarea where areaid in (select areaid from MastAreaGrp where MainGrp " +
            //      "in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode=" +Settings.DMInt32(contextKey) + ")) and areatype='City' and Active=1) ORDER BY PartyName";
          //  string str = "select * FROM MastParty where (PartyName like '%" + prefixText + "%' or ) and PartyDist=1";
            DataTable dt = new DataTable();
            dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            List<string> customers = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem("(" + dt.Rows[i]["AreaName"].ToString() + ") "+ dt.Rows[i]["PartyName"].ToString() + " (" + dt.Rows[i]["SyncId"].ToString() + ") " ,  dt.Rows[i]["PartyId"].ToString());
                customers.Add(item);
            }
            return customers;
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            string str = "";
            if (Convert.ToDateTime(txttodate.Text) >= Convert.ToDateTime(txtmDate.Text))
            {
                if (txtmDate.Text != "" && txttodate.Text != "")
                {
                    str = @"select * from DistributerCollection DC inner join MastParty MP on DC.DistId=MP.PartyId where Active=1 and DC.SMId=" + ddlUndeUser.SelectedValue + " and PaymentDate>='" + Settings.dateformat1(txtmDate.Text) + "' and PaymentDate<='" + Settings.dateformat1(txttodate.Text) + "'";
                }
                else
                {
                    str = @"select * from DistributerCollection DC inner join MastParty MP on DC.DistId=MP.PartyId where Active=1 and DC.SMId=" + ddlUndeUser.SelectedValue + "";
                }
                DataTable depdt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                if (depdt.Rows.Count > 0)
                {
                    rpt.DataSource = depdt;
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

        protected void ImgPrint_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton btn = (ImageButton)sender;
            var item = (RepeaterItem)btn.NamingContainer;
            HiddenField POID = (HiddenField)item.FindControl("HiddenField1");
            string str = POID.Value;

            //string s = "select * from DistributerCollection DC inner join MastParty MP on DC.DistId=MP.PartyId where CollId="+str;
            //DataTable dtPrint = DbConnectionDAL.GetDataTable(CommandType.Text, s);
            //if(dtPrint.Rows.Count>0)
            //{

            //}
          //  Session["POIDPrint"] = str;
            Response.Redirect("~/DistributorPrint.aspx?ID=" + str);

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (Level == "1")
            {
                Response.Redirect("DSREntryForm1.aspx?CityID=" + CityID + "&VisitID=" + VisitID +"&Level=1");
            }
            else if (Level == "2")
            {
                Response.Redirect("DistributorDashboardLevel2.aspx?CityID=" + CityID + "&VisitID=" + VisitID + "&DistID=" + PartyId + "&Level=2");
            }
            else
            {
                Response.Redirect("DistributorDashboardLevel3.aspx?CityID=" + CityID + "&VisitID=" + VisitID + "&DistID=" + PartyId + "&Level=3");
            }
        }
    }
}