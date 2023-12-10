appModule.controller('HomeController',
    function ($scope, $timeout, $http) {
        $scope.widgets = []
        $scope.carousels = []

        $scope.init = function (widgetsString, carouselString) {
            if (carouselString)
                $scope.carousels = JSON.parse(carouselString).WidgetCarousels
            $scope.initCarouselLibs()

            const obj = JSON.parse(widgetsString)
            if (obj.length > 0)
                obj.forEach(o => {
                    if (o.Type === 'Category') {
                        $http({
                            method: 'POST',
                            url: `/api/v1/products`,
                            data: { pageSize: 20, categoryId: o.ItemId }
                        }).then(function mySuccess(response) {
                            if (response.data) {
                                $scope.widgets.push({
                                    name: o.Name,
                                    type: o.Type,
                                    data: response.data
                                })
                                //Slider fake
                                $scope.widgets.push({
                                    name: o.Name,
                                    type: 'Slider',
                                    data: response.data
                                })
                                $scope.initLibs()
                            }
                        }, function myError(response) {
                        });

                    }
                })
        }

        $scope.calculatePercentage = function (price, oldPrice) {
            return 100 - Math.round(price * 100 / oldPrice)
        }

        $scope.initLibs = function () {
            $timeout(function () {
                $('.new-arrivals-product-activation-2').slick({
                    infinite: true,
                    slidesToShow: 4,
                    slidesToScroll: 4,
                    arrows: true,
                    dots: false,
                    prevArrow: '<button class="slide-arrow prev-arrow" aria-label="prev-arrow-button"><i class="fal fa-long-arrow-left"></i></button>',
                    nextArrow: '<button class="slide-arrow next-arrow" aria-label="next-arrow-button"><i class="fal fa-long-arrow-right"></i></button>',
                    responsive: [{
                        breakpoint: 1199,
                        settings: {
                            slidesToShow: 3,
                            slidesToScroll: 3
                        }
                    },
                    {
                        breakpoint: 991,
                        settings: {
                            slidesToShow: 2,
                            slidesToScroll: 2
                        }
                    },
                    {
                        breakpoint: 576,
                        settings: {
                            variableWidth: true,
                            slidesToShow: 1,
                            slidesToScroll: 1
                        }
                    }
                    ]
                });
            }, 0);
        }

        $scope.initCarouselLibs = function () {
            $timeout(function () {
                $('.slider-activation-one').on('init', function () {
                    $('.slider-activation-one').css({ visibility: 'visible' });
                });
                $('.slider-activation-one').slick({
                    infinite: true,
                    autoplay: true,
                    slidesToShow: 1,
                    slidesToScroll: 1,
                    arrows: false,
                    dots: true,
                    fade: true,
                    focusOnSelect: false,
                    speed: 400

                });
            }, 0);
        }
        $scope.convertUrlImage = function (url) {
            return url && url.path && url.path ? `/${url.path}` : '/admin/assets/img/no-image.svg'
        }
    })


