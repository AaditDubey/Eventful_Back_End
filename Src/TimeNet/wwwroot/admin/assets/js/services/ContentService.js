adminApp.service('ContentService', function (CommonService) {
    this.Get = (data) => CommonService.Http.Post(`/admin/content/GetAll`, data)

    this.GetContent = (id) => CommonService.Http.Get(`/admin/content/FindById?id=${id}`)

    this.AddContent = (content) => CommonService.Http.Post(`/admin/content/add`, content)

    this.UpdateContent = (content) => CommonService.Http.Put(`/admin/content/update`, content)

    this.BulkDelete = (listIds) => CommonService.Http.Post(`/admin/content/DeleteMany`, listIds)

    this.AddImage = (id, image) => CommonService.Http.Post(`/admin/content/AddImage?id=${id}`, image)

    this.DeleteImage = (id) => CommonService.Http.Delete(`/admin/content/DeleteImage?id=${id}`)
})