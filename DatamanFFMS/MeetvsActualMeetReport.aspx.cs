using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLayer;
using System.IO;
using System.Globalization;

namespace AstralFFMS
{
    public partial class MeetvsActualMeetReport : System.Web.UI.Page
    {
        BAL.MeetTarget.MeetTargetBAL MT = new BAL.MeetTarget.MeetTargetBAL();
        string roleType = "";
        string IDStr = "", IDStr1 = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;

            if (!IsPostBack)
            {//Ankita - 18/may/2016- (For Optimization)
                //GetRoleType(Settings.Instance.RoleID);
                roleType = Settings.Instance.RoleType;
                BindDDLMonth();
                //fillUnderUsers();
               // fill_TreeArea();
                fillfyear();
                BindTreeViewControl();
                //string pageName = Path.GetFileName(Request.Path);
                if (btnexport.Text == "Export")
                {
                    btnexport.Enabled = Settings.Instance.CheckExportPermission(pageName, Convert.ToString(Session["user_name"]));
                    btnexport.CssClass = "btn btn-primary";
                }
            }
        }
        private void fillfyear()
        {//Ankita - 18/may/2016- (For Optimization)
            // string str = "select * from Financialyear ";

            string str = "select id,Yr from Financialyear ";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (dt.Rows.Count > 0)
            {
                txtcurrentyear.DataSource = dt;
                txtcurrentyear.DataTextField = "Yr";
                txtcurrentyear.DataValueField = "id";
                txtcurrentyear.DataBind();
            }
            txtcurrentyear.Items.Insert(0, new ListItem("-- Select --", "0"));

           
        }
        private void BindDDLMonth()
        {
            try
            {

                //for (int month = 1; month <= 12; month++)
                //{
                //    string monthName = DateTimeFormatInfo.CurrentInfo.GetMonthName(month);
                //    listboxmonth.Items.Add(new ListItem(monthName.Substring(0, 3), (month.ToString().PadLeft(2, '0')).TrimStart('0')));

                //}

                //listboxmonth.Items.Add(new ListItem("Select", "0", true));
                for (int month = 4; month <= 12; month++)
                {
                    string monthName = DateTimeFormatInfo.CurrentInfo.GetMonthName(month);
                    listboxmonth.Items.Add(new ListItem(monthName.Substring(0, 3), (month.ToString().PadLeft(2, '0')).TrimStart('0')));
                    // listboxmonth.Items.Add(new ListItem(monthName.Substring(0, 3),monthName.Substring(0, 3)));                    
                }
                for (int month = 1; month <= 3; month++)
                {
                    string monthName = DateTimeFormatInfo.CurrentInfo.GetMonthName(month);
                    listboxmonth.Items.Add(new ListItem(monthName.Substring(0, 3), (month.ToString().PadLeft(2, '0')).TrimStart('0')));
                    // listboxmonth.Items.Add(new ListItem(monthName.Substring(0, 3),monthName.Substring(0, 3)));                    
                }


            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
        private string checkRole()
        {
            //string IDStr = "", IDStr1 = "";
            //foreach (TreeNode node in trview.CheckedNodes)
            //{
            //    IDStr1 = node.Value;
            //    {
            //        IDStr += node.Value + ",";
            //    }
            //}
            //IDStr1 = IDStr.TrimStart(',').TrimEnd(',');
            //string st = @"select RoleType from MastSalesRep M left join MastRole R on M.RoleId=R.RoleId where SMId=" + Settings.DMInt32(Settings.DMInt32(ddlunderUser.SelectedValue).ToString());
            string st = @"select RoleType from MastSalesRep M left join MastRole R on M.RoleId=R.RoleId where SMId=" + Settings.Instance.SMID + "";            
            string RName = Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, st));
            return RName;
        }
        protected void btnexport_Click(object sender, EventArgs e)
        {
            if (GridView1.Rows.Count > 0)
            {
                Response.ClearContent();
                Response.Buffer = true;
                //Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "Meet Vs Actual Meet Report.xls"));
                //Response.ContentType = "application/ms-excel";

                //Added 07-12-2015
                Response.AddHeader("content-disposition", "attachment;filename=Meet Vs Actual Meet Report.xls");
                Response.Charset = "";
                Response.ContentType = "application/vnd.ms-excel";
                //End

                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                //grd.AllowPaging = false;
                //BindGridview();
                //Change the Header Row back to white color
                GridView1.HeaderRow.Style.Add("background-color", "#FFFFFF");
                //Applying stlye to gridview header cells
                for (int i = 0; i < GridView1.HeaderRow.Cells.Count; i++)
                {
                    GridView1.HeaderRow.Cells[i].Style.Add("background-color", "#df5015");
                }

                //for (int i = 0; i < GridView1.Rows.Count; i++)
                //{
                //    GridViewRow row = GridView1.Rows[i];
                //    // row.Visible = false;
                //    row.FindControl("chkRow").Visible = false;

                //}

                GridView1.RenderControl(htw);
                Response.Write(sw.ToString());
                Response.End();
            }
        }
        public override void VerifyRenderingInServerForm(Control control)
        {

        }

        private void fillUserType()
        {
            try
            {
                // string str1 = "select Name,Id from MastMeetType where Id=" + e.CommandArgument.ToString();
                 
                string s = "select * from MastMeetType";
                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, s);
                if (dt.Rows.Count > 0)
                {

                    GridView1.DataSource = dt;
                    GridView1.DataBind();

                    // btnsave.Visible = false;

                    GridView1.HeaderRow.Cells[2].Text = "Region Head";
                    GridView1.HeaderRow.Cells[3].Text = "State Head";
                    GridView1.HeaderRow.Cells[4].Text = "District Head";
                    GridView1.HeaderRow.Cells[5].Text = "City Head";


                    GridView1.Columns[2].Visible = true;
                    GridView1.Columns[3].Visible = true;
                    GridView1.Columns[4].Visible = true;
                    GridView1.Columns[5].Visible = true;
                    

                    if (checkRole() == "RegionHead")                   
                    {
                    }
                    if (checkRole() == "StateHead")
                    {
                        GridView1.Columns[2].Visible = false;
                        //GridView1.HeaderRow.Cells[3].Text = "Assign to Team";
                    }

                    if (checkRole() == "DistrictHead")
                    {
                        GridView1.Columns[2].Visible = false;
                        GridView1.Columns[3].Visible = false;
                        //GridView1.HeaderRow.Cells[4].Text = "Assign to Team";
                    }
                    if (checkRole() == "CityHead")
                    {
                        GridView1.Columns[2].Visible = false;
                        GridView1.Columns[3].Visible = false;
                        GridView1.Columns[4].Visible = false;
                        //GridView1.HeaderRow.Cells[5].Text = "Assign to Team";
                    }
                    if (checkRole() == "AreaIncharge")
                    {
                        GridView1.Columns[2].Visible = false;
                        GridView1.Columns[3].Visible = false;
                        GridView1.Columns[4].Visible = false;
                        GridView1.Columns[5].Visible = false;
                    }

                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

        }

        private void fillUserTypebydateselect()
        {
            try
            {
                // string str1 = "select Name,Id from MastMeetType where Id=" + e.CommandArgument.ToString();

                string s = "select * from MastMeetType";
                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, s);
                if (dt.Rows.Count > 0)
                {

                    GridView1.DataSource = dt;
                    GridView1.DataBind();

                    // btnsave.Visible = false;

                    GridView1.HeaderRow.Cells[2].Text = "Region Head";
                    GridView1.HeaderRow.Cells[3].Text = "State Head";
                    GridView1.HeaderRow.Cells[4].Text = "District Head";
                    GridView1.HeaderRow.Cells[5].Text = "City Head";


                    GridView1.Columns[2].Visible = true;
                    GridView1.Columns[3].Visible = true;
                    GridView1.Columns[4].Visible = true;
                    GridView1.Columns[5].Visible = true;

                    if (Settings.Instance.RoleType == "RegionHead")
                    {
                    }
                    if (Settings.Instance.RoleType == "StateHead")
                    {
                        GridView1.Columns[2].Visible = false;
                        //GridView1.HeaderRow.Cells[3].Text = "Assign to Team";
                    }

                    if (Settings.Instance.RoleType == "DistrictHead")
                    {
                        GridView1.Columns[2].Visible = false;
                        GridView1.Columns[3].Visible = false;
                        //GridView1.HeaderRow.Cells[4].Text = "Assign to Team";
                    }
                    if (Settings.Instance.RoleType == "CityHead")
                    {
                        GridView1.Columns[2].Visible = false;
                        GridView1.Columns[3].Visible = false;
                        GridView1.Columns[4].Visible = false;
                        //GridView1.HeaderRow.Cells[5].Text = "Assign to Team";
                    }
                    if (Settings.Instance.RoleType == "AreaIncharge")
                    {
                        GridView1.Columns[2].Visible = false;
                        GridView1.Columns[3].Visible = false;
                        GridView1.Columns[4].Visible = false;
                        GridView1.Columns[5].Visible = false;
                    }

                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

        }
        //protected void txtcurrentyear_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (txtcurrentyear.SelectedIndex > 0)
        //    {
        //        fillUserTypebydateselect();
        //    }
        //    else
        //    {
        //        GridView1.DataSource = null;
        //        GridView1.DataBind();
        //    }
        //}

        private int GetSalesPerId(int uid)
        {
            string st = "select SMId from MastSalesRep where UserId=" + uid;
            int SID = Settings.DMInt32(Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, st)));
            return SID;


        }
        private DataTable GetSUnderPerSMId(int uid)
        {
            int s = GetSalesPerId(uid);
            string st = "select SMId from MastSalesRep where UserId=" + s;
            DataTable UperDT = DbConnectionDAL.GetDataTable(CommandType.Text, st);
            return UperDT;
        }

        private DataTable GetSUnderPerSMId1(int uid)
        {
            int s = GetSalesPerId(uid);
            string st = "select SMId from MastSalesRep where UserId=" + s;
            DataTable UperDT = DbConnectionDAL.GetDataTable(CommandType.Text, st);
            return UperDT;
            string p = "";
            for (int i = 0; i < UperDT.Rows.Count; i++)
            {
                p += UperDT.Rows[i]["SMId"].ToString() + ",";
            }
            p = p.Remove(p.Length - 1);
            string strqw = "select * from MastSalesRep where UnderId in (" + p + ")";
            DataTable ds = new DataTable();
            ds = DbConnectionDAL.GetDataTable(CommandType.Text, strqw);
            return ds;
        }
        private DataTable GetSUnderPerSMId2(int uid)
        {
            int s = GetSalesPerId(uid);
            string st = "select SMId from MastSalesRep where UserId=" + s;
            DataTable UperDT = DbConnectionDAL.GetDataTable(CommandType.Text, st);
            return UperDT;
            string p = "";
            string q = "";
            for (int i = 0; i < UperDT.Rows.Count; i++)
            {
                p += UperDT.Rows[i]["SMId"].ToString() + ",";
            }
            p = p.Remove(p.Length - 1);

            string strqw = "select SMId from MastSalesRep where UnderId in (" + p + ")";
            DataTable ds = new DataTable();
            ds = DbConnectionDAL.GetDataTable(CommandType.Text, strqw);

            for (int j = 0; j < ds.Rows.Count; j++)
            {
                q += ds.Rows[j]["SMId"].ToString() + ",";
            }
            q = q.Remove(q.Length - 1);

            string strqw1 = "select SMId from MastSalesRep where UnderId in (" + q + ")";
            DataTable ds1 = new DataTable();
            ds1 = DbConnectionDAL.GetDataTable(CommandType.Text, strqw1);
            return ds1;
        }

        private DataTable GetSUnderPerSMId6(int uid)
        {
            int s = GetSalesPerId(uid);
            string st = "select SMId from MastSalesRep where UserId=" + s;
            DataTable UperDT = DbConnectionDAL.GetDataTable(CommandType.Text, st);
            return UperDT;
            string p = "";
            string q = "";
            string r = "";
            for (int i = 0; i < UperDT.Rows.Count; i++)
            {
                p += UperDT.Rows[i]["SMId"].ToString() + ",";
            }
            p = p.Remove(p.Length - 1);

            string strqw = "select SMId from MastSalesRep where UnderId in (" + p + ")";
            DataTable ds = new DataTable();
            ds = DbConnectionDAL.GetDataTable(CommandType.Text, strqw);

            for (int j = 0; j < ds.Rows.Count; j++)
            {
                q += ds.Rows[j]["SMId"].ToString() + ",";
            }
            q = q.Remove(q.Length - 1);

            string strqw1 = "select SMId from MastSalesRep where UnderId in (" + q + ")";
            DataTable ds1 = new DataTable();
            ds1 = DbConnectionDAL.GetDataTable(CommandType.Text, strqw1);

            for (int k = 0; k < ds1.Rows.Count; k++)
            {
                r += ds1.Rows[k]["SMId"].ToString() + ",";
            }
            r = r.Remove(r.Length - 1);

            string strqw2 = "select SMId from MastSalesRep where UnderId in (" + r + ")";
            DataTable ds2 = new DataTable();
            ds2 = DbConnectionDAL.GetDataTable(CommandType.Text, strqw2);

            return ds2;



        }
        private DataTable GetSUnderPerSMId7(int uid)
        {
            int s1 = GetSalesPerId(uid);
            string st = "select SMId from MastSalesRep where UserId=" + s1;
            DataTable UperDT = DbConnectionDAL.GetDataTable(CommandType.Text, st);
            return UperDT;
            string p = "";
            string q = "";
            string r = "";
            string s = "";
            for (int i = 0; i < UperDT.Rows.Count; i++)
            {
                p += UperDT.Rows[i]["SMId"].ToString() + ",";
            }
            p = p.Remove(p.Length - 1);

            string strqw = "select SMId from MastSalesRep where UnderId in (" + p + ")";
            DataTable ds = new DataTable();
            ds = DbConnectionDAL.GetDataTable(CommandType.Text, strqw);

            for (int j = 0; j < ds.Rows.Count; j++)
            {
                q += ds.Rows[j]["SMId"].ToString() + ",";
            }
            q = q.Remove(q.Length - 1);

            string strqw1 = "select SMId from MastSalesRep where UnderId in (" + q + ")";
            DataTable ds1 = new DataTable();
            ds1 = DbConnectionDAL.GetDataTable(CommandType.Text, strqw1);

            for (int k = 0; k < ds1.Rows.Count; k++)
            {
                r += ds1.Rows[k]["SMId"].ToString() + ",";
            }
            r = r.Remove(r.Length - 1);

            string strqw2 = "select SMId from MastSalesRep where UnderId in (" + r + ")";
            DataTable ds2 = new DataTable();
            ds2 = DbConnectionDAL.GetDataTable(CommandType.Text, strqw2);


            for (int l = 0; l < ds1.Rows.Count; l++)
            {
                s += ds2.Rows[l]["SMId"].ToString() + ",";
            }
            s = s.Remove(r.Length - 1);

            string strqw3 = "select SMId from MastSalesRep where UnderId in (" + s + ")";
            DataTable ds3 = new DataTable();
            ds3 = DbConnectionDAL.GetDataTable(CommandType.Text, strqw3);

            return ds3;
        }

        private int GetSalesPerUserId(int uid)
        {
            string st = "select UserId from MastSalesRep where SMId=" + uid;
            int SID = Settings.DMInt32(Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, st)));
            return SID;
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lk = (Label)e.Row.Cells[0].FindControl("lblTargetFromHo1");
                Label lnkRegion = (Label)e.Row.FindControl("lnkRegion");

                Label lnkState = (Label)e.Row.Cells[0].FindControl("lnkState");
                Label lnkDistrict = (Label)e.Row.Cells[0].FindControl("lnkDistrict");
                Label lnkCity = (Label)e.Row.Cells[0].FindControl("lnkCity");
                Label lnkArea = (Label)e.Row.Cells[0].FindControl("lnkArea");
                //LinkButton lk7 = (LinkButton)e.Row.Cells[0].FindControl("lnkAssignedByLevel7");

                Label lnkRegion1 = (Label)e.Row.FindControl("lnkRegion1");
                Label lnkState1 = (Label)e.Row.Cells[0].FindControl("lnkState1");
                Label lnkDistrict1 = (Label)e.Row.Cells[0].FindControl("lnkDistrict1");
                Label lnkCity1 = (Label)e.Row.Cells[0].FindControl("lnkCity1");
                Label lnkArea1 = (Label)e.Row.Cells[0].FindControl("lnkArea1");
                HiddenField hid = (HiddenField)e.Row.Cells[0].FindControl("hidPartyTypeId");

                string MonthStr = "", MonthStr1 = "", MonthStrvalue = "", MonthStrvalue1 = "";
                decimal sumvalueRegion = 0; string s1 = "";
                foreach (ListItem item in listboxmonth.Items)
                {
                    if (item.Selected)
                    {
                        MonthStr1 += item.Text + ",";

                    }
                }
                MonthStr = MonthStr1.TrimStart(',').TrimEnd(',');


                foreach (ListItem item in listboxmonth.Items)
                {
                    if (item.Selected)
                    {
                        MonthStrvalue1 += item.Value + ",";

                    }
                }
                MonthStrvalue = MonthStrvalue1.TrimStart(',').TrimEnd(',');

                string IDStr = "", IDStr1 = "";                
                foreach (TreeNode node in trview.CheckedNodes)
                {
                    IDStr1 = node.Value;
                    {
                        IDStr += node.Value + ",";
                    }
                }
                IDStr1 = IDStr.TrimStart(',').TrimEnd(',');
              // Added - Abhishek - 30-05-2016


                if (IDStr1 == "")
                {
                    foreach (TreeNode node in trview.Nodes)
                    {
                        IDStr1 = node.Value;
                        {
                            IDStr += node.Value + ",";
                        }
                    }
                    IDStr1 = IDStr.TrimStart(',').TrimEnd(',');
                }
                //End
                if (lk != null)
                {
                    //string s = @"select * from MeetTragetFromHO Where  PartyTypeId=" + hid.Value + " and SMID=" +Settings.DMInt32(ddlunderUser.SelectedValue) + " and MeetYear='" + txtcurrentyear.SelectedItem.Text + "'";
                    string s = @"select * from MeetTragetFromHO Where  PartyTypeId=" + hid.Value + " and SMID in (" + IDStr1 + " and MeetYear='" + txtcurrentyear.SelectedItem.Text + "'";
                    DataTable dtr = new DataTable();
                    dtr = DbConnectionDAL.GetDataTable(CommandType.Text, s);
                    if (dtr.Rows.Count > 0)
                    {
                        try
                        {
                            object sum1 = dtr.Compute("Sum(JanValue)", "");
                            object sum2 = dtr.Compute("Sum(FebValue)", "");
                            object sum3 = dtr.Compute("Sum(MarValue)", "");
                            object sum4 = dtr.Compute("Sum(AprValue)", "");
                            object sum5 = dtr.Compute("Sum(MayValue)", "");
                            object sum6 = dtr.Compute("Sum(JunValue)", "");
                            object sum7 = dtr.Compute("Sum(JulValue)", "");
                            object sum8 = dtr.Compute("Sum(AugValue)", "");
                            object sum9 = dtr.Compute("Sum(SepValue)", "");
                            object sum10 = dtr.Compute("Sum(OctValue)", "");
                            object sum11 = dtr.Compute("Sum(NovValue)", "");
                            object sum12 = dtr.Compute("Sum(DecValue)", "");
                            if (MonthStr != "")
                            {
                                if (MonthStr.Contains("Jan"))
                                { sumvalueRegion = Convert.ToDecimal(sum1); }
                                if (MonthStr.Contains("Feb"))
                                { sumvalueRegion = sumvalueRegion + Convert.ToDecimal(sum2); }
                                if (MonthStr.Contains("Mar"))
                                { sumvalueRegion = sumvalueRegion + Convert.ToDecimal(sum3); }
                                if (MonthStr.Contains("Apr"))
                                { sumvalueRegion = sumvalueRegion + Convert.ToDecimal(sum4); }
                                if (MonthStr.Contains("May"))
                                { sumvalueRegion = sumvalueRegion + Convert.ToDecimal(sum5); }
                                if (MonthStr.Contains("Jun"))
                                { sumvalueRegion = sumvalueRegion + Convert.ToDecimal(sum6); }
                                if (MonthStr.Contains("Jul"))
                                { sumvalueRegion = sumvalueRegion + Convert.ToDecimal(sum7); }
                                if (MonthStr.Contains("Aug"))
                                { sumvalueRegion = sumvalueRegion + Convert.ToDecimal(sum8); }
                                if (MonthStr.Contains("Sep"))
                                { sumvalueRegion = sumvalueRegion + Convert.ToDecimal(sum9); }
                                if (MonthStr.Contains("Oct"))
                                { sumvalueRegion = sumvalueRegion + Convert.ToDecimal(sum10); }
                                if (MonthStr.Contains("Nov"))
                                { sumvalueRegion = sumvalueRegion + Convert.ToDecimal(sum11); }
                                if (MonthStr.Contains("Dec"))
                                { sumvalueRegion = sumvalueRegion + Convert.ToDecimal(sum12); }
                                decimal df = Convert.ToDecimal(sumvalueRegion);
                                lnkRegion.Text = df.ToString();
                            }
                            else
                            {
                                decimal df = Convert.ToDecimal(sum1) + Convert.ToDecimal(sum2) + Convert.ToDecimal(sum3) + Convert.ToDecimal(sum4) + Convert.ToDecimal(sum5) + Convert.ToDecimal(sum6) + Convert.ToDecimal(sum7) + Convert.ToDecimal(sum8) + Convert.ToDecimal(sum9) + Convert.ToDecimal(sum10) + Convert.ToDecimal(sum11) + Convert.ToDecimal(sum12);
                                lnkRegion.Text = df.ToString();
                            }
                          
                            //lk.Text = df.ToString();
                        }
                        catch
                        {
                            lk.Text = "0";
                        }

                    }
                    else
                    {
                        //        lk.Text = "Target";
                        lk.Text = "0";
                    }

                }
                if (lnkRegion != null)
                {

                    object sum1 = 0; object sum2 = 0;
                    object sum3 = 0;
                    object sum4 = 0;
                    object sum5 = 0;
                    object sum6 = 0;
                    object sum7 = 0; object sum8 = 0; object sum9 = 0; object sum10 = 0; object sum11 = 0; object sum12 = 0;

                    string s = @"select * from MeetTragetFromHO Where  PartyTypeId=" + hid.Value + " and AssignedByID=" + Settings.Instance.UserID + " and MeetYear='" + txtcurrentyear.SelectedItem.Text + "'";
                    DataTable dtr2 = new DataTable();
                    dtr2 = DbConnectionDAL.GetDataTable(CommandType.Text, s);
                    if (dtr2.Rows.Count > 0)
                    {
                        try
                        {

                            try
                            {
                                sum1 = dtr2.Compute("Sum(JanValue)", "");
                            }
                            catch
                            {

                            }
                            try
                            {
                                sum2 = dtr2.Compute("Sum(FebValue)", "");
                            }
                            catch { }

                            try
                            {
                                sum3 = dtr2.Compute("Sum(MarValue)", "");
                            }
                            catch { }
                            try
                            {
                                sum4 = dtr2.Compute("Sum(AprValue)", "");
                            }
                            catch { }
                            try
                            {
                                sum5 = dtr2.Compute("Sum(MayValue)", "");
                            }
                            catch { }
                            try
                            {
                                sum6 = dtr2.Compute("Sum(JunValue)", "");
                            }
                            catch { }
                            try
                            {
                                sum7 = dtr2.Compute("Sum(JulValue)", "");
                            }
                            catch { }
                            try
                            {
                                sum8 = dtr2.Compute("Sum(AugValue)", "");
                            }
                            catch { }
                            try
                            {
                                sum9 = dtr2.Compute("Sum(SepValue)", "");
                            }
                            catch { }
                            try
                            {
                                sum10 = dtr2.Compute("Sum(OctValue)", "");
                            }
                            catch { }
                            try
                            {
                                sum11 = dtr2.Compute("Sum(NovValue)", "");
                            }
                            catch { }
                            try
                            {
                                sum12 = dtr2.Compute("Sum(DecValue)", "");
                            }
                            catch { }
                            if (MonthStr != "")
                            {
                                if (MonthStr.Contains("Jan"))
                                { sumvalueRegion = Convert.ToDecimal(sum1); }
                                if (MonthStr.Contains("Feb"))
                                { sumvalueRegion = sumvalueRegion + Convert.ToDecimal(sum2); }
                                if (MonthStr.Contains("Mar"))
                                { sumvalueRegion = sumvalueRegion + Convert.ToDecimal(sum3); }
                                if (MonthStr.Contains("Apr"))
                                { sumvalueRegion = sumvalueRegion + Convert.ToDecimal(sum4); }
                                if (MonthStr.Contains("May"))
                                { sumvalueRegion = sumvalueRegion + Convert.ToDecimal(sum5); }
                                if (MonthStr.Contains("Jun"))
                                { sumvalueRegion = sumvalueRegion + Convert.ToDecimal(sum6); }
                                if (MonthStr.Contains("Jul"))
                                { sumvalueRegion = sumvalueRegion + Convert.ToDecimal(sum7); }
                                if (MonthStr.Contains("Aug"))
                                { sumvalueRegion = sumvalueRegion + Convert.ToDecimal(sum8); }
                                if (MonthStr.Contains("Sep"))
                                { sumvalueRegion = sumvalueRegion + Convert.ToDecimal(sum9); }
                                if (MonthStr.Contains("Oct"))
                                { sumvalueRegion = sumvalueRegion + Convert.ToDecimal(sum10); }
                                if (MonthStr.Contains("Nov"))
                                { sumvalueRegion = sumvalueRegion + Convert.ToDecimal(sum11); }
                                if (MonthStr.Contains("Dec"))
                                { sumvalueRegion = sumvalueRegion + Convert.ToDecimal(sum12); }
                                decimal df = Convert.ToDecimal(sumvalueRegion);
                                lnkRegion.Text = df.ToString();
                            }
                            else
                            {
                                decimal df = Convert.ToDecimal(sum1) + Convert.ToDecimal(sum2) + Convert.ToDecimal(sum3) + Convert.ToDecimal(sum4) + Convert.ToDecimal(sum5) + Convert.ToDecimal(sum6) + Convert.ToDecimal(sum7) + Convert.ToDecimal(sum8) + Convert.ToDecimal(sum9) + Convert.ToDecimal(sum10) + Convert.ToDecimal(sum11) + Convert.ToDecimal(sum12);
                                lnkRegion.Text = df.ToString();
                            }

                            //decimal df = Convert.ToDecimal(sum1) + Convert.ToDecimal(sum2) + Convert.ToDecimal(sum3) + Convert.ToDecimal(sum4) + Convert.ToDecimal(sum5) + Convert.ToDecimal(sum6) + Convert.ToDecimal(sum7) + Convert.ToDecimal(sum8) + Convert.ToDecimal(sum9) + Convert.ToDecimal(sum10) + Convert.ToDecimal(sum11) + Convert.ToDecimal(sum12);
                            //lnkRegion.Text = df.ToString();
                        }
                        catch
                        {
                            lnkRegion.Text = "0";
                        }

                    }
                    else
                    {
                        //        lnkRegion.Text = "Target";
                        lnkRegion.Text = "0";
                    }
                    //DataTable d1 = Settings.UnderUsers(Settings.Instance.SMID);
                    DataTable d1 = Settings.FindunderUsers(IDStr1);
                    DataView dv1 = new DataView(d1);
                    // dv1.RowFilter = "RoleType='RegionHead'";
                    DataTable gauravDT = dv1.ToTable();
                    string smIDStr1 = "";
                    for (int i = 0; i < gauravDT.Rows.Count; i++)
                    {
                        smIDStr1 += gauravDT.Rows[i]["SMId"].ToString() + ",";

                    }
                    smIDStr1 = smIDStr1.TrimStart(',').TrimEnd(',');


                    //Ankita - 18/may/2016- (For Optimization)
                    //string s1 = @"select * from TransMeetPlanEntry where  AppStatus='Approved' and MeetDate>='" + Settings.GetStartFinancialYearfrmDate(txtcurrentyear.SelectedItem.Text) + "' and MeetDate<='" + Settings.GetStartFinancialYearToDate(txtcurrentyear.SelectedItem.Text) + "' and SMId in (" + smIDStr1 + ") and MeetTypeId=" + hid.Value + "";
                    if (smIDStr1 != "")
                    {     
                       
                        if (MonthStr != "") s1 = @"select count(*) from TransMeetPlanEntry where  AppStatus='Approved' and MeetDate>='" + Settings.GetStartFinancialYearfrmDate(txtcurrentyear.SelectedItem.Text) + "' and MeetDate<='" + Settings.GetStartFinancialYearToDate(txtcurrentyear.SelectedItem.Text) + "' and SMId in (" + smIDStr1 + ") and MeetTypeId=" + hid.Value + " and month(MeetDate) in (" + MonthStrvalue + ")";
                        else  s1 = @"select count(*) from TransMeetPlanEntry where  AppStatus='Approved' and MeetDate>='" + Settings.GetStartFinancialYearfrmDate(txtcurrentyear.SelectedItem.Text) + "' and MeetDate<='" + Settings.GetStartFinancialYearToDate(txtcurrentyear.SelectedItem.Text) + "' and SMId in (" + smIDStr1 + ") and MeetTypeId=" + hid.Value + "";
                        //DataTable dtr21 = new DataTable();
                        //dtr21 = DbConnectionDAL.GetDataTable(CommandType.Text, s1);
                        int val = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, s1));
                        try
                        {
                            //lnkRegion1.Text = Convert.ToString(dtr21.Rows.Count);
                            lnkRegion1.Text = Convert.ToString(val);
                        }

                        catch
                        {

                        }
                    }

                }
                if (lnkState != null )
                {
                    DataTable d2 = Settings.FindunderUsers(IDStr1);
                    DataView dv2 = new DataView(d2);
                    dv2.RowFilter = "RoleType='StateHead'";
                    DataTable gauravDT1 = dv2.ToTable();
                    object sum011 = 0; object sum21 = 0;
                    object sum31 = 0;
                    object sum41 = 0;
                    object sum51 = 0;
                    object sum61 = 0;
                    decimal sumvalueState = 0;   
                    object sum71 = 0; object sum81 = 0; object sum91 = 0; object sum101 = 0; object sum111 = 0; object sum121 = 0;
                    string smIDStr = "";
                    string smIDStr1 = "";
                    
                    for (int i = 0; i < gauravDT1.Rows.Count; i++)
                    {
                        smIDStr1 += gauravDT1.Rows[i]["SMId"].ToString() + ",";
                        
                    }
                    smIDStr1 = smIDStr1.TrimStart(',').TrimEnd(',');
                    //if(smIDStr1 != "")
                    //{ QryChk = "and SmId in (" + smIDStr1 + ")"; }
                    //if(txtcurrentyear.SelectedIndex > 0)
                    //{
                    //    QryChk += " and MeetYear='" + txtcurrentyear.SelectedItem.Text + "'";
                    //}

                    //smIDStr1 = smIDStr1.TrimStart(',').TrimEnd(',');

                    string s = @"select * from MeetTragetFromHO Where  PartyTypeId=" + hid.Value + " and SmId in (" + smIDStr1 + ") and MeetYear='" + txtcurrentyear.SelectedItem.Text + "'";
                   
                    DataTable dtr2 = new DataTable();
                    try
                    {
                        dtr2 = DbConnectionDAL.GetDataTable(CommandType.Text, s);
                    }
                    catch { }
                    if (dtr2.Rows.Count > 0)
                    {
                        try
                        {
                            try
                            {
                                sum011 = dtr2.Compute("Sum(JanValue)", "");
                            }
                            catch
                            {

                            }
                            try
                            {
                                sum21 = dtr2.Compute("Sum(FebValue)", "");
                            }
                            catch { }

                            try
                            {
                                sum31 = dtr2.Compute("Sum(MarValue)", "");
                            }
                            catch { }
                            try
                            {
                                sum41 = dtr2.Compute("Sum(AprValue)", "");
                            }
                            catch { }
                            try
                            {
                                sum51 = dtr2.Compute("Sum(MayValue)", "");
                            }
                            catch { }
                            try
                            {
                                sum61 = dtr2.Compute("Sum(JunValue)", "");
                            }
                            catch { }
                            try
                            {
                                sum71 = dtr2.Compute("Sum(JulValue)", "");
                            }
                            catch { }
                            try
                            {
                                sum81 = dtr2.Compute("Sum(AugValue)", "");
                            }
                            catch { }
                            try
                            {
                                sum91 = dtr2.Compute("Sum(SepValue)", "");
                            }
                            catch { }
                            try
                            {
                                sum101 = dtr2.Compute("Sum(OctValue)", "");
                            }
                            catch { }
                            try
                            {
                                sum111 = dtr2.Compute("Sum(NovValue)", "");
                            }
                            catch { }
                            try
                            {
                                sum121 = dtr2.Compute("Sum(DecValue)", "");
                            }
                            catch { }
                            if (MonthStr != "")
                            {
                                if (MonthStr.Contains("Jan"))
                                { sumvalueState = Convert.ToDecimal(sum011); }
                                if (MonthStr.Contains("Feb"))
                                { sumvalueState = sumvalueState + Convert.ToDecimal(sum21); }
                                if (MonthStr.Contains("Mar"))
                                { sumvalueState = sumvalueState + Convert.ToDecimal(sum31); }
                                if (MonthStr.Contains("Apr"))
                                { sumvalueState = sumvalueState + Convert.ToDecimal(sum41); }
                                if (MonthStr.Contains("May"))
                                { sumvalueState = sumvalueState + Convert.ToDecimal(sum51); }
                                if (MonthStr.Contains("Jun"))
                                { sumvalueState = sumvalueState + Convert.ToDecimal(sum61); }
                                if (MonthStr.Contains("Jul"))
                                { sumvalueState = sumvalueState + Convert.ToDecimal(sum71); }
                                if (MonthStr.Contains("Aug"))
                                { sumvalueState = sumvalueState + Convert.ToDecimal(sum81); }
                                if (MonthStr.Contains("Sep"))
                                { sumvalueState = sumvalueState + Convert.ToDecimal(sum91); }
                                if (MonthStr.Contains("Oct"))
                                { sumvalueState = sumvalueState + Convert.ToDecimal(sum101); }
                                if (MonthStr.Contains("Nov"))
                                { sumvalueState = sumvalueState + Convert.ToDecimal(sum111); }
                                if (MonthStr.Contains("Dec"))
                                { sumvalueState = sumvalueState + Convert.ToDecimal(sum121); }
                                decimal df = Convert.ToDecimal(sumvalueState);
                                lnkState.Text = df.ToString();
                            }
                            else
                            {
                                decimal df = Convert.ToDecimal(sum011) + Convert.ToDecimal(sum21) + Convert.ToDecimal(sum31) + Convert.ToDecimal(sum41) + Convert.ToDecimal(sum51) + Convert.ToDecimal(sum61) + Convert.ToDecimal(sum71) + Convert.ToDecimal(sum81) + Convert.ToDecimal(sum91) + Convert.ToDecimal(sum101) + Convert.ToDecimal(sum111) + Convert.ToDecimal(sum121);
                                lnkState.Text = df.ToString();
                            }
                            //decimal df = Convert.ToDecimal(sum011) + Convert.ToDecimal(sum21) + Convert.ToDecimal(sum31) + Convert.ToDecimal(sum41) + Convert.ToDecimal(sum51) + Convert.ToDecimal(sum61) + Convert.ToDecimal(sum71) + Convert.ToDecimal(sum81) + Convert.ToDecimal(sum91) + Convert.ToDecimal(sum101) + Convert.ToDecimal(sum111) + Convert.ToDecimal(sum121);
                            //lnkState.Text = df.ToString();
                        }
                        catch
                        {
                            lnkState.Text = "0";
                        }

                    }
                    else
                    {
                        //     lnkState.Text = "Target";
                        lnkState.Text = "0";
                    }

                    //DataTable d1 = Settings.UnderUsers(Settings.DMInt32(ddlunderUser.SelectedValue).ToString());
                    DataTable d1 = Settings.FindunderUsers(IDStr1);
                    DataView dv1 = new DataView(d1);
                    // dv1.RowFilter = "RoleType='StateHead'";
                    dv1.RowFilter = "RoleType='StateHead' or RoleType='DistrictHead' or RoleType='CityHead' or RoleType = 'AreaIncharge'";
                    DataTable gauravDT = dv1.ToTable();
                    string smIDStr2 = "";
                    for (int i = 0; i < gauravDT.Rows.Count; i++)
                    {
                        smIDStr2 += gauravDT.Rows[i]["SMId"].ToString() + ",";

                    }
                    smIDStr2 = smIDStr2.TrimStart(',').TrimEnd(',');
                    //Ankita - 18/may/2016- (For Optimization)
                    //string s1 = @"select * from TransMeetPlanEntry where  AppStatus='Approved' and MeetDate>='" + Settings.GetStartFinancialYearfrmDate(txtcurrentyear.SelectedItem.Text) + "' and MeetDate<='" + Settings.GetStartFinancialYearToDate(txtcurrentyear.SelectedItem.Text) + "' and SMId in (" + smIDStr2 + ") and MeetTypeId=" + hid.Value + "";
                    if (smIDStr1 != "")
                    {

                        if (MonthStr != "") s1 = @"select count(*) from TransMeetPlanEntry where  AppStatus='Approved' and MeetDate>='" + Settings.GetStartFinancialYearfrmDate(txtcurrentyear.SelectedItem.Text) + "' and MeetDate<='" + Settings.GetStartFinancialYearToDate(txtcurrentyear.SelectedItem.Text) + "' and SMId in (" + smIDStr2 + ") and MeetTypeId=" + hid.Value + " and month(MeetDate) in (" + MonthStrvalue + ")";
                        else s1 = @"select count(*) from TransMeetPlanEntry where  AppStatus='Approved' and MeetDate>='" + Settings.GetStartFinancialYearfrmDate(txtcurrentyear.SelectedItem.Text) + "' and MeetDate<='" + Settings.GetStartFinancialYearToDate(txtcurrentyear.SelectedItem.Text) + "' and SMId in (" + smIDStr2 + ") and MeetTypeId=" + hid.Value + "";
                        // DataTable dtr21 = new DataTable();
                        //dtr21 = DbConnectionDAL.GetDataTable(CommandType.Text, s1);
                        int val = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, s1));
                        try
                        {
                            lnkState1.Text = Convert.ToString(val);
                            //lnkState1.Text = Convert.ToString(dtr21.Rows.Count);
                        }

                        catch
                        {

                        }
                    }

                }


                //  District////

                if (lnkDistrict != null)
                {
                    //DataTable d2 = Settings.UnderUsers(Settings.DMInt32(ddlunderUser.SelectedValue).ToString());
                    DataTable d2 = Settings.FindunderUsers(IDStr1);
                    DataView dv2 = new DataView(d2);
                    dv2.RowFilter = "RoleType='DistrictHead'";
                    DataTable gauravDT1 = dv2.ToTable();
                    object sum011 = 0; object sum21 = 0;
                    object sum31 = 0;
                    object sum41 = 0;
                    object sum51 = 0;
                    object sum61 = 0;
                    decimal sumvalueDistrict = 0;  
                    object sum71 = 0; object sum81 = 0; object sum91 = 0; object sum101 = 0; object sum111 = 0; object sum121 = 0;
                    string smIDStr = "";
                    string smIDStr1 = "";
                    
                    for (int i = 0; i < gauravDT1.Rows.Count; i++)
                    {
                        smIDStr1 += gauravDT1.Rows[i]["SMId"].ToString() + ",";

                    }
                    smIDStr1 = smIDStr1.TrimStart(',').TrimEnd(',');

                    string s = @"select * from MeetTragetFromHO Where  PartyTypeId=" + hid.Value + " and SMId in (" + smIDStr1 + ") and MeetYear='" + txtcurrentyear.SelectedItem.Text + "'";
                    DataTable dtr2 = new DataTable();
                    try
                    {
                        dtr2 = DbConnectionDAL.GetDataTable(CommandType.Text, s);
                    }
                    catch { }
                    if (dtr2.Rows.Count > 0)
                    {
                        try
                        {

                            try
                            {
                                sum011 = dtr2.Compute("Sum(JanValue)", "");
                            }
                            catch
                            {

                            }
                            try
                            {
                                sum21 = dtr2.Compute("Sum(FebValue)", "");
                            }
                            catch { }

                            try
                            {
                                sum31 = dtr2.Compute("Sum(MarValue)", "");
                            }
                            catch { }
                            try
                            {
                                sum41 = dtr2.Compute("Sum(AprValue)", "");
                            }
                            catch { }
                            try
                            {
                                sum51 = dtr2.Compute("Sum(MayValue)", "");
                            }
                            catch { }
                            try
                            {
                                sum61 = dtr2.Compute("Sum(JunValue)", "");
                            }
                            catch { }
                            try
                            {
                                sum71 = dtr2.Compute("Sum(JulValue)", "");
                            }
                            catch { }
                            try
                            {
                                sum81 = dtr2.Compute("Sum(AugValue)", "");
                            }
                            catch { }
                            try
                            {
                                sum91 = dtr2.Compute("Sum(SepValue)", "");
                            }
                            catch { }
                            try
                            {
                                sum101 = dtr2.Compute("Sum(OctValue)", "");
                            }
                            catch { }
                            try
                            {
                                sum111 = dtr2.Compute("Sum(NovValue)", "");
                            }
                            catch { }
                            try
                            {
                                sum121 = dtr2.Compute("Sum(DecValue)", "");
                            }
                            catch { }
                            if (MonthStr != "")
                            {
                                if (MonthStr.Contains("Jan"))
                                { sumvalueDistrict = Convert.ToDecimal(sum011); }
                                if (MonthStr.Contains("Feb"))
                                { sumvalueDistrict = sumvalueDistrict + Convert.ToDecimal(sum21); }
                                if (MonthStr.Contains("Mar"))
                                { sumvalueDistrict = sumvalueDistrict + Convert.ToDecimal(sum31); }
                                if (MonthStr.Contains("Apr"))
                                { sumvalueDistrict = sumvalueDistrict + Convert.ToDecimal(sum41); }
                                if (MonthStr.Contains("May"))
                                { sumvalueDistrict = sumvalueDistrict + Convert.ToDecimal(sum51); }
                                if (MonthStr.Contains("Jun"))
                                { sumvalueDistrict = sumvalueDistrict + Convert.ToDecimal(sum61); }
                                if (MonthStr.Contains("Jul"))
                                { sumvalueDistrict = sumvalueDistrict + Convert.ToDecimal(sum71); }
                                if (MonthStr.Contains("Aug"))
                                { sumvalueDistrict = sumvalueDistrict + Convert.ToDecimal(sum81); }
                                if (MonthStr.Contains("Sep"))
                                { sumvalueDistrict = sumvalueDistrict + Convert.ToDecimal(sum91); }
                                if (MonthStr.Contains("Oct"))
                                { sumvalueDistrict = sumvalueDistrict + Convert.ToDecimal(sum101); }
                                if (MonthStr.Contains("Nov"))
                                { sumvalueDistrict = sumvalueDistrict + Convert.ToDecimal(sum111); }
                                if (MonthStr.Contains("Dec"))
                                { sumvalueDistrict = sumvalueDistrict + Convert.ToDecimal(sum121); }
                                decimal df = Convert.ToDecimal(sumvalueDistrict);
                                lnkDistrict.Text = df.ToString();
                            }
                            else
                            {
                                decimal df = Convert.ToDecimal(sum011) + Convert.ToDecimal(sum21) + Convert.ToDecimal(sum31) + Convert.ToDecimal(sum41) + Convert.ToDecimal(sum51) + Convert.ToDecimal(sum61) + Convert.ToDecimal(sum71) + Convert.ToDecimal(sum81) + Convert.ToDecimal(sum91) + Convert.ToDecimal(sum101) + Convert.ToDecimal(sum111) + Convert.ToDecimal(sum121);
                                lnkDistrict.Text = df.ToString();
                            }
                            //decimal df = Convert.ToDecimal(sum011) + Convert.ToDecimal(sum21) + Convert.ToDecimal(sum31) + Convert.ToDecimal(sum41) + Convert.ToDecimal(sum51) + Convert.ToDecimal(sum61) + Convert.ToDecimal(sum71) + Convert.ToDecimal(sum81) + Convert.ToDecimal(sum91) + Convert.ToDecimal(sum101) + Convert.ToDecimal(sum111) + Convert.ToDecimal(sum121);
                            //lnkDistrict.Text = df.ToString();
                        }
                        catch
                        {
                            lnkDistrict.Text = "0";
                        }

                    }
                    else
                    {
                        //   lnkDistrict.Text = "Target";
                        lnkDistrict.Text = "0";
                    }

                    //DataTable d1 = Settings.UnderUsers(Settings.DMInt32(ddlunderUser.SelectedValue).ToString());
                    DataTable d1 = Settings.FindunderUsers(IDStr1);
                    DataView dv1 = new DataView(d1);
                  //dv1.RowFilter = "RoleType='DistrictHead'";
                    dv1.RowFilter = "RoleType='DistrictHead' or RoleType='CityHead' or RoleType = 'AreaIncharge'";
                    DataTable gauravDT = dv1.ToTable();
                    string smIDStr2 = "";
                    for (int i = 0; i < gauravDT.Rows.Count; i++)
                    {
                        smIDStr2 += gauravDT.Rows[i]["SMId"].ToString() + ",";

                    }
                    smIDStr2 = smIDStr2.TrimStart(',').TrimEnd(',');
                    if (smIDStr1 != "")
                    {
                        if (MonthStr != "") s1 = @"select * from TransMeetPlanEntry where  AppStatus='Approved' and MeetDate>='" + Settings.GetStartFinancialYearfrmDate(txtcurrentyear.SelectedItem.Text) + "' and MeetDate<='" + Settings.GetStartFinancialYearToDate(txtcurrentyear.SelectedItem.Text) + "' and SMId in (" + smIDStr2 + ") and MeetTypeId=" + hid.Value + " and month(MeetDate) in (" + MonthStrvalue + ")";
                        else s1 = @"select * from TransMeetPlanEntry where  AppStatus='Approved' and MeetDate>='" + Settings.GetStartFinancialYearfrmDate(txtcurrentyear.SelectedItem.Text) + "' and MeetDate<='" + Settings.GetStartFinancialYearToDate(txtcurrentyear.SelectedItem.Text) + "' and SMId in (" + smIDStr2 + ") and MeetTypeId=" + hid.Value + "";
                        DataTable dtr21 = new DataTable();
                        dtr21 = DbConnectionDAL.GetDataTable(CommandType.Text, s1);
                        try
                        {
                            lnkDistrict1.Text = Convert.ToString(dtr21.Rows.Count);
                        }

                        catch
                        {

                        }
                    }
                }
                //// City ///

                if (lnkCity != null)
                {
                    //DataTable d2 = Settings.UnderUsers(Settings.DMInt32(ddlunderUser.SelectedValue).ToString());
                    DataTable d2 = Settings.FindunderUsers(IDStr1);
                    DataView dv2 = new DataView(d2);
                    dv2.RowFilter = "RoleType='CityHead'";
                    DataTable gauravDT1 = dv2.ToTable();
                    object sum011 = 0; object sum21 = 0;
                    object sum31 = 0;
                    object sum41 = 0;
                    object sum51 = 0;
                    object sum61 = 0;
                    object sum71 = 0; object sum81 = 0; object sum91 = 0; object sum101 = 0; object sum111 = 0; object sum121 = 0;
                    //  string smIDStr = "";
                    decimal sumvalueCity = 0;   
                    string smIDStr1 = "";
                    for (int i = 0; i < gauravDT1.Rows.Count; i++)
                    {
                        smIDStr1 += gauravDT1.Rows[i]["SMId"].ToString() + ",";

                    }
                    smIDStr1 = smIDStr1.TrimStart(',').TrimEnd(',');

                    string s = @"select * from MeetTragetFromHO Where  PartyTypeId=" + hid.Value + " and SMId in (" + smIDStr1 + ") and MeetYear='" + txtcurrentyear.SelectedItem.Text + "'";
                    DataTable dtr2 = new DataTable();
                    try
                    {
                        dtr2 = DbConnectionDAL.GetDataTable(CommandType.Text, s);
                    }
                    catch { }
                    if (dtr2.Rows.Count > 0)
                    {
                        try
                        {

                            try
                            {
                                sum011 = dtr2.Compute("Sum(JanValue)", "");
                            }
                            catch
                            {

                            }
                            try
                            {
                                sum21 = dtr2.Compute("Sum(FebValue)", "");
                            }
                            catch { }

                            try
                            {
                                sum31 = dtr2.Compute("Sum(MarValue)", "");
                            }
                            catch { }
                            try
                            {
                                sum41 = dtr2.Compute("Sum(AprValue)", "");
                            }
                            catch { }
                            try
                            {
                                sum51 = dtr2.Compute("Sum(MayValue)", "");
                            }
                            catch { }
                            try
                            {
                                sum61 = dtr2.Compute("Sum(JunValue)", "");
                            }
                            catch { }
                            try
                            {
                                sum71 = dtr2.Compute("Sum(JulValue)", "");
                            }
                            catch { }
                            try
                            {
                                sum81 = dtr2.Compute("Sum(AugValue)", "");
                            }
                            catch { }
                            try
                            {
                                sum91 = dtr2.Compute("Sum(SepValue)", "");
                            }
                            catch { }
                            try
                            {
                                sum101 = dtr2.Compute("Sum(OctValue)", "");
                            }
                            catch { }
                            try
                            {
                                sum111 = dtr2.Compute("Sum(NovValue)", "");
                            }
                            catch { }
                            try
                            {
                                sum121 = dtr2.Compute("Sum(DecValue)", "");
                            }
                            catch { }
                            if (MonthStr != "")
                            {
                                if (MonthStr.Contains("Jan"))
                                { sumvalueCity = Convert.ToDecimal(sum011); }
                                if (MonthStr.Contains("Feb"))
                                { sumvalueCity = sumvalueCity + Convert.ToDecimal(sum21); }
                                if (MonthStr.Contains("Mar"))
                                { sumvalueCity = sumvalueCity + Convert.ToDecimal(sum31); }
                                if (MonthStr.Contains("Apr"))
                                { sumvalueCity = sumvalueCity + Convert.ToDecimal(sum41); }
                                if (MonthStr.Contains("May"))
                                { sumvalueCity = sumvalueCity + Convert.ToDecimal(sum51); }
                                if (MonthStr.Contains("Jun"))
                                { sumvalueCity = sumvalueCity + Convert.ToDecimal(sum61); }
                                if (MonthStr.Contains("Jul"))
                                { sumvalueCity = sumvalueCity + Convert.ToDecimal(sum71); }
                                if (MonthStr.Contains("Aug"))
                                { sumvalueCity = sumvalueCity + Convert.ToDecimal(sum81); }
                                if (MonthStr.Contains("Sep"))
                                { sumvalueCity = sumvalueCity + Convert.ToDecimal(sum91); }
                                if (MonthStr.Contains("Oct"))
                                { sumvalueCity = sumvalueCity + Convert.ToDecimal(sum101); }
                                if (MonthStr.Contains("Nov"))
                                { sumvalueCity = sumvalueCity + Convert.ToDecimal(sum111); }
                                if (MonthStr.Contains("Dec"))
                                { sumvalueCity = sumvalueCity + Convert.ToDecimal(sum121); }
                                decimal df = Convert.ToDecimal(sumvalueCity);
                                lnkCity.Text = df.ToString();
                            }
                            else
                            {
                                decimal df = Convert.ToDecimal(sum011) + Convert.ToDecimal(sum21) + Convert.ToDecimal(sum31) + Convert.ToDecimal(sum41) + Convert.ToDecimal(sum51) + Convert.ToDecimal(sum61) + Convert.ToDecimal(sum71) + Convert.ToDecimal(sum81) + Convert.ToDecimal(sum91) + Convert.ToDecimal(sum101) + Convert.ToDecimal(sum111) + Convert.ToDecimal(sum121);
                                lnkCity.Text = df.ToString();
                            }
                            //decimal df = Convert.ToDecimal(sum011) + Convert.ToDecimal(sum21) + Convert.ToDecimal(sum31) + Convert.ToDecimal(sum41) + Convert.ToDecimal(sum51) + Convert.ToDecimal(sum61) + Convert.ToDecimal(sum71) + Convert.ToDecimal(sum81) + Convert.ToDecimal(sum91) + Convert.ToDecimal(sum101) + Convert.ToDecimal(sum111) + Convert.ToDecimal(sum121);
                            //lnkCity.Text = df.ToString();
                        }
                        catch
                        {
                            lnkCity.Text = "0";
                        }

                    }
                    else
                    {
                        //     lnkCity.Text = "Target";
                        lnkCity.Text = "0";
                    }
                    //DataTable d1 = Settings.UnderUsers(Settings.DMInt32(ddlunderUser.SelectedValue).ToString());
                    DataTable d1 = Settings.FindunderUsers(IDStr1);
                    DataView dv1 = new DataView(d1);
                    DataView checkdv1 = new DataView(d1);
                    checkdv1.RowFilter = "RoleType='CityHead'";
                    DataTable priDT = checkdv1.ToTable();
                    if (priDT.Rows.Count > 0)
                    {
                        dv1.RowFilter = "RoleType='CityHead' or RoleType = 'AreaIncharge'";
                    }
                    else
                    {
                        dv1.RowFilter = "RoleType='CityHead'";
                    }
                    DataTable gauravDT = dv1.ToTable();
                    string smIDStr2 = "0";
                    for (int i = 0; i < gauravDT.Rows.Count; i++)
                    {
                        smIDStr2 += gauravDT.Rows[i]["SMId"].ToString() + ",";

                    }
                    smIDStr2 = smIDStr2.TrimStart(',').TrimEnd(',');
                    if (smIDStr1 != "")
                    {
                        if (MonthStr != "") s1 = @"select * from TransMeetPlanEntry where  AppStatus='Approved' and MeetDate>='" + Settings.GetStartFinancialYearfrmDate(txtcurrentyear.SelectedItem.Text) + "' and MeetDate<='" + Settings.GetStartFinancialYearToDate(txtcurrentyear.SelectedItem.Text) + "' and SMId in (" + smIDStr2 + ") and MeetTypeId=" + hid.Value + " and month(MeetDate) in (" + MonthStrvalue + ")";
                        else s1 = @"select * from TransMeetPlanEntry where  AppStatus='Approved' and MeetDate>='" + Settings.GetStartFinancialYearfrmDate(txtcurrentyear.SelectedItem.Text) + "' and MeetDate<='" + Settings.GetStartFinancialYearToDate(txtcurrentyear.SelectedItem.Text) + "' and SMId in (" + smIDStr2 + ") and MeetTypeId=" + hid.Value + "";
                        DataTable dtr21 = new DataTable();
                        dtr21 = DbConnectionDAL.GetDataTable(CommandType.Text, s1);
                        try
                        {
                            lnkCity1.Text = Convert.ToString(dtr21.Rows.Count);
                        }

                        catch
                        {

                        }
                    }
                }

                if (lnkArea != null)
                {
                    //DataTable d2 = Settings.UnderUsers(Settings.DMInt32(ddlunderUser.SelectedValue).ToString());
                    DataTable d2 = Settings.FindunderUsers(IDStr1);
                    DataView dv2 = new DataView(d2);
                    //dv2.RowFilter = "RoleType='AreaIncharge' or RoleType = 'DistrictHead'";
                    dv2.RowFilter = "RoleType='AreaIncharge'";

                    DataTable gauravDT1 = dv2.ToTable();
                    object sum011 = 0; object sum21 = 0;
                    object sum31 = 0;
                    object sum41 = 0;
                    object sum51 = 0;
                    object sum61 = 0;
                    object sum71 = 0; object sum81 = 0; object sum91 = 0; object sum101 = 0; object sum111 = 0; object sum121 = 0;
                    //  string smIDStr = "";
                    decimal sumvalueArea = 0;   
                    string smIDStr1 = "";
                    for (int i = 0; i < gauravDT1.Rows.Count; i++)
                    {
                        smIDStr1 += gauravDT1.Rows[i]["SMId"].ToString() + ",";

                    }
                    smIDStr1 = smIDStr1.TrimStart(',').TrimEnd(',');

                    string s = @"select * from MeetTragetFromHO Where  PartyTypeId=" + hid.Value + " and SMId in (" + smIDStr1 + ") and MeetYear='" + txtcurrentyear.SelectedItem.Text + "'";
                    DataTable dtr2 = new DataTable();
                    try
                    {
                        dtr2 = DbConnectionDAL.GetDataTable(CommandType.Text, s);
                    }
                    catch { }
                    if (dtr2.Rows.Count > 0)
                    {
                        try
                        {

                            try
                            {
                                sum011 = dtr2.Compute("Sum(JanValue)", "");
                            }
                            catch
                            {

                            }
                            try
                            {
                                sum21 = dtr2.Compute("Sum(FebValue)", "");
                            }
                            catch { }

                            try
                            {
                                sum31 = dtr2.Compute("Sum(MarValue)", "");
                            }
                            catch { }
                            try
                            {
                                sum41 = dtr2.Compute("Sum(AprValue)", "");
                            }
                            catch { }
                            try
                            {
                                sum51 = dtr2.Compute("Sum(MayValue)", "");
                            }
                            catch { }
                            try
                            {
                                sum61 = dtr2.Compute("Sum(JunValue)", "");
                            }
                            catch { }
                            try
                            {
                                sum71 = dtr2.Compute("Sum(JulValue)", "");
                            }
                            catch { }
                            try
                            {
                                sum81 = dtr2.Compute("Sum(AugValue)", "");
                            }
                            catch { }
                            try
                            {
                                sum91 = dtr2.Compute("Sum(SepValue)", "");
                            }
                            catch { }
                            try
                            {
                                sum101 = dtr2.Compute("Sum(OctValue)", "");
                            }
                            catch { }
                            try
                            {
                                sum111 = dtr2.Compute("Sum(NovValue)", "");
                            }
                            catch { }
                            try
                            {
                                sum121 = dtr2.Compute("Sum(DecValue)", "");
                            }
                            catch { }
                            if (MonthStr != "")
                            {
                                if (MonthStr.Contains("Jan"))
                                { sumvalueArea = Convert.ToDecimal(sum011); }
                                if (MonthStr.Contains("Feb"))
                                { sumvalueArea = sumvalueArea + Convert.ToDecimal(sum21); }
                                if (MonthStr.Contains("Mar"))
                                { sumvalueArea = sumvalueArea + Convert.ToDecimal(sum31); }
                                if (MonthStr.Contains("Apr"))
                                { sumvalueArea = sumvalueArea + Convert.ToDecimal(sum41); }
                                if (MonthStr.Contains("May"))
                                { sumvalueArea = sumvalueArea + Convert.ToDecimal(sum51); }
                                if (MonthStr.Contains("Jun"))
                                { sumvalueArea = sumvalueArea + Convert.ToDecimal(sum61); }
                                if (MonthStr.Contains("Jul"))
                                { sumvalueArea = sumvalueArea + Convert.ToDecimal(sum71); }
                                if (MonthStr.Contains("Aug"))
                                { sumvalueArea = sumvalueArea + Convert.ToDecimal(sum81); }
                                if (MonthStr.Contains("Sep"))
                                { sumvalueArea = sumvalueArea + Convert.ToDecimal(sum91); }
                                if (MonthStr.Contains("Oct"))
                                { sumvalueArea = sumvalueArea + Convert.ToDecimal(sum101); }
                                if (MonthStr.Contains("Nov"))
                                { sumvalueArea = sumvalueArea + Convert.ToDecimal(sum111); }
                                if (MonthStr.Contains("Dec"))
                                { sumvalueArea = sumvalueArea + Convert.ToDecimal(sum121); }
                                decimal df = Convert.ToDecimal(sumvalueArea);
                                lnkArea.Text = df.ToString();
                            }
                            else
                            {
                                decimal df = Convert.ToDecimal(sum011) + Convert.ToDecimal(sum21) + Convert.ToDecimal(sum31) + Convert.ToDecimal(sum41) + Convert.ToDecimal(sum51) + Convert.ToDecimal(sum61) + Convert.ToDecimal(sum71) + Convert.ToDecimal(sum81) + Convert.ToDecimal(sum91) + Convert.ToDecimal(sum101) + Convert.ToDecimal(sum111) + Convert.ToDecimal(sum121);
                                lnkArea.Text = df.ToString();
                            }
                            //decimal df = Convert.ToDecimal(sum011) + Convert.ToDecimal(sum21) + Convert.ToDecimal(sum31) + Convert.ToDecimal(sum41) + Convert.ToDecimal(sum51) + Convert.ToDecimal(sum61) + Convert.ToDecimal(sum71) + Convert.ToDecimal(sum81) + Convert.ToDecimal(sum91) + Convert.ToDecimal(sum101) + Convert.ToDecimal(sum111) + Convert.ToDecimal(sum121);
                            //lnkArea.Text = df.ToString();
                        }
                        catch
                        {
                            lnkArea.Text = "0";
                        }

                    }
                    else
                    {
                        //   lnkArea.Text = "Target";
                        lnkArea.Text = "0";
                    }


                    //DataTable d1 = Settings.UnderUsers(Settings.DMInt32(ddlunderUser.SelectedValue).ToString());
                    DataTable d1 = Settings.FindunderUsers(IDStr1);
                    DataView dv1 = new DataView(d1);
                    //   dv1.RowFilter = "RoleType='CityHead'";
                    dv1.RowFilter = "RoleType='AreaIncharge'";
                    DataTable gauravDT = dv1.ToTable();
                    string smIDStr2 = "";
                    for (int i = 0; i < gauravDT.Rows.Count; i++)
                    {
                        smIDStr2 += gauravDT.Rows[i]["SMId"].ToString() + ",";

                    }
                    smIDStr2 = smIDStr2.TrimStart(',').TrimEnd(',');
                    if (smIDStr1 != "")
                    {
                        if (MonthStr != "") s1 = @"select * from TransMeetPlanEntry where  AppStatus='Approved' and MeetDate>='" + Settings.GetStartFinancialYearfrmDate(txtcurrentyear.SelectedItem.Text) + "' and MeetDate<='" + Settings.GetStartFinancialYearToDate(txtcurrentyear.SelectedItem.Text) + "' and SMId in (" + smIDStr2 + ") and MeetTypeId=" + hid.Value + " and month(MeetDate) in (" + MonthStrvalue + ")";
                        else  s1 = @"select * from TransMeetPlanEntry where  AppStatus='Approved' and MeetDate>='" + Settings.GetStartFinancialYearfrmDate(txtcurrentyear.SelectedItem.Text) + "' and MeetDate<='" + Settings.GetStartFinancialYearToDate(txtcurrentyear.SelectedItem.Text) + "' and SMId in (" + smIDStr2 + ") and MeetTypeId=" + hid.Value + "";
                        DataTable dtr21 = new DataTable();
                        dtr21 = DbConnectionDAL.GetDataTable(CommandType.Text, s1);
                        try
                        {
                            lnkArea1.Text = Convert.ToString(dtr21.Rows.Count);
                        }

                        catch
                        {

                        }
                    }

                }

            }
        }

        private int LoginUserLevel()
        {
            int lvl = 0;
            string str = "select Lvl  from MastSalesRep where UserId=" + Settings.Instance.UserID;
            DataTable LVLDT = new DataTable();
            LVLDT = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (LVLDT.Rows.Count > 0)
            {
                lvl = Convert.ToInt32(LVLDT.Rows[0][0].ToString()) + 1;
            }
            return lvl;

        }
        private void fillUnderUsers()
        {
            if (roleType == "Admin")
            {   //Ankita - 18/may/2016- (For Optimization)
                //string strrole = "select mastrole.RoleName,MastSalesRep.*,mastrole.RoleType from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId where MastSalesRep.Active=1 and SMName<>'.' order by MastSalesRep.SMName";
                string strrole = "select mastrole.RoleName,MastSalesRep.SMId,MastSalesRep.SMName,mastrole.RoleType from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId where MastSalesRep.Active=1 and SMName<>'.' order by MastSalesRep.SMName";
                DataTable dtcheckrole = new DataTable();
                dtcheckrole = DbConnectionDAL.GetDataTable(CommandType.Text, strrole);
                DataView dv1 = new DataView(dtcheckrole);
                dv1.RowFilter = "SMName<>.";
                dv1.Sort = "SMName asc";

                ddlunderUser.DataSource = dv1.ToTable();
                ddlunderUser.DataTextField = "SMName";
                ddlunderUser.DataValueField = "SMId";
                ddlunderUser.DataBind();
            }
            else
            {
                DataTable d = Settings.UnderUsers(Settings.Instance.SMID);
                if (d.Rows.Count > 0)
                {
                    try
                    {
                        DataView dv = new DataView(d);
                        //  dv.RowFilter = "RoleType='AreaIncharge' OR RoleType='CityHead'";
                        ddlunderUser.DataSource = dv;
                        ddlunderUser.DataTextField = "SMName";
                        ddlunderUser.DataValueField = "SMId";
                        ddlunderUser.DataBind();
                        string Role = "Select RoleType from Mastrole mr left join Mastsalesrep ms on mr.RoleId=ms.RoleId where ms.Smid in (" + Settings.Instance.SMID + ") ";
                        DataTable dtRole = new DataTable();
                        dtRole = DbConnectionDAL.GetDataTable(CommandType.Text, Role);
                        string RoleTy = dtRole.Rows[0]["RoleType"].ToString();
                        if (RoleTy == "CityHead" || RoleTy == "DistrictHead")
                        {
                            ddlunderUser.SelectedValue = Settings.Instance.SMID;
                        }
                    }
                    catch { }

                }
            }
        }

        /////// Abhishek jaiswal 23may2017 start--------------------------------------------------------
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
        }
        /////// Abhishek jaiswal 23may2017 End--------------------------------------------------------

        void fill_TreeArea()
        {
            int lowestlvl = 0;
            DataTable St = new DataTable();
            if (roleType == "Admin")
            {

                //string strrole = "select SMID,SMName from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId where MastSalesRep.Active=1 and SMName<>'.' order by MastSalesRep.SMName";
                //St = DbConnectionDAL.GetDataTable(CommandType.Text, strrole);
                //    lowestlvl = Settings.UnderUsersforlowest(Settings.Instance.SMID);
                //St = Settings.UnderUsersforlowerlevels(Settings.Instance.SMID,1);
                St = Settings.UnderUsersforlevels(Settings.Instance.SMID, 1);
            }
            else
            {
                //Ankita - 18/may/2016- (For Optimization)
                //lowestlvl = Settings.UnderUsersforlowest(Settings.Instance.SMID);
                //St = Settings.UnderUsersforlowerlevels(Settings.Instance.SMID, lowestlvl);
                St = DbConnectionDAL.GetDataTable(CommandType.Text, "SELECT mastrole.rolename,mastsalesrep.smid,smname + ' (' + mastsalesrep.syncid + ' - '+ mastrole.rolename + ')' AS smname, mastsalesrep.lvl,mastrole.roletype FROM   mastsalesrep LEFT JOIN mastrole ON mastrole.roleid = mastsalesrep.roleid WHERE  smid =" + Settings.Instance.SMID + "");
            }


            trview.Nodes.Clear();

            if (St.Rows.Count <= 0)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('No Record Found !');", true);
                return;
            }
            foreach (DataRow row in St.Rows)
            {
                TreeNode tnParent = new TreeNode();
                tnParent.Text = row["SMName"].ToString();
                tnParent.Value = row["SMId"].ToString();
                trview.Nodes.Add(tnParent);
                //tnParent.ExpandAll();
                tnParent.CollapseAll();
                //Ankita - 18/may/2016- (For Optimization)
                // FillChildArea(tnParent, tnParent.Value, (Convert.ToInt32(row["Lvl"])), Convert.ToInt32(row["SMId"].ToString()));
                getchilddata(tnParent, tnParent.Value);
            }
        }
        //Ankita - 18/may/2016- (For Optimization)
        private void getchilddata(TreeNode parent, string ParentId)
        {

            string SmidVar = string.Empty;
            string GetFirstChildData = string.Empty;
            int levelcnt = 0;
            if (Settings.Instance.RoleType == "Admin")
                //levelcnt = Convert.ToInt16(Settings.Instance.SalesPersonLevel) + 2;
                levelcnt = Convert.ToInt16("0") + 2;
            else
                levelcnt = Convert.ToInt16(Settings.Instance.SalesPersonLevel) + 1;


            GetFirstChildData = "select msrg.smid,Smname +' ('+ msr.Syncid + ' - ' + mr.RoleName + ')' as smname from mastsalesrepgrp msrg left join mastsalesrep msr on msrg.smid=msr.smid  LEFT JOIN mastrole mr ON mr.RoleId=msr.RoleId where msrg.maingrp =" + ParentId + " and msr.lvl=" + (levelcnt) + " and msrg.smid <> " + ParentId + " and msr.Active=1 order by SMName,lvl desc ";
            DataTable FirstChildDataDt = DbConnectionDAL.GetDataTable(CommandType.Text, GetFirstChildData);

            if (FirstChildDataDt.Rows.Count > 0)
            {

                for (int i = 0; i < FirstChildDataDt.Rows.Count; i++)
                {
                    SmidVar += FirstChildDataDt.Rows[i]["smid"].ToString() + ",";
                    FillChildArea(parent, ParentId, FirstChildDataDt.Rows[i]["smid"].ToString(), FirstChildDataDt.Rows[i]["smname"].ToString());
                }
                SmidVar = SmidVar.Substring(0, SmidVar.Length - 1);

                for (int j = levelcnt + 1; j <= 6; j++)
                {
                    string AreaQueryChild = "select msrg.smid,msr.lvl,maingrp,Smname +' ('+ msr.Syncid + ' - ' + mr.RoleName + ')' as smname from mastsalesrepgrp msrg left join mastsalesrep msr on msrg.smid=msr.smid  LEFT JOIN mastrole mr ON mr.RoleId=msr.RoleId where msrg.maingrp  in (" + SmidVar + ") and msr.lvl=" + j + "  and msr.Active=1 order by SMName,lvl desc ";
                    DataTable dtChild = DbConnectionDAL.GetDataTable(CommandType.Text, AreaQueryChild);
                   // SmidVar = string.Empty;
                    int mTotRows = dtChild.Rows.Count;
                    if (mTotRows > 0)
                    {
                        SmidVar = string.Empty;
                        var str = "";
                        for (int k = 0; k < mTotRows; k++)
                        {
                            SmidVar += dtChild.Rows[k]["smid"].ToString() + ",";
                        }

                        TreeNode Oparent = parent;
                        switch (j)
                        {
                            case 3:
                                if (Oparent.Parent != null)
                                {
                                    foreach (TreeNode child in Oparent.ChildNodes)
                                    {
                                        str += child.Value + ","; parent = child;
                                        DataRow[] dr = dtChild.Select("maingrp =" + child.Value);
                                        for (int l = 0; l < dr.Length; l++)
                                        {
                                            FillChildArea(child, child.Value, dr[l]["smid"].ToString(), dr[l]["smname"].ToString());
                                        }
                                        dtChild.Select();
                                    }
                                }
                                else
                                {
                                    foreach (TreeNode child in Oparent.ChildNodes)
                                    {
                                        str += child.Value + ","; parent = child;
                                        DataRow[] dr = dtChild.Select("maingrp =" + child.Value);
                                        for (int l = 0; l < dr.Length; l++)
                                        {
                                            FillChildArea(child, child.Value, dr[l]["smid"].ToString(), dr[l]["smname"].ToString());
                                        }
                                        dtChild.Select();
                                    }
                                }
                                break;
                            case 4:
                                if (Oparent.Parent != null)
                                {
                                    foreach (TreeNode child in Oparent.ChildNodes)
                                    {
                                        str += child.Value + ","; parent = child;
                                        DataRow[] dr = dtChild.Select("maingrp =" + child.Value);
                                        for (int l = 0; l < dr.Length; l++)
                                        {
                                            FillChildArea(child, child.Value, dr[l]["smid"].ToString(), dr[l]["smname"].ToString());
                                        }
                                        dtChild.Select();
                                    }
                                }
                                else
                                {
                                    foreach (TreeNode child in Oparent.ChildNodes)
                                    {
                                        str += child.Value + ","; parent = child;
                                        DataRow[] dr = dtChild.Select("maingrp =" + child.Value);
                                        for (int l = 0; l < dr.Length; l++)
                                        {
                                            FillChildArea(child, child.Value, dr[l]["smid"].ToString(), dr[l]["smname"].ToString());
                                        }
                                        dtChild.Select();
                                    }
                                }
                                break;
                            case 5:
                                if (Oparent.Parent != null)
                                {
                                    foreach (TreeNode Pchild in Oparent.Parent.ChildNodes)
                                    {
                                        foreach (TreeNode child in Pchild.ChildNodes)
                                        {
                                            str += child.Value + ","; parent = child;
                                            DataRow[] dr = dtChild.Select("maingrp =" + child.Value);
                                            for (int l = 0; l < dr.Length; l++)
                                            {
                                                FillChildArea(child, child.Value, dr[l]["smid"].ToString(), dr[l]["smname"].ToString());
                                            }
                                            dtChild.Select();
                                        }
                                    }
                                }
                                else
                                {
                                    foreach (TreeNode child in Oparent.ChildNodes)
                                    {
                                        str += child.Value + ","; parent = child;
                                        DataRow[] dr = dtChild.Select("maingrp =" + child.Value);
                                        for (int l = 0; l < dr.Length; l++)
                                        {
                                            FillChildArea(child, child.Value, dr[l]["smid"].ToString(), dr[l]["smname"].ToString());
                                        }
                                        dtChild.Select();
                                    }
                                }


                                break;
                            case 6:
                                if (Oparent.Parent != null)
                                {
                                    if (Settings.Instance.RoleType == "Admin")
                                    {
                                        foreach (TreeNode Pchild in Oparent.Parent.Parent.ChildNodes)
                                        {
                                            foreach (TreeNode Qchild in Pchild.ChildNodes)
                                            {
                                                foreach (TreeNode child in Qchild.ChildNodes)
                                                {
                                                    str += child.Value + ","; parent = child;
                                                    DataRow[] dr = dtChild.Select("maingrp =" + child.Value);
                                                    for (int l = 0; l < dr.Length; l++)
                                                    {
                                                        FillChildArea(child, child.Value, dr[l]["smid"].ToString(), dr[l]["smname"].ToString());
                                                    }
                                                    dtChild.Select();
                                                }
                                            }
                                        }
                                    }

                                    else
                                    {
                                        foreach (TreeNode Pchild in Oparent.Parent.ChildNodes)
                                        {
                                            foreach (TreeNode child in Pchild.ChildNodes)
                                            {
                                                str += child.Value + ","; parent = child;
                                                DataRow[] dr = dtChild.Select("maingrp =" + child.Value);
                                                for (int l = 0; l < dr.Length; l++)
                                                {
                                                    FillChildArea(child, child.Value, dr[l]["smid"].ToString(), dr[l]["smname"].ToString());
                                                }
                                                dtChild.Select();
                                            }
                                        }
                                    }

                                }

                                else
                                {
                                    foreach (TreeNode child in Oparent.ChildNodes)
                                    {
                                        str += child.Value + ","; parent = child;
                                        DataRow[] dr = dtChild.Select("maingrp =" + child.Value);
                                        for (int l = 0; l < dr.Length; l++)
                                        {
                                            FillChildArea(child, child.Value, dr[l]["smid"].ToString(), dr[l]["smname"].ToString());
                                        }
                                        dtChild.Select();
                                    }
                                }


                                break;
                        }

                        SmidVar = SmidVar.Substring(0, SmidVar.Length - 1);
                    }
                }
            }
        }

        public void FillChildArea(TreeNode parent, string ParentId, string Smid, string SMName)
        {
            TreeNode child = new TreeNode();
            child.Text = SMName;
            child.Value = Smid;
            child.SelectAction = TreeNodeSelectAction.Expand;
            parent.ChildNodes.Add(child);
            child.CollapseAll();
        }
        //public void FillChildArea(TreeNode parent, string ParentId, int LVL, int SMId)
        //{
        //    //var AreaQueryChild = "select * from Mastsalesrep where lvl=" + (LVL + 1) + " and UnderId=" + SMId + " order by SMName,lvl";
        //    var AreaQueryChild = "SELECT SMId,Smname +' ('+ ms.Syncid + ' - ' + mr.RoleName + ')' as smname,Lvl from Mastsalesrep ms LEFT JOIN mastrole mr ON mr.RoleId=ms.RoleId where lvl=" + (LVL + 1) + " and UnderId=" + SMId + " and ms.Active=1 order by SMName,lvl";
        //    DataTable dtChild = DbConnectionDAL.GetDataTable(CommandType.Text, AreaQueryChild);
        //    parent.ChildNodes.Clear();
        //    foreach (DataRow dr in dtChild.Rows)
        //    {
        //        TreeNode child = new TreeNode();
        //        child.Text = dr["SMName"].ToString().Trim();
        //        child.Value = dr["SMId"].ToString().Trim();
        //        child.SelectAction = TreeNodeSelectAction.Expand;
        //        parent.ChildNodes.Add(child);
        //        //child.ExpandAll();
        //        child.CollapseAll();
        //        FillChildArea(child, child.Value, (Convert.ToInt32(dr["Lvl"])), Convert.ToInt32(dr["SMId"].ToString()));
        //    }

        //}
        //private void GetRoleType(string p)
        //{
        //    try
        //    {
        //        string roleqry = @"select * from MastRole where RoleId=" + Convert.ToInt32(p) + "";
        //        DataTable roledt = DbConnectionDAL.GetDataTable(CommandType.Text, roleqry);
        //        if (roledt.Rows.Count > 0)
        //        {
        //            roleType = roledt.Rows[0]["RoleType"].ToString();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.ToString();
        //    }
        //}


        protected void Cancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/MeetvsActualMeetReport.aspx");
        }

        protected void btnshow_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtcurrentyear.SelectedIndex > 0)
                {
                    fillUserType();
                }
                else
                {
                    GridView1.DataSource = null;
                    GridView1.DataBind();
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please select Year');", true);
                }
            }
            catch
            {

            }
        }



    }
}