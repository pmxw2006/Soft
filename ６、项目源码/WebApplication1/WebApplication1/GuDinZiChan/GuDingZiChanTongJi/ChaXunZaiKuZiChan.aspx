<%@ Page Title="" Language="C#" MasterPageFile="~/MuBanYe/GuDinZiChan.Master" AutoEventWireup="true" CodeBehind="ChaXunZaiKuZiChan.aspx.cs" Inherits="WebApplication1.GuDinZiChan.GuDingZiChanTongJi.ChaXunZaiKuZiChan" %>
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
            <h2>固定资产报表</h2>
            <div class="fu-biao-ti">公司固定资产信息列表</div>
        </div>
        <div class="SouSuoQuYu">
            <div class="CaoZuoQuBiaoTi">搜索条件</div>
            <div class="SouSuoPaiBan">
                
                <div class="SouSuoTiaoJian">
                    <span class="SouSuoBiaoQian">仓库编号或仓库姓名：</span>
                     <asp:TextBox ID="txtSearch" runat="server"  class="SouSuoShuRu" placeholder="输入关键词"></asp:TextBox>
                </div>
                
                <!-- 添加日期查询条件 -->
                <div class="SouSuoTiaoJian">
                    <span class="SouSuoBiaoQian">出厂日期：</span>
                    <asp:TextBox ID="txtStartDate" runat="server" TextMode="Date" CssClass="SouSuoShuRu" style="width: 150px;"></asp:TextBox>
                    <span style="margin: 0 5px;">至</span>
                    <asp:TextBox ID="txtEndDate" runat="server" TextMode="Date" CssClass="SouSuoShuRu" style="width: 150px;"></asp:TextBox>
                </div>
                
                <asp:Button ID="btnSouSuo" runat="server" Text="搜索" CssClass="AnNiuXiao AnNiuJiChu" OnClick="btnSouSuo_Click" />
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
                </tr>
                <asp:Repeater ID="Repeater1" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td><%#Eval("GuZiID") %></td>
                            <td><%#Eval("GuZiMing") %></td>
                            <td><%#Eval("CangKuMing") %></td>
                            <td><%#Eval("GuZiShuLiang") %></td>
                            <td><%#Eval("GuZiJiaZhi") %></td>
                            <td><%# Convert.ToDateTime(Eval("ChuChangRiQi")).ToString("yyyy-MM-dd") %></td>
                            <td>
    <%# Convert.ToInt32(Eval("ZhuangTaiID")) == 1 ? "在库" : 
         Convert.ToInt32(Eval("ZhuangTaiID")) == 2 ? "维修中" : 
         Convert.ToInt32(Eval("ZhuangTaiID")) == 3 ? "借出中" : 
         "未知" %>
</td>
                        </tr>
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
                <a href="ChaXunZaiKuZiChan.aspx">在库资产</a>
                <a href="ChaXunWeiXiuZiChan.aspx">维修资产</a>
                <a href="ChaXunJeiHuanZiChan.aspx">借还资产</a>
            </div>
        </div>
    </div>
    </div>
    
    <script>
        // 页面加载时设置默认日期
        document.addEventListener('DOMContentLoaded', function() {
            var endDateInput = document.getElementById('<%= txtEndDate.ClientID %>');
            var startDateInput = document.getElementById('<%= txtStartDate.ClientID %>');

            // 如果结束日期为空，设置为今天
            if (endDateInput && !endDateInput.value) {
                var today = new Date().toISOString().split('T')[0];
                endDateInput.value = today;
            }

            // 如果开始日期为空，设置为一个月前
            if (startDateInput && !startDateInput.value) {
                var oneMonthAgo = new Date();
                oneMonthAgo.setMonth(oneMonthAgo.getMonth() - 1);
                startDateInput.value = oneMonthAgo.toISOString().split('T')[0];
            }
        });
    </script>
</asp:Content>
