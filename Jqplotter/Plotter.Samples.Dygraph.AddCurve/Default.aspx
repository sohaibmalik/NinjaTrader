<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Plotter.Samples.Dygraph.AddCurve.Default"
         UICulture="en" Culture="en-US" EnableSessionState="False" %>

<%@ Register Assembly="Plotter.Controls.DygraphControl" Namespace="Plotter.Controls.DygraphControl" TagPrefix="asp" %>
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
        <title>Plotter.Samples.Dygraph.AddCurve</title>
    </head>
    <body>
        <form id="form1" runat="server">
            <div>
                <asp:Dygraph ID="Dygraph" runat="server" Width="600px" Height="300px" 
                             >
                </asp:Dygraph>
                <asp:Button ID="ButtonAddCurve" runat="server" Text="Add Curve" OnClick="ButtonAddCurve_Click" />
            </div>
        </form>
    </body>
</html>