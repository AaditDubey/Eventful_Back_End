adminApp.service('SpeakerService', function (CommonService) {
    this.Get = (data) => CommonService.Http.Post(`/admin/speaker/GetAll`, data)
  
    this.GetSpeaker = (id) => CommonService.Http.Get(`/admin/speaker/FindById?id=${id}`)

    this.AddSpeaker = (speaker) => CommonService.Http.Post(`/admin/speaker/add`, speaker)

    this.UpdateSpeaker = (speaker) => CommonService.Http.Put(`/admin/speaker/update`, speaker)

    this.BulkDelete = (listIds) => CommonService.Http.Post(`/admin/speaker/DeleteMany`, listIds)

    this.AddImage = (id, image) => CommonService.Http.Post(`/admin/speaker/AddImage?id=${id}`, image)

    this.DeleteImage = (id) => CommonService.Http.Delete(`/admin/speaker/DeleteImage?id=${id}`)
})