adminApp.controller('OrderController',
    function (OrderService, ReportService, $scope, $timeout) {
        let order_table = null,
            getOrdersUrl = '/admin/order/get'
        $scope.deleteids_arr = []
        $scope.listOrderStatuses = globalLocalizer.sales.orderStatuses.map(o => ({ value: o.id, name: o.name }))
        $scope.listOrderStatuses.unshift({ value: '', name: globalLocalizer.common.all })

        $scope.shippingStatuses = globalLocalizer.sales.shippingStatuses.map(o => ({ value: o.id, name: o.name }))
        $scope.shippingStatuses.unshift({ value: '', name: globalLocalizer.common.all })

        $scope.paymentStatuses = globalLocalizer.sales.paymentStatuses.map(o => ({ value: o.id, name: o.name }))
        $scope.paymentStatuses.unshift({ value: '', name: globalLocalizer.common.all })
        let firstLoading = false
        $scope.FilterModel = {
            OrderStatus: '',
            ShippingStatus: '',
            PaymentStatus: '',
            SearchText: searchText
        }
        $timeout(function () {
            order_table = $('#order_table').DataTable({
                "dom": '<"row"rt"<"col-sm-12 col-md-6 table-paging-customize"l><"col-sm-12 col-md-6 table-paging-customize"p>>',
                "language": globalLocalizer.datatableLanguageConfig,
                "processing": true,
                "serverSide": true,
                "ajax": {
                    "url": getOrdersUrl,
                    "type": "POST",
                    data: {
                        'FilterModel.SearchText': function () { return $scope.FilterModel.SearchText },
                        'FilterModel.OrderStatus': function () { return $scope.FilterModel.OrderStatus },
                        'FilterModel.ShippingStatus': function () { return $scope.FilterModel.ShippingStatus },
                        'FilterModel.PaymentStatus': function () { return $scope.FilterModel.PaymentStatus },
                        'FilterModel.FromDate': function () {
                            if (!$scope.FilterModel.FromDate)
                              return null
                            
                            const str = $scope.FilterModel.FromDate.toISOString()
                            var date = new Date(str);
                            date.setDate(date.getDate() + 1);
                            return date.toISOString()
                        },
                        'FilterModel.ToDate': function () {
                            if (!$scope.FilterModel.ToDate)
                                return null

                            const str = $scope.FilterModel.ToDate.toISOString()
                            var date = new Date(str);
                            date.setDate(date.getDate() + 1);
                            return date.toISOString()
                        },
                    },
                    beforeSend: function () {
                        loading.show()
                        if (!firstLoading) {
                            loadSkeletonTable('order_table')
                        }
                    },
                    complete: function () {
                        loading.hide()
                        if (!firstLoading) firstLoading = true
                    },
                    error: function () {
                        message.error()
                        $(`#order_table tbody tr`).empty()
                        $('#order_table_processing').hide()
                    },
                },
                columnDefs: [
                    { orderable: false, targets: 0 }
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
                        data: 'OrderNumber',
                        render: function (col, type, row) {
                            let html = `<a href="/admin/order/update/${row.id}" class="table-row-key">#${row.orderNumber}</a>`
                            return html;
                        }
                    },
                    {
                        data: 'OrderStatus',
                        render: function (col, type, row) {
                            const status = globalLocalizer.sales.orderStatuses.find(s => s.id === row.orderStatus).name
                            let html = ''
                            switch (row.orderStatus) {
                                case 'Pending':
                                    html = `<span class="badge bg-pill bg-warning-light">${status}</span>`
                                    break
                                case 'Complete':
                                    html = `<span class="badge bg-pill bg-success-light">${status}</span>`
                                    break
                                case 'Cancelled':
                                    html = `<span class="badge bg-pill bg-danger-light">${status}</span>`
                                    break
                                default:
                                    html = `<span class="badge bg-pill bg-info-light">${status}</span>`
                                    break
                            }
                            return html
                        }
                    },
                    {
                        data: 'OrderTotal',
                        render: function (col, type, row) {
                            return row.orderTotal
                        }
                    },
                    {
                        data: 'CustomerEmail',
                        render: function (col, type, row) {
                            let html = ''
                            if (row.customerEmail )
                                html = `<a href="/admin/user/update/${row.customerId}" class="table-row-key">${row.customerEmail}</a>`
                            return html;
                        }
                    },
                    {
                        data: 'OrderStatus',
                        render: function (col, type, row) {
                            const status = globalLocalizer.sales.shippingStatuses.find(s => s.id === row.shippingStatus).name
                            let html = ''
                            switch (row.shippingStatus) {
                                case 'WaitingForDeliver':
                                    html = `<span class="badge bg-pill bg-warning-light">${status}</span>`
                                    break
                                default:
                                    html = `<span class="badge bg-pill bg-success-light">${status}</span>`
                                    break
                            }
                            return html
                        }
                    },
                    {
                        data: 'PaymentStatus',
                        render: function (col, type, row) {
                            const status = globalLocalizer.sales.paymentStatuses.find(s => s.id === row.paymentStatus).name
                            let html = ''
                            switch (row.paymentStatus) {
                                case 'Pending':
                                    html = `<span class="badge bg-pill bg-warning-light">${status}</span>`
                                    break
                                default:
                                    html = `<span class="badge bg-pill bg-success-light">${status}</span>`
                                    break
                            }
                            return html
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
       
        $scope.getOrders = function () {
            order_table.ajax.reload();
        }
        $scope.setActive = function (val) {
            $scope.FilterModel.OrderStatus = val
            $scope.getOrders();
        }
        $scope.deleteOrders = function () {
            OrderService.BulkDelete($scope.deleteids_arr).then(function (data) {
                if (data) {
                    message.deleteSuccess()
                    $scope.getOrders()
                    $scope.deleteids_arr = []
                    $scope.updateTableUi()
                    $scope.isSelectedAll = false
                }
                else
                    message.error()
                $('#modal-comfirm-delete').modal('hide')
            });
        }

        $scope.selectedAll = function () {
            if ($scope.isSelectedAll) {
                $scope.deleteids_arr = []
                $('#order_table input[type="checkbox"]').prop('checked', true);
                $('#order_table input[type="checkbox"].delete_check:checked').each(function () {
                    $scope.deleteids_arr.push($(this).val());
                });
            }
            else {
                $scope.deleteids_arr = []
                $('#order_table input[type="checkbox"]').prop('checked', false);
            }
            $scope.updateTableUi()
        }

        $scope.deleteSelected = function () {
            $scope.deleteOrders()
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
                $('#table-tab').hide()
            }
            else {
                $('#delete-selected-button').hide()
                $('#table-tab').show()
            }
        }

        $scope.export = async function () {
            const parms = order_table.ajax.params()
            const pageIndex = parms.start / parms.length + 1
            const pageSize = parms.length
            const orderStatus = $scope.FilterModel.OrderStatus
            const searchText = $scope.FilterModel.SearchText
            const language = utils.getCurrentLanguage()
            let url = `/admin/reports/ExportOrderReport?orderStatus=${orderStatus}&pageIndex=${pageIndex}&pageSize=${pageSize}&language=${language}`
            if (searchText)
                url += `&searchText=${searchText}`

            const order = parms.order
            if (order && order.length > 0) {
                const OrderBy = parms.columns[order[0].column].data
                const ascending = order[0].dir === 'asc'
                url += `&OrderBy=${OrderBy}&ascending=${ascending}`
            }
            const fileName = language === 'vi' ? 'thuong_hieu.xlsx' : 'orders_data_export.xlsx';
            await ReportService.Export(url, fileName)
        }
    })