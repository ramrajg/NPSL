function pollServer() {
    $.ajax({
        async: true,
        cache: true,
        type: "GET",
        url: '/MainMenu/RefreshData',
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (data) {
            $("#partial").html(data);
            setTimeout(pollServer, 10000);
        }
    });

}

window.onload = function () { pollServer() }
function addContributor(data, event) {
    self.isContributorRowAddClicked(true);
    self.listContributors.push(new ClsContributor(self));
    return false;
}