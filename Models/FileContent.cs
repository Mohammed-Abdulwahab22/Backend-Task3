namespace Task3.Models
{
    public enum QueryType
    {
        Create,
        Update,
        Delete,
        Retrieve
    }

    public class FileContent 
    {
        public QueryType OperationType { get; set; }
        public IFormFile? File { get; set; }
        public string FileName { get; set; }
        public string Owner { get; set; }
        public string? Description { get; set; }

        //public DateTime CreationDate { get; set; } = DateTime.Now;

        //public DateTime ModificationDate { get; set; } = DateTime.Now;


    }
}
