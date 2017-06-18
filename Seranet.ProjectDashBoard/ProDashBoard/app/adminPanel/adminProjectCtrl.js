(function () {
    'use strict';
    //create angularjs controller

    app.controller('adminProjectController', ['$scope', 'toaster', '$mdDialog', '$http', '$window', adminProjectController]);

    //angularjs controller method
    function adminProjectController($scope,toaster, $mdDialog,  $http, $window) {
       
        loadActiveProjects();
        $scope.clickedButton = 0;
        $scope.employeeCombo = [];
        $scope.billableCheck = [];
        $scope.allocationField = [];
        $scope.leadCheck = [];
        $scope.fieldsEnability = [];
        $scope.availabilityCheck = [];

        $scope.newemployeeCombo = [];
        $scope.newbillableCheck = [];
        $scope.newallocationField = [];
        $scope.newleadCheck = [];
        $scope.newavailabilityCheck = [];
        $scope.billableArray = [[1,'Hourly Billable'],[2,'FullTime Billable'],[3,'Non Billable']];
                                                                                
        $scope.fieldsEnability1 = false;
        $scope.newProjects = [];
        $scope.searchKeyword = "";

        function loadInactiveProjects() {
            $http.get('api/Project/getAdminPanelInactiveProjects').success(function (data) {
                $scope.inactiveProjects = data;
            })
            .error(function () {
                $scope.error = "An Error has occured while loading posts!";
            });
        }

        function loadActiveProjects() {
            $http.get('api/Project/getAdminPanelActiveProjects').success(function (data) {
                console.log(data[0]);
                if (data.length != 0) {
                    loadClickedProjectData(data[0]);
                }
                $scope.activeProjects = data;
            })
            .error(function () {
                $scope.error = "An Error has occured while loading posts!";
            });
        }
        
        function loadEmployeesOfTheProject(projectId) {
            //api/EmployeeProjects/getEmployeesOfSelectedProject/{projectId}
            $http.get('api/EmployeeProjects/getEmployeesOfSelectedProject/'+projectId).success(function (data) {
                
                angular.forEach(data, function (value, key) {
                    $scope.fieldsEnability[value.Id] = true;
                });
                $scope.employeeData = data;
            })
            .error(function () {
                $scope.error = "An Error has occured while loading posts!";
            });
        }

        loadActiveEmployees();

        $scope.edit = function (empProject) {
            
            $scope.fieldsEnability[empProject.Id] = false;
            //$scope.fieldsEnability[1] = true;
        }

        function loadActiveEmployees() {
            $http.get('api/TeamMembers').success(function (data) {
                $scope.employees = data;
                
                
            }).error(function () {
                $scope.error = "An Error has occured while loading posts!";
            });
        }

        $scope.activityChanged = function () {
            loadActiveOrInactiveAccountsProjects();
        }

        $scope.projectClick = function (project) {
            $scope.newProjects = [];
            loadClickedProjectData(project);
        }

        $scope.tempProject;
        function loadClickedProjectData(project) {
            $scope.RiskPageUrl = "";
            $scope.clickedButton = project.Id;
            $scope.projectName = project.Name;
            $scope.projectCode = project.ProjetCode;
            $scope.productOwner = project.ProjectOwner;
            $scope.isProjectInactive = !project.Enabled;
            $scope.isProjectBillable = project.Billable;
            $scope.projectDescription = project.Description;
            
            $scope.tempProject = project;
            setAccountName(project.AccountId);
            loadEmployeesOfTheProject(project.Id);
            
            loadCommondata(project.Id);
        }

        function loadActiveOrInactiveAccountsProjects() {
            var checkboxes = $("#projectCheck");

            if (checkboxes.is(":checked")) {
                loadActiveProjects();
                loadInactiveProjects();
                // $scope.inactiveAccounts = $scope.tempInactiveAccounts;

            } else {
                loadActiveProjects();
                $scope.activeProjects = null;
                $scope.inactiveProjects = null;
                //$scope.activeAccounts = $scope.tempActiveAccounts;
            }
        }

        function setAccountName(id) {
            $http.get('api/Account/' + id).success(function (data) {
                if(data!=null){
                $scope.account = data.AccountName;
                }
            }).error(function () {
                $scope.error = "An Error has occured while loading posts!";
            });
        }

        function loadCommondata(projectId) {
            $scope.confluencePageId = "";
            $scope.wikiPageLink = "";
            $scope.riskUrl = "";
            $http.get('api/CommonData/' + projectId).success(function (data) {
                if (data != null) {
                    $scope.confluencePageId = data.ConfluencePageId;
                    $scope.wikiPageLink = data.WikiPageLink;
                    $scope.riskUrl=data.RiskPageUrl;
                }
            }).error(function () {
                $scope.error = "An Error has occured while loading posts!";
            });
        }
        var count = 1;
        $scope.addNewEmployee = function () {
            $scope.newProjects.push(count);
            count++;
        }

        $scope.saveProject = function () {
            //var isProjectInactive = $("#isProjectInactive");
            //var isProjectBillable = $("#isProjectBillable");
            //console.log("Inactive " + isProjectInactive.is(":checked") + " Billable" + isProjectBillable.is(":checked"));
            var project = { "Id": $scope.tempProject.Id, "AccountId": $scope.tempProject.AccountId, "Name": $scope.projectName, "ProjetCode": null, "Enabled": !$scope.isProjectInactive, "ProjectOwner": $scope.productOwner, "Description": $scope.projectDescription, "Billable": $scope.isProjectBillable };
            updateProject(project);
            var commonData = { "ProjectId": $scope.tempProject.Id, "WikiPageLink": $scope.wikiPageLink, "ConfluencePageId": $scope.confluencePageId, "RiskPageUrl" :$scope.riskUrl };
            addOrUpdateCommonData(commonData);
            $scope.empProjectsListToAdd = [];
            $scope.empProjectsListToUpdate = [];
            angular.forEach($scope.employeeData, function (value, key) {
                var employeeProject = { "Id": value.Id, "EmpId": $scope.employeeCombo[value.Id], "AccountId": value.AccountId, "ProjectId": value.ProjectId, "Availability": !$scope.availabilityCheck[value.Id],"BillableHours":$scope.allocationField[value.Id],"Lead":$scope.leadCheck[value.Id],"Billable":$scope.billableCheck[value.Id] };
                $scope.empProjectsListToUpdate.push(employeeProject);
                //alert("empId" + $scope.employeeCombo[value.Id] + " AccountId" + value.AccountId + " " + value.ProjectId + " " + !$scope.availabilityCheck[value.Id] + " " + $scope.allocationField[value.Id] + " " + $scope.leadCheck[value.Id] + " " + $scope.billableCheck[value.Id]);
            });
            updateEmployeeProjects($scope.empProjectsListToUpdate);
            angular.forEach($scope.newProjects, function (value, key) {
                //alert($scope.newbillableCheck[value]);
                var newEmployeeProject = { "Id": 0, "EmpId": $scope.newemployeeCombo[value], "AccountId": $scope.tempProject.AccountId, "ProjectId": $scope.tempProject.Id, "Availability": true, "BillableHours": $scope.newallocationField[value], "Lead": $scope.newleadCheck[value], "Billable": $scope.newbillableCheck[value] };
                $scope.empProjectsListToAdd.push(newEmployeeProject);
                //alert("empId" + $scope.newemployeeCombo[value] + " " + $scope.tempProject.AccountId + " " + $scope.tempProject.Id + " " + true + " " + $scope.newallocationField[value] + " " + $scope.newleadCheck[value] + " " + $scope.newbillableCheck[value]);
            });
            addNewEmployeeProjects($scope.empProjectsListToAdd);
        }

        function updateEmployeeProjects(empProjectsList) {
            $http.put('api/EmployeeProjects/update', empProjectsList).success(function (data) {
                loadActiveProjects();
                var checkboxes = $("#projectCheck");
                if (checkboxes.is(":checked")) {
                    loadInactiveProjects();

                } else {

                }

                loadActiveOrInactiveAccountsProjects();
            }).error(function () {
                $scope.error = "An Error has occured while loading posts!";
            });
            //api/EmployeeProjects/update
        }

        function addNewEmployeeProjects(empProjectsList) {
            $http.post('api/EmployeeProjects/add', empProjectsList).success(function (data) {
                toaster.pop('success', "Notificaton", "Project Updated Successfully");
                //alert("Updated Successfully");
            }).error(function () {
                $scope.error = "An Error has occured while loading posts!";
            });
            //api/EmployeeProjects/update
        }
        
        function updateProject(project) {
            //api/Project/updateFullProject
            $http.put('api/Project/updateFullProject', project).success(function (data) {
                //alert("Updated Successfully");
            }).error(function () {
                $scope.error = "An Error has occured while loading posts!";
            });
        }

        function addOrUpdateCommonData(commonData) {
            $http.put('api/CommonData/updateOrAdd', commonData).success(function (data) {
            
            }).error(function () {
                $scope.error = "An Error has occured while loading posts!";
            });
        }
    }
})();
