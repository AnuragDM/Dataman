using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessLayer;
using System.Web.Services;
using Newtonsoft.Json;
using System.IO;
namespace AstralFFMS
{
    public partial class ItempriceDistcityWise : System.Web.UI.Page
    {
        public bool flag = true;

        protected void Page_Load(object sender, EventArgs e)
        {
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;

            if (IsPostBack) return;
            BindState();
           // BindDistributors();
            //BindProductGroups();

        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            mainDiv.Style.Add("display", "block");
            rptmain.Style.Add("display", "none");
        }
        private void BindState()
        {
            string regQry = @"select * from MastArea where AreaType='State' and Active=1  order by AreaName";
            DataTable regiondt = DbConnectionDAL.GetDataTable(CommandType.Text, regQry);
            if (regiondt.Rows.Count > 0)
            {
                lstState.DataSource = regiondt;
                lstState.DataTextField = "AreaName";
                lstState.DataValueField = "AreaId";
                lstState.DataBind();


                ddlstate.DataSource = regiondt;
                ddlstate.DataTextField = "AreaName";
                ddlstate.DataValueField = "AreaId";
                ddlstate.DataBind();
                ddlstate.Items.Insert(0, new ListItem("--Select--", "0"));
            }
        }
        //private void BindDistributors()
        //{
        //    try
        //    {
        //        string str = "";
        //        str = "select PartyId,PartyName FROM MastParty WHERE PartyDist=1 AND Active=1 ORDER BY PartyName";
        //        fillDDLDirect(ddlDistributor, str, "PartyId", "PartyName");
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.ToString();
        //        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while binding the records');", true);
        //    }
        //}

