using BusinessLayer;
using DAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AstralFFMS
{
    public partial class SaleInvoiceform : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
                 txtfmDate.Attributes.Add("readonly", "readonly");
            txttodate.Attributes.Add("readonly", "readonly");
            if (!IsPostBack)
            {

                txtfmDate.Text = Settings.GetUTCTime().AddDays(-6).ToString("dd/MMM/yyyy");
                txttodate.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");
                List<orderrpt1> orderpurchrptnew = new List<orderrpt1>();
                orderpurchrptnew.Add(new orderrpt1());
                orderpurchrpt.DataSource = orderpurchrptnew;
                orderpurchrpt.DataBind();
                List<itemdetail> itemdet = new List<itemdetail>();
                itemdet.Add(new itemdetail());
                rpt.DataSource = itemdet;
                rpt.DataBind();
                string pageName = Path.GetFileName(Request.Path);
                string PermAll = Settings.Instance.CheckPagePermissions(pageName, Convert.ToString(Session["user_name"]));
                string[] SplitPerm = PermAll.Split(',');


                hidaddpermission.Value = SplitPerm[1];
                hidviewpermission.Value = SplitPerm[0];
                hiddeletepermission.Value = SplitPerm[3];
            }
        }
        public class orderrpt1
        {
            public string SNo { get; set; }
            public string Distributor { get; set; }
            public string Mobile { get; set; }
            public string PODocId { get; set; }
            public string PODate { get; set; }
            public string totalamount { get; set; }
            public string Orderby { get; set; }

        }
        public class itemdetail
        {
            public string Docid { get; set; }
              public string ItemId { get; set; }
                       public string ItemName { get; set; }
                  public string OrderQty { get; set; }
                     public string Rate { get; set; }
                    public string Discount { get; set; }
                       public string Tax { get; set; }
                       public string OrderAmount { get; set; }
        }
        [WebMethod(EnableSession = true)]
        public static ArrayList BindState()
        {
            ArrayList list = new ArrayList();
            string str = "";
            string condtion = "";
            string areaid = "";
            if (Settings.Instance.RoleType.ToUpper() == "ADMIN")
            {

            }
            else if (Settings.Instance.RoleType.ToUpper() == "DISTRIBUTOR")
            {
                areaid = Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, "Select AreaId from mastparty where PartyId =" + Settings.Instance.DistributorID + ""));
                condtion = "and Areaid =" + areaid + "";

            }

            string cityQry = "";
            if (Settings.Instance.RoleType.ToUpper() == "ADMIN")
            {
                //cityQry = @" SELECT DISTINCT(VG.stateName)+'-'+Vg.cityName+'-'+Vg.AreaName As Name,VG.areaName,Vg.statename,VG.AreaId As Id,";
                //cityQry = cityQry + " AreaType As [Type_Id],Vg.CityID,Ma.SyncId,Ma.Active,CreatedDate ";
                //cityQry = cityQry + " FROM MastLink ML INNER JOIN ViewGeo VG ON ML.LinkCode=VG.areaId Inner Join MastArea Ma On Vg.AreaId=Ma.AreaId ";
                //cityQry = cityQry + " where Vg.CityID in (select distinct underid from mastarea ";
                //cityQry = cityQry + "  where areaid in (select linkcode from mastlink where primtable='SALESPERSON' ";
                //cityQry = cityQry + " and LinkTable='AREA' ))";
                /////and PrimCode in (SELECT smid FROM MastSalesRep WHERE ";
                //// cityQry = cityQry + "  SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + Settings.Instance.SMID + ")))) and Active=1 ) ";
                //cityQry = cityQry + "  order by VG.areaName";


                cityQry = "SELECT MS.AreaName+'-'+MC.AreaName+'-'+MA1.AreaName AS Name,MA1.AreaId AS id   FROM Mastarea MA1";
                cityQry = cityQry + " LEFT JOIN MastArea MC ON MC.AreaId=MA1.underid LEFT JOIN MastArea MD ON MD.AreaId=MC.underid";
                cityQry = cityQry + " LEFT JOIN MastArea MS ON MS.AreaId=MD.underid  WHERE MA1.AreaType='Area'";
                cityQry = cityQry + " AND    MA1.areaid in (select linkcode from mastlink where primtable='SALESPERSON'  and LinkTable='AREA' ) ORDER BY MA1.AreaName";

            }
            else
            {

                cityQry = "SELECT MS.AreaName+'-'+MC.AreaName+'-'+MA1.AreaName AS Name,MA1.AreaId AS id   FROM Mastarea MA1";
                cityQry = cityQry + " LEFT JOIN MastArea MC ON MC.AreaId=MA1.underid LEFT JOIN MastArea MD ON MD.AreaId=MC.underid";
                cityQry = cityQry + " LEFT JOIN MastArea MS ON MS.AreaId=MD.underid  WHERE MA1.AreaType='Area'";
                cityQry = cityQry + " AND    MA1.areaid in (select linkcode from mastlink where primtable='SALESPERSON' ";
                cityQry = cityQry + " and LinkTable='AREA' and PrimCode in (SELECT smid FROM MastSalesRep WHERE ";
                cityQry = cityQry + "  SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + Settings.Instance.SMID + ")))) ORDER BY MA1.AreaName";



                //cityQry = @" SELECT DISTINCT(VG.stateName)+'-'+Vg.cityName+'-'+Vg.AreaName As Name,VG.areaName,Vg.statename,VG.AreaId As Id,";
                //cityQry = cityQry + " AreaType As [Type_Id],Vg.CityID,Ma.SyncId,Ma.Active,CreatedDate ";
                //cityQry = cityQry + " FROM MastLink ML INNER JOIN ViewGeo VG ON ML.LinkCode=VG.areaId Inner Join MastArea Ma On Vg.AreaId=Ma.AreaId ";
                //cityQry = cityQry + " where Vg.CityID in (select distinct underid from mastarea ";
                //cityQry = cityQry + "  where areaid in (select linkcode from mastlink where primtable='SALESPERSON' ";
                //cityQry = cityQry + " and LinkTable='AREA' and PrimCode in (SELECT smid FROM MastSalesRep WHERE ";
                //cityQry = cityQry + "  SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + Settings.Instance.SMID + ")))) and Active=1 ) ";
                //cityQry = cityQry + "  order by VG.areaName";
            }
            if (areaid != "")
            {


                cityQry = "SELECT MS.AreaName+'-'+MC.AreaName+'-'+MA1.AreaName AS Name,MA1.AreaId AS id   FROM Mastarea MA1";
                cityQry = cityQry + " LEFT JOIN MastArea MC ON MC.AreaId=MA1.underid LEFT JOIN MastArea MD ON MD.AreaId=MC.underid";
                cityQry = cityQry + " LEFT JOIN MastArea MS ON MS.AreaId=MD.underid  WHERE MA1.AreaType='Area'";
                cityQry = cityQry + " AND    MA1.areaid in (" + areaid + ")";
                // cityQry = @" SELECT DISTINCT(VG.stateName)+'-'+Vg.cityName+'-'+Vg.AreaName As Name,VG.areaName,Vg.statename,VG.AreaId As Id,";
                // cityQry = cityQry + " AreaType As [Type_Id],Vg.CityID,Ma.SyncId,Ma.Active,CreatedDate ";
                // cityQry = cityQry + " FROM MastLink ML INNER JOIN ViewGeo VG ON ML.LinkCode=VG.areaId Inner Join MastArea Ma On Vg.AreaId=Ma.AreaId ";
                // cityQry = cityQry + " where Vg.areaid in  (" + areaid + ")";
                //// cityQry = cityQry + "  where areaid in (" + areaid + ")) ";
                // cityQry = cityQry + "  order by VG.areaName";
            }
            //  str = "select AreaID,AreaName from mastarea where AreaType='State' and Active='1' " +  condtion +" order by AreaName";

            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, cityQry);
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(new ListItem(dr["Name"].ToString(), dr["Id"].ToString()));
                }
            }
            //if (list.Count == 1)
            //    PopulateCities(dt.Rows[0]["Id"].ToString());
            return list;
        }
        [WebMethod(EnableSession = true)]
        public static ArrayList BindDistributor(string CityId)
        {
            ArrayList list = new ArrayList();
            string strQ = "";
            if (CityId != "")
            {

                if (Settings.Instance.RoleType.ToUpper() == "DISTRIBUTOR")
                    strQ = "select MP.PartyId As Id,MP.PartyName As Name from MastParty MP where MP.areaid in (" + CityId + ") and PartyDist=1 and Active=1 and partyid=" + Settings.Instance.DistributorID + " order by PartyName";
                else
                    strQ = "select MP.PartyId As Id,MP.PartyName As Name from MastParty MP where MP.areaid in (" + CityId + ") and PartyDist=1 and Active=1 order by PartyName";
                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        list.Add(new ListItem(dr["Name"].ToString(), dr["Id"].ToString()));
                    }
                }
            }
            return list;
        }
    }
}