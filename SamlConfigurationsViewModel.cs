using EMSuite.Models;

namespace EMSuite.ViewModels
{
    public class SamlConfigurationsViewModel
    {

        public EMSuiteContext _context { get; set; }
        public int Id { get; set; }

        public string Description { get; set; }
        public string Name { get; set; }
        public string SingleSignOnServiceUrl { get; set; }
        public string SingleLogoutServiceUrl { get; set; }


        public string FileName { get; set; }

        public string Password { get; set; }



        public string LocalName { get; set; }

        public string LocalDescription { get; set; }

        public string LocalSingleLogoutServiceUrl { get; set; }
        public string AssertionConsumerServiceUrl { get; set; }
        public string ArtifactResolutionServiceUrl { get; set; }

        public string LocalFileName { get; set; }





        public List<PartnerIdentityProviderConfiguration> PartnerIdentityProviderConfigurations { get; set; }

        //public virtual IEnumerable<PartnerIdentityProviderConfiguration> PartnerIdentityProviderConfigurations { get; set; }
    }
}
