using System.ComponentModel.DataAnnotations.Schema;

namespace StoreAppApi.models.user
{
    [Table("AdminUser")]
    public class AdminUser:BaseUser
    {
        public override string Role => "AdminUser";

    }
}
