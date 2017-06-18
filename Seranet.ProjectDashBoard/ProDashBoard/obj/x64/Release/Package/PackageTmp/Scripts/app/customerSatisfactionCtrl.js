(function () {
    //create angularjs controller

    app.controller('customerSatisfaction', ['$scope', '$http', '$stateParams', CustomerSatisfaction]);

    //angularjs controller method
    function CustomerSatisfaction($scope, $http, $stateParams) {
        $scope.accountId = $stateParams.id;
        //alert($scope.accountId);
        $scope.data = [];
        $scope.labels = [];
        $scope.keepData = [];
        $scope.myData = [];
        loadsubProjects();
        $scope.series = [];

        //Plot all the subproject summaries of the particular account to the chart (multi line chart)
        $http.get('api/CustomerSatisfaction/getSelectedAccountSummaries/' + $scope.accountId).success(function (data) {
            $scope.selectedAccountSummaries = data;
            $scope.keepData = $scope.selectedAccountSummaries;
            var x = 0;
            
            angular.forEach(data, function (value, key) {
                $scope.tempArray = [];
                angular.forEach(value, function (secondaryValue, key) {
                    
                    $scope.tempArray.push(secondaryValue.Rating);
                    if (x == 0) {
                        $scope.labels.push(secondaryValue.Year + "-Q" + secondaryValue.Quarter);
                    }

                });
                x++;
                $scope.data.push($scope.tempArray);
                
                console.log("Cus " + $scope.labels.toString());
                
            });
            $scope.myData = $scope.data;
            
            
        })

        .error(function () {
            $scope.error = "An Error has occured while loading posts!";
            
        });
        $scope.colours = [{ // default
            //"fillColor": "rgba(223, 110, 112, 1)",
            //"strokeColor": "rgba(207,100,103,1)",
            //"pointColor": "rgba(220,220,220,1)",
            //"pointStrokeColor": "#fff",
            //"pointHighlightFill": "#fff",
            //"pointHighlightStroke": "rgba(151,187,205,0.8)"
        }];
        $scope.fill = false;

        //Declare chart options
        $scope.options = {
            legend: { display: true },
            datasetFill: $scope.fill,
            scaleBeginAtZero: true,
        };

        //Declare chart on click event
        $scope.onClick = function (points, evt) {
            if (points.length == 1) {
                var receivedProject = JSON.parse($scope.projectCombo);
                console.log("gfg " + points[0].value + " " + points[0].label);
                $scope.clickedLabel = points[0].label;
                $scope.clickedYear = $scope.clickedLabel.split('-')[0];
                $scope.clickedQuarter = $scope.clickedLabel.split('-')[1].toString();
                
                $scope.clickedQuarter = $scope.clickedQuarter.substring(1, 2);
                fillCustomerSatisfactionResults($scope.accountId, receivedProject.Id, $scope.clickedYear, $scope.clickedQuarter);
                $scope.cusResults = true;
                $scope.yearCombo = $scope.clickedYear;
                if ($scope.clickedQuarter == 1) {
                    $scope.quarter1 = 'q1';
                    $scope.quarter2 = 'qnone';
                } else {
                    $scope.quarter2 = 'q1';
                    $scope.quarter1 = 'qnone';
                }
            } else {
                alert("Do not allow for MultiChart");
            }
            
        }

        $scope.yearArray = [];

        //Load yearCombo data
        var currentYear = new Date().getFullYear();
        for (var k = 2008; k <= currentYear; k++) {
            $scope.yearArray.push(k);
        }
        $scope.yearCombo = $scope.yearArray[0];
        $scope.projectCombo = 'All';
        
        //Load data to subproject combo
        function loadsubProjects() {
            $http.get('api/Project/getSelectedAccountProjects/' + $scope.accountId).success(function (data) {
                $scope.projects = data;
                if (data != "" && data.length == 1) {
                    
                    $scope.projectCombo = angular.toJson(data[0]);
                    fillCustomerSatisfactionResults($scope.accountId, data[0].Id, 0, 0);
                }


                angular.forEach($scope.projects, function (val, key) {
                    $scope.series.push(val.Name);
                });
                

            })

            .error(function () {
                $scope.error = "An Error has occured while loading posts!";
                
            });
        }

        //Declare subprojectCombo change action
        $scope.subProjectChange = function () {
            $scope.myTemp = [];
            $scope.data = $scope.keepData;
            
            
            if ($scope.projectCombo == 'All') {
                console.log($scope.projectCombo);
                $scope.data = $scope.myData;
                $scope.subProjectName = "";
                loadsubProjects();
                $scope.cusResults = false;
                $scope.quarter1 = 'qnone';
                $scope.quarter2 = 'qnone';
                $scope.options.datasetFill = false;
            } else {
                var receivedProject = JSON.parse($scope.projectCombo);
                $scope.subProjectName=" - "+receivedProject.Name;
                $scope.cusResults = false;
                $scope.quarter1 = 'qnone';
                $scope.quarter2 = 'qnone';
                $scope.series = [];
                $scope.options.datasetFill = true;
                //$scope.fill = true;
                $scope.series.push(receivedProject.Name);
                angular.forEach($scope.keepData, function (value, key) {

                    angular.forEach(value, function (Secondvalue, key) {
                        
                        if (Secondvalue.ProjectId == receivedProject.Id) {
                            $scope.myTemp.push(Secondvalue.Rating);
                            console.log(Secondvalue.Rating);
                        }
                    });
                    
                });
                $scope.data = [];
                $scope.data.push($scope.myTemp);
                var receivedProject = JSON.parse($scope.projectCombo);
                fillCustomerSatisfactionResults($scope.accountId, receivedProject.Id, 0, 0);
            }
            
        }

        //check whether selected item in the combo is 'All'
        function checkFieldsValidation() {
            if ($scope.projectCombo == 'All') {
                return false;
            } else {
                return true;
            }
        }

        //Declare quarter button actions
        $scope.q1Click = function () {
            $scope.quarter1 = 'q1';
            $scope.quarter2 = 'qnone';
            console.log("LOG " + checkFieldsValidation());
            if (checkFieldsValidation()) {
                $scope.cusResults = true;
                var receivedProject = JSON.parse($scope.projectCombo);
                fillCustomerSatisfactionResults($scope.accountId, receivedProject.Id, $scope.yearCombo, 1);
            } else {
                $scope.quarter1 = 'qnone';
                $scope.quarter2 = 'qnone';
                $scope.cusResults = false;
                alert("Please select a Project");
            }
        }
        $scope.q2Click = function () {
            $scope.quarter2 = 'q1';
            $scope.quarter1 = 'qnone';
            if (checkFieldsValidation()) {
                $scope.cusResults = true;
                var receivedProject = JSON.parse($scope.projectCombo);
                fillCustomerSatisfactionResults($scope.accountId, receivedProject.Id, $scope.yearCombo, 2);
            } else {
                $scope.quarter1 = 'qnone';
                $scope.quarter2 = 'qnone';
                $scope.cusResults = false;
                alert("Please select a Project");
            }
        }
        

        function loadSelectedProjectSummary(accountId,projectId,year,quarter) {
            $http.get('api/CustomerSatisfaction/getSelectedProjectSummary/' + accountId + '/ ' + projectId + '/ ' + year + '/ ' + quarter).success(function (data) {
                $scope.projectSummary = data;
            })
            .error(function () {
                $scope.error = "An Error has occured while loading posts!";
            });
        }


        //Fill table with customer satisfaction results
        function fillCustomerSatisfactionResults(accountId,projectId,year,quarter){
            $http.get('api/CustomerSatisfaction/getSelectedProjectResults/' + accountId + '/ ' + projectId + '/ ' + year + '/ ' + quarter).success(function (data) {
                $scope.CusResults = data;
                if (data == "") {
                    $scope.cusResults = false;
                } else {
                    $scope.cusResults = true;

                    $scope.CusTopResult = $scope.CusResults[1];
                    

                    console.log($scope.CusTopResult.Year + " " + $scope.CusTopResult.Quarter);

                    $scope.yearCombo = $scope.CusTopResult.Year;
                    loadSelectedProjectSummary(accountId, projectId, $scope.CusTopResult.Year, $scope.CusTopResult.Quarter);
                    if ($scope.CusTopResult.Quarter == 1) {
                        $scope.quarter1 = 'q1';
                        $scope.quarter2 = 'qnone';
                    } else {
                        $scope.quarter2 = 'q1';
                        $scope.quarter1 = 'qnone';
                    }
                }
            })

            .error(function () {
                $scope.error = "An Error has occured while loading posts!";
            });
            
        }
    }
})();