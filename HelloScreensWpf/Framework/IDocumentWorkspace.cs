using System.Threading.Tasks;

namespace Caliburn.Micro.HelloScreens.Framework
{
    public interface IDocumentWorkspace : IWorkspace
    {
        Task EditAsync(object document);
    }
}