namespace SeaPublicWebsite.BusinessLogic.Models
{
    public class HistoricEnglandSearchResult
    {
        public uint ObjectId { get; set; }

        public uint ListEntry { get; set; }
        
        public string Name { get; set; }
        
        public string Grade { get; set; }
        
        public long ListDate { get; set; }
        
        public long? AmendDate { get; set; }
        
        public string CaptureScale { get; set; }
        
        public string Hyperlink { get; set; }

        public HistoricEnglandSearchResult(uint objectId, uint listEntry, string name, string grade, long listDate,
            long? amendDate, string captureScale, string hyperlink)
        {
            ObjectId = objectId;
            ListEntry = listEntry;
            Name = name;
            Grade = grade;
            ListDate = listDate;
            AmendDate = amendDate;
            CaptureScale = captureScale;
            Hyperlink = hyperlink;
        }
    }
}