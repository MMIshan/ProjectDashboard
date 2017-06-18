(function () {
    'use strict';
    //create angularjs controller

    app.controller('adminPanelController', ['$scope', '$http', '$window', adminPanelController]);

    //angularjs controller method
    function adminPanelController($scope, $http, $window) {
        $window.location.href = '#/adminPanel/account';
    }
})();