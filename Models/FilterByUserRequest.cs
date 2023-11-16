namespace Task3.Models
{
    public enum SortTypeByCreationDateuser
    {
        ascendinguser1,
        descendinguser1
    }

    public enum SortTypeByModificationDateuser
    { ascendingm1, descendingm1 }

    public class FilterByUserRequest
    {
        public string[] Usernames { get; set; }

        public DateTime? startdate { get; set; }
        public DateTime? enddate { get; set; }
        public SortTypeByCreationDateuser? sort_creation_date { get; set; }
        
        public SortTypeByModificationDateuser? sort_modification_date { get; set; }


    }
}
