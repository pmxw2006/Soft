<%@ Page Title="" Language="C#" MasterPageFile="~/MuBanYe/XiTong.Master" AutoEventWireup="true" CodeBehind="ZiChanTongJi.aspx.cs" Inherits="WebApplication1.XiTong.CangKuGuanLi.ZiChanTongJi" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/css/TanChuKuang.css" rel="stylesheet" />
    <link href="/css/FenYeYangShi.css" rel="stylesheet" />
    <link href="/css/SouSuo.css" rel="stylesheet" />
    <link href="/css/BiaoDan.css" rel="stylesheet" />
    <link href="/css/DaHeZi.css" rel="stylesheet" />
    <link href="/XiTong/css/ZiChanTongJi.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
            <div class="DaHeZi">
        <!-- 添加模块标题 -->
        <div class="MoKuaiBiaoTi">
            <h2>固定资产管理</h2>
            <div class="fu-biao-ti">公司固定资产信息列表</div>
        </div>
        
        <!-- 搜索区域 - 修改为与固定资产页面相同样式 -->
        <div class="SouSuoQuYu">
            <div class="CaoZuoQuBiaoTi">搜索条件</div>
            <div class="SouSuoPaiBan">
                <div class="SouSuoTiaoJian">
                    <span class="SouSuoBiaoQian">分类：</span>
                    <asp:DropDownList ID="DropDownList1" runat="server" CssClass="SouSuoXiaLa">
                    </asp:DropDownList>
                </div>
                <div class="SouSuoTiaoJian">
                    <span class="SouSuoBiaoQian">编号：</span>
                    <asp:TextBox ID="TextBox2" runat="server" CssClass="SouSuoShuRu" placeholder="输入编号"></asp:TextBox>
                </div>
                <div class="SouSuoTiaoJian">
                    <span class="SouSuoBiaoQian">仓库名：</span>
                    <asp:TextBox ID="TextBox3" runat="server" CssClass="SouSuoShuRu" placeholder="输入仓库名"></asp:TextBox>
                </div>
                <asp:Button ID="Button1" runat="server" Text="查询" CssClass="AnNiuXiao AnNiuJiChu" OnClick="Button1_Click" />
                <asp:Button ID="btnQingKong" runat="server" Text="清空" CssClass="AnNiuXiao AnNiuJianJie" OnClick="btnMoYe_Click" />
            </div>
        </div>
          </div>
        <div class="BiaoDan">
            <table>  <!-- 移除 border="1"，由CSS控制边框 -->
                <!-- 第一行作为标题行 -->
                <tr>
                    <td class="auto-style1">仓库类型编号</td>
                    <td class="auto-style1">仓库类型</td>
                    <td class="auto-style1">仓库名称</td>
                    <td class="auto-style1">操作</td>
                   
                </tr>
                <asp:Repeater ID="Repeater1" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td><%#Eval("CangKuID") %></td>
                            <td><%#Eval("LeiXingI") %></td>
                            <td><%#Eval("CangKuMing") %></td>
                            <td><a href="CangKuXioGai.aspx?CangKuID=<%#Eval("CangKuID") %>"class="AnNiuXiao AnNiuWeiXian btn-shanchu">修改</a></td>
                           
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </div>
        <!-- 修改分页控件部分，使用与人员管理相同的HTML结构 -->
<div class="FenYeKongZhi" runat="server" id="divFenYe" visible="false">
    <asp:Button ID="btnShouYe" runat="server" Text="首页" CssClass="FenYeAnNiu" OnClick="btnShouYe_Click" />
    <asp:Button ID="btnShangYiYe" runat="server" Text="上一页" CssClass="FenYeAnNiu" OnClick="btnShangYiYe_Click" />
    <span style="margin: 0 10px; color: #666;">
        第 <asp:Label ID="lblDangQianYe" runat="server" Text="1"></asp:Label> 页 / 
        共 <asp:Label ID="lblZongYeShu" runat="server" Text="1"></asp:Label> 页
    </span>
    <asp:Button ID="btnXiaYiYe" runat="server" Text="下一页" CssClass="FenYeAnNiu" OnClick="btnXiaYiYe_Click" />
     <asp:Button ID="Button2" runat="server" Text="末页" CssClass="FenYeAnNiu" OnClick="btnXiaYiYe_Click" />
    
</div>
        <div class="TianJiaHeChaXunPaiBan">
            <div class="XingZenYongHu">
                <a href="/XiTong/CangKuGuanLi/XingZenCangKuFenLei.aspx">新增仓库分类</a>
            </div>
            <div class="ChaXunYongHu">
                <a href="/XiTong/CangKuGuanLi/XinZengCangKu.aspx">新增仓库</a>
            </div>
        </div>
   
</asp:Content>