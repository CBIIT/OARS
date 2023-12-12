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
        public Task<IList<ThorAlert>> GetAllThorAlertsAsync();
        public Task<IList<ThorAlert>> GetAllAlertsAsync();
        public Task<IList<ThorAlert>> GetAllNotesAsync();
        public Task<ThorAlert> GetAlertById(int id);
        public Task<IList<ThorAlert>> GetAllActiveAlertsAsync();
        public Task<IList<ThorAlert>> GetAllActiveNotesAsync();
        public Task<IList<ThorAlert>> GetActiveSystemAlertsAsync();
        public Task<IList<ThorAlert>> GetActiveSystemNotesAsync();
        public Task<IList<ThorAlert>> GetActiveLoginAlertsAsync();
        public Task<IList<ThorAlert>> GetActiveLoginNotesAsync();
        public Task<IList<ThorAlert>> GetActiveDashboardAlertsByIdAsync(int dashboardId);
        public Task<IList<ThorAlert>> GetActiveDashboardNotesByIdAsync(int dashboardId);
        public bool SaveAlert(int userId, ThorAlert alert);
        public bool DeactivateAlert(int userId, int alertId);

        public bool AlertDatesValid(DateTime? startDate, DateTime? endDate);
    }
}
