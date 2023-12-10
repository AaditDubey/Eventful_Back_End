appModule.controller('SearchController',
    function ($scope, CommonService, $timeout) {
        $scope.siteController = angular.element(document.getElementById("SiteController")).scope()
        $scope.categories = []
        $scope.brands = []
        $scope.attributes = []
        $scope.data = {}
        $scope.products = []
        $scope.sortingOptions = [
            {
                value: '',
                title: 'Sorting options'
            },
            {
                value: 'createdOnDesc',
                title: 'Latest'
            },
            {
                value: 'createdOnEsc',
                title: 'Oldest'
            },
            {
                value: 'nameEsc',
                title: 'Name: A to Z'
            },
            {
                value: 'nameDesc',
                title: 'Name: Z to A'
            },
            {
                value: 'priceEsc',
                title: 'Price: Low to High'
            },
            {
                value: 'priceDesc',
                title: 'Price: High to Low'
            }
        ]
        $scope.currentPrice = 0
        $scope.filter = {
            categoryId: '',
            brandId: '',
            categorySlug: '',
            brandSlug: '',
            searchText: '',
            pageIndex: 1,
            pageSize: 10,
            totalPage: 0,
            totalItems: 0,
            hasNextPage: true,
            sortKey: $scope.sortingOptions[0].value
        }
        $scope.seletedAttributes = []

        $scope.calculatePercentage = function (price, oldPrice) {
            return 100 - Math.round(price * 100 / oldPrice)
        }

        $scope.getAttributeClass = function (attributeId, value, isColorAttribute) {
            if (!isColorAttribute)
                return $scope.seletedAttributes.find(a => a.id === attributeId && a.values.includes(value)) ? 'attribute-value attribute-value-chosen' : 'attribute-value'
            else
                return $scope.seletedAttributes.find(a => a.id === attributeId && a.values.includes(value)) ? 'chosen' : ''
        }
        $scope.updateAttributeSelected = function (attributeId, value) {
            console.log('slect', value)
            const att = $scope.seletedAttributes.find(a => a.id === attributeId)
            if (!att)
                $scope.seletedAttributes.push({ id: attributeId, values: [value] })
            else if (!att.values.includes(value)) {
                att.values.push(value)
            }
            else {
                var index = att.values.indexOf(value);
                if (index > -1) {
                    att.values.splice(index, 1);
                }
            }
            $scope.getProducts()
        }
        $scope.init = function (filterString) {
            const filter = JSON.parse(filterString)
            $scope.filter = {
                ...$scope.filter,
                categoryId: '',
                brandId: '',
                categorySlug: filter.Category,
                brandSlug: filter.Brand,
                searchText: filter.SearchText,
            }
            $scope.getCategories()
            $scope.getBrands()
            $scope.getAttributes()
        }
        
        $scope.getCategories = function () {
            CommonService.Category.GetTree().then(function (data) {
                if (data) {
                    $scope.categories = data
                    const categoryExisting = search($scope.categories, $scope.filter.categorySlug, 'seName')
                    if (categoryExisting) {
                        $scope.filter = {
                            ...$scope.filter,
                            categoryId: categoryExisting.id
                        }
                    }
                    $scope.getProducts()
                }
            });
        }

        $scope.getBrands = function () {
            CommonService.Brand.Get().then(function (data) {
                if (data) {
                    $scope.brands = data.items
                }
            });
        }

        $scope.getAttributes = function () {
            CommonService.Attribute.Get({ pageSize: 1000 }).then(function (data) {
                if (data) {
                    $scope.attributes = data.items
                    console.log($scope.attributes)

                    $timeout(function () {
                        $('.toggle-attribute-list > .title').on('click', function (e) {
                            var target = $(this).parent().children('.shop-submenu');
                            var target2 = $(this).parent();
                            $(target).slideToggle();
                            $(target2).toggleClass('active');
                        });
                    })
                  
                }
            });
        }

        $scope.setCategory = function (id) {
            $scope.filter = { ...$scope.filter, categoryId: id }
            $scope.getProducts()
        }
        $scope.setBrand = function (id) {
            $scope.filter = { ...$scope.filter, brandId: id }
            $scope.getProducts()
        }
        $scope.getProducts = function (loadMore) {
            const attributes = $scope.seletedAttributes.filter(a => a && a.values.length > 0)
            if (attributes.length > 0) {
                const attributesQuery = $scope.seletedAttributes.map(a => ({ id: a.id, values: a.values.join(',') }))
                console.log('attributesQuery', attributesQuery)
                $scope.filter = { ...$scope.filter, attributes: attributesQuery }
            }
            else { 
                $scope.filter = { ...$scope.filter, attributes: null }
            }

            CommonService.Product.Get($scope.filter).then(function (data) {
                if (data) {

                    const sort = $scope.sortingOptions.find(s => s.value === $scope.filter.sortKey)
                    let ascending = true
                    let orderBy = ''
                    switch (sort.value) {
                        case 'createdOnEsc':
                            ascending = true
                            orderBy = 'CreatedOn'
                            break
                        case 'nameEsc':
                            ascending = true
                            orderBy = 'Name'
                            break
                        case 'nameDesc':
                            ascending = false
                            orderBy = 'Name'
                            break
                        case 'priceEsc':
                            ascending = true
                            orderBy = 'Price'
                            break
                        case 'priceDesc':
                            ascending = false
                            orderBy = 'Price'
                            break
                        case 'createdOnDesc':
                        default:
                            ascending = false
                            orderBy = 'CreatedOn'
                            break
                    }

                    $scope.filter = {
                        ...$scope.filter,
                        pageIndex: data.pageIndex,
                        pageSize: data.pageSize,
                        totalPage: data.totalPage,
                        totalItems: data.totalItems,
                        hasNextPage: data.pageIndex < data.totalPage,
                        ascending: ascending,
                        orderBy: orderBy
                    }
                    if (loadMore)
                    {
                        console.log(' data ', data)
                        data.items.forEach(i => {
                            $scope.products.push(i)
                        })
                        console.log('  $scope.filter ', $scope.filter)
                    }
                    else
                        $scope.products = data.items
                }
            });
        }

        $scope.loadMore = function () {
            $scope.filter = { ...$scope.filter, pageIndex: $scope.filter.pageIndex + 1 }
            $scope.getProducts(true)
        }

        $scope.sortChange = function () {
            console.log($scope.filter.sortKey)
            $scope.getProducts()
        }
    })

function search(tree, value, key = 'id', reverse = false) {
    const stack = [tree[0]]
    while (stack.length) {
        const node = stack[reverse ? 'pop' : 'shift']()
        if (node[key]?.toLowerCase() === value?.toLowerCase()) return node
        node.children && stack.push(...node.children)
    }
    return null
}