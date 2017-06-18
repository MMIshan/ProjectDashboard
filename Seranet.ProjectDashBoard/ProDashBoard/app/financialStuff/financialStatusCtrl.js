(function () {
    'use strict';

    app.controller('financialStatusController', ['$scope', 'toaster', '$mdDialog', '$window', '$http', '$stateParams', FinancialStatusController]);

    function FinancialStatusController($scope,toaster, $mdDialog,$window, $http, $stateParams) {
        // alert("FinancialStatus");
        $scope.returnedAccountId = $stateParams.id;
        localStorage.setItem('account', $scope.returnedAccountId);
        $scope.data = [];
        $scope.labels = [];
        $scope.returnedMonthArray = [];
        $scope.set_color = function () {
            
                return { color: "red" }
            
        }
        Initialize();
        isAuthorized();
        loadChartData($scope.returnedAccountId, 0, 0);
        getSelectedFinancialResults($scope.returnedAccountId, 0, 0);
        $scope.options = {
            responsive: true,
            legend: { display: true },
            scaleBeginAtZero: true,
            
            //datasetFill: false,
        };

        $scope.colors = ['#3498DB', '#FF7F50', '#717984', '#F1C40F'];
          
        $scope.yearArray = [];

        //Load yearCombo data
        var currentYear = new Date().getFullYear();
        for (var k = 2008; k <= currentYear; k++) {
            $scope.yearArray.push(k);
        }

       // $scope.colors = [
       //     {
       //         "fillcolor": ['#3498DB', '#FF7F50', '#717984', '#F1C40F'],
       //     //"strokecolor": ["#00FFFF", "#8B008B"],
       //     //"pointcolor": "rgba(220,220,220,1)",
       //     //"pointstrokecolor": "#fff",
       //     //"pointhighlightfill": "#fff",
       //     //"pointhighlightstroke": "rgba(151,187,205,0.8)"
       //}];

        function Initialize() {
            $http.get('api/Account/' + $stateParams.id).success(function (data) {
                $scope.selectedPro1 = data;
                if (data != null) {
                    $scope.projectName1 = $scope.selectedPro1.AccountName;
                    $scope.te = $scope.projectName1;
                }
            }).error(function () {

            });
            
        }

        function loadChartData(accountId, year, quarter) {
            $scope.data = [];
            $scope.labels = [];
            $scope.returnedMonthArray = [];
            //api/FinancialSummary/getSummaryDataForChart/{AccountId}/{Year}/{Quarter}
            $http.get('api/FinancialSummary/getSummaryDataForChart/' + accountId+'/'+year+'/'+quarter).success(function (data) {
                $scope.chartData = data;

                console.log("------------");
                console.log(data);
                console.log(data[0]);
                $scope.series = [];
                $scope.series[0] = 'Expected';
                $scope.series[1] = 'Actual';
                //$scope.series[2] = '';

                var array1 = [];
                //$scope.data.push(array1);
                if (data.length != 0) {
                    $scope.tempExpectedArray = [];
                    $scope.tempActualArray = [];
                    var firstdata = data[0];
                    $scope.yearCombo = firstdata.Year;
                    
                    if (firstdata.Quarter == 1) {
                        $scope.quarter1 = 'q1';
                         $scope.quarter2 = 'qnone';
                    } else {
                        $scope.quarter2 = 'q1';
                        $scope.quarter1 = 'qnone';
                    }

                   

                    angular.forEach(data, function (value, key) {
                        $scope.tempExpectedArray.push(value.ExpectedHours);
                        $scope.tempActualArray.push(value.coveredBillableHours);
                        $scope.labels.push(value.Year + "-" + value.Month);
                        $scope.returnedMonthArray.push(value.MonthName);
                        console.log("Month " + value.MonthName);
                    });
                    //$scope.returnedMonthArray.push("Allocation");
                    //$scope.returnedMonthArray.push("Reported");
                    //$scope.returnedMonthArray.push("Extra/Lag");
                    $scope.data.push($scope.tempExpectedArray);
                    $scope.data.push($scope.tempActualArray);
                }
                console.log("ChartDataLength " + data.length);
                
                var array2 = [];
                
               //$scope.data.push(array2);
            }).error(function () {
                $window.location.href = '#/error';
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


        $scope.q1Click = function () {
            $scope.quarter1 = 'q1';
            $scope.quarter2 = 'qnone';
           // $scope.tableData = null;
            
            loadChartData($scope.returnedAccountId, $scope.yearCombo, 1);
            getSelectedFinancialResults($scope.returnedAccountId, $scope.yearCombo, 1);
        }

        $scope.q2Click = function () {
            $scope.quarter2 = 'q1';
            $scope.quarter1 = 'qnone';
            //$scope.tableData = null;
            
            loadChartData($scope.returnedAccountId, $scope.yearCombo, 2);
            getSelectedFinancialResults($scope.returnedAccountId, $scope.yearCombo, 2);
        }
        $scope.tableData = null;
        $scope.endRowItemIndexes = [];
        function getSelectedFinancialResults(accountId, year, quarter) {
            $scope.endRowItemIndexes = [];
            $scope.tempEndrowItemValue = 0;
            $scope.tempEndrowItemAllocatedValue = 0;
            $scope.totalAllocation = 0;
            $scope.totalBillable = 0;
            $scope.totalextraorlagCal = 0;
            $scope.totalnonBillableCal = 0;
            $scope.totalhourlyBillableCal = 0;
            $http.get('api/FinancialResults/getSelectedFinancialResults/' + year + '/' + quarter + '/' + accountId).success(function (data) {
                console.log("FinancialData " + data.length);
                if (data.length != 0) {
                    $scope.tableData = data;

                    $scope.financialResults = true;

                    for (var x = 0; x < $scope.tableData[0].length; x++) {
                        console.log(x);
                        $scope.endRowItemIndexes.push(x);
                    }

                } else {
                    toaster.pop('warning', "Notificaton", "No Results Found");
                    $scope.financialResults = false;
                }
            })
            .error(function () {
                $window.location.href = '#/error';
            });
        }

        $scope.tempEndrowItemValue = 0;
        $scope.tempEndrowItemAllocatedValue = 0;
        $scope.endrowItemValueCal = function (endrowItemValue) {
            var itemValue = 0;
            var allocatedValue = 0;
            for (var x = 0; x < $scope.tableData.length; x++) {
                
                if ($scope.tableData[x][endrowItemValue].BillableType == 2) {
                    console.log("b "+$scope.tableData[x][endrowItemValue].BillableType);
                    itemValue += $scope.tableData[x][endrowItemValue].ConsiderableHours;
                    allocatedValue += $scope.tableData[x][endrowItemValue].AllocatedHours;
                }
                
            }
            

            itemValue = $scope.tempEndrowItemValue + itemValue;
            allocatedValue=$scope.tempEndrowItemAllocatedValue+allocatedValue;
            $scope.tempEndrowItemValue = itemValue;
            $scope.tempEndrowItemAllocatedValue = allocatedValue;
            console.log(itemValue+"  Value   "+allocatedValue);
            //if (itemValue > allocatedValue) {
            //    itemValue = allocatedValue;
            //}
            return itemValue.toFixed(2);
        }
        
        $scope.totalAllocation=0;
        $scope.allocationCal = function (financialStatus) {
            var allocationCount = 0;
            angular.forEach(financialStatus, function (value, key) {
                if (value.BillableType == 2) {
                    allocationCount = allocationCount + value.AllocatedHours;
                }
                
            });
            $scope.totalAllocation += allocationCount;
            return allocationCount;
        }

        $scope.totalBillable = 0;
        $scope.billableCal = function (financialStatus) {
            var billableCount = 0;
            angular.forEach(financialStatus, function (value, key) {
                if (value.BillableType == 2) {
                    billableCount = billableCount + value.BillableHours;
                }
            });
            $scope.totalBillable += billableCount;
            return billableCount;
        }

        $scope.totalextraorlagCal = 0;
        $scope.extraorlagCal = function (financialStatus) {
            var extralagCount = 0;
            angular.forEach(financialStatus, function (value, key) {
                if (value.BillableType == 2) {
                    extralagCount = extralagCount + value.ExtraOrLag;
                }
            });
            console.log("LOG " + extralagCount);
                $scope.totalextraorlagCal += extralagCount;
            return extralagCount;
        }


        $scope.extraorlagCalColour = function (financialStatus) {
            var extralagCount = 0;
            angular.forEach(financialStatus, function (value, key) {
                if (value.BillableType == 2) {
                    extralagCount = extralagCount + value.ExtraOrLag;
                }
            });
            
            return extralagCount;
        }


        $scope.totalnonBillableCal = 0;
        $scope.nonBillableCal = function (financialStatus) {
            var nonBillableCount = 0;
            angular.forEach(financialStatus, function (value, key) {
                if (value.BillableType == 3) {
                    nonBillableCount = nonBillableCount + value.BillableHours;
                }
            });
            $scope.totalnonBillableCal += nonBillableCount;
            return nonBillableCount;
        }

        $scope.totalhourlyBillableCal = 0;
        $scope.hourlyBillableCal = function (financialStatus) {
            var hourlyBillableCount = 0;
            angular.forEach(financialStatus, function (value, key) {
                if (value.BillableType == 1) {
                    hourlyBillableCount = hourlyBillableCount + value.BillableHours;
                }
            });
            $scope.totalhourlyBillableCal += hourlyBillableCount;
            return hourlyBillableCount;
        }

        $scope.proNameChange = function (key) {
            $scope.quarter1 = 'qnone';
            $scope.quarter2 = 'qnone';
            //Initialize();
            //isAuthorized();
            localStorage.setItem('account',$scope.key1);
            loadChartData($scope.key1, 0, 0);
            getSelectedFinancialResults($scope.key1, 0, 0);
        }

    }

})();


