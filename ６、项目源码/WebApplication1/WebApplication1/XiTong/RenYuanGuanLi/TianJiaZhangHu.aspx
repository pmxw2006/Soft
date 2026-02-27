<%@ Page Title="" Language="C#" MasterPageFile="~/MuBanYe/XiTong.Master" AutoEventWireup="true" CodeBehind="TianJiaZhangHu.aspx.cs" Inherits="WebApplication1.XiTong.RenYuanGuanLi.TianJiaZhangHu" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<!-- 引入jQuery -->
	<script type="text/javascript" src="/XiTong/js/jquery-3.3.1.min.js"></script>
	<script type="text/javascript" src="/Js/TanChuKuang.js"></script>
	<link href="/css/TanChuKuang.css" rel="stylesheet" />
	<link href="/XiTong/css/TianJiaZhangHu.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<div class="TianJiaDaHeZi">
    <!-- 成功/失败提示 - 使用 div 而不是 asp:Panel -->
    <div id="divChengGong" runat="server" class="ChengGongTiShi" style="display:none;">
        用户添加成功！
    </div>
    <div id="divCuoWu" runat="server" class="CuoWuTiShi" style="display:none;">
        添加失败，请检查输入信息！
    </div>

    <!-- 页面标题 -->
    <div class="TianJiaBiaoTi">
        <h2>添加新用户</h2>
        <div class="fu-biao-ti">填写用户信息以创建新账户</div>
    </div>

    <!-- 用户信息表单 -->
    <div class="YongHuXinXiBiaoDan">
        <!-- 用户名 -->
        <div class="XinXiHang">
            <label for="txtXingMing">
                用户名
                <span class="XuZhiXing">*</span>
            </label>
            <asp:TextBox ID="TextBox1" runat="server" MaxLength="20" placeholder="请输入用户名（至少3位）" CssClass="biaodan-kongzhi"></asp:TextBox>
            <span class="tishi" id="yonghuming-tishi"></span>
        </div>
        
        <!-- 真实姓名 -->
        <div class="XinXiHang">
            <label for="txtXingMing">
                真实姓名
                <span class="XuZhiXing">*</span>
            </label>
            <asp:TextBox ID="txtXingMing" runat="server" MaxLength="10" placeholder="请输入真实姓名" CssClass="biaodan-kongzhi"></asp:TextBox>
            <span class="tishi" id="xingming-tishi"></span>
        </div>

        <!-- 身份证号 -->
        <div class="XinXiHang">
            <label for="txtShenFenZheng">
                身份证号
                <span class="XuZhiXing">*</span>
            </label>
            <asp:TextBox ID="txtShenFenZheng" runat="server" MaxLength="18" placeholder="请输入18位身份证号" CssClass="biaodan-kongzhi"></asp:TextBox>
            <span class="tishi" id="shenfenzheng-tishi"></span>
        </div>

        <!-- 所属部门 -->
        <div class="XinXiHang">
            <label for="ddlBuMen">
                所属部门
                <span class="XuZhiXing">*</span>
            </label>
            <asp:DropDownList ID="ddlBuMen" runat="server" CssClass="biaodan-kongzhi">
                <asp:ListItem Value="">请选择部门</asp:ListItem>
                <asp:ListItem Value="系统">系统</asp:ListItem>
                <asp:ListItem Value="固定资产">固定资产</asp:ListItem>
            </asp:DropDownList>
            <span class="tishi" id="bumen-tishi"></span>
        </div>

        <!-- 登录密码 -->
        <div class="XinXiHang">
            <label for="txtMiMa">
                登录密码
                <span class="XuZhiXing">*</span>
            </label>
            <asp:TextBox ID="txtMiMa" runat="server" TextMode="Password" placeholder="请输入登录密码（至少6位）"
                autocomplete="new-password" CssClass="biaodan-kongzhi"></asp:TextBox>
            <span class="tishi" id="mima-tishi"></span>
        </div>
        <div class="TiShiXinXi">
            <div id="MiMaQiangDuTiao" class="MiMaQiangDuTiao"></div>
            <span id="MiMaQiangDuTiShi" style="font-size: 12px; color: #666;">密码强度：无</span>
        </div>

        <!-- 确认密码 -->
        <div class="XinXiHang">
            <label for="txtQueRenMiMa">
                确认密码
                <span class="XuZhiXing">*</span>
            </label>
            <asp:TextBox ID="txtQueRenMiMa" runat="server" TextMode="Password" placeholder="请再次输入密码"
                autocomplete="new-password" CssClass="biaodan-kongzhi"></asp:TextBox>
            <span class="tishi" id="querenmima-tishi"></span>
        </div>

        <!-- 密码要求提示 -->
        <div style="background-color: #f8f9fa; padding: 15px; border-radius: 5px; margin-top: 20px;">
            <div style="font-weight: bold; color: #333; margin-bottom: 10px;">密码要求：</div>
            <ul style="margin: 0; padding-left: 20px; color: #666; font-size: 13px;">
                <li>密码长度至少6位</li>
                <li>建议包含字母大小写、数字和特殊字符</li>
                <li>较强的密码可以更好地保护账户安全</li>
            </ul>
        </div>

        <!-- 按钮区域 -->
        <div class="AnNiuQuYu">
            <asp:Button ID="btnTianJia" runat="server" Text="添加用户" CssClass="AnNiuTianJia denglu-anniu"
                OnClick="btnTianJia_Click" OnClientClick="return validateForm();" />
            <asp:Button ID="btnFanHui" runat="server" Text="返回列表" CssClass="AnNiuFanHui" 
                OnClick="btnFanHui_Click" />
        </div>
    </div>
