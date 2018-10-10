var selectedFromDate;
var selectedToday;
var singleObj = {}
var primaryresult = [];
var nonPrimaryresult = [];
function onManualReconsile() {
    var e = document.getElementById("ddlGroupTemplate");
    var group_Id = e.options[e.selectedIndex].value;
    Array.prototype.push.apply(nonPrimaryresult, primaryresult);
    $.ajax({
        async: true,
        cache: true,
        type: "POST",
        data: {
            selectedResult: nonPrimaryresult,
            groupId: group_Id,
            FromDate: selectedFromDate,
            ToDate: selectedToday
        },
        url: '/ManualReconsile/ManualReconsile',
        success: function (data) {
            $("#_ReconsileReportpartial").html(data);
        }
    });
}
function onNonReconsileSearchClick() {
    var e = document.getElementById("ddlGroupTemplate");
    var group_Id = e.options[e.selectedIndex].value;
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
    primaryresult = []
    $('input.chkClass').on('change', function () {
        $('input.chkClass').not(this).prop('checked', false);
    });
    var tableControl = document.getElementById('nonReconsilePrimaryData');
    singleObj = {};
    $('input:checkbox:checked', tableControl).each(function () {
        singleObj['Id'] = $(this).closest('tr').find('td:last').text();
        singleObj['Type'] = 'P';
    });
    primaryresult.push(singleObj);
}
function onNonPrimaryCheckBox() {
    nonPrimaryresult = [];
    nonPrimaryAmount = 0;
    var tableControl = document.getElementById('nonReconsileNonPrimaryTable');
    $('input:checkbox:checked', tableControl).each(function () {
        singleObj = {};
        singleObj['Id'] = $(this).closest('tr').find('td:last').text();
        singleObj['Type'] = 'NP';
        nonPrimaryresult.push(singleObj);
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
