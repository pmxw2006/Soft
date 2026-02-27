<%@ Page Title="" Language="C#" MasterPageFile="~/MuBanYe/GuDinZiChan.Master" AutoEventWireup="true" CodeBehind="ZiChanZhuTi.aspx.cs" Inherits="WebApplication1.GuDinZiChan.GuDinZiChanChaoZuo.ZiChanZhuTi" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/css/TanChuKuang.css" rel="stylesheet" />
    <link href="/css/TanChuKuang.css" rel="stylesheet" />
    <link href="/css/FenYeYangShi.css" rel="stylesheet" />
    <link href="/css/SouSuo.css" rel="stylesheet" />
    <link href="/css/BiaoDan.css" rel="stylesheet" />
    <link href="/css/DaHeZi.css" rel="stylesheet" />
    <link href="/GuDinZiChan/css/ZiCHanZHuTi.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <div class="DaHeZi">
        <!-- 添加模块标题 -->
        <div class="MoKuaiBiaoTi">
            <h2>固定资产管理</h2>
            <div class="fu-biao-ti">公司固定资产信息列表</div>
        </div>
        <div class="SouSuoQuYu">
            <div class="CaoZuoQuBiaoTi">搜索条件</div>
            <div class="SouSuoPaiBan">
                
                <div class="SouSuoTiaoJian">
                    <span class="SouSuoBiaoQian">仓库编号或仓库姓名：</span>
                     <asp:TextBox ID="txtSearch" runat="server"  class="SouSuoShuRu" placeholder="输入关键词"></asp:TextBox>
                </div>
                <asp:Button ID="btnSouSuo" runat="server" Text="搜索" CssClass="AnNiuXiao AnNiuJiChu" OnClick="btnSearch_Click" />
                <asp:Button ID="btnQingKong" runat="server" Text="清空" CssClass="AnNiuXiao AnNiuJianJie" OnClick="btnQingKong_Click" />
            </div>
        </div>
        

        <div class="BiaoDan">
            <table>  <!-- 移除 border="1"，由CSS控制边框 -->
                <!-- 第一行作为标题行 -->
                <tr>
                    <td>编号</td>
                    <td>名称</td>
                    <td>所属仓库</td>
                    <td>数量</td>
                    <td>价值</td>
                    <td>日期</td>
                    <td>状态</td>
                    <td>操作</td>
                </tr>
                <asp:Repeater ID="Repeater1" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td><%#Eval("GuZiID") %></td>
                            <td><%#Eval("GuZiMing") %></td>
                            <td><%#Eval("CangKuMing") %></td>
                            <td><%#Eval("GuZiShuLiang") %></td>
                            <td><%#Eval("GuZiJiaZhi") %></td>
                            <td><%#Eval("ChuChangRiQi") %></td>
                            <td>
    <%# Convert.ToInt32(Eval("ZhuangTaiID")) == 1 ? "在库" : 
         Convert.ToInt32(Eval("ZhuangTaiID")) == 2 ? "维修中" : 
         Convert.ToInt32(Eval("ZhuangTaiID")) == 3 ? "借出中" : 
         Eval("ZhuangTaiID") %>
</td>
                          <td>   
                              <div class="CaoZuoAnNiu">
                              <a class="AnNiuXiao AnNiuWeiXian btn-shanchu"" href="ZiChanZhuTi.aspx?GuZiID=<%#Eval("GuZiID") %>">删除</a>
                                  </div>

                </td>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        <!-- 分页控件 -->
        <div class="FenYeKongZhi" runat="server" id="divFenYe" visible="false">
            <asp:Button ID="btnShouYe" runat="server" Text="首页" CssClass="FenYeAnNiu" OnClick="btnShouYe_Click" />
            <asp:Button ID="btnShangYiYe" runat="server" Text="上一页" CssClass="FenYeAnNiu" OnClick="btnShangYiYe_Click" />
            <span style="margin: 0 10px; color: #666;">
                第 <asp:Label ID="lblDangQianYe" runat="server" Text="1"></asp:Label> 页 / 
                共 <asp:Label ID="lblZongYeShu" runat="server" Text="1"></asp:Label> 页
            </span>
            <asp:Button ID="btnXiaYiYe" runat="server" Text="下一页" CssClass="FenYeAnNiu" OnClick="btnXiaYiYe_Click" />
            <asp:Button ID="Button2" runat="server" Text="末页" CssClass="FenYeAnNiu" OnClick="btnMoYe_Click" />
        </div>
        <div class="TianJiaHeChaXunPaiBan">
            <div class="XingZenYongHu">
                <a href="XinZenZiChan.aspx">新增资产</a>
            </div>
        </div>
    </div>
    </div>
</asp:Content>

