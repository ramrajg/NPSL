﻿var selectedFromDate;
var selectedToday;
var singleObj = {}
var primaryresult = [];
var nonPrimaryresult = [];
var tableSplitValuePrimary = 0;
var tableSplitValueNonPrimary = 0;




function onManualReconsile() {
    singleObj = {};
    var $loading = $('.se-pre-con').hide();
    $loading.show();
    var e = document.getElementById("ddlGroupTemplate");
    var group_Id = e.options[e.selectedIndex].value;

    var e = document.getElementById("ddltemplate");
    var template_Id = e.options[e.selectedIndex].value;

    // var reasonDesc = $('#reasonTxt').val();
    singleObj['Id'] = 0;
    singleObj['Type'] = 'D';
    singleObj['ReasonDesc'] = $('#reasonTxt').val();
    primaryresult.push(singleObj);
    Array.prototype.push.apply(nonPrimaryresult, primaryresult);
    $.ajax({
        async: true,
        cache: true,
        type: "POST",
        data: {
            selectedResult: nonPrimaryresult,
            groupId: group_Id,
            selectedTemplateId: template_Id,
            FromDate: selectedFromDate,
            ToDate: selectedToday
        },
        url: '/ManualReconsile/ManualReconsile',
        success: function (data) {
            $("#_manualReconsileReportpartial").html(data);
            $(".se-pre-con").fadeOut("slow");;
            primaryresult = [];
            nonPrimaryresult = [];
            $('#reasonTxt').val('');
            SetButtonStatus();
        }
    });
}
function onNonReconsileSearchClick() {
    var $loading = $('.se-pre-con').hide();
    $loading.show();
    var e = document.getElementById("ddlGroupTemplate");
    var group_Id = e.options[e.selectedIndex].value;
    var e = document.getElementById("ddltemplate");
    var template_Id = e.options[e.selectedIndex].value;

    $.ajax({
        async: true,
        cache: true,
        type: "POST",
        data: {
            groupId: group_Id,
            selectedTemplateId: template_Id,
            FromDate: selectedFromDate,
            ToDate: selectedToday
        },
        url: '/ManualReconsile/RefreshSearchResult',
        success: function (data) {
            $("#_manualReconsileReportpartial").html(data);
            primaryresult = [];
            nonPrimaryresult = [];
            $('#reasonTxt').val('');
            $(".se-pre-con").fadeOut("slow");;
            SetButtonStatus();
        }
    });
}
function onPrimaryCheckBox(primaryChk) {
    primaryresult = []
    singleObj = {};
    var currentPrimaryId = 0;
    currentPrimaryId = parseInt($(primaryChk).closest('tr').find('td:last').text());
    $('input.chkClass').not(primaryChk).prop('checked', false);
    primaryresult = []
    if (tableSplitValuePrimary == 0) {
        tableSplitValuePrimary = parseInt(currentPrimaryId);
        $.each($("input[id='primaryCheckBox']:checked"), function () {
            singleObj['Id'] = $(this).val();
            singleObj['Type'] = 'P';
            singleObj['ReasonDesc'] = "";
        });
    }
    else if (tableSplitValuePrimary == parseInt(currentPrimaryId)) {
        tableSplitValuePrimary = parseInt(currentPrimaryId);
        $.each($("input[id='primaryCheckBox']:checked"), function () {
            singleObj['Id'] = $(this).val();
            singleObj['Type'] = 'P';
            singleObj['ReasonDesc'] = "";
        });
    }
    else {
        nonPrimaryresult = [];
        nonPrimaryAmount = 0;
        tableSplitValuePrimary = parseInt(currentPrimaryId);
        $.each($("input[id='primaryCheckBox']:checked"), function () {
            singleObj['Id'] = $(this).val();
            singleObj['Type'] = 'P';
            singleObj['ReasonDesc'] = "";
        });
        $('input.chkNonClass').prop('checked', false);
    }
    primaryresult.push(singleObj);
    SetButtonStatus();
}
function onNonPrimaryCheckBox(nonPrimaryChk) {
    nonPrimaryresult = [];
    nonPrimaryAmount = 0;
    tableSplitValueNonPrimary = 0;
    if (parseInt($(nonPrimaryChk).closest('tr').find('td:last').text()) != tableSplitValuePrimary + 1) {
        $(nonPrimaryChk).prop('checked', false);
    }
    $.each($("input[id='nonPrimaryCheckBox']:checked"), function () {
        singleObj = {};
        singleObj['Id'] = $(this).val();
        singleObj['Type'] = 'NP';
        singleObj['ReasonDesc'] = "";
        nonPrimaryresult.push(singleObj);
    });
    SetButtonStatus();
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
            url: '/ManualReconsile/GetTemplateByGroupId',
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
            $('#manualSearchbtn').prop('disabled', false);
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
    SetButtonStatus();
    $('#manualSearchbtn').prop('disabled', true);
    $("#datetimepickerFromDate").datepicker().datepicker("setDate", new Date());
    $("#datetimepickerToDate").datepicker().datepicker("setDate", new Date());
}
function SetButtonStatus() {
    if (primaryresult.length > 0 && nonPrimaryresult.length > 0) {
        $('#manualProcessbtn').prop('disabled', false);
        $('#reasonTxt').prop('disabled', false);
    }
    else {
        $('#manualProcessbtn').prop('disabled', true);
        $('#reasonTxt').prop('disabled', true);
    }

}
