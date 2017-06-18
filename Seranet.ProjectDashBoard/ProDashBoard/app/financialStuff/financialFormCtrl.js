(function () {

    app.controller("financialDataController", ['$scope', 'toaster', '$mdDialog', '$http', '$stateParams', '$window', FinancialDataController]);

    function FinancialDataController($scope,toaster,$mdDialog, $http, $stateParams,$window) {
        $scope.yearArray = [];
        $scope.timeReportDataMissingMembers = '';
        
        fillYearCombo();
        loadDate();
        loadAccounts();
        isAuthorized();
        

        $scope.AllocatedHours = [];
        $scope.BillableHours = [];
        $scope.TotalHours = [];
        $scope.billableCheck = [];
        $scope.billableArray = [[1, 'Hourly Billable'], [2, 'FullTime Billable'], [3, 'Non Billable']];
        //$scope.yearArray = [];

        function loadTimeReportData(year,month,accountId) {

            $http.get('api/FinancialData/getDataFromTimeReports/' + year + '/' + month + '/' + accountId).success(function (data) {
                $scope.timeReportData = data.sendingData;
                console.log(data.financialDataMissingMembers);
                if ($scope.timeReportData.length != 0) {
                    if (data.financialDataMissingMembers != "" && data.financialDataMissingMembers!=null){
                        $scope.timeReportDataMissingMembers = data.financialDataMissingMembers;
                    }
                    console.log("Time Report Data Retrieved " + data.sendingData.length);
                } else {
                    toaster.pop('warning', "Notificaton", "No Time Report Data Found");
                }
            })
            .error(function () {
                $window.location.href = '#/error';
            });
        }
        
        function fillYearCombo() {
            
            var currentYear = new Date().getFullYear();
            for (var k = 2010; k <= currentYear; k++) {
                $scope.yearArray.push(k);
            }
            
        }

        function loadDate() {
            var currentYear = new Date().getFullYear();
            var currentMonth = new Date().getMonth() + 1;
            $scope.monthCombo =JSON.stringify(currentMonth-1);
            $scope.yearCombo1 = JSON.parse(currentYear);
            $scope.selectedYear = currentYear;
            console.log(currentYear + " " + currentMonth + " " + $scope.yearCombo1);
        }

        function loadAccounts() {
            $http.get('api/Account/adminPanelActiveAccounts').success(function (data) {
                $scope.Accounts = data;
                if (data != null) {
                    $scope.accountCombo = JSON.stringify(data[0]);
                    loadTimeReportData($scope.yearCombo1, $scope.monthCombo, $scope.Accounts[0].Id);
                }
            })
            .error(function (error) {
                alert(error);
                $scope.error = "An Error has occured while loading posts!";

            });
        }

        $scope.accountChange = function () {
            var receivedObject = JSON.parse($scope.accountCombo);
            $scope.displayingAccount = " - " + receivedObject.AccountName;
        }

        $scope.SearchData = function () {
            $scope.timeReportDataMissingMembers = "";
            var receivedObject = JSON.parse($scope.accountCombo);
            loadTimeReportData($scope.yearCombo1, $scope.monthCombo, receivedObject.Id);
        }

        $scope.SaveData = function (ev) {
            var year = $scope.yearCombo1;
            var month = $scope.monthCombo;
            var receivedObject = JSON.parse($scope.accountCombo);
            var accountId = receivedObject.Id;
            var returnArray = [];
            var quarter = 0;
            if (month >= 1 && month <= 6) {
                quarter = 1;
            } else {
                quarter = 2;
            }
            $scope.sendingData=[];
            angular.forEach($scope.timeReportData, function (value, key) {
                var returnData = { "EmpId": value.EmpId, "EmpName": value.EmpName, "AccountId": accountId, "AccountName": receivedObject.AccountName, "Year": year, "Month": month, "Quarter": quarter, "BillableType": $scope.billableCheck[value.EmpId], "AllocatedHours": $scope.AllocatedHours[value.EmpId], "BillableHours": $scope.BillableHours[value.EmpId], "TotalReportedHours": $scope.TotalHours[value.EmpId] };
                $scope.sendingData.push(returnData);
            });
            
            if ($scope.timeReportData != 0) {
                checkSummaryAvailability(accountId, month, year, quarter, $scope.sendingData,ev);
            } else {
                toaster.pop('warning', "Notificaton", "No Time Report Data To Be Stored");
                //alert("No Time Report Data To Be Stored");
            }
            //saveData($scope.sendingData);

        }

        function createConfirmDialog(title,content,ev) {
        var confirm = $mdDialog.confirm()
        .title(title)
        .textContent(content)
        .ariaLabel('Lucky day')
        .targetEvent(ev)
        .ok('Yes')
        .cancel('No');
        return confirm;
        }

        //api/FinancialSummary/getFinalMonthSummary/{AcountId}/{Year}/{Quarter}
        function checkSummaryAvailability(AccountId, month, Year, Quarter, returnData,ev) {
            $http.get('api/FinancialSummary/getFinalMonthSummary/'+AccountId+'/'+Year+'/'+Quarter).success(function (data) {
                console.log(angular.isObject(data));
                if (angular.isObject(data)) {
                    
                    var selectedDate = new Date("'" + month + "/01/" + Year + "" + "'");
                    var pastDate = new Date(selectedDate.setMonth(selectedDate.getMonth() - 1));
                    console.log("PastDate " + (pastDate.getMonth() + 1) + " " + pastDate.getFullYear());
                    var returnedYear = data.Year;
                    var returnedMonth = data.Month;

                    console.log(returnedMonth + " " + returnedYear + " " + data.ExpectedHours);
                    if (returnedYear == pastDate.getFullYear()) {
                        if ((pastDate.getMonth() + 1) == data.Month) {
                            saveData(returnData);
                        } else {
                            console.log("DATA "+(returnedYear == Year & returnedMonth == month) + " " + returnedYear+" "+ Year +" "+ returnedMonth +" "+ month);
                            if (returnedYear == Year & returnedMonth == month) {

                                $mdDialog.show(createConfirmDialog('Time Report Data Entering', 'Do you want to update the existing time report data for the latest month?', ev)).then(function () {
                                    updateExistingResults(returnData);
                                }, function () {
                                    
                                });
                                //if(confirm("Do you want to update the existing time report data for the latest month?")){
                                //    updateExistingResults(returnData);
                                //}
                            } 
                            else {
                                $mdDialog.show(createConfirmDialog('Time Report Data Entering', 'You Have not entered data for the past month. However If you proceed ahead, then you will not be able to enter past month time report data. Do You Want To Proceed And Save ?', ev)).then(function () {
                                    saveData(returnData);
                                }, function () {
                                
                                });

                                //if (confirm("You Have not entered data for the past month. However If you proceed ahead, then you will not be able to enter past month time report data. Do You Want To Proceed And Save ?") == true) {
                                //    saveData(returnData);
                                //}
                            }
                        }
                    } else {
                        toaster.pop('note', "Notificaton", "You Have not entered data for the past month.");
                        //alert("You Have not entered data for the past month.");
                    }
                } else {
                    //toaster.pop('note', "Notificaton", "No Records Have Not Added..");
                    //alert("No Records Have Not Added..");
                    saveData(returnData);
                }

            }).error(function () {

            });
        }

        function updateExistingResults(results) {
            $http.put('api/FinancialResults/update', results).success(function (data) {
                toaster.pop('success', "Notificaton", "Existing Time Report Data Updated Successfully");
                
            }).error(function () {

            });
        }

        function isAuthorized() {
            $http.get('api/Authorization').success(function (data) {
                if (data != null) {
                    $scope.LoggedInUserName = "Hi, Seranet / " + data;
                    
                }


            })
        .error(function () {
            $scope.error = "An Error has occured while loading posts!";

        });
        }
        
        function saveData(results) {
            //api/FinancialSummary/doesSummaryExists/{AccountId}/{Year}/{Month}
            var receivedObject = JSON.parse($scope.accountCombo);
            var accountId = receivedObject.Id;
            $http.get('api/FinancialSummary/doesSummaryExists/' + accountId + '/' + $scope.yearCombo1 + '/' + $scope.monthCombo).success(function (data) {
                $scope.isExists = data;
                if (!$scope.isExists) {
                    $http.post('api/FinancialResults/add', results).success(function (data) {
                        toaster.pop('success', "Notificaton", "Time Report Data Saved Successfully");
                        //alert("Time Report Data Saved Successfully");

                    }).error(function () {

                    });
                } else {
                    //if (confirm("Do You Want To Update Last Month Time Report Data ?")) {
                        $http.post('api/FinancialResults/add', results).success(function (data) {
                            toaster.pop('success', "Notificaton", "Time Report Data Saved Successfully");
                            //alert("Time Report Data Saved Successfully");

                        }).error(function () {

                        });
                    //} else {

                   // }
                }
                console.log("Exists" +$scope.isExists);
            }).error(function () {

            });
            
        }
    }

})();
