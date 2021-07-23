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

namespace AstralFFMS
{
    public partial class OrderTemplates : System.Web.UI.Page
    {
        string roleType = "";
        int PartyId = 0;
        string parameter = "";
        string VisitID = "";
        string CityID = "";
        int AreaId = 0;
        String Level = "0"; string pageSalesName = "";
        string discount = "";
        protected void Page_Load(object sender, EventArgs e)
        {

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
            if (!IsPostBack)
            {
                
                roleType = Settings.Instance.RoleType;
                
               
            }
            BindDistributorDDl(PartyId);
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

        private void BindDistributorDDl(int partyid)
        {
            try
            {               
                {
                    string distqry = @"select mi.itemname,mt.* from MastItemTemplat mt,MastItem mi where mi.ItemId=mt.ItemId and  partyid=" + partyid;
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

      
        protected void LinkButton1_Command(object sender, CommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument.ToString());
            string distqry = @"delete from MastItemTemplat where  id=" + id;
           if(( DbConnectionDAL.ExecuteQuery(distqry))==1)
           {
               distqry = @"update MastItemTemplat set createddate=getdate() where partyid=" + PartyId + "";
               DbConnectionDAL.ExecuteQuery(distqry);
               System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Deleted Successfully');", true);
           }
           else
           {
               System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Failed To Delete Item');", true);
               return;
           }
           BindDistributorDDl(PartyId);
           clear();
        }

        protected void btnadd_Click(object sender, EventArgs e)
        {
            string qry = "";
            qry = @"select * from MastItemTemplat where partyid=" + PartyId + " and itemid=" + Convert.ToInt32(hiditemid.Value);
            DataTable dtcheck = DbConnectionDAL.GetDataTable(CommandType.Text,qry);
            if(dtcheck.Rows.Count>0)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Item Already Added');", true);
                clear();
                return;

            }
             qry = @"insert into MastItemTemplat (partyid,itemid)values(" + PartyId + "," + Convert.ToInt32(hiditemid.Value) + ")";
            if ((DbConnectionDAL.ExecuteQuery(qry)) == 1)
            {
                qry = @"update MastItemTemplat set createddate=getdate() where partyid=" + PartyId + "";
                DbConnectionDAL.ExecuteQuery(qry);
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Item Added Successfully');", true);
            }
            else
            {

                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Failed To Add Item');", true);
                clear();
                return;
            }
            clear();
            BindDistributorDDl(PartyId);
        }
        protected void btnBack_Click1(object sender, EventArgs e)
        {
            if (pageSalesName == "Secondary")
            {
                Response.Redirect("~/PartyDashboard.aspx?PartyId=" + PartyId + "&AreaId=" + AreaId + "&CityID=" + CityID + "&VisitID=" + VisitID + "&Level=" + Level + "&PageView=" + pageSalesName);
            }
            else
            {
                Response.Redirect("~/PartyDashboard.aspx?PartyId=" + PartyId + "&AreaId=" + AreaId + "&CityID=" + CityID + "&VisitID=" + VisitID + "&Level=" + Level);
            }

        }
        public void clear()
        {
            txtitem.Text = "";
            hiditemid.Value = "";

        }



    }
}