<%@ Page Language="C#" AutoEventWireup="true"MasterPageFile="~/Site.Master"  CodeBehind="DownloadVideo.aspx.cs" Inherits="kollus_samples_dotnet.DownloadVideo" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <p><%: Title %></p>
    <iframe id="wPlayer" runat="server" allowfullscreen></iframe>
</asp:Content>
