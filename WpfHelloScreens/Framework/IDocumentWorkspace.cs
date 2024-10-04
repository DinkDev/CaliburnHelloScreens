using System.Threading.Tasks;

namespace Caliburn.Micro.HelloScreens.Framework
{
    public interface IDocumentWorkspace : IWorkspace
    {
        //void Edit(object document);
        Task EditAsync(object document);
    }
}