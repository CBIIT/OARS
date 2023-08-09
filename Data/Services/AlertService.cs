using Microsoft.EntityFrameworkCore;
using TheradexPortal.Data.Models;
using TheradexPortal.Data.Services.Abstract;

namespace TheradexPortal.Data.Services
{
    public class AlertService : BaseService, IAlertService
    {
        private readonly char active = '1';
        private readonly string noteType = "Note";
        private readonly string alertType = "Alert";
        private readonly string system = "System";
        private readonly string loginPage = "Login";
        private readonly DateTime dateTime = DateTime.Now;

        public AlertService(IDbContextFactory<WrDbContext> dbFactory) : base(dbFactory) { }

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
            (dateTime >= a.StartDate && (dateTime <= a.EndDate || a.EndDate.Equals(null))) &&
            a.AlertType == alertType).ToListAsync();
        }

        public async Task<IList<WRAlert>> GetAllActiveNotesAsync()
        {
            return await context.Alerts.Where(a => a.IsActive == active &&
            (dateTime >= a.StartDate && (dateTime <= a.EndDate || a.EndDate.Equals(null))) &&
            a.AlertType == noteType).ToListAsync();
        }

        public async Task<IList<WRAlert>> GetActiveSystemAlertsAsync()
        {
            return await context.Alerts.Where(a => a.IsActive == active &&
            (dateTime >= a.StartDate && (dateTime <= a.EndDate || a.EndDate.Equals(null))) &&
            a.AlertType == alertType && a.PageName == system).OrderByDescending(a => a.WRAlertId).ToListAsync();
        }

        public async Task<IList<WRAlert>> GetActiveSystemNotesAsync()
        {
            return await context.Alerts.Where(a => a.IsActive == active &&
            (dateTime >= a.StartDate && (dateTime <= a.EndDate || a.EndDate.Equals(null))) &&
            a.AlertType == noteType && a.PageName == system).OrderByDescending(a => a.WRAlertId).ToListAsync();
        }

        public async Task<IList<WRAlert>> GetActiveLoginAlertsAsync()
        {
            return await context.Alerts.Where(a => a.IsActive == active && 
            (dateTime >= a.StartDate && (dateTime <= a.EndDate || a.EndDate.Equals(null))) && 
            a.AlertType == alertType && a.PageName == loginPage).OrderByDescending(a => a.WRAlertId).ToListAsync();
        }
        
        public async Task<IList<WRAlert>> GetActiveLoginNotesAsync()
        {
            return await context.Alerts.Where(a => a.IsActive == active &&
            (dateTime >= a.StartDate && (dateTime <= a.EndDate || a.EndDate.Equals(null))) &&  
            a.AlertType == noteType && a.PageName == loginPage).OrderByDescending(a => a.WRAlertId).ToListAsync();
        }
        
        public async Task<IList<WRAlert>> GetActiveDashboardAlertsByIdAsync(int dashboardId)
        {
            return await context.Alerts.Where(a => a.IsActive == active &&
            (dateTime >= a.StartDate && (dateTime <= a.EndDate || a.EndDate.Equals(null))) &&
            a.AlertType == alertType && a.DashboardId == dashboardId).OrderByDescending(a => a.WRAlertId).ToListAsync();
        }

        public async Task<IList<WRAlert>> GetActiveDashboardNotesByIdAsync(int dashboardId)
        {
            return await context.Alerts.Where(a => a.IsActive == active &&
            (dateTime >= a.StartDate && (dateTime <= a.EndDate || a.EndDate.Equals(null))) &&
            a.AlertType == noteType && a.DashboardId == dashboardId).OrderByDescending(a => a.WRAlertId).ToListAsync();
        }
    }
}
