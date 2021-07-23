using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using System.Data;
using System.Web.UI.HtmlControls;
using BAL;
using System.Reflection;
using BusinessLayer;
using System.IO;

namespace AstralFFMS
{
    public partial class ItemPriceDistWise : System.Web.UI.Page
    {
        public bool flag = true;
        protected void Page_Load(object sender, EventArgs e)
        {
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;

            if (IsPostBack) return;

            BindDistributors();
            //BindProductGroups();
            
        }

        private void BindDistributors()
        {
            try
            {
                string str = "";
                str = "select PartyId,PartyName FROM MastParty WHERE PartyDist=1 AND Active=1 ORDER BY PartyName"; 
                fillDDLDirect(ddlDistributor, str, "PartyId", "PartyName");
            }
            catch (Exception ex)
            {
                ex.ToString();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while binding the records');", true);
            }
        }

        private void BindProductGroups()
        {
            try
            {
                string str = "";
                str = "SELECT ItemId,ItemName FROM MastItem WHERE ItemType='MaterialGroup' AND Active=1 ORDER BY ItemName";
                //fillDDLDirect(ddlProductGroup, str, "ItemId", "ItemName");
                DataTable dtProdRep = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                if (dtProdRep.Rows.Count > 0)
                {

                    ddlProductGroup.DataSource = dtProdRep;
                    ddlProductGroup.DataTextField = "ItemName";
                    ddlProductGroup.DataValueField = "ItemId";
                    ddlProductGroup.DataBind();

                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while binding the records');", true);
            }
        }

        public static void fillDDLDirect(DropDownList xddl, string xmQry, string xvalue, string xtext)
        {
            DataTable xdt = new DataTable();
            xdt = DbConnectionDAL.GetDataTable(CommandType.Text, xmQry);
            xddl.DataSource = xdt;
            xddl.DataTextField = xtext.Trim();
            xddl.DataValueField = xvalue.Trim();
            xddl.DataBind();
            if (xdt.Rows.Count >= 1)
                xddl.Items.Insert(0, new ListItem("--Select--", "0"));
            else if (xdt.Rows.Count == 0)
                 xddl.Items.Insert(0, new ListItem("---", "0"));
            xdt.Dispose();
        }


        //protected void btnGetPrice_Click(object sender, EventArgs e)
        private void getPrice()
        {
            try
            {
                Div1.Style.Add("display", "none");
                fillRepeter();
                if (flag == true)
                {
                    Div1.Style.Add("display", "block");
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while getting the records');", true);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ItemPriceDistWise.aspx");
        }

        //protected void distPrice_TextChanged(object sender, EventArgs e)
        //{
        //    TextBox txt = (TextBox)sender;
        //    if(txt.Text == "")
        //    {
        //        //System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Field cannot be blank.');", true);
        //        txt.Text = "0.00";
        //    }
            
           

        //}


        protected void ddlDistributor_SelectedIndexChanged(object sender, EventArgs e)
        {
            int did = Convert.ToInt32(ddlDistributor.SelectedValue);
            Div1.Style.Add("display", "none");

            ddlProductGroup.Items.Clear();

            if(did>0)
            {
                BindProductGroups();
            }
            
        }

        protected void ddlProductGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            getPrice();
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                string insertsql = "", result = "", updatesql = ""; int pid = 0;
                int did = Convert.ToInt32(ddlDistributor.SelectedValue);
                //int pid = Convert.ToInt32(ddlProductGroup.SelectedValue);
                int uid = Convert.ToInt32(Settings.Instance.UserID);

                foreach (RepeaterItem item in rpt.Items)
                {
                    HiddenField hdField = (HiddenField)item.FindControl("HiddenField1");
                    HtmlInputText txtName = (HtmlInputText)item.FindControl("distPrice");
                    Label lblProductgroupId = (Label)item.FindControl("lblProductgroupId");
                    pid = Convert.ToInt32(lblProductgroupId.Text);

                    result = DbConnectionDAL.GetStringScalarVal("select count(*) from [dbo].[MastItemPriceDistWise] where DistId = " + did + " and  ItemId = " + Convert.ToInt32(hdField.Value) + " and ProdGrpId = " + pid + "");


                    if (Convert.ToInt32(result) == 0)
                    //if (Convert.ToInt32(result) == 0)
                    {
                        insertsql = "INSERT INTO [dbo].[MastItemPriceDistWise] ([DistId],[ItemId],[DistPrice],[CreatedByUserId],[CreateDate],[ProdGrpId],[LastModifiedDate])" +
                                                " VALUES(" + did + "," + Convert.ToInt32(hdField.Value) + "," + Convert.ToDouble(txtName.Value) + "," + uid + ",DateAdd(minute,330,getutcdate())," + pid + ",DateAdd(minute,330,getutcdate()))";
                        DbConnectionDAL.ExecuteQuery(insertsql);
                    }
                    else if (Convert.ToInt32(result) == 1)
                    {
                        updatesql = "update [dbo].[MastItemPriceDistWise] set DistPrice= " + Convert.ToDouble(txtName.Value) + ", LastModifiedDate = DateAdd(minute, 330, getutcdate()) where DistId = " + did + " and  ItemId = " + Convert.ToInt32(hdField.Value) + " and ProdGrpId = " + pid + "";
                        DbConnectionDAL.ExecuteQuery(updatesql);
                    }

                    updatesql = "update [dbo].[MastItem] set CreatedDate = DateAdd(minute, 330, getutcdate()) where ItemId = " + Convert.ToInt32(hdField.Value) + " and Underid = " + pid + "";
                    DbConnectionDAL.ExecuteQuery(updatesql);

                }
                //ShowAlert("Records Updated Successfully.");
                //System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Alert", "Alert('Records Updated Successfully.');", true);
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Errormessage", "geek();", true);
                HtmlMeta meta = new HtmlMeta();
                meta.HttpEquiv = "Refresh";
                meta.Content = "2;url=ItemPriceDistWise.aspx";
                this.Page.Controls.Add(meta);
                //Response.Redirect("~/ItemPriceDistWise.aspx");
               
            }
            catch (Exception ex)
            {
                ex.ToString();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while inserting the records');", true);
            }
        }

        //public void ShowAlert(string Message)
        //{
        //    string script = "window.alert(\"" + Message.Normalize() + "\");";
        //    ScriptManager.RegisterClientScriptBlock(this, GetType(), "UniqueKey", script, true);

            
        //}
      
        private void fillRepeter()
        {
            try
            { 
                flag = true;
                int did = Convert.ToInt32(ddlDistributor.SelectedValue);
              //  int pid = Convert.ToInt32(ddlProductGroup.SelectedValue);
                string ProductGroup = string.Empty;

                foreach (ListItem matGrpItems in ddlProductGroup.Items)
                {
                    if (matGrpItems.Selected)
                    {
                        ProductGroup += matGrpItems.Value + ",";
                    }
                }
                ProductGroup = ProductGroup.TrimStart(',').TrimEnd(',');


                if (did == 0)
                {
                    flag = false;
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please Select Distributor.');", true);
                    return;
                }

                //if (pid == 0)
                //{
                //    flag = false;
                //    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please Select Product Group.');", true);
                //    return;
                //}
                if (ProductGroup == "")
                {
                    flag = false;
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please Select Product Group.');", true);
                    return;
                }

                //ISNULL(i.DistPrice,0.00) as DistPrice
                //string str = @"SELECT m.ItemId,m.ItemName, ISNULL(i.DistPrice,0.00) as DistPrice FROM MastItem m" + 
                //          " LEFT JOIN " +
                //          "MastItemPriceDistWise i on m.ItemId = i.ItemId and  i.DistId = " + did +
                //          " AND i.ProdGrpId= " + pid +
                //         " where m.ItemType='ITEM' AND m.Active=1 AND m.Underid = " + pid + " ORDER BY m.ItemName";

                string str = @"SELECT m.ItemId,m.ItemName,m1.ItemName as ProductGroup,m1.ItemId as ProductgroupId,ISNULL(m.DP,0.00) as ItemDP,ISNULL(i.DistPrice,0.00) as DistPrice FROM MastItem m" +
                         " LEFT JOIN " +
                         " MastItemPriceDistWise i on m.ItemId = i.ItemId and  i.DistId = " + did +
                         " AND i.ProdGrpId in ( " + ProductGroup + ") " +
                         " Left Join MastItem m1 on m.underid=m1.Itemid where m.ItemType='ITEM' AND m.Active=1 AND m.Underid in (" + ProductGroup + " ) ORDER BY m.ItemName";

                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);

               

                if(dt.Rows.Count == 0)
                {
                    flag = false;
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('No Records Found.');", true);
                    return;
                }
                rpt.DataSource = dt;
                rpt.DataBind();
            }
            catch (Exception ex)
            {
                ex.ToString();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while getting the records');", true);
            }
        }
        protected void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox btn = (CheckBox)sender;
            var item = (RepeaterItem)btn.NamingContainer;
            CheckBox chkAllBox = (CheckBox)item.FindControl("chkAll");
            for (int i = 0; i < rpt.Items.Count; i++)
            {
                CheckBox chk = (CheckBox)rpt.Items[i].FindControl("chkItem");
                //Label lblItemPrice = (Label)rpt.Items[i].FindControl("lblItemPrice");
                //HtmlInputText txtName = (HtmlInputText)rpt.Items[i].FindControl("distPrice");
                //txtName.Value = lblItemPrice.Text;              
                if (chkAllBox.Checked)
                {
                    chk.Checked = true;
                    Label lblItemPrice = (Label)rpt.Items[i].FindControl("lblItemPrice");
                    HtmlInputText txtName = (HtmlInputText)rpt.Items[i].FindControl("distPrice");
                    txtName.Value = lblItemPrice.Text;
              
                }
                else
                {
                    chk.Checked = false;
                    getPrice();
                }
            }
        }

        protected void chkItem_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox btn = (CheckBox)sender;
            var item = (RepeaterItem)btn.NamingContainer;
            CheckBox chkItem = (CheckBox)item.FindControl("chkItem");   
        
            if (chkItem.Checked)
            {
                Label lblItemPrice = (Label)item.FindControl("lblItemPrice");
                HtmlInputText txtName = (HtmlInputText)item.FindControl("distPrice");
                txtName.Value = lblItemPrice.Text;
            }
            else
            {
                getPrice();
            }
        }

    }
}