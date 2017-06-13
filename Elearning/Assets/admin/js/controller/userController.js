var user = {
    init: function () {
        user.registerEvents();
    },
    registerEvents: function () {
        $('.btn-active').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);
            var id = btn.data('id1'); //id lấy từ view ListUser
            $.ajax({
                url: "/Admin/User/ChangeStatus", //link trỏ đến controller
                data: { id: id },
                dataType: "json",
                type:"POST",
                success: function (response) {
                    console.log(response);
                    if(response.status == true)
                    {                   
                        btn.text('Kích hoạt');
                        //document.getElementById("btnTest").className = "btn btn-block btn-success btn-flat";
                    }
                    else
                    {                      
                        btn.text('Khóa');
                        //document.getElementById("btnTest").className = "btn btn-block btn-danger btn-flat";
                    }
                }
            });
        });
    }
}
user.init();