using TheradexPortal.Data.Models;
using TheradexPortal.Data.Models.ADDR;

namespace TheradexPortal.Data.Services.Abstract.ADDR
{
    public interface INotesService<T>
    {
        Task<List<AddrNotes<T>>> GetAllNotesAsync(string userId, string searchKey);
        Task<bool> SaveNotesAsync(AddrNotes<T> notes);
    }
}