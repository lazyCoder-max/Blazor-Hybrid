namespace Shared.Models
{
    public class DiagramNode
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public ReportItem Item { get; set; } = new ReportItem();
        public string Title { get; set; } = "";
        public string Type { get; set; } = "";

        public double X { get; set; }
        public double Y { get; set; }

        public double Width { get; set; } = 300;
        public double Height { get; set; } = 91;

        // List of node IDs this node connects to
        public List<Guid> ConnectedNodeIds { get; set; } = new List<Guid>();
        public bool IsSubReportItem { get; set; } = false;
    }

    public class NodeConnection
    {
        public Guid FromNodeId { get; set; }
        public Guid ToNodeId { get; set; }
        public string FromCode { get; set; } = "";
        public string ToCode { get; set; } = "";
        public bool IsSubReportConnection { get; set; } = false; // NEW: Flag for subreport connections
    }

    public class ReportItem
    {
        public int ReportItemId { get; set; }
        public string ReportItemShort { get; set; }
        public string ReportItemCode { get; set; }
        public int ReportID { get; set; }
        public object? attributeValues { get; set; }
        public IList<Involvedentity> InvolvedEntities { get; set; }
        public object? Chapter { get; set; }
        public string ReportItemContentText { get; set; }
        public Itemstatus? ItemStatus { get; set; }
        public object StatusDate { get; set; }
        public string StatusText { get; set; }
        public string ReportAndReportItemCode { get; set; }
        public int ReportCollectionID { get; set; }
        public string ReportCode { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public object DeletionDate { get; set; }
        public bool IsDeleted { get; set; }
        public object ExternalId { get; set; }
    }

    public class Itemstatus
    {
        public int ReportItemStatusId { get; set; }
        public string StatusType { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public object DeletionDate { get; set; }
        public bool IsDeleted { get; set; }
        public object ExternalId { get; set; }
    }

    public class Involvedentity
    {
        public bool EntityIsResponsible { get; set; }
        public bool IsPostAppointment { get; set; }
        public object StartDateIsToBe { get; set; }
        public object StartDateActual { get; set; }
        public object DueDateIsToBe { get; set; }
        public object DueDateActual { get; set; }
        public int ID { get; set; }
        public string CompanyProjectDataGuid { get; set; }
        public object CompanyProjectDataID { get; set; }
        public string CompanyProjectDataShort { get; set; }
        public string CompanyProjectDataLong { get; set; }
        public string EmployeeProjectDataGuid { get; set; }
        public int EmployeeProjectDataID { get; set; }
        public string EmployeeProjectDataShort { get; set; }
        public string EmployeeProjectDataLong { get; set; }
        public string EmployeeProjectDataCompanyShort { get; set; }
        public string EmployeeProjectDataCompanyLong { get; set; }
        public string EmployeeProjectDataGroupGuid { get; set; }
        public string InvolvedLongName { get; set; }
        public string InvolvedShortName { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public object DeletionDate { get; set; }
        public bool IsDeleted { get; set; }
        public object ExternalId { get; set; }
    }

}
