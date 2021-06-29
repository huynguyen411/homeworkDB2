using HomeWork2.Common;
using HomeWork2.Entities;
using HomeWork2.Services;
using HomeWork2.Services.MBudget;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeWork2.Controllers.company
{
    public class CompanyController : ControllerBase
    {
        private ICompanyService CompanyService;
        public CompanyController(ICompanyService CompanyService)
        {
            this.CompanyService = CompanyService;
        }
        [Route(CompanyRoute.Count), HttpPost]
        public async Task<ActionResult<int>> Count([FromBody] Company_CompanyFilterDTO Company_CompanyFilterDTO)
        {

            CompanyFilter CompanyFilter = ConvertFilterDTOToFilterEntity(Company_CompanyFilterDTO);
            int count = await CompanyService.Count(CompanyFilter);
            return count;
        }

        [Route(CompanyRoute.List), HttpPost]
        public async Task<ActionResult<List<Company_CompanyDTO>>> List([FromBody] Company_CompanyFilterDTO Company_CompanyFilterDTO)
        {

            CompanyFilter CompanyFilter = ConvertFilterDTOToFilterEntity(Company_CompanyFilterDTO);
            List<Company> Companys = await CompanyService.List(CompanyFilter);
            List<Company_CompanyDTO> Company_CompanyDTOs = Companys
                .Select(c => new Company_CompanyDTO(c)).ToList();
            return Company_CompanyDTOs;
        }

        [Route(CompanyRoute.Get), HttpPost]
        public async Task<ActionResult<Company_CompanyDTO>> Get([FromBody] Company_CompanyDTO Company_CompanyDTO)
        {

            Company Company = await CompanyService.Get(Company_CompanyDTO.Id);
            return new Company_CompanyDTO(Company);
        }

        [Route(CompanyRoute.Create), HttpPost]
        public async Task<ActionResult<Company_CompanyDTO>> Create([FromBody] Company_CompanyDTO Company_CompanyDTO)
        {

            Company Company = ConvertDTOToEntity(Company_CompanyDTO);
            Company = await CompanyService.Create(Company);
            Company_CompanyDTO = new Company_CompanyDTO(Company);
            if (Company.IsValidated)
                return Company_CompanyDTO;
            else
                return BadRequest(Company_CompanyDTO);
        }

        [Route(CompanyRoute.Update), HttpPost]
        public async Task<ActionResult<Company_CompanyDTO>> Update([FromBody] Company_CompanyDTO Company_CompanyDTO)
        {

            Company Company = ConvertDTOToEntity(Company_CompanyDTO);
            Company = await CompanyService.Update(Company);
            Company_CompanyDTO = new Company_CompanyDTO(Company);
            if (Company.IsValidated)
                return Company_CompanyDTO;
            else
                return BadRequest(Company_CompanyDTO);
        }

        [Route(CompanyRoute.Delete), HttpPost]
        public async Task<ActionResult<Company_CompanyDTO>> Delete([FromBody] Company_CompanyDTO Company_CompanyDTO)
        {
            Company Company = ConvertDTOToEntity(Company_CompanyDTO);
            Company = await CompanyService.Delete(Company);
            Company_CompanyDTO = new Company_CompanyDTO(Company);
            if (Company.IsValidated)
                return Company_CompanyDTO;
            else
                return BadRequest(Company_CompanyDTO);
        }

        [Route(CompanyRoute.BulkDelete), HttpPost]
        public async Task<ActionResult<bool>> BulkDelete([FromBody] List<long> Ids)
        {
            CompanyFilter CompanyFilter = new CompanyFilter();
            CompanyFilter.Id = new IdFilter { In = Ids };
            CompanyFilter.Selects = CompanySelect.Id;
            CompanyFilter.Skip = 0;
            CompanyFilter.Take = int.MaxValue;

            List<Company> Companys = await CompanyService.List(CompanyFilter);
            Companys = await CompanyService.BulkDelete(Companys);
            if (Companys.Any(x => !x.IsValidated))
                return BadRequest(Companys.Where(x => !x.IsValidated));
            return true;
        }

        private Company ConvertDTOToEntity(Company_CompanyDTO Company_CompanyDTO)
        {
            Company Company = new Company();
            Company.Id = Company_CompanyDTO.Id;
            Company.Code = Company_CompanyDTO.Code;
            Company.Name = Company_CompanyDTO.Name;
            Company.ParentId = Company_CompanyDTO.ParentId;
            Company.Level = Company_CompanyDTO.Level;
            Company.Parent = Company_CompanyDTO.Parent == null ? null : new Company
            {
                Id = Company_CompanyDTO.Parent.Id,
                Code = Company_CompanyDTO.Parent.Code,
                Name = Company_CompanyDTO.Parent.Name,
                ParentId = Company_CompanyDTO.Parent.ParentId,
                Level = Company_CompanyDTO.Parent.Level
            };
            return Company;
        }

        private CompanyFilter ConvertFilterDTOToFilterEntity(Company_CompanyFilterDTO Company_CompanyFilterDTO)
        {
            CompanyFilter CompanyFilter = new CompanyFilter();
            CompanyFilter.Selects = CompanySelect.ALL;
            CompanyFilter.Skip = Company_CompanyFilterDTO.Skip;
            CompanyFilter.Take = Company_CompanyFilterDTO.Take;
            CompanyFilter.OrderBy = Company_CompanyFilterDTO.OrderBy;
            CompanyFilter.OrderType = Company_CompanyFilterDTO.OrderType;

            CompanyFilter.Id = Company_CompanyFilterDTO.Id;
            CompanyFilter.Code = Company_CompanyFilterDTO.Code;
            CompanyFilter.Name = Company_CompanyFilterDTO.Name;
            CompanyFilter.ParentId = Company_CompanyFilterDTO.ParentId;
            CompanyFilter.Level = Company_CompanyFilterDTO.Level;
            return CompanyFilter;
        }
    }
}
