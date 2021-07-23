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
using DAL;
using System.Web.Services;
using System.IO;
namespace AstralFFMS
{
    public partial class DItemStock : System.Web.UI.Page
    {
        string parameter1 = "";
        int DistId = 0, AreaId = 0,SMId=0;
        BAL.DistributorBAL db = new DistributorBAL();
        String Level = "0";
        string pageSalesName = "", discount = "", strItem = "", strDItem = "", loseQty = "", stockQty = "", bUF = "", Rate = "", cQty = "", unit = "", roleType = "", parameter = "", VisitID = "", CityID = "";
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Request.QueryString["DistId"] != null)
            {
                DistId = Convert.ToInt32(Request.QueryString["DistId"].ToString());
            }
            if (Request.QueryString["VisitID"] != null)
            {
                VisitID = Request.QueryString["VisitID"].ToString();
            }
            if (Request.QueryString["CityID"] != null)
            {
                CityID = Request.QueryString["CityID"].ToString();
            }
            if (Request.QueryString["AreaId"] != null)
            {
                AreaId = Convert.ToInt32(Request.QueryString["AreaId"].ToString());
            }
            if (Request.QueryString["Level"] != null)
            {
                Level = Request.QueryString["Level"].ToString();
            }
            //Added
            if (Request.QueryString["PageView"] != null)
            {
                pageSalesName = Request.QueryString["PageView"].ToString();
            }
            //DistId = Convert.ToInt32(Settings.Instance.DistributorID);
          

            //string pageName = Path.GetFileName(Request.Path);
            //string PermAll = Settings.Instance.CheckPagePermissions(pageName, Convert.ToString(Session["user_name"]));
            //string[] SplitPerm = PermAll.Split(',');
            //if (btnadd.Text == "Save")
            //{
            //    btnadd.Enabled = Convert.ToBoolean(SplitPerm[1]);
            //    //btnSave.Enabled = Settings.Instance.CheckAddPermission(pageName, Convert.ToString(Session["user_name"]));
            //    btnadd.CssClass = "btn btn-primary";
            //}
            //else
            //{
            //    btnadd.Enabled = Convert.ToBoolean(SplitPerm[2]);
            //    //btnSave.Enabled = Settings.Instance.CheckEditPermission(pageName, Convert.ToString(Session["user_name"]));
            //    btnadd.CssClass = "btn btn-primary";
            //}
           
            if (!IsPostBack)
            {
                if (VisitID == "" || VisitID == null)
                {
                    txtmDate.Text = DateTime.Parse(DateTime.Now.ToUniversalTime().AddSeconds(19800).ToShortDateString()).ToString("dd/MMM/yyyy");
                }
                else
                { txtmDate.Text = GetVisitDate(Convert.ToInt32(VisitID)); }
                this.FillItem();
                this.FillUnderItem();
                roleType = Settings.Instance.RoleType;
                BindDistributorDDl(DistId);
                //mainDiv.Style.Add("display", "block");
                //rptmain.Style.Add("display", "none");
            }
            parameter1 = Request["__EVENTARGUMENT"];
            if (parameter1 != null)
            {
                if (parameter1 != "")
                {
                    ViewState["STKId"] = parameter1;
                    fillControl(Convert.ToInt32(parameter1));
                    //mainDiv.Style.Add("display", "block");
                    //rptmain.Style.Add("display", "none");
                    //btnDelete.Enabled = Convert.ToBoolean(SplitPerm[3]);
                    btnDelete.Visible = true;
                    btnadd.Text = "Update";
                    btnadd.CssClass = "btn btn-primary";
                    btnDelete.CssClass = "btn btn-primary";

                }
            }
         }
        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> SearchItem(string prefixText)
        {//Ankita - 12/may/2016- (For Optimization)
            //string str = "select * FROM MastItem where (ItemName like '%" + prefixText + "%' or SyncId like '%" + prefixText + "%' or ItemCode like '%" + prefixText + "%') and ItemType='Item' and Active=1";
            string str = "select SyncId,ItemName,ItemCode,ItemId FROM MastItem where (ItemName like '%" + prefixText + "%' or SyncId like '%" + prefixText + "%' or ItemCode like '%" + prefixText + "%') and ItemType='Item' and Active=1";
            DataTable dt = new DataTable();

            dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            List<string> customers = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem("(" + dt.Rows[i]["SyncId"].ToString() + ")" + " " + dt.Rows[i]["ItemName"].ToString() + " " + "(" + dt.Rows[i]["ItemCode"].ToString() + ")", dt.Rows[i]["ItemId"].ToString());
                customers.Add(item);
                //customers.Add("(" + dt.Rows[i]["SyncId"].ToString() + ")" + " " + dt.Rows[i]["ItemName"].ToString() + " " + "(" + dt.Rows[i]["ItemCode"].ToString() + ")");
            }
            return customers;
        }

        private void FillItem()
        {
            string strParent = "";
            strParent = "select  mi.itemname,mt.* from MastDistItemTemplate mt,MastItem mi where mi.ItemId=mt.ItemId and  Distid=" + DistId;
            fillDDLDirect(ddlItem, strParent, "ItemId", "ItemName", 1);
        }
        private void FillUnderItem()
        {
            string strParent = "";
            strParent = "select  mi.itemname,mt.* from MastDistItemTemplate mt,MastItem mi where mi.ItemId=mt.ItemId and  Distid=" + DistId;
            fillDDLDirect(ddlUnderItem, strParent, "ItemId", "ItemName", 1);
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

        private void fillControl(int STKId)
        {
            string distqry = "";
            distqry = @"select mt.STKId,mi.ItemId ,mi.itemname,mi.MRP,mi.ItemCode,mi.StdPack as UnitFactor,mt.VDate,mt.Qty as StockQty,mt.unit as unit,mt.cases as cases,mt.Qty % mi.StdPack as LooseQty,cast(((mt.Qty -(mt.Qty % mi.StdPack))/mi.StdPack) as decimal(18,2)) as CaseQty	from Temp_TransDistStock mt,MastItem mi where mi.ItemId=mt.ItemId  and  STKId=" + STKId;
            DataTable dtDist = DbConnectionDAL.GetDataTable(CommandType.Text, distqry);
          
            this.FillItem();
            ddlItem.SelectedValue = dtDist.Rows[0]["ItemId"].ToString();
            txtCase.Text = dtDist.Rows[0]["CaseQty"].ToString();
            txtUnit.Text = dtDist.Rows[0]["LooseQty"].ToString();
            ddlItem.Enabled = false;
            string text = "Price :" + dtDist.Rows[0]["MRP"].ToString() + " , Availability :[C : " + dtDist.Rows[0]["CaseQty"].ToString() + ", U : " + dtDist.Rows[0]["LooseQty"].ToString() + "]  ";
            lblAvailability.Text = text;

        }
        private void BindDistributorDDl(int DistId)
        {
            string distqry="";
            try
            {               
                {
                    distqry = @"Select Count(*) From Temp_TransDistStock mt where mt.VDate='" + Settings.dateformat(txtmDate.Text) + "' and  mt.Distid=" + DistId;
                    int count = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, distqry));
                    if (count > 0)
                    {
                        if (Settings.DMInt32(ddlUnderItem.SelectedValue) > 0)
                            //distqry = @"select Isnull(mt.STKId,0) as STKId ,mi.ItemId,mi.itemname,mi.MRP,mi.ItemCode,mi.StdPack as UnitFactor,mt.VDate,isnull(mt.Qty,0) as StockQty,case isnull(mt.Qty,0) when 0 then 0 else mt.Qty % mi.StdPack end as LooseQty,case isnull(mt.Qty,0) when 0 then 0 else cast(((mt.Qty -(mt.Qty % mi.StdPack))/mi.StdPack) as decimal(18,2)) end as CaseQty,case isnull(mt.Qty,0) when 0 then 0 else cast((mt.Qty * mi.MRP) as decimal(18,2)) end as Amount from Temp_TransDistStock mt right join MastDistItemTemplate dit on mt.ItemId=dit.ItemId inner join MastItem mi on dit.ItemId=mi.ItemId Where mt.VDate='" + Settings.dateformat(txtmDate.Text) + "' And mt.ItemId=" + Settings.DMInt32(ddlUnderItem.SelectedValue) + " and  mt.Distid=" + DistId;
                            distqry = @"select distinct ItemId,itemname ,STKId,MRP,ItemCode, UnitFactor,
                                       VDate,StockQty, cases unit  from 
                                        (select distinct Isnull(mt.STKId,0) as STKId,mi.ItemId,mi.itemname,mi.MRP,mi.ItemCode,mi.StdPack as UnitFactor,mt.VDate,isnull(mt.Qty,0) as StockQty,mt.cases,mt.unit from Temp_TransDistStock mt right join MastDistItemTemplate dit on mt.ItemId=dit.ItemId inner join MastItem mi on dit.ItemId=mi.ItemId Where mt.VDate='" + Settings.dateformat(txtmDate.Text) + "' And mt.ItemId=" + Settings.DMInt32(ddlUnderItem.SelectedValue) + " and  mt.Distid=" + DistId + " union select 0 as STKId,mi.ItemId,mi.itemname,mi.MRP,mi.ItemCode,mi.StdPack as UnitFactor,Getdate() as VDate,0 as StockQty,0 as Cases,0 as  unit from MastDistItemTemplate dit inner join MastItem mi on dit.ItemId=mi.ItemId Where dit.Distid=" + DistId + "  and mi.itemid not in(select mi.ItemId from Temp_TransDistStock mt right join MastDistItemTemplate dit on mt.ItemId=dit.ItemId inner join MastItem mi on dit.ItemId=mi.ItemId where mt.VDate='" + Settings.dateformat(txtmDate.Text) + "' and  mt.Distid=" + Settings.DMInt32(ddlUnderItem.SelectedValue) + ") a";

                        else distqry = @"select distinct ItemId,itemname ,STKId,MRP,ItemCode, UnitFactor,
                                       VDate,StockQty, cases, unit  from  
                                      (select distinct Isnull(mt.STKId,0) as STKId,mi.ItemId,mi.itemname,mi.MRP,mi.ItemCode,mi.StdPack as UnitFactor,mt.VDate,isnull(mt.Qty,0) as StockQty,mt.cases,mt.unit from Temp_TransDistStock mt right join MastDistItemTemplate dit on mt.ItemId=dit.ItemId inner join MastItem mi on dit.ItemId=mi.ItemId where mt.VDate='" + Settings.dateformat(txtmDate.Text) + "' and  mt.Distid=" + DistId + " union select 0 as STKId,mi.ItemId,mi.itemname,mi.MRP,mi.ItemCode,mi.StdPack as UnitFactor,Getdate() as VDate,0 as StockQty,0 as Cases,0 as  unit from MastDistItemTemplate dit inner join MastItem mi on dit.ItemId=mi.ItemId Where dit.Distid=" + DistId + "  and mi.itemid not in(select mi.ItemId from Temp_TransDistStock mt right join MastDistItemTemplate dit on mt.ItemId=dit.ItemId inner join MastItem mi on dit.ItemId=mi.ItemId where mt.VDate='" + Settings.dateformat(txtmDate.Text) + "' and  mt.Distid=" + DistId + ") ) a";
                    }
                    else {
                        distqry = @"select 0 as STKId,mi.ItemId,mi.itemname,mi.MRP,mi.ItemCode,mi.StdPack as UnitFactor,Getdate() as VDate,0 as StockQty,0 as Cases,0 as  unit from MastDistItemTemplate dit inner join MastItem mi on dit.ItemId=mi.ItemId Where dit.Distid=" + DistId;
                    }
                    DataTable dtDist = DbConnectionDAL.GetDataTable(CommandType.Text, distqry);
                    if (dtDist.Rows.Count > 0)
                    {
                        itemforparty.DataSource = dtDist;
                        itemforparty.DataBind();
                      
                    }
                    else
                    {
                        itemforparty.DataSource = null;
                        itemforparty.DataBind();
                    }
                } 
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        protected void txtquantity_TextChanged(object sender, EventArgs e)
        {
            TextBox txtquantity = sender as TextBox;
            GridViewRow row = (GridViewRow)txtquantity.Parent.Parent;
            TextBox txtQty = (row.FindControl("txtquantity") as TextBox);
            TextBox txtLooseQty = (row.FindControl("txtLooseQty") as TextBox);
            Label lblStockQty = (row.FindControl("lblStockQty") as Label);
            Label lblrate = (row.FindControl("lblrate") as Label);
            Label lblstdpack = (row.FindControl("lblstdpack") as Label);
            double CaseQty = double.Parse(txtQty.Text, System.Globalization.CultureInfo.InvariantCulture);
            double LooseQty = double.Parse(txtLooseQty.Text, System.Globalization.CultureInfo.InvariantCulture);
            double Rate = double.Parse(lblrate.Text, System.Globalization.CultureInfo.InvariantCulture);
            double StdPack = double.Parse(lblstdpack.Text, System.Globalization.CultureInfo.InvariantCulture);
            double stockQtyAmount = ((CaseQty * StdPack) + LooseQty);
            //double q3 = (q * q1) + 0.00;
            //Label lblamt = (row.FindControl("lblamount") as Label);
            //lblamt.Text = Convert.ToString(stockQtyAmount);
            lblStockQty.Text = Convert.ToString(stockQtyAmount);
        }

        protected void txtCaseQty_TextChanged(object sender, EventArgs e)
        {
            TextBox txtLooseQty = sender as TextBox;
            GridViewRow row = (GridViewRow)txtLooseQty.Parent.Parent;
            TextBox txtQty = (row.FindControl("txtquantity") as TextBox);
            TextBox txtLQty = (row.FindControl("txtLooseQty") as TextBox);
            Label lblrate = (row.FindControl("lblrate") as Label);
            Label lblStockQty = (row.FindControl("lblStockQty") as Label);
            Label lblstdpack = (row.FindControl("lblstdpack") as Label);
            double CaseQty = double.Parse(txtQty.Text, System.Globalization.CultureInfo.InvariantCulture);
            double LooseQty = double.Parse(txtLooseQty.Text, System.Globalization.CultureInfo.InvariantCulture);
            double Rate = double.Parse(lblrate.Text, System.Globalization.CultureInfo.InvariantCulture);
            double StdPack = double.Parse(lblstdpack.Text, System.Globalization.CultureInfo.InvariantCulture);
            double stockQtyAmount = ((CaseQty * StdPack) + LooseQty);
            //double q3 = (q * q1) + 0.00;
            //Label lblamt = (row.FindControl("lblamount") as Label);
            lblStockQty.Text = Convert.ToString(stockQtyAmount);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearControls();
            Response.Redirect("~/DistributorItemStock.aspx");
        }
        private void ClearControls()
        {
           
            ddlItem.SelectedIndex = 0;
            txtCase.Text = "";
            txtUnit.Text = "";
            lblAvailability.Text = "";
        }
        protected void LinkButton1_Command(object sender, CommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument.ToString());
            string distqry = @"delete from Temp_TransDistStock where  DistItemId=" + id;
           if(( DbConnectionDAL.ExecuteQuery(distqry))==1)
           {
               distqry = @"update Temp_TransDistStock set createddate=getdate() where DistId=" + DistId + "";
               DbConnectionDAL.ExecuteQuery(distqry);
               System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Deleted Successfully');", true);
           }
           else
           {
               System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Failed To Delete Item');", true);
               return;
           }
           BindDistributorDDl(DistId);
           clear();
        }

        protected void btnadd_Click(object sender, EventArgs e)
        {
            string qry = "";
            decimal totalQty = 0;
            string SMId = "";
            string SyncId = "";
            string AreaId = "";
            string DistCode = "";
            int retsave = 0;
            string itemBUF = "";
            string mszforatleastoneitem = "";
            strItem = string.Format("Delete  from Temp_TransDistStock where DistId={0} And VDate='{1}'", DistId, Settings.dateformat(txtmDate.Text));
            DbConnectionDAL.ExecuteNonQuery(CommandType.Text, strItem);

            string docID = Settings.GetDocID("DIS", DateTime.Now);
            Settings.SetDocID("DIS", docID);

            qry = string.Format("  select PartyName,AreaId,SyncId,CityId from MastParty where PartyId={0} And Active=1 and PartyDist=1", DistId);
            DataTable dtDist = DbConnectionDAL.GetDataTable(CommandType.Text, qry);           
            AreaId = dtDist.Rows[0]["AreaId"].ToString();
            DistCode = dtDist.Rows[0]["PartyName"].ToString();
            SyncId = dtDist.Rows[0]["SyncId"].ToString();
             int RetSave = 0;
             if (itemforparty.Rows.Count == 0)
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please Fill Item Template First');", true);
                }
             for (int i = 0; i < itemforparty.Rows.Count; i++)
                {
                    HiddenField ItemId = (itemforparty.Rows[i].FindControl("hfItemId") as HiddenField);
                    Label itemname = (itemforparty.Rows[i].FindControl("lblitemname") as Label);

                    TextBox Unit = (itemforparty.Rows[i].FindControl("txtquantity") as TextBox);
                    Label rate = (itemforparty.Rows[i].FindControl("lblrate") as Label);
                    Label stdpack = (itemforparty.Rows[i].FindControl("lblstdpack") as Label);
                    //Label amount = (itemforparty.Rows[i].FindControl("lblamount") as Label);
                    TextBox txtqty = (itemforparty.Rows[i].FindControl("txtquantity") as TextBox);
                    TextBox StockQty = (itemforparty.Rows[i].FindControl("lblStockQty") as TextBox);
                    TextBox LooseQty = (itemforparty.Rows[i].FindControl("txtLooseQty") as TextBox);
                    if (Convert.ToString(stdpack.Text) != null)
                    {
                        if (Convert.ToString(stdpack.Text) != "")
                        {
                            string ItemBuf = Convert.ToString(stdpack.Text);
                            totalQty = (Convert.ToDecimal(txtqty.Text) * Convert.ToDecimal(ItemBuf)) + Convert.ToDecimal(LooseQty.Text);
                        }
                    }
                    if ((totalQty.ToString() == "0.00") || ((totalQty.ToString() == "0")))
                    {
                       
                        mszforatleastoneitem = "0";
                        continue;
                    }
                    retsave = db.InsertDItemStock(docID, Settings.DMInt32(Settings.Instance.UserID), GetVisitDate(Convert.ToInt32(VisitID)), Settings.DMInt32(Settings.Instance.VistID), Settings.DMInt32(Settings.Instance.SMID), DistId, dtDist.Rows[0]["SyncId"].ToString(), Settings.DMInt32(dtDist.Rows[0]["AreaId"].ToString()), Settings.DMInt32(ItemId.Value), totalQty, null, DateTime.Now.ToString("dd-MMM-yyyy"), Convert.ToDecimal(LooseQty.Text), Convert.ToDecimal(Unit.Text));
                    string updateandroidid = "update Temp_transdiststock set android_id='" + docID + "' where stkdocid='" + docID + "'";
                    DbConnectionDAL.ExecuteNonQuery(CommandType.Text, updateandroidid);
                    mszforatleastoneitem = "1";
                }
            //if (btnadd.Text != "Update")
            //{
            //    qry = string.Format("select * from TransDistStock where ItemId={0} and VDate='{1}'", Settings.DMInt32(ddlItem.SelectedValue), DateTime.Today.ToString("dd-MMM-yyyy"));
            //    DataTable dtcheck = DbConnectionDAL.GetDataTable(CommandType.Text, qry);
            //    if (dtcheck.Rows.Count > 0)
            //    {
            //        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Stock Already Added');", true);
            //        clear();
            //        return;

            //    }
            //}
            //if (btnadd.Text != "Update")
            //{
            //     retsave = db.InsertDItemStock(docID, Settings.DMInt32(Settings.Instance.UserID), DateTime.Now.ToString("dd-MMM-yyyy"), Settings.DMInt32(dtDist.Rows[0]["SMId"].ToString()), DistId, dtDist.Rows[0]["PartyName"].ToString(), Settings.DMInt32(dtDist.Rows[0]["AreaId"].ToString()), Settings.DMInt32(ddlItem.SelectedValue), totalQty, null, DateTime.Now.ToString("dd-MMM-yyyy"));

            //}
            //else {
            //    retsave = db.InsertDItemStock(Settings.DMInt32(ViewState["STKId"].ToString()), docID, Settings.DMInt32(Settings.Instance.UserID), DateTime.Now.ToString("dd-MMM-yyyy"), Settings.DMInt32(dtDist.Rows[0]["SMId"].ToString()), DistId, dtDist.Rows[0]["PartyName"].ToString(), Settings.DMInt32(dtDist.Rows[0]["AreaId"].ToString()), Settings.DMInt32(ddlItem.SelectedValue), totalQty, null, DateTime.Now.ToString("dd-MMM-yyyy"));
            //}

            ////
            if (retsave >0)
            {
                //if (btnadd.Text == "Update") System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Stock Updated Successfully');", true);
               
                //else System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Stock Added Successfully');", true);
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Stock Added Successfully');", true);
            }
            else
            {
                if (mszforatleastoneitem == "0")
                {
                    //if (btnadd.Text == "Update") System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Stock Updated Successfully');", true);

                    //else System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Stock Added Successfully');", true);
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Item Qty. Should Be Greater Than 0');", true);
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Failed To Add Stock');", true);
                    ClearControls();
                }
                return;
            }
            itemforparty.DataSource = null;
            ClearControls();
            ViewState["BUF"] = "";
            BindDistributorDDl(DistId);
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            //mainDiv.Style.Add("display", "block");
            //rptmain.Style.Add("display", "none");
            itemforparty.DataSource = null;
            ddlUnderItem.SelectedIndex = 0;
          //  Response.Redirect("~/DSREntryForm1.aspx?PartyId=" + DistId + "&AreaId=" + AreaId + "&CityID=" + CityID + "&VisitID=" + VisitID + "&Level=" + Level);
            if (Level == "1")
            {
                Response.Redirect("~/DSREntryForm1.aspx?PartyId=" + DistId + "&AreaId=" + AreaId + "&CityID=" + CityID + "&VisitID=" + VisitID + "&Level=" + Level);
            }
            else if (Level == "2")
            {               
                Response.Redirect("~/DistributorDashboardLevel2.aspx?PartyId=" + DistId + "&AreaId=" + AreaId + "&CityID=" + CityID + "&VisitID=" + VisitID + "&Level=" + Level);
            }

            else
            {              
                Response.Redirect("~/DistributorDashboardLevel3.aspx?PartyId=" + DistId + "&AreaId=" + AreaId + "&CityID=" + CityID + "&VisitID=" + VisitID + "&Level=" + Level);
            }
        }
        protected void btnFind_Click(object sender, EventArgs e)
        {
            btnadd.Text = "Save";
            //mainDiv.Style.Add("display", "none");
            //rptmain.Style.Add("display", "block");

        }

        public void clear()
        {
            hiditemid.Value = "";

        }

        protected void ddlItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtCase.Text = "";
            txtUnit.Text = "";
            lblAvailability.Text = "";
            strItem = string.Format("select IsNull(StdPack,0) as StdPack,IsNUll(MRP,0) as MRP from MastItem where ItemId={0}", Settings.DMInt32(ddlItem.SelectedValue));
            DataTable dtItem = DbConnectionDAL.GetDataTable(CommandType.Text, strItem);

            strDItem = string.Format("select IsNUll(Qty,0) as Qty from Temp_TransDistStock where ItemId={0} And DistId={1}", Settings.DMInt32(ddlItem.SelectedValue), DistId);
            DataTable dtDItem = DbConnectionDAL.GetDataTable(CommandType.Text, strDItem);
            if (dtItem.Rows.Count > 0)
            {
                Rate = dtItem.Rows[0]["MRP"].ToString();
                bUF = dtItem.Rows[0]["StdPack"].ToString();
                if (dtDItem.Rows.Count > 0)
                {
                    stockQty = dtDItem.Rows[0]["Qty"].ToString();
                    if ((Convert.ToDecimal(stockQty)) > 0)
                    {
                        unit = Convert.ToString((Convert.ToDecimal(stockQty)) % (Convert.ToDecimal(bUF)));
                        stockQty = Convert.ToString((Convert.ToDecimal(stockQty) - Convert.ToDecimal(unit)));
                        cQty = Convert.ToString((Convert.ToDecimal(stockQty)) / (Convert.ToDecimal(bUF)));
                        decimal tqty = Convert.ToDecimal(cQty);
                        cQty = Convert.ToString(Math.Round((decimal)tqty, 2));
                    }
                }
            }
            ViewState["BUF"] = bUF;
          
            string text = "Price :" + Rate + " , Availability :[C : " + cQty + ", U : " + unit + "]  ";
            lblAvailability.Text = text;
           
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            this.BindDistributorDDl(DistId);
        }
        private string GetVisitDate(int VisiID)
        {
            string st = "select VDate from TransVisit where VisId=" + VisitID;
            string VisitDate = Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, st));
            return VisitDate;
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string confirmValue = Request.Form["confirm_value"];

            if (confirmValue == "Yes")
            {
                int retsave = db.delete(Settings.DMInt32(ViewState["STKId"].ToString()));
                if (retsave == 1)
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Deleted Successfully');", true);
                   
                    btnDelete.Visible = false;
                    btnadd.Text = "Save";
                    ClearControls();
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record cannot be deleted as it is in use');", true);
                   
                }

            }
        }


    }
}