﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReporteObs.aspx.cs" Inherits="CapaPresentacion.AspNetForms.ReporteObs" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
      <div id="dvReport">
        <CR:CrystalReportViewer ID="crv_obs" runat="server" HasCrystalLogo="False"
            HasRefreshButton="True" EnableDatabaseLogonPrompt="False"  PrintMode="Pdf" OnUnload="crv_obs_Unload"   />   
    </div>
    </form>
</body>
</html>
