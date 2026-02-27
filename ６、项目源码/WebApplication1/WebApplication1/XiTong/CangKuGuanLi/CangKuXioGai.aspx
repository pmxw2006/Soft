<%@ Page Title="" Language="C#" MasterPageFile="~/MuBanYe/XiTong.Master" AutoEventWireup="true" CodeBehind="CangKuXioGai.aspx.cs" Inherits="WebApplication1.XiTong.CangKuGuanLi.CangKuXioGai" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
      <!-- 引入jQuery -->
    <script type="text/javascript" src="/XiTong/js/jquery-3.3.1.min.js"></script>
    <link href="/XiTong/css/CangKuXioGai.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
         <div class="ge-ren-zhong-xin">
        <!-- 消息提示区域 -->
        <asp:Panel ID="PanXiaoXi" runat="server" CssClass="xiao-xi-pan" Visible="false">
            <asp:Label ID="LabXiaoXi" runat="server" Text=""></asp:Label>
        </asp:Panel>
        
        <div class="tou-bu">
            <h2>修改仓库名称</h2>
            <div class="fu-biao-ti">修改仓库名称信息</div>
        </div>
        
        <div class="xin-xi-qu">
            <div class="xin-xi-biao-ti">修改信息</div>
            <div class="xin-xi-xiang-mu">
                <span class="xin-xi-biao-qian">新的仓库名称</span>
                <asp:TextBox ID="WenBenCangKuMing" runat="server" CssClass="wen-ben-shu-ru" 
                    placeholder="请输入新的仓库名称" MaxLength="50"></asp:TextBox>
            </div>
        </div>
        
        <!-- 操作按钮 -->
        <div class="an-niu-zu">
            <asp:Button ID="AnNiuGengGai" runat="server" OnClick="AnNiuGengGai_Click" Text="确认更改" CssClass="an-niu-ji-chu" />
        </div>
        
        <!-- 返回链接 -->
        <div style="text-align: center; margin-top: 20px;">
            <a href="/XiTong/CangKuGuanLi/ZiChanTongJi.aspx" class="fan-hui-lian-jie">← 返回仓库管理</a>
        </div>
    </div>
</asp:Content>
