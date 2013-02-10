﻿<%@ Page Title="" Language="C#" MasterPageFile="~/NestedMaster.master" AutoEventWireup="true"
         CodeBehind="Default.aspx.cs" Inherits="Plotter.Samples.Dygraph.NestedMasterPageUpdatePanel.Default"
         UICulture="en" Culture="en-US" EnableSessionState="False" %>

<%@ Register Assembly="Plotter.Controls.DygraphControl" Namespace="Plotter.Controls.DygraphControl"
             TagPrefix="asp" %>
<asp:Content ID="Header" ContentPlaceHolderID="NestedHead" runat="server">
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
</asp:Content>
<asp:Content ID="MainView" ContentPlaceHolderID="NestedMain" runat="server">
    <asp:ScriptManager ID="ScriptManager" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel" runat="server">
        <ContentTemplate>
            <asp:Dygraph ID="Dygraph" runat="server" Width="600px" Height="300px" 
                          EnableViewState="false">
            </asp:Dygraph>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ButtonRedraw" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:Button ID="ButtonRedraw" runat="server" Text="Redraw" OnClick="ButtonRedraw_Click" />
</asp:Content>