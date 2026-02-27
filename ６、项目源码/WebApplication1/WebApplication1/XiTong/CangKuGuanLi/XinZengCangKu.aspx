<%@ Page Title="" Language="C#" MasterPageFile="~/MuBanYe/XiTong.Master" AutoEventWireup="true" CodeBehind="XinZengCangKu.aspx.cs" Inherits="WebApplication1.XiTong.CangKuGuanLi.XinZengCangKu" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <!-- 引入jQuery -->
    <script type="text/javascript" src="/XiTong/js/jquery-3.3.1.min.js"></script>
    <link href="/XiTong/css/XingZenCangKuFenLei.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <div class="ge-ren-zhong-xin">
        <!-- 消息提示区域 -->
        <asp:Panel ID="PanXiaoXi" runat="server" CssClass="xiao-xi-pan" Visible="false">
            <asp:Label ID="LabXiaoXi" runat="server" Text=""></asp:Label>
        </asp:Panel>
        
        <div class="tou-bu">
            <h2>新增仓库</h2>
            <div class="fu-biao-ti">创建新的仓库信息</div>
        </div>
        
        <div class="xin-xi-qu">
            <div class="xin-xi-biao-ti">仓库信息</div>
            <div class="xin-xi-wang-ge">
                <!-- 仓库分类下拉框 -->
                <div class="xin-xi-xiang-mu">
                    <span class="xin-xi-biao-qian">仓库分类</span>
                    <asp:DropDownList ID="XiaLaCangKuFenLei" runat="server" CssClass="xia-la-xuanze">
                        <asp:ListItem Text="--请选择仓库分类--" Value=""></asp:ListItem>
                    </asp:DropDownList>
                    <span class="tishi" id="cangku-fenlei-tishi"></span>
                </div>
                
                <!-- 仓库名称输入框 -->
                <div class="xin-xi-xiang-mu">
                    <span class="xin-xi-biao-qian">仓库名称</span>
                    <asp:TextBox ID="WenBenCangKuMingCheng" runat="server" CssClass="wen-ben-shu-ru" 
                        placeholder="请输入仓库名称" MaxLength="50"></asp:TextBox>
                    <span class="tishi" id="cangku-mingcheng-tishi"></span>
                </div>
            </div>
            
            <!-- 仓库要求说明 -->
            <div style="background-color: #f8f9fa; padding: 15px; border-radius: 5px; margin-top: 20px;">
                <div style="font-weight: bold; color: #333; margin-bottom: 10px;">仓库要求说明：</div>
                <ul style="margin: 0; padding-left: 20px; color: #666; font-size: 13px;">
                    <li>仓库分类必须选择</li>
                    <li>仓库名称不能为空</li>
                    <li>仓库名称长度不能超过50个字符</li>
                    <li>不同分类下可以使用相同的仓库名称</li>
                    <li>同一分类下的仓库名称不能重复</li>
                    <li>建议使用有意义的仓库名称，如：电子产品仓库、办公用品仓库等</li>
                </ul>
            </div>
        </div>
        
        <!-- 操作按钮 -->
        <div class="an-niu-zu">
            <asp:Button ID="AnNiuChuangJian" runat="server" Text="创建仓库" CssClass="an-niu-cheng-gong" OnClick="AnNiuChuangJian_Click" />
        </div>
        
        <!-- 返回链接 -->
        <div style="text-align: center; margin-top: 20px;">
            <a href="/XiTong/CangKuGuanLi/ZiChanTongJi.aspx" class="fan-hui-lian-jie">← 返回仓库管理</a>
        </div>
    </div>
    
    <script type="text/javascript">
        $(document).ready(function () {
            // 为创建按钮绑定验证事件
            $('#<%= AnNiuChuangJian.ClientID %>').on('click', function (e) {
                // 获取输入值
                var fenLeiID = $('#<%= XiaLaCangKuFenLei.ClientID %>').val();
                var cangKuMing = $('#<%= WenBenCangKuMingCheng.ClientID %>').val().trim();

                // 初始化验证状态
                var isValid = true;

                // 清除错误提示
                $('#cangku-fenlei-tishi').text("");
                $('#cangku-mingcheng-tishi').text("");
                $('.xia-la-xuanze, .wen-ben-shu-ru').removeClass("error");

                // 验证仓库分类
                if (fenLeiID === '' || fenLeiID === null) {
                    $('#cangku-fenlei-tishi').text("请选择仓库分类");
                    $('#<%= XiaLaCangKuFenLei.ClientID %>').addClass("error");
                    isValid = false;
                }

                // 验证仓库名称
                if (cangKuMing === '') {
                    $('#cangku-mingcheng-tishi').text("仓库名称不能为空");
                    $('#<%= WenBenCangKuMingCheng.ClientID %>').addClass("error");
                    isValid = false;
                } else if (cangKuMing.length > 50) {
                    $('#cangku-mingcheng-tishi').text("仓库名称不能超过50个字符");
                    $('#<%= WenBenCangKuMingCheng.ClientID %>').addClass("error");
                    isValid = false;
                } else if (!/^[\u4e00-\u9fa5a-zA-Z0-9\s\-_（）()]+$/.test(cangKuMing)) {
                    $('#cangku-mingcheng-tishi').text("仓库名称只能包含中文、字母、数字、空格和常用标点");
                    $('#<%= WenBenCangKuMingCheng.ClientID %>').addClass("error");
                    isValid = false;
                }
                
                // 如果验证失败，阻止表单提交
                if (!isValid) {
                    e.preventDefault();
                    return false;
                }
                
                // 验证通过，允许提交
                console.log("仓库信息验证通过，提交表单到后台");
            });
            
            // 实时验证：当用户输入时清除错误提示
            $('#<%= WenBenCangKuMingCheng.ClientID %>').on('input', function () {
                $(this).removeClass('error');
                $('#cangku-mingcheng-tishi').text("");
            });
            
            // 当下拉框改变时清除错误提示
            $('#<%= XiaLaCangKuFenLei.ClientID %>').on('change', function () {
                $(this).removeClass('error');
                $('#cangku-fenlei-tishi').text("");
            });
        });
    </script>
</asp:Content>
