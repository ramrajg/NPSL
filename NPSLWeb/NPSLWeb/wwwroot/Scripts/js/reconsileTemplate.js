$(document).ready(function () {
    var oTable = $('#reconsileDatatable').DataTable();
    $('.btn-bootstrap-dialog').click(function () {
        var url = $(this).data('url');
        var title = $(this).attr('title');
        $.get(url, function (data) {
            $('#bootstrapDialog').html(data);
            $('#bootstrapDialog').modal('show');
            $('#ModalPopUp').find('#myModalLabel').html($(this).attr("title"));
        });
    });
    $('#reconsileDatatable tbody').on('click', 'tr', function () {
        $(this).toggleClass('selected');
        var pos = oTable.row(this).index();
        var row = oTable.row(pos).data();
        $('#btnHeader-label').html("Delete User Id: " + "<b>" + row[0] + "</b>");
        $('#pMsg').html("Are you sure you want to delete user : " + "<b>" + row[1] + " " + row[2] + "</b>");
        //console.log(row);
    });

    $('.btn-danger').click(function () {
        $('#delModel').modal({
            backdrop: 'static',
            keyboard: false
        })
            .on('click', '#confirmOk', function (e) {
                var userId = parseInt(oTable.rows('.selected').data()[0][0]);
                var data = { Id: userId }
                apiGetCallController('Users', 'DeleteUser', 'POST', data, function () {
                    window.location.reload();
                    $('#delModel ').modal('hide');
                    //$.notify("Deleted Sucessfully", 'danger');
                }, function (responseText) {
                    $.notify(responseText, 'danger');
                    return false;
                });
            });

    });
    //$(document).on('click', '#addButton', function (ev) {
    //    if (($('.form-horizontal .form-group').length + 1) > 10) {

    //        alert("Only 10 Parameters are allowed");
    //        return false;
    //    }
    //    $('#addButton').unbind('click');
    //    var id = ($('.form-horizontal .form-group').length + 1).toString();
    //    $('.form-horizontal').append('<div class="form-group" id="form-group' + id + '"><div class= "row" ><div class="col-xs-offset-1 col-xs-5"><span class="input-group-addon">Parameter ' + id + ': </span></div><div class="col-xs-offset-0 col-xs-5"><div class="input-group"><input id="Parameter' + id + '" type="text" class="form-control" placeholder="Parameter ' + id + '"><span class="input-group-btn"><button class="btn btn-info" type="button">X</button></span></div></div></div></div>');
    //});
    $(document).unbind('click.addParameters').on('click.addParameters', '#addButton', function (ev) {
        if (($('.form-horizontal .form-group').length + 1) > 10) {
     
        alert("Only 10 Parameters are allowed");
            return false;
        }
        var id = ($('.form-horizontal .form-group').length + 1).toString();
        $('.form-horizontal').append('<div class="form-group" id="form-group' + id + '"><div class= "row" ><div class="col-xs-offset-1 col-xs-5"><span class="input-group-addon">Parameter ' + id + ': </span></div><div class="col-xs-offset-0 col-xs-5"><div class="input-group"><input id="Parameter' + id + '" type="text" class="form-control" placeholder="Parameter ' + id + '"><span class="input-group-btn"><button class="btn btn-info" type="button">X</button></span></div></div></div></div>');
       
    });


    //$('#addButton').click(function () {
    //    if ($('.form-horizontal .control-group').length == 1) {
    //        alert("No more textbox to remove");
    //        return false;
    //    }

    //    $(".form-horizontal .control-group:last").remove();
    //});

});