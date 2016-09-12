<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Bayes_Learn.aspx.cs" Inherits="Bayes_Learn" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
        #form1
        {
            width: 1694px;
            height: 731px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
 

  <div id="Div5" runat="server" 
        style="border: medium solid #0000FF; height:270px; position:absolute; top: 10px; left: 1px; width: 1286px; margin-left:10px;" 
        align="left">
       <asp:Label ID="Label35" runat="server" Font-Size="X-Large" Height="25px" 
           Text="请选择学习样本集：" Width="230px"></asp:Label>
       <asp:DropDownList ID="DropDownList5" runat="server" BackColor="#CCFFFF" 
           Font-Size="X-Large" Height="32px" Width="253px">
          <%-- <asp:ListItem Value="0">请选择</asp:ListItem>--%>
          <%-- <asp:ListItem Value="1">根据标准生成样本</asp:ListItem>--%>
           <asp:ListItem Value="2">来自实际样本</asp:ListItem>
       </asp:DropDownList>
       &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;
       <br />
       <br />
            
        <asp:Label ID="Label10" runat="server" Text="训练集表名："  Font-Size="X-Large"  
             Height="25px" Width="160px"></asp:Label>
        <asp:TextBox  Enabled="false" ID="TextBox8" runat="server" Text="" BackColor="#CCFFFF" 
             Font-Size="Large" Height="25px" Width="143px"></asp:TextBox>
         &nbsp;&nbsp;<asp:Label ID="Label11" runat="server" Text="验证集表名：" Font-Size="X-Large"  Height="25px" Width="160px"></asp:Label>
        <asp:TextBox ID="TextBox11"  Enabled="false" runat="server" Text="" BackColor="#CCFFFF" 
             Font-Size="Large" Height="25px" Width="138px"></asp:TextBox>
              &nbsp;&nbsp;
              <asp:Label ID="Label8" runat="server" Text="训练样本:验证样本："  Font-Size="X-Large"  Height="25px" Width="230px"></asp:Label>
        <asp:TextBox ID="TextBox6" runat="server" Text="" BackColor="#CCFFFF" 
           Font-Size="Large" Height="25px" Width="57px"></asp:TextBox>
        <asp:Label ID="Label9" runat="server" Text="："  BackColor="#CCFFFF" 
           Font-Size="Large"  Height="28px" Width="20px"></asp:Label>
        <asp:TextBox ID="TextBox7" runat="server" Text="" BackColor="#CCFFFF" 
           Font-Size="Large" Height="25px" Width="60px"></asp:TextBox>
         &nbsp;&nbsp;<br />
       <br />
       <asp:Label ID="Label1" runat="server" Text="样本总数:"  
           Font-Size="X-Large"  Height="25px" Width="113px"></asp:Label>
        <asp:TextBox ID="TextBox1"  Enabled="false" runat="server" Text="" 
           BackColor="#CCFFFF" Font-Size="X-Large" Height="25px" Width="67px"></asp:TextBox>
         &nbsp;&nbsp;<asp:Label ID="Label17" runat="server" Text="训练样本数:"  
           Font-Size="X-Large"  Height="25px" Width="143px"></asp:Label>
        <asp:TextBox ID="TextBox14"  Enabled="false" runat="server" Text="" 
           BackColor="#CCFFFF" Font-Size="X-Large" Height="25px" Width="63px"></asp:TextBox>
         &nbsp;<asp:Label ID="Label7" runat="server" Text="验证样本数:"  Font-Size="X-Large"  Height="25px" Width="150px"></asp:Label>
        <asp:TextBox ID="TextBox15" Enabled="false" runat="server" Text="" BackColor="#CCFFFF" Font-Size="X-Large" Height="25px" Width="80px"></asp:TextBox>
        &nbsp;&nbsp;
         &nbsp;&nbsp;
               &nbsp;&nbsp; &nbsp;&nbsp;
              <br />
              <br />
             
          <asp:Label ID="Label20" runat="server" Text="类概率密度函数:"  Font-Size="X-Large"  
            Height="25px" Width="193px" ></asp:Label>
        <asp:DropDownList ID="DropDownList1" runat="server" BackColor="#CCFFFF"  
            Font-Size="X-Large" Height="32px" Width="207px">
            <asp:ListItem Value = "0">正态分布</asp:ListItem>
        </asp:DropDownList>
        
         &nbsp; &nbsp; &nbsp;&nbsp;
           &nbsp;
           &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:Button  ID="Button2" runat="server"  Text="开始学习"  Font-Size="X-Large" onclick="Button7_Click" />
         &nbsp;&nbsp; 
       &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
              
        </br >
        
         
        <asp:Label ID="Label23" runat="server" Text="正确识别数：" 
            Font-Size="X-Large"  Height="25px" Width="147px" ></asp:Label>
        <asp:TextBox ID="TextBox25"  Enabled="false" runat="server" Text="" BackColor="#CCFFFF" 
            Font-Size="X-Large" Height="25px" Width="77px" ></asp:TextBox>
 &nbsp;&nbsp;
         <asp:Label ID="Label32" runat="server" Text="总识别率(%)：" 
            Font-Size="X-Large"  Height="25px" Width="180px" ></asp:Label>
        <asp:TextBox ID="TextBox26"  Enabled="false" runat="server" Text="" BackColor="#CCFFFF" 
            Font-Size="X-Large" Height="25px" Width="76px" ></asp:TextBox>
           
        <br />
         <br />
           </div >
                   
        </br>
         <div id="Div6" runat="server" 
          style="border: medium solid #0000FF; height:516px; position:absolute; top:372px; left: 13px; width: 1386px; margin-left:1px;" 
          align="left">
          </br >
         &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp<asp:Button  
                 ID="Button1" runat="server"  Text="自动寻优（列出不同概率密度函数下的最大总识别率和各类识别率）"  
                 Font-Size="X-Large" onclick="Button1_Click" Width="844px" />
         </br >
         </br >
           <asp:GridView ID="GridView4" runat="server" CellPadding="3" ForeColor="Black" 
            GridLines="Vertical" style="font-size: large;" Width="1268px" 
            PageSize="5" AllowPaging="True" BackColor="#339966" BorderColor="#0066CC" BorderStyle="Solid" 
            BorderWidth="1px" OnPageIndexChanging="GridView4_PageIndexChanging" 
                 OnPageIndexChanged="GridView4_PageIndexChanged" Height="414px">
            <AlternatingRowStyle BackColor="#CCCCCC" />
            <FooterStyle BackColor="#CCCCCC" />
            <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
            <SortedAscendingCellStyle BackColor="#F1F1F1" />
            <SortedAscendingHeaderStyle BackColor="#808080" />
            <SortedDescendingCellStyle BackColor="#CAC9C9" />
            <SortedDescendingHeaderStyle BackColor="#383838" />
       </asp:GridView>
       </div>
       
         

    </form>
</body>
</html>
