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
           // LayoutNodesInGrid();

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
            var fromCode = fromNode.Item.ReportItemCode ?? "";
            var toCode = toNode.Item.ReportItemCode ?? "";

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

        public void RemoveConnection(DiagramNode node)
        {
            var totalRemovedElements = connections.RemoveAll(c => c.FromNodeId == node.Id || c.ToNodeId == node.Id);
            
            foreach (var n in Nodes)
            {
                n.ConnectedNodeIds.Remove(node.Id);
            }
            if (totalRemovedElements >= 1)
                connections.RemoveAll(c => c.FromNodeId == node.Id || c.ToNodeId == node.Id);
            InvokeAsync(StateHasChanged);
        }

        private void UpdateCanvasSize()
        {
            if (!Nodes.Any()) return;

            // Calculate bounds of all nodes
            var minX = Nodes.Min(n => n.X);
            var minY = Nodes.Min(n => n.Y);
            var maxX = Nodes.Max(n => n.X + n.Width * 2);
            var maxY = Nodes.Max(n => n.Y + n.Height * 2);

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
                foreach (var node in Nodes)
                {
                    node.X += offsetX;
                    node.Y += offsetY;
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
    }
}
