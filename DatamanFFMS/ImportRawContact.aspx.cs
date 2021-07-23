using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using FFMS.ClassFiles;
using DAL;
using BAL;
using BusinessLayer;

namespace AstralFFMS
{
    public partial class ImportRawContact : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnExport_Click(object sender, EventArgs e)
        {
            DataTable dtnew = new DataTable();
           string columnname="SELECT column_name FROM information_schema.columns WHERE table_name = 'CRM_MastRawContact'";
            DataTable dt=DAL.DbConnectionDAL.GetDataTable(CommandType.Text,columnname);
            if(dt.Rows.Count>0)
            {
                for(int i=1;i<dt.Rows.Count;i++)
                {
                    dtnew.Columns.Add(dt.Rows[i]["column_name"].ToString());
                }
                //dtnew.Columns.Remove("column_name");
                dtnew.AcceptChanges();
            }
            columnname = "SELECT column_name FROM information_schema.columns WHERE table_name = 'crm_mastcompany' and column_name<>'Address' and column_name<>'Phone' and column_name<>'city' and column_name<>'state' and column_name<>'Country' and column_name<>'zip' ";
             dt = DAL.DbConnectionDAL.GetDataTable(CommandType.Text, columnname);
            if (dt.Rows.Count > 0)
            {
              
              
                for (int i = 1; i < dt.Rows.Count; i++)
                {
                   
                    dtnew.Columns.Add(dt.Rows[i]["column_name"].ToString());
                }
              //dtnew.Columns.Remove("column_name");
                dtnew.Columns.Add("CompAddress");
                dtnew.Columns.Add("CompPhone");
                dtnew.Columns.Add("CompCity");
                dtnew.Columns.Add("CompState");
                dtnew.Columns.Add("CompCountry");
                dtnew.Columns.Add("CompZip");
                dtnew.AcceptChanges();
            }
            columnname = "SELECT column_name FROM information_schema.columns WHERE table_name = 'crm_contactemail' and column_name<>'RawContactId' ";
             dt = DAL.DbConnectionDAL.GetDataTable(CommandType.Text, columnname);
            if (dt.Rows.Count > 0)
            {
       
                for (int i = 1; i < dt.Rows.Count; i++)
                {
                    dtnew.Columns.Add(dt.Rows[i]["column_name"].ToString());
                }
               // dtnew.Columns.Remove("column_name");
                dtnew.AcceptChanges();
            }
            columnname = "SELECT column_name FROM information_schema.columns WHERE table_name = 'crm_contactmobile' and column_name<>'RawContactId' and column_name<>'Contactname ' and column_name<>'Contact_id'";
             dt = DAL.DbConnectionDAL.GetDataTable(CommandType.Text, columnname);
            if (dt.Rows.Count > 0)
            {
               
                for (int i = 1; i < dt.Rows.Count; i++)
                {
                    dtnew.Columns.Add(dt.Rows[i]["column_name"].ToString());
                }
               // dtnew.Columns.Remove("column_name");
                dtnew.AcceptChanges();
            }
            columnname = "SELECT column_name FROM information_schema.columns WHERE table_name = 'crm_contacturl' and column_name<>'RawContactId' and column_name<>'Contactname' and column_name<>'Contact_id'";
            dt = DAL.DbConnectionDAL.GetDataTable(CommandType.Text, columnname);
            if (dt.Rows.Count > 0)
            {
             
                for (int i = 1; i < dt.Rows.Count; i++)
                {
                    dtnew.Columns.Add(dt.Rows[i]["column_name"].ToString());
                }
               // dtnew.Columns.Remove("column_name");
                dtnew.AcceptChanges();
            }
                string csv = string.Empty;

                foreach (DataColumn column in dtnew.Columns)
                {
                    //Add the Header row for CSV file.
                    csv += column.ColumnName + ',';
                }

                //Add new line.
                csv += "\r\n";

                foreach (DataRow row in dtnew.Rows)
                {
                    foreach (DataColumn column in dtnew.Columns)
                    {
                        //Add the Data rows.
                        csv += row[column.ColumnName].ToString().Replace(",", ";") + ',';
                    }

                    //Add new line.
                    csv += "\r\n";
                }