</div>
    <script type="text/javascript">
        // 确保页面加载完成后执行
        $(document).ready(function () {
            // 为所有输入框和下拉框添加实时验证
            $('#<%= TextBox1.ClientID %>, #<%= txtXingMing.ClientID %>, #<%= txtShenFenZheng.ClientID %>, #<%= txtMiMa.ClientID %>, #<%= txtQueRenMiMa.ClientID %>').on('input', function () {
        var $this = $(this);
        $this.removeClass('error');

        // 根据ID清除对应的错误提示
        var id = $this.attr('id');
        switch (id) {
            case '<%= TextBox1.ClientID %>':
                $('#yonghuming-tishi').text("");
                break;
            case '<%= txtXingMing.ClientID %>':
                $('#xingming-tishi').text("");
                break;
            case '<%= txtShenFenZheng.ClientID %>':
                $('#shenfenzheng-tishi').text("");
                break;
            case '<%= txtMiMa.ClientID %>':
                $('#mima-tishi').text("");
                break;
            case '<%= txtQueRenMiMa.ClientID %>':
                $('#querenmima-tishi').text("");
                break;
        }
    });

    // 为下拉框添加变更事件
    $('#<%= ddlBuMen.ClientID %>').on('change', function () {
        $(this).removeClass('error');
        $('#bumen-tishi').text("");
    });

    // 密码强度检测 - 使用修改密码页面的样式
    $('#<%= txtMiMa.ClientID %>').on('input', function () {
        var password = $(this).val();
        var strength = 0;
        var tips = "";

        // 获取密码强度条元素
        var $bar = $('#MiMaQiangDuTiao');

        if (password.length === 0) {
            // 清空时重置样式
            $bar.removeClass('mi-ma-qiang-du-ruo mi-ma-qiang-du-zhong mi-ma-qiang-du-qiang mi-ma-qiang-du-fei-qiang');
            $bar.css('width', '0%');
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

        // 移除所有现有样式类
        $bar.removeClass('mi-ma-qiang-du-ruo mi-ma-qiang-du-zhong mi-ma-qiang-du-qiang mi-ma-qiang-du-fei-qiang');

        // 根据强度等级添加对应的样式类
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

    // 实时检查密码匹配
    $('#<%= txtQueRenMiMa.ClientID %>').on('input', function () {
        var miMa = $('#<%= txtMiMa.ClientID %>').val();
        var queRenMiMa = $(this).val();
        var $tishi = $('#querenmima-tishi');

        if (queRenMiMa.length === 0) {
            $tishi.text('');
            return;
        }

        if (miMa !== queRenMiMa) {
            $tishi.text('两次输入的密码不一致');
            $(this).addClass('error');
            $('#<%= txtMiMa.ClientID %>').addClass('error');
        } else {
            $tishi.text('');
            $(this).removeClass('error');
            $('#<%= txtMiMa.ClientID %>').removeClass('error');
        }
    });
});

        // 表单验证函数 - 被添加按钮调用
        function validateForm() {
            // 获取所有输入值
            var yongHuMing = $('#<%= TextBox1.ClientID %>').val().trim();
    var xingMing = $('#<%= txtXingMing.ClientID %>').val().trim();
    var shenFenZheng = $('#<%= txtShenFenZheng.ClientID %>').val().trim();
    var buMen = $('#<%= ddlBuMen.ClientID %>').val();
    var miMa = $('#<%= txtMiMa.ClientID %>').val().trim();
    var queRenMiMa = $('#<%= txtQueRenMiMa.ClientID %>').val().trim();

    // 初始化验证状态
    var isValid = true;

    // 清除所有错误提示和错误样式
    $('#yonghuming-tishi, #xingming-tishi, #shenfenzheng-tishi, #bumen-tishi, #mima-tishi, #querenmima-tishi').text("");
    $('.biaodan-kongzhi').removeClass("error");

    // 验证用户名
    if (yongHuMing === '') {
        $('#yonghuming-tishi').text("用户名不能为空");
        $('#<%= TextBox1.ClientID %>').addClass("error");
        isValid = false;
    } else if (yongHuMing.length < 3) {
        $('#yonghuming-tishi').text("用户名至少需要3个字符");
        $('#<%= TextBox1.ClientID %>').addClass("error");
        isValid = false;
    }

    // 验证真实姓名
    if (xingMing === '') {
        $('#xingming-tishi').text("真实姓名不能为空");
        $('#<%= txtXingMing.ClientID %>').addClass("error");
        isValid = false;
    } else if (xingMing.length < 2) {
        $('#xingming-tishi').text("真实姓名至少需要2个字符");
        $('#<%= txtXingMing.ClientID %>').addClass("error");
        isValid = false;
    }

    // 验证身份证号
    if (shenFenZheng === '') {
        $('#shenfenzheng-tishi').text("身份证号不能为空");
        $('#<%= txtShenFenZheng.ClientID %>').addClass("error");
        isValid = false;
    } else if (shenFenZheng.length !== 18) {
        $('#shenfenzheng-tishi').text("身份证号必须是18位");
        $('#<%= txtShenFenZheng.ClientID %>').addClass("error");
        isValid = false;
    } else if (!/^\d{17}[\dXx]$/.test(shenFenZheng)) {
        $('#shenfenzheng-tishi').text("身份证号格式不正确");
        $('#<%= txtShenFenZheng.ClientID %>').addClass("error");
        isValid = false;
    }

    // 验证部门
    if (buMen === '') {
        $('#bumen-tishi').text("请选择所属部门");
        $('#<%= ddlBuMen.ClientID %>').addClass("error");
        isValid = false;
    }

    // 验证密码
    if (miMa === '') {
        $('#mima-tishi').text("密码不能为空");
        $('#<%= txtMiMa.ClientID %>').addClass("error");
        isValid = false;
    } else if (miMa.length < 6) {
        $('#mima-tishi').text("密码长度不能少于6位");
        $('#<%= txtMiMa.ClientID %>').addClass("error");
        isValid = false;
    }

    // 验证确认密码
    if (queRenMiMa === '') {
        $('#querenmima-tishi').text("请确认密码");
        $('#<%= txtQueRenMiMa.ClientID %>').addClass("error");
        isValid = false;
    } else if (miMa !== queRenMiMa) {
        $('#querenmima-tishi').text("两次输入的密码不一致");
        $('#<%= txtQueRenMiMa.ClientID %>').addClass("error");
        $('#<%= txtMiMa.ClientID %>').addClass("error");
        isValid = false;
    }

    // 如果验证失败，阻止表单提交
    if (!isValid) {
        // 滚动到第一个错误位置
        var firstError = $('.biaodan-kongzhi.error').first();
        if (firstError.length) {
            $('html, body').animate({
                scrollTop: firstError.offset().top - 100
            }, 500);
        }
        return false;
    }

    // 验证通过，允许提交
    console.log("用户添加表单验证通过，提交到后台");
    return true;
}
    </script>
</asp:Content>