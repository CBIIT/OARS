using Blazorise;

namespace TheradexPortal.Data.ViewModels
{
    public class SecondaryNavAction
    {
        public string Label { get; set; } = "";
        public IconName IconName { get; set; } = IconName.Add;
        public Action? OnClick { get; set; }
        public string? To { get; set; }
        public Target? Target { get; set; }
    }
}
