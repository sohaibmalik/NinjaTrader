<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        ALSI Trade<br />
        <asp:TextBox ID="pswTextBox" runat="server" Width="183px"></asp:TextBox>
        <br />
        <asp:Button ID="Button1" runat="server" onclick="Button1_Click" 
            Text="Restart" Height="89px" Width="187px" />
    
        <br />
        <br />
        <br />
        <br />
        REFRESH STATUS<br />
        <asp:Button ID="Button2" runat="server" Height="105px" onclick="Button2_Click" 
            Text="REFRESH" Width="181px" />
        <br />
        <br />
        <asp:Label ID="alsiTradeLabel" runat="server" Text="Alsi Trade"></asp:Label>
        <br />
        <br />
        <asp:Label ID="dataManagerLabel" runat="server" Text="Data Manager"></asp:Label>
    
        <br />
        <br />
        <a href="ChartForm.aspx">ChartForm.aspx</a><br />
        <br />
        <br />
        <br />
    
    </div>
    </form>
</body>
</html>
