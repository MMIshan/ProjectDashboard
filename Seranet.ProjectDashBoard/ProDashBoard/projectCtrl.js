(function () {
    'use strict';
    //create angularjs controller

    app.controller('ProjectController', ['$scope', 'toaster', '$mdDialog', '$rootScope', '$http', '$window', projectController]);

    //angularjs controller method


    function projectController($scope, toaster, $mdDialog, $rootScope, $http, $window) {
        $scope.myclass = 'info';
        $scope.specLevel = "Level ";
        $scope.pending = "Pending";
        $scope.teamClass = 'info';
        $scope.customerSatClass = 'info';
        $scope.processComClass = 'info';
        $scope.qualityClass = 'info';
        // $scope.riskClass = 'info';
        $scope.financialClass = 'info';
        $scope.teamSat = "";
        $scope.specConfigLink;
        $scope.qval = "No Information Found";
        $scope.fval = "No Information Found";
        $scope.rval = "No Information Found";
        $scope.loggedInUserId = 0;
        $scope.images = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9];
        slider();
        $scope.dataArray = [
      {
          src: 'https://www.travelexcellence.com/images/movil/La_Paz_Waterfall.jpg'
      },
      {
          src: 'http://www.parasholidays.in/blog/wp-content/uploads/2014/05/holiday-tour-packages-for-usa.jpg'
      },
      {
          src: 'http://clickker.in/wp-content/uploads/2016/03/new-zealand-fy-8-1-Copy.jpg'
      },
      {
          src: 'http://images.kuoni.co.uk/73/indonesia-34834203-1451484722-ImageGalleryLightbox.jpg'
      },
      {
          src: 'http://www.holidaysaga.com/wp-content/uploads/2014/09/Day08-SIN-day-Free-City-View.jpg'
      },
      {
          src: 'http://images.kuoni.co.uk/73/malaysia-21747826-1446726337-ImageGalleryLightbox.jpg'
      },
      {
          src: 'http://www.kimcambodiadriver.com/uploads/images/tours/kim-cambodia-driver-angkor-wat.jpg'
      },
      {
          src: 'https://www.travcoa.com/sites/default/files/styles/flexslider_full/public/tours/images/imperialvietnam-halong-bay-14214576.jpg?itok=O-q1yr5_'
      }
        ];
        function slider() {
            
        }

        //Initialize specLink which is stored in the web.config

        function Initializer() {
            $http.get('api/AppSettings/getSpecLink').success(function (data) {
                $scope.specConfigLink = data.split('"')[1];
            })

         .error(function () {
             $scope.error = "An Error has occured while loading posts!";
         });

        }

        //Account combo changing its items

        $scope.proNameChange = function (key1) {
            loadStart();
        }

        //$http.get("http://99xt.lk/services/api/Employees", { withCredentials: true }).success(function (data) {
        //    alert("Login ");
        //    getData();
        //    console.log(data[0].id);
        //}).
        //        error(function (data, status, headers, config) {
        //            console.log(data);
        //        });


        Initializer();
        isAuthorized();

        // Initialize widgets
        function loadStart() {
            $scope.myVal = $scope.key1;
            $scope.teamLink = '#/home/' + $scope.myVal;
            $scope.customerSatisfactionLink = '#/customerSatisfaction/' + $scope.myVal;
            $scope.processComLink = "#/processCompliance/" + $scope.myVal;
            $scope.timeReportLink = "#/financialStatus/" + $scope.myVal;;
            $scope.selectedProject = null;
            localStorage.setItem('account', JSON.stringify($scope.myVal));

            $scope.teamClick = function () {

            };
            angular.isUndefinedOrNull = function (val) {
                return angular.isUndefined(val) || val === null
            }

            //Load Spec widget data
            $http.get('api/Project/getSpec/' + $scope.myVal).success(function (data) {
                $scope.selectedProject = data;


                if (angular.isObject($scope.selectedProject)) {

                    $scope.title = $scope.selectedProject.linkId;
                    $scope.specLevel = "Level " + $scope.selectedProject.SpecLevel;
                    $scope.pending = "Pending  " + $scope.selectedProject.PendingCount;


                    if ($scope.selectedProject.SpecLevel == 0 || $scope.selectedProject.SpecLevel == 1) {
                        $scope.myclass = 'teamlev0';
                    } else if ($scope.selectedProject.SpecLevel == 2) {
                        $scope.myclass = 'teamlev1';
                    } else {
                        $scope.myclass = 'teamlev2';
                    }

                    $scope.specLink = $scope.specConfigLink + $scope.selectedProject.linkId;

                    $scope.redirectTo = function () {
                        if ($scope.specLink != '/#') {
                            $window.open($scope.specLink, '_blank');
                        }
                    };

                } else {
                    $scope.specLevel = "No Information Found";
                    $scope.pending = "";
                    $scope.myclass = 'info';
                    $scope.specLink = "/#";
                }


            })

        .error(function () {
            $scope.error = "An Error has occured while loading posts!";

        });

            //Declare widgets click actions (assign valid urls to redirect to inside pages)
            $scope.teamClick = function () {

                $window.location.href = $scope.teamLink;
            };

            $scope.customerSatClick = function () {
                $window.location.href = $scope.customerSatisfactionLink;
            };

            $scope.processComplianceClick = function () {
                $window.location.href = $scope.processComLink;
            }

            $scope.financialStatusClick = function () {
                $window.location.href = $scope.timeReportLink;
            }

            //Load accounts - according to the logginuser's restrictions
            $http.get('api/Account/' + $scope.myVal).success(function (data) {
                $scope.selectedPro = data;

                $scope.projectName = $scope.selectedPro.AccountName;
                console.log("acc " + $scope.projectName);
                $rootScope.project = $scope.projectName;

            })

        .error(function () {
            $scope.error = "An Error has occured while loading posts!";

        });

            // $scope.specLink = "http://99xt.lk/spec/#/project/" + $scope.Selectedproject.projectId;
            $scope.loading = false;

            //Load TeamSatisfaction widget data
            $http.get('api/Summary/getLatestProjectSummaries/' + $scope.myVal + '/' + 0 + '/' + 0).success(function (data) {
                $scope.selectedSummary = data;
                if (angular.isObject($scope.selectedSummary)) {
                    $scope.teamSat = $scope.selectedSummary.Rating;
                    $scope.teamDuration = $scope.selectedSummary.Year + " - Q" + $scope.selectedSummary.Quarter;
                    console.log($scope.selectedSummary.Year + " " + $scope.selectedSummary.Quarter);

                    if ($scope.selectedSummary.Rating <= 4.4 & $scope.selectedSummary.Rating >= 0) {
                        $scope.teamClass = 'teamlev0';
                    } else if ($scope.selectedSummary.Rating <= 7.4 & $scope.selectedSummary.Rating >= 4.5) {
                        $scope.teamClass = 'teamlev1';
                    } else if ($scope.selectedSummary.Rating <= 10 & $scope.selectedSummary.Rating >= 7.5) {
                        $scope.teamClass = 'teamlev2';
                    } else {
                        $scope.teamClass = 'info';
                    }
                } else {

                    $scope.teamLink = '#/';
                    $scope.teamSat = "No Information Found";
                    $scope.teamDuration = "";
                    $scope.teamClass = 'info';
                }


            })

        .error(function () {
            $scope.error = "An Error has occured while loading posts!";

        });
            loadCustomerSatisfactionWidgetDetails();

            loadProcessComplianceWidgetDetails();

            loadFinancialWidgetDetails();

            //loadRiskManagementWidgetDetails();
        };

        //Load customersatisfaction Widget data
        function loadCustomerSatisfactionWidgetDetails() {
            $scope.cusSatData = "";

            $http.get('api/CustomerSatisfaction/getCustomerSatisfactionWidgetDetails/' + $scope.myVal).success(function (data) {
                $scope.cusSatData = data;

                if ($scope.cusSatData[2]) {
                    $scope.cusVal = $scope.cusSatData[2];
                    $scope.cusYear = $scope.cusSatData[0];
                    $scope.cusQuarter = $scope.cusSatData[1];
                    $scope.cusDuration = $scope.cusYear + " - Q" + $scope.cusQuarter;
                    $scope.cusPending = "Pending Projects " + $scope.cusSatData[3];
                    if ($scope.cusVal < 3 & $scope.cusVal >= 0) {
                        $scope.customerSatClass = 'teamlev0';
                    } else if ($scope.cusVal < 4 & $scope.cusVal >= 3) {
                        $scope.customerSatClass = 'teamlev1';
                    } else if ($scope.cusVal <= 5 & $scope.cusVal >= 4) {
                        $scope.customerSatClass = 'teamlev2';
                    } else {
                        $scope.customerSatClass = 'info';
                    }

                } else {
                    $scope.customerSatClass = 'info';
                    $scope.cusVal = "No Information Found";
                    $scope.customerSatisfactionLink = '#/';
                    $scope.cusYear = "";
                    $scope.cusDuration = "";
                    $scope.cusPending = "";

                }
            })

        .error(function () {
            $scope.error = "An Error has occured while loading posts!";

        });
        }

        function loadRiskManagementWidgetDetails() {
            $http.get('api/RiskManagement/getTotalRisksForSelectedAccountSubProjects/' + $scope.myVal).success(function (data) {
                console.log(data);
                $scope.riskData = data;
                angular.forEach(data, function (value, key) {
                    $scope.images.push(value.riskValue);
                });
                
            })
            .error(function () {

            });
        }
        //Load processcompliance widget data
        function loadProcessComplianceWidgetDetails() {
            $scope.processComData = "";
            $http.get('api/ProcessCompliance/getProcessComplianceWidgetDetails/' + $scope.myVal).success(function (data) {
                $scope.processComData = data;
                //console.log($scope.processComData + " " + ($scope.processComData != null));
                if ($scope.processComData != null && $scope.processComData[3] != 0) {
                    $scope.processComClass = 'processVal';
                    $scope.processComVal = $scope.processComData[2];
                    $scope.processComYear = $scope.processComData[0];
                    $scope.processComQuarter = $scope.processComData[1];
                    $scope.processComDuration = $scope.processComYear + " - Q" + $scope.processComQuarter;
                    $scope.processComPending = "Pending Projects " + $scope.processComData[3];

                    if ($scope.processComVal < 0.3 & $scope.processComVal >= 0) {
                        $scope.processComClass = 'teamlev0';
                    } else if ($scope.processComVal <= 0.5 & $scope.processComVal >= 0.3) {
                        $scope.processComClass = 'teamlev1';
                    } else if ($scope.processComVal >= 0.5) {
                        $scope.processComClass = 'teamlev2';
                    } else {
                        $scope.processComClass = 'info';
                    }

                } else {
                    $scope.processComClass = 'info';
                    $scope.processComVal = "No Information Found";
                    $scope.processComLink = '#/';
                    $scope.processComYear = "";
                    $scope.processComDuration = "";
                    $scope.processComPending = "";

                }
            })

        .error(function () {
            $scope.error = "An Error has occured while loading posts!";

        });
        }

        function loadFinancialWidgetDetails() {
            //api/FinancialSummary/getFinalMonthSummary/{AccountId}/{Year}/{Quarter}
            $http.get('api/FinancialSummary/getFinalMonthSummary/' + $scope.myVal + '/0/0').success(function (data) {

                if (angular.isObject(data)) {
                    $scope.fval = data.ActualHours + " h / " + data.ExpectedHours + " h";
                    $scope.fDuration = data.Year + " - Q" + data.Quarter + " - " + data.MonthName;
                    var percentageOfBillableHours = ((data.ActualHours / data.ExpectedHours) * 100).toFixed(2);
                    if (percentageOfBillableHours > 95) {
                        $scope.financialClass = 'teamlev2';
                    } else if (percentageOfBillableHours <= 95 & percentageOfBillableHours > 85) {
                        $scope.financialClass = 'teamlev1';
                    } else {
                        $scope.financialClass = 'teamlev0';
                    }
                    $scope.billableHourPercentage = percentageOfBillableHours + " %";
                } else {
                    $scope.fval = "No Information Found";
                    $scope.financialClass = 'info';
                    $scope.fDuration = "";
                    $scope.timeReportLink = "#/";
                    $scope.billableHourPercentage = "";
                }

            })
            .error(function () {
                $scope.error = "An Error has occured while loading posts!";

            });
        }

        //Chek authorization
        function isAuthorized() {
            $http.get('api/Authorization').success(function (data) {
                getEmployee(data);
            })
        .error(function () {
            $scope.error = "An Error has occured while loading posts!";
            $scope.loading = false;
        });
        }


        $scope.loggedInUserId;
        function getEmployee(username) {
            $http.get('api/TeamMembers/' + username).success(function (data) {
                if (data != 'null') {
                    $scope.employee = data;
                    $scope.loggedInUserId = data.Id;
                    loadUserAccounts();
                }

            })
        .error(function () {
            $scope.error = "An Error has occured while loading posts!";
            $scope.loading = false;
        });
        }

        function loadUserAccounts() {
            $http.get('api/EmployeeProjects/getEmployeeAccounts/' + $scope.loggedInUserId).success(function (data) {
                $scope.userAccounts = data;
                $scope.projects = data;
                $scope.key1 = $scope.userAccounts[0][1];
                loadStart();
                $scope.loading = false;
            })
                     .error(function () {
                         $scope.error = "An Error has occured while loading posts!";
                         $scope.loading = false;
                     });
        }



    }

})();