﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Plotter.Samples.JQPlot.XmlSample.Default"
         EnableSessionState="false" UICulture="en" Culture="en-US" %>

<%@ Register Assembly="Plotter.Controls.JQPlotControl" Namespace="Plotter.Controls.JQPlotControl"
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
        <title>Plotter.Samples.JQPlot.XmlSample</title>
        <link href="Styles/Default.css" rel="stylesheet" type="text/css" />
    </head>
    <body>
        <form id="form1" runat="server">
            <div class="container">
                <div class="settings">
                    <img src="Styles/Images/Settings-icon.png" alt="" />
                    <span>Options</span>
                    <div class="options">
                        <asp:CheckBox ID="CheckBoxAnimation" runat="server" Text="Enable animation" Checked="true" />
                        <br />
                        <asp:CheckBox ID="CheckBoxLabels" runat="server" Text="Show labels" Checked="true" />
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
                <asp:JQPlot ID="JQPlot" runat="server" EnableViewState="false" CssClass="jqPlot">
                </asp:JQPlot>
                <div id="metadata" runat="server">
                    <img src="Styles/Images/asset-green-16.png" alt="" />
                    <span>Description</span>
                    <div class="description">
                        <asp:Label ID="LabelDescription" runat="server" Text="Label"></asp:Label>
                    </div>
                </div>
            </div>
        </form>
    </body>
</html>