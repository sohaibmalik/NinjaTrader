<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Trades.aspx.cs" Inherits="Trades" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ListView ID="ListView1" runat="server" DataSourceID="SqlDataSource1">
            <AlternatingItemTemplate>
                <tr style="background-color:#FAFAD2; color: #284775;">
                    <td>
                        <asp:Label ID="TimeLabel" runat="server" Text='<%# Eval("Time") %>' />
                    </td>
                    <td>
                        <asp:Label ID="BuySellLabel" runat="server" Text='<%# Eval("BuySell") %>' />
                    </td>
                    <td>
                        <asp:Label ID="ReasonLabel" runat="server" Text='<%# Eval("Reason") %>' />
                    </td>
                    <td>
                        <asp:Label ID="PriceLabel" runat="server" Text='<%# Eval("Price") %>' />
                    </td>
                    <td>
                        <asp:Label ID="VolumeLabel" runat="server" Text='<%# Eval("Volume") %>' />
                    </td>
                    <td>
                        <asp:CheckBox ID="MatchedCheckBox" runat="server" 
                            Checked='<%# Eval("Matched") %>' Enabled="false" />
                    </td>
                </tr>
            </AlternatingItemTemplate>
            <EditItemTemplate>
                <tr style="background-color:#FFCC66; color: #000080;">
                    <td>
                        <asp:Button ID="UpdateButton" runat="server" CommandName="Update" 
                            Text="Update" />
                        <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" 
                            Text="Cancel" />
                    </td>
                    <td>
                        <asp:TextBox ID="TimeTextBox" runat="server" Text='<%# Bind("Time") %>' />
                    </td>
                    <td>
                        <asp:TextBox ID="BuySellTextBox" runat="server" Text='<%# Bind("BuySell") %>' />
                    </td>
                    <td>
                        <asp:TextBox ID="ReasonTextBox" runat="server" Text='<%# Bind("Reason") %>' />
                    </td>
                    <td>
                        <asp:TextBox ID="PriceTextBox" runat="server" Text='<%# Bind("Price") %>' />
                    </td>
                    <td>
                        <asp:TextBox ID="VolumeTextBox" runat="server" Text='<%# Bind("Volume") %>' />
                    </td>
                    <td>
                        <asp:CheckBox ID="MatchedCheckBox" runat="server" 
                            Checked='<%# Bind("Matched") %>' />
                    </td>
                </tr>
            </EditItemTemplate>
            <EmptyDataTemplate>
                <table runat="server" 
                    style="background-color: #FFFFFF;border-collapse: collapse;border-color: #999999;border-style:none;border-width:1px;">
                    <tr>
                        <td>
                            No data was returned.</td>
                    </tr>
                </table>
            </EmptyDataTemplate>
            <InsertItemTemplate>
                <tr style="">
                    <td>
                        <asp:Button ID="InsertButton" runat="server" CommandName="Insert" 
                            Text="Insert" />
                        <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" 
                            Text="Clear" />
                    </td>
                    <td>
                        <asp:TextBox ID="TimeTextBox" runat="server" Text='<%# Bind("Time") %>' />
                    </td>
                    <td>
                        <asp:TextBox ID="BuySellTextBox" runat="server" Text='<%# Bind("BuySell") %>' />
                    </td>
                    <td>
                        <asp:TextBox ID="ReasonTextBox" runat="server" Text='<%# Bind("Reason") %>' />
                    </td>
                    <td>
                        <asp:TextBox ID="PriceTextBox" runat="server" Text='<%# Bind("Price") %>' />
                    </td>
                    <td>
                        <asp:TextBox ID="VolumeTextBox" runat="server" Text='<%# Bind("Volume") %>' />
                    </td>
                    <td>
                        <asp:CheckBox ID="MatchedCheckBox" runat="server" 
                            Checked='<%# Bind("Matched") %>' />
                    </td>
                </tr>
            </InsertItemTemplate>
            <ItemTemplate>
                <tr style="background-color:#FFFBD6; color: #333333;">
                    <td>
                        <asp:Label ID="TimeLabel" runat="server" Text='<%# Eval("Time") %>' />
                    </td>
                    <td>
                        <asp:Label ID="BuySellLabel" runat="server" Text='<%# Eval("BuySell") %>' />
                    </td>
                    <td>
                        <asp:Label ID="ReasonLabel" runat="server" Text='<%# Eval("Reason") %>' />
                    </td>
                    <td>
                        <asp:Label ID="PriceLabel" runat="server" Text='<%# Eval("Price") %>' />
                    </td>
                    <td>
                        <asp:Label ID="VolumeLabel" runat="server" Text='<%# Eval("Volume") %>' />
                    </td>
                    <td>
                        <asp:CheckBox ID="MatchedCheckBox" runat="server" 
                            Checked='<%# Eval("Matched") %>' Enabled="false" />
                    </td>
                </tr>
            </ItemTemplate>
            <LayoutTemplate>
                <table runat="server">
                    <tr runat="server">
                        <td runat="server">
                            <table ID="itemPlaceholderContainer" runat="server" border="1" 
                                style="background-color: #FFFFFF;border-collapse: collapse;border-color: #999999;border-style:none;border-width:1px;font-family: Verdana, Arial, Helvetica, sans-serif;">
                                <tr runat="server" style="background-color:#FFFBD6; color: #333333;">
                                    <th runat="server">
                                        Time</th>
                                    <th runat="server">
                                        BuySell</th>
                                    <th runat="server">
                                        Reason</th>
                                    <th runat="server">
                                        Price</th>
                                    <th runat="server">
                                        Volume</th>
                                    <th runat="server">
                                        Matched</th>
                                </tr>
                                <tr ID="itemPlaceholder" runat="server">
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr runat="server">
                        <td runat="server" 
                            
                            style="text-align: center;background-color: #FFCC66; font-family: Verdana, Arial, Helvetica, sans-serif;color: #333333;">
                            <asp:DataPager ID="DataPager1" runat="server">
                                <Fields>
                                    <asp:NextPreviousPagerField ButtonType="Button" ShowFirstPageButton="True" 
                                        ShowNextPageButton="False" ShowPreviousPageButton="False" />
                                    <asp:NumericPagerField />
                                    <asp:NextPreviousPagerField ButtonType="Button" ShowLastPageButton="True" 
                                        ShowNextPageButton="False" ShowPreviousPageButton="False" />
                                </Fields>
                            </asp:DataPager>
                        </td>
                    </tr>
                </table>
            </LayoutTemplate>
            <SelectedItemTemplate>
                <tr style="background-color:#FFCC66; font-weight: bold;color: #000080;">
                    <td>
                        <asp:Label ID="TimeLabel" runat="server" Text='<%# Eval("Time") %>' />
                    </td>
                    <td>
                        <asp:Label ID="BuySellLabel" runat="server" Text='<%# Eval("BuySell") %>' />
                    </td>
                    <td>
                        <asp:Label ID="ReasonLabel" runat="server" Text='<%# Eval("Reason") %>' />
                    </td>
                    <td>
                        <asp:Label ID="PriceLabel" runat="server" Text='<%# Eval("Price") %>' />
                    </td>
                    <td>
                        <asp:Label ID="VolumeLabel" runat="server" Text='<%# Eval("Volume") %>' />
                    </td>
                    <td>
                        <asp:CheckBox ID="MatchedCheckBox" runat="server" 
                            Checked='<%# Eval("Matched") %>' Enabled="false" />
                    </td>
                </tr>
            </SelectedItemTemplate>
        </asp:ListView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
            ConnectionString="<%$ ConnectionStrings:AlsiDbConnectionString %>" 
            
            SelectCommand="SELECT [Time], [BuySell], [Reason], [Price], [Volume], [Matched] FROM [TradeLog]">
        </asp:SqlDataSource>
    <br />
       


    </div>
    </form>
</body>
</html>
