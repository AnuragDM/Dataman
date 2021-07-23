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
using Ionic.Zip;

namespace AstralFFMS
{
    public partial class ExpenseApproval : System.Web.UI.Page
    {
        string _sql = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;
            DateFrom.Attributes.Add("ReadOnly", "true");
            DateTo.Attributes.Add("ReadOnly", "true");
            if (!Page.IsPostBack)
            {
                Div1.Attributes.Add("style", "display:none");
                DateFrom.Text = Settings.GetUTCTime().AddMonths(-1).ToString("dd/MMM/yyyy");// DateTime.UtcNow.AddSeconds(19800).ToString("dd/MMM/yyyy");
                DateTo.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");// DateTime.UtcNow.AddSeconds(19800).ToString("dd/MMM/yyyy");
                BindStatus(); BindArea();
                Session["IsExport"] = Settings.Instance.CheckExportPermission("ExpenseApproval.aspx", Convert.ToString(Session["user_name"]));
                Session["IsEditView"] = Settings.Instance.CheckEditPermission("ExpenseApproval.aspx", Convert.ToString(Session["user_name"]));
            }
        }
        #region BindDropdown
        //private void BindEmployee()
        // {
        //     string str = "select Id,Name from MastLogin where Active=1 order by Name";
        //     fillDDLDirect(ddlemp, str, "Id", "Name", 1);
        // }
        private void BindStatus()
        {
            ddlstatus.Items.Insert(0, new ListItem("Submitted", "1"));
            ddlstatus.Items.Insert(1, new ListItem("Approved", "2"));
            ddlstatus.Items.Insert(2, new ListItem("Active", "3"));
            ddlstatus.Items.Insert(3, new ListItem("Verified", "4"));
            ddlstatus.Items.Insert(4, new ListItem("Rejected", "5"));
            ddlstatus.Items.Insert(5, new ListItem("All", "0"));

            ddlstatus.DataBind();
        }
        private void BindArea()
        {
            string str = "select AreaId,AreaName from mastarea where areatype='CITY' and ACTIVE=1 order by AreaName";
            fillDDLDirect(ddlcity, str, "AreaId", "AreaName", 1);
        }
        public static void fillDDLDirect(DropDownList xddl, string xmQry, string xvalue, string xtext, int sele)
        {
            DataTable xdt = new DataTable();
            xdt = DbConnectionDAL.GetDataTable(CommandType.Text, xmQry);
            xddl.DataSource = xdt;
            xddl.DataTextField = xtext.Trim();
            xddl.DataValueField = xvalue.Trim();
            xddl.DataBind();
            if (sele == 1)
            {
                if (xdt.Rows.Count >= 1)
                    xddl.Items.Insert(0, new ListItem("--Select--", "0"));
                else if (xdt.Rows.Count == 0)
                    xddl.Items.Insert(0, new ListItem("---", "0"));
            }
            else if (sele == 2)
            {
                xddl.Items.Insert(0, new ListItem("--Others--", "0"));
            }
            xdt.Dispose();
        }
        #endregion
        private void GetPendingList()
        {
            DataTable dt = new DataTable();
            string addqry = "";
            //IsApproved- 1=Approved 2=save expense sheet 3=UnApproved
            if (!string.IsNullOrEmpty(hdnEmpId.Value)) { addqry += "and ml.Id=" + hdnEmpId.Value + ""; }
            if (ddlcity.SelectedIndex > 0) { addqry += "and msr.CityId=" + ddlcity.SelectedValue + ""; }
            if (Convert.ToInt16(ddlstatus.SelectedValue) > 0)
            {

                if (ddlstatus.SelectedValue == "1") { addqry += " and isnull(EG.IsSubmitted,0)=1 and (EG.IsApproved is null or EG.IsApproved=0)  and (eg.IsVerified is null or eg.IsVerified=0) and (eg.openforresubmit is null or eg.openforresubmit=0)"; }
                else if (ddlstatus.SelectedValue == "2") { addqry += " and isnull(EG.IsApproved,0)=1"; }
                else if (ddlstatus.SelectedValue == "4") { addqry += " and isnull(EG.IsVerified,0)=1 And (EG.openforresubmit is null or EG.openforresubmit=0) And (EG.IsApproved is null or EG.IsApproved=0)"; }
                else if (ddlstatus.SelectedValue == "3") { addqry += " and EG.IsSubmitted is null "; }
                else if (ddlstatus.SelectedValue == "5") { addqry += " and EG.openforresubmit=1 and EG.IsSubmitted is null"; }

            }
            //string strq = "SELECT DISTINCT vg.stateName,vg.cityName, EG.expensegroupid,ml.EmpSyncID,ml.EmpName as Name,ml.Id,EG.CreatedOn,EG.DateOfSubmission,  CASE Isnull(eg.isapproved, 0)  WHEN 1 THEN 'Approved' ELSE CASE Isnull(eg.issubmitted, 0) WHEN 0 THEN 'Active' WHEN 1 THEN 'Submitted' END END            AS status,EG.GroupName,EG.fromdate,EG.ToDate,EG.TotalApprovedAmount,EG.totalamount                                 AS TCA,msr.CityId,MXB.maxBillDt,totalverifiedamt,(Select Count(*) From ExpenseAttachmentDetails ea inner join ExpenseDetails ed on ed.ExpenseDetailID=ea.ExpenseDetailID Where ed.ExpenseGroupId=EG.expensegroupid ) as cnt    FROM expensegroup EG INNER JOIN MastLogin Ml ON EG.CreatedBy=Ml.Id LEFT JOIN MastSalesRep Msr ON Msr.UserId=ml.Id LEFT JOIN ViewGeo VG ON VG.cityid =msr.CityId left join (select smid,max(billdate) as maxBillDt from expensedetails expd " +
            //" left join expensegroup expg on expd.expensegroupid=expg.expensegroupid where IsSubmitted=1 group by smid) MXB on mxb.smid=eg.smid WHERE msr.CityId<>0 and cast(EG.CreatedOn as date)>=cast('" + DateFrom.Text + "' as date) and cast(EG.CreatedOn as date)<=cast('" + DateTo.Text + "' as date)" + addqry + "";


            string strq = "SELECT DISTINCT vg.stateName,vg.cityName, EG.expensegroupid,ml.EmpSyncID,ml.EmpName as Name,ml.Id,EG.CreatedOn,EG.DateOfSubmission,[dbo].[GetStatus](EG.expensegroupid)  AS status,EG.GroupName,EG.fromdate,EG.ToDate,EG.TotalApprovedAmount,EG.totalamount                                 AS TCA,msr.CityId,MXB.maxBillDt,totalverifiedamt,(Select Count(*) From ExpenseAttachmentDetails ea inner join ExpenseDetails ed on ed.ExpenseDetailID=ea.ExpenseDetailID Where ed.ExpenseGroupId=EG.expensegroupid ) as cnt    FROM expensegroup EG INNER JOIN MastLogin Ml ON EG.CreatedBy=Ml.Id LEFT JOIN MastSalesRep Msr ON Msr.UserId=ml.Id LEFT JOIN ViewGeo VG ON VG.cityid =msr.CityId left join (select smid,max(billdate) as maxBillDt from expensedetails expd " +
          " left join expensegroup expg on expd.expensegroupid=expg.expensegroupid where IsSubmitted=1 group by smid) MXB on mxb.smid=eg.smid WHERE msr.CityId<>0  and cast(EG.CreatedOn as date)<=cast('" + DateTo.Text + "' as date)" + addqry + "";


            //string str = "select EG.ExpenseGroupId,MSR.SMName,cast(EG.GroupName as varchar) +' -- '+ convert(varchar(12),EG.fromdate,110) + ' -- ' + convert(varchar(12),EG.ToDate,110) as GrpDetails,EG.TotalAmount as TCA  from ExpenseGroup EG inner join MastSalesRep MSR On EG.SMID=MSR.SMID  where IsNull(EG.IsApproved,0)<>1 ";
            dt = DbConnectionDAL.GetDataTable(CommandType.Text, strq);
            Div1.Attributes.Remove("style");
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

        protected void btnsearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToDateTime(DateFrom.Text) > Convert.ToDateTime(DateTo.Text))
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('From Date Should be less than To Date');", true);
                    return;
                }
                GetPendingList();
            }
            catch (Exception xe) { xe.ToString(); }
        }

        protected void rpt_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HiddenField hdfExpGroupId = (HiddenField)e.Item.FindControl("hdfExpGroupId");
                HiddenField hdCnt = (HiddenField)e.Item.FindControl("hfCnt");
                LinkButton btn = e.Item.FindControl("LinkButton1") as LinkButton;
                if (hdCnt.Value != "0")
                {
                    btn.Visible = true;
                }
                else btn.Visible = false;
                //LinkButton btn = e.Item.FindControl("LinkButton1") as LinkButton;
                //btn.Attributes.Add("onClick", "return false;");
                //    HyperLink Hp = (HyperLink)e.Item.FindControl("hpdwnld");
                //    HyperLink Hpv = (HyperLink)e.Item.FindControl("hplview");
                HyperLink Hpe = (HyperLink)e.Item.FindControl("hpledit");
                //    if (Convert.ToBoolean(Session["IsExport"]))
                //    {
                if ((DataBinder.Eval(e.Item.DataItem, "status")).ToString().ToUpper() == "Approved".ToUpper())
                {
                    Hpe.Visible = false;
                }
                else { Hpe.Visible = true; }
                //}
                //    else { Hp.Enabled = false; Hp.ForeColor = System.Drawing.Color.Red; Hp.ToolTip = ""; }

                //    if (!Convert.ToBoolean(Session["IsEditView"])) {
                //        Hpv.Enabled = false; Hpv.ForeColor = System.Drawing.Color.Red; Hpv.ToolTip = "";
                //        Hpe.Enabled = false; Hpe.ForeColor = System.Drawing.Color.Red; Hpe.ToolTip = "";
                //    }
                //    else { Hpv.Enabled = true;  Hpv.ToolTip = "View Expenses";
                //    Hpe.Enabled = true; Hpe.ToolTip = "Edit Expenses";
                //    }
            }
        }
        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> SearchEmployee(string prefixText)
        {
            string str = @"SELECT ml.EmpName,ml.Id,ml.EmpSyncID FROM MastLogin ml INNER JOIN MastSalesRep msr
ON msr.UserId=ml.Id WHERE msr.SMName<>'.' AND (msr.SMName LIKE '%" + prefixText + "%' or ml.Name LIKE '%" + prefixText + "%' )  and ml.Active=1 order by ml.EmpName";

            DataTable dt = new DataTable();
            dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            List<string> employees = new List<string>();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem("" + dt.Rows[i]["EmpName"].ToString() + "" + "( " + dt.Rows[i]["EmpSyncID"].ToString() + " )", dt.Rows[i]["Id"].ToString());
                    employees.Add(item);
                }
            }
            else
            {
                string item = "";
                employees.Add(item);
            }
            return employees;
        }

        protected void rpt_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "download")
            {
                string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ',' });
                string ExpenseGroupID = commandArgs[0];
                string Name = commandArgs[1];
                string GroupName = commandArgs[2];
                // string ExpenseGroupID = e.CommandArgument.ToString();
                GetFile(ExpenseGroupID, Name, GroupName);
                // GetAtt(ExpenseGroupID); 
                //  rty();
            }
        }

        //public void rty()
        //{
        //    string path = Server.MapPath("~/ExpenseAttachment/");//Location for inside Test Folder
        //    string[] Filenames = Directory.GetFiles(path);
        //    using (ZipFile zip = new ZipFile())
        //    {
        //        string path1 = Server.MapPath("~/ExpenseAttachment/245-1583389175734test.jpg");
        //        string[] bn={"G:\\Vikram\\Goldiee\\new\\Goldiee_19_Feb_2020\\Dataman\\DatamanFFMS\\ExpenseAttachment\\1-IMG-20190410-WA0005.jpg"};
        //        zip.AlternateEncodingUsage = ZipOption.AsNecessary;
        //        zip.AddDirectoryByName("ExpenseAttachment");
        //        zip.AddFiles(bn, "ExpenseAttachment");//Zip file inside filename
        //       // zip.Save(@"C:\Users\user\Desktop\Projectzip.zip");//location and name for creating zip file
        //        Response.Clear();
        //        Response.BufferOutput = false;
        //        string zipName = String.Format("Zip_{0}.zip", DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"));
        //        Response.ContentType = "application/zip";
        //        Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
        //        zip.Save(Response.OutputStream);
        //        Response.End();
        //    }

        //}

        public void GetFile(string ExpenseGroupID, string empName, string Group)
        {
            string sql = "";
            string[] files = null;
            List<string> list = new List<string>();
            List<ListItem> filesPath = new List<ListItem>();
            try
            {
                sql = "Select ea.AttachmentUrl From ExpenseAttachmentDetails ea inner join ExpenseDetails ed on ed.ExpenseDetailID=ea.ExpenseDetailID Where ed.ExpenseGroupId=" + ExpenseGroupID + "";
                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        list.Add(Server.MapPath(Convert.ToString(dr["AttachmentUrl"])));
                    }
                    using (ZipFile zip = new ZipFile())
                    {
                        files = list.ToArray();
                        zip.AlternateEncodingUsage = ZipOption.AsNecessary;
                        zip.AddDirectoryByName("ExpenseAttachment");
                        zip.AddFiles(files, "ExpenseAttachment");
                        Response.Clear();
                        Response.BufferOutput = false;
                        //string zipName = String.Format("Zip_{0}.zip", DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"));
                        string zipName = empName + "_" + Group + ".zip";
                        Response.ContentType = "application/zip";
                        Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
                        zip.Save(Response.OutputStream);
                        Response.End();
                    }
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Group do not have any attachment');", true);
                    GetPendingList();
                    return;
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            //finally {
            //    GetPendingList();
            //}

        }
        //public void GetAtt(string ExpenseGroupID)
        //{
        //    string sql = "";
        //    String[] files = null;
        //      List<ListItem> filesPath = new List<ListItem>();
        //    try
        //    {
        //        sql = "Select ea.AttachmentUrl From ExpenseAttachmentDetails ea inner join ExpenseDetails ed on ed.ExpenseDetailID=ea.ExpenseDetailID Where ed.ExpenseGroupId=" + ExpenseGroupID + "";
        //        DataTable dt=DbConnectionDAL.GetDataTable(CommandType.Text,sql);
        //        //List<ListItem> files = new List<ListItem>();
        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            files = Directory.GetFiles(Server.MapPath("~/ExpenseAttachment/"));
        //        }
        //        foreach (string file in files)
        //        {
        //            filesPath.Add(new ListItem(Path.GetFileName(file), Path.GetFileName(file)));
        //      // filesPath=Path.GetFileName(file);
        //        }
        //        using (ZipFile zip = new ZipFile())
        //        {
        //            zip.AlternateEncodingUsage = ZipOption.AsNecessary;
        //            zip.AddDirectoryByName("ExpenseAttachment");

        //            //foreach (DataRow dr in dt.Rows)
        //            //{
        //            //    string vb = Server.MapPath(Convert.ToString(dr["AttachmentUrl"]));

        //            //    zip.AddFile(Server.MapPath(Convert.ToString(dr["AttachmentUrl"])), "ExpenseAttachment");
        //            //}
        //            foreach(var file in filesPath)
        //            {
        //                zip.AddFile(file.ToString(), "Files");
        //            }
        //            Response.Clear();
        //            Response.BufferOutput = false;
        //            string zipName = String.Format("Zip_{0}.zip", DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"));
        //            Response.ContentType = "application/zip";
        //            Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
        //            zip.Save(Response.OutputStream);
        //            Response.End();
        //        }
        //    }
        //    catch(Exception ex){
        //        ex.Message.ToString();

        //    }

        //}

        protected void LinkButton1_Click(object sender, EventArgs e)
        {

        }

    }
}