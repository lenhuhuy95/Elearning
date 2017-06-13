var money = {
    init: function () {
        money.registerEvents();
    },
    registerEvents: function () {
        $('.btn-active').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);
            var id = btn.data('idmoney'); //id lấy từ view
            $.ajax({
                url: "/Admin/Money/ChangeStatusMoney", //link trỏ đến controller
                data: { id: id },
                dataType: "json",
                type: "POST",
                success: function (response) {
                    console.log(response);
                    if (response.status == true) {
                        btn.text('Mở');
                    }
                    else {
                        btn.text('Đóng');
                    }
                }
            });
        });
    }
}
money.init();