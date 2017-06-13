var category = {
    init: function () {
        category.registerEvents();
    },
    registerEvents: function () {
        $('.btn-active').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);
            var id = btn.data('idcategory'); //id lấy từ view ListCategory
            $.ajax({
                url: "/Admin/Category/ChangeStatusCategory", //link trỏ đến controller
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
category.init();