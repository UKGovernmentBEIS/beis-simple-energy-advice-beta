namespace SeaPublicWebsite.DataModels
{
    public class UserRecommendation : Recommendation
    {
        public bool SavedToWishList { get; set; }
        public bool Discarded { get; set; }
        public bool Viewed { get; set; }
    }
}
