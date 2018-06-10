//var webUtils = (function () {
//var pub = {};

function apiGetCall(serviceUrl, listDestination, isDisabled, callback, errorCallback) {
    serviceUrl = 'http://localhost:50704' + serviceUrl;
    listDestination([]);
    $(document).ready(function () {

        $.ajax({
            type: "GET",
            url: serviceUrl,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result) {
                    listDestination(result);
                    if (callback && typeof (callback) === "function")
                        callback();
                }
            },
            failure: function (data) {
                alert(data.responseText);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log("The following error occurred: " + jqXHR.responseJSON.error);
                console.log(errorThrown);
                if (errorCallback && typeof (errorCallback) === "function") {
                    errorCallback(jqXHR, textStatus, errorThrown);
                }
            }
        }).always(function () {
            //if (isDisabled)
            //    isDisabled(false);
        });
    });
}

function apiGetCallController(controllerName, methodName, methodType, data, successCallBack, errorCallback) {
    urlString = '/' + controllerName + '/' + methodName;
    $(document).ready(function () {
        $.ajax({
            url: urlString,
            type: methodType,
            data: data,
            //dataType: 'json',
            //contentType: 'application/json; charset=utf-8',
            success: function (data) {
                if (successCallBack && typeof (successCallBack) === "function")
                    successCallBack();
            },
            failure: function (data) {
                alert(data.responseText);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log("The following error occurred: " + jqXHR.responseJSON.error);
                console.log(jqXHR, textStatus, errorThrown);
                if (errorCallback && typeof (errorCallback) === "function") {
                    errorCallback(jQuery.parseJSON(jqXHR.responseJSON).error);
                }
            }
        });
    });
}

function getNowDate() {
    var now = new Date(),
        month = '' + (now.getMonth() + 1),
        day = '' + now.getDate(),
        year = now.getFullYear();
    if (month.length < 2) month = '0' + month;
    if (day.length < 2) day = '0' + day;
    return [year, month, day].join('-');
}

function IsNullOrUndefinedOrBlank(x) {
    if (x === undefined || x === null || x === "") {
        return true;
    }
    else {
        return false;
    }

}
function AllowNumbersOnly(e) {
    var code = (e.which) ? e.which : e.keyCode;
    if (code > 31 && (code < 48 || code > 57)) {
        e.preventDefault();
    }
}

function typeWriter(elementId, txt, speed) {
    if (i < txt.length) {
        document.getElementById(elementId).innerHTML += txt.charAt(i);
        i++;
        setTimeout(typeWriter, speed);
    }
}


function setTextValue(elementId, txt) {
    document.getElementById(elementId).value = txt;
}

