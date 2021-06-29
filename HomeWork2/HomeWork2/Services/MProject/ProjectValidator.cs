using HomeWork2.Common;
using HomeWork2.Entities;
using HomeWork2.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeWork2.Services.MProject
{
    public interface IProjectValidator : IServiceScoped
    {
        Task<bool> Create(Project Project);
        Task<bool> Update(Project Project);
    }
    public class ProjectValidator : IProjectValidator
    {
        public enum ErrorCode
        {
            IdNotExisted,
            CodeExisted,
            StartAtGreaterThanEndAt
        }
        private IUOW UOW;
        public ProjectValidator(IUOW UOW)
        {
            this.UOW = UOW;
        }
        public async Task<bool> ValidateId(Project Project)
        {
            ProjectFilter ProjectFilter = new ProjectFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = Project.Id },
                Selects = ProjectSelect.Id
            };

            int count = await UOW.ProjectRepository.Count(ProjectFilter);
            if (count == 0)
                Project.AddError(nameof(ProjectValidator), nameof(Project.Id), ErrorCode.IdNotExisted);
            return Project.IsValidated;
        }

        public async Task<bool> ValidateCode(Project Project)
        {
            ProjectFilter ProjectFilter = new ProjectFilter
            {
                Skip = 0,
                Take = 10,
                Code = new StringFilter { Equal = Project.Code },
                Selects = ProjectSelect.Code
            };
            int count = await UOW.ProjectRepository.Count(ProjectFilter);
            if (count != 0)
                Project.AddError(nameof(ProjectValidator), nameof(Project.Code), ErrorCode.CodeExisted);
            return Project.IsValidated;
        }
        public async Task<bool> ValidateTime(Project Project)
        {
            if (Project.StartAt > Project.EndAt)
            {
                Project.AddError(nameof(ProjectValidator), nameof(Project.Code), ErrorCode.StartAtGreaterThanEndAt);
            }
            return Project.IsValidated;
        }
        public async Task<bool> Create(Project Project)
        {
            await ValidateCode(Project);
            await ValidateTime(Project);
            return Project.IsValidated;
        }

        public async Task<bool> Update(Project Project)
        {
            if (await ValidateId(Project))
            {
                await ValidateCode(Project);
                await ValidateTime(Project);
            }
            return Project.IsValidated;
        }
    }
}
