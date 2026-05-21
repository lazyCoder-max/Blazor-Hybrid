using Microsoft.AspNetCore.Components;
using Shared.Models;

namespace Shared.Components.DiagramBoard.DiagramCanvas
{
    public partial class DiagramNodeView
    {
        [Parameter]
        public DiagramNode Node { get; set; } = new();
    }
}
