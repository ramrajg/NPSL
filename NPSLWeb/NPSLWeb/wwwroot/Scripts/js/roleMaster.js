$(document).ready(function () {
    var oTable = $('#roleData').DataTable();
    $('.btn-bootstrap-dialog').click(function () {
        var url = $(this).data('url');
        var title = $(this).attr('title');
        $.get(url, function (data) {
            $('#bootstrapDialog').html(data);
            $('#bootstrapDialog').modal('show');
            $('#ModalPopUp').find('#myModalLabel').html($(this).attr("title"));
        });
    });
    $(function () {
        $('input[type="checkbox"]').change(checkboxChanged);
        function checkboxChanged() {
            var $this = $(this),
                checked = $this.prop("checked"),
                container = $this.parent(),
                siblings = container.siblings();
            container.find('input[type="checkbox"]')
                .prop({
                    indeterminate: false,
                    checked: checked
                })
                .siblings('label')
                .removeClass('custom-checked custom-unchecked custom-indeterminate')
                .addClass(checked ? 'custom-checked' : 'custom-unchecked');
            checkSiblings(container, checked);
        }

        function checkSiblings($el, checked) {
            var parent = $el.parent().parent(),
                all = true,
                indeterminate = false;
            $el.siblings().each(function () {
                return all = ($(this).children('input[type="checkbox"]').prop("checked") === checked);
            });
            if (all && checked) {
                parent.children('input[type="checkbox"]')
                    .prop({
                        indeterminate: false,
                        checked: checked
                    })
                    .siblings('label')
                    .removeClass('custom-checked custom-unchecked custom-indeterminate')
                    .addClass(checked ? 'custom-checked' : 'custom-unchecked');
                checkSiblings(parent, checked);
            }
            else if (all && !checked) {
                indeterminate = parent.find('input[type="checkbox"]:checked').length > 0;

                parent.children('input[type="checkbox"]')
                    .prop("checked", checked)
                    .prop("indeterminate", indeterminate)
                    .siblings('label')
                    .removeClass('custom-checked custom-unchecked custom-indeterminate')
                    .addClass(indeterminate ? 'custom-indeterminate' : (checked ? 'custom-checked' : 'custom-unchecked'));
                checkSiblings(parent, checked);
            }
            else {
                $el.parents("li").children('input[type="checkbox"]')
                    .prop({
                        indeterminate: true,
                        checked: false
                    })
                    .siblings('label')
                    .removeClass('custom-checked custom-unchecked custom-indeterminate')
                    .addClass('custom-indeterminate');
            }
        }



    });

    $('#roleEdit').submit(function (event) {
        var selectedMenuId = [];
        var result = [];
        singleObj = {};
        var RoleId = "";
        RoleId = document.getElementById('RoleId').value;
        $('input[type="checkbox"]').each(function () {
            if ($(this).is(':checked')) {
                singleObj = {};
                singleObj['SubmenuId'] = this.id;
                singleObj['RoleId'] = RoleId;
                selectedMenuId.push(singleObj);
            }
        });


        if (RoleName != "") {
            apiGetCallController('RoleMaster', 'EditrecordAPICall', 'POST', { selectedMenuId: selectedMenuId }, function () {
                window.location.href = '/RoleMaster/Index';
            }, function (responseText) {
                $.notify(responseText, 'danger');
                return false;
            });
        }

    });

    $('#roleAdd').submit(function (event) {
        var selectedMenuId = [];
        var result = [];
        singleObj = {};
        var RoleName = "";
        RoleName = document.getElementById('RoleName').value;
        $('input[type="checkbox"]').each(function () {
            if ($(this).is(':checked')) {
                singleObj = {};
                singleObj['SubmenuId'] = this.id;
                singleObj['RoleName'] = RoleName;
                selectedMenuId.push(singleObj);
            }
        });


        if (RoleName != "") {
            apiGetCallController('RoleMaster', 'AddrecordAPICall', 'POST', { selectedMenuId: selectedMenuId }, function () {
                window.location.href = '/RoleMaster/Index';
            }, function (responseText) {
                $.notify(responseText, 'danger');
                return false;
            });
        }

    });



});