                //Download the CSV file.
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=ImportRawContact.csv");
                Response.Charset = "";
                Response.ContentType = "application/text";
                Response.Output.Write(csv);
                Response.Flush();
                Response.End();
            }


        protected void btnUpload_Click(object sender, EventArgs e)
        {

            if (Fupd.HasFile)
            {
                lblcols.InnerText = "";

                try
                {
                    String ext = System.IO.Path.GetExtension(Fupd.FileName);
                    if (ext.ToLower() != ".csv")
                    {
                        ShowAlert("Please Upload Only .csv File.");
                        return;
                    }
                    string filename = Path.GetFileName(Fupd.FileName);
                    String csv_file_path = Path.Combine(Server.MapPath("~/FileUploads"), filename);
                    Fupd.SaveAs(csv_file_path);
                    GetDataTableFromCsv(csv_file_path);
                }
                catch (Exception ex)
                {
                    ShowAlert("ERROR: " + ex.Message.ToString());
                }
            }
            else
            {
                ShowAlert("You have not specified a file.");
            }
        }
        public void ShowAlert(string Message)
        {
            string script = "window.alert(\"" + Message.Normalize() + "\");";
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "UniqueKey", script, true);
        }
        private void GetDataTableFromCsv(string path)
        {
            DataTable dt = new DataTable();
            string CSVFilePathName = path;
            string[] Lines = File.ReadAllLines(CSVFilePathName);
            string[] Fields;
            Fields = (Lines[0].TrimEnd(',')).Split(new char[] { ',' });
            int Cols = Fields.GetLength(0);

            //1st row must be column names; force lower case to ensure matching later on.
            for (int i = 0; i < Cols; i++)
                dt.Columns.Add(Fields[i].ToLower().Trim(), typeof(string));
            dt.AcceptChanges();
            if (GetValidCols(dt))
            {
                DataRow Row;
                for (int i = 2; i < Lines.GetLength(0); i++)
                {
                    Fields = Lines[i].Split(new char[] { ',' });
                    Row = dt.NewRow();
                    for (int f = 0; f < Cols; f++)
                        Row[f] = Fields[f].Trim();
                    dt.Rows.Add(Row);
                }
                dt.AcceptChanges();
                InsertProductClass(dt);
                //InsertItems(dt);
            }
            else
            {
                lblcols.Visible = true;
                ShowAlert("Incorrect Column Names.Please Check Uploaded File.");
            }

        }
        private Boolean GetValidCols(DataTable dtcols)
        {
            bool Valid = true; DataTable dtnew = new DataTable();
            lblcols.InnerText = "Required columns format :";
            string spaces = ". * Please discard blank spaces after/before columns names";
         //   string columnname = "SELECT column_name FROM information_schema.columns WHERE table_name = 'CRM_MastRawContact'";
           // DataTable dt = DAL.DbConnectionDAL.GetDataTable(CommandType.Text, columnname);
            //for (int i =1 ; i < dt.Rows.Count; i++)
            //{
            //    if(dtcols.Columns[i-1].ToString()!=dt.Rows[i]["column_name"].ToString().ToLower())
            //    {
            //        lblcols.InnerText += dtcols.Columns[i].ToString();
            //        Valid = false;
            //    }

            //}

            string columnname = "SELECT column_name FROM information_schema.columns WHERE table_name = 'CRM_MastRawContact'";
            DataTable dt = DAL.DbConnectionDAL.GetDataTable(CommandType.Text, columnname);
            if (dt.Rows.Count > 0)
            {
                for (int i = 1; i < dt.Rows.Count; i++)
                {
                    dtnew.Columns.Add(dt.Rows[i]["column_name"].ToString());
                }
                //dtnew.Columns.Remove("column_name");
                dtnew.AcceptChanges();
            }

            columnname = "SELECT column_name FROM information_schema.columns WHERE table_name = 'crm_mastcompany' and column_name<>'Address' and column_name<>'Phone' and column_name<>'city' and column_name<>'state' and column_name<>'Country' and column_name<>'zip' ";
            dt = DAL.DbConnectionDAL.GetDataTable(CommandType.Text, columnname);
            if (dt.Rows.Count > 0)
            {


                for (int i = 1; i < dt.Rows.Count; i++)
                {

                    dtnew.Columns.Add(dt.Rows[i]["column_name"].ToString());
                }
                //dtnew.Columns.Remove("column_name");
                dtnew.Columns.Add("CompAddress");
                dtnew.Columns.Add("CompPhone");
                dtnew.Columns.Add("CompCity");
                dtnew.Columns.Add("CompState");
                dtnew.Columns.Add("CompCountry");
                dtnew.Columns.Add("CompZip");
                dtnew.AcceptChanges();
            }
            columnname = "SELECT column_name FROM information_schema.columns WHERE table_name = 'crm_contactemail' and column_name<>'RawContactId' ";
            dt = DAL.DbConnectionDAL.GetDataTable(CommandType.Text, columnname);
            if (dt.Rows.Count > 0)
            {

                for (int i = 1; i < dt.Rows.Count; i++)
                {
                    dtnew.Columns.Add(dt.Rows[i]["column_name"].ToString());
                }
                // dtnew.Columns.Remove("column_name");
                dtnew.AcceptChanges();
            }
            columnname = "SELECT column_name FROM information_schema.columns WHERE table_name = 'crm_contactmobile' and column_name<>'RawContactId' and column_name<>'Contactname ' and column_name<>'Contact_id'";
            dt = DAL.DbConnectionDAL.GetDataTable(CommandType.Text, columnname);
            if (dt.Rows.Count > 0)
            {

                for (int i = 1; i < dt.Rows.Count; i++)
                {
                    dtnew.Columns.Add(dt.Rows[i]["column_name"].ToString());
                }
                // dtnew.Columns.Remove("column_name");
                dtnew.AcceptChanges();
            }
            columnname = "SELECT column_name FROM information_schema.columns WHERE table_name = 'crm_contacturl' and column_name<>'RawContactId' and column_name<>'Contactname' and column_name<>'Contact_id'";
            dt = DAL.DbConnectionDAL.GetDataTable(CommandType.Text, columnname);
            if (dt.Rows.Count > 0)
            {

                for (int i = 1; i < dt.Rows.Count; i++)
                {
                    dtnew.Columns.Add(dt.Rows[i]["column_name"].ToString());
                }
                // dtnew.Columns.Remove("column_name");
                dtnew.AcceptChanges();
            }
            for (int i = 1; i < dtnew.Columns.Count; i++)
            {
                if (dtcols.Columns[i].ToString() != dtnew.Columns[i].ToString().ToLower())
                {
                    lblcols.InnerText += dtcols.Columns[i].ToString();
                    Valid = false;
                }

            }
              
           // lblcols.InnerText += "ItemClass*,SyncCode*,Active" + spaces;

            return Valid;
        }
        private void InsertProductClass(DataTable dtrows)
        {
            if (dtrows.Rows.Count > 0)
            {
                String varname1 = ""; string valuesToSave = string.Empty; string columnfordt = string.Empty;
              
                string customfields = "  select AttributeField from crm_customfields where AttributeTable='Contact'";
                DataTable dt = DAL.DbConnectionDAL.GetDataTable(CommandType.Text, customfields);
                if (dt.Rows.Count > 0)
                {
                    customfields = "";
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        columnfordt += dt.Rows[i]["AttributeField"].ToString() + ',';
                        customfields += '[' + dt.Rows[i]["AttributeField"].ToString() + ']' + ',';
                    }
                    customfields = ',' + customfields.TrimEnd(',');
                    columnfordt = ',' + columnfordt.TrimEnd(',');
                }

                for (int k = 0; k < dtrows.Rows.Count; k++)
                {
                    string datacheck = string.Empty; string checkvalue = "";
                    datacheck = "select status_id from [dbo].[CRM_MastStatus] where status='" + dtrows.Rows[k]["status_id"].ToString().Trim() + "'";
                    checkvalue = Convert.ToString(DAL.DbConnectionDAL.GetScalarValue(CommandType.Text, datacheck));
                    if (string.IsNullOrEmpty(checkvalue))
                    {
                        ShowAlert(dtrows.Rows[k]["firstname"].ToString() + ' ' + dtrows.Rows[k]["lastname"].ToString() +" Do Not Have Valid Status");
                        continue;
                    }
                    dtrows.Rows[k]["status_id"] = checkvalue;
                    datacheck = "select Tag_Id from [dbo].[CRM_MastTag] where Tag='" + dtrows.Rows[k]["tag_id"].ToString().Trim() + "'";
                    checkvalue = Convert.ToString(DAL.DbConnectionDAL.GetScalarValue(CommandType.Text, datacheck));
                    if (string.IsNullOrEmpty(checkvalue))
                    {
                        ShowAlert(dtrows.Rows[k]["firstname"].ToString() + ' ' + dtrows.Rows[k]["lastname"].ToString() + " Do Not Have Valid Tag");
                        continue;
                    }
                    dtrows.Rows[k]["tag_id"] = checkvalue;
                    datacheck = "select Lead_Id from [dbo].[CRM_Mastleadsource] where lead='" + dtrows.Rows[k]["lead_id"].ToString().Trim() + "'";
                    checkvalue = Convert.ToString(DAL.DbConnectionDAL.GetScalarValue(CommandType.Text, datacheck));
                    if (string.IsNullOrEmpty(checkvalue))
                    {
                        ShowAlert(dtrows.Rows[k]["firstname"].ToString() + ' ' + dtrows.Rows[k]["lastname"].ToString() + " Do Not Have Valid Lead Source");
                        continue;
                    }
                    dtrows.Rows[k]["lead_id"] = checkvalue;
                    datacheck = "select AreaID from mastarea where AreaType='Country' and AreaName='" + dtrows.Rows[k]["country"].ToString().Trim() + "'";
                    checkvalue = Convert.ToString(DAL.DbConnectionDAL.GetScalarValue(CommandType.Text, datacheck));
                    if (string.IsNullOrEmpty(checkvalue))
                    {
                        ShowAlert(dtrows.Rows[k]["firstname"].ToString() + ' ' + dtrows.Rows[k]["lastname"].ToString() + " Do Not Have Valid Country");
                        continue;
                    }
                    dtrows.Rows[k]["country"] = checkvalue;
                    datacheck = "select AreaID from mastarea where AreaType='Country' and AreaName='" + dtrows.Rows[k]["CompCountry"].ToString().Trim() + "'";
                    checkvalue = Convert.ToString(DAL.DbConnectionDAL.GetScalarValue(CommandType.Text, datacheck));
                    if (string.IsNullOrEmpty(checkvalue))
                    {
                        ShowAlert(dtrows.Rows[k]["firstname"].ToString() + ' ' + dtrows.Rows[k]["lastname"].ToString() + " Do Not Have Valid Country Of Company");
                        continue;
                    }
                    dtrows.Rows[k]["CompCountry"] = checkvalue;
                    datacheck = "select smid from mastsalesrep where smname='" + dtrows.Rows[k]["ownersp"].ToString().Trim() + "'";
                    checkvalue = Convert.ToString(DAL.DbConnectionDAL.GetScalarValue(CommandType.Text, datacheck));
                    if (string.IsNullOrEmpty(checkvalue))
                    {
                        ShowAlert(dtrows.Rows[k]["firstname"].ToString() + ' ' + dtrows.Rows[k]["lastname"].ToString() + " Do Not Have Valid Owner");
                        continue;
                    }
                    dtrows.Rows[k]["ownersp"] = checkvalue;
                    dtrows.AcceptChanges();
                    varname1 = varname1 + "insert into CRM_MastCompany(CompName,Description,Phone,Address,city,state,zip,country) OUTPUT INSERTED.Comp_Id values ('" + dtrows.Rows[k]["Compname"].ToString() + "','" + dtrows.Rows[k]["Description"].ToString() + "','" + dtrows.Rows[k]["CompPhone"].ToString() + "','" + dtrows.Rows[k]["CompAddress"].ToString() + "','" + dtrows.Rows[k]["CompCity"].ToString() + "','" + dtrows.Rows[k]["CompState"].ToString() + "','" + Convert.ToInt16(dtrows.Rows[k]["CompZip"].ToString()) + "','" + dtrows.Rows[k]["CompCountry"].ToString() + "')";
                    string CompID = DAL.DbConnectionDAL.GetScalarValue(CommandType.Text, varname1).ToString();
                    string[] customfValue = (columnfordt.TrimStart(',')).Split(',');
                    valuesToSave = "";
                    for (int l = 0; l < customfValue.Length; l++)
                    {
                        string column = customfValue[l];
                        valuesToSave += "'" + dtrows.Rows[k][column].ToString() + "',";
                    }
                    valuesToSave = "," + valuesToSave.TrimEnd(',');
                    // string CompID = DAL.DbConnectionDAL.GetScalarValue(CommandType.Text, varname1).ToString();
                    String varname12 = "";
                    varname12 = varname12 + "INSERT INTO [dbo].[CRM_MastRawContact] " + "\n";
                    varname12 = varname12 + "           ([FirstName] " + "\n";
                    varname12 = varname12 + "           ,[LastName] " + "\n";
                    varname12 = varname12 + "           ,[JobTitle] " + "\n";
                    varname12 = varname12 + "           ,[Address] " + "\n";
                    varname12 = varname12 + "           ,[City] " + "\n";
                    varname12 = varname12 + "           ,[State] " + "\n";
                    varname12 = varname12 + "           ,[Country] " + "\n";
                    varname12 = varname12 + "           ,[ZipCode] " + "\n";
                    varname12 = varname12 + "           ,[Status_Id] " + "\n";
                    varname12 = varname12 + "           ,[Tag_Id] " + "\n";
                    varname12 = varname12 + "           ,[Lead_Id] " + "\n";
                    varname12 = varname12 + "           ,[OwnerSp] " + "\n";
                    varname12 = varname12 + "           ,[Manager] " + "\n";
                    varname12 = varname12 + "           ,[SmId] " + "\n";
                    varname12 = varname12 + "           ,[CreatedDate] " + "\n";
                    varname12 = varname12 + "           ,[Active] " + "\n";
                    varname12 = varname12 + "           ,[Background],[CompanyId]" + customfields + ") " + "\n OUTPUT INSERTED.Contact_Id";
                    varname12 = varname12 + "     VALUES " + "\n";
                    varname12 = varname12 + "           ('" + dtrows.Rows[k]["firstname"].ToString() + "'\n";
                    varname12 = varname12 + "           ,'" + dtrows.Rows[k]["lastname"].ToString() + "'\n";
                    varname12 = varname12 + "           ,'" + dtrows.Rows[k]["JobTitle"].ToString() + "'\n";
                    varname12 = varname12 + "            ,'" + dtrows.Rows[k]["address"].ToString() + "'\n";
                    varname12 = varname12 + "           ,'" + dtrows.Rows[k]["city"].ToString() + "'\n";
                    varname12 = varname12 + "          ,'" + dtrows.Rows[k]["state"].ToString() + "'\n";
                    varname12 = varname12 + "             ,'" + dtrows.Rows[k]["country"].ToString() + "'\n";
                    varname12 = varname12 + "            ,'" + dtrows.Rows[k]["zipcode"].ToString() + "'\n";
                    varname12 = varname12 + "            ,'" + dtrows.Rows[k]["status_id"].ToString() + "'\n";
                    varname12 = varname12 + "            ,'" + dtrows.Rows[k]["tag_id"].ToString() + "'\n";
                    varname12 = varname12 + "             ,'" + dtrows.Rows[k]["lead_id"].ToString() + "'\n";
                    varname12 = varname12 + "             ,'" + dtrows.Rows[k]["ownersp"].ToString() + "'\n";
                    varname12 = varname12 + "             ,'" + dtrows.Rows[k]["manager"].ToString() + "'\n";
                    varname12 = varname12 + "           ," + Settings.Instance.SMID + "\n";
                    varname12 = varname12 + "             ,Getdate() \n";
                    varname12 = varname12 + "  ,'" + dtrows.Rows[k]["active"].ToString() + "','" + dtrows.Rows[k]["background"].ToString() + "'," + (CompID) + valuesToSave + ")";
                    string ContactID = DAL.DbConnectionDAL.GetScalarValue(CommandType.Text, varname12).ToString();

                    ////insert into crm phone
                    //string[] arrPhnval = phnval.Split(',');
                    //string[] arrPhnddlval = phnddlval.Split(',');
                    //string[] phnctval = phncontName.Split(',');
                   // for (int i = 0; i < arrPhnval.Length; i++)
                    {
                        String strphn = "";
                        strphn = strphn + "INSERT INTO [dbo].[CRM_ContactMobile] " + "\n";
                        strphn = strphn + "           ([RawContactId] " + "\n";
                        strphn = strphn + "           ,[Phone] " + "\n";
                        strphn = strphn + "           ,[Contact_id] " + "\n";
                        strphn = strphn + "           ,[PhoneType],ContactName) " + "\n";
                        strphn = strphn + "     VALUES " + "\n";
                        strphn = strphn + "           (" + (ContactID) + "\n";
                        strphn = strphn + "          ,'" + dtrows.Rows[k]["Phone"].ToString() + "',0\n";
                        strphn = strphn + "   ,'" + dtrows.Rows[k]["phonetype"].ToString() + "','" + dtrows.Rows[k]["firstname"].ToString() + ' ' + dtrows.Rows[k]["lastname"].ToString() + "')";

                        DAL.DbConnectionDAL.ExecuteNonQuery(CommandType.Text, strphn);
                    }
                    ////insert into crm email
                    //string[] arrEmailval = emailval.Split(',');
                    //string[] arrEmailddlval = emailddlval.Split(',');
                    //string[] EmailCtArr = EmailcontName.Split(',');
                   // for (int i = 0; i < arrEmailval.Length; i++)
                    {
                        String stremail = "";
                        stremail = stremail + "INSERT INTO [dbo].[CRM_ContactEmail] " + "\n";
                        stremail = stremail + "           ([RawContactId] " + "\n";
                        stremail = stremail + "           ,[Email] " + "\n";
                        stremail = stremail + "           ,[Contact_id] " + "\n";
                        stremail = stremail + "           ,[EmailType],ContactName) " + "\n";
                        stremail = stremail + "     VALUES " + "\n";
                        stremail = stremail + "           (" + (ContactID) + "\n";
                        stremail = stremail + "          ,'" + dtrows.Rows[k]["Email"].ToString() + "',0\n";
                        stremail = stremail + "            ,'" + dtrows.Rows[k]["Emailtype"].ToString() + "','" + dtrows.Rows[k]["firstname"].ToString() + ' ' + dtrows.Rows[k]["lastname"].ToString() + "')";

                        DAL.DbConnectionDAL.ExecuteNonQuery(CommandType.Text, stremail);
                    }
                    //insert into crm url
                  //  string[] arrUrlval = urlval.Split(',');
                   // string[] arrUrlddlval = urlddlval.Split(',');
                   // for (int i = 0; i < arrUrlval.Length; i++)
                    {
                        String strurl = "";
                        strurl = strurl + "INSERT INTO [dbo].[CRM_ContactURL] " + "\n";
                        strurl = strurl + "           ([RawContactId] " + "\n";
                        strurl = strurl + "           ,[URL] " + "\n";
                        strurl = strurl + "           ,[Contact_id] " + "\n";
                        strurl = strurl + "           ,[URLType]) " + "\n";
                        strurl = strurl + "     VALUES " + "\n";
                        strurl = strurl + "           (" + (ContactID) + "\n";
                        strurl = strurl + "          ,'" + dtrows.Rows[k]["Url"].ToString() + "',0\n";
                        strurl = strurl + "            ,'" + dtrows.Rows[k]["urltype"].ToString() + "')";
                        DAL.DbConnectionDAL.ExecuteNonQuery(CommandType.Text, strurl);
                    }
                }
                ShowAlert("Contact Imported Successfully");
            }
            else
            {
                ShowAlert("There Is No Any Data To Upload");
        }
       

    }
        }
    }
