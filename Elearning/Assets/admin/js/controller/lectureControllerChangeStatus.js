var lecture = {
    init: function () {
        lecture.registerEvents();
    },
    registerEvents: function () {
        $('.btn-active').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);
            var id = btn.data('idlecture1'); //id lấy từ view ListLecture
            $.ajax({
                url: "/Admin/Course/ChangeStatusLecture", //link trỏ đến controller
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
lecture.init();