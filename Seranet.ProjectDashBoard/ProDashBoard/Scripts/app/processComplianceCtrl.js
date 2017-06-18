(function () {
    'use strict';
    //create angularjs controller
    app.controller('processComplianceController', ['$scope', '$window', '$http', '$stateParams', ProcessCompliance]);

    //angularjs controller method
    function ProcessCompliance($scope, $window, $http, $stateParams) {
        $scope.accountId = $stateParams.id;
        $scope.yearArray = [];
        loadsubProjects();
        Initializer();
        $scope.ProcessComplianceConfigLink;
        var currentYear = new Date().getFullYear();
        for (var k = 2008; k <= currentYear; k++) {
            $scope.yearArray.push(k);
        }

        $scope.options = {
            legend: {
                display: true
            },
            responsive: true,
            datasetFill: false,
            scaleBeginAtZero: true,

        };

        //Prepare data to the table rows according to the structure (two by two)
        function chunk(arr) {
            var newArr = [];
            var tempArray = [];
            for (var i = 1; i <= arr.length; i++) {
                tempArray.push(arr[i - 1]);
                if (i % 2 == 0) {
                    newArr.push(tempArray);
                    tempArray = [];
                }

                if (i == arr.length & i % 2 != 0) {
                    newArr.push(tempArray);
                }
            }
            return newArr;
        }

        //Load results according to the input paramatesr (fill trable)
        function loadSelectedProjectResults(accountId, projectId, year, quarter) {
            $http.get('api/ProcessCompliance/getSelectedProjectResults/' + accountId + '/' + projectId + '/' + year + '/' + quarter).success(function (data) {
                $scope.Results = data;
                $scope.ChangedResults = chunk(data);
                console.log("ch " + chunk(data)[1] + " " + chunk(data)[3] + " " + chunk(data)[0]);
                if (data != '') {

                    setInteractions($scope.Results[0].Year, $scope.Results[0].Quarter);
                    $scope.chunkedData = chunk($scope.Results, 3);
                    console.log("Chunk " + $scope.chunkedData);
                    getSelectedProjectDurationSummary(accountId, projectId, year, quarter);
                } else {
                    $scope.Results = "";
                }
            })
                .error(function () {
                    $scope.error = "An Error has occured while loading posts!";
                });
        }



        //Fill subprojectCombo
        function loadsubProjects() {
            $http.get('api/Project/getSelectedAccountProjects/' + $scope.accountId).success(function (data) {
                $scope.projects = data;
                console.log(data[0].Id);

                $scope.projectCombo = angular.toJson(data[0]);
                $scope.subProjectName = data[0].Name;
                loadProjectSummary();
                loadCommonData(data[0].Id);
            })
                .error(function () {
                    $scope.error = "An Error has occured while loading posts!";
                });
        }

        //Change selected quarter colour
        function setInteractions(year, quarter) {
            $scope.yearCombo = year;
            if (quarter == 1) {
                $scope.quarter1 = 'q1';
                $scope.quarter2 = 'qnone';
            } else {
                $scope.quarter2 = 'q1';
                $scope.quarter1 = 'qnone';
            }
        }

        //Declare quarter button click evets
        $scope.q1Click = function () {
            $scope.quarter1 = 'q1';
            $scope.quarter2 = 'qnone';
            var receivedProject = JSON.parse($scope.projectCombo);
            loadSelectedProjectResults($scope.accountId, receivedProject.Id, $scope.yearCombo, 1);
        }

        $scope.q2Click = function () {
            $scope.quarter2 = 'q1';
            $scope.quarter1 = 'qnone';
            var receivedProject = JSON.parse($scope.projectCombo);
            loadSelectedProjectResults($scope.accountId, receivedProject.Id, $scope.yearCombo, 2);
        }

        //Load subproject summary data to the chart
        function loadProjectSummary() {
            var receivedProject = JSON.parse($scope.projectCombo);
            $scope.labels = [];
            $scope.data = [
                [],
                []
            ];
            $scope.series = [];

            var x = 0;
            $http.get('api/ProcessCompliance/getSelectedProjectSummaries/' + $scope.accountId + '/' + receivedProject.Id).success(function (data) {
                $scope.selectedSummaries = data;

                if (data != '') {
                    $scope.pcShow = true;
                    setInteractions(data[$scope.selectedSummaries.length - 1].Year, data[$scope.selectedSummaries.length - 1].Quarter);
                    console.log("df " + data[$scope.selectedSummaries.length - 1].Year + " " + 1);
                    $scope.IsoQuality = data[$scope.selectedSummaries.length - 1].ProcessVersion;
                    loadSelectedProjectResults($scope.accountId, receivedProject.Id, data[$scope.selectedSummaries.length - 1].Year, data[$scope.selectedSummaries.length - 1].Quarter);
                } else {
                    $scope.pcShow = false;
                    $scope.quarter1 = 'qnone';
                    $scope.quarter2 = 'qnone';
                }
                angular.forEach(data, function (value, key) {

                    var year = value.Year.toString();
                    $scope.labels.push(year + "-Q" + value.Quarter);


                    $scope.data[1][x] = value.Rating;
                    x++;
                });

                for (var x1 = 0; x1 < $scope.selectedSummaries.length; x1++) {
                    $scope.data[0][x1] = 6.2;
                }
                var receivedProject1 = JSON.parse($scope.projectCombo);
                $scope.series[1] = receivedProject1.Name;
                $scope.series[0] = "Threshold";


            })
                .error(function () {
                    $scope.error = "An Error has occured while loading posts!";
                });

        }
        $http.get('api/Account/' + $stateParams.id).success(function (data) {
            $scope.selectedPro1 = data;
            $scope.projectName1 = $scope.selectedPro1.AccountName;

            $scope.te = $scope.projectName1;
        });

        //Declare otherlinks click actions
        $scope.auditClick = function () {

            $window.open($scope.ProcessComplianceConfigLink + "=" + "'" + $scope.CommonData.ConfluencePageId + "'", '_blank');
        }

        $scope.metricsClick = function () {
            $window.open($scope.CommonData.WikiPageLink, '_blank');
        }

        //Declare subprojectCombo change event
        $scope.subProjectChange = function () {
            var receivedProject = JSON.parse($scope.projectCombo);
            $scope.subProjectName = receivedProject.Name;
            loadProjectSummary();
            var receivedProject = JSON.parse($scope.projectCombo);

            loadCommonData(receivedProject.Id);
        }

        //declare chart click event
        $scope.pcChartClick = function (points, evt) {
            console.log(points);

            $scope.clickedLabel = points[0].label;
            $scope.clickedYear = $scope.clickedLabel.split('-')[0];
            $scope.clickedQuarter = $scope.clickedLabel.split('-')[1].toString();
            $scope.clickedQuarter = $scope.clickedQuarter.substring(1, 2);
            var receivedProject = JSON.parse($scope.projectCombo);
            loadSelectedProjectResults($scope.accountId, receivedProject.Id, $scope.clickedYear, $scope.clickedQuarter);

        }

        //Load other links according to the selected ubproject
        function loadCommonData(projectId) {
            $http.get('api/CommonData/' + projectId).success(function (data) {
                $scope.CommonData = data;
                console.log("CommonData " + $scope.CommonData.ConfluencePageId);
            })
                .error(function () {
                    $scope.error = "An Error has occured while loading posts!";
                });
        }

        function Initializer() {
            $http.get('api/AppSettings/getProcessComplianceLink').success(function (data) {

                $scope.ProcessComplianceConfigLink = data.split('"')[1];
            })

            .error(function () {
                $scope.error = "An Error has occured while loading posts!";
            });
        }

        function getSelectedProjectDurationSummary(accountId, projectId, year, quarter) {
            $http.get('api/ProcessCompliance/getSelectedProjectDurationSummary/' + accountId + '/' + projectId + '/' + year + '/' + quarter).success(function (data) {
                $scope.selectedProjectDurationSummary = data;

            })
                .error(function () {
                    $scope.error = "An Error has occured while loading posts!";
                });
        }


    }
})();