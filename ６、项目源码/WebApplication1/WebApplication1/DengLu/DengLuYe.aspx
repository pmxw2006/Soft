<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DengLuYe.aspx.cs" Inherits="WebApplication1.DengLu.DengLuYe" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
	<head runat="server">
		<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
		<title>固定资产管理系统 - 登录</title>
		<script type="text/javascript" src="js/jquery-3.3.1.min.js"></script>
        <link href="/DengLu/css/DengLuYe.css" rel="stylesheet" />
	</head>
	<body>
		<form id="form1" class="denglu-rongqi" runat="server">
			<div class="denglu-toubu">
				<h1>固定资产管理系统</h1>
				<p>Fixed Assets Management System</p>
			</div>

			<div class="denglu-zhuti">
				<div class="biaodan-zu">
					<label for="yonghu-ming">账户</label>
					<asp:TextBox ID="TextBox1" runat="server" CssClass="biaodan-kongzhi" placeholder="请输入账户" >
					</asp:TextBox>
					<span class="tishi" id="yong-tishi"></span>
				</div>

				<div class="biaodan-zu">
					<label for="mi-ma">密码</label>
					<asp:TextBox ID="TextBox2" runat="server" TextMode="Password" CssClass="biaodan-kongzhi"
						placeholder="请输入密码"></asp:TextBox>
					<span class="tishi" id="mi-tishi"></span>
				</div>

				<div class="biaodan-xuanxiang">
					<div class="jizhu-mima">
						<asp:CheckBox ID="CheckBox1" runat="server" />
						<label for="jizhu-mima">记住密码</label>
					</div>
					<a href="WangJiMiMa.aspx" class="wangji-mima">忘记密码?</a>
				</div>
				<asp:Button ID="Button1" runat="server" Text="登录" CssClass="denglu-anniu" OnClick="Button1_Click" />
			</div>
		</form>
	</body>
	<script>
        // 使用jQuery的ready函数，确保在HTML文档完全加载后再执行代码
        $(document).ready(function () {

            // 为所有具有'denglu-anniu'类的元素（登录按钮）绑定点击事件
            $('.denglu-anniu').on('click', function (e) {

                // 获取用户名输入框的值，并去除首尾空格
                var username = $('#TextBox1').val().trim();
                // 获取密码输入框的值，并去除首尾空格
                var password = $('#TextBox2').val().trim();
                // 初始化验证状态变量，true表示验证通过
                var isValid = true;

                // 清除错误提示 - 开始验证前先清空之前的错误信息
                $('#yong-tishi').text(""); // 清空用户名错误提示区域的文本
                $('#mi-tishi').text("");   // 清空密码错误提示区域的文本
                $('.biaodan-kongzhi').removeClass("error"); // 移除所有表单控件的错误样式

                // 验证账户是否为空
                if (username === '') {
                    // 如果用户名为空，显示错误提示
                    $('#yong-tishi').text("账户不能为空");
                    // 为用户名输入框添加错误样式类
                    $('#TextBox1').addClass("error");
                    // 设置验证状态为失败
                    isValid = false;
                }

                // 验证密码是否为空
                if (password === '') {
                    // 如果密码为空，显示错误提示
                    $('#mi-tishi').text("密码不能为空");
                    // 为密码输入框添加错误样式类
                    $('#TextBox2').addClass("error");
                    // 设置验证状态为失败
                    isValid = false;
                }

                // 关键修改：只有验证失败时才阻止提交
                if (!isValid) {
                    // 阻止事件的默认行为（即阻止表单提交）
                    e.preventDefault();
                    // 立即退出函数，确保不会继续执行后续代码
                    return false;
                }

                // 验证成功时，不执行任何阻止操作，让表单正常提交到后台
                // 在浏览器控制台输出调试信息，便于排查问题
                console.log("验证通过，表单将提交到后台");
            });

            // 实时验证：当用户在输入框中输入时触发
            $('#TextBox1, #TextBox2').on('input', function () {
                // 将当前触发事件的DOM元素转换为jQuery对象，方便操作
                var $this = $(this);

                // 移除错误样式类，当用户开始输入时立即清除错误状态
                $this.removeClass('error');

                // 根据输入框的ID判断是哪个输入框，然后清除对应的错误提示
                if ($this.attr('id') === 'TextBox1') {
                    // 如果是用户名输入框，清空用户名错误提示
                    $('#yong-tishi').text("");
                } else {
                    // 如果是密码输入框，清空密码错误提示
                    $('#mi-tishi').text("");
                }
            });
        });
    </script>
</html>