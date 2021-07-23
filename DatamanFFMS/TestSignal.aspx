<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestSignal.aspx.cs" Inherits="AstralFFMS.TestSignal" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
<form id="frmHome" runat="server">
    <div>
        <ul id="ulInbox">
            <li><span id="lblName">Name:</span> <span id="lblMessage">Message</span></li>
        </ul>
        <ul>
            <li>
                <input type="text" id="txtMessage" value="" />
                <input type="button" id="btnSend" value="Send" />
                <input type="hidden" id="txtName" />
            </li>
        </ul>
    </div>
    </form>
<script src="http://code.jquery.com/jquery-1.8.2.min.js" type="text/javascript"></script>
<%--<script src="Scripts/jquery.signalR-1.0.1.min.js" type="text/javascript"></script>--%>
    <script src="Scripts/jquery.signalR-2.4.0.min.js"></script>
<script src="signalr/hubs" type="text/javascript"></script>
<script type="text/javascript">
    $(function () {
        // Proxy created on the fly          
        var chat = $.connection.chatHub;
        // Get the user name and store it to prepend to messages.
        $('#txtName').val(prompt('Enter your name:', ''));
        // Declare a function on the chat hub so the server can invoke it      
        chat.client.sendMessage = function (name, message) {
            var username = $('<div />').text(name).html();
            var chatMessage = $('<div />').text(message).html();
            $('#ulInbox').append('<li>' + username + ':&nbsp;&nbsp;' + chatMessage + '</li>');
        };
        // Start the connection
        $.connection.hub.start().done(function () {
            $('#btnSend').click(function () {
                // Call the chat method on the server
                chat.server.send($('#txtName').val(), $('#txtMessage').val());
                $('#txtMessage').val('');
            });
        });
    });
    </script>
</body>
</html>
