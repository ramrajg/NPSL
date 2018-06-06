
$(document).ready(function () {
    $('.btn-bootstrap-dialog').click(function () {
        var url = $(this).data('url');
        var title = $(this).attr('title');
        $.get(url, function (data) {
            $('#bootstrapDialog').html(data);
            $('#bootstrapDialog').modal('show');
            $('#ModalPopUp').find('#myModalLabel').html($(this).attr("title"));
        });
    });

    $('.btn-danger').click(function () {
        var std = $(this).attr('id');
        $('#myModal1').modal({
            backdrop: 'static',
            keyboard: false
        })

            .on('click', '#confirmOk', function (e) {
                $.ajax({
                    type: "POST",
                    url: '@Url.Action("DeleteEmployee")',
                    data: '{Id: ' + JSON.stringify(std) + '}',
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (success) {
                        window.location.reload();
                        $('#myModal ').modal('hide');
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert('oops, something bad happened')
                    }
                });
            });

    });
});