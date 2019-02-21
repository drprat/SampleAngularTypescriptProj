define(["require", "exports"], function (require, exports) {
    "use strict";
    var CommunityController = (function () {
        function CommunityController($scope, $location, services, accountService, userRelationService) {
            this.accountService = accountService;
            this.accountService.assign(services);
            this.$location = $location;
            this.$scope = $scope;
            // set scope variables
            this.$scope.message = "Community";
        }
        return CommunityController;
    }());
    exports.CommunityController = CommunityController;
});
//# sourceMappingURL=community-ctrl.js.map