using Microsoft.AspNet.Identity.EntityFramework;

namespace TafelFlyingServices.Models
{
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole()
        {
        }

        public ApplicationRole(string name, string description) : base(name)
        {
            Description = description;
        }

        public virtual string Description { get; set; }
    }
}