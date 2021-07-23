<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="ParticlarsForDist.aspx.cs" Inherits="AstralFFMS.ParticlarsForDist" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="plugins/jquery.numeric.min.js"></script>
    <style type="text/css">
        .grid td {
            height: 40px;
            padding: 6px;
            text-align: right;
        }

        .grid th {
            height: 40px;
            padding: 6px;
        }

        #ContentPlaceHolder1_GridView4 {
            border: 1px solid #3c8dbc;
        }

        #ContentPlaceHolder1_gridhalfyearly {
            border: 1px solid #3c8dbc;
        }

        #ContentPlaceHolder1_gridquarterly {
            border: 1px solid #3c8dbc;
        }

        #ContentPlaceHolder1_GridDealReport {
            border: 1px solid #3c8dbc;
        }

        GridViewFooterStyle {
            background-color: red;
            font-weight: bold;
            color: White;
        }

        .aligngridcell {
            text-align: right;
        }

        .FixedHeader {
            /*position: absolute;
            font-weight: bold;*/
            position: relative;
            top: expression(this.offsetParent.scrollTop);
            z-index: 10;
        }

        #ContentPlaceHolder1_gridDeliveryarrangements {
            border: 1px solid #3c8dbc;
        }

        #ContentPlaceHolder1_gridRetailCustomerUnderYou {
            border: 1px solid #3c8dbc;
        }

        #ContentPlaceHolder1_gridprimeretailers {
            border: 1px solid #3c8dbc;
        }

        #ContentPlaceHolder1_gridestimatedturnover {
            border: 1px solid #3c8dbc;
        }
          #ContentPlaceHolder1_gridCompaniesRepresented {
            border: 1px solid #3c8dbc;
        }
        .formlay {
    width: 100% !important;
   
}   
      @media screen and (max-width: 600px) {
    .top-pad {
        padding-top:78px;
    }
}
        @media screen and (max-width: 600px) {
    .pad-bottom {
        padding-bottom: 63px;
    padding-top: 20px;
    }
}
    </style>
    
    <script type="text/javascript">
        var V1 = "";
        function errormessage(V1) {
            $("#messageNotification").jqxNotification({
                width: 300, position: "top-right", opacity: 2,
                autoOpen: false, animationOpenDelay: 800, autoClose: true, autoCloseDelay: 3000, template: "error"
            });
            $('#<%=lblmasg.ClientID %>').html(V1);
            $("#messageNotification").jqxNotification("open");

        }
    </script>
    <script type="text/javascript">
        var V = "";
        function Successmessage(V) {
            $("#messageNotification").jqxNotification({
                width: 300, position: "top-right", opacity: 2,
                autoOpen: false, animationOpenDelay: 800, autoClose: true, autoCloseDelay: 3000, template: "success"
            });
            $('#<%=lblmasg.ClientID %>').html(V);
            $("#messageNotification").jqxNotification("open");
        }



        

        function validate() {

            if ($('#<%=txtDinNo.ClientID%>').val() == "") {
                errormessage("Please Enter Distributor Identification No.");
                return false;
            }

            if ($('#<%=txtfirmname.ClientID%>').val() == "") {
                errormessage("Please Enter Firm Name.");
                return false;
            }
            if ($('#<%=txtaddress.ClientID%>').val() == "") {
                errormessage("Please Enter Address.");
                return false;
            }
            var value = ($('#<%=txtfirmname.ClientID%>').val().charAt(0));
            var chrcode = value.charCodeAt(0);
            if ((chrcode < 97 || chrcode > 122) && (chrcode < 65 || chrcode > 90)) {
                errormessage("Do not start Name with special characters.")
                return false;
            }
            if ($('#<%=txtcontactPerson.ClientID%>').val() == "") {
                errormessage("Please Enter Contact Person.");
                return false;
            }
             value = ($('#<%=txtcontactPerson.ClientID%>').val().charAt(0));
            var chrcode = value.charCodeAt(0);
            if ((chrcode < 97 || chrcode > 122) && (chrcode < 65 || chrcode > 90)) {
                errormessage("Do not start Name with special characters.")
                return false;
            }
           <%-- if ($('#<%=txtoffice.ClientID%>').val() == "") {
                errormessage("Please Enter Office Phone No.");
                return false;
            }--%>
            <%--if ($('#<%=txtoffice.ClientID%>').val() != "") {
                varpnlLength = "";
                varpnlLength = ($('#<%=txtoffice.ClientID%>').val().length);
                if (varpnlLength < 10) {
                    errormessage("Please Enter Office Phone No. With Std Code");
                    return false;
                }
            }--%>
           <%-- if ($('#<%=txtresidence.ClientID%>').val() == "") {
                errormessage("Please Enter Residence Phone No.");
                return false;
            }--%>
           <%-- if ($('#<%=txtemail.ClientID%>').val() == "") {
                errormessage("Please Enter Email.");
                return false;
            }--%>
            if ($('#<%=txtmobile.ClientID%>').val() == "") {
                errormessage("Please Enter Mobile.");
                return false;
            }
            if ($('#<%=txtmobile.ClientID%>').val() != "") {
                varpnlLength = "";
                varpnlLength = ($('#<%=txtmobile.ClientID%>').val().length);
                if (varpnlLength < 10) {
                    errormessage("Please Enter Valid Mobile No.");
                    return false;
                }
            }
            <%--if ($('#<%=ddlgrade.ClientID%>').val() == "0") {
                errormessage("Please select a Grade.");
                return false;
            }--%>
           <%-- if ($('#<%=txtProprieter.ClientID%>').val() == "") {
                errormessage("Please Enter Partner Name.");
                return false;
            }--%>
            if ($('#<%=DDlStorage.ClientID%>').val() == "0") {
                errormessage("Please Storage Type.");
                return false;
            }
            if ($('#<%=txtfeet.ClientID%>').val() == "") {
                errormessage("Please Enter Storage Area.");
                return false;
            }
        <%--    if ($('#<%=Pin.ClientID%>').val() != "") {
                varpnlLength = "";
                varpnlLength = ($('#<%=Pin.ClientID%>').val().length);
                if (varpnlLength < 6) {
                    errormessage("Please Enter 6 digit Pincode");
                    return false;
                }
            }--%>
            if ($('#<%=txtSalesman.ClientID%>').val() == "") {
                errormessage("Please Enter SalesPerson No.");
                return false;
            }
            if ($('#<%=txtothers.ClientID%>').val() == "") {
                errormessage("Please Enter Other Employee No.");
                return false;
            }
            <%-- varmblLength = "";
            varmblLength = ($('#<%=Mobile.ClientID%>').val().length);
            if (varmblLength < 10) {
                errormessage("Please Enter 10 digit mobile No.");
                return false;
            }--%>
            var images = document.getElementById('imgproprieter');
            if (images.src == "") {
                errormessage("Please Upload Photo of Properitor or Partner ");
                return false;
            }
            images = document.getElementById('imgProperitory');
            if (images.src == "") {
                errormessage("Please Upload Photo of Properitory/Partnership/Directorship");
                return false;
            }
        }

        function ValidateSecondForm()
        {

          
            if ($('#<%=txtinvestmentpropoed.ClientID%>').val() == "") {
                errormessage("Please Enter Proposed Investment");
                return false;
            }
            if ($('#<%=txtturnover.ClientID%>').val() == "") {
                errormessage("Please Enter Expected Turnover per month/year");
                return false;
            }
            if ($('#<%=txtunder.ClientID%>').val() == "") {
                errormessage("Please Enter No. of Distributors/ Retailers Under You.");
                return false;
            }
            if ($('#<%=txtsystems.ClientID%>').val() == "") {
                errormessage("Please Enter No. Of Systems");
                return false;
            }
            if ($('#<%=txtnewspaper.ClientID%>').val() == "") {
                errormessage("Please Enter NewsPaper Published In Your Area");
                return false;
            }
            if ($('#<%=txtgst.ClientID%>').val() == "") {
                errormessage("Please Enter GST No.");
                return false;
            }
            if ($('#<%=txtpan.ClientID%>').val() == "") {
                errormessage("Please Enter PAN No.");
                return false;
            }
            if ($('#<%=txtaadhar.ClientID%>').val() == "") {
                errormessage("Please Enter Aadhar No.");
                return false;
            }
            if ($('#<%=txtfoodlicense.ClientID%>').val() == "") {
                errormessage("Please Enter Food License No.");
                return false;
            }
            if ($('#<%=txtbank.ClientID%>').val() == "") {
                errormessage("Please Enter Bank Name.");
                return false;
            }
            if ($('#<%=txtbankbranch.ClientID%>').val() == "") {
                errormessage("Please Enter Bank Brach Name.");
                return false;
            }
            if ($('#<%=txtaccno.ClientID%>').val() == "") {
                errormessage("Please Enter Bank Account No.");
                return false;
            }
            //imgGST.Src = dt.Rows[0]["GSTImagePath"].ToString();
            //imgPan.Src = dt.Rows[0]["PANImagePath"].ToString();
            //imgaadhar.Src = dt.Rows[0]["AadharImagePath"].ToString();
            //imgFoodLicence.Src = dt.Rows[0]["FoodLicenseImagePath"].ToString();
            //imgbank.Src = dt.Rows[0]["BankChequeScanImagePath"].ToString();
            var images = document.getElementById('imgGST');
            if (images.src == "")
            {
                errormessage("Please Upload Your GST No. Doc ");
                return false;
            }
            images = document.getElementById('imgPan');
            if (images.src == "") {
                errormessage("Please Upload Your PAN Card ");
                return false;
            }
            images = document.getElementById('imgaadhar');
            if (images.src == "") {
                errormessage("Please Upload Your Aadhar Card ");
                return false;
            }
            images = document.getElementById('imgFoodLicence');
            if (images.src == "") {
                errormessage("Please Upload Your Food Licence No. ");
                return false;
            }
            images = document.getElementById('imgbank');
            if (images.src == "") {
                errormessage("Please Upload Your Bank Cheque Scan Copy ");
                return false;
            }

        }
    </script>
    <section class="content">
        <div id="spinner" class="spinner" style="display: none;">
            <img id="img-spinner" src="img/loader.gif" alt="Loading" /><br />
            Loading Data....
        </div>
        <div id="messageNotification">
            <div>
                <asp:Label ID="lblmasg" runat="server"></asp:Label>
            </div>
        </div>
        <div class="box-body" id="mainDiv" runat="server">
            <div class="row">
                <!-- left column -->
                <div class="col-md-12">
                    <!-- general form elements -->
                    <div class="box box-default">
                        <div class="box-header">
                            <h3 class="box-title" runat="server" id="formheader" >Distributor Registration : Entry Form-1</h3>
                            <div style="float: right">
                                <%--<asp:Button Style="margin-right: 5px;" type="button" ID="btnFind" runat="server" Text="Find" class="btn btn-primary" />--%>
                                <%--  <input style="margin-right: 5px;" type="button" id="Find" value="Find" class="btn btn-primary" runat="server" />--%>
                            </div>
                            <%--<div style="float: right">                                                         
                                <asp:CheckBox ID="chkuser" Style="margin-right: 20px;" runat="server" CssClass="checkbox" Text="Create user" OnCheckedChanged="chkuser_CheckedChanged" AutoPostBack="true" Checked="false" />
                            </div>--%>
                        </div>
                        <!-- /.box-header -->
                        <!-- form start -->
                        <div class="box-body">
                            <%-- Panel 1 start --%>
                            <asp:Panel runat="server" ID="panel1" Visible="true">
                               
                                <div class="row">
                                        <div class="col-md-12 col-sm-12 col-xs-12">
                                        <div class="row">
                                            <div class="col-md-6 col-sm-12 col-xs-12">

                                        <div class="form-group formlay">
                                            <label for="exampleInputEmail1">Distributor Identification No.</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <input runat="server" type="text" class="form-control" maxlength="50" id="txtDinNo" placeholder="Distributor Identification No" tabindex="2">
                                        </div>

                                        </div>
                                    <div class="col-md-6 col-sm-12 col-xs-12">

                                        <div class="form-group formlay">
                                            <label for="exampleInputEmail1">Name of the firm:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <input runat="server" type="text" class="form-control" maxlength="50" id="txtfirmname" placeholder="Enter Firm Name" tabindex="2">
                                        </div>

                                        </div>
                                    <div class="col-md-6 col-sm-12 col-xs-12">

                                        <div class="form-group formlay">
                                            <label for="exampleInputEmail1">Address:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <input runat="server" type="text" class="form-control" maxlength="100" id="txtaddress" placeholder="Enter Address" tabindex="2">
                                        </div>
                                        </div>
                                            <div class="col-md-6 col-sm-12 col-xs-12"> 
                                        <div class="form-group formlay">
                                            <label for="exampleInputEmail1">Contact Person:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <input runat="server" type="text" class="form-control " maxlength="50" id="txtcontactPerson" placeholder="Enter Contact Person Name" tabindex="2">
                                        </div>
                                                </div>
                                             <div class="col-md-6 col-sm-12 col-xs-12">
                                        <div class="form-group formlay">

                                            <label for="exampleInputEmail1">Phone No. Office</label>
                                            <input runat="server" type="text" class="form-control  numeric text" maxlength="11" id="txtoffice" placeholder="Enter Std-Office No." tabindex="2">
                                        </div>
                                                 </div>
                                             <div class="col-md-6 col-sm-12 col-xs-12">
                                        <div class="form-group formlay">

                                            <label for="exampleInputEmail1">Phone No. Residence</label>

                                          <input runat="server" type="text" class="form-control  numeric text" maxlength="11" id="txtresidence" placeholder="Enter Std-Residence No." tabindex="2">
                                        </div>
                                                 </div>
                                             <div class="col-md-6 col-sm-12 col-xs-12">
                                        <div class="form-group formlay">
                                            <label for="exampleInputEmail1">Email</label>
                                            <input runat="server" type="email" class="form-control " maxlength="50" id="txtemail" placeholder="Enter Email." tabindex="2">
                                        </div>
                                                 </div>
                                             <div class="col-md-6 col-sm-12 col-xs-12">
                                        <div class="form-group formlay">
                                            <label for="exampleInputEmail1">Mobile</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <input runat="server" type="text" class="form-control numeric text" maxlength="10" id="txtmobile" placeholder="Enter Mobile No." tabindex="2">
                                        </div>
                                                 </div>
                                             <div class="col-md-6 col-sm-12 col-xs-12">
                                        <div class="form-group formlay">
                                            <label for="exampleInputEmail1">Name Of Partner/Proprieter/Director:</label>
                                            <input runat="server" type="text" class="form-control " maxlength="30" id="txtProprieter" placeholder="Enter Name Of Partner/Proprieter/Director" tabindex="2">
                                        </div>
                                                 </div>
                                             <div class="col-md-6 col-sm-12 col-xs-12">
                                        <div class="form-group formlay">
                                            <label for="exampleInputEmail1">Storage Facility:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <br />
                                            <%--<input type="radio" name="Storage" value="Own" checked>Own
                                        <input type="radio" name="Storage" value="Rented">Rented--%>
                                            <asp:DropDownList runat="server" ID="DDlStorage" CssClass="form-control">
                                                <asp:ListItem Value="Own">Own</asp:ListItem>
                                                  <asp:ListItem Value="Rented">Rented</asp:ListItem>
                                            </asp:DropDownList>
                                            

                                        </div>
                                                 </div>
                                             <div class="col-md-6 col-sm-12 col-xs-12">
                                        <div class="form-group formlay">
                                            <label for="exampleInputEmail1">Storage Facility:Sq. Feet</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <input runat="server" type="text" class="form-control numeric text" maxlength="5" id="txtfeet" placeholder="Enter Sq.Feet" tabindex="2">
                                        </div>
                                       </div>
                                             <div class="col-md-6 col-sm-12 col-xs-12">
                                        <div class="form-group formlay">
                                            <label for="exampleInputEmail1">No of Employees : Salesperson</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <input runat="server" type="text" class="form-control  numeric text" maxlength="4" id="txtSalesman" placeholder="Enter No. of Employees (Salesperson)" tabindex="2">
                                        </div>
                                                 </div>
                                                  <div class="col-md-6 col-sm-12 col-xs-12">
                                        <div class="form-group formlay">
                                            <label for="exampleInputEmail1">No of Employees : Others</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <input runat="server" type="text" class="form-control  numeric text" maxlength="4" id="txtothers" placeholder="Enter No of Employees (Others)" tabindex="2">
                                        </div>
                                                      </div>
                                             <div class="col-md-6 col-sm-12 col-xs-12">
                                        <div class="form-group formlay" style="height: 160px">
                                            <label for="exampleInputEmail1">Upload Photo of Properitor or Partner:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;"></label>
                                            <asp:FileUpload ID="FileUpload2" runat="server" TabIndex="44" onchange="showpreview(this);" accept=".png,.jpg,.jpeg,.gif" />
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" Visible="false" runat="server" ValidationExpression="(.*?)\.(jpg|jpeg|png|gif|JPG|JPEG|PNG|GIF)$"
                                                ControlToValidate="FileUpload2" ErrorMessage="Please select only image file." ForeColor="Red" Display="Dynamic">
                                            </asp:RegularExpressionValidator>
                                            <img id="imgproprieter" height="120" width="200" src="" style="border-width: 0px; display: none;" runat="server" />
                                        </div>
                                              </div>   
                                        <%--   <div class="form-group formlay">
                                        
                                    </div>--%>
                                             <div class="col-md-6 col-sm-12 col-xs-12 pad-bottom">
                                        <div class="form-group formlay" style="height: 160px">
                                            <label for="exampleInputEmail1">Upload Photo of Properitory/Partnership/Directorship:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;"></label>
                                            <asp:FileUpload ID="FileUpload3" runat="server" TabIndex="44" onchange="showpreview1(this);" accept=".png,.jpg,.jpeg,.gif" />
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator3" Visible="false" runat="server" ValidationExpression="(.*?)\.(jpg|jpeg|png|gif|JPG|JPEG|PNG|GIF)$"
                                                ControlToValidate="FileUpload3" ErrorMessage="Please select only image file." ForeColor="Red" Display="Dynamic">
                                            </asp:RegularExpressionValidator>
                                            <img id="imgProperitory" height="120" width="200" src="" style="border-width: 0px; display: none;" runat="server" />
                                        </div>
                                          </div>
                                        <%--  <div class="form-group formlay">
                                        <label for="exampleInputEmail1">No of Employees (Others)</label>
                                        
                                    </div>--%>
                                    </div>
                                   </div>
                                </div>
                            </asp:Panel>


                            <%-- Panel 2 start --%>
                            <asp:Panel runat="server" ID="panel2" Visible="false">
                                <div class="row">
                                    <div class="col-md-12 col-sm-12 col-xs-12">
                                           <div class="row">
                                         <div class="col-md-6 col-sm-12 col-xs-12">

                                        <div class="form-group formlay">
                                            <label for="exampleInputEmail1">Investment Proposed:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <input runat="server" type="text" class="form-control  numeric text" maxlength="9" id="txtinvestmentpropoed" placeholder="Enter Investment Proposed" tabindex="2">
                                        </div>
                                             </div>
                                         <div class="col-md-6 col-sm-12 col-xs-12">
                                        <div class="form-group formlay">
                                            <label for="exampleInputEmail1">Exp. Turnover per month/year:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <input runat="server" type="text" class="form-control  numeric text" maxlength="9" id="txtturnover" placeholder="Enter Turnover" tabindex="2">
                                        </div>
                                             </div>
                                         <div class="col-md-6 col-sm-12 col-xs-12">
                                        <div class="form-group formlay">
                                            <label for="exampleInputEmail1">No. Of Distributors/Retailers under you</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <input runat="server" type="text" class="form-control  numeric text" maxlength="4" id="txtunder" placeholder="Enter Distributors/Retailers Under You" tabindex="2">
                                        </div>
                                             </div>
                                         <div class="col-md-6 col-sm-12 col-xs-12">
                                        <div class="form-group formlay">
                                            <label for="exampleInputEmail1">No.Of Systems/Computing:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <input runat="server" type="text" class="form-control  numeric text" maxlength="4" id="txtsystems" placeholder="Enter No.Of Systems/Computing" tabindex="2" />
                                        </div>
                                        </div>


                                         <div class="col-md-6 col-sm-12 col-xs-12">
                                        <div class="form-group formlay">

                                            <label for="exampleInputEmail1">Newspaper Published In Your Area</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>

                                            <input runat="server" type="text" class="form-control" maxlength="50" id="txtnewspaper" placeholder="Enter Newspaper Published Area" tabindex="2">
                                        </div>
                                             </div>
                                         <div class="col-md-6 col-sm-12 col-xs-12">
                                        <div class="form-group formlay">

                                            <label for="exampleInputEmail1">Willing To Work Exclusively with SGMPL</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <br />
                                      <%--      <input type="radio" name="WorkExclusively" value="Yes" checked>Yes
                                        <input type="radio" name="WorkExclusively" value="No">No--%>
                                            <asp:DropDownList runat="server" ID="ddlWorkExclusively" CssClass="form-control">
                                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                <asp:ListItem Value="No">No</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                             </div>
                                         <div class="col-md-6 col-sm-12 col-xs-12">
                                        <div class="form-group formlay" style="height: 160px">
                                            <label for="exampleInputEmail1">Upload & Enter  GST Registration No.:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <input runat="server" type="text" class="form-control" maxlength="20" id="txtgst" placeholder="Enter GST Registration No." tabindex="2">
                                            <asp:FileUpload ID="FileUpload5" runat="server" TabIndex="44" onchange="showpreview2(this);" accept=".png,.jpg,.jpeg,.gif" />
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator5" Visible="false" runat="server" ValidationExpression="(.*?)\.(jpg|jpeg|png|gif|JPG|JPEG|PNG|GIF)$"
                                                ControlToValidate="FileUpload5" ErrorMessage="Please select only image file." ForeColor="Red" Display="Dynamic">
                                            </asp:RegularExpressionValidator>
                                            <img id="imgGST" height="80" width="200" src="" style="border-width: 0px; display: none;" runat="server" />
                                        </div>
                                             </div>
                                         <div class="col-md-6 col-sm-12 col-xs-12">
                                        <div class="form-group formlay" style="height: 160px">
                                            <label for="exampleInputEmail1">Upload & Enter  PAN No.:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <input runat="server" type="text" class="form-control" maxlength="20" id="txtpan" placeholder="Enter PAN No." tabindex="2">
                                            <asp:FileUpload ID="FileUpload11" runat="server" TabIndex="44" onchange="showpreview3(this);" accept=".png,.jpg,.jpeg,.gif" />
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" Visible="false" runat="server" ValidationExpression="(.*?)\.(jpg|jpeg|png|gif|JPG|JPEG|PNG|GIF)$"
                                                ControlToValidate="FileUpload11" ErrorMessage="Please select only image file." ForeColor="Red" Display="Dynamic">
                                            </asp:RegularExpressionValidator>
                                            <img id="imgPan" height="80" width="200" src="" style="border-width: 0px; display: none;" runat="server" />
                                        </div>
                                             </div>
                                        <%--   <div class="form-group formlay">
                                        
                                    </div>--%>
                                         <div class="col-md-6 col-sm-12 col-xs-12">
                                        <div class="form-group formlay" style="height: 160px">
                                            <label for="exampleInputEmail1">Upload & Enter  Aadhar No.</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <input runat="server" type="text" class="form-control  numeric text-right" maxlength="20" id="txtaadhar" placeholder="Enter Aadhar No." tabindex="2">
                                            <asp:FileUpload ID="FileUpload4" runat="server" TabIndex="44" onchange="showpreview4(this);" accept=".png,.jpg,.jpeg,.gif" />
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator4" Visible="false" runat="server" ValidationExpression="(.*?)\.(jpg|jpeg|png|gif|JPG|JPEG|PNG|GIF)$"
                                                ControlToValidate="FileUpload4" ErrorMessage="Please select only image file." ForeColor="Red" Display="Dynamic">
                                            </asp:RegularExpressionValidator>
                                            <img id="imgaadhar" height="80" width="200" src="" style="border-width: 0px; display: none;" runat="server" />
                                        </div>
                                             </div>
                                         <div class="col-md-6 col-sm-12 col-xs-12">
                                        <div class="form-group formlay" style="height: 160px">
                                            <label for="exampleInputEmail1">Any Additional Information:</label>
                                            <textarea runat="server" type="text" class="form-control" maxlength="150" style="height: 100px" id="txtadditional" placeholder="Enter Additional Information" tabindex="2" />
                                        </div>
                                             </div>
                                         <div class="col-md-6 col-sm-12 col-xs-12">
                                        <div class="form-group formlay" style="height: 160px">
                                            <label for="exampleInputEmail1">Upload & Enter Food Licence No.</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <input runat="server" type="text" class="form-control" maxlength="20" id="txtfoodlicense" placeholder="Enter Food License No." tabindex="2">
                                            <asp:FileUpload ID="FileUpload7" runat="server" TabIndex="44" onchange="showpreview6(this);" accept=".png,.jpg,.jpeg,.gif" />
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator7" Visible="false" runat="server" ValidationExpression="(.*?)\.(jpg|jpeg|png|gif|JPG|JPEG|PNG|GIF)$"
                                                ControlToValidate="FileUpload7" ErrorMessage="Please select only image file." ForeColor="Red" Display="Dynamic">
                                            </asp:RegularExpressionValidator>
                                            <img id="imgFoodLicence" height="80" width="200" src="" style="border-width: 0px; display: none;" runat="server" />
                                        </div>
                                             </div>

                                         <div class="col-md-6 col-sm-12 col-xs-12">
                                        <div class="form-group formlay" style="height: 160px">
                                            <label for="exampleInputEmail1">Upload & Enter  Bank Name And  </label>
                                            &nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <input runat="server" type="text" class="form-control" maxlength="50" id="txtbank" placeholder="Enter Bank Name" tabindex="2">
                                            <asp:FileUpload ID="FileUpload6" runat="server" TabIndex="44" onchange="showpreview5(this);" accept=".png,.jpg,.jpeg,.gif" />
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator6" Visible="false" runat="server" ValidationExpression="(.*?)\.(jpg|jpeg|png|gif|JPG|JPEG|PNG|GIF)$"
                                                ControlToValidate="FileUpload6" ErrorMessage="Please select only image file." ForeColor="Red" Display="Dynamic">
                                            </asp:RegularExpressionValidator>
                                            <img id="imgbank" height="80" width="200" src="" style="border-width: 0px; display: none;" runat="server" />
                                        </div>
                                             </div>
                                         <div class="col-md-6 col-sm-12 col-xs-12">
                                        <div class="form-group formlay">
                                            <label for="exampleInputEmail1">Bank Branch</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <input runat="server" type="text" class="form-control " maxlength="100" id="txtbankbranch" placeholder="Enter Bank Branch Name" tabindex="2">
                                        </div>
                                             </div>
                                         <div class="col-md-6 col-sm-12 col-xs-12">
                                        <div class="form-group formlay">
                                            <label for="exampleInputEmail1">Account No.</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <input runat="server" type="text" class="form-control" maxlength="20" id="txtaccno" placeholder="Enter Account No." tabindex="2">
                                        </div>
                                             </div>
                                        </div>
                                    </div>
                                </div>
                                </div>
                            </asp:Panel>


                            <%-- Panel 3 --%>
                            <asp:Panel runat="server" ID="panel3" Visible="false">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div style="height: 20%;">
                                            <label for="exampleInputEmail1">Name Of Companies you Represented:</label>
                                            <asp:GridView runat="server" ID="gridCompaniesRepresented" EmptyDataText="dd" AutoGenerateColumns="false" CssClass="grid" ItemStyle-HorizontalAlign="right" HeaderStyle-BackColor="#3c8dbc" Width="100%" HeaderStyle-ForeColor="White" ShowHeaderWhenEmpty="true">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="S.No">
                                                        <ItemTemplate>
                                                            <%#Container.DataItemIndex+1 %>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Name">
                                                        <ItemTemplate>
                                                            <%--<input type="text" id="txtname" class="form-control" maxlength="50" runat="server" />--%>
                                                            <asp:TextBox runat="server" CssClass="form-control" id="txtname" maxlength="50"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Since">
                                                        <ItemTemplate>
                                                            <%--<input type="text" id="txtsince" class="form-control" maxlength="10" runat="server" />--%>
                                                               <asp:TextBox runat="server" CssClass="form-control" id="txtsince" maxlength="10"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Turnover For Last 2 Years">
                                                        <ItemTemplate>
                                                            <%--<input type="text" id="txtturnovertwoyear" class="form-control  numeric text-right" maxlength="8" runat="server" />--%>
                                                             <asp:TextBox runat="server" CssClass="form-control numeric text-right" id="txtturnovertwoyear" maxlength="8"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>

                                        </div>
                                        <div style="height: 20%;">
                                            <label for="exampleInputEmail1">Delivery Arrangements:</label>
                                            <asp:GridView runat="server" ID="gridDeliveryarrangements" EmptyDataText="dd" AutoGenerateColumns="false" CssClass="grid" ItemStyle-HorizontalAlign="right" HeaderStyle-BackColor="#3c8dbc" Width="100%" HeaderStyle-ForeColor="White" ShowHeaderWhenEmpty="true">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="S.No">
                                                        <ItemTemplate>
                                                            <%#Container.DataItemIndex+1 %>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Type Of Vehicle">
                                                        <ItemTemplate>
                                                          <%-- <asp:TextBox runat="server" ID="lblvehicletype" CssClass="form-control" Font-Bold="true" Width="100%" ForeColor="#3c8dbc" Text='<%# Eval("VehicleType") %>' style="text-align:left" MaxLength="25"></asp:TextBox>--%>
                                                            <asp:TextBox runat="server" ID="txtVehicleType" CssClass="form-control" Font-Bold="true" Width="100%" ForeColor="#3c8dbc" Text='<%# Eval("VehicleType") %>' style="text-align:left" MaxLength="25"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="No.">
                                                        <ItemTemplate>
                                                            <%--<input type="text" id="txtnovehicle" class="form-control  numeric text-right" maxlength="3" runat="server" />--%>
                                                             <asp:TextBox runat="server" ID="txtNoOfVehicle" CssClass="form-control  numeric text-right" maxlength="3" ></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Own/Hired">
                                                        <ItemTemplate>
                                                            <%--<input type="text" id="txtownhired" class="form-control" maxlength="5" runat="server" />--%>
                                                            <%-- <asp:TextBox runat="server"  Id="txtOwnOrHired"  class="form-control" maxlength="5" ></asp:TextBox>--%>
                                                            <asp:DropDownList runat="server" ID="ddlownhire" CssClass="form-control">
                                                                <asp:ListItem Value="Own">Own</asp:ListItem>
                                                                  <asp:ListItem Value="Own">Hired</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>

                                        </div>
                                        <div style="height:20%">
                                            <label for="exampleInputEmail1">No. Of Retail Customer Under You:   </label>
                                            <div class="table-responsive">
                                            <asp:GridView runat="server" ID="gridRetailCustomerUnderYou" EmptyDataText="dd" AutoGenerateColumns="false" CssClass="grid table" ItemStyle-HorizontalAlign="right" HeaderStyle-BackColor="#3c8dbc" Width="100%" HeaderStyle-ForeColor="White" ShowHeaderWhenEmpty="true">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="S.No">
                                                        <ItemTemplate>
                                                            <%#Container.DataItemIndex+1 %>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Market Name">
                                                        <ItemTemplate>
                                                            <%--<input type="text" id="txtmarketname" class="form-control" maxlength="50" runat="server" />--%>
                                                             <asp:TextBox runat="server" CssClass="form-control"  maxlength="50" id="txtmarketname"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Distance (in Km) From Nearest Godown">
                                                        <ItemTemplate>
                                                            <%--<input type="text" id="txtdistance" class="form-control  numeric text-right" maxlength="5" runat="server" />--%><asp:TextBox runat="server" CssClass="form-control numeric text-right"  maxlength="5" id="txtdistance"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="No.Of Retails Counter">
                                                        <ItemTemplate>
                                                           <%-- <input type="text" id="txtretailscounter" class="form-control  numeric text-right" maxlength="4" runat="server" />--%>
                                                                <asp:TextBox runat="server" CssClass="form-control numeric text-right"  maxlength="4" id="txtretailscounter"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Counters presently associated">
                                                        <ItemTemplate>
                                                            <%--<input type="text" id="txtcounters" class="form-control  numeric text-right"  maxlength="4" runat="server" />--%>
                                                            <asp:TextBox runat="server" CssClass="form-control numeric text-right"  maxlength="4" id="txtcounters"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="TurnOver of Market">
                                                        <ItemTemplate>
                                                            <%--<input type="text" id="txtturnovermarket" class="form-control  numeric text-right" maxlength="10" runat="server" />--%>
                                                              <asp:TextBox runat="server" CssClass="form-control numeric text-right"  maxlength="10" id="txtturnovermarket"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Your TurnOver">
                                                        <ItemTemplate>
                                                            <%--<input type="text" id="txtyourturnover" class="form-control  numeric text-right" maxlength="10" runat="server" />--%>
                                                             <asp:TextBox runat="server" CssClass="form-control  numeric text-right"  maxlength="10" id="txtyourturnover"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                            </div>
                                        </div>
                                        <div style="height: 20%;">
                                            <label for="exampleInputEmail1" class="top-pad">Name Of Prime Retailers/Modern & their Location:</label>
                                            <asp:GridView runat="server" ID="gridprimeretailers" EmptyDataText="dd" AutoGenerateColumns="false" CssClass="grid" ItemStyle-HorizontalAlign="right" HeaderStyle-BackColor="#3c8dbc" Width="100%" HeaderStyle-ForeColor="White" ShowHeaderWhenEmpty="true">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="S.No">
                                                        <ItemTemplate>
                                                            <%#Container.DataItemIndex+1 %>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Name Of Retailer">
                                                        <ItemTemplate>
                                                            <%--<input type="text" id="txtnameofretailer" class="form-control" maxlength="50" runat="server" />--%>
                                                              <asp:TextBox runat="server" CssClass="form-control"  maxlength="50" id="txtnameofretailer"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Location">
                                                        <ItemTemplate>
                                                            <%--<input type="text" id="txtlocation" class="form-control" maxlength="100" runat="server" />--%>
                                                               <asp:TextBox runat="server" CssClass="form-control"  maxlength="100" id="txtlocation"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>

                                        </div>
                                        <div style="height: 30%;">
                                            <label for="exampleInputEmail1" class="top-pad">Estimated Turnover:</label>
                                             <div class="table-responsive">
                                            <asp:GridView runat="server" ID="gridestimatedturnover" AutoGenerateColumns="false" CssClass="grid table" ItemStyle-HorizontalAlign="right" HeaderStyle-BackColor="#3c8dbc" Width="100%" HeaderStyle-ForeColor="White" ShowHeaderWhenEmpty="true">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="S.No">
                                                        <ItemTemplate>
                                                            <%#Container.DataItemIndex+1 %>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Name Of Commodity">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblcommodity" CssClass="form-control" Font-Bold="true" Width="100%" ForeColor="#3c8dbc" Text='<%# Eval("NameOfCommodity") %>' style="text-align:left" ></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="TurnOver Estimated">
                                                        <ItemTemplate>
                                                            <%--<input type="text" id="txtestimated" class="form-control  numeric text-right" maxlength="10" runat="server" />--%>
                                                             <asp:TextBox runat="server" CssClass="form-control  numeric text-right"  maxlength="10" id="txtestimated"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Current Year Target">
                                                        <ItemTemplate>
                                                            <%--<input type="text" id="txtcurrentyeartarget" maxlength="10" class="form-control  numeric text-right" runat="server" />--%>
                                                             <asp:TextBox runat="server" CssClass="form-control  numeric text-right"  maxlength="10" id="txtcurrentyeartarget"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <FooterStyle BackColor="#3c8dbc" Font-Bold="true" HorizontalAlign="Right" Font-Size="18px"
                                                    ForeColor="White" />
                                            </asp:GridView>
                                          </div>

                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>
                        </div>
                    </div>
                    <div class="box-footer" runat="server" id="divfooter" >
                        <asp:Button Style="margin-right: 2%;" type="button" ID="BtnNext1" runat="server" Text="Next" class="btn btn-primary pull-right" OnClientClick="javascript:return validate();" OnClick="BtnNext1_Click1" TabIndex="52" />
                        <asp:Button Style="margin-left: 2%;" type="button" ID="Btnpre1" runat="server" Text="Back" class="btn btn-primary " OnClick="Btnpre1_Click1" TabIndex="52" />
                        <asp:Button Style="margin-right: 2%;" type="button" ID="BtnNext2" runat="server" Text="Next" class="btn btn-primary pull-right" OnClientClick="javascript:return ValidateSecondForm();" OnClick="BtnNext2_Click" TabIndex="52" />
                        <asp:Button Style="margin-left: 2%;"  type="button" ID="btnprevious2" runat="server" Text="Back" class="btn btn-primary "  OnClick="btnprevious2_Click" TabIndex="52" Visible="false" />
                        <asp:Button Style="margin-right: 2%;" type="button" ID="btnFinalSave" runat="server" Text="Final Save" class="btn btn-primary pull-right" OnClick="btnFinalSave_Click" TabIndex="52" Visible="false" />
                    </div>
                    <br />
                    <div>
                        <b>Note : It is mandatory to fill in all the required fields marked with asterisks(<span style="color: red">*</span>)</b>
                    </div>
                </div>
            </div>


        </div>





    </section>
    <script type="text/javascript">
        function showpreview(input) {
            var uploadcontrol = document.getElementById('<%=FileUpload2.ClientID%>').value;
            var reg = /(.*?)\.(jpg|jpeg|png|gif|JPG|JPEG|PNG|GIF)$/;
            if (uploadcontrol.length > 0) {
                if (reg.test(uploadcontrol)) {
                    if (input.files && input.files[0]) {
                        var reader = new FileReader();
                        reader.onload = function (e) {
                            $("#ContentPlaceHolder1_imgproprieter").css('display', 'block');
                            $("#ContentPlaceHolder1_imgproprieter").attr('src', e.target.result);
                        }
                        reader.readAsDataURL(input.files[0]);
                    }
                }
                else {
                    errormessage("Only image files are allowed!");
                    $("#ContentPlaceHolder1_imgproprieter").css('display', 'none');
                    return false;
                }
            }
        }

        function showpreview1(input) {
            var uploadcontrol = document.getElementById('<%=FileUpload3.ClientID%>').value;
            var reg = /(.*?)\.(jpg|jpeg|png|gif|JPG|JPEG|PNG|GIF)$/;
            if (uploadcontrol.length > 0) {
                if (reg.test(uploadcontrol)) {
                    if (input.files && input.files[0]) {
                        var reader = new FileReader();
                        reader.onload = function (e) {
                            $("#ContentPlaceHolder1_imgProperitory").css('display', 'block');
                            $("#ContentPlaceHolder1_imgProperitory").attr('src', e.target.result);
                        }
                        reader.readAsDataURL(input.files[0]);
                    }
                }
                else {
                    errormessage("Only image files are allowed!");
                    $("#ContentPlaceHolder1_imgProperitory").css('display', 'none');
                    return false;
                }
            }
        }
        function showpreview2(input) {
            var uploadcontrol = document.getElementById('<%=FileUpload5.ClientID%>').value;
            var reg = /(.*?)\.(jpg|jpeg|png|gif|JPG|JPEG|PNG|GIF)$/;
            if (uploadcontrol.length > 0) {
                if (reg.test(uploadcontrol)) {
                    if (input.files && input.files[0]) {
                        var reader = new FileReader();
                        reader.onload = function (e) {
                            $("#ContentPlaceHolder1_imgGST").css('display', 'block');
                            $("#ContentPlaceHolder1_imgGST").attr('src', e.target.result);
                        }
                        reader.readAsDataURL(input.files[0]);
                    }
                }
                else {
                    errormessage("Only image files are allowed!");
                    $("#ContentPlaceHolder1_imgGST").css('display', 'none');
                    return false;
                }
            }
        }
        function showpreview3(input) {
            var uploadcontrol = document.getElementById('<%=FileUpload11.ClientID%>').value;
            var reg = /(.*?)\.(jpg|jpeg|png|gif|JPG|JPEG|PNG|GIF)$/;
            if (uploadcontrol.length > 0) {
                if (reg.test(uploadcontrol)) {
                    if (input.files && input.files[0]) {
                        var reader = new FileReader();
                        reader.onload = function (e) {
                            $("#ContentPlaceHolder1_imgPan").css('display', 'block');
                            $("#ContentPlaceHolder1_imgPan").attr('src', e.target.result);
                        }
                        reader.readAsDataURL(input.files[0]);
                    }
                }
                else {
                    errormessage("Only image files are allowed!");
                    $("#ContentPlaceHolder1_imgPan").css('display', 'none');
                    return false;
                }
            }
        }
        function showpreview4(input) {
            var uploadcontrol = document.getElementById('<%=FileUpload4.ClientID%>').value;
            var reg = /(.*?)\.(jpg|jpeg|png|gif|JPG|JPEG|PNG|GIF)$/;
            if (uploadcontrol.length > 0) {
                if (reg.test(uploadcontrol)) {
                    if (input.files && input.files[0]) {
                        var reader = new FileReader();
                        reader.onload = function (e) {
                            $("#ContentPlaceHolder1_imgaadhar").css('display', 'block');
                            $("#ContentPlaceHolder1_imgaadhar").attr('src', e.target.result);
                        }
                        reader.readAsDataURL(input.files[0]);
                    }
                }
                else {
                    errormessage("Only image files are allowed!");
                    $("#ContentPlaceHolder1_imgaadhar").css('display', 'none');
                    return false;
                }
            }
        }
        function showpreview5(input) {
            var uploadcontrol = document.getElementById('<%=FileUpload6.ClientID%>').value;
            var reg = /(.*?)\.(jpg|jpeg|png|gif|JPG|JPEG|PNG|GIF)$/;
            if (uploadcontrol.length > 0) {
                if (reg.test(uploadcontrol)) {
                    if (input.files && input.files[0]) {
                        var reader = new FileReader();
                        reader.onload = function (e) {
                            $("#ContentPlaceHolder1_imgbank").css('display', 'block');
                            $("#ContentPlaceHolder1_imgbank").attr('src', e.target.result);
                        }
                        reader.readAsDataURL(input.files[0]);
                    }
                }
                else {
                    errormessage("Only image files are allowed!");
                    $("#ContentPlaceHolder1_imgbank").css('display', 'none');
                    return false;
                }
            }
        }
        function showpreview6(input) {
            var uploadcontrol = document.getElementById('<%=FileUpload7.ClientID%>').value;
            var reg = /(.*?)\.(jpg|jpeg|png|gif|JPG|JPEG|PNG|GIF)$/;
            if (uploadcontrol.length > 0) {
                if (reg.test(uploadcontrol)) {
                    if (input.files && input.files[0]) {
                        var reader = new FileReader();
                        reader.onload = function (e) {
                            $("#ContentPlaceHolder1_imgFoodLicence").css('display', 'block');
                            $("#ContentPlaceHolder1_imgFoodLicence").attr('src', e.target.result);
                        }
                        reader.readAsDataURL(input.files[0]);
                    }
                }
                else {
                    errormessage("Only image files are allowed!");
                    $("#ContentPlaceHolder1_imgFoodLicence").css('display', 'none');
                    return false;
                }
            }
        }
    </script>
</asp:Content>
