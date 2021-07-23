using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Drawing.Design;
using BusinessLayer;
using System.Globalization;

public partial class ctlCalendar : System.Web.UI.UserControl
{
    Common _common;
    protected void Page_Init(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtDate.Attributes.Add("readonly", "readonly");
            if (IsDefaultDate)
            {
                if (String.IsNullOrEmpty(CalendarText))
                    txtDate.Text = DateTime.Now.ToUniversalTime().AddSeconds(19800).ToString(ajaxCalendar.Format);
            }
            if (IsLTEQCurrent)
                txtDate.Attributes.Add("onchange", "ValidateDateLessThenEqualsToCurrentDate(this)");

            else if (IsGTEQCurrent)
                txtDate.Attributes.Add("onchange", "ValidateDateGreaterThenEqualsToCurrentDate(this)");
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
    }
    public Unit TextWidth
    {
        get
        {
            return txtDate.Width;
        }
        set
        {
            //txtDate.Width = value; 
        }
    }

    [DefaultValue("")]
    [Editor("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
    [Description("Image_ImageUrl")]
    [Bindable(true)]
    [Category("Appearance")]
    [UrlProperty]
    public string CalendarImageUrl
    {
        get { return imgDate.ImageUrl; }
        set { imgDate.ImageUrl = value; }
    }
    public string TextCssClass
    {
        get { return txtDate.CssClass; }
        set { txtDate.CssClass = value; }
    }
    public string CalendarText
    {
        get { return txtDate.Text; }
        set { txtDate.Text = value; }
    }
    public DateTime CalendarDate
    {
        get
        {
            if (String.IsNullOrEmpty(txtDate.Text))
                return new DateTime(1753, 1, 1);//1/1/1753 12:00:00 AM
            else
            {
                _common = new Common();
                return DateTime.ParseExact(txtDate.Text.Trim(), ajaxCalendar.Format, CultureInfo.InvariantCulture);
                //return _common.ConvertStringToDateTime(txtDate.Text.Trim(), ajaxCalendar.Format);
            }
        }
        set
        {
            if (!value.Equals(new DateTime(1753, 1, 1)))
                txtDate.Text = value.ToString(ajaxCalendar.Format);
        }
    }
    public string DateFormat
    {
        get { return ajaxCalendar.Format; }
        set { ajaxCalendar.Format = value; }
    }
    bool _isDefaultDate = true;
    public bool IsDefaultDate
    {
        get { return _isDefaultDate; }
        set { _isDefaultDate = value; }
    }
    public bool Enabled
    {
        get { return txtDate.Enabled; }
        set { txtDate.Enabled = value; imgDate.Enabled = value; }
    }
    public void SetDefaultDate()
    {
        txtDate.Text = DateTime.Now.ToUniversalTime().AddSeconds(19800).ToString(ajaxCalendar.Format);
    }
    public short TabIndex
    {
        get { return txtDate.TabIndex; }
        set { txtDate.TabIndex = value; }
    }
    public bool IsLTEQCurrent { get; set; }
    public bool IsGTEQCurrent { get; set; }
}
