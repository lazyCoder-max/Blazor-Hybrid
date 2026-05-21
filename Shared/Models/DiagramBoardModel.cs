using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class DiagramBoardModel
    {
        public List<DiagramNode> Nodes { get; set; } = new();
        public List<DiagramConnection> Connections { get; set; } = new();

        public double Zoom { get; set; } = 1;
        public double PanX { get; set; }
        public double PanY { get; set; }
    }
}
