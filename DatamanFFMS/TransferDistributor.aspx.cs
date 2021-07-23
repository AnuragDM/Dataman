using DAL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLayer;
namespace AstralFFMS
{
    public partial class TransferDistributor : System.Web.UI.Page
    {
        string roleType = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                List<Distributors> distributors = new List<Distributors>();
                distributors.Add(new Distributors());
                distreportrpt.DataSource = distributors;
                distreportrpt.DataBind();
                roleType = Settings.Instance.RoleType;
                BindSalePersonDDl();
                bindCity();
            }
        }
        public class Distributors
        {

            public string Distributor { get; set; }
        }
        private void bindCity()
        {
            string Qry = @"select * from MastArea where AreaType='City' and Active=1 order by AreaName";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, Qry);
            if (dt.Rows.Count > 0)
            {
                LstCity.DataSource = dt;
                LstCity.DataTextField = "AreaName";
                LstCity.DataValueField = "AreaId";
                LstCity.DataBind();
            }
        }
        private void BindSalePersonDDl()
        {
            try
            {
                if (roleType == "Admin")
                {//Ankita - 17/may/2016- (For Optimization)
                    //string strrole = "select mastrole.RoleName,MastSalesRep.*,mastrole.RoleType from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId where MastSalesRep.Active=1 and SMName<>'.' order by MastSalesRep.SMName";
                    string strrole = "select mastrole.RoleName,MastSalesRep.SMId,MastSalesRep.SMName,mastrole.RoleType from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId where MastSalesRep.Active=1 and SMName<>'.' order by MastSalesRep.SMName";
                    DataTable dtcheckrole = new DataTable();
                    dtcheckrole = DbConnectionDAL.GetDataTable(CommandType.Text, strrole);
                    DataView dv1 = new DataView(dtcheckrole);
                    dv1.RowFilter = "SMName<>.";
                    dv1.Sort = "SMName asc";

                    LstSales.DataSource = dv1.ToTable();
                    LstSales.DataTextField = "SMName";
                    LstSales.DataValueField = "SMId";
                    LstSales.DataBind();
                }
                else
                {
                    DataTable dt = Settings.UnderUsers(Settings.Instance.SMID);
                    DataView dv = new DataView(dt);
                    //     dv.RowFilter = "RoleName='Level 1'";
                    dv.RowFilter = "SMName<>.";
                    dv.Sort = "SMName asc";
                    if (dv.ToTable().Rows.Count > 0)
                    {
                        LstSales.DataSource = dv.ToTable();
                        LstSales.DataTextField = "SMName";
                        LstSales.DataValueField = "SMId";
                        LstSales.DataBind();
                    }
                }
                //    DdlSalesPerson.Items.Insert(0, new ListItem("--Please select--"));
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
        [WebMethod(EnableSession = true)]
        public static string binddistributor(string City, string Smid)
        {

            //string Qrychk = " t1.VDate>='" + Settings.dateformat(Fromdate) + " 00:00' and t1.VDate<='" + Settings.dateformat(Todate) + " 23:59'";
            //            string query = @"SELECT t1.VDate,da.PartyName AS Distributor,max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],sum(qty) as Qty,sum(Amount) as Amount from TransDistInv1 t1 left outer join MastItem mi
            //                           on mi.ItemId=t1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=t1.DistId where (" + Qrychk + ") group BY  t1.VDate,da.PartyName, mi.ItemName order BY  t1.VDate desc";
            //string smIDStr1 = "";
            //smIDStr1 = HttpContext.Current.Session["treenodes"].ToString();
            //string Qrychk = " TransDistributerLedger.VDate<='" + Settings.dateformat(Todate) + " 23:59'";
            //HttpContext.Current.Session["aaa"] = Fromdate;
            //HttpContext.Current.Session["bbb"] = Todate;
            //HttpContext.Current.Session["DistId"] = Distid;
            string smname = DbConnectionDAL.GetScalarValue(CommandType.Text, "select smname from mastsalesrep where smid=" + Smid + "").ToString();

            string query = @"select * from (Select  PartyId,PartyName+' ('+Syncid+') '+' - Assigned To '+(Select smname from mastsalesrep where smid=MastParty.smid) PartyName,smid,userid,case when (Select smname from mastsalesrep where smid=MastParty.smid) ='" + smname + "' then 1 else 0 end as Sorting from MastParty where PartyDist=1 and CityId in (" + City + "))tbl order by Sorting desc,PartyName"; 

            DataTable dtItem = DbConnectionDAL.GetDataTable(CommandType.Text, query);
            return JsonConvert.SerializeObject(dtItem);


        }

        [WebMethod(EnableSession = true)]
        public static string updatedistsmid(string smid,bool check,string distid)
        {
              string res = "";
            try
            {
                string str1 = "";
                int defaultsmid = 0;
                if (check == true)
                {
                    str1 = "update MastParty set smid=" + smid + ",created_date=getdate() where PartyId=" + Convert.ToInt32(distid) + "";
                    DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str1);
                }
                else
                {
                    defaultsmid = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, "Select SMId from MastSalesRep where SMName='.'"));
                    str1 = "update MastParty set smid=" + defaultsmid + ",created_date=getdate()  where PartyId=" + Convert.ToInt32(distid) + "";
                    DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str1);
                }
              res="Record updated"; 

            }
            catch 
            {
                res="";
            }
             return JsonConvert.SerializeObject(res);
        }
    }
}