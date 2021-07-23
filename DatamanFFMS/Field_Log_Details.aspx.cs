using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLayer;
using DAL;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Text;

namespace AstralFFMS
{
    public partial class Field_Log_Details : System.Web.UI.Page
    {
        int smID = 0;
        int msg = 0;
        string roleType = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;

            frmTextBox.Attributes.Add("readonly", "readonly");
            toTextBox.Attributes.Add("readonly", "readonly");

            if (!Page.IsPostBack)
            {
                //string pageName = Path.GetFileName(Request.Path);
                btnExport.Enabled = Settings.Instance.CheckExportPermission(pageName, Convert.ToString(Session["user_name"]));
                btnExport.CssClass = "btn btn-primary";

                //frmTextBox.Text = Settings.GetUTCTime().AddMonths(-1).ToString("dd/MMM/yyyy");
                frmTextBox.Text = Settings.GetUTCTime().AddDays(-6).ToString("dd/MMM/yyyy");
                toTextBox.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");
                //Ankita - 18/may/2016- (For Optimization)
                // GetRoleType(Settings.Instance.RoleID);
                roleType = Settings.Instance.RoleType;
                // BindSalePersonDDl();
                // fill_TreeArea();
                BindTreeViewControl();
                btnExport.Visible = false;
            }
        }

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
            string smIDStr = "", smIDStr1 = "", Query = "", qrychk = "";

            foreach (TreeNode node in trview.CheckedNodes)
            {
                smIDStr1 = node.Value;
                {
                    smIDStr += node.Value + ",";
                }
            }

            if (smIDStr1 == "")
            {
                DataTable dtSMId = Settings.UnderUsers(Settings.Instance.SMID);
                DataView dv = new DataView(dtSMId);

                if (dv.ToTable().Rows.Count > 0)
                {
                    foreach (DataRow dr in dv.ToTable().Rows)
                    {
                        smIDStr = smIDStr + "," + Convert.ToString(dr["SMId"]);
                    }
                    smIDStr1 = smIDStr.TrimStart(',').TrimEnd(',');
                }
                dtSMId.Dispose();
                dv.Dispose();
            }
            smIDStr1 = smIDStr.TrimStart(',').TrimEnd(',');

            if (smIDStr1 != "")
            {
                Query = "select fld.CurrentDate as CurrDte,fld.Status as Status,fld.SMID as SMID,fld.[FromTime] as FDate,fld.[ToTime] as TDate,fld.[vDate] as VisDate,case when fldm.Name IS NULL then fld.Status else fldm.Name end as StatusMaster,msp.SMName as SalePerson,tv.VisId as VisitId,tv.VDate as VisitDate from Field_Log_Details fld left join Field_Log_Details_Master fldm on fld.Status=fldm.Status left join MastSalesrep msp on msp.SMid=fld.Smid left join TransVisit tv on tv.smid=msp.smid and convert(varchar(10),tv.vdate,101)=convert(varchar(10),fld.CurrentDate,101) where fld.SMID in (" + smIDStr1 + ") and fld.CurrentDate between '" + Settings.dateformat(frmTextBox.Text) + " 00:00:00' and '" + Settings.dateformat(toTextBox.Text) + " 23:59:59' Order by fld.CurrentDate";

                DataTable dtItem = DbConnectionDAL.GetDataTable(CommandType.Text, Query);
                if (dtItem.Rows.Count > 0)
                {
                    rptmain.Style.Add("display", "block");
                    rpt.DataSource = dtItem;
                    rpt.DataBind();
                    btnExport.Visible = true;
                }
                else
                {
                    rptmain.Style.Add("display", "block");
                    rpt.DataSource = dtItem;
                    rpt.DataBind();
                    btnExport.Visible = false;
                }

                dtItem.Dispose();
            }

            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select Sales Person');", true);
                rpt.DataSource = null;
                rpt.DataBind();
                return;
            }
        }



        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Field_Log_Details.aspx", true);
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            string str = "", straddqry = ""; StringBuilder sb = new StringBuilder(); string col = "";
            string smIDStr = "", smIDStr1 = "", Query = "", qrychk = "";

            foreach (TreeNode node in trview.CheckedNodes)
            {
                smIDStr1 = node.Value;
                {
                    smIDStr += node.Value + ",";
                }
            }
            //smIDStr1 = smIDStr.TrimStart(',').TrimEnd(',');

            if (smIDStr1 == "")
            {
                DataTable dtSMId = Settings.UnderUsers(Settings.Instance.SMID);
                DataView dv = new DataView(dtSMId);

                if (dv.ToTable().Rows.Count > 0)
                {
                    foreach (DataRow dr in dv.ToTable().Rows)
                    {
                        smIDStr = smIDStr + "," + Convert.ToString(dr["SMId"]);
                    }
                    smIDStr1 = smIDStr.TrimStart(',').TrimEnd(',');
                }
                dtSMId.Dispose();
                dv.Dispose();
            }
            smIDStr1 = smIDStr.TrimStart(',').TrimEnd(',');

            if (smIDStr1 != "")
            {
                str = @"select fld.SMID as SMID,msp.SMName as [Sales Person],tv.VisId as [Visit ID],tv.VDate as [Visit Date],fldm.Name as Status,fld.CurrentDate as [Log Date Time] from Field_Log_Details fld left join Field_Log_Details_Master fldm on fld.Status=fldm.Status left join MastSalesrep msp on msp.SMid=fld.Smid left join TransVisit tv on tv.smid=msp.smid and convert(varchar(10),tv.vdate,101)=convert(varchar(10),fld.CurrentDate,101) where fld.SMID in (" + smIDStr1 + ") and fld.CurrentDate between '" + Settings.dateformat(frmTextBox.Text) + " 00:00:00' and '" + Settings.dateformat(toTextBox.Text) + " 23:59:59' Order by fld.CurrentDate";
                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                // DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                foreach (DataColumn dc in dt.Columns)
                {
                    col += dc.ColumnName + ",";
                }

                sb.AppendLine(col.Trim(','));
                foreach (DataRow row in dt.Rows)
                {
                    IEnumerable<string> fields = row.ItemArray.Select(field =>
                      string.Concat("\"", field.ToString().Replace("\"", "\"\""), "\""));
                    sb.AppendLine(string.Join(",", fields));
                }

                Response.Clear();
                Response.ContentType = "text/csv";
                Response.AddHeader("content-disposition", "attachment;filename=FieldMobileLogDetails.csv");
                Response.Write(sb.ToString());
                Response.End();
            }

            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select Sales Person');", true);
                rpt.DataSource = null;
                rpt.DataBind();
                return;
            }
        }
    }
}