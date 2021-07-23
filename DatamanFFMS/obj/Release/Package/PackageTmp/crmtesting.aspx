<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="crmtesting.aspx.cs" Inherits="AstralFFMS.crmtesting" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
      
           td {
              border:0px solid !important;
          }
        .bgwhite{
            padding:8px;
            background-color:white;
            margin-bottom:10px;
        }

        .paddingleft0{
            padding-left:0px;
        }
        .paddingright0{
            padding-right:0px;
        }

        .padding0{
            padding:0px;
        }

        .leadinfo_fontfamily{
       
              font-family: sans-serif;
        font-size: 12px;

        }

        .bold {
            font-weight:600;
           font-family:sans-serif;
                    
           }

        .spanaction{
      font-weight: 600;
    padding: 5px;
    display: inline-block;
    background-color: #fff;
    border-radius: 4px;
        margin-bottom: 12px;

        }


    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<section class="content">
  <!-- row -->
  <div class="col-md-12 col-sm-12 col-xs-12 bgwhite">
    <div class="col-md-3 col-sm-3 col-xs-3 paddingleft0">
      <button class="btn btn-primary">Back
      </button>
    </div>
    <div class="col-md-3 col-sm-3 col-xs-3 paddingright0 pull-right">
      <button class="btn btn-primary pull-right">Add Lead
      </button>
    </div>
  </div>
  <div class="clearfix">
  </div>
  <div class="col-md-12 bgwhite">
    <div class="row">
      <div class="col-md-6"> 
        <p style="font-size:31px;">Mr. Sanjeev Gupta
        </p>
      </div>
      <div class="col-md-6 paddingleft0 pull-right">
        <div class="col-md-4" style="float:right;">
          <div class="form-group">
            <label>Status:
            </label>
            <select class="form-control select2 select2-hidden-accessible" style="width: 100%;" tabindex="-1" aria-hidden="true">
              <option selected="selected">Alabama
              </option>
              <option>Alaska
              </option>
              <option disabled="disabled">California (disabled)
              </option>
              <option>Delaware
              </option>
              <option>Tennessee
              </option>
              <option>Texas
              </option>
              <option>Washington
              </option>
            </select>
          </div>
        </div>
        <div class="col-md-4" style="float:right;">
          <div class="form-group">
            <label>Tags :
            </label>
            <select class="form-control select2 select2-hidden-accessible" style="width: 100%;" tabindex="-1" aria-hidden="true">
              <option selected="selected">Alabama
              </option>
              <option>Alaska
              </option>
              <option disabled="disabled">California (disabled)
              </option>
              <option>Delaware
              </option>
              <option>Tennessee
              </option>
              <option>Texas
              </option>
              <option>Washington
              </option>
            </select>
          </div>
        </div>
      </div>
    </div>
  </div>
  <!-- first section -->
  <!-- 2nd section -->
  <%-- 
       <div class="col-md-12 bgwhite">
  <center>
    <h2 class="text-center"> Lead Information 
    </h2>
  </center>  
  </div>--%>
<!-- end 2nd section -->
<!-- 3rd section -->
<div class="col-md-12 col-sm-12 col-xs-12 bgwhite">
  <div class="col-md-6 col-sm-5" style="padding:28px 0px">
    <%--               
         <div class="col-md-3">
    <span class="leadinfo_fontfamily bold">Lead Name :
    </span> 
  </div>  
  <div class="col-md-6">
    <span class="leadinfo_fontfamily"> Mr. Sanjeev Gupta 
    </span>
  </div>--%>
  <div class="clearfix">
  </div>
  <div class="col-md-3">
    <span class="leadinfo_fontfamily bold">Company Name :
    </span> 
  </div>  
  <div class="col-md-6">
    <span class="leadinfo_fontfamily">DatamanComputer Systems Pvt ltd.
    </span>
  </div>
  <div class="clearfix">
  </div>
  <div class="col-md-3">
    <span class="leadinfo_fontfamily bold">Address :
    </span> 
  </div>  
  <div class="col-md-6">
    <span class="leadinfo_fontfamily">131 / 362 begum purwa juhi lal colony kanpur
    </span>
  </div>
  <div class="clearfix">
  </div>
  <div class="col-md-3">
    <span class="leadinfo_fontfamily bold">Website Name :
    </span> 
  </div>  
  <div class="col-md-6">
    <span class="leadinfo_fontfamily">www.dataman.in
    </span>
  </div>
  <div class="clearfix">
  </div>
</div>
<div class="col-md-6 col-sm-7">
  <div class="table-responsive">
    <table class="table">
      <tr>
        <th> Name 
        </th>
        <th>  Phone No. 
        </th>
        <th> Email 
        </th>
      </tr>
      <tr>
        <td> Sanjeev Gupta 
        </td>
        <td> +91-8564957155  
        </td>
        <td>  mohddanish8564@gmail.com  
        </td>
      </tr>
      <tr>
        <td> Sanjeev Gupta 
        </td>
        <td> +91-8564957155  
        </td>
        <td>  mohddanish8564@gmail.com  
        </td>
      </tr>
      <tr>
        <td> Sanjeev Gupta 
        </td>
        <td> +91-8564957155  
        </td>
        <td>  mohddanish8564@gmail.com  
        </td>
      </tr>
    </table>
  </div>
</div>
</div>
<div class="clearfix">
</div>
<!-- end 3rd section -->
<!-- end 4th section -->
<div class="col-md-12 col-sm-12 col-xs-12 bgwhite">
  <%-- 
       <div class="col-md-2" style="margin-bottom:10px;">
  <img src="img/arrow.png">
</div>--%>
<div class="col-md-12" style="background: #f4f4f4; padding: 20px;border-radius: 9px;">
  <div class="col-md-4">
    <h3>Action
    </h3>
  </div>
  <div class="clearfix">
  </div>
  <div class="col-md-12">
    <div class="form-group">
      <textarea class="form-control" style="height:105px;" placeholder="Next Action">
      </textarea>
    </div>
    <div class="col-md-3 paddingleft0">
      <div class="form-group">
        <label>Date:
        </label>
        <div class="input-group date">
          <div class="input-group-addon">
            <i class="fa fa-calendar">
            </i>
          </div>
          <input type="text" class="form-control pull-right" id="datepicker">
        </div>
        <!-- /.input group -->
      </div>
    </div>   
    <div class="col-md-3 paddingleft0">
      <div class="form-group">
        <label>Assign To :
        </label>
        <select class="form-control select2 select2-hidden-accessible" style="width: 100%;" tabindex="-1" aria-hidden="true">
          <option selected="selected">Alabama
          </option>
          <option>Alaska
          </option>
          <option disabled="disabled">California (disabled)
          </option>
          <option>Delaware
          </option>
          <option>Tennessee
          </option>
          <option>Texas
          </option>
          <option>Washington
          </option>
        </select>
      </div>
    </div>
    <div class="col-md-3 paddingleft0">
      <div class="form-group">
        <label>Set Task Status To :
        </label>
        <select class="form-control select2 select2-hidden-accessible" style="width: 100%;" tabindex="-1" aria-hidden="true">
          <option selected="selected">Alabama
          </option>
          <option>Alaska
          </option>
          <option disabled="disabled">California (disabled)
          </option>
          <option>Delaware
          </option>
          <option>Tennessee
          </option>
          <option>Texas
          </option>
          <option>Washington
          </option>
        </select>
      </div>
    </div>
    <div class="col-md-6 paddingleft0">
      <br />
      <button class="btn btn-primary">Save
      </button>
      <button class="btn btn-primary">Save
      </button>
    </div>
  </div>
</div>
</div>
<!-- end 4th section -->
<!-- tabs -->
<div class="col-md-12 bgwhite" style="clear:both;">
  <ul class="nav nav-tabs">
    <li class="active">
      <a class="btn btn-primary" data-toggle="collapse" data-target="#home">
        <i class="glyphicon glyphicon-plus">
        </i>Add Note
      </a>
    </li>
    <li>
      <a data-toggle="collapse" data-target="#menu1"> <i class="glyphicon glyphicon-plus">
        </i>Add Deal
      </a>
    </li>
    <li>
      <a data-toggle="collapse" data-target="#menu2"> <i class="glyphicon glyphicon-plus">
        </i>Add Call
      </a>
    </li>
  </ul>
  <div class="tab-content">
    <div id="home" class="collapse">
      <h3>Add Note
      </h3>
      <div class="col-md-12" style="background: #f4f4f4; padding: 20px;border-radius: 9px;">
        <div class="col-md-12">
          <div class="form-group">
            <textarea class="form-control" style="height:105px;" placeholder="Add Note">
            </textarea>
          </div>
          <div class="col-md-6 paddingleft0">
            <button class="btn btn-primary">Save
            </button>
            <button class="btn btn-primary">Save
            </button>
          </div>
        </div>
      </div>
    </div>
      <div class="clearfix"></div>
    <div id="menu1" class="collapse">
      <h3>Add Deal
      </h3>
      <div class="col-md-12 paddingleft0 paddingright0" style="background: #f4f4f4; padding: 20px;border-radius: 9px;">
        <div class="col-md-12">
          <div class="col-md-3 paddingleft0">
            <div class="form-group">
              <label>Dealer Name :
              </label>
              <input type="text" id="" class="form-control" />
            </div>
          </div>
          <div class="col-md-2 paddingleft0 pull-right">
            <div class="form-group">
              <label>Date:
              </label>
              <div class="input-group date">
                <div class="input-group-addon">
                  <i class="fa fa-calendar">
                  </i>
                </div>
                <input type="text" class="form-control pull-right" id="datepicker">
              </div>
              <!-- /.input group -->
            </div>
          </div> 
          <div class="clearfix">
          </div>
          <div class="col-md-2 paddingleft0">
            <div class="form-group">
              <label>Amount Rs :
              </label>
              <input type="text" id="" class="form-control" />
            </div>
            <a href=""> Multi-month deal? 
            </a>
          </div>
          <div class="col-md-2 paddingleft0">
            <div class="form-group">
              <label>Exp.close date :
              </label>
              <div class="input-group date">
                <div class="input-group-addon">
                  <i class="fa fa-calendar">
                  </i>
                </div>
                <input type="text" class="form-control pull-right" id="datepicker">
              </div>
              <!-- /.input group -->
            </div>
          </div>
          <div class="col-md-3 paddingleft0">
            <div class="form-group">
              <label>Deal Storage :
              </label>
              <select class="form-control select2 select2-hidden-accessible" style="width: 100%;" tabindex="-1" aria-hidden="true">
                <option selected="selected">Alabama
                </option>
                <option>Alaska
                </option>
                <option disabled="disabled">California (disabled)
                </option>
                <option>Delaware
                </option>
                <option>Tennessee
                </option>
                <option>Texas
                </option>
                <option>Washington
                </option>
              </select>
            </div>
          </div>
          <div class="col-md-4">
            <br />
            <a hreF="#">Won
            </a>&nbsp; &nbsp; &nbsp;
            <a hreF="#">Loss
            </a>
          </div>
          <div class="col-md-12 paddingleft0" style="margin-top:10px;">
            <div class="form-group">
              <textarea class="form-control" style="height:105px;" placeholder="Add Note">
              </textarea>
            </div>
          </div>
          <div class="col-md-6 paddingleft0">
            <button class="btn btn-primary">Save
            </button>
            <button class="btn btn-primary">Save
            </button>
          </div>
        </div>
      </div>
    </div>
       <div class="clearfix"></div>
    <div id="menu2" class="collapse">
      <h3>Add Call
      </h3>
      <div class="col-md-12" style="background: #f4f4f4; padding: 20px;border-radius: 9px;">
        <div class="col-md-12">
          <div class="col-md-3 paddingleft0">
            <div class="form-group">
              <label>Add Call :
              </label>
              <select class="form-control select2 select2-hidden-accessible" style="width: 100%;" tabindex="-1" aria-hidden="true">
                <option selected="selected">Alabama
                </option>
                <option>Alaska
                </option>
                <option disabled="disabled">California (disabled)
                </option>
                <option>Delaware
                </option>
                <option>Tennessee
                </option>
                <option>Texas
                </option>
                <option>Washington
                </option>
              </select>
            </div>
          </div>
          <div class="col-md-3 paddingleft0">
            <div class="form-group">
              <label>Result :
              </label>
              <select class="form-control select2 select2-hidden-accessible" style="width: 100%;" tabindex="-1" aria-hidden="true">
                <option selected="selected">Alabama
                </option>
                <option>Alaska
                </option>
                <option disabled="disabled">California (disabled)
                </option>
              </select>
            </div>
          </div>
          <div class="form-group">
            <textarea class="form-control" style="height:105px;" placeholder="Call Note">
            </textarea>
          </div>
          <div class="col-md-6 paddingleft0">
            <button class="btn btn-primary">Save
            </button>
            <button class="btn btn-primary">Save
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>
</div>
<!-- 5th section -->
<div class="col-md-12 col-sm-12 col-xs-12 bgwhite">
  <div class="col-md-2 col-sm-2 col-xs-12 paddingleft0">
    <h2>History
    </h2> 
  </div>
  <div class="col-md-6 col-sm-8 col-xs-12 paddingright0 pull-right paddingleft0"  style="margin-top:20px;margin-bottom:10px;">
    <ul style="text-align:right;" class="list-inline list-unstyled">
      <li>
        <span class="bg-green spanaction">All
        </span>
      </li>
      <li>
        <span class="bg-blue spanaction">Notes & Calls
        </span>
      </li>
      <li>
        <span class="bg-red spanaction">Deals
        </span>
      </li>
      <li>
        <span class="bg-yellow spanaction">Action
        </span>
      </li>
    </ul>
  </div>
