using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using BusinessLayer;
using System.Data;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using DAL;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.Net;
using System.IO;
namespace AstralFFMS
{
    /// <summary>
    /// Summary description for CRMDBService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class CRMDBService : System.Web.Services.WebService
    {

        [WebMethod(EnableSession = true)]
        public string GetCustomfieldLineData(string HeaderId)
        {

            DataTable dt = new DataTable();
            if (!string.IsNullOrEmpty(HeaderId) && HeaderId != "0")
            {
                string strQ = "select Replace(convert(varchar(12),[Todate],106),' ','/') As Todate,Replace(convert(varchar(12),[Fromdate],106),' ','/') As Fromdate,ma.AttributeField,ma.AttributFieldType,ma.AttributeData,ma.sort,ma.Active,ma.Header_Id,mh.Title,ma.Custom_Id,mh.[FOR] AS Type from MastActivityCustom ma ";
                strQ += " LEFT JOIN MastActivityCustomHeader mh ON mh.Header_Id=ma.Header_Id  where ma.Header_Id=" + Convert.ToInt32(HeaderId) + " order by AttributeField";
                dt = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
            }
            return JsonConvert.SerializeObject(dt);
        }
       [WebMethod(EnableSession = true)]
       public string GetCustomFieldsForActivity()
       {
           DataTable dt = new DataTable();

           string strQ = "select  replace(convert(varchar(12),[Todate],106),' ','/') As Todate, replace(convert(varchar(12),[Fromdate],106),' ','/') As Fromdate,Title,Header_Id,[FOR] AS Type from MastActivityCustomHeader order by Title";
           dt = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
           return JsonConvert.SerializeObject(dt);
       }
        [WebMethod(EnableSession=true)]
        public string getcountry(string stateid)
        {
            string strQ = " SELECT countryid  FROM ViewGeo WHERE stateid=" + Convert.ToInt32(stateid) + " ";
            DataTable dtcountry = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
            return JsonConvert.SerializeObject(dtcountry);
        }
         [WebMethod(EnableSession = true)]
        public string getcompanydetail(string companyname)
        {
            string strQ = "SELECT * FROM CRM_MastCompany WHERE CompName='" + companyname  + "'";
            DataTable dtcountry = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
            return JsonConvert.SerializeObject(dtcountry);
        }
         [WebMethod(EnableSession = true)]
         public string SaveCustomFieldsForActivity(string Table, string Title, string Fromdate, string ToDate, string For, string Fieldname, string Fieldtype, string Data, string Sorting, string filterctrlno, string parametername, string Active, string CustomIds, string HeadId)
         {
             string result = "0";
             string viewname = "";
             string ins = "";
             int HeaderId = 0;
             string[] arrFieldname = Fieldname.Split(',');
             string[] arrFieldtype = Fieldtype.Split(',');
             string[] arrData = Data.Split('*');
             string[] arrSorting = Sorting.Split(',');
             string[] arrActive = Active.Split(',');
             string[] arrCustomIds = CustomIds.Split(',');
             int existingheader_id = 0;
             try
             {
                 if ((Fieldname == "") || (Title == "") || (Sorting == ""))
                 { result = "-3"; return result; }
                     if (Convert.ToInt16(DAL.DbConnectionDAL.GetScalarValue(CommandType.Text, "select count(*) from [MastActivityCustomHeader] where Fromdate='" + Fromdate + "' and Todate='" + ToDate + "'  and [For] ='" + For + "'")) > 0)     //and Title='" + Title + "'
                     {
                         existingheader_id = DAL.DbConnectionDAL.ExecuteScaler("select Header_Id from [MastActivityCustomHeader] where Fromdate='" + Fromdate + "' and Todate='" + ToDate + "'  and [For] ='" + For + "'");
                         ins = "Update MastActivityCustomHeader set Title ='" + Title.Replace("'", "''") + "',Fromdate ='" + Fromdate + "',Todate ='" + ToDate + "' where Header_Id=" + existingheader_id + "";
                         DAL.DbConnectionDAL.ExecuteQuery(ins);

                         for (int i = 0; i < arrFieldname.Length; i++)
                         {
                             if (Convert.ToInt16(DAL.DbConnectionDAL.GetScalarValue(CommandType.Text, "SELECT count(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE table_name = 'ActivityTransaction' AND column_name = '" + arrFieldname[i].Replace("'", "''") + "'")) > 0)
                             {
                                 if (Convert.ToInt16(DAL.DbConnectionDAL.GetScalarValue(CommandType.Text, "select * from MastActivityCustom where Custom_Id='" + arrCustomIds[i] + "' and Header_Id= '" + existingheader_id + "'")) > 0)
                                     result = "-2";
                                 else
                                     result = "-1";
                             }
                             else
                                 result = "";
                             string strQ = "select * from MastActivityCustom where Custom_Id='" + arrCustomIds[i] + "' and Header_Id= '" + existingheader_id + "'";
                             DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
                             if (dt.Rows.Count == 0)
                             {
                                 if (result != "-2" && result != "-1")
                                 {
                                     ins = "insert into MastActivityCustom (AttributeTable,AttributeField,AttributFieldType,AttributeData,sort,filctrrlbysortno,parametername,Active,Header_Id) values('" + Table + "','" + arrFieldname[i].Replace("'", "''") + "','" + arrFieldtype[i] + "','" + arrData[i] + "'," + arrSorting[i] + ",'" + filterctrlno + "','" + parametername + "','" + arrActive[i] + "'," + existingheader_id + ")";
                                     //ins = "insert into MastActivityCustom (AttributeTable,AttributeField,AttributFieldType,AttributeData,sort,filctrrlbysortno,parametername,Active,Header_Id) values('" + Table + "','" + arrFieldname[i] + "','" + arrFieldtype[i] + "','" + arrData[i] + "'," + arrSorting[i] + ",'" + filterctrlno + "','" + parametername + "','" + arrActive[i] + "'," + existingheader_id + ")";
                                     DAL.DbConnectionDAL.ExecuteQuery(ins);
                                     string datatype = "varchar(25)";
                                     if (arrFieldtype[i] == "Number")
                                     {
                                         datatype = "numeric(18,2)";
                                     }                                     
                                     else if (arrFieldtype[i] == "Date")
                                     {
                                         datatype = "datetime";
                                     }
                                     else if (arrFieldtype[i] == "Single Line Text")
                                     {
                                         datatype = "varchar(25)";
                                     }
                                     else if (arrFieldtype[i] == "Multiple Line Text")
                                     {
                                         datatype = "varchar(max)";
                                     }

                                     string insContact = "alter table [ActivityTransaction] add [" + arrFieldname[i] + "] " + datatype + " null";
                                     DAL.DbConnectionDAL.ExecuteQuery(insContact);
                                 }
                             }
                             else
                             {
                                 ins = "Update MastActivityCustom set Active ='" + arrActive[i] + "', AttributeData='" + arrData[i] + "' where Custom_Id = '" + arrCustomIds[i] + "'";
                                 DAL.DbConnectionDAL.ExecuteQuery(ins);
                             }
                         }
                     }
                     else
                     {

                         if (HeadId != "")
                         {
                             string[] arr = CustomIds.Split(',');
                             ins = "Update MastActivityCustomHeader set Title ='" + Title.Replace("'", "''") + "',Fromdate ='" + Fromdate + "',Todate ='" + ToDate + "' where Header_Id=" + HeadId + "";
                             DAL.DbConnectionDAL.ExecuteQuery(ins);

                             for (int i = 0; i < arr.Length; i++)
                             {
                                 ins = "Update MastActivityCustom set AttributeField='" + arrFieldname[i].Replace("'", "''") + "',AttributFieldType='" + arrFieldtype[i] + "',AttributeData='" + arrData[i] + "',Sort='" + arrSorting[i] + "',Active='" + arrActive[i] + "' where custom_id ='" + arr[i] + "'";
                                 DAL.DbConnectionDAL.ExecuteQuery(ins);
                             }
                         }
                         else if (HeadId == "")
                         {
                             ins = "INSERT INTO MastActivityCustomHeader(Fromdate,	Todate,	[FOR],	Title	) OUTPUT Inserted.Header_Id values('" + Fromdate + "','" + ToDate + "','" + For + "','" + Title.Replace("'", "''") + "')";
                             HeaderId = DAL.DbConnectionDAL.ExecuteScaler(ins);
                             if (HeaderId != 0)
                             {
                                 for (int i = 0; i < arrFieldname.Length; i++)
                                 {
                                     if (Convert.ToInt16(DAL.DbConnectionDAL.GetScalarValue(CommandType.Text, "SELECT count(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE table_name = 'ActivityTransaction' AND column_name = '" + arrFieldname[i].Replace("'", "''") + "'")) > 0)
                                     {
                                         result = "-1";
                                         ins = "delete from MastActivityCustomHeader where Header_Id='" + HeaderId + "'";
                                         DAL.DbConnectionDAL.ExecuteQuery(ins);
                                         return result;
                                     }
                                 }

                                 for (int i = 0; i < arrFieldname.Length; i++)
                                 {

                                     ins = "insert into MastActivityCustom (AttributeTable,AttributeField,AttributFieldType,AttributeData,sort,filctrrlbysortno,parametername,Active,Header_Id) values('" + Table + "','" + arrFieldname[i].Replace("'", "''") + "','" + arrFieldtype[i] + "','" + arrData[i] + "'," + arrSorting[i] + ",'" + filterctrlno + "','" + parametername + "','" + arrActive[i] + "'," + HeaderId + ")";
                                     DAL.DbConnectionDAL.ExecuteQuery(ins);
                                     string datatype = "varchar(25)";
                                     if (arrFieldtype[i] == "Number")
                                     {
                                         datatype = "numeric(18,2)";
                                     }
                                     else if (arrFieldtype[i] == "Date")
                                     {
                                         datatype = "datetime";
                                     }
                                     else if (arrFieldtype[i] == "Single Line Text")
                                     {
                                         datatype = "varchar(25)";
                                     }
                                     else if (arrFieldtype[i] == "Multiple Line Text")
                                     {
                                         datatype = "varchar(max)";
                                     }

                                     string insContact = "alter table [ActivityTransaction] add [" + arrFieldname[i] + "] " + datatype + " null";
                                     DAL.DbConnectionDAL.ExecuteQuery(insContact);

                                 }
                             }
                         }

                     }                 
             }
             catch (Exception Ex)
             {
                 throw Ex;
             }
             return result;
         }
        [WebMethod(EnableSession = true)]
        public string SaveCustomFieldsData(string Table, string Fieldname, string Fieldtype, string Data, int Sort, string filterctrlno, string parametername,bool Active)
        {
            string result = "0";
            string viewname = "";
            string ins = "";
            if (Convert.ToInt16(DAL.DbConnectionDAL.GetScalarValue(CommandType.Text, "select count(*) from [CRM_CustomFields] where AttributeTable='" + Table + "' and AttributeField='" + Fieldname + "'")) > 0)
            {
                result = "-1";
            }

            else
            {
                //if (Fieldtype == "SqlView")
                //{
                //    Fieldtype = "Dropdown";
                //     ins = "insert into CRM_CustomFields (AttributeTable,AttributeField,AttributFieldType,AttributeData) values('" + Table + "','" + Fieldname + "','" + Fieldtype + "','" + Data + "')";
                //}
                ins = "insert into CRM_CustomFields (AttributeTable,AttributeField,AttributFieldType,AttributeData,sort,filctrrlbysortno,parametername,Active) values('" + Table + "','" + Fieldname + "','" + Fieldtype + "','" + Data + "'," + Sort + ",'" + filterctrlno + "','" + parametername + "','" + Active + "')";
                DAL.DbConnectionDAL.ExecuteQuery(ins);
                string datatype = "varchar(25)";
                if (Fieldtype == "Number")
                {
                    datatype = "numeric(18,2)";
                }
                else if (Fieldtype == "Date")
                {
                    datatype = "datetime";
                }
                else if (Fieldtype == "Single Line Text")
                {
                    datatype = "varchar(25)";
                }
                else if (Fieldtype == "Multiple Line Text")
                {
                    datatype = "varchar(max)";
                }
               //if (Active == true)
               //{
                   if (Table == "Contact")
                   {
                       string insContact = "alter table [CRM_MastContact] add [" + Fieldname + "] " + datatype + " null";
                       DAL.DbConnectionDAL.ExecuteQuery(insContact);
                       insContact = "alter table [CRM_MastRawContact] add [" + Fieldname + "] " + datatype + " null";
                       DAL.DbConnectionDAL.ExecuteQuery(insContact);
                   }
                   else
                   {
                       string insCompany = "alter table [CRM_MastCompany] add [" + Fieldname + "] " + datatype + " null";
                       DAL.DbConnectionDAL.ExecuteQuery(insCompany);
                   }
               //}
              
            }
            return result;
        }
        [WebMethod(EnableSession=true)]
        public string UpdateCustomFieldsActive(bool active, string Id)
        {
            string result = "0";
            string upd = "Update CRM_CustomFields set Active='" + active + "' where Custom_Id=" + Id + "";
            DAL.DbConnectionDAL.ExecuteQuery(upd);

            string strQ = " select (case when Isnull(Active,1)=1 then 'Activated' else 'Deactivated' end) As Status FROM [dbo].[CRM_CustomFields] where Custom_Id=" + Id + "  order by AttributeTable,Custom_Id";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
            if (dt.Rows.Count > 0)
            {
            }

            return JsonConvert.SerializeObject(dt);
            //result = "1";
            //return result;
        }
        [WebMethod(EnableSession = true)]
        public string UpdateCustomFields(string Table, string Fieldname, string Data, string Id)
        {
            string result = "0";
            if (Convert.ToInt16(DAL.DbConnectionDAL.GetScalarValue(CommandType.Text, "select count(*) from [CRM_CustomFields] where AttributeTable='" + Table + "' and AttributeField='" + Fieldname + "' and Custom_Id<>" + Id)) > 0)
            {
                result = "-1";
            }
            else
            {
                string getOldFieldname = "select AttributeField FROM [dbo].[CRM_CustomFields] where Custom_Id=" + Id + "";
                string oldfield = DbConnectionDAL.GetScalarValue(CommandType.Text, getOldFieldname).ToString();
                string upd = "Update CRM_CustomFields set AttributeField='" + Fieldname + "',AttributeData='" + Data + "'  where Custom_Id=" + Id + "";
                DAL.DbConnectionDAL.ExecuteQuery(upd);
                if (Table == "Contact")
                {
                    string updContact = "EXEC sp_rename 'CRM_MastContact." + oldfield + "','" + Fieldname + "', 'COLUMN'";
                    DAL.DbConnectionDAL.ExecuteQuery(updContact);
                     updContact = "EXEC sp_rename 'CRM_MastRawContact." + oldfield + "','" + Fieldname + "', 'COLUMN'";
                    DAL.DbConnectionDAL.ExecuteQuery(updContact);
                }
                else
                {
                    string updCompany = "EXEC sp_rename 'CRM_MastCompany." + oldfield + "','" + Fieldname + "', 'COLUMN'";
                    DAL.DbConnectionDAL.ExecuteQuery(updCompany);
                }
            }
            return result;
        }
        [WebMethod(EnableSession = true)]
        public string GetCustomFieldsData()
        {
            string strQ = " select [Custom_Id],[AttributeTable] +' - ('+[AttributeField] +','+[AttributFieldType] +')' as CFdata,(case when Isnull(Active,1)=1 then 'Activated' else 'Deactivated' end) As Status FROM [dbo].[CRM_CustomFields]  order by AttributeTable,Custom_Id";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
            if (dt.Rows.Count > 0)
            {
            }

            return JsonConvert.SerializeObject(dt);
        }
        [WebMethod(EnableSession = true)]
        public string GetCustomFieldsDataById(string Id)
        {
            string strQ = " select *,Isnull(Active,1) As Active1 FROM [dbo].[CRM_CustomFields] where Custom_Id=" + Id + "  order by AttributeTable,Custom_Id";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
            if (dt.Rows.Count > 0)
            {
            }

            return JsonConvert.SerializeObject(dt);
        }
        [WebMethod(EnableSession = true)]
        public void DeleteCustomFieldsById(string Id)
        {
            string strgettable = "select AttributeTable,AttributeField from [dbo].[CRM_CustomFields] where Custom_Id=" + Id + "";
            DataTable dtbl = new DataTable();
            dtbl = DbConnectionDAL.GetDataTable(CommandType.Text, strgettable);
            if (dtbl.Rows.Count > 0)
            {
                string deltablefield = string.Empty;
               
                if (dtbl.Rows[0]["AttributeTable"].ToString() == "Contact")
                { deltablefield = "alter table [CRM_MastContact] drop column [" + dtbl.Rows[0]["AttributeField"] + "]"; }
                else { deltablefield = "alter table [CRM_MastCompany] drop column [" + dtbl.Rows[0]["AttributeField"] + "]"; }

                DAL.DbConnectionDAL.ExecuteQuery(deltablefield);
                if (dtbl.Rows[0]["AttributeTable"].ToString() == "Contact")
                { deltablefield = "alter table [CRM_MastRawContact] drop column [" + dtbl.Rows[0]["AttributeField"] + "]"; }
                DAL.DbConnectionDAL.ExecuteQuery(deltablefield);
                string strQ = "delete FROM [dbo].[CRM_CustomFields] where Custom_Id=" + Id + "";
                DAL.DbConnectionDAL.ExecuteQuery(strQ);
            }
        }

        [WebMethod(EnableSession = true)]
        public string GetCustomFieldsforactivityT(string AttrType, string HeaderId)
        {
            string view_value = "";
            string strQ = "SELECT * FROM MastActivityCustom m LEFT JOIN MastActivityCustomHeader mh ON mh.Header_Id=m.Header_Id where mh.[FOR]='" + AttrType + "' and mh.Header_Id='" + HeaderId + "' AND m.Active=1 ORDER BY m.sort";
            //strQ += " WHERE Replace(Convert(VARCHAR,mh.Fromdate,106),' ','/')='" + Fromdate + "' AND Replace(Convert(VARCHAR,mh.Todate,106),' ','/')='" + todate + "' AND Isnull(m.Active,0)=1 AND mh.[FOR]='" + AttrType + "'";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
            dt.Columns.Add("FieldValues");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["AttributFieldType"].ToString() == "Sql View")
                    {
                        // string view = dt.Rows[i]["AttributeData"].ToString();
                        string[] view = (dt.Rows[i]["AttributeData"].ToString()).Split('*');
                        string sql = "select * from " + view[0];
                        DataTable dtchild = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
                        if (dtchild.Rows.Count > 0)
                        {
                            for (int j = 0; j < dtchild.Rows.Count; j++)
                            {
                                view_value += dtchild.Rows[j]["name"].ToString() + "*#";
                            }
                            view_value = view_value.TrimStart('#');
                            view_value = view_value.TrimStart('*');
                        }
                        dt.Rows[i]["AttributeData"] = "";
                        dt.Rows[i]["AttributeData"] = view_value;
                        dt.AcceptChanges();
                        view_value = "";
                    }

                }
            }

            return JsonConvert.SerializeObject(dt);
        }

        [WebMethod(EnableSession = true)]
        public string GetActivityTransactionList(int VisId, int HeaderId)
        {
            DataTable dt = new DataTable();
            string strQ = "select  replace(convert(varchar(12),at.[Todate],106),' ','/') As Todate, replace(convert(varchar(12),at.[Fromdate],106),' ','/') As Fromdate,Activity_Id,ForType,VisId,HeaderId,Title from ActivityTransaction at inner join MastActivityCustomHeader mac on at.HeaderId=mac.Header_Id  where visid='" + VisId + "'";
            //string strQ = "select  * from ActivityTransaction where visid='" + VisId + "' and HeaderID='" + HeaderId + "' and getdate() between fromdate and todate+1";
            dt = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
            return JsonConvert.SerializeObject(dt);
        }

        [WebMethod(EnableSession = true)]
        public string GetActivityTransTitleList(string ForType)
        {
            DataTable dt = new DataTable();
            string strQ = "select header_id,title from MastActivityCustomHeader where [for]='" + ForType + "' and getdate() between fromdate and todate+1 ORDER BY Title";
            dt = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
            return JsonConvert.SerializeObject(dt);
        }

        [WebMethod(EnableSession = true)]
        public string GetActivityTransactionFill(int VisId, int HeaderId)
        {
            //  DataTable dt = new DataTable();
            DataTable dtField = new DataTable();
            DataTable dtActivity = new DataTable();
            string qry = "select distinct AttributeField,AttributFieldType from MastActivityCustom where Header_Id=" + HeaderId + "";
            dtField = DbConnectionDAL.GetDataTable(CommandType.Text, qry);
            dtField.Columns.Add("AttributeValue");
            if (dtField.Rows.Count > 0)
            {
                for (int i = 0; i < dtField.Rows.Count; i++)
                {
                    string strQ = "select  * from ActivityTransaction where visid='" + VisId + "' and HeaderId=" + HeaderId + "";
                    dtActivity = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
                    if (dtActivity.Rows.Count > 0)
                    {
                        dtField.Rows[i]["AttributeValue"] = dtActivity.Rows[0][dtField.Rows[i]["AttributeField"].ToString()];
                        dtField.AcceptChanges();
                    }
                }
            }
            return JsonConvert.SerializeObject(dtField);
        }


        [WebMethod(EnableSession = true)]
        public string GetCustomFieldsByAttrTable(string AttrType)
        {
            string view_value = "";
            string strQ = " select * FROM [dbo].[CRM_CustomFields] where [AttributeTable]='" + AttrType + "'  and Isnull(Active,1)=1 order by sort";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
            // dt.Columns.Add("viewname");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["AttributFieldType"].ToString() == "Sql View")
                    {
                        // string view = dt.Rows[i]["AttributeData"].ToString();
                        string[] view = (dt.Rows[i]["AttributeData"].ToString()).Split('*');
                        string sql = "select * from " + view[0];
                        DataTable dtchild = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
                        if (dtchild.Rows.Count > 0)
                        {
                            for (int j = 0; j < dtchild.Rows.Count; j++)
                            {
                                view_value += dtchild.Rows[j]["name"].ToString() + "*#";
                            }
                            view_value = view_value.TrimStart('#');
                            view_value = view_value.TrimStart('*');
                        }
                        dt.Rows[i]["AttributeData"] = "";
                        dt.Rows[i]["AttributeData"] = view_value;
                        dt.AcceptChanges();
                        view_value = "";
                    }
                  
                }
            }

            return JsonConvert.SerializeObject(dt);
        }
        //abhishek jaiswal

        [WebMethod(EnableSession = true)]
        public string GetDataPhoneEmailUrl(string AttrType)
        {
             string view = "";
            string strQ = " select * FROM [dbo].[CRM_MastContactType] where [Data]='" + AttrType + "'  order by sort";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
            dt.Columns.Add("concatedvalue");
            dt.AcceptChanges();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                  
                    {
                      
                        // string view = dt.Rows[i]["AttributeData"].ToString();
                         view += dt.Rows[i]["value"].ToString()+",";
                        
                    }

                }
                view=view.TrimEnd(',');
                dt.Rows[0]["concatedvalue"] = view;
                dt.AcceptChanges();
               
            }

            return JsonConvert.SerializeObject(dt);
        }
        [WebMethod(EnableSession = true)]
        public string GetCustomStoreprocedureFieldsData(string AttrType, string DdlId, string DDLValue)
        {
            string view_value = "";
            DataTable dtproc = new DataTable();
            string strQ = " select * FROM [dbo].[CRM_CustomFields] where [AttributeTable]='" + AttrType + "'  order by Custom_Id";
            //  string strQ = " select * FROM [dbo].[CRM_CustomFields] where [AttributeTable]='" + AttrType + "' and AttributeField='" + DdlId.Trim()+ "'  order by Custom_Id";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
            // dt.Columns.Add("viewname");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["AttributFieldType"].ToString() == "Sql View")
                    {
                        // string view = dt.Rows[i]["AttributeData"].ToString();
                        string[] view = (dt.Rows[i]["AttributeData"].ToString()).Split('*');
                        string sql = "select * from " + view[0];
                        DataTable dtchild = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
                        if (dtchild.Rows.Count > 0)
                        {
                            for (int j = 0; j < dtchild.Rows.Count; j++)
                            {
                                view_value += dtchild.Rows[j]["name"].ToString() + "*#";
                            }
                            view_value = view_value.TrimStart('#');
                            view_value = view_value.TrimStart('*');
                        }
                        dt.Rows[i]["AttributeData"] = "";
                        dt.Rows[i]["AttributeData"] = view_value;
                        dt.AcceptChanges();
                        view_value = "";
                    }
                    if (dt.Rows[i]["AttributeField"].ToString() == DdlId.Trim())
                    {

                        string[] view = (dt.Rows[i]["filctrrlbysortno"].ToString()).Split(',');
                        for (int k = 0; k < view.Length; k++)
                        {
                            string sql = " select AttributeData FROM [dbo].[CRM_CustomFields] where  [AttributFieldType]='Sql Procedure' and sort=" + view[k] + "  order by Custom_Id";
                            //dtproc  = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
                            for (int l = 0; l < dt.Rows.Count; l++)
                            {
                                if ((dt.Rows[l]["AttributFieldType"].ToString() == "Sql Procedure") && (dt.Rows[l]["sort"].ToString() == view[k]))
                                {
                                    if (dt.Rows[i]["parametername"].ToString() != "")
                                    {
                                        DbParameter[] dbParam = new DbParameter[2];
                                        string parametern = dt.Rows[i]["parametername"].ToString();
                                        if (parametern == "@parameter1")
                                        {

                                            dbParam[0] = new DbParameter("@parameter1", DbParameter.DbType.VarChar, 30, DDLValue);
                                            dbParam[1] = new DbParameter("@parameter2", DbParameter.DbType.VarChar, 30, "00");
                                        }
                                        else
                                        {

                                            dbParam[0] = new DbParameter("@parameter1", DbParameter.DbType.VarChar, 30, "00");
                                            dbParam[1] = new DbParameter("@parameter2", DbParameter.DbType.VarChar, 30, DDLValue);
                                        }


                                        DataTable dtprocedure = DbConnectionDAL.GetDataTable(CommandType.StoredProcedure, dt.Rows[l]["AttributeData"].ToString(), dbParam);

                                        for (int j = 0; j < dtprocedure.Rows.Count; j++)
                                        {
                                            view_value += dtprocedure.Rows[j]["name"].ToString() + "*#";
                                        }
                                        view_value = view_value.TrimEnd('#');
                                        view_value = view_value.TrimEnd('*');
                                    }
                                    dt.Rows[l]["AttributeData"] = "";
                                    dt.Rows[l]["AttributeData"] = view_value;
                                    dt.AcceptChanges();
                                    view_value = "";
                                }
                            }

                        }

                    }


                }
            }

            return JsonConvert.SerializeObject(dt);
        }
        [WebMethod(EnableSession = true)]
        public string FillDropDown(string AttrType, string DdlId, string DDLValue)
        {
            return JsonConvert.SerializeObject("");

        }
        [WebMethod(EnableSession = true)]
        public string GetCustomStoreprocedureFieldsData_testing(string AttrType, string DdlId, string DDLValue)
        {
            string view_value = ""; string strQ = ""; DataTable dtforsp = new DataTable();
            DataTable dtproc = new DataTable();
            strQ = " select * FROM [dbo].[CRM_CustomFields] where [AttributeTable]='" + AttrType + "' and attributefield='" + DdlId + "'  order by Custom_Id";
            //  string strQ = " select * FROM [dbo].[CRM_CustomFields] where [AttributeTable]='" + AttrType + "' and AttributeField='" + DdlId.Trim()+ "'  order by Custom_Id";
            DataTable dtsortnoofview = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
            // dt.Columns.Add("viewname");
            if (dtsortnoofview.Rows.Count > 0)
            {
                //for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //if (dt.Rows[0]["AttributFieldType"].ToString() == "Sql View")
                    //{
                    //    // string view = dt.Rows[i]["AttributeData"].ToString();
                    //    string[] view = (dt.Rows[0]["AttributeData"].ToString()).Split('*');
                    //    string sql = "select * from " + view[0];
                    //    DataTable dtchild = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
                    //    if (dtchild.Rows.Count > 0)
                    //    {
                    //        for (int j = 0; j < dtchild.Rows.Count; j++)
                    //        {
                    //            view_value += dtchild.Rows[j]["name"].ToString() + "*#";
                    //        }
                    //        view_value = view_value.TrimStart('#');
                    //        view_value = view_value.TrimStart('*');
                    //    }
                    //    dt.Rows[0]["AttributeData"] = "";
                    //    dt.Rows[0]["AttributeData"] = view_value;
                    //    dt.AcceptChanges();
                    //    view_value = "";
                    //}
                   // if (dt.Rows[0]["AttributeField"].ToString() == DdlId.Trim())
                    {
                        string[] procsortlist  ;
                        string sortnoview = dtsortnoofview.Rows[0]["sort"].ToString();
                        strQ = " select * FROM [dbo].[CRM_CustomFields] where [AttributeTable]='" + AttrType + "' and AttributFieldType='Sql Procedure'   order by Custom_Id";
                         dtforsp = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
                        for (int l = 0; l < dtforsp.Rows.Count; l++)
                        {
                           // if (dt.Rows[l]["AttributFieldType"].ToString() == "Sql Procedure")
                            {
                                procsortlist = (dtforsp.Rows[l]["parametername"].ToString()).Split(',');
                                if(procsortlist.Contains(sortnoview))
                                {
                                    int index1 = Array.IndexOf(procsortlist, sortnoview);
                                   // index1 = index1 + 1;

                                   // dt.Rows[l]["AttributeData"].ToString()
                                    if (dtforsp.Rows[l]["AttributeData"].ToString() != "")
                                    {
                                        string[] proc = (dtforsp.Rows[l]["AttributeData"].ToString()).Split('*');
                                        string procedure_count = @"SELECT  p.name AS Parameter,  t.name AS [Type],p.max_length as parameter_length FROM sys.procedures sp
                                                                  JOIN sys.parameters p    ON sp.object_id = p.object_id JOIN sys.types t  
                                                                  ON p.system_type_id = t.system_type_id WHERE sp.name ='" + proc[0] + "'";
                                        DataTable dtParameter = DbConnectionDAL.GetDataTable(CommandType.Text, procedure_count);
                                        if(dtParameter.Rows.Count>0)
                                        {
                                            int count = dtParameter.Rows.Count;
                                            DbParameter[] dbParam = new DbParameter[count];
                                          //  string parametern = dt.Rows[i]["parametername"].ToString();
                                            for (int m = 0; m < count;m++ )
                                            {

                                                if (m == index1)
                                                {
                                                    dbParam[m] = new DbParameter(dtParameter.Rows[m]["Parameter"].ToString(), DbParameter.DbType.VarChar, Convert.ToInt32(dtParameter.Rows[m]["parameter_length"].ToString()), DDLValue);
                                                }
                                                else
                                                {
                                                    dbParam[m] = new DbParameter(dtParameter.Rows[m]["Parameter"].ToString(), DbParameter.DbType.VarChar, Convert.ToInt32(dtParameter.Rows[m]["parameter_length"].ToString()), "00");
                                                }

                                            }

                                            DataTable dtprocedure = DbConnectionDAL.GetDataTable(CommandType.StoredProcedure, proc[0], dbParam);

                                            for (int j = 0; j < dtprocedure.Rows.Count; j++)
                                            {
                                                view_value += dtprocedure.Rows[j]["name"].ToString() + "*#";
                                            }
                                            view_value = view_value.TrimEnd('#');
                                            view_value = view_value.TrimEnd('*');
                                        }
                                  
                                    }
                                    dtforsp.Rows[l]["AttributeData"] = "";
                                    dtforsp.Rows[l]["AttributeData"] = view_value;
                                    dtforsp.AcceptChanges();
                                        view_value = "";
                                }

                            }
                        }
                      

                    }


                }
            }

            return JsonConvert.SerializeObject(dtforsp);
        }
        //[WebMethod(EnableSession = true)]
        //public string GetContactsType(string Value)
        //{
        //    string strQ = "select * from CRM_MastContactType where value='" + Value + "'  order by sort asc";
        //    //  " select * FROM [dbo].[CRM_CustomFields] where [AttributeTable]='" + Value + "'  order by Custom_Id";
        //    DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
        //    if (dt.Rows.Count > 0)
        //    {
        //    }

        //    return JsonConvert.SerializeObject(dt);
        //}
        [WebMethod(EnableSession = true)]
        public string GetDataForCustomFields(string AttrType, string Contactid)
        {
            string view_value = "";
            string strQ = " select * FROM [dbo].[CRM_CustomFields] where [AttributeTable]='" + AttrType + "' and Isnull(Active,1)=1  order by sort";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["AttributFieldType"].ToString() == "Sql View")
                    {
                        // string view = dt.Rows[i]["AttributeData"].ToString();
                        string[] view = (dt.Rows[i]["AttributeData"].ToString()).Split('*');
                        string sql = "select * from " + view[0];
                        DataTable dtchild = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
                        if (dtchild.Rows.Count > 0)
                        {
                            for (int j = 0; j < dtchild.Rows.Count; j++)
                            {
                                view_value += dtchild.Rows[j]["name"].ToString() + "*#";
                            }
                            view_value = view_value.TrimStart('#');
                            view_value = view_value.TrimStart('*');
                        }
                        dt.Rows[i]["AttributeData"] = "";
                        dt.Rows[i]["AttributeData"] = view_value;
                        dt.AcceptChanges();
                        view_value = "";
                    }

                }

                dt.Columns.Add("ctrlvalue");
                dt.AcceptChanges();
                //            string str = @"  select cmco.CompName,cms.Status,ccu.URL,cce.Email,ccm.Phone , cmc.* from CRM_MastContact cmc left join CRM_ContactURL ccu on cmc.Contact_Id=ccu.Contact_Id
                //                             left join CRM_ContactEmail cce on cce.Contact_Id=cmc.Contact_Id
                //                             left join CRM_ContactMobile ccm on ccm.Contact_Id=cmc.Contact_Id
                //                             left join CRM_MastStatus cms on cms.Status_Id=cmc.Status_Id
                //                            left join CRM_MastCompany cmco on cmco.Comp_Id=cmc.CompanyId where cmc.Contact_Id=" + Convert.ToInt32(Contactid);
                string str = @"  select * from CRM_MastContact where Contact_Id=" + Convert.ToInt32(Contactid);
                DataTable ValueDt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int j = 0; j < ValueDt.Columns.Count; j++)
                        {
                            string sw = dt.Rows[i]["AttributeField"].ToString().ToUpper();
                            string ew = ValueDt.Columns[j].ToString().ToUpper();
                            // if (dtCustomfield.Rows[i]["AttributeField"] ==ValueDt.Columns[j])
                            int s = sw.CompareTo(ew);
                            if (sw.CompareTo(ew) == 0)
                            {
                                //hidcustomval.Value += dtCustomfield.Rows[i]["AttributeField"].ToString() +":"+ ValueDt.Columns[j].ToString()+",";
                                //    hidchk.Value += dt.Rows[i]["AttributeField"].ToString() + ":" + ValueDt.Rows[0][ValueDt.Columns[j].ToString()].ToString() + ",";
                                //  dt.Columns.Add(sw);
                                // dt.AcceptChanges();
                                if (ValueDt.Rows[0][ValueDt.Columns[j].ToString()].ToString() == "01/01/1900 12:00:00 AM")
                                {
                                    dt.Rows[i]["ctrlvalue"] = DateTime.Now.ToUniversalTime().AddSeconds(19800).ToString();
                                }
                                else
                                {
                                    dt.Rows[i]["ctrlvalue"] = ValueDt.Rows[0][ValueDt.Columns[j].ToString()].ToString();
                                }
                                dt.AcceptChanges();
                            }

                        }

                    }

                }
                //if (dt.Rows.Count > 0)
                //{
                //}
            }
                return JsonConvert.SerializeObject(dt);
            
        }

        [WebMethod(EnableSession = true)]
        public string GetTasksData(string contactId,string username,string Adate)
        {
            string loginusersmid = Settings.Instance.SMID; string loginusername = Settings.Instance.UserName; string strQ = ""; string AssignedTo = ""; string subqry = "";
         
            string addqry = "";
            if (!string.IsNullOrEmpty(contactId) && contactId != "0")
            {
                addqry += " and mct.Contact_Id=" + contactId + "";
            }
            if(username!="0")
            {
                if (username == "Me")
                {

                    addqry += " and mct.Manager In(" + Settings.Instance.SMID + ") ";
                   // AssignedTo += "and AssignedTo = '" + Settings.Instance.SMID + "'";


                    subqry = "UNION   SELECT isnull(cmt.Tag,'') as Tag,cms.status, ct.Task,ct.Contact_Id , Isnull(firstname,'') + ' ' + Isnull(lastname,'')  AS NAME,";
                    subqry += " '' As compname,ms.SMName AS Towner,(CASE ct.status  WHEN 'c' THEN 'Close' ELSE 'Open'  END  ) AS Tstatus,";
   subqry += "  isnull(CONVERT(VARCHAR(6), ct.AssignDate, 106),'')   as assgndate,    isnull(CONVERT(VARCHAR(10), ct.AssignDate, 101),'')   as assgndate1,";
     subqry += " CONVERT(VARCHAR(10),dateadd(WW,1,cc.CreatedDate), 101)  as weekdt,  CONVERT(VARCHAR(10),CONVERT(VARCHAR(10),GETDATE(), 101), 101)  as currentdt,CC.Flag as Flag,isnull(ct.AssignDate,'')  as Adate ";

 
 subqry += " FROM CRM_TASK ct LEFT JOIN CRM_MastContact CC ON CC.Contact_Id=ct.Contact_Id  left JOIN CRM_MastTag cmt  ON cc.Tag_Id = cmt.Tag_Id ";
  subqry += " LEFT JOIN MastSalesRep ms ON ms.SMId=cc.Manager  ";
  subqry += " INNER JOIN crm_maststatus cms  ON cc.status_id = cms.status_id  WHERE ct.AssignedTo=" + Settings.Instance.SMID + " ";

                }
                else if (username == "All")
                {
                    if (Settings.Instance.UserName.ToUpper() == "SA")
                    {

                        strQ = " Select SMID,SMName from [MastSalesRep]   order by SMName "; //////////where SMName <>'.'
                    }

                    else
                    {
                        string comma = "'";
                        DataTable dtOwner = Settings.UnderUsers(Settings.Instance.SMID);
                        strQ = "";
                        for (int i = 0; i < dtOwner.Rows.Count; i++)
                        {

                            strQ += dtOwner.Rows[i]["SMID"].ToString() + ',';
                        }
                        strQ = strQ.TrimEnd(',');
                        addqry += " and mct.Manager In(" + strQ + ") ";
                        subqry = "UNION   SELECT isnull(cmt.Tag,'') as Tag,cms.status, ct.Task,ct.Contact_Id ,Isnull(firstname,'') + ' ' + Isnull(lastname,'')   AS NAME,";
                        subqry += " '' As compname,ms.SMName AS Towner,(CASE ct.status  WHEN 'c' THEN 'Close' ELSE 'Open'  END  ) AS Tstatus,";
                        subqry += "  isnull(CONVERT(VARCHAR(6), ct.AssignDate, 106),'')   as assgndate,    isnull(CONVERT(VARCHAR(10), ct.AssignDate, 101),'')   as assgndate1,";
                        subqry += " CONVERT(VARCHAR(10),dateadd(WW,1,cc.CreatedDate), 101)  as weekdt,  CONVERT(VARCHAR(10),CONVERT(VARCHAR(10),GETDATE(), 101), 101)  as currentdt,CC.Flag as Flag,isnull(ct.AssignDate,'')  as Adate ";


                        subqry += " FROM CRM_TASK ct LEFT JOIN CRM_MastContact CC ON CC.Contact_Id=ct.Contact_Id  left JOIN CRM_MastTag cmt  ON cc.Tag_Id = cmt.Tag_Id ";
                        subqry += " LEFT JOIN MastSalesRep ms ON ms.SMId=cc.Manager  ";
                        subqry += " INNER JOIN crm_maststatus cms  ON cc.status_id = cms.status_id  WHERE ct.AssignedTo In(" + strQ + ") ";

                    }
                }
                else
                {
                  
                    addqry += " and msr.SMName='" + username + "'";
            
                }
            }
            else
            {
                if (Settings.Instance.UserName.ToUpper() == "SA")
                {

                }
                else
                {
                       addqry += " and mct.Manager In(" + Settings.Instance.SMID + ") ";
                       subqry = "UNION   SELECT  isnull(cmt.Tag,'') as Tag,cms.status, ct.Task,ct.Contact_Id , Isnull(firstname,'') + ' ' + Isnull(lastname,'')  AS NAME,";
                       subqry += " '' As compname,ms.SMName AS Towner,(CASE ct.status  WHEN 'c' THEN 'Close' ELSE 'Open'  END  ) AS Tstatus,";
                       subqry += "  isnull(CONVERT(VARCHAR(6), ct.AssignDate, 106),'')   as assgndate,    isnull(CONVERT(VARCHAR(10), ct.AssignDate, 101),'')   as assgndate1,";
                       subqry += " CONVERT(VARCHAR(10),dateadd(WW,1,cc.CreatedDate), 101)  as weekdt,  CONVERT(VARCHAR(10),CONVERT(VARCHAR(10),GETDATE(), 101), 101)  as currentdt,CC.Flag as Flag,isnull(ct.AssignDate,'')  as Adate ";

 
 subqry += " FROM CRM_TASK ct LEFT JOIN CRM_MastContact CC ON CC.Contact_Id=ct.Contact_Id  left JOIN CRM_MastTag cmt  ON cc.Tag_Id = cmt.Tag_Id ";
  subqry += " LEFT JOIN MastSalesRep ms ON ms.SMId=cc.Manager ";
  subqry += " INNER JOIN crm_maststatus cms  ON cc.status_id = cms.status_id  WHERE ct.AssignedTo=" + Settings.Instance.SMID + " ";
                }
              
            }

            string outercondition="";
            if (Adate == "0" || Adate == "")
            {
                outercondition = " where Adate<=Getdate()";
            }
            else
            {
               
                outercondition = " where Adate<='" + Settings.dateformat(Adate) + " 23:59'"; 
            }
            String varname1 = "";
            varname1 = varname1 + " SELECT Max(Tag) AS Tag,Max(status) AS status,Max(task) AS task, contact_id,Max(NAME) AS NAME ,Max(compname) AS compname,Max(Towner) AS Towner," + "\n";
            varname1 = varname1 + " Max(Tstatus) AS Tstatus,Max(assgndate) AS assgndate ,Max(assgndate1) AS assgndate1,Max(weekdt) AS weekdt,Max(currentdt) AS currentdt,max(Flag) as Flag,max(Adate) as Adate " + "\n";
            varname1 = varname1 + "From(select   isnull(cmt.Tag,'') as Tag,cms.status, isnull((select top(1) isnull(Task,'')  from crm_task where Contact_Id=mct.Contact_Id  " + AssignedTo +" order by CreatedDate desc),'') as task, " + "\n";
            varname1 = varname1 + "       mct.contact_id, " + "\n";
            varname1 = varname1 + "       firstname + ' ' + lastname  AS NAME, " + "\n";
            varname1 = varname1 + "       cmc.compname, Isnull(msr.SMName,'') As Towner, " + "\n";
            varname1 = varname1 + "  isnull( (select  top(1) CASE ct.status  WHEN 'c' THEN 'Close' ELSE 'Open' " + "\n";
            varname1 = varname1 + "       END    from crm_task ct where ct.Contact_Id=mct.Contact_Id  " + AssignedTo + " order by CreatedDate desc),'') as Tstatus, " + "\n";
            varname1 = varname1 + " " + "\n";

            varname1 = varname1 + " (select  CASE when (select top (1) isnull(ct1.AssignDate,'') from CRM_Task ct1 where ct1.Contact_Id=mct.Contact_Id  " + AssignedTo + "  order by CreatedDate desc) <>''" + "\n";
            varname1 = varname1 + " THEN (select top (1) isnull(CONVERT(VARCHAR(6), ct1.AssignDate, 106),'') from CRM_Task ct1 where ct1.Contact_Id=mct.Contact_Id " + AssignedTo + " order by CreatedDate desc) " + "\n";
            varname1 = varname1 + " ELSE convert(varchar(6),mct.CreatedDate,106) END   ) as assgndate," + "\n";
            varname1 = varname1 + "    (select  CASE when (select top (1) isnull(ct1.AssignDate,'') from CRM_Task ct1 where ct1.Contact_Id=mct.Contact_Id " + AssignedTo + " order by CreatedDate desc) <> '' " + "\n";
            varname1 = varname1 + " THEN (select top (1) isnull(CONVERT(VARCHAR(10), ct1.AssignDate, 101),'') from CRM_Task ct1 where ct1.Contact_Id=mct.Contact_Id " + AssignedTo + " order by CreatedDate desc) " + "\n";
            varname1 = varname1 + " ELSE convert(varchar(10),mct.CreatedDate,101) END   ) as assgndate1, " + "\n";
            varname1 = varname1 + " (select case when (select top (1) CONVERT(VARCHAR(10),dateadd(WW,1,ct1.AssignDate), 101) from CRM_Task ct1 where ct1.Contact_Id=mct.Contact_Id " + AssignedTo + " order by CreatedDate desc)<>'' " + "\n";
            varname1 = varname1 + " then (select top (1) CONVERT(VARCHAR(10),dateadd(WW,1,mct.CreatedDate), 101) from CRM_Task ct1 where ct1.Contact_Id=mct.Contact_Id " + AssignedTo + " order by CreatedDate desc) " + "\n";
            varname1 = varname1 + "  else CONVERT(VARCHAR(10),dateadd(WW,1,mct.CreatedDate), 101) end) as weekdt, " + "\n";
            varname1 = varname1 + " (select  CONVERT(VARCHAR(10),CONVERT(VARCHAR(10),GETDATE(), 101), 101) ) " + "\n";
            varname1 = varname1 + "  as currentdt,mct.Flag as Flag, (select  CASE when (select top (1) isnull(ct1.AssignDate,'') from CRM_Task ct1 " + "\n";
            varname1 = varname1 + " where ct1.Contact_Id=mct.Contact_Id " + AssignedTo + " order by CreatedDate desc) <>'' " + "\n";
            varname1 = varname1 + "THEN (select top (1) isnull(ct1.AssignDate,'') from CRM_Task ct1 where ct1.Contact_Id=mct.Contact_Id " + AssignedTo + " order by CreatedDate desc)  " + "\n";
            varname1 = varname1 + " ELSE isnull(mct.CreatedDate,'') END   ) as Adate " + "\n";
          
            varname1 = varname1 + "FROM   [crm_mastcontact] mct " + "\n";
            varname1 = varname1 + "    left JOIN CRM_MastTag cmt  ON mct.Tag_Id = cmt.Tag_Id    INNER JOIN crm_mastcompany cmc " + "\n";
            varname1 = varname1 + "               ON mct.companyid = cmc.comp_id " + "\n";
            varname1 = varname1 + "       INNER JOIN crm_maststatus cms " + "\n";
            varname1 = varname1 + "               ON mct.status_id = cms.status_id " + "\n";
            varname1 = varname1 + "          Left JOIN MastSalesRep msr on msr.SMID = mct.Manager " + "\n";
           
            varname1 = varname1 + "WHERE  1 = 1 " + addqry + "" + "\n";
            varname1 = varname1 + " UNION " + "\n";
            varname1 = varname1 + " select   isnull(cmt.Tag,'') as Tag,cms.status, isnull((select top(1) isnull(Task,'')  from crm_task where Contact_Id=mct.Contact_Id " + AssignedTo + " order by CreatedDate desc),'') as task, " + "\n";
            varname1 = varname1 + " mct.contact_id, firstname  AS NAME, '' AS compname, Isnull(msr.SMName,'') As Towner,  isnull( (select  top(1) CASE ct.status  WHEN 'c' THEN 'Close' ELSE 'Open'  END    from crm_task ct where ct.Contact_Id=mct.Contact_Id " + AssignedTo + "  order by CreatedDate desc),'') as Tstatus, " + "\n";
            varname1 = varname1 + " (select  CASE when (select top (1) isnull(ct1.AssignDate,'') from CRM_Task ct1 where ct1.Contact_Id=mct.Contact_Id " + AssignedTo + " order by CreatedDate desc) <>''  " + "\n";
            varname1 = varname1 + " THEN (select top (1) isnull(CONVERT(VARCHAR(6), ct1.AssignDate, 106),'') from CRM_Task ct1 where ct1.Contact_Id=mct.Contact_Id " + AssignedTo + " order by CreatedDate desc)  " + "\n";
            varname1 = varname1 + " ELSE convert(varchar(6),mct.CreatedDate,106) END   ) as assgndate, (select  CASE when (select top (1) isnull(ct1.AssignDate,'') from CRM_Task ct1 where ct1.Contact_Id=mct.Contact_Id " + AssignedTo + " order by CreatedDate desc) <> ''  " + "\n";
            varname1 = varname1 + " THEN (select top (1) isnull(CONVERT(VARCHAR(10), ct1.AssignDate, 101),'') from CRM_Task ct1 where ct1.Contact_Id=mct.Contact_Id " + AssignedTo + " order by CreatedDate desc)  " + "\n";
            varname1 = varname1 + " ELSE convert(varchar(10),mct.CreatedDate,101) END   ) as assgndate1, (select case when (select top (1) CONVERT(VARCHAR(10),dateadd(WW,1,ct1.AssignDate), 101) from CRM_Task ct1 where ct1.Contact_Id=mct.Contact_Id " + AssignedTo + " order by CreatedDate desc)<>'' " + "\n";
            varname1 = varname1 + " then (select top (1) CONVERT(VARCHAR(10),dateadd(WW,1,mct.CreatedDate), 101) from CRM_Task ct1 where ct1.Contact_Id=mct.Contact_Id " + AssignedTo + " order by CreatedDate desc) " + "\n";
            varname1 = varname1 + " else CONVERT(VARCHAR(10),dateadd(WW,1,mct.CreatedDate), 101) end) as weekdt, (select  CONVERT(VARCHAR(10),CONVERT(VARCHAR(10),GETDATE(), 101), 101) )    as currentdt,mct.Flag as Flag, (select  CASE when (select top (1) isnull(ct1.AssignDate,'') from CRM_Task ct1  where ct1.Contact_Id=mct.Contact_Id " + AssignedTo + " order by CreatedDate desc) <>''   THEN (select top (1) isnull(ct1.AssignDate,'') from CRM_Task ct1 where ct1.Contact_Id=mct.Contact_Id " + AssignedTo + " order by CreatedDate desc)   ELSE isnull(mct.CreatedDate,'') END   ) as Adate FROM   [crm_mastcontact] mct " + "\n";
            varname1 = varname1 + " left JOIN CRM_MastTag cmt  ON mct.Tag_Id = cmt.Tag_Id     INNER JOIN crm_maststatus cms  ON mct.status_id = cms.status_id " + "\n";
            varname1 = varname1 + " Left JOIN MastSalesRep msr on msr.SMID = mct.Manager WHERE  1 = 1  " + addqry + "" + "\n";            
             varname1 = varname1 + " " + subqry  + "";
             varname1 = varname1 + ") a " + outercondition + " GROUP BY contact_id " + "\n";            
            varname1 = varname1 +" order by assgndate1 desc";
        
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, varname1);
            if (dt.Rows.Count > 0)
            {
            }
            return JsonConvert.SerializeObject(dt);
        }
        [WebMethod(EnableSession = true)]
        public string GetContactDataByID(string contactId)
        {
            //string strQ = "select smname, FROM [dbo].[CRM_Task] ct inner join [MastSalesRep] msr on ct.smid=msr.smid Inner Join Crm_MastCompany mcn on  where [status]='P' order by AssignDate desc";
            string strQ = "select FirstName + ' '+ Isnull(LastName,'') as Name,mct.Address AS Add1,mcm.CompName,mcm.Description,";
            strQ += " mct.Status_Id,mct.Tag_Id,(select ct.Background from [CRM_Task] ct  where  mct.Contact_Id=ct.Contact_Id and (ct.Ref_DocId is null or ct.Ref_DocId='')) as Background,";
            strQ += " (select DocId from [CRM_Task] ct  where  mct.Contact_Id=ct.Contact_Id and (ct.Ref_DocId is null or ct.Ref_DocId='')) as CTDocId,";
            strQ += " (select top(1)ccm.Phone from CRM_ContactMobile ccm  where  mct.Contact_Id=ccm.Contact_Id ) as Phone, (select top(1)cce.Email from [CRM_ContactEmail] cce  ";
            strQ += " where  mct.Contact_Id=cce.Contact_Id ) as Email,(select top(1)ccu.Url from [CRM_ContactURL] ccu  where  mct.Contact_Id=ccu.Contact_Id ) as Url,mct.Manager As manager,";
            strQ += "  mcm.Address AS compadd,mcm.City AS compcity,mcm.state + ' - ' + CAST(mcm.zip AS VARCHAR) AS compstate,  c.AreaName AS compcountry ";
            strQ += " ,mct.City AS contactCity,   mct.state + ' - ' + CAST(mct.ZipCode AS VARCHAR) AS contactstate,   b.AreaName AS contactcountry from [CRM_MastContact] mct ";
            strQ += " left join CRM_MastCompany mcm on mct.CompanyId=mcm.Comp_Id ";
            strQ += " LEFT JOIN (select AreaID,AreaName from mastarea where AreaType='Country' and Active='1' ) AS b ON b.AreaID=mct.Country";
            strQ += " LEFT JOIN (select AreaID,AreaName from mastarea where AreaType='Country' and Active='1' ) AS c ON c.AreaID=mcm.Country";
            strQ += " where mct.Contact_Id=" + contactId + "";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
            if (dt.Rows.Count > 0)
            {
            }
            return JsonConvert.SerializeObject(dt);
        }
        [WebMethod(EnableSession = true)]
        public string GetAllTag()
        {
            //string strQ = "select smname, FROM [dbo].[CRM_Task] ct inner join [MastSalesRep] msr on ct.smid=msr.smid Inner Join Crm_MastCompany mcn on  where [status]='P' order by AssignDate desc";
            string strQ = "select Tag_Id ,Tag from CRM_MastTag order by Tag_Id";
            DataTable dtstatus = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
            if (dtstatus.Rows.Count > 0)
            {
            }
            return JsonConvert.SerializeObject(dtstatus);
        }

        [WebMethod(EnableSession = true)]
        public string GetAllOwner()
        {
            string strQ = ""; string loginusersmid = Settings.Instance.SMID; string loginusername = Settings.Instance.UserName;
            //string strQ = "select smname, FROM [dbo].[CRM_Task] ct inner join [MastSalesRep] msr on ct.smid=msr.smid Inner Join Crm_MastCompany mcn on  where [status]='P' order by AssignDate desc";
             if (Settings.Instance.UserName.ToUpper() == "SA") 
             {

                 strQ = " Select SMID,SMName from [MastSalesRep] where SMName <>'.'  order by SMName ";
             }
         
            else
             {
                 strQ = " ((select MSRG.maingrp As SMID,MSR.SMName from mastsalesrepgrp MSRG Left Join [MastSalesRep] MSR on MSR.smid=MSRG.maingrp  where MSRG.smid in (" + Settings.Instance.SMID + ") and MSR.SMName <>'.'  union ";
                 strQ = strQ + " SELECT MSRG.smid  As SMID,MSR.SMName FROM mastsalesrepgrp  MSRG Left Join [MastSalesRep] MSR on MSR.smid=MSRG.smid  WHERE  MSRG.maingrp in (" + Settings.Instance.SMID + ")  and MSR.SMName <>'.' ))  order by MSR.SMName ";

                  
             }
            DataTable dtOwner = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
            if (dtOwner.Rows.Count > 0)
            {
                foreach (DataRow row in dtOwner.Rows)
                {
                    if (row["SMID"].ToString() == loginusersmid)
                    {
                        row["SMName"] = " Me";
                    }
                }
            }
            DataView dv = new DataView(dtOwner);
            dv.Sort = "SMName";
            dtOwner = dv.ToTable();
         //   .DefaultView.Sort = "SmName ASC"; 
            return JsonConvert.SerializeObject(dtOwner);
        }
        [WebMethod(EnableSession = true)]
        public string GetAllOwnerForFilter()
        {
            string strQ = ""; string loginusersmid = Settings.Instance.SMID; string loginusername = Settings.Instance.UserName;
            DataTable dtOwner = new DataTable();
            //string strQ = "select smname, FROM [dbo].[CRM_Task] ct inner join [MastSalesRep] msr on ct.smid=msr.smid Inner Join Crm_MastCompany mcn on  where [status]='P' order by AssignDate desc";
            if (Settings.Instance.UserName.ToUpper() == "SA")
            {

                strQ = " Select SMID,SMName from [MastSalesRep] where SMName <>'.'  order by SMName ";
                dtOwner = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
            }

            else
            {

              //  dtOwner = Settings.UnderUsers(Settings.Instance.SMID);
              //  strQ = " ((select MSRG.maingrp As SMID,MSR.SMName from mastsalesrepgrp MSRG Left Join [MastSalesRep] MSR on MSR.smid=MSRG.maingrp  where MSRG.smid in (" + Settings.Instance.SMID + ") and MSR.SMName <>'.'  union ";
                //strQ = strQ + " SELECT MSRG.smid  As SMID,MSR.SMName FROM mastsalesrepgrp  MSRG Left Join [MastSalesRep] MSR on MSR.smid=MSRG.smid  WHERE  MSRG.maingrp in (" + Settings.Instance.SMID + ")  and MSR.SMName <>'.' ))  order by MSR.SMName ";

                 strQ = @"select MastSalesRep.SMID,MastSalesRep.SMName from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId
            where smid in (select distinct smid from MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + ")) and  level>= (select distinct level from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + " ))) and MastSalesRep.Active=1 order by MastSalesRep.SMName";

                 dtOwner = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);

            }
           // DataTable dtOwner = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
            if (dtOwner.Rows.Count > 0)
            {
                foreach (DataRow row in dtOwner.Rows)
                {
                    if (row["SMID"].ToString() == loginusersmid)
                    {
                        row["SMName"] = "  Me";
                    }
                }
            }
            DataRow newCustomersRow = dtOwner.NewRow();

            newCustomersRow["SMID"] = "0";
            newCustomersRow["SMName"] = " All";

            dtOwner.Rows.Add(newCustomersRow);
            DataView dv = new DataView(dtOwner);
            dv.Sort = "SMName";
            dtOwner = dv.ToTable();
            //   .DefaultView.Sort = "SmName ASC"; 
            return JsonConvert.SerializeObject(dtOwner);
        }
        [WebMethod(EnableSession = true)]
        public string GetAllStatus()
        {
            //string strQ = "select smname, FROM [dbo].[CRM_Task] ct inner join [MastSalesRep] msr on ct.smid=msr.smid Inner Join Crm_MastCompany mcn on  where [status]='P' order by AssignDate desc";
            string strQ = "select Status_Id,Status from CRM_MastStatus order by Status_Id";
            DataTable dtstatus = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
            if (dtstatus.Rows.Count > 0)
            {
            }
            return JsonConvert.SerializeObject(dtstatus);
        }
        [WebMethod(EnableSession = true)]
        public string GetOwners()
        {
            DataTable dtowners = Settings.UnderUsers(Settings.Instance.SMID);
            DataView dv = new DataView(dtowners);
            dv.RowFilter = "SMName<>.";
            return JsonConvert.SerializeObject(dv.ToTable());
        }
        [WebMethod(EnableSession = true)]
        public void UpdateStatus(string status_Id, string contactId)
        {
            if (!string.IsNullOrEmpty(contactId) && contactId != "0")
            {
                string strupdstatus = "update [CRM_MastContact] set status_Id=" + status_Id + " where Contact_Id=" + contactId + "";
                DAL.DbConnectionDAL.ExecuteQuery(strupdstatus);
            }
        }
        [WebMethod(EnableSession = true)]
        public void SaveBG(string Tdocid, string bg, string ContactId)
        {
            if (!string.IsNullOrEmpty(Tdocid) && Tdocid != "0")
            {
                string strupdstatus = "update [CRM_MastContact] set Background='" + bg + "' where contact_id=" + Convert.ToInt32(ContactId) ;
                //string strupdstatus = "update [CRM_Task] set Background='" + bg + "' where cast(REPLACE(DocId, ' ', '') as varchar)=cast(REPLACE('" + Tdocid + "', ' ', '') as varchar)";
                DAL.DbConnectionDAL.ExecuteQuery(strupdstatus);
            }
        }
        [WebMethod(EnableSession = true)]
        public void UpdateStatusTask(string status, string Tdocid)
        {
            if (!string.IsNullOrEmpty(Tdocid) && Tdocid != "0")
            {
                string strupdtstatus = "update [CRM_Task] set Status='" + status + "' where DocId='" + Tdocid + "' or  Ref_DocId='" + Tdocid + "'";
                DAL.DbConnectionDAL.ExecuteQuery(strupdtstatus);
            }
        }


        [WebMethod(EnableSession = true)]
        public void UpdateTag(string Tag_Id, string contactId1)
        {
            if (!string.IsNullOrEmpty(contactId1) && contactId1 != "0")
            {
                string strupdstatus = "update [CRM_MastContact] set Tag_Id=" + Tag_Id + " where Contact_Id=" + contactId1 + "";
                DAL.DbConnectionDAL.ExecuteQuery(strupdstatus);
            }
        }

        [WebMethod(EnableSession = true)]
        public void UpdateOwner(string Owner_Id, string contactId2)
        {
            if (!string.IsNullOrEmpty(contactId2) && contactId2 != "0")
            {
                string strupdowner = "update [CRM_MastContact] set OwnerSp =  (case when  [OwnerSp]  Like '%" + Owner_Id + "%' then [OwnerSp] else [OwnerSp] + ',' + '" + Owner_Id + "' end)  ,Manager= " + Owner_Id + " where Contact_Id=" + contactId2 + "";
                DAL.DbConnectionDAL.ExecuteQuery(strupdowner);
            }
        }
          [WebMethod(EnableSession = true)]
        public string Checkduplicate(string firstname,string lastname,string company)
        {
            string delY = "Y";


            string str = "";


           
                str = "SELECT count(*) FROM CRM_MastContact cm LEFT JOIN CRM_MastCompany cc ON CC.Comp_Id=CM.Contact_Id ";
                 str += " WHERE cm.FirstName ='" + firstname.Replace("'", "''") + "' AND cm.LastName='" + lastname.Replace("'", "''") + "' AND cc.CompName='" + company + "'";
                if (Convert.ToInt32(DAL.DbConnectionDAL.GetScalarValue(CommandType.Text, str)) <= 0)
                {
                  
                }
                else
                {
                    delY = "N";
                }
           
            Msg rst = new Msg
            {
                SetMsg = delY
            };
            return (JsonConvert.SerializeObject(rst));
        }
        [WebMethod(EnableSession = true)]
        public string DeleteTask(string Tdocid)
        {
            string delY = "Y";


            string str = "";

           
            if (!string.IsNullOrEmpty(Tdocid) && Tdocid != "0")
            {
                str = "SELECT sum(T.count1) FROM(";
                str = str + "select count(*) AS count1 from crm_task where cast(REPLACE(Ref_DocId, ' ', '') as varchar)='" + Tdocid + "'";
                str = str + " UNION SELECT count(*) AS count1 FROM CRM_Tasknotes where cast(REPLACE(CRMTask_DocId, ' ', '') as varchar)='" + Tdocid + "'";
                str = str + " UNION SELECT count(*) AS count1 FROM CRM_Taskdeals where cast(REPLACE(CRM_TaskDocId, ' ', '') as varchar)='" + Tdocid + "'";
                str = str + " UNION SELECT count(*) AS count1 FROM CRM_TaskCall where cast(REPLACE(CRMTask_DocId, ' ', '') as varchar)='" + Tdocid + "') AS T";
                if (Convert.ToInt32(DAL.DbConnectionDAL.GetScalarValue(CommandType.Text, str)) <= 0)
                {
                    int contactId = Convert.ToInt32(DAL.DbConnectionDAL.GetScalarValue(CommandType.Text, "  SELECT contact_Id FROM CRM_task where cast(REPLACE(DocId, ' ', '') as varchar)='" + Tdocid + "'"));
                    string deltask = "Delete [CRM_MastContact] where Contact_Id=" + contactId + " and  Isnull(Flag,'L') ='T'";
                    DAL.DbConnectionDAL.ExecuteQuery(deltask);                 
                    
                    deltask = "Delete [CRM_Task] where cast(REPLACE(DocId, ' ', '') as varchar)='" + Tdocid + "'";
                    DAL.DbConnectionDAL.ExecuteQuery(deltask);
                    deltask = "DELETE FROM TransNotification WHERE msgURL LIKE '%" + Tdocid + "%'";
                    DAL.DbConnectionDAL.ExecuteQuery(deltask);
                }
                else
                {
                    delY = "N";
                }
            }
            Msg rst = new Msg
            {
                SetMsg = delY
            };
            return (JsonConvert.SerializeObject(rst));
        }
        //[WebMethod(EnableSession = true)]
        //public string GetTaskDetails(string Ref_DocId)
        //{
        //     DataTable dt=new DataTable();
        //    if (Ref_DocId != "")
        //    {


        //        //string strQ = "select smname, FROM [dbo].[CRM_Task] ct inner join [MastSalesRep] msr on ct.smid=msr.smid Inner Join Crm_MastCompany mcn on  where [status]='P' order by AssignDate desc";
        //        string strQ = "select cast(REPLACE(DocId, ' ', '') as varchar)as[DocId],ISNULL(cast(REPLACE(DocId, ' ', '') as varchar)+ cast( Ref_Sno as varchar),DocId) as TDocId,[Task],isnull([dbo].[CRMGetAssignedToNames]([AssignedTo]),'none') as AssignedTo,[AssignedBy],convert(varchar(12),[AssignDate],106) as Adate,[Ref_DocId],[Ref_Sno],[Status],CRM_Task.CreatedBySmId as [SmId],Smname  from [CRM_Task] inner join [MastSalesRep] msr on CRM_Task.AssignedBy=msr.Smid  where 1=1 and (Ref_DocId='" + Ref_DocId + "' or DocId='" + Ref_DocId + "') order by AssignDate";
        //         dt = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
        //        if (dt.Rows.Count > 0)
        //        {
        //        }
        //    }
        //    return JsonConvert.SerializeObject(dt);
        //}
        [WebMethod(EnableSession = true)]
        public string GetTaskDetailByID(string DocId)
        {
            //string strQ = "select smname, FROM [dbo].[CRM_Task] ct inner join [MastSalesRep] msr on ct.smid=msr.smid Inner Join Crm_MastCompany mcn on  where [status]='P' order by AssignDate desc";
            string strQ = "select cast(REPLACE(DocId, ' ', '') as varchar)as[DocId],ISNULL(cast(REPLACE(DocId, ' ', '') as varchar)+ cast( Ref_Sno as varchar),DocId) as TDocId,[Task],AssignedTo as AssignedToId,isnull([dbo].[CRMGetAssignedToNames]([AssignedTo]),'none') as AssignedTo,[AssignedBy],convert(varchar(12),[AssignDate],106) as Adate,[Ref_DocId],[Ref_Sno],[Status],Smname,convert(char(5), CONVERT(Datetime, time, 120), 108) [time]  from [CRM_Task] inner join [MastSalesRep] msr on CRM_Task.AssignedBy=msr.Smid  where cast(REPLACE(DocId, ' ', '') as varchar)='" + DocId + "'";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    if (row["AssignedToId"].ToString() == Settings.Instance.SMID)
                    {
                        row["AssignedTo"] = " Me";
                    }
                }
            }
            return JsonConvert.SerializeObject(dt);
        }
         [WebMethod(EnableSession = true)]
        public string getFilePath(string Task_DealID)
        {
            //string strQ = "select smname, FROM [dbo].[CRM_Task] ct inner join [MastSalesRep] msr on ct.smid=msr.smid Inner Join Crm_MastCompany mcn on  where [status]='P' order by AssignDate desc";
            string strQ = "SELECT Isnull(FileName,'') AS filename,Isnull(replace(Path,' ','%20') ,'') AS filepath FROM CRM_UploadFile  WHERE DealId=" + Convert.ToInt32(Task_DealID) + "";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
            if (dt.Rows.Count > 0)
            {
               
            }
            return JsonConvert.SerializeObject(dt);
        }
        [WebMethod(EnableSession=true)]
         public string getdealstage(string Task_DealID)
         {

             string strQ = "SELECT Id,DealText,DealAmt,(Case WHEN Isnull(replace(convert(varchar(12),[paydate],106),' ','/'),'') ='01/Jan/1900' THEN '' ELSE  Isnull(replace(convert(varchar(12),[paydate],106),' ','/'),'') end ) as paydate,Qty,Rate FROM CRM_DealStage WHERE DealId=" + Convert.ToInt32(Task_DealID) + "";
             DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
             if (dt.Rows.Count > 0)
             {

             }
             return JsonConvert.SerializeObject(dt);
        
        }
        [WebMethod(EnableSession = true)]
        public void  removedealstage(string dealstageid)
        {
            if (!string.IsNullOrEmpty(dealstageid) && dealstageid != "0")
            {
                string strQ = "Delete FROM CRM_DealStage WHERE Id=" + Convert.ToInt32(dealstageid) + "";
                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
                //if (dt.Rows.Count > 0)
                //{

                //}
                //return JsonConvert.SerializeObject(dt);
            }
        }
        [WebMethod(EnableSession = true)]
        public void removecontactdetail(string Id,string Type)
        {
            if (!string.IsNullOrEmpty(Id) && Id != "0")
            {
                if (Type == "P")
                {
                    string strQ = "Delete FROM CRM_ContactMobile WHERE CMbl_Id=" + Convert.ToInt32(Id) + "";
                    DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
                }
                else if (Type == "E")
                {
                    string strQ = "Delete FROM CRM_ContactEmail WHERE CEmail_Id=" + Convert.ToInt32(Id) + "";
                    DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
                }
                else
                {
                    string strQ = "Delete FROM CRM_ContactURL WHERE CUrl_Id=" + Convert.ToInt32(Id) + "";
                    DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
                }
            }

        }
        [WebMethod(EnableSession = true)]
        public string DeleteNote(string TaskNoteId)
        {
            string delY = "N";
            if (!string.IsNullOrEmpty(TaskNoteId) && TaskNoteId != "0")
            {
                 string delnote = "delete [CRM_TaskNotes] where Task_NoteID='" + TaskNoteId + "'";
                DAL.DbConnectionDAL.ExecuteQuery(delnote);
                delY = "Y";
            }
            Msg rst = new Msg
            {
                SetMsg = delY
            };
            return (JsonConvert.SerializeObject(rst));
        }
        [WebMethod(EnableSession = true)]
        public string DeleteDeal(string TaskDealId)
        {
            string delY = "N";
            DataTable dt = new DataTable();
            string path = "";
            if (!string.IsNullOrEmpty(TaskDealId) && TaskDealId != "0")
            {
                string delnote = "delete [CRM_TaskDeals] where Task_DealID='" + TaskDealId + "'";
                DAL.DbConnectionDAL.ExecuteQuery(delnote);
                string strQ = "select Path from CRM_UploadFile where  DealId='" + TaskDealId + "'";
                dt = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        path = dt.Rows[i]["Path"].ToString();
                        if (System.IO.File.Exists(Server.MapPath(path)))
                        {

                            System.IO.File.Delete(Server.MapPath(path));
                        }
                       
                    }
                    delnote = "delete from CRM_UploadFile where DealId='" + TaskDealId + "'";
                    DAL.DbConnectionDAL.ExecuteQuery(delnote);
                }
                delnote = "delete from CRM_DealStage where DealId='" + TaskDealId + "'";
                DAL.DbConnectionDAL.ExecuteQuery(delnote);
                delY = "Y";
            }
            Msg rst = new Msg
            {
                SetMsg = delY
            };
            return (JsonConvert.SerializeObject(rst));
        }
        [WebMethod(EnableSession=true)]
        public void DeleteFile(string TaskDealId, string FilePath)
        {
            if (!string.IsNullOrEmpty(TaskDealId) && TaskDealId != "0")
            {
             if(System.IO.File.Exists(Server.MapPath(FilePath.Replace("%20", " "))))
                {
                System.IO.File.Delete(Server.MapPath(FilePath.Replace("%20", " ")));
                }
                string delnote = "delete [CRM_UploadFile] where DealId='" + TaskDealId + "' and Path ='" + FilePath.Replace("%20"," ") + "'";
                DAL.DbConnectionDAL.ExecuteQuery(delnote);
              
            }
        }
        [WebMethod(EnableSession= true)]
        public void Savedealstage(string Task_DealID, string dealtext, string dealamt, string stageid,string qty,string rate,string totalamt)
        {

            Int64 amt = 0;
            string str = "";
             DataTable dt= new DataTable();
             string[] dealtext1 = dealtext.Split(',');
             string[] dealamt1 = dealamt.Split(',');
             string[] dealqty1 = qty.Split(',');
             string[] dealrate1 = rate.Split(',');
           //  string[] date1 = date.Split(',');
             if (!string.IsNullOrEmpty(Task_DealID) && Task_DealID != "0")
             {
                 if (stageid != "")
                 {
                    
                     string[] stageid1 = stageid.Split(',');
                   
                     for (int i = 0; i < dealamt1.Length; i++)
                     {
                         string strQ = "select * from CRM_DealStage where DealId='" + Task_DealID + "' and Id= '" + stageid1[i] + "'";
                         dt = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
                         if (dt.Rows.Count == 0)
                         {
                             str = "";
                             str = str + "INSERT INTO [dbo].[CRM_DealStage] " + "\n";
                             str = str + "           ([DealId] " + "\n";
                             str = str + "           ,[DealText] " + "\n";
                             str = str + "           ,[DealAmt],[Qty],[Rate]) " + "\n";
                             str = str + "     VALUES " + "\n";
                             str = str + "           ('" + Task_DealID + "'\n";
                             str = str + "          ,'" + dealtext1[i].ToString().Replace("'", "''") + "'\n";
                             str = str + "            ,'" + dealamt1[i].ToString() + "','" + dealqty1[i].ToString() + "','" + dealrate1[i].ToString() + "')";

                             DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
                         }
                         else
                         {
                             str = "Update CRM_DealStage set DealText ='" + dealtext1[i].ToString().Replace("'", "''") + "',DealAmt='" + dealamt1[i].ToString() + "',Qty='" + dealqty1[i].ToString() + "',Rate='" + dealrate1[i].ToString() + "'  where DealId='" + Task_DealID + "' and Id= " + Convert.ToInt32(stageid1[i]) + "";
                             DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
                         }
                         amt = amt + Convert.ToInt64(dealamt1[i].ToString());
                         }
                     str = "Update CRM_TaskDeals set Amount =" + totalamt + " where Task_DealID='" + Task_DealID + "'";
                         DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
                     
                 }
                 else
                 {
                    for (int i = 0; i < dealamt1.Length; i++)
                     {
                       
                             str = "";
                             str = str + "INSERT INTO [dbo].[CRM_DealStage] " + "\n";
                             str = str + "           ([DealId] " + "\n";
                             str = str + "           ,[DealText] " + "\n";
                             str = str + "           ,[DealAmt],[Qty],[Rate]) " + "\n";
                             str = str + "     VALUES " + "\n";
                             str = str + "           ('" + Task_DealID + "'\n";
                             str = str + "          ,'" + dealtext1[i].ToString().Replace("'", "''") + "'\n";
                             str = str + "            ,'" + dealamt1[i].ToString() + "','" + dealqty1[i].ToString() + "','" + dealrate1[i].ToString() + "')";

                             DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
                         
                         amt = amt + Convert.ToInt64(dealamt1[i].ToString());
                     }
                    str = "Update CRM_TaskDeals set Amount =" + totalamt + " where Task_DealID='" + Task_DealID + "'";
                         DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
                     
                 }
             }
        }


         [WebMethod(EnableSession = true)]
        public void SaveFilePath(string Task_DealID, string FilePath, string Filename)
        {
            int ID = 0;
            string fileLogicalname = "";
             DataTable dt= new DataTable();
            if (!string.IsNullOrEmpty(Task_DealID) && Task_DealID != "0")
            {
                if (!string.IsNullOrEmpty(FilePath) && FilePath != "0")
                {
                    Filename = Filename.TrimEnd(',');
                    string[] arrFilename = Filename.Split(',');
                    string[] arrfilepath = FilePath.Split(',');
                    for (int i = 0; i < arrfilepath.Length; i++)
                    {
                        fileLogicalname=arrfilepath[i].Substring(arrfilepath[i].LastIndexOf('/') + 1);
                        //string strQ = "select * from CRM_UploadFile where  DealId='" + Task_DealID + "' and  Path='" + arrfilepath[i] + "'";
                        string strQ = "select * from CRM_UploadFile where DealId='" + Task_DealID + "' and Filename= '" + arrFilename[i] + "'";
                        dt = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
                        if (dt.Rows.Count == 0)
                        {
                            if (arrFilename[i] != "")
                            {
                                String varname1 = "";
                                varname1 = varname1 + "INSERT INTO [dbo].[CRM_UploadFile] " + "\n";
                                varname1 = varname1 + "           ([DealId] " + "\n";
                                varname1 = varname1 + "           ,[Path],[Filename],[LogicalName]) " + "\n";
                                varname1 = varname1 + "     VALUES " + "\n";
                                varname1 = varname1 + "             ('" + Task_DealID + "',\n";
                                varname1 = varname1 + "             '" + arrfilepath[i] + "','" + arrFilename[i] + "','" + fileLogicalname + "')";

                                DAL.DbConnectionDAL.ExecuteQuery(varname1);
                                String varname11 = "";
                                varname11 = varname11 + "GO";
                            }
                        }
                        else
                        {
                            if (System.IO.File.Exists(Server.MapPath(arrfilepath[i])))
                            {
                                System.IO.File.Delete(Server.MapPath(arrfilepath[i]));
                            }
                        }
                    }
                }

            }
          
        }
        [WebMethod(EnableSession = true)]
        public string SaveNxtTask(string TDocId,string time, string task, string contactId, string Asgndate, string AsgnTo, string Ref_DocId, string Status)
        {
            string[] timesplit = time.Split(':');
            TimeSpan timespan = new TimeSpan(Convert.ToInt32(timesplit[0]), Convert.ToInt32(timesplit[0]), 00);
            DateTime time1 = DateTime.Today.Add(timespan);
            string displayTime = time1.ToString("hh:mm tt");
            string docID = "";
            if (!string.IsNullOrEmpty(TDocId) && TDocId != "0")
            {
                DAL.DbConnectionDAL.ExecuteQuery("Update CRM_Task set time='" + displayTime + "',AssignedTo='" + AsgnTo + "',AssignedBy=" + Settings.Instance.SMID + ",Task='" + task.Replace("'", "''") + "',AssignDate='" + Asgndate + "',UpdatedBySmId=" + Settings.Instance.SMID + ", [UpdatedDate]=Getdate(),[Status] ='" + Status + "' where cast(REPLACE(DocId, ' ', '') as varchar)='" + TDocId + "'");
           
                
            }
            else
            {
                DateTime Currdt = Settings.GetUTCTime();
                docID = Settings.GetDocID("CRMTK", Currdt);
                Settings.SetDocID("CRMT", docID);
                int sno = 0;
                if (!string.IsNullOrEmpty(Ref_DocId))
                {
                    sno = Convert.ToInt16(DAL.DbConnectionDAL.GetScalarValue(CommandType.Text, "select count(*) from Crm_task where Ref_DocId='" + Ref_DocId + "'"));
                    sno++;
                }


                string smIDStr1 = "";
                //foreach (ListItem item in ListBox1.Items)
                //{
                //    if (item.Selected)
                //    {
                //        smIDStr1 += item.Value + ",";
                //    }
                //}
                string strQ = "";

                if (Settings.Instance.UserName.ToUpper() == "SA")
                {

                    // strQ = " Select SMID,SMName from [MastSalesRep]   order by SMName "; //////////where SMName <>'.'
                    strQ = Settings.Instance.SMID;
                }

                else
                {
                    strQ = " SELECT Smid,SMName FROM MastSalesRep WHERE SMId IN (SELECT maingrp FROM MastSalesRepGrp WHERE SMId=" + Settings.Instance.SMID + ")";
                    DataTable dtOwner = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
                    strQ = "";
                    for (int i = 0; i < dtOwner.Rows.Count; i++)
                    {

                        strQ += dtOwner.Rows[i]["SMID"].ToString() + ',';
                    }
                    strQ = strQ.TrimEnd(',');

                }

              


                smIDStr1 = strQ;

                if (contactId == "")
                {
                    string Task_new = !String.IsNullOrWhiteSpace(task) && task.Length >= 30 ? task.Substring(0, 30) : task;
                    String str = "INSERT INTO dbo.CRM_MastContact (FirstName, Status_Id,  Flag,Manager,OwnerSp,SmId,Tag_Id) VALUES ('" + Task_new.Replace("'", "''") + "',3,'T','" + Convert.ToString(Settings.Instance.SMID) + "','" + smIDStr1 + "'," + Settings.Instance.SMID + ",'1')";
                    DAL.DbConnectionDAL.ExecuteQuery(str);
                    contactId = Convert.ToString(DAL.DbConnectionDAL.GetScalarValue(CommandType.Text, "SELECT max(contact_id) FROM CRM_MastContact WHERE FirstName ='" + Task_new + "'"));



                }
                String varname1 = "";
                varname1 = varname1 + "INSERT INTO [dbo].[CRM_Task] " + "\n";
                varname1 = varname1 + "           ([DocId] " + "\n";
                varname1 = varname1 + "           ,[AssignedTo] " + "\n";
                varname1 = varname1 + "           ,[time] " + "\n";
                varname1 = varname1 + "           ,[AssignedBy] " + "\n";
                varname1 = varname1 + "           ,[Task] " + "\n";
                varname1 = varname1 + "           ,[AssignDate] " + "\n";
                varname1 = varname1 + "           ,[Ref_DocId] " + "\n";
                varname1 = varname1 + "           ,[Ref_Sno] " + "\n";
                varname1 = varname1 + "           ,[Status] " + "\n";
                varname1 = varname1 + "           ,[CreatedBySmId] " + "\n";
                varname1 = varname1 + "           ,[Contact_Id] " + "\n";
                varname1 = varname1 + "           ,[CreatedDate]) " + "\n";
                varname1 = varname1 + "     VALUES " + "\n";
                varname1 = varname1 + "        ('" + docID + "',\n";
                varname1 = varname1 + "          '" + AsgnTo + "',\n";
                varname1 = varname1 + "          '" + displayTime + "',\n";
                varname1 = varname1 + "            '" + Settings.Instance.SMID + "',\n";
                varname1 = varname1 + "             '" + task.Replace("'", "''") + "',\n";
                varname1 = varname1 + "             '" + Asgndate + "',\n";
                varname1 = varname1 + "             '" + Ref_DocId + "',\n";
                varname1 = varname1 + "          '" + sno + "',\n";
                varname1 = varname1 + "              '" + Status + "',\n";
                varname1 = varname1 + "            '" + Settings.Instance.SMID + "',\n";
                varname1 = varname1 + "            '" + contactId + "',\n";
                varname1 = varname1 + "             Getdate())";

                DateTime chdate = Convert.ToDateTime(Asgndate);
                string format = "dd/MMM/yyyy"; 
                String varname11 = "";
                varname11 = varname11 + "GO";
                DAL.DbConnectionDAL.ExecuteQuery(varname1);
                string msgurl = ""; string displaytitle = "";
                msgurl = "CRMTask.aspx?DocId=" + docID +" ";
               
                string Assignedby = Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, "Select SMName From MastSalesRep where SMId = " +  Settings.Instance.SMID  +" "));
                string Assignedto = Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, "Select SMName From MastSalesRep where SMId = " + AsgnTo + " "));
                int Assigntouserid = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, "Select UserId From MastSalesRep where SMId = " + AsgnTo + " "));
                displaytitle = "Task By - " + Assignedby + " , To - " + Assignedto + "  " + chdate.ToString(format);
                varname1 = "INSERT INTO TransNotification (pro_id, userid, VDate, msgURL, displayTitle, Status, FromUserId, SMId, ToSMId) VALUES ('CRMTASK', " + Assigntouserid  + ", Getdate(),'" + msgurl + "', '" + displaytitle + "', 0, " + Settings.Instance.UserID + ", " + Settings.Instance.SMID + ", " + AsgnTo + ")";

                DAL.DbConnectionDAL.ExecuteQuery(varname1);
            }
            if (contactId  !="")
            {
                string strupdowner = "update [CRM_MastContact] set OwnerSp =  (case when  [OwnerSp]  Like '%" + AsgnTo + "%' then [OwnerSp] else [OwnerSp] + ',' + '" + AsgnTo + "' end)   where Contact_Id=" + contactId + "";
                DAL.DbConnectionDAL.ExecuteQuery(strupdowner);
            }
          

            if (string.IsNullOrEmpty(Ref_DocId))
            {
                Msg rst = new Msg
                {
                    SetMsg = docID
                };
                return (JsonConvert.SerializeObject(rst));
            }
            else
            {
                return null;
            }

        }
        public class Msg
        {
            public string SetMsg { get; set; }
        }

        [WebMethod(EnableSession = true)]
        public void InsertUpdTaskNote(string TaskDocID, string Note, string NoteId)
        {
            if (!string.IsNullOrEmpty(TaskDocID) && TaskDocID != "0")
            {
                if (!string.IsNullOrEmpty(NoteId) && NoteId != "0")
                {
                    string strupdtstatus = "update [CRM_TaskNotes] set [Notes]='" + Note.Replace("'", "''") + "',[UpdatedBySmId]=" + Settings.Instance.SMID + ",[UpdatedDate]=Getdate() where Task_NoteID='" + NoteId + "'";
                    DAL.DbConnectionDAL.ExecuteQuery(strupdtstatus);
                }
                else
                {
                    String varname1 = "";
                    varname1 = varname1 + "INSERT INTO [dbo].[CRM_TaskNotes] " + "\n";
                    varname1 = varname1 + "           ([CRMTask_DocId] " + "\n";
                    varname1 = varname1 + "           ,[Notes] " + "\n";
                    varname1 = varname1 + "           ,[CreatedBySmId] " + "\n";
                    varname1 = varname1 + "           ,[CreatedDate]) " + "\n";
                    varname1 = varname1 + "     VALUES ";
                    varname1 = varname1 + "           ('" + TaskDocID + "'\n";
                    varname1 = varname1 + "           ,'" + Note.Replace("'", "''") + "'\n";
                    varname1 = varname1 + "             ," + Settings.Instance.SMID + ",\n";
                    varname1 = varname1 + "             Getdate())";
                    String varname11 = "";
                    varname11 = varname11 + "GO";
                    DAL.DbConnectionDAL.ExecuteQuery(varname1);
                }
            }
        }
       
        [WebMethod(EnableSession = true)]
        public void SaveCall(string TaskDocID, string TCallID, string Phone, string Result, string CallNotes)
        {
            if (!string.IsNullOrEmpty(TaskDocID) && TaskDocID != "0")
            {
                if (!string.IsNullOrEmpty(TCallID) && TCallID != "0")
                {
                    string strupdtstatus = "update [CRM_TaskCall] set [CallNotes]='" + CallNotes.Replace("'", "''") + "', [Result]='" + Result.Replace("'", "''") + "', [Phone]='" + Phone.Replace("'", "''") + "',[UpdatedBySmId]=" + Settings.Instance.SMID + ",[UpdatedDate]=Getdate() where Task_CallID='" + TCallID + "'";
                    DAL.DbConnectionDAL.ExecuteQuery(strupdtstatus);
                }
                else
                {
                    String varname1 = "";
                    varname1 = varname1 + "INSERT INTO [dbo].[CRM_TaskCall] " + "\n";
                    varname1 = varname1 + "           ([CRMTask_DocId] " + "\n";
                    varname1 = varname1 + "           ,[Phone] " + "\n";
                    varname1 = varname1 + "           ,[Result] " + "\n";
                    varname1 = varname1 + "           ,[CallNotes] " + "\n";
                    varname1 = varname1 + "           ,[CreatedBySmId] " + "\n";
                    varname1 = varname1 + "           ,[CreatedDate]) " + "\n";
                    varname1 = varname1 + "     VALUES ";
                    varname1 = varname1 + "           ('" + TaskDocID + "'\n";
                    varname1 = varname1 + "           ,'" + Phone.Replace("'", "''") + "'\n";
                    varname1 = varname1 + "           ,'" + Result.Replace("'", "''") + "'\n";
                    varname1 = varname1 + "           ,'" + CallNotes.Replace("'", "''") + "'\n";
                    varname1 = varname1 + "             ," + Settings.Instance.SMID + ",\n";
                    varname1 = varname1 + "             Getdate())";
                    String varname11 = "";
                    varname11 = varname11 + "GO";
                    DAL.DbConnectionDAL.ExecuteQuery(varname1);
                }
            }
        }
        [WebMethod(EnableSession = true)]
        public void SaveEmail(string TaskDocID, string TCallID, string Phone, string Result, string CallNotes)
        {
            if (!string.IsNullOrEmpty(TaskDocID) && TaskDocID != "0")
            {
                if (!string.IsNullOrEmpty(TCallID) && TCallID != "0")
                {
                    string strupdtstatus = "update [CRM_TaskCall] set [CallNotes]='" + CallNotes.Replace("'", "''") + "', [Result]='" + Result.Replace("'", "''") + "', [mailto]='" + Phone.Replace("'", "''") + "',[UpdatedBySmId]=" + Settings.Instance.SMID + ",[UpdatedDate]=Getdate() where Task_CallID='" + TCallID + "'";
                    DAL.DbConnectionDAL.ExecuteQuery(strupdtstatus);
                }
                else
                {
                    String varname1 = "";
                    varname1 = varname1 + "INSERT INTO [dbo].[CRM_TaskCall] " + "\n";
                    varname1 = varname1 + "           ([CRMTask_DocId] " + "\n";
                    varname1 = varname1 + "           ,[mailto] " + "\n";
                    varname1 = varname1 + "           ,[Result] " + "\n";
                    varname1 = varname1 + "           ,[CallNotes] " + "\n";
                    varname1 = varname1 + "           ,[CreatedBySmId] " + "\n";
                    varname1 = varname1 + "           ,[CreatedDate]) " + "\n";
                    varname1 = varname1 + "     VALUES ";
                    varname1 = varname1 + "           ('" + TaskDocID + "'\n";
                    varname1 = varname1 + "           ,'" + Phone.Replace("'", "''") + "'\n";
                    varname1 = varname1 + "           ,'" + Result.Replace("'", "''") + "'\n";
                    varname1 = varname1 + "           ,'" + CallNotes.Replace("'", "''") + "'\n";
                    varname1 = varname1 + "             ," + Settings.Instance.SMID + ",\n";
                    varname1 = varname1 + "             Getdate())";
                    String varname11 = "";
                    varname11 = varname11 + "GO";
                    DAL.DbConnectionDAL.ExecuteQuery(varname1);
                }
            }
        }

        //[WebMethod(EnableSession = true)]
        //public string GetTaskNote(string TaskDocId, string TaskNoteId)
        //{
        //    string addqry = "";
        //    if (!string.IsNullOrEmpty(TaskNoteId) && TaskNoteId != "0")
        //    {
        //       // addqry += " and Task_NoteID=" + TaskNoteId + "";
        //    }
        //    string strQ = "select Task_NoteID,Notes from [CRM_TaskNotes] where CRMTask_DocId='" + TaskDocId + "'" + addqry + " order by CreatedDate";
        //    DataTable dttask = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
        //    if (dttask.Rows.Count > 0)
        //    {
        //    }
        //    return JsonConvert.SerializeObject(dttask);
        //}
        //[WebMethod(EnableSession = true)]
        //public string GetTaskDeal(string TaskDocId, string TaskDealId)
        //{
        //    string addqry = "";
        //    if (!string.IsNullOrEmpty(TaskDealId) && TaskDealId != "0")
        //    {
        //        //addqry += " and Task_DealID=" + TaskDealId + "";
        //    }
        //    string strQ = "select [Task_DealID],[CRM_TaskDocID],MonthlyDeal,isnull(TotalDealAmt,0)as TotalDealAmt,[DealName],[Amount],convert(varchar(12),[DealDate],106) as DealDate,convert(varchar(12),[ExpClsDt],106) as ExpClsDt,case [DealStage] when 'l' then 'Lost' when 'w' then 'Won' else 'Pending' end as DealStage,[DealStagePerc],[DealNote] from [CRM_TaskDeals] where [CRM_TaskDocID]='" + TaskDocId + "'" + addqry + " order by CreatedDate";
        //    DataTable dttaskd = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
        //    if (dttaskd.Rows.Count > 0)
        //    {
        //    }
        //    return JsonConvert.SerializeObject(dttaskd);
        //}
        [WebMethod(EnableSession = true)]
        public string GetCallNumbers(string ContactId)
        {
            string strQ = "select contact_Id, cast(phone as varchar) + '('+ cast(phonetype as varchar) + ')' as contactnumber from CRM_ContactMobile where  Contact_Id=" + ContactId + "";
            DataTable dtcall = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
            if (dtcall.Rows.Count > 0)
            {
            }
            return JsonConvert.SerializeObject(dtcall);
        }
        [WebMethod(EnableSession = true)]
        public string GetEmailContactNo(string ContactId)
        {
            string strQ = @"select contact_Id, cast(email as varchar)  as contactemail,phonetype,contactname +'-'+phonetype as con,cmbl_id from CRM_ContactMobile where  Contact_Id=" + ContactId + "";
            DataTable dtEmailContactNo = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
            if (dtEmailContactNo.Rows.Count > 0)
            {
            }
            return JsonConvert.SerializeObject(dtEmailContactNo);
        }
        [WebMethod(EnableSession = true)]
        public string GetEmail(string taskid)
        {
            string strQ = @"select contact_Id, cast(email as varchar)  as contactemail,phonetype,contactname +'-'+phonetype as con,cmbl_id from CRM_ContactMobile where  cmbl_id=" + taskid + "";
            DataTable dtEmailContactNo = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
            if (dtEmailContactNo.Rows.Count > 0)
            {
            }
            return JsonConvert.SerializeObject(dtEmailContactNo);
        }
        [WebMethod(EnableSession = true)]
       // public string UploadFile(HttpPostedFileBase files)
        public string UploadFile()
        {
            //string f = files.FileName;
            //string strQ = "select contact_Id, cast(phone as varchar) + '('+ cast(phonetype as varchar) + ')' as contactnumber from CRM_ContactMobile where  Contact_Id=" + 1 + "";
            //DataTable dtcall = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
            //if (dtcall.Rows.Count > 0)
            //{
            //}
            string result = null;
            string fileName = "";

            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count > 0)
            {
                string docfiles = "";
                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];
                    fileName = postedFile.FileName;
                    var filePath = HttpContext.Current.Server.MapPath("~/Prescription/" + fileName);
                    postedFile.SaveAs(filePath);
                    docfiles += fileName + ",";
                }
                result = docfiles.TrimEnd(',');
            }

            else
            {
                result = HttpStatusCode.BadRequest.ToString();
            }
            return result;
          //  return JsonConvert.SerializeObject(dtcall);
        }

        public string AddFile(HttpPostedFileBase files)
        {
            string f = files.FileName;
            return "tst";
        }
        //[WebMethod(EnableSession = true)]
        //public string GetTaskCalls(string TaskDocId, string TaskCallId)
        //{
        //    string addqry = "";
        //    if (!string.IsNullOrEmpty(TaskCallId) && TaskCallId != "0")
        //    {
        //       // addqry += " and Task_CallID=" + TaskCallId + "";
        //    }
        //    string strQ = "select [Task_CallID],[CRMTask_DocId],Phone,Result,CallNotes from [CRM_TaskCall] where [CRMTask_DocId]='" + TaskDocId + "'" + addqry + " order by CreatedDate";
        //    DataTable dttaskc = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
        //    if (dttaskc.Rows.Count > 0)
        //    {
        //    }
        //    return JsonConvert.SerializeObject(dttaskc);
        //}

        [WebMethod(EnableSession = true)]
        public string DeleteCall(string TaskCallId)
        {
            string delY = "N";
            if (!string.IsNullOrEmpty(TaskCallId) && TaskCallId != "0")
            {
                string delcall = "delete [CRM_TaskCall] where Task_CallID='" + TaskCallId + "'";
                DAL.DbConnectionDAL.ExecuteQuery(delcall);
                delY = "Y";
            }
            Msg rst = new Msg
            {
                SetMsg = delY
            };
            return (JsonConvert.SerializeObject(rst));
        }

        [WebMethod(EnableSession = true)]
        public string SaveDeal(string Task_DealID, string CRM_TaskDocID, string DealName, string Amount, string DealDate, string ExpClsDt, string DealStage, string DealStagePerc, string DealNote, string MonthlyDeal, string TotalDealAmt, string commper, string commamt)
        {
            int ID = 0;
            if (!string.IsNullOrEmpty(CRM_TaskDocID) && CRM_TaskDocID != "0")
            {
                if (!string.IsNullOrEmpty(Task_DealID) && Task_DealID != "0")
                {
                    string strupdt = "update [CRM_TaskDeals] set [DealName]='" + DealName.Replace("'", "''") + "',[Amount]=" + Amount + ",[DealDate]='" + DealDate + "',[ExpClsDt]='" + ExpClsDt + "',[DealStage]='" + DealStage + "',[DealStagePerc]='" + DealStagePerc + "',[DealNote]='" + DealNote.Replace("'", "''") + "',[commper] = " + commper + ",[commamt] = " + commamt +",[UpdatedBySmId]=" + Settings.Instance.SMID + " , [UpdatedDate]=Getdate() where Task_DealID='" + Task_DealID + "'";
                    DAL.DbConnectionDAL.ExecuteQuery(strupdt);
                    ID = Convert.ToInt32(Task_DealID);
                }
                else
                {
                    String varname1 = "";
                    varname1 = varname1 + "INSERT INTO [dbo].[CRM_TaskDeals] " + "\n";
                    varname1 = varname1 + "           ([DealName] " + "\n";
                    varname1 = varname1 + "           ,[CRM_TaskDocID] " + "\n";
                    varname1 = varname1 + "           ,[Amount] " + "\n";
                    varname1 = varname1 + "           ,[DealDate] " + "\n";
                    varname1 = varname1 + "           ,[ExpClsDt] " + "\n";
                    varname1 = varname1 + "           ,[DealStage] " + "\n";
                    varname1 = varname1 + "           ,[DealStagePerc] " + "\n";
                    varname1 = varname1 + "           ,[DealNote] " + "\n";
                    varname1 = varname1 + "           ,[CreatedBySmId] " + "\n";
                    varname1 = varname1 + "           ,[MonthlyDeal] " + "\n";
                    varname1 = varname1 + "           ,[TotalDealAmt],[commper],[commamt] " + "\n";
                    varname1 = varname1 + "           ,[CreatedDate]) " + "\n";
                    varname1 = varname1 + " OUTPUT Inserted.Task_DealID ";
                    varname1 = varname1 + "     VALUES " + "\n";
                    varname1 = varname1 + "             ('" + DealName.Replace("'", "''") + "',\n";
                    varname1 = varname1 + "             '" + CRM_TaskDocID + "',\n";
                    varname1 = varname1 + "             " + Amount + ",\n";
                    varname1 = varname1 + "             '" + DealDate + "',\n";
                    varname1 = varname1 + "             '" + ExpClsDt + "',\n";
                    varname1 = varname1 + "             '" + DealStage + "',\n";
                    varname1 = varname1 + "             '" + DealStagePerc + "',\n";
                    varname1 = varname1 + "             '" + DealNote.Replace("'", "''") + "',\n";
                    varname1 = varname1 + "             " + Settings.Instance.SMID + ",\n";
                    varname1 = varname1 + "             " + MonthlyDeal + ",\n";
                    varname1 = varname1 + "             " + TotalDealAmt + ",\n";
                    varname1 = varname1 + "             " + commper + ",\n";
                    varname1 = varname1 + "             " + commamt + ",\n";
                    varname1 = varname1 + "             Getdate())";                 

                    ID = DAL.DbConnectionDAL.ExecuteScaler(varname1);
                    String varname11 = "";
                    varname11 = varname11 + "GO";
                }
            }
            return Convert.ToString(ID);
        }

       [WebMethod(EnableSession = true)]
        public  string SaveImage(string Based64BinaryString)
        {

            string[] Based64BinaryString1 = Based64BinaryString.Split(',');
            string result = "";
            try
            {
                string format = "";
                string path = HttpContext.Current.Server.MapPath("imageupload/");
                string name = DateTime.Now.ToString("hhmmss");

                if (Based64BinaryString.Contains("data:application/zip;base64,"))
                {
                    format = "zip";
                }
                if (Based64BinaryString.Contains("data:;base64,"))
                {
                    format = "zip";
                }
                if (Based64BinaryString.Contains("data:image/jpeg;base64,"))
                {
                    format = "jpg";
                }
                if (Based64BinaryString.Contains("data:image/png;base64,"))
                {
                    format = "png";
                }
                if (Based64BinaryString.Contains("data:text/plain;base64,"))
                {
                    format = "txt";
                }
            
                string str = Based64BinaryString.Replace("data:image/jpeg;base64,", " ");//jpg check
                str = str.Replace("data:image/png;base64,", " ");//png check
                str = str.Replace("data:text/plain;base64,", " ");//text file check
                str = str.Replace("data:application/vnd.ms-excel;base64,", " ");//text file check
                str = str.Replace("data:;base64,", " ");//zip file check
                str = str.Replace("data:application/zip;base64,", " ");//zip file check

                byte[] data = Convert.FromBase64String(str);
               
                {
                    //MemoryStream ms = new MemoryStream(data, 0, data.Length);
                    //ms.Write(data, 0, data.Length);

               
                    //System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
                    ////image.Save(path + "/Image" + name + ".jpg");
                    ////result = "image uploaded successfully";
                }
                {
                    string strQuery = "insert into CRM_TaskDeals (uploadfile1,CRM_TaskDocID,DealName) values ('" + data + "','45','lll')";
                    DAL.DbConnectionDAL.ExecuteQuery(strQuery);
                    MemoryStream ms = new MemoryStream(data, 0, data.Length);
                    //ms.Write(data, 0, data.Length);
                    //MemoryStream ms = new MemoryStream(data);
                    FileStream fs = new FileStream(System.Web.Hosting.HostingEnvironment.MapPath
                    ("~/imageupload/") + name, FileMode.Create);
                    ms.WriteTo(fs);
                    //ms.Close();
                    //fs.Close();
                    //fs.Dispose(); 
                 
                }
            }
            catch (Exception ex)
            {
                result = "Error : " + ex;
            }
            return result;
        }

       [WebMethod(EnableSession = true)]
       public string SetPagePermission(string Page, string User)
       {
           DataTable dttask = new DataTable();
           if (!string.IsNullOrEmpty(User))
           {
               int PageID = Settings.Instance.PageIDfromPageName(Page);
               string strQ = @" select  (case ViewP when 1 then 'true' else 'false' end) As ViewP,
 (case AddP when 1 then 'true' else 'false' end ) As AddP,
  (case EditP when 1 then 'true' else 'false' end) As EditP,
  (case DeleteP when 1 then 'true' else 'false' end ) As DeleteP,
   (case ExportP when 1 then 'true' else 'false' end)  as ExportP from MastRolePermission where PageId=" + PageID + " and RoleId=" + Settings.Instance.RoleID + "";
               dttask = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
               if (dttask.Rows.Count > 0)
               {
               }

           }
           return JsonConvert.SerializeObject(dttask);
       }

       [WebMethod(EnableSession = true)]
       public string SetPagePermissionActivity(string Activity, string User)
       {
           DataTable dttask = new DataTable();
           if (!string.IsNullOrEmpty(User))
           {

               string strQ = @" select  (case Allow when 1 then 'true' else 'false' end) As Allow from CRM_MastRolePermission where [Activity Name]='" + Activity + "' and RoleId=" + Settings.Instance.RoleID + "";
               dttask = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
               if (dttask.Rows.Count > 0)
               {
               }

           }
           return JsonConvert.SerializeObject(dttask);
       }

       [WebMethod(EnableSession = true)]
       public string GetCompleteHistory(string Ref_DocId)
       {
           DataTable dtHis = new  DataTable();
           if (Ref_DocId != "")
           {

               string strQ = "";
               strQ = strQ + "select [Task_CallID],[CRMTask_DocId], CASE WHEN [CRM_TaskCall].Phone is null THEN [CRM_TaskCall].mailTo  ELSE [CRM_TaskCall].Phone END AS Phone,Result,CallNotes,0 As Task_NoteID,'' As Notes, '' as[DocId],'' as TDocId,'' As [Task]," + "\n";
               strQ = strQ + " '' as AssignedTo,'' As [AssignedBy],'' as Adate,'' As [Ref_DocId],0 As [Ref_Sno],'' As [Status],0 as [SmId],'' As Smname,0 As [Task_DealID]," + "\n";
               strQ = strQ + " '' As [CRM_TaskDocID],0 As MonthlyDeal,0 as TotalDealAmt,'' As [DealName],0 As [Amount],'' as DealDate,'' as ExpClsDt,'' as DealStage," + "\n";
               strQ = strQ + "'' As [DealStagePerc],'' As [DealNote],CASE WHEN [CRM_TaskCall].Phone is null THEN 'E'  ELSE 'C' END  As Type,[CRM_TaskCall].CreatedDate As CreatedDate,convert(varchar(12),[CRM_TaskCall].[CreatedDate],106) as DisplayDate,ms.SMName AS username from [CRM_TaskCall] LEFT JOIN MastSalesRep ms ON ms.SMID=[CRM_TaskCall].CreatedBySmId where [CRMTask_DocId]='" + Ref_DocId + "' " + "\n";
               strQ = strQ + " Union all select 0 As [Task_CallID],'' As [CRMTask_DocId],'' As Phone,'' As Result,'' As CallNotes,Task_NoteID,Notes, '' as[DocId], " + "\n";
               strQ = strQ + " '' as TDocId,'' As [Task],'' as AssignedTo,'' As [AssignedBy],'' as Adate,'' As [Ref_DocId],0 As [Ref_Sno],'' As [Status],0 as [SmId]," + "\n";
               strQ = strQ + "'' As Smname,0 As [Task_DealID],'' As [CRM_TaskDocID],0 As MonthlyDeal,0 as TotalDealAmt,'' As [DealName],0 As [Amount],'' as DealDate," + "\n";
               strQ = strQ + "'' as ExpClsDt,'' as DealStage,'' As [DealStagePerc],'' As [DealNote],'N' As Type,[CRM_TaskNotes].CreatedDate As CreatedDate,convert(varchar(12),[CRM_TaskNotes].[CreatedDate],106) as DisplayDate,ms.SMName AS username from [CRM_TaskNotes] LEFT JOIN MastSalesRep ms ON ms.SMID=[CRM_TaskNotes].CreatedBySmId where CRMTask_DocId='" + Ref_DocId + "' " + "\n";
               strQ = strQ + " Union all select  0 As [Task_CallID],'' As [CRMTask_DocId],'' As Phone,'' As Result,'' As CallNotes,0 As Task_NoteID,'' As Notes, '' as[DocId]," + "\n";
               strQ = strQ + "'' as TDocId,'' As [Task],'' as AssignedTo,'' As [AssignedBy],'' as Adate,'' As [Ref_DocId],0 As [Ref_Sno],'' As [Status],0 as [SmId],'' As Smname," + "\n";
               strQ = strQ + "[Task_DealID],[CRM_TaskDocID],MonthlyDeal,isnull(TotalDealAmt,0)as TotalDealAmt,[DealName],[Amount],convert(varchar(12),[DealDate],106) as DealDate," + "\n";
               strQ = strQ + " Convert(varchar(12),[ExpClsDt],106) as ExpClsDt,case [DealStage] when 'l' then 'Lost' when 'w' then 'Won' else 'Pending' end as DealStage," + "\n";
               strQ = strQ + "[DealStagePerc],[DealNote],'D' As Type,[CRM_TaskDeals].CreatedDate As CreatedDate,convert(varchar(12),[CRM_TaskDeals].[CreatedDate],106) as DisplayDate,ms.SMName AS username from [CRM_TaskDeals] LEFT JOIN MastSalesRep ms ON ms.SMID=[CRM_TaskDeals].CreatedBySmId where [CRM_TaskDocID]='" + Ref_DocId + "' " + "\n";
               strQ = strQ + " Union all select  0 As [Task_CallID],'' As [CRMTask_DocId],'' As Phone,'' As Result,'' As CallNotes,0 As Task_NoteID,'' As Notes," + "\n";
               strQ = strQ + " cast(REPLACE(DocId, ' ', '') as varchar)as[DocId],ISNULL(cast(REPLACE(DocId, ' ', '') as varchar)+ cast( Ref_Sno as varchar),DocId) as TDocId," + "\n";
               strQ = strQ + "[Task],isnull([dbo].[CRMGetAssignedToNames]([AssignedTo]),'none') as AssignedTo,[AssignedBy],convert(varchar(12),[AssignDate],106) as Adate," + "\n";
               strQ = strQ + "[Ref_DocId],[Ref_Sno],[Status],CRM_Task.CreatedBySmId as [SmId],msr.Smname,0 As [Task_DealID],'' As [CRM_TaskDocID],0 As MonthlyDeal,0 as TotalDealAmt," + "\n";
               strQ = strQ + "'' As [DealName],0 As [Amount],'' as DealDate,'' as ExpClsDt,'' as DealStage,'' As [DealStagePerc],'' As [DealNote],'A' As Type,CRM_Task.CreatedDate As CreatedDate,convert(varchar(12),CRM_Task.CreatedDate,106) as DisplayDate,ms.SMName AS username   from [CRM_Task] LEFT JOIN MastSalesRep ms ON ms.SMID=[CRM_Task].CreatedBySmId " + "\n";
               strQ = strQ + "inner join [MastSalesRep] msr on CRM_Task.AssignedBy=msr.Smid  where 1=1 and (Ref_DocId='" + Ref_DocId + "' or DocId='" + Ref_DocId + "') order by CreatedDate Desc";
               //string strQ = "select Task_NoteID,Notes from [CRM_TaskNotes] where CRMTask_DocId='" + TaskDocId + "'" + addqry + " order by CreatedDate";
                dtHis = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
               if (dtHis.Rows.Count > 0)
               {
               }
           }
           return JsonConvert.SerializeObject(dtHis);
       }
       [WebMethod(EnableSession = true)]
       public string GetTaskCalls(string TaskDocId, string TaskCallId)
       {
           string addqry = "";
           if (!string.IsNullOrEmpty(TaskCallId) && TaskCallId != "0")
           {
               addqry += " and Task_CallID=" + TaskCallId + "";
           }
           string strQ = "select '' mailedpersonid,'' mailedpersonemail,[Task_CallID],[CRMTask_DocId],mailto,Phone,Result,CallNotes,CreatedDate As CreatedDate,convert(varchar(12),CreatedDate,106) as DisplayDate from [CRM_TaskCall] where [CRMTask_DocId]='" + TaskDocId + "'" + addqry + " order by CreatedDate Desc";
           DataTable dttaskc = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
      
               if (!string.IsNullOrEmpty(Convert.ToString(dttaskc.Rows[0]["mailto"])))
           {
               string[] mail = Convert.ToString(dttaskc.Rows[0]["mailto"]).Split('-');
               string sql = @"select * from CRM_ContactMobile where contactname='" + mail[0] + "' and phonetype='" + mail[1] + "' ";
               dttaskc = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
               strQ = "select '" + dttaskc.Rows[0]["CMbl_Id"].ToString() + "' mailedpersonid,'" + dttaskc.Rows[0]["email"].ToString() + "' mailedpersonemail,[Task_CallID],[CRMTask_DocId],mailto,Phone,Result,CallNotes,CreatedDate As CreatedDate,convert(varchar(12),CreatedDate,106) as DisplayDate from [CRM_TaskCall] where [CRMTask_DocId]='" + TaskDocId + "'" + addqry + " order by CreatedDate Desc";
                dttaskc = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
           }
           if (dttaskc.Rows.Count > 0)
           {
           }
           return JsonConvert.SerializeObject(dttaskc);
       }

       [WebMethod(EnableSession = true)]
       public string GetTaskDeal(string TaskDocId, string TaskDealId)
       {
           string addqry = "";
           if (!string.IsNullOrEmpty(TaskDealId) && TaskDealId != "0")
           {
               addqry += " and Task_DealID=" + TaskDealId + "";
           }
           string strQ = "select [Task_DealID],[CRM_TaskDocID],MonthlyDeal,isnull(commper,0) as commper,isnull(commamt,0) as commamt ,isnull(TotalDealAmt,0)as TotalDealAmt,[DealName],[Amount],convert(varchar(12),[DealDate],106) as DealDate,convert(varchar(12),[ExpClsDt],106) as ExpClsDt,case [DealStage] when 'l' then 'Lost' when 'w' then 'Won' else 'Pending' end as DealStage,[DealStagePerc],[DealNote],CreatedDate As CreatedDate,convert(varchar(12),CreatedDate,106) as DisplayDate  from [CRM_TaskDeals] where [CRM_TaskDocID]='" + TaskDocId + "'" + addqry + " order by CreatedDate Desc";
           DataTable dttaskd = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
           if (dttaskd.Rows.Count > 0)
           {
           }
           return JsonConvert.SerializeObject(dttaskd);
       }

       [WebMethod(EnableSession=true)]
       public string gettaskbydocid (string DocId)
       {
            DataTable dttaskd = new DataTable();
            if (!string.IsNullOrEmpty(DocId) && DocId != "0")
            {
                string strQ = " SELECT Max(Tag) AS Tag,Max(status) AS status,Max(task) AS Task, contact_id,Max(NAME) AS NAME ,Max(compname) AS compname,Max(Towner) AS Towner,";
                    strQ += " Max(Tstatus) AS Tstatus,Max(assgndate) AS assgndate ,Max(assgndate1) AS assgndate1,Max(weekdt) AS weekdt,Max(currentdt) AS currentdt,max(Flag) as Flag,max(Adate) as Adate  From(";
                    strQ += "SELECT  isnull(cmt.Tag,'') as Tag,cms.status, ct.Task,ct.contact_id ,Isnull(cc. firstname,'') + ' ' + Isnull(cc.lastname,'')  AS NAME, cmc.compname,ms.SMName AS Towner,(CASE ct.status  WHEN 'c' THEN 'Close' ELSE 'Open'  END  ) AS Tstatus,";
                    strQ += " isnull(CONVERT(VARCHAR(6), ct.AssignDate, 106),'')   as assgndate,    isnull(CONVERT(VARCHAR(10), ct.AssignDate, 101),'')   as assgndate1,     CONVERT(VARCHAR(10),dateadd(WW,1,cc.CreatedDate), 101)  as weekdt,  CONVERT(VARCHAR(10),CONVERT(VARCHAR(10),GETDATE(), 101), 101)  as currentdt,";
                    strQ += " CC.Flag as Flag,isnull(ct.AssignDate,'')  as Adate  FROM CRM_TASK ct LEFT JOIN CRM_MastContact CC ON CC.Contact_Id=ct.Contact_Id  left JOIN CRM_MastTag cmt  ON cc.Tag_Id = cmt.Tag_Id   LEFT JOIN MastSalesRep ms ON ms.SMId=cc.Manager ";
                    strQ += " INNER JOIN crm_mastcompany cmc  ON cc.companyid = cmc.comp_id  INNER JOIN crm_maststatus cms  ON cc.status_id = cms.status_id  WHERE REPLACE(ct.DocId,' ','') ='" + DocId + "'";
                    strQ += " Union ";
                    strQ += " SELECT  isnull(cmt.Tag,'') as Tag,cms.status, ct.Task,ct.contact_id ,Isnull(cc. firstname,'') + ' ' + Isnull(cc.lastname,'')  AS NAME,'' AS compname,ms.SMName AS Towner,(CASE ct.status  WHEN 'c' THEN 'Close' ELSE 'Open'  END  ) AS Tstatus, isnull(CONVERT(VARCHAR(6), ct.AssignDate, 106),'')   as assgndate,";  
                    strQ += " isnull(CONVERT(VARCHAR(10), ct.AssignDate, 101),'')   as assgndate1,  CONVERT(VARCHAR(10),dateadd(WW,1,cc.CreatedDate), 101)  as weekdt,  CONVERT(VARCHAR(10),CONVERT(VARCHAR(10),GETDATE(), 101), 101)  as currentdt, CC.Flag as Flag,";
                    strQ += " isnull(ct.AssignDate,'')  as Adate  FROM CRM_TASK ct LEFT JOIN CRM_MastContact CC ON CC.Contact_Id=ct.Contact_Id  left JOIN CRM_MastTag cmt  ON cc.Tag_Id = cmt.Tag_Id   LEFT JOIN MastSalesRep ms ON ms.SMId=cc.Manager  ";
                    strQ += " INNER JOIN crm_maststatus cms  ON cc.status_id = cms.status_id  WHERE REPLACE(ct.DocId,' ','') ='" + DocId + "')";
                strQ += " a GROUP BY contact_id  order by assgndate1 desc ";
          
                    dttaskd = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
                    if (dttaskd.Rows.Count > 0)
                    {

                    }
        
             
            }
            return JsonConvert.SerializeObject(dttaskd);

       }
       [WebMethod(EnableSession = true)]
       public string GetTaskNote(string TaskDocId, string TaskNoteId)
       {
           string addqry = "";
           if (!string.IsNullOrEmpty(TaskNoteId) && TaskNoteId != "0")
           {
               addqry += " and Task_NoteID=" + TaskNoteId + "";
           }
           string strQ = "select Task_NoteID,Notes,CreatedDate As CreatedDate,convert(varchar(12),CreatedDate,106) as DisplayDate  from [CRM_TaskNotes] where CRMTask_DocId='" + TaskDocId + "'" + addqry + " order by CreatedDate Desc";
           DataTable dttask = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
           if (dttask.Rows.Count > 0)
           {
           }
           return JsonConvert.SerializeObject(dttask);
       }

       [WebMethod(EnableSession = true)]
       public string GetTaskDetails(string Ref_DocId)
       {
           DataTable dt = new DataTable();
           if (Ref_DocId != "")
           {


               //string strQ = "select smname, FROM [dbo].[CRM_Task] ct inner join [MastSalesRep] msr on ct.smid=msr.smid Inner Join Crm_MastCompany mcn on  where [status]='P' order by AssignDate desc";
               string strQ = "select cast(REPLACE(DocId, ' ', '') as varchar)as[DocId],ISNULL(cast(REPLACE(DocId, ' ', '') as varchar)+ cast( Ref_Sno as varchar),DocId) as TDocId,[Task],isnull([dbo].[CRMGetAssignedToNames]([AssignedTo]),'none') as AssignedTo,[AssignedBy],convert(varchar(12),[AssignDate],106) as Adate,[Ref_DocId],[Ref_Sno],[Status],CRM_Task.CreatedBySmId as [SmId],Smname,CRM_Task.CreatedDate As CreatedDate,convert(varchar(12),CRM_Task.CreatedDate,106) as DisplayDate   from [CRM_Task] inner join [MastSalesRep] msr on CRM_Task.AssignedBy=msr.Smid  where 1=1 and (Ref_DocId='" + Ref_DocId + "' or DocId='" + Ref_DocId + "') order by CreatedDate Desc";
               dt = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
               if (dt.Rows.Count > 0)
               {
               }
           }
           return JsonConvert.SerializeObject(dt);
       }

       [WebMethod(EnableSession = true)]
       public string GetContacts(string contactId)
       {
           //string strQ = "select Contact_Id,isnull(max(ContactName),'') as ContactName,isnull(max(Phone),'') as Phone,isnull(max(Email),'') as Email from (SELECT Contact_Id,ContactName,Phone     ,'' as Email from CRM_ContactMobile   union Select Contact_Id,ContactName,'' asPhone,      Email from  CRM_ContactEmail   ) a   where Contact_Id=" + contactId + "   group by Contact_Id,ContactName";
           string strQ = "SELECT cm.Contact_Id,isnull(cm.ContactName,'') AS ContactName,isnull(cm.Phone,'') AS Phone from CRM_ContactMobile cm  where cm.Contact_Id=" + contactId + "";
           DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
           if (dt.Rows.Count > 0)
           {
           }
           return JsonConvert.SerializeObject(dt);
       }
       [WebMethod(EnableSession = true)]
       public string GetContactsMail(string contactId)
       {
           //string strQ = "select Contact_Id,isnull(max(ContactName),'') as ContactName,isnull(max(Phone),'') as Phone,isnull(max(Email),'') as Email from (SELECT Contact_Id,ContactName,Phone     ,'' as Email from CRM_ContactMobile   union Select Contact_Id,ContactName,'' asPhone,      Email from  CRM_ContactEmail   ) a   where Contact_Id=" + contactId + "   group by Contact_Id,ContactName";
           string strQ = "SELECT ce.Contact_Id,ce.CEmail_Id,ce.Email as email from CRM_ContactEmail	ce  where ce.Contact_Id=" + contactId + "";
           DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
           if (dt.Rows.Count > 0)
           {
           }
           return JsonConvert.SerializeObject(dt);
       }
         [WebMethod(EnableSession = true)]
       public string GetContactsUrls (string contactId)
       {
           //string strQ = "select Contact_Id,isnull(max(ContactName),'') as ContactName,isnull(max(Phone),'') as Phone,isnull(max(Email),'') as Email from (SELECT Contact_Id,ContactName,Phone     ,'' as Email from CRM_ContactMobile   union Select Contact_Id,ContactName,'' asPhone,      Email from  CRM_ContactEmail   ) a   where Contact_Id=" + contactId + "   group by Contact_Id,ContactName";
           string strQ = "SELECT ce.Contact_Id,ce.CUrl_Id,ce.Url as Url from CRM_ContactURL	ce  where ce.Contact_Id=" + contactId + "";
           DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
           if (dt.Rows.Count > 0)
           {
           }
           return JsonConvert.SerializeObject(dt);
       }
       [WebMethod(EnableSession = true)]
       public string GetTaskNoteCall(string ref_docid)
       {
           string strQ = "";
           strQ = strQ + "select [Task_CallID],[CRMTask_DocId],[CRM_TaskCall].Phone,Result,CallNotes,0 As Task_NoteID,'' As Notes, '' as[DocId],'' as TDocId,'' As [Task]," + "\n";
           strQ = strQ + " '' as AssignedTo,'' As [AssignedBy],'' as Adate,'' As [Ref_DocId],0 As [Ref_Sno],'' As [Status],0 as [SmId],'' As Smname,0 As [Task_DealID]," + "\n";
           strQ = strQ + " '' As [CRM_TaskDocID],0 As MonthlyDeal,0 as TotalDealAmt,'' As [DealName],0 As [Amount],'' as DealDate,'' as ExpClsDt,'' as DealStage," + "\n";
           strQ = strQ + "'' As [DealStagePerc],'' As [DealNote],'C' As Type,[CRM_TaskCall].CreatedDate As CreatedDate,convert(varchar(12),[CRM_TaskCall].[CreatedDate],106) as DisplayDate,ms.SMName as username from [CRM_TaskCall] LEFT JOIN MastSalesRep ms ON ms.SMID=[CRM_TaskCall].CreatedBySmId where [CRMTask_DocId]='" + ref_docid + "' " + "\n";
           strQ = strQ + " Union all select 0 As [Task_CallID],'' As [CRMTask_DocId],'' As Phone,'' As Result,'' As CallNotes,Task_NoteID,Notes, '' as[DocId], " + "\n";
           strQ = strQ + " '' as TDocId,'' As [Task],'' as AssignedTo,'' As [AssignedBy],'' as Adate,'' As [Ref_DocId],0 As [Ref_Sno],'' As [Status],0 as [SmId]," + "\n";
           strQ = strQ + "'' As Smname,0 As [Task_DealID],'' As [CRM_TaskDocID],0 As MonthlyDeal,0 as TotalDealAmt,'' As [DealName],0 As [Amount],'' as DealDate," + "\n";
           strQ = strQ + "'' as ExpClsDt,'' as DealStage,'' As [DealStagePerc],'' As [DealNote],'N' As Type,[CRM_TaskNotes].CreatedDate As CreatedDate,convert(varchar(12),[CRM_TaskNotes].[CreatedDate],106) as DisplayDate,ms.SMName as username  from [CRM_TaskNotes] LEFT JOIN MastSalesRep ms ON ms.SMID=[CRM_TaskNotes].CreatedBySmId where CRMTask_DocId='" + ref_docid + "' ";

           DataTable dttaskc = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
           if (dttaskc.Rows.Count > 0)
           {
           }
           return JsonConvert.SerializeObject(dttaskc);
       }


        //20-07-2021----------- work merged by jyoti mam for activity template 

       #region DynamicActivity

       [WebMethod(EnableSession = true)]
       public string SaveCustomFieldsForActivityNew(string Table, string Title, string Fromdate, string ToDate, string For, string Fieldname, string Fieldtype, string Data, string Sorting, string filterctrlno, string parametername, string Active, string TempActive, string Required, string CustomIds, string NoImgsForGallery)
       {
           string result = "0";
           string viewname = "";
           string ins = "";
           int HeaderId = 0;
           string[] arrFieldname = Fieldname.Split(',');
           string[] arrFieldtype = Fieldtype.Split(',');
           string[] arrData = Data.Split('*');
           string[] arrSorting = Sorting.Split(',');
           string[] arrActive = Active.Split(',');
           string[] arrRequired = Required.Split(',');
           string[] arrCustomIds = CustomIds.Split(',');
           string[] arrNoImgsForGallery = NoImgsForGallery.Split(',');
           int existingheader_id = 0;
           string strQ = "";
           DataTable dt = new DataTable();


           if (Convert.ToInt16(DAL.DbConnectionDAL.GetScalarValue(CommandType.Text, "select count(*) from [MastActivityCustomHeader] where Fromdate='" + Fromdate + "' and Todate='" + ToDate + "' and Title='" + Title + "'")) > 0)   // and [For] ='" + For + "'  //and Title='" + Title + "'
           {
               existingheader_id = DAL.DbConnectionDAL.ExecuteScaler("select Header_Id from [MastActivityCustomHeader] where Fromdate='" + Fromdate + "' and Todate='" + ToDate + "' and Title='" + Title + "' ");// and [For] ='" + For + "'

               strQ = "select COUNT(id)  from TransAcivityMapping  where TemplateHeaderId=" + existingheader_id;
               dt = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
               if (dt.Rows.Count > 0) //if Template Is assigned Someone.
               {
                   ins = "Update MastActivityCustomHeader set Active ='" + TempActive + "' where Header_Id='" + existingheader_id + "'";
                   DAL.DbConnectionDAL.ExecuteQuery(ins);

               }
               else
               {
                   ins = "Update MastActivityCustomHeader set Title ='" + Title + "' where Header_Id=" + existingheader_id + "";
                   DAL.DbConnectionDAL.ExecuteQuery(ins);

                   for (int i = 0; i < arrFieldname.Length; i++)
                   {
                       strQ = "select * from MastActivityCustom where Custom_Id='" + arrCustomIds[i] + "' and Header_Id= '" + existingheader_id + "'";

                       dt = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
                       if (dt.Rows.Count == 0)
                       {
                           ins = "insert into MastActivityCustom (AttributeTable,AttributeField,AttributFieldType,AttributeData,sort,filctrrlbysortno,parametername,Active,Required,Header_Id,NoImgsForGallery) values('" + Table + "','" + arrFieldname[i].ToString().Trim() + "','" + arrFieldtype[i] + "','" + arrData[i] + "'," + arrSorting[i] + ",'" + filterctrlno + "','" + parametername + "','" + arrActive[i] + "','" + arrRequired[i] + "'," + existingheader_id + ",'" + arrNoImgsForGallery[i] + "')";
                           DAL.DbConnectionDAL.ExecuteQuery(ins);

                       }
                       else
                       {
                           ins = "Update MastActivityCustom set Active ='" + arrActive[i] + "' where Custom_Id='" + arrCustomIds[i] + "' ";
                           DAL.DbConnectionDAL.ExecuteQuery(ins);
                       }


                   }

               }

               //result = "-1";

           }
           else
           {
               ins = "INSERT INTO MastActivityCustomHeader(Fromdate,	Todate,	[FOR],	Title,Active	) OUTPUT Inserted.Header_Id values('" + Fromdate + "','" + ToDate + "','" + For + "','" + Title + "','" + TempActive + "')";
               HeaderId = DAL.DbConnectionDAL.ExecuteScaler(ins);
               if (HeaderId != 0)
               {
                   for (int i = 0; i < arrFieldname.Length; i++)
                   {
                       ins = "insert into MastActivityCustom (AttributeTable,AttributeField,AttributFieldType,AttributeData,sort,filctrrlbysortno,parametername,Active,Required,Header_Id,NoImgsForGallery) values('" + Table + "','" + arrFieldname[i] + "','" + arrFieldtype[i] + "','" + arrData[i] + "'," + arrSorting[i] + ",'" + filterctrlno + "','" + parametername + "','" + arrActive[i] + "','" + arrRequired[i] + "'," + HeaderId + ",'" + arrNoImgsForGallery[i] + "')";
                       DAL.DbConnectionDAL.ExecuteQuery(ins);
                       //string datatype = "varchar(25)";
                       //if (arrFieldtype[i] == "Number")
                       //{
                       //    datatype = "numeric(18,2)";
                       //}
                       //else if (arrFieldtype[i] == "Date")
                       //{
                       //    datatype = "datetime";
                       //}
                       //else if (arrFieldtype[i] == "Single Line Text")
                       //{
                       //    datatype = "varchar(25)";
                       //}
                       //else if (arrFieldtype[i] == "Multiple Line Text")
                       //{
                       //    datatype = "varchar(max)";
                       //}

                       //string insContact = "alter table [ActivityTransaction] add [" + arrFieldname[i] + "] " + datatype + " null";
                       //DAL.DbConnectionDAL.ExecuteQuery(insContact);
                   }
               }
           }
           return result;
       }



       [WebMethod(EnableSession = true)]
       public string GetCustomFieldsForActivityNew()
       {
           DataTable dt = new DataTable();

           string strQ = "select  replace(convert(varchar(12),[Todate],106),' ','/') As Todate, replace(convert(varchar(12),[Fromdate],106),' ','/') As Fromdate,Title,Header_Id,[FOR] AS Type,isnull(active,0) as active from MastActivityCustomHeader order by Title";
           dt = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
           return JsonConvert.SerializeObject(dt);
       }
       [WebMethod(EnableSession = true)]
       public string GetCustomfieldLineDataNew(string HeaderId)
       {

           DataTable dt = new DataTable();
           if (!string.IsNullOrEmpty(HeaderId) && HeaderId != "0")
           {
               string strQ = "select Replace(convert(varchar(12),[Todate],106),' ','/') As Todate,Replace(convert(varchar(12),[Fromdate],106),' ','/') As Fromdate,ma.AttributeField,ma.AttributFieldType,ma.AttributeData,ma.sort,ma.Active,ma.Header_Id,mh.Title,ma.Custom_Id,mh.[FOR] AS Type,ma.required,mh.Active as tempActive,ma.NoImgsForGallery from MastActivityCustom ma ";
               strQ += " LEFT JOIN MastActivityCustomHeader mh ON mh.Header_Id=ma.Header_Id  where ma.Header_Id=" + Convert.ToInt32(HeaderId) + " order by ma.sort";
               dt = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
           }
           return JsonConvert.SerializeObject(dt);
       }

       #endregion
    }
}
