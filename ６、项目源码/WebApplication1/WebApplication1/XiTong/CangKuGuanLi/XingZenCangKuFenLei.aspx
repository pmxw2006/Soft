<%@ Page Title="" Language="C#" MasterPageFile="~/MuBanYe/XiTong.Master" AutoEventWireup="true" CodeBehind="XingZenCangKuFenLei.aspx.cs" Inherits="WebApplication1.XiTong.CangKuGuanLi.XingZenCangKuFenLei" %>
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
            <h2>新增仓库分类</h2>
            <div class="fu-biao-ti">创建新的仓库分类</div>
        </div>
        
        <div class="xin-xi-qu">
            <div class="xin-xi-biao-ti">分类信息</div>
            <div class="xin-xi-wang-ge">
                <div class="xin-xi-xiang-mu">
                    <span class="xin-xi-biao-qian">分类名称</span>
                    <asp:TextBox ID="WenBenFenLeiMingCheng" runat="server" CssClass="wen-ben-shu-ru" 
                        placeholder="请输入仓库分类名称" MaxLength="50"></asp:TextBox>
                    <span class="tishi" id="fen-lei-ming-cheng-tishi"></span>
                </div>
            </div>
            
            <!-- 分类要求说明 -->
            <div style="background-color: #f8f9fa; padding: 15px; border-radius: 5px; margin-top: 20px;">
                <div style="font-weight: bold; color: #333; margin-bottom: 10px;">分类要求：</div>
                <ul style="margin: 0; padding-left: 20px; color: #666; font-size: 13px;">
                    <li>分类名称不能为空</li>
                    <li>分类名称长度不能超过50个字符</li>
                    <li>建议使用有意义的分类名称，如：电子产品、办公用品等</li>
                    <li>分类名称创建后不能重复</li>
                </ul>
            </div>
        </div>
        
        <!-- 操作按钮 -->
        <div class="an-niu-zu">
            <asp:Button ID="AnNiuChuangJianFenLei" runat="server" Text="创建分类" 
                CssClass="an-niu-cheng-gong" OnClick="AnNiuChuangJianFenLei_Click" />
        </div>
        
        <!-- 返回链接 -->
        <div style="text-align: center; margin-top: 20px;">
            <a href="/XiTong/CangKuGuanLi/ZiChanTongJi.aspx" class="fan-hui-lian-jie">← 返回上一页</a>
        </div>
    </div>
    
    <script type="text/javascript">
        $(document).ready(function () {
            // 为创建分类按钮绑定验证事件
            $('#<%= AnNiuChuangJianFenLei.ClientID %>').on('click', function (e) {
                // 获取输入值
                var fenLeiMingCheng = $('#<%= WenBenFenLeiMingCheng.ClientID %>').val().trim();

                // 初始化验证状态
                var isValid = true;

                // 清除错误提示
                $('#fen-lei-ming-cheng-tishi').text("");
                $('.wen-ben-shu-ru').removeClass("error");

                // 验证分类名称
                if (fenLeiMingCheng === '') {
                    $('#fen-lei-ming-cheng-tishi').text("分类名称不能为空");
                    $('#<%= WenBenFenLeiMingCheng.ClientID %>').addClass("error");
                    isValid = false;
                } else if (fenLeiMingCheng.length > 50) {
                    $('#fen-lei-ming-cheng-tishi').text("分类名称不能超过50个字符");
                    $('#<%= WenBenFenLeiMingCheng.ClientID %>').addClass("error");
                    isValid = false;
                } else if (!/^[\u4e00-\u9fa5a-zA-Z0-9]+$/.test(fenLeiMingCheng)) {
                    $('#fen-lei-ming-cheng-tishi').text("分类名称只能包含中文、字母和数字");
                    $('#<%= WenBenFenLeiMingCheng.ClientID %>').addClass("error");
                    isValid = false;
                }
                
                // 如果验证失败，阻止表单提交
                if (!isValid) {
                    e.preventDefault();
                    return false;
                }
                
                // 验证通过，允许提交
                console.log("分类名称验证通过，提交表单到后台");
            });
            
            // 实时验证：当用户输入时清除错误提示
            $('#<%= WenBenFenLeiMingCheng.ClientID %>').on('input', function () {
                $(this).removeClass('error');
                $('#fen-lei-ming-cheng-tishi').text("");
            });
        });
    </script>
</asp:Content>
