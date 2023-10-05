namespace JegyekAPI.Model {
    public class Jegyek {
        public Guid Id { get; set; }
        public int Grade { get; set; }
        public string Description { get; set; }
        public DateTimeOffset Created { get; set; }
    }
}
