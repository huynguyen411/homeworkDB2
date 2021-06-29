using HomeWork2.Common;
using HomeWork2.Entities;
using HomeWork2.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeWork2.Services.MBudgetType
{
    public interface IBudgetTypeService : IServiceScoped
    {
        Task<int> Count(BudgetTypeFilter BudgetTypeFilter);
        Task<List<BudgetType>> List(BudgetTypeFilter BudgetTypeFilter);
        Task<BudgetType> Get(long Id);
        Task<BudgetType> Create(BudgetType BudgetType);
        Task<BudgetType> Update(BudgetType BudgetType);
        Task<BudgetType> Delete(BudgetType BudgetType);
    }
    public class BudgetTypeService : IBudgetTypeService
    {
        private IUOW UOW;
        private IBudgetTypeValidator BudgetTypeValidator;
        public BudgetTypeService(IUOW UOW, IBudgetTypeValidator BudgetTypeValidator)
        {
            this.UOW = UOW;
            this.BudgetTypeValidator = BudgetTypeValidator;
        }
        public async Task<int> Count(BudgetTypeFilter BudgetTypeFilter)
        {
            try
            {
                int result = await UOW.BudgetTypeRepository.Count(BudgetTypeFilter);
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
        public async Task<List<BudgetType>> List(BudgetTypeFilter BudgetTypeFilter)
        {
            try
            {
                List<BudgetType> BudgetTypes = await UOW.BudgetTypeRepository.List(BudgetTypeFilter);
                return BudgetTypes;
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
        public async Task<BudgetType> Get(long Id)
        {
            BudgetType BudgetType = await UOW.BudgetTypeRepository.Get(Id);
            if (BudgetType == null)
                return null;
            return BudgetType;
        }
        public async Task<BudgetType> Create(BudgetType BudgetType)
        {
            if (!await BudgetTypeValidator.Create(BudgetType))
                return BudgetType;
            try
            {
                await UOW.Begin();
                await UOW.BudgetTypeRepository.Create(BudgetType);
                await UOW.Commit();

                var BudgetTypes = await UOW.BudgetTypeRepository.List(new List<long> { BudgetType.Id });
                BudgetType = BudgetTypes.FirstOrDefault();
                return BudgetType;
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
        public async Task<BudgetType> Update(BudgetType BudgetType)
        {
            if (!await BudgetTypeValidator.Update(BudgetType))
                return BudgetType;
            try
            {
                await UOW.Begin();
                await UOW.BudgetTypeRepository.Update(BudgetType);
                await UOW.Commit();

                var BudgetTypes = await UOW.BudgetTypeRepository.List(new List<long> { BudgetType.Id });
                BudgetType = BudgetTypes.FirstOrDefault();
                return BudgetType;
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
        public async Task<BudgetType> Delete(BudgetType BudgetType)
        {
            try
            {
                await UOW.Begin();
                await UOW.BudgetTypeRepository.Delete(BudgetType);
                await UOW.Commit();

                var Companies = await UOW.BudgetTypeRepository.List(new List<long> { BudgetType.Id });
                BudgetType = Companies.FirstOrDefault();
                return BudgetType;
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
