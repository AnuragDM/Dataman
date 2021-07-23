using BAL;
using DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.Serialization;
using System.Transactions;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using BusinessLayer;
using Newtonsoft.Json;



namespace AstralFFMS
{
    /// <summary>
    /// Summary description for Tally_webservice
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Tally_webservice : System.Web.Services.WebService
    {
        DistributorBAL DB = new DistributorBAL();
        PartyBAL PB = new PartyBAL();
        string constrDmLicense = "data source=103.231.40.154,1565; user id=dmlicense_user; pwd=SaG@e321; initial catalog=dmlicense;";
        public class Purchorderlist
        {
            public List<Transpurchorder> result { get; set; }
        }
        public class Transpurchorder
        {
            public string Docid { get; set; }
            public int Pordid { get; set; }
            public string Distid { get; set; }
            public string Partyname { get; set; }
            public string VDate { get; set; }

            public string Remark { get; set; }

            public DateTime createddate { get; set; }
            public decimal totalamount { get; set; }
            public string Vno { get; set; }
            public string referenceno { get; set; }
            public List<Transpurchorder1> purchorder1 { get; set; }

        }

        public class Transpurchorder1
        {
            public string Docid { get; set; }

            public int sno { get; set; }

            public string itemid { get; set; }

            public decimal Quantity { get; set; }
            public decimal Amount { get; set; }

            public string Unit { get; set; }
            public decimal Rate { get; set; }

            public DateTime VDate { get; set; }



        }
        public class errorresult
        {
            public string msg;
            public List<mandatorymsg> errormsg;
        }
        public class mandatorymsg
        {
            public string msdmsg;
        }
        public class Parameters
        {
            public string LastSyncId { get; set; }
            public string Stockgroupname { get; set; }
        }
        public class Getparameter
        {
            public Parameters getparameter1 { get; set; }
        }
        public class Result1
        {
            public string Message;
        }
        [DataContract]
        public class Result
        {
            [DataMember]
            public string ResultMsg { get; set; }

        }

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }


