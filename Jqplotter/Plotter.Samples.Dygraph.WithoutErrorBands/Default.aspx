<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Plotter.Samples.Dygraph.WithoutErrorBands.Default"
    UICulture="en" Culture="en-US" EnableSessionState="False" %>

<%@ Register Assembly="Plotter.Controls.DygraphControl" Namespace="Plotter.Controls.DygraphControl"
    TagPrefix="asp" %>
<!DOCTYPE HTML>
<html>
<!--

        Copyright 2012 Akram El Assas.

        Licensed to the Apache Software Foundation (ASF) under one
        or more contributor license agreements. See the NOTICE file
        distributed with this work for additional information
        regarding copyright ownership. The ASF licenses this file
        to you under the Apache License, Version 2.0 (the
        "License"); you may not use this file except in compliance
        with the License. You may obtain a copy of the License at

        http://www.apache.org/licenses/LICENSE-2.0

        Unless required by applicable law or agreed to in writing,
        software distributed under the License is distributed on an
        "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
        KIND, either express or implied. See the License for the
        specific language governing permissions and limitations
        under the License.

        -->
<head runat="server">
    <title>Plotter.Samples.Dygraph.WithoutErrorBands</title>
    <link href="Styles/Default.css" rel="stylesheet" type="text/css" />
    <link href="Styles/colorpicker/css/colorpicker.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/jquery.min.js" type="text/javascript"> </script>
    <script src="Scripts/default.js" type="text/javascript"> </script>
    <script src="Scripts/colorpicker.js" type="text/javascript"> </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanelContainer" runat="server">
        <ContentTemplate>
            <div class="container">
                <div class="settings">
                    <img src="Styles/Images/Settings-icon.png" alt="" />
                    <span>Options</span>
                    <div class="options">
                        <table>
                            <tr>
                                <td colspan="2">
                                    <asp:CheckBox ID="CheckBoxLabels" runat="server" Text="Show labels" Checked="true" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Grid line color
                                </td>
                                <td>
                                    <asp:TextBox ID="TextBoxColor" runat="server" Text="FF0000" Width="50px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Circle size
                                </td>
                                <td>
                                    <asp:TextBox ID="TextBoxCircleSize" runat="server" Text="7" Width="50px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Stroke width
                                </td>
                                <td>
                                    <asp:TextBox ID="TextBoxStrokeWidth" runat="server" Text="4" Width="50px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:CheckBox ID="CheckBoxShowRangeSelector" runat="server" Text="Show Range Selector"
                                        Checked="true" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Range Selector Height
                                </td>
                                <td>
                                    <asp:TextBox ID="TextBoxRangeSelectorHeight" runat="server" Text="30" Width="50px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Range Selector Plot Stroke Color
                                </td>
                                <td>
                                    <asp:TextBox ID="TextBoxRangeSelectorPlotStrokeColor" runat="server" Text="FFFF00"
                                        Width="50px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Range Selector Plot Fill Color
                                </td>
                                <td>
                                    <asp:TextBox ID="TextBoxRangeSelectorPlotFillColor" runat="server" Text="FFFFE0"
                                        Width="50px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:CheckBox ID="CheckBoxAnimatedZooms" runat="server" Text="Animated Zooms" Checked="false"
                                        ToolTip="Must be disabled if the range selector is enabled." />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <asp:Repeater ID="RepeaterPlots" runat="server" OnItemCommand="RepeaterImages_ItemCommand">
                    <HeaderTemplate>
                        <ul class="thumb">
                    </HeaderTemplate>
                    <ItemTemplate>
                        <li>
                            <asp:LinkButton ID="LinkButtonThumbnail" runat="server" CommandName="Plot" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>'>
                                        <img src='<%# DataBinder.Eval(Container.DataItem, "ThumbnailPath") %>' alt="" title='<%# DataBinder.Eval(Container.DataItem, "Title") %>' />
                            </asp:LinkButton>
                        </li>
                    </ItemTemplate>
                    <FooterTemplate>
                        </ul>
                    </FooterTemplate>
                </asp:Repeater>
                <div id="pager">
                    <asp:Label ID="LabelCurrentPage" runat="server"></asp:Label>
                    <asp:ImageButton ID="ImageButtonPrevious" runat="server" ImageUrl="Styles/Images/Arrow-Left-icon.png"
                        OnClick="ImageButtonPrevious_Click" CssClass="previous" />
                    <asp:ImageButton ID="ImageButtonNext" runat="server" ImageUrl="Styles/Images/Arrow-right-icon.png"
                        OnClick="ImageButtonNext_Click" CssClass="next" />
                </div>
                <asp:Dygraph ID="Dygraph" runat="server" CssClass="dygraph" EnableViewState="false">
                </asp:Dygraph>
                <div id="metadata" runat="server">
                    <img src="Styles/Images/asset-green-16.png" alt="" />
                    <span>Description</span>
                    <div class="description">
                        <asp:Label ID="LabelDescription" runat="server" Text="Label"></asp:Label>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
