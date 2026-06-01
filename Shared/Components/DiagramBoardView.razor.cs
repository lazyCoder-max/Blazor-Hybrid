using Shared.Components.DiagramBoard;
using Shared.Components.DiagramBoard.DiagramCanvas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Components
{
    public partial class DiagramBoardView
    {
        private DiagramCanvasView? diagramCanvasView;
        private DiagramToolbar? diagramToolbar;

        private void HandleDiagramNodePositioning()
        {
            diagramCanvasView?.DeselectAllNodes();
            diagramCanvasView?.UpdateCanvasSize();
            diagramCanvasView?.InitializeNodePositions();
        }

        private void HandleZoomIn()
        {
            diagramCanvasView?.ZoomIn();
            StateHasChanged();
        }

        private void HandleZoomOut()
        {
            diagramCanvasView?.ZoomOut();
            StateHasChanged();
        }

        private void HandleZoomReset()
        {
            diagramCanvasView?.ResetZoom();
            StateHasChanged();
        }

        private void HandleFitToScreen()
        {
            diagramCanvasView?.FitToScreen();
            StateHasChanged();
        }

        private double GetCurrentZoomLevel()
        {
            return diagramCanvasView?.ZoomLevel ?? 1.0;
        }
    }
}
