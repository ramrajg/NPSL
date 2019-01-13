$(document).ready(function () {
    var oTable = $('#reconsileDatatable').DataTable();
    $('#reconsileDatatable').on('click', '.btn-bootstrap-dialog', function () {
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
        $('#btnHeader-label').html("Delete Template Id: " + "<b>" + row[0] + "</b>");
        $('#pMsg').html("Are you sure you want to delete Tempate Name : " + "<b>" + row[1] + "</b>");
        //console.log(row);
    });

    $('#reconsileDatatable').on('click','.delete-template1',function () {
        $('#delModel').modal({
            backdrop: 'static',
            keyboard: false
        })
            .on('click', '#confirmOk', function (e) {
                var pos = oTable.row('.selected').index();
                var row = oTable.row(pos).data();
                var templateId = parseInt(row[0]);
                var data = { Id: templateId }
                apiGetCallController('ReconsileTemplateMaster', 'DeleteTemplate', 'POST', data, function () {
                    window.location.reload();
                    $('#delModel ').modal('hide');
                    //$.notify("Deleted Sucessfully", 'danger');
                }, function (responseText) {
                    $.notify(responseText, 'danger');
                    return false;
                });
            });

    });
    $(document).unbind('click.addParameters').on('click.addParameters', '#addButton', function (ev) {
        if (($('.form-horizontal .form-group').length + 1) > 10) {

            alert("Only 10 Parameters are allowed");
            return false;
        }
        var id = ($('.form-horizontal .form-group').length + 1).toString();
        $('.form-horizontal').append('<div class="form-group" id="form-group-' + id + '"><div class= "row" ><div class="col-xs-offset-1 col-xs-5"><span class="input-group-addon">Parameter ' + id + ': </span></div><div class="col-xs-offset-0 col-xs-5"><div class="input-group"><input id="Parameter' + id + '" type="text" class="form-control" placeholder="Parameter ' + id + '"><span class="input-group-btn"><button id ="removeParameter' + id + '" class="btn btn-danger remove-me" type="button">X</button></span></div></div></div></div>');

    });


    $(document).unbind('click.removeParameters').on('click.removeParameters', '.remove-me', function (e) {
        e.preventDefault();
        var fieldNum = this.id.charAt(this.id.length - 1);
        var fieldID = "#form-group-" + fieldNum;
        $(this).remove();
        $(fieldID).remove();
    });


    $('#TemplateAddForm').submit(function (event) {
        var i;
        var parameterValue = "";
        for (i = 0; i < $('.form-horizontal .form-group').length; i++) {
            var fieldID = "#Parameter" + $('.form-horizontal .form-group')[i].id.split('-')[2];
            if (i == 0) {
                parameterValue = $(fieldID).val();
            }
            else {
                parameterValue = parameterValue + "|" + $(fieldID).val();
            }
        }
        document.getElementById('SourceSubstringValue').value = parameterValue;
        document.getElementById('NumberOfParameters').value = $('.form-horizontal .form-group').length;
    });

    $('#templateEditForm').submit(function (event) {

        var i;
        var parameterValue = "";
        for (i = 0; i < $('.form-horizontal .form-group').length; i++) {
            var fieldID = "#Parameter" + $('.form-horizontal .form-group')[i].id.split('-')[2];
            if (i == 0) {
                parameterValue = $(fieldID).val();
            }
            else {
                parameterValue = parameterValue + "|" + $(fieldID).val();
            }
        }
        document.getElementById('SourceSubstringValue').value = parameterValue;
        document.getElementById('NumberOfParameters').value = $('.form-horizontal .form-group').length;

        var data = $(templateEditForm).serialize();
        apiGetCallController('ReconsileTemplateMaster', 'EditTemplateApiCall', 'POST', data, function () {
            window.location.href = '/ReconsileTemplateMaster/Index';
        }, function (responseText) {
            $.notify(responseText, 'danger');
            return false;
        });
        return false;


    });

});