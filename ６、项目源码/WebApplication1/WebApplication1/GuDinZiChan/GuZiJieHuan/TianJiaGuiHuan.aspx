<%@ Page Title="" Language="C#" MasterPageFile="~/MuBanYe/GuDinZiChan.Master" AutoEventWireup="true" CodeBehind="TianJiaGuiHuan.aspx.cs" Inherits="WebApplication1.GuDinZiChan.GuZiJieHuan.TianJiaGuiHuan" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/css/DaHeZi.css" rel="stylesheet" />
    <link href="/css/BiaoDan.css" rel="stylesheet" />
    <link href="/css/SouSuo.css" rel="stylesheet" />
    <link href="/GuDinZiChan/css/TianJiaGuiHuan.css" rel="stylesheet" />
    <style type="text/css">
        
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="DaHeZi">
        <!-- 成功/失败提示 -->
        <div id="divChengGong" runat="server" class="TiShiKuang ChengGongTiShi" style="display:none;">
            归还记录添加成功！
        </div>
        <div id="divCuoWu" runat="server" class="TiShiKuang CuoWuTiShi" style="display:none;">
        </div>
        
        <!-- 模块标题 -->
        <div class="MoKuaiBiaoTi">
            <h2>新增固定资产归还记录</h2>
            <div class="fu-biao-ti">添加新的资产归还信息</div>
        </div>
        
        <!-- 表单区域 -->
        <div class="TianJiaBiaoDan">
            <div class="CaoZuoQuBiaoTi">归还信息</div>
            
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
                <label class="BiaoQian BiTianXiang">归还数量</label>
                <div class="ShuRuKong">
                    <asp:TextBox ID="txtGuiHuanShuLiang" runat="server" CssClass="ShuRuKuang" placeholder="请输入归还数量" TextMode="Number"></asp:TextBox>
                    <span class="tishi" id="cuowuShuLiang"></span>
                </div>
            </div>
            
            <div class="BiaoDanHang">
                <label class="BiaoQian BiTianXiang">归还公司</label>
                <div class="ShuRuKong">
                    <asp:TextBox ID="txtGuiHuanGongSi" runat="server" CssClass="ZhiDuKuang" ReadOnly="true" placeholder="选择资产后自动显示"></asp:TextBox>
                    <span class="tishi" id="cuowuGongSi"></span>
                </div>
            </div>
            
            <div class="BiaoDanHang">
                <label class="BiaoQian BiTianXiang">归还日期</label>
                <div class="ShuRuKong">
                    <asp:TextBox ID="txtGuiHuanRiQi" runat="server" CssClass="ShuRuKuang" TextMode="Date"></asp:TextBox>
                    <span class="tishi" id="cuowuRiQi"></span>
                </div>
            </div>
            
            <!-- 按钮区域 -->
            <div class="AnNiuQu">
                <asp:Button ID="btnBaoCun" runat="server" Text="保存归还" CssClass="AnNiuDa AnNiuChengGong" OnClick="btnBaoCun_Click" />
                <asp:Button ID="btnQuXiao" runat="server" Text="取消" CssClass="AnNiuDa AnNiuQuXiao" OnClick="btnQuXiao_Click" />
            </div>
        </div>
    </div>
    
    <script type="text/javascript">
        // 表单验证
        $(document).ready(function () {
            // 初始化时隐藏所有错误提示
            $('.tishi').hide();

            // 实时验证
            $('#<%= ddlGuZi.ClientID %>').change(function () {
                validateDropdown($(this), 'cuowuGuZi', '请选择要归还的资产');
            });

            $('#<%= txtGuiHuanShuLiang.ClientID %>').on('input', function () {
                validateNumber($(this), 'cuowuShuLiang', '请输入有效的归还数量');
            });

            $('#<%= txtGuiHuanRiQi.ClientID %>').change(function () {
                validateDate($(this), 'cuowuRiQi', '请选择归还日期');
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
                    var selectedDate = new Date($element.val());
                    var today = new Date();

                    // 重置两个日期的时间部分，只比较年月日
                    selectedDate.setHours(0, 0, 0, 0);
                    today.setHours(0, 0, 0, 0);

                    if (selectedDate > today) {
                        $element.addClass('error');
                        $error.text('归还日期不能是未来').show();
                        return false;
                    }

                    $element.removeClass('error');
                    $error.hide();
                    return true;
                }
            }
            
            function validateAll() {
                var isValid = true;
                
                isValid = validateDropdown($('#<%= ddlGuZi.ClientID %>'), 'cuowuGuZi', '请选择要归还的资产') && isValid;
                isValid = validateNumber($('#<%= txtGuiHuanShuLiang.ClientID %>'), 'cuowuShuLiang', '请输入有效的归还数量') && isValid;
                isValid = validateDate($('#<%= txtGuiHuanRiQi.ClientID %>'), 'cuowuRiQi', '请选择归还日期') && isValid;
                
                // 验证公司名称
                var gongSiValue = $('#<%= txtGuiHuanGongSi.ClientID %>').val().trim();
                if (gongSiValue === '') {
                    $('#cuowuGongSi').text('请选择资产或输入归还公司名称').show();
                    isValid = false;
                } else {
                    $('#cuowuGongSi').hide();
                }

                return isValid;
            }
        });
    </script>
</asp:Content>