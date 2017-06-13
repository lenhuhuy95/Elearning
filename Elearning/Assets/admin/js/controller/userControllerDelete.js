var user = {
    init: function () {
        user.registerEvents();
    },
    registerEvents: function () {
        $('.btn-danger').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);
            var id = btn.data('id2'); //id lấy từ view ListUser
            $.ajax({
                url: "/Admin/User/DeleteUser", //link trỏ đến controller
                data: { id: id },
                dataType: "json",
                type: "POST",
                success: function (response) {
                    console.log(response);
                    if (response.status == true) {
                        btn.text('Đã xóa tài khoản');
                    }

                }
            });
        });
    }
}
user.init();