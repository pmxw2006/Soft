<%@ Page Title="" Language="C#" MasterPageFile="~/MuBanYe/XiTong.Master" AutoEventWireup="true" CodeBehind="RenYuanGuanLi.aspx.cs" Inherits="WebApplication1.XiTong.RenYuanGuanLi.RenYuanGuanLi" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- 引入jQuery -->
    <script type="text/javascript" src="/XiTong/js/jquery-3.3.1.min.js"></script>
    <script type="text/javascript" src="/Js/TanChuKuang.js"></script>
    <link href="/css/TanChuKuang.css" rel="stylesheet" />
    <link href="/css/FenYeYangShi.css" rel="stylesheet" />
    <link href="/css/SouSuo.css" rel="stylesheet" />
    <link href="/css/BiaoDan.css" rel="stylesheet" />
    <link href="/css/DaHeZi.css" rel="stylesheet" />
    <link href="/XiTong/css/RenYuanGuanLi.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="DaHeZi">
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                      <!-- 模块标题 -->
        <div class="MoKuaiBiaoTi">
            <h2>人员管理</h2>
            <div class="fu-biao-ti">系统用户信息列表</div>
        </div>
        
        <!-- 搜索区域 -->
        <div class="SouSuoQuYu">
            <div class="CaoZuoQuBiaoTi">搜索条件</div>
            <div class="SouSuoPaiBan">
                <div class="SouSuoTiaoJian">
                    <span class="SouSuoBiaoQian">用户账号：</span>
                    <input type="text" class="SouSuoShuRu" id="txtYongHuZhangHao" placeholder="输入用户账号" runat="server" />
                </div>
                <div class="SouSuoTiaoJian">
                    <span class="SouSuoBiaoQian">真实姓名：</span>
                    <input type="text" class="SouSuoShuRu" id="txtXingMing" placeholder="输入真实姓名" runat="server" />
                </div>
                <div class="SouSuoTiaoJian">
                    <span class="SouSuoBiaoQian">所属部门：</span>
                    <select class="SouSuoXiaLa" id="ddlBuMen" runat="server">
                        <option value="">全部部门</option>
                        <option value="系统">系统</option>
                        <option value="固定资产">固定资产</option>
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
                    <td>用户账号</td>
                    <td>用户名</td>
                    <td>真实姓名</td>
                    <td>身份证号</td>
                    <td>所属部门</td>
                    <td>操作</td>
                </tr>
                <asp:Repeater ID="RepeaterYongHu" runat="server">
    <ItemTemplate>
        <tr>
            <td><%#Eval("YongHuID") %></td>
            <td><%#Eval("YongHuMing") %></td>
            <td><%#Eval("XingMing") %></td>
            <td><%#Eval("ShenFenZheng") %></td>
            <td><%#Eval("BuMen") %></td>
            <td>
                <div class="CaoZuoAnNiu">
                    <asp:Button ID="btnShanChu" runat="server" Text="删除"
                        CssClass="AnNiuXiao AnNiuWeiXian btn-shanchu"
                        CommandArgument='<%#Eval("YongHuID") %>'
                        OnClientClick="return false;"
                        data-zhanghao='<%#Eval("YongHuID") %>'
                        data-xingming='<%#Eval("XingMing") %>'
                        data-bumen='<%#Eval("BuMen") %>' />
                </div>
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
        
        <!-- 新增和查询按钮 -->
        <div class="TianJiaHeChaXunPaiBan">
            <div class="XingZenYongHu">
                <a href="/XiTong/RenYuanGuanLi/TianJiaZhangHu.aspx">新增用户</a>
            </div>
        </div>
    </div>
<!-- 自定义删除确认模态框 -->
<div id="shanChuMoTai" class="shan-chu-mo-tai">
    <div class="mo-tai-nei-rong">
        <div class="mo-tai-biao-ti">
            <span class="mo-tai-ti-shi">确认删除</span>
            <button type="button" class="guan-bi" onclick="closeModal()">×</button>
        </div>
        <div class="mo-tai-ti">
            <div class="jing-gao-tu-biao">
                <svg viewBox="0 0 24 24">
                    <path fill="#e74c3c" d="M12,2C6.48,2,2,6.48,2,12s4.48,10,10,10s10-4.48,10-10S17.52,2,12,2z M13,17h-2v-2h2V17z M13,13h-2V7h2V13z"/>
                </svg>
            </div>
            <h3>确定要删除此用户吗？</h3>
            <p class="mo-tai-wen-zi">删除后，该用户的所有信息将被永久删除且无法恢复。</p>
            
            <div class="yong-hu-xin-xi">
                <div class="xin-xi-hang">
                    <span class="xin-xi-biao-qian">用户账号：</span>
                    <span class="xin-xi-zhi" id="shanChuZhangHao2" runat="server"></span>
                    <asp:TextBox ID="tckHiddenZhangHaoInfo" style="display:none;" runat="server" ClientIDMode="Static"></asp:TextBox>
                </div>
                <div class="xin-xi-hang">
                    <span class="xin-xi-biao-qian">真实姓名：</span>
                    <span class="xin-xi-zhi" id="shanChuXingMing2"></span>
                </div>
                <div class="xin-xi-hang">
                    <span class="xin-xi-biao-qian">所属部门：</span>
                    <span class="xin-xi-zhi" id="shanChuBuMen2"></span>
                </div>
            </div>
            
            <div class="que-ren-quan">
                <label class="que-ren-biao-qian">
                    <input type="checkbox" id="confirmCheck2" onchange="toggleConfirmButton()"/>
                    <span>我已确认要删除此用户</span>
                </label>
            </div>
        </div>
        <div class="mo-tai-jiao">
            <button type="button" class="an-niu an-niu-fu-zhu" onclick="closeModal()">取消</button>
            <asp:Button ID="btnConfirmDelete2" class="an-niu an-niu-wei-xian" runat="server" disabled Text="删除" OnClick="btnShanChu_Command" />
            
        </div>
    </div>
</div>
</asp:Content>