(function () {
    'use strict';
    //create angularjs controller

    app.controller('adminAccountController', ['$scope', '$rootScope', '$http', '$window', adminAccountController]);

    //angularjs controller method
    function adminAccountController($scope, $rootScope, $http, $window) {

        loadactiveAccounts();
        loadInactiveAccounts();
        loadActiveEmployees();

        $scope.toCreateNewAccount = false;
        $scope.createNewAccount = false;
        $scope.isAccountInactive = false;
        $scope.accountDisabled = false;
        $scope.accountButtonArray = [];
        $scope.accountColorClass = ['x1', 'x2', 'x3', 'x4'];
        $scope.clickedButton = 0;
        $scope.createdProjects = [];
        $scope.projectsAvailability = [];
        $scope.projectCheck = [];
        function loadInactiveAccounts() {
            //api/Account/getInacativeAccounts
            $http.get('api/Account/getInacativeAccounts').success(function (data) {
                // $scope.inactiveAccounts = data
                $scope.tempInactiveAccounts = data;
                console.log("Inactive " + data);
            })
            .error(function () {
                $scope.error = "An Error has occured while loading posts!";
            });
        }

        function loadactiveAccounts() {
            //api/Account/getInacativeAccounts
            $http.get('api/Account').success(function (data) {
                $scope.activeAccounts = data;
                $scope.tempActiveAccounts = data;

            }).error(function () {
                $scope.error = "An Error has occured while loading posts!";
            });
        }

        function loadSelectedAccountProjects(accountId) {
            //api/Project/getSelectedAdminAccountProjects/{accountId}
            $http.get('api/Project/getSelectedAdminAccountProjects/' + accountId).success(function (data) {
                $scope.projects = data;

            }).error(function () {
                $scope.error = "An Error has occured while loading posts!";
            });
        }
        $scope.newProjects = [];
        $scope.selectedAccount = 0;
        $scope.accountClick = function (account) {
            $scope.projectCheck = [];
            $scope.changedProjectsEnabilities = [];
            loadSelectedAccountProjects(account.Id);
            $scope.newProjects = [];
            $scope.selectedAccount = account.Id;
            fillAccountData(account);
            $scope.createNewAccount = false;
            $scope.accountDisabled = false;
            var x = $scope.accountColorClass[1];
            $scope.accountColorClass = 'info';
            $scope.clickedButton = account.Id;
            //alert(account.AccountName);
        }

        function fillAccountData(account) {
            $scope.isAccountInactive = false;
            $scope.accountName = account.AccountName;
            $scope.accountCode = account.AccCode;
            $scope.accountDescription = account.Description;
            $scope.accountOwner = account.AccountOwner;
            $scope.isAccountInactive = !account.Availability;
            //alert(account.Description);
        }

        function loadActiveEmployees() {
            //api/TeamMembers
            $http.get('api/TeamMembers').success(function (data) {
                $scope.employees = data;

            }).error(function () {
                $scope.error = "An Error has occured while loading posts!";
            });
        }

        $scope.count = 1;
        $scope.createNewProject = function () {
            //$scope.newProjects = [];
            $scope.newProjects.push($scope.count);
            $scope.count++;
        }

        $scope.activityChanged = function () {
            var checkboxes = $("#accountCheck");
            if (checkboxes.is(":checked")) {
                $scope.inactiveAccounts = $scope.tempInactiveAccounts;

            } else {
                $scope.activeAccounts = null;
                $scope.inactiveAccounts = null;
                $scope.activeAccounts = $scope.tempActiveAccounts;
            }
            //alert(checkboxes.is(":checked"));
        }

        $scope.createAccountClick = function () {
            $scope.createNewAccount = true;
            $scope.accountDisabled = true;
            clearFileds();
        }

        function clearFileds() {
            $scope.accountName = "";
            $scope.accountOwner = 0;
            $scope.accountDescription = "";
            $scope.accountCode = "";
            $scope.projects = [];
            $scope.newProjects = [];
            $scope.clickedButton = 0;
        }

        function addNewAccount(account) {
            //api/Account/add
            $http.post('api/Account/add', account).success(function (data) {
                for (var x = 0; x < $scope.newProjects.length; x++) {

                    addNewProject($scope.createdProjects[$scope.newProjects[x]], 0);
                }
                clearFileds();
                loadactiveAccounts();
                alert('New Account Created Successfully ');


                //for (var x = 0; x < $scope.newProjects.length; x++) {

                //    addNewProject($scope.createdProjects[$scope.newProjects[x]], 0);
                //}
                
            }).error(function () {
                $scope.error = "An Error has occured while loading posts!";
            });
        }

        function addNewProject(projectName, accountNo) {
            //api/Project/add
            console.log("Got " + projectName);
            var sendingValue = JSON.stringify(projectName + ":" + accountNo);
            $http.post('api/Project/add', sendingValue).success(function (data) {
                //alert(projectName + " " + accountNo);
                //clearFileds();
                //loadactiveAccounts();
                

            }).error(function () {
                $scope.error = "An Error has occured while loading posts!";
            });
        }

        $scope.changedProjectsEnabilities = [];
        $scope.setselectedProjectAvailability = function (projectId) {
            var checkboxes = $("#selectedProjectCheck");
            $scope.changedProjectsEnabilities.push(projectId + "  " + $scope.projectCheck[projectId]);
            //console.log(projectId+"  "+$scope.projectCheck[projectId]);
        }

        $scope.saveAccount = function () {
            var checkboxes = $("#selectedAccountCheck");
            // alert($scope.val);
            
        if ($scope.createNewAccount) {
                //create a new account
                var newAccount = { 'Id': 0, 'AccountName': $scope.accountName, 'AccCode': $scope.accountCode, 'Availability': !checkboxes.is(":checked"), 'AccountOwner': $scope.accountOwner, 'Description': $scope.accountDescription };
                addNewAccount(newAccount);
                



            } else {
                for (var x = 0; x < $scope.newProjects.length; x++) {

                    addNewProject($scope.createdProjects[$scope.newProjects[x]], $scope.selectedAccount);
                }
                if ($scope.createdProjects.length != 0) {
                    alert('New Projects Created Successfully');
                    //clearFileds();
                    //loadactiveAccounts();
                }
            }
        }
    }
})();