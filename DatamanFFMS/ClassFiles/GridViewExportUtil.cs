using System;
using System.Data;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;

/// <summary>
/// 
/// </summary>
public class GridViewExportUtil
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="gv"></param>
    public static void Export(string fileName, GridView gv)
    {
        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.AddHeader(
            "content-disposition", string.Format("attachment; filename={0}", fileName+".xls"));
        HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";

        using (StringWriter sw = new StringWriter())
        {
            using (HtmlTextWriter htw = new HtmlTextWriter(sw))
            {
                //  Create a form to contain the grid
                Table table = new Table();
                //table.BackColor = System.Drawing.Color.Azure;

                //  add the header row to the table
                if (gv.HeaderRow != null)
                {
                    for (int i = 0; i < gv.HeaderRow.Cells.Count; i++)
                    {
                        gv.HeaderRow.Cells[i].Style.Add("background-color", "#df5015");
                    }
                   // GridViewExportUtil.PrepareControlForExport(gv.HeaderRow);
                    table.Rows.Add(gv.HeaderRow);
                }

                //  add each of the data rows to the table
                foreach (GridViewRow row in gv.Rows)
                {

                 //   GridViewExportUtil.PrepareControlForExport(row);
                    table.Rows.Add(row);
                }

                //  add the footer row to the table
                if (gv.FooterRow != null)
                {
                  //  GridViewExportUtil.PrepareControlForExport(gv.FooterRow);
                    table.Rows.Add(gv.FooterRow);
                }

                //  render the table into the htmlwriter
                table.RenderControl(htw);

                //  render the htmlwriter into the response
                HttpContext.Current.Response.Write(sw.ToString());
                HttpContext.Current.Response.End();
            }
        }
    }

    /// <summary>
    /// Replace any of the contained controls with literals
    /// </summary>
    /// <param name="control"></param>
    private static void PrepareControlForExport(Control control)
    {
        for (int i = 0; i < control.Controls.Count; i++)
        {
            Control current = control.Controls[i];
            if (current is LinkButton)
            {
                control.Controls.Remove(current);
                control.Controls.AddAt(i, new LiteralControl((current as LinkButton).Text));
            }
            else if (current is ImageButton)
            {
                control.Controls.Remove(current);
                control.Controls.AddAt(i, new LiteralControl((current as ImageButton).AlternateText));
            }
            else if (current is HyperLink)
            {
                control.Controls.Remove(current);
                control.Controls.AddAt(i, new LiteralControl((current as HyperLink).Text));
            }
            else if (current is DropDownList)
            {
                control.Controls.Remove(current);
                control.Controls.AddAt(i, new LiteralControl((current as DropDownList).SelectedItem.Text));
            }
            else if (current is CheckBox)
            {
                control.Controls.Remove(current);
                control.Controls.AddAt(i, new LiteralControl((current as CheckBox).Checked ? "True" : "False"));
            }

            if (current.HasControls())
            {
                GridViewExportUtil.PrepareControlForExport(current);
            }
        }
    }

    public static void ExportGridToCSV(GridView GridView1,string FileName)
    {

        HttpContext.Current.Response.Clear();
       HttpContext.Current.Response.Buffer = true;
       HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename="+FileName+".csv");
       HttpContext.Current.Response.Charset = "";
       HttpContext.Current.Response.ContentType = "application/text";
        //GridView1.AllowPaging = false;
        //GridView1.DataBind();

        StringBuilder columnbind = new StringBuilder();
        for (int k = 0; k < GridView1.Columns.Count; k++)
        {
        
            columnbind.Append(GridView1.Columns[k].HeaderText + ',');
        }

        columnbind.Append("\r\n");
       // StringBuilder sb = new StringBuilder();
        for (int i = 0; i < GridView1.Rows.Count; i++)
        {
            for (int k = 0; k < GridView1.Columns.Count; k++)
            {
                AddComma(GridView1.Rows[i].Cells[k].Text, columnbind);
             //   GridView1.Rows[i].Cells[k].Text.Replace(',', ' ');
                //columnbind.Append(sb.ToString());
            }

            columnbind.Append("\r\n");
        }
        HttpContext.Current.Response.Output.Write(columnbind.ToString());
        HttpContext.Current.Response.Flush();
        HttpContext.Current.Response.End();

    }


    private static void AddComma(string value, StringBuilder columnbind)
    {

        if (!string.IsNullOrEmpty(value) && value != "&nbsp;")
        {
            columnbind.Append(value.Replace(',', ' '));
            columnbind.Append(", ");
        }
        else
        {
            columnbind.Append(", ");
        }
    }
}
