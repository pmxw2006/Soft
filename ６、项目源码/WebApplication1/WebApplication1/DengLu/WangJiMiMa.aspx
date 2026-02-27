<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WangJiMiMa.aspx.cs" Inherits="WebApplication1.DengLu.WangJiMiMa" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
	<head runat="server">
		<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
		<title>固定资产管理系统 - 忘记密码</title>
		<script type="text/javascript" src="js/jquery-3.3.1.min.js"></script>
        <link href="/DengLu/css/WangJiMiMa.css" rel="stylesheet" />
	</head>
	<body>
		<form id="Form1" class="denglu-rongqi" runat="server">
			<div class="denglu-toubu">
				<h1>固定资产管理系统</h1>
				<p>Fixed Assets Management System</p>
			</div>

			<div class="denglu-zhuti">
				<div class="biaodan-zu">
					<label for="Text1">账户</label>
					<asp:TextBox ID="Text1" runat="server" CssClass="biaodan-kongzhi" placeholder="请输入账户">
					</asp:TextBox>
					<span class="tishi" id="yong-tishi"></span>
				</div>

				<div class="biaodan-zu">
					<label for="shenfenzheng">身份证</label>
					<asp:TextBox ID="shenfenzheng" runat="server" CssClass="biaodan-kongzhi" placeholder="请输入身份证">
					</asp:TextBox>
					<span class="tishi" id="shen-tishi"></span>
				</div>

				<div class="biaodan-zu">
					<label for="Password1">新密码</label>
					<asp:TextBox ID="Password1" runat="server" TextMode="Password" CssClass="biaodan-kongzhi"
						placeholder="请输入新密码"></asp:TextBox>
					<span class="tishi" id="mima-tishi"></span>
				</div>

				<div class="biaodan-zu">
					<label for="querenmima">确认新密码</label>
					<asp:TextBox ID="querenmima" runat="server" TextMode="Password" CssClass="biaodan-kongzhi"
						placeholder="请输入确认新密码"></asp:TextBox>
					<span class="tishi" id="mima1-tishi"></span>
				</div>
				<div class="biaodan-xuanxiang">
					<a href="/DengLu/DengLuYe.aspx" class="fanhui">返回登录</a>
				</div>
				<asp:Button ID="Submit1" runat="server" CssClass="denglu-anniu" Text="修改密码" OnClick="Submit1_Click" />
			</div>
		</form>
	</body>
	<script>
        // 使用jQuery的ready方法，确保在HTML文档完全加载完成后执行代码
        $(document).ready(function () {

            // 为修改密码按钮添加点击事件处理函数
            $('.denglu-anniu').on('click', function (e) {

                // 获取账户输入框的值，并去除首尾空格
                var username = $('#Text1').val().trim();
                // 获取身份证输入框的值，并去除首尾空格
                var shenfenzheng = $('#shenfenzheng').val().trim();
                // 获取新密码输入框的值，并去除首尾空格
                var newPassword = $('#Password1').val().trim();
                // 获取确认密码输入框的值，并去除首尾空格
                var confirmPassword = $('#querenmima').val().trim();
                // 设置验证状态变量，初始为true（通过）
                var isValid = true;

                // 清除之前的错误提示 - 开始验证前先清空之前的错误信息
                $('#yong-tishi').text(""); // 清空账户错误提示区域的文本
                $('#shen-tishi').text(""); // 清空身份证错误提示区域的文本
                $('#mima-tishi').text(""); // 清空新密码错误提示区域的文本
                $('#mima1-tishi').text(""); // 清空确认密码错误提示区域的文本
                $('.biaodan-kongzhi').removeClass("error"); // 移除所有表单控件的错误样式

                // 验证账户是否为空
                if (username === '') {
                    // 如果账户为空，显示错误提示
                    $('#yong-tishi').text("账户不能为空");
                    // 为账户输入框添加错误样式类
                    $('#Text1').addClass("error");
                    // 设置验证状态为失败
                    isValid = false;
                }

                // 验证身份证是否为空
                if (shenfenzheng === '') {
                    // 如果身份证为空，显示错误提示
                    $('#shen-tishi').text("身份证不能为空");
                    // 为身份证输入框添加错误样式类
                    $('#shenfenzheng').addClass("error");
                    // 设置验证状态为失败
                    isValid = false;
                } else if (!/^\d{17}[\dXx]$/.test(shenfenzheng)) {
                    // 验证身份证格式（18位，最后一位可以是数字或X）
                    $('#shen-tishi').text("请输入正确的18位身份证号码");
                    $('#shenfenzheng').addClass("error");
                    isValid = false;
                }

                // 验证新密码是否为空
                if (newPassword === '') {
                    // 如果新密码为空，显示错误提示
                    $('#mima-tishi').text("新密码不能为空");
                    // 为新密码输入框添加错误样式类
                    $('#Password1').addClass("error");
                    // 设置验证状态为失败
                    isValid = false;
                } else if (newPassword.length < 6) {
                    // 验证密码长度（至少6位）
                    $('#mima-tishi').text("密码长度不能少于6位");
                    $('#Password1').addClass("error");
                    isValid = false;
                }

                // 验证确认密码是否为空
                if (confirmPassword === '') {
                    // 如果确认密码为空，显示错误提示
                    $('#mima1-tishi').text("请确认新密码");
                    // 为确认密码输入框添加错误样式类
                    $('#querenmima').addClass("error");
                    // 设置验证状态为失败
                    isValid = false;
                } else if (newPassword !== confirmPassword) {
                    // 验证两次输入的密码是否一致
                    $('#mima1-tishi').text("两次输入的密码不一致");
                    $('#querenmima').addClass("error");
                    isValid = false;
                }

                // 关键逻辑：只有验证失败时才阻止提交
                if (!isValid) {
                    // 阻止事件的默认行为（即阻止表单提交）
                    e.preventDefault();
                    // 立即退出函数，确保不会继续执行后续代码
                    return false;
                }

                // 验证成功时，不执行任何阻止操作，让表单正常提交到后台
                // 在浏览器控制台输出调试信息，便于排查问题
                console.log("所有验证通过，表单将提交到后台进行密码修改");
            });

            // 实时验证：当用户在输入框中输入时触发
            $('#Text1, #shenfenzheng, #Password1, #querenmima').on('input', function () {
                // 将当前触发事件的DOM元素转换为jQuery对象，方便操作
                var $this = $(this);
                // 获取当前输入框的值并去除首尾空格
                var value = $this.val().trim();

                // 移除错误样式类，当用户开始输入时立即清除错误状态
                $this.removeClass('error');

                // 根据输入框的ID判断是哪个输入框，然后清除对应的错误提示
                if ($this.attr('id') === 'Text1') {
                    // 如果是账户输入框，清空账户错误提示
                    $('#yong-tishi').text("");
                } else if ($this.attr('id') === 'shenfenzheng') {
                    // 如果是身份证输入框，清空身份证错误提示
                    $('#shen-tishi').text("");
                } else if ($this.attr('id') === 'Password1') {
                    // 如果是新密码输入框，清空新密码错误提示
                    $('#mima-tishi').text("");
                } else if ($this.attr('id') === 'querenmima') {
                    // 如果是确认密码输入框，清空确认密码错误提示
                    $('#mima1-tishi').text("");
                }
            });

            // 专门为确认密码框添加输入事件，实时检查密码是否匹配
            $('#querenmima').on('input', function () {
                var confirmPassword = $(this).val().trim();
                var newPassword = $('#Password1').val().trim();

                // 如果两个密码框都有值且不匹配，显示提示但不阻止输入
                if (newPassword !== '' && confirmPassword !== '' && newPassword !== confirmPassword) {
                    $('#mima1-tishi').text("两次输入的密码不一致");
                    $(this).addClass("error");
                } else if (confirmPassword !== '' && newPassword === confirmPassword) {
                    // 密码匹配时清除错误提示
                    $('#mima1-tishi').text("");
                    $(this).removeClass("error");
                }
            });
        });
    </script>
</html>