<%@ Control Language="C#" AutoEventWireup="True" Inherits="ctlCalendar" Codebehind="ctlCalendar.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<%@ Import Namespace="BusinessLayer" %>

<div>

    <script type="text/javascript">

        function ValidateDateLessThenEqualsToCurrentDate(me) {
            try {
                var tDate = me.value;
                var cDate = '<%= DateTime.Now.ToUniversalTime().AddSeconds(19800).ToString("dd/MM/yyyy")%>';
                //variables for date from.
                //In dob1 i am taking the date which the user is giving 
                //and split it into mm/dd/yyyy to compare with current date
                var tArr = tDate.split("/");
                //variables for date from.
                //In dob i am taking the date which the user is giving 
                //and split it into mm/dd/yyyy to compare with current date
                var cArr = cDate.split("/");

                //these variable for checking the datefiels should not be blank
                var toDate = new Date(tArr[2], tArr[1] - 1, tArr[0]);
                var currDate = new Date(cArr[2], cArr[1] - 1, cArr[0]);

                if (toDate > currDate) {
                    alert("Date cannot be greater than current date!");
                    me.value = cDate;
                }
            }
            catch (e) {
                alert("Invalid date format!");
            }
        }
        function ValidateDateGreaterThenEqualsToCurrentDate(me) {
            try {
                var tDate = me.value;
                var cDate = '<%= DateTime.Now.ToUniversalTime().AddSeconds(19800).ToString("dd/MM/yyyy")%>';
                //variables for date from.
                //In dob1 i am taking the date which the user is giving 
                //and split it into mm/dd/yyyy to compare with current date
                var tArr = tDate.split("/");
                //variables for date from.
                //In dob i am taking the date which the user is giving 
                //and split it into mm/dd/yyyy to compare with current date
                var cArr = cDate.split("/");

                //these variable for checking the datefiels should not be blank
                var toDate = new Date(tArr[2], tArr[1] - 1, tArr[0]);
                var currDate = new Date(cArr[2], cArr[1] - 1, cArr[0]);

                if (toDate < currDate) {
                    alert("Date cannot be less than current date!");
                    me.value = cDate;
                }
            }
            catch (e) {
                alert("Invalid date format!");
            }
        }


        function ValidateDate(input) {
            var validformat = /^\d{2}\/\d{2}\/\d{4}$/ //Basic check for format validity
            var returnval = false
            if (!validformat.test(input.value)) {
                alert("Invalid date format!")
                input.value = '<%= DateTime.Now.ToUniversalTime().AddSeconds(19800).ToString("dd/MM/yyyy")%>';
            }
            else { //Detailed check for valid date ranges
                var monthfield = input.value.split("/")[1]
                var dayfield = input.value.split("/")[0]
                var yearfield = input.value.split("/")[2]
                var dayobj = new Date(yearfield, monthfield - 1, dayfield)
                if ((dayobj.getMonth() + 1 != monthfield) || (dayobj.getDate() != dayfield) || (dayobj.getFullYear() != yearfield)) {
                    alert("Invalid date format!")
                    input.value = '<%= DateTime.Now.ToUniversalTime().AddSeconds(19800).ToString("dd/MM/yyyy")%>';
                }
                else
                    returnval = true
            }
            if (returnval == false) input.select()
            return returnval
        }

    </script>

    <div style="float: left;">
        <asp:TextBox ID="txtDate" runat="server" CausesValidation="false" CssClass="textbox" Width="90px" onchange="ValidateDate(this)" />
    </div>
    <div style="float: left;">
        <asp:ImageButton ID="imgDate" runat="server" ImageUrl="~/Images/Calendar.png"
            Style="padding: 1px;" CausesValidation="False" />
        <AjaxToolkit:CalendarExtender ID="ajaxCalendar" runat="server" PopupButtonID="imgDate"
            TargetControlID="txtDate" Format="dd/MM/yyyy" />
    </div>
</div>
