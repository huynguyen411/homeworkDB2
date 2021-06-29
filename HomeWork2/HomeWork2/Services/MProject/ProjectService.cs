using HomeWork2.Common;
using HomeWork2.Entities;
using HomeWork2.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeWork2.Services.MProject
{
    public interface IProjectService : IServiceScoped
    {
        Task<int> Count(ProjectFilter ProjectFilter);
        Task<List<Project>> List(ProjectFilter ProjectFilter);
        Task<Project> Get(long Id);
        Task<Project> Create(Project Project);
        Task<Project> Update(Project Project);
        Task<Project> Delete(Project Project);
        Task<List<Project>> BulkMerge(List<Project> Projects);
        Task<List<Project>> BulkDelete(List<Project> Projects);
    }
    public class ProjectService : IProjectService
    {
        private IUOW UOW;
        private IProjectValidator ProjectValidator;
        public ProjectService(IUOW UOW, IProjectValidator ProjectValidator)
        {
            this.UOW = UOW;
            this.ProjectValidator = ProjectValidator;
        }
        public async Task<int> Count(ProjectFilter ProjectFilter)
        {
            try
            {
                int result = await UOW.ProjectRepository.Count(ProjectFilter);
                return result;
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                {
                    throw new MessageException(ex);
                }
                else
                {
                    throw new MessageException(ex.InnerException);
                }
            }
        }
        public async Task<List<Project>> List(ProjectFilter ProjectFilter)
        {
            try
            {
                List<Project> Projects = await UOW.ProjectRepository.List(ProjectFilter);
                return Projects;
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                {
                    throw new MessageException(ex);
                }
                else
                {
                    throw new MessageException(ex.InnerException);
                }
            }
        }
        public async Task<Project> Get(long Id)
        {
            Project Project = await UOW.ProjectRepository.Get(Id);
            if (Project == null)
                return null;
            return Project;
        }
        public async Task<Project> Create(Project Project)
        {
            if (!await ProjectValidator.Create(Project))
                return Project;
            try
            {
                await UOW.Begin();
                await UOW.ProjectRepository.Create(Project);
                await UOW.Commit();

                var Projects = await UOW.ProjectRepository.List(new List<long> { Project.Id });
                Project = Projects.FirstOrDefault();
                return Project;
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                {
                    throw new MessageException(ex);
                }
                else
                {
                    throw new MessageException(ex.InnerException);
                }
            }

        }
        public async Task<Project> Update(Project Project)
        {
            if (!await ProjectValidator.Update(Project))
                return Project;
            try
            {
                await UOW.Begin();
                await UOW.ProjectRepository.Update(Project);
                await UOW.Commit();

                var Projects = await UOW.ProjectRepository.List(new List<long> { Project.Id });
                Project = Projects.FirstOrDefault();
                return Project;
            }
            catch (Exception ex)
            {
                await UOW.Rollback();
                if (ex.InnerException == null)
                {
                    throw new MessageException(ex);
                }
                else
                {
                    throw new MessageException(ex.InnerException);
                }
            }
        }
        public async Task<Project> Delete(Project Project)
        {
            try
            {
                await UOW.Begin();
                await UOW.ProjectRepository.Delete(Project);
                await UOW.Commit();

                var Companies = await UOW.ProjectRepository.List(new List<long> { Project.Id });
                Project = Companies.FirstOrDefault();
                return Project;
            }
            catch (Exception ex)
            {
                await UOW.Rollback();
                if (ex.InnerException == null)
                {
                    throw new MessageException(ex);
                }
                else
                {
                    throw new MessageException(ex.InnerException);
                }
            }
        }
        public async Task<List<Project>> BulkMerge(List<Project> Projects)
        {
            try
            {
                await UOW.Begin();
                await UOW.ProjectRepository.BulkMerge(Projects);
                await UOW.Commit();

                var Ids = Projects.Select(x => x.Id).ToList();
                Projects = await UOW.ProjectRepository.List(Ids);
                return Projects;
            }
            catch (Exception ex)
            {
                await UOW.Rollback();
                if (ex.InnerException == null)
                {
                    throw new MessageException(ex);
                }
                else
                {
                    throw new MessageException(ex.InnerException);
                }
            }
        }
        public async Task<List<Project>> BulkDelete(List<Project> Projects)
        {
            try
            {
                await UOW.Begin();
                await UOW.ProjectRepository.BulkDelete(Projects);
                await UOW.Commit();

                var Ids = Projects.Select(x => x.Id).ToList();
                Projects = await UOW.ProjectRepository.List(Ids);
                return Projects;
            }
            catch (Exception ex)
            {
                await UOW.Rollback();
                if (ex.InnerException == null)
                {
                    throw new MessageException(ex);
                }
                else
                {
                    throw new MessageException(ex.InnerException);
                }
            }
        }
    }
}
