$(document).ready(function () {
    var oTable = $('#templateGroupData').DataTable();
    $('.btn-bootstrap-dialog').click(function () {
        var url = $(this).data('url');
        var title = $(this).attr('title');
        $.get(url, function (data) {
            $('#bootstrapDialog').html(data);
            $('#bootstrapDialog').modal('show');
            $('#ModalPopUp').find('#myModalLabel').html($(this).attr("title"));
        });
    });
    $('#templateGroupData tbody').on('click', 'tr', function () {
        $(this).toggleClass('selected');
        var pos = oTable.row(this).index();
        var row = oTable.row(pos).data();
        $('#btnHeader-label').html("Delete Template Group Id: " + "<b>" + row[0] + "</b>");
        $('#pMsg').html("Are you sure you want to delete Tempate Group Name : " + "<b>" + row[1] + "</b>");
    });

    $('.delete-templateGroup').click(function () {
        $('#delModel').modal({
            backdrop: 'static',
            keyboard: false
        })
            .on('click', '#confirmOk', function (e) {
                var pos = oTable.row('.selected').index();
                var row = oTable.row(pos).data();
                var templateGroupId = parseInt(row[0]);
                var data = { Id: templateGroupId }
                apiGetCallController('TemplateGroup', 'DeleteTemplate', 'POST', data, function (sucessfullText) {
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