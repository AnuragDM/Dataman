using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace AstralFFMS
{
    /// <summary>
    /// Summary description for UploadHandler
    /// </summary>
    public class UploadHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            if (context.Request.Files.Count > 0)
            {
                HttpFileCollection files = context.Request.Files;
                for (int i = 0; i < files.Count; i++)
                {
                    HttpPostedFile file = files[i];
                    string fname = context.Server.MapPath("~/CRM_UploadFile/" + file.FileName);
                    file.SaveAs(fname);
                }
                context.Response.ContentType = "text/plain";
                context.Response.Write("File Uploaded Successfully!");
            }

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