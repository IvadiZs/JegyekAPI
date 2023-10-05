namespace JegyekAPI {
    public class DTOs {
        public record GradeDto(Guid Id, int Grade, string Description, DateTimeOffset Created);
        public record CreateGradeDto(int Grade, string Description);
        public record UpdateGradeDto(int Grade, string Description);
    }
}
