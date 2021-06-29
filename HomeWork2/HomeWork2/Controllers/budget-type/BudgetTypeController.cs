using HomeWork2.Common;
using HomeWork2.Entities;
using HomeWork2.Services.MBudgetType;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeWork2.Controllers.budget_type
{ 
    public class BudgetTypeController : ControllerBase
    {
        private IBudgetTypeService BudgetTypeService;
        public BudgetTypeController(IBudgetTypeService BudgetTypeService)
        {
            this.BudgetTypeService = BudgetTypeService;
        }
        [Route(BudgetTypeRoute.Count), HttpPost]
        public async Task<ActionResult<int>> Count([FromBody] BudgetType_BudgetTypeFilterDTO BudgetType_BudgetTypeFilterDTO)
        {

            BudgetTypeFilter BudgetTypeFilter = ConvertFilterDTOToFilterEntity(BudgetType_BudgetTypeFilterDTO);
            int count = await BudgetTypeService.Count(BudgetTypeFilter);
            return count;
        }

        [Route(BudgetTypeRoute.List), HttpPost]
        public async Task<ActionResult<List<BudgetType_BudgetTypeDTO>>> List([FromBody] BudgetType_BudgetTypeFilterDTO BudgetType_BudgetTypeFilterDTO)
        {

            BudgetTypeFilter BudgetTypeFilter = ConvertFilterDTOToFilterEntity(BudgetType_BudgetTypeFilterDTO);
            List<BudgetType> BudgetTypes = await BudgetTypeService.List(BudgetTypeFilter);
            List<BudgetType_BudgetTypeDTO> BudgetType_BudgetTypeDTOs = BudgetTypes
                .Select(c => new BudgetType_BudgetTypeDTO(c)).ToList();
            return BudgetType_BudgetTypeDTOs;
        }

        [Route(BudgetTypeRoute.Get), HttpPost]
        public async Task<ActionResult<BudgetType_BudgetTypeDTO>> Get([FromBody] BudgetType_BudgetTypeDTO BudgetType_BudgetTypeDTO)
        {

            BudgetType BudgetType = await BudgetTypeService.Get(BudgetType_BudgetTypeDTO.Id);
            return new BudgetType_BudgetTypeDTO(BudgetType);
        }

        [Route(BudgetTypeRoute.Create), HttpPost]
        public async Task<ActionResult<BudgetType_BudgetTypeDTO>> Create([FromBody] BudgetType_BudgetTypeDTO BudgetType_BudgetTypeDTO)
        {

            BudgetType BudgetType = ConvertDTOToEntity(BudgetType_BudgetTypeDTO);
            BudgetType = await BudgetTypeService.Create(BudgetType);
            BudgetType_BudgetTypeDTO = new BudgetType_BudgetTypeDTO(BudgetType);
            if (BudgetType.IsValidated)
                return BudgetType_BudgetTypeDTO;
            else
                return BadRequest(BudgetType_BudgetTypeDTO);
        }

        [Route(BudgetTypeRoute.Update), HttpPost]
        public async Task<ActionResult<BudgetType_BudgetTypeDTO>> Update([FromBody] BudgetType_BudgetTypeDTO BudgetType_BudgetTypeDTO)
        {

            BudgetType BudgetType = ConvertDTOToEntity(BudgetType_BudgetTypeDTO);
            BudgetType = await BudgetTypeService.Update(BudgetType);
            BudgetType_BudgetTypeDTO = new BudgetType_BudgetTypeDTO(BudgetType);
            if (BudgetType.IsValidated)
                return BudgetType_BudgetTypeDTO;
            else
                return BadRequest(BudgetType_BudgetTypeDTO);
        }

        [Route(BudgetTypeRoute.Delete), HttpPost]
        public async Task<ActionResult<BudgetType_BudgetTypeDTO>> Delete([FromBody] BudgetType_BudgetTypeDTO BudgetType_BudgetTypeDTO)
        {
            BudgetType BudgetType = ConvertDTOToEntity(BudgetType_BudgetTypeDTO);
            BudgetType = await BudgetTypeService.Delete(BudgetType);
            BudgetType_BudgetTypeDTO = new BudgetType_BudgetTypeDTO(BudgetType);
            if (BudgetType.IsValidated)
                return BudgetType_BudgetTypeDTO;
            else
                return BadRequest(BudgetType_BudgetTypeDTO);
        }

        private BudgetType ConvertDTOToEntity(BudgetType_BudgetTypeDTO BudgetType_BudgetTypeDTO)
        {
            BudgetType BudgetType = new BudgetType();
            BudgetType.Id = BudgetType_BudgetTypeDTO.Id;
            BudgetType.Code = BudgetType_BudgetTypeDTO.Code;
            BudgetType.Name = BudgetType_BudgetTypeDTO.Name;
            return BudgetType;
        }

        private BudgetTypeFilter ConvertFilterDTOToFilterEntity(BudgetType_BudgetTypeFilterDTO BudgetType_BudgetTypeFilterDTO)
        {
            BudgetTypeFilter BudgetTypeFilter = new BudgetTypeFilter();
            BudgetTypeFilter.Selects = BudgetTypeSelect.ALL;
            BudgetTypeFilter.Skip = BudgetType_BudgetTypeFilterDTO.Skip;
            BudgetTypeFilter.Take = BudgetType_BudgetTypeFilterDTO.Take;
            BudgetTypeFilter.OrderBy = BudgetType_BudgetTypeFilterDTO.OrderBy;
            BudgetTypeFilter.OrderType = BudgetType_BudgetTypeFilterDTO.OrderType;

            BudgetTypeFilter.Id = BudgetType_BudgetTypeFilterDTO.Id;
            BudgetTypeFilter.Code = BudgetType_BudgetTypeFilterDTO.Code;
            BudgetTypeFilter.Name = BudgetType_BudgetTypeFilterDTO.Name;
            return BudgetTypeFilter;
        }
    }
}
