(function () {
    'use strict';
    //create angularjs controller

    app.controller('adminAccountController', ['$scope', 'toaster', '$mdDialog', '$http', '$window', adminAccountController]);

    //angularjs controller method
    function adminAccountController($scope, toaster, $mdDialog, $http, $window) {

        loadactiveAccounts();
        // loadInactiveAccounts();
        loadActiveEmployees();


        $scope.toCreateNewAccount = false;
        $scope.createNewAccount = false;
        $scope.isAccountInactive = false;
        $scope.accountDisabled = false;

        $scope.accountColorClass;
        $scope.clickedButton = 0;
        $scope.createdProjects = [];
        $scope.searchKeyword = "";
        isAdminOrTeamLead();
        $scope.projectCheck = [];
        function loadInactiveAccounts() {
            $http.get('api/Account/adminPanelInactiveAccounts').success(function (data) {
                $scope.tempInactiveAccounts = data;
                $scope.inactiveAccounts = data;
                //$scope.$apply();
            })
            .error(function () {
                $scope.error = "An Error has occured while loading posts!";
            });
        }

        function isAdminOrTeamLead() {
            $http.get("api/Authorization/getAdminOrTeamLeadRights/0").success(function (data) {
                $scope.isAdmin = data.split('-')[0].toLowerCase() == 'true';
                $scope.isTeamLead = data.split('-')[1].toLowerCase() == 'true';

            })
            .error(function () {

            });
        }

        function loadactiveAccounts() {
            $http.get('api/Account/adminPanelActiveAccounts').success(function (data) {
                $scope.activeAccounts = data;
                //$scope.$apply();
                if ($scope.activeAccounts.length != 0) {
                    loadClickedAccountData($scope.activeAccounts[0]);
                }
                $scope.activeAccounts = data;
                if (data.length > 0) {
                  $scope.accountOwner = $scope.activeAccounts[0].AccountOwner;
                  $scope.selectedEmpId = $scope.activeAccounts[0].AccountOwner;
                }
                $scope.tempActiveAccounts = data;

            }).error(function () {
                $scope.error = "An Error has occured while loading posts!";
            });
        }

        function loadSelectedAccountProjects(accountId) {
            $http.get('api/Project/getSelectedAdminAccountProjects/' + accountId).success(function (data) {
                $scope.projects = data;

            }).error(function () {
                $window.location.href = '#/error';
                $scope.error = "An Error has occured while loading posts!";
            });
        }
        $scope.newProjects = [];
        $scope.selectedAccount = 0;


        $scope.accountClick = function (account) {
            loadClickedAccountData(account);

        }

        function loadClickedAccountData(account) {
            $scope.projectCheck = [];
            $scope.changedProjectsEnabilities = [];
            loadSelectedAccountProjects(account.Id);
            $scope.newProjects = [];
            $scope.selectedAccount = account.Id;
            fillAccountData(account);
            $scope.createNewAccount = false;
            $scope.accountDisabled = false;
            $scope.accountColorClass = 'info';
            $scope.clickedButton = account.Id;
        }

        function fillAccountData(account) {
            $scope.isAccountInactive = false;
            $scope.accountName = account.AccountName;
            $scope.accountCode = account.AccCode;
            $scope.accountDescription = account.Description;
            $scope.accountOwner = account.AccountOwner;
            $scope.selectedEmpId = account.AccountOwner;
            $scope.isAccountInactive = !account.Availability;
            $scope.allProjectCodes = account.AllProjectCodes;

        }

        function loadActiveEmployees() {
            $http.get('api/TeamMembers').success(function (data) {
                $scope.employees = data;

            }).error(function () {
                $scope.error = "An Error has occured while loading posts!";
            });
        }

        $scope.count = 1;
        $scope.createNewProject = function () {
            $scope.newProjects.push($scope.count);
            $scope.count++;
        }

        $scope.activityChanged = function () {
            loadActiveOrInactiveAccounts();

        }

        function loadActiveOrInactiveAccounts() {
            var checkboxes = $("#accountCheck");
            if (checkboxes.is(":checked")) {
                loadactiveAccounts();
                loadInactiveAccounts();
                // $scope.inactiveAccounts = $scope.tempInactiveAccounts;

            } else {
                loadactiveAccounts();
                $scope.activeAccounts = null;
                $scope.inactiveAccounts = null;
                //$scope.activeAccounts = $scope.tempActiveAccounts;
            }
        }

        $scope.createAccountClick = function () {
            $scope.createNewAccount = true;
            $scope.accountDisabled = true;
            clearFileds();
        }

        function clearFileds() {
            $scope.accountName = "";
            if ($scope.employees.length!=0){
              $scope.accountOwner = $scope.employees[0].Id;
              $scope.selectedEmpId = $scope.employees[0].Id;
            }
            $scope.accountDescription = "";
            $scope.accountCode = "";
            $scope.allProjectCodes = "";
            $scope.projects = [];
            $scope.newProjects = [];
            $scope.clickedButton = 0;
        }

        function addNewAccount(account) {
            $http.post('api/Account/add', account).success(function (data) {
                for (var x = 0; x < $scope.newProjects.length; x++) {

                    addNewProject($scope.createdProjects[$scope.newProjects[x]], 0);
                }
                clearFileds();
                loadactiveAccounts();
                toaster.pop('success', "Notificaton", "New Account Created Successfully");
                //alert('New Account Created Successfully ');

            }).error(function () {
                $scope.error = "An Error has occured while loading posts!";
            });
        }

        function addNewProject(projectName, accountNo) {

            var sendingValue = JSON.stringify(projectName + ":" + accountNo);
            $http.post('api/Project/add', sendingValue).success(function (data) {

            }).error(function () {
                $scope.error = "An Error has occured while loading posts!";
            });
        }

        //$scope.changedProjectsEnabilities = [];
        //$scope.setselectedProjectAvailability = function (projectId) {

        //    var checkboxes = $("#selectedProjectCheck");
        //    $scope.changedProjectsEnabilities.push(projectId + "  " + $scope.projectCheck[projectId]);

        //}

        function EditData() {
            var checkboxes = $("#selectedAccountCheck");
            var newAccount = { 'Id': $scope.selectedAccount, 'AccountName': $scope.accountName, 'AccCode': $scope.accountCode, 'Availability': !$scope.isAccountInactive, 'AccountOwner': $scope.accountOwner, 'Description': $scope.accountDescription, 'AllProjectCodes': $scope.allProjectCodes };
            console.log(newAccount);


            //Update Account Details
            $http.put('api/Account/update', newAccount).success(function (data) {

                loadactiveAccounts();
                var checkboxes = $("#accountCheck");
                if (checkboxes.is(":checked")) {
                    loadInactiveAccounts();

                } else {

                }

                loadActiveOrInactiveAccounts();
            }).error(function () {
                $scope.error = "An Error has occured while loading posts!";
            });

            //Update Project Details
            for (var x = 0; x < $scope.projects.length; x++) {

                if ($scope.projectCheck[$scope.projects[x].Id] != null) {

                    var newProject = { Id: $scope.projects[x].Id, AccountId: 0, Name: null, ProjetCode: null, Enabled: !$scope.projectCheck[$scope.projects[x].Id] };

                    $http.put('api/Project/update', newProject).success(function (data) {

                    }).error(function () {
                        $scope.error = "An Error has occured while loading posts!";
                    });
                }
            }

        }

        $scope.saveAccount = function () {

            var checkboxes = $("#selectedAccountCheck");

            if ($scope.createNewAccount) {
                //create a new account
                var newAccount = { 'Id': 0, 'AccountName': $scope.accountName, 'AccCode': $scope.accountCode, 'Availability': !checkboxes.is(":checked"), 'AccountOwner': $scope.accountOwner, 'Description': $scope.accountDescription, 'AllProjectCodes': $scope.allProjectCodes };
                addNewAccount(newAccount);

            } else {
                for (var x = 0; x < $scope.newProjects.length; x++) {

                    addNewProject($scope.createdProjects[$scope.newProjects[x]], $scope.selectedAccount);
                }

                EditData();
                toaster.pop('success', "Notificaton", "Account Updated Successfully");
                //loadactiveAccounts();
                //alert('Account Updated Successfully');

            }
        }
    }
})();
