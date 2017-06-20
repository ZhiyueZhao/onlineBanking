<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="wfTransaction.aspx.cs" Inherits="wfTransaction" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <table style="width: 100%; margin-right: 0px;">
        <tr>
            <td class="transactionlable">
                <asp:Label ID="Label1" runat="server" style="float: none; " 
        Text="Account Number:"></asp:Label>
            </td>
            <td>
    <asp:Label ID="lblAccountNumber" runat="server" 
        
        style="left: -575px; top: 0px; height: 17px; width: 200px; float: none; right: 0px;"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="transactionlable">
    <asp:Label ID="Label2" runat="server" 
        style="float: none; top: 30px; left: -100px" 
        Text="Balance:"></asp:Label>
            </td>
            <td>
    <asp:Label ID="lblBalance" runat="server" 
        
        style="top: 30px; left: -575px; height: 17px; width: 200px; float: none;"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="transactionlable">
    <asp:Label ID="Label3" runat="server" 
        style="float: none; top: 60px; left: -148px" 
        Text="Transaction Type:"></asp:Label>
            </td>
            <td>
    <asp:DropDownList ID="ddlTransactionType" runat="server" AutoPostBack="True" 
        
        style="top: 176px; left: 354px; height: 22px; width: 150px;" 
        onselectedindexchanged="ddlTransactionType_SelectedIndexChanged">
        <asp:ListItem>Pay Bills</asp:ListItem>
        <asp:ListItem Value="Transfer Funds"></asp:ListItem>
    </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="transactionlable">
    <asp:Label ID="Label4" runat="server" 
        style="top: 90px; float: none; left: -250px" Text="Amount:"></asp:Label>
            </td>
            <td>
    <asp:TextBox ID="txtAmount" runat="server" style="top: 206px; left: 353px;" class="amount" Width="186px"></asp:TextBox>
                <asp:RangeValidator ID="rnvAmount" runat="server" ControlToValidate="txtAmount" 
                    ErrorMessage="Amount must be between 0.01 and 10,000.00" 
                    MaximumValue="10000.00" MinimumValue="0.01" SetFocusOnError="True" 
                    Type="Currency"></asp:RangeValidator>
            </td>
        </tr>
        <tr>
            <td class="transactionlable">
    <asp:Label ID="Label5" runat="server" 
        style="top: 120px; float: none; left: -298px" Text="To:"></asp:Label>
            </td>
            <td>
    <asp:DropDownList ID="ddlAccountPayee" runat="server" AutoPostBack="True" 
        
        style="top: 236px; left: 353px; height: 22px; width: 150px;">
    </asp:DropDownList>
            </td>
        </tr>
    </table>
    <asp:Button ID="btnSubmit" runat="server" Font-Bold="True" 
        Font-Underline="False" style="top: 266px; float: none; left: 220px; " 
        Text="Complete Transaction" onclick="btnSubmit_Click" Width="165px" />
            
    <br />
    <br />
            
    <asp:Label ID="Label6" runat="server" 
        style="top: 190px; float: none; left: -316px" 
        Text="Transaction Summary"></asp:Label>
            
    <br />
    <br />
            
    <asp:TextBox ID="txtSummary" runat="server" Height="165px" 
        style="top: 200px; float: none; left: 0px; " 
        TextMode="MultiLine" Width="322px" ReadOnly="True"></asp:TextBox>
</asp:Content>

