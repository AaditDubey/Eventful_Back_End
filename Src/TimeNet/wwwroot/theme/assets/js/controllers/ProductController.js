(function ($) {
    'use strict';
    var themeInit = {
        i: function (e) {
            $('.product-small-thumb').slick({
                infinite: false,
                slidesToShow: 6,
                slidesToScroll: 1,
                arrows: false,
                dots: false,
                focusOnSelect: true,
                vertical: true,
                speed: 800,
                asNavFor: '.product-large-thumbnail',
                responsive: [{
                    breakpoint: 992,
                    settings: {
                        vertical: false,
                    }
                },
                {
                    breakpoint: 768,
                    settings: {
                        vertical: false,
                        slidesToShow: 4,
                    }
                }
                ]

            });
            $('.product-large-thumbnail').slick({
                infinite: false,
                slidesToShow: 1,
                slidesToScroll: 1,
                arrows: false,
                dots: false,
                speed: 800,
                draggable: false,
                asNavFor: '.product-small-thumb'
            });
            $('.product-large-thumbnail-2').slick({
                infinite: true,
                slidesToShow: 1,
                slidesToScroll: 1,
                arrows: true,
                dots: false,
                speed: 800,
                draggable: false,
                asNavFor: '.product-small-thumb-2',
                prevArrow: '<button class="slide-arrow prev-arrow"><i class="fal fa-long-arrow-left"></i></button>',
                nextArrow: '<button class="slide-arrow next-arrow"><i class="fal fa-long-arrow-right"></i></button>'
            });

            $('.product-small-thumb-2').slick({
                infinite: true,
                slidesToShow: 6,
                slidesToScroll: 1,
                arrows: false,
                dots: false,
                focusOnSelect: true,
                speed: 800,
                asNavFor: '.product-large-thumbnail-2',
                responsive: [{
                    breakpoint: 768,
                    settings: {
                        slidesToShow: 5,
                    }
                },
                {
                    breakpoint: 479,
                    settings: {
                        slidesToShow: 4,
                    }
                }
                ]

            });
        },
    }
    themeInit.i();

})(jQuery);

appModule.controller('ProductController',
    function ($scope, CommonService) {
        $scope.product = {}
        $scope.productsRelated = {}
        $scope.currentPrice = 0
        $scope.quantity = 0
        $scope.siteController = angular.element(document.getElementById("SiteController")).scope()
        $scope.init = function (productString) {

            $scope.product = JSON.parse(productString)
            $scope.product.Attributes.forEach(attribute => {
                let flag = 0
                let obj = []
                attribute.Values.forEach(val => {
                    obj.push({
                        Value: val,
                        Active: flag === 0
                    })
                    flag++
                })
                attribute.Values = obj
            })
            $scope.product.OldPriceFormated = angular.element(document.getElementById("SiteController")).scope().convertCurrency($scope.product.OldPrice)
            $scope.resetPrice()
            const categories = $scope.product.ProductCategoryMapping.map(x => x.CategoryId)
            $scope.getProductsRelated(categories[0])
        }
        $scope.selectAttributeValue = function (attributeId, value) {
            console.log(attributeId, value)
            let attribute = $scope.product.Attributes.find(a => a.Id === attributeId)
            attribute.Values = attribute.Values.map(a => ({ ...a, Active: a.Value === value }))
            console.log('attribute', attribute)
            $scope.resetPrice()
        }

        $scope.resetPrice = function () {
            const actives = $scope.getCurrentAttributes()
            let priceChanged = false
            for (let i = 0; i < $scope.product.Variants.length; i++) {
                const variant = $scope.product.Variants[i]
                const attributes = variant.Attributes
                if (attributes.length === actives.length) {
                    const result = attributes.every((v, i) => actives.find(a => a.Key === v.Key && a.Value === v.Value));
                    if (result && variant.Price > 0) {
                        $scope.product.currentPrice = variant.Price
                        priceChanged = true
                        break
                    }
                }
            }
            if (!priceChanged)
                $scope.product.currentPrice = $scope.product.Price

            $scope.product.currentPriceFormated = angular.element(document.getElementById("SiteController")).scope().convertCurrency($scope.product.currentPrice)
        }

        $scope.getCurrentAttributes = () => $scope.product.Attributes.map(a => ({ Key: a.Name, Value: a.Values.find(v => v.Active).Value }))

        $scope.buyNow = async function () {
            $scope.updateCart ()
        }
        $scope.addToCart = async function () {
            $scope.updateCart()
        }
      
        $scope.updateCart = async function () {
            const qty = Number($('#pro-qty').val())
            if (qty === 0) return

            const attributes = $scope.getCurrentAttributes()
            await $scope.siteController.addProductsToCart([
                {
                    productId: $scope.product.Id,
                    attributes: attributes,
                    quantity: qty
                }
            ])
            const cartQty = Number($('#cart-count').text())
            $('#cart-count').text(cartQty + qty)
            $('#pro-qty').val(0)
            $scope.quantity = 0
        }

        $scope.updateQuantity = function (qty) {
            $scope.quantity += qty
            if ($scope.quantity < 0) $scope.quantity = 0
        }

        $scope.getProductsRelated = function (categoryId) {

            CommonService.Product.Get({ pageSize: 20, categoryId: categoryId }).then(function (data) {
                if (data)
                    $scope.productsRelated = data
                console.log($scope.productsRelated)
            })
        }
    })