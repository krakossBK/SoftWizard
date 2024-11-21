namespace SoftWizard.Services
{
    public interface IUnitOfWork
    {
        IOkpdCategoryRepository OkpdCategory { get; }
    }
}
