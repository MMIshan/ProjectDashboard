(function () {
    'use strict';
    //create angularjs controller

    app.controller('adminPanelController', ['$scope', 'toaster', '$mdDialog', '$http', '$window', adminPanelController]);
    
    //angularjs controller method
    function adminPanelController($scope,toaster,$mdDialog,$http,$window,$event) {
        $window.location.href = '#/adminPanel/account';
        $scope.employeeNames = [];
        $scope.accountData;
        EmployeesInitializer();
        isAuthorized();
        //$mdDialog.show(createConfirmDialog('HRIS Data Synchronization', 'Do you want to sysnchronize the employee and account details with HR IS ?', $event)).then(function () {
        //  //  EmployeesInitializer();
        //    // AccountsInitializer();
        //    //retrieveListItems();
        //}, function () {

        //});


        //if (confirm("Do you want to sysnchronize the employee and account details with HR IS!") == true) {
        //    EmployeesInitializer();
        //} else {
            
        //}
        
        function createConfirmDialog(title, content, ev) {
            var confirm = $mdDialog.confirm()
            .title(title)
            .textContent(content)
            .ariaLabel('Lucky day')
            .targetEvent(ev)
            .ok('Yes')
            .cancel('No');
            return confirm;
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
        
        function EmployeesInitializer() {

            $http.get('http://99xt.lk/services/api/Employees', { withCredentials: true }).
                        success(function (data, status, headers, config) {
                            angular.forEach(data, function (value, key) {
                                $scope.employeeNames.push(value.id);
                                console.log(value.id);
                                
                            });
                            addEmployees();
                        }).
                        error(function (data, status, headers, config) {
                            console.log(data);
                        });
            
        }


        function AccountsInitializer() {

            $http.get('http://99xt.lk/services/api/Projects', { withCredentials: true }).
                        success(function (data, status, headers, config) {
                            
                            angular.forEach(data, function (value, key) {
                                console.log(value.assignment + ":" + value.name);
                                $scope.accountData = $scope.accountData + "~" + value.assignment + ":" + value.name + ":" + value.rep;
                                
                            });
                            addAccount();
                        }).
                        error(function (data, status, headers, config) {
                            console.log(data);
                        });

            //$http({ headers: { "Accept": "application/json; odata=verbose" }, method: 'GET', url: "https://99xtech.sharepoint.com/departments/Delivery_stage/Project_Test2/_api/web/lists/getByTitle('Risk List')/Items" })

            //.success(function (data) {
            //    console.log(JSON.parse(data));
            //    //$scope.customers = data.d.results;

            //})
            //.error(function () {
            //    console.log("error");
            //});

            //$http.get("https://99xtech.sharepoint.com/departments/Delivery_stage/Project_Test2/_api/web/lists/getByTitle('Risk List')/Items", { headers: { "Accept": "application/json; odata=verbose" } }).
            //            success(function (data, status, headers, config) {

            //                angular.forEach(data, function (value, key) {
            //                    console.log(value);
                                

            //                });
                            
            //            }).
            //            error(function (data, status, headers, config) {
            //                console.log(data);
            //            });
        }

        var siteUrl = 'https://99xtech.sharepoint.com/departments/Delivery_stage/Project_Test2';

        function retrieveListItems() {
            var clientContext = new SP.ClientContext(siteUrl);
            var oList = clientContext.get_web().get_lists().getByTitle('Announcements');

            var camlQuery = new SP.CamlQuery();
            camlQuery.set_viewXml('<View><Query><Where><Geq><FieldRef Name=\'ID\'/>' +
                '<Value Type=\'Number\'>1</Value></Geq></Where></Query><RowLimit>10</RowLimit></View>');
            this.collListItem = oList.getItems(camlQuery);

            clientContext.load(collListItem);

            clientContext.executeQueryAsync(Function.createDelegate(this, this.onQuerySucceeded), Function.createDelegate(this, this.onQueryFailed));

        }

        function addEmployees() {
            
            var sendingData = JSON.stringify($scope.employeeNames);
            console.log("len " + $scope.employeeNames.length);
            $http.post('api/TeamMembers/addIfNotExist', $scope.employeeNames).success(function (data) {
                AccountsInitializer();
            })
            .error(function () {
                $scope.error = "An Error has occured while loading posts!";
            });
        }

        function addAccount() {

            var sendingData = JSON.stringify($scope.accountData);
            console.log("AccountData " + $scope.accountData)
            $http.post('api/Account/addIfNotexists', sendingData).success(function (data) {
                //alert("Synchronization Finished");
            })
            .error(function () {
                $scope.error = "An Error has occured while loading posts!";
            });
        }
    }

    
})();






