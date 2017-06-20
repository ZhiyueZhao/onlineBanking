<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="wfAccount.aspx.cs" Inherits="wfAccount" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <p>
        <asp:Label ID="Label1" runat="server" Text="Client Number: "></asp:Label>
        <asp:Label ID="lblClientNumber" runat="server"></asp:Label>
    </p>
    <p>
        <asp:Label ID="Label2" runat="server" Text="Account Number: "></asp:Label>
        <asp:Label ID="lblAccountNumber" runat="server"></asp:Label>
&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Label ID="Label3" runat="server" Text="Balance: "></asp:Label>
        <asp:Label ID="lblBalance" runat="server"></asp:Label>
    </p>
    <p>
        <asp:GridView ID="gvAccountDetail" runat="server" AutoGenerateColumns="False" 
            Width="483px">
            <Columns>
                <asp:BoundField DataField="DateCreated" 
                    HeaderText="Date" >
                <ItemStyle Width="145px" />
                </asp:BoundField>
                <asp:BoundField DataField="Description" HeaderText="Transaction Type" />
                <asp:BoundField DataField="Deposit" DataFormatString="{0:c}" 
                    HeaderText="Amount In" >
                <ItemStyle HorizontalAlign="Right" />
                </asp:BoundField>
                <asp:BoundField DataField="Withdrawal" DataFormatString="{0:c}" 
                    HeaderText="Amount Out" >
                <ItemStyle HorizontalAlign="Right" />
                </asp:BoundField>
                <asp:BoundField DataField="Notes" HeaderText="Details" />
            </Columns>
        </asp:GridView>
    </p>
    <p>
        <asp:LinkButton ID="lnkPay" runat="server" onclick="lnkPay_Click">Pay Bills and Transfer Funds</asp:LinkButton>
        <asp:Label ID="lblWarnings" runat="server" ForeColor="Red"></asp:Label>
    </p>
</asp:Content>

