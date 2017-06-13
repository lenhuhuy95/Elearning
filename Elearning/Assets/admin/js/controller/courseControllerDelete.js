var course = {
    init: function () {
        course.registerEvents();
    },
    registerEvents: function () {
        $('.btn-danger').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);
            var id = btn.data('idcourse2'); //id lấy từ view ListCourses
            $.ajax({
                url: "/Admin/Course/DeleteCourse", //link trỏ đến Course controller
                data: { id: id },
                dataType: "json",
                type: "POST",
                success: function (response) {
                    console.log(response);
                    if (response.status == true) {
                        btn.text('Đã xóa khóa học');    
                    }

                }
            });
        });
    }
}
course.init();