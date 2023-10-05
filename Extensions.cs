using JegyekAPI.Model;
using static JegyekAPI.DTOs;

namespace JegyekAPI {
    public static class Extensions {
        public static GradeDto AsDto(this Jegyek user) {
            return new GradeDto(user.Id, user.Grade, user.Description, user.Created);
        }
    }
}
