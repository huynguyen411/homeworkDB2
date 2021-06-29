using HomeWork2.Common;
using HomeWork2.Entities;
using HomeWork2.Services;
using HomeWork2.Services.MBudget;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeWork2.Controllers.budget
{
    public class BudgetController : ControllerBase
    {
        private IBudgetService BudgetService;
        public BudgetController(IBudgetService BudgetService)
        {
            this.BudgetService = BudgetService;
        }
        [Route(BudgetRoute.Count), HttpPost]
        public async Task<ActionResult<int>> Count([FromBody] Budget_BudgetFilterDTO Budget_BudgetFilterDTO)
        {

            BudgetFilter BudgetFilter = ConvertFilterDTOToFilterEntity(Budget_BudgetFilterDTO);
            int count = await BudgetService.Count(BudgetFilter);
            return count;
        }

        [Route(BudgetRoute.List), HttpPost]
        public async Task<ActionResult<List<Budget_BudgetDTO>>> List([FromBody] Budget_BudgetFilterDTO Budget_BudgetFilterDTO)
        {

            BudgetFilter BudgetFilter = ConvertFilterDTOToFilterEntity(Budget_BudgetFilterDTO);
            List<Budget> Budgets = await BudgetService.List(BudgetFilter);
            List<Budget_BudgetDTO> Budget_BudgetDTOs = Budgets
                .Select(c => new Budget_BudgetDTO(c)).ToList();
            return Budget_BudgetDTOs;
        }

        [Route(BudgetRoute.Get), HttpPost]
        public async Task<ActionResult<Budget_BudgetDTO>> Get([FromBody] Budget_BudgetDTO Budget_BudgetDTO)
        {

            Budget Budget = await BudgetService.Get(Budget_BudgetDTO.Id);
            return new Budget_BudgetDTO(Budget);
        }

        [Route(BudgetRoute.Create), HttpPost]
        public async Task<ActionResult<Budget_BudgetDTO>> Create([FromBody] Budget_BudgetDTO Budget_BudgetDTO)
        {

            Budget Budget = ConvertDTOToEntity(Budget_BudgetDTO);
            Budget = await BudgetService.Create(Budget);
            Budget_BudgetDTO = new Budget_BudgetDTO(Budget);
            if (Budget.IsValidated)
                return Budget_BudgetDTO;
            else
                return BadRequest(Budget_BudgetDTO);
        }

        [Route(BudgetRoute.Update), HttpPost]
        public async Task<ActionResult<Budget_BudgetDTO>> Update([FromBody] Budget_BudgetDTO Budget_BudgetDTO)
        {

            Budget Budget = ConvertDTOToEntity(Budget_BudgetDTO);
            Budget = await BudgetService.Update(Budget);
            Budget_BudgetDTO = new Budget_BudgetDTO(Budget);
            if (Budget.IsValidated)
                return Budget_BudgetDTO;
            else
                return BadRequest(Budget_BudgetDTO);
        }

        [Route(BudgetRoute.Delete), HttpPost]
        public async Task<ActionResult<Budget_BudgetDTO>> Delete([FromBody] Budget_BudgetDTO Budget_BudgetDTO)
        {
            Budget Budget = ConvertDTOToEntity(Budget_BudgetDTO);
            Budget = await BudgetService.Delete(Budget);
            Budget_BudgetDTO = new Budget_BudgetDTO(Budget);
            if (Budget.IsValidated)
                return Budget_BudgetDTO;
            else
                return BadRequest(Budget_BudgetDTO);
        }

        [Route(BudgetRoute.BulkDelete), HttpPost]
        public async Task<ActionResult<bool>> BulkDelete([FromBody] List<long> Ids)
        {
            BudgetFilter BudgetFilter = new BudgetFilter();
            BudgetFilter.Id = new IdFilter { In = Ids };
            BudgetFilter.Selects = BudgetSelect.Id;
            BudgetFilter.Skip = 0;
            BudgetFilter.Take = int.MaxValue;

            List<Budget> Budgets = await BudgetService.List(BudgetFilter);
            Budgets = await BudgetService.BulkDelete(Budgets);
            if (Budgets.Any(x => !x.IsValidated))
                return BadRequest(Budgets.Where(x => !x.IsValidated));
            return true;
        }

        private Budget ConvertDTOToEntity(Budget_BudgetDTO Budget_BudgetDTO)
        {
            Budget Budget = new Budget();
            Budget.Id = Budget_BudgetDTO.Id;
            Budget.BudgetTypeId = Budget_BudgetDTO.BudgetTypeId;
            Budget.ProjectId = Budget_BudgetDTO.ProjectId;
            Budget.CompanyId = Budget_BudgetDTO.CompanyId;
            Budget.Amount = Budget_BudgetDTO.Amount;
            Budget.Month = Budget_BudgetDTO.Month;
            Budget.Year = Budget_BudgetDTO.Year;
            Budget.BudgetType = new BudgetType()
            {
                Id = Budget_BudgetDTO.BudgetType.Id,
                Code = Budget_BudgetDTO.BudgetType.Code,
                Name = Budget_BudgetDTO.BudgetType.Name,
            };
            Budget.Project = new Project()
            {
                Id = Budget_BudgetDTO.Project.Id,
                Code = Budget_BudgetDTO.Project.Code,
                Name = Budget_BudgetDTO.Project.Name,
                StartAt = Budget_BudgetDTO.Project.StartAt,
                EndAt = Budget_BudgetDTO.Project.EndAt,
            };
            Budget.Company = new Company()
            {
                Id = Budget_BudgetDTO.Company.Id,
                Code = Budget_BudgetDTO.Company.Code,
                Name = Budget_BudgetDTO.Company.Name,
                ParentId = Budget_BudgetDTO.Company.ParentId,
                Level = Budget_BudgetDTO.Company.Level,
            };
            return Budget;
        }

        private BudgetFilter ConvertFilterDTOToFilterEntity(Budget_BudgetFilterDTO Budget_BudgetFilterDTO)
        {
            BudgetFilter BudgetFilter = new BudgetFilter();
            BudgetFilter.Selects = BudgetSelect.ALL;
            BudgetFilter.Skip = Budget_BudgetFilterDTO.Skip;
            BudgetFilter.Take = Budget_BudgetFilterDTO.Take;
            BudgetFilter.OrderBy = Budget_BudgetFilterDTO.OrderBy;
            BudgetFilter.OrderType = Budget_BudgetFilterDTO.OrderType;

            BudgetFilter.Id = Budget_BudgetFilterDTO.Id;
            BudgetFilter.BudgetTypeId = Budget_BudgetFilterDTO.BudgetTypeId;
            BudgetFilter.ProjectId = Budget_BudgetFilterDTO.ProjectId;
            BudgetFilter.CompanyId = Budget_BudgetFilterDTO.CompanyId;
            BudgetFilter.Amount = Budget_BudgetFilterDTO.Amount;
            BudgetFilter.AccumulatedAmount = Budget_BudgetFilterDTO.AccumulatedAmount;
            BudgetFilter.Month = Budget_BudgetFilterDTO.Month;
            BudgetFilter.Year = Budget_BudgetFilterDTO.Year;
            return BudgetFilter;
        }
    }
}
