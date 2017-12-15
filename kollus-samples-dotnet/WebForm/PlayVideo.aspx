<%@ Page Language="C#" AutoEventWireup="true"MasterPageFile="~/Site.Master"  CodeBehind="PlayVideo.aspx.cs" Inherits="kollus_samples_dotnet.PlayVideo" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <p><%: Title %></p>
    <iframe id="wPlayer" runat="server" allowfullscreen></iframe>
</asp:Content>