</div>
<!-- end 5th section -->
<div class="col-md-12 col-sm-12 col-xs-12 bgwhite">
  <div class="row">
    <div class="col-md-12">
      <!-- The time line -->
      <ul class="timeline">
        <!-- timeline time label -->
        <li class="time-label">
          <span class="bg-red">
            NOTES
          </span>
        </li>
        <!-- /.timeline-label -->
        <!-- timeline item -->
        <li>
          <i class="fa fa-envelope bg-blue">
          </i>
          <div class="timeline-item">
            <span class="time">
              <i class="fa fa-clock-o">
              </i> 12:05
            </span>
            <h3 class="timeline-header">
              <a href="#">Support Team
              </a> sent you an email
            </h3>
            <div class="timeline-body">
              Etsy doostang zoodles disqus groupon greplin oooj voxy zoodles,
              weebly ning heekya handango imeem plugg dopplr jibjab, movity
              jajah plickers sifteo edmodo ifttt zimbra. Babblely odeo kaboodle
              quora plaxo ideeli hulu weebly balihoo...
            </div>
            <div class="timeline-footer">
              <a class="btn btn-primary btn-xs">Delete
              </a>
              <a class="btn btn-danger btn-xs">Edit
              </a>
            </div>
          </div>
        </li>
        <li class="time-label">
          <span class="bg-green">
            CALLS
          </span>
        </li>
        <!-- /.timeline-label -->
        <!-- timeline item -->
        <li>
          <i class="fa fa-envelope bg-blue">
          </i>
          <div class="timeline-item">
            <span class="time">
              <i class="fa fa-clock-o">
              </i> 12:05
            </span>
            <h3 class="timeline-header">
              <a href="#">Support Team
              </a> sent you an email
            </h3>
            <div class="timeline-body">
              Etsy doostang zoodles disqus groupon greplin oooj voxy zoodles,
              weebly ning heekya handango imeem plugg dopplr jibjab, movity
              jajah plickers sifteo edmodo ifttt zimbra. Babblely odeo kaboodle
              quora plaxo ideeli hulu weebly balihoo...
            </div>
            <div class="timeline-footer">
              <a class="btn btn-primary btn-xs">Delete
              </a>
              <a class="btn btn-danger btn-xs">Edit
              </a>
            </div>
          </div>
        </li>
        <!-- END timeline item -->
        <!-- timeline item -->
        <!-- END timeline item -->
        <li>
          <i class="fa fa-clock-o bg-gray">
          </i>
        </li>
      </ul>
    </div>
    <!-- /.col -->
  </div>
  <!-- /.row -->
  <div class="row" style="margin-top: 10px;">
    <!-- /.col -->
  </div>
  <!-- /.row -->
</div>
</section>



    <div class="clearfix"></div>


</asp:Content>
