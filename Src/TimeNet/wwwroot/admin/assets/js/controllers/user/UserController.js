adminApp.controller('UserController',
    function (UserService, ReportService, $scope, $timeout) {
        let user_table = null,
            getUsersUrl = '/admin/user/get'
        $scope.deleteids_arr = []
        $scope.listActiveFilter = globalLocalizer.activeTabValues
        let firstLoading = false
        $scope.FilterModel = {
            Active: null,
            SearchText: searchText
        }
        $timeout(function () {
            user_table = $('#user_table').DataTable({
                "dom": '<"row"rt"<"col-sm-12 col-md-6 table-paging-customize"l><"col-sm-12 col-md-6 table-paging-customize"p>>',
                "language": globalLocalizer.datatableLanguageConfig,
                "processing": true,
                "serverSide": true,
                "ajax": {
                    "url": getUsersUrl,
                    "type": "POST",
                    data: {
                        'FilterModel.SearchText': function () { return $scope.FilterModel.SearchText },
                        'FilterModel.Active': function () { return $scope.FilterModel.Active }
                    },
                    beforeSend: function () {
                        loading.show()
                        if (!firstLoading) {
                            loadSkeletonTable('user_table')
                        }
                    },
                    complete: function () {
                        loading.hide()
                        if (!firstLoading) firstLoading = true
                    },
                    error: function () {
                        message.error()
                        $(`#user_table tbody tr`).empty()
                        $('#user_table_processing').hide()
                    },
                },
                columnDefs: [
                    { orderable: false, targets: 0 },
                    { orderable: false, targets: 6 },
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
                        data: 'Email',
                        render: function (col, type, row) {
                            let html = `<a href="/admin/user/update/${row.id}" class="table-row-key">${row.email}</a>`
                            return html;
                        }
                    },
                    {
                        data: 'firstName', render: (col, type, row) => row.firstName
                    },
                    {
                        data: 'lastName', render: (col, type, row) => row.lastName
                    },
                    {
                        data: 'phoneNumber', render: (col, type, row) => row.phoneNumber
                    },
                    {
                        data: 'Active',
                        render: function (col, type, row) {
                            console.log(row)
                            return row.active ? `<span class="badge-success">${globalLocalizer.activeTabValues[1].name}</span>` : `<span class="badge-warning">${globalLocalizer.activeTabValues[2].name}</span>`;
                        }
                    },
                    {
                        data: 'Roles',
                        render: function (col, type, row) {
                            let html = ''
                            const roles = row.roles
                            if (roles.length > 0) {
                                html += `<div class="d-flex">`
                                roles.forEach(o => {
                                    html += `<span class="tag tag-dark tag-pill me-2">${o.name}</span>`
                                })
                                html += `</div>`
                            }
                            return html;
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

        $scope.getUsers = function () {
            user_table.ajax.reload();
        }
        $scope.setActive = function (val) {
            $scope.FilterModel.Active = val
            $scope.getUsers();
        }
        $scope.deleteUsers = function () {
            UserService.BulkDelete($scope.deleteids_arr).then(function (data) {
                if (data) {
                    message.deleteSuccess()
                    $scope.getUsers()
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
                $('#user_table input[type="checkbox"]').prop('checked', true);
                $('#user_table input[type="checkbox"].delete_check:checked').each(function () {
                    $scope.deleteids_arr.push($(this).val());
                });
            }
            else {
                $scope.deleteids_arr = []
                $('#user_table input[type="checkbox"]').prop('checked', false);
            }
            $scope.updateTableUi()
        }

        $scope.deleteSelected = function () {
            $scope.deleteUsers()
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
            const parms = user_table.ajax.params()
            const pageIndex = parms.start / parms.length + 1
            const pageSize = parms.length
            const published = $scope.FilterModel.Active
            const searchText = $scope.FilterModel.SearchText
            const language = utils.getCurrentLanguage()
            let url = `/admin/reports/ExportUserReport?published=${published}&pageIndex=${pageIndex}&pageSize=${pageSize}&language=${language}`
            if (searchText)
                url += `&searchText=${searchText}`

            const order = parms.order
            if (order && order.length > 0) {
                const OrderBy = parms.columns[order[0].column].data
                const ascending = order[0].dir === 'asc'
                url += `&OrderBy=${OrderBy}&ascending=${ascending}`
            }
            const fileName = language === 'vi' ? 'thuong_hieu.xlsx' : 'users_data_export.xlsx';
            await ReportService.Export(url, fileName)
        }
    })