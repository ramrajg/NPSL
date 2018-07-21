using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using NPSL.Extensions.CustomException;
using NPSL.Repository.Core.Template;
using NPSLCore.Models.DB;

namespace NPSLCore.Controllers
{
  
    [Produces("application/json")]
    [EnableCors("AllowAllHeaders")]
    public class ReconsileTemplateMasterController : Controller
    {
        private readonly IReconsileTemplateRepository _template;
        public ReconsileTemplateMasterController(IReconsileTemplateRepository template)
        {
            _template = template;
        }
      
 
        [HttpGet]
        [Route("api/GetTemplateById")]
        public IEnumerable<ReconsileTemplate> GetTemplatesById(int id)
        {
            var records = _template.GetTemplatesById(id);
            if (!records.Any())
            {
                const string msg = "Template does not exsists";
                throw new Exception(msg);
            }
            return records;
        }

        [HttpGet]
        [Route("api/GetFileExtension")]
        public IEnumerable<FileExtension> GetFileExtension(int id)
        {
            try
            {
                var records = _template.GetFileExtenstion(id);
                return records;
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message.ToString());
            }
        }
    }
}