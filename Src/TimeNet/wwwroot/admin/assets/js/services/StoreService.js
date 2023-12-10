adminApp.service('StoreService', function (CommonService) {
    this.Get = (data) => CommonService.Http.Post(`/admin/store/getAll`, data)
  
    this.GetStore = (id) => CommonService.Http.Get(`/admin/store/findById?id=${id}`)

    this.AddStore = (store) => CommonService.Http.Post(`/admin/store/add`, store)

    this.UpdateStore = (store) => CommonService.Http.Put(`/admin/store/update`, store)

    this.BulkDelete = (listIds) => CommonService.Http.Post(`/admin/store/deleteMany`, listIds)

    this.AddImage = (id, image) => CommonService.Http.Post(`/admin/store/addLogo?id=${id}`, image)

    this.DeleteImage = (id) => CommonService.Http.Delete(`/admin/store/deleteLogo?id=${id}`)
})