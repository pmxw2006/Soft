<%@ Page Title="" Language="C#" MasterPageFile="~/MuBanYe/GuDinZiChan.Master" AutoEventWireup="true" CodeBehind="XinZenZiChan.aspx.cs" Inherits="WebApplication1.GuDinZiChan.GuDinZiChanChaoZuo.XinZenZiChan" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <!-- 引入jQuery -->
    <script type="text/javascript" src="/XiTong/js/jquery-3.3.1.min.js"></script>
    <link href="/GuDinZiChan/css/XinZenZiChan.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
       <div class="xin-zen-zi-chan">
        <!-- 消息提示区域 -->
        <asp:Panel ID="PanXiaoXi" runat="server" CssClass="xiao-xi-pan" Visible="false">
            <asp:Label ID="LabXiaoXi" runat="server" Text=""></asp:Label>
        </asp:Panel>
        
        <!-- 个人信息头部 -->
        <div class="tou-bu">
            <h2>新增固定资产</h2>
            <div class="fu-biao-ti">录入新的固定资产信息</div>
        </div>
        
        <!-- 资产信息 -->
        <div class="xin-xi-qu">
            <div class="xin-xi-biao-ti">资产信息</div>
            <div class="xin-xi-wang-ge">
                <!-- 资产名称输入框 -->
                <div class="xin-xi-xiang-mu">
                    <span class="xin-xi-biao-qian">资产名称</span>
                    <asp:TextBox ID="WenBenZiChanMingCheng" runat="server" 
                        CssClass="bian-ji-kuang" placeholder="请输入资产名称"></asp:TextBox>
                    <span class="tishi" id="zi-chan-ming-cheng-tishi"></span>
                </div>
                
                <!-- 仓库选择下拉框 -->
                <div class="xin-xi-xiang-mu">
                    <span class="xin-xi-biao-qian">所属仓库</span>
                    <asp:DropDownList ID="XiaLaSuoShuCangKu" runat="server" 
                        CssClass="xia-la-xuan-ze">
                        <asp:ListItem Text="--请选择仓库--" Value=""></asp:ListItem>
                    </asp:DropDownList>
                    <span class="tishi" id="cang-ku-tishi"></span>
                </div>
                
                <!-- 资产数量输入框 -->
                <div class="xin-xi-xiang-mu">
                    <span class="xin-xi-biao-qian">资产数量</span>
                    <asp:TextBox ID="WenBenZiChanShuLiang" runat="server" 
                        CssClass="bian-ji-kuang" placeholder="请输入资产数量（正整数）"></asp:TextBox>
                    <span class="tishi" id="zi-chan-shu-liang-tishi"></span>
                </div>
                
                <!-- 资产价值输入框 -->
                <div class="xin-xi-xiang-mu">
                    <span class="xin-xi-biao-qian">资产价值</span>
                    <asp:TextBox ID="WenBenZiChanJiaZhi" runat="server" 
                        CssClass="bian-ji-kuang" placeholder="请输入资产价值（正数）"></asp:TextBox>
                    <span class="tishi" id="zi-chan-jia-zhi-tishi"></span>
                </div>
                
                <!-- 出厂日期输入框 -->
                <div class="xin-xi-xiang-mu">
                    <span class="xin-xi-biao-qian">出厂日期</span>
                    <asp:TextBox ID="WenBenChuChangRiQi" runat="server" 
                        CssClass="bian-ji-kuang" placeholder="请选择出厂日期" TextMode="Date"></asp:TextBox>
                    <span class="tishi" id="chu-chang-ri-qi-tishi"></span>
                </div>
            </div>
        </div>
        
        <!-- 操作按钮 -->
        <div class="an-niu-zu">
            <asp:Button ID="AnNiuBaoCun" runat="server" Text="保存资产" 
                CssClass="an-niu-cheng-gong" OnClick="AnNiuBaoCun_Click" />
            <asp:Button ID="AnNiuQuXiao" runat="server" Text="取消" 
                CssClass="an-niu-ya-se" OnClick="btnFanHui_Click" />
        </div>
        
        <!-- 返回链接 -->
        <div style="text-align: center; margin-top: 20px;">
            <a href="/GuDinZiChan/GuDinZiChanChaoZuo/ZiChanZhuTi.aspx" class="fan-hui-lian-jie">← 返回资产列表</a>
        </div>
    </div>
    
    <script type="text/javascript">
        $(document).ready(function () {
            // 为保存按钮绑定验证事件
            $('#<%= AnNiuBaoCun.ClientID %>').on('click', function (e) {
                // 获取输入值
                var ziChanMingCheng = $('#<%= WenBenZiChanMingCheng.ClientID %>').val().trim();
                var suoShuCangKu = $('#<%= XiaLaSuoShuCangKu.ClientID %>').val();
                var ziChanShuLiang = $('#<%= WenBenZiChanShuLiang.ClientID %>').val().trim();
                var ziChanJiaZhi = $('#<%= WenBenZiChanJiaZhi.ClientID %>').val().trim();
                var chuChangRiQi = $('#<%= WenBenChuChangRiQi.ClientID %>').val().trim();
                
                // 初始化验证状态
                var isValid = true;
                
                // 清除错误提示
                $('#zi-chan-ming-cheng-tishi').text("");
                $('#cang-ku-tishi').text("");
                $('#zi-chan-shu-liang-tishi').text("");
                $('#zi-chan-jia-zhi-tishi').text("");
                $('#chu-chang-ri-qi-tishi').text("");
                $('.bian-ji-kuang, .xia-la-xuan-ze').removeClass("error");
                
                // 验证资产名称
                if (ziChanMingCheng === '') {
                    $('#zi-chan-ming-cheng-tishi').text("资产名称不能为空");
                    $('#<%= WenBenZiChanMingCheng.ClientID %>').addClass("error");
                    isValid = false;
                } else if (ziChanMingCheng.length > 50) {
                    $('#zi-chan-ming-cheng-tishi').text("资产名称不能超过50个字符");
                    $('#<%= WenBenZiChanMingCheng.ClientID %>').addClass("error");
                    isValid = false;
                }
                
                // 验证所属仓库
                if (suoShuCangKu === '') {
                    $('#cang-ku-tishi').text("请选择所属仓库");
                    $('#<%= XiaLaSuoShuCangKu.ClientID %>').addClass("error");
                    isValid = false;
                }
                
                // 验证资产数量
                if (ziChanShuLiang === '') {
                    $('#zi-chan-shu-liang-tishi').text("资产数量不能为空");
                    $('#<%= WenBenZiChanShuLiang.ClientID %>').addClass("error");
                    isValid = false;
                } else if (!/^\d+$/.test(ziChanShuLiang)) {
                    $('#zi-chan-shu-liang-tishi').text("资产数量必须为正整数");
                    $('#<%= WenBenZiChanShuLiang.ClientID %>').addClass("error");
                    isValid = false;
                } else if (parseInt(ziChanShuLiang) <= 0) {
                    $('#zi-chan-shu-liang-tishi').text("资产数量必须大于0");
                    $('#<%= WenBenZiChanShuLiang.ClientID %>').addClass("error");
                    isValid = false;
                }
                
                // 验证资产价值
                if (ziChanJiaZhi === '') {
                    $('#zi-chan-jia-zhi-tishi').text("资产价值不能为空");
                    $('#<%= WenBenZiChanJiaZhi.ClientID %>').addClass("error");
                    isValid = false;
                } else if (!/^\d+(\.\d+)?$/.test(ziChanJiaZhi)) {
                    $('#zi-chan-jia-zhi-tishi').text("资产价值必须为正数");
                    $('#<%= WenBenZiChanJiaZhi.ClientID %>').addClass("error");
                    isValid = false;
                } else if (parseFloat(ziChanJiaZhi) <= 0) {
                    $('#zi-chan-jia-zhi-tishi').text("资产价值必须大于0");
                    $('#<%= WenBenZiChanJiaZhi.ClientID %>').addClass("error");
                    isValid = false;
                }
                
                // 验证出厂日期
                if (chuChangRiQi === '') {
                    $('#chu-chang-ri-qi-tishi').text("出厂日期不能为空");
                    $('#<%= WenBenChuChangRiQi.ClientID %>').addClass("error");
                    isValid = false;
                } else {
                    var selectedDate = new Date(chuChangRiQi);
                    var today = new Date();
                    if (selectedDate > today) {
                        $('#chu-chang-ri-qi-tishi').text("出厂日期不能超过今天");
                        $('#<%= WenBenChuChangRiQi.ClientID %>').addClass("error");
                        isValid = false;
                    }
                }
                
                // 如果验证失败，阻止表单提交
                if (!isValid) {
                    e.preventDefault();
                    return false;
                }
                
                // 验证通过，允许提交
                console.log("资产信息验证通过，提交表单到后台");
            });
            
            // 实时验证：当用户输入时清除错误提示
            $('#<%= WenBenZiChanMingCheng.ClientID %>, #<%= WenBenZiChanShuLiang.ClientID %>, #<%= WenBenZiChanJiaZhi.ClientID %>, #<%= WenBenChuChangRiQi.ClientID %>').on('input', function () {
                var $this = $(this);
                $this.removeClass('error');
                
                if ($this.attr('id') === '<%= WenBenZiChanMingCheng.ClientID %>') {
                    $('#zi-chan-ming-cheng-tishi').text("");
                } else if ($this.attr('id') === '<%= WenBenZiChanShuLiang.ClientID %>') {
                    $('#zi-chan-shu-liang-tishi').text("");
                } else if ($this.attr('id') === '<%= WenBenZiChanJiaZhi.ClientID %>') {
                    $('#zi-chan-jia-zhi-tishi').text("");
                } else if ($this.attr('id') === '<%= WenBenChuChangRiQi.ClientID %>') {
                    $('#chu-chang-ri-qi-tishi').text("");
                }
            });
            
            // 当下拉框改变时清除错误提示
            $('#<%= XiaLaSuoShuCangKu.ClientID %>').on('change', function () {
                $(this).removeClass('error');
                $('#cang-ku-tishi').text("");
            });
        });
    </script>
</asp:Content>
