using Microsoft.EntityFrameworkCore;
using TheradexPortal.Data.Models;
using TheradexPortal.Data.Services.Abstract;

namespace TheradexPortal.Data.Services
{
    public class AlertService : BaseService, IAlertService
    {
        private readonly bool active = true;
        private readonly DateTime dateTime = DateTime.Now;

        public readonly string NoteType = "Note";
        public readonly string AlertType = "Alert";
        public readonly string SystemPage = "System";
        public readonly string LoginPage = "Login";
        public readonly string DashboardPage = "Dashboard";

        public AlertService(IDbContextFactory<WrDbContext> dbFactory) : base(dbFactory) { }

        public async Task<IList<WRAlert>> GetAllWRAlertsAsync()
        {
            return await context.Alerts.ToListAsync();
        }

        public async Task<IList<WRAlert>> GetAllAlertsAsync()
        {
            return await context.Alerts.Where(a => a.AlertType == AlertType).ToListAsync();
        }
        public async Task<IList<WRAlert>> GetAllNotesAsync()
        {
            return await context.Alerts.Where(a => a.AlertType == NoteType).ToListAsync();
        }

        public async Task<IList<WRAlert>> GetAllActiveAlertsAsync()
        {
            return await context.Alerts.Where(a => a.IsActive == active &&
            (dateTime >= a.StartDate!.Value.Date && (dateTime <= a.EndDate!.Value.Date.AddDays(1) || a.EndDate.Equals(null))) &&
            a.AlertType == AlertType).ToListAsync();
        }

        public async Task<IList<WRAlert>> GetAllActiveNotesAsync()
        {
            return await context.Alerts.Where(a => a.IsActive == active &&
            (dateTime >= a.StartDate!.Value.Date && (dateTime <= a.EndDate!.Value.Date.AddDays(1) || a.EndDate.Equals(null))) &&
            a.AlertType == NoteType).ToListAsync();
        }

        public async Task<IList<WRAlert>> GetActiveSystemAlertsAsync()
        {
            return await context.Alerts.Where(a => a.IsActive == active &&
            (dateTime >= a.StartDate!.Value.Date && (dateTime <= a.EndDate!.Value.Date.AddDays(1) || a.EndDate.Equals(null))) &&
            a.AlertType == AlertType && a.PageName == SystemPage).OrderByDescending(a => a.WRAlertId).ToListAsync();
        }

        public async Task<IList<WRAlert>> GetActiveSystemNotesAsync()
        {
            return await context.Alerts.Where(a => a.IsActive == active &&
            (dateTime >= a.StartDate!.Value.Date && (dateTime <= a.EndDate!.Value.Date.AddDays(1) || a.EndDate.Equals(null))) &&
            a.AlertType == NoteType && a.PageName == SystemPage).OrderByDescending(a => a.WRAlertId).ToListAsync();
        }

        public async Task<IList<WRAlert>> GetActiveLoginAlertsAsync()
        {
            return await context.Alerts.Where(a => a.IsActive == active && 
            (dateTime >= a.StartDate!.Value.Date && (dateTime <= a.EndDate!.Value.Date.AddDays(1) || a.EndDate.Equals(null))) && 
            a.AlertType == AlertType && a.PageName == LoginPage).OrderByDescending(a => a.WRAlertId).ToListAsync();
        }
        
        public async Task<IList<WRAlert>> GetActiveLoginNotesAsync()
        {
            return await context.Alerts.Where(a => a.IsActive == active &&
            (dateTime >= a.StartDate!.Value.Date && (dateTime <= a.EndDate!.Value.Date.AddDays(1) || a.EndDate.Equals(null))) &&  
            a.AlertType == NoteType && a.PageName == LoginPage).OrderByDescending(a => a.WRAlertId).ToListAsync();
        }
        
        public async Task<IList<WRAlert>> GetActiveDashboardAlertsByIdAsync(int dashboardId)
        {
            return await context.Alerts.Where(a => a.IsActive == active &&
            (dateTime >= a.StartDate!.Value.Date && (dateTime <= a.EndDate!.Value.Date.AddDays(1) || a.EndDate.Equals(null))) &&
            a.AlertType == AlertType && a.DashboardId == dashboardId).OrderByDescending(a => a.WRAlertId).ToListAsync();
        }

        public async Task<IList<WRAlert>> GetActiveDashboardNotesByIdAsync(int dashboardId)
        {
            return await context.Alerts.Where(a => a.IsActive == active &&
            (dateTime >= a.StartDate!.Value.Date && (dateTime <= a.EndDate!.Value.Date.AddDays(1) || a.EndDate.Equals(null))) &&
            a.AlertType == NoteType && a.DashboardId == dashboardId).OrderByDescending(a => a.WRAlertId).ToListAsync();
        }
    }
}
