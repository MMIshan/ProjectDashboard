(function () {
    'use strict';
    //create angularjs controller

    app.controller('errorController', ['$scope', '$http', '$stateParams', ErrorController]);

    //angularjs controller method
    function ErrorController($scope, $http, $stateParams) {
        //alert("Unauthorized WarFare............Access denied");
    }
})();