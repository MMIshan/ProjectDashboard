(function () {
    'use strict';
    //create angularjs controller
    
    app.controller('myController', ['$scope', '$window', '$rootScope', '$modal', '$http', '$stateParams', summaryController]);

    //angularjs controller method
    function summaryController($scope, $window, $rootScope, $modal, $http, $stateParams) {
        $scope.overall;
        $scope.proId = $stateParams.Id;
        
        $scope.loggedInUserId;
        isAuthorized();
        $scope.activeRights = false;


        //Redirect to team satisfcation review popup
        $scope.open = function (employee) {
            console.log('opening pop up ' + employee);
            var modalInstance = $modal.open({
                templateUrl: 'Layout/PopupPage.html',
                controller: 'popupController',
                resolve: {
                    ProId: function () {
                        return $stateParams.id;
                    },
                    Year: function () {
                        return $scope.sendingyear;
                    },
                    Quarter: function () {
                        return $scope.sendingmonth;
                    },
                    EmpId: function () {
                        return employee;
                    }
                }
            });
            
        }
        $scope.employee;
        //check authorization
        function isAuthorized() {
            $http.get('api/Authorization').success(function (data) {
                getEmployee(JSON.parse(data));
                
            })
        .error(function () {
            $scope.error = "An Error has occured while loading posts!";
            
        });
        }

        
        function getEmployee(username) {
            $http.get('api/TeamMembers/' + username).success(function (data) {
                if (data != 'null') {
                    $scope.employee = data;
                    $scope.loggedInUserId = data.Id;
                    if ($scope.employee.AdminRights == 1) {
                        $scope.activeRights = true;
                    }
                    
                    loadEmployeeProjects($scope.employee.Id);
                    
                } 
                
            })
        .error(function () {
            $scope.error = "An Error has occured while loading posts!";
            
        });
        }
      
        getProjectSummaries();
        getSelectedResults();

        $scope.exists = false;

        //Load loggedIn user's projects
        function loadEmployeeProjects(empId) {
            $http.get('api/EmployeeProjects/getEmployeeProjects/' + empId).success(function (data) {
                if (data != 'null') {
                    $scope.employeeProjects = data;
                    var x=0;
                    angular.forEach(data, function (value, key) {
                        if (value.AccountId == $scope.proId) {
                            x++;
                            if(x==1){
                                
                            }
                            $scope.exists = true;
                            if (value.Lead == true) {
                                $scope.activeRights = true;
                            }
                            
                        }
                    });
                    
                    
                }
                
            })
                    .error(function () {
                        $scope.error = "An Error has occured while loading posts!";
                        
                    });
        }

        //Load selected account teamsatisfaction summary
        function getProjectSummaries() {

            $http.get('api/Summary/getSelectedProjectSummaries/' + $stateParams.id).success(function (data) {
                $scope.selectedSummaries = data;
                
                if ($scope.selectedSummaries == '') {
                    $window.location.href = '#/error';
                
                }else{
                    $scope.overall = $scope.selectedSummaries[$scope.selectedSummaries.length - 1].Rating;
                    $scope.labelArray = [];
                    var seriesArray = [];
                    $scope.labels = [];
                    $scope.data = [[]];
                    var x = 0;
            
                    angular.forEach(data, function (value, key) {

                        var year = value.Year.toString();
                        $scope.labels.push(year + "-Q" + value.Quarter);
                        seriesArray.push(value.Rating);
                        $scope.data[0][x] = value.Rating;
                        x++;
                    });
            
                }
                })

                .error(function () {
                    $scope.error = "An Error has occured while loading posts!";
           
                });
            
        }

        //Load selected account latest results
        function getSelectedResults() {

            $http.get('api/Results/getSelectedResults/' + $stateParams.id + '/' + 0 + '/' + 0).success(function (data) {
                $scope.selectedPro = data;
                if (data.length == 0) {
                    $scope.sh1 = false;
                } else {
                    $scope.sh1 = true;

                    var arr = [];
                    for (var i = 0; i < ($scope.selectedPro[0].length - 1) ; i++) {
                        arr.push($scope.selectedPro[0][i][0]);
                    }
                    
                    $scope.send = arr;
                    $scope.yearCombo = $scope.selectedPro[0][0][4];
                    $scope.q = $scope.selectedPro[0][0][5];
                    if ($scope.q == 1) {
                        $scope.quarter1 = 'q1';
                    } else if ($scope.q == 2) {
                        $scope.quarter2 = 'q1';
                    } else if ($scope.q == 3) {
                        $scope.quarter3 = 'q1';
                    } else {
                        $scope.quarter4 = 'q1';
                    }

                    $scope.sendingyear = $scope.yearCombo;
                    $scope.sendingmonth = $scope.q;
                    loadEmpAverages($stateParams.id, $scope.sendingyear, $scope.sendingmonth);
                }
                
            })

            .error(function () {
                $scope.error = "An Error has occured while loading posts!";
                
            });
        }
        var item = 0;
        $scope.rowVal = 0;
        $scope.tempVal = 0;
        $scope.setRowTotal1 = 9;

        //Calculate row average
        $scope.setRowTotal1 = function (val) {
        item = item + parseInt(val);
        return item;
        };
        
        //Load yeararray
        $scope.yearArray = [];
        var currentYear = new Date().getFullYear();
        for (var k = 2008; k <= currentYear; k++) {
            $scope.yearArray.push(k);
        }
        
        //Declare quarter button clicks
        $scope.q1Click = function () {
            $scope.quarter1 = 'q1';
            $scope.quarter2 = 'qnone';
            $scope.quarter3 = 'qnone';
            $scope.quarter4 = 'qnone';
            setTableData($stateParams.id, $scope.yearCombo,1);
            
        };
        $scope.q2Click = function () {
            $scope.quarter1 = 'qnone';
            $scope.quarter2 = 'q1';
            $scope.quarter3 = 'qnone';
            $scope.quarter4 = 'qnone';
            setTableData($stateParams.id, $scope.yearCombo, 2);
        };
        $scope.q3Click = function () {
            $scope.quarter1 = 'qnone';
            $scope.quarter2 = 'qnone';
            $scope.quarter3 = 'q1';
            $scope.quarter4 = 'qnone';
            setTableData($stateParams.id, $scope.yearCombo, 3);
        };
        $scope.q4Click = function () {
            $scope.quarter1 = 'qnone';
            $scope.quarter2 = 'qnone';
            $scope.quarter3 = 'qnone';
            $scope.quarter4 = 'q1';
            setTableData($stateParams.id, $scope.yearCombo, 4);
        };

        //Load teamsatisfactionresults to table 
        function setTableData(projectId, year, month) {
            $scope.sendingProjectId = projectId;
            $scope.sendingyear = year;
            $scope.sendingmonth = month;
            $http.get('api/Results/getSelectedResults/' + projectId + '/' + year + '/' + month).success(function (data) {
                $scope.selectedPro = data;
                if (data.length == 0) {
                    $scope.sh1 = false;
                } else {
                    $scope.sh1 = true;
                    var arr = [];
                    for (var i = 0; i < ($scope.selectedPro[0].length - 1) ; i++) {
                        arr.push($scope.selectedPro[0][i][0]);
                    }
                    $scope.send = arr;
                }
                
        })

        .error(function () {
            $scope.error = "An Error has occured while loading posts!";
    
        });

        //Load summaries for selected year and quarter
        $http.get('api/Summary/getLatestProjectSummaries/' + projectId + '/' + year + '/' + month).success(function (data) {
                    $scope.selectedSummary = data;
                    $scope.overall = $scope.selectedSummary.Rating
                    console.log("Overrall " + $scope.overall);
        })

            .error(function () {
                $scope.error = "An Error has occured while loading posts!";

            });

        loadEmpAverages(projectId, year, month);
        }

        //Load employee averages
        function loadEmpAverages(accountId, year, quarter) {
            $http.get('api/TeamSatisfactionEmployeeSummary/' + accountId + '/' + year + '/' + quarter).success(function (data) {
                $scope.empAverages = data;
                console.log(data.Rating);
        
            })

               .error(function () {
                   $scope.error = "An Error has occured while loading posts!";
        
               });
        }

        $scope.message = "Click on the hyper link to view the students list.";
        
        $scope.series = ['Series A'];

        

        $scope.colours = [{ // default
            "fillColor": "rgba(224, 108, 112, 1)",
            "strokeColor": "rgba(207,100,103,1)",
            "pointColor": "rgba(220,220,220,1)",
            "pointStrokeColor": "#fff",
            "pointHighlightFill": "#fff",
            "pointHighlightStroke": "rgba(151,187,205,0.8)"
        }];

        //Declare teamSatisfaction chart pointer's on click event
        $scope.onClick = function (points, evt) {
            $scope.arr = [65, 59, 80];
        
            $scope.overall = points[0].value;
            $scope.clickedLabel=points[0].label;
            $scope.clickedYear = $scope.clickedLabel.split('-')[0];
            $scope.clickedQuarter=$scope.clickedLabel.split('-')[1].toString();
            $scope.clickedQuarter = $scope.clickedQuarter.substring(1, 2);

            $scope.yearCombo = $scope.clickedYear;
            $scope.q = $scope.clickedQuarter;
            $scope.sendingyear = $scope.yearCombo;
            $scope.sendingmonth = $scope.q;
            if ($scope.q == 1) {
                $scope.quarter1 = 'q1';
                $scope.quarter2 = 'qnone';
                $scope.quarter3 = 'qnone';
                $scope.quarter4 = 'qnone';
            } else if ($scope.q == 2) {
                $scope.quarter1 = 'qnone';
                $scope.quarter2 = 'q1';
                $scope.quarter3 = 'qnone';
                $scope.quarter4 = 'qnone';
            } else if ($scope.q == 3) {
                $scope.quarter1 = 'qnone';
                $scope.quarter2 = 'qnone';
                $scope.quarter3 = 'q1';
                $scope.quarter4 = 'qnone';
            } else {
                $scope.quarter1 = 'qnone';
                $scope.quarter2 = 'qnone';
                $scope.quarter3 = 'qnone';
                $scope.quarter4 = 'q1';
            }
            $http.get('api/Results/getSelectedResults/' + $stateParams.id + '/' + $scope.clickedYear + '/' + $scope.clickedQuarter).success(function (data) {
                $scope.selectedPro = data;
                if (data.length == 0) {
                    $scope.sh1 = false;
                } else {
                    $scope.sh1 = true;
                    var arr = [];
                    for (var i = 0; i < ($scope.selectedPro[0].length - 1) ; i++) {
                        arr.push($scope.selectedPro[0][i][0]);
                    }
                    $scope.send = arr;
                }

                loadEmpAverages($stateParams.id, $scope.clickedYear, $scope.clickedQuarter);

            })

        .error(function () {
            $scope.error = "An Error has occured while loading posts!";
        });
        };

        

        $http.get('api/Account/' + $stateParams.id).success(function (data) {
            $scope.selectedPro1 = data;
            $scope.projectName1 = $scope.selectedPro1.AccountName;
            $scope.te = $scope.projectName1;
        });

        //chart options
        $scope.options = {
            responsive: false,
            maintainAspectRatio: false,
            scaleBeginAtZero: true,
            
        }
       
        $scope.sh1 = true;

        $scope.formClick = function () {
            console.log('opening pop up ');
            var modalInstance = $modal.open({
                templateUrl: 'Layout/TeamSatisfactionForm.html',
                controller: 'teamFormController',
        
            });

        }
    }
})();