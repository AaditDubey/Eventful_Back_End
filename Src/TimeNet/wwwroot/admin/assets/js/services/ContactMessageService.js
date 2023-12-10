adminApp.service('ContactMessageService', function (CommonService) {
    
    this.Get = (data) => CommonService.Http.Post(`/admin/contactMessage/GetAll`, data)

    this.GetContactMessage= (id) => CommonService.Http.Get(`/admin/contactMessage/FindById?id=${id}`)

    this.AddContactMessage= (contactMessage) => CommonService.Http.Post(`/admin/contactMessage/add`, contactMessage)

    this.UpdateContactMessage= (contactMessage) => CommonService.Http.Put(`/admin/contactMessage/update`, contactMessage)

    this.BulkDelete = (listIds) => CommonService.Http.Post(`/admin/contactMessage/DeleteMany`, listIds)

    this.AddImage = (id, image) => CommonService.Http.Post(`/admin/contactMessage/AddImage?id=${id}`, image)

    this.DeleteImage = (id) => CommonService.Http.Delete(`/admin/contactMessage/DeleteImage?id=${id}`)
})