(function () {
    'use strict';
    //create angularjs controller

    app.controller('errorController', ['$scope', '$http', '$routeParams', ErrorController]);

    //angularjs controller method
    function ErrorController($scope, $http, $routeParams) {
        //alert("Unauthorized WarFare............Access denied");
    }
})();