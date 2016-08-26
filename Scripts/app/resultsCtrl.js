(function () {
    //create angularjs controller

    app.controller('myController', ['$scope', '$http', '$routeParams', ResultsController]);
    function ResultsController($scope, $http, $routeParams) {
        

        $http.get('/api/Project/' + $scope.myVal).success(function (data) {
            $scope.selectedPro = data;
            $scope.projectName = $scope.selectedPro.Name;
            
        })

        .error(function () {
            $scope.error = "An Error has occured while loading posts!";
            
        });
    }
})();