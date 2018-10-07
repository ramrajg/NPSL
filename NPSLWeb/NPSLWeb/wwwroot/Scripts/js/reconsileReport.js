var selectedFromDate;
var selectedToday;
var singleObj = {}
var nonPrimaryresult = [];
var primaryresult = [];
var primaryAmount;
var nonPrimaryAmount;
function onSearchClick() {
    var e = document.getElementById("ddlGroupTemplate");
    var group_Id = e.options[e.selectedIndex].value;
    var data_D = {
        groupId: group_Id,
        FromDate: selectedFromDate,
        ToDate: selectedToday
    }
    $.ajax({
        async: true,
        cache: true,
        type: "POST",
        data: {
            groupId: group_Id,
            FromDate: selectedFromDate,
            ToDate: selectedToday
        },
        url: '/ReconsileReport/RefreshSearchResult',
        success: function (data) {
            $("#_ReconsileReportpartial").html(data);
        }
    });
}
function onNonReconsileSearchClick() {
    var e = document.getElementById("ddlGroupTemplate");
    var group_Id = e.options[e.selectedIndex].value;
    var data_D = {
        groupId: group_Id,
        FromDate: selectedFromDate,
        ToDate: selectedToday
    }
    $.ajax({
        async: true,
        cache: true,
        type: "POST",
        data: {
            groupId: group_Id,
            FromDate: selectedFromDate,
            ToDate: selectedToday
        },
        url: '/ManualReconsile/RefreshSearchResult',
        success: function (data) {
            $("#_ReconsileReportpartial").html(data);
        }
    });
}
function onPrimaryCheckBox() {
    primaryAmount = 0;
    primaryresult = []
    $('input.chkClass').on('change', function () {
        $('input.chkClass').not(this).prop('checked', false);
    });
    var tableControl = document.getElementById('nonReconsilePrimaryData');
    $('input:checkbox:checked', tableControl).each(function () {
        primaryAmount = $(this).parent().parent().find('td:eq(2)').text();
    });
    if (primaryAmount > 0 && nonPrimaryAmount > primaryAmount) {
        primaryAmount = 0;
        alert('Non Primary Value is Greater than Primary Value selected');
        $('input.chkClass').not(this).prop('checked', false);
    } else {
        singleObj = {};
        singleObj['amount'] = $(this).parent().parent().find('td:eq(2)').text();
        singleObj['id'] = $(this).closest('tr').find('td:last').text();
        singleObj['Type'] = 'P';
        primaryresult.push(singleObj);
    }
}
function onNonPrimaryCheckBox() {
    nonPrimaryresult = [];
    nonPrimaryAmount = 0;
    var tableControl = document.getElementById('#nonReconsileNonPrimaryTable');
    $('input:checkbox:checked', tableControl).each(function () {
        singleObj = {};
        singleObj['amount'] = $(this).parent().parent().find('td:eq(2)').text();
        singleObj['id'] = $(this).closest('tr').find('td:last').text();
        singleObj['Type'] = 'NP';
        if (nonPrimaryAmount > 0 && primaryAmount > 0 && (Number(nonPrimaryAmount) + Number($(this).parent().parent().find('td:eq(2)').text()) > primaryAmount)) {
            alert('Non Primary Value is Greater than Primary Value selected');
            $(this).prop('checked', false);
        } else {
            nonPrimaryAmount = Number(nonPrimaryAmount) + Number($(this).parent().parent().find('td:eq(2)').text());
            nonPrimaryresult.push(singleObj);
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
    $("#datetimepickerFromDate").datepicker().datepicker("setDate", new Date());
    $("#datetimepickerToDate").datepicker().datepicker("setDate", new Date());
}
