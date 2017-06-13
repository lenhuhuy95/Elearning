var section = {
    init: function () {
        section.registerEvents();
    },
    registerEvents: function () {
        $('.btn-danger').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);
            var id = btn.data('idsection2'); //id lấy từ view ListSection
            $.ajax({
                url: "/Admin/Course/DeleteSection", //link trỏ đến Course controller
                data: { id: id },
                dataType: "json",
                type: "POST",
                success: function (response) {
                    console.log(response);
                    if (response.status == true) {
                        btn.text('Đã xóa Chương');
                    }

                }
            });
        });
    }
}
section.init();