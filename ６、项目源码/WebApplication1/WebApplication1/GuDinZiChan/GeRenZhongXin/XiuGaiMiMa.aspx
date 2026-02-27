<%@ Page Title="" Language="C#" MasterPageFile="~/MuBanYe/GuDinZiChan.Master" AutoEventWireup="true" CodeBehind="XiuGaiMiMa.aspx.cs" Inherits="WebApplication1.GuDinZiChan.GeRenZhongXin.XiuGaiMiMa" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<!-- 引入jQuery -->
	<script type="text/javascript" src="/XiTong/js/jquery-3.3.1.min.js"></script>
    <link href="/GuDinZiChan/css/XiuGaiMiMa.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<div class="xiu-gai-mi-ma">
		<!-- 个人信息头部 -->
		<div class="tou-bu">
			<h2>修改密码</h2>
			<div class="fu-biao-ti">请设置新的登录密码</div>
		</div>

		<!-- 密码信息区域 -->
		<div class="mi-ma-xin-xi-qu">
			<div class="mi-ma-biao-ti">密码设置</div>

			<div class="mi-ma-xiang-mu">
				<span class="mi-ma-biao-qian">当前密码</span>
				<asp:TextBox ID="WenBenDangQianMiMa" runat="server" TextMode="Password" CssClass="mi-ma-shu-ru-kuang"
					placeholder="请输入当前密码" autocomplete="off"></asp:TextBox>
				<span class="tishi" id="dangqianmi-ma-tishi"></span>
			</div>

			<div class="mi-ma-xiang-mu">
				<span class="mi-ma-biao-qian">新密码</span>
				<asp:TextBox ID="WenBenXinMiMa" runat="server" TextMode="Password" CssClass="mi-ma-shu-ru-kuang"
					placeholder="请输入新密码（至少6位字符）" autocomplete="new-password"></asp:TextBox>
				<div id="MiMaQiangDuTiao" class="mi-ma-qiang-du-tiao"></div>
				<span class="mi-ma-ti-shi" id="MiMaQiangDuTiShi">密码强度：无</span>
				<span class="tishi" id="xinmi-ma-tishi"></span>
			</div>

			<div class="mi-ma-xiang-mu">
				<span class="mi-ma-biao-qian">确认新密码</span>
				<asp:TextBox ID="WenBenQueRenMiMa" runat="server" TextMode="Password" CssClass="mi-ma-shu-ru-kuang"
					placeholder="请再次输入新密码" autocomplete="new-password"></asp:TextBox>
				<span class="tishi" id="querenmi-ma-tishi"></span>
			</div>

			<div style="background-color: #f8f9fa; padding: 15px; border-radius: 5px; margin-top: 20px;">
				<div style="font-weight: bold; color: #333; margin-bottom: 10px;">密码要求：</div>
				<ul style="margin: 0; padding-left: 20px; color: #666; font-size: 13px;">
					<li>密码长度至少6位</li>
					<li>建议包含字母大小写、数字和特殊字符</li>
					<li>新密码不能与当前密码相同</li>
				</ul>
			</div>
		</div>

		<!-- 操作按钮 -->
		<div class="an-niu-zu">
			<asp:Button ID="AnNiuBaoCun" runat="server" Text="保存密码" CssClass="an-niu-cheng-gong" OnClick="AnNiuBaoCun_Click" />
		</div>

		<!-- 返回链接 -->
		<div style="text-align: center; margin-top: 20px;">
			<a href="/GuDinZiChan/GeRenZhongXin/GeRenXinXi.aspx" class="fan-hui-lian-jie">← 返回个人中心</a>
		</div>
	</div>

	<script type="text/javascript">
        $(document).ready(function () {
            // 为保存按钮绑定验证事件
            $('.an-niu-cheng-gong').on('click', function (e) {
                // 获取输入值
                var currentPwd = $('#<%= WenBenDangQianMiMa.ClientID %>').val().trim();
                var newPwd = $('#<%= WenBenXinMiMa.ClientID %>').val().trim();
                var confirmPwd = $('#<%= WenBenQueRenMiMa.ClientID %>').val().trim();

                // 初始化验证状态
                var isValid = true;

                // 清除错误提示
                $('#dangqianmi-ma-tishi').text("");
                $('#xinmi-ma-tishi').text("");
                $('#querenmi-ma-tishi').text("");
                $('.mi-ma-shu-ru-kuang').removeClass("error");

                // 验证当前密码
                if (currentPwd === '') {
                    $('#dangqianmi-ma-tishi').text("当前密码不能为空");
                    $('#<%= WenBenDangQianMiMa.ClientID %>').addClass("error");
                    isValid = false;
                }

                // 验证新密码
                if (newPwd === '') {
                    $('#xinmi-ma-tishi').text("新密码不能为空");
                    $('#<%= WenBenXinMiMa.ClientID %>').addClass("error");
                    isValid = false;
                } else if (newPwd.length < 6) {
                    $('#xinmi-ma-tishi').text("密码长度不能少于6位");
                    $('#<%= WenBenXinMiMa.ClientID %>').addClass("error");
                    isValid = false;
                }

                // 验证确认密码
                if (confirmPwd === '') {
                    $('#querenmi-ma-tishi').text("确认新密码不能为空");
                    $('#<%= WenBenQueRenMiMa.ClientID %>').addClass("error");
                    isValid = false;
                } else if (newPwd !== confirmPwd) {
                    $('#querenmi-ma-tishi').text("两次输入的密码不一致");
                    $('#<%= WenBenQueRenMiMa.ClientID %>').addClass("error");
					$('#<%= WenBenXinMiMa.ClientID %>').addClass("error");
					isValid = false;
				}

				// 验证新密码不能与当前密码相同
				if (currentPwd !== '' && newPwd !== '' && currentPwd === newPwd) {
					$('#xinmi-ma-tishi').text("新密码不能与当前密码相同");
					$('#<%= WenBenXinMiMa.ClientID %>').addClass("error");
					isValid = false;
				}

				// 如果验证失败，阻止表单提交
				if (!isValid) {
					e.preventDefault();
					return false;
				}

				// 验证通过，允许提交
				console.log("密码修改验证通过，提交表单到后台");
			});

			// 密码强度检测
			$('#<%= WenBenXinMiMa.ClientID %>').on('input', function() {
				var password = $(this).val();
				var strength = 0;
				var tips = "";

				if (password.length === 0) {
					$('#MiMaQiangDuTiao').removeClass().addClass('mi-ma-qiang-du-tiao');
					$('#MiMaQiangDuTiShi').text('密码强度：无');
					return;
				}

				// 长度评分
				if (password.length >= 6) strength++;
				if (password.length >= 8) strength++;
				if (password.length >= 12) strength++;

				// 复杂度评分
				if (/[a-z]/.test(password)) strength++;
				if (/[A-Z]/.test(password)) strength++;
				if (/[0-9]/.test(password)) strength++;
				if (/[^a-zA-Z0-9]/.test(password)) strength++;

				// 设置强度等级
				var $bar = $('#MiMaQiangDuTiao');
				$bar.removeClass().addClass('mi-ma-qiang-du-tiao');

				if (strength <= 2) {
					$bar.addClass('mi-ma-qiang-du-ruo');
					tips = '密码强度：弱';
				} else if (strength <= 4) {
					$bar.addClass('mi-ma-qiang-du-zhong');
					tips = '密码强度：中';
				} else if (strength <= 6) {
					$bar.addClass('mi-ma-qiang-du-qiang');
					tips = '密码强度：强';
				} else {
					$bar.addClass('mi-ma-qiang-du-fei-qiang');
					tips = '密码强度：非常强';
				}

				$('#MiMaQiangDuTiShi').text(tips);
			});

			// 实时验证：当用户输入时清除错误提示
			$('#<%= WenBenDangQianMiMa.ClientID %>, #<%= WenBenXinMiMa.ClientID %>, #<%= WenBenQueRenMiMa.ClientID %>')
				.on('input', function() {
					var $this = $(this);
					$this.removeClass('error');

					if ($this.attr('id') === '<%= WenBenDangQianMiMa.ClientID %>') {
						$('#dangqianmi-ma-tishi').text("");
					} else if ($this.attr('id') === '<%= WenBenXinMiMa.ClientID %>') {
                        $('#xinmi-ma-tishi').text("");
                    } else {
                        $('#querenmi-ma-tishi').text("");
                    }
                });
        });
    </script>
</asp:Content>