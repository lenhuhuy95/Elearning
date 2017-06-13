var order = {
    init: function () {
        order.registerEvents();
    },
    registerEvents: function () {
        $('.btn-active').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);
            var id = btn.data('idorder'); //id lấy từ view ListOrder
            $.ajax({
                url: "/Admin/Order/ChangeStatusOrder", //link trỏ đến controller
                data: { id: id },
                dataType: "json",
                type: "POST",
                success: function (response) {
                    console.log(response);
                    if (response.status == true) {
                        btn.text('Hoàn tất');
                    }
                    else {
                        btn.text('Đang xử lý');
                    }
                }
            });
        });
    }
}
order.init();