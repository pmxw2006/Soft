<%@ Page Title="" Language="C#" MasterPageFile="~/MuBanYe/GuDinZiChan.Master" AutoEventWireup="true" CodeBehind="ChaXunWeiXiuZiChan.aspx.cs" Inherits="WebApplication1.GuDinZiChan.GuDingZiChanTongJi.ChaXunWeiXiuZiChan" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <link href="/css/FenYeYangShi.css" rel="stylesheet" />
 <link href="/css/TanChuKuang.css" rel="stylesheet" />
 <link href="/css/SouSuo.css" rel="stylesheet" />
 <link href="/css/DaHeZi.css" rel="stylesheet" />
 <link href="/css/BiaoDan.css" rel="stylesheet" />
 <link href="/GuDinZiChan/css/GuZiWeiXiu.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
       <div class="DaHeZi">
     <!-- 模块标题 -->
     <div class="MoKuaiBiaoTi">
         <h2>固定资产维修报表</h2>
         <div class="fu-biao-ti">资产维修记录与状态跟踪</div>
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
                 <div class="SouSuoTiaoJian">
    <span class="SouSuoBiaoQian">送修日期：</span>
    <input type="date" class="RiQiKongJian" id="txtSongXiuStart" runat="server" />
    <span class="RiQiFuHao">至</span>
    <input type="date" class="RiQiKongJian" id="txtSongXiuEnd" runat="server" />
</div>
             </div>
             <div class="SouSuoTiaoJian">
    <span class="SouSuoBiaoQian">维修状态：</span>
    <select class="SouSuoShuRu" id="ddlWeiXiuZhuangTai" runat="server">
        <option value="">全部</option>
        <option value="0">未完成</option>
        <option value="1">已完成</option>
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
                 <td>维修数量</td>
                 <td>送修日期</td>
                 <td>预期完成</td>
                 <td>实际完成</td>
                 <td>维修金额</td>
                 <!-- 移除了操作列 -->
             </tr>
             <asp:Repeater ID="RepeaterWeiXiu" runat="server">
    <ItemTemplate>
        <tr>
            <td><%# Eval("GuZiID") %></td>
            <td><%# Eval("GuZiMing") %></td>
            <td><%# Eval("WeiXiuShuLiang") %></td>
            <td><%# Convert.ToDateTime(Eval("SongXiuRiQi")).ToString("yyyy-MM-dd") %></td>
            <td><%# Eval("YuWanRiQi") != DBNull.Value ? Convert.ToDateTime(Eval("YuWanRiQi")).ToString("yyyy-MM-dd") : "" %></td>
            <td><%# Eval("ShiWanRiQi") != DBNull.Value ? Convert.ToDateTime(Eval("ShiWanRiQi")).ToString("yyyy-MM-dd") : "未完成" %></td>
            <td>¥<%# string.Format("{0:N2}", Eval("WeiXiuJinE")) %></td>
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
     
     <!-- 新增按钮 -->
      <div class="TianJiaHeChaXunPaiBan">
    <div class="XingZenYongHu">
          <a href="ChaXunZaiKuZiChan.aspx">在库资产</a>
          <a href="ChaXunWeiXiuZiChan.aspx">维修资产</a>
          <a href="ChaXunJeiHuanZiChan.aspx">借还资产</a>
      </div>
  </div>
 </div>
</asp:Content>
