
//set and get the angular module

var app = angular.module("app", ['ngRoute', 'ui.bootstrap', 'chart.js']);

//Configure app Urls

app.config(function ($routeProvider) {
    $routeProvider.when('/', {
        templateUrl: 'Layout/Home.html',
        

    })
    .when('/home/:id', {
        templateUrl: 'Layout/Team.html',
        

    })
        .when('/teamForm/', {
            templateUrl: 'Layout/TeamSatisfactionForm.html',
            

        })
        .when('/customerSatisfaction/:id', {
            templateUrl: 'Layout/CustomerSatisfaction.html',
            

        })
        .when('/processCompliance/:id', {
            templateUrl: 'Layout/ProcessCompliance.html',
            

        })
        .when('/processComplianceForm/', {
            templateUrl: 'Layout/ProcessComplianceForm.html',
            
            
        })
        .when('/error', {
            templateUrl: 'Layout/ErrorPage.html',
            

        })
        .otherwise('/');

   
});

app.controller('con', ['$scope', '$rootScope', '$http', '$window', myfun]);

function myfun($scope, $rootScope, $http) {

    //$http.get('http://99xt.lk/services/api/Projects', { withCredentials: true }).
    //            success(function (data, status, headers, config) {
    //                console.log(data);
    //                //$scope.userName = data.split("\\")[1].toString().toLowerCase();

                    

    //            }).
    //            error(function (data, status, headers, config) {
    //                console.log(data);
    //            });

    
    
}