using System;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using BusinessLayer;
using System.Data;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using DAL;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
namespace AstralFFMS
{
    /// <summary>
    /// Summary description for DataBindService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
     [System.Web.Script.Services.ScriptService]
    public class DataBindService : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }
        [WebMethod(EnableSession = true)]
        public string PopulateState()
        {
            string strQ = "select Distinct(StateID),StateName from ViewGeo VG  order by StateName";
            //  string strQ = "select Distinct(CityID),CityName from ViewGeo VG inner join MastLink ML On VG.cityid=ML.LinkCode where VG.StateId=" + StateID + " and ML.ECode='SA' and ml.PrimCode="+ Settings.Instance.SMID+ " order by CityName";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
            if (dt.Rows.Count > 0)
            {
            }

            return JsonConvert.SerializeObject(dt);
        }
        [WebMethod(EnableSession = true)]
        public string PopulateCityByState(Int32 StateID)
        {
            string strQ = "select Distinct(CityID),CityName from ViewGeo VG where VG.StateId=" + StateID + " order by CityName";
            //  string strQ = "select Distinct(CityID),CityName from ViewGeo VG inner join MastLink ML On VG.cityid=ML.LinkCode where VG.StateId=" + StateID + " and ML.ECode='SA' and ml.PrimCode="+ Settings.Instance.SMID+ " order by CityName";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text,strQ);
            if (dt.Rows.Count > 0)
            {
            }

            return JsonConvert.SerializeObject(dt);
        }

        [WebMethod(EnableSession = true)]
        public string PopulateAreaByCity(Int32 CityID)
        {
            string strQ = "select Distinct areaId,areaName from ViewGeo VG where VG.CityId=" + CityID + " order by areaName";
            //  string strQ = "select Distinct(CityID),CityName from ViewGeo VG inner join MastLink ML On VG.cityid=ML.LinkCode where VG.StateId=" + StateID + " and ML.ECode='SA' and ml.PrimCode="+ Settings.Instance.SMID+ " order by CityName";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
            if (dt.Rows.Count > 0)
            {
            }

            return JsonConvert.SerializeObject(dt);
        }
        [WebMethod(EnableSession = true)]
        public string PopulateBeatByArea(Int32 AreaID)
        {
            string strQ = "select Distinct BeatId,BeatName from ViewGeo VG where VG.AreaId=" + AreaID + " order by BeatName";
            //  string strQ = "select Distinct(CityID),CityName from ViewGeo VG inner join MastLink ML On VG.cityid=ML.LinkCode where VG.StateId=" + StateID + " and ML.ECode='SA' and ml.PrimCode="+ Settings.Instance.SMID+ " order by CityName";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
            if (dt.Rows.Count > 0)
            {
            }

            return JsonConvert.SerializeObject(dt);
        }

        [WebMethod(EnableSession = true)]
        public string PopulatePartyByBeat(Int32 BeatID)
        {
            string strQ = "select PartyId,partyName from mastParty where BeatId=" + BeatID + " and partydist = 0 order by PartyName";
            //  string strQ = "select Distinct(CityID),CityName from ViewGeo VG inner join MastLink ML On VG.cityid=ML.LinkCode where VG.StateId=" + StateID + " and ML.ECode='SA' and ml.PrimCode="+ Settings.Instance.SMID+ " order by CityName";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
            if (dt.Rows.Count > 0)
            {
            }

            return JsonConvert.SerializeObject(dt);
        }

        [WebMethod(EnableSession = true)]
        public string GetAmtAllowedByCity(Int32 CityID)
        {
            DataTable dt;
            string strQ = "select ConveyanceAmt FROM mastemployeecityconvlimit where CityId=" + CityID + " and SMID=" + Settings.Instance.SMID + "";
            dt = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["ConveyanceAmt"].ToString();
            }
            else
            {
                String varname1 = "";
                varname1 = varname1 + "SELECT Isnull(amount, 0) as ConveyanceAmt FROM   mastlocalconveyancelimt WHERE  desid = " + Settings.Instance.DesigID + " AND    citytypeid=(SELECT citytype  FROM   mastarea WHERE  areaid =" + CityID + ")";

                dt = DbConnectionDAL.GetDataTable(CommandType.Text, varname1);
                if (dt.Rows.Count > 0)
                {
                    return dt.Rows[0]["ConveyanceAmt"].ToString();
                }
                else { return "0"; }
            }
        }

        [WebMethod(EnableSession = true)]
        public string PopulateCity(Int32 StateID, string ExptypeVal)
        {
            DataTable dt = null;
            string str = "select ExpenseTypeCode from MastExpenseType where ID=" + ExptypeVal + "";
            string ExpCode = DbConnectionDAL.GetScalarValue(CommandType.Text, str).ToString();
            if (ExpCode == "CONVEYANCE")
            {
                string strChangeCity = @"SELECT AllowChangeCity FROM MastSalesRep where Smid=" + Settings.Instance.SMID;
                string ChangeCity = DbConnectionDAL.GetScalarValue(CommandType.Text, strChangeCity).ToString();
                if (ChangeCity == "True")
                {
                    string strAllowCity = @"SELECT Distinct T2.CityID,T2.CityName FROM MastEmployeeCityConvLimit AS T1
                                                        Inner JOIN ViewGeo AS T2
                                                        ON T1.CityId=T2.cityid WHERE  T1.SmId=" + Settings.Instance.SMID + " and T2.stateid=" + StateID;
                    dt = DbConnectionDAL.GetDataTable(CommandType.Text,strAllowCity);
                }
                else
                {
                    string strQ = "select Distinct(CityID),CityName from ViewGeo VG where VG.StateId=" + StateID + " order by CityName";
                    //  string strQ = "select Distinct(CityID),CityName from ViewGeo VG inner join MastLink ML On VG.cityid=ML.LinkCode where VG.StateId=" + StateID + " and ML.ECode='SA' and ml.PrimCode="+ Settings.Instance.SMID+ " order by CityName";
                    dt = DbConnectionDAL.GetDataTable(CommandType.Text,strQ);
                }
            }
            else
            {
                string strQ = "select Distinct(CityID),CityName from ViewGeo VG where VG.StateId=" + StateID + " order by CityName";
                //  string strQ = "select Distinct(CityID),CityName from ViewGeo VG inner join MastLink ML On VG.cityid=ML.LinkCode where VG.StateId=" + StateID + " and ML.ECode='SA' and ml.PrimCode="+ Settings.Instance.SMID+ " order by CityName";
                dt = DbConnectionDAL.GetDataTable(CommandType.Text,strQ);
            }

            return JsonConvert.SerializeObject(dt);
        }
           [WebMethod(EnableSession = true)]
        public string PopulatePartyDistributorBycity(Int32 CityID, string ExptypeVal)
        {
            string strq = "select ExpenseTypeCode from MastExpenseType where ID=" + ExptypeVal + "";
            string ExpCode = DbConnectionDAL.GetScalarValue(CommandType.Text, strq).ToString();
            string str = "";
            if (ExpCode == "CONVEYANCE")
            {
                string strChangeCity = @"SELECT AllowChangeCity FROM MastSalesRep where Smid=" + Settings.Instance.SMID;
                string ChangeCity = DbConnectionDAL.GetScalarValue(CommandType.Text, strChangeCity).ToString();
                if (ChangeCity == "True")
                {
                    str = "SELECT Isnull( CASE isnull((SELECT Isnull(ConveyanceAmt,0) FROM MastEmployeeCityConvLimit WHERE CityId=" + CityID + " AND SmId=" + Settings.Instance.SMID + "),0) WHEN 0 THEN (Isnull((SELECT Isnull(amount, 0) FROM   mastlocalconveyancelimt WHERE  desid = " + Settings.Instance.DesigID + " AND citytypeid = (SELECT citytype FROM   mastarea WHERE  areaid = " + CityID + " AND areatype ='city')),0)) ELSE (isnull((SELECT Isnull(ConveyanceAmt,0) FROM MastEmployeeCityConvLimit WHERE CityId=" + CityID + "  AND SmId=" + Settings.Instance.SMID + "),0)) end,0) AS ConveyanceAmt ";
                }
                else
                {
                    str =
                        @"SELECT isnull((select isnull(Amount,0) 
from MastLocalConveyanceLimt where DesId=" + Settings.Instance.DesigID + " and CityTypeId=(select CityType from MastArea where AreaId=" + CityID + " and AreaType='city')),0) AS ConveyanceAmt  ";
                }
            }
            else {
                str = @"SELECT '' AS ConveyanceAmt ";
            }
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text,str);
            if (dt.Rows.Count > 0)
            {
            }

            return JsonConvert.SerializeObject(dt);
        }
           
          
           [WebMethod(EnableSession = true)]
           public string GetPerKmRate(Int32 TMID)
           {
               try
               {
                   string message = string.Empty;

                   string strQ = "select isnull(PerKmRate,0) as PerKmRate FROM MastTravelMode where Id=" + TMID + "";
                   return message = DbConnectionDAL.GetScalarValue(CommandType.Text, strQ).ToString();

               }
               catch (Exception ex)
               {

                   return string.Empty;
               }
               
           }
           [WebMethod(EnableSession = true)]
           public string PopulateExpenseParty(Int32 ExpDetailId)
           {
               string strQ = "select mp.partyname,ep.productgroup,ep.remarks from expenseparty ep inner join mastparty mp on ep.partyid=mp.partyid  where ep.ExpenseDetailId=" + ExpDetailId + "";
               DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
               if (dt.Rows.Count > 0)
               {
               }

               return JsonConvert.SerializeObject(dt);
           }

        //Added 4 jan 2016
           [WebMethod(EnableSession = true)]
           public string PopulateSalesPersonByEnviro()
           {
               DataTable dt = Settings.UnderUsers(Settings.Instance.SMID);
               DataView dv = new DataView(dt);
               dv.RowFilter = "RoleType='AreaIncharge' and SMName<>'.'";
               DataTable dtbl = dv.ToTable();
               return JsonConvert.SerializeObject(dtbl);
           }

           [WebMethod(EnableSession = true)]
           public ArrayList BindCountryhead()
           {               
               int smID = Convert.ToInt32(Settings.Instance.SMID);
               ArrayList list = new ArrayList();
               string query = "select SMId,SMName from MastSalesRep ms LEFT JOIN mastrole mr ON ms.RoleId=mr.RoleId where (smid in (select maingrp from MastSalesRepGrp where smid=" + smID + ")  or smid in (select smid from MastSalesRepGrp where maingrp=" + smID + ") ) AND mr.RoleType IN ('Country Head') order by SMName";
               DataTable dt = new DataTable();
               dt = DbConnectionDAL.GetDataTable(CommandType.Text, query);
              
               if (dt.Rows.Count > 0)
               {
                   foreach (DataRow dr in dt.Rows)
                   {
                       list.Add(new ListItem(dr["SMName"].ToString(), dr["SMId"].ToString()));
                   }
               }
               return list;
           }
           [WebMethod(EnableSession = true)]
           public ArrayList PopulateCountryhead()
           {
               int smID = Convert.ToInt32(Settings.Instance.SMID);
               ArrayList list = new ArrayList();
               string query = "select SMId,SMName from MastSalesRep ms LEFT JOIN mastrole mr ON ms.RoleId=mr.RoleId where (smid in (select maingrp from MastSalesRepGrp where smid=" + smID + ")  or smid in (select smid from MastSalesRepGrp where maingrp=" + smID + ") ) AND mr.RoleType IN ('Country Head') order by SMName";
               DataTable dt = new DataTable();
               dt = DbConnectionDAL.GetDataTable(CommandType.Text, query);

               if (dt.Rows.Count > 0)
               {
                   foreach (DataRow dr in dt.Rows)
                   {
                       list.Add(new ListItem(dr["SMName"].ToString(), dr["SMId"].ToString()));
                   }
               }
               return list;
           }
           [WebMethod(EnableSession = true)]
           public ArrayList BindRSM()
           {
               int smID = Convert.ToInt32(Settings.Instance.SMID);
               ArrayList list = new ArrayList();
               string query = "select SMId,SMName from MastSalesRep ms LEFT JOIN mastrole mr ON ms.RoleId=mr.RoleId where (smid in (select maingrp from MastSalesRepGrp where smid=" + smID + ")  or smid in (select smid from MastSalesRepGrp where maingrp=" + smID + ") ) AND mr.RoleType IN ('RegionHead') order by SMName";
               DataTable dt = new DataTable();
               dt = DbConnectionDAL.GetDataTable(CommandType.Text, query);

               if (dt.Rows.Count > 0)
               {
                   foreach (DataRow dr in dt.Rows)
                   {
                       list.Add(new ListItem(dr["SMName"].ToString(), dr["SMId"].ToString()));
                   }
               }
               return list;
           }
           [WebMethod(EnableSession = true)]
           public ArrayList PopulateRSM(int srep)
           {
               int smID = srep;
               ArrayList list = new ArrayList();
               string query = "select SMId,SMName from MastSalesRep ms LEFT JOIN mastrole mr ON ms.RoleId=mr.RoleId where (smid in (select maingrp from MastSalesRepGrp where smid=" + smID + ")  or smid in (select smid from MastSalesRepGrp where maingrp=" + smID + ") ) AND mr.RoleType IN ('RegionHead') order by SMName";
               DataTable dt = new DataTable();
               dt = DbConnectionDAL.GetDataTable(CommandType.Text, query);

               if (dt.Rows.Count > 0)
               {
                   foreach (DataRow dr in dt.Rows)
                   {
                       list.Add(new ListItem(dr["SMName"].ToString(), dr["SMId"].ToString()));
                   }
               }
               return list;
           }           
           [WebMethod(EnableSession = true)]
           public ArrayList BindSM()
           {
               int smID = Convert.ToInt32(Settings.Instance.SMID);
               ArrayList list = new ArrayList();
               string query = "select SMId,SMName from MastSalesRep ms LEFT JOIN mastrole mr ON ms.RoleId=mr.RoleId where (smid in (select maingrp from MastSalesRepGrp where smid=" + smID + ")  or smid in (select smid from MastSalesRepGrp where maingrp=" + smID + ") ) AND mr.RoleType IN ('StateHead') order by SMName";
               DataTable dt = new DataTable();
               dt = DbConnectionDAL.GetDataTable(CommandType.Text, query);

               if (dt.Rows.Count > 0)
               {
                   foreach (DataRow dr in dt.Rows)
                   {
                       list.Add(new ListItem(dr["SMName"].ToString(), dr["SMId"].ToString()));
                   }
               }
               return list;
           }
           [WebMethod(EnableSession = true)]
           public ArrayList PopulateSM(int srep)
           {
               int smID = srep;
               ArrayList list = new ArrayList();
               string query = "select SMId,SMName from MastSalesRep ms LEFT JOIN mastrole mr ON ms.RoleId=mr.RoleId where (smid in (select maingrp from MastSalesRepGrp where smid=" + smID + ")  or smid in (select smid from MastSalesRepGrp where maingrp=" + smID + ") ) AND mr.RoleType IN ('StateHead') order by SMName";
               DataTable dt = new DataTable();
               dt = DbConnectionDAL.GetDataTable(CommandType.Text, query);

               if (dt.Rows.Count > 0)
               {
                   foreach (DataRow dr in dt.Rows)
                   {
                       list.Add(new ListItem(dr["SMName"].ToString(), dr["SMId"].ToString()));
                   }
               }
               return list;
           }
           [WebMethod(EnableSession = true)]
           public ArrayList BindASM()
           {
               int smID = Convert.ToInt32(Settings.Instance.SMID);
               ArrayList list = new ArrayList();
               string query = "select SMId,SMName from MastSalesRep ms LEFT JOIN mastrole mr ON ms.RoleId=mr.RoleId where (smid in (select maingrp from MastSalesRepGrp where smid=" + smID + ")  or smid in (select smid from MastSalesRepGrp where maingrp=" + smID + ") ) AND mr.RoleType IN ('DistrictHead') order by SMName";
               DataTable dt = new DataTable();
               dt = DbConnectionDAL.GetDataTable(CommandType.Text, query);

               if (dt.Rows.Count > 0)
               {
                   foreach (DataRow dr in dt.Rows)
                   {
                       list.Add(new ListItem(dr["SMName"].ToString(), dr["SMId"].ToString()));
                   }
               }
               return list;
           }
           [WebMethod(EnableSession = true)]
           public ArrayList PopulateASM(int srep)
           {
               int smID = srep;
               ArrayList list = new ArrayList();
               string query = "select SMId,SMName from MastSalesRep ms LEFT JOIN mastrole mr ON ms.RoleId=mr.RoleId where (smid in (select maingrp from MastSalesRepGrp where smid=" + smID + ")  or smid in (select smid from MastSalesRepGrp where maingrp=" + smID + ") ) AND mr.RoleType IN ('DistrictHead') order by SMName";
               DataTable dt = new DataTable();
               dt = DbConnectionDAL.GetDataTable(CommandType.Text, query);

               if (dt.Rows.Count > 0)
               {
                   foreach (DataRow dr in dt.Rows)
                   {
                       list.Add(new ListItem(dr["SMName"].ToString(), dr["SMId"].ToString()));
                   }
               }
               return list;
           }
           [WebMethod(EnableSession = true)]
           public ArrayList BindSO()
           {
               int smID = Convert.ToInt32(Settings.Instance.SMID);
               ArrayList list = new ArrayList();
               string query = "select SMId,SMName from MastSalesRep ms LEFT JOIN mastrole mr ON ms.RoleId=mr.RoleId where (smid in (select maingrp from MastSalesRepGrp where smid=" + smID + ")  or smid in (select smid from MastSalesRepGrp where maingrp=" + smID + ") ) AND mr.RoleType IN ('CityHead') order by SMName";
               DataTable dt = new DataTable();
               dt = DbConnectionDAL.GetDataTable(CommandType.Text, query);

               if (dt.Rows.Count > 0)
               {
                   foreach (DataRow dr in dt.Rows)
                   {
                       list.Add(new ListItem(dr["SMName"].ToString(), dr["SMId"].ToString()));
                   }
               }
               return list;
           }
           [WebMethod(EnableSession = true)]
           public ArrayList PopulateSO(int srep)
           {
               int smID = srep;
               ArrayList list = new ArrayList();
               string query = "select SMId,SMName from MastSalesRep ms LEFT JOIN mastrole mr ON ms.RoleId=mr.RoleId where (smid in (select maingrp from MastSalesRepGrp where smid=" + smID + ")  or smid in (select smid from MastSalesRepGrp where maingrp=" + smID + ") ) AND mr.RoleType IN ('CityHead') order by SMName";
               DataTable dt = new DataTable();
               dt = DbConnectionDAL.GetDataTable(CommandType.Text, query);

               if (dt.Rows.Count > 0)
               {
                   foreach (DataRow dr in dt.Rows)
                   {
                       list.Add(new ListItem(dr["SMName"].ToString(), dr["SMId"].ToString()));
                   }
               }
               return list;
           }
           [WebMethod(EnableSession = true)]
           public ArrayList BindTSI()
           {
               int smID = Convert.ToInt32(Settings.Instance.SMID);
               ArrayList list = new ArrayList();
               string query = "select SMId,SMName from MastSalesRep ms LEFT JOIN mastrole mr ON ms.RoleId=mr.RoleId where (smid in (select maingrp from MastSalesRepGrp where smid=" + smID + ")  or smid in (select smid from MastSalesRepGrp where maingrp=" + smID + ") ) AND mr.RoleType IN ('AreaIncharge') order by SMName";
               DataTable dt = new DataTable();
               dt = DbConnectionDAL.GetDataTable(CommandType.Text, query);

               if (dt.Rows.Count > 0)
               {
                   foreach (DataRow dr in dt.Rows)
                   {
                       list.Add(new ListItem(dr["SMName"].ToString(), dr["SMId"].ToString()));
                   }
               }
               return list;
           }
           [WebMethod(EnableSession = true)]
           public ArrayList PopulateTSI(int srep)
           {
               int smID = srep;
               ArrayList list = new ArrayList();
               string query = "select SMId,SMName from MastSalesRep ms LEFT JOIN mastrole mr ON ms.RoleId=mr.RoleId where (smid in (select maingrp from MastSalesRepGrp where smid=" + smID + ")  or smid in (select smid from MastSalesRepGrp where maingrp=" + smID + ") ) AND mr.RoleType IN ('AreaIncharge') order by SMName";
               DataTable dt = new DataTable();
               dt = DbConnectionDAL.GetDataTable(CommandType.Text, query);

               if (dt.Rows.Count > 0)
               {
                   foreach (DataRow dr in dt.Rows)
                   {
                       list.Add(new ListItem(dr["SMName"].ToString(), dr["SMId"].ToString()));
                   }
               }
               return list;
           }
    }
}
