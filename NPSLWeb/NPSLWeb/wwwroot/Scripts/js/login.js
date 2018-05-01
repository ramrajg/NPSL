function LoginUser() {
    self.listMenus = ko.observableArray([]);
    var serviceUrl = '/api/GetUsersValidation?userId=' + userId.value + '&password=' + userPassword.value;
    apiGetCall(serviceUrl, self.listMenus, true, function () {
        alert('SucessFull')
    }, function (jqXHR, textStatus, errorThrown) {
        alert(jqXHR.responseJSON.error)
    });
};