        [WebMethod]
        public Getparameter getparameter(string compcode)
        {
            Parameters p = new Parameters();


            p.LastSyncId = Convert.ToString(DbConnectionDAL.ExecuteScaler("Select LastSynID from TallyCounter where Type='S' and compcode='" + compcode + "'"));
            p.Stockgroupname = "SAMSUNG";

            var gq = new Getparameter
            {
                getparameter1 = p

            };



            return gq;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public Result insertdistributorLedfromtally()
        {

            string _query = "", _fList = "", _sql = "";
            var myInsertList = new List<string>();
            var myDuplicateList = new List<string>();
            int cnt = 0, result = 0;
            decimal _amount = 0;
            decimal amtdr = 0;
            decimal amtcr = 0;
            string docID;
            int success = 0;
            int failure = 0;
            string str = "";
            Result rs = new Result();
            var bodyStream = new StreamReader(HttpContext.Current.Request.InputStream);
            DataTable DTadmin = new DataTable();
            bodyStream.BaseStream.Seek(0, SeekOrigin.Begin);
            var bodyText = bodyStream.ReadToEnd();
            StringReader theReader = new StringReader(bodyText);
            DataSet ds = new DataSet();
            string Narration = "";
            ds.ReadXml(theReader);
            if (ds.Tables.Count == 0)
            {
                rs.ResultMsg = "No Record Found";
                return rs;
            }
            //List<TransDistributerLedger> PurchaseOrderImportList = JsonConvert.DeserializeObject<List<TransDistributerLedger>>(value);
            if (ds.Tables[0].Rows.Count > 0)
            {
                string sqlConnectionstring = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                SqlConnection cn = new SqlConnection(sqlConnectionstring);
                cn.Open();
                SqlBulkCopy objbulk = new SqlBulkCopy(cn);
                //assigning Destination table name  
                objbulk.DestinationTableName = "TempTally_TransDistLedger";
                //Mapping Table column  
                objbulk.ColumnMappings.Add("PARTYNAME", "PARTYNAME");
                objbulk.ColumnMappings.Add("NARRATION", "NARRATION");
                objbulk.ColumnMappings.Add("AMOUNTDR", "AMOUNTDR");
                objbulk.ColumnMappings.Add("AMOUNTCR", "AMOUNTCR");

                objbulk.ColumnMappings.Add("VTYPE", "VTYPE");
                objbulk.ColumnMappings.Add("VOUCHERID", "VOUCHERID");
                objbulk.ColumnMappings.Add("DATE", "DATE");
                objbulk.ColumnMappings.Add("PARTYID", "PARTYID");


                objbulk.ColumnMappings.Add("PARENT", "PARENT");
                objbulk.ColumnMappings.Add("PARENT2", "PARENT2");
                objbulk.ColumnMappings.Add("COMPANYNAME", "COMPANYNAME");
                objbulk.ColumnMappings.Add("COMPANYNUMBER", "COMPANYNUMBER");
                objbulk.ColumnMappings.Add("SYSTEMDATE", "SYSTEMDATE");
                objbulk.ColumnMappings.Add("SYSTEMTIME", "SYSTEMTIME");

                //inserting bulk Records into DataBase   
                objbulk.WriteToServer(ds.Tables[0]);
                cn.Close();

                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, "Delete FROM TransDistributerLedger WHERE DistLedId NOT In(SELECT DistLedId FROM TransDistributerLedger WHERE Narration=' Opening Balance' OR COMPANYCODE !='" + ds.Tables[0].Rows[0]["COMPANYNUMBER"].ToString() + "') ");


                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (ds.Tables[0].Rows[i]["AMOUNTCR"].ToString() == "5,210.00")
                    {

                    }
                    if (ds.Tables[0].Rows[i]["AMOUNTDR"].ToString() == "5,212.00")
                    {

                    }
                    if (ds.Tables[0].Rows[i]["AMOUNTCR"].ToString() != "")
                    {
                        if (ds.Tables[0].Rows[i]["AMOUNTCR"].ToString().IndexOf(")") > 0)
                        {
                            amtcr = Convert.ToDecimal(ds.Tables[0].Rows[i]["AMOUNTCR"].ToString().Substring(ds.Tables[0].Rows[i]["AMOUNTCR"].ToString().IndexOf(")") + 1, ds.Tables[0].Rows[i]["AMOUNTCR"].ToString().Length - 1 - ds.Tables[0].Rows[i]["AMOUNTCR"].ToString().IndexOf(")")));
                        }
                        else
                        {
                            amtcr = Convert.ToDecimal(ds.Tables[0].Rows[i]["AMOUNTCR"].ToString());
                        }
                    }
                    if (ds.Tables[0].Rows[i]["AMOUNTDR"].ToString() != "")
                    {
                        if (ds.Tables[0].Rows[i]["AMOUNTDR"].ToString().IndexOf(")") > 0)
                        {
                            amtdr = Convert.ToDecimal(ds.Tables[0].Rows[i]["AMOUNTDR"].ToString().Substring(ds.Tables[0].Rows[i]["AMOUNTDR"].ToString().IndexOf(")") + 1, ds.Tables[0].Rows[i]["AMOUNTDR"].ToString().Length - 1 - ds.Tables[0].Rows[i]["AMOUNTDR"].ToString().IndexOf(")")));
                        }
                        else
                        {
                            amtdr = Convert.ToDecimal(ds.Tables[0].Rows[i]["AMOUNTDR"].ToString());
                        }
                    }
                    docID = "TL" + ds.Tables[0].Rows[i]["VTYPE"].ToString().Substring(0, 3).ToUpper() + ds.Tables[0].Rows[i]["COMPANYNUMBER"].ToString().Substring(ds.Tables[0].Rows[i]["COMPANYNUMBER"].ToString().Length - 3) + " " + Convert.ToString(Convert.ToDateTime(ds.Tables[0].Rows[i]["DATE"].ToString()).Year) + " " + ds.Tables[0].Rows[i]["VOUCHERID"].ToString();

                    if (ds.Tables[0].Rows[i]["VTYPE"].ToString() != "")
                        Narration = ds.Tables[0].Rows[i]["NARRATION"].ToString() + "  (Vch Type-" + ds.Tables[0].Rows[i]["VTYPE"].ToString() + ")";
                    else
                        Narration = ds.Tables[0].Rows[i]["NARRATION"].ToString();
                    _query = "Select isnull(PartyId,0) from MastParty where SyncId='" + ds.Tables[0].Rows[i]["COMPANYNUMBER"].ToString() + "#" + ds.Tables[0].Rows[i]["PARTYID"].ToString() + "' and compcode='" + ds.Tables[0].Rows[i]["COMPANYNUMBER"].ToString() + "' And partydist=1";
                    int _DistID = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, _query));
                    if (_DistID != 0)
                    {
                        if (amtcr > 0) _amount = amtcr * -1;
                        else if (amtdr > 0) _amount = amtdr;

                        result = DB.insertdistledger_Tally(_DistID, docID,
                            ds.Tables[0].Rows[i]["DATE"].ToString(),
                            amtdr, amtcr, _amount, Narration, ds.Tables[0].Rows[i]["VOUCHERID"].ToString(), ds.Tables[0].Rows[i]["VTYPE"].ToString(),
                            ds.Tables[0].Rows[i]["COMPANYNUMBER"].ToString());

                        if (result > 0)
                        {
                            success = success + 1;
                            amtcr = 0;
                            amtdr = 0;
                            _amount = 0;


                        }
                        else
                        {
                            failure = failure + 1;
                            amtcr = 0;
                            amtdr = 0;
                            _amount = 0;

                        }



                    }
                    amtcr = 0;
                    amtdr = 0;
                    _amount = 0;

                }
            }
            str = success + " record inserted successfully ";

            if (failure > 0)
            {
                str = str + "," + failure + " record are failed";
            }

            rs.ResultMsg = str;
            return rs;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public Result insertItemfromtally()
        {
            int cnt = 0, exist = 0;
            int result1 = 0;
            int result = 0;
            string _query = "";
            string _sql = "";
            bool active;
            List<Result1> rst = new List<Result1>();
            string paramInfo = "";
            int success = 0;
            int failure = 0;
            string str = "";
            DataTable DTitem = new DataTable();
            var bodyStream = new StreamReader(HttpContext.Current.Request.InputStream);
            DataTable DTadmin = new DataTable();
            DataTable DTitemgrp = new DataTable();
            bodyStream.BaseStream.Seek(0, SeekOrigin.Begin);
            var bodyText = bodyStream.ReadToEnd();
            StringReader theReader = new StringReader(bodyText);
            DataSet ds = new DataSet();
            ds.ReadXml(theReader);
            Result rs = new Result();
            MaterialGroup MG = new MaterialGroup();
            ItemBAL IB = new ItemBAL();
            var distinctitemgroup = ds.Tables[0].AsEnumerable().GroupBy(s => s.Field<string>("ITEMPARENT")).Select(s => new
            {
                ITEMPARENT = s.Key,
                ParentMasterID = s.Max(d => d.Field<string>("PARENTMASTID")),
                COMPANYNUMBER = s.Max(d => d.Field<string>("COMPANYNUMBER"))
                //VDATE = s.Max(x => x.Field<string>("DATE")),
                //BillAmount = s.Max(x => x.Field<string>("AMOUNT")),
                //Masterid = s.Max(x => x.Field<string>("MASTER")),
                //Partyid = s.Max(x => x.Field<string>("PARTYMASTERID"))

            }).ToList();



            for (int i = 0; i < distinctitemgroup.Count; i++)
            {
                //DTitemgrp = DbConnectionDAL.GetDataTable(CommandType.Text, "Select * from MastItem where SyncId='" + distinctitemgroup[i].COMPANYNUMBER.Replace("'", "''") + "#" + distinctitemgroup[i].ParentMasterID.Replace("'", "''") + "' and Itemtype='MATERIALGROUP'");

                //if (DTitemgrp.Rows.Count == 0)
                //{
                //    _query = "Insert Into mastitem(Itemname,Active,SyncId,DisplayName,Itemtype";
                //    _query = _query + " ,CreatedDate,SegmentId,ClassId,PriceGroup,UnderId) values('" + distinctitemgroup[i].ITEMPARENT.Replace("'", "''").Replace("\n", "").Replace("\r", "") + "', 'true','" + distinctitemgroup[i].COMPANYNUMBER.Replace("'", "''") + "#" + distinctitemgroup[i].ParentMasterID.Replace("'", "''").Replace("\n", "").Replace("\r", "") + "','" + distinctitemgroup[i].ITEMPARENT.Replace("'", "''") + "','MATERIALGROUP'";
                //    _query = _query + ",'" + DateTime.Now.ToString("dd/MMM/yyyy hh:mm:ss") + "',1,1,'A',0);SELECT SCOPE_IDENTITY();";
                //    result1 = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, _query));
                //}
                //else
                //{
                //    _query = "Update mastitem set Itemname ='" + distinctitemgroup[i].ITEMPARENT.Replace("'", "''").Replace("\n", "").Replace("\r", "") + "'  where ItemId=" + DTitemgrp.Rows[0]["ItemId"].ToString() + " ";
                //    result1 = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, _query));
                //    result1 = Convert.ToInt32(DTitemgrp.Rows[0]["ItemId"].ToString());
                //}
                result1 = MG.InsertMatGrp(distinctitemgroup[i].ITEMPARENT.Replace("'", "''").Replace("\n", "").Replace("\r", ""), distinctitemgroup[i].COMPANYNUMBER.Replace("'", "''") + "#" + distinctitemgroup[i].ParentMasterID.Replace("'", "''").Replace("\n", "").Replace("\r", ""), "1", "");
                if (result1 == -1)
                {
                    //System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Duplicate MaterialGroup Exists');", true);
                    //MaterialGroup.Focus();

                }
                else if (result1 == -2)
                {
                    DTitemgrp = DbConnectionDAL.GetDataTable(CommandType.Text, "Select * from MastItem where SyncId='" + distinctitemgroup[i].COMPANYNUMBER.Replace("'", "''") + "#" + distinctitemgroup[i].ParentMasterID.Replace("'", "''") + "' and Itemtype='MATERIALGROUP'");

                    result1 = MG.UpdateMatGrp(Convert.ToInt32(DTitemgrp.Rows[0]["ItemId"].ToString()), distinctitemgroup[i].ITEMPARENT.Replace("'", "''").Replace("\n", "").Replace("\r", ""), distinctitemgroup[i].COMPANYNUMBER.Replace("'", "''") + "#" + distinctitemgroup[i].ParentMasterID.Replace("'", "''").Replace("\n", "").Replace("\r", ""), "1", "");
                }
                else
                {
                    //if (SyncId.Value == "")
                    //{
                    //    string syncid = "update MastItem set SyncId='" + retval + "' where Itemid=" + retval + "";
                    //    DbConnectionDAL.ExecuteNonQuery(CommandType.Text, syncid);
                    //}
                    //System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Inserted Successfully');", true);
                    //ClearControls();
                    //MaterialGroup.Focus();
                }


                if (result1 < 0)
                {
                    failure = failure + 1;
                    continue;
                }
                var getproducts = ds.Tables[0].AsEnumerable().Where(x => x.Field<string>("ITEMPARENT") == distinctitemgroup[i].ITEMPARENT).Select(s => new
                {
                    ITEM = s.Field<string>("ITEMNAME"),
                    UNIT = s.Field<string>("ITEMUNIT"),
                    MASTERID = s.Field<string>("MASTERID"),
                    COMPANYNUMBER = s.Field<string>("COMPANYNUMBER"),
                    OPENINGRATE = s.Field<string>("OPENINGRATE").ToString().IndexOf("/") > 0 ? s.Field<string>("OPENINGRATE").ToString().Substring(0, s.Field<string>("OPENINGRATE").ToString().IndexOf("/")) : "0",
                    IGST = s.Field<string>("IGST") == "" ? "0" : s.Field<string>("IGST"),
                    CGST = s.Field<string>("CGST") == "" ? "0" : s.Field<string>("CGST"),
                    SGST = s.Field<string>("SGST") == "" ? "0" : s.Field<string>("SGST"),

                }).ToList();
                for (int j = 0; j < getproducts.Count; j++)
                {
                    //DTitem = DbConnectionDAL.GetDataTable(CommandType.Text, "Select * from MastItem where Syncid='" + getproducts[j].COMPANYNUMBER + "#" + getproducts[j].MASTERID + "' and Itemtype='ITEM'");
                    //if (DTitem.Rows.Count == 0)
                    //{
                    result = IB.InsertItems(result1.ToString(), getproducts[j].ITEM.Replace("'", "''").Replace("\n", "").Replace("\r", ""), "", getproducts[j].UNIT.Replace("'", "''"), "1", "0", getproducts[j].COMPANYNUMBER + "#" + getproducts[j].MASTERID, Convert.ToDecimal(getproducts[j].OPENINGRATE), 0, 0, "1", "1", "A", "", "", 0, 0, 0, "1", Convert.ToDecimal(getproducts[j].CGST), Convert.ToDecimal(getproducts[j].SGST), Convert.ToDecimal(getproducts[j].IGST));
                    //_sql = "Insert Into mastitem(Itemname,SyncId,DisplayName,Itemtype";
                    //_sql = _sql + " ,CreatedDate,SegmentId,ClassId,PriceGroup,Underid,";
                    //_sql = _sql + "Lvl,StdPack,MRP,DP,RP,GST,Unit,Active,CentralTaxPer,StateTaxPer,IntegratedTaxPer) values('" + getproducts[j].ITEM.Replace("'", "''").Replace("\n", "").Replace("\r", "") + "','" + getproducts[j].COMPANYNUMBER + "#" + getproducts[j].MASTERID + "','" + getproducts[j].ITEM.Replace("'", "''").Replace("\n", "").Replace("\r", "") + "','ITEM'";
                    //_sql = _sql + ",'" + DateTime.Now.ToString("dd/MMM/yyyy hh:mm:ss") + "',1,1,'A'," + result1 + "";
                    //_sql = _sql + ",1,0," + Convert.ToDecimal(getproducts[j].OPENINGRATE) + ",0,0,0";
                    //_sql = _sql + ",'" + getproducts[j].UNIT.Replace("'", "''") + "','true'," + getproducts[j].CGST + "," + getproducts[j].SGST + "," + getproducts[j].IGST + ");SELECT SCOPE_IDENTITY();";


                    // result = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, _sql));
                    if (result > 0)
                    {
                        // success = success + 1;
                    }
                    else if (result == -2)
                    {
                        DTitem = DbConnectionDAL.GetDataTable(CommandType.Text, "Select * from MastItem where Syncid='" + getproducts[j].COMPANYNUMBER + "#" + getproducts[j].MASTERID + "' and Itemtype='ITEM'");
                        result = IB.UpdateItems(Convert.ToInt32(DTitem.Rows[0]["ItemId"].ToString()), result1.ToString(), getproducts[j].ITEM.Replace("'", "''").Replace("\n", "").Replace("\r", ""), "", getproducts[j].UNIT.Replace("'", "''"), "1", "0", getproducts[j].COMPANYNUMBER + "#" + getproducts[j].MASTERID, Convert.ToDecimal(getproducts[j].OPENINGRATE.Replace(",", " ")), 0, 0, "1", "1", "A", "", "", 0, 0, 0, "1", Convert.ToDecimal(getproducts[j].CGST), Convert.ToDecimal(getproducts[j].SGST), Convert.ToDecimal(getproducts[j].IGST));
                    }
                    if (result < 0)
                    {
                        failure = failure + 1;
                        continue;

                    }
                    else
                    {
                        success = success + 1;

                    }

                    //}
                    //else
                    //{
                    //    _sql = "update mastitem set Itemname= '" + getproducts[j].ITEM.Replace("'", "''").Replace("\n", "").Replace("\r", "") + "',unit='" + getproducts[j].UNIT.Replace("'", "''") + "',Active= 'true',StdPack=0,MRP=" + getproducts[j].OPENINGRATE.Replace(",", " ") + ",DP=0,RP=0,DisplayName='" + getproducts[j].ITEM.Replace("\n", "").Replace("\r", "") + "',Itemtype='ITEM',CreatedDate=Getdate(),SegmentId=1,ClassId=1,PriceGroup='A',underid=" + result1 + ",CentralTaxPer=" + getproducts[j].CGST + ",StateTaxPer=" + getproducts[j].SGST + ",IntegratedTaxPer=" + getproducts[j].IGST + "  where ItemId=" + DTitem.Rows[0]["ItemId"].ToString() + "";
                    //    result = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, _sql));
                    //    result = Convert.ToInt32(DTitem.Rows[0]["ItemId"].ToString());
                    //    if (result > 0)
                    //    {
                    //        success = success + 1;
                    //    }
                    //    else
                    //    {
                    //        failure = failure + 1;
                    //    }
                    //}
                }


            }

            str = success + " record inserted successfully ";

            if (failure > 0)
            {
                str = str + "," + failure + " record are failed";
            }

            rs.ResultMsg = str;

            return rs;


        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public errorresult insertdistributorfromtally()
        {

            int cnt = 0, exist = 0;
            string _query = "", status = "";
            bool active;
            List<Result1> rst = new List<Result1>();
            string paramInfo = "", _Message = "";
            int success = 0;
            int failure = 0;
            int err = 0;
            string str = "";
            DataTable DTparty = new DataTable();
            var bodyStream = new StreamReader(HttpContext.Current.Request.InputStream);
            DataTable DTadmin = new DataTable();
            bodyStream.BaseStream.Seek(0, SeekOrigin.Begin);
            var bodyText = bodyStream.ReadToEnd();
            StringReader theReader = new StringReader(bodyText);
            DataSet ds = new DataSet();
            ds.ReadXml(theReader);
            errorresult rs = new errorresult();
            int Distinid = 0;
            int cityid = 0;
            int regionid = 0;
            int citytypeid = 0;
            int cityconveyancetype = 0;
            int distictid = 0;
            int countryid = 0;
            int stateid = 0;
            int Areaid = 0;
            int roleid = 0;
            int opvoucherno = 1;
            int result = 0;
            cityid = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, "SELECT AreaId from MastArea where AreaType='CITY' AND AreaName='Blank'"));
            List<mandatorymsg> msglist = new List<mandatorymsg>();
            regionid = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, "SELECT AreaId from MastArea where AreaType='REGION' AND AreaName='Blank'"));
            distictid = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, "Select AreaId from MastArea where Areatype='DISTRICT' And AreaName='Blank'"));
            citytypeid = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, "Select Id From MastCityType Where Name='Other'"));
            cityconveyancetype = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, "Select Id From MastCityType Where Name='OTHERS'"));
            roleid = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, "Select RoleId From MastRole Where RoleName='DISTRIBUTOR'"));
            //   Areaid = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, "SELECT AreaId from MastArea where AreaType='AREA' AND AreaName='Blank'"));
            //  File.WriteAllText("C:/log/ErrorLog.txt", String.Empty);
            //using (TransactionScope transactionScope = new TransactionScope())
            //{
            string docID;
            try
            {
                DTadmin = DbConnectionDAL.GetDataTable(CommandType.Text, "Select * from MastSalesRep Where SMName='DIRECTOR'");

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    mandatorymsg ms = new mandatorymsg();

                    string Date;
                    Areaid = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, "SELECT AreaId from MastArea where AreaType='AREA' AND AreaName='Area-" + ds.Tables[0].Rows[i]["PARTYNAME"].ToString() + "'"));
                    if (ds.Tables[0].Rows[i]["CREDITLIMIT"].ToString() == "")
                        ds.Tables[0].Rows[i]["CREDITLIMIT"] = "0";

                    cnt = DB.InsertDistributors_Tally(ds.Tables[0].Rows[i]["PARTYNAME"].ToString(), ds.Tables[0].Rows[i]["ADDRESS"].ToString(), ds.Tables[0].Rows[i]["ADDRESS2"].ToString(), Convert.ToString(cityid), ds.Tables[0].Rows[i]["PINCODE"].ToString(), ds.Tables[0].Rows[i]["EMAIL"].ToString(), ds.Tables[0].Rows[i]["LEDGERMOBILE"].ToString(), "", ds.Tables[0].Rows[i]["PARTYID"].ToString(), "", ds.Tables[0].Rows[i]["PARTYNAME"].ToString(), true, ds.Tables[0].Rows[i]["LEDGERPHONE"].ToString(), Convert.ToInt32(roleid), ds.Tables[0].Rows[i]["LEDGRCONTACT"].ToString(), "", "", "", ds.Tables[0].Rows[i]["ITN"].ToString() == "" ? "" : ds.Tables[0].Rows[i]["ITN"].ToString(), 0, ds.Tables[0].Rows[i]["CREDITLIMIT"].ToString() == "" ? 0 : Convert.ToDecimal(ds.Tables[0].Rows[i]["CREDITLIMIT"].ToString()), 0, 0, Convert.ToInt32(DTadmin.Rows[0]["Userid"]), "", ds.Tables[0].Rows[i]["FAX"].ToString(), "", Convert.ToInt32(DTadmin.Rows[0]["SMid"].ToString()), "", "", Areaid,
                       ds.Tables[0].Rows[i]["COUNTRYNAME"].ToString(), ds.Tables[0].Rows[i]["STATENAME"].ToString(), countryid, regionid, stateid, distictid, citytypeid, cityconveyancetype, ds.Tables[0].Rows[i]["GSTNO"].ToString(), ds.Tables[0].Rows[i]["COMPANYNUMBER"].ToString());
                    if (cnt > 0)
                    {
                        if (Convert.ToDecimal(ds.Tables[0].Rows[i]["OPENINGBALANCEDR"].ToString()) != 0 || Convert.ToDecimal(ds.Tables[0].Rows[i]["OPENINGBALANCECR"].ToString()) != 0)
                        {
                            decimal Amount = 0;
                            decimal Entryno = 0;
                            Date = "01/Apr/" + Convert.ToDateTime(ds.Tables[0].Rows[i]["DATE"].ToString()).Year.ToString();
                            docID = "TLOP" + ds.Tables[0].Rows[i]["COMPANYNUMBER"].ToString().Substring(ds.Tables[0].Rows[i]["COMPANYNUMBER"].ToString().Length - 3) + " " + Convert.ToString(Convert.ToDateTime(ds.Tables[0].Rows[i]["DATE"].ToString()).Year) + " " + Convert.ToString(opvoucherno);
                            if (Convert.ToDecimal(ds.Tables[0].Rows[i]["OPENINGBALANCEDR"].ToString()) != 0)
                            {
                                Amount = Convert.ToDecimal(ds.Tables[0].Rows[i]["OPENINGBALANCEDR"].ToString());
                            }
                            else
                            {
                                Amount = -Convert.ToDecimal(ds.Tables[0].Rows[i]["OPENINGBALANCECR"].ToString());
                            }
                            string str1 = "Select Count(*) from TransDistributerLedger where VDate='" + Date + "' and Narration=' Opening Balance' and DistId=" + cnt + "";
                            int count = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, str1));
                            if (count == 0)
                            {

                                Entryno = Convert.ToDecimal(DbConnectionDAL.GetScalarValue(CommandType.Text, "select Isnull(Max(DistLedId),0)+1 from  TransDistributerLedger"));
                                result = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, "Insert into TransDistributerLedger(DLDocId,Vdate,DistID,Amount,AmtCr,AmtDr,Narration,COMPANYCODE,EntryNo) values('" + docID + "','" + Date + "', " + cnt + "," + Amount + "," + Convert.ToDecimal(ds.Tables[0].Rows[i]["OPENINGBALANCECR"].ToString()) + "," + Convert.ToDecimal(ds.Tables[0].Rows[i]["OPENINGBALANCEDR"].ToString()) + ",' Opening Balance','" + ds.Tables[0].Rows[i]["COMPANYNUMBER"].ToString() + "','" + Entryno +"')"));
                            }

                        }
                        status = InsertLicense(ds.Tables[0].Rows[i]["PARTYNAME"].ToString(), ds.Tables[0].Rows[i]["LEDGERMOBILE"].ToString());
                        if (status == "Yes") _Message = "";
                        else if (status == "No") _Message = " But somthing went wrong in License Creation process";
                        ms.msdmsg = ds.Tables[0].Rows[i]["PARTYNAME"].ToString() + " is inserted successfully";
                        msglist.Add(ms);
                        success = success + 1;

                    }
                    else if (cnt == -1)
                    {
                        ms.msdmsg = ds.Tables[0].Rows[i]["PARTYNAME"].ToString() + " is not inserted,Already exist in login Mast";
                        msglist.Add(ms);
                        failure = failure + 1;
                        // LogError(ds.Tables[0].Rows[i]["PARTYNAME"].ToString() + " is not inserted,Already exist in login Mast");
                    }
                    else if (cnt == -3)
                    {
                        ms.msdmsg = ds.Tables[0].Rows[i]["PARTYNAME"].ToString() + " is not inserted,Duplicate Mobile No.";
                        msglist.Add(ms);
                        failure = failure + 1;
                        // LogError(ds.Tables[0].Rows[i]["PARTYNAME"].ToString() + " is not inserted,Already exist in login Mast");
                    }
                    else if (cnt == -2)
                    {
                        int retval = DB.UpdateDistributors_Tally(ds.Tables[0].Rows[i]["PARTYNAME"].ToString(), ds.Tables[0].Rows[i]["ADDRESS"].ToString(), ds.Tables[0].Rows[i]["ADDRESS2"].ToString(), Convert.ToString(cityid), ds.Tables[0].Rows[i]["PINCODE"].ToString(), ds.Tables[0].Rows[i]["EMAIL"].ToString(), ds.Tables[0].Rows[i]["LEDGERMOBILE"].ToString(), "", ds.Tables[0].Rows[i]["PARTYID"].ToString(), "", "", true, ds.Tables[0].Rows[i]["LEDGERPHONE"].ToString(), Convert.ToInt32(roleid), ds.Tables[0].Rows[i]["LEDGRCONTACT"].ToString(), "", "", "", ds.Tables[0].Rows[i]["ITN"].ToString() == "" ? "" : ds.Tables[0].Rows[i]["ITN"].ToString(), 0, ds.Tables[0].Rows[i]["CREDITLIMIT"].ToString() == "" ? 0 : Convert.ToDecimal(ds.Tables[0].Rows[i]["CREDITLIMIT"].ToString()), 0, 0, Convert.ToInt32(DTadmin.Rows[0]["Userid"]), "", ds.Tables[0].Rows[i]["FAX"].ToString(), "", Convert.ToInt32(DTadmin.Rows[0]["SMid"].ToString()), "", "", Areaid,
                          ds.Tables[0].Rows[i]["COUNTRYNAME"].ToString(), ds.Tables[0].Rows[i]["STATENAME"].ToString(), countryid, regionid, stateid, distictid, citytypeid, cityconveyancetype, ds.Tables[0].Rows[i]["GSTNO"].ToString(), ds.Tables[0].Rows[i]["COMPANYNUMBER"].ToString());
                        //_query = "UPDATE MastParty SET PartyName = '" + ds.Tables[0].Rows[i]["PARTYNAME"].ToString() + "',Address1 = '" + ds.Tables[0].Rows[i]["ADDRESS"].ToString() + "',	Address2 ='" + ds.Tables[0].Rows[i]["ADDRESS2"].ToString() + "',Pin = '" +  ds.Tables[0].Rows[i]["PINCODE"].ToString() + "'',	AreaId = " + Areaid + ",";
                        //_query = _query + " Email = '" + ds.Tables[0].Rows[i]["EMAIL"].ToString() + "',Mobile = '" + ds.Tables[0].Rows[i]["LEDGERMOBILE"].ToString() + "',	Active = 'true',PartyDist = 'true',UserId = " + Convert.ToInt32(DTadmin.Rows[0]["Userid"]) + ",	Lvl = 1,Created_User_id = " + Convert.ToInt32(DTadmin.Rows[0]["Userid"]) + ",";
                        //_query = _query + " DisplayName = '" + ds.Tables[0].Rows[i]["PARTYNAME"].ToString() + "',ContactPerson = '" + ds.Tables[0].Rows[i]["LEDGRCONTACT"].ToString() + "',PANNo = '" + ds.Tables[0].Rows[i]["ITN"].ToString() + "',	CityId = " + cityid + ",CreditLimit = " +  ds.Tables[0].Rows[i]["CREDITLIMIT"].ToString() + ",";
                        //_query = _query + " SMID = " + Convert.ToInt32(DTadmin.Rows[0]["SMID"]) + ",Fax = '" + ds.Tables[0].Rows[i]["FAX"].ToString() + "',	GSTINNo ='" +  ds.Tables[0].Rows[i]["GSTNO"].ToString() + "'' where PartyId=" + DTparty.Rows[0]["PartyId"].ToString() + "";
                        //cnt = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, _query));

                        if (retval > 0)
                        {
                            if (Convert.ToDecimal(ds.Tables[0].Rows[i]["OPENINGBALANCEDR"].ToString()) != 0 || Convert.ToDecimal(ds.Tables[0].Rows[i]["OPENINGBALANCECR"].ToString()) != 0)
                            {

                                decimal Amount = 0;
                                decimal Entryno = 0;
                                Date = "01/Apr/" + Convert.ToDateTime(ds.Tables[0].Rows[i]["DATE"].ToString()).Year.ToString();
                                docID = "TLOP" + ds.Tables[0].Rows[i]["COMPANYNUMBER"].ToString().Substring(ds.Tables[0].Rows[i]["COMPANYNUMBER"].ToString().Length - 3) + " " + Convert.ToString(Convert.ToDateTime(ds.Tables[0].Rows[i]["DATE"].ToString()).Year) + " " + Convert.ToString(opvoucherno);
                                if (Convert.ToDecimal(ds.Tables[0].Rows[i]["OPENINGBALANCEDR"].ToString()) != 0)
                                {
                                    Amount = Convert.ToDecimal(ds.Tables[0].Rows[i]["OPENINGBALANCEDR"].ToString());
                                }
                                else
                                {
                                    Amount = -Convert.ToDecimal(ds.Tables[0].Rows[i]["OPENINGBALANCECR"].ToString());
                                }
                                string str1 = "Select DLDocId from TransDistributerLedger where VDate='" + Date + "' and Narration=' Opening Balance' and DistId=" + retval + "";
                                string DD = Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, str1));
                                if (DD == "")
                                {
                                    Entryno = Convert.ToDecimal(DbConnectionDAL.GetScalarValue(CommandType.Text, "select Isnull(Max(DistLedId),0)+1 from  TransDistributerLedger"));
                                    result = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, "Insert into TransDistributerLedger(DLDocId,Vdate,DistID,Amount,AmtCr,AmtDr,Narration,COMPANYCODE,entryno) values('" + docID + "','" + Date + "', " + retval + "," + Amount + "," + Convert.ToDecimal(ds.Tables[0].Rows[i]["OPENINGBALANCECR"].ToString()) + "," + Convert.ToDecimal(ds.Tables[0].Rows[i]["OPENINGBALANCEDR"].ToString()) + ",' Opening Balance','" + ds.Tables[0].Rows[i]["COMPANYNUMBER"].ToString() + "'," + Entryno + ")"));
                                }
                                else
                                {
                                    DbConnectionDAL.ExecuteNonQuery(CommandType.Text, "Update TransDistributerLedger set Amount=" + Amount + ",AmtCr=" + Convert.ToDecimal(ds.Tables[0].Rows[i]["OPENINGBALANCECR"].ToString()) + ",AmtDr=" + Convert.ToDecimal(ds.Tables[0].Rows[i]["OPENINGBALANCEDR"].ToString()) + "  where VDate='" + Date + "' and Narration=' Opening Balance' and DistId=" + retval + "");

                                }
                                //       result = DB.insertdistledger_Tally(retval, docID,Date,
                                //Convert.ToDecimal(ds.Tables[0].Rows[i]["OPENINGBALANCEDR"].ToString()) != 0 ? Convert.ToDecimal(ds.Tables[0].Rows[i]["OPENINGBALANCEDR"].ToString()) : -Convert.ToDecimal(ds.Tables[0].Rows[i]["OPENINGBALANCECR"].ToString()), " Opening Balance",
                                //      ds.Tables[0].Rows[i]["COMPANYNUMBER"].ToString());

                            }
                            status = UpdateLicense(ds.Tables[0].Rows[i]["PARTYID"].ToString(), ds.Tables[0].Rows[i]["LEDGERMOBILE"].ToString(), ds.Tables[0].Rows[i]["PARTYNAME"].ToString(), ds.Tables[0].Rows[i]["COMPANYNUMBER"].ToString());
                            if (status == "Yes") _Message = "";
                            else if (status == "No") _Message = " But somthing went wrong in License Creation process";
                            ms.msdmsg = ds.Tables[0].Rows[i]["PARTYNAME"].ToString() + " is updated successfully" + _Message + "";

                            msglist.Add(ms);
                            success = success + 1;
                            opvoucherno++;
                        }
                        else
                        {
                            ms.msdmsg = ds.Tables[0].Rows[i]["PARTYNAME"].ToString() + " is not updated,Something went wrong";
                            msglist.Add(ms);
                            failure = failure + 1;
                            // LogError(ds.Tables[0].Rows[i]["PARTYNAME"].ToString() + " is not inserted,Something went wrong");
                        }
                        //ms.msdmsg = ds.Tables[0].Rows[i]["PARTYNAME"].ToString() + " is not inserted,Sync id is duplicate";
                        //msglist.Add(ms);
                        //failure = failure + 1;
                        //LogError(ds.Tables[0].Rows[i]["PARTYNAME"].ToString() + "  is not inserted,Sync id is duplicate");
                    }
                    else
                    {
                        ms.msdmsg = ds.Tables[0].Rows[i]["PARTYNAME"].ToString() + " is not inserted,Something went wrong";
                        msglist.Add(ms);
                        failure = failure + 1;
                        // LogError(ds.Tables[0].Rows[i]["PARTYNAME"].ToString() + " is not inserted,Something went wrong");
                    }


                }

                //transactionScope.Complete();
                //transactionScope.Dispose();
                str = success + " record inserted successfully";
                if (failure > 0)
                    str = str + "," + failure + " record are failed";
                if (err > 0)
                    str = str + ",Check log ";
                if (msglist.Count == 0)
                {
                    mandatorymsg ms = new mandatorymsg();
                    ms.msdmsg = "No error";
                    msglist.Add(ms);
                }

                rs.msg = str;
                rs.errormsg = msglist;
                this.SucessEmailStatus();
            }
            catch (TransactionException ex)
            {
                //transactionScope.Dispose();
                cnt = 0;
                str = ex.Message;
                err = err + 1;
                LogError(str);
                // msglist..Add(str);
                if (err > 0)
                    str = str + ",Check log ";
                rs.msg = str;
                rs.errormsg = msglist;

            }
            //}
            return rs;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void SucessEmailStatus()
        {
            string subject = "", retstatus = "", sql = "", _subject = "", _sql = "", Subject = ""; ;
            string str = @"SELECT T1.SenderEmailId,T1.SenderPassword,T1.Port,T1.MailServer,T1.[Log_Email_CC],[Log_Email_To]
                            FROM MastEnviro AS T1";
            DataTable dtEmail = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            string AttachmentFile = "Tally\\Tally.ERP9\\tdlfunc.log";
            Subject = "Regarding to Transfer Distributor from Tally.";
            if (dtEmail != null && dtEmail.Rows.Count > 0)
            {
                try
                {
                    MailMessage mail = new MailMessage();

                    // mail.From = new MailAddress(Convert.ToString(dtEmail.Rows[0]["SenderEmailId"]));
                    mail.From = new MailAddress(Convert.ToString("noreply@grahaak.com"));
                    mail.Subject = Subject;
                    string strMailBody1 = "Dear Sir/Mam , <br /><br /> Please find the attachment.<br />Regards<br /><br />------- This is a system generated email please do not reply------- ";

                    mail.Body = strMailBody1;
                    mail.To.Add(new MailAddress(Convert.ToString(dtEmail.Rows[0]["Log_Email_To"])));
                    //mail.To.Add(new MailAddress(Convert.ToString("vsvikramsinghrajput@gmail.com")));
                    // mail.To.Add(new mailaddress(convert.tostring("abhishek.jaiswal@dataman.in")));
                    string[] CCId = dtEmail.Rows[0]["Log_Email_CC"].ToString().Split(',');
                    if (CCId.Length > 0)
                    {
                        foreach (string CCEmail in CCId)
                        {

                            if (!string.IsNullOrEmpty(CCEmail))
                            {
                                mail.CC.Add(Convert.ToString(CCEmail));
                            }
                        }
                    }
                    bool exists = System.IO.Directory.Exists(Server.MapPath("temp"));

                    if (!exists)
                        System.IO.Directory.CreateDirectory(Server.MapPath("temp"));
                    //mail.cc.add(convert.tostring("abhishek.jaiswal@dataman.in"));
                    if (File.Exists(@"C:\\temp\\tdlfunc.log"))
                    {
                        File.Delete(@"C:\\temp\\tdlfunc.log");
                        File.Copy(@"C:\\Program Files\\Tally\\Tally.ERP9\\tdlfunc.log", @"C:\\temp\\tdlfunc.log");
                    }



                    if (!String.IsNullOrEmpty(AttachmentFile))
                    {
                        string root = @"C:\\temp\\tdlfunc.log";
                        // If directory does not exist, don't even try  
                        if (File.Exists(root))
                        {
                            GC.Collect();
                            GC.WaitForPendingFinalizers();
                            // string pathh1 = Server.MapPath(Convert.ToString("~/" + AttachmentFile));
                            mail.Attachments.Add(new System.Net.Mail.Attachment(root));
                        }
                    }



                    //NetworkCredential mailAuthenticaion = new NetworkCredential(Convert.ToString(dtEmail.Rows[0]["SenderEmailId"]), Convert.ToString(dtEmail.Rows[0]["SenderPassword"]));
                    //SmtpClient mailclient = new SmtpClient(Convert.ToString(dtEmail.Rows[0]["MailServer"]), Convert.ToInt32(dtEmail.Rows[0]["Port"]));

                    NetworkCredential mailAuthenticaion = new NetworkCredential(Convert.ToString("noreply@grahaak.com"), Convert.ToString("dataman@knp12345"));

                    SmtpClient mailclient = new SmtpClient(Convert.ToString("mail.dataman.in"), Convert.ToInt32("587"));

                    mailclient.EnableSsl = true;
                    mailclient.UseDefaultCredentials = false;
                    mailclient.Credentials = mailAuthenticaion;
                    mail.IsBodyHtml = true;
                    mailclient.Send(mail);


                    //  strSubject = strSubject + "new" + Convert.ToString(cnt1);
                    _subject = "Mail Successfully Send for:" + Subject + " at " + DateTime.Now + "";
                    sql = "Insert into [EmailDataStatus]([Subject],[ErrorMessage],[EmailId],[EmailStatus],[UserId],[VDate]) Values('" + Subject + "','','" + Convert.ToString(dtEmail.Rows[0]["Log_Email_To"]) + "','Success',1,getdate())";
                    DbConnectionDAL.ExecuteNonQuery(CommandType.Text, _sql);

                    if (File.Exists(@"C:\\temp\\tdlfunc.log"))
                    {
                        File.Delete(@"C:\\temp\\tdlfunc.log");
                    }


                    // Context.Response.Write(JsonConvert.SerializeObject(retstatus));
                }
                catch (Exception ex)
                {
                    _subject = "Mail Failure for:" + Subject + " at " + DateTime.Now + "";
                    sql = "Insert into  [EmailDataStatus]([Subject],[ErrorMessage],[EmailId],[EmailStatus],[UserId],[VDate]) Values('" + Subject + "','','" + Convert.ToString(dtEmail.Rows[0]["Log_Email_To"]) + "','Failure',1,getdate())";
                    DbConnectionDAL.ExecuteNonQuery(CommandType.Text, sql);
                }
            }
        }


        public string InsertLicense(string PersonName, string mob)
        {
            string result = "No";
            try
            {
                string mastenviro = "select * from mastenviro";
                DataTable dtenviro = DbConnectionDAL.GetDataTable(CommandType.Text, mastenviro);
                if (dtenviro.Rows.Count > 0)
                {
                    if (dtenviro.Rows[0]["CompCode"].ToString() == "GHN")
                    {
                        if (!string.IsNullOrEmpty(mob))
                        {
                            string insqry = "SELECT count(*) FROM LineMaster WHERE Mobile='" + mob + "' and upper(Product)='GOLDIEE' and compcode='" + dtenviro.Rows[0]["CompCode"].ToString() + "'";
                            int count = DbConnectionDAL.GetDemoLicenseIntScalarVal(insqry);
                            if (count < 1)
                            {
                                string insLine = "insert into LineMaster (CompCode,DeviceID,Active,LineDate,PersonName,Product,URL,mobile)" +
                                                        "values('" + dtenviro.Rows[0]["CompCode"].ToString() + "','" + mob + "','Y','" + DateTime.Now.ToUniversalTime().AddSeconds(19800).ToString("yyyy-MM-dd") + "','" + PersonName + "','GOLDIEE','" + dtenviro.Rows[0]["compurl"].ToString() + "','" + mob + "')";

                                DbConnectionDAL.ExecuteNonQueryforlicence(constrDmLicense, CommandType.Text, insLine);
                                result = "Yes";
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { ex.ToString(); }
            return result;
        }

        public string UpdateLicense(string SyncId, string mob, string distributorname, string CompCode)
        {
            string result = "No", insqry = "", oldMob = "", _syncId = "";

            try
            {
                _syncId = CompCode + '#' + SyncId;
                insqry = "Select Mobile from MastParty where SyncId='" + _syncId + "' And CompCode='" + CompCode + "'";
                oldMob = DbConnectionDAL.GetStringScalarVal(insqry);
                string mastenviro = "select * from mastenviro";
                DataTable dtenviro = DbConnectionDAL.GetDataTable(CommandType.Text, mastenviro);
                if (dtenviro.Rows[0]["CompCode"].ToString() == "GHN")
                {
                    if (!string.IsNullOrEmpty(oldMob))
                    {
                        insqry = "SELECT count(*) FROM LineMaster WHERE Mobile='" + oldMob + "' and upper(Product)='GOLDIEE' and compcode='" + dtenviro.Rows[0]["CompCode"].ToString() + "'";
                        int count = DbConnectionDAL.GetDemoLicenseIntScalarVal(insqry);
                        if (count >= 1)
                        {
                            insqry = " update linemaster set mobile='" + mob + "' where Mobile='" + oldMob + "' and upper(Product)='GOLDIEE' and compcode='" + dtenviro.Rows[0]["CompCode"].ToString() + "'";
                            DbConnectionDAL.ExecuteNonQueryforlicence(constrDmLicense, CommandType.Text, insqry);
                            result = "Yes";
                        }
                        else
                        {
                            string insLine = "insert into LineMaster (CompCode,Active,LineDate,PersonName,Product,URL,mobile)" +
                                               "values('" + dtenviro.Rows[0]["CompCode"].ToString() + "','Y','" + DateTime.Now.ToUniversalTime().AddSeconds(19800).ToString("yyyy-MM-dd") + "','" + distributorname + "','GOLDIEE','" + dtenviro.Rows[0]["compurl"].ToString() + "','" + mob + "')";

                            DbConnectionDAL.ExecuteNonQueryforlicence(constrDmLicense, CommandType.Text, insLine);
                            result = "Yes";
                        }
                    }
                }
            }

            catch (Exception ex) { ex.ToString(); }
            return result;
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public Purchorderlist postpurchordertotally()
        {
            DataTable DTpurchorder = new DataTable();
            DataTable DTpurchorder1 = new DataTable();
            List<Transpurchorder> purchorder = new List<Transpurchorder>();
            Purchorderlist POL = new Purchorderlist();
            decimal _amt = 0;
            string _query;


            int pid = Convert.ToInt32(DbConnectionDAL.ExecuteScaler("Select LastSynID from TallyCounter where Type='P'"));
            _query = @"Select POrdId,PODocid,VDate,SyncId as distid,Remarks,CreatedDate,SUBSTRING([PartyName],1, CHARINDEX([Compcode],[PartyName])-3) As PartyName,CompCode from [TransPurchOrder] TP
            Left join MastParty MP on MP.PartyId=TP.DistId where Isnull(SyncId,'')<>''  "; //and TP.POrdId>" + pid + "
            DTpurchorder = DbConnectionDAL.GetDataTable(CommandType.Text, _query);
            for (int i = 0; i < DTpurchorder.Rows.Count; i++)
            {
                Transpurchorder TP = new Transpurchorder();
                _query = @"Select PODocid,VDate, MI.ItemName as ItemName,MI.SyncId As item,(Case when Isnull(Rate,0)=0 then Isnull(MI.DP,0) else Isnull(Rate,0) end)  As Rate ,Qty
,SNo,MI.unit As Unit from TransPurchOrder1 TP1
                   Left join  MastItem MI on MI.ItemId=TP1.ItemId  where Isnull(SyncId,'')<>'' and PODocid='" + DTpurchorder.Rows[i]["PODocid"].ToString() + "' ";
                DTpurchorder1 = DbConnectionDAL.GetDataTable(CommandType.Text, _query);

                TP.Docid = DTpurchorder.Rows[i]["PODocid"].ToString();
                TP.VDate = Convert.ToDateTime(DTpurchorder.Rows[i]["VDate"].ToString()).ToString("dd-MM-yyyy");
                TP.Remark = DTpurchorder.Rows[i]["Remarks"].ToString();
                TP.Distid = DTpurchorder.Rows[i]["distid"].ToString();
                TP.Partyname = DTpurchorder.Rows[i]["PartyName"].ToString();
                TP.createddate = Convert.ToDateTime(DTpurchorder.Rows[i]["CreatedDate"].ToString());
                TP.Pordid = Convert.ToInt32(DTpurchorder.Rows[i]["POrdId"].ToString());
                TP.Vno = "Sales/" + DTpurchorder.Rows[i]["POrdId"].ToString();
                TP.referenceno = "Sales/" + DTpurchorder.Rows[i]["POrdId"].ToString() + "/" + DTpurchorder.Rows[i]["distid"].ToString();
                List<Transpurchorder1> purchorder1 = new List<Transpurchorder1>();
                for (int j = 0; j < DTpurchorder1.Rows.Count; j++)
                {
                    Transpurchorder1 TP1 = new Transpurchorder1();
                    TP1.Docid = DTpurchorder1.Rows[j]["PODocid"].ToString();
                    TP1.VDate = Convert.ToDateTime(DTpurchorder1.Rows[j]["VDate"].ToString());
                    TP1.itemid = DTpurchorder1.Rows[j]["ItemName"].ToString().Trim();
                    TP1.Quantity = Convert.ToDecimal(DTpurchorder1.Rows[j]["Qty"].ToString());
                    TP1.Rate = Convert.ToDecimal(DTpurchorder1.Rows[j]["Rate"].ToString());
                    TP1.sno = Convert.ToInt32(DTpurchorder1.Rows[j]["SNo"].ToString());
                    TP1.Amount = (Convert.ToDecimal(DTpurchorder1.Rows[j]["Qty"].ToString()) * Convert.ToDecimal(DTpurchorder1.Rows[j]["Rate"].ToString()));
                    _amt = _amt + TP1.Amount;
                    TP1.Unit = DTpurchorder1.Rows[j]["Unit"].ToString();
                    purchorder1.Add(TP1);

                }
                TP.totalamount = _amt;
                TP.purchorder1 = purchorder1;
                purchorder.Add(TP);

                POL.result = purchorder;
                _amt = 0;
            }

            return POL;

        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public Purchorderlist postpurchordertotallyv2()
        {
            DataTable DTpurchorder = new DataTable();
            DataTable DTpurchorder1 = new DataTable();
            List<Transpurchorder> purchorder = new List<Transpurchorder>();
            Purchorderlist POL = new Purchorderlist();
            decimal _amt = 0;
            string _query;


            int pid = Convert.ToInt32(DbConnectionDAL.ExecuteScaler("Select LastSynID from TallyCounter where Type='P'"));
            _query = @"Select POrdId,PODocid,VDate,SyncId as distid,Remarks,CreatedDate,PartyName from [TransPurchOrder] TP
            Left join MastParty MP on MP.PartyId=TP.DistId where Isnull(SyncId,'')<>''  and TP.POrdId>" + pid + "";
            DTpurchorder = DbConnectionDAL.GetDataTable(CommandType.Text, _query);
            for (int i = 0; i < DTpurchorder.Rows.Count; i++)
            {
                Transpurchorder TP = new Transpurchorder();
                _query = @"Select PODocid,VDate, MI.ItemId as itemid,MI.SyncId As item,(Case when Isnull(Rate,0)=0 then Isnull(MI.DP,0) else Isnull(Rate,0) end)  As Rate ,Qty,SNo,MI.unit As Unit from TransPurchOrder1 TP1
                   Left join  MastItem MI on MI.ItemId=TP1.ItemId  where Isnull(SyncId,'')<>'' and PODocid='" + DTpurchorder.Rows[i]["PODocid"].ToString() + "' ";
                DTpurchorder1 = DbConnectionDAL.GetDataTable(CommandType.Text, _query);

                TP.Docid = DTpurchorder.Rows[i]["PODocid"].ToString();
                TP.VDate = Convert.ToDateTime(DTpurchorder.Rows[i]["VDate"].ToString()).ToString("dd-MM-yyyy");
                TP.Remark = DTpurchorder.Rows[i]["Remarks"].ToString();
                TP.Distid = DTpurchorder.Rows[i]["distid"].ToString();
                TP.Partyname = DTpurchorder.Rows[i]["PartyName"].ToString();
                TP.createddate = Convert.ToDateTime(DTpurchorder.Rows[i]["CreatedDate"].ToString());
                TP.Pordid = Convert.ToInt32(DTpurchorder.Rows[i]["POrdId"].ToString());
                TP.Vno = "Sales/" + DTpurchorder.Rows[i]["POrdId"].ToString();
                TP.referenceno = "Sales/" + DTpurchorder.Rows[i]["POrdId"].ToString() + "/" + DTpurchorder.Rows[i]["distid"].ToString();
                List<Transpurchorder1> purchorder1 = new List<Transpurchorder1>();
                for (int j = 0; j < DTpurchorder1.Rows.Count; j++)
                {
                    Transpurchorder1 TP1 = new Transpurchorder1();
                    TP1.Docid = DTpurchorder1.Rows[j]["PODocid"].ToString();
                    TP1.VDate = Convert.ToDateTime(DTpurchorder1.Rows[j]["VDate"].ToString());
                    TP1.itemid = DTpurchorder1.Rows[j]["itemid"].ToString();
                    TP1.Quantity = Convert.ToDecimal(DTpurchorder1.Rows[j]["Qty"].ToString());
                    TP1.Rate = Convert.ToDecimal(DTpurchorder1.Rows[j]["Rate"].ToString());
                    TP1.sno = Convert.ToInt32(DTpurchorder1.Rows[j]["SNo"].ToString());
                    TP1.Amount = (Convert.ToDecimal(DTpurchorder1.Rows[j]["Qty"].ToString()) * Convert.ToDecimal(DTpurchorder1.Rows[j]["Rate"].ToString()));
                    _amt = _amt + TP1.Amount;
                    TP1.Unit = DTpurchorder1.Rows[j]["Unit"].ToString();
                    purchorder1.Add(TP1);

                }
                TP.totalamount = _amt;
                TP.purchorder1 = purchorder1;
                purchorder.Add(TP);

                POL.result = purchorder;
                _amt = 0;
            }

            return POL;

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public errorresult insertsalesinvoicefromtally()
        {
            string _query = "", _fList = "", _sql = "";
            int cnt = 0, result = 0;
            int DistInvId = 0;
            int DistInv1Id = 0;
            string Insert = "";
            string Duplicate = "";
            int sno = 0;
            string DistInvDocId = "";
            decimal taxamt = 0;
            decimal roundoff = 0;
            string roff = "";
            //List<TransDistInv> TransDistInvList = JsonConvert.DeserializeObject<List<TransDistInv>>(value);
            int success = 0;
            int failure = 0;
            string str = "";
            //this.IsValidJson(value);
            CultureInfo provider = new CultureInfo("en-US");
            var myInsertList = new List<string>();
            var myDuplicateList = new List<string>();
            var bodyStream = new StreamReader(HttpContext.Current.Request.InputStream);
            DataTable DTadmin = new DataTable();
            int DistInv2Id = 0;
            bodyStream.BaseStream.Seek(0, SeekOrigin.Begin);
            var bodyText = bodyStream.ReadToEnd();
            StringReader theReader = new StringReader(bodyText);
            DataSet ds = new DataSet();
            List<mandatorymsg> msglist = new List<mandatorymsg>();
            ds.ReadXml(theReader);
            errorresult rs = new errorresult();
            if (ds.Tables.Count > 1)
            {
                if (ds.Tables["ITEMDETAIL"].Rows.Count > 0)
                {
                    string sqlConnectionstring = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                    SqlConnection cn = new SqlConnection(sqlConnectionstring);
                    cn.Open();
                    SqlBulkCopy objbulk = new SqlBulkCopy(cn);
                    //assigning Destination table name  
                    objbulk.DestinationTableName = "TempTally_Saleinvoiceitemdetail";

                    objbulk.WriteToServer(ds.Tables["ITEMDETAIL"]);
                    cn.Close();
                }
                if (ds.Tables.Count > 3 && ds.Tables["LEDGERDETAIL"].Rows.Count > 0)
                {
                    string sqlConnectionstring = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                    SqlConnection cn = new SqlConnection(sqlConnectionstring);
                    cn.Open();
                    SqlBulkCopy objbulk = new SqlBulkCopy(cn);
                    //assigning Destination table name  
                    objbulk.DestinationTableName = "TempTally_SaleInvoiceTaxDetails";
                    //Mapping Table column  
                    objbulk.ColumnMappings.Add("LEDGERNAME", "LEDGERNAME");
                    objbulk.ColumnMappings.Add("LEDGERAMOUNT", "LEDGERAMOUNT");
                    objbulk.ColumnMappings.Add("BILLNO", "BILLNO");
                    objbulk.ColumnMappings.Add("MASTER", "MASTER");
                    objbulk.ColumnMappings.Add("PARENTTAX", "PARENTTAX");
                    //inserting bulk Records into DataBase   
                    objbulk.WriteToServer(ds.Tables["LEDGERDETAIL"]);
                    cn.Close();
                }
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, "Delete from TransDistInv where Compcode='" + ds.Tables["ITEMDETAIL"].Rows[0]["COMPANYNUMBER"].ToString() + "'");
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, "Delete from TransDistInv1 where Compcode='" + ds.Tables["ITEMDETAIL"].Rows[0]["COMPANYNUMBER"].ToString() + "'");
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, "Delete from TransDistInv2 where Compcode='" + ds.Tables["ITEMDETAIL"].Rows[0]["COMPANYNUMBER"].ToString() + "'");
                var distinctbill = ds.Tables["ITEMDETAIL"].AsEnumerable().GroupBy(s => s.Field<string>("BILLNO")).Select(s => new
                {
                    BillNo = s.Key,
                    VDATE = s.Max(x => x.Field<string>("DATE")),
                    BillAmount = s.Max(x => x.Field<string>("TOTALAMOUNT")),
                    Masterid = s.Max(x => x.Field<string>("MASTER")),
                    Partyid = s.Max(x => x.Field<string>("PARTYMASTERID")),
                    Partyname = s.Max(x => x.Field<string>("PARTY")),
                    Companynumber = s.Max(x => x.Field<string>("COMPANYNUMBER"))

                }).ToList();
                for (int i = 0; i < distinctbill.Count; i++)
                {

                    mandatorymsg ms = new mandatorymsg();
                    DistInvDocId = "TLINV" + " " + Convert.ToString(Convert.ToDateTime(distinctbill[i].VDATE).Year) + " " + distinctbill[i].BillNo;
                    if (ds.Tables.Count > 3 && ds.Tables["LEDGERDETAIL"].AsEnumerable().Where(s => s.Field<string>("BILLNO") == distinctbill[i].BillNo) != null)
                    {

                        if (ds.Tables["LEDGERDETAIL"].AsEnumerable().Where(s => s.Field<string>("BILLNO") == distinctbill[i].BillNo && s.Field<string>("LEDGERNAME") != "R/o" && s.Field<string>("PARENTTAX") != "Sales Accounts").ToList() != null)
                        {



                            taxamt = ds.Tables["LEDGERDETAIL"].AsEnumerable().Where(s => s.Field<string>("BILLNO") == distinctbill[i].BillNo && s.Field<string>("LEDGERNAME") != "R/o" && s.Field<string>("PARENTTAX") != "Sales Accounts").Sum(f => Convert.ToDecimal(f.Field<string>("LEDGERAMOUNT")));
                        }


                        roff = ds.Tables["LEDGERDETAIL"].AsEnumerable().Where(s => s.Field<string>("BILLNO") == distinctbill[i].BillNo && s.Field<string>("LEDGERNAME") == "R/o").Select(f => f.Field<string>("LEDGERAMOUNT")).FirstOrDefault();
                        if (roff != null)
                            roundoff = roff.ToString().IndexOf(")") > 0 ? -Convert.ToDecimal(roff.ToString().Substring(roff.ToString().IndexOf(")") + 1, roff.ToString().Length - 1 - roff.ToString().IndexOf(")"))) : Convert.ToDecimal(roff.ToString());

                    }
                    DistInvId = DB.insertsaleinvheader_Tally(DistInvDocId, distinctbill[i].VDATE, taxamt, (Convert.ToDecimal(distinctbill[i].BillAmount.IndexOf(")") > 0 ? distinctbill[i].BillAmount.Substring(distinctbill[i].BillAmount.IndexOf(")") + 1, distinctbill[i].BillAmount.Length - 1 - distinctbill[i].BillAmount.ToString().IndexOf(")")) : distinctbill[i].BillAmount)), roundoff, distinctbill[i].Partyid, distinctbill[i].Companynumber);
                    if (DistInvId == -2)
                    {
                        ms.msdmsg = "Party -" + distinctbill[i].Partyname + " not exist on web portal for Bill no " + distinctbill[i].BillNo;
                        msglist.Add(ms);
                        failure = failure + 1;

                    }
                    else
                    {
                        taxamt = 0;
                        roundoff = 0;
                        if (ds.Tables["ITEMDETAIL"].AsEnumerable().Where(x => x.Field<string>("BILLNO") == distinctbill[i].BillNo) != null)
                        {


                            var getproducts = ds.Tables["ITEMDETAIL"].AsEnumerable().Where(x => x.Field<string>("BILLNO") == distinctbill[i].BillNo).Select(s => new
                            {
                                ITEM = s.Field<string>("ITEM"),
                                QTY = s.Field<string>("QTY").ToString() != "" ? s.Field<string>("QTY").ToString().Substring(0, s.Field<string>("QTY").ToString().IndexOf(" ")) : "0",
                                BILLNO = s.Field<string>("BILLNO").ToString(),
                                AMOUNT = s.Field<string>("AMOUNT").ToString() == "" ? 0 : s.Field<string>("AMOUNT").ToString().IndexOf(")") > 0 ? -Convert.ToDecimal(s.Field<string>("AMOUNT").ToString().Substring(s.Field<string>("AMOUNT").ToString().IndexOf(")") + 1, s.Field<string>("AMOUNT").ToString().Length - 1 - s.Field<string>("AMOUNT").ToString().IndexOf(")"))) : Convert.ToDecimal(s.Field<string>("AMOUNT").ToString()),
                                DATE = s.Field<string>("DATE").ToString(),
                                PARTY = s.Field<string>("PARTY").ToString(),
                                MASTER = s.Field<string>("MASTER").ToString(),
                                RATE = s.Field<string>("RATE").ToString() != "" ? s.Field<string>("RATE").ToString().Substring(0, s.Field<string>("RATE").ToString().IndexOf("/")) : "0",
                                PARTYID = s.Field<string>("PARTYMASTERID").ToString(),
                                ITEMMASTERID = s.Field<string>("ITEMMASTERID").ToString()

                            }).ToList();
                            for (int j = 0; j < getproducts.Count; j++)
                            {
                                if (getproducts[j].ITEM != "")
                                {
                                    sno = sno + 1;
                                    DistInv1Id = DB.insertsaleinvdetail_Tally(DistInvId, sno, DistInvDocId, getproducts[j].DATE, 0, Convert.ToDecimal(getproducts[j].QTY.IndexOf(")") > 0 ? getproducts[j].QTY.Substring(getproducts[j].QTY.IndexOf(")") + 1, getproducts[j].QTY.Length - 1 - getproducts[j].QTY.ToString().IndexOf(")")) : getproducts[j].QTY), Convert.ToDecimal(getproducts[j].RATE), Convert.ToDecimal(getproducts[j].AMOUNT), distinctbill[i].Partyid, getproducts[j].ITEM, getproducts[j].ITEMMASTERID, distinctbill[i].Companynumber);
                                    if (DistInv1Id > 0)
                                    {

                                        success = success + 1;
                                    }
                                }
                            }
                        }
                        //// all charges including tax amount  and rounding off 

                        if (ds.Tables.Count > 3 && ds.Tables["LEDGERDETAIL"].AsEnumerable().Where(s => s.Field<string>("BILLNO") == distinctbill[i].BillNo) != null)
                        {
                            var othertaxesofbill = ds.Tables["LEDGERDETAIL"].AsEnumerable().Where(s => s.Field<string>("BILLNO") == distinctbill[i].BillNo && s.Field<string>("PARENTTAX") != "Sales Accounts").Select(z => new
                            {

                                Description = z.Field<string>("TAXTYPE") == "Central Tax" ? "CGST" : z.Field<string>("TAXTYPE") == "State Tax" ? "SGST" : z.Field<string>("TAXTYPE") == "Integrated Tax" ? "IGST" : z.Field<string>("LEDGERNAME"),
                                Amount = z.Field<string>("LEDGERAMOUNT").IndexOf(")") > 0 ? -Convert.ToDecimal(z.Field<string>("LEDGERAMOUNT").ToString().Substring(z.Field<string>("LEDGERAMOUNT").ToString().IndexOf(")") + 1, z.Field<string>("LEDGERAMOUNT").ToString().Length - 1 - z.Field<string>("LEDGERAMOUNT").ToString().IndexOf(")"))) : Convert.ToDecimal(z.Field<string>("LEDGERAMOUNT").ToString())

                            }).ToList();


                            for (int j = 0; j < othertaxesofbill.Count; j++)
                            {
                                DistInv2Id = DB.insertsaleinvexpensedetail_Tally(DistInvDocId, othertaxesofbill[j].Description, Convert.ToDecimal(othertaxesofbill[j].Amount));
                            }

                        }

                        ////


                    }
                    sno = 0;
                    _sql = "Update TallyCounter set LastSynID=" + distinctbill[i].Masterid + ",LastDate='" + DateTime.Now.ToString("dd/MMM/yyyy") + "' where Type='S' and compcode='" + distinctbill[i].Companynumber + "'";
                    DistInv1Id = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, _sql));
                }
                str = success + " record inserted successfully ";


                if (msglist.Count == 0)
                {
                    mandatorymsg ms = new mandatorymsg();
                    ms.msdmsg = "No error";
                    msglist.Add(ms);
                }


                if (failure > 0)
                {
                    str = str + "," + failure + " record are failed.Check log ";
                }
            }

            if (msglist.Count == 0)
            {
                mandatorymsg ms = new mandatorymsg();
                ms.msdmsg = "No error";
                msglist.Add(ms);
            }
            rs.msg = str;
            rs.errormsg = msglist;

            return rs;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public errorresult insertExpensesfromtally()
        {
            int success = 0;
            int cnt = 0;
            int failure = 0;
            int err = 0;
            string str = "";
            string _query = "";
            errorresult rs = new errorresult();
            List<mandatorymsg> msglist = new List<mandatorymsg>();
            var bodyStream = new StreamReader(HttpContext.Current.Request.InputStream);
            DataTable DTadmin = new DataTable();
            DataTable DTExpense = new DataTable();
            bodyStream.BaseStream.Seek(0, SeekOrigin.Begin);
            var bodyText = bodyStream.ReadToEnd();
            StringReader theReader = new StringReader(bodyText);
            DataSet ds = new DataSet();
            ds.ReadXml(theReader);

            try
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    mandatorymsg ms = new mandatorymsg();

                    DTExpense = DbConnectionDAL.GetDataTable(CommandType.Text, @"SELECT Id, Name, SyncId, Expensetype FROM InvExpenseMast 
where SyncId='" + ds.Tables[0].Rows[i]["COMPANYNUMBER"].ToString() + "#" + ds.Tables[0].Rows[i]["PARTYID"].ToString() + "' and Expensetype='" + ds.Tables[0].Rows[i]["PARENTNAME"].ToString().Replace("'", "''") + "'");

                    if (DTExpense.Rows.Count == 0)
                    {
                        _query = "INSERT INTO dbo.InvExpenseMast (Name, SyncId, Expensetype) VALUES ( ";
                        _query = _query + " '" + ds.Tables[0].Rows[i]["PARTYNAME"].ToString().Replace("'", "''") + "','" + ds.Tables[0].Rows[i]["COMPANYNUMBER"].ToString() + "#" + ds.Tables[0].Rows[i]["PARTYID"].ToString() + "','" + ds.Tables[0].Rows[i]["PARENTNAME"].ToString().Replace("'", "''") + "');SELECT SCOPE_IDENTITY();";

                        cnt = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, _query));
                        if (cnt > 0)
                        {
                            ms.msdmsg = ds.Tables[0].Rows[i]["PARTYNAME"].ToString() + " is inserted successfully";
                            msglist.Add(ms);
                            success = success + 1;

                        }
                        else
                        {
                            failure = failure + 1;
                        }

                    }
                    else
                    {
                        _query = "UPDATE dbo.InvExpenseMast SET 	Name ='" + ds.Tables[0].Rows[i]["PARTYNAME"].ToString().Replace("'", "''") + "' ,Expensetype = '" + ds.Tables[0].Rows[i]["PARENTNAME"].ToString().Replace("'", "''") + "' where SyncId='" + ds.Tables[0].Rows[i]["COMPANYNUMBER"].ToString() + "#" + ds.Tables[0].Rows[i]["PARTYID"].ToString() + "' ";
                        DbConnectionDAL.ExecuteNonQuery(CommandType.Text, _query);
                        cnt = Convert.ToInt32(DTExpense.Rows[0]["Id"]);
                        if (cnt > 0)
                        {
                            ms.msdmsg = ds.Tables[0].Rows[i]["PARTYNAME"].ToString() + " is updated successfully";

                            msglist.Add(ms);
                            success = success + 1;
                        }
                        else
                        {
                            failure = failure + 1;
                        }
                    }


                }
                str = success + " record inserted successfully";
                if (failure > 0)
                    str = str + "," + failure + " record are failed";
                if (err > 0)
                    str = str + ",Check log ";
                if (msglist.Count == 0)
                {
                    mandatorymsg ms = new mandatorymsg();
                    ms.msdmsg = "No error";
                    msglist.Add(ms);
                }

                rs.msg = str;
                rs.errormsg = msglist;

            }
            catch (Exception ex)
            {

                cnt = 0;
                str = ex.Message;
                err = err + 1;
                LogError(str);
                // msglist..Add(str);
                if (err > 0)
                    str = str + ",Check log ";
                rs.msg = str;
                rs.errormsg = msglist;
            }

            return rs;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public Saleinvoicelist PostSalesinvoice()
        {
            Saleinvoicelist SOI = new Saleinvoicelist();
            List<TransDistinv> SaleInv = new List<TransDistinv>();
            DataTable DtsalesInv = new DataTable();
            string _query;
            decimal TotalCGSTAmt = 0, TotalSGSTAmt = 0, TotalIGSTAmt = 0;
            DataTable DtsalesInv1 = new DataTable();
            DataTable DtsalesInv2 = new DataTable();
            _query = @"Select DistInvId,DistInvDocId,VDate,SyncId as distid,Isnull(Roundoff,0) As Roundoff,taxamt1,BillAmount,SUBSTRING([PartyName],1, CHARINDEX(MP.Compcode,[PartyName])-3) As PartyName
 from [TransDistInv] TP
            Left join MastParty MP on MP.PartyId=TP.DistId where Isnull(SyncId,'')<>'' and ISnull(TallyImport,'N')='N'  "; //and TP.POrdId>" + pid + "
            DtsalesInv = DbConnectionDAL.GetDataTable(CommandType.Text, _query);
            for (int i = 0; i < DtsalesInv.Rows.Count; i++)
            {
                TransDistinv SI = new TransDistinv();
                SI.Distid = DtsalesInv.Rows[i]["distid"].ToString();
                SI.DistInvId = Convert.ToInt32(DtsalesInv.Rows[i]["DistInvId"].ToString());
                SI.Distname = DtsalesInv.Rows[i]["PartyName"].ToString();
                SI.VDate = Convert.ToDateTime(DtsalesInv.Rows[i]["VDate"].ToString()).ToString("dd-MM-yyyy");
                SI.totalamount = Convert.ToDecimal(DtsalesInv.Rows[i]["BillAmount"].ToString());
                SI.taxamount = Convert.ToDecimal(DtsalesInv.Rows[i]["taxamt1"].ToString());
                SI.Docid = DtsalesInv.Rows[i]["DistInvDocId"].ToString();
                SI.totalRoundingoff = Convert.ToDecimal(DtsalesInv.Rows[i]["Roundoff"].ToString());
                //SI.
                SI.referenceno = "SalesInv/" + DtsalesInv.Rows[i]["DistInvId"].ToString() + "/" + DtsalesInv.Rows[i]["distid"].ToString();
                _query = @"Select DistInv1Id, DistInvId, DistInvDocId,VDate, MI.ItemName as ItemName,Isnull(MI.IntegratedTaxPer,0) As IntegratedTaxPer ,Isnull(MI.CentralTaxPer,0) As CentralTaxPer ,Isnull(MI.StateTaxPer,0) As StateTaxPer,
(Case when Isnull(Rate,0)=0 then Isnull(MI.DP,0) else Isnull(Rate,0) end)  As Rate ,Isnull(Qty,0) As Qty, Isnull(Amount,0) As Amount, Isnull(TP1.Discount,0) As Discount, Isnull(Disc_Amt,0) As Disc_Amt,
Isnull(Exise,0) As Exise, Isnull(Exi_Amt,0) As Exi_Amt ,  Isnull(Tax,0) As Tax, Isnull(TP1.Tax_Amt,0) As Tax_Amt
,SNo,MI.unit As Unit, Isnull(TP1.CGST_Amt,0) As CGST_Amt ,Isnull(TP1.SGST_Amt,0) As SGST_Amt ,Isnull(TP1.IGST_Amt,0) As IGST_Amt,Isnull((Select POrdId from Transpurchorder TP2 where TP2.PODocId=TP1.OrderNo ),'')  As OrderId from TransDistInv1 TP1
                   Left join  MastItem MI on MI.ItemId=TP1.ItemId  where Isnull(SyncId,'')<>'' and DistInvDocId='" + DtsalesInv.Rows[i]["DistInvDocId"].ToString() + "' ";
                DtsalesInv1 = DbConnectionDAL.GetDataTable(CommandType.Text, _query);
                List<TransDistinv1> TDI1 = new List<TransDistinv1>();
                for (int j = 0; j < DtsalesInv1.Rows.Count; j++)
                {
                    TransDistinv1 SI1 = new TransDistinv1();
                    SI1.Docid = DtsalesInv1.Rows[j]["DistInvDocId"].ToString();
                    SI1.itemid = DtsalesInv1.Rows[j]["ItemName"].ToString();
                    SI1.Quantity = Convert.ToDecimal(DtsalesInv1.Rows[j]["Qty"].ToString());
                    SI1.Rate = Convert.ToDecimal(DtsalesInv1.Rows[j]["Rate"].ToString());
                    SI1.Tax_Amt = Convert.ToDecimal(DtsalesInv1.Rows[j]["Tax_Amt"].ToString());
                    SI1.Disc_Amt = Convert.ToDecimal(DtsalesInv1.Rows[j]["Disc_Amt"].ToString());
                    SI1.Amount = (Convert.ToDecimal(DtsalesInv1.Rows[j]["Qty"].ToString()) * Convert.ToDecimal(DtsalesInv1.Rows[j]["Rate"].ToString())) - Convert.ToDecimal(DtsalesInv1.Rows[j]["Disc_Amt"].ToString());
                    if ((Convert.ToDecimal(DtsalesInv1.Rows[j]["Qty"].ToString()) * Convert.ToDecimal(DtsalesInv1.Rows[j]["Rate"].ToString())) > 0)
                    {
                        SI1.Discount = (Convert.ToDecimal(DtsalesInv1.Rows[j]["Disc_Amt"].ToString()) * 100) / (Convert.ToDecimal(DtsalesInv1.Rows[j]["Qty"].ToString()) * Convert.ToDecimal(DtsalesInv1.Rows[j]["Rate"].ToString()));
                    }
                    else
                    {
                        SI1.Discount = 0;
                    }

                    SI1.VDate = Convert.ToDateTime(DtsalesInv.Rows[i]["VDate"].ToString());
                    SI1.CentralTaxPer = Convert.ToDecimal(DtsalesInv1.Rows[j]["CentralTaxPer"].ToString());
                    SI1.IntegratedTaxPer = Convert.ToDecimal(DtsalesInv1.Rows[j]["IntegratedTaxPer"].ToString());
                    SI1.StateTaxPer = Convert.ToDecimal(DtsalesInv1.Rows[j]["StateTaxPer"].ToString());
                    //SI1.CGSTAmt = Convert.ToDecimal(DtsalesInv1.Rows[j]["CGST_Amt"].ToString());
                    //TotalCGSTAmt = TotalCGSTAmt + Convert.ToDecimal(DtsalesInv1.Rows[j]["CGST_Amt"].ToString());
                    //SI1.SGSTAmt = Convert.ToDecimal(DtsalesInv1.Rows[j]["SGST_Amt"].ToString());
                    //TotalSGSTAmt = TotalSGSTAmt + Convert.ToDecimal(DtsalesInv1.Rows[j]["SGST_Amt"].ToString());
                    //SI1.IGSTAmt = Convert.ToDecimal(DtsalesInv1.Rows[j]["IGST_Amt"].ToString());
                    //TotalIGSTAmt = TotalIGSTAmt + Convert.ToDecimal(DtsalesInv1.Rows[j]["IGST_Amt"].ToString());
                    if (DtsalesInv1.Rows[j]["OrderId"].ToString() != "0")
                    {
                        SI1.orderno = "Sales/" + DtsalesInv1.Rows[j]["OrderId"].ToString();
                    }

                    TDI1.Add(SI1);
                }
                _query = @"Select Description,ISnull(Amount,0) As Amount from TransDistInv2 where
              DistInvDocId='" + DtsalesInv.Rows[i]["DistInvDocId"].ToString() + "' ";
                DtsalesInv2 = DbConnectionDAL.GetDataTable(CommandType.Text, _query);
                List<TransDistinv2> TDI2 = new List<TransDistinv2>();
                for (int j = 0; j < DtsalesInv2.Rows.Count; j++)
                {
                    TransDistinv2 SI2 = new TransDistinv2();
                    SI2.ExpName = DtsalesInv2.Rows[j]["Description"].ToString();
                    SI2.Amount = Convert.ToDecimal(DtsalesInv2.Rows[j]["Amount"].ToString());
                    TDI2.Add(SI2);
                }
                //if (DtsalesInv2.Rows.Count > 0)
                //{

                //    DataRow[] drcgst = DtsalesInv2.Select("Description='CGST'");
                //    if (drcgst.Count() > 0)
                //    {
                //        SI.TotalCGSTAmt = Convert.ToDecimal(drcgst[0]["Amount"].ToString());
                //    }
                //    drcgst = DtsalesInv2.Select("Description='IGST'");
                //    if (drcgst.Count() > 0)
                //    {
                //        SI.TotalIGSTAmt = Convert.ToDecimal(drcgst[0]["Amount"].ToString());
                //    }

                //    drcgst = DtsalesInv2.Select("Description='SGST'");
                //    if (drcgst.Count() > 0)
                //    {
                //        SI.TotalSGSTAmt = Convert.ToDecimal(drcgst[0]["Amount"].ToString());
                //    }
                //    drcgst = DtsalesInv2.Select("Description='FREIGHT'");
                //    if (drcgst.Count() > 0)
                //    {
                //        SI.TotalFright = Convert.ToDecimal(drcgst[0]["Amount"].ToString());
                //    }
                //    //SI.TotalCGSTAmt = DtsalesInv2.AsEnumerable().Where(x => x.Field<string>("Description") == "CGST") ==null?0: Convert.ToDecimal(DtsalesInv2.AsEnumerable().Where(x => x.Field<string>("Description") == "CGST").Max(f => f.Field<string>("Amount")));
                //    //SI.TotalIGSTAmt = DtsalesInv2.AsEnumerable().Where(x => x.Field<string>("Description") == "IGST") == null ? 0 : Convert.ToDecimal(DtsalesInv2.AsEnumerable().Where(x => x.Field<string>("Description") == "IGST").Max(f => f.Field<string>("Amount")));
                //    //SI.TotalSGSTAmt = DtsalesInv2.AsEnumerable().Where(x => x.Field<string>("Description") == "SGST") == null ? 0 : Convert.ToDecimal(DtsalesInv2.AsEnumerable().Where(x => x.Field<string>("Description") == "SGST").Max(f => f.Field<string>("Amount")));
                //    //SI.TotalFright = DtsalesInv2.AsEnumerable().Where(x => x.Field<string>("Description") == "FREIGHT") == null ? 0 : Convert.ToDecimal(DtsalesInv2.AsEnumerable().Where(x => x.Field<string>("Description") == "FREIGHT").Max(f => f.Field<string>("Amount")));
                //}


                SI.Distinv1 = TDI1;
                SI.Distinv2 = TDI2;
                SaleInv.Add(SI);
            }
            SOI.result = SaleInv;

            return SOI;
        }


        public class Saleinvoicelist
        {
            public List<TransDistinv> result { get; set; }
        }

        public class TransDistinv
        {
            public string Docid { get; set; }
            public int DistInvId { get; set; }
            public string Distid { get; set; }
            public string Distname { get; set; }
            public string VDate { get; set; }

            //public string Remark { get; set; }

            //public DateTime createddate { get; set; }
            public decimal totalamount { get; set; }
            public decimal taxamount { get; set; }
            //public string Vno { get; set; }
            public string referenceno { get; set; }

            public decimal TotalCGSTAmt { get; set; }
            public decimal TotalSGSTAmt { get; set; }
            public decimal TotalIGSTAmt { get; set; }
            public decimal TotalFright { get; set; }
            public decimal totalRoundingoff { get; set; }
            //  public decimal TotalAmount { get; set; }
            public List<TransDistinv1> Distinv1 { get; set; }
            public List<TransDistinv2> Distinv2 { get; set; }
        }

        public class TransDistinv1
        {
            public string Docid { get; set; }

            public int sno { get; set; }

            public string itemid { get; set; }
            public string orderno { get; set; }
            public decimal Quantity { get; set; }
            public decimal Amount { get; set; }
            public decimal Discount { get; set; }
            public decimal CentralTaxPer { get; set; }
            public decimal StateTaxPer { get; set; }
            public decimal IntegratedTaxPer { get; set; }

            public decimal CGSTAmt { get; set; }
            public decimal SGSTAmt { get; set; }
            public decimal IGSTAmt { get; set; }

            //   public string Unit { get; set; }
            public decimal Rate { get; set; }
            public decimal Disc_Amt { get; set; }
            public decimal Tax_Amt { get; set; }
            public DateTime VDate { get; set; }



        }

        public class TransDistinv2
        {
            public string ExpName { get; set; }

            public decimal Amount { get; set; }
        }

        private void LogError(string msg)
        {
            //string message = string.Format("Time: {0}", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
            //message += Environment.NewLine;
            //message += "-----------------------------------------------------------";
            //message += Environment.NewLine;
            //message += string.Format("Message: {0}", ex.Message);
            //message += Environment.NewLine;
            //message += string.Format("StackTrace: {0}", ex.StackTrace);
            //message += Environment.NewLine;
            //message += string.Format("Source: {0}", ex.Source);
            //message += Environment.NewLine;
            //message += string.Format("TargetSite: {0}", ex.TargetSite.ToString());
            //message += Environment.NewLine;
            //message += "-----------------------------------------------------------";
            //message += Environment.NewLine;
            //string path = Server.MapPath("~/ErrorLog/ErrorLog.txt");
            using (StreamWriter writer = new StreamWriter("C:/log/ErrorLog.txt", true))
            {
                writer.WriteLine(msg);
                writer.Close();
            }
        }

        //[WebMethod]
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //public Result insertPriceListfromtally()
        //{
        //    int cnt = 0, exist = 0;
        //    int result1 = 0;
        //    int result = 0;
        //    string _query = "";
        //    string _sql = "";
        //    bool active;
        //    List<Result1> rst = new List<Result1>();
        //    string paramInfo = "";
        //    string DistId = "", GroupId = "";
        //    string ItemId = "";
        //    int success = 0;
        //    int failure = 0;
        //    string str = "", strGrp = "";

        //    DataTable DTitem = new DataTable();
        //    var bodyStream = new StreamReader(HttpContext.Current.Request.InputStream);
        //    DataTable DTadmin = new DataTable();
        //    DataTable DTitemgrp = new DataTable();
        //    bodyStream.BaseStream.Seek(0, SeekOrigin.Begin);
        //    var bodyText = bodyStream.ReadToEnd();
        //    StringReader theReader = new StringReader(bodyText);
        //    DataSet ds = new DataSet();
        //    ds.ReadXml(theReader);
        //    int hj = ds.Tables["ITEMDETAIL"].Rows.Count;
        //    DataTable dt = ds.Tables[0];
        //    Result rs = new Result();
        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        try
        //        {
        //            string[] splitStr = Convert.ToString(dt.Rows[i]["Rate"]).Replace("'", "''").Split('/');
        //            string Rate = splitStr[0];
        //            decimal ffrate = (Convert.ToDecimal(Rate) * Convert.ToDecimal(Convert.ToString(dt.Rows[i]["Discount"]).Replace("'", "''"))) / 100;
        //            decimal fRate = Convert.ToDecimal(Rate) - ffrate;
        //            decimal fg = Math.Round(fRate, 2);
        //            ItemId = DbConnectionDAL.GetStringScalarVal("Select ItemId from MastItem where SyncId='" + Convert.ToString(dt.Rows[i]["COMPANYNUMBER"]).Replace("'", "''") + "#" + Convert.ToString(dt.Rows[i]["MasterID"]).Replace("'", "''") + "'");

        //            DistId = DbConnectionDAL.GetStringScalarVal("Select PartyId from MastParty where DisplayName='" + Convert.ToString(dt.Rows[i]["Party"]).Replace("'", "''") + "'");

        //            GroupId = DbConnectionDAL.GetStringScalarVal("Select ItemId from MastItem where ItemName='" + Convert.ToString(dt.Rows[i]["GRPID"]).Replace("'", "''") + "'");
        //            //string qw = "Select PartyId from MastParty where DisplayName='" + Convert.ToString(dt.Rows[i]["Party"]).Replace("'", "''") + "'";
        //            //string gf = "Select ItemId from MastItem where ItemName='" + Convert.ToString(dt.Rows[i]["GRPID"]).Replace("'", "''") + "'";
        //            //DataTable dtDist = DbConnectionDAL.getFromDataTable("Select PartyId from MastParty where DisplayName='" + Convert.ToString(dt.Rows[i]["Party"]).Replace("'", "''") + "'");
        //            //DataTable dtGroup = DbConnectionDAL.getFromDataTable("Select ItemId from MastItem where ItemName='" + Convert.ToString(dt.Rows[i]["GRPID"]).Replace("'", "''") + "'");

        //            if (!string.IsNullOrEmpty(DistId))
        //            {
        //                _sql = "Select Count(*) from MastItemPriceDistWise Where DistId=" + DistId + " And ItemId=" + ItemId + " And ProdGrpId=" + GroupId + " And ApplicableDate='" + Convert.ToString(dt.Rows[i]["Date"]) + "'";
        //                string cnt1 = DbConnectionDAL.GetStringScalarVal(_sql);
        //                if (Convert.ToInt32(cnt1) == 0)
        //                {
        //                    _sql = "Insert into MastItemPriceDistWise([DistId],[ItemId],[DistPrice],[CreatedByUserId],[CreateDate],[ProdGrpId],[LastModifiedDate],[ApplicableDate]) values(" + DistId + "," + ItemId + "," + Math.Round(fRate, 2) + ",1,getdate()," + GroupId + ",getdate(),'" + Convert.ToString(dt.Rows[i]["Date"]) + "');SELECT SCOPE_IDENTITY();";
        //                }
        //                else
        //                {
        //                    _sql = "Update MastItemPriceDistWise set DistId=" + DistId + ",ItemId=" + ItemId + ",DistPrice=" + Math.Round(fRate, 2) + ",ApplicableDate='" + Convert.ToString(dt.Rows[i]["Date"]) + "'  Where DistId=" + DistId + " And ItemId=" + ItemId + " And ProdGrpId=" + GroupId + " And ApplicableDate='" + Convert.ToString(dt.Rows[i]["Date"]) + "'";
        //                }
        //                //DbConnectionDAL.ExecuteNonQuery(CommandType.Text, _sql);

        //                result = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, _sql));

        //                _sql = "Update MastItem set DP=" + Rate + " where ItemId=" + ItemId + "";
        //                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, _sql);

        //            }
        //            else
        //            {
        //                strGrp += "" + Convert.ToString(dt.Rows[i]["Party"]) + "" + ",";
        //            }
        //            if (result > 0)
        //            {
        //                success = success + 1;
        //            }
        //            else
        //            {
        //                failure = failure + 1;
        //            }

        //        }
        //        catch (Exception ex)
        //        {

        //        }
        //    }
        //    str = success + " record inserted successfully ";

        //    if (failure > 0)
        //    {
        //        str = str + "," + failure + " record are failed And " + strGrp + " Party Not Found";
        //    }

        //    rs.ResultMsg = str;
        //    return rs;
        //}

        #region 10/Nov/2020 jyoti mam


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public errorresult insertPriceListfromtally()
        {
            int cnt = 0, exist = 0;
            int result1 = 0;
            int result = 0;
            string _query = "";
            string _sql = "";
            bool active;
            List<Result1> rst = new List<Result1>();
            string paramInfo = "";
            string DistId = "", GroupId = "", ItemNFound = "", GroupNFound = "";
            string ItemId = "";
            int success = 0;
            int failure = 0;

            string str = "", strGrp = "";
            int insertedRecord = 0;
            int updatedRecord = 0;
            DataTable DTitem = new DataTable();
            var bodyStream = new StreamReader(HttpContext.Current.Request.InputStream);
            DataTable DTadmin = new DataTable();
            DataTable DTitemgrp = new DataTable();
            bodyStream.BaseStream.Seek(0, SeekOrigin.Begin);
            var bodyText = bodyStream.ReadToEnd();
            StringReader theReader = new StringReader(bodyText);
            DataSet ds = new DataSet();
            errorresult rs = new errorresult();
            List<mandatorymsg> msglist = new List<mandatorymsg>();

            ds.ReadXml(theReader);
            int hj = ds.Tables["ITEMDETAIL"].Rows.Count;
            string party = Convert.ToString(ds.Tables["ITEMDETAIL"].Rows[4]["Party"]);
            DataTable dt = ds.Tables[0];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                mandatorymsg ms = new mandatorymsg();

                try
                {
                    string[] splitStr = Convert.ToString(dt.Rows[i]["Rate"]).Replace("'", "''").Split('/');
                    string Rate = splitStr[0];
                    decimal ffrate = (Convert.ToDecimal(Rate) * Convert.ToDecimal(Convert.ToString(dt.Rows[i]["Discount"]).Replace("'", "''"))) / 100;
                    decimal fRate = Convert.ToDecimal(Rate) - ffrate;
                    decimal fg = Math.Round(fRate, 2);
                    ItemId = DbConnectionDAL.GetStringScalarVal("Select ItemId from MastItem where SyncId='" + Convert.ToString(dt.Rows[i]["COMPANYNUMBER"]).Replace("'", "''") + "#" + Convert.ToString(dt.Rows[i]["MasterID"]).Replace("'", "''") + "'");

                    //DistId = DbConnectionDAL.GetStringScalarVal("Select PartyId from MastParty where replace(DisplayName,' ','')='" + Convert.ToString(dt.Rows[i]["Party"]).Replace("'", "''").Replace(" ","") + "'");
                    //DistId = DbConnectionDAL.GetStringScalarVal("Select PartyId from MastParty where DisplayName='" + Convert.ToString(dt.Rows[i]["Party"]).Replace("'", "''") + "'");

                    DistId = DbConnectionDAL.GetStringScalarVal("Select PartyId from MastParty where SyncId='" + Convert.ToString(dt.Rows[i]["COMPANYNUMBER"]).Replace("'", "''") + "#" + Convert.ToString(dt.Rows[i]["PartyID"]).Replace("'", "''") + "'");

                    GroupId = DbConnectionDAL.GetStringScalarVal("Select ItemId from MastItem where ItemName='" + Convert.ToString(dt.Rows[i]["GRPID"]).Replace("'", "''") + "'");

                    //string qw = "Select PartyId from MastParty where DisplayName='" + Convert.ToString(dt.Rows[i]["Party"]).Replace("'", "''") + "'";
                    //string gf = "Select ItemId from MastItem where ItemName='" + Convert.ToString(dt.Rows[i]["GRPID"]).Replace("'", "''") + "'";
                    //DataTable dtDist = DbConnectionDAL.getFromDataTable("Select PartyId from MastParty where DisplayName='" + Convert.ToString(dt.Rows[i]["Party"]).Replace("'", "''") + "'");
                    //DataTable dtGroup = DbConnectionDAL.getFromDataTable("Select ItemId from MastItem where ItemName='" + Convert.ToString(dt.Rows[i]["GRPID"]).Replace("'", "''") + "'");
                    if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["PartyID"]).Replace("'", "''")))
                    {
                        if (!string.IsNullOrEmpty(DistId))
                        {
                            if (!string.IsNullOrEmpty(GroupId))
                            {
                                if (!string.IsNullOrEmpty(ItemId))
                                {
                                    _sql = "Select Count(*) from MastItemPriceDistWise Where DistId=" + DistId + " And ItemId=" + ItemId + " And ProdGrpId=" + GroupId + " And ApplicableDate='" + Convert.ToString(dt.Rows[i]["Date"]) + "'";
                                    string cnt1 = DbConnectionDAL.GetStringScalarVal(_sql);
                                    if (Convert.ToInt32(cnt1) == 0)
                                    {
                                        _sql = "Insert into MastItemPriceDistWise([DistId],[ItemId],[DistPrice],[CreatedByUserId],[CreateDate],[ProdGrpId],[LastModifiedDate],[ApplicableDate]) values(" + DistId + "," + ItemId + "," + Math.Round(fRate, 2) + ",1,getdate()," + GroupId + ",getdate(),'" + Convert.ToString(dt.Rows[i]["Date"]) + "');SELECT SCOPE_IDENTITY();";
                                        result = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, _sql));
                                        insertedRecord++;
                                        success = success + 1;
                                        ms.msdmsg = "Price List For- " + Convert.ToString(dt.Rows[i]["Party"]).Replace("'", "''") + " is inserted successfully";
                                        msglist.Add(ms);
                                    }
                                    else
                                    {
                                        _sql = "Update MastItemPriceDistWise set DistId=" + DistId + ",ItemId=" + ItemId + ",DistPrice=" + Math.Round(fRate, 2) + ",ApplicableDate='" + Convert.ToString(dt.Rows[i]["Date"]) + "'  Where DistId=" + DistId + " And ItemId=" + ItemId + " And ProdGrpId=" + GroupId + " And ApplicableDate='" + Convert.ToString(dt.Rows[i]["Date"]) + "'";
                                        DbConnectionDAL.ExecuteNonQuery(CommandType.Text, _sql);
                                        updatedRecord++;
                                        ms.msdmsg = "Price List For- " + Convert.ToString(dt.Rows[i]["Party"]).Replace("'", "''") + " is updated successfully";
                                        msglist.Add(ms);
                                    }
                                }
                                else ItemNFound += Convert.ToString(dt.Rows[i]["COMPANYNUMBER"]).Replace("'", "''") + "#" + Convert.ToString(dt.Rows[i]["MasterID"]).Replace("'", "''") + ",";
                                //DbConnectionDAL.ExecuteNonQuery(CommandType.Text, _sql);

                            }
                            else GroupNFound = Convert.ToString(dt.Rows[i]["GRPID"]).Replace("'", "''") + ",";

                        }
                        else
                        {
                            if (strGrp.Contains(Convert.ToString(dt.Rows[i]["Party"])))
                            {
                                // Do Something // 
                            }
                            else strGrp += "" + Convert.ToString(dt.Rows[i]["Party"]) + "" + ",";

                        }
                        //if (result > 0)
                        //{
                        //    success = success + 1;

                        //}
                        //else
                        //{
                        //    failure = failure + 1;
                        //}
                    }
                    else
                    {
                        if (strGrp.Contains(Convert.ToString(dt.Rows[i]["Party"])))
                        {
                            // Do Something // 
                        }
                        else strGrp += "" + Convert.ToString(dt.Rows[i]["Party"]) + "" + ",";

                    }
                }
                catch (Exception ex)
                {
                    failure = failure + 1;
                }
            }
            str = "Total Record " + dt.Rows.Count + " And " + insertedRecord + " record inserted successfully And " + " " + updatedRecord + " record updated successfully And " + " Given Party are Not found " + strGrp.TrimEnd(',') + " And Given Item group are not found " + GroupNFound.TrimEnd(',') + "And Given Item are not found " + ItemNFound.TrimEnd(',');

            if (failure > 0)
            {
                str = str + "," + failure + " record are failed And " + strGrp + " Party Not Found";
            }
            if (msglist.Count == 0)
            {
                mandatorymsg ms = new mandatorymsg();
                ms.msdmsg = "No error";
                msglist.Add(ms);
            }
            rs.msg = str;

            rs.errormsg = msglist;
            return rs;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void insertRetailerfromtally()
        {
            int cnt = 0, exist = 0;
            string _query = "", status = "";
            bool active;
            List<Result1> rst = new List<Result1>();
            string paramInfo = "", _Message = "";
            int success = 0;
            int failure = 0;
            int err = 0;
            string str = "";
            DataTable DTparty = new DataTable();
            var bodyStream = new StreamReader(HttpContext.Current.Request.InputStream);
            DataTable DTadmin = new DataTable();
            bodyStream.BaseStream.Seek(0, SeekOrigin.Begin);
            var bodyText = bodyStream.ReadToEnd();
            StringReader theReader = new StringReader(bodyText);
            DataSet ds = new DataSet();
            ds.ReadXml(theReader);
            errorresult rs = new errorresult();
            int Distinid = 0;
            int cityid = 0;
            int regionid = 0;
            int citytypeid = 0;
            int cityconveyancetype = 0;
            int distictid = 0;
            int countryid = 0;
            int stateid = 0;
            int Areaid = 0;
            int roleid = 0;
            int opvoucherno = 1;
            int result = 0;
            int beatid = 0;
            DataTable DtDistributor = new DataTable();
            cityid = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, "SELECT AreaId from MastArea where AreaType='CITY' AND AreaName='Blank'"));
            List<mandatorymsg> msglist = new List<mandatorymsg>();
            regionid = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, "SELECT AreaId from MastArea where AreaType='REGION' AND AreaName='Blank'"));
            distictid = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, "Select AreaId from MastArea where Areatype='DISTRICT' And AreaName='Blank'"));
            citytypeid = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, "Select Id From MastCityType Where Name='Other'"));
            cityconveyancetype = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, "Select Id From MastCityType Where Name='OTHERS'"));
            roleid = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, "Select RoleId From MastRole Where RoleName='DISTRIBUTOR'"));
            //   Areaid = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, "SELECT AreaId from MastArea where AreaType='AREA' AND AreaName='Blank'"));
            //  File.WriteAllText("C:/log/ErrorLog.txt", String.Empty);
            //using (TransactionScope transactionScope = new TransactionScope())
            //{
            if (ds.Tables.Count > 0)
            {
                str = "Total Retailer is " + ds.Tables[0].Rows.Count + ".<br/>";
            }
            string docID;
            try
            {
                if (ds.Tables.Count > 0)
                {
                    DtDistributor = DbConnectionDAL.GetDataTable(CommandType.Text, "Select PartyName,MP.Areaid AS Areaid,cityid,MD.AreaId AS Distinctid,MS.AreaId AS Stateid,MR.AreaId AS Regionid from MastParty  MP LEFT JOIN MastArea MC ON MC.AreaId=MP.CityId LEFT JOIN MastArea MD ON MD.AreaId=MC.UnderId LEFT JOIN MastArea MS ON MS.AreaId=MD.UnderId LEFT JOIN MastArea MR ON MR.AreaId=MS.UnderId  where PartyID=" + ds.Tables[0].Rows[0]["DistID"].ToString() + "");
                    cityid = Convert.ToInt32(DtDistributor.Rows[0]["cityid"].ToString());
                    regionid = Convert.ToInt32(DtDistributor.Rows[0]["Regionid"].ToString());
                    distictid = Convert.ToInt32(DtDistributor.Rows[0]["Distinctid"].ToString());

                    beatid = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, "SELECT AreaId from MastArea where AreaType='BEAT' AND AreaName='Beat-" + DtDistributor.Rows[0]["PartyName"].ToString() + "'"));
                    DTadmin = DbConnectionDAL.GetDataTable(CommandType.Text, "Select * from MastSalesRep Where SMName='DIRECTOR'");



                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        if (ds.Tables[0].Rows[i]["GRAHAAKID"].ToString() == "")
                        {


                            mandatorymsg ms = new mandatorymsg();

                            string Date;

                            if (ds.Tables[0].Rows[i]["CREDITLIMIT"].ToString() == "")
                                ds.Tables[0].Rows[i]["CREDITLIMIT"] = "0";

                            cnt = PB.InsertParty_Tally(ds.Tables[0].Rows[i]["PARTYNAME"].ToString(), DtDistributor.Rows[0]["PartyName"].ToString(), ds.Tables[0].Rows[i]["ADDRESS"].ToString(), ds.Tables[0].Rows[i]["ADDRESS2"].ToString(), Convert.ToInt32(cityid), Convert.ToInt32(DtDistributor.Rows[0]["Areaid"].ToString()), beatid, Convert.ToInt32(ds.Tables[0].Rows[0]["DistID"].ToString()), ds.Tables[0].Rows[i]["PINCODE"].ToString(), ds.Tables[0].Rows[i]["LEDGERMOBILE"].ToString(), ds.Tables[0].Rows[i]["LEDGERPHONE"].ToString(), "", ds.Tables[0].Rows[i]["PARTYID"].ToString(), true, "", 0, ds.Tables[0].Rows[i]["LEDGRCONTACT"].ToString(), "", "", "", ds.Tables[0].Rows[i]["ITN"].ToString() == "" ? "" : ds.Tables[0].Rows[i]["ITN"].ToString(), ds.Tables[0].Rows[i]["CREDITLIMIT"].ToString() == "" ? 0 : Convert.ToDecimal(ds.Tables[0].Rows[i]["CREDITLIMIT"].ToString()), 0, Convert.ToInt32(DTadmin.Rows[0]["Userid"]), "", "", ds.Tables[0].Rows[i]["EMAIL"].ToString(), "", ds.Tables[0].Rows[i]["GSTNO"].ToString(), Convert.ToInt32(DTadmin.Rows[0]["SMid"].ToString()), ds.Tables[0].Rows[i]["GSTNO"].ToString(), ds.Tables[0].Rows[i]["COMPANYNUMBER"].ToString(),
                               ds.Tables[0].Rows[i]["COUNTRYNAME"].ToString(), ds.Tables[0].Rows[i]["STATENAME"].ToString(), countryid, regionid, stateid, distictid, citytypeid, cityconveyancetype, Convert.ToInt32(ds.Tables[0].Rows[i]["DistID"].ToString()));
                            if (cnt > 0)
                            {
                                //if (Convert.ToDecimal(ds.Tables[0].Rows[i]["OPENINGBALANCEDR"].ToString()) != 0 || Convert.ToDecimal(ds.Tables[0].Rows[i]["OPENINGBALANCECR"].ToString()) != 0)
                                //{
                                //    decimal Amount = 0;
                                //    decimal Entryno = 0;
                                //    Date = "01/Apr/" + Convert.ToDateTime(ds.Tables[0].Rows[i]["DATE"].ToString()).Year.ToString();
                                //    docID = "TLOP" + ds.Tables[0].Rows[i]["COMPANYNUMBER"].ToString().Substring(ds.Tables[0].Rows[i]["COMPANYNUMBER"].ToString().Length - 3) + " " + Convert.ToString(Convert.ToDateTime(ds.Tables[0].Rows[i]["DATE"].ToString()).Year) + " " + Convert.ToString(opvoucherno);
                                //    if (Convert.ToDecimal(ds.Tables[0].Rows[i]["OPENINGBALANCEDR"].ToString()) != 0)
                                //    {
                                //        Amount = Convert.ToDecimal(ds.Tables[0].Rows[i]["OPENINGBALANCEDR"].ToString());
                                //    }
                                //    else
                                //    {
                                //        Amount = -Convert.ToDecimal(ds.Tables[0].Rows[i]["OPENINGBALANCECR"].ToString());
                                //    }
                                //    string str1 = "Select Count(*) from TransRetailerLedger where VDate='" + Date + "' and Narration=' Opening Balance' and DistId=" + cnt + "";
                                //    int count = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, str1));
                                //    if (count == 0)
                                //    {

                                //        Entryno = Convert.ToDecimal(DbConnectionDAL.GetScalarValue(CommandType.Text, "select Isnull(Max(DistLedId),0)+1 from  TransRetailerLedger"));
                                //        result = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, "Insert into TransRetailerLedger(DLDocId,Vdate,DistID,Amount,AmtCr,AmtDr,Narration,COMPANYCODE) values('" + docID + "','" + Date + "', " + cnt + "," + Amount + "," + Convert.ToDecimal(ds.Tables[0].Rows[i]["OPENINGBALANCECR"].ToString()) + "," + Convert.ToDecimal(ds.Tables[0].Rows[i]["OPENINGBALANCEDR"].ToString()) + ",' Opening Balance','" + ds.Tables[0].Rows[i]["COMPANYNUMBER"].ToString() + "')"));
                                //    }

                                //}
                                //status = InsertLicense(ds.Tables[0].Rows[i]["PARTYNAME"].ToString(), ds.Tables[0].Rows[i]["LEDGERMOBILE"].ToString());
                                //if (status == "Yes") _Message = "";
                                //else if (status == "No") _Message = " But somthing went wrong in License Creation process";
                                ms.msdmsg = ds.Tables[0].Rows[i]["PARTYNAME"].ToString() + " is inserted successfully";
                                msglist.Add(ms);
                                success = success + 1;

                            }
                            else if (cnt == -1)
                            {
                                ms.msdmsg = ds.Tables[0].Rows[i]["PARTYNAME"].ToString() + " is not inserted,Already exist in login Mast";
                                msglist.Add(ms);
                                failure = failure + 1;
                                // LogError(ds.Tables[0].Rows[i]["PARTYNAME"].ToString() + " is not inserted,Already exist in login Mast");
                            }
                            else if (cnt == -3)
                            {
                                ms.msdmsg = ds.Tables[0].Rows[i]["PARTYNAME"].ToString() + " is not inserted,Duplicate Mobile No.";
                                msglist.Add(ms);
                                failure = failure + 1;
                                // LogError(ds.Tables[0].Rows[i]["PARTYNAME"].ToString() + " is not inserted,Already exist in login Mast");
                            }
                            else if (cnt == -2)
                            {
                                int retval = PB.UpdateParty_Tally(ds.Tables[0].Rows[i]["PARTYNAME"].ToString(), DtDistributor.Rows[0]["PartyName"].ToString(), ds.Tables[0].Rows[i]["ADDRESS"].ToString(), ds.Tables[0].Rows[i]["ADDRESS2"].ToString(), Convert.ToInt32(cityid), Convert.ToInt32(DtDistributor.Rows[0]["Areaid"].ToString()), beatid, Convert.ToInt32(ds.Tables[0].Rows[0]["DistID"].ToString()), ds.Tables[0].Rows[i]["PINCODE"].ToString(), ds.Tables[0].Rows[i]["LEDGERMOBILE"].ToString(), ds.Tables[0].Rows[i]["LEDGERPHONE"].ToString(), "", ds.Tables[0].Rows[i]["DistID"].ToString() + "#" + ds.Tables[0].Rows[i]["PARTYID"].ToString(), true, "", 0, ds.Tables[0].Rows[i]["LEDGRCONTACT"].ToString(), "", "", "", ds.Tables[0].Rows[i]["ITN"].ToString() == "" ? "" : ds.Tables[0].Rows[i]["ITN"].ToString(), ds.Tables[0].Rows[i]["CREDITLIMIT"].ToString() == "" ? 0 : Convert.ToDecimal(ds.Tables[0].Rows[i]["CREDITLIMIT"].ToString()), 0, Convert.ToInt32(DTadmin.Rows[0]["Userid"]), "", "", ds.Tables[0].Rows[i]["EMAIL"].ToString(), "", ds.Tables[0].Rows[i]["GSTNO"].ToString(), Convert.ToInt32(DTadmin.Rows[0]["SMid"].ToString()), ds.Tables[0].Rows[i]["GSTNO"].ToString(), ds.Tables[0].Rows[i]["COMPANYNUMBER"].ToString(),
                                 ds.Tables[0].Rows[i]["COUNTRYNAME"].ToString(), ds.Tables[0].Rows[i]["STATENAME"].ToString(), countryid, regionid, stateid, distictid, citytypeid, cityconveyancetype, Convert.ToInt32(ds.Tables[0].Rows[i]["DistID"].ToString()));
                                //_query = "UPDATE MastParty SET PartyName = '" + ds.Tables[0].Rows[i]["PARTYNAME"].ToString() + "',Address1 = '" + ds.Tables[0].Rows[i]["ADDRESS"].ToString() + "',	Address2 ='" + ds.Tables[0].Rows[i]["ADDRESS2"].ToString() + "',Pin = '" +  ds.Tables[0].Rows[i]["PINCODE"].ToString() + "'',	AreaId = " + Areaid + ",";
                                //_query = _query + " Email = '" + ds.Tables[0].Rows[i]["EMAIL"].ToString() + "',Mobile = '" + ds.Tables[0].Rows[i]["LEDGERMOBILE"].ToString() + "',	Active = 'true',PartyDist = 'true',UserId = " + Convert.ToInt32(DTadmin.Rows[0]["Userid"]) + ",	Lvl = 1,Created_User_id = " + Convert.ToInt32(DTadmin.Rows[0]["Userid"]) + ",";
                                //_query = _query + " DisplayName = '" + ds.Tables[0].Rows[i]["PARTYNAME"].ToString() + "',ContactPerson = '" + ds.Tables[0].Rows[i]["LEDGRCONTACT"].ToString() + "',PANNo = '" + ds.Tables[0].Rows[i]["ITN"].ToString() + "',	CityId = " + cityid + ",CreditLimit = " +  ds.Tables[0].Rows[i]["CREDITLIMIT"].ToString() + ",";
                                //_query = _query + " SMID = " + Convert.ToInt32(DTadmin.Rows[0]["SMID"]) + ",Fax = '" + ds.Tables[0].Rows[i]["FAX"].ToString() + "',	GSTINNo ='" +  ds.Tables[0].Rows[i]["GSTNO"].ToString() + "'' where PartyId=" + DTparty.Rows[0]["PartyId"].ToString() + "";
                                //cnt = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, _query));

                                if (retval > 0)
                                {
                                    ////if (Convert.ToDecimal(ds.Tables[0].Rows[i]["OPENINGBALANCEDR"].ToString()) != 0 || Convert.ToDecimal(ds.Tables[0].Rows[i]["OPENINGBALANCECR"].ToString()) != 0)
                                    ////{

                                    ////    decimal Amount = 0;
                                    ////    decimal Entryno = 0;
                                    ////    Date = "01/Apr/" + Convert.ToDateTime(ds.Tables[0].Rows[i]["DATE"].ToString()).Year.ToString();
                                    ////    docID = "TLOP" + ds.Tables[0].Rows[i]["COMPANYNUMBER"].ToString().Substring(ds.Tables[0].Rows[i]["COMPANYNUMBER"].ToString().Length - 3) + " " + Convert.ToString(Convert.ToDateTime(ds.Tables[0].Rows[i]["DATE"].ToString()).Year) + " " + Convert.ToString(opvoucherno);
                                    ////    if (Convert.ToDecimal(ds.Tables[0].Rows[i]["OPENINGBALANCEDR"].ToString()) != 0)
                                    ////    {
                                    ////        Amount = Convert.ToDecimal(ds.Tables[0].Rows[i]["OPENINGBALANCEDR"].ToString());
                                    ////    }
                                    ////    else
                                    ////    {
                                    ////        Amount = -Convert.ToDecimal(ds.Tables[0].Rows[i]["OPENINGBALANCECR"].ToString());
                                    ////    }
                                    ////    string str1 = "Select DLDocId from TransRetailerLedger where VDate='" + Date + "' and Narration=' Opening Balance' and DistId=" + retval + "";
                                    ////    string DD = Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, str1));
                                    ////    if (DD == "")
                                    ////    {
                                    ////        Entryno = Convert.ToDecimal(DbConnectionDAL.GetScalarValue(CommandType.Text, "select Isnull(Max(DistLedId),0)+1 from  TransRetailerLedger"));
                                    ////        result = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, "Insert into TransRetailerLedger(DLDocId,Vdate,DistID,Amount,AmtCr,AmtDr,Narration,COMPANYCODE,entryno) values('" + docID + "','" + Date + "', " + retval + "," + Amount + "," + Convert.ToDecimal(ds.Tables[0].Rows[i]["OPENINGBALANCECR"].ToString()) + "," + Convert.ToDecimal(ds.Tables[0].Rows[i]["OPENINGBALANCEDR"].ToString()) + ",' Opening Balance','" + ds.Tables[0].Rows[i]["COMPANYNUMBER"].ToString() + "'," + Entryno + ")"));
                                    ////    }
                                    ////    else
                                    ////    {
                                    ////        DbConnectionDAL.ExecuteNonQuery(CommandType.Text, "Update TransRetailerLedger set Amount=" + Amount + ",AmtCr=" + Convert.ToDecimal(ds.Tables[0].Rows[i]["OPENINGBALANCECR"].ToString()) + ",AmtDr=" + Convert.ToDecimal(ds.Tables[0].Rows[i]["OPENINGBALANCEDR"].ToString()) + "  where VDate='" + Date + "' and Narration=' Opening Balance' and DistId=" + retval + "");

                                    ////    }
                                    ////    //       result = DB.insertdistledger_Tally(retval, docID,Date,
                                    ////    //Convert.ToDecimal(ds.Tables[0].Rows[i]["OPENINGBALANCEDR"].ToString()) != 0 ? Convert.ToDecimal(ds.Tables[0].Rows[i]["OPENINGBALANCEDR"].ToString()) : -Convert.ToDecimal(ds.Tables[0].Rows[i]["OPENINGBALANCECR"].ToString()), " Opening Balance",
                                    ////    //      ds.Tables[0].Rows[i]["COMPANYNUMBER"].ToString());

                                    ////}
                                    //status = UpdateLicense(ds.Tables[0].Rows[i]["PARTYID"].ToString(), ds.Tables[0].Rows[i]["LEDGERMOBILE"].ToString(), ds.Tables[0].Rows[i]["PARTYNAME"].ToString(), ds.Tables[0].Rows[i]["COMPANYNUMBER"].ToString());
                                    //if (status == "Yes") _Message = "";
                                    //else if (status == "No") _Message = " But somthing went wrong in License Creation process";
                                    ms.msdmsg = ds.Tables[0].Rows[i]["PARTYNAME"].ToString() + " is updated successfully" + _Message + "";

                                    msglist.Add(ms);
                                    success = success + 1;
                                    opvoucherno++;
                                }
                                else
                                {
                                    ms.msdmsg = ds.Tables[0].Rows[i]["PARTYNAME"].ToString() + " is not updated,Something went wrong";
                                    msglist.Add(ms);
                                    failure = failure + 1;
                                    // LogError(ds.Tables[0].Rows[i]["PARTYNAME"].ToString() + " is not inserted,Something went wrong");
                                }
                                //ms.msdmsg = ds.Tables[0].Rows[i]["PARTYNAME"].ToString() + " is not inserted,Sync id is duplicate";
                                //msglist.Add(ms);
                                //failure = failure + 1;
                                //LogError(ds.Tables[0].Rows[i]["PARTYNAME"].ToString() + "  is not inserted,Sync id is duplicate");
                            }
                            else
                            {
                                ms.msdmsg = ds.Tables[0].Rows[i]["PARTYNAME"].ToString() + " is not inserted,Something went wrong";
                                msglist.Add(ms);
                                failure = failure + 1;
                                // LogError(ds.Tables[0].Rows[i]["PARTYNAME"].ToString() + " is not inserted,Something went wrong");
                            }

                        }
                        else
                        {
                            string _sql = "Update MastParty Set SyncId='" + Convert.ToInt32(ds.Tables[0].Rows[i]["DistID"].ToString()) + "#" + ds.Tables[0].Rows[i]["PARTYID"].ToString() + "' where PartyId=" + ds.Tables[0].Rows[i]["GRAHAAKID"].ToString() + "";
                            int DistInv1Id = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, _sql));
                            success = success + 1;
                        }
                    }

                }

                //transactionScope.Complete();
                //transactionScope.Dispose();
                str += success + " record inserted successfully <br/>";
                if (failure > 0)
                    str = str + "," + failure + " record are failed";
                //if (err > 0)
                //    str = str + ",Check log ";
                if (msglist.Count == 0)
                {
                    mandatorymsg ms = new mandatorymsg();
                    ms.msdmsg = "No error";
                    msglist.Add(ms);
                }

                rs.msg = str;
                rs.errormsg = msglist;
                //this.SucessEmailStatus();
            }
            catch (TransactionException ex)
            {
                //transactionScope.Dispose();
                cnt = 0;
                str = ex.Message;
                err = err + 1;
                LogError(str);
                // msglist..Add(str);
                if (err > 0)
                    str = str + ",Check log ";
                rs.msg = str;
                rs.errormsg = msglist;

            }
            if (str != "")
            {
                SendEmail("Regarding Log Detail of Retailer Transfer from tally", rs, str);
            }
            //}
            Context.Response.Write(JsonConvert.SerializeObject(rs));

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void insertretailerhavinginvoice()
        {
            string str = "";
            int regionid = 0;
            int citytypeid = 0;
            int cityconveyancetype = 0;
            int distictid = 0;
            int countryid = 0;
            int stateid = 0;
            int beatid = 0;
            int cityid = 0;
            int cnt = 0;
            int success = 0;
            int failure = 0;
            string _sql = "";
            DataTable DtDistributor = new DataTable();
            DataTable DTadmin = new DataTable();
            mandatorymsg ms = new mandatorymsg();
            List<mandatorymsg> msglist = new List<mandatorymsg>();
            errorresult rs = new errorresult();
            var bodyStream = new StreamReader(HttpContext.Current.Request.InputStream);
            try
            {


                bodyStream.BaseStream.Seek(0, SeekOrigin.Begin);
                var bodyText = bodyStream.ReadToEnd();
                StringReader theReader = new StringReader(bodyText);
                DataSet ds = new DataSet();

                ds.ReadXml(theReader);

                //////////////////////Get PartyData////////////////

                if (ds.Tables.Count > 1)
                {
                    if (ds.Tables["ITEMDETAIL"].Rows.Count > 0)
                    {

                        var distinctParty = ds.Tables["ITEMDETAIL"].AsEnumerable().GroupBy(s => s.Field<string>("PARTYMASTERID")).Select(s => new
                        {
                            Partyid = s.Key,
                            Address = s.Max(x => x.Field<string>("PartyAddress")),
                            Address1 = s.Max(x => x.Field<string>("PartyAddressnative")),
                            Email = s.Max(x => x.Field<string>("PartyEmail")),
                            Pincode = s.Max(x => x.Field<string>("PartyPinCode")),
                            Partyname = s.Max(x => x.Field<string>("PARTY")),
                            PartyCreditlimit = s.Max(x => x.Field<string>("PartyCreditlimit")),
                            Mobile = s.Max(x => x.Field<string>("PartyLedgermobile")),
                            Phone = s.Max(x => x.Field<string>("PartyLedgerphone")),
                            Contact = s.Max(x => x.Field<string>("PartyLedgercontact")),
                            GSTIN = s.Max(x => x.Field<string>("Partygstin")),
                            Fax = s.Max(x => x.Field<string>("PartyLedgerfax")),
                            IncomeTaxNumber = s.Max(x => x.Field<string>("PartyIncomeTaxNumber")),
                            Statename = s.Max(x => x.Field<string>("PartyLedstatename")),
                            CountryName = s.Max(x => x.Field<string>("PartyCountryName")),
                            GrahaakID = s.Max(x => x.Field<string>("PartyGrahaakID")),
                            Companynumber = s.Max(x => x.Field<string>("COMPANYNUMBER")),
                            Distributor = s.Max(x => x.Field<string>("DistID")),
                            GrahaakParty = s.Max(x => x.Field<string>("GrahaakParty")),

                        }).ToList();



                        DtDistributor = DbConnectionDAL.GetDataTable(CommandType.Text, "Select PartyName,MP.Areaid AS Areaid,cityid,MD.AreaId AS Distinctid,MS.AreaId AS Stateid,MR.AreaId AS Regionid from MastParty  MP LEFT JOIN MastArea MC ON MC.AreaId=MP.CityId LEFT JOIN MastArea MD ON MD.AreaId=MC.UnderId LEFT JOIN MastArea MS ON MS.AreaId=MD.UnderId LEFT JOIN MastArea MR ON MR.AreaId=MS.UnderId  where PartyID=" + distinctParty[0].Distributor + "");
                        cityid = Convert.ToInt32(DtDistributor.Rows[0]["cityid"].ToString());
                        regionid = Convert.ToInt32(DtDistributor.Rows[0]["Regionid"].ToString());
                        distictid = Convert.ToInt32(DtDistributor.Rows[0]["Distinctid"].ToString());

                        beatid = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, "SELECT AreaId from MastArea where AreaType='BEAT' AND AreaName='Beat-" + DtDistributor.Rows[0]["PartyName"].ToString() + "'"));
                        DTadmin = DbConnectionDAL.GetDataTable(CommandType.Text, "Select * from MastSalesRep Where SMName='DIRECTOR'");


                        if (distinctParty.Count > 0)
                        {
                            str = "Total Retailer having invoice " + distinctParty.Count + ".<br/>";
                        }

                        for (int i = 0; i < distinctParty.Count; i++)
                        {
                            if (distinctParty[i].GrahaakID == "")
                            {


                                ms = new mandatorymsg();

                                string Date;

                                //if (distinctParty[i].PartyCreditlimit == "")
                                //  distinctParty[i].PartyCreditlimit = "0";

                                cnt = PB.InsertParty_Tally(distinctParty[i].Partyname, DtDistributor.Rows[0]["PartyName"].ToString(), distinctParty[i].Address, distinctParty[i].Address1, Convert.ToInt32(cityid), Convert.ToInt32(DtDistributor.Rows[0]["Areaid"].ToString()), beatid, Convert.ToInt32(distinctParty[i].Distributor), distinctParty[i].Pincode, distinctParty[i].Mobile, distinctParty[i].Phone, "", distinctParty[i].Partyid, true, "", 0, distinctParty[i].Contact, "", "", "", distinctParty[i].IncomeTaxNumber == "" ? "" : distinctParty[i].IncomeTaxNumber, distinctParty[i].PartyCreditlimit == "" ? 0 : Convert.ToDecimal(distinctParty[i].PartyCreditlimit), 0, Convert.ToInt32(DTadmin.Rows[0]["Userid"]), "", "", distinctParty[i].Email, "", distinctParty[i].GSTIN, Convert.ToInt32(DTadmin.Rows[0]["SMid"].ToString()), distinctParty[i].GSTIN, distinctParty[i].Companynumber,
                                  distinctParty[i].CountryName, distinctParty[i].Statename, countryid, regionid, stateid, distictid, citytypeid, cityconveyancetype, Convert.ToInt32(distinctParty[i].Distributor));
                                if (cnt > 0)
                                {

                                    ms.msdmsg = distinctParty[i].Partyname + " is inserted successfully";
                                    msglist.Add(ms);
                                    success = success + 1;

                                }
                                else if (cnt == -1)
                                {
                                    ms.msdmsg = distinctParty[i].Partyname + " is not inserted,Already exist in login Mast";
                                    msglist.Add(ms);
                                    failure = failure + 1;
                                    // LogError(ds.Tables[0].Rows[i]["PARTYNAME"].ToString() + " is not inserted,Already exist in login Mast");
                                }
                                else if (cnt == -3)
                                {
                                    ms.msdmsg = distinctParty[i].Partyname + " is not inserted,Duplicate Mobile No.";
                                    msglist.Add(ms);
                                    failure = failure + 1;
                                    // LogError(ds.Tables[0].Rows[i]["PARTYNAME"].ToString() + " is not inserted,Already exist in login Mast");
                                }
                                else if (cnt == -2)
                                {
                                    int retval = PB.UpdateParty_Tally(distinctParty[i].Partyname, DtDistributor.Rows[0]["PartyName"].ToString(), distinctParty[i].Address, distinctParty[i].Address1, Convert.ToInt32(cityid), Convert.ToInt32(DtDistributor.Rows[0]["Areaid"].ToString()), beatid, Convert.ToInt32(distinctParty[i].Distributor), distinctParty[i].Pincode, distinctParty[i].Mobile, distinctParty[i].Phone, "", distinctParty[i].Distributor + "#" + distinctParty[i].Partyid, true, "", 0, distinctParty[i].Contact, "", "", "", distinctParty[i].IncomeTaxNumber == "" ? "" : distinctParty[i].IncomeTaxNumber, distinctParty[i].PartyCreditlimit == "" ? 0 : Convert.ToDecimal(distinctParty[i].PartyCreditlimit), 0, Convert.ToInt32(DTadmin.Rows[0]["Userid"]), "", "", distinctParty[i].Email, "", distinctParty[i].GSTIN, Convert.ToInt32(DTadmin.Rows[0]["SMid"].ToString()), distinctParty[i].GSTIN, distinctParty[i].Companynumber,
                                     distinctParty[i].CountryName, distinctParty[i].Statename, countryid, regionid, stateid, distictid, citytypeid, cityconveyancetype, Convert.ToInt32(distinctParty[i].Distributor));


                                    if (retval > 0)
                                    {
                                        ms.msdmsg = distinctParty[i].Partyname + " is updated successfully";

                                        msglist.Add(ms);
                                        success = success + 1;

                                    }
                                    else
                                    {
                                        ms.msdmsg = distinctParty[i].Partyname + " is not updated,Something went wrong";
                                        msglist.Add(ms);
                                        failure = failure + 1;
                                        // LogError(ds.Tables[0].Rows[i]["PARTYNAME"].ToString() + " is not inserted,Something went wrong");
                                    }
                                    //ms.msdmsg = ds.Tables[0].Rows[i]["PARTYNAME"].ToString() + " is not inserted,Sync id is duplicate";
                                    //msglist.Add(ms);
                                    //failure = failure + 1;
                                    //LogError(ds.Tables[0].Rows[i]["PARTYNAME"].ToString() + "  is not inserted,Sync id is duplicate");
                                }
                                else
                                {
                                    ms.msdmsg = distinctParty[i].Partyname + " is not inserted,Something went wrong";
                                    msglist.Add(ms);
                                    failure = failure + 1;
                                    // LogError(ds.Tables[0].Rows[i]["PARTYNAME"].ToString() + " is not inserted,Something went wrong");
                                }

                            }
                            else
                            {
                                _sql = "Update MastParty Set SyncId='" + Convert.ToInt32(distinctParty[i].Distributor) + "#" + distinctParty[i].Partyid + "' where PartyId=" + distinctParty[i].GrahaakID + "";
                                int updateid = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, _sql));
                            }
                        }


                        str += success + " record inserted successfully <br/>";
                        if (failure > 0)
                            str = str + "," + failure + " record are failed";
                        //if (err > 0)
                        //    str = str + ",Check log ";
                        if (msglist.Count == 0)
                        {
                            ms = new mandatorymsg();
                            ms.msdmsg = "No error";
                            msglist.Add(ms);
                        }

                        rs.msg = str;
                        rs.errormsg = msglist;
                    }
                }

            }



            catch (Exception ex)
            {

                cnt = 0;
                str = ex.Message;
                //  err = err + 1;
                LogError(str);
                // msglist..Add(str);
                //if (err > 0)
                //    str = str + ",Check log ";
                rs.msg = str;
                rs.errormsg = msglist;

            }
            if (str != "")
            {


                SendEmail("Regarding Log Detail of Retailer(having invoice) Transfer from tally", rs, str);
            }
            Context.Response.Write(JsonConvert.SerializeObject(rs));
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void insertretailersalesinvoicefromtally()
        {
            string _query = "", _fList = "", _sql = "";
            int cnt = 0, result = 0;
            int DistInvId = 0;
            int DistInv1Id = 0;
            string Insert = "";
            string Duplicate = "";
            int sno = 0;
            string DistInvDocId = "";
            decimal taxamt = 0;
            decimal roundoff = 0;
            decimal discount = 0;
            string dis = "";
            string taxstr = "";
            string roff = "";
            //List<TransDistInv> TransDistInvList = JsonConvert.DeserializeObject<List<TransDistInv>>(value);
            int success = 0;
            int failure = 0;
            string str = "";
            int itemid = 0;
            int itemid1 = 0;
            int cityid = 0;
            int regionid = 0;
            int citytypeid = 0;
            int cityconveyancetype = 0;
            int distictid = 0;
            int countryid = 0;
            int stateid = 0;
            int beatid = 0;
            string zi = "0";
            //this.IsValidJson(value);
            CultureInfo provider = new CultureInfo("en-US");
            var myInsertList = new List<string>();
            var myDuplicateList = new List<string>();
            var bodyStream = new StreamReader(HttpContext.Current.Request.InputStream);
            DataTable DTadmin = new DataTable();
            mandatorymsg ms = new mandatorymsg();
            List<mandatorymsg> msglist = new List<mandatorymsg>();
            errorresult rs = new errorresult();
            DataTable DtDistributor = new DataTable();
            int DistInv2Id = 0;
            citytypeid = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, "Select Id From MastCityType Where Name='Other'"));
            cityconveyancetype = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, "Select Id From MastCityType Where Name='OTHERS'"));
            try
            {



                bodyStream.BaseStream.Seek(0, SeekOrigin.Begin);
                var bodyText = bodyStream.ReadToEnd();
                StringReader theReader = new StringReader(bodyText);
                DataSet ds = new DataSet();

                ds.ReadXml(theReader);


                if (ds.Tables.Count > 1)
                {
                    if (ds.Tables["ITEMDETAIL"].Rows.Count > 0)
                    {
                        string sqlConnectionstring = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                        SqlConnection cn = new SqlConnection(sqlConnectionstring);
                        cn.Open();
                        SqlBulkCopy objbulk = new SqlBulkCopy(cn);
                        //assigning Destination table name  
                        objbulk.DestinationTableName = "TempTally_RetailerSaleinvoiceitemdetail";
                        objbulk.ColumnMappings.Add("ITEM", "ITEM");
                        objbulk.ColumnMappings.Add("QTY", "QTY");
                        objbulk.ColumnMappings.Add("BILLNO", "BILLNO");
                        objbulk.ColumnMappings.Add("AMOUNT", "AMOUNT");
                        objbulk.ColumnMappings.Add("DATE", "DATE");

                        objbulk.ColumnMappings.Add("PARTY", "PARTY");
                        objbulk.ColumnMappings.Add("MASTER", "MASTER");
                        objbulk.ColumnMappings.Add("SYSTEMDATE", "SYSTEMDATE");
                        objbulk.ColumnMappings.Add("SYSTEMTIME", "SYSTEMTIME");

                        objbulk.ColumnMappings.Add("RATE", "RATE");
                        objbulk.ColumnMappings.Add("PARTYMASTERID", "PARTYMASTERID");
                        objbulk.ColumnMappings.Add("TOTALAMOUNT", "TOTALAMOUNT");
                        objbulk.ColumnMappings.Add("COMPANYNAME", "COMPANYNAME");

                        objbulk.ColumnMappings.Add("COMPANYNUMBER", "COMPANYNUMBER");
                        objbulk.ColumnMappings.Add("ITEMMASTERID", "ITEMMASTERID");
                        objbulk.ColumnMappings.Add("DISTID", "DISTID");

                        objbulk.WriteToServer(ds.Tables["ITEMDETAIL"]);
                        cn.Close();
                    }
                    if (ds.Tables.Count > 3 && ds.Tables["LEDGERDETAIL"].Rows.Count > 0)
                    {
                        string sqlConnectionstring = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                        SqlConnection cn = new SqlConnection(sqlConnectionstring);
                        cn.Open();
                        SqlBulkCopy objbulk = new SqlBulkCopy(cn);
                        //assigning Destination table name  
                        objbulk.DestinationTableName = "TempTally_RetailerSaleInvoiceTaxDetails";
                        //Mapping Table column  
                        objbulk.ColumnMappings.Add("LEDGERNAME", "LEDGERNAME");
                        objbulk.ColumnMappings.Add("LEDGERAMOUNT", "LEDGERAMOUNT");
                        objbulk.ColumnMappings.Add("BILLNO", "BILLNO");
                        objbulk.ColumnMappings.Add("MASTER", "MASTER");
                        objbulk.ColumnMappings.Add("PARENTTAX", "PARENTTAX");
                        //inserting bulk Records into DataBase   
                        objbulk.WriteToServer(ds.Tables["LEDGERDETAIL"]);
                        cn.Close();
                    }
                    //////////////////////Get PartyData////////////////

                    //  var distinctParty = ds.Tables["ITEMDETAIL"].AsEnumerable().GroupBy(s => s.Field<string>("PARTYMASTERID")).Select(s => new
                    //{
                    //    Partyid = s.Key,
                    //    Address = s.Max(x => x.Field<string>("PartyAddress")),
                    //    Address1 = s.Max(x => x.Field<string>("PartyAddressnative")),
                    //    Email = s.Max(x => x.Field<string>("PartyEmail")),
                    //    Pincode = s.Max(x => x.Field<string>("PartyPinCode")),
                    //    Partyname = s.Max(x => x.Field<string>("PARTY")),
                    //    PartyCreditlimit = s.Max(x => x.Field<string>("PartyCreditlimit")),
                    //    Mobile = s.Max(x => x.Field<string>("PartyLedgermobile")),
                    //    Phone = s.Max(x => x.Field<string>("PartyLedgerphone")),
                    //    Contact = s.Max(x => x.Field<string>("PartyLedgercontact")),
                    //    GSTIN = s.Max(x => x.Field<string>("Partygstin")),
                    //    Fax = s.Max(x => x.Field<string>("PartyLedgerfax")),
                    //    IncomeTaxNumber = s.Max(x => x.Field<string>("PartyIncomeTaxNumber")),
                    //    Statename = s.Max(x => x.Field<string>("PartyLedstatename")),
                    //    CountryName = s.Max(x => x.Field<string>("PartyCountryName")),
                    //    GrahaakID = s.Max(x => x.Field<string>("PartyGrahaakID")),
                    //          Companynumber = s.Max(x => x.Field<string>("COMPANYNUMBER")),

                    //       Distributor = s.Max(x => x.Field<string>("DistID")),

                    //}).ToList();



                    //  DtDistributor = DbConnectionDAL.GetDataTable(CommandType.Text, "Select PartyName,MP.Areaid AS Areaid,cityid,MD.AreaId AS Distinctid,MS.AreaId AS Stateid,MR.AreaId AS Regionid from MastParty  MP LEFT JOIN MastArea MC ON MC.AreaId=MP.CityId LEFT JOIN MastArea MD ON MD.AreaId=MC.UnderId LEFT JOIN MastArea MS ON MS.AreaId=MD.UnderId LEFT JOIN MastArea MR ON MR.AreaId=MS.UnderId  where PartyID=" + distinctParty[0].Distributor + "");
                    //  cityid = Convert.ToInt32(DtDistributor.Rows[0]["cityid"].ToString());
                    //  regionid = Convert.ToInt32(DtDistributor.Rows[0]["Regionid"].ToString());
                    //  distictid = Convert.ToInt32(DtDistributor.Rows[0]["Distinctid"].ToString());

                    //  beatid = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, "SELECT AreaId from MastArea where AreaType='BEAT' AND AreaName='Beat-" + DtDistributor.Rows[0]["PartyName"].ToString() + "'"));
                    //  DTadmin = DbConnectionDAL.GetDataTable(CommandType.Text, "Select * from MastSalesRep Where SMName='DIRECTOR'");

                    //  for (int i = 0; i < distinctParty.Count; i++)
                    //  {
                    //      if (distinctParty[i].GrahaakID == "")
                    //      {


                    //         ms = new mandatorymsg();

                    //          string Date;

                    //          //if (distinctParty[i].PartyCreditlimit == "")
                    //          //  distinctParty[i].PartyCreditlimit = "0";

                    //          cnt = PB.InsertParty_Tally(distinctParty[i].Partyname, DtDistributor.Rows[0]["PartyName"].ToString(), distinctParty[i].Address, distinctParty[i].Address1, Convert.ToInt32(cityid), Convert.ToInt32(DtDistributor.Rows[0]["Areaid"].ToString()), beatid, Convert.ToInt32(distinctParty[i].Distributor), distinctParty[i].Pincode, distinctParty[i].Mobile, distinctParty[i].Phone, "", distinctParty[i].Partyid, true, "", 0, distinctParty[i].Contact, "", "", "", distinctParty[i].IncomeTaxNumber == "" ? "" : distinctParty[i].IncomeTaxNumber, distinctParty[i].PartyCreditlimit == "" ? 0 : Convert.ToDecimal(distinctParty[i].PartyCreditlimit), 0, Convert.ToInt32(DTadmin.Rows[0]["Userid"]), "", "", distinctParty[i].Email, "", distinctParty[i].GSTIN, Convert.ToInt32(DTadmin.Rows[0]["SMid"].ToString()), distinctParty[i].GSTIN, distinctParty[i].Companynumber,
                    //            distinctParty[i].CountryName, distinctParty[i].Statename, countryid, regionid, stateid, distictid, citytypeid, cityconveyancetype, Convert.ToInt32(distinctParty[i].Distributor));
                    //          if (cnt > 0)
                    //          {

                    //              ms.msdmsg = distinctParty[i].Partyname + " is inserted successfully";
                    //              msglist.Add(ms);
                    //              success = success + 1;

                    //          }
                    //          else if (cnt == -1)
                    //          {
                    //              ms.msdmsg = distinctParty[i].Partyname + " is not inserted,Already exist in login Mast";
                    //              msglist.Add(ms);
                    //              failure = failure + 1;
                    //              // LogError(ds.Tables[0].Rows[i]["PARTYNAME"].ToString() + " is not inserted,Already exist in login Mast");
                    //          }
                    //          else if (cnt == -3)
                    //          {
                    //              ms.msdmsg = distinctParty[i].Partyname + " is not inserted,Duplicate Mobile No.";
                    //              msglist.Add(ms);
                    //              failure = failure + 1;
                    //              // LogError(ds.Tables[0].Rows[i]["PARTYNAME"].ToString() + " is not inserted,Already exist in login Mast");
                    //          }
                    //          else if (cnt == -2)
                    //          {
                    //              int retval = PB.UpdateParty_Tally(distinctParty[i].Partyname, DtDistributor.Rows[0]["PartyName"].ToString(), distinctParty[i].Address, distinctParty[i].Address1, Convert.ToInt32(cityid), Convert.ToInt32(DtDistributor.Rows[0]["Areaid"].ToString()), beatid, Convert.ToInt32(distinctParty[i].Distributor), distinctParty[i].Pincode, distinctParty[i].Mobile, distinctParty[i].Phone, "", distinctParty[i].Distributor + "#" + distinctParty[i].Partyid, true, "", 0, distinctParty[i].Contact, "", "", "", distinctParty[i].IncomeTaxNumber == "" ? "" : distinctParty[i].IncomeTaxNumber, distinctParty[i].PartyCreditlimit == "" ? 0 : Convert.ToDecimal(distinctParty[i].PartyCreditlimit), 0, Convert.ToInt32(DTadmin.Rows[0]["Userid"]), "", "", distinctParty[i].Email, "", distinctParty[i].GSTIN, Convert.ToInt32(DTadmin.Rows[0]["SMid"].ToString()), distinctParty[i].GSTIN, distinctParty[i].Companynumber,
                    //               distinctParty[i].CountryName, distinctParty[i].Statename, countryid, regionid, stateid, distictid, citytypeid, cityconveyancetype, Convert.ToInt32(distinctParty[i].Distributor));


                    //              if (retval > 0)
                    //              {
                    //                  ms.msdmsg = distinctParty[i].Partyname + " is updated successfully";

                    //                  msglist.Add(ms);
                    //                  success = success + 1;

                    //              }
                    //              else
                    //              {
                    //                  ms.msdmsg = distinctParty[i].Partyname + " is not updated,Something went wrong";
                    //                  msglist.Add(ms);
                    //                  failure = failure + 1;
                    //                  // LogError(ds.Tables[0].Rows[i]["PARTYNAME"].ToString() + " is not inserted,Something went wrong");
                    //              }
                    //              //ms.msdmsg = ds.Tables[0].Rows[i]["PARTYNAME"].ToString() + " is not inserted,Sync id is duplicate";
                    //              //msglist.Add(ms);
                    //              //failure = failure + 1;
                    //              //LogError(ds.Tables[0].Rows[i]["PARTYNAME"].ToString() + "  is not inserted,Sync id is duplicate");
                    //          }
                    //          else
                    //          {
                    //              ms.msdmsg = distinctParty[i].Partyname + " is not inserted,Something went wrong";
                    //              msglist.Add(ms);
                    //              failure = failure + 1;
                    //              // LogError(ds.Tables[0].Rows[i]["PARTYNAME"].ToString() + " is not inserted,Something went wrong");
                    //          }

                    //      }
                    //      else
                    //      {
                    //          _sql = "Update MastParty Set SyncId='" + Convert.ToInt32(distinctParty[i].Distributor) + "#" + distinctParty[i].Partyid + "' where PartyId=" + distinctParty[i].GrahaakID + "";
                    //          int updateid = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, _sql));
                    //      }
                    //  }



                    //////////////////////Get PartyData////////////////
                    itemid1 = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, "SELECT Itemid from MastItem where ItemName='Other Item' and  itemtype='ITEM' "));
                    DbConnectionDAL.ExecuteNonQuery(CommandType.Text, "Delete from TransRetailerInv where Compcode='" + ds.Tables["ITEMDETAIL"].Rows[0]["COMPANYNUMBER"].ToString() + "'");
                    DbConnectionDAL.ExecuteNonQuery(CommandType.Text, "Delete from TransRetailerInv1 where Compcode='" + ds.Tables["ITEMDETAIL"].Rows[0]["COMPANYNUMBER"].ToString() + "'");
                    DbConnectionDAL.ExecuteNonQuery(CommandType.Text, "Delete from TransRetailerInv2 where Compcode='" + ds.Tables["ITEMDETAIL"].Rows[0]["COMPANYNUMBER"].ToString() + "'");
                    var distinctbill = ds.Tables["ITEMDETAIL"].AsEnumerable().GroupBy(s => s.Field<string>("BILLNO")).Select(s => new
                    {
                        BillNo = s.Key,
                        VDATE = s.Max(x => x.Field<string>("DATE")),
                        BillAmount = s.Max(x => x.Field<string>("TOTALAMOUNT")),
                        Masterid = s.Max(x => x.Field<string>("MASTER")),
                        Partyid = s.Max(x => x.Field<string>("PARTYMASTERID")),
                        Distributor = s.Max(x => x.Field<string>("DistID")),
                        Partyname = s.Max(x => x.Field<string>("PARTY")),
                        Companynumber = s.Max(x => x.Field<string>("COMPANYNUMBER"))

                    }).ToList();
                    if (distinctbill.Count > 0)
                    {
                        str = "Total Bill is " + distinctbill.Count + ".<br/>";
                    }
                    for (int i = 0; i < distinctbill.Count; i++)
                    {
                        var IsGrahaakItemexist = ds.Tables["ITEMDETAIL"].AsEnumerable().Where(x => x.Field<string>("BILLNO") == distinctbill[i].BillNo && x.Field<string>("GRAHAAKCODE") != "").ToList();
                        ms = new mandatorymsg();
                        if (IsGrahaakItemexist.Count > 0)
                        {



                            DistInvDocId = "TLINV" + " " + Convert.ToString(Convert.ToDateTime(distinctbill[i].VDATE).Year) + " " + distinctbill[i].BillNo;
                            if (ds.Tables.Count > 3 && ds.Tables["LEDGERDETAIL"].AsEnumerable().Where(s => s.Field<string>("BILLNO") == distinctbill[i].BillNo) != null)
                            {
                                taxstr = ds.Tables["LEDGERDETAIL"].AsEnumerable().Where(s => s.Field<string>("BILLNO") == distinctbill[i].BillNo && s.Field<string>("LEDGERNAME") != "Round Off" && s.Field<string>("LEDGERNAME") != "DISCOUNT" && s.Field<string>("PARENTTAX") != "Sales Accounts").Select(f => f.Field<string>("LEDGERAMOUNT")).FirstOrDefault();
                                if (taxstr != null && taxstr != "")
                                    taxamt = ds.Tables["LEDGERDETAIL"].AsEnumerable().Where(s => s.Field<string>("BILLNO") == distinctbill[i].BillNo && s.Field<string>("LEDGERNAME") != "Round Off" && s.Field<string>("LEDGERNAME") != "DISCOUNT" && s.Field<string>("PARENTTAX") != "Sales Accounts").Sum(f => Convert.ToDecimal(f.Field<string>("LEDGERAMOUNT")));



                                roff = ds.Tables["LEDGERDETAIL"].AsEnumerable().Where(s => s.Field<string>("BILLNO") == distinctbill[i].BillNo && s.Field<string>("LEDGERNAME") == "Round Off").Select(f => f.Field<string>("LEDGERAMOUNT")).FirstOrDefault();

                                if (roff != null && roff != "")
                                    roundoff = roff.ToString().IndexOf(")") > 0 ? -Convert.ToDecimal(roff.ToString().Substring(roff.ToString().IndexOf(")") + 1, roff.ToString().Length - 1 - roff.ToString().IndexOf(")"))) : Convert.ToDecimal(roff.ToString());
                                dis = ds.Tables["LEDGERDETAIL"].AsEnumerable().Where(s => s.Field<string>("BILLNO") == distinctbill[i].BillNo && s.Field<string>("LEDGERNAME") == "DISCOUNT").Select(f => f.Field<string>("LEDGERAMOUNT")).FirstOrDefault();
                                if (dis != null && dis != "")
                                    discount = dis.ToString().IndexOf(")") > 0 ? -Convert.ToDecimal(dis.ToString().Substring(dis.ToString().IndexOf(")") + 1, dis.ToString().Length - 1 - dis.ToString().IndexOf(")"))) : Convert.ToDecimal(dis.ToString());
                            }
                            DistInvId = PB.insertRetailersaleinvheader_Tally(DistInvDocId, distinctbill[i].VDATE, taxamt, (Convert.ToDecimal(distinctbill[i].BillAmount.IndexOf(")") > 0 ? distinctbill[i].BillAmount.Substring(distinctbill[i].BillAmount.IndexOf(")") + 1, distinctbill[i].BillAmount.Length - 1 - distinctbill[i].BillAmount.ToString().IndexOf(")")) : distinctbill[i].BillAmount)), roundoff, discount, distinctbill[i].Distributor + "#" + distinctbill[i].Partyid, distinctbill[i].Companynumber);
                            if (DistInvId == -2)
                            {
                                ms.msdmsg = "Party -" + distinctbill[i].Partyname + " not exist on web portal for Bill no " + distinctbill[i].BillNo;
                                msglist.Add(ms);
                                failure = failure + 1;

                            }
                            else
                            {

                                if (ds.Tables["ITEMDETAIL"].AsEnumerable().Where(x => x.Field<string>("BILLNO") == distinctbill[i].BillNo) != null)
                                {


                                    var getproducts = ds.Tables["ITEMDETAIL"].AsEnumerable().Where(x => x.Field<string>("BILLNO") == distinctbill[i].BillNo).Select(s => new
                                    {
                                        ITEM = s.Field<string>("ITEM"),
                                        QTY = s.Field<string>("QTY").ToString() != "" ? s.Field<string>("QTY").ToString().Substring(0, s.Field<string>("QTY").ToString().IndexOf(" ")) : "0",
                                        BILLNO = s.Field<string>("BILLNO").ToString(),
                                        AMOUNT = s.Field<string>("AMOUNT").ToString() == "" ? 0 : s.Field<string>("AMOUNT").ToString().IndexOf(")") > 0 ? -Convert.ToDecimal(s.Field<string>("AMOUNT").ToString().Substring(s.Field<string>("AMOUNT").ToString().IndexOf(")") + 1, s.Field<string>("AMOUNT").ToString().Length - 1 - s.Field<string>("AMOUNT").ToString().IndexOf(")"))) : Convert.ToDecimal(s.Field<string>("AMOUNT").ToString()),
                                        DATE = s.Field<string>("DATE").ToString(),
                                        PARTY = s.Field<string>("PARTY").ToString(),
                                        MASTER = s.Field<string>("MASTER").ToString(),
                                        RATE = s.Field<string>("RATE").ToString() != "" ? s.Field<string>("RATE").ToString().Substring(0, s.Field<string>("RATE").ToString().IndexOf("/")) : "0",
                                        PARTYID = s.Field<string>("PARTYMASTERID").ToString(),
                                        ITEMMASTERID = s.Field<string>("ITEMMASTERID").ToString(),
                                        GRAHAAKCODE = s.Field<string>("GRAHAAKCODE").ToString(),
                                        Unit = s.Field<string>("ITEMUNIT").ToString()

                                    }).ToList();
                                    for (int j = 0; j < getproducts.Count; j++)
                                    {
                                        if (getproducts[j].ITEM != "")
                                        {
                                            if (getproducts[j].GRAHAAKCODE != "")
                                            {
                                                itemid = Convert.ToInt32(getproducts[j].GRAHAAKCODE);
                                            }
                                            else
                                            {
                                                itemid = itemid1;
                                            }
                                            sno = sno + 1;
                                            DistInv1Id = PB.insertRetailersaleinvdetail_Tally(DistInvId, sno, DistInvDocId, getproducts[j].DATE, 0, Convert.ToDecimal(getproducts[j].QTY.IndexOf(")") > 0 ? getproducts[j].QTY.Substring(getproducts[j].QTY.IndexOf(")") + 1, getproducts[j].QTY.Length - 1 - getproducts[j].QTY.ToString().IndexOf(")")) : getproducts[j].QTY), Convert.ToDecimal(getproducts[j].RATE), Convert.ToDecimal(getproducts[j].AMOUNT), distinctbill[i].Distributor + "#" + distinctbill[i].Partyid, getproducts[j].ITEM, itemid, getproducts[j].Unit, distinctbill[i].Companynumber);
                                            if (DistInv1Id > 0)
                                            {


                                            }
                                        }
                                    }
                                    success = success + 1;
                                }
                                //// all charges including tax amount  and rounding off 

                                if (ds.Tables.Count > 3 && ds.Tables["LEDGERDETAIL"].AsEnumerable().Where(s => s.Field<string>("BILLNO") == distinctbill[i].BillNo) != null)
                                {
                                    var othertaxesofbill = ds.Tables["LEDGERDETAIL"].AsEnumerable().Where(s => s.Field<string>("BILLNO") == distinctbill[i].BillNo && s.Field<string>("PARENTTAX") != "Sales Accounts").Select(z => new
                                    {

                                        Description = z.Field<string>("TAXTYPE") == "Central Tax" ? "CGST" : z.Field<string>("TAXTYPE") == "State Tax" ? "SGST" : z.Field<string>("TAXTYPE") == "Integrated Tax" ? "IGST" : z.Field<string>("LEDGERNAME"),
                                        Amount = z.Field<string>("LEDGERAMOUNT").IndexOf(")") > 0 ? -Convert.ToDecimal(z.Field<string>("LEDGERAMOUNT").ToString().Substring(z.Field<string>("LEDGERAMOUNT").ToString().IndexOf(")") + 1, z.Field<string>("LEDGERAMOUNT").ToString().Length - 1 - z.Field<string>("LEDGERAMOUNT").ToString().IndexOf(")"))) : Convert.ToDecimal(z.Field<string>("LEDGERAMOUNT").ToString())

                                    }).ToList();


                                    for (int j = 0; j < othertaxesofbill.Count; j++)
                                    {
                                        DistInv2Id = PB.insertsaleinvexpensedetail_Tally(DistInvDocId, othertaxesofbill[j].Description, Convert.ToDecimal(othertaxesofbill[j].Amount), distinctbill[i].Companynumber);
                                    }

                                }

                                ////


                            }
                            taxamt = 0;
                            roundoff = 0;
                            discount = 0;
                            sno = 0;
                            _sql = "Update TallyCounter set LastSynID=" + distinctbill[i].Masterid + ",LastDate='" + DateTime.Now.ToString("dd/MMM/yyyy") + "' where Type='S' and compcode='" + distinctbill[i].Companynumber + "'";
                            DistInv1Id = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, _sql));
                        }
                        else
                        {
                            ms.msdmsg = "No Item exist on web portal for Bill no " + distinctbill[i].BillNo;
                            msglist.Add(ms);
                            failure = failure + 1;

                        }
                    }
                    str += success + " record inserted successfully <br/>";


                    if (msglist.Count == 0)
                    {
                        mandatorymsg ms1 = new mandatorymsg();
                        ms1.msdmsg = "No error";
                        msglist.Add(ms1);
                    }


                    if (failure > 0)
                    {
                        str = str + "," + failure + " record are failed.Check log ";
                    }
                }
            }

            catch (Exception ex)
            {
                rs.msg = zi.ToString();
            }
            if (msglist.Count == 0)
            {
                mandatorymsg ms1 = new mandatorymsg();
                ms1.msdmsg = "No error";
                msglist.Add(ms1);
            }
            rs.msg = str;
            rs.errormsg = msglist;
            if (str != "")
            {
                SendEmail("Regarding Log Detail of Sale Invoice Transfer from tally", rs, str);
            }
            Context.Response.Write(JsonConvert.SerializeObject(rs));
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void insertdistStockfromtally()
        {
            errorresult rs = new errorresult();
            string str = "";
            string _qry = "";
            int success = 0, failure = 0;
            List<mandatorymsg> msglist = new List<mandatorymsg>();
            try
            {
                var bodyStream = new StreamReader(HttpContext.Current.Request.InputStream);
                DataTable DTadmin = new DataTable();
                bodyStream.BaseStream.Seek(0, SeekOrigin.Begin);
                var bodyText = bodyStream.ReadToEnd();
                StringReader theReader = new StringReader(bodyText);
                DataSet ds = new DataSet();
                ds.ReadXml(theReader);
                DataTable DtDistributor = new DataTable();
                if (ds.Tables.Count > 0)
                {
                    DtDistributor = DbConnectionDAL.GetDataTable(CommandType.Text, "Select PartyName,MP.Areaid AS Areaid,cityid,MD.AreaId AS Distinctid,MS.AreaId AS Stateid,MR.AreaId AS Regionid,MP.UserId As  UserId from MastParty  MP LEFT JOIN MastArea MC ON MC.AreaId=MP.CityId LEFT JOIN MastArea MD ON MD.AreaId=MC.UnderId LEFT JOIN MastArea MS ON MS.AreaId=MD.UnderId LEFT JOIN MastArea MR ON MR.AreaId=MS.UnderId  where PartyID=" + ds.Tables[0].Rows[0]["DistID"].ToString() + "");

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        str = "Total number of records " + ds.Tables[0].Rows.Count + ".<br/>";
                    }
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        mandatorymsg ms = new mandatorymsg();
                        if (ds.Tables[0].Rows[i]["CLOSINGBALANCE"].ToString() != "")
                        {

                            string docID = Settings.GetDocID("DIS", DateTime.Now);
                            Settings.SetDocID("DIS", docID);
                            string[] closing = ds.Tables[0].Rows[i]["CLOSINGBALANCE"].ToString().Split(' ');
                            string str2 = closing[0].Replace("(", "").Replace(")", "");
                            int cnt = 0;
                            cnt = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, "Select count(*) From TransDistStock where DistId=" + ds.Tables[0].Rows[i]["DistID"].ToString() + " and ItemId=" + ds.Tables[0].Rows[i]["ITEMCODE"].ToString() + " and VDate='" + ds.Tables[0].Rows[i]["SYSTEMDATE"].ToString() + "'"));
                            if (cnt == 0)
                            {


                                _qry = "INSERT INTO dbo.TransDistStock (VisId, STKDocId, UserId, VDate, SMId, DistId, DistCode, AreaId, ItemId, Qty, Created_date,TallyImportYN,Tallyunit) VALUES (0, '" + docID + "', " + DtDistributor.Rows[0]["UserId"].ToString() + ", '" + ds.Tables[0].Rows[i]["SYSTEMDATE"].ToString() + "', 0," + ds.Tables[0].Rows[0]["DistID"].ToString() + "," + ds.Tables[0].Rows[0]["ITEMCODE"].ToString() + ", " + DtDistributor.Rows[0]["Areaid"].ToString() + ", " + ds.Tables[0].Rows[i]["ITEMCODE"].ToString() + ",  " + Convert.ToDecimal(closing[0].Replace("(", "").Replace(")", "")) + ",DateAdd(minute,330,getutcdate()),'Y','" + ds.Tables[0].Rows[i]["Unit"].ToString() + "')";

                                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, _qry);
                                ms.msdmsg = "Distributor stock for Distributor " + DtDistributor.Rows[0]["PartyName"].ToString() + " and item ID " + ds.Tables[0].Rows[i]["ITEMCODE"].ToString() + " inserted successfully ";
                                msglist.Add(ms);
                                success = success + 1;
                            }
                            else
                            {
                                ms.msdmsg = "Stock Transfer only occurs once in a day";
                                msglist.Add(ms);
                                failure = failure + 1;
                            }

                        }
                        else
                        {
                            ms.msdmsg = "Closing Balance is zero for item " + ds.Tables[0].Rows[i]["ITEMCODE"].ToString();
                            msglist.Add(ms);
                            failure = failure + 1;
                        }
                        // success = success + 1;
                    }

                }
                str += success + " record inserted successfully <br/>";
                if (failure > 0)
                {
                    str += "," + failure + " record Failed ";
                }
                if (msglist.Count == 0)
                {
                    mandatorymsg ms = new mandatorymsg();
                    ms.msdmsg = "No error";
                    msglist.Add(ms);
                }

                rs.msg = str;
                rs.errormsg = msglist;

            }


            catch (Exception ex)
            {

                str = ex.Message;

                //if (err > 0)
                //    str = str + ",Check log ";
                rs.msg = str;
                rs.errormsg = msglist;

            }
            if (str != "")
            {

                SendEmail("Regarding Log Detail of Stock Transfer from tally", rs, str);
            }
            Context.Response.Write(JsonConvert.SerializeObject(rs));



        }

        public void SendEmail(string subject, errorresult rs, string header)
        {
            string inserTomail = ""; string insertSubject = ""; string _subject = "";
            string ToMail = "", body, remark = "", compurl = "", srName = "";
            string str = ""; string Message = "";

            string defaultmailId = ""; string defaultpassword = ""; string host = ""; string port = "";
            string retval = "";


            try
            {
                str = "select SenderEmailID,SenderPassword,Port,MailServer,CompUrl from [MastEnviro]";
                DataTable checkemaildt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                if (checkemaildt.Rows.Count > 0)
                {
                    defaultmailId = checkemaildt.Rows[0]["SenderEmailID"].ToString();
                    defaultpassword = checkemaildt.Rows[0]["SenderPassword"].ToString();
                    host = checkemaildt.Rows[0]["MailServer"].ToString();
                    port = checkemaildt.Rows[0]["Port"].ToString();
                    compurl = checkemaildt.Rows[0]["CompUrl"].ToString();
                }

                ToMail = "support@grahaak.com";


                body = "This is a system generated mail, do not need reply on this.";

                body += @"<br /><span style='font-size:14px;'>  " + header + " </span><br />";

                body += "<span style='font-weight: bold;font-size:14px;'>--------Log Details---------</span> <br/>";
                for (int i = 0; i < rs.errormsg.Count; i++)
                {
                    body += "<span style='font-size:12px;'>" + rs.errormsg[i].msdmsg + "</span> <br/>";
                }

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(defaultmailId);
                mail.Subject = subject;
                mail.Body = body;
                mail.To.Add(new MailAddress(ToMail));
                //    mail.CC.Add(new MailAddress(CcMail));

                NetworkCredential mailAuthenticaion = new NetworkCredential(Convert.ToString(checkemaildt.Rows[0]["SenderEmailId"]), Convert.ToString(checkemaildt.Rows[0]["SenderPassword"]));

                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                SmtpClient mailclient = new SmtpClient(Convert.ToString(checkemaildt.Rows[0]["MailServer"]), Convert.ToInt32(checkemaildt.Rows[0]["Port"]));

                mailclient.EnableSsl = true;
                mailclient.UseDefaultCredentials = false;
                mailclient.Credentials = mailAuthenticaion;
                mail.IsBodyHtml = true;
                mailclient.Send(mail);

                Message = "-";

                _subject = "Mail Successfully Send for:" + subject + " at " + DateTime.Now + "";
                str = "Insert into [CRM_EmailDataStatus]([Subject],[ErrorMessage],[EmailId],[EmailStatus],[UserId],[VDate] ,ToEmail, FromEmail) Values('" + _subject + "','" + Message + "','" + ToMail + "','Success',1,getdate(),'" + ToMail + "','" + defaultmailId + "')";
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);


            }
            catch (Exception ex)
            {
                _subject = "Mail Failure for:" + subject + " at " + DateTime.Now + "";
                str = "Insert into  [CRM_EmailDataStatus]([Subject],[ErrorMessage],[EmailId],[EmailStatus],[UserId],[VDate], ToEmail, FromEmail) Values('" + _subject + "','" + ex.ToString() + "','" + inserTomail + "','Failure',1,getdate(),'" + ToMail + "','" + defaultmailId + "')";
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
                // throw ex;

            }
        }


        # endregion


    }
}
