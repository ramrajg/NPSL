$(document).ready(function () {
    $(loginForm).submit(function (event) {
        self.listMenus = ko.observableArray([]);
        var data = $(loginForm).serialize();
        apiGetCallController('Login', 'GetUserDetails', 'GET', data, function () {
            window.location.href = '/MainMenu/Index';
        }, function (responseText) {
            $.notify(responseText, 'danger');
            return false;
        });
        return false;
    });
});



