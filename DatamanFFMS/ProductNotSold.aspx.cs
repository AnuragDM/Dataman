using BusinessLayer;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AstralFFMS
{
    public partial class ProductNotSold : System.Web.UI.Page
    {

        string roleType = "";
        int cnt = 0;
        protected void Page_Load(object sender, EventArgs e)
        {

            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;
            txtasondate.Attributes.Add("readonly", "readonly");
            txtfromdate.Attributes.Add("readonly", "readonly");
            trview.Attributes.Add("onclick", "fireCheckChanged(event)");
            if (!IsPostBack)
            {
                BindMaterialGroup();
                roleType = Settings.Instance.RoleType;
                BindTreeViewControl();
                txtfromdate.Text = Settings.GetUTCTime().AddMonths(-1).ToString("dd/MMM/yyyy");
                txtasondate.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");
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
                {
                    //  St = Settings.UnderUsersforlevels(Settings.Instance.SMID, 1);
                    St = DbConnectionDAL.GetDataTable(CommandType.Text, "Select msr.smid,msr.Smname +' ('+ msr.Syncid + ' - ' + mr.RoleName + ')' as smname,msr.underid,msr.lvl from mastsalesrep msr  LEFT JOIN mastrole mr ON mr.RoleId=msr.RoleId where msr.active=1 and msr.underid<>0 order by msr.smname");
                }
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

        private void BindMaterialGroup()
        {
            try
            {
                string prodClassQry = @"select ItemId,ItemName from MastItem where ItemType='MATERIALGROUP' and Active=1 order by ItemName";
                DataTable dtProdRep = DbConnectionDAL.GetDataTable(CommandType.Text, prodClassQry);
                if (dtProdRep.Rows.Count > 0)
                {
                    matGrpListBox.DataSource = dtProdRep;
                    matGrpListBox.DataTextField = "ItemName";
                    matGrpListBox.DataValueField = "ItemId";
                    matGrpListBox.DataBind();
                }
                dtProdRep.Dispose();
            }

            catch (Exception ex)
            {
                ex.ToString();
            }
        }
        private void ClearControls()
        {
            try
            {
                productListBox.Items.Clear();
                rptPNotSold.DataSource = null;
                rptPNotSold.DataBind();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {

            string smIDStr = "", smIDStr1 = "", Qrychk = "", Qry = "", QueryMatGrp = "", matGrpStrNew = "", matProStrNew = "";

            foreach (TreeNode node in trview.CheckedNodes)
            {
                smIDStr1 = node.Value;
                {
                    smIDStr += node.Value + ",";
                }
            }
            smIDStr1 = smIDStr.TrimStart(',').TrimEnd(',');

            //For MatGrp
            foreach (ListItem matGrpItems in matGrpListBox.Items)
            {
                if (matGrpItems.Selected)
                {
                    matGrpStrNew += matGrpItems.Value + ",";
                }
            }
            matGrpStrNew = matGrpStrNew.TrimStart(',').TrimEnd(',');

            //For Product
            foreach (ListItem product in productListBox.Items)
            {
                if (product.Selected)
                {
                    matProStrNew += product.Value + ",";
                }
            }
            matProStrNew = matProStrNew.TrimStart(',').TrimEnd(',');

            if (txt_noofdays.Text == "")
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please fill no of days');", true);
                rptPNotSold.DataSource = null;
                rptPNotSold.DataBind();
                return;

            }
            if (smIDStr1 != "")
            {
                Qrychk = " os1.smid in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + smIDStr1 + ")) and Active=1)";
                Qry = " TransOrder1.smid in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + smIDStr1 + ")) and Active=1)";
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select Sales Person');", true);
                rptPNotSold.DataSource = null;
                rptPNotSold.DataBind();
                return;
            }

            if (matGrpStrNew != "" && matProStrNew != "")
            {
                Qrychk = Qrychk + " and i.ItemId in (" + matProStrNew + ") ";
                QueryMatGrp = " and i.ItemId in (" + matProStrNew + ") ";
            }
            if (matGrpStrNew != "" && matProStrNew == "")
            {
                Qrychk = Qrychk + " and psg.ItemId in (" + matGrpStrNew + ") ";
                QueryMatGrp = "and psg.ItemId in (" + matGrpStrNew + ")";
            }
            //DateTime nD = Convert.ToDateTime(txtasondate.Text);
            //string from = nD.AddDays(-Convert.ToInt32(txt_noofdays.Text)).ToString("dd/MMM/yyyy");
            string query = @"SELECT  a.ItemId, a.ItemName AS Name, a.ProductGroup AS [Group],isnull(convert(varchar, max(os1.VDate), 103), 'NA') AS LastSoldOn FROM ( " +
           " SELECT distinct i.ItemId, i.ItemName,psg.ItemId AS ProductGroupId,psg.ItemName AS ProductGroup FROM mastitem i INNER JOIN mastitem psg ON psg.ItemId = i.Underid WHERE i.ItemId NOT IN " +
           " (SELECT distinct TransOrder1.ItemId FROM TransOrder1 WHERE TransOrder1.VDate BETWEEN '" + txtfromdate.Text + " 00:01' AND '" + txtasondate.Text + " 23:59'  and " + Qry + " ) " + QueryMatGrp + " ) a " +
           " left JOIN transorder1 Os1 ON Os1.ItemId = a.ItemId left JOIN mastitem i on i.ItemId=os1.ItemId INNER JOIN mastitem psg ON psg.ItemId =i.Underid where " + Qrychk + " GROUP BY a.ItemId, a.ItemName, a.ProductGroup Order by LastSoldOn desc,Name";

            DataTable dtItem = DbConnectionDAL.GetDataTable(CommandType.Text, query);
            if (dtItem.Rows.Count > 0)
            {
                rptmain.Style.Add("display", "block");
                rptPNotSold.DataSource = dtItem;
                rptPNotSold.DataBind();
                btnExport.Visible = true;
            }
            else
            {
                rptmain.Style.Add("display", "block");
                rptPNotSold.DataSource = dtItem;
                rptPNotSold.DataBind();
                btnExport.Visible = false;
            }
            dtItem.Dispose();


        }
        protected void Cancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ProductNotSold.aspx");
        }
        protected void matGrpListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string matGrpStr = "";
            //         string message = "";
            foreach (ListItem matGrp in matGrpListBox.Items)
            {
                if (matGrp.Selected)
                {
                    matGrpStr += matGrp.Value + ",";
                }
            }
            matGrpStr = matGrpStr.TrimStart(',').TrimEnd(',');

            if (matGrpStr != "")
            {
                string mastItemQry1 = @"select ItemId,ItemName from MastItem where Underid in (" + matGrpStr + ") and ItemType='ITEM' and Active=1 order by itemname";
                DataTable dtMastItem1 = DbConnectionDAL.GetDataTable(CommandType.Text, mastItemQry1);
                if (dtMastItem1.Rows.Count > 0)
                {
                    productListBox.DataSource = dtMastItem1;
                    productListBox.DataTextField = "ItemName";
                    productListBox.DataValueField = "ItemId";
                    productListBox.DataBind();
                }
                //     ddlProduct.Items.Insert(0, new ListItem("--Please select--"));
                dtMastItem1.Dispose();
            }
            else
            {
                ClearControls();
            }

        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("content-disposition", "attachment;filename=ProductNotSold.csv");
            string headertext = "Product".TrimStart('"').TrimEnd('"') + "," + "Product Group".TrimStart('"').TrimEnd('"') + "," + "Last Sold On".TrimStart('"').TrimEnd('"');

            StringBuilder sb = new StringBuilder();
            sb.Append(headertext);
            sb.AppendLine(System.Environment.NewLine);

            DataTable dtParams = new DataTable();
            dtParams.Columns.Add(new DataColumn("Name", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Group", typeof(String)));
            dtParams.Columns.Add(new DataColumn("LastSoldOn", typeof(String)));

            foreach (RepeaterItem item in rptPNotSold.Items)
            {
                DataRow dr = dtParams.NewRow();
                Label lblName = item.FindControl("lblName") as Label;
                dr["Name"] = lblName.Text.ToString().Replace("\n", "").Replace("\r", "").Replace(',', ' ');
                Label lblGroup = item.FindControl("lblGroup") as Label;
                dr["Group"] = lblGroup.Text.ToString().Replace("\n", "").Replace("\r", "").Replace(',', ' ');
                Label lblLSoldOn = item.FindControl("lblLSoldOn") as Label;
                dr["LastSoldOn"] = lblLSoldOn.Text.ToString();
                dtParams.Rows.Add(dr);
            }

            for (int j = 0; j < dtParams.Rows.Count; j++)
            {
                for (int k = 0; k < dtParams.Columns.Count; k++)
                {
                    if (dtParams.Rows[j][k].ToString().Contains(","))
                    {
                        string h = dtParams.Rows[j][k].ToString();
                        string d = h.Replace(",", " ");
                        dtParams.Rows[j][k] = "";
                        dtParams.Rows[j][k] = d;
                        dtParams.AcceptChanges();
                        if (k == 0)
                        {
                            //sb.Append(String.Format("\"{0}\"", Convert.ToDateTime(dtParams.Rows[j][k]).ToString("dd/MMM/yyyy")) + ',');
                            sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k]).ToString() + ',');
                        }
                        else
                        {
                            sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k].ToString()) + ',');
                        }
                    }
                    else if (dtParams.Rows[j][k].ToString().Contains(System.Environment.NewLine))
                    {
                        if (k == 0)
                        {
                            //sb.Append(String.Format("\"{0}\"", Convert.ToDateTime(dtParams.Rows[j][k]).ToString("dd/MMM/yyyy")) + ',');
                            sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k]).ToString() + ',');
                        }
                        else
                        {
                            sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k].ToString()) + ',');
                        }
                    }
                    else
                    {
                        if (k == 0)
                        {
                            //sb.Append(Convert.ToDateTime(dtParams.Rows[j][k]).ToString("dd/MMM/yyyy") + ',');
                            sb.Append(dtParams.Rows[j][k].ToString() + ',');
                        }
                        else
                        {
                            sb.Append(dtParams.Rows[j][k].ToString() + ',');
                        }
                    }
                }
                sb.Append(Environment.NewLine);
            }
            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("content-disposition", "attachment;filename=ProductNotSold.csv");
            Response.Write(sb.ToString());
            Response.End();

            sb.Clear();
            dtParams.Dispose();
        }
    }
}
