//set module
var app = angular.module("app", ['ui.router', 'ui.bootstrap', 'chart.js', 'toaster', 'ngAnimate', 'ngMaterial', 'HiggidyCarousel', 'angularSpinner', 'n3-pie-chart', 'ngSanitize']);

//Configure app Urls

app.config(function ($stateProvider, $urlRouterProvider) {
    $urlRouterProvider.otherwise('/');

    $stateProvider.state('main', {
        url: '/',
        templateUrl: 'app/home/Home.html',


    })
    .state('teamSatisfaction', {
        url: '/teamSatisfaction/:id',
        templateUrl: 'app/teamSatisfaction/Team.html',


    })
        .state('teamForm', {
            url: '/teamForm/',
            templateUrl: 'app/teamSatisfaction/TeamSatisfactionForm.html',


        })
        .state('customerSatisfaction', {
            url: '/customerSatisfaction/:id/:projectId/:year/:quarter',
            templateUrl: 'app/customerSatisfaction/CustomerSatisfaction.html',


        })
        .state('processCompliance', {
            url: '/processCompliance/:id/:projectId/:year/:quarter',
            templateUrl: 'app/processCompliance/ProcessCompliance.html',


        })
        .state('processComplianceForm', {
            url: '/processComplianceForm/',
            templateUrl: 'app/processCompliance/ProcessComplianceForm.html',


        })
        .state('error', {
            url: '/error',
            templateUrl: 'app/error/ErrorPage.html',


        })
        .state('adminPanel', {
            url: '/adminPanel',
            templateUrl: 'app/adminPanel/AdminPanel.html',

        })
        .state('adminPanel.account', {
            url: '/account',
            templateUrl: 'app/adminPanel/AdminAccount.html',


        })
    .state('adminPanel.project', {
        url: '/project',
        templateUrl: 'app/adminPanel/AdminProject.html',


    })
    .state('adminPanel.employee', {
        url: '/employee',
        templateUrl: 'app/adminPanel/AdminEmployees.html',


    })
    .state('financialForm', {
        url: '/financialForm',
        templateUrl: 'app/financialStuff/FinancialForm.html',


    })
    .state('financialStatus', {
        url: '/financialStatus/:id',
        templateUrl: 'app/financialStuff/FinancialStatus.html',


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
