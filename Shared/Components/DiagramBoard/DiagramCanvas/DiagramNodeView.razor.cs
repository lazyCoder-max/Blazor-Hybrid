using Microsoft.AspNetCore.Components;
using Shared.Models;

namespace Shared.Components.DiagramBoard.DiagramCanvas
{
    public partial class DiagramNodeView
    {
        [Parameter, EditorRequired]
        public DiagramNode Node { get; set; } = null!;

        [Parameter]
        public bool IsSelected { get; set; }

        [Parameter]
        public EventCallback<DiagramNode> OnNodeSelected { get; set; }

        [Parameter]
        public EventCallback<DiagramNode> OnStartLinking { get; set; }

        [Parameter]
        public bool IsLinkingMode { get; set; }

        [Parameter]
        public EventCallback<DiagramNode> OnRemoveConnections { get; set; }

        private async Task HandleClick()
        {
            await OnNodeSelected.InvokeAsync(Node);
        }

        private string positionX = "0";
        private string positionY = "0";
        private double lastX;
        private double lastY;
        private bool lastIsSelected;
        private bool isLinkButtonClicked = false;
        private bool lastIsLinkButtonClicked = false;
        private string linkButtonTooltip = "Link to another node";
        private string linkButtonIcon = @"<svg width=""24"" height=""24"" fill=""black"" viewBox=""-3 -3 24 24"" xmlns=""http://www.w3.org/2000/svg"" preserveAspectRatio=""xMinYMin"" class=""jam jam-link""><path d=""M3.19 9.345a.97.97 0 0 1 1.37 0 .966.966 0 0 1 0 1.367l-2.055 2.052a1.93 1.93 0 0 0 0 2.735 1.94 1.94 0 0 0 2.74 0l4.794-4.787a.966.966 0 0 0 0-1.367.966.966 0 0 1 0-1.368.97.97 0 0 1 1.37 0 2.9 2.9 0 0 1 0 4.103l-4.795 4.787a3.88 3.88 0 0 1-5.48 0 3.864 3.864 0 0 1 0-5.47L3.19 9.344zm11.62-.69a.97.97 0 0 1-1.37 0 .966.966 0 0 1 0-1.367l2.055-2.052a1.93 1.93 0 0 0 0-2.735 1.94 1.94 0 0 0-2.74 0L7.962 7.288a.966.966 0 0 0 0 1.367.966.966 0 0 1 0 1.368.97.97 0 0 1-1.37 0 2.9 2.9 0 0 1 0-4.103l4.795-4.787a3.88 3.88 0 0 1 5.48 0 3.864 3.864 0 0 1 0 5.47L14.81 8.656z""/></svg>";
        string unlinkIcon = @"<svg width=""24"" height=""24"" fill=""black"" viewBox=""0 0 17 17"" xmlns=""http://www.w3.org/2000/svg""><path d=""m2.134 5.139 1.402-2.587C4.622.904 6.798.457 8.407 1.523l-.551.833a2.505 2.505 0 0 0-3.464.709L2.99 5.652c-.781 1.188-.464 2.742.687 3.501 1.143.752 2.41.547 3.313-.538l.768.641c-.742.892-1.694 1.352-2.678 1.352-.655 0-1.323-.204-1.954-.62a3.504 3.504 0 0 1-.992-4.849m1.907 2.895-.13.129.705.709.131-.13c.975-.975 2.561-.975 3.535 0s.975 2.561 0 3.535l-3.023 3.024c-.975.975-2.561.975-3.535 0s-.975-2.561 0-3.535l1.058-1.059L2.075 10l-1.058 1.06a3.504 3.504 0 0 0 0 4.949c.683.683 1.578 1.023 2.475 1.023s1.792-.341 2.475-1.023l3.023-3.024a3.504 3.504 0 0 0 0-4.949 3.503 3.503 0 0 0-4.949-.002M10.963 7h4.074V6h-4.074zm-.468 1.347 1.951 1.127.5-.865-1.951-1.127zm.255-7.191L9.17 3.893l.865.5 1.58-2.736zm5.117 1.569-.5-.865-4.992 2.882.5.865z""/></svg>";
        string linkIcon = @"<svg width=""24"" height=""24"" fill=""black"" viewBox=""-3 -3 24 24"" xmlns=""http://www.w3.org/2000/svg"" preserveAspectRatio=""xMinYMin"" class=""jam jam-link""><path d=""M3.19 9.345a.97.97 0 0 1 1.37 0 .966.966 0 0 1 0 1.367l-2.055 2.052a1.93 1.93 0 0 0 0 2.735 1.94 1.94 0 0 0 2.74 0l4.794-4.787a.966.966 0 0 0 0-1.367.966.966 0 0 1 0-1.368.97.97 0 0 1 1.37 0 2.9 2.9 0 0 1 0 4.103l-4.795 4.787a3.88 3.88 0 0 1-5.48 0 3.864 3.864 0 0 1 0-5.47L3.19 9.344zm11.62-.69a.97.97 0 0 1-1.37 0 .966.966 0 0 1 0-1.367l2.055-2.052a1.93 1.93 0 0 0 0-2.735 1.94 1.94 0 0 0-2.74 0L7.962 7.288a.966.966 0 0 0 0 1.367.966.966 0 0 1 0 1.368.97.97 0 0 1-1.37 0 2.9 2.9 0 0 1 0-4.103l4.795-4.787a3.88 3.88 0 0 1 5.48 0 3.864 3.864 0 0 1 0 5.47L14.81 8.656z""/></svg>";
        string duplicateIcon = @"<svg width=""24"" height=""24"" viewBox=""0 0 18 18"" xmlns=""http://www.w3.org/2000/svg"" fill=""none""><g fill=""#000""><path d=""M2.25 0A2.25 2.25 0 0 0 0 2.25v7.5A2.25 2.25 0 0 0 2.25 12h.25a.75.75 0 0 0 0-1.5h-.25a.75.75 0 0 1-.75-.75v-7.5a.75.75 0 0 1 .75-.75h7.5a.75.75 0 0 1 .75.75v.25a.75.75 0 0 0 1.5 0v-.25A2.25 2.25 0 0 0 9.75 0z""/><path fill-rule=""evenodd"" d=""M6.25 4A2.25 2.25 0 0 0 4 6.25v7.5A2.25 2.25 0 0 0 6.25 16h7.5A2.25 2.25 0 0 0 16 13.75v-7.5A2.25 2.25 0 0 0 13.75 4zM5.5 6.25a.75.75 0 0 1 .75-.75h7.5a.75.75 0 0 1 .75.75v7.5a.75.75 0 0 1-.75.75h-7.5a.75.75 0 0 1-.75-.75z"" clip-rule=""evenodd""/></g></svg>";
        string deleteIcon = @"<svg width=""24"" height=""24"" viewBox=""0 0 24 24"" fill=""none"" xmlns=""http://www.w3.org/2000/svg""><path d=""M7 4a2 2 0 0 1 2-2h6a2 2 0 0 1 2 2v2h4a1 1 0 1 1 0 2h-1.069l-.867 12.142A2 2 0 0 1 17.069 22H6.93a2 2 0 0 1-1.995-1.858L4.07 8H3a1 1 0 0 1 0-2h4zm2 2h6V4H9zM6.074 8l.857 12H17.07l.857-12zM10 10a1 1 0 0 1 1 1v6a1 1 0 1 1-2 0v-6a1 1 0 0 1 1-1m4 0a1 1 0 0 1 1 1v6a1 1 0 1 1-2 0v-6a1 1 0 0 1 1-1"" fill=""#0d0d0d""/></svg>";
        string navDrawerIcon = @"<svg xmlns=""http://www.w3.org/2000/svg"" viewBox=""0 0 24 24"" width=""24"" height=""24""><rect x = ""2.5"" y=""3.5"" width=""19"" height=""17"" rx=""0.8"" fill=""#FFFFFF"" stroke=""#000000"" stroke-width=""1.7""/><rect x = ""13.8"" y=""6"" width=""5.2"" height=""12"" fill=""#000000""/></svg>";
        string descriptionIcon = @"<path d=""M2.75 17h12.5a.75.75 0 0 1 .102 1.493l-.102.007H2.75a.75.75 0 0 1-.102-1.493zh12.5zm0-4h18.5a.75.75 0 0 1 .102 1.493l-.102.007H2.75a.75.75 0 0 1-.102-1.493zh18.5zm0-4h18.5a.75.75 0 0 1 .102 1.493l-.102.007H2.75a.75.75 0 0 1-.102-1.493zh18.5zm0-4h18.5a.75.75 0 0 1 .102 1.493l-.102.007H2.75a.75.75 0 0 1-.102-1.493zh18.5z"" fill=""#212121""/>";
        string involvedIcon = @"<svg width=""25"" height=""25"" viewBox=""0 0 48 48"" fill=""none"" xmlns=""http://www.w3.org/2000/svg""><path d=""M3 29.4c0-4.256 8.661-6.4 13-6.4s13 2.144 13 6.4V35H3zM23 14c0 3.867-3.133 7-7 7s-7-3.133-7-7 3.133-7 7-7 7 3.133 7 7m17 4c0 2.762-2.237 5-5 5s-5-2.238-5-5 2.237-5 5-5 5 2.238 5 5"" fill=""#333""/><path fill-rule=""evenodd"" clip-rule=""evenodd"" d=""M31 35v-5.6c0-1.364-.532-2.511-1.28-3.437C31.57 25.322 33.583 25 35 25c3.337 0 10 1.787 10 5.333V35z"" fill=""#333""/></svg>";

