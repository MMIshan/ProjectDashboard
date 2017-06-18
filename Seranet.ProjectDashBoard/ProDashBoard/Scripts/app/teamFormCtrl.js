(function () {
    'use strict';
    //create angularjs controller

    app.controller('teamFormController', ['$scope', '$rootScope', '$http', '$window', teamFormController]);

    //angularjs controller method
    function teamFormController($scope, $rootScope, $http, $window) {
        $scope.close = function () {
            $modalInstance.dismiss('cancel');
        };

        $scope.len = 8;
        $scope.te = $rootScope.project;
        isAuthorized();

        $http.get('api/Account/').success(function (data) {
        $scope.projects = data;
        })
        .error(function () {
            $scope.error = "An Error has occured while loading posts!";
        });

        //Load loggedIn user's accounts
        function loadAccounts() {
            $scope.yearArray = [];
            loadYearArray();
            $scope.quaterArray = [];
            $http.get('api/EmployeeProjects/getEmployeeAccounts/' + $scope.employee.Id).success(function (data) {
                $scope.accounts = data;
                $scope.projects = "";
                
                angular.element(document.getElementById('subBut'))[0].disabled = true;
        
            })
        .error(function () {
            $scope.error = "An Error has occured while loading posts!";
            
        });
        };

        //Declare account combo change action
        $scope.accountChange = function () {
            $scope.te = $scope.accountCombo.name;
            $scope.yearArray = [];
            loadYearArray();
            $scope.quaterArray = [];
            $scope.selectedAccountId = $scope.accountCombo;

            $http.get('api/EmployeeProjects/getEmployeeAccountProjects/' + $scope.selectedAccountId + '/' + $scope.employee.Id).success(function (data) {
                console.log("SelectedEmp " + data.length);
                $scope.projects = data;
                
            })
        .error(function () {
            $scope.error = "An Error has occured while loading posts!";
            
        });
        }

        
        $scope.quaterArray = [];

        //Declare year combo change action
        $scope.yearChange = function () {
            $scope.selectedAccountId = $scope.accountCombo;
            $scope.quaterArray = [];
            $http.get('api/TeamSatisfactionEmployeeSummary/getEmployeeSummaryList/' + $scope.employee.Id + '/' + $scope.accountCombo + '/' + $scope.yearCombo).success(function (data) {
        
                $scope.employeeSummaries = data;
                $scope.quaterArray.push(1);
                $scope.quaterArray.push(2);
                $scope.quaterArray.push(3);
                $scope.quaterArray.push(4);
                
                    for (var x = 0; x < $scope.employeeSummaries.length; x++) {
                        console.log("EmployeeSummary " + $scope.employeeSummaries[x].Quarter);
                        $scope.quaterArray.splice(($scope.quaterArray.indexOf($scope.employeeSummaries[x].Quarter)),1);
                    }
                
                
            })
        .error(function () {
            $scope.error = "An Error has occured while loading posts!";
            
        });
        }


        $scope.yearArray = [];
        
        $scope.rateArray = [];
        $scope.yesArray = [];
        $scope.commentArray = [];
        var currentYear = new Date().getFullYear();

        //Fill yearArray (yearCombo data)
        function loadYearArray() {
            for (var k = 2008; k <= currentYear; k++) {
                $scope.yearArray.push(k);
            }
        }
        
        //Load quastions
        $http.get('api/Question/').success(function (data) {
            $scope.Questions = data;
        })
        .error(function () {
            $scope.error = "An Error has occured while loading posts!";
            
        });
         
        $scope.myFun = function (sendingValue) {
            var sendArr = [];
            for (var x = 1; x <= sendingValue; x++) {
                sendArr.push(x);
            }
            return sendArr;
        }

        //Submit answers and comments (To save them in the database)
        $scope.submitForm = function () {
            $scope.vb='1x';
            $scope.newResultArray = [];
            $scope.sendingData="";
            for (var y = 0; y < $scope.Questions.length; y++) {
                
                $scope.sendingData = $scope.sendingData + ":" + $scope.employee.Id + '|' + $scope.accountCombo + '|' + 1 + '|' + $scope.yearCombo + '|' + $scope.quarterCombo + '|' + $scope.Questions[y].Id + '|' + $scope.rateArray[$scope.Questions[y].Id] + '|' + $scope.commentArray[$scope.Questions[y].Id]
                console.log($scope.rateArray[$scope.Questions[y].Id]);
                if (typeof $scope.rateArray[$scope.Questions[y].Id] == "undefined") {
                    console.log("ERRR");
                    $scope.rateArray[$scope.Questions[y].Id] = -1;
                }
                console.log($scope.Questions.length + "--------" + $scope.employee.Id + '/' + $scope.accountCombo + '/' + 1 + '/' + $scope.yearCombo + '/' + $scope.quarterCombo + '/' + $scope.Questions[y].QuestionType + '/' +  $scope.rateArray[$scope.Questions[y].Id] + '/' + $scope.commentArray[$scope.Questions[y].Id]);
            }


            var sendingData = JSON.stringify($scope.sendingData);
        
            
            $http.post('api/results/add', sendingData).success(function (data) {
                    alert("survey details saved successfully");
                    $window.location.href = '#/';
                })
                .error(function () {
                    $scope.error = "an error has occured while loading posts!";
                    
                });
                
        }

        //check authorization
        function isAuthorized() {
            $http.get('api/Authorization').success(function (data) {

                getEmployee(JSON.parse(data));
    
            })
        .error(function () {
            $scope.error = "An Error has occured while loading posts!";
            
        });
        }

        $scope.employee;
        function getEmployee(username) {
            $http.get('api/TeamMembers/' + username).success(function (data) {
                if (data != 'null') {
                    $scope.employee = data;
                    $scope.employeeName = data.MemberName;
                    loadAccounts();
                    $rootScope.userId = data.Id;
                } else {
                    $rootScope.userId = 'úndefined';
                }
        
            })
        .error(function () {
            $scope.error = "An Error has occured while loading posts!";
            
        });
        }
    }
})();