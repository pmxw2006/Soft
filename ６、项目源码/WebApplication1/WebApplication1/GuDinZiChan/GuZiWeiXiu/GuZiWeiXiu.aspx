<%@ Page Title="" Language="C#" MasterPageFile="~/MuBanYe/GuDinZiChan.Master" AutoEventWireup="true" CodeBehind="GuZiWeiXiu.aspx.cs" Inherits="WebApplication1.GuDinZiChan.GuZiWeiXiu.GuZiWeiXiu" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- 引入jQuery -->
    <script type="text/javascript" src="/XiTong/js/jquery-3.3.1.min.js"></script>
    <script type="text/javascript" src="/Js/TanChuKuasng.js"></script>
    <link href="/css/FenYeYangShi.css" rel="stylesheet" />
    <link href="/css/TanChuKuang.css" rel="stylesheet" />
    <link href="/css/SouSuo.css" rel="stylesheet" />
    <link href="/css/DaHeZi.css" rel="stylesheet" />
    <link href="/css/BiaoDan.css" rel="stylesheet" />
    <link href="/GuDinZiChan/css/GuZiWeiXiu.css" rel="stylesheet" />
    <style type="text/css">
        .auto-style2 {
            height: 64px;
        }
        .auto-style3 {
            font-family: 'Consolas', monospace;
            color: #666;
            height: 64px;
        }
        .auto-style4 {
            font-weight: bold;
            color: #d32f2f;
            height: 64px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="DaHeZi">
        <!-- 模块标题 -->
        <div class="MoKuaiBiaoTi">
            <h2>固定资产维修管理</h2>
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
                    <td>维修金额</td>
                    <td>实际完成</td>
                    <td>操作</td>
                </tr>
                <asp:Repeater ID="RepeaterWeiXiu" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td><%# Eval("GuZiID") %></td>
                            <td><%# Eval("GuZiMing") %></td>
                            <td><%# Eval("WeiXiuShuLiang") %></td>
                            <td class="RiQiZhi"><%# Convert.ToDateTime(Eval("SongXiuRiQi")).ToString("yyyy-MM-dd") %></td>
                            <td class="RiQiZhi"><%# Convert.ToDateTime(Eval("YuWanRiQi")).ToString("yyyy-MM-dd") %></td>
                            <td class="JinEZhi">¥<%# string.Format("{0:N2}", Eval("WeiXiuJinE")) %></td>
                            <td class="RiQiZhi">
    <%# Eval("ShiWanRiQi") != DBNull.Value ? 
         Convert.ToDateTime(Eval("ShiWanRiQi")).ToString("yyyy-MM-dd") : 
         "未完成" %>
</td>
<td>
    <asp:Button ID="btnShanChu" runat="server" Text="完修"
        CssClass="AnNiuXiao AnNiuWeiXian btn-shanchu"
        OnClientClick="return false;"
        data-guziid='<%# Eval("GuZiID") %>'
        data-zichanming='<%# Eval("GuZiMing") %>'
        data-weixiujine='<%# 
            Eval("WeiXiuJinE") != DBNull.Value ? 
            string.Format("{0:N2}", Eval("WeiXiuJinE")) : 
            "0.00"
        %>'
        Visible='<%# Eval("ShiWanRiQi") == DBNull.Value %>' />
    <span class="yi-wan-cheng" style="color: #4CAF50; font-weight: bold;" 
          Visible='<%# Eval("ShiWanRiQi") != DBNull.Value %>' runat="server">
        已完成
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
        
        <!-- 新增按钮 -->
        <div class="TianJiaHeChaXunPaiBan">
            <div class="XingZenYongHu">
                <a href="/GuDinZiChan/GuZiWeiXiu/TianJiaWeiXiu.aspx">新增维修记录</a>
            </div>
        </div>
    </div>
    
<!-- 自定义删除确认模态框 -->
<div id="shanChuMoTai" class="shan-chu-mo-tai" style="display: none;">
    <div class="mo-tai-nei-rong">
        <div class="mo-tai-biao-ti">
            <span class="mo-tai-ti-shi">确认是否完修记录</span>
            <button type="button" class="guan-bi" onclick="closeModal()">×</button>
        </div>
        <div class="mo-tai-ti">
            <div class="jing-gao-tu-biao">
                <svg viewBox="0 0 24 24">
                    <path fill="#e74c3c" d="M12,2C6.48,2,2,6.48,2,12s4.48,10,10,10s10-4.48,10-10S17.52,2,12,2z M13,17h-2v-2h2V17z M13,13h-2V7h2V13z"/>
                </svg>
            </div>
            <h3>请确认该固定资产是否已经维修？</h3>
            <p class="mo-tai-wen-zi">确认后，该维修记录的所有信息将无法在该界面展示，详情请查看固定资产维修报表。</p>
            
            <div class="yong-hu-xin-xi">
                <div class="xin-xi-hang">
                    <span class="xin-xi-biao-qian">固定资产ID：</span>
                    <span class="xin-xi-zhi" id="shanChuGuZiID"></span>
                    <asp:HiddenField ID="hiddenGuZiID" runat="server" />
                </div>
                <div class="xin-xi-hang">
                    <span class="xin-xi-biao-qian">资产名称：</span>
                    <span class="xin-xi-zhi" id="shanChuZiChanMing"></span>
                </div>
                <div class="xin-xi-hang">
                    <span class="xin-xi-biao-qian">维修金额：</span>
                    <span class="xin-xi-zhi" id="shanChuWeiXiuJinE"></span>
                </div>
            </div>
            
            <div class="que-ren-quan">
                <label class="que-ren-biao-qian">
                    <input type="checkbox" id="confirmCheck" onchange="toggleConfirmButton()"/>
                    <span>我已确认该固定资产已经维修</span>
                </label>
            </div>
        </div>
        <div class="mo-tai-jiao">
            <button type="button" class="an-niu an-niu-fu-zhu" onclick="closeModal()">取消</button>
            <asp:Button ID="btnConfirmDelete" class="an-niu an-niu-wei-xian" runat="server" 
                disabled="disabled" Text="完修" OnClick="btnShanChu_Command" />
        </div>
    </div>
</div>
    <script type="text/javascript">
        // 页面加载完成后执行
        $(document).ready(function () {
            // 绑定删除按钮点击事件
            $('.btn-shanchu').click(function () {
                // 获取按钮上存储的数据
                var guZiID = $(this).data('guziid');
                var ziChanMing = $(this).data('zichanming');
                var weiXiuJinE = $(this).data('weixiujine');

                // 显示在模态框中
                $('#shanChuGuZiID').text(guZiID);
                $('#shanChuZiChanMing').text(ziChanMing);
                $('#shanChuWeiXiuJinE').text('¥' + weiXiuJinE);

                // 设置隐藏字段的值
                $('#<%= hiddenGuZiID.ClientID %>').val(guZiID);
            
            // 重置确认复选框
            $('#confirmCheck').prop('checked', false);
            toggleConfirmButton();
            
            // 显示模态框 - 使用CSS类控制显示和居中
            $('#shanChuMoTai').css('display', 'flex');
        });
    });
    
    // 关闭模态框
    function closeModal() {
        $('#shanChuMoTai').css('display', 'none');
    }
    
    // 切换确认按钮状态
    function toggleConfirmButton() {
        var isChecked = $('#confirmCheck').prop('checked');
        $('#<%= btnConfirmDelete.ClientID %>').prop('disabled', !isChecked);
        }

        // 点击模态框外部关闭
        $(document).on('click', function (e) {
            if ($(e.target).hasClass('shan-chu-mo-tai')) {
                closeModal();
            }
        });

        // ESC键关闭模态框
        $(document).keydown(function (e) {
            if (e.keyCode === 27) { // ESC键
                closeModal();
            }
        });
    </script>
</asp:Content>
