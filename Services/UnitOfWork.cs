namespace SoftWizard.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(IOkpdCategoryRepository okpdCategoryRepository)
        {
            OkpdCategory = okpdCategoryRepository;
        }

        public IOkpdCategoryRepository OkpdCategory { get; }
    }
}
