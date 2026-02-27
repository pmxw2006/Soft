// 全局变量
var currentDeleteBtn = null;

// 显示删除模态框
function showDeleteModal(button) {
    // 保存当前按钮
    currentDeleteBtn = button;

    // 获取用户信息
    var zhanghao = $(button).data('zhanghao');
    var xingming = $(button).data('xingming');
    var bumen = $(button).data('bumen');

    // 填充用户信息
    $('#ContentPlaceHolder1_shanChuZhangHao2').text(zhanghao);
    $("#tckHiddenZhangHaoInfo").val(zhanghao);
    $('#shanChuXingMing2').text(xingming);
    $('#shanChuBuMen2').text(bumen);

    // 重置复选框和按钮
    $('#confirmCheck2').prop('checked', false);
    $('#btnConfirmDelete2').prop('disabled', true);

    // 显示模态框
    $('#shanChuMoTai').addClass('xian-shi');
    $('body').css('overflow', 'hidden');

    return false;
}

// 关闭模态框
function closeModal() {
    // 隐藏模态框
    $('#shanChuMoTai').removeClass('xian-shi');
    $('body').css('overflow', 'auto');

    // 重置状态
    currentDeleteBtn = null;
    $('#confirmCheck2').prop('checked', false);
}

// 切换确认按钮状态
function toggleConfirmButton() {
    var isChecked = $('#confirmCheck2').is(':checked');
    $('#ContentPlaceHolder1_btnConfirmDelete2').prop('disabled', !isChecked);
}

// 确认删除
function confirmDelete() {
    if (!currentDeleteBtn) {
        console.error('没有找到要删除的按钮');
        return;
    }

    // 关闭模态框
    closeModal();

    // 延迟执行，确保模态框动画完成
    setTimeout(function () {
        // 获取命令参数
        var commandArgument = $(currentDeleteBtn).attr('CommandArgument');
        if (!commandArgument) {
            commandArgument = $(currentDeleteBtn).data('zhanghao');
        }

        // 触发服务器端删除
        if (typeof __doPostBack !== 'undefined') {
            __doPostBack(currentDeleteBtn.name, commandArgument);
        } else {
            // 如果__doPostBack不存在，模拟点击
            $(currentDeleteBtn).removeAttr('OnClientClick');
            $(currentDeleteBtn).click();
        }
    }, 300);
}

// 页面加载完成后初始化
$(document).ready(function () {
    console.log('页面加载完成，初始化模态框功能');

    // 为删除按钮绑定点击事件
    $(document).on('click', '.btn-shanchu', function (e) {
        showDeleteModal(this);
    });

    // 点击模态框背景关闭
    $('#shanChuMoTai').click(function (e) {
        if (e.target === this) {
            closeModal();
        }
    });

    // ESC键关闭模态框
    $(document).keydown(function (e) {
        if (e.keyCode === 27) { // ESC键
            if ($('#shanChuMoTai').hasClass('xian-shi')) {
                closeModal();
            }
        }
    });

    // 防止事件冒泡
    $('.mo-tai-nei-rong').click(function (e) {
        e.stopPropagation();
    });
});