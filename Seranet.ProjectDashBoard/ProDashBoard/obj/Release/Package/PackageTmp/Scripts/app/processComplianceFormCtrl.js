(function () {
    'use strict';
    //create angularjs controller
    app.controller('processComplianceFormController', ['$scope', '$window', '$http', '$stateParams', processComplianceFormController]);
    
    //angularjs controller method
    function processComplianceFormController($scope, $window, $http, $stateParams) {
        $scope.answerArray = [];
        function chunk(arr) {
            var newArr = [];
            var tempArray = [];
            for (var i = 1; i <= arr.length; i++) {
                tempArray.push(arr[i - 1]);
                if (i % 2 == 0) {
                    newArr.push(tempArray);
                    tempArray = [];
                }

                if (i == arr.length & i % 2 != 0) {
                    newArr.push(tempArray);
                }
            }
            return newArr;
        }
        $scope.originalParameters;

        //Load all the available quality paramaters
        $http.get('api/ProcessCompliance/getQualityParameters').success(function (data) {
            $scope.originalParameters=data;
            $scope.QualityParameters = chunk(data);
        })

    .error(function () {
        $scope.error = "An Error has occured while loading posts!";
    });
        $scope.qParameters = [];
        $scope.qParameters.push("Consistent");
        $scope.qParameters.push("Inconsistent");
        $scope.qParameters.push("Poor");

        $scope.yearArray = [];

        var currentYear = new Date().getFullYear();
        function loadYearArray() {
            $scope.yearArray = [];
            for (var k = 2008; k <= currentYear; k++) {
                $scope.yearArray.push(k);
            }
        }

        //Declare key pressed action for rating text field (validate the input as a numeric value)
        $scope.filterValue = function ($event) {
            var val = String.fromCharCode($event.keyCode);
            console.log(val == '.');
            if (val == '.') {
            } else if (val != '.') {
                if (!IsNumeric1(String.fromCharCode($event.keyCode))) {
                    $event.preventDefault();
                    
                }
            }
            
        };
        function IsNumeric1(input) {
           var RE = /^-?\d+(?:[.,]\d*?)?$/;
            return (RE.test(input));
        }

        //Load all the enabled subProjects
        function loadProjectData() {
            $http.get('api/Project/getProjectData').success(function (data) {
                $scope.projects = data;
            })
        .error(function () {
            $scope.error = "An Error has occured while loading posts!";
            
        });
        }
        loadProjectData();

        //Declare projectCombo change action 
        $scope.projectComboChange = function () {
            loadYearArray();
            $scope.quaterArray = [];
            var selectedProject = JSON.parse($scope.projectCombo);
            $scope.account = selectedProject.AccountName;
        }
        $scope.quaterArray=[];

        //Declare yearCombo change action
        $scope.yearChange = function () {
            var selectedProject = JSON.parse($scope.projectCombo);
            $scope.quaterArray = [];
            $scope.quaterArray.push(1);
            $scope.quaterArray.push(2);

            //Load valid quarters to the quarterCombo 
            $http.get('api/ProcessCompliance/checkSummaryAvailabilityForYear/' +selectedProject.Id+ '/' + $scope.yearCombo).success(function (data) {
                console.log("Checked " + data);
                angular.forEach(data, function (value, key) {
                    $scope.quaterArray.splice(($scope.quaterArray.indexOf(value.Quarter)), 1);
                });
                
                

            })
            .error(function () {
                        $scope.error = "An Error has occured while loading posts!";
                    });
        }

        $scope.submitForm = function () {
           // submitReceivedData();
        }

        //Declare submitAnother button action - submit data to the database and refresh the page
        $scope.submitFormData = function () {
            submitReceivedData();
            $scope.projectCombo = "";
            $scope.yearCombo = "";
            loadYearArray();
            $scope.quarterCombo = "";
            $scope.account = "";
            loadProjectData();
            $scope.rating = "";
            $scope.QualityParameters = "";
            $scope.qParameters = [];
            $scope.qParameters.push("Consistent");
            $scope.qParameters.push("Inconsistent");
            $scope.qParameters.push("Poor");
            $http.get('api/ProcessCompliance/getQualityParameters').success(function (data) {
                $scope.originalParameters = data;
                $scope.QualityParameters = chunk(data);
            })

            .error(function () {
            $scope.error = "An Error has occured while loading posts!";
        });
        };

        //Declare submit button action - submit data to the database and return to the main page
        $scope.submitAndReturn = function () {
            submitReceivedData();
            $window.location.href = '#/';
        };

        //gather data from the form 
        function submitReceivedData() {
            var project = JSON.parse($scope.projectCombo);
            $scope.sendingResults = project.Id + "|" + project.AccountId + "|" + $scope.yearCombo + "|" + $scope.quarterCombo + "|" + $scope.rating + "|";
            for (var y = 0; y < $scope.originalParameters.length; y++) {
                
                $scope.sendingResults += $scope.originalParameters[y].Id + "-" + $scope.answerArray[$scope.originalParameters[y].Id] + ":";
            }

            submitData($scope.sendingResults);
        }



        function submitData(results) {
            var send = results;
                                 
            var sendingData = JSON.stringify(send);
            $http.post('api/ProcessCompliance/add', sendingData).success(function (data) {
                alert("Survey Details Saved Successfully");
        })

        .error(function () {
            $scope.error = "An Error has occured while loading posts!";
        });
        }
    }

    
})();