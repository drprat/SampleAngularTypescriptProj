define(["require", "exports"], function (require, exports) {
    "use strict";
    var NotificationService = (function () {
        function NotificationService() {
        }
        // constructor() { }
        NotificationService.prototype.assign = function (service) {
            this.service = service;
        };
        NotificationService.prototype.getNotifications = function () {
            var deferred = this.service.$q.defer();
            this.service.$http({
                method: "GET",
                url: "/api/Notifications/GetNotifications"
            }).then(function (result) {
                deferred.resolve(result);
            }).catch(function (err) {
                deferred.reject(err);
            });
            return deferred;
        };
        NotificationService.prototype.getActivities = function (userId, nPage, nResults, activityTypeString) {
            var deferred = this.service.$q.defer();
            this.service.$http({
                method: "GET",
                url: "/api/Activities/GetActivities?userId=" + userId + "&nPage=" + nPage +
                    "&nResults=" + nResults + "&activityTypeString=" + activityTypeString
            }).then(function (result) {
                deferred.resolve(result);
            }).catch(function (err) {
                deferred.reject(err);
            });
            return deferred;
        };
        return NotificationService;
    }());
    exports.NotificationService = NotificationService;
});
//# sourceMappingURL=notifications-services.js.map