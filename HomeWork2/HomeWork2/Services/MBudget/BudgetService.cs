using HomeWork2.Common;
using HomeWork2.Entities;
using HomeWork2.Repositories;
using HomeWork2.Services.MBudget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeWork2.Services
{
    public interface IBudgetService : IServiceScoped
    {
        Task<int> Count(BudgetFilter BudgetFilter);
        Task<List<Budget>> List(BudgetFilter BudgetFilter);
        Task<Budget> Get(long Id);
        Task<Budget> Create(Budget Budget);
        Task<Budget> Update(Budget Budget);
        Task<Budget> Delete(Budget Budget);
        Task<List<Budget>> BulkMerge(List<Budget> Budgets);
        Task<List<Budget>> BulkDelete(List<Budget> Budgets);
    }
    public class BudgetService : IBudgetService
    {
        private IUOW UOW;
        private IBudgetValidator BudgetValidator;
        public BudgetService(IUOW UOW, IBudgetValidator BudgetValidator)
        {
            this.UOW = UOW;
            this.BudgetValidator = BudgetValidator;
        }
        public async Task<int> Count(BudgetFilter BudgetFilter)
        {
            try
            {
                int result = await UOW.BudgetRepository.Count(BudgetFilter);
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
        public async Task<List<Budget>> List(BudgetFilter BudgetFilter)
        {
            try
            {
                List<Budget> Budgets = await UOW.BudgetRepository.List(BudgetFilter);
                if (Budgets != null)
                {
                    foreach(var Budget in Budgets)
                    {
                        decimal AccmulatedAmount = 0;
                        BudgetFilter budgetFilter = new BudgetFilter()
                        {
                            Skip = 0,
                            Take = int.MaxValue,
                            Year = new LongFilter { Equal = Budget.Year },
                            Month = new LongFilter { Less = Budget.Month },
                            CompanyId = new IdFilter { Equal = Budget.CompanyId },
                            ProjectId = new IdFilter { Equal = Budget.ProjectId },
                            BudgetTypeId = new IdFilter { Equal = Budget.BudgetTypeId },
                            Selects = BudgetSelect.Amount
                        };
                        var budgets = await UOW.BudgetRepository.List(budgetFilter);
                        if (budgets != null)
                        {
                            AccmulatedAmount = Budgets.Select(x => x.Amount).Sum();
                        }
                        Budget.AccumulatedAmount = AccmulatedAmount;
                    }
                }
                return Budgets;
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
        public async Task<Budget> Get(long Id)
        {
            
            Budget Budget = await UOW.BudgetRepository.Get(Id);
            if (Budget != null)
            {
                decimal AccmulatedAmount = 0;
                BudgetFilter BudgetFilter = new BudgetFilter()
                {
                    Skip = 0,
                    Take = int.MaxValue,
                    Year = new LongFilter { Equal = Budget.Year },
                    Month = new LongFilter { Less = Budget.Month },
                    CompanyId = new IdFilter { Equal = Budget.CompanyId },
                    ProjectId = new IdFilter { Equal = Budget.ProjectId },
                    BudgetTypeId = new IdFilter { Equal = Budget.BudgetTypeId },
                    Selects = BudgetSelect.Amount
                };
                var Budgets = await UOW.BudgetRepository.List(BudgetFilter);
                if (Budgets != null)
                {
                    AccmulatedAmount = Budgets.Select(x => x.Amount).Sum();
                }
                Budget.AccumulatedAmount = AccmulatedAmount;
            }
            else return null;
            return Budget;
        }
        public async Task<Budget> Create(Budget Budget)
        {
            if (!await BudgetValidator.Create(Budget))
                return Budget;
            try
            {
                await UOW.Begin();
                await UOW.BudgetRepository.Create(Budget);
                await UOW.Commit();

                var Budgets = await UOW.BudgetRepository.List(new List<long> { Budget.Id });
                Budget = Budgets.FirstOrDefault();
                return Budget;
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
        public async Task<Budget> Update(Budget Budget)
        {
            if (!await BudgetValidator.Update(Budget))
                return Budget;
            try
            {
                await UOW.Begin();
                await UOW.BudgetRepository.Update(Budget);
                await UOW.Commit();

                var Budgets = await UOW.BudgetRepository.List(new List<long> { Budget.Id });
                Budget = Budgets.FirstOrDefault();
                return Budget;
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
        public async Task<Budget> Delete(Budget Budget)
        {
            try
            {
                await UOW.Begin();
                await UOW.BudgetRepository.Delete(Budget);
                await UOW.Commit();

                var Budgets = await UOW.BudgetRepository.List(new List<long> { Budget.Id });
                Budget = Budgets.FirstOrDefault();
                return Budget;
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
        public async Task<List<Budget>> BulkMerge(List<Budget> Budgets)
        {
            try
            {
                await UOW.Begin();
                await UOW.BudgetRepository.BulkMerge(Budgets);
                await UOW.Commit();

                var Ids = Budgets.Select(x => x.Id).ToList();
                Budgets = await UOW.BudgetRepository.List(Ids);
                return Budgets;
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
        public async Task<List<Budget>> BulkDelete(List<Budget> Budgets)
        {
            try
            {
                await UOW.Begin();
                await UOW.BudgetRepository.BulkDelete(Budgets);
                await UOW.Commit();

                var Ids = Budgets.Select(x => x.Id).ToList();
                Budgets = await UOW.BudgetRepository.List(Ids);
                return Budgets;
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
