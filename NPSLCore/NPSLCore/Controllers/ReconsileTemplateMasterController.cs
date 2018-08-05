using System;
using System.Collections.Generic;
using System.Data;
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
            //if (!records.Any())
            //{
            //    const string msg = "Template does not exsists";
            //    throw new Exception(msg);
            //}
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
        [HttpGet]
        [Route("api/GetDelimeterValue")]
        public IEnumerable<Delimiter> GetDelimeterValue(int id)
        {
            try
            {
                var records = _template.GetDelimeterValue(id);
                return records;
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message.ToString());
            }
        }
        [HttpPost]
        [Route("api/SaveReconsileTemplate")]
        public void SaveUser([FromBody] ReconsileTemplate templateDetail)
        {
            try
            {
                var template = new DataTable();
                template.Columns.Add("TemplateName", typeof(string));
                template.Columns.Add("SourceFolder", typeof(string));
                template.Columns.Add("SourceExtention", typeof(string));
                template.Columns.Add("SourceCompletionPath", typeof(string));
                template.Columns.Add("SourceSubstringValue", typeof(string));
                template.Columns.Add("SourceDelimiter", typeof(string));
                template.Columns.Add("SourceHasHeader", typeof(bool));
                template.Columns.Add("IsActive", typeof(bool));


                DataRow newRow = template.Rows.Add();
                newRow["TemplateName"] = templateDetail.TemplateName;
                newRow["SourceFolder"] = templateDetail.SourceFolder;
                newRow["SourceExtention"] = templateDetail.SourceExtention;
                newRow["SourceCompletionPath"] = templateDetail.SourceCompletionPath;
                newRow["SourceSubstringValue"] = templateDetail.SourceSubstringValue;
                newRow["SourceDelimiter"] = templateDetail.SourceDelimiter;
                newRow["SourceHasHeader"] = templateDetail.SourceHasHeader;
                newRow["IsActive"] = templateDetail.IsActive;
           

                _template.SaveTemplate(template);
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message.ToString());
            }
        }
    }
}