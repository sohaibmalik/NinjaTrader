<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ChartForm.aspx.cs" Inherits="ChartForm" %>

<%@ Register assembly="Plotter.Controls.FlotControl" namespace="Plotter.Controls.FlotControl" tagprefix="asp" %>
<%@ Register assembly="Plotter.Controls.GoogleChartControl" namespace="Plotter.Controls.GoogleChartControl" tagprefix="cc1" %>

<%@ Register assembly="Plotter.Controls.DygraphControl" namespace="Plotter.Controls.DygraphControl" tagprefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body style="height: 568px; width: 1018px">
    <form id="form1" runat="server">
    <div style="height: 577px; width: 1125px">
    
        <asp:Dygraph ID="Dygraph1" runat="server" Height="529px">
        </asp:Dygraph>
    
    </div>
    <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
    <br />
    </form>
</body>
</html>
