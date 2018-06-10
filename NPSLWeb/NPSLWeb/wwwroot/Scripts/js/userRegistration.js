$(document).ready(function () {
    $(userRegistrationForm).submit(function (event) {
        self.listUsers = ko.observableArray([]);
        var data = $(userRegistrationForm).serialize();
        apiGetCallController('Users', 'AddUserrecord', 'POST', data, function () {
            window.location.href = '/Users/Index';
        }, function (responseText) {
            $.notify(responseText, 'danger');
            return false;
        });
        return false;
    });
});
