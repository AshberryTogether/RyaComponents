using Microsoft.AspNetCore.Components;

namespace RyaComponents.Components.Common.Menu
{
    public interface IRyaMenuItem
    {
        public string? IconCssClass { get; set; }
        public string? CssClass { get; set; }
        public string? Text { get; set; }
        public string? Name { get; set; }
        public string? NavigateUrl { get; set; }
        public bool Visible { get; set; }
        public bool Enabled { get; set; }
        public RenderFragment? Template { get; set; }
        public RenderFragment? TextTemplate { get; set; }
        public EventCallback ItemClick { get; set; }
    }
}
