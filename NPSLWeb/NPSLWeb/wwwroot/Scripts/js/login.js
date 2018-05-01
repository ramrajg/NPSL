$(document).ready(function () {
    $(loginForm).submit(function (event) {
        //setTextValue("erroMessage", "");
        self.listMenus = ko.observableArray([]);
        var serviceUrl = '/api/GetUsersValidation?userId=' + userId.value + '&password=' + userPassword.value;
        apiGetCall(serviceUrl, self.listMenus, true, function () {
            $.notify('I am a success box.', 'success');
        }, function (jqXHR, textStatus, errorThrown) {
            $.notify(jqXHR.responseJSON.error, 'danger');
            return false;
        });
        return false;
    });
});


function clearError() {
    setTextValue("erroMessage", "");
}
function TestNotification() {
    $.notify('I am a success box.', 'success');
}




