using BusinessLayer;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AstralFFMS
{
    public partial class AttendanceReport : System.Web.UI.Page
    {
        DateTime mDate1 = DateTime.Now, mDate2 = DateTime.Now;
        string roleType = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;

            if (!IsPostBack)
            {//Ankita - 13/may/2016- (For Optimization)
             // GetRoleType(Settings.Instance.RoleID);
                roleType = Settings.Instance.RoleType;
                //BindSalePersonDDl();
                //fill_TreeArea();
                BindTreeViewControl();
                BindDDLMonth();
                monthDDL.SelectedValue = System.DateTime.Now.Month.ToString();
                yearDDL.SelectedValue = System.DateTime.Now.Year.ToString();
                btnExport.Visible = false;
                noteLabel.Visible = false;
            }
        }
        private void BindDDLMonth()
        {
            try
            {
                for (int month = 1; month <= 12; month++)
                {
                    string monthName = DateTimeFormatInfo.CurrentInfo.GetMonthName(month);
                    monthDDL.Items.Add(new ListItem(monthName.Substring(0, 3), month.ToString().PadLeft(2, '0')));
                }
                for (int i = System.DateTime.Now.Year - 10; i <= (System.DateTime.Now.Year); i++)
                {
                    yearDDL.Items.Add(new ListItem(i.ToString()));
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        //private void BindSalePersonDDl()
        //{
        //    try
        //    {
        //        if (roleType == "Admin")
        //        {
        //            //Ankita - 13/may/2016- (For Optimization)
        //            //string strrole = "select mastrole.RoleName,MastSalesRep.*,mastrole.RoleType from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId where MastSalesRep.Active=1 and SMName<>'.' order by MastSalesRep.SMName";
        //            string strrole = "select mastrole.RoleName,MastSalesRep.SMName,MastSalesRep.SMId,mastrole.RoleType from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId where MastSalesRep.Active=1 and SMName<>'.' order by MastSalesRep.SMName";
        //            DataTable dtcheckrole = new DataTable();
        //            dtcheckrole = DbConnectionDAL.GetDataTable(CommandType.Text, strrole);
        //            DataView dv1 = new DataView(dtcheckrole);
        //            dv1.RowFilter = "SMName<>.";
        //            dv1.Sort = "SMName asc";

        //            ListBox1.DataSource = dv1.ToTable();
        //            ListBox1.DataTextField = "SMName";
        //            ListBox1.DataValueField = "SMId";
        //            ListBox1.DataBind();

        //            dtcheckrole.Dispose();
        //            dv1.Dispose();
        //        }
        //        else
        //        {
        //            DataTable dt = Settings.UnderUsers(Settings.Instance.SMID);
        //            DataView dv = new DataView(dt);
        //            dv.RowFilter = "SMName<>.";
        //            if (dv.ToTable().Rows.Count > 0)
        //            {
        //                ListBox1.DataSource = dv.ToTable();
        //                ListBox1.DataTextField = "SMName";
        //                ListBox1.DataValueField = "SMId";
        //                ListBox1.DataBind();
        //            }

        //            dt.Dispose();
        //            dv.Dispose();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.ToString();
        //    }
        //}

        private void BindTreeViewControl()
        {
            try
            {
                DataTable St = new DataTable();
                if (roleType == "Admin")
                {
                    //  St = Settings.UnderUsersforlevels(Settings.Instance.SMID, 1);
                    St = DbConnectionDAL.GetDataTable(CommandType.Text, "Select msr.smid,msr.Smname +' ('+ msr.Syncid + ' - ' + mr.RoleName + ')' as smname,msr.underid,msr.lvl from mastsalesrep msr  LEFT JOIN mastrole mr ON mr.RoleId=msr.RoleId where msr.active=1 and msr.underid<>0 order by msr.smname");
                }
                else
                {
                    string query = "select msr.smid,msr.Smname +' ('+ msr.Syncid + ' - ' + mr.RoleName + ')' as smname,msr.underid ,msr.lvl from mastsalesrep msr  LEFT JOIN mastrole mr ON mr.RoleId=msr.RoleId where msr.smid in (select smid from MastSalesRepGrp where MainGrp  in (" + Settings.Instance.SMID + ")) and  msr.active=1 and msr.lvl >= (select distinct level from MastSalesRepGrp  where MainGrp in (" + Settings.Instance.SMID + " )) order by msr.smname";
                    St = DbConnectionDAL.GetDataTable(CommandType.Text, query);
                }
                //    DataSet ds = GetDataSet("Select smid,smname,underid,lvl from mastsalesrep where active=1 and underid<>0 order by smname");


                DataRow[] Rows = St.Select("lvl=MIN(lvl)"); // Get all parents nodes
                for (int i = 0; i < Rows.Length; i++)
                {
                    TreeNode root = new TreeNode(Rows[i]["smname"].ToString(), Rows[i]["smid"].ToString());
                    root.SelectAction = TreeNodeSelectAction.Expand;
                    root.CollapseAll();
                    CreateNode(root, St);
                    trview.Nodes.Add(root);
                }

                St.Dispose();
                Rows = null;
            }
            catch (Exception Ex) { throw Ex; }
        }
        public void CreateNode(TreeNode node, DataTable Dt)
        {
            DataRow[] Rows = Dt.Select("underid =" + node.Value);
            if (Rows.Length == 0) { return; }
            for (int i = 0; i < Rows.Length; i++)
            {
                TreeNode Childnode = new TreeNode(Rows[i]["smname"].ToString(), Rows[i]["smid"].ToString());
                Childnode.SelectAction = TreeNodeSelectAction.Expand;
                node.ChildNodes.Add(Childnode);
                Childnode.CollapseAll();
                CreateNode(Childnode, Dt);
            }

            Rows = null;
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        public void BindGrid()
        {
            try
            {
                string smIDStr = "", smIDStr1 = "";
                //foreach (ListItem item in ListBox1.Items)
                //{
                //    if (item.Selected){smIDStr1 += item.Value + ",";}
                //}
                //smIDStr1 = smIDStr1.TrimStart(',').TrimEnd(',');
                foreach (TreeNode node in trview.CheckedNodes)
                {
                    smIDStr1 = node.Value;
                    smIDStr += node.Value + ",";
                }
                smIDStr1 = smIDStr.TrimStart(',').TrimEnd(',');
                if (smIDStr1 == "")
                {
                    DataTable dtSMId = Settings.UnderUsers(Settings.Instance.SMID);
                    DataView dv = new DataView(dtSMId);
                    dv.RowFilter = "RoleType='AreaIncharge' and SMName<>.";
                    dv.Sort = "SMName asc";
                    if (dv.ToTable().Rows.Count > 0)
                    {
                        foreach (DataRow dr in dv.ToTable().Rows)
                        {
                            smIDStr = smIDStr + "," + Convert.ToString(dr["SMId"]);
                        }
                        smIDStr1 = smIDStr.TrimStart(',').TrimEnd(',');
                    }
                }
                mDate1 = DateTime.Parse("01/" + monthDDL.SelectedValue + "/" + yearDDL.SelectedValue);
                mDate2 = DateTime.Parse("01/" + monthDDL.SelectedValue + "/" + yearDDL.SelectedValue).AddMonths(1).AddDays(-1);
                int year = Convert.ToInt32(yearDDL.SelectedValue);
                int Month = Convert.ToInt32(monthDDL.SelectedValue);
                int dyasInMonth = DateTime.DaysInMonth(year, Month);
                if (dyasInMonth == 30)
                { gvData.Columns[35].Visible = false; }
                else if (dyasInMonth == 29)
                {
                    gvData.Columns[35].Visible = false;
                    gvData.Columns[34].Visible = false;
                    gvData.Columns[33].Visible = true;
                }

                else if (dyasInMonth == 28)
                {
                    gvData.Columns[35].Visible = false;
                    gvData.Columns[34].Visible = false;
                    gvData.Columns[33].Visible = false;
                    //gvData.Columns[32].Visible = false;
                }
                else
                {
                    gvData.Columns[35].Visible = true;
                    gvData.Columns[34].Visible = true;
                    gvData.Columns[33].Visible = true;
                    gvData.Columns[32].Visible = true;
                }

                DataTable gvdt = new DataTable();
                gvdt.Columns.Add(new DataColumn("Name", typeof(String)));
                gvdt.Columns.Add(new DataColumn("SyncId", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Month", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Year", typeof(String)));
                gvdt.Columns.Add(new DataColumn("EmpName", typeof(String)));
                gvdt.Columns.Add(new DataColumn("SMId", typeof(String)));
                for (int ii = 1; ii <= dyasInMonth; ii++)
                {
                    gvdt.Columns.Add(new DataColumn("d" + Convert.ToString(ii).Trim(), typeof(String)));
                }
                gvdt.Columns.Add(new DataColumn("Enter", typeof(Int32)));
                gvdt.Columns.Add(new DataColumn("Approve", typeof(Int32)));
                DataRow pDataRow = gvdt.NewRow();
                string mSRepCode = "", transTemp = "", strholiday = String.Empty;

                strholiday = "select distinct smid,DAY(holiday_date) as day1,holiday_date,Reason,areaID,AreaType from View_Holiday where holiday_date between  '" + Settings.dateformat(mDate1.ToString()) + " 00:00' and '" + Settings.dateformat(mDate2.ToString()) + " 23:59'";
                DataTable dt_holiday = DbConnectionDAL.GetDataTable(CommandType.Text, strholiday);
                DataView dvdt_holiday = new DataView(dt_holiday);
                Session["holiday"] = dvdt_holiday;


                mSRepCode = @"select SMId,SMName,SyncId from MastSalesRep where SMId in (" + smIDStr1 + ") and Active=1 order by SMId";
                DataTable ssdt = DbConnectionDAL.GetDataTable(CommandType.Text, mSRepCode);

                string Leave_str = "", strleave = "", status_pr = string.Empty, ls_val = string.Empty, dsr_val = string.Empty, DsrDate = string.Empty, dsrQuery = string.Empty;
                int dayloop = 0;

                dsrQuery = "select vl.SMId,vdate,ISNULL(AppStatus,'') as Status from TransVisit vl where vdate between '" + Settings.dateformat(mDate1.ToString()) + "' and '" + Settings.dateformat(mDate2.ToString()) + "' order by vl.SMId,vdate";
                DataTable dsrData = DbConnectionDAL.GetDataTable(CommandType.Text, dsrQuery);
                DataView dvdsrData = new DataView(dsrData);

                string No_Days = "select NoOfDays,SMId,FromDate ,ToDate as NoDays,AppStatus,LeaveString from TransLeaveRequest lr where lr.SMId IN (" + smIDStr1 + ") and (lr.FromDate between '" + Settings.dateformat(mDate1.ToString()) + " 00:00' and '" + Settings.dateformat(mDate2.ToString()) + " 23:59' OR lr.ToDate between '" + Settings.dateformat(mDate1.ToString()) + " 00:00' and '" + Settings.dateformat(mDate2.ToString()) + " 23:59') and lr.appstatus in ('Approve','Pending') order by smid,FromDate";
                DataTable dt_NoOfdays = DbConnectionDAL.GetDataTable(CommandType.Text, No_Days);
                DataView dvdt_NoOfdays = new DataView(dt_NoOfdays);

                foreach (DataRow dr in ssdt.Rows)
                {
                    int en = 0, ap = 0;
                    string mRepName = dr["SMName"].ToString();
                    string mSync = dr["SyncId"].ToString();
                    DataRow mDataRow = gvdt.NewRow();
                    mDataRow["Name"] = mRepName.ToString();
                    mDataRow["SyncId"] = mSync.ToString();
                    mDataRow["EmpName"] = dr["SMName"].ToString();

                    mDataRow["SMId"] = dr["SMId"].ToString();
                    mDataRow["Enter"] = en.ToString();
                    mDataRow["Approve"] = ap.ToString();
                    mDataRow["Month"] = monthDDL.SelectedItem.Text.ToString();
                    mDataRow["Year"] = yearDDL.SelectedValue.ToString();

                    dvdsrData.RowFilter = "smid=" + dr["SMId"];
                    //dsrData.Select("smid=" + dr["SMId"]);
                    foreach (DataRow dsr1 in dvdsrData.ToTable().Rows)
                    {
                        DsrDate = Convert.ToDateTime(dsr1["vdate"].ToString()).Day.ToString().Trim();
                        if (dsr1["SMId"].ToString() == dr["SMId"].ToString())
                        {
                            if (dsr1["Status"].ToString() == "Approve")
                            {
                                mDataRow["d" + DsrDate] = "P";
                                //tanvi 2/jan/2021
                                ap = ap + 1;
                            }
                            if (dsr1["Status"].ToString() == "Reject") { mDataRow["d" + DsrDate] = "E/R"; }
                            if (dsr1["Status"].ToString() == "")
                            {
                                //anurag ---------- 28-04-2021
                                if (chkIsActive.Checked == true)
                                {
                                    mDataRow["d" + DsrDate] = "E";
                                    en = en + 1;
                                }
                                else
                                {
                                    mDataRow["d" + DsrDate] = "P";
                                    ap = ap + 1;
                                }
                                //mDataRow["d" + DsrDate] = "E";
                                //tanvi 2/jan/2021
                                //en = en + 1;
                            }
                        }
                    }

                    //tanvi 2/jan/2021
                    mDataRow["Enter"] = en.ToString();
                    mDataRow["Approve"] = ap.ToString();

                    //dvdt_holiday.RowFilter = "smid=" + dr["SMId"];
                    //foreach (DataRow dsr1 in dvdt_holiday.ToTable().Rows)
                    //{
                    //    DsrDate = Convert.ToDateTime(dsr1["holiday_date"].ToString()).Day.ToString().Trim();
                    //    mDataRow["d" + DsrDate] = "H";
                    //}

                    //for (int k = 1; k <= dyasInMonth; k++)
                    //{
                    //    if (DateTime.Parse(k + "/" + monthDDL.SelectedValue + "/" + yearDDL.SelectedValue).DayOfWeek.ToString().Trim() == "Sunday")
                    //    {
                    //        mDataRow["d" + k.ToString()] = "Off"; //Filling Sunday
                    //    }
                    //}

                    dvdt_NoOfdays.RowFilter = "smid=" + dr["SMId"];
                    if (dvdt_NoOfdays.ToTable().Rows.Count > 0)
                    {
                        for (int cc = 0; cc < dvdt_NoOfdays.ToTable().Rows.Count; cc++)
                        {
                            if (dvdt_NoOfdays.ToTable().Rows[cc]["SMId"].ToString() == dr["SMId"].ToString())
                            {
                                double daysNo = Convert.ToDouble(dvdt_NoOfdays.ToTable().Rows[cc]["NoOfDays"].ToString());
                                DateTime fromdate = new DateTime();
                                DateTime todate = new DateTime();
                                todate = Convert.ToDateTime(dvdt_NoOfdays.ToTable().Rows[cc]["NoDays"].ToString());
                                fromdate = Convert.ToDateTime(dvdt_NoOfdays.ToTable().Rows[cc]["FromDate"].ToString());

                                dayloop = Convert.ToInt32((todate - fromdate).TotalDays);
                                for (int c1 = 0; c1 <= dayloop; c1++)
                                {
                                    DateTime dateTime1 = new DateTime();
                                    dateTime1 = Convert.ToDateTime(dvdt_NoOfdays.ToTable().Rows[cc]["FromDate"].ToString());
                                    dateTime1 = dateTime1.AddDays(c1);

                                    if (dateTime1 > mDate2)
                                    {
                                    }
                                    else if (dateTime1 < mDate1)
                                    {
                                    }
                                    else
                                    {
                                        //Leave_str = "select cp.SMName,AppStatus,CONVERT (varchar,'" + Settings.dateformat(dateTime1.ToString()) + "' ,106) as [vdate],lr.LeaveString from TransLeaveRequest lr left join MastSalesRep cp on cp.SMId=lr.SMId where lr.FromDate Between '" + Settings.dateformat(dvdt_NoOfdays.ToTable().Rows[cc]["FromDate"].ToString()) + " 00:00' and '" + Settings.dateformat(dateTime1.ToString()) + " 23:59' and lr.SMId in (" + dvdt_NoOfdays.ToTable().Rows[cc]["SMId"].ToString() + ") and lr.appstatus in ('Approve','Pending')";
                                        DateTime dateTime2 = new DateTime();
                                        DateTime dateTime3 = new DateTime();
                                        dateTime2 = Convert.ToDateTime(dvdt_NoOfdays.ToTable().Rows[cc]["FromDate"].ToString());
                                        dateTime3 = Convert.ToDateTime(dateTime1.ToString());
                                        double NrOfDays = ((dateTime3 - dateTime2).TotalDays) * 2;
                                        //  DataTable dtleave = DbConnectionDAL.GetDataTable(CommandType.Text, Leave_str);
                                        //string strleave = dtleave.Rows[0]["LeaveString"].ToString();
                                        //status_pr = dtleave.Rows[0]["AppStatus"].ToString();
                                        strleave = dvdt_NoOfdays.ToTable().Rows[cc]["LeaveString"].ToString();
                                        status_pr = dvdt_NoOfdays.ToTable().Rows[cc]["AppStatus"].ToString();
                                        string str = strleave.Substring((Convert.ToInt32(NrOfDays)), 2);

                                        dvdsrData.RowFilter = "smid=" + dr["SMId"];
                                        foreach (DataRow dsr in dvdsrData.ToTable().Rows)
                                        {
                                            if (dsr["SMId"].ToString() == dr["SMId"].ToString())
                                            {
                                                if (dateTime3.Day.ToString().Trim() == Convert.ToDateTime(dsr["vdate"].ToString()).Day.ToString().Trim())
                                                {
                                                    if (dsr["Status"].ToString() == "Approve")
                                                    { ls_val += "P"; }
                                                    if (dsr["Status"].ToString() == "Reject")
                                                    { ls_val += "E/R"; }
                                                    if (dsr["Status"].ToString() == "")
                                                    {
                                                        //Anurag-----28-04-2021
                                                        if (chkIsActive.Checked == true)
                                                        { ls_val += "E"; }
                                                        else { ls_val += "P"; }

                                                    }
                                                }
                                            }
                                        }
                                        dvdsrData.RowFilter = null;
                                        if (ls_val != "") { ls_val += ", "; }
                                        if (status_pr == "Pending")
                                        {
                                            if (str.Substring(0, 1) == " " && str.Substring(1, 1) == "L") { ls_val += "SHL"; }
                                            if (str.Substring(0, 1) == "L" && str.Substring(1, 1) == " ")
                                            {
                                                transTemp = ls_val.Replace(", ", "");
                                                if (transTemp != "")
                                                { ls_val = "FHL" + ", " + transTemp; }
                                                else
                                                { ls_val += "FHL"; }
                                            }
                                            if (str.Substring(0, 1) == "L" && str.Substring(1, 1) == "L") { ls_val += "L"; }
                                        }
                                        if (status_pr == "Approve")
                                        {
                                            if (str.Substring(0, 1) == " " && str.Substring(1, 1) == "L") { ls_val += "SHL/A"; }
                                            if (str.Substring(0, 1) == "L" && str.Substring(1, 1) == " ")
                                            {
                                                transTemp = ls_val.Replace(", ", "");
                                                if (transTemp != "")
                                                { ls_val = "FHL/A" + ", " + transTemp; }
                                                else
                                                { ls_val += "FHL/A"; }
                                            }
                                            if (str.Substring(0, 1) == "L" && str.Substring(1, 1) == "L") { ls_val += "L/A"; }
                                        }
                                        if (status_pr == "Reject")
                                        {
                                            if (str.Substring(0, 1) == " " && str.Substring(1, 1) == "L") { ls_val += "SHL/R"; }
                                            if (str.Substring(0, 1) == "L" && str.Substring(1, 1) == " ")
                                            {
                                                transTemp = ls_val.Replace(", ", "");
                                                if (transTemp != "")
                                                { ls_val = "FHL/R" + ", " + transTemp; }
                                                else
                                                { ls_val += "FHL/R"; }
                                            }
                                            if (str.Substring(0, 1) == "L" && str.Substring(1, 1) == "L") { ls_val += "L/R"; }
                                        }
                                        mDataRow["d" + dateTime3.Day.ToString().Trim()] = ls_val;
                                        ls_val = "";
                                    }
                                }

                            }
                        }

                    }
                    dvdt_holiday.RowFilter = null;
                    dvdsrData.RowFilter = null;
                    dvdt_NoOfdays.RowFilter = null;
                    gvdt.Rows.Add(mDataRow);
                    gvdt.AcceptChanges();
                }

                if (gvdt.Rows.Count > 0)
                {
                    gvData.DataSource = null;
                    gvData.DataSource = gvdt;
                    gvData.DataBind();
                    noteDiv.Style.Add("display", "block");
                    noteLabel.Visible = true;
                    btnExport.Visible = true;
                }
                else
                {
                    gvData.DataSource = gvdt;
                    gvData.DataBind();
                    btnExport.Visible = false;
                    noteDiv.Style.Add("display", "none");
                    noteLabel.Visible = false;
                }

                gvdt.Dispose();
                pDataRow = null;
                dt_holiday.Dispose();
                dvdt_holiday.Dispose();
                ssdt.Dispose();
                dsrData.Dispose();
                dvdsrData.Dispose();
                dt_NoOfdays.Dispose();
                dvdt_NoOfdays.Dispose();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/AttendanceReport.aspx");
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            string headertxt = string.Empty;
            StringBuilder sb = new StringBuilder();
            int r = 40; int r1 = 40; int r2 = 40; int r3 = 40;
            int Month = System.DateTime.DaysInMonth(Convert.ToInt32(yearDDL.SelectedValue), Convert.ToInt32(monthDDL.SelectedValue));
            if (gvData.Rows.Count != 0)
            {
                if (Month == 30)
                { r = 35; }
                else if (Month == 29)
                {
                    r = 35; r1 = 34;
                }
                else if (Month == 28)
                {
                    //r = 32;
                    r1 = 33;
                    r2 = 34;
                    r3 = 35;
                }
                //Forloop for header
                for (int i = 0; i < gvData.HeaderRow.Cells.Count; i++)
                {
                    //          dt.Columns.Add(gvData.HeaderRow.Cells[i].Text);
                    if (!(i == 4 || i == 38 || i == r || i == r1 || i == r2 || i == r3))
                    {
                        headertxt += gvData.HeaderRow.Cells[i].Text.TrimStart('"').TrimEnd('"') + ",";
                    }
                }
                sb.Append(headertxt);
                sb.Append(System.Environment.NewLine);
                //foreach for datarow
                foreach (GridViewRow row in gvData.Rows)
                {
                    for (int j = 0; j < row.Cells.Count; j++)
                    {
                        if (!(j == 4 || j == 38 || j == r || j == r1 || j == r2 || j == r3))
                        {
                            if (row.Cells[j].Text.ToString().Contains(","))
                            {
                                sb.Append(String.Format("\"{0}\"", row.Cells[j].Text.ToString()) + ',');
                            }
                            else if (row.Cells[j].Text.ToString().Contains(System.Environment.NewLine))
                            {
                                sb.Append(String.Format("\"{0}\"", row.Cells[j].Text.ToString()) + ',');
                            }
                            else
                            {
                                sb.Append(row.Cells[j].Text + ",");
                            }
                            //     sb.Append(row.Cells[j].Text + ",");
                        }
                    }
                    sb.Append(Environment.NewLine);
                }
            }
            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("content-disposition", "attachment;filename=AttendanceReport.csv");
            Response.Write(sb.ToString());
            Response.End();

        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
        }

        protected void gvData_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            try
            {
                string mDay = "", str = String.Empty, filledVal = String.Empty; int ptChange = 0; int checkChange = 0; int changeFlag = 0; Boolean chkNext = false;
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    mDate1 = DateTime.Parse("01/" + monthDDL.SelectedValue + "/" + yearDDL.SelectedValue);
                    mDate2 = DateTime.Parse("01/" + monthDDL.SelectedValue + "/" + yearDDL.SelectedValue).AddMonths(1).AddDays(-1);
                    Label smLabel = (e.Row.FindControl("lblSMId") as Label);
                    int mSRepCode = Convert.ToInt32(smLabel.Text);
                    //str = "select distinct DAY(holiday_date) as day1,holiday_date,description as Reason,Area_id as areaID,ma.AreaType from MastHoliday hm left join MastArea ma on ma.AreaId=hm.Area_id where holiday_date between  '" + Settings.dateformat(mDate1.ToString()) + " 00:00' and '" + Settings.dateformat(mDate2.ToString()) + " 23:59' AND hm.Active=1 and hm.Area_id in(SELECT distinct MainGrp FROM   mastareagrp WHERE  areaid IN (SELECT linkcode FROM   mastlink WHERE primtable = 'SALESPERSON' AND linktable = 'AREA' AND primcode IN (" + mSRepCode + ")))";

                    //DataTable dt_holiday = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                    DataView dv;
                    dv = (DataView)Session["holiday"];
                    for (int ii = 1; ii <= mDate2.Day; ii++)
                    {
                        mDay = Convert.ToString(ii).Trim();
                        if (mDay.Length == 1)
                            mDay = "0" + mDay;

                        if (DateTime.Parse(mDay + "/" + monthDDL.SelectedValue + "/" + yearDDL.SelectedValue).DayOfWeek.ToString().Trim() == "Sunday")
                        {

                            if (e.Row.Cells[ii + 4].Text == string.Empty || e.Row.Cells[ii + 4].Text == "&nbsp;")
                            {
                                e.Row.Cells[ii + 4].Text = "Off";
                            }

                            e.Row.Cells[ii + 4].BackColor = Color.FromName("#EED690");
                        }


                        dv.RowFilter = "smid=" + mSRepCode;
                        if (dv.ToTable().Rows.Count > 0)
                        {
                            for (int l = 0; l < dv.ToTable().Rows.Count; l++)
                            {
                                if (ii == Convert.ToInt32(dv.ToTable().Rows[l]["day1"].ToString()))
                                {
                                    if (e.Row.Cells[ii + 4].Text == string.Empty || e.Row.Cells[ii + 4].Text == "&nbsp;")
                                    {
                                        e.Row.Cells[ii + 4].Text = "H";
                                    }

                                    e.Row.Cells[ii + 4].ForeColor = Color.FromName("#fff");
                                    e.Row.Cells[ii + 4].BackColor = Color.Red;
                                }
                            }
                        }

                        filledVal = e.Row.Cells[ii + 4].Text;
                        if (filledVal == "L/A" || filledVal == "L/R" || filledVal == "L")
                        {
                            if (ptChange != 0)
                            {
                                checkChange = ptChange;
                            }
                            ptChange = ii + 4;
                            if (checkChange != 0 && ptChange != 0)
                            {
                                for (int x = checkChange; x <= ptChange; x++)
                                {
                                    if (e.Row.Cells[x].Text == "Off" || e.Row.Cells[x].Text == "H" || e.Row.Cells[x].Text == filledVal)
                                    {
                                        changeFlag = 1;
                                    }
                                    else
                                    {
                                        changeFlag = 0;
                                        break;
                                    }
                                }
                                if (changeFlag == 1)
                                {
                                    for (int x = checkChange + 1; x < ptChange; x++)
                                    {
                                        e.Row.Cells[x].Text = filledVal;
                                    }

                                }

                            }
                        }

                        if ((e.Row.Cells[ii + 4].Text == "Off" || e.Row.Cells[ii + 4].Text == "H") && ii == mDate2.Day)
                        {

                            for (int x = ptChange; x < (ii + 4); x++)
                            {
                                if (e.Row.Cells[x].Text == "Off" || e.Row.Cells[x].Text == "H" || e.Row.Cells[x].Text == "L" || e.Row.Cells[x].Text == "L/A" || e.Row.Cells[x].Text == "L/R")
                                {
                                    chkNext = true;
                                }
                                else
                                {
                                    chkNext = false;
                                    break;
                                }
                            }
                            if (chkNext == true)
                            {
                                int nxtMonth = DateTime.Parse("01/" + monthDDL.SelectedValue + "/" + yearDDL.SelectedValue).AddMonths(1).Month;
                                int nxtyear = DateTime.Parse("01/" + monthDDL.SelectedValue + "/" + yearDDL.SelectedValue).AddMonths(1).Year;
                                string q1 = "select LeaveString,DATEPART(d,FromDate) as nxtDay from TransLeaveRequest where SMId = '" + mSRepCode + "' and DATEPART(yyyy,FromDate) = '" + nxtyear + "' and DATEPART(m,FromDate) = '" + nxtMonth + "'";
                                DataTable nxtEntry = new DataTable();
                                nxtEntry = DbConnectionDAL.GetDataTable(CommandType.Text, q1);
                                foreach (DataRow ne in nxtEntry.Rows)
                                {
                                    if (ne["nxtDay"].ToString() == "1")
                                    {
                                        string nxtlstr = ne["LeaveString"].ToString();
                                        if (nxtlstr.Substring(0, 2) == "LL" || nxtlstr.Substring(0, 2) == "L ")
                                        {
                                            e.Row.Cells[ii + 4].Text = e.Row.Cells[ptChange].Text;
                                        }
                                    }
                                }
                            }
                        }


                    }
                    Boolean stApp = true; int Eval = 0; int Etval = 0; int p = 0;
                    for (int i2 = 0; i2 <= mDate2.Day + 4; i2++)
                    {
                        string[] namesArray = e.Row.Cells[i2 + 4].Text.Split(',');
                        for (int i3 = 0; i3 < namesArray.Length; i3++)
                        {
                            if (namesArray[i3].IndexOf('A') == -1 && namesArray[i3].IndexOf('P') == -1)
                            {
                                stApp = false;
                            }
                        }
                        if (stApp == true)
                        {
                            Eval++;
                        }
                        if (i2 <= mDate2.Day)
                        {
                            if (!(e.Row.Cells[i2 + 4].Text == string.Empty || e.Row.Cells[i2 + 4].Text == "&nbsp;") && !(e.Row.Cells[i2 + 4].Text == "H" || e.Row.Cells[i2 + 4].Text == "Off"))
                            {
                                Etval++;
                            }
                        }
                        if (e.Row.Cells[i2 + 4].Text == string.Empty || e.Row.Cells[i2 + 4].Text == "&nbsp;")
                        {
                            e.Row.Cells[i2 + 4].Text = "A";
                        }
                        stApp = true; p = i2;
                    }
                    //e.Row.Cells[p + 4].Text = Eval.ToString();
                    //e.Row.Cells[p + 3].Text = Etval.ToString();
                    string h = e.Row.Cells[34].Text;

                }
            }
            catch (Exception ex)
            {

                ex.ToString();
            }

            //if (e.Row.RowType == DataControlRowType.Header)
            //{
            //    if (chkIsActive.Checked == true)
            //    {
            //        e.Row.Cells[36].Text = "Entered";
            //        e.Row.Cells[37].Text = "Approved";
            //    }
            //    else
            //    {
            //        e.Row.Cells[36].Visible = false;
            //        e.Row.Cells[37].Text = "Total Present";
            //    }
            //}

            //if (e.)
            //{
            //    if (chkIsActive.Checked == true)
            //    {
            //        e.Row.Cells[36].Text = "Entered";
            //        e.Row.Cells[37].Text = "Approved";
            //    }
            //    else
            //    {
            //        e.Row.Cells[36].Visible = false;
            //        e.Row.Cells[37].Text = "Total Present";
            //    }
            //}
        }
    }
}