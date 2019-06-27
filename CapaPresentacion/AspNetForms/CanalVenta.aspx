<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CanalVenta.aspx.cs" Inherits="CapaPresentacion.AspNetForms.CanalVenta" %>

<%@ Register assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div id="dvReport">
    
       
    
        <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server"  HasCrystalLogo="False"
            HasRefreshButton="True" EnableDatabaseLogonPrompt="False"  PrintMode="Pdf"/>
    
       
    
    </div>
    </form>
</body>
</html>
