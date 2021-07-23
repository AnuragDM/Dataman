using BusinessLayer;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AstralFFMS
{
    public partial class DistComplaintReport : System.Web.UI.Page
    {
         int distID = 0;
         string roleType = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {//Ankita - 17/may/2016- (For Optimization)
               // GetRoleType(Settings.Instance.RoleID);
                roleType = Settings.Instance.RoleType;
                distID = GetDistId(Convert.ToInt32(Settings.Instance.UserID));
                //Added By - Nishu 06/12/2015 
                txttodate.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");
                txtfmDate.Text = Settings.GetUTCTime().AddMonths(-1).ToString("dd/MMM/yyyy");
                //End 
                //        BindProductClass();
                BindDistributorDDlist();
                BindMaterialGroup();

            }
        }

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

        private int GetDistId(int p)
        {
            try
            {
                //var envObj1 = (from e in context.MastParties.Where(x => x.UserId == p && x.PartyDist == true)
                //               select new { e.PartyId, e.PartyName }).FirstOrDefault();
                string distQry = @"select e.PartyId, e.PartyName from MastParty e where e.UserId=" + p + " and e.PartyDist=1 and Active=1";
                DataTable DtNew2 = DbConnectionDAL.GetDataTable(CommandType.Text, distQry);
                if (DtNew2.Rows.Count > 0)
                {
                    return Convert.ToInt32(DtNew2.Rows[0]["PartyId"]);
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                return 0;
            }
        }

        private void BindDistributorDDlist()
        {
            try
            {//Ankita - 17/may/2016- (For Optimization)
                string distqry = string.Empty;
                if (roleType == "Admin")
                {
                    distqry = @"select PartyId,PartyName from MastParty where PartyDist=1 and Active=1 order by PartyName";
                   // distqry = @"select * from MastParty where PartyDist=1 and Active=1 order by PartyName";
                }
                else
                {
                    distqry = @"select PartyId,PartyName from MastParty where PartyId in (" + distID + ") and PartyDist=1 and Active=1 order by PartyName";
                    //distqry = @"select * from MastParty where PartyId in (" + distID + ") and PartyDist=1 and Active=1 order by PartyName";
                }
                DataTable dtDist = DbConnectionDAL.GetDataTable(CommandType.Text, distqry);
                if (dtDist.Rows.Count > 0)
                {

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

        private void BindMaterialGroup()
        {
            try
            {//Ankita - 17/may/2016- (For Optimization)
                //string prodClassQry = @"select * from MastItem where ItemType='MATERIALGROUP' and Active=1 order by ItemName";
                string prodClassQry = @"select ItemId,ItemName from MastItem where ItemType='MATERIALGROUP' and Active=1 order by ItemName";
                DataTable dtProdRep = DbConnectionDAL.GetDataTable(CommandType.Text, prodClassQry);
                if (dtProdRep.Rows.Count > 0)
                {
                    matGrpListBox.DataSource = dtProdRep;
                    matGrpListBox.DataTextField = "ItemName";
                    matGrpListBox.DataValueField = "ItemId";
                    matGrpListBox.DataBind();
                }
                //   ddlMatGrp.Items.Insert(0, new ListItem("--Please select--"));
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
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
            {//Ankita - 17/may/2016- (For Optimization)
                //string mastItemQry1 = @"select * from MastItem where Underid in (" + matGrpStr + ") and ItemType='ITEM' and Active=1";
                string mastItemQry1 = @"select ItemId,ItemName from MastItem where Underid in (" + matGrpStr + ") and ItemType='ITEM' and Active=1";
                DataTable dtMastItem1 = DbConnectionDAL.GetDataTable(CommandType.Text, mastItemQry1);
                if (dtMastItem1.Rows.Count > 0)
                {
                    productListBox.DataSource = dtMastItem1;
                    productListBox.DataTextField = "ItemName";
                    productListBox.DataValueField = "ItemId";
                    productListBox.DataBind();
                }
                //       ddlProduct.Items.Insert(0, new ListItem("--Please select--"));
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
                //    ddlProduct.Items.Clear();
                productListBox.Items.Clear();
                txtfmDate.Text = System.DateTime.Now.ToShortDateString();
                txttodate.Text = System.DateTime.Now.ToShortDateString();
                complreportrpt.DataSource = null;
                complreportrpt.DataBind();

            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            try
            {
                string Qrychk = "", getComplQry = "";
                string distIDStr1 = "", matProStrNew = "", matGrpStr = "";
                foreach (ListItem item in ListBox1.Items)
                {
                    if (item.Selected)
                    {
                        distIDStr1 += item.Value + ",";
                    }
                }
                distIDStr1 = distIDStr1.TrimStart(',').TrimEnd(',');

                //For Product
                foreach (ListItem product in productListBox.Items)
                {
                    if (product.Selected)
                    {
                        matProStrNew += product.Value + ",";
                    }
                }
                matProStrNew = matProStrNew.TrimStart(',').TrimEnd(',');
                //For Material Group
                foreach (ListItem MatGrp in matGrpListBox.Items)
                {
                    if (MatGrp.Selected)
                    {
                        matGrpStr += MatGrp.Value + ",";
                    }
                }
                matGrpStr = matGrpStr.TrimStart(',').TrimEnd(',');

                Qrychk = " c.VDate>='" + Settings.dateformat(txtfmDate.Text) + " 00:00' and c.VDate<='" + Settings.dateformat(txttodate.Text) + " 23:59'";


                if (matProStrNew != "")
                    Qrychk = Qrychk + " and c.ItemId in (" + matProStrNew + ") ";

                if (matGrpStr != "")
                {
                    Qrychk = Qrychk + " and I.UnderId in (" + matGrpStr + ")";
                }
                if (distIDStr1 != "")
                {

                    getComplQry = @"select Convert(varchar(20),c.VDate,106) AS CompDate,cp.SMName as CompBY,ma.AreaName as City,
                 i.ItemName as Item,cn.Name AS ComplaintNature, c.Remark [Complaint],c.Imgurl as URL,isnull(mp.PartyName,'') AS Distributor from TransComplaint c 
                 inner join MastItem i on i.ItemId=c.ItemId INNER JOIN MastComplaintNature cn ON cn.Id=c.ComplNatId LEFT JOIN MastParty mp ON c.DistId = mp.PartyId where " + Qrychk + " and mp.PartyDist=1 and c.DistId in (" + distIDStr1 + ") order by c.Vdate desc ";
                    DataTable dtComplaint = DbConnectionDAL.GetDataTable(CommandType.Text, getComplQry);
                    if (dtComplaint.Rows.Count > 0)
                    {
                        complreportrpt.DataSource = dtComplaint;
                        complreportrpt.DataBind();
                    }
                    else
                    {
                        complreportrpt.DataSource = dtComplaint;
                        complreportrpt.DataBind();
                    }
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select distributor');", true);
                    complreportrpt.DataSource = null;
                    complreportrpt.DataBind();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/DistComplaintReport.aspx");
        }
    }
}