using BusinessLayer;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AstralFFMS
{
    public partial class PendingOrderReport : System.Web.UI.Page
    {
         int runningTotal = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDistributorDDl();
                BindMaterialGroup();
                string pageName = Path.GetFileName(Request.Path);
                //btnExport.Enabled = Settings.Instance.CheckExportPermission(pageName, Convert.ToString(Session["user_name"]));
                //btnExport.CssClass = "btn btn-primary";
            }
        }
        private void BindMaterialGroup()
        {
            try
            { //Ankita - 18/may/2016- (For Optimization)
                //string prodClassQry = @"select * from MastItem where ItemType='MATERIALGROUP' and Active=1 order by ItemName";
                string prodClassQry = @"select ItemId,ItemName from MastItem where ItemType='MATERIALGROUP' and Active=1 order by ItemName";
                DataTable dtProdRep = DbConnectionDAL.GetDataTable(CommandType.Text, prodClassQry);
                if (dtProdRep.Rows.Count > 0)
                {
                    ddlMatGrp.DataSource = dtProdRep;
                    ddlMatGrp.DataTextField = "ItemName";
                    ddlMatGrp.DataValueField = "ItemId";
                    ddlMatGrp.DataBind();
                }
                ddlMatGrp.Items.Insert(0, new ListItem("--Please select--"));
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
        private void BindDistributorDDl()
        {
            try
            {
                string citystr = "";
                //string cityQry = @"  select * from mastarea where areaid in (select distinct underid from mastarea where areaid in (select distinct underid from mastarea where areaid in (select areaid from MastAreaGrp where MainGrp in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode=" + Settings.Instance.SMID + ")) and Active=1 )) and areatype='City' and Active=1 order by AreaName";
                string cityQry = @"  select * from mastarea where areaid in (select distinct underid from mastarea where areaid in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode in (" + Settings.Instance.SMID + ")) and Active=1 ) and areatype='City' and Active=1 order by AreaName";

                DataTable dtCity = DbConnectionDAL.GetDataTable(CommandType.Text, cityQry);

                for (int i = 0; i < dtCity.Rows.Count; i++)
                {
                    citystr += dtCity.Rows[i]["AreaId"] + ",";
                }
                citystr = citystr.TrimStart(',').TrimEnd(',');
                string distqry = @"select * from MastParty where CityId in (" + citystr + ") and PartyDist=1 and Active=1 order by PartyName";

                //            string distqry = @"select * from MastParty where PartyDist=1 and Active=1 order by PartyName";
                DataTable dtDist = DbConnectionDAL.GetDataTable(CommandType.Text, distqry);
                if (dtDist.Rows.Count > 0)
                {
                    //DdlSalesPerson.DataSource = dtDist;
                    //DdlSalesPerson.DataTextField = "PartyName";
                    //DdlSalesPerson.DataValueField = "PartyId";
                    //DdlSalesPerson.DataBind();   
                    ListBox1.DataSource = dtDist;
                    ListBox1.DataTextField = "PartyName";
                    ListBox1.DataValueField = "PartyId";
                    ListBox1.DataBind();

                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
        protected void ddlMatGrp_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlMatGrp.SelectedIndex != 0)
            { //Ankita - 18/may/2016- (For Optimization)
                //string mastItemQry1 = @"select * from MastItem where Underid=" + ddlMatGrp.SelectedValue + " and ItemType='ITEM' and Active=1";
                string mastItemQry1 = @"select ItemId,ItemName from MastItem where Underid=" + ddlMatGrp.SelectedValue + " and ItemType='ITEM' and Active=1";
                DataTable dtMastItem1 = DbConnectionDAL.GetDataTable(CommandType.Text, mastItemQry1);
                if (dtMastItem1.Rows.Count > 0)
                {
                    ddlProduct.DataSource = dtMastItem1;
                    ddlProduct.DataTextField = "ItemName";
                    ddlProduct.DataValueField = "ItemId";
                    ddlProduct.DataBind();
                }
                ddlProduct.Items.Insert(0, new ListItem("--Please select--"));
            }
            else
            {
                ClearControls();
            }
        }
        private void ClearControls()
        {
            try
            {
                ddlProduct.Items.Clear();
                pendingorderrpt.DataSource = null;
                pendingorderrpt.DataBind();

            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            string smIDStr = "";
            string smIDStr1 = "", qrychk = "";
            foreach (ListItem item in ListBox1.Items)
            {
                if (item.Selected)
                {
                    smIDStr1 += item.Value + ",";
                }
            }
            smIDStr1 = smIDStr1.TrimStart(',').TrimEnd(',');

            if (ddlMatGrp.SelectedIndex != 0)
            {
                qrychk = " and i.Underid=" + ddlMatGrp.SelectedValue + "";
            }
            if (ddlProduct.SelectedIndex != 0 && ddlProduct.SelectedValue != "")
            {
                qrychk = qrychk + " and i.ItemId=" + ddlProduct.SelectedValue + "";
            }

            if (smIDStr1 != "")
            {
                string pendingOrderQry = @"select opd.OrderNo as OrderNo,convert (varchar,opd.OrderDate,106) as OrderDate,opd.ItemId ,
                i.ItemName as ItemName,da.PartyName as DistributorName,i.Unit ,
                opd.Rate,opd.Qty,opd.SuppliedQty,opd.PendingQty,(opd.Rate*opd.PendingQty) as PendingAmt from TransOrderPendingDetail opd
                left join MastItem i on i.ItemId=opd.ItemId
                left join MastParty da on da.PartyId =opd.DistrId 
                left join MastItemClass psg on psg.Id=I.ClassId 
                where da.PartyDist=1 AND (da.PartyId in (" + smIDStr1 + ")) " + qrychk + " Order by opd.OrderDate Desc,opd.OrderNo";

                DataTable dtPendingOrder = DbConnectionDAL.GetDataTable(CommandType.Text, pendingOrderQry);
                if (dtPendingOrder.Rows.Count > 0)
                {
                    rptmain.Style.Add("display", "block");
                    pendingorderrpt.DataSource = dtPendingOrder;
                    pendingorderrpt.DataBind();
                }
                else
                {
                    rptmain.Style.Add("display", "block");
                    pendingorderrpt.DataSource = dtPendingOrder;
                    pendingorderrpt.DataBind();
                }
            }
        }

        protected void Cancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/PendingOrderReport.aspx");
        }

        //protected void pendingorderrpt_ItemDataBound(object sender, RepeaterItemEventArgs e)
        //{
           
        //    if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        //    {
        //        runningTotal += Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "PendingAmt"));
        //    }
        //    else if (e.Item.ItemType == ListItemType.Footer)
        //    {
        //        Label lbl = (Label)e.Item.FindControl("totalLabel");
        //        lbl.Text = runningTotal.ToString();
        //    }
        //}
    }
}