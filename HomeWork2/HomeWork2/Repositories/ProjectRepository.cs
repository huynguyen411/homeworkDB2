using HomeWork2.Entities;
using HomeWork2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeWork2.Common;
using Microsoft.EntityFrameworkCore;

namespace HomeWork2.Repositories
{
    public interface IProjectRepository
    {
        Task<int> Count(ProjectFilter ProjectFilter);
        Task<List<Project>> List(ProjectFilter ProjectFilter);
        Task<List<Project>> List(List<long> Ids);
        Task<Project> Get(long Id);
        Task<bool> Create(Project Project);
        Task<bool> Update(Project Project);
        Task<bool> Delete(Project Project);
        Task<bool> BulkMerge(List<Project> Projects);
        Task<bool> BulkDelete(List<Project> Projects);
    }

    public class ProjectRepository : IProjectRepository
    {
        private DataContext DataContext;
        public ProjectRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }
        private IQueryable<ProjectDAO> DynamicFilter(IQueryable<ProjectDAO> query, ProjectFilter filter)
        {
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Code, filter.Code);
            query = query.Where(q => q.Name, filter.Name);
            query = query.Where(q => q.StartAt, filter.StartAt);
            query = query.Where(q => q.EndAt, filter.EndAt);
            return query;
        }
        private async Task<List<Project>> DynamicSelect(IQueryable<ProjectDAO> query, ProjectFilter filter)
        {
            List<Project> Projects = await query.Select(q => new Project()
            {
                Id = filter.Selects.Contains(ProjectSelect.Id) ? q.Id : default(long),
                Name = filter.Selects.Contains(ProjectSelect.Name) ? q.Name : default(string),
                Code = filter.Selects.Contains(ProjectSelect.Code) ? q.Code : default(string),
                StartAt = q.StartAt,
                EndAt = q.EndAt,
            }).ToListAsync();
            return Projects;
        }

        private IQueryable<ProjectDAO> DynamicOrder(IQueryable<ProjectDAO> query, ProjectFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case ProjectOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case ProjectOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                        case ProjectOrder.Code:
                            query = query.OrderBy(q => q.Code);
                            break;
                        case ProjectOrder.StartAt:
                            query = query.OrderBy(q => q.StartAt);
                            break;
                        case ProjectOrder.EndAt:
                            query = query.OrderBy(q => q.EndAt);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case ProjectOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case ProjectOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                        case ProjectOrder.Code:
                            query = query.OrderByDescending(q => q.Code);
                            break;
                        case ProjectOrder.StartAt:
                            query = query.OrderByDescending(q => q.StartAt);
                            break;
                        case ProjectOrder.EndAt:
                            query = query.OrderByDescending(q => q.EndAt);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        public async Task<Project> Get(long Id)
        {
            Project Project = await DataContext.Project.
                Where(x => x.Id == Id).Select(x => new Project()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Code = x.Code,
                    StartAt = x.StartAt,
                    EndAt = x.EndAt
                }).FirstOrDefaultAsync();
            if (Project == null)
            {
                return null;
            }
            return Project;
        }

        public async Task<bool> Create(Project Project)
        {
            ProjectDAO ProjectDAO = new ProjectDAO();
            ProjectDAO.Id = Project.Id;
            ProjectDAO.Name = Project.Name;
            ProjectDAO.Code = Project.Code;
            ProjectDAO.StartAt = Project.StartAt;
            ProjectDAO.EndAt = Project.EndAt;
            DataContext.Project.Add(ProjectDAO);
            await DataContext.SaveChangesAsync();
            Project.Id = ProjectDAO.Id;
            return true;
        }

        public async Task<bool> Update(Project Project)
        {
            ProjectDAO ProjectDAO = DataContext.Project.Where(x => x.Id == Project.Id).FirstOrDefault();
            if (ProjectDAO == null)
                return false;
            ProjectDAO.Id = Project.Id;
            ProjectDAO.Name = Project.Name;
            ProjectDAO.Code = Project.Code;
            ProjectDAO.StartAt = Project.StartAt;
            ProjectDAO.EndAt = Project.EndAt;
            await DataContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(Project Project)
        {
            await DataContext.Project.Where(x => x.Id == Project.Id).DeleteFromQueryAsync();
            return true;
        }
        public async Task<bool> BulkMerge(List<Project> Projects)
        {
            List<ProjectDAO> ProjectDAOs = new List<ProjectDAO>();
            foreach (Project Project in Projects)
            {
                ProjectDAO ProjectDAO = new ProjectDAO();
                ProjectDAO.Id = Project.Id;
                ProjectDAO.Name = Project.Name;
                ProjectDAO.Code = Project.Code;
                ProjectDAO.StartAt = Project.StartAt;
                ProjectDAO.EndAt = Project.EndAt;
                ProjectDAOs.Add(ProjectDAO);
            }
            await DataContext.BulkMergeAsync(ProjectDAOs);
            return true;
        }
        public async Task<bool> BulkDelete(List<Project> Projects)
        {
            List<long> Ids = Projects.Select(x => x.Id).ToList();
            await DataContext.Project
                .Where(x => Ids.Contains(x.Id))
                .DeleteFromQueryAsync();
            return true;
        }
        public async Task<int> Count(ProjectFilter ProjectFilter)
        {
            IQueryable<ProjectDAO> Projects = DataContext.Project.AsNoTracking();
            Projects = DynamicFilter(Projects, ProjectFilter);
            return await Projects.CountAsync();
        }

        public async Task<List<Project>> List(ProjectFilter filter)
        {
            if (filter == null)
                return new List<Project>();
            IQueryable<ProjectDAO> ProjectDAOs = DataContext.Project.AsNoTracking();
            ProjectDAOs = DynamicFilter(ProjectDAOs, filter);
            ProjectDAOs = DynamicOrder(ProjectDAOs, filter);
            List<Project> Projects = await DynamicSelect(ProjectDAOs, filter);
            return Projects;
        }
        public async Task<List<Project>> List(List<long> Ids)
        {
            List<Project> Projects = await DataContext.Project.AsNoTracking()
                .Where(x => Ids.Contains(x.Id)).Select(x => new Project()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Code = x.Code,
                    StartAt = x.StartAt,
                    EndAt = x.EndAt
                }).ToListAsync();
            return Projects;
        }
    }
}
