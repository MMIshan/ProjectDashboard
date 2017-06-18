(function () {
    //create angularjs controller

    app.controller('customerSatisfaction', ['$scope', 'toaster', '$mdDialog', '$window', '$http', '$stateParams', CustomerSatisfaction]);

    //angularjs controller method
    function CustomerSatisfaction($scope,toaster,$mdDialog,$window, $http, $stateParams) {

        $scope.quarter1 = 'qnone';
        $scope.quarter2 = 'qnone';

        $scope.accountId = $stateParams.id;
        $scope.initialProjectId = $stateParams.projectId;
        $scope.initialYear = $stateParams.year;
        $scope.initialQuarter = $stateParams.quarter;
        $scope.key1 = $scope.accountId;
        localStorage.setItem('account', JSON.stringify($scope.key1));
        //alert($scope.accountId);\
        //alert(JSON.parse(localStorage.getItem('account')));
        $scope.data = [];
        $scope.labels = [];
        $scope.keepData = [];
        $scope.myData = [];
        loadsubProjects($scope.accountId);
        $scope.series = [];
        isAuthorized();
        loadInitialChartData($scope.accountId);
        $http.get('api/Account/' + $stateParams.id).success(function (data) {
            $scope.selectedPro1 = data;
            $scope.projectName1 = $scope.selectedPro1.AccountName;

            $scope.te = $scope.projectName1;
        });

        function initializeChartData() {
            $scope.myTemp = [];
            $scope.data = $scope.keepData;
            var receivedProject = null;
            angular.forEach($scope.projects, function (value, key) {
                console.log($scope.initialProjectId + "    " + value.Id);
                if ($scope.initialProjectId == value.Id) {
                    console.log("fdfdfd");
                    receivedProject = value;
                    $scope.projectCombo = angular.toJson(value);
                    console.log(receivedProject);
                }

            });
            //var receivedProject = JSON.parse($scope.projectCombo);
            
            console.log(receivedProject);
            $scope.subProjectName = " - " + receivedProject.Name;
            $scope.cusResults = false;
            $scope.quarter1 = 'qnone';
            $scope.quarter2 = 'qnone';
            $scope.series = [];
            $scope.options.datasetFill = true;
            //$scope.fill = true;
            $scope.series.push(receivedProject.Name);

            $scope.yearCombo = $scope.initialYear;
            if ($scope.initialQuarter == 1) {
                $scope.quarter1 = 'q1';
                $scope.quarter2 = 'qnone';
            } else {
                $scope.quarter2 = 'q1';
                $scope.quarter1 = 'qnone';
            }


            angular.forEach($scope.keepData, function (value, key) {

                angular.forEach(value, function (Secondvalue, key) {

                    if (Secondvalue.ProjectId == $scope.initialProjectId) {
                        if (Secondvalue.Rating == 0) {
                            $scope.myTemp.push(null);
                            console.log(Secondvalue.Rating);
                        } else {
                            $scope.myTemp.push(Secondvalue.Rating);
                            console.log(Secondvalue.Rating);
                        }
                    }
                });

            });
            $scope.data = [];
            $scope.data.push($scope.myTemp);
            //var receivedProject = JSON.parse($scope.projectCombo);
            fillCustomerSatisfactionResults($scope.accountId, $scope.initialProjectId, $scope.initialYear, $scope.initialQuarter);
        }

        //Plot all the subproject summaries of the particular account to the chart (multi line chart)
        function loadInitialChartData(accountId) {
            $scope.data = [];
            $scope.labels = [];
        $http.get('api/CustomerSatisfaction/getSelectedAccountSummaries/' + accountId).success(function (data) {
           
            var x = 0;
            if (data != "") {
                $scope.selectedAccountSummaries = data;
                $scope.keepData = $scope.selectedAccountSummaries;
                angular.forEach(data, function (value, key) {
                    $scope.tempArray = [];
                    angular.forEach(value, function (secondaryValue, key) {
                        console.log("Test " + secondaryValue.Rating);
                        if (secondaryValue.Rating == 0) {
                            $scope.tempArray.push(null);

                        } else {
                            $scope.tempArray.push(secondaryValue.Rating);
                        }

                        if (x == 0) {
                            $scope.labels.push(secondaryValue.Year + "-Q" + secondaryValue.Quarter);
                        }

                    });
                    x++;
                    $scope.data.push($scope.tempArray);
                    //$scope.testArray = [null, 1, 2];
                    // $scope.data.push($scope.testArray);
                    console.log("Cus " + $scope.labels.toString());

                });
                $scope.myData = $scope.data;
                initializeChartData();
            } else {
                $scope.labels = [];
            }
            
            
        })

        .error(function () {
            $window.location.href = '#/error';
            $scope.error = "An Error has occured while loading posts!";
            
        });
        }


        function loadChartData(accountId) {
            $scope.data = [];
            $scope.labels = [];
            $http.get('api/CustomerSatisfaction/getSelectedAccountSummaries/' + accountId).success(function (data) {

                var x = 0;
                if (data != "") {
                    $scope.selectedAccountSummaries = data;
                    $scope.keepData = $scope.selectedAccountSummaries;
                    angular.forEach(data, function (value, key) {
                        $scope.tempArray = [];
                        angular.forEach(value, function (secondaryValue, key) {
                            console.log("Test " + secondaryValue.Rating);
                            if (secondaryValue.Rating == 0) {
                                $scope.tempArray.push(null);

                            } else {
                                $scope.tempArray.push(secondaryValue.Rating);
                            }

                            if (x == 0) {
                                $scope.labels.push(secondaryValue.Year + "-Q" + secondaryValue.Quarter);
                            }

                        });
                        x++;
                        $scope.data.push($scope.tempArray);
                        //$scope.testArray = [null, 1, 2];
                        // $scope.data.push($scope.testArray);
                        console.log("Cus " + $scope.labels.toString());

                    });
                    $scope.myData = $scope.data;
                } else {
                    $scope.labels = [];
                }


            })

            .error(function () {
                $window.location.href = '#/error';
                $scope.error = "An Error has occured while loading posts!";

            });
        }


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
            responsive: true,
            legend: { display: true },
            datasetFill: $scope.fill,
            scaleBeginAtZero: true,
        };
        $scope.colours = ['#3598DC', '#CD853F', '#2E8B57', '#6A5ACD', '#20B2AA'];
        //Declare chart on click event
        $scope.onClick = function (points, evt) {
            if (points.length == 1) {
                var receivedProject = JSON.parse($scope.projectCombo);
                console.log("gfg " + points[0].value + " " + points[0].label);
                $scope.clickedLabel = points[0].label;
                $scope.clickedYear = $scope.clickedLabel.split('-')[0];
                $scope.clickedQuarter = $scope.clickedLabel.split('-')[1].toString();
                
                $scope.clickedQuarter = $scope.clickedQuarter.substring(1, 2);
                fillCustomerSatisfactionResults($scope.key1, receivedProject.Id, $scope.clickedYear, $scope.clickedQuarter);
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
                toaster.pop('warning', "Notificaton", "Please select a project from the dropdown first");
                //alert("Do not allow for MultiChart");
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
        function loadsubProjects(accountId) {
            $scope.series = [];
            $http.get('api/Project/getSelectedAccountProjects/' + accountId).success(function (data) {
                $scope.projects = data;
                console.log("dsdsds--------------------dsdsdsd");
                console.log($scope.projects)
                
                if (data != "" && data.length == 1) {
                    
                    $scope.projectCombo = angular.toJson(data[0]);
                    fillCustomerSatisfactionResults($scope.key1, data[0].Id, 0, 0);
                }


                angular.forEach($scope.projects, function (val, key) {
                    console.log("fdf " + val.Name);
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
                loadsubProjects($scope.key1);
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
                            if (Secondvalue.Rating == 0) {
                                $scope.myTemp.push(null);
                                console.log(Secondvalue.Rating);
                            } else {
                                $scope.myTemp.push(Secondvalue.Rating);
                                console.log(Secondvalue.Rating);
                            }
                        }
                    });
                    
                });
                $scope.data = [];
                $scope.data.push($scope.myTemp);
                var receivedProject = JSON.parse($scope.projectCombo);
                fillCustomerSatisfactionResults($scope.key1, receivedProject.Id, 0, 0);
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

        $scope.convertData = function (result) {
            //JSON.parse(result);
        }
        //Declare quarter button actions
        $scope.q1Click = function () {
            $scope.quarter1 = 'q1';
            $scope.quarter2 = 'qnone';
            console.log("LOG " + checkFieldsValidation());
            if (checkFieldsValidation()) {
                $scope.cusResults = true;
                var receivedProject = JSON.parse($scope.projectCombo);
                fillCustomerSatisfactionResults($scope.key1, receivedProject.Id, $scope.yearCombo, 1);
            } else {
                $scope.quarter1 = 'qnone';
                $scope.quarter2 = 'qnone';
                $scope.cusResults = false;
                toaster.pop('warning', "Notificaton", "Please select a Project");
                //alert("Please select a Project");
            }
        }
        $scope.q2Click = function () {
            $scope.quarter2 = 'q1';
            $scope.quarter1 = 'qnone';
            if (checkFieldsValidation()) {
                $scope.cusResults = true;
                var receivedProject = JSON.parse($scope.projectCombo);
                fillCustomerSatisfactionResults($scope.key1, receivedProject.Id, $scope.yearCombo, 2);
            } else {
                $scope.quarter1 = 'qnone';
                $scope.quarter2 = 'qnone';
                $scope.cusResults = false;
                toaster.pop('warning', "Notificaton", "Please select a Project");
                
            }
        }
        

        function loadSelectedProjectSummary(accountId,projectId,year,quarter) {
            $http.get('api/CustomerSatisfaction/getSelectedProjectSummary/' + accountId + '/ ' + projectId + '/ ' + year + '/ ' + quarter).success(function (data) {
                $scope.projectSummary = data;
            })
            .error(function () {
                $window.location.href = '#/error';
                $scope.error = "An Error has occured while loading posts!";
            });
        }

        function isAuthorized() {
            $http.get('api/Authorization').success(function (data) {
                if (data != null) {
                    $scope.LoggedInUserName = "Hi, Seranet / " + data;
                    isAdminOrTeamLead();
                    getLoggedUser();
                }


            })
        .error(function () {
            $scope.error = "An Error has occured while loading posts!";

        });
        }

        //api/Authorization/getLoggedInUser
        function getLoggedUser() {
            $http.get('api/Authorization/getLoggedInUser').success(function (data) {
                $scope.loggedUser = data;
                loadUserAccounts();
            })
            .error(function () {
                $scope.error = "An Error has occured while loading posts!";

            });
        }

        function loadUserAccounts() {
            $http.get('api/EmployeeProjects/getEmployeeAccounts/' + $scope.loggedUser.Id).success(function (data) {
                $scope.userAccounts = data;
            })
                     .error(function () {
                         $scope.error = "An Error has occured while loading posts!";
                     });
        }

        function isAdminOrTeamLead() {
            $http.get("api/Authorization/getAdminOrTeamLeadRights/" + $stateParams.id).success(function (data) {
                $scope.isAdmin = data.split('-')[0].toLowerCase() == 'true';
                $scope.isTeamLead = data.split('-')[1].toLowerCase() == 'true';

            })
            .error(function () {

            });
        }

        //Fill table with customer satisfaction results
        function fillCustomerSatisfactionResults(accountId, projectId, year, quarter) {
            console.log("DATA "+accountId + " " + projectId + " " + year + " " + quarter);
            $http.get('api/CustomerSatisfaction/getSelectedProjectResults/' + accountId + '/ ' + projectId + '/ ' + year + '/ ' + quarter).success(function (data) {
                $scope.CusResults = data;
                if (data == "") {
                    toaster.pop('warning', "Notificaton", "No Results Found");
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
                $window.location.href = '#/error';
                $scope.error = "An Error has occured while loading posts!";
            });
            
        }

        $scope.proNameChange = function (key) {
            $scope.quarter1 = 'qnone';
            $scope.quarter2 = 'qnone';
            $scope.fill = false;
            $scope.options.datasetFill = false;
            $scope.projectCombo = 'All';
            $scope.labels = [];
            $scope.series = [];
            $scope.CusResults = null;
            $scope.cusResults = false;
            localStorage.setItem('account', JSON.stringify($scope.key1));
            loadsubProjects($scope.key1);
            loadChartData($scope.key1);
            
        }
    }
})();
