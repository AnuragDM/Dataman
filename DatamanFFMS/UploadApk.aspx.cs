using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using BusinessLayer;
using System.Data;
using DAL;
using System.IO;
using System.Web.Services;
using System.Web.Script.Services;
using Newtonsoft.Json;

namespace AstralFFMS
{
    public partial class UploadApk : System.Web.UI.Page
    {
        BAL.Uploads.UploadBAL up = new BAL.Uploads.UploadBAL();
        string parameter = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                divdocid.Visible = false;
                fileLabel.Style.Add("display", "none");
                BindProductCode();
                fillRepeater();

            }
            parameter = Request["__EVENTARGUMENT"];
            if (parameter != "")
            {
                mainDiv.Style.Add("display", "block");
            }
            mainDiv.Style.Add("display", "block");
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            mainDiv.Style.Add("display", "block");
        }

        private int Save(string path)
        {
            int Retsave = 0;
            string insertquery = "";
            string deletequery = "";
            string strcheck = "Select * from UploadMobileApk where AppName='" + ddlAppname.SelectedValue + "'";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, strcheck);
            if (dt.Rows.Count > 0)
            {
                string filename1 = dt.Rows[0]["LinkUrl"].ToString();
                string filename = filename1.Replace("/", string.Empty);

                string filepath = Server.MapPath("~/UploadMobileAPK/" + filename.Replace("~", string.Empty).Trim());
                FileInfo file = new FileInfo(filepath);
                if (file.Exists)
                {
                    file.Delete();
                }
                deletequery = "delete from UploadMobileApk where Appname = '" + ddlAppname.SelectedValue + "'";
                Retsave = Convert.ToInt32(DbConnectionDAL.ExecuteQuery(deletequery));
            }
            insertquery = @"INSERT INTO [dbo].[UploadMobileApk] ([DocDate],[AppName],[LinkURL],[Active],[VersionName],[VersionCode]) VALUES  (DateAdd(minute,330,getutcdate()),'" + ddlAppname.SelectedValue + "','" + path + "','" + chk.Checked + "','" + txtVersionName.Text + "','" + txtversionCode.Text + "')";
            Retsave = Convert.ToInt32(DbConnectionDAL.ExecuteQuery(insertquery));

            return Retsave;
        }

        protected void btnsave_Click(object sender, EventArgs e)
        {
            #region Variable Declaration
            string str = "";
            string filePath = "", path = ""; string retval = "", ProductName = "", directoryPath="";
            DataTable dt = new DataTable();
            #endregion
            try
            {
                if (File1.PostedFile != null)
                {
                    try
                    {
                        dt =DbConnectionDAL.getFromDataTableDmLicence("select productname from ProdMaster where productcode='"+ lstProduct.SelectedValue +"'");
                        if(dt.Rows.Count==0)
                        {
                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Selected product is not found.');", true);
                            return;
                        }
                        else
                        {
                            ProductName = dt.Rows[0]["productname"].ToString();
                            if (ProductName == "")
                            {
                                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Selected product is not found.');", true);
                                return;
                            }
                        }
                       

                         directoryPath = Server.MapPath(string.Format("~/{0}/", "UploadMobileAPK"));
                        if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);

                        directoryPath = Server.MapPath(string.Format("~/{0}/", "UploadMobileAPK/" + ProductName));
                        if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);

                        directoryPath = Server.MapPath(string.Format("~/{0}/", "UploadMobileAPK/" + ProductName+"/"+ txtversionCode.Text));
                        if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);

                        filePath = Server.MapPath(string.Format("~/{0}", "UploadMobileAPK/" + ProductName + "/" + txtversionCode.Text + "/" + File1.FileName));
                        path = "~/UploadMobileAPK/" + ProductName + "/" + txtversionCode.Text + "/" + File1.FileName;

                        if (System.IO.File.Exists(Server.MapPath(path))) System.IO.File.Delete(Server.MapPath(path));
                        File1.PostedFile.SaveAs(filePath);

                        for (int i = 0; i < lstCompany.Items.Count; i++)
                        {
                            if (lstCompany.Items[i].Selected)
                            {
                                str = "select id from uploadmobileapk where VersionName='" + txtVersionName.Text + "' and VersionCode=" + txtversionCode.Text + " and  productcode='" + lstProduct.SelectedValue + "' and CompCode='" + lstCompany.Items[i].Value + "' ";
                                dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                                if(dt.Rows.Count==0)
                                {
                                    str = @"INSERT INTO [dbo].[UploadMobileApk] ([DocDate],[LinkURL],[Active],[VersionName],[VersionCode],productcode,CompCode,productname,compname) output inserted.id VALUES  (getdate(),'" + path + "','" + chk.Checked + "','" + txtVersionName.Text + "','" + txtversionCode.Text + "','" + lstProduct.SelectedValue + "','" + lstCompany.Items[i].Value + "','" + lstProduct.SelectedItem + "','" + lstCompany.Items[i].Text + "')";
                                    retval = Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, str));
                                }
                            }
                        }

                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('File Uploaded Successfully');", true);
                        ClearControls();
                        fillRepeater();
                        //int retsave = Save("~/" + File1.FileName);
                        //if (retsave > 0)
                        //{
                        //    string strDestPath = Server.MapPath("~/UploadMobileAPK/" + File1.FileName);
                        //    File1.PostedFile.SaveAs(strDestPath);

                        //    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('File Uploaded Successfully');", true);
                        //    ClearControls();
                        //}
                        //else
                        //{
                        //    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while uploading the " + File1.FileName + "');", true);
                        //}

                    }
                    catch (Exception ex)
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while uploading the " + File1.FileName + "');", true);
                        if (System.IO.File.Exists(Server.MapPath(path))) System.IO.File.Delete(Server.MapPath(path));
                    }
                }
            }

            catch (Exception ex)
            { ex.ToString(); }
        }

        private void ClearControls()
        {
            try
            {
                chk.Checked = true;
                fileLabel.Style.Add("display", "none");
                btnsave.Text = "Save";
                ddlAppname.SelectedIndex = 0;
                txtversionCode.Text = "";
                txtVersionName.Text = "";
                lstProduct.SelectedIndex = -1;
                lstCompany.DataSource = new DataTable();
                lstCompany.DataBind();

            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string getProductFromLicense()
        {
            #region Variable Declaration
            string str = "";
            DataTable dt = new DataTable();
            #endregion

            try
            {

                str = "select ProductCode,ProductName+  case when description<>'' then  ' ('+ description +')' else '' end as productName from [dbo].[ProdMaster]   where productcode in ('P7','P2','P5','P4'  ) order by Productcode";
                dt = DbConnectionDAL.getFromDataTableDmLicence(str);

            }
            catch (Exception)
            {

            }
            return JsonConvert.SerializeObject(dt);


        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string getCompanyByProductFromLicense(string ProductCode)
        {
            #region Variable Declaration
            string str = "";
            DataTable dt = new DataTable();
            #endregion

            try
            {

                str = "select cmpd.compcode,cmp.compname from compProdmaster cmpd left join compmaster cmp on cmp.compcode=cmpd.compcode  where  cmpd.productcode in (" + ProductCode + ") and cmp.compname is not null order by cmp.CompName";
                dt = DbConnectionDAL.getFromDataTableDmLicence(str);

            }
            catch (Exception)
            {

            }
            return JsonConvert.SerializeObject(dt);


        }

        private void BindProductCode()
        {
            string str = "select ProductCode,ProductName+  case when description<>'' then  ' ('+ description +')' else '' end as productName from [dbo].[ProdMaster]   where productcode in ('P7','P2','P5','P4'  ) order by Productcode";
            DataTable dt = DbConnectionDAL.getFromDataTableDmLicence(str);
            lstProduct.DataSource = dt;
            lstProduct.DataValueField = "ProductCode";
            lstProduct.DataTextField = "productName";
            lstProduct.DataBind();
        }

        protected void lstProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            #region Variable Declaration
            string str = "";
            DataTable dt = new DataTable();
           // string productCode = "";

            //foreach(ListItem item in lstProduct.Items)
            //{
            //    if(item.Selected)
            //    {
            //        productCode += "'" +item.Value +"',";
            //    }
            //}
            //productCode = productCode.TrimEnd(',');
            #endregion

            try
            {
                str = "select cmpd.compcode,cmp.compname from compProdmaster cmpd left join compmaster cmp on cmp.compcode=cmpd.compcode  where  cmpd.productcode in (" + hiddenProduct.Value + ") and cmp.compname is not null order by cmp.CompName";
                dt = DbConnectionDAL.getFromDataTableDmLicence(str);              

            }
            catch (Exception)
            {

            }
            lstCompany.DataSource = dt;
            lstCompany.DataTextField = "compname";
            lstCompany.DataValueField = "compcode";
            lstCompany.DataBind();
            
        }

        private void fillRepeater()
        {
            string str = "";
            DataTable dt = new DataTable();
            str = "select  row_number() over(order by id) Sno,DocDate,Productname +'('+ productcode+')' as productname,CompName +'('+ CompCode+')' as CompName,versionname,versionCode,case when active=1 then 'Yes' else 'No' end as Active,Case when Isnull(LinkURL,'') <>'' then  LinkURL else 'NoImage' end url from uploadmobileapk";
            dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            rptapp.DataSource = dt;
            rptapp.DataBind();
        }

        protected void rptapp_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Download")
            {
                try
                {
                    string filePath = Server.MapPath(e.CommandArgument.ToString());
                    // get file object as FileInfo
                    System.IO.FileInfo file = new System.IO.FileInfo(filePath);
                    // -- if the file exists on the server
                    if (file.Exists)
                    {
                        // set appropriate headers
                        Response.Clear();
                        Response.AddHeader("Content-Disposition", ("attachment; filename=" + file.Name.Replace("&", "").Replace(",", "")));
                        Response.AddHeader("Content-Length", file.Length.ToString());
                        Response.ContentType = "application/octet-stream";

                        Response.TransmitFile(file.FullName);

                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                    }
                    else
                    {
                        // if file does not exist
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' This file does not exist.');", true);

                    }


                }
                catch (Exception ex)
                {
                    ex.ToString();
                }

            }
        }





    }
}