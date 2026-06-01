using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Components.DiagramBoard
{
    public partial class DiagramToolbar
    {
        [Parameter]
        public EventCallback OnMappingSaved { get; set; }

        [Parameter]
        public EventCallback OnMappingReset { get; set; }

        [Parameter]
        public EventCallback OnRenderMapping { get; set; }


        [Parameter]
        public EventCallback OnAutoAdjustNodePosition { get; set; }
        
        [Parameter]
        public EventCallback OnAutoLink { get; set; }

        [Parameter]
        public EventCallback OnUndo { get; set; }

        [Parameter]
        public EventCallback OnRedo { get; set; }

        [Parameter]
        public EventCallback OnZoomIn { get; set; }

        [Parameter]
        public EventCallback OnZoomOut { get; set; }

        [Parameter]
        public EventCallback OnZoomReset { get; set; }

        [Parameter]
        public EventCallback OnFitToScreen { get; set; }

        [Parameter]
        public double ZoomLevel { get; set; } = 1.0;
    }
}
