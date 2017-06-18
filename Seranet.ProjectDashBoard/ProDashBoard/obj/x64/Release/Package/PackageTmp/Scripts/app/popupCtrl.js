(function () {
    'use strict';
    //create angularjs controller

    app.controller('popupController', ['$scope', '$http', '$modalInstance', 'ProId', 'Year', 'Quarter', 'EmpId', popupController]);

    //angularjs controller method
    function popupController($scope, $http, $modalInstance, ProId, Year, Quarter, EmpId) {
        $scope.close = function () {
            $modalInstance.dismiss('cancel');
        };
        $scope.len = 8;

        //Load selected employee's teamsatisfaction survey questions and answers 
        $http.get('api/Results/getReviewData/' + ProId + '/' + Year + '/' + Quarter + '/' + EmpId).success(function (data) {
            $scope.reviewData = data;
            $scope.len = $scope.reviewData.length;
            var i=1;
            for (var x = 0; x < $scope.reviewData.length; x++) {
                $scope.reviewData[x][8] = i;
                i++;
            }
            
        })

        .error(function () {
            $scope.error = "An Error has occured while loading posts!";
            
        });
        
    }
})();