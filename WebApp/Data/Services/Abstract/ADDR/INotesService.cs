using OARS.Data.Models;
using OARS.Data.Models.ADDR;

namespace OARS.Data.Services.Abstract.ADDR
{
    public interface INotesService<T>
    {
        Task<List<AddrNotes<T>>> GetAllNotesAsync(string userId, string searchKey);
        Task<bool> SaveNotesAsync(AddrNotes<T> notes);
    }
}