(function () {
    'use strict';
    //create angularjs controller

    app.controller('ProjectController', ['$scope', 'toaster', '$mdDialog', '$rootScope', '$http', '$window','$timeout', projectController]);
    app.directive('loading', function () {
        return {
            restrict: 'E',
            replace: true,
            template: '<div class="loading" style="display:block"><img src="http://www.nasa.gov/multimedia/videogallery/ajax-loader.gif" width="20px" height="20px" />LOADING...</div>',
            link: function (scope, element, attr) {
                scope.$watch('loading', function (val) {
                    if (val)
                        $(element).show();
                    else
                        $(element).hide();
                });
            }
        }
    });
    //angularjs controller method


    function projectController($scope, toaster, $mdDialog, $rootScope, $http, $window, $timeout) {
        //$scope.myclass = 'info';
        $scope.specLevel = "";
        //$scope.divWidth="50%";
        //$scope.specShow = false;
        $scope.pending = "Pending";
        $scope.teamClass = 'info';
        $scope.customerSatClass = 'info';
        $scope.processComClass = 'info';
        $scope.qualityClass = 'info';
        $scope.riskClass = 'info';
        $scope.financialClass = 'info';
        $scope.teamSat = "";
        $scope.specConfigLink;
        $scope.qval = "No Information Found";
        $scope.fval = "No Information Found";
        $scope.rval = "No Information Found";
        $scope.loggedInUserId = 0;
        $scope.isAdmin = false;
        $scope.images = [];
        $scope.images1 = [];
        //$scope.specShow = true;
        $scope.specHover = [["Level 0 / 1", "Low", 'teamlev0'], ["Level 2", "Medium", 'teamlev1'], ["Level 3", "Good", 'teamlev2'], ["Level 3", "Excellent"]];
        $scope.teamSatHover = [["0 - 4", "Low", 'teamlev0'], ["5 - 7", "Medium", 'teamlev1'], ["8 - 10", "Good", 'teamlev2'], ["Level 3", "Excellent"]];
        $scope.cusSatHover = [["0 - 2", "Low", 'teamlev0'], ["3 - 4", "Medium", 'teamlev1'], [" 4 Above", "Good", 'teamlev2'], ["Level 3", "Excellent"]];
        $scope.processComplianceHover = [["Above 0.75", "Low", 'teamlev0'], ["0.5 – 0.75", "Medium", 'teamlev1'], ["0 – 0.5", "Good", 'teamlev2'], ["Level 3", "Excellent"]];
        $scope.financialHover = [["0% - 79%", "Low", 'teamlev0'], ["80% - 99%", "Medium", 'teamlev1'], ["100%", "Good", 'teamlev2'], ["Level 3", "Excellent"]];
        $scope.riskHover = [["4 - 12", "Low", 'teamlev0'], ["1 - 3", "Medium", 'teamlev1'], ["0", "Good", 'teamlev2']];
        //$scope.teamShow = false;
        $scope.myInterval = 3000;
        //$scope.slides = [];
        $window.onload = function () {
            
            $scope.login = function () {
                spinnerService.show('html5spinner');
                $timeout(function () {
                    spinnerService.hide('html5spinner');
                    $scope.loggedIn = true;
                }, 2500);
            };
            
           // alert("Called on page load..");
        };
        
        $scope.riskDataVales = [];
        $scope.loading1 = true;
        

        $scope.slides = [1,2,3,4,5];

        $scope.labels = ["", ""];
        $scope.dougnutData = [70, 30];
        $scope.dougnutData1 = [10,90];

        $scope.colors = ['#FFA500', '#FF7F50'];

        $scope.lowColour = ['#e22626', '#e47474'];
        $scope.mediumColour = ['#FD9C34', '#FDC52A'];
        $scope.highColour = ['#4edab3', '#8accb9'];

        $scope.fincolors = ['#4edab3', '#78eccc'];
        $scope.options = {
            percentageInnerCutout: 80,
            animation: true
            //onAnimationComplete: function () {
            //    this.showTooltip(this.segments, true);
            //},
        };
       // var c = $('#myChart');
       // var ct = c.get(0).getContext('2d');
       // var ctx = document.getElementById("myChart").getContext("2d");

       //var myNewChart = new Chart(ct).Doughnut($scope.dougnutData, $scope.options);
        //Initialize specLink which is stored in the web.config

        function Initializer() {
            $http.get('api/AppSettings/getSpecLink').success(function (data) {
                $scope.specConfigLink = data;
            })

         .error(function () {
             $scope.error = "An Error has occured while loading posts!";
         });

        }


        //Account combo changing its items

        $scope.proNameChange = function (key1) {
            $scope.riskDataVales = [];
            $scope.images = [];
            $scope.images1 = [];
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


        
        isAuthorized();
        
        // Initialize widgets
        function loadStart() {
            Initializer();
            $scope.myVal = $scope.key1;
            isAdminOrTeamLead();
            $scope.teamLink = '#/teamSatisfaction/' + $scope.myVal;
            $scope.customerSatisfactionLink = '#/customerSatisfaction/' + $scope.myVal;
            $scope.processComLink = "#/processCompliance/" + $scope.myVal;
            $scope.timeReportLink = "#/financialStatus/" + $scope.myVal;;
            $scope.selectedProject = null;
            //localStorage.setItem('account', JSON.stringify($scope.myVal));
            //$scope.key1 = localStorage.getItem('account');
            $scope.teamClick = function () {

            };
            angular.isUndefinedOrNull = function (val) {
                return angular.isUndefined(val) || val === null
            }

            //Load Spec widget data
            $http.get('api/Spec/getSpecLevel/' + $scope.myVal).success(function (data) {
                $scope.specLevelData = data;
                console.log("Spec "+data[0]+" "+data[1]);

                if ($scope.specLevelData[0] != -1 & $scope.specLevelData[1] != -1) {
                    $scope.specShow = true;
                    $scope.title = $scope.specLevelData[1];
                    $scope.specLevel = $scope.specLevelData[0];
                    $scope.pending = "Pending  " + 3;


                    if ($scope.specLevelData[0] == 0 || $scope.specLevelData[0] == 1) {
                        $scope.myclass = 'teamlev0';
                    } else if ($scope.specLevelData[0] == 2) {
                        $scope.myclass = 'teamlev1';
                    } else {
                        $scope.myclass = 'teamlev2';
                    }

                   $scope.specLink = $scope.specConfigLink + $scope.specLevelData[1];

                    $scope.redirectTo = function () {
                        if ($scope.specLink != '/#') {
                            $window.open($scope.specLink, '_blank');
                        }
                    };

                } else {
                    $scope.specShow = false;
                    //$scope.specLevel = "No Information Found";
                    //$scope.pending = "";
                    //$scope.myclass = 'info';
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

            $scope.customerSatClick = function (projectId,year,quarter) {
                $window.location.href = $scope.customerSatisfactionLink + '/' + projectId + '/' + year + '/' + quarter;
            };

            $scope.processComplianceClick = function (projectId,year,quarter) {
                $window.location.href = $scope.processComLink + '/' + projectId+'/'+year+'/'+quarter;
            }

            $scope.financialStatusClick = function () {
                $window.location.href = $scope.timeReportLink;
            }

            $scope.riskWidgetClick = function (riskUrl) {
                if (riskUrl != null) {
                    $window.open(riskUrl, '_blank');
                }
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
                    $scope.teamShow = true;
                    $scope.teamSat = $scope.selectedSummary.Rating.toFixed(2);
                    $scope.teamDuration = $scope.selectedSummary.Year + " - Q" + $scope.selectedSummary.Quarter;
                    $scope.teamSatChartData = [$scope.teamSat, (10 - $scope.teamSat).toFixed(2)];

                    console.log($scope.selectedSummary.Year + " " + $scope.selectedSummary.Quarter);

                    if ($scope.selectedSummary.Rating < 5 & $scope.selectedSummary.Rating >= 0) {
                        $scope.teamClass = 'teamlev0';
                        $scope.teamChartColors = $scope.lowColour;
                    } else if ($scope.selectedSummary.Rating < 8 & $scope.selectedSummary.Rating >= 5) {
                        $scope.teamClass = 'teamlev1';
                        $scope.teamChartColors = $scope.mediumColour;
                    } else if ($scope.selectedSummary.Rating <= 10 & $scope.selectedSummary.Rating >= 8) {
                        $scope.teamClass = 'teamlev2';
                        $scope.teamChartColors = $scope.highColour;
                    } else {
                        $scope.teamClass = 'info';
                    }
                } else {
                    $scope.teamShow = false;
                    $scope.teamLink = '#/';
                    $scope.teamSat = "No Information Found";
                    $scope.teamDuration = "";
                    $scope.teamClass = 'info';
                }


            })

        .error(function () {
            $scope.error = "An Error has occured while loading posts!";

        });
            //loadCustomerSatisfactionWidgetDetails();
            loadCusWidgetData();

            //loadProcessComplianceWidgetDetails();
            loadProcessComWidgetData();

            loadFinancialWidgetDetails();

            loadRiskManagementWidgetDetails();
        };

        function loadCusWidgetData() {
            //getCusSatWidgetData
            $scope.cusSatDataValue = [];
            $http.get('api/CustomerSatisfaction/getCusSatWidgetData/' + $scope.myVal).success(function (data) {
                $scope.cusSatWidgetData = data;
                
                console.log(data);
                
                if (data.length != 0) {
                    $scope.customerSatShow = true;
                    angular.forEach(data, function (element, key) {
                        var tempReturnArray = ['teamlev2', element];
                        if (element.rating < 3 & element.rating >= 0) {
                            tempReturnArray[0] = 'teamlev0';
                        } else if (element.rating < 4 & element.rating >= 3) {
                            tempReturnArray[0] = 'teamlev1';
                        } else if (element.rating <= 5 & element.rating >= 4) {
                            tempReturnArray[0] = 'teamlev2';
                        }
                        $scope.cusSatDataValue.push(tempReturnArray);

                    })

                    if ($scope.cusVal < 3 & $scope.cusVal >= 0) {
                        $scope.customerSatClass = 'teamlev0';
                        $scope.customerSatColors = $scope.lowColour;
                    } else if ($scope.cusVal < 4 & $scope.cusVal >= 3) {
                        $scope.customerSatClass = 'teamlev1';
                        $scope.customerSatColors = $scope.mediumColour;
                    } else if ($scope.cusVal <= 5 & $scope.cusVal >= 4) {
                        $scope.customerSatClass = 'teamlev2';
                        $scope.customerSatColors = $scope.highColour;
                    } else {
                        $scope.customerSatClass = 'info';
                    }


                } else {
                    $scope.customerSatShow = false;
                    $scope.customerSatClass = 'info';
                    $scope.cusVal = "No Information Found";
                    $scope.customerSatisfactionLink = '#/';
                    $scope.cusYear = "";
                    $scope.cusDuration = "";
                    $scope.cusPending = "";
                }
                $scope.images = $scope.cusSatDataValue;
                $scope.cusDivVal = 100 / $scope.cusSatDataValue.length;
                $scope.cusDivWidth = $scope.cusDivVal + "%";
            })
            .error(function () {
            })
        }

        //Load customersatisfaction Widget data
        function loadCustomerSatisfactionWidgetDetails() {
            $scope.cusSatData = "";

            $http.get('api/CustomerSatisfaction/getCustomerSatisfactionWidgetDetails/' + $scope.myVal).success(function (data) {
                $scope.cusSatData = data;

                if ($scope.cusSatData[2]) {
                    $scope.customerSatShow = true;
                    $scope.cusVal = $scope.cusSatData[2].toFixed(2);
                    
                    $scope.customerSatData = [$scope.cusVal, (5 - $scope.cusVal).toFixed(2)];
                    $scope.cusYear = $scope.cusSatData[0];
                    $scope.cusQuarter = $scope.cusSatData[1];
                    $scope.cusDuration = $scope.cusYear + " - Q" + $scope.cusQuarter;
                    $scope.cusPending = $scope.cusSatData[3];
                    if ($scope.cusVal < 3 & $scope.cusVal >= 0) {
                        $scope.customerSatClass = 'teamlev0';
                        $scope.customerSatColors = $scope.lowColour;
                    } else if ($scope.cusVal < 4 & $scope.cusVal >= 3) {
                        $scope.customerSatClass = 'teamlev1';
                        $scope.customerSatColors = $scope.mediumColour;
                    } else if ($scope.cusVal <= 5 & $scope.cusVal >= 4) {
                        $scope.customerSatClass = 'teamlev2';
                        $scope.customerSatColors = $scope.highColour;
                    } else {
                        $scope.customerSatClass = 'info';
                    }

                } else {
                    $scope.customerSatShow = false;
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
            $scope.riskDataVales = [];
            $scope.riskData = null;
            $scope.loading1 = true;
            $scope.riskShow = true;
            
            $http.get('api/RiskManagement/getTotalRisksForSelectedAccountSubProjects/' + $scope.myVal).success(function (data) {
                $scope.riskDataVales = [];
                $scope.riskData = null;
                console.log(data);
                
                if (data.length != 0) {
                    
                    $scope.riskData = data;
                    
                    angular.forEach(data, function (image, key) {
                        var tempReturnArray = ['teamlev2', image];
                        if (image.riskValue <= 12 & image.riskValue >= 4) {
                            tempReturnArray[0] = 'teamlev0';
                        } else if (image.riskValue < 4 & image.riskValue >= 1) {
                            tempReturnArray[0] = 'teamlev1';
                        } else {
                            tempReturnArray[0] = 'teamlev2';
                        }
                        $scope.riskDataVales.push(tempReturnArray);
                        $scope.riskShow = true;
                    })
                } else {
                    $scope.riskDataVales = [];
                    $scope.riskShow = false;
                    //$scope.riskDataVales.push(null);
                }
                $scope.riskDivVal = 100 / $scope.riskDataVales.length;
                $scope.riskDivWidth = $scope.riskDivVal + "%";
                console.log("RISKVALUE    " + $scope.riskDivWidth);
                $scope.loading1 = false;
                
            })
            .error(function () {

            });
        }

        //Load processcompliance widget data
        function loadProcessComWidgetData(){
            $scope.processComplianceDataVal = [];
            $scope.images1 = [];
            $http.get('api/ProcessCompliance/getProcessComplianceWidgetDetails/' + $scope.myVal).success(function (data) {
                $scope.processComplianceWidgetData = data;
                //$scope.images1 = $scope.processComplianceDataVal;
                if (data.length != 0) {
                    $scope.processComplianceShow = true;
                    angular.forEach(data, function (element, key) {
                        var tempReturnArray = ['teamlev2', element];
                        if (element.Rating >=0.75) {
                            tempReturnArray[0] = 'teamlev0';
                        } else if (element.Rating < 0.75 & element.Rating >= 0.5) {
                            tempReturnArray[0] = 'teamlev1';
                        } else if (element.Rating <= 0.5) {
                            tempReturnArray[0] = 'teamlev2';
                        }
                        $scope.processComplianceDataVal.push(tempReturnArray);
                        $scope.images1.push(tempReturnArray);

                    })
                    $scope.divVal = 100 / $scope.images1.length;
                    if ($scope.images1.length != 1) {
                        $scope.mainDevWidth = (387 * $scope.images1.length) + "px";
                    } else {
                        $scope.mainDevWidth = "100% !important";
                    }
                    $scope.divWidth = $scope.divVal + "%";

                } else {
                    $scope.processComplianceShow = false;
                    
                }
                
            })
            .error(function () {

            });
        }






        function loadProcessComplianceWidgetDetails() {
            $scope.processComData = "";
            $http.get('api/ProcessCompliance/getProcessComplianceWidgetDetails/' + $scope.myVal).success(function (data) {
                $scope.processComData = data;
                console.log(data + " Proce  " + ($scope.processComData != null));
                if ($scope.processComData != null && (angular.toJson($scope.processComData[0]) != 'null')) {
                    $scope.processComClass = 'processVal';
                    $scope.processComplianceShow = true;
                    $scope.processComVal = $scope.processComData[2].toFixed(2);
                    $scope.processComYear = $scope.processComData[0];
                    $scope.processComQuarter = $scope.processComData[1];
                    $scope.processComDuration = $scope.processComYear + " - Q" + $scope.processComQuarter;
                    $scope.processComPending = $scope.processComData[3];

                    if ($scope.processComVal >= 0.75) {
                        $scope.processComClass = 'teamlev0';
                        $scope.processClass = 'teamlev0';
                    } else if ($scope.processComVal < 0.75 & $scope.processComVal >= 0.5) {
                        $scope.processComClass = 'teamlev1';
                        $scope.processClass = 'teamlev2';
                    } else if ($scope.processComVal < 0.5) {
                        $scope.processComClass = 'teamlev2';
                        $scope.processClass = 'teamlev2';
                    } else {
                        $scope.processComClass = 'info';
                    }

                } else {
                    $scope.processComplianceShow = false;
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
                    $scope.financialSatShow = true;
                    $scope.fval = data.coveredBillableHours + "/" + data.ExpectedHours;
                    $scope.fDuration = data.Year + " - Q" + data.Quarter + " - " + data.MonthName;
                    var percentageOfBillableHours = ((data.coveredBillableHours * 100) / data.ExpectedHours).toFixed(2);
                    if (percentageOfBillableHours ==100) {
                        $scope.financialClass = 'teamlev2';
                        $scope.financialChartColors = $scope.highColour;
                    } else if (percentageOfBillableHours < 100 & percentageOfBillableHours >= 80) {
                        $scope.financialClass = 'teamlev1';
                        $scope.financialChartColors = $scope.mediumColour;
                    } else {
                        $scope.financialClass = 'teamlev0';
                        $scope.financialChartColors = $scope.lowColour;
                    }
                    $scope.billableHourPercentage = percentageOfBillableHours + " %";
                    
                    $scope.financialChartData = [percentageOfBillableHours,(100 - percentageOfBillableHours).toFixed(2), ];
                } else {
                    $scope.financialSatShow = false;
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

        function isAdminOrTeamLead() {
            //api/Authorization/getAdminOrTeamLeadRights/{accountId}
            $http.get("api/Authorization/getAdminOrTeamLeadRights/" + $scope.myVal).success(function (data) {
                $scope.isAdmin = data.split('-')[0].toLowerCase() == 'true';
                $scope.isTeamLead = data.split('-')[1].toLowerCase() == 'true';
                
            })
            .error(function () {

            });
        }


        $scope.loggedInUserId;
        function getEmployee(username) {
            $http.get('api/TeamMembers/' + username).success(function (data) {
                console.log(data);
                if (data != null | data != "") {
                    $scope.employee = data;
                    $scope.LoggedInUserName = "Seranet / "+data.MemberName;
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
                if (data.length == 0) {
                    $scope.specShow = false;
                    $scope.teamShow = false;
                    $scope.customerSatShow = false;
                    $scope.processComplianceShow = false;
                    $scope.financialSatShow = false;
                }
                $scope.projects = data;
                $scope.key1 =JSON.parse($scope.userAccounts[0][1]);
                console.log(localStorage.getItem('account') +"      LOCALSTORAGE");
                if (localStorage.getItem('account') != null) {
                    $scope.proId = JSON.parse(localStorage.getItem('account'));
                    $scope.key1 = JSON.parse(localStorage.getItem('account'));
                } else {
                    localStorage.setItem('account',$scope.key1);
                }
                loadStart();
                $scope.loading = false;
            })
                     .error(function () {
                         $scope.error = "An Error has occured while loading posts!";
                         $scope.loading = false;
                     });
        }
        //$scope.hoverValues = $scope.specHover;
        $scope.specHoverOver = function () {
            $scope.hoverValues = $scope.specHover;
        }
        
        $scope.specHoverLeave = function () {
            $scope.hoverValues = "";
            
        }

        $scope.setTooltip = function (e) {
            $("#slider-tooltip").css("left", 20);
        }

        $scope.teamSatHoverOver = function () {
            $scope.hoverValues = $scope.teamSatHover;
        }

        $scope.teamSatHoverLeave = function () {
            $scope.hoverValues = "";

        }

        $scope.cusSatHoverOver = function () {
            $scope.hoverValues = $scope.cusSatHover;
        }

        $scope.cusSatHoverLeave = function () {
            $scope.hoverValues = "";

        }

        $scope.financialHoverOver = function () {
            $scope.hoverValues = $scope.financialHover;
        }

        $scope.financialHoverLeave = function () {
            $scope.hoverValues = "";

        }

        $scope.processComHoverOver = function () {
            $scope.hoverValues = $scope.processComplianceHover;
        }

        $scope.processComHoverLeave = function () {
            $scope.hoverValues = "";

        }

        $scope.riskHoverOver = function () {
            $scope.hoverValues = $scope.riskHover;
        }

        $scope.riskHoverLave = function () {
            $scope.hoverValues = "";

        }
        


    }

})();
