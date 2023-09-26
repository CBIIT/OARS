using Blazorise;
using TheradexPortal.Data.Models;

namespace TheradexPortal.Data.Services.Abstract
{
    public interface IAlertService
    {
        public Task<string> NoteType();
        public Task<string> AlertType();
        public Task<string> SystemPage();
        public Task<string> LoginPage();
        public Task<string> DashboardPage();
        public Task<Color> AlertColor();
        public Task<Color> NoteColor();
        public Task<Color> InfoColor();
        public Task<IList<WRAlert>> GetAllWRAlertsAsync();
        public Task<IList<WRAlert>> GetAllAlertsAsync();
        public Task<IList<WRAlert>> GetAllNotesAsync();
        public Task<WRAlert> GetAlertById(int id);
        public Task<IList<WRAlert>> GetAllActiveAlertsAsync();
        public Task<IList<WRAlert>> GetAllActiveNotesAsync();
        public Task<IList<WRAlert>> GetActiveSystemAlertsAsync();
        public Task<IList<WRAlert>> GetActiveSystemNotesAsync();
        public Task<IList<WRAlert>> GetActiveLoginAlertsAsync();
        public Task<IList<WRAlert>> GetActiveLoginNotesAsync();
        public Task<IList<WRAlert>> GetActiveDashboardAlertsByIdAsync(int dashboardId);
        public Task<IList<WRAlert>> GetActiveDashboardNotesByIdAsync(int dashboardId);
    }
}
