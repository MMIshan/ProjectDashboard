"use strict";

angular.module('HiggidyCarousel', [])

  .controller('HiggidyCarousel.controller', function ($scope, $interval) {
      var timeout;
      var timeout1;
      $scope.carousel = {
          current: 0,
          max: 0
      };
      $scope.carousel1 = {
          current: 0,
          max: 0
      };
      $scope.setMax = function () {
          if ($scope.cusSatDataValue) {
              $scope.carousel.max = $scope.cusSatDataValue.length;
          } else {
              $scope.carousel.max = 1;
          }

          if ($scope.processComplianceDataVal) {
              $scope.carousel1.max = $scope.processComplianceDataVal.length;
          } else {
              $scope.carousel1.max = 1;
          }
      };
      $scope.show = function (i) {
          $scope.resetTimeout();
          $scope.carousel.current = i;
          $scope.carousel1.current = i;
      };
      $scope.moveOn = function () {
          $scope.carousel.current++;
          if ($scope.carousel.current >= $scope.carousel.max) {
              $scope.carousel.current = 0;
          }
          $scope.carousel1.current++;
          if ($scope.carousel1.current >= $scope.carousel1.max) {
              $scope.carousel1.current = 0;
          }
      };
      $scope.initTimeout = function () {
          timeout = $interval($scope.moveOn, $scope.carousel.timeout);
      };
      $scope.resetTimeout = function () {
          $interval.cancel(timeout);
          $interval.cancel(timeout1);
          $scope.initTimeout();
      };
      $scope.$watch('carousel.timeout', $scope.initTimeout);
      $scope.$watch('images', $scope.setMax);
      $scope.$watch('images1', $scope.setMax);
  })

  .directive('higgidyCarousel', function () {
      var directive = {
          controller: 'HiggidyCarousel.controller',
          scope: true,
          link: {
              pre: function (scope, element, attrs) {
                  scope.carousel.width = element[0].offsetWidth;
                  scope.getWidth = function () {
                      scope.carousel.width = element[0].offsetWidth;
                  };
                  scope.carousel.timeout = attrs.timeout || 1000;
              }
          }
      };
      return directive;
  })

  .directive('higgidyCarouselImages', function () {
      var directive = {
          scope: true,
          link: {
              post: function (scope, element) {
                  scope.setsWidths = function () {
                      var totalWidth = scope.carousel.width * scope.carousel.max;
                      if (totalWidth == 0) {
                          totalWidth = 380;
                      }
                      element.find('img').css({
                          width: scope.carousel.width + 'px'
                      });
                      element.css({
                          width: totalWidth + 'px'
                      });
                  };
                  scope.animateScroll = function () {
                      element.css({ 'margin-left': 0 - scope.carousel.width * scope.carousel.current + "px" });
                  };
                  scope.$watch('carousel.max', scope.setsWidths);
                  scope.$watch('carousel.current', scope.animateScroll);
              }
          }
      };
      return directive;
  });
