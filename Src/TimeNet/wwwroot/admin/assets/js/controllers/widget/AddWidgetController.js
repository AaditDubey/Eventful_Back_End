adminApp.controller('AddWidgetController',
    function (WidgetService, StorageService, BrandService, CategoryService, $scope, $timeout) {
        $scope.widgetTypes = ["HeaderMenus", "Carousel", "Category", "Brand", "Grid", "Html", "Footer" ]
        $scope.widget = {
            id: '',
            name: '',
            displayOrder: 0,
            published: true,
            metaKeywords: '',
            metaDescription: '',
            metaTitle: '',
            description: '',
            type: $scope.widgetTypes[0],
            widgetFooters: {
                columns:[]
            },
            widgetCarousels: [],
            widgetMenus: []
        }
        $scope.categories = []
        $scope.brands = []
        $scope.items = []
        $scope.init = function (storeId) { $scope.widget.storeId = storeId }
        $scope.onWidgetTypeChange = function () {
            console.log($scope.widget.type)
            switch ($scope.widget.type) {
                case 'Category':
                    if (!$scope.categories || $scope.categories.length < 1)
                        CategoryService.GetAll().then(function (data) {
                            if (data) {
                                $scope.categories = data;
                                $scope.categories.forEach(c => {
                                    c.name = c.levelName
                                })
                                $scope.categories.push({ id: '', name: `-- ${globalLocalizer.common.select} --` })
                                console.log($scope.categories)
                                $scope.items = $scope.categories
                            }
                        });
                    else
                        $scope.items = $scope.categories
                    break
                case 'Brand':
                    if (!$scope.brands || $scope.brands.length < 1)
                        BrandService.Get({ pageSize: 500 }).then(function (data) {
                            if (data && data.items) {
                                $scope.brands = data.items;
                                $scope.brands.push({ id: '', name: `-- ${globalLocalizer.common.select} --` })
                                console.log($scope.brands)
                                $scope.items = $scope.brands
                            }
                        });
                    else
                        $scope.items = $scope.brands
                    break
            }
        }
        $scope.addWidget = async function (continueEdit) {
            if ($scope.addWidgetform.$valid) {
                loading.show()
                if ($scope.widget.type === 'HeaderMenus') {
                    $scope.widget.widgetMenus = $scope.menuList.filter(x => x.id).map(x => ({ menuId: x.id, title: x.title, displayOrder: x.displayOrder, parentId: x.parentId, link: x.link }))
                }
                const result = await WidgetService.Add($scope.widget)
                if (result)
                    window.location.href = `/admin/widget?storeId=${$scope.widget.storeId}` 
                loading.hide()
            } else {
                return false;
            }
        };

        /*==========================
        Carousel navigation function
        ==========================*/
        $scope.deleteCarousel = function (id) {
            const carousels = $scope.widget.widgetCarousels.filter(x => x.id !== id)
            $scope.widget.widgetCarousels = carousels
            console.log(carousels)
        }

        $scope.addMoreCarousel = function () {
            $scope.widget.widgetCarousels.push(
                {
                    id: `fake_id_${$scope.widget.widgetCarousels.length + 1}`,
                    caption: '',
                    subCaption: '',
                    linkText: '',
                    linkUrl: '',
                    displayOrder: $scope.widget.widgetCarousels.length + 1,
                }
            )
        }

        $scope.uploadCarouselImage = function (event) {
            var elementId = event.id
            var formData = new FormData();
            var file = document.getElementById(elementId).files[0]
            formData.append("file", file);
            loading.show()
            StorageService.UploadFile(formData).then(function (data) {
                if (data) {
                    console.log(data)
                    var carouselId = elementId.replace('file-upload_', '')
                    var objIndex = $scope.widget.widgetCarousels.findIndex((obj => obj.id === carouselId));
                    $scope.widget.widgetCarousels[objIndex].image = {
                        id: data.id,
                        path: data.path,
                        alternateText: 'baner',
                        title: 'baner'
                    }
                }
            });
            loading.hide()
        }

        $scope.updateCarousel = function (continues) {
            loading(true)
            console.log($scope.widget.widgetCarousels)
            ThemeService.UpdateTheme($scope.widget).then(function (data) {
                if (data) {
                    $scope.widget = data;
                    if (continues)
                        appNotification.showSuccess()
                    else
                        window.location.href = '/theme'
                    $scope.GetTheme($scope.widget.id)
                }
                else
                    appNotification.showError()

            });
            loading(false)
        };

        /*==============================
        End carousel navigation function
        ==============================*/

        /*==========================
        Footer navigation function
        ==========================*/
        $scope.addMoreColumn = function () {
            $scope.widget.widgetFooters.columns.push({
                id: `fake_column_id_${$scope.widget.widgetFooters.columns.length + 1}`,
                title: '',
                widgetFootersRows: []
            })
        }

        $scope.deleteColumn = function (id) {
            $scope.widget.widgetFooters.columns = $scope.widget.widgetFooters.columns.filter(x => x.id !== id)
        }

        $scope.deleteRow = function (columnId, rowId) {
            var objIndex = $scope.widget.widgetFooters.columns.findIndex((obj => obj.id === columnId));
            $scope.widget.widgetFooters.columns[objIndex].widgetFootersRows = $scope.widget.widgetFooters.columns[objIndex].widgetFootersRows.filter(r => r.id !== rowId)
        }

        $scope.addNewRow = function (columnId) {
            var objIndex = $scope.widget.widgetFooters.columns.findIndex((obj => obj.id === columnId));
            $scope.widget.widgetFooters.columns[objIndex].widgetFootersRows.push({
                id: `fake_row_id_${columnId}_${$scope.widget.widgetFooters.columns[objIndex].widgetFootersRows.length + 1}`,
                linkText: '',
                linkUrl: ''
            })
        }

        $scope.updateFooter = function (continues) {
            console.log($scope.widget.widgetFooters)
            ThemeService.UpdateTheme($scope.widget).then(function (data) {
                if (data) {
                    $scope.widget = data;
                    if (continues)
                        appNotification.showSuccess()
                    else
                        window.location.href = '/theme'
                    $scope.GetTheme($scope.widget.id)
                }
                else
                    appNotification.showError()

            });
        };
        /*=============================
        End Footer navigation function
        =============================*/


        /*=============================
        Header menus navigation function
        =============================*/
        $scope.tempMenu = { parentId: '' }
        $scope.menuList = [{ id: '', title: 'Parent id' }]
        $scope.menus = []
        $scope.menuAddOrUpdate = {}
        $scope.addMenu = function () {
            if ($scope.tempMenu.title) {
                $scope.menuList.push({ ...$scope.tempMenu, id: utils.createGuid(), children: null })
                $scope.tempMenu = { parentId: '' }
                $scope.updateTreeData()
            }
        }
        $scope.addOrUpdateMenu = function (menu) {
            console.log(menu)
            $scope.menuAddOrUpdate = angular.copy($scope.menuList.find(x => x.id === menu.id))
            $('#modal-add-or-update-menu').modal('show')
        }
        $scope.saveMenu = function () {
            $scope.menuList = $scope.menuList.filter(x => x.id !== $scope.menuAddOrUpdate.id)
            $scope.menuList.push($scope.menuAddOrUpdate)
            $scope.menuAddOrUpdate = {}
            $scope.updateTreeData()
            $('#modal-add-or-update-menu').modal('hide')
        }

        $scope.deleteMenu = function (id) {
            $scope.menuList = $scope.menuList.filter(x => x.id !== id && x.parentId !== id)
            $scope.updateTreeData()
        }

        $scope.updateTreeData = function () {
            const list = $scope.menuList.filter(m => m.id !== '')
            $scope.menus = []
            $scope.menus = listToTree(list)
            console.log($scope.menus);
            //console.log(list_to_tree(list));
            $timeout(function () {
                $(".select2").select2();
            }, 100);
        }
        /*=============================
        End Header menus navigation function
        =============================*/
       
    })

function listToTree(data, options) {
    options = options || {};
    var ID_KEY = options.idKey || 'id';
    var PARENT_KEY = options.parentKey || 'parentId';
    var CHILDREN_KEY = options.childrenKey || 'children';

    var tree = [],
        childrenOf = {};
    var item, id, parentId;

    for (var i = 0, length = data.length; i < length; i++) {
        item = data[i];
        id = item[ID_KEY];
        parentId = item[PARENT_KEY] || '';
        // every item may have children
        childrenOf[id] = childrenOf[id] || [];
        // init its children
        item[CHILDREN_KEY] = childrenOf[id];
        if (parentId != 0) {
            // init its parent's children object
            childrenOf[parentId] = childrenOf[parentId] || [];
            // push it into its parent's children object
            childrenOf[parentId].push(item);
        } else {
            tree.push(item);
        }
    };

    return tree;
}