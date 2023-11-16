namespace Task3.Models
{
    public enum SortTypeByCreationDate
    {
       ascendingc,
       descendingc
    }

    public enum SortTypeByModificationDate
    { ascendingm, descendingm }

    public class filter1
    {
        
        public DateTime startdate { get; set; }
        public DateTime enddate { get; set; }

        public SortTypeByCreationDate? sort_by_creation_date { get; set; }

        public SortTypeByModificationDate? sort_by_modification_date { get; set; } 


    }
}
