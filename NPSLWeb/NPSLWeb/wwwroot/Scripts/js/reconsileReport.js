var selectedFromDate;
var selectedToday;
function onSearchClick() {
    var $loading = $('.se-pre-con').hide();
    $loading.show();
    var e = document.getElementById("ddlGroupTemplate");
    var group_Id = e.options[e.selectedIndex].value;
    var e = document.getElementById("ddltemplate");
    var template_Id = e.options[e.selectedIndex].value;
    var t = document.getElementById("ddlReconsileType");
    var reconsile_Type = t.options[t.selectedIndex].value;
    $.ajax({
        async: true,
        cache: true,
        type: "POST",
        data: {
            groupId: group_Id,
            selectedTemplateId: template_Id,
            reconsileType: reconsile_Type,
            FromDate: selectedFromDate,
            ToDate: selectedToday
        },
        url: '/ReconsileReport/RefreshSearchResult',
        success: function (data) {
            $("#_ReconsileReportpartial").html(data);
            $("#exportDiv").show();
            $(".se-pre-con").fadeOut("slow");
        },
        failure: function (response) {
            alert(response.responseText);
            $(".se-pre-con").fadeOut("slow");
        },
        error: function (response) {
            alert(response.responseText);
            $(".se-pre-con").fadeOut("slow");
        }
    });
}

$(document).ready(function () {
    $('#datepicker').datepicker();
    $(function () {
        $('#datetimepickerFromDate').datepicker();

        $('#datetimepickerToDate').datepicker({
            useCurrent: false //Important! See issue #1075
        });

        $("#datetimepickerFromDate").on("dp.change", function (e) {
            $('#datetimepickerToDate').data("DatePicker").minDate(e.date);
            var selected = $(this).val();
            alert(selected);
        });

        $("#datetimepickerToDate").on("dp.change", function (e) {
            $('#datetimepickerFromDate').data("DatePicker").maxDate(e.date);
            var selected = $(this).val();
            alert(selected);
        });
    });
    $('#ddlGroupTemplate').on('change', function () {
        var data = "";
        $.ajax({
            type: "GET",
            url: '/ReconsileReport/GetTemplateByGroupId',
            data: "groupId=" + $(this).val(),
            async: false,
            success: function (data) {
                $('#ddltemplate').empty(); // clear the current elements in select box
                $("#ddltemplate").html("<option value=''>--Select One--</option>");
                for (row in data) {
                    $('#ddltemplate').append($('<option></option>').attr('value', data[row].templateId).text(data[row].templateName));
                }
            },
            error: function () {
                alert('Error occured');
            }
        });

    });
    $('#ddltemplate').on('change', function () {
        var val = $(this).val();
        if (val != "" || val != "0") {
            $('#reconsileSearchbtn').prop('disabled', false);
        }
    });
});

$(function () {
    $(function () {
        $('#datetimepickerFromDate').datepicker({
            clearBtn: true,
            todayHighlight: false,
            multidate: true,
            dateFormat: "yy-mm-dd",

        }).on('changeDate', function (e) {
            selectedFromDate = e.date.toDateString()
        });
    });
    $(function () {
        $('#datetimepickerToDate').datepicker({
            clearBtn: true,
            todayHighlight: false,
            multidate: true,
            dateFormat: "yy-mm-dd",

        }).on('changeDate', function (e) {
            selectedToday = e.date.toDateString()
        });
    });
});
window.onload = function () {
    $('#reconsileSearchbtn').prop('disabled', true);
    $("#datetimepickerFromDate").datepicker().datepicker("setDate", new Date());
    $("#datetimepickerToDate").datepicker().datepicker("setDate", new Date());
}
