<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GuiaEcommerce.aspx.cs" Inherits="CapaPresentacion.AspNetForms.GuiaEC" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
   <%-- <link href="../Content/bootstrap.css" rel="stylesheet" />
    <link href="../Content/bootstrap.min.css" rel="stylesheet" />
    <link href="../Content/Site.css" rel="stylesheet" />--%>
</head>
<body>
    <form id="form1" runat="server">
      <%--  <asp:Button runat="server" ID="btnPrint" CssClass="btn-danger" OnClick="Print" Text="Imprimir" />--%>
        <%--<asp:Button ID="print" runat="server" Text="Button" OnClick="print_Click" />
        <input id="btnPrint" type="button" value="Print" onclick="Print()" />--%>
    <div id="dvReport">
        <CR:CrystalReportViewer ID="crv_guiaEC" runat="server" HasCrystalLogo="False"
            HasRefreshButton="True" EnableDatabaseLogonPrompt="False"  PrintMode="Pdf"   />   
    </div>
    </form>
</body>
</html>
<%-- <script type="text/javascript"> 


       function Print() { 
         var dvReport = document.getElementById("dvReport"); 
         var frame1 = dvReport.getElementsByTagName("iframe")[0]; 
         if (navigator.appName.indexOf("Internet Explorer") != -1) { 
          frame1.name = frame1.id; 
          window.frames[frame1.id].focus(); 
          window.frames[frame1.id].print(); 
         } 
         else { 
          var frameDoc = frame1.contentWindow ? frame1.contentWindow : frame1.contentDocument.document ? frame1.contentDocument.document : frame1.contentDocument; 
          frameDoc.print(); 
         } 
        } 
      </script> --%>