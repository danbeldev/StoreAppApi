using StoreAppApi.models.сompany;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace StoreAppApi.models.user
{
    [Table("CompanyUsers")]
    public class CompanyUser : BaseUser
    {
        [JsonIgnore, Required] public Сompany Сompany { get; set; }

        public override string Role => "CompanyUser";
    }
}
