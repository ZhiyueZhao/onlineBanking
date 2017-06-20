<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="wfClient.aspx.cs" Inherits="wfClient" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:Label ID="Label1" runat="server" Text="Client Number: "></asp:Label>
    <asp:Label ID="lblClientNumber" runat="server"></asp:Label>
    <br />
    <br />
    <asp:GridView ID="gvAccounts" runat="server" 
    AutoGenerateSelectButton="True" AutoGenerateColumns="False" Height="122px" 
        onselectedindexchanged="gvAccounts_SelectedIndexChanged" Width="335px">
        <Columns>
            <asp:BoundField HeaderText="Account Number" DataField="AccountNumber" />
            <asp:BoundField HeaderText="Account Note" DataField="Notes" />
            <asp:BoundField HeaderText="Balance" DataField="Balance" 
                DataFormatString="{0:c}" >
            <ItemStyle HorizontalAlign="Right" />
            </asp:BoundField>
        </Columns>
    </asp:GridView>
    <br />
&nbsp;<asp:Label ID="lblExchangeRate" runat="server"></asp:Label>
    <asp:Label ID="lblWarnings" runat="server" ForeColor="Red"></asp:Label>
    <br />
</asp:Content>

