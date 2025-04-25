using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace OARS.Data.Services
{
    public sealed class TimeZoneService
    {
        private IJSRuntime _jsRuntime;

        private TimeSpan? _userOffset;
        private string? _timeZoneAbbrev;

        public TimeZoneService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async ValueTask<Tuple<string, TimeSpan>> GetZoneAndOffset()
        {
            if (_userOffset == null)
            {
                int offsetInMinutes = await _jsRuntime.InvokeAsync<int>("getTimezoneOffset");
                _userOffset = TimeSpan.FromMinutes(-offsetInMinutes);
            }

            if (_timeZoneAbbrev == null)
            {
                _timeZoneAbbrev = await _jsRuntime.InvokeAsync<string>("getTimeZone");
                _timeZoneAbbrev = _timeZoneAbbrev.Split(' ')[2];
            }

            return new Tuple<string, TimeSpan>(_timeZoneAbbrev, _userOffset.Value);
        }
    }
}