        private void BindProductGroups()
        {
            try
            {
                string str = "";
                str = "SELECT ItemId,ItemName FROM MastItem WHERE ItemType='MaterialGroup' AND Active=1 ORDER BY ItemName";
                fillDDLDirect(ddlProductGroup, str, "ItemId", "ItemName");
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

        //[WebMethod(EnableSession = true)]
        //public static string getprice(string Distid, string productgroupid, string Todate)
        //{

        //    //string Qrychk = " t1.VDate>='" + Settings.dateformat(Fromdate) + " 00:00' and t1.VDate<='" + Settings.dateformat(Todate) + " 23:59'";
        //    //            string query = @"SELECT t1.VDate,da.PartyName AS Distributor,max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],sum(qty) as Qty,sum(Amount) as Amount from TransDistInv1 t1 left outer join MastItem mi
        //    //                           on mi.ItemId=t1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=t1.DistId where (" + Qrychk + ") group BY  t1.VDate,da.PartyName, mi.ItemName order BY  t1.VDate desc";
        //    string smIDStr1 = "";
        //    smIDStr1 = HttpContext.Current.Session["treenodes"].ToString();
        //    string Qrychk = " TransDistributerLedger.VDate<='" + Settings.dateformat(Todate) + " 23:59'";
        //    HttpContext.Current.Session["aaa"] = Fromdate;
        //    HttpContext.Current.Session["bbb"] = Todate;
        //    HttpContext.Current.Session["DistId"] = Distid;
        //    string query = @"select TransDistributerLedger.DistId, partyname as Distributor, sum(Amtdr) AS Debit, Sum(Amtcr) AS Credit,(sum(AmtDr)-sum(AmtCr))  as Closing from TransDistributerLedger left join MastParty on MastParty.PartyId=TransDistributerLedger.DistId WHERE TransDistributerLedger.DistId IN (" + Distid + ") AND mastparty.PartyDist=1 and " + Qrychk + " group by DistId,PartyName order by Partyname"; ;

        //    DataTable dtItem = DbConnectionDAL.GetDataTable(CommandType.Text, query);
        //    return JsonConvert.SerializeObject(dtItem);


        //}
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
            Response.Redirect("~/ItempriceDistcityWise.aspx");
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
            if (ddlDistributor.SelectedValue != "")
            {
                int did = Convert.ToInt32(ddlDistributor.SelectedValue);
                Div1.Style.Add("display", "none");

                ddlProductGroup.Items.Clear();

                if (did > 0)
                {
                    BindProductGroups();
                }
            }

        }

        protected void ddlProductGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            getPrice();
        }
        protected void btnFind_Click(object sender, EventArgs e)
        {
            //    fillRepeter();
            Repeater1.DataSource = null;
            Repeater1.DataBind();
        
            mainDiv.Style.Add("display", "none");
            rptmain.Style.Add("display", "block");
        }
        protected void btnCancel1_Click(object sender, EventArgs e)
        {
            ddlstate.SelectedIndex = 0;
            ddlCity.Items.Clear();
            ddlDist.Items.Clear();
            ddlpro.Items.Clear();
            Repeater1.DataSource = null;
            Repeater1.DataBind();
        }
        protected void btnGo_Click(object sender, EventArgs e)
        {
            DataTable dtSMId = Settings.UnderUsers(Settings.Instance.SMID);
  

            if (ddlDist.SelectedValue != "")
            {
                int did = Convert.ToInt32(ddlDist.SelectedValue);

                int pid = Convert.ToInt32(ddlpro.SelectedValue);

                if (did == 0)
                {
                    flag = false;
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please Select Distributor.');", true);
                    return;
                }

                if (pid == 0)
                {
                    flag = false;
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please Select Product Group.');", true);
                    return;
                }
                string str = "";

                //ISNULL(i.DistPrice,0.00) as DistPrice
                str = @"SELECT m.ItemId,Max(m.ItemName) AS ItemName,(Case when  Max(ISNULL(i.DistPrice,0.00))=0 then  Max(ISNULL(m.DP,0.00)) else Max(ISNULL(i.DistPrice,0.00)) end) as DistPrice FROM MastItem m" +
                         " LEFT JOIN " +
                         "MastItemPriceDistWise i on m.ItemId = i.ItemId and  i.DistId=" + did + "" +
                         " AND i.ProdGrpId= " + pid +
                        " where m.ItemType='ITEM' AND m.Active=1 AND m.Underid = " + pid + "  GROUP BY m.ItemId ORDER BY Max(m.ItemName)";

                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);



                if (dt.Rows.Count == 0)
                {
                    // flag = false;
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('No Records Found.');", true);
                    return;
                }
                // btnEdit.Visible = true;
                Repeater1.DataSource = dt;
                Repeater1.DataBind();
            }
            else
            {
                flag = false;
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please Select Distributor.');", true);
                return;
            }
        }
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                string insertsql = "", result = "", updatesql = "";
                int did = Convert.ToInt32(ddlDistributor.SelectedValue);
                int pid = Convert.ToInt32(ddlProductGroup.SelectedValue);
                int uid = Convert.ToInt32(Settings.Instance.UserID);

                foreach (RepeaterItem item in rpt.Items)
                {
                    HiddenField hdField = (HiddenField)item.FindControl("HiddenField1");
                    HtmlInputText txtName = (HtmlInputText)item.FindControl("distPrice");

                    

                    foreach (ListItem item1 in ddlDistributor.Items)
                    {
                        result = DbConnectionDAL.GetStringScalarVal("select count(*) from [dbo].[MastItemPriceDistWise] where DistId = " + item1.Value + " and  ItemId = " + Convert.ToInt32(hdField.Value) + " and ProdGrpId = " + pid + "");
                        if (item1.Selected)
                        {
                            if (Convert.ToInt32(result) == 0)
                            //if (Convert.ToInt32(result) == 0)
                            {
                                insertsql = "INSERT INTO [dbo].[MastItemPriceDistWise] ([DistId],[ItemId],[DistPrice],[CreatedByUserId],[CreateDate],[ProdGrpId],[LastModifiedDate])" +
                                                        " VALUES(" + item1.Value + "," + Convert.ToInt32(hdField.Value) + "," + Convert.ToDouble(txtName.Value) + "," + uid + ",DateAdd(minute,330,getutcdate())," + pid + ",DateAdd(minute,330,getutcdate()))";
                                DbConnectionDAL.ExecuteQuery(insertsql);
                            }
                            else if (Convert.ToInt32(result) == 1)
                            {
                                updatesql = "update [dbo].[MastItemPriceDistWise] set DistPrice= " + Convert.ToDouble(txtName.Value) + ", LastModifiedDate = DateAdd(minute, 330, getutcdate()) where DistId = " + item1.Value + " and  ItemId = " + Convert.ToInt32(hdField.Value) + " and ProdGrpId = " + pid + "";
                                DbConnectionDAL.ExecuteQuery(updatesql);
                            }

                        }
                    }
                    updatesql = "update [dbo].[MastItem] set CreatedDate = DateAdd(minute, 330, getutcdate()) where ItemId = " + Convert.ToInt32(hdField.Value) + " and Underid = " + pid + "";
                    DbConnectionDAL.ExecuteQuery(updatesql);

                }
                //ShowAlert("Records Updated Successfully.");
                //System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Alert", "Alert('Records Updated Successfully.');", true);

                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Errormessage", "geek();", true);
                //Response.Redirect("~/ItemPriceDistWise.aspx");

