using HomeWork2.Common;
using HomeWork2.Entities;
using HomeWork2.Services.MProject;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeWork2.Controllers.project
{
    public class ProjectController : ControllerBase
    {
        private IProjectService ProjectService;
        public ProjectController(IProjectService ProjectService)
        {
            this.ProjectService = ProjectService;
        }
        [Route(ProjectRoute.Count), HttpPost]
        public async Task<ActionResult<int>> Count([FromBody] Project_ProjectFilterDTO Project_ProjectFilterDTO)
        {

            ProjectFilter ProjectFilter = ConvertFilterDTOToFilterEntity(Project_ProjectFilterDTO);
            int count = await ProjectService.Count(ProjectFilter);
            return count;
        }

        [Route(ProjectRoute.List), HttpPost]
        public async Task<ActionResult<List<Project_ProjectDTO>>> List([FromBody] Project_ProjectFilterDTO Project_ProjectFilterDTO)
        {

            ProjectFilter ProjectFilter = ConvertFilterDTOToFilterEntity(Project_ProjectFilterDTO);
            List<Project> Projects = await ProjectService.List(ProjectFilter);
            List<Project_ProjectDTO> Project_ProjectDTOs = Projects
                .Select(c => new Project_ProjectDTO(c)).ToList();
            return Project_ProjectDTOs;
        }

        [Route(ProjectRoute.Get), HttpPost]
        public async Task<ActionResult<Project_ProjectDTO>> Get([FromBody] Project_ProjectDTO Project_ProjectDTO)
        {

            Project Project = await ProjectService.Get(Project_ProjectDTO.Id);
            return new Project_ProjectDTO(Project);
        }

        [Route(ProjectRoute.Create), HttpPost]
        public async Task<ActionResult<Project_ProjectDTO>> Create([FromBody] Project_ProjectDTO Project_ProjectDTO)
        {

            Project Project = ConvertDTOToEntity(Project_ProjectDTO);
            Project = await ProjectService.Create(Project);
            Project_ProjectDTO = new Project_ProjectDTO(Project);
            if (Project.IsValidated)
                return Project_ProjectDTO;
            else
                return BadRequest(Project_ProjectDTO);
        }

        [Route(ProjectRoute.Update), HttpPost]
        public async Task<ActionResult<Project_ProjectDTO>> Update([FromBody] Project_ProjectDTO Project_ProjectDTO)
        {

            Project Project = ConvertDTOToEntity(Project_ProjectDTO);
            Project = await ProjectService.Update(Project);
            Project_ProjectDTO = new Project_ProjectDTO(Project);
            if (Project.IsValidated)
                return Project_ProjectDTO;
            else
                return BadRequest(Project_ProjectDTO);
        }

        [Route(ProjectRoute.Delete), HttpPost]
        public async Task<ActionResult<Project_ProjectDTO>> Delete([FromBody] Project_ProjectDTO Project_ProjectDTO)
        {
            Project Project = ConvertDTOToEntity(Project_ProjectDTO);
            Project = await ProjectService.Delete(Project);
            Project_ProjectDTO = new Project_ProjectDTO(Project);
            if (Project.IsValidated)
                return Project_ProjectDTO;
            else
                return BadRequest(Project_ProjectDTO);
        }

        [Route(ProjectRoute.BulkDelete), HttpPost]
        public async Task<ActionResult<bool>> BulkDelete([FromBody] List<long> Ids)
        {
            ProjectFilter ProjectFilter = new ProjectFilter();
            ProjectFilter.Id = new IdFilter { In = Ids };
            ProjectFilter.Selects = ProjectSelect.Id;
            ProjectFilter.Skip = 0;
            ProjectFilter.Take = int.MaxValue;

            List<Project> Projects = await ProjectService.List(ProjectFilter);
            Projects = await ProjectService.BulkDelete(Projects);
            if (Projects.Any(x => !x.IsValidated))
                return BadRequest(Projects.Where(x => !x.IsValidated));
            return true;
        }

        private Project ConvertDTOToEntity(Project_ProjectDTO Project_ProjectDTO)
        {
            Project Project = new Project();
            Project.Id = Project_ProjectDTO.Id;
            Project.Code = Project_ProjectDTO.Code;
            Project.Name = Project_ProjectDTO.Name;
            Project.StartAt = Project_ProjectDTO.StartAt;
            Project.EndAt = Project_ProjectDTO.EndAt;
            return Project;
        }

        private ProjectFilter ConvertFilterDTOToFilterEntity(Project_ProjectFilterDTO Project_ProjectFilterDTO)
        {
            ProjectFilter ProjectFilter = new ProjectFilter();
            ProjectFilter.Selects = ProjectSelect.ALL;
            ProjectFilter.Skip = Project_ProjectFilterDTO.Skip;
            ProjectFilter.Take = Project_ProjectFilterDTO.Take;
            ProjectFilter.OrderBy = Project_ProjectFilterDTO.OrderBy;
            ProjectFilter.OrderType = Project_ProjectFilterDTO.OrderType;

            ProjectFilter.Id = Project_ProjectFilterDTO.Id;
            ProjectFilter.Code = Project_ProjectFilterDTO.Code;
            ProjectFilter.Name = Project_ProjectFilterDTO.Name;
            ProjectFilter.StartAt = Project_ProjectFilterDTO.StartAt;
            ProjectFilter.EndAt = Project_ProjectFilterDTO.EndAt;
            return ProjectFilter;
        }
    }
}
