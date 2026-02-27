<%@ Page Title="" Language="C#" MasterPageFile="~/MuBanYe/XiTong.Master" AutoEventWireup="true" CodeBehind="GeRenXinXi.aspx.cs" Inherits="WebApplication1.XiTong.GeRenZhongXin.GeRenXinXi" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- 引入jQuery -->
	<script type="text/javascript" src="/XiTong/js/jquery-3.3.1.min.js"></script>
    <link href="/XiTong/css/GeRenZhongXin.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="ge-ren-zhong-xin">
        <!-- 消息提示区域 -->
        <asp:Panel ID="PanXiaoXi" runat="server" CssClass="xiao-xi-pan" Visible="false">
            <asp:Label ID="LabXiaoXi" runat="server" Text=""></asp:Label>
        </asp:Panel>
        
        <!-- 个人信息头部 -->
        <div class="tou-bu">
            <h2>个人中心</h2>
            <div class="fu-biao-ti">管理您的个人信息和账户设置</div>
        </div>
        
        <!-- 账户信息 -->
        <div class="xin-xi-qu">
            <div class="xin-xi-biao-ti">账户信息
                <a href="/XiTong/GeRenZhongXin/XiuGaiMiMa.aspx" class="tiao-zhuan-lian-jie" title="跳转到修改密码页面">修改密码</a>
            </div>
            <div class="xin-xi-wang-ge">
                <div class="xin-xi-xiang-mu">
                    <span class="xin-xi-biao-qian">用户账号</span>
                    <asp:TextBox ID="WenBenYongHuZhangHao" runat="server" CssClass="xin-xi-zhi zhi-du" ReadOnly="true"></asp:TextBox>
                </div>
                
                <div class="xin-xi-xiang-mu">
                    <span class="xin-xi-biao-qian">用户名</span>
                    <asp:TextBox ID="WenBenYongHuMing" runat="server" CssClass="bian-ji-kuang" 
                        MaxLength="50" Enabled="false"></asp:TextBox>
                </div>
                
                <div class="xin-xi-xiang-mu">
                    <span class="xin-xi-biao-qian">所属部门</span>
                    <asp:TextBox ID="WenBenBuMen" runat="server" CssClass="xin-xi-zhi zhi-du" ReadOnly="true"></asp:TextBox>
                </div>
            </div>
        </div>
        
        <!-- 个人资料 -->
        <div class="xin-xi-qu">
            <div class="xin-xi-biao-ti">个人资料</div>
            <div class="xin-xi-wang-ge">
                <div class="xin-xi-xiang-mu">
                    <span class="xin-xi-biao-qian">真实姓名</span>
                    <asp:TextBox ID="WenBenXingMing" runat="server" CssClass="bian-ji-kuang" 
                        MaxLength="20" Enabled="false"></asp:TextBox>
                </div>
                
                <div class="xin-xi-xiang-mu">
                    <span class="xin-xi-biao-qian">身份证号码</span>
                    <asp:TextBox ID="WenBenShenFenZheng" runat="server" CssClass="xin-xi-zhi zhi-du" 
                        ReadOnly="true" MaxLength="18"></asp:TextBox>
                </div>
            </div>
        </div>
        
        <!-- 操作按钮 -->
        <div class="an-niu-zu">
            <asp:Button ID="AnNiuBianJi" runat="server" Text="编辑信息" CssClass="an-niu-ji-chu"/>
            <asp:Button ID="AnNiuBaoCun" runat="server" Text="保存修改" CssClass="an-niu-cheng-gong" style="display: none;" OnClick="AnNiuBaoCun_Click"/>
            <asp:Button ID="AnNiuQuXiao" runat="server" Text="取消" CssClass="an-niu-ya-se" style="display: none;"/>
        </div>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            // 初始化状态
            jinYongBianJiMoShi();

            // 绑定编辑信息按钮点击事件
            $('#<%= AnNiuBianJi.ClientID %>').click(function (e) {
                e.preventDefault();
                qiYongBianJiMoShi();
            });

            // 绑定取消按钮点击事件
            $('#<%= AnNiuQuXiao.ClientID %>').click(function (e) {
                e.preventDefault();
                jinYongBianJiMoShi();
            });
        });

        // 启用编辑模式
        function qiYongBianJiMoShi() {
            $('.bian-ji-kuang:not(.zhi-du)')
                .prop('disabled', false)
                .css('background-color', '#fff');

            // 切换按钮显示
            $('#<%= AnNiuBianJi.ClientID %>').hide();
    $('#<%= AnNiuBaoCun.ClientID %>').show();
    $('#<%= AnNiuQuXiao.ClientID %>').show();
}

// 禁用编辑模式
function jinYongBianJiMoShi() {
    $('.bian-ji-kuang:not(.zhi-du)')
        .prop('disabled', true)
        .css('background-color', '#f9f9f9');

    // 切换按钮显示
    $('#<%= AnNiuBianJi.ClientID %>').show();
    $('#<%= AnNiuBaoCun.ClientID %>').hide();
    $('#<%= AnNiuQuXiao.ClientID %>').hide();
}
    </script>
</asp:Content>
