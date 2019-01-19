$(document).ready(function () {

    var table = $('#fileExtension').DataTable({
        drawCallback: function (oSettings) {
            $('.delete-fileExtension').prop('disabled', true);
        }
    });

    var oTable = $('#fileExtension').DataTable();

    $('#fileExtension').on('click', '.btn-bootstrap-dialog', function () {
        var url = $(this).data('url');
        var title = $(this).attr('title');
        $.get(url, function (data) {
            $('#bootstrapDialog').html(data);
            $('#bootstrapDialog').modal('show');
            $('#ModalPopUp').find('#myModalLabel').html($(this).attr("title"));
        });
    });
    $('.addFileExtensionbtn').click(function () {
        var url = $(this).data('url');
        var title = $(this).attr('title');
        $.get(url, function (data) {
            $('#bootstrapDialog').html(data);
            $('#bootstrapDialog').modal('show');
            $('#ModalPopUp').find('#myModalLabel').html($(this).attr("title"));
        });
    });
    $('#fileExtension tbody').on('click', 'tr', function () {
        $(this).toggleClass('selected');
        var pos = oTable.row(this).index();
        var row = oTable.row(pos).data();
        $('#btnHeader-label').html("Delete File Extension Id: " + "<b>" + row[0] + "</b>");
        $('#pMsg').html("Are you sure you want to delete File Extension Name : " + "<b>" + row[1] + "</b>");
        //console.log(row);
    });

    $('#fileExtension').on('click', '.delete-fileExtension', function () {
        $('#delModel').modal({
            backdrop: 'static',
            keyboard: false
        })
            .on('click', '#confirmOk', function (e) {
                var pos = oTable.row('.selected').index();
                var row = oTable.row(pos).data();
                var templateGroupId = parseInt(row[0]);
                var data = { Id: templateGroupId }
                apiGetCallController('FileExtension', 'DeleteFileExtension', 'POST', data, function (sucessfullText) {
                    $('#delModel ').modal('hide');
                    window.location.reload();
                }, function (responseText) {
                    $('#delModel ').modal('hide');
                    $.notify(responseText, 'danger');
                    return false;
                });
            });

    });

});
window.onload = function () {
    $('.delete-fileExtension').prop('disabled', true);
}