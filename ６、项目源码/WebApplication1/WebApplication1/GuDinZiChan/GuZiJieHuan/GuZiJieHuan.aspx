<%@ Page Title="" Language="C#" MasterPageFile="~/MuBanYe/GuDinZiChan.Master" AutoEventWireup="true" CodeBehind="GuZiJieHuan.aspx.cs" Inherits="WebApplication1.GuDinZiChan.GuZiJieHuan.GuZiJieHuan" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- 引入jQuery -->
    <script type="text/javascript" src="/XiTong/js/jquery-3.3.1.min.js"></script>
    <script type="text/javascript" src="/Js/TanChuKuang.js"></script>
    <link href="/css/FenYeYangShi.css" rel="stylesheet" />
    <link href="/css/TanChuKuang.css" rel="stylesheet" />
    <link href="/css/SouSuo.css" rel="stylesheet" />
    <link href="/css/DaHeZi.css" rel="stylesheet" />
    <link href="/css/BiaoDan.css" rel="stylesheet" />
    <link href="/GuDinZiChan/css/GuZiJieHuan.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="DaHeZi">
        <!-- 模块标题 -->
        <div class="MoKuaiBiaoTi">
            <h2>固定资产借还管理</h2>
            <div class="fu-biao-ti">资产借出与归还记录管理</div>
        </div>
        
        <!-- 搜索区域 -->
        <div class="SouSuoQuYu">
            <div class="CaoZuoQuBiaoTi">搜索条件</div>
            <div class="SouSuoPaiBan">
                <div class="SouSuoTiaoJian">
                    <span class="SouSuoBiaoQian">资产编号：</span>
                    <input type="text" class="SouSuoShuRu" id="txtZiChanBianHao" placeholder="输入资产编号" runat="server" />
                </div>
                <div class="SouSuoTiaoJian">
                    <span class="SouSuoBiaoQian">资产名称：</span>
                    <input type="text" class="SouSuoShuRu" id="txtZiChanMing" placeholder="输入资产名称" runat="server" />
                </div>
                <div class="SouSuoTiaoJian">
                    <span class="SouSuoBiaoQian">类型：</span>
                    <select class="SouSuoXiaLa" id="ddlJieHuanLeiXing" runat="server">
                        <option value="">全部类型</option>
                        <option value="借出">借出记录</option>
                        <option value="归还">归还记录</option>
                    </select>
                </div>
                <asp:Button ID="btnSouSuo" runat="server" Text="搜索" CssClass="AnNiuXiao AnNiuJiChu" OnClick="btnSouSuo_Click" />
                <asp:Button ID="btnQingKong" runat="server" Text="清空" CssClass="AnNiuXiao AnNiuJianJie" OnClick="btnQingKong_Click" />
            </div>
        </div>
        
        <!-- 表格区域 -->
        <div class="BiaoDan">
            <table>
                <!-- 第一行作为标题行 -->
                <tr>
                    <td>资产编号</td>
                    <td>资产名称</td>
                    <td>公司名称</td>
                    <td>数量</td>
                    <td>日期</td>
                    <td>类型</td>
                    <%--<td>操作</td>--%>
                </tr>
                <asp:Repeater ID="RepeaterJieHuan" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td><%# Eval("GuZiID") %></td>
                            <td><%# Eval("GuZiMing") %></td>
                            <td><%# Eval("GongSi") %></td>  
                            <td><%# Eval("ShuLiang") %></td>
                            <td class="RiQiZhi">
                                <%# Convert.ToDateTime(Eval("RiQi")).ToString("yyyy-MM-dd") %>
                            </td>
                            <td>
                                <span
                                    <%# Eval("LeiXing").ToString() == "借出" ? "ZhuangTaiJieChu" : "ZhuangTaiGuiHuan" %>">
                                    <%# Eval("LeiXing") %>
                                </span>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </div>
        
        <!-- 分页控制 -->
        <div class="FenYeKongZhi" runat="server" id="divFenYe" visible="false">
            <asp:Button ID="btnShouYe" runat="server" Text="首页" CssClass="FenYeAnNiu" OnClick="btnShouYe_Click" />
            <asp:Button ID="btnShangYiYe" runat="server" Text="上一页" CssClass="FenYeAnNiu" OnClick="btnShangYiYe_Click" />
            <span style="margin: 0 10px; color: #666;">第 <asp:Label ID="lblDangQianYe" runat="server" Text="1"></asp:Label> 页 / 共 <asp:Label ID="lblZongYeShu" runat="server" Text="1"></asp:Label> 页</span>
            <asp:Button ID="btnXiaYiYe" runat="server" Text="下一页" CssClass="FenYeAnNiu" OnClick="btnXiaYiYe_Click" />
            <asp:Button ID="btnMoYe" runat="server" Text="末页" CssClass="FenYeAnNiu" OnClick="btnMoYe_Click" />
        </div>

        <!-- 新增按钮区域 -->
        <div class="TianJiaHeChaXunPaiBan">
            <div class="XingZenYongHu">
                <a href="/GuDinZiChan/GuZiJieHuan/TianJiaJieChu.aspx" class="jiechu">新增借出记录</a>
                </div>
            <div class="XingZenYongHu">
                <a href="/GuDinZiChan/GuZiJieHuan/TianJiaGuiHuan.aspx" class="guihuan">新增归还记录</a>
            </div>
        </div>
    </div>
</asp:Content>
