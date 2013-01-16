<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Plotter.Samples.Flot.UpdatePanel.Default"
         EnableSessionState="false" UICulture="en" Culture="en-US" %>

<%@ Register Assembly="Plotter.Controls.FlotControl" Namespace="Plotter.Controls.FlotControl"
             TagPrefix="asp" %>
<!DOCTYPE html>
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
        <title>Plotter.Samples.Flot.UpdatePanel</title>
    </head>
    <body>
        <form id="form1" runat="server">
            <asp:ScriptManager ID="ScriptManager" runat="server">
            </asp:ScriptManager>
            <div>
                <asp:UpdatePanel ID="UpdatePanel" runat="server">
                    <ContentTemplate>
                        <asp:Flot ID="Flot" runat="server" Width="600px" Height="300px" EnableViewState="false">
                        </asp:Flot>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ButtonRedraw" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
                <br />
                <asp:Button ID="ButtonRedraw" runat="server" Text="Redraw" OnClick="ButtonRedraw_Click" />
            </div>
        </form>
    </body>
</html>