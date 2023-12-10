using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Time.Commerce.Application.Helpers;
using Time.Commerce.Application.Services.Cms;
using Time.Commerce.Domains.Entities.Cms;
using Time.Commerce.Domains.Repositories.Cms;
using Time.Commerce.Infras.Repositories.Cms;

namespace Time.Commerce.Infras.Services.Cms
{
    public class ModelService : IModelService
    {
        private readonly IWorkContext _workContext;
        private readonly IModelRepository _modelRepository;

        public ModelService(IWorkContext workContext, IModelRepository modelRepository)
        {
            _workContext= workContext;
            _modelRepository= modelRepository;
        }
        public async Task<Model> CreateAsync(Model model, CancellationToken cancellationToken = default)
        {
            model.CreatedOn = DateTime.UtcNow;
            model.CreatedBy = _workContext.GetCurrentUserId();
            return await _modelRepository.InsertAsync(model);
        }

        public async Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default)
            => await _modelRepository.DeleteAsync(id);

        public async Task<bool> DeleteManyAsync(List<string> ids, CancellationToken cancellationToken = default)
            => await _modelRepository.DeleteManyAsync(ids);

        public async Task<IEnumerable<Model>> GetAllAsync(CancellationToken cancellationToken = default)
            => await _modelRepository.GetAllAsync();

        public async Task<Model> GetByIdAsync(string id, CancellationToken cancellationToken = default)
            => await _modelRepository.GetByIdAsync(id);

        public async Task<Model> GetByNameAsync(string name, CancellationToken cancellationToken = default)
             => await _modelRepository.FindOneAsync(x => x.Name == name);

        public async Task<Model> UpdateAsync(Model model, CancellationToken cancellationToken = default)
        {
            List<string> fieldsCheck = new List<string>();
            var fields = model.FieldSettings;
            foreach (var field in fields)
            {
                if (fieldsCheck.Contains(field.Name))
                    return null;
                fieldsCheck.Add(field.Name);
            }

            var modelUpdate = await _modelRepository.GetByIdAsync(model.Id);
            modelUpdate.FieldSettings = new List<Field>() { };
            if (fieldsCheck.Count > 0)
                modelUpdate.FieldSettings = model.FieldSettings;

            string currentUserID = _workContext.GetCurrentUserId();

            modelUpdate.Name = model.Name;
            modelUpdate.Description = model.Description;
            model.UpdatedOn = DateTime.UtcNow;
            model.UpdatedBy = _workContext.GetCurrentUserId();

            modelUpdate.ApiSettings = model.ApiSettings;

            return await _modelRepository.UpdateAsync(modelUpdate);
        }
    }
}
