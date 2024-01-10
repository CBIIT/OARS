namespace TheradexPortal.Data
{
    public class ThorConstants
    {
        // Navigation
        public const string DASHBOARD_PAGE_PATH = "/dashboard";
        public const string FAVORITE_PAGE_PATH = "/myfavorites";

        // PBI Embed
        public static string PBI_VISUAL_TYPE_SLIICER = "slicer";
        public static IList<object> PBI_STUDY_FILTER_TARGETS = new List<object>
        {
            new { table = "IFA_TEST_AND_RESULTS", column = "STUDYID" },
            new { table = "TSO500_TESTS_AND_RESULTS", column = "STUDY_ID" },
            new { table = "BIOSPECIMEN_ROADMAP", column = "STUDYID" },
            new { table = "LUMINEX", column = "STUDY_ID" },
        };
    }
}
