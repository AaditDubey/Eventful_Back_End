adminApp.service('CategoryService', function (CommonService) {
    this.GetAll = () => CommonService.Http.Post(`/admin/category/GetAll`, { pageSize: 1000 })

    this.Find = (data) => CommonService.Http.Post(`/admin/category/Find`, data)

    this.GetCategory = (id) => CommonService.Http.Get(`/admin/category/FindById?id=${id}`)

    this.AddCategory = (category) => CommonService.Http.Post(`/admin/category/add`, category)

    this.UpdateCategory = (category) => CommonService.Http.Put(`/admin/category/update`, category)

    this.BulkDelete = (listIds) => CommonService.Http.Post(`/admin/category/DeleteMany`, listIds)

    this.AddImage = (id, image) => CommonService.Http.Post(`/admin/category/AddImage?id=${id}`, image)

    this.DeleteImage = (id) => CommonService.Http.Delete(`/admin/category/DeleteImage?id=${id}`)
})