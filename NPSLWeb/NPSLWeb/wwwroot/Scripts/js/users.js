
$(document).ready(function () {
    var oTable = $('#userDatatable').DataTable();
    $('.btn-bootstrap-dialog').click(function () {
        var url = $(this).data('url');
        var title = $(this).attr('title');
        $.get(url, function (data) {
            $('#bootstrapDialog').html(data);
            $('#bootstrapDialog').modal('show');
            $('#ModalPopUp').find('#myModalLabel').html($(this).attr("title"));
        });
    });
    $('#userDatatable tbody').on('click', 'tr', function () {
        $(this).toggleClass('selected');
        var pos = oTable.row(this).index();
        var row = oTable.row(pos).data();
        $('#btnHeader-label').html("Delete User Id: " + "<b>" + row[0] + "</b>");
        $('#pMsg').html("Are you sure you want to delete user : " + "<b>" + row[1] + " " + row[2] + "</b>");
        //console.log(row);
    });

    $('.btn-danger').click(function () {
        $('#myModal1').modal({
            backdrop: 'static',
            keyboard: false
        })
            .on('click', '#confirmOk', function (e) {
                var userId = parseInt(oTable.rows('.selected').data()[0][0]);
                var data = {Id: userId }
                apiGetCallController('Users', 'DeleteUser', 'POST', data, function () {
                    window.location.reload();
                    $('#myModal ').modal('hide');
                    //$.notify("Deleted Sucessfully", 'danger');
                }, function (responseText) {
                    $.notify(responseText, 'danger');
                    return false;
                });
            });

    });
});
