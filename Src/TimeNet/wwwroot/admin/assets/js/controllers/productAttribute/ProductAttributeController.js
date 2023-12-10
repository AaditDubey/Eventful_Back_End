adminApp.controller('ProductAttributeController',
    function (ProductAttributeService, $scope, $timeout) {
        var productAttribute_table = null,
            getProductAttributesUrl = '/admin/attribute/get'
        $scope.deleteids_arr = []
        $scope.listActiveFilter = globalLocalizer.activeTabValues
        $scope.FilterModel = {
            SearchText: searchText
        }
        $timeout(function () {
            productAttribute_table = $('#productAttribute_table').DataTable({
                "dom": '<"row"rt"<"col-sm-12 col-md-6 table-paging-customize"l><"col-sm-12 col-md-6 table-paging-customize"p>>',
                "language": globalLocalizer.datatableLanguageConfig,
                "processing": true,
                "serverSide": true,
                "ajax": {
                    "url": getProductAttributesUrl,
                    "type": "POST",
                    data: {
                        'FilterModel.SearchText': function () { return $scope.FilterModel.SearchText }
                    }
                },
                columnDefs: [
                    { orderable: false, targets: 0 },
                    { orderable: false, targets: 1 }
                ],
                order: [],
                "columns": [
                    {
                        data: 'Id',
                        render: function (col, type, row, meta) {
                            let html = '<td>'
                            html += `<label class="custom-control custom-checkbox">`
                            html += `<input type="checkbox" class="custom-control-input delete_check" value="${row.id}" onchange="angular.element(this).scope().selectedIdChange(this)" />`
                            html += `<span class="custom-control-label">&nbsp;</span>`
                            html += `</label>`
                            html += '</td>'

                            return html
                        }
                    },
                    {
                        data: 'Name',
                        render: function (col, type, row) {
                            let html = `<a href="/admin/attribute/update/${row.id}" class="table-row-key">${row.name}</a>`
                            return html;
                        }
                    },
                    {
                        data: 'Published',
                        render: function (col, type, row) {
                            let html = ''
                            const optionValues = row.optionValues
                            if (optionValues.length > 0) {
                                html += `<div class="d-flex">`
                                optionValues.forEach(o => {
                                    html += `<span class="tag tag-dark tag-pill me-2">${o.name}</span>`
                                })
                                html += `</div>`
                            }
                            return html;
                        }
                    },
                    {
                        data: 'CreatedOn',
                        render: function (col, type, row) {
                            return row.createdOn ? (new Date(row.createdOn)).toLocaleString() : '';
                        }
                    },
                    {
                        data: 'UpdatedOn',
                        render: function (col, type, row) {
                            return row.updatedOn ? (new Date(row.updatedOn)).toLocaleString() : '';
                        }
                    }
                ]
            });
        }, 0);
        $scope.deleteIds = []
        $scope.categories = []

        $scope.isSelectedAll = false
        $scope.init = function () {
            $(".select2").select2();
            loading.hide()
        }

        $scope.getProductAttributes = function () {
            productAttribute_table.ajax.reload();
        }
        $scope.setActive = function (val) {
            $scope.FilterModel.Published = val
            $scope.getProductAttributes();
        }
        $scope.deleteProductAttributes = function () {
            console.log('deleteProductAttributes')
            ProductAttributeService.DeleteMany($scope.deleteids_arr).then(function (data) {
                if (data) {
                    $scope.getProductAttributes()
                    $scope.deleteids_arr = []
                    $scope.updateTableUi()
                }
                else {
                    message.error()
                }
                $('#modal-comfirm-delete').modal('hide')
            });
        }

        $scope.selectedAll = function () {
            if ($scope.isSelectedAll) {
                $scope.deleteids_arr = []
                $('#productAttribute_table input[type="checkbox"]').prop('checked', true);
                $('#productAttribute_table input[type="checkbox"].delete_check:checked').each(function () {
                    $scope.deleteids_arr.push($(this).val());
                });
            }
            else {
                $scope.deleteids_arr = []
                $('#productAttribute_table input[type="checkbox"]').prop('checked', false);
            }
            $scope.updateTableUi()
        }

        $scope.deleteSelected = function () {
            console.log('delete')
            $scope.deleteProductAttributes()
        }

        $scope.convertUrlImage = function (url) {
            return url && url.path && url.path ? `/${url.path}` : '/admin/assets/img/no-image.svg'
        }

        $scope.selectedIdChange = function (e) {
            var id = e.value
            var checked = e.checked
            if (checked && !$scope.deleteids_arr.find(i => i === id)) {
                $scope.deleteids_arr.push(id)
            }
            else if (!checked && $scope.deleteids_arr.find(i => i === id)) {
                $scope.deleteids_arr = $scope.deleteids_arr.filter(i => i !== id)
            }
            $scope.updateTableUi()

        }
        $scope.updateTableUi = function () {
            if ($scope.deleteids_arr.length > 0) {
                $('#delete-selected-button').show()
            }
            else {
                $('#delete-selected-button').hide()
            }
        }

    })