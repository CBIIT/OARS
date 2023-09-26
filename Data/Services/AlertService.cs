using Blazorise;
using Microsoft.EntityFrameworkCore;
using TheradexPortal.Data.Models;
using TheradexPortal.Data.Services.Abstract;

namespace TheradexPortal.Data.Services
{
    public class AlertService : BaseService, IAlertService
    {
        private readonly bool active = true;
        private readonly DateTime dateTime = DateTime.Now;

        private readonly string noteType = "Note";
        private readonly string alertType = "Alert";
        private readonly string systemPage = "System";
        private readonly string loginPage = "Login";
        private readonly string dashboardPage = "Dashboard";

        private readonly Color alertColor = Color.Danger;
        private readonly Color noteColor = Color.Warning;
        private readonly Color infoColor = Color.Info;

        public Task<string> NoteType()
        {
            return Task.FromResult(noteType);
        }
        public Task<string> AlertType()
        {
            return Task.FromResult(alertType);
        }
        public Task<string> SystemPage()
        {
            return Task.FromResult(systemPage);
        }
        public Task<string> LoginPage()
        {
            return Task.FromResult(loginPage);
        }
        public Task<string> DashboardPage()
        {
            return Task.FromResult(dashboardPage);
        }
        public Task<Color> AlertColor()
        {
            return Task.FromResult(alertColor);
        }
        public Task<Color> NoteColor()
        {
            return Task.FromResult(noteColor);
        }
        public Task<Color> InfoColor()
        {
            return Task.FromResult(infoColor);
        }

        public AlertService(IDbContextFactory<WrDbContext> dbFactory) : base(dbFactory) { }

        public async Task<IList<WRAlert>> GetAllWRAlertsAsync()
        {
            return await context.Alerts.ToListAsync();
        }

        public async Task<IList<WRAlert>> GetAllAlertsAsync()
        {
            return await context.Alerts.Where(a => a.AlertType == alertType).ToListAsync();
        }
        public async Task<IList<WRAlert>> GetAllNotesAsync()
        {
            return await context.Alerts.Where(a => a.AlertType == noteType).ToListAsync();
        }

        public async Task<IList<WRAlert>> GetAllActiveAlertsAsync()
        {
            return await context.Alerts.Where(a => a.IsActive == active &&
            (dateTime >= a.StartDate!.Value.Date && (dateTime <= a.EndDate!.Value.Date.AddDays(1) || a.EndDate.Equals(null))) &&
            a.AlertType == alertType).ToListAsync();
        }

        public async Task<IList<WRAlert>> GetAllActiveNotesAsync()
        {
            return await context.Alerts.Where(a => a.IsActive == active &&
            (dateTime >= a.StartDate!.Value.Date && (dateTime <= a.EndDate!.Value.Date.AddDays(1) || a.EndDate.Equals(null))) &&
            a.AlertType == noteType).ToListAsync();
        }

        public async Task<IList<WRAlert>> GetActiveSystemAlertsAsync()
        {
            return await context.Alerts.Where(a => a.IsActive == active &&
            (dateTime >= a.StartDate!.Value.Date && (dateTime <= a.EndDate!.Value.Date.AddDays(1) || a.EndDate.Equals(null))) &&
            a.AlertType == alertType && a.PageName == systemPage).OrderByDescending(a => a.WRAlertId).ToListAsync();
        }

        public async Task<IList<WRAlert>> GetActiveSystemNotesAsync()
        {
            return await context.Alerts.Where(a => a.IsActive == active &&
            (dateTime >= a.StartDate!.Value.Date && (dateTime <= a.EndDate!.Value.Date.AddDays(1) || a.EndDate.Equals(null))) &&
            a.AlertType == noteType && a.PageName == systemPage).OrderByDescending(a => a.WRAlertId).ToListAsync();
        }

        public async Task<IList<WRAlert>> GetActiveLoginAlertsAsync()
        {
            return await context.Alerts.Where(a => a.IsActive == active && 
            (dateTime >= a.StartDate!.Value.Date && (dateTime <= a.EndDate!.Value.Date.AddDays(1) || a.EndDate.Equals(null))) && 
            a.AlertType == alertType && a.PageName == loginPage).OrderByDescending(a => a.WRAlertId).ToListAsync();
        }
        
        public async Task<IList<WRAlert>> GetActiveLoginNotesAsync()
        {
            return await context.Alerts.Where(a => a.IsActive == active &&
            (dateTime >= a.StartDate!.Value.Date && (dateTime <= a.EndDate!.Value.Date.AddDays(1) || a.EndDate.Equals(null))) &&  
            a.AlertType == noteType && a.PageName == loginPage).OrderByDescending(a => a.WRAlertId).ToListAsync();
        }
        
        public async Task<IList<WRAlert>> GetActiveDashboardAlertsByIdAsync(int dashboardId)
        {
            return await context.Alerts.Where(a => a.IsActive == active &&
            (dateTime >= a.StartDate!.Value.Date && (dateTime <= a.EndDate!.Value.Date.AddDays(1) || a.EndDate.Equals(null))) &&
            a.AlertType == alertType && a.DashboardId == dashboardId).OrderByDescending(a => a.WRAlertId).ToListAsync();
        }

        public async Task<IList<WRAlert>> GetActiveDashboardNotesByIdAsync(int dashboardId)
        {
            return await context.Alerts.Where(a => a.IsActive == active &&
            (dateTime >= a.StartDate!.Value.Date && (dateTime <= a.EndDate!.Value.Date.AddDays(1) || a.EndDate.Equals(null))) &&
            a.AlertType == noteType && a.DashboardId == dashboardId).OrderByDescending(a => a.WRAlertId).ToListAsync();
        }
    }
}
