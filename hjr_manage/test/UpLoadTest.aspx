<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UpLoadTest.aspx.cs" Inherits="test_UpLoadTest" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server" enctype="multipart/form-data">
        <div>
        <%--<form method="post" name="myForm" action="../Operate/Admin_add.ashx?T=web_add" id="myForm" enctype="multipart/form-data">--%>
        <table cellpadding="0" cellspacing="0" width="100%" border="0" align="center"  runat="server">
            <tr>
                <td colspan="2" height="10">
                    &nbsp;
                </td>
            </tr>
            
           <tr>
                <td width="150"  height="33" class="td_center" >
                    aaaa
                </td>
                <td align="left" style="padding:5px 0px 5px 0px;">
                
                   <input type="file" name="file" />

                </td>
            </tr>
            <tr>
                <td></td>
                <td height="60" align="left">
                    
                    <asp:Button ID="Button1" runat="server" Text="上传" OnClick="Button1_Click" />
                    
                </td>
            </tr>
        </table>
        </div>
    </form>

        <script type="text/javascript">
        // 等待DOM加载
        $(document).ready(function () {
            // 绑定"#myForm"，然后加入回调函数 
            $('#myForm').ajaxForm(function (data) {
                if (data.indexOf("success") >= 0) {
                    $.messager.confirm('温馨提示', '添加成功，是否继续添加?', function (r) {
                        if (r) {
                            location.reload();
                        } else {
                            location.href = 'g_web.aspx?t=[table]&n=[n]';
                        }
                    });
                } else {
                    $.messager.alert('温馨提示', '提交失败：' + data, 'warning');
                }
            });
        });


    </script>
</body>
</html>
