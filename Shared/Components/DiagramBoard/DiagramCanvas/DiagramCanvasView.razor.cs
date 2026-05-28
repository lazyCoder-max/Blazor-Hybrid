using Microsoft.AspNetCore.Components;
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

        private void InitializeNodePositions()
        {
            if (!Nodes.Any()) return;

            // Option 1: Grid Layout
            //LayoutNodesInGrid();

            // Option 2: Vertical Flow Layout (uncomment to use)
             // LayoutNodesVertically();

            // Option 3: Horizontal Flow Layout (uncomment to use)
            // LayoutNodesHorizontally();

            // Option 4: Centered Layout (uncomment to use)
             LayoutNodesCentered();
        }

        private void LayoutNodesInGrid()
        {
            const int columns = 3; // Number of columns in grid
            const double horizontalSpacing = 350; // Space between nodes horizontally
            const double verticalSpacing = 250; // Space between nodes vertically
            const double startX = PADDING;
            const double startY = PADDING;

            for (int i = 0; i < Nodes.Count; i++)
            {
                int row = i / columns;
                int col = i % columns;

                Nodes[i].X = startX + (col * horizontalSpacing);
                Nodes[i].Y = startY + (row * verticalSpacing);
            }
        }

        private void LayoutNodesVertically()
        {
            const double verticalSpacing = 200; // Space between nodes
            const double startX = PADDING;
            const double startY = PADDING;

            for (int i = 0; i < Nodes.Count; i++)
            {
                Nodes[i].X = startX;
                Nodes[i].Y = startY + (i * verticalSpacing);
            }
        }

        private void LayoutNodesHorizontally()
        {
            const double horizontalSpacing = 350; // Space between nodes
            const double startX = PADDING;
            const double startY = PADDING;

            for (int i = 0; i < Nodes.Count; i++)
            {
                Nodes[i].X = startX + (i * horizontalSpacing);
                Nodes[i].Y = startY;
            }
        }

        private void LayoutNodesCentered()
        {
            const double spacing = 250;
            double centerX = (MIN_WIDTH - Nodes.First().Width) / 2;
            double centerY = (MIN_HEIGHT - Nodes.First().Height) / 2;

            for (int i = 0; i < Nodes.Count; i++)
            {
                // Arrange in a circle or staggered pattern
                double angle = (2 * Math.PI * i) / Nodes.Count;
                Nodes[i].X = centerX + (spacing * Math.Cos(angle));
                Nodes[i].Y = centerY + (spacing * Math.Sin(angle));
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


        private void DeselectAllNodes()
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

        private void CreateConnection(DiagramNode fromNode, DiagramNode toNode)
        {
            // Determine order based on ReportItemCode
            var fromCode = fromNode.Item.ReportAndReportItemCode ?? "";
            var toCode = toNode.Item.ReportAndReportItemCode ?? "";

            // Check if connection already exists
            if (connections.Any(c => 
                (c.FromNodeId == fromNode.Id && c.ToNodeId == toNode.Id) ||
                (c.FromNodeId == toNode.Id && c.ToNodeId == fromNode.Id)))
            {
                return;
            }

            // Order by ReportItemCode
            //if (string.Compare(fromCode, toCode, StringComparison.Ordinal) <= 0)
            //{
            //    connections.Add(new NodeConnection
            //    {
            //        FromNodeId = fromNode.Id,
            //        ToNodeId = toNode.Id,
            //        FromCode = fromCode,
            //        ToCode = toCode
            //    });
            //    fromNode.ConnectedNodeIds.Add(toNode.Id);
            //}
            //else
            //{
                
            //}
            connections.Add(new NodeConnection
            {
                FromNodeId = fromNode.Id,
                ToNodeId = toNode.Id,
                FromCode = fromCode,
                ToCode = toCode
            });
            fromNode.ConnectedNodeIds.Add(toNode.Id);
        }
        private void HandleNodeSelection(DiagramNode node)
        {
            selectedNode = node;

            // If in linking mode, create connection
            if (isLinkingMode && linkingFromNode != null && linkingFromNode != node)
            {
                CreateConnection(linkingFromNode, node);
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
                Height = node.Height
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
            double buffer = 8.0)
        {
            // Already clear — use it immediately
            if (!HasOverlapAt(node, startX, startY, buffer))
                return (startX, startY);

            const int MaxAttempts = 100;
            const double SpiralSpacing = 1.2;

            for (int attempt = 1; attempt <= MaxAttempts; attempt++)
            {
                double angle = attempt * 0.5;
                double radius = SpiralSpacing * angle * Math.Max(node.Width, node.Height);

                double candidateX = startX + radius * Math.Cos(angle);
                double candidateY = startY + radius * Math.Sin(angle);

                if (!HasOverlapAt(node, candidateX, candidateY, buffer))
                    return (candidateX, candidateY);
            }

            // Spiral exhausted — fall back to below all existing nodes
            return GetFallbackPosition(node, buffer);
        }

        /// <summary>
        /// Returns true if placing <paramref name="node"/> at (<paramref name="x"/>, <paramref name="y"/>)
        /// would overlap any existing node.
        /// </summary>
        private bool HasOverlapAt(DiagramNode node, double x, double y, double buffer = 8.0)
        {
            return Nodes.Any(existing =>
            {
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
        private (double X, double Y) GetFallbackPosition(DiagramNode node, double buffer = 8.0)
        {
            if (!Nodes.Any())
                return (0, 0);

            double lowestY = Nodes.Max(n => n.Y + n.Height);
            return (0, lowestY + buffer);
        }
       
        private void UpdateCanvasSize()
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

        /// <summary>
        /// Determines the optimal connection points between two nodes based on their relative positions.
        /// Returns (startX, startY, endX, endY) coordinates for the connection line.
        /// </summary>
        private (double StartX, double StartY, double EndX, double EndY) CalculateConnectionPoints(
            DiagramNode fromNode, 
            DiagramNode toNode)
        {
            // Calculate center points of both nodes
            var fromCenterX = fromNode.X + fromNode.Width / 2;
            var fromCenterY = fromNode.Y + fromNode.Height / 2;
            var toCenterX = toNode.X + toNode.Width / 2;
            var toCenterY = toNode.Y + toNode.Height / 2;

            // Calculate the angle between node centers
            var deltaX = toCenterX - fromCenterX;
            var deltaY = toCenterY - fromCenterY;
            var angle = Math.Atan2(deltaY, deltaX);

            // Define connection edge points
            // For fromNode: Calculate which edge to use based on angle to target
            double startX, startY;
            double endX, endY;

            // Determine which edge of fromNode to connect from
            // Use the edge that faces toward the toNode
            var absAngle = Math.Abs(angle);
            
            if (absAngle < Math.PI / 4) // Right edge (0° ± 45°)
            {
                startX = fromNode.X + fromNode.Width;
                startY = fromCenterY;
            }
            else if (absAngle > 3 * Math.PI / 4) // Left edge (180° ± 45°)
            {
                startX = fromNode.X;
                startY = fromCenterY;
            }
            else if (angle > 0) // Bottom edge (90° ± 45°)
            {
                startX = fromCenterX;
                startY = fromNode.Y + fromNode.Height;
            }
            else // Top edge (-90° ± 45°)
            {
                startX = fromCenterX;
                startY = fromNode.Y;
            }

            // Now determine which edge of toNode to connect to
            // Use the opposite angle (from toNode back to fromNode)
            var reverseAngle = Math.Atan2(-deltaY, -deltaX);
            var absReverseAngle = Math.Abs(reverseAngle);

            if (absReverseAngle < Math.PI / 4) // Right edge
            {
                endX = toNode.X + toNode.Width;
                endY = toCenterY;
            }
            else if (absReverseAngle > 3 * Math.PI / 4) // Left edge
            {
                endX = toNode.X;
                endY = toCenterY;
            }
            else if (reverseAngle > 0) // Bottom edge
            {
                endX = toCenterX;
                endY = toNode.Y + toNode.Height;
            }
            else // Top edge
            {
                endX = toCenterX;
                endY = toNode.Y;
            }

            return (startX, startY, endX, endY);
        }

        /// <summary>
        /// Alternative simpler approach: connect from the closest edges between two nodes.
        /// This is useful when you want horizontal/vertical preference.
        /// </summary>
        private (double StartX, double StartY, double EndX, double EndY) CalculateConnectionPointsSimple(
            DiagramNode fromNode,
            DiagramNode toNode)
        {
            var fromCenterX = fromNode.X + fromNode.Width / 2;
            var fromCenterY = fromNode.Y + fromNode.Height / 2;
            var toCenterX = toNode.X + toNode.Width / 2;
            var toCenterY = toNode.Y + toNode.Height / 2;

            double startX, startY, endX, endY;

            // Horizontal distance vs vertical distance
            var horizontalGap = Math.Abs(toCenterX - fromCenterX);
            var verticalGap = Math.Abs(toCenterY - fromCenterY);

            if (horizontalGap > verticalGap)
            {
                // Prefer horizontal connection (right to left or left to right)
                if (toCenterX > fromCenterX)
                {
                    // Connect from right edge of fromNode to left edge of toNode
                    startX = fromNode.X + fromNode.Width;
                    startY = fromCenterY;
                    endX = toNode.X;
                    endY = toCenterY;
                }
                else
                {
                    // Connect from left edge of fromNode to right edge of toNode
                    startX = fromNode.X;
                    startY = fromCenterY;
                    endX = toNode.X + toNode.Width;
                    endY = toCenterY;
                }
            }
            else
            {
                // Prefer vertical connection (top to bottom or bottom to top)
                if (toCenterY > fromCenterY)
                {
                    // Connect from bottom edge of fromNode to top edge of toNode
                    startX = fromCenterX;
                    startY = fromNode.Y + fromNode.Height;
                    endX = toCenterX;
                    endY = toNode.Y;
                }
                else
                {
                    // Connect from top edge of fromNode to bottom edge of toNode
                    startX = fromCenterX;
                    startY = fromNode.Y;
                    endX = toCenterX;
                    endY = toNode.Y + toNode.Height;
                }
            }

            return (startX, startY, endX, endY);
        }
    }
}
