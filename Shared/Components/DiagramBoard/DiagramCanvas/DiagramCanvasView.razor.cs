using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using MudBlazor;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Components.DiagramBoard.DiagramCanvas
{
    public partial class DiagramCanvasView
    {
        [Inject]  private ISnackbar _snackbar { get; set; } = default!;

        private MudPaper? canvasRef;
        private DiagramNode? draggingNode;
        private double canvasWidth = 849;
        private double canvasHeight = 549;
        private const double PADDING = 100;
        private const double MIN_WIDTH = 849;
        private const double MIN_HEIGHT = 549;
        private DiagramNode? selectedNode;
        private bool isInitialized = false;

        // Connection management
        private DiagramNode? linkingFromNode = null;
        private bool isLinkingMode = false;
        private List<NodeConnection> connections = new List<NodeConnection>();


        private double panX = 0;
        private double panY = 0;
        private bool isPanning = false;
        private double lastMouseX = 0;
        private double lastMouseY = 0;
        private const double MIN_ZOOM = 0.25;
        private const double MAX_ZOOM = 3.0;
        private const double ZOOM_STEP = 0.1;

        public double ZoomLevel { get; set; } = 1.0;

        [Parameter]
        public List<DiagramNode> Nodes { get; set; } = new()
        {
            new DiagramNode { Title = "Node 1", Type = "Process", Item = new()
            {
                ReportItemId = 12,
                ReportItemShort = "Heizungsinstallationen",
                ReportItemCode = "002",
                ReportID = 27,
                attributeValues = null,
                InvolvedEntities = new List<Involvedentity>
                {
                    new Involvedentity
                    {
                        EntityIsResponsible = false,
                        ID = 166,
                        CompanyProjectDataGuid = "4668e7d7-5951-4e82-b09d-4d8d13e78a42",
                        EmployeeProjectDataGuid = "21c9acdc-28d4-422f-88a2-5632589803bf",
                        EmployeeProjectDataID = 44,
                        EmployeeProjectDataShort = "Pezzei",
                        EmployeeProjectDataLong = "Roland Pezzei Pezzei",
                        EmployeeProjectDataCompanyShort = "PROMAN",
                        EmployeeProjectDataCompanyLong = "Proman",
                        EmployeeProjectDataGroupGuid = "4559de35-6406-46c3-90f1-d3ec332b8272",
                        InvolvedLongName = "Roland Pezzei Pezzei",
                        InvolvedShortName = "Pezzei",
                        LastModifiedDate = new DateTime(2024, 6, 1, 12, 0, 0)
                    },
                },
                Chapter = null,
                ReportItemContentText = "Die Heizungsinstallationen werden nächste Woche anfangen, wie geplant.",
                ItemStatus = new Itemstatus
                {
                    ReportItemStatusId = 382,
                    StatusType = "InProgress",
                    LastModifiedDate = new DateTime(2024, 6, 1, 12, 0, 0),
                    DeletionDate = null,
                    IsDeleted = false,
                    ExternalId = null
                },
                StatusText = "in Arbeit",
                ReportAndReportItemCode = "001.002",
                ReportCollectionID = 41,
                ReportCode = "001",
            }},
            new DiagramNode { Title = "Node 1", Type = "Process", Item = new()
            {
                ReportItemId = 12,
                ReportItemShort = "Wohnzimmeranpassungen",
                ReportItemCode = "001",
                ReportID = 27,
                attributeValues = null,
                InvolvedEntities = new List<Involvedentity>
                {
                    new Involvedentity
                    {
                        EntityIsResponsible = false,
                        ID = 166,
                        CompanyProjectDataGuid = "4668e7d7-5951-4e82-b09d-4d8d13e78a42",
                        EmployeeProjectDataGuid = "21c9acdc-28d4-422f-88a2-5632589803bf",
                        EmployeeProjectDataID = 44,
                        EmployeeProjectDataShort = "Pezzei",
                        EmployeeProjectDataLong = "Roland Pezzei Pezzei",
                        EmployeeProjectDataCompanyShort = "PROMAN",
                        EmployeeProjectDataCompanyLong = "Proman",
                        EmployeeProjectDataGroupGuid = "4559de35-6406-46c3-90f1-d3ec332b8272",
                        InvolvedLongName = "Roland Pezzei Pezzei",
                        InvolvedShortName = "Pezzei",
                        LastModifiedDate = new DateTime(2024, 6, 1, 12, 0, 0)
                    },
                     new Involvedentity
                    {
                        EntityIsResponsible = false,
                        ID = 166,
                        CompanyProjectDataGuid = "4668e7d7-5951-4e82-b09d-4d8d13e78a42",
                        EmployeeProjectDataGuid = "21c9acdc-28d4-422f-88a2-5632589803bf",
                        EmployeeProjectDataID = 44,
                        EmployeeProjectDataShort = "Pezzei",
                        EmployeeProjectDataLong = "Roland Pezzei Pezzei",
                        EmployeeProjectDataCompanyShort = "PROMAN",
                        EmployeeProjectDataCompanyLong = "Proman",
                        EmployeeProjectDataGroupGuid = "4559de35-6406-46c3-90f1-d3ec332b8272",
                        InvolvedLongName = "Roland Pezzei Pezzei",
                        InvolvedShortName = "Pezzei",
                        LastModifiedDate = new DateTime(2024, 6, 1, 12, 0, 0)
                    }
                },
                Chapter = null,
                ReportItemContentText = "Im Wohnzimmer werden mehr Steckdosen und Beleuchtungspunkte hinzugef?gt. Die Bodenbel?ge werden von Laminat auf Fliesen (60x60 cm, Beige) ge?ndert. Die Fliesen sind bereits besorgt und m?ssen abgeholt werden.",
                ItemStatus = new Itemstatus
                {
                    ReportItemStatusId = 382,
                    StatusType = "InProgress",
                    LastModifiedDate = new DateTime(2024, 6, 1, 12, 0, 0),
                    DeletionDate = null,
                    IsDeleted = false,
                    ExternalId = null
                },
                StatusText = "Beschluss",
                ReportAndReportItemCode = "001.001",
                ReportCollectionID = 41,
                ReportCode = "001",
            }},
             new DiagramNode { Title = "Node 1", Type = "Process", Item = new()
            {
                ReportItemId = 12,
                ReportItemShort = "T?rposition und Stemmung",
                ReportItemCode = "004",
                ReportID = 27,
                attributeValues = null,
                InvolvedEntities = new List<Involvedentity>
                {
                    new Involvedentity
                    {
                        EntityIsResponsible = false,
                        ID = 166,
                        CompanyProjectDataGuid = "4668e7d7-5951-4e82-b09d-4d8d13e78a42",
                        EmployeeProjectDataGuid = "21c9acdc-28d4-422f-88a2-5632589803bf",
                        EmployeeProjectDataID = 44,
                        EmployeeProjectDataShort = "Pezzei",
                        EmployeeProjectDataLong = "Roland Pezzei Pezzei",
                        EmployeeProjectDataCompanyShort = "PROMAN",
                        EmployeeProjectDataCompanyLong = "Proman",
                        EmployeeProjectDataGroupGuid = "4559de35-6406-46c3-90f1-d3ec332b8272",
                        InvolvedLongName = "Roland Pezzei Pezzei",
                        InvolvedShortName = "Pezzei",
                        LastModifiedDate = new DateTime(2024, 6, 1, 12, 0, 0)
                    },
                     new Involvedentity
                    {
                        EntityIsResponsible = false,
                        ID = 166,
                        CompanyProjectDataGuid = "4668e7d7-5951-4e82-b09d-4d8d13e78a42",
                        EmployeeProjectDataGuid = "21c9acdc-28d4-422f-88a2-5632589803bf",
                        EmployeeProjectDataID = 44,
                        EmployeeProjectDataShort = "Pezzei",
                        EmployeeProjectDataLong = "Roland Pezzei Pezzei",
                        EmployeeProjectDataCompanyShort = "PROMAN",
                        EmployeeProjectDataCompanyLong = "Proman",
                        EmployeeProjectDataGroupGuid = "4559de35-6406-46c3-90f1-d3ec332b8272",
                        InvolvedLongName = "Roland Pezzei Pezzei",
                        InvolvedShortName = "Pezzei",
                        LastModifiedDate = new DateTime(2024, 6, 1, 12, 0, 0)
                    }
                },
                Chapter = null,
                ReportItemContentText = "Die T?rposition der Toilettent?r muss angepasst werden, um mit der Therme zu ?bereinstimmen. Die Stemmungen m?ssen sich entsprechend verschieben, um die T?r zu passen.",
                ItemStatus = new Itemstatus
                {
                    ReportItemStatusId = 382,
                    StatusType = "InProgress",
                    LastModifiedDate = new DateTime(2024, 6, 1, 12, 0, 0),
                    DeletionDate = null,
                    IsDeleted = false,
                    ExternalId = null
                },
                StatusText = "Beschluss",
                ReportAndReportItemCode = "001.004",
                ReportCollectionID = 41,
                ReportCode = "001",
            }},
        };

        protected override void OnInitialized()
        {
            if (!isInitialized)
            {
                InitializeNodePositions();
                isInitialized = true;
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await JSRuntime.InvokeVoidAsync("initDiagramCanvas", "diagram-canvas",
                    DotNetObjectReference.Create(this));
                UpdateCanvasSize();
            }
        }

        public void InitializeNodePositions()
        {
            if (!Nodes.Any()) return;

            // Clear existing positions to ensure clean re-layout
            // This prevents overlap detection from considering old positions
            foreach (var node in Nodes)
            {
                node.X = 0;
                node.Y = 0;
            }

            LayoutNodesInGrid();
        }

        private void LayoutNodesInGrid()
        {
            const double buffer = 20.0;
            const double horizontalSpacing = 350; // Space between nodes horizontally
            const double verticalSpacing = 280; // Space between nodes vertically
            const double startX = PADDING;
            const double startY = PADDING;

            // Calculate optimal number of columns based on node count
            int columns = (int)Math.Ceiling(Math.Sqrt(Nodes.Count));

            // Track which nodes have been positioned
            var positionedNodes = new HashSet<DiagramNode>();

            for (int i = 0; i < Nodes.Count; i++)
            {
                var node = Nodes[i];

                int row = i / columns;
                int col = i % columns;

                double initialX = startX + (col * horizontalSpacing);
                double initialY = startY + (row * verticalSpacing);

                // Check for overlaps and adjust if needed
                var (finalX, finalY) = FindNonOverlappingPosition(
                    node,
                    initialX,
                    initialY,
                    buffer,
                    positionedNodes);

                node.X = finalX;
                node.Y = finalY;

                positionedNodes.Add(node);
            }
        }




        [JSInvokable]
        public Task OnNodeDragComplete(string nodeId, double newX, double newY)
        {
            var node = Nodes.FirstOrDefault(n => n.Id.ToString() == nodeId);
            if (node != null)
            {
                node.X = newX;
                node.Y = newY;
                UpdateCanvasSize();
                InvokeAsync(StateHasChanged);
            }
            return Task.CompletedTask;
        }


        public void DeselectAllNodes()
        {
            selectedNode = null;
            isLinkingMode = false;
            linkingFromNode = null;
            InvokeAsync(StateHasChanged);
        }

        public void StartLinking(DiagramNode node)
        {
            linkingFromNode = node;
            isLinkingMode = true;
            InvokeAsync(StateHasChanged);
        }

        public void CreateConnection(DiagramNode fromNode, DiagramNode toNode, bool isSubReportConnection = false)
        {
            // Determine order based on ReportItemCode
            var fromCode = fromNode.Item.ReportAndReportItemCode ?? "";
            var toCode = toNode.Item.ReportAndReportItemCode ?? "";

            // Check if connection already exists
            if (connections.Any(c =>
                (c.FromNodeId == fromNode.Id && c.ToNodeId == toNode.Id) ||
                (c.FromNodeId == toNode.Id && c.ToNodeId == fromNode.Id)))
            {
                _snackbar.Add("A connection between these nodes already exists.", Severity.Warning);
                return;
            }

            // Connection from subreport-item node to parent report-item node should be allowed

            if(fromNode.IsSubReportItem && !toNode.IsSubReportItem)
            {
                _snackbar.Add("Connection from subreport item to parent report item is not allowed.", Severity.Warning);    
                return;
            }
            if(!fromNode.IsSubReportItem && toNode.IsSubReportItem)
            {
                // Check if any subreport in the target's chain is already connected to another parent
                if (HasSubreportChainWithOtherParent(toNode, fromNode))
                {
                    _snackbar.Add($"only one parent is allowed for subreport chains.", Severity.Warning);
                    return;
                }
            }
            if(fromNode.IsSubReportItem && toNode.IsSubReportItem)
            {
                if(HasSubreportChainWithOtherParent(toNode, fromNode))
                {
                    _snackbar.Add($"Cannot connect two subreport items that are already connected to different parent nodes.", Severity.Warning);
                    return;
                }
            }
            connections.Add(new NodeConnection
            {
                FromNodeId = fromNode.Id,
                ToNodeId = toNode.Id,
                FromCode = fromCode,
                ToCode = toCode,
                IsSubReportConnection = isSubReportConnection 
            });
            fromNode.ConnectedNodeIds.Add(toNode.Id);
        }
        private void HandleNodeSelection(DiagramNode node)
        {
            selectedNode = node;

            // If in linking mode, create connection
            if (isLinkingMode && linkingFromNode != null && linkingFromNode != node)
            {
                CreateConnection(linkingFromNode, node, isSubReportConnection: linkingFromNode.IsSubReportItem);
                isLinkingMode = false;
                linkingFromNode = null;
            }

            InvokeAsync(StateHasChanged);
        }

        public void RemoveConnection(DiagramNode node)
        {
            // Find all connections involving this node
            var connectionsToRemove = connections
                .Where(c => c.FromNodeId == node.Id || c.ToNodeId == node.Id)
                .ToList();

            // Remove the connections
            foreach (var connection in connectionsToRemove)
            {
                connections.Remove(connection);

                // Clean up both sides of the connection
                var fromNode = Nodes.FirstOrDefault(n => n.Id == connection.FromNodeId);
                var toNode = Nodes.FirstOrDefault(n => n.Id == connection.ToNodeId);

                // Remove references from both nodes
                if (fromNode != null)
                {
                    fromNode.ConnectedNodeIds.Remove(connection.ToNodeId);
                }

                if (toNode != null)
                {
                    toNode.ConnectedNodeIds.Remove(connection.FromNodeId);
                }
            }

            // Also clear the node's own ConnectedNodeIds list
            node.ConnectedNodeIds.Clear();

            InvokeAsync(StateHasChanged);
        }
        private void HandleNodeDeletion(DiagramNode node)
        {
            Nodes.Remove(node);
            UpdateCanvasSize();
            InvokeAsync(StateHasChanged);
        }
        private void HandleNodeDuplication(DiagramNode node)
        {
            const double Buffer = 8.0;
            const double BaseOffsetX = 180.0;

            double newX = node.X + BaseOffsetX;
            double newY = node.Y;

            (newX, newY) = FindNonOverlappingPosition(node, newX, newY, Buffer);

            var newNode = new DiagramNode
            {
                Id = Guid.NewGuid(),
                Title = node.Title + " (Copy)",
                Type = node.Type,
                X = newX,
                Y = newY,
                Width = node.Width,
                Height = node.Height,
                IsSubReportItem = node.IsSubReportItem
            };

            newNode.Item = new ReportItem
            {
                ReportItemId = node.Item.ReportItemId,
                ReportItemShort = node.Item.ReportItemShort + " (Copy)",
                ReportItemCode = node.Item.ReportItemCode + "-Copy",
                ReportID = node.Item.ReportID,
                attributeValues = node.Item.attributeValues,
                InvolvedEntities = node.Item.InvolvedEntities?.ToList() ?? new List<Involvedentity>(),
                Chapter = node.Item.Chapter,
                ReportItemContentText = node.Item.ReportItemContentText,
                ItemStatus = node.Item.ItemStatus,
                StatusText = node.Item.StatusText,
                ReportAndReportItemCode = node.Item.ReportAndReportItemCode,
                ReportCollectionID = node.Item.ReportCollectionID,
                ReportCode = node.Item.ReportCode
            };

            Nodes.Add(newNode);

            if (node.IsSubReportItem)
            {
                CreateConnection(node, newNode, isSubReportConnection: true);
            }

            // Update canvas size and ensure proper positioning
            UpdateCanvasSize();


            InvokeAsync(StateHasChanged);
        }

        /// <summary>
        /// Searches outward in an Archimedean spiral from the given start position
        /// until a non-overlapping spot is found for <paramref name="node"/>.
        /// </summary>
        private (double X, double Y) FindNonOverlappingPosition(
            DiagramNode node,
            double startX,
            double startY,
            double buffer = 8.0,
            HashSet<DiagramNode>? alreadyPositioned = null)
        {
            // Already clear — use it immediately
            if (!HasOverlapAt(node, startX, startY, buffer, alreadyPositioned))
                return (startX, startY);

            const int MaxAttempts = 100;
            const double SpiralSpacing = 1.2;

            for (int attempt = 1; attempt <= MaxAttempts; attempt++)
            {
                double angle = attempt * 0.5;
                double radius = SpiralSpacing * angle * Math.Max(node.Width, node.Height);

                double candidateX = startX + radius * Math.Cos(angle);
                double candidateY = startY + radius * Math.Sin(angle);

                if (!HasOverlapAt(node, candidateX, candidateY, buffer, alreadyPositioned))
                    return (candidateX, candidateY);
            }

            // Spiral exhausted — fall back to below all existing nodes
            return GetFallbackPosition(node, buffer, alreadyPositioned);
        }

        /// <summary>
        /// Returns true if placing <paramref name="node"/> at (<paramref name="x"/>, <paramref name="y"/>)
        /// would overlap any existing node.
        /// </summary>
        private bool HasOverlapAt(
            DiagramNode node,
            double x,
            double y,
            double buffer = 8.0,
            HashSet<DiagramNode>? alreadyPositioned = null)
        {
            // If alreadyPositioned is provided, only check against those nodes
            // Otherwise check against all nodes (useful for duplication where all nodes are positioned)
            var nodesToCheck = alreadyPositioned ?? Nodes.ToHashSet();

            return nodesToCheck.Any(existing =>
            {
                // Don't check against self
                if (existing.Id == node.Id)
                    return false;

                double horizontalGap = Math.Abs(existing.X - x);
                double verticalGap = Math.Abs(existing.Y - y);

                double minHorizontal = (existing.Width + node.Width) / 2.0 + buffer;
                double minVertical = (existing.Height + node.Height) / 2.0 + buffer;

                return horizontalGap < minHorizontal
                    && verticalGap < minVertical;
            });
        }

        /// <summary>
        /// Fallback: places the node below all existing nodes when the spiral search fails.
        /// </summary>
        private (double X, double Y) GetFallbackPosition(
            DiagramNode node,
            double buffer = 8.0,
            HashSet<DiagramNode>? alreadyPositioned = null)
        {
            var nodesToCheck = alreadyPositioned ?? Nodes.ToHashSet();

            if (!nodesToCheck.Any())
                return (PADDING, PADDING);

            double lowestY = nodesToCheck.Max(n => n.Y + n.Height);
            return (PADDING, lowestY + buffer);
        }


        public void UpdateCanvasSize()
        {
            if (!Nodes.Any()) return;

            // Calculate bounds of all nodes
            var minX = Nodes.Min(n => n.X);
            var minY = Nodes.Min(n => n.Y);
            var maxX = Nodes.Max(n => n.X + n.Width);
            var maxY = Nodes.Max(n => n.Y + n.Height);

            // Calculate required offsets to maintain padding on left/top
            var offsetX = 0.0;
            var offsetY = 0.0;

            if (minX < PADDING)
            {
                offsetX = PADDING - minX;
            }

            if (minY < PADDING)
            {
                offsetY = PADDING - minY;
            }

            // Apply offsets to all nodes if needed
            if (offsetX > 0 || offsetY > 0)
            {
                foreach (var n in Nodes)
                {
                    n.X += offsetX;
                    n.Y += offsetY;
                }

                // Recalculate bounds after offset
                minX += offsetX;
                minY += offsetY;
                maxX += offsetX;
                maxY += offsetY;
            }

            // Calculate required canvas size with padding on all sides
            var requiredWidth = Math.Max(maxX + PADDING, MIN_WIDTH);
            var requiredHeight = Math.Max(maxY + PADDING, MIN_HEIGHT);

            // Expand canvas if needed
            if (requiredWidth > canvasWidth)
            {
                canvasWidth = requiredWidth;
            }

            if (requiredHeight > canvasHeight)
            {
                canvasHeight = requiredHeight;
            }
            InvokeAsync(StateHasChanged);
        }



        private string GetCanvasStyle()
        {
            return $@"
        height:{canvasHeight}px;
        width:{canvasWidth}px;
        min-height:549px;
        min-width:849px;
        background:white;
        position:relative;
        box-shadow:0 0 10px rgba(0,0,0,0.2);
        background-image:linear-gradient(#ddd 1px, transparent 1px),linear-gradient(90deg, #ddd 1px, transparent 1px);
        background-size:{20 * ZoomLevel}px {20 * ZoomLevel}px;
        overflow:hidden;
        user-select:none;
        transform:scale({ZoomLevel}) translate({panX}px, {panY}px);
        transform-origin:0 0;
        transition:transform 0.1s ease-out;
    ";
        }

        public void ZoomIn()
        {
            if (ZoomLevel < MAX_ZOOM)
            {
                ZoomLevel = Math.Min(ZoomLevel + ZOOM_STEP, MAX_ZOOM);
                InvokeAsync(StateHasChanged);
            }
        }

        public void ZoomOut()
        {
            if (ZoomLevel > MIN_ZOOM)
            {
                ZoomLevel = Math.Max(ZoomLevel - ZOOM_STEP, MIN_ZOOM);
                InvokeAsync(StateHasChanged);
            }
        }

        public void ResetZoom()
        {
            ZoomLevel = 1.0;
            panX = 0;
            panY = 0;
            InvokeAsync(StateHasChanged);
        }

        public void FitToScreen()
        {
            if (!Nodes.Any()) return;

            // Calculate bounds of all nodes
            var minX = Nodes.Min(n => n.X);
            var minY = Nodes.Min(n => n.Y);
            var maxX = Nodes.Max(n => n.X + n.Width);
            var maxY = Nodes.Max(n => n.Y + n.Height);

            var contentWidth = maxX - minX + PADDING * 2;
            var contentHeight = maxY - minY + PADDING * 2;

            // Calculate zoom to fit
            var zoomX = MIN_WIDTH / contentWidth;
            var zoomY = MIN_HEIGHT / contentHeight;
            ZoomLevel = Math.Min(Math.Min(zoomX, zoomY), MAX_ZOOM);
            ZoomLevel = Math.Max(ZoomLevel, MIN_ZOOM);

            // Center the content
            panX = -minX + PADDING;
            panY = -minY + PADDING;

            InvokeAsync(StateHasChanged);
        }

        private void HandleMouseWheel(WheelEventArgs e)
        {
            // Zoom with mouse wheel (Ctrl + Wheel)
            if (e.CtrlKey)
            {
                if (e.DeltaY < 0)
                {
                    ZoomIn();
                }
                else
                {
                    ZoomOut();
                }
            }
        }

        private void HandleMouseDown(MouseEventArgs e)
        {
            // Start panning with middle mouse button or Shift + Left click
            if (e.Button == 1 || (e.Button == 0 && e.ShiftKey))
            {
                isPanning = true;
                lastMouseX = e.ClientX;
                lastMouseY = e.ClientY;
            }
        }

        private void HandleMouseMove(MouseEventArgs e)
        {
            if (isPanning)
            {
                var deltaX = e.ClientX - lastMouseX;
                var deltaY = e.ClientY - lastMouseY;

                panX += deltaX / ZoomLevel;
                panY += deltaY / ZoomLevel;

                lastMouseX = e.ClientX;
                lastMouseY = e.ClientY;

                InvokeAsync(StateHasChanged);
            }
        }

        private void HandleMouseUp(MouseEventArgs e)
        {
            isPanning = false;
        }
        private void HandleAddSubReportItemClicked(DiagramNode node)
        {
            const double Buffer = 8.0;
            const double BaseOffsetX = 180.0;

            double newX = node.X + BaseOffsetX;
            double newY = node.Y;

            (newX, newY) = FindNonOverlappingPosition(node, newX, newY, Buffer);

            var newNode = new DiagramNode
            {
                Id = Guid.NewGuid(),
                Title = node.Title + " (Copy)",
                Type = node.Type,
                X = newX,
                Y = newY,
                Width = node.Width,
                Height = node.Height,
                IsSubReportItem = true
            };

            newNode.Item = new ReportItem
            {
                ReportItemId = node.Item.ReportItemId,
                ReportItemShort = node.Item.ReportItemShort + " (Copy)",
                ReportItemCode = node.Item.ReportItemCode + "-Copy",
                ReportID = node.Item.ReportID,
                attributeValues = node.Item.attributeValues,
                InvolvedEntities = node.Item.InvolvedEntities?.ToList() ?? new List<Involvedentity>(),
                Chapter = node.Item.Chapter,
                ReportItemContentText = node.Item.ReportItemContentText,
                ItemStatus = node.Item.ItemStatus,
                StatusText = node.Item.StatusText,
                ReportAndReportItemCode = node.Item.ReportAndReportItemCode,
                ReportCollectionID = node.Item.ReportCollectionID,
                ReportCode = node.Item.ReportCode
            };

            Nodes.Add(newNode);

            // Update canvas size and ensure proper positioning
            UpdateCanvasSize();

            CreateConnection(node, newNode, isSubReportConnection: true);


            InvokeAsync(StateHasChanged);
        }
        /// <summary>
        /// Removes a specific connection between two nodes.
        /// Called when the connection label is double-clicked.
        /// </summary>
        public void RemoveSpecificConnection(NodeConnection connection)
        {
            // Remove the connection from the list
            connections.Remove(connection);

            // Clean up both sides of the connection
            var fromNode = Nodes.FirstOrDefault(n => n.Id == connection.FromNodeId);
            var toNode = Nodes.FirstOrDefault(n => n.Id == connection.ToNodeId);

            // Remove references from both nodes
            if (fromNode != null)
            {
                fromNode.ConnectedNodeIds.Remove(connection.ToNodeId);
            }

            if (toNode != null)
            {
                toNode.ConnectedNodeIds.Remove(connection.FromNodeId);
            }

            InvokeAsync(StateHasChanged);
        }

        /// <summary>
        /// Recursively checks if a subreport node or any of its connected subreports
        /// is already connected to a different parent node.
        /// </summary>
        private bool HasSubreportChainWithOtherParent(DiagramNode subreportNode, DiagramNode excludeParent)
        {
            if (!subreportNode.IsSubReportItem)
                return false;

            // Check if this subreport is connected to any other parent
            var parentConnections = connections
                .Where(c => c.FromNodeId == subreportNode.Id || c.ToNodeId == subreportNode.Id)
                .Select(c => c.FromNodeId == subreportNode.Id ? c.ToNodeId : c.FromNodeId)
                .Select(id => Nodes.FirstOrDefault(n => n.Id == id))
                .Where(n => n != null && !n.IsSubReportItem && n.Id != excludeParent.Id)
                .ToList();

            if (parentConnections.Any())
                return true;

            // Get all connected subreport nodes
            var connectedSubreports = connections
                .Where(c => c.FromNodeId == subreportNode.Id || c.ToNodeId == subreportNode.Id)
                .Select(c => c.FromNodeId == subreportNode.Id ? c.ToNodeId : c.FromNodeId)
                .Select(id => Nodes.FirstOrDefault(n => n.Id == id))
                .Where(n => n != null && n.IsSubReportItem)
                .ToList();

            // Recursively check each connected subreport
            foreach (var connectedSubreport in connectedSubreports)
            {
                if (HasSubreportChainWithOtherParent(connectedSubreport, excludeParent))
                    return true;
            }

            return false;
        }
    }

}
