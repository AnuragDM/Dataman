<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="DistributorPrint.aspx.cs" Inherits="AstralFFMS.DistributorPrint" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     

    <script language="javascript" type="text/javascript">
        function printDiv(divID) {
            //Get the HTML of div
            var divElements = document.getElementById(divID).innerHTML;
            //Get the HTML of whole page
            var oldPage = document.body.innerHTML;

            //Reset the page's HTML with div's HTML only
            document.body.innerHTML =
              "<html><head><title></title></head><body>" +
              divElements + "</body>";

            //Print Page
            window.print();

            //Restore orignal HTML
            document.body.innerHTML = oldPage;

        }
    </script>

    <section class="content">
          
      <div id="printablediv" style="width: 100%;height: 200px">
<div class="box-body"  runat="server">
            <div class="row">
                <!-- left column -->
                <div class="col-md-12">
                    <!-- general form elements -->
                    <div class="box box-default">
                        <div class="box-header">
                            <h3 class="box-title">Distributor Collection</h3>
                        </div>
                        <!-- /.box-header -->
                        <!-- form start -->
                        <div class="box-body">
                            <div class="col-lg-5 col-md-8 col-sm-7 col-xs-9">
                                <div class="form-group  col-sm-12 paddingleft0">
                                    
                                    <label for="exampleInputEmail1">Distributor Name:</label>
                                        <asp:Label ID="lbldistName" runat="server"></asp:Label>
                                </div>
                                   <div class="form-group  col-sm-12 paddingleft0">
                                    
                                    <label for="exampleInputEmail1">Collection Date:</label>
                                        <asp:Label ID="lblDate" runat="server"></asp:Label>
                                </div>
                                   <div class="form-group  col-sm-12 paddingleft0">
                                    <label for="exampleInputEmail1">Amount:</label>
                                        <asp:Label ID="lblAmount" runat="server"></asp:Label>
                                </div>
                                 <div class="form-group  col-sm-12 paddingleft0">
                                    <label for="exampleInputEmail1">Mode:</label>
                                        <asp:Label ID="lblMode" runat="server"></asp:Label>
                                </div>
                                 <div class="form-group col-sm-12 paddingleft0">
                                    <label for="exampleInputEmail1">Cheque/DD/RTGS No.:</label>
                                        <asp:Label ID="lbCheqNo" runat="server"></asp:Label>
                                </div>
                                  <div class="form-group  col-sm-12 paddingleft0">
                                    <label for="exampleInputEmail1">Cheque/DD/RTGS Date:</label>
                                        <asp:Label ID="lblCheqDate" runat="server"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-12 col-sm-12">
                    <input type="button" value="Print" onclick="javascript: printDiv('printablediv')" Class="btn btn-primary"/>
                    <asp:Button ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click" CssClass="btn btn-primary"/>
                    
                </div>

            </div>
        </div>
      
    </div>
    
    


    </section>
 
    

  


 
</asp:Content>
