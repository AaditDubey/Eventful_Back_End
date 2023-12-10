adminApp.controller('UpdateCouponController',
    function (CouponService, ProductService, CategoryService, BrandService, $scope, $timeout) {
        $scope.coupon = {
            id: '',
            name: '',
            displayOrder: 0,
            published: true,
            metaKeywords: '',
            metaDescription: '',
            metaTitle: '',
            description: '',
        }
        $scope.discountTypes = globalLocalizer.discountTypeValues
        $scope.generateCode = function () {
            $scope.coupon.couponCode = utils.string.generateCode()
        }
        $scope.init = function (id) {
            $scope.GetCoupon(id)
        }

        $scope.GetCoupon = function (id) {
            CouponService.GetCoupon(id).then(function (data) {
                if (data) {
                    $scope.coupon = data;
                    console.log($scope.coupon)
                    if ($scope.coupon.startDateUtc)
                        $scope.coupon.startDateUtc = new Date($scope.coupon.startDateUtc)

                    if ($scope.coupon.endDateUtc)
                        $scope.coupon.endDateUtc = new Date($scope.coupon.endDateUtc)

                    if ($scope.coupon.idsApply) {
                        const query = { ids: $scope.coupon.idsApply, pageSize: $scope.coupon.idsApply.length }
                        if ($scope.coupon.type === 'AssignedToProducts')
                            ProductService.FindProducts(query).then(function (result) {
                                if (result && result.items) {
                                    result.items.forEach(i => {
                                        $scope.itemsSelected.push({
                                            id: i.id,
                                            name: i.name
                                        })
                                    })
                                }
                            });
                        else if ($scope.coupon.type === 'AssignedToCategories')
                            CategoryService.Find(query).then(function (result) {
                                console.log(result)
                                if (result && result.items) {
                                    result.items.forEach(i => {
                                        $scope.itemsSelected.push({
                                            id: i.id,
                                            name: i.name
                                        })
                                    })
                                }
                            });
                        else if ($scope.coupon.type === 'AssignedToBrands')
                            BrandService.Get(query).then(function (result) {
                                if (result && result.items) {
                                    result.items.forEach(i => {
                                        $scope.itemsSelected.push({
                                            id: i.id,
                                            name: i.name
                                        })
                                    })
                                }
                            });
                    }
                }
            });
        }
        $scope.updateCoupon = async function (continueEdit) {
            if ($scope.updateCouponform.$valid) {
                loading.show()
                if ($scope.coupon.startDateUtc)
                    $scope.coupon.startDateUtc = $scope.coupon.startDateUtc.toISOString()

                if ($scope.coupon.endDateUtc)
                    $scope.coupon.endDateUtc = $scope.coupon.endDateUtc.toISOString()

                if ($scope.itemsSelected)
                    $scope.coupon.IdsApply = $scope.itemsSelected.map(x => x.id)
                CouponService.UpdateCoupon($scope.coupon).then(function (data) {
                    if (data) {
                        const link = continueEdit ? `/admin/coupon/update/${data.id}` : `/admin/coupon`
                        window.location.href = link
                    }
                    else
                        message.error()
                });
                loading.hide()
            } else {
                return false;
            }
        };

        //Search and add item
        $scope.searchText = ''
        $scope.items = []
        $scope.itemsSelected = []
        $scope.couponTypeOnChange = function () {
            $scope.itemsSelected = []
        }

        var timeout = $timeout(function () { });
        $scope.search = function () {
            $scope.searchText = ''
            $scope.items = []
            $('#modal-add-items-apply').modal('show')
        }
        $scope.searchOnChange = function () {
            $timeout.cancel(timeout); //cancel the last timeout
            $scope.items = []
            timeout = $timeout(function () {
                const query = { searchText: $scope.searchText, pageSize: 20 }
                if ($scope.coupon.type === 'AssignedToProducts')
                    ProductService.FindProducts(query).then(function (result) {
                        if (result && result.items) {
                            result.items.forEach(i => {
                                $scope.items.push({
                                    id: i.id,
                                    name: i.name
                                })
                            })
                        }
                    });
                else if ($scope.coupon.type === 'AssignedToCategories')
                    CategoryService.Find(query).then(function (result) {
                        console.log(result)
                        if (result && result.items) {
                            result.items.forEach(i => {
                                $scope.items.push({
                                    id: i.id,
                                    name: i.name
                                })
                            })
                        }
                    });
                else if ($scope.coupon.type === 'AssignedToBrands')
                    BrandService.Get(query).then(function (result) {
                        if (result && result.items) {
                            result.items.forEach(i => {
                                $scope.items.push({
                                    id: i.id,
                                    name: i.name
                                })
                            })
                        }
                    });
            }, 300);
        }

        $scope.selectItem = function (id) {
            if (!$scope.itemsSelected.find(x => x.id === id))
                $scope.itemsSelected.push($scope.items.find(x => x.id === id))
        }
        $scope.deleteItem = function (id) {
            const objWithIdIndex = $scope.itemsSelected.findIndex((obj) => obj.id === id);

            if (objWithIdIndex > -1) {
                $scope.itemsSelected.splice(objWithIdIndex, 1);
            }
        }
    })