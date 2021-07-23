using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLayer;
using DAL;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;

namespace AstralFFMS
{
    public partial class new_form : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            toTextBox1.Attributes.Add("readonly", "readonly");
            toTextBox.Attributes.Add("readonly", "readonly");
            if (!Page.IsPostBack)
            {
                toTextBox.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");
                toTextBox1.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");
            }
        }
    }
}