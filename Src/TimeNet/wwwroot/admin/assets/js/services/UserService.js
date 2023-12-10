adminApp.service('UserService', function (CommonService) {
    
    this.Get = (data) => CommonService.Http.Post(`/admin/user/GetAll`, data)

    this.GetUser = (id) => CommonService.Http.Get(`/admin/user/FindById?id=${id}`)

    this.AddUser = (user) => CommonService.Http.Post(`/admin/user/add`, user)

    this.UpdateUser = (user) => CommonService.Http.Put(`/admin/user/update`, user)

    this.BulkDelete = (listIds) => CommonService.Http.Post(`/admin/user/DeleteMany`, listIds)

    this.AddImage = (id, image) => CommonService.Http.Post(`/admin/user/AddImage?id=${id}`, image)

    this.DeleteImage = (id) => CommonService.Http.Delete(`/admin/user/DeleteImage?id=${id}`)
})