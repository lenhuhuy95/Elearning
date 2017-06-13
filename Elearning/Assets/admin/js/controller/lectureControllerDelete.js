var lecture = {
    init: function () {
        lecture.registerEvents();
    },
    registerEvents: function () {
        $('.btn-danger').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);
            var id = btn.data('idlecture2'); //id lấy từ view ListLecture
            $.ajax({
                url: "/Admin/Course/DeleteLecture", //link trỏ đến Course controller
                data: { id: id },
                dataType: "json",
                type: "POST",
                success: function (response) {
                    console.log(response);
                    if (response.status == true) {
                        btn.text('Đã xóa bài học');
                    }

                }
            });
        });
    }
}
lecture.init();