adminApp.service('WidgetService', function (CommonService, $http) {
    this.GetWidget = (id) => CommonService.Http.Get(`/admin/widget/FindById?id=${id}`)
    this.Add = (widget) => CommonService.Http.Post(`/admin/widget/add`, widget)
    this.Update = (widget) => CommonService.Http.Put(`/admin/widget/update`, widget)
    this.BulkDelete = (ids) => CommonService.Http.Post(`/admin/widget/DeleteMany`, ids)
})