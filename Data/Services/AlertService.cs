using Blazorise;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.EntityFrameworkCore;
using TheradexPortal.Data.Models;
using TheradexPortal.Data.Services.Abstract;

namespace TheradexPortal.Data.Services
{
    public class AlertService : BaseService, IAlertService
    {
        private readonly IErrorLogService _errorLogService;
        private readonly NavigationManager _navManager;

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

        public AlertService(IDatabaseConnectionService databaseConnectionService, IErrorLogService errorLogService, NavigationManager navigationManager) : base(databaseConnectionService)
        {
            _errorLogService = errorLogService;
            _navManager = navigationManager;
        }


        public async Task<IList<ThorAlert>> GetAllThorAlertsAsync()
        {
            return await context.Alerts.ToListAsync();
        }

        public async Task<IList<ThorAlert>> GetAllAlertsAsync()
        {
            return await context.Alerts.Where(a => a.AlertType == alertType).ToListAsync();
        }
        public async Task<IList<ThorAlert>> GetAllNotesAsync()
        {
            return await context.Alerts.Where(a => a.AlertType == noteType).ToListAsync();
        }
        public async Task<ThorAlert?> GetAlertById(int id)
        {
            return await context.Alerts.FirstOrDefaultAsync(a => a.AlertId == id);
        }

        public async Task<IList<ThorAlert>> GetAllActiveAlertsAsync()
        {
            return await context.Alerts.Where(a => a.IsActive == active &&
            (dateTime >= a.StartDate!.Value.Date && (dateTime <= a.EndDate!.Value.Date.AddDays(1) || a.EndDate.Equals(null))) &&
            a.AlertType == alertType).ToListAsync();
        }

        public async Task<IList<ThorAlert>> GetAllActiveNotesAsync()
        {
            return await context.Alerts.Where(a => a.IsActive == active &&
            (dateTime >= a.StartDate!.Value.Date && (dateTime <= a.EndDate!.Value.Date.AddDays(1) || a.EndDate.Equals(null))) &&
            a.AlertType == noteType).ToListAsync();
        }

        public async Task<IList<ThorAlert>> GetActiveSystemAlertsAsync()
        {
            return await context.Alerts.Where(a => a.IsActive == active &&
            (dateTime >= a.StartDate!.Value.Date && (dateTime <= a.EndDate!.Value.Date.AddDays(1) || a.EndDate.Equals(null))) &&
            a.AlertType == alertType && a.PageName == systemPage).OrderByDescending(a => a.AlertId).ToListAsync();
        }

        public async Task<IList<ThorAlert>> GetActiveSystemNotesAsync()
        {
            return await context.Alerts.Where(a => a.IsActive == active &&
            (dateTime >= a.StartDate!.Value.Date && (dateTime <= a.EndDate!.Value.Date.AddDays(1) || a.EndDate.Equals(null))) &&
            a.AlertType == noteType && a.PageName == systemPage).OrderByDescending(a => a.AlertId).ToListAsync();
        }

        public async Task<IList<ThorAlert>> GetActiveLoginAlertsAsync()
        {
            return await context.Alerts.Where(a => a.IsActive == active && 
            (dateTime >= a.StartDate!.Value.Date && (dateTime <= a.EndDate!.Value.Date.AddDays(1) || a.EndDate.Equals(null))) && 
            a.AlertType == alertType && a.PageName == loginPage).OrderByDescending(a => a.AlertId).ToListAsync();
        }
        
        public async Task<IList<ThorAlert>> GetActiveLoginNotesAsync()
        {
            return await context.Alerts.Where(a => a.IsActive == active &&
            (dateTime >= a.StartDate!.Value.Date && (dateTime <= a.EndDate!.Value.Date.AddDays(1) || a.EndDate.Equals(null))) &&  
            a.AlertType == noteType && a.PageName == loginPage).OrderByDescending(a => a.AlertId).ToListAsync();
        }
        
        public async Task<IList<ThorAlert>> GetActiveDashboardAlertsByIdAsync(int dashboardId)
        {
            return await context.Alerts.Where(a => a.IsActive == active &&
            (dateTime >= a.StartDate!.Value.Date && (dateTime <= a.EndDate!.Value.Date.AddDays(1) || a.EndDate.Equals(null))) &&
            a.AlertType == alertType && a.DashboardId == dashboardId).OrderByDescending(a => a.AlertId).ToListAsync();
        }

        public async Task<IList<ThorAlert>> GetActiveDashboardNotesByIdAsync(int dashboardId)
        {
            return await context.Alerts.Where(a => a.IsActive == active &&
            (dateTime >= a.StartDate!.Value.Date && (dateTime <= a.EndDate!.Value.Date.AddDays(1) || a.EndDate.Equals(null))) &&
            a.AlertType == noteType && a.DashboardId == dashboardId).OrderByDescending(a => a.AlertId).ToListAsync();
        }

        public bool SaveAlert(int userId, ThorAlert alert)
        {
            try
            {
                var primaryTable = context.Model.FindEntityType(typeof(ThorAlert)).ToString().Replace("EntityType: ", "");
                if (alert.AlertId == 0)
                {
                    context.Alerts.Add(alert);
                    context.SaveChangesAsync(userId, primaryTable);
                }
                else
                {
                    var dbAlert = context.Alerts.FirstOrDefault(a => a.AlertId == alert.AlertId);

                    if (dbAlert != null)
                    {
                        dbAlert.PageName = alert.PageName;
                        if (alert.DashboardId != null && alert.DashboardId > 0)
                        {
                            dbAlert.DashboardId = alert.DashboardId;
                        }
                        dbAlert.AlertType = alert.AlertType;
                        dbAlert.AlertText = alert.AlertText;
                        dbAlert.IsActive = alert.IsActive;
                        dbAlert.StartDate = alert.StartDate;
                        dbAlert.EndDate = alert.EndDate;
                        if (context.Entry(dbAlert).State == EntityState.Modified)
                            dbAlert.UpdateDate = DateTime.UtcNow;
                        context.SaveChangesAsync(userId, primaryTable);
                    }
                }

                return true;
            }
            catch(Exception ex)
            {
                _errorLogService.SaveErrorLogAsync(userId, _navManager.Uri, ex.InnerException, ex.Source, ex.Message, ex.StackTrace);
                return false;
            }
        }

        public bool DeactivateAlert(int userId, int alertId)
        {
            try
            {
                var primaryTable = context.Model.FindEntityType(typeof(ThorAlert)).ToString().Replace("EntityType: ", "");
                var alert = context.Alerts.FirstOrDefault(a => a.AlertId == alertId);
                alert.IsActive = false;
                alert.UpdateDate = DateTime.UtcNow;
                context.SaveChangesAsync(userId, primaryTable);

                return true;
            }
            catch (Exception ex)
            {
                _errorLogService.SaveErrorLogAsync(userId, _navManager.Uri, ex.InnerException, ex.Source, ex.Message, ex.StackTrace);
                return false;
            }
        }

        public bool AlertDatesValid(DateTime? startDate, DateTime? endDate)
        {
            if (startDate == null)
            {
                return false; 
            }

            var datesValid = dateTime >= startDate!.Value.Date && (endDate == null || (dateTime <= endDate!.Value.Date.AddDays(1)));

            return datesValid;
        }
    }
}
