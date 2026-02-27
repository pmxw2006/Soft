<%@ Page Title="" Language="C#" MasterPageFile="~/MuBanYe/GuDinZiChan.Master" AutoEventWireup="true" CodeBehind="TianJiaJieChu.aspx.cs" Inherits="WebApplication1.GuDinZiChan.GuZiJieHuan.TianJiaJieChu" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/css/DaHeZi.css" rel="stylesheet" />
    <link href="/css/BiaoDan.css" rel="stylesheet" />
    <link href="/css/SouSuo.css" rel="stylesheet" />
    <link href="/GuDinZiChan/css/TianJiaJieChu.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="DaHeZi">
        <!-- 成功/失败提示 -->
        <div id="divChengGong" runat="server" class="TiShiKuang ChengGongTiShi" style="display:none;">
            借出记录添加成功！
        </div>
        <div id="divCuoWu" runat="server" class="TiShiKuang CuoWuTiShi" style="display:none;">
        </div>
        
        <!-- 模块标题 -->
        <div class="MoKuaiBiaoTi">
            <h2>新增固定资产借出记录</h2>
            <div class="fu-biao-ti">添加新的资产借出信息</div>
        </div>
        
        <!-- 表单区域 -->
        <div class="TianJiaBiaoDan">
            <div class="CaoZuoQuBiaoTi">借出信息</div>
            
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
                <label class="BiaoQian BiTianXiang">借出数量</label>
                <div class="ShuRuKong">
                    <asp:TextBox ID="txtJieChuShuLiang" runat="server" CssClass="ShuRuKuang" placeholder="请输入借出数量" TextMode="Number"></asp:TextBox>
                    <span class="tishi" id="cuowuShuLiang"></span>
                </div>
            </div>
            
            <div class="BiaoDanHang">
                <label class="BiaoQian BiTianXiang">租借公司</label>
                <div class="ShuRuKong">
                    <asp:TextBox ID="txtZuJieGongSi" runat="server" CssClass="ShuRuKuang" placeholder="请输入租借公司名称"></asp:TextBox>
                    <span class="tishi" id="cuowuGongSi"></span>
                </div>
            </div>
            
            <div class="BiaoDanHang">
                <label class="BiaoQian BiTianXiang">拟还日期</label>
                <div class="ShuRuKong">
                    <asp:TextBox ID="txtNiHuanRiQi" runat="server" CssClass="ShuRuKuang" TextMode="Date"></asp:TextBox>
                    <span class="tishi" id="cuowuRiQi"></span>
                </div>
            </div>
            
            <!-- 按钮区域 -->
            <div class="AnNiuQu">
                <asp:Button ID="btnBaoCun" runat="server" Text="保存借出" CssClass="AnNiuDa AnNiuChengGong" OnClick="btnBaoCun_Click" />
                <asp:Button ID="btnQuXiao" runat="server" Text="取消" CssClass="AnNiuDa AnNiuQuXiao" OnClick="btnQuXiao_Click" />
            </div>
        </div>
    </div>
    
    <script type="text/javascript">
        // 表单验证
        $(document).ready(function () {
            // 设置默认日期为明天
            var tomorrow = new Date();
            tomorrow.setDate(tomorrow.getDate() + 1);
            var formattedDate = tomorrow.toISOString().split('T')[0];
            $('#<%= txtNiHuanRiQi.ClientID %>').val(formattedDate);

            // 实时验证
            $('#<%= ddlGuZi.ClientID %>').change(function () {
                validateDropdown($(this), 'cuowuGuZi', '请选择要借出的资产');
            });

            $('#<%= txtJieChuShuLiang.ClientID %>').on('input', function () {
                validateNumber($(this), 'cuowuShuLiang', '请输入有效的借出数量');
            });

            $('#<%= txtZuJieGongSi.ClientID %>').on('input', function () {
                validateRequired($(this), 'cuowuGongSi', '请输入租借公司名称');
            });

            $('#<%= txtNiHuanRiQi.ClientID %>').change(function () {
                validateDate($(this), 'cuowuRiQi', '请选择拟还日期');
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
            
            function validateRequired($element, errorId, errorMessage) {
                var $error = $('#' + errorId);
                if ($element.val().trim() === '') {
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
                    var selectedDate = new Date($element.val());
                    var today = new Date();
                    today.setHours(0, 0, 0, 0);
                    
                    if (selectedDate <= today) {
                        $element.addClass('error');
                        $error.text('拟还日期必须大于今天').show();
                        return false;
                    }
                    
                    $element.removeClass('error');
                    $error.hide();
                    return true;
                }
            }
            
            function validateAll() {
                var isValid = true;
                
                isValid = validateDropdown($('#<%= ddlGuZi.ClientID %>'), 'cuowuGuZi', '请选择要借出的资产') && isValid;
                isValid = validateNumber($('#<%= txtJieChuShuLiang.ClientID %>'), 'cuowuShuLiang', '请输入有效的借出数量') && isValid;
                isValid = validateRequired($('#<%= txtZuJieGongSi.ClientID %>'), 'cuowuGongSi', '请输入租借公司名称') && isValid;
                isValid = validateDate($('#<%= txtNiHuanRiQi.ClientID %>'), 'cuowuRiQi', '请选择拟还日期') && isValid;

                return isValid;
            }
        });
    </script>
</asp:Content>