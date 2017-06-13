var exam = {
    init: function () {
        exam.registerEvents();
    },
    registerEvents: function () {
        $('.btn-danger').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);
            var id = btn.data('idexam1'); //id lấy từ view 
            $.ajax({
                url: "/Admin/Course/DeleteExam", //link trỏ đến Course controller
                data: { id: id },
                dataType: "json",
                type: "POST",
                success: function (response) {
                    console.log(response);
                    if (response.status == true) {
                        btn.text('Đã xóa bài thi');
                    }

                }
            });
        });
    }
}
exam.init();