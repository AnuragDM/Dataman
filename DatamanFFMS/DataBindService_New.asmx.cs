using BusinessLayer;
using DAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI.WebControls;

namespace AstralFFMS
{
    /// <summary>
    /// Summary description for DataBindService_New
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
   [System.Web.Script.Services.ScriptService]
    public class DataBindService_New : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod(EnableSession = true)]
        public ArrayList PopulateUser(int EmpId)
        {
            ArrayList list = new ArrayList();
            string str = "";
            if (EmpId > 0)
            {
                str = "select Id,Name from MastLogin where (DeptId is not null) and (DesigId is not null) and (Active=1 or Id in (" + EmpId + "))  order by Name";
            }
            else
            {
                str = "select Id,Name from MastLogin where (DeptId is not null) and (DesigId is not null) and Active=1 order by Name";
            }
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
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
        public ArrayList PopulateReportTo(int ReportTo)
        {
            ArrayList list = new ArrayList();
           
            string str = "";
            if (ReportTo > 0)
            { str = "select SMID,SMName from MastSalesRep inner join MastLogin Ml on MastSalesRep.userId=Ml.Id Inner Join MastRole Mr on Ml.RoleId=Mr.RoleId where 1=1 and RoleType<>'AreaIncharge' and (MastSalesRep.Active='1' or SMID in (" + ReportTo + ")) order by SMName "; }
            else { str = "select SMID,SMName from MastSalesRep  inner join MastLogin Ml on MastSalesRep.userId=Ml.Id Inner Join MastRole Mr on Ml.RoleId=Mr.RoleId where 1=1 and RoleType<>'AreaIncharge' and MastSalesRep.Active='1' order by SMName "; }

            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(new ListItem(dr["SMName"].ToString(), dr["SMID"].ToString()));
                }
            }
            //if (list.Count == 1)
            //    PopulateCities(dt.Rows[0]["Id"].ToString());
            return list;
        }

        [WebMethod(EnableSession = true)]
        public ArrayList PopulateDepartment(int DeptId)
        {
            ArrayList list = new ArrayList();
            string str = "";
            if (DeptId > 0)
                str = "select DepId,DepName from MastDepartment where (active='1' or DepId in(" + DeptId + ")) order by Depname";
            else
                str = "select DepId,DepName from MastDepartment where active='1' order by Depname";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(new ListItem(dr["DepName"].ToString(), dr["DepId"].ToString()));
                }
            }
            //if (list.Count == 1)
            //    PopulateCities(dt.Rows[0]["Id"].ToString());
            return list;
        }

        [WebMethod(EnableSession = true)]
        public ArrayList PopulateDesignation(int DesigId)
        {
            ArrayList list = new ArrayList();
            string str = "";
            if (DesigId > 0)
                str = "select DesId,DesName from MastDesignation where (active='1' or DesId in(" + DesigId + ")) order by Desname";
            else
                str = "select DesId,DesName from MastDesignation where Active='1' order by Desname";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(new ListItem(dr["DesName"].ToString(), dr["DesId"].ToString()));
                }
            }
            //if (list.Count == 1)
            //    PopulateCities(dt.Rows[0]["Id"].ToString());
            return list;
        }
        //ResCentre
        [WebMethod(EnableSession = true)]
        public ArrayList PopulateResCentre(int ResCenId)
        {
            ArrayList list = new ArrayList();
            string str = "";
            if (ResCenId > 0)
                str = "select ResCenId,ResCenName from MastResCentre where (Active='1' or ResCenId in (" + ResCenId + ")) order by ResCenName";
            else
                str = "select ResCenId,ResCenName from MastResCentre where Active='1' order by ResCenName";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(new ListItem(dr["ResCenName"].ToString(), dr["ResCenId"].ToString()));
                }
            }
            //if (list.Count == 1)
            //    PopulateCities(dt.Rows[0]["Id"].ToString());
            return list;
        }


        //City
        [WebMethod(EnableSession = true)]
        public ArrayList PopulateCity(int CityId ,string RecType)
        {
            ArrayList list = new ArrayList();
            string str = "";
            if (RecType != "")
                str = @"select AreaId As cityid ,AreaName As cityName  from MastArea where AreaType='CITY' and Active=1 order by AreaName";
            else
            {
                 if (CityId > 0)
                str = "SELECT DISTINCT T.cityid,T.cityName+' - '+T.districtName AS cityName FROM ViewGeo AS T WHERE T.cityid>0 and (T.cityAct=1  OR T.cityid=" + CityId + ") ORDER BY cityName";
            else
                str = "SELECT DISTINCT T.cityid,T.cityName+' - '+T.districtName AS cityName FROM ViewGeo AS T WHERE T.cityid>0 and T.cityAct=1 ORDER BY cityName";
            }
           
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(new ListItem(dr["cityName"].ToString(), dr["cityid"].ToString()));
                }
            }
            //if (list.Count == 1)
            //    PopulateCities(dt.Rows[0]["Id"].ToString());
            return list;
        }

        [WebMethod(EnableSession = true)]
        public ArrayList PopulateRole(string RoleType)
        {
            ArrayList list = new ArrayList();
            string str = "";

            if ( RoleType != "")
               str = "select RoleId,RoleName from MastRole where roletype='" + RoleType + "' order by RoleName";
            else
               str = "select RoleId,RoleName from MastRole order by RoleName";

            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(new ListItem(dr["RoleName"].ToString(), dr["RoleId"].ToString()));
                }
            }
            //if (list.Count == 1)
            //    PopulateCities(dt.Rows[0]["Id"].ToString());
            return list;
        }
         [WebMethod(EnableSession = true)]
         public ArrayList PopulateCountry(int CountryId)
         {
             ArrayList list = new ArrayList();
             string str = "";

             if (CountryId > 0)
                 str = "select AreaID,AreaName from mastarea where AreaType='Country' and (Active='1' or Areaid in (" + CountryId + "))  order by AreaName";
             else
                 str = "select AreaID,AreaName from mastarea where AreaType='Country' and Active='1' order by AreaName";

             DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
             {
                 foreach (DataRow dr in dt.Rows)
                 {
                     list.Add(new ListItem(dr["AreaName"].ToString(), dr["AreaID"].ToString()));
                 }
             }
             //if (list.Count == 1)
             //    PopulateCities(dt.Rows[0]["Id"].ToString());
             return list;
         }
         [WebMethod(EnableSession = true)]
         public ArrayList PopulateRegion(int regionId)
         {
             ArrayList list = new ArrayList();
             string str = "";

             if (regionId > 0)
                 str = "select AreaID,AreaName from mastarea where AreaType='Region' and (Active='1' or Areaid in (" + regionId + "))  order by AreaName";

             else
                 str = "select AreaID,AreaName from mastarea where AreaType='Region' and Active='1' order by AreaName"; 

             DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
             {
                 foreach (DataRow dr in dt.Rows)
                 {
                     list.Add(new ListItem(dr["AreaName"].ToString(), dr["AreaID"].ToString()));
                 }
             }
             //if (list.Count == 1)
             //    PopulateCities(dt.Rows[0]["Id"].ToString());
             return list;

         }
        [WebMethod(EnableSession=true)]
         public ArrayList PopulateState(int StateId)
        {
            ArrayList list = new ArrayList();
            string str = "";

            if (StateId > 0)
                str = "select AreaID,AreaName from mastarea where AreaType='State' and (Active='1' or Areaid in (" + StateId + "))  order by AreaName";
            else
                str = "select AreaID,AreaName from mastarea where AreaType='State' and Active='1' order by AreaName";

             DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
             {
                 foreach (DataRow dr in dt.Rows)
                 {
                     list.Add(new ListItem(dr["AreaName"].ToString(), dr["AreaID"].ToString()));
                 }
             }
             //if (list.Count == 1)
             //    PopulateCities(dt.Rows[0]["Id"].ToString());
             return list;
        }
        [WebMethod(EnableSession=true)]
        public ArrayList PopulateDistrict(int DistrictId)
        {
            ArrayList list = new ArrayList();
            string str = "";

            if (DistrictId > 0)
                str = "select AreaID,AreaName from mastarea where AreaType='District' and (Active='1' or Areaid in (" + DistrictId + "))  order by AreaName";

            else
                str = "select AreaID,AreaName from mastarea where AreaType='District' and Active='1' order by AreaName";

            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(new ListItem(dr["AreaName"].ToString(), dr["AreaID"].ToString()));
                }
            }
         
            return list;
        }
        [WebMethod(EnableSession=true)]
        public ArrayList PopulateArea(int AreaId ,int CityID )
        {
            ArrayList list = new ArrayList();
            string str = "";

            if (CityID>0)
            {
                str = @"select AreaId,AreaName from MastArea where Underid=" + CityID + " and AreaType='Area' and Active=1 order by AreaName";
            }
            else
            {
                if (AreaId > 0)
                    str = "select AreaID,AreaName from mastarea where AreaType='Area' and (Active='1' or Areaid in (" + AreaId + "))  order by AreaName";

                else
                    str = "select AreaID,AreaName from mastarea where AreaType='Area' and Active='1' order by AreaName";
            }
           

            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(new ListItem(dr["AreaName"].ToString(), dr["AreaID"].ToString()));
                }
            }

            return list;
        }
        [WebMethod(EnableSession=true)]
        public ArrayList PopulateGrade(int GradeId)
        {
            ArrayList list = new ArrayList();
            string str = "";

            if (GradeId > 0)
                str = "select Id,Name from MastGrade where (Active='1' or Id in (" + GradeId + ")) order by name";
            else
                str = "select Id,Name from MastGrade where Active='1' order by name";

            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(new ListItem(dr["Name"].ToString(), dr["Id"].ToString()));
                }
            }
            return list;

        }

        [WebMethod(EnableSession=true)]
        public ArrayList PopulateItemSegment(int SegmentId)
        {
            ArrayList list = new ArrayList();
            string str = "";

            if (SegmentId > 0)
                //strSegment = "select * from MastItemSegment where (Active='1' or Id in ("+SegmentId+")) Order By Name";
                str = "select Id,Name from MastItemSegment where (Active='1' or Id in (" + SegmentId + ")) Order By Name";
            else
                // strSegment = "select * from MastItemSegment  where Active='1' Order By Name";
                str = "select Id,Name from MastItemSegment  where Active='1' Order By Name";

            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(new ListItem(dr["Name"].ToString(), dr["Id"].ToString()));
                }
            }
            return list;
        }

        [WebMethod(EnableSession=true)]
        public ArrayList PopulateItemClass(int ClassId)
        {
            ArrayList list = new ArrayList();
            string str = "";

            if (ClassId > 0)
                //strClass = "select * from MastItemClass where (Active='1' or Id in (" + ClassId + ")) Order By Name";
                str = "select Id,Name from MastItemClass where (Active='1' or Id in (" + ClassId + ")) Order By Name";
            else
                //strClass = "select * from MastItemClass where Active='1' Order By Name";
                str = "select Id,Name from MastItemClass where Active='1' Order By Name";

            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(new ListItem(dr["Name"].ToString(), dr["Id"].ToString()));
                }
            }
            return list;
        }

        [WebMethod(EnableSession=true)]
        public ArrayList PopulateItemGroup(int ParentId)
        {
            ArrayList list = new ArrayList();
            string str = "";

            if (ParentId > 0)
                str = "select ItemId,ItemName from MastItem where ItemType='MATERIALGROUP' and (Active='1' or ItemId in (" + ParentId + ")) Order By ItemName";
            else
                str = "select ItemId,ItemName from MastItem where ItemType='MATERIALGROUP' and Active='1' Order By ItemName";


            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(new ListItem(dr["ItemName"].ToString(), dr["ItemId"].ToString()));
                }
            }
            return list;
        }

       

        [WebMethod(EnableSession = true)]
        public ArrayList PopulateGeolocation()
        {
            ArrayList list = new ArrayList();
            string str = "";

            str = @"select AreaId,DisplayName from mastarea where areatype='COUNTRY' OR areatype='REGION' OR areatype='STATE' OR areatype='CITY' and Active=1 order by AreaName";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(new ListItem(dr["DisplayName"].ToString(), dr["AreaId"].ToString()));
                }
            }
            return list;
        }

         [WebMethod(EnableSession = true)]
        public ArrayList PopulateGeoName(string GeoType)
        {
            ArrayList list = new ArrayList();
            string str = "";

            if (GeoType == "1")
            {
                 str = @"SELECT DISTINCT T1.AreaId,T1.AreaName FROM MastArea AS T1 WHERE T1.AreaType='COUNTRY' and T1.Active=1 ORDER BY T1.AreaName";

            }
            else if (GeoType == "2")
            {
                str = @"SELECT DISTINCT T1.AreaId,T1.AreaName FROM MastArea AS T1 WHERE T1.AreaType='STATE' and T1.Active=1 ORDER BY T1.AreaName";

            }
            else if (GeoType == "3")
            {
                str = @"SELECT DISTINCT T1.AreaId,T1.AreaName FROM MastArea AS T1 WHERE T1.AreaType='CITY' and T1.Active=1 ORDER BY T1.AreaName";

            }
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(new ListItem(dr["AreaName"].ToString(), dr["AreaId"].ToString()));
                }
            }
            return list;
        }
        [WebMethod(EnableSession = true)]
        public ArrayList PopulateNarration()
         {
             ArrayList list = new ArrayList();
             string str = "";

             str = @"SELECT T1.Id,T1.NarrationType FROM MastDsrNarrationType AS T1";
             DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
             {
                 foreach (DataRow dr in dt.Rows)
                 {
                     list.Add(new ListItem(dr["NarrationType"].ToString(), dr["Id"].ToString()));
                 }
             }
             return list;
         }

        [WebMethod(EnableSession=true)]
        public ArrayList PopulateSalesMan(int SalesID)
        {
            ArrayList list = new ArrayList();
            string str = "";

            if (SalesID > 0) { str = "select SMID,SMName from MastSalesRep where Active=1 or SMID in (" + SalesID + ")    order by SMName"; }
            else { str = "select SMID,SMName from MastSalesRep where Active=1  order by SMName"; }
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(new ListItem(dr["SMName"].ToString(), dr["SMID"].ToString()));
                }
            }
            return list;
        }
        [WebMethod(EnableSession=true)]
        public ArrayList PopulatePartyCity(int CityId)
        {
            string str = "";
            ArrayList list = new ArrayList();
            if (Settings.Instance.RoleType == "Admin")
            {
                //if (CityId > 0) { strq = "select AreaId,AreaName from MastArea where areatype in ('CITY')  and (Active='1' or Areaid in (" + CityId + "))    order by AreaName"; }
                //else { strq = "select AreaId,AreaName from MastArea where areatype in ('CITY') and Active='1' order by AreaName"; }

                if (CityId > 0)
                {
                    str = "SELECT DISTINCT T.cityid,T.cityName+' - '+T.districtName AS cityName FROM ViewGeo AS T WHERE T.cityid>0 and (T.cityAct=1  OR T.cityid=" + CityId + ") ORDER BY cityName";
                }
                else
                {
                    str = "SELECT DISTINCT T.cityid,T.cityName+' - '+T.districtName AS cityName FROM ViewGeo AS T WHERE T.cityid>0 and T.cityAct=1 ORDER BY cityName";
                }
            }
            else
            {
                if (CityId > 0)
                {
                    str = @"SELECT DISTINCT cityid,cityName+' - '+districtName AS cityName FROM ViewGeo  where cityid!=0 and cityAct=1 and cityName!='' and CityId in (select AreaId from mastarea where areaid in (select underid from mastarea where areaId " +
                " in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode=" + Convert.ToInt32(Settings.Instance.SMID) + ")) and areatype='City' and Active=1)  ORDER BY cityName";
                }
                else
                {
                    str = @"SELECT DISTINCT cityid,cityName+' - '+districtName AS cityName FROM ViewGeo  where cityid!=0 and (cityAct=1 OR cityid=" + CityId + ") and cityName!='' and CityId in (select AreaId from mastarea where areaid in (select underid from mastarea where areaId " +
               " in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode=" + Convert.ToInt32(Settings.Instance.SMID) + ")) and areatype='City' and Active=1)  ORDER BY cityName";
                }
            }
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(new ListItem(dr["cityName"].ToString(), dr["cityid"].ToString()));
                }
            }
            return list;

        }

        [WebMethod(EnableSession=true)]
        public ArrayList PopulateIndustry(int IndId)
        {
            string str = "";
            ArrayList list = new ArrayList();
            if (IndId > 0)
            {
                str = "select IndId,IndName from MastIndustry where (Active='1' or IndId in (" + IndId + " ))  order by IndName";
            }
            else
            {
                str = "select IndId,IndName from MastIndustry where Active='1' order by IndName";
            }
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(new ListItem(dr["IndName"].ToString(), dr["IndId"].ToString()));
                }
            }
            return list;

        }

        [WebMethod(EnableSession=true)]
        public ArrayList PopulatePartyType()
        {
            string str = "";
            ArrayList list = new ArrayList();
            str = "select PartyTypeId,PartyTypeName from PartyType order by PartyTypeName";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(new ListItem(dr["PartyTypeName"].ToString(), dr["PartyTypeId"].ToString()));
                }
            }
            return list;

        }
        [WebMethod(EnableSession=true)]
        public ArrayList PopulatePartyArea(Int32 CityId, Int32 AreaID)
        {
            string str = "";
            ArrayList list = new ArrayList();
            if (Settings.Instance.RoleType == "Admin")
            {
                if (AreaID > 0)
                { str = "select AreaId,AreaName from MastArea where areatype in ('AREA') and UnderId=" + CityId + " and (Active='1' or Areaid in (" + AreaID + "))    order by AreaName "; }
                else
                {
                    str= "select AreaId,AreaName from MastArea where areatype in ('AREA') and UnderId=" + CityId + "  and  Active='1'  order by AreaName ";
                }
            }
            else
            {
                if (AreaID > 0)
                {
                    str = "select AreaId,AreaName  from MastArea where AreaId in (select MainGrp from MastAreaGrp where areaid in (select linkcode from mastlink where ecode='SA'" +
                  " and PrimCode=" + Settings.Instance.SMID + "))and areatype in ('AREA') and underId=" + CityId + " and (Active='1' or Areaid in (" + AreaID + "))  order by AreaName";
                }
                else
                {
                    str = "select AreaId,AreaName  from MastArea where AreaId in (select MainGrp from MastAreaGrp where areaid in (select linkcode from mastlink where ecode='SA'" +
                        " and PrimCode=" + Settings.Instance.SMID + "))and areatype in ('AREA') and underId=" + CityId + " and  Active='1'  order by AreaName";
                }
            }
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(new ListItem(dr["AreaName"].ToString(), dr["AreaId"].ToString()));
                }
            }
            return list;
        }

        [WebMethod(EnableSession=true)]
        public ArrayList PopulatePartyBeat(Int32 AreaId, Int32 BeatId)
        {
            string str = "";
            ArrayList list = new ArrayList();

            if (BeatId > 0)
            {
                str = "select AreaId,AreaName from MastArea where areatype in ('BEAT') and UnderId=" + AreaId + " and (Active='1' or Areaid in (" + BeatId + ")) order by AreaName ";
            }
            else { str = "select AreaId,AreaName from MastArea where areatype in ('BEAT') and UnderId=" + AreaId + " and Active='1' order by AreaName "; }

            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(new ListItem(dr["AreaName"].ToString(), dr["AreaId"].ToString()));
                }
            }
            return list;
        }

        [WebMethod(EnableSession=true)]
        public ArrayList PopulatePartyDistributor(Int32 DistId)
        {
            string str = "";
            ArrayList list = new ArrayList();

            if (DistId > 0)
            {
                if (Settings.Instance.RoleType == "Admin")
                {
                    str = "select PartyId,PartyName+'('+SyncId+')- '+(select areaname from mastarea where areaId=MastParty.CityId and AreaType='City') as PartyName from MastParty where PartyDist=1 and (Active='1' or PartyId in (" + DistId + ")) order by PartyName";
                }
                else
                {
                    str = "select PartyId,PartyName+'('+SyncId+')- '+(select areaname from mastarea where areaId=MastParty.CityId and AreaType='City') as PartyName from MastParty where PartyDist=1 and (Active='1' or PartyId in (" + DistId + ")) and CityId in (select AreaId from mastarea where areaid in (select underid from mastarea where areaId " +
                   " in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode=" + Convert.ToInt32(Settings.Instance.SMID) + ")) and areatype='City' and Active=1) order by PartyName ";
                }
            }
            else
            {
                if (Settings.Instance.RoleType == "Admin")
                {
                    str = "select PartyId,PartyName+'('+SyncId+')- '+(select areaname from mastarea where areaId=MastParty.CityId and AreaType='City') as PartyName from MastParty where PartyDist=1 and Active=1 order by PartyName";
                }
                else
                {
                    str = "select PartyId,PartyName+'('+SyncId+')- '+(select areaname from mastarea where areaId=MastParty.CityId and AreaType='City') as PartyName from MastParty where PartyDist=1 and Active=1 and CityId in (select AreaId from mastarea where areaid in (select underid from mastarea where areaId " +
                     " in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode=" + Convert.ToInt32(Settings.Instance.SMID) + ")) and areatype='City' and Active=1) order by PartyName ";
                }
            }
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(new ListItem(dr["PartyName"].ToString(), dr["PartyId"].ToString()));
                }
            }
            return list;
        }

        [WebMethod(EnableSession=true)]
        public ArrayList PopulateSalesPerson()
        {
          //  string str = "";
            ArrayList list = new ArrayList();
            DataTable dt1 = Settings.UnderUsers(Settings.Instance.SMID);
            DataView dv = new DataView(dt1);
            string RoleTy = Settings.Instance.RoleType;
            if (RoleTy == "RegionHead" || RoleTy == "StateHead")
            {  
                dv.RowFilter = "RoleType='CityHead' or RoleType='DistrictHead' or RoleType='AreaIncharge' and SMName<>'.'";
                dv.Sort = "SMName asc";               
            }
            else
            {               
                dv.RowFilter = "SMId<>" + Convert.ToInt32(Settings.Instance.SMID) + " and RoleType='AreaIncharge'";
                dv.Sort = "SMName asc";
             }
            //DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            {
                foreach (DataRow dr in dv.Table.Rows)
                {
                    list.Add(new ListItem(dr["SMName"].ToString(), dr["SMId"].ToString()));
                }
            }
            return list;
        }

        [WebMethod(EnableSession=true)]
        public ArrayList PopulateBeatEntrySalesman ()
        {
            ArrayList list = new ArrayList();
            DataTable dt1 = Settings.UnderUsers(Settings.Instance.SMID);
            DataView dv = new DataView(dt1);
            string RoleTy = Settings.Instance.RoleType;
           
            if (RoleTy == "CityHead" || RoleTy == "DistrictHead")
            {            
                dv.RowFilter = "RoleType='CityHead' or RoleType='DistrictHead' or RoleType='AreaIncharge'   and SMName<>'.'";               
            }
            else if (RoleTy == "AreaIncharge")
            {               
                dv.RowFilter = "RoleType='AreaIncharge' and SMName<>'.'";             
            }
            else
            {            
                dv.RowFilter = "RoleType='CityHead' or RoleType='DistrictHead' or RoleType='AreaIncharge' and SMName<>'.'";
                dv.Sort = "SMName asc";         

            }
            //DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            {
                foreach (DataRow dr in dv.Table.Rows)
                {
                    list.Add(new ListItem(dr["SMName"].ToString(), dr["SMId"].ToString()));
                }
            }
            return list;
        }

        [WebMethod(EnableSession = true)]
        public ArrayList PopulateCityByMultiState(string StateID)
        {
            ArrayList list = new ArrayList();
            if (StateID != "")
            {
                string strQ = "select Distinct(CityID),CityName from ViewGeo VG where VG.StateId in (" + StateID + ") and cityact=1 order by CityName";
                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        list.Add(new ListItem(dr["CityName"].ToString(), dr["CityID"].ToString()));
                    }
                }
            }
            return list;
        }
        [WebMethod(EnableSession = true)]
        public ArrayList PopulateDistributorByMultiCity(string CityID)
        {
            ArrayList list = new ArrayList();
            if (CityID != "")
            {
                //string strQ = "select partyid,partyname+'-'+mp.SyncId+'-'+vg.AreaName +'-'+ vg.CityName +'-'+ vg.StateName  as PartyName  from mastparty mp left join [dbo].[ViewGeo] vg on mp.AreaId =vg.areaId Where mp.CityId in("+CityID+")";

                string strQ = "select distinct partyid,partyname+'-'+mp.SyncId+'-'+maa.AreaName +'-'+ ma.AreaName  +'-'+ mas.AreaName   as PartyName  from mastparty mp inner join MastArea maa on mp.AreaId=maa.AreaId inner join MastArea ma on mp.CityId=ma.AreaId inner join MastArea mad on ma.UnderId=mad.AreaId inner join MastArea mas on mad.UnderId=mas.AreaId Where mp.CityId in(" + CityID + ")";
                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        list.Add(new ListItem(dr["PartyName"].ToString(), dr["partyid"].ToString()));
                    }
                }
            }
            return list;
        }
        [WebMethod(EnableSession = true)]
        public ArrayList PopulateMastunit()
        {
            ArrayList list = new ArrayList();
            string str = "";


            str = "select Unit As Id,Unit as Name from MastUnit Order By Unit";

            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(new ListItem(dr["Name"].ToString(), dr["Id"].ToString()));
                }
            }
            return list;
        }
        [WebMethod(EnableSession = true)]
        public ArrayList PopulateSuperDistributor(string disttype)
        {
            ArrayList list = new ArrayList();
            string condition = "";

            if (disttype == "UNDERSD")
                condition = " and isnull(mp.DistType,'')='SUPERDIST' ";
            else if (disttype == "SUPERDIST")
            {
                condition = " and isnull(mp.DistType,'') in (select value from mastdistributortype where sort < (select sort from mastdistributortype where value ='" + disttype + "') ) ";
            }
            //condition = " and isnull(mp.DistType,'')='C&F' ";
            else if (disttype == "C&F")
            {
                condition = " and isnull(mp.DistType,'') in (select value from mastdistributortype where sort < (select sort from mastdistributortype where value ='" + disttype + "') ) ";
            }
            //condition = " and isnull(mp.DistType,'')='DEPO' ";

            string strQ = "select mp.PartyId,mp.PartyName + ' - ' + md.Text + ' ( ' + ma.AreaName + ' )' as PartyName from MastParty mp left join MastDistributorType md on md.Value=mp.DistType left join MastArea ma on ma.AreaId=mp.CityId where isnull(mp.PartyDist,0)=1 " + condition;
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(new ListItem(dr["PartyName"].ToString(), dr["PartyId"].ToString()));
                }
            }

            return list;
        }

        [WebMethod(EnableSession = true)]
        public ArrayList PopulateDistributorType()
        {
            ArrayList list = new ArrayList();
            string strQ = "select Text,Value from MastDistributorType order by sort";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(new ListItem(dr["Text"].ToString(), dr["Value"].ToString()));
                }
            }

            return list;
        }

        //20-07-2021-------- Get Modules

        [WebMethod(EnableSession = true)]
        public ArrayList PopulateModule(int ModuleId)
        {
            ArrayList list = new ArrayList();
            string str = "";

            if (ModuleId > 0)
                str = "select Distinct Parent_id,(Module+'-'+cast(Parent_id as varchar)) as Module from mastpage where Parent_id in (" + ModuleId + ")  order by Module";

            else
                str = "select Distinct Parent_id,(Module+'-'+cast(Parent_id as varchar)) as Module from mastpage";

            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(new ListItem(dr["Module"].ToString(), dr["Parent_id"].ToString()));
                }
            }
            dt.Dispose();
            return list;
        }

    }
}