                HtmlMeta meta = new HtmlMeta();
                meta.HttpEquiv = "Refresh";
                meta.Content = "2;url=ItempriceDistcityWise.aspx";
                this.Page.Controls.Add(meta);

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
                int i = 0;
                flag = true;
                int did = Convert.ToInt32(ddlDistributor.SelectedValue);
                string strdistID = "";
                foreach (ListItem item in ddlDistributor.Items)
                {
                    if (item.Selected)
                    {
                        strdistID += item.Value + ",";
                        i = i + 1;
                    }
                }
                strdistID = strdistID.TrimStart(',').TrimEnd(',');
                int pid = Convert.ToInt32(ddlProductGroup.SelectedValue);

                if (did == 0)
                {
                    flag = false;
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please Select Distributor.');", true);
                    return;
                }

                if (pid == 0)
                {
                    flag = false;
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please Select Product Group.');", true);
                    return;
                }
                string str = "";
                if(i==1)
                //ISNULL(i.DistPrice,0.00) as DistPrice
                 str = @"SELECT m.ItemId,Max(m.ItemName) AS ItemName,(Case when  Max(ISNULL(i.DistPrice,0.00))=0 then  Max(ISNULL(m.DP,0.00)) else Max(ISNULL(i.DistPrice,0.00)) end) as DistPrice FROM MastItem m" +
                          " LEFT JOIN " +
                          "MastItemPriceDistWise i on m.ItemId = i.ItemId and  i.DistId in(" + strdistID + ")" +
                          " AND i.ProdGrpId= " + pid +
                         " where m.ItemType='ITEM' AND m.Active=1 AND m.Underid = " + pid + "  GROUP BY m.ItemId ORDER BY Max(m.ItemName)";
                else
                    str = @"SELECT m.ItemId,Max(m.ItemName) AS ItemName,  Max(ISNULL(m.DP,0.00))  as DistPrice FROM MastItem m" +
                      " LEFT JOIN " +
                      "MastItemPriceDistWise i on m.ItemId = i.ItemId and  i.DistId in(" + strdistID + ")" +
                      " AND i.ProdGrpId= " + pid +
                     " where m.ItemType='ITEM' AND m.Active=1 AND m.Underid = " + pid + "  GROUP BY m.ItemId ORDER BY Max(m.ItemName)";
                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);

               

                if (dt.Rows.Count == 0)
                {
                    flag = false;
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('No Records Found.');", true);
                    return;
                }
                btnEdit.Visible = true;
                rpt.DataSource = dt;
                rpt.DataBind();
            }
            catch (Exception ex)
            {
                ex.ToString();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while getting the records');", true);
            }
        }

        protected void lstCity_SelectedIndexChanged(object sender,EventArgs e)
        {
            string strcityID = "";

         //   System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "onchangecity", "onchangecity();", true);
            foreach (ListItem item in lstCity.Items)
            {
                if (item.Selected)
                {
                    strcityID += item.Value + ",";
                }
            }
            strcityID = strcityID.TrimStart(',').TrimEnd(',');

            if (strcityID != "")
            {
                string Qry = @"select PartyId,PartyName FROM MastParty WHERE PartyDist=1 AND Active=1  and cityid in (" + strcityID + ") ORDER BY PartyName";
                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, Qry);
                if (dt.Rows.Count > 0)
                {
                    ddlDistributor.DataSource = dt;
                    ddlDistributor.DataTextField = "PartyName";
                    ddlDistributor.DataValueField = "PartyId";
                    ddlDistributor.DataBind();
                }

            }
            else
            {
                ddlDistributor.Items.Clear();
                rpt.DataSource = null;
                rpt.DataBind();

            }
        }

        protected void lstState_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strStID = "";
            foreach (ListItem item in lstState.Items)
            {
                if (item.Selected)
                {
                    strStID += item.Value + ",";
                }
            }
            strStID = strStID.TrimStart(',').TrimEnd(',');

            if (strStID != "")
            {
                string Qry = @"select * from MastArea where AreaType='City' and Active=1 and UnderId IN (select AreaId from MastArea where AreaType='District' and Active=1 and UnderId IN (" + strStID + ")) order by AreaName";
                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, Qry);
                if (dt.Rows.Count > 0)
                {
                    lstCity.DataSource = dt;
                    lstCity.DataTextField = "AreaName";
                    lstCity.DataValueField = "AreaId";
                    lstCity.DataBind();
                }

            }
            else
            {
                lstCity.Items.Clear();
                ddlDistributor.Items.Clear();
                ddlProductGroup.Items.Clear();
                btnEdit.Visible = false;
                rpt.DataSource = null;
                rpt.DataBind();

            }
        }


        protected void ddlCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strcityID = "";

            //   System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "onchangecity", "onchangecity();", true);
            foreach (ListItem item in ddlCity.Items)
            {
                if (item.Selected)
                {
                    strcityID += item.Value + ",";
                }
            }
            strcityID = strcityID.TrimStart(',').TrimEnd(',');

            if (strcityID != "")
            {
                string Qry = @"select PartyId,PartyName FROM MastParty WHERE PartyDist=1 AND Active=1  and cityid in (" + strcityID + ") ORDER BY PartyName";
                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, Qry);
                if (dt.Rows.Count > 0)
                {
                    ddlDist.DataSource = dt;
                    ddlDist.DataTextField = "PartyName";
                    ddlDist.DataValueField = "PartyId";
                    ddlDist.DataBind();
                    ddlDist.Items.Insert(0, new ListItem("--Select--", "0"));
                }

            }
            else
            {
                ddlDist.Items.Clear();
                Repeater1.DataSource = null;
                Repeater1.DataBind();

            }
        }
        protected void ddlstate_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strStID = "";
            foreach (ListItem item in ddlstate.Items)
            {
                if (item.Selected)
                {
                    strStID += item.Value + ",";
                }
            }
            strStID = strStID.TrimStart(',').TrimEnd(',');

            if (strStID != "")
            {
                string Qry = @"select * from MastArea where AreaType='City' and Active=1 and UnderId IN (select AreaId from MastArea where AreaType='District' and Active=1 and UnderId IN (" + strStID + ")) order by AreaName";
                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, Qry);
                if (dt.Rows.Count > 0)
                {
                    ddlCity.DataSource = dt;
                    ddlCity.DataTextField = "AreaName";
                    ddlCity.DataValueField = "AreaId";
                    ddlCity.DataBind();
                    ddlCity.Items.Insert(0, new ListItem("--Select--", "0"));
                }

            }
            else
            {
                ddlCity.Items.Clear();
                ddlDist.Items.Clear();
                ddlpro.Items.Clear();
               // btnEdit.Visible = false;
                Repeater1.DataSource = null;
                Repeater1.DataBind();

            }
        }
        protected void ddlDist_SelectedIndexChanged(object sender, EventArgs e)
        {
            Repeater1.DataSource = null;
            Repeater1.DataBind();
            if (ddlDist.SelectedValue != "")
            {
                int did = Convert.ToInt32(ddlDist.SelectedValue);
                Div1.Style.Add("display", "none");

                ddlpro.Items.Clear();

                if (did > 0)
                {
                    try
                    {
                        string str = "";
                        str = "SELECT ItemId,ItemName FROM MastItem WHERE ItemType='MaterialGroup' AND Active=1 ORDER BY ItemName";
                        fillDDLDirect(ddlpro, str, "ItemId", "ItemName");
                    }
                    catch (Exception ex)
                    {
                        ex.ToString();
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while binding the records');", true);
                    }
                }
            }

        }

    }
}