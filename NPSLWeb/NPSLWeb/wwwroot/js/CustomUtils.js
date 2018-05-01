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
var input = document.getElementById('userId');
if (input !== null) {
    input.addEventListener('keyup', function (e) {

        if (input.value.length > 0) {
            document.getElementById("userIdBlank").style.visibility = "hidden";
        }
        if (input.value.length == 0) {
            searchHits.style.display = 'none';
        }

    })
};

function typeWriter(elementId,txt,speed) {
    if (i < txt.length) {
        document.getElementById(elementId).innerHTML += txt.charAt(i);
        i++;
        setTimeout(typeWriter, speed);
    }
}


function setTextValue(elementId, txt) {
    document.getElementById("erroMessage").innerHTML = txt;
}


