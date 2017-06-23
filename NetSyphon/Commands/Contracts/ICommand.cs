
namespace NetSyphon.Commands.Contracts
{
    public interface ICommand<in TModel>
    {
        void Execute(TModel model);
    }
}