        protected override void OnParametersSet()
        {
            if (Math.Abs(Node.X - lastX) > 0.1 || Math.Abs(Node.Y - lastY) > 0.1)
            {
                positionX = Node.X.ToString("F0");
                positionY = Node.Y.ToString("F0");
                lastX = Node.X;
                lastY = Node.Y;
            }

            linkButtonTooltip = IsLinkingMode 
                ? "Cancel linking" 
                : Node.ConnectedNodeIds.Any() 
                    ? "Remove connections" 
                    : "Link to another node";
        }

        protected override bool ShouldRender()
        {
            bool shouldRender = Math.Abs(Node.X - lastX) > 0.1 ||
                               Math.Abs(Node.Y - lastY) > 0.1 ||
                               IsSelected != lastIsSelected ||
                               isLinkButtonClicked != lastIsLinkButtonClicked;

            if (shouldRender)
            {
                lastIsSelected = IsSelected;
                lastIsLinkButtonClicked = isLinkButtonClicked;
            }
            return shouldRender;
        }

        private async Task OnLinkButtonClicked()
        {
            if (!IsSelected) return;

            if (Node.ConnectedNodeIds.Any())
            {
                // Remove existing connections
                await OnRemoveConnections.InvokeAsync(Node);
            }
            else
            {
                // Start linking mode
                await OnStartLinking.InvokeAsync(Node);
            }
            
            await InvokeAsync(StateHasChanged);
        }
    }
}
