using BusinessLayer;
using DAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AstralFFMS
{
    public partial class rptBrandwiseSecondarySales : System.Web.UI.Page
    {
        string roleType = "";
        int cnt = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;

            trview.Attributes.Add("onclick", "fireCheckChanged(event)");
            if (!IsPostBack)
            {
                roleType = Settings.Instance.RoleType;
                BindTreeViewControl();
                btnExport.Visible = true;
                //string pageName = Path.GetFileName(Request.Path);
                btnExport.Enabled = Settings.Instance.CheckExportPermission(pageName, Convert.ToString(Session["user_name"]));
                btnExport.CssClass = "btn btn-primary";

            }
        }

        private void BindTreeViewControl()
        {
            try
            {
                DataTable St = new DataTable();
                if (roleType == "Admin")
                { St = DbConnectionDAL.GetDataTable(CommandType.Text, "Select msr.smid,msr.Smname +' ('+ msr.Syncid + ' - ' + mr.RoleName + ')' as smname,msr.underid,msr.lvl from mastsalesrep msr  LEFT JOIN mastrole mr ON mr.RoleId=msr.RoleId where msr.active=1 and msr.underid<>0 order by msr.smname"); }
                else
                {
                    string query = "select msr.smid,msr.Smname +' ('+ msr.Syncid + ' - ' + mr.RoleName + ')' as smname,msr.underid ,msr.lvl from mastsalesrep msr  LEFT JOIN mastrole mr ON mr.RoleId=msr.RoleId where msr.smid in (select smid from MastSalesRepGrp where MainGrp  in (" + Settings.Instance.SMID + ")) and  msr.active=1 and msr.lvl >= (select distinct level from MastSalesRepGrp  where MainGrp in (" + Settings.Instance.SMID + " )) order by msr.smname";
                    St = DbConnectionDAL.GetDataTable(CommandType.Text, query);
                }
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
            smIDStr1 = smIDStr.TrimStart(',').TrimEnd(',');

            if (smIDStr1 != "")
            {
                qrychk = "where os.smid in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + smIDStr1 + ")) and Active=1)";
            }

            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select Sales Person');", true);
                TopRetailer.DataSource = null;
                TopRetailer.DataBind();
                return;
            }

            DateTime currentMonth = Settings.GetUTCTime();
            DateTime frdate1 = new DateTime(Convert.ToInt32((DateTime.ParseExact(currentMonth.ToString("dd/MMM/yyyy"), "dd/MMM/yyyy", CultureInfo.InvariantCulture)).ToString("yyyy")), Convert.ToInt32((DateTime.ParseExact(currentMonth.ToString("dd/MMM/yyyy"), "dd/MMM/yyyy", CultureInfo.InvariantCulture)).ToString("MM")), 1);

            DateTime Last = currentMonth.AddMonths(-1);
            int yr = Convert.ToInt32((DateTime.ParseExact(Last.ToString("dd/MMM/yyyy"), "dd/MMM/yyyy", CultureInfo.InvariantCulture)).ToString("yyyy"));
            int mnth = Convert.ToInt32((DateTime.ParseExact(Last.ToString("dd/MMM/yyyy"), "dd/MMM/yyyy", CultureInfo.InvariantCulture)).ToString("MM"));
            DateTime todate2 = new DateTime(yr, mnth, DateTime.DaysInMonth(yr, mnth));
            DateTime frdate2 = new DateTime(yr, mnth, 1);

            Last = todate2.AddMonths(-1);
            yr = Convert.ToInt32((DateTime.ParseExact(Last.ToString("dd/MMM/yyyy"), "dd/MMM/yyyy", CultureInfo.InvariantCulture)).ToString("yyyy"));
            mnth = Convert.ToInt32((DateTime.ParseExact(Last.ToString("dd/MMM/yyyy"), "dd/MMM/yyyy", CultureInfo.InvariantCulture)).ToString("MM"));
            DateTime todate3 = new DateTime(yr, mnth, DateTime.DaysInMonth(yr, mnth));
            DateTime frdate3 = new DateTime(yr, mnth, 1);

            Last = todate3.AddMonths(-1);
            yr = Convert.ToInt32((DateTime.ParseExact(Last.ToString("dd/MMM/yyyy"), "dd/MMM/yyyy", CultureInfo.InvariantCulture)).ToString("yyyy"));
            mnth = Convert.ToInt32((DateTime.ParseExact(Last.ToString("dd/MMM/yyyy"), "dd/MMM/yyyy", CultureInfo.InvariantCulture)).ToString("MM"));
            DateTime todate4 = new DateTime(yr, mnth, DateTime.DaysInMonth(yr, mnth));
            DateTime frdate4 = new DateTime(yr, mnth, 1);

            Last = todate4.AddMonths(-1);
            yr = Convert.ToInt32((DateTime.ParseExact(Last.ToString("dd/MMM/yyyy"), "dd/MMM/yyyy", CultureInfo.InvariantCulture)).ToString("yyyy"));
            mnth = Convert.ToInt32((DateTime.ParseExact(Last.ToString("dd/MMM/yyyy"), "dd/MMM/yyyy", CultureInfo.InvariantCulture)).ToString("MM"));
            DateTime todate5 = new DateTime(yr, mnth, DateTime.DaysInMonth(yr, mnth));
            DateTime frdate5 = new DateTime(yr, mnth, 1);

            Last = todate5.AddMonths(-1);
            yr = Convert.ToInt32((DateTime.ParseExact(Last.ToString("dd/MMM/yyyy"), "dd/MMM/yyyy", CultureInfo.InvariantCulture)).ToString("yyyy"));
            mnth = Convert.ToInt32((DateTime.ParseExact(Last.ToString("dd/MMM/yyyy"), "dd/MMM/yyyy", CultureInfo.InvariantCulture)).ToString("MM"));
            DateTime todate6 = new DateTime(yr, mnth, DateTime.DaysInMonth(yr, mnth));
            DateTime frdate6 = new DateTime(yr, mnth, 1);

            Last = todate6.AddMonths(-1);
            yr = Convert.ToInt32((DateTime.ParseExact(Last.ToString("dd/MMM/yyyy"), "dd/MMM/yyyy", CultureInfo.InvariantCulture)).ToString("yyyy"));
            mnth = Convert.ToInt32((DateTime.ParseExact(Last.ToString("dd/MMM/yyyy"), "dd/MMM/yyyy", CultureInfo.InvariantCulture)).ToString("MM"));
            DateTime todate7 = new DateTime(yr, mnth, DateTime.DaysInMonth(yr, mnth));
            DateTime frdate7 = new DateTime(yr, mnth, 1);

            Query = "select a.ProductGroup,SUM(a.t1_Value) as t1Value,SUM(a.t2_value) as t2Value,SUM(a.t3_value) as t3Value,SUM(a.t4_value) as t4Value,SUM(a.t5_value) as t5Value,SUM(a.t6_value) as t6Value,SUM(a.t7_value) as t7Value from (" +
                       "select pg.ItemName as ProductGroup, convert(numeric(18,2) ,SUM (os.Qty*os.Rate )) as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value, 0 as t5_value,0 as t6_value,0 as t7_value from TransOrder1 os left join MastItem i on i.ItemId=os.ItemId left join MastItem pg on pg.ItemId=i.Underid left JOIN mastparty p on p.PartyId=os.PartyId " + qrychk + " and os.VDate between '" + frdate1.ToString("dd/MMM/yyyy") + " 00:00' and '" + currentMonth.ToString("dd/MMM/yyyy") + " 23:59' group by pg.ItemName union all" +
                       " select pg.ItemName as ProductGroup,0 as t1_Value ,convert(numeric(18,2) ,SUM (os.Qty*os.Rate )) as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value from TransOrder1 os left join MastItem i on i.ItemId=os.ItemId left join MastItem pg on pg.ItemId=i.Underid left JOIN mastparty p on p.PartyId=os.PartyId " + qrychk + " and os.VDate between '" + frdate2.ToString("dd/MMM/yyyy") + " 00:00' and '" + todate2.ToString("dd/MMM/yyyy") + " 23:59' group by pg.ItemName union all " +
                       "select pg.ItemName as ProductGroup,0 as t1_Value ,0 as t2_value,convert(numeric(18,2) ,SUM (os.Qty*os.Rate )) as t3_value,0 as t4_value, 0 as t5_value,0 as t6_value,0 as t7_value from TransOrder1 os left join MastItem i on i.ItemId=os.ItemId left join MastItem pg on pg.ItemId=i.Underid left JOIN mastparty p on p.PartyId=os.PartyId " + qrychk + " and os.VDate between '" + frdate3.ToString("dd/MMM/yyyy") + " 00:00' and '" + todate3.ToString("dd/MMM/yyyy") + " 23:59' group by pg.ItemName union all " +
                       "select pg.ItemName as ProductGroup,0 as t1_Value ,0 as t2_value,0 as t3_value,convert(numeric(18,2) ,SUM (os.Qty*os.Rate )) as t4_value, 0 as t5_value,0 as t6_value,0 as t7_value from TransOrder1 os left join MastItem i on i.ItemId=os.ItemId left join MastItem pg on pg.ItemId=i.Underid left JOIN mastparty p on p.PartyId=os.PartyId " + qrychk + " and os.VDate between '" + frdate4.ToString("dd/MMM/yyyy") + " 00:00' and '" + todate4.ToString("dd/MMM/yyyy") + " 23:59' group by pg.ItemName union all " +
                       "select pg.ItemName as ProductGroup,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,convert(numeric(18,2) ,SUM (os.Qty*os.Rate )) as t5_value,0 as t6_value,0 as t7_value from TransOrder1 os left join MastItem i on i.ItemId=os.ItemId left join MastItem pg on pg.ItemId=i.Underid left JOIN mastparty p on p.PartyId=os.PartyId " + qrychk + " and os.VDate between '" + frdate5.ToString("dd/MMM/yyyy") + " 00:00' and '" + todate5.ToString("dd/MMM/yyyy") + " 23:59'  group by pg.ItemName union all " +
                       "select pg.ItemName as ProductGroup,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value, 0 as t5_value,convert(numeric(18,2) ,SUM (os.Qty*os.Rate )) as t6_value,0 as t7_value from TransOrder1 os left join MastItem i on i.ItemId=os.ItemId left join MastItem pg on pg.ItemId=i.Underid left JOIN mastparty p on p.PartyId=os.PartyId " + qrychk + " and os.VDate between '" + frdate6.ToString("dd/MMM/yyyy") + " 00:00' and '" + todate6.ToString("dd/MMM/yyyy") + " 23:59' group by pg.ItemName union all " +
                       "select pg.ItemName as ProductGroup,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,convert(numeric(18,2) ,SUM (os.Qty*os.Rate )) as t7_value from TransOrder1 os left join MastItem i on i.ItemId=os.ItemId left join MastItem pg on pg.ItemId=i.Underid left JOIN mastparty p on p.PartyId=os.PartyId " + qrychk + " and os.VDate between '" + frdate7.ToString("dd/MMM/yyyy") + " 00:00' and '" + todate7.ToString("dd/MMM/yyyy") + " 23:59'  group by pg.ItemName " +
                       ")a group by a.ProductGroup Order by a.ProductGroup";


            DataTable dtItem = DbConnectionDAL.GetDataTable(CommandType.Text, Query);
            if (dtItem.Rows.Count > 0)
            {
                rptmain.Style.Add("display", "block");
                TopRetailer.DataSource = dtItem;
                TopRetailer.DataBind();
                btnExport.Visible = true;
            }
            else
            {
                rptmain.Style.Add("display", "block");
                TopRetailer.DataSource = dtItem;
                TopRetailer.DataBind();
                btnExport.Visible = false;
            }
            dtItem.Dispose();

        }



        protected void Cancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/TopSalesPerson.aspx");
        }
        protected void trview_TreeNodeCheckChanged(object sender, TreeNodeEventArgs e)
        {
            string smIDStr = "", smIDStr12 = "";
            if (cnt == 0)
            {
                foreach (TreeNode node in trview.CheckedNodes)
                {
                    smIDStr12 = node.Value;
                    {
                        smIDStr += node.Value + ",";
                    }
                }
                smIDStr12 = smIDStr.TrimStart(',').TrimEnd(',');

                ViewState["tree"] = smIDStr12;

            }

            cnt = cnt + 1;
            return;
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=BrandwiseSecondarySales.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            TopRetailer.RenderControl(hw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
        }

        protected void TopRetailer_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            //Finding the HeaderTemplate and access its controls                 
            string month1 = DateTime.Now.ToString("MMM");
            string month2 = DateTime.Now.AddMonths(-1).ToString("MMM");
            string month3 = DateTime.Now.AddMonths(-2).ToString("MMM");
            string month4 = DateTime.Now.AddMonths(-3).ToString("MMM");
            string month5 = DateTime.Now.AddMonths(-4).ToString("MMM");
            string month6 = DateTime.Now.AddMonths(-5).ToString("MMM");
            string month7 = DateTime.Now.AddMonths(-6).ToString("MMM");
            Control HeaderTemplate = TopRetailer.Controls[0].Controls[0];
            Label lblMonth1 = HeaderTemplate.FindControl("lblMonth1") as Label;
            lblMonth1.Text = month1;
            Label lblMonth2 = HeaderTemplate.FindControl("lblMonth2") as Label;
            lblMonth2.Text = month2;
            Label lblMonth3 = HeaderTemplate.FindControl("lblMonth3") as Label;
            lblMonth3.Text = month3;
            Label lblMonth4 = HeaderTemplate.FindControl("lblMonth4") as Label;
            lblMonth4.Text = month4;
            Label lblMonth5 = HeaderTemplate.FindControl("lblMonth5") as Label;
            lblMonth5.Text = month5;
            Label lblMonth6 = HeaderTemplate.FindControl("lblMonth6") as Label;
            lblMonth6.Text = month6;
            Label lblMonth7 = HeaderTemplate.FindControl("lblMonth7") as Label;
            lblMonth7.Text = month7;
        }
    }
}
