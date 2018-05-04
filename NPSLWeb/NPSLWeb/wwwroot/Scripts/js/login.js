$(document).ready(function () {
    $(loginForm).submit(function (event) {
        self.listMenus = ko.observableArray([]);
        //var serviceUrl = '/api/GetUsersValidation?userId=' + userId.value + '&password=' + userPassword.value;
        //apiGetCall(serviceUrl, self.listMenus, true, function () {
        //    window.location.href = '/MainMenu/Index';
        //}, function (jqXHR, textStatus, errorThrown) {
        //    $.notify(jqXHR.responseJSON.error, 'danger');
        //    return false;
        //});
        //return false;


        //var serviceUrl = '/api/GetUsersValidation?userId=' + userId.value + '&password=' + userPassword.value;
        //apiGetCallController('LoginController', 'GetUserDetails', 'GET', 'json', null, null);

        var serviceUrl = '/api/GetUsersValidation?userId=' + userId.value + '&password=' + userPassword.value;
        apiGetCallController('Login', 'GetUserDetails', 'GET', 'json', function () {
            window.location.href = '/MainMenu/Index';
        }, function (jqXHR, textStatus, errorThrown) {
            $.notify(jqXHR.responseJSON.error, 'danger');
            return false;
        });
        return false;



        //    apiGetCallController(LoginController, GetUserDetails, 'GET', 'json',
        //        function () {
        //            window.location.href = '/MainMenu/Index';
        //        }

        //        ,
        //        function (jqXHR, textStatus, errorThrown) {
        //            $.notify(jqXHR.responseJSON.error, 'danger');
        //            return false;
        //        });



        //    );
        //return false;
    });
});



