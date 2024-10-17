using TheradexPortal.Data.Models;
using TheradexPortal.Data.Models.ADDR;

namespace TheradexPortal.Data.Services.Abstract.ADDR
{
    public interface INotesService<T>
    {
        Task<T> GetNoteByIdAsync(int id);
        Task<List<Note<T>>> GetNotesByStatusIdAsync(int statusId);
        Task SaveNotesAsync(int statusId, Note<T> note);
    }
}
