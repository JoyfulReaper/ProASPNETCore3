using Advanced.Blazor;
using Microsoft.JSInterop;

namespace Advanced.Services
{
    public class ToggleService
    {
        private List<MultiNavLink> components = new();
        private bool enabled = true;

        public void EnrollComponents(IEnumerable<MultiNavLink> comps)
        {
            components.AddRange(comps);
        }

        [JSInvokable]
        public bool ToggleComponents()
        {
            enabled = !enabled;
            components.ForEach(c => c.SetEnabled(enabled));
            return enabled;
        }
    }
}
