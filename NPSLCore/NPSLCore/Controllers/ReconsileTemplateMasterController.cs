using System;
using System.Collections.Generic;
using System.Data;
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

        [HttpGet]
        [Route("api/GetTemplateGroupValue")]
        public IEnumerable<TemplateGroup> GetTemplateGroupValue(int id,int OnlyActive)
        {
            try
            {
                var records = _template.GetTemplateGroupValue(id, OnlyActive);
                return records;
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message.ToString());
            }
        }
        [HttpPost]
        [Route("api/SaveReconsileTemplate")]
        public void SaveReconsileTemplate([FromBody] ReconsileTemplate templateDetail)
        {
            try
            {
                var template = new DataTable();
                template.Columns.Add("TemplateId", typeof(int));
                template.Columns.Add("TemplateName", typeof(string));
                template.Columns.Add("SourceFolder", typeof(string));
                template.Columns.Add("SourceExtention", typeof(string));
                template.Columns.Add("SourceCompletionPath", typeof(string));
                template.Columns.Add("SourceSubstringValue", typeof(string));
                template.Columns.Add("SourceDelimiter", typeof(string));
                template.Columns.Add("SourceHasHeader", typeof(bool));
                template.Columns.Add("NumberOfParameters", typeof(int));
                template.Columns.Add("TemplateGroupId", typeof(int));
                template.Columns.Add("TemplateGroupStatus", typeof(bool));
                template.Columns.Add("IsActive", typeof(bool));
                template.Columns.Add("IsPrimary", typeof(bool));
                template.Columns.Add("ConditionQuery", typeof(string));

                DataRow newRow = template.Rows.Add();
                newRow["TemplateId"] = 0;
                newRow["TemplateName"] = templateDetail.TemplateName;
                newRow["SourceFolder"] = templateDetail.SourceFolder;
                newRow["SourceExtention"] = templateDetail.SourceExtention;
                newRow["SourceCompletionPath"] = templateDetail.SourceCompletionPath;
                newRow["SourceSubstringValue"] = templateDetail.SourceSubstringValue;
                newRow["SourceDelimiter"] = templateDetail.SourceDelimiter;
                newRow["SourceHasHeader"] = templateDetail.SourceHasHeader;
                newRow["NumberOfParameters"] = templateDetail.NumberOfParameters;
                newRow["TemplateGroupId"] = templateDetail.TemplateGroupId;
                newRow["TemplateGroupStatus"] = templateDetail.TemplateGroupStatus == null ? true : (bool)templateDetail.TemplateGroupStatus;
                newRow["IsActive"] = templateDetail.IsActive;
                newRow["IsPrimary"] = templateDetail.IsPrimary;
                newRow["ConditionQuery"] = templateDetail.ConditionQuery;
                
                _template.SaveTemplate(template);
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message.ToString());
            }
        }
        [HttpPost]
        [Route("api/UpdateTemplate")]
        public void UpdateTemplate([FromBody] ReconsileTemplate templateDetail)
        {
            try
            {
                var template = new DataTable();
                template.Columns.Add("TemplateId", typeof(int));
                template.Columns.Add("TemplateName", typeof(string));
                template.Columns.Add("SourceFolder", typeof(string));
                template.Columns.Add("SourceExtention", typeof(string));
                template.Columns.Add("SourceCompletionPath", typeof(string));
                template.Columns.Add("SourceSubstringValue", typeof(string));
                template.Columns.Add("SourceDelimiter", typeof(string));
                template.Columns.Add("SourceHasHeader", typeof(bool));
                template.Columns.Add("NumberOfParameters", typeof(int));
                template.Columns.Add("TemplateGroupId", typeof(int));
                template.Columns.Add("TemplateGroupStatus", typeof(bool));
                template.Columns.Add("IsActive", typeof(bool));
                template.Columns.Add("IsPrimary", typeof(bool));
                template.Columns.Add("ConditionQuery", typeof(string));

                DataRow newRow = template.Rows.Add();
                newRow["TemplateId"] = templateDetail.TemplateId;
                newRow["TemplateName"] = templateDetail.TemplateName;
                newRow["SourceFolder"] = templateDetail.SourceFolder;
                newRow["SourceExtention"] = templateDetail.SourceExtention;
                newRow["SourceCompletionPath"] = templateDetail.SourceCompletionPath;
                newRow["SourceSubstringValue"] = templateDetail.SourceSubstringValue;
                newRow["SourceDelimiter"] = templateDetail.SourceDelimiter;
                newRow["SourceHasHeader"] = templateDetail.SourceHasHeader;
                newRow["NumberOfParameters"] = templateDetail.NumberOfParameters;
                newRow["TemplateGroupId"] = templateDetail.TemplateGroupId;
                newRow["TemplateGroupStatus"] = templateDetail.TemplateGroupStatus == null ? true : (bool)templateDetail.TemplateGroupStatus;
                newRow["IsActive"] = templateDetail.IsActive;
                newRow["IsPrimary"] = templateDetail.IsPrimary;
                newRow["ConditionQuery"] = templateDetail.ConditionQuery;

                _template.UpdateTemplate(template);
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message.ToString());
            }
        }




        [HttpPost]
        [Route("api/DeleteTemplate")]
        public void DeleteTemplate([FromBody] int Id)
        {
            _template.DeleteTemplate(Id);
        }

        [HttpPost]
        [Route("api/SaveTemplateGroup")]
        public void SaveTemplateGroup([FromBody] TemplateGroup templateGroupDetail)
        {
            try
            {
                var templateGroup = new DataTable();
                templateGroup.Columns.Add("TemplateGroupId", typeof(int));
                templateGroup.Columns.Add("TemplateGroupName", typeof(string));
                templateGroup.Columns.Add("IsTemplateGroupActive", typeof(bool));


                DataRow newRow = templateGroup.Rows.Add();
                newRow["TemplateGroupId"] = 0;
                newRow["TemplateGroupName"] = templateGroupDetail.TemplateGroupName;
                newRow["IsTemplateGroupActive"] = templateGroupDetail.IsTemplateGroupActive;


                _template.SaveTemplateGroup(templateGroup);
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message.ToString());
            }
        }
        [HttpPost]
        [Route("api/UpdateTemplateGroup")]
        public void UpdateTemplateGroup([FromBody] TemplateGroup templateGroupDetail)
        {
            try
            {
                var templateGroup = new DataTable();
                templateGroup.Columns.Add("TemplateGroupId", typeof(int));
                templateGroup.Columns.Add("TemplateGroupName", typeof(string));
                templateGroup.Columns.Add("IsTemplateGroupActive", typeof(bool));


                DataRow newRow = templateGroup.Rows.Add();
                newRow["TemplateGroupId"] = templateGroupDetail.TemplateGroupId;
                newRow["TemplateGroupName"] = templateGroupDetail.TemplateGroupName;
                newRow["IsTemplateGroupActive"] = templateGroupDetail.IsTemplateGroupActive;


                _template.UpdateTemplateGroup(templateGroup);
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message.ToString());
            }
        }




        [HttpPost]
        [Route("api/DeleteTemplateGroup")]
        public void DeleteTemplateGroup([FromBody] int Id)
        {
            try
            {
                _template.DeleteTemplateGroup(Id);
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message.ToString());
            }
        }

        [HttpGet]
        [Route("api/GetDashboardData")]
        public IEnumerable<Dashboard> GetDashboardData(int groupTemplateId)
        {
            try
            {
                var records = _template.GetDashboardData(groupTemplateId);
                return records;
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message.ToString());
            }
        }
        [HttpGet]
        [Route("api/GetReconsileReportData")]
        public IEnumerable<ReconsileReportData> GetReconsileReportData(int groupTemplateId,DateTime fromDate,DateTime toDate)
        {
            try
            {
                var records = _template.GetReconsileReportData(groupTemplateId, fromDate, toDate);
                return records;
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message.ToString());
            }
        }

        [HttpGet]
        [Route("api/GetNonReconsileData")]
        public IEnumerable<NonReconsileData> GetNonReconsileData(int groupTemplateId, DateTime fromDate, DateTime toDate)
        {
            try
            {
                var records = _template.GetNonReconsileData(groupTemplateId, fromDate, toDate);
                return records;
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message.ToString());
            }
        }

        [HttpPost]
        [Route("api/ProcessManualReconsile")]
        public void ProcessManualReconsile([FromBody] List<ManualResult> selectedResult)
        {
            try
            {
                var slectedResults = new DataTable();
                slectedResults.Columns.Add("Id", typeof(int));
                slectedResults.Columns.Add("Type", typeof(string));
                slectedResults.Columns.Add("ReasonDesc", typeof(string));

                foreach (var result in selectedResult)
                {
                    DataRow newRow = slectedResults.Rows.Add();
                    newRow["Id"] = result.Id;
                    newRow["Type"] = result.Type;
                    newRow["ReasonDesc"] = result.ReasonDesc;
                }
               
                _template.ProcessManualReconsile(slectedResults);
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message.ToString());
            }
        }
    }
}