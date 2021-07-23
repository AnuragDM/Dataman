using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using BusinessLayer;
using Newtonsoft.Json;
using System.Data;
using System.Web.Script.Serialization;

namespace AstralFFMS
{
    /// <summary>
    /// Summary description for upload
    /// </summary>
    public class upload : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string FileNamePath = "";
            string FileNamePath1 = String.Empty;   // Set it to Empty Instead of  ""        
            context.Response.ContentType = "text/json";  
            if (context.Request.Files.Count > 0)
            {
                HttpFileCollection UploadedFilesCollection = context.Request.Files;
                string SMID = context.Request.Params["SMID"].ToString();
                for (int i = 0; i < UploadedFilesCollection.Count; i++)
                {
                    System.Threading.Thread.Sleep(2000);
                    HttpPostedFile PostedFiles = UploadedFilesCollection[i];
                    string filename = PostedFiles.FileName;
                    String timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    filename = SMID + '-' + timeStamp + '-' + filename;
                    string FilePath = context.Server.MapPath("~/CRM_UploadFile/" + System.IO.Path.GetFileName(filename));
                   // FileNamePath += FilePath + ",";
                    FileNamePath += "/CRM_UploadFile/" + System.IO.Path.GetFileName(filename) + ",";
                    PostedFiles.SaveAs(FilePath);
                   
                }
                FileNamePath = FileNamePath.TrimEnd(',');
        JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
        FileNamePath1 = javaScriptSerializer.Serialize(FileNamePath);
                  //  context.Response.ContentType = "text/html";
                  //  context.Response.Write(FileNamePath1);
                  // outputToReturn = String.Format("{ \"FilePath\" : \"{0}\"  }",FileNamePath); 
            }
            context.Response.Write(FileNamePath1); 
        }
      
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}