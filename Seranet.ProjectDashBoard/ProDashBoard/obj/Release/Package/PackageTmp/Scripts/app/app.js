
//set and get the angular module

var app = angular.module("app", ['ui.router', 'ui.bootstrap', 'chart.js']);

//Configure app Urls

app.config(function ($stateProvider, $urlRouterProvider) {
    $urlRouterProvider.otherwise('/');

    $stateProvider.state('main', {
        url:'/',
        templateUrl: 'Layout/Home.html',
        

    })
    .state('home', {
        url: '/home/:id',
        templateUrl: 'Layout/Team.html',
        

    })
        .state('teamForm', {
            url: '/teamForm/',
            templateUrl: 'Layout/TeamSatisfactionForm.html',
            

        })
        .state('customerSatisfaction', {
            url: '/customerSatisfaction/:id',
            templateUrl: 'Layout/CustomerSatisfaction.html',
            

        })
        .state('processCompliance', {
            url: '/processCompliance/:id',
            templateUrl: 'Layout/ProcessCompliance.html',
            

        })
        .state('processComplianceForm', {
            url: '/processComplianceForm/',
            templateUrl: 'Layout/ProcessComplianceForm.html',
            
            
        })
        .state('error', {
            url: '/error',
            templateUrl: 'Layout/ErrorPage.html',
            

        })
        .state('adminPanel', {
            url: '/adminPanel',
            templateUrl: 'Layout/AdminPanel.html',

        })
        .state('adminPanel.account', {
            url: '/account',
            templateUrl: 'Layout/AdminAccount.html',
            

        })
    .state('adminPanel.project', {
        url: '/project',
        templateUrl: 'Layout/AdminProject.html',


    })
    .state('adminPanel.employee', {
        url: '/employee',
        templateUrl: 'Layout/AdminEmployees.html',


    });
        

   
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