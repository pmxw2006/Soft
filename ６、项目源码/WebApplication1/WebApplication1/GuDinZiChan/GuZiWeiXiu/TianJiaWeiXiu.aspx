<%@ Page Title="" Language="C#" MasterPageFile="~/MuBanYe/GuDinZiChan.Master" AutoEventWireup="true" CodeBehind="TianJiaWeiXiu.aspx.cs" Inherits="WebApplication1.GuDinZiChan.GuZiWeiXiu.TianJiaWeiXiu" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/css/DaHeZi.css" rel="stylesheet" />
    <link href="/css/BiaoDan.css" rel="stylesheet" />
    <link href="/css/SouSuo.css" rel="stylesheet" />
    <link href="/GuDinZiChan/css/TianJiaWeiXiu.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="DaHeZi">
        <!-- 成功/失败提示 -->
        <div id="divChengGong" runat="server" class="TiShiKuang ChengGongTiShi" style="display:none;">
            维修记录添加成功！
        </div>
        <div id="divCuoWu" runat="server" class="TiShiKuang CuoWuTiShi" style="display:none;">
        </div>
        
        <!-- 模块标题 -->
        <div class="MoKuaiBiaoTi">
            <h2>固定资产维修管理</h2>
            <div class="fu-biao-ti">新增资产维修记录</div>
        </div>
        
        <!-- 表单区域 -->
        <div class="TianJiaBiaoDan">
            <div class="CaoZuoQuBiaoTi">维修信息</div>
            
            <div class="BiaoDanHang">
                <label class="BiaoQian BiTianXiang">选择资产</label>
                <div class="ShuRuKong">
                    <asp:DropDownList ID="ddlGuZi" runat="server" CssClass="XiaLaKuang" AutoPostBack="true" OnSelectedIndexChanged="ddlGuZi_SelectedIndexChanged">
                        <asp:ListItem Value="">-- 请选择资产 --</asp:ListItem>
                    </asp:DropDownList>
                    <span class="tishi" id="cuowuGuZi"></span>
                </div>
            </div>
            
            <div class="BiaoDanHang">
                <label class="BiaoQian">当前数量</label>
                <div class="ShuRuKong">
                    <asp:TextBox ID="txtDangQianShuLiang" runat="server" CssClass="ZhiDuKuang" ReadOnly="true" placeholder="选择资产后自动显示"></asp:TextBox>
                </div>
            </div>
            
            <div class="BiaoDanHang">
                <label class="BiaoQian BiTianXiang">维修数量</label>
                <div class="ShuRuKong">
                    <asp:TextBox ID="txtWeiXiuShuLiang" runat="server" CssClass="ShuRuKuang" placeholder="请输入维修数量" TextMode="Number"></asp:TextBox>
                    <span class="tishi" id="cuowuShuLiang"></span>
                </div>
            </div>
            
            <div class="BiaoDanHang">
                <label class="BiaoQian BiTianXiang">送修日期</label>
                <div class="ShuRuKong">
                    <asp:TextBox ID="txtSongXiuRiQi" runat="server" CssClass="ShuRuKuang" TextMode="Date"></asp:TextBox>
                    <span class="tishi" id="cuowuSongXiuRiQi"></span>
                </div>
            </div>
            
            <div class="BiaoDanHang">
                <label class="BiaoQian BiTianXiang">预期完成日期</label>
                <div class="ShuRuKong">
                    <asp:TextBox ID="txtYuWanRiQi" runat="server" CssClass="ShuRuKuang" TextMode="Date"></asp:TextBox>
                    <span class="tishi" id="cuowuYuWanRiQi"></span>
                </div>
            </div>
            
            <div class="BiaoDanHang">
                <label class="BiaoQian BiTianXiang">维修金额</label>
                <div class="ShuRuKong">
                    <asp:TextBox ID="txtWeiXiuJinE" runat="server" CssClass="ShuRuKuang" placeholder="请输入维修金额" TextMode="Number" step="0.01"></asp:TextBox>
                    <span class="tishi" id="cuowuJinE"></span>
                </div>
            </div>
            
            <!-- 按钮区域 -->
            <div class="AnNiuQu">
                <asp:Button ID="btnBaoCun" runat="server" Text="保存维修" CssClass="AnNiuDa AnNiuChengGong" OnClick="btnBaoCun_Click" />
                <asp:Button ID="btnQuXiao" runat="server" Text="取消" CssClass="AnNiuDa AnNiuQuXiao" OnClick="btnQuXiao_Click" />
            </div>
        </div>
    </div>
    
    <script type="text/javascript">
        // 表单验证
        $(document).ready(function () {
            // 设置默认日期
            var today = new Date();
            var tomorrow = new Date();
            tomorrow.setDate(tomorrow.getDate() + 1);

            var todayStr = today.toISOString().split('T')[0];
            var tomorrowStr = tomorrow.toISOString().split('T')[0];

            $('#<%= txtSongXiuRiQi.ClientID %>').val(todayStr);
            $('#<%= txtYuWanRiQi.ClientID %>').val(tomorrowStr);

            // 实时验证
            $('#<%= ddlGuZi.ClientID %>').change(function () {
                validateDropdown($(this), 'cuowuGuZi', '请选择要维修的资产');
            });

            $('#<%= txtWeiXiuShuLiang.ClientID %>').on('input', function () {
                validateNumber($(this), 'cuowuShuLiang', '请输入有效的维修数量');
            });

            $('#<%= txtSongXiuRiQi.ClientID %>').change(function () {
                validateDate($(this), 'cuowuSongXiuRiQi', '请选择送修日期');
            });

            $('#<%= txtYuWanRiQi.ClientID %>').change(function () {
                validateDate($(this), 'cuowuYuWanRiQi', '请选择预期完成日期');
            });

            $('#<%= txtWeiXiuJinE.ClientID %>').on('input', function () {
                validateAmount($(this), 'cuowuJinE', '请输入有效的维修金额');
            });
            
            // 保存按钮点击验证
            $('#<%= btnBaoCun.ClientID %>').click(function (e) {
                if (!validateAll()) {
                    e.preventDefault();
                }
            });
            
            // 验证函数
            function validateDropdown($element, errorId, errorMessage) {
                var $error = $('#' + errorId);
                if ($element.val() === '') {
                    $element.addClass('error');
                    $error.text(errorMessage).show();
                    return false;
                } else {
                    $element.removeClass('error');
                    $error.hide();
                    return true;
                }
            }
            
            function validateNumber($element, errorId, errorMessage) {
                var $error = $('#' + errorId);
                var value = $element.val();
                if (value === '' || parseInt(value) <= 0) {
                    $element.addClass('error');
                    $error.text(errorMessage).show();
                    return false;
                } else {
                    $element.removeClass('error');
                    $error.hide();
                    return true;
                }
            }
            
            function validateDate($element, errorId, errorMessage) {
                var $error = $('#' + errorId);
                if ($element.val() === '') {
                    $element.addClass('error');
                    $error.text(errorMessage).show();
                    return false;
                } else {
                    $element.removeClass('error');
                    $error.hide();
                    return true;
                }
            }
            
            function validateAmount($element, errorId, errorMessage) {
                var $error = $('#' + errorId);
                var value = $element.val();
                if (value === '' || parseFloat(value) <= 0) {
                    $element.addClass('error');
                    $error.text(errorMessage).show();
                    return false;
                } else {
                    $element.removeClass('error');
                    $error.hide();
                    return true;
                }
            }
            
            function validateAll() {
                var isValid = true;
                
                isValid = validateDropdown($('#<%= ddlGuZi.ClientID %>'), 'cuowuGuZi', '请选择要维修的资产') && isValid;
                isValid = validateNumber($('#<%= txtWeiXiuShuLiang.ClientID %>'), 'cuowuShuLiang', '请输入有效的维修数量') && isValid;
                isValid = validateDate($('#<%= txtSongXiuRiQi.ClientID %>'), 'cuowuSongXiuRiQi', '请选择送修日期') && isValid;
                isValid = validateDate($('#<%= txtYuWanRiQi.ClientID %>'), 'cuowuYuWanRiQi', '请选择预期完成日期') && isValid;
                isValid = validateAmount($('#<%= txtWeiXiuJinE.ClientID %>'), 'cuowuJinE', '请输入有效的维修金额') && isValid;

                return isValid;
            }
        });
    </script>
</asp:Content>