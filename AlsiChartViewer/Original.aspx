<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Original.aspx.cs" Inherits="WebApplication1.Original" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="/Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script src="/Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="/Scripts/highcharts/highcharts.js"></script>
    <script type="text/javascript" src="/Scripts/highcharts/modules/exporting.js"></script>
</head>
<body>
    <form id="form1" runat="server">
   <div id="linecontainer" style="width: 500px; height: 250px; margin: 0 auto">
    </div>
    <div id="columncontainer" style="width: 500px; height: 250px; margin: 0 auto">
    </div>
    <div id="piecontainer" style="width: 500px; height: 250px; margin: 0 auto">
    </div>
    </form>
</body>
</html>
