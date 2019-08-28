var ByteBlocksIdentityApp = angular.module("ByteBlocksIdentityPortal", []);

ByteBlocksIdentityApp.filter('displaydate', function () {
    return function (input, tz) {
        //var hr = input.hour();
        return input.format('MMM Do, YYYY - hh:mm:ss A');
    };
});
ByteBlocksIdentityApp.controller("PortalClockController", [
    '$scope', '$timeout', '$http', function ($scope, $timeout, $http) {
        $scope.currentTime = moment();
        var tick = function () {
            $scope.currentTime = moment();
            $timeout(tick, 1000);
        }
        $timeout(tick, 1000);
    }
]);