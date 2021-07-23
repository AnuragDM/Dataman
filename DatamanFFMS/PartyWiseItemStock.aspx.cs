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
    public partial class PartyWiseItemStock : System.Web.UI.Page
    {
        string parameter1 = "";
        int DistId = 0, AreaId = 0, PartyId=0;
        BAL.PartyBAL db = new PartyBAL();
        String Level = "0";
        string pageSalesName = "", discount = "", strItem = "", strDItem = "", loseQty = "", stockQty = "", bUF = "", Rate = "", cQty = "", unit = "", roleType = "", parameter = "", VisitID = "", CityID = "";
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Request.QueryString["DistId"] != null)
            {
               // DistId = Convert.ToInt32(Request.QueryString["DistId"].ToString());
            }
            if (Request.QueryString["PartyId"] != null)
            {
                PartyId = Convert.ToInt32(Request.QueryString["PartyId"].ToString());
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
            PartyId = Convert.ToInt32(Request.QueryString["PartyId"].ToString());
            string pageName = Path.GetFileName(Request.Path);
            string PermAll = Settings.Instance.CheckPagePermissions(pageName, Convert.ToString(Session["user_name"]));
            string[] SplitPerm = PermAll.Split(',');
            if (btnadd.Text == "Save")
            {
                // btnadd.Enabled = Convert.ToBoolean(SplitPerm[1]);
                //btnSave.Enabled = Settings.Instance.CheckAddPermission(pageName, Convert.ToString(Session["user_name"]));
                btnadd.CssClass = "btn btn-primary";
            }
            else
            {
                btnadd.Enabled = Convert.ToBoolean(SplitPerm[2]);
                //btnSave.Enabled = Settings.Instance.CheckEditPermission(pageName, Convert.ToString(Session["user_name"]));
                btnadd.CssClass = "btn btn-primary";
            }
           
            if (!IsPostBack)
            {
                txtmDate.Text = DateTime.Parse(DateTime.Now.ToUniversalTime().AddSeconds(19800).ToShortDateString()).ToString("dd/MMM/yyyy");
                this.FillItem();
                this.FillUnderItem();
                roleType = Settings.Instance.RoleType;
                BindDistributorDDl(PartyId);
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
                    btnDelete.Enabled = Convert.ToBoolean(SplitPerm[3]);
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
            strParent = "select  mi.itemname,mt.* from MastItemTemplat mt,MastItem mi where mi.ItemId=mt.ItemId and  PartyId=" + PartyId;
            fillDDLDirect(ddlItem, strParent, "ItemId", "ItemName", 1);
        }
        private void FillUnderItem()
        {
            string strParent = "";
            strParent = "select  mi.itemname,mt.* from MastItemTemplat mt,MastItem mi where mi.ItemId=mt.ItemId and  PartyId=" + PartyId;
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
            distqry = @"select mt.STKId,mi.ItemId ,mi.itemname,mi.MRP,mi.ItemCode,mi.StdPack as UnitFactor,mt.VDate,mt.Qty as StockQty,mt.Qty % mi.StdPack as LooseQty,cast(((mt.Qty -(mt.Qty % mi.StdPack))/mi.StdPack) as decimal(18,2)) as CaseQty	from TransPartyStock mt,MastItem mi where mi.ItemId=mt.ItemId  and  PartySTKId=" + STKId;
            DataTable dtDist = DbConnectionDAL.GetDataTable(CommandType.Text, distqry);
          
            this.FillItem();
            ddlItem.SelectedValue = dtDist.Rows[0]["ItemId"].ToString();
            txtCase.Text = dtDist.Rows[0]["CaseQty"].ToString();
            txtUnit.Text = dtDist.Rows[0]["LooseQty"].ToString();
            ddlItem.Enabled = false;
            string text = "Price :" + dtDist.Rows[0]["MRP"].ToString() + " , Availability :[C : " + dtDist.Rows[0]["CaseQty"].ToString() + ", U : " + dtDist.Rows[0]["LooseQty"].ToString() + "]  ";
            lblAvailability.Text = text;

        }
        private void BindDistributorDDl(int PartyId)
        {
            string distqry="";
            try
            {               
                {
                    distqry = @"Select Count(*) From TransPartyStock mt where mt.VDate='" + txtmDate.Text + "' and  mt.PartyId=" + PartyId;
                    int count = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, distqry));
                    if (count > 0)
                    {
                        if (Settings.DMInt32(ddlUnderItem.SelectedValue) > 0) distqry = @"select Isnull(mt.PartySTKId,0) as STKId ,mi.ItemId,mi.itemname,mi.MRP,mi.ItemCode,mi.Unit,mi.StdPack as UnitFactor,mt.VDate,isnull(mt.Qty,0) as StockQty,case isnull(mt.Qty,0) when 0 then 0 else mt.Qty % mi.StdPack end as LooseQty,case isnull(mt.Qty,0) when 0 then 0 else cast(((mt.Qty -(mt.Qty % mi.StdPack))/mi.StdPack) as decimal(18,2)) end as CaseQty,case isnull(mt.Qty,0) when 0 then 0 else cast((mt.Qty * mi.MRP) as decimal(18,2)) end as Amount from TransPartyStock mt right join MastItemTemplat dit on mt.ItemId=dit.ItemId inner join MastItem mi on dit.ItemId=mi.ItemId Where mt.VDate='" + txtmDate.Text + "' And mt.ItemId=" + Settings.DMInt32(ddlUnderItem.SelectedValue) + " and  mt.PartyId=" + PartyId;
                        else distqry = @"select Isnull(mt.PartySTKId,0) as STKId,mi.ItemId,mi.itemname,mi.MRP,mi.ItemCode,mi.Unit,mi.StdPack as UnitFactor,mt.VDate,isnull(mt.Qty,0) as StockQty,case isnull(mt.Qty,0) when 0 then 0 else mt.Qty % mi.StdPack end as LooseQty,case isnull(mt.Qty,0) when 0 then 0 else cast(((mt.Qty -(mt.Qty % mi.StdPack))/mi.StdPack) as decimal(18,2)) end as CaseQty,case isnull(mt.Qty,0) when 0 then 0 else cast((mt.Qty * mi.MRP) as decimal(18,2)) end as Amount from TransPartyStock mt right join MastItemTemplat dit on mt.ItemId=dit.ItemId inner join MastItem mi on dit.ItemId=mi.ItemId where mt.VDate='" + txtmDate.Text + "' and  mt.PartyId=" + PartyId;
                    }
                    else {
                        distqry = @"select 0 as STKId,mi.ItemId,mi.itemname,mi.MRP,mi.ItemCode,mi.Unit,mi.StdPack as UnitFactor,Getdate() as VDate,0 as StockQty,0 as  LooseQty,0 as CaseQty,0 as Amount from MastItemTemplat dit inner join MastItem mi on dit.ItemId=mi.ItemId Where dit.Partyid=" +PartyId;
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
            double LooseQty = double.Parse(txtLQty.Text, System.Globalization.CultureInfo.InvariantCulture);
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
            Response.Redirect("~/PartyDashboard.aspx");
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
            string distqry = @"delete from TransPartyStock where  DistItemId=" + id;
           if(( DbConnectionDAL.ExecuteQuery(distqry))==1)
           {
               distqry = @"update TransPartyStock set createddate=getdate() where PartyId=" + PartyId + "";
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
            string AreaId = "";
            string SyncId = "";
            string DistCode = "";
            int retsave = 0;
            string itemBUF = "";
            //strItem = string.Format("select IsNull(StdPack,0) as StdPack,IsNUll(MRP,0) as MRP from MastItem where ItemId={0}", Settings.DMInt32(ddlItem.SelectedValue));
            //DataTable dtItem = DbConnectionDAL.GetDataTable(CommandType.Text, strItem);
            //itemBUF = dtItem.Rows[0]["StdPack"].ToString();
            //if (Convert.ToString(itemBUF) != null)
            //{
            //    if (Convert.ToString(itemBUF) != "")
            //    {
            //        string ItemBuf = Convert.ToString(itemBUF);
            //        totalQty = (Convert.ToDecimal(txtCase.Text) * Convert.ToDecimal(ItemBuf)) + Convert.ToDecimal(txtUnit.Text);
            //    }
            //}

            strItem = string.Format("Delete  from TransPartyStock where PartyId={0} And VDate='{1}'", PartyId, txtmDate.Text);
            DbConnectionDAL.ExecuteNonQuery(CommandType.Text, strItem);

            string docID = Settings.GetDocID("PIS", DateTime.Now);
            Settings.SetDocID("PIS", docID);

           // qry = string.Format("  select SMID,PartyName,SyncId,AreaId,CityId from MastParty where UserId={0} And Active=1 and PartyDist=0", PartyId);
            qry = string.Format("  select SMID,PartyName,SyncId,AreaId,CityId from MastParty where PartyId={0} And Active=1 and PartyDist=0", PartyId);
            DataTable dtDist = DbConnectionDAL.GetDataTable(CommandType.Text, qry);

            //qry = string.Format("  select SMID from MastParty where PartyId={0} And Active=1 and PartyDist=0", Settings.Instance.SMID);
            //SMId =Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, qry));

          //  SMId = dtDist.Rows[0]["SMID"].ToString();
            AreaId = dtDist.Rows[0]["AreaId"].ToString();
            DistCode = dtDist.Rows[0]["PartyName"].ToString();
            SyncId= dtDist.Rows[0]["SyncId"].ToString();
             int RetSave = 0;
             if (itemforparty.Rows.Count == 0)
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please Fill Item Template First');", true);
                }
             for (int i = 0; i < itemforparty.Rows.Count; i++)
                {
                    HiddenField ItemId = (itemforparty.Rows[i].FindControl("hfItemId") as HiddenField);
                    Label itemname = (itemforparty.Rows[i].FindControl("lblitemname") as Label);
                    Label unit = (itemforparty.Rows[i].FindControl("lblunit") as Label);
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

                    retsave = db.InsertPItemStock(docID, Settings.DMInt32(Settings.Instance.UserID), txtmDate.Text, Settings.DMInt32(VisitID), Settings.DMInt32(Settings.Instance.SMID), PartyId, SyncId, Settings.DMInt32(dtDist.Rows[0]["AreaId"].ToString()), Settings.DMInt32(ItemId.Value), totalQty, null, DateTime.Now.ToString("dd-MMM-yyyy"), "");
                    
                }
            //if (btnadd.Text != "Update")
            //{
            //    qry = string.Format("select * from TransPartyStock where ItemId={0} and VDate='{1}'", Settings.DMInt32(ddlItem.SelectedValue), DateTime.Today.ToString("dd-MMM-yyyy"));
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

                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Failed To Add Stock');", true);
               ClearControls();
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
            Response.Redirect("~/PartyDashboard.aspx?PartyId=" + PartyId + "&AreaId=" + AreaId + "&CityID=" + CityID + "&VisitID=" + VisitID + "&Level=" + Level);
            itemforparty.DataSource = null;
            ddlUnderItem.SelectedIndex = 0;
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

            strDItem = string.Format("select IsNUll(Qty,0) as Qty from TransPartyStock where ItemId={0} And DistId={1}", Settings.DMInt32(ddlItem.SelectedValue), DistId);
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
            this.BindDistributorDDl(PartyId);
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string confirmValue = Request.Form["confirm_value"];

            //if (confirmValue == "Yes")
            //{
            //    int retsave = db.delete(Settings.DMInt32(ViewState["STKId"].ToString()));
            //    if (retsave == 1)
            //    {
            //        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Deleted Successfully');", true);
                   
            //        btnDelete.Visible = false;
            //        btnadd.Text = "Save";
            //        ClearControls();
            //    }
            //    else
            //    {
            //        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record cannot be deleted as it is in use');", true);
                   
            //    }

            //}
        }


    }
}