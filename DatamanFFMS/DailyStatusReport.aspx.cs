using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BusinessLayer;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using DAL;
using BusinessLayer;
using BAL;
using System.IO;
namespace AstralFFMS
{
    public partial class DailyStatusReport : System.Web.UI.Page
    {
        Settings SetObj = Settings.Instance;
        protected void Page_Load(object sender, EventArgs e)
        {
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;

            ScriptManager.GetCurrent(this).RegisterPostBackControl(btnExport);
            if (!IsPostBack)
            {
                txt_fromdate.Text = DateTime.Now.ToUniversalTime().AddSeconds(Convert.ToInt32(19800)).ToString("dd-MMM-yyyy");
               // GetGroupName();
                BindPerson();

            }
            if (SetObj.GroupID != "0" && SetObj.GroupID != "")
            {
                if (!IsPostBack)
                {
                    ddlType.SelectedValue = SetObj.GroupID;
                }
            }
            else
            {
                SetObj.GroupID = ddlType.SelectedValue;
            }
        }
        private void GetGroupName()
        {
            string str = "select GroupMaster.GroupID as Code, GroupMaster.Description as Description from UserGrp inner Join GroupMaster on GroupMaster.GroupID = UserGrp.GroupId where UserGrp.UserID = '" + Settings.Instance.UserID + "' order by GroupMaster.Description";
            fillDDLDirect(ddlType, str, "Code", "Description", 1);
        }
        public void ShowAlert(string Message)
        {
            string script = "window.alert(\"" + Message.Normalize() + "\");";
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "UniqueKey", script, true);
        }
        private void BindPerson()
        {
            DataTable dt = Settings.UnderUsers(Settings.Instance.SMID);
            DataView dv = new DataView(dt);
            dv.RowFilter = "SMId<>" + Convert.ToInt32(Settings.Instance.SMID) + "";
            DropDownList2.DataSource = dv.ToTable();
            DropDownList2.DataTextField = "SMName";
            DropDownList2.DataValueField = "deviceNo";
            DropDownList2.DataBind();
            //Add Default Item in the DropDownList
            DropDownList2.Items.Insert(0, new ListItem("--Please select--"));
        }
        protected void gvData_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            BindGrid();
            gvData.PageIndex = e.NewPageIndex;
            gvData.DataBind();
        }
        protected void txt_fromdate_TextChanged(object sender, EventArgs e)
        {
            Regex regexDt = new Regex("(^(((([1-9])|([0][1-9])|([1-2][0-9])|(30))\\-([A,a][P,p][R,r]|[J,j][U,u][N,n]|[S,s][E,e][P,p]|[N,n][O,o][V,v]))|((([1-9])|([0][1-9])|([1-2][0-9])|([3][0-1]))\\-([J,j][A,a][N,n]|[M,m][A,a][R,r]|[M,m][A,a][Y,y]|[J,j][U,u][L,l]|[A,a][U,u][G,g]|[O,o][C,c][T,t]|[D,d][E,e][C,c])))\\-[0-9]{4}$)|(^(([1-9])|([0][1-9])|([1][0-9])|([2][0-8]))\\-([F,f][E,e][B,b])\\-[0-9]{2}(([02468][1235679])|([13579][01345789]))$)|(^(([1-9])|([0][1-9])|([1][0-9])|([2][0-9]))\\-([F,f][E,e][B,b])\\-[0-9]{2}(([02468][048])|([13579][26]))$)");
            Match mtStartDt = Regex.Match(txt_fromdate.Text, regexDt.ToString());

            if (mtStartDt.Success)
            {
                if (txt_fromdate.Text != "")
                {
                    DateTime dt1 = Convert.ToDateTime(txt_fromdate.Text.Trim()).Date;
                    DateTime dt2 = DateTime.Now.ToUniversalTime().AddSeconds(Convert.ToInt32(19800));
                    if (dt1 <= dt2)
                    {
                    }
                    else
                    {
                        ShowAlert("Date should not be greater than current date!!");
                        txt_fromdate.Text = DateTime.Now.ToUniversalTime().AddSeconds(Convert.ToInt32(19800)).ToString("dd-MMM-yyyy");
                        return;

                    }
                }
            }
            else
            {
                ShowAlert("Invalid Date!");
            }
        }
        public void ClearData()
        {
            Settings.Instance.RedirectCurrentPage(Settings.Instance.GetCurrentPageNameWithQueryString(), this);
        }
        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetObj.GroupID = ddlType.SelectedValue;
        }
        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                // GridViewExportUtil.Export("DailyStatusReport", gvData);
                GridViewExportUtil.ExportGridToCSV(gvData, "DailyStatusReport");
            }
            catch (Exception ex)
            {
                ShowAlert("There are some errors while loading records!");
            }
        }
        public static void fillDDLDirect(DropDownList xddl, string xmQry, string xvalue, string xtext, int sele)
        {
            xddl.DataSource = null;
            xddl.DataBind();
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
                xddl.Items.Insert(0, new ListItem("All", "0"));
            }
            xdt.Dispose();
        }
        protected void btngen_Click(object sender, EventArgs e)
        {
            BindGrid();
        }
        private void BindGrid()
        {
            try
            {
                DataTable dtbl = new DataTable();
                string str = "";
                if (ddlstatus.SelectedValue == "1")
                {
                    //str = "select p.DeviceNo,p.PersonName,p.Mobile, currentdate='"+txt_fromdate.Text+"',Status='Data not Received' from  PersonMaster p inner join GrpMapp gmp on p.ID=gmp.PersonID " +
                    //        " where gmp.GroupID=" + ddlType.SelectedValue + " and p.DeviceNo not in(select DeviceNo from LocationDetails where convert(date,CurrentDate)=CONVERT(date,'" + txt_fromdate.Text + "') ) order by p.PersonName";
                    str = "select p.DeviceNo,concat(p.PersonName, ' ('+ p.empcode ) + ' )'  as PersonName,p.Mobile, currentdate='" + txt_fromdate.Text + "',Status='Data not Received' from  PersonMaster p inner join GrpMapp gmp on p.ID=gmp.PersonID " +
                           " where  p.DeviceNo='" + DropDownList2.SelectedValue + "' and p.DeviceNo not in(select DeviceNo from LocationDetails where convert(date,CurrentDate)=CONVERT(date,'" + txt_fromdate.Text + "') ) order by p.PersonName";
                }
                else if (ddlstatus.SelectedValue == "2")
                {
                    //str = "SELECT p.DeviceNo,p.PersonName,p.Mobile, max(lg.currentdate) As currentdate,Status='GPS Off' FROM Log_Tracker lg LEFT JOIN PersonMaster p ON lg.DeviceNo=p.DeviceNo left join GrpMapp gmp on p.ID=gmp.PersonID " +
                    //         " WHERE gmp.GroupID=" + ddlType.SelectedValue + "and Convert(date,lg.CurrentDate)=CONVERT(date,'" + txt_fromdate.Text + "') and lg.Status='GO' GROUP BY p.PersonName,p.DeviceNo,p.Mobile order by p.PersonName";
                    str = "SELECT p.DeviceNo,concat(p.PersonName, ' ('+ p.empcode ) + ' )'  as PersonName,p.Mobile, max(lg.currentdate) As currentdate,Status='GPS Off' FROM Log_Tracker lg LEFT JOIN PersonMaster p ON lg.DeviceNo=p.DeviceNo left join GrpMapp gmp on p.ID=gmp.PersonID " +
                             " WHERE p.DeviceNo='" + DropDownList2.SelectedValue + "' and Convert(date,lg.CurrentDate)=CONVERT(date,'" + txt_fromdate.Text + "') and lg.Status='GO' GROUP BY p.PersonName,p.DeviceNo,p.Mobile,p.empcode order by p.PersonName";
                }
                else if (ddlstatus.SelectedValue == "3")
                {
                    //str = "SELECT p.DeviceNo,p.PersonName,p.Mobile,max(lg.currentdate) As currentdate,Status='Data Off' FROM Log_Tracker lg LEFT JOIN PersonMaster p ON lg.DeviceNo=p.DeviceNo left join GrpMapp gmp on p.ID=gmp.PersonID " +
                    //       " WHERE gmp.GroupID=" + ddlType.SelectedValue + "and Convert(date,lg.CurrentDate)=CONVERT(date,'" + txt_fromdate.Text + "') and lg.Status='MO' GROUP BY p.PersonName,p.DeviceNo,p.Mobile order by p.PersonName";
                    str = "SELECT p.DeviceNo,concat(p.PersonName, ' ('+ p.empcode ) + ' )'  as PersonName,p.Mobile,max(lg.currentdate) As currentdate,Status='Data Off' FROM Log_Tracker lg LEFT JOIN PersonMaster p ON lg.DeviceNo=p.DeviceNo left join GrpMapp gmp on p.ID=gmp.PersonID " +
                          " WHERE p.DeviceNo='" + DropDownList2.SelectedValue + "' and Convert(date,lg.CurrentDate)=CONVERT(date,'" + txt_fromdate.Text + "') and lg.Status='MO' GROUP BY p.PersonName,p.DeviceNo,p.Mobile,p.empcode order by p.PersonName";
                }
                else
                {
                    ShowAlert("Please Select a Status");
                }
                if (!string.IsNullOrEmpty(str))
                {
                    dtbl = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                    if (dtbl.Rows.Count > 0)
                    {
                        btnExport.Visible = true;
                        gvData.DataSource = dtbl;
                        gvData.DataBind();
                    }
                    else
                    {
                        gvData.DataSource = null;
                        gvData.DataBind();
                        btnExport.Visible = false;
                        ShowAlert("No Record Found !!");
                    }
                }

                #region Old Code
                //int cntdnr = 0;
                //gvData.DataSource = null;
                //gvData.DataBind();
                // DataTable dttemp = new DataTable();
                // DataTable dtbl = new DataTable();
                // dttemp.Columns.Add("PersonName");
                // dttemp.Columns.Add("DeviceNo");
                // dttemp.Columns.Add("CurrentDate");
                // dttemp.Columns.Add("Mobile");
                // DataRow dr1 = dttemp.NewRow();
                // dr1["PersonName"] = "Data Not Received".ToUpper();
                // dttemp.Rows.Add(dr1);
                // DataRow drow0 = dttemp.NewRow();
                // dttemp.Rows.InsertAt(drow0, 2);
                // dttemp.AcceptChanges();
                // string str = "select p.DeviceNo,p.PersonName,p.Mobile from  PersonMaster p inner join GrpMapp gmp on p.ID=gmp.PersonID " +
                //            " where gmp.GroupID=" + ddlType.SelectedValue + " and p.DeviceNo not in(select DeviceNo from LocationDetails where convert(date,CurrentDate)=CONVERT(date,'" + txt_fromdate.Text + "')) order by p.PersonName";
                // dtbl = DAL.getFromDataTable(str);
                // btnExport.Visible = true;
                //if (dtbl.Rows.Count > 0)
                //{
                //    cntdnr = dtbl.Rows.Count;
                //    for (int i = 0; i < dtbl.Rows.Count; i++)
                //    {
                //       DataRow dr = dttemp.NewRow();
                //       dr["PersonName"] = dtbl.Rows[i]["PersonName"].ToString();
                //       dr["DeviceNo"] = dtbl.Rows[i]["DeviceNo"].ToString();
                //       dr["CurrentDate"] = txt_fromdate.Text.Trim();
                //       dr["Mobile"] = dtbl.Rows[i]["Mobile"].ToString();
                //       dttemp.Rows.Add(dr);
                //    }
                //    DataRow drow = dttemp.NewRow();
                //    dttemp.Rows.InsertAt(drow, cntdnr + 2);
                //    dttemp.AcceptChanges();
                //}
                //else
                //{
                //    DataRow dr = dttemp.NewRow();
                //    dttemp.Rows.InsertAt(dr, 4);
                //    dr["PersonName"] = "No Record Found !!";
                //    DataRow drow = dttemp.NewRow();
                //    dttemp.Rows.InsertAt(drow, 4);
                //    dttemp.AcceptChanges();
                //}

                //dtbl.Clear();
                //DataRow dr2 = dttemp.NewRow();
                //dr2["PersonName"] = "GPS Off".ToUpper();
                //dttemp.Rows.Add(dr2);
                //DataRow drow1 = dttemp.NewRow();
                //dttemp.Rows.InsertAt(drow1, dttemp.Rows.Count +1);
                //dttemp.AcceptChanges();
                //string strGO = "SELECT p.DeviceNo,p.PersonName,p.Mobile,max(lg.currentdate) As currentdate FROM Log_Tracker lg LEFT JOIN PersonMaster p ON lg.DeviceNo=p.DeviceNo left join GrpMapp gmp on p.ID=gmp.PersonID " +
                //           " WHERE gmp.GroupID=" + ddlType.SelectedValue + "and Convert(date,lg.CurrentDate)=CONVERT(date,'" + txt_fromdate.Text + "') and lg.Status='GO' GROUP BY p.PersonName,p.DeviceNo,p.Mobile";
                //dtbl = DAL.getFromDataTable(strGO);
                //if (dtbl.Rows.Count > 0)
                //{

                //    for (int i = 0; i < dtbl.Rows.Count; i++)
                //    {
                //        DataRow dr = dttemp.NewRow();
                //        dr["PersonName"] = dtbl.Rows[i]["PersonName"].ToString();
                //        dr["DeviceNo"] = dtbl.Rows[i]["DeviceNo"].ToString();
                //        dr["CurrentDate"] = dtbl.Rows[i]["CurrentDate"].ToString();
                //        dr["Mobile"] = dtbl.Rows[i]["Mobile"].ToString();
                //        dttemp.Rows.Add(dr);
                //    }
                //}
                //else
                //{
                //    DataRow dr = dttemp.NewRow();
                //    dttemp.Rows.InsertAt(dr, dttemp.Rows.Count + 4);
                //    dr["PersonName"] = "No Record Found !!";
                //}
                //DataRow drow2 = dttemp.NewRow();
                //dttemp.Rows.InsertAt(drow2, dttemp.Rows.Count);
                //dttemp.AcceptChanges();

                //dtbl.Clear();
                //DataRow dr3 = dttemp.NewRow();
                //dr3["PersonName"] = "Data Off".ToUpper() ;
                //dttemp.Rows.Add(dr3);
                //DataRow drow3 = dttemp.NewRow();
                //dttemp.Rows.InsertAt(drow3, dttemp.Rows.Count + 4);
                //dttemp.AcceptChanges();

                //string strMO = "SELECT p.DeviceNo,p.PersonName,p.Mobile,max(lg.currentdate) As currentdate FROM Log_Tracker lg LEFT JOIN PersonMaster p ON lg.DeviceNo=p.DeviceNo left join GrpMapp gmp on p.ID=gmp.PersonID " +
                //           " WHERE gmp.GroupID=" + ddlType.SelectedValue + "and Convert(date,lg.CurrentDate)=CONVERT(date,'" + txt_fromdate.Text + "') and lg.Status='MO' GROUP BY p.PersonName,p.DeviceNo,p.Mobile";
                //dtbl = DAL.getFromDataTable(strMO);
                //if (dtbl.Rows.Count > 0)
                //{
                //    for (int i = 0; i < dtbl.Rows.Count; i++)
                //    {
                //        DataRow dr = dttemp.NewRow();
                //        dr["PersonName"] = dtbl.Rows[i]["PersonName"].ToString();
                //        dr["DeviceNo"] = dtbl.Rows[i]["DeviceNo"].ToString();
                //        dr["CurrentDate"] = dtbl.Rows[i]["CurrentDate"].ToString();
                //        dr["Mobile"] = dtbl.Rows[i]["Mobile"].ToString();
                //        dttemp.Rows.Add(dr);
                //    }
                //}
                //else
                //{
                //    DataRow dr = dttemp.NewRow();
                //    dttemp.Rows.InsertAt(dr, dttemp.Rows.Count + 2);
                //    dr["PersonName"] = "No Record Found !!";
                //}
                //dttemp.AcceptChanges();
                //dtbl.Clear();
                //gvData.DataSource = dttemp;
                //gvData.DataBind();
                //dttemp.Clear();
                #endregion
            }
            catch (Exception)
            {
                ShowAlert("There are some problems while loading records!");
            }
        }
    }
}