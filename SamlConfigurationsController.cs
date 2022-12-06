using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
//using EMSuite.Models;
using EMSuite.ViewModels;
using ComponentSpace.Saml2.Configuration;
using ComponentSpace.Saml2.Configuration.Database;
using ComponentSpace.Saml2.Utility;


namespace EMSuite.Controllers
{
    public class SamlConfigurationsController : Controller
    {
        //private readonly EMSuite.ViewModels.EMSuiteContext _context;
        private readonly SamlConfigurationContext _context;
       
        public SamlConfigurationsViewModel model = new SamlConfigurationsViewModel();
        

        public SamlConfigurationsController(SamlConfigurationContext context)
        {
            this._context = context;
        }

        // GET: SamlConfigurations
        public async Task<IActionResult> Index(SamlConfigurationsViewModel model)
        {
           
            var data = _context.SamlConfigurations
                .Include(p => p.PartnerIdentityProviderConfigurations)
                .First().PartnerIdentityProviderConfigurations.ToList();
               
            return View(data);

           
        }

        // GET: SamlConfigurations/Create
        public IActionResult Create()
        {
            //ViewData["LocalIdentityProviderConfigurationId"] = new SelectList(_context.LocalIdentityProviderConfigurations, "Id", "Id");
            //ViewData["LocalServiceProviderConfigurationId"] = new SelectList(_context.LocalServiceProviderConfigurations, "Id", "Id");
            return View();
        }

        // POST: SamlConfigurations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SamlConfigurationsViewModel model)
        {
         
            try
            {
                var samlConfiguration = new SamlConfiguration()
                {
                    //LocalServiceProviderConfiguration = new LocalServiceProviderConfiguration()
                    //{
                    //    Name = "https://EMSuite",
                    //    Description = "",
                    //    AssertionConsumerServiceUrl = "https://localhost:7231/SAML/AssertionConsumerService",
                    //    SingleLogoutServiceUrl = "https://localhost:7231/SAML/SingleLogoutService",
                    //    ArtifactResolutionServiceUrl = "https://localhost:7231/SAML/ArtifactResolutionService",
                    //    LocalCertificates = new List<Certificate>()
                    //    {
                    //        new Certificate()
                    //        {
                    //            FileName = "certificates/sp.pfx",
                    //            Password = "password"
                    //        }
                    //    }
                    //},
                    LocalServiceProviderConfiguration = new LocalServiceProviderConfiguration()
                    {
                        Name = model.LocalName,
                        Description = model.LocalDescription,
                        AssertionConsumerServiceUrl = model.AssertionConsumerServiceUrl,
                        SingleLogoutServiceUrl = model.LocalSingleLogoutServiceUrl,
                        //ArtifactResolutionServiceUrl = model.ArtifactResolutionServiceUrl,
                        LocalCertificates = new List<Certificate>()
                        {
                            new Certificate()
                            {
                                FileName = "certificates/sp.pfx",
                                Password = "password"
                            }
                        }
                    },

                    PartnerIdentityProviderConfigurations = new List<PartnerIdentityProviderConfiguration>()
                    {
                        new PartnerIdentityProviderConfiguration()
                        {
                            Name = model.Name,
                            Description = model.Description,
                            SignAuthnRequest = true,
                            SignLogoutRequest = true,
                            SignLogoutResponse = true,
                            WantLogoutRequestSigned = true,
                            WantLogoutResponseSigned = true,
                            SingleSignOnServiceUrl = model.SingleSignOnServiceUrl,
                            SingleLogoutServiceUrl = model.SingleLogoutServiceUrl,
                            PartnerCertificates = new List<Certificate>()
                            {
                                new Certificate()
                                {
                                    FileName = model.FileName
                                }
                            }
                        }
                    }
                };

                _context.SamlConfigurations.Add(samlConfiguration);
                _context.SaveChanges();
                RedirectToAction("/Index");
            }
            catch
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }

            return View(model);

         }
        

        // GET: SamlConfigurations/Edit/5
        public async Task<IActionResult> Edit(int? id, SamlConfigurationsViewModel model)
        {
            if (id == null || _context.SamlConfigurations == null)
            {
                return NotFound();
            }

            var samlConfiguration = await _context.SamlConfigurations.FindAsync(id);
            if (samlConfiguration == null)
            {
                return NotFound();
            }
            //ViewData["PartnerIdentityProviderConfigurationId"] = new SelectList(_context.PartnerIdentityProviderConfigurations, "Id", "Id", model.Id);
            //ViewData["LocalServiceProviderConfigurationId"] = new SelectList(_context.LocalServiceProviderConfigurations, "Id", "Id", samlConfiguration.LocalServiceProviderConfigurationId);
            return View(model);
        }

        // POST: SamlConfigurations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SamlConfigurationsViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            //if (ModelState.IsValid)
            //{
                try
                {
                    var samlConfiguration = new SamlConfiguration()
                    {
                        LocalServiceProviderConfiguration = new LocalServiceProviderConfiguration()
                        {
                            Name = "https://EMSuite",
                            Description = "",
                            AssertionConsumerServiceUrl = "https://localhost:7231/SAML/AssertionConsumerService",
                            SingleLogoutServiceUrl = "https://localhost:7231/SAML/SingleLogoutService",
                            ArtifactResolutionServiceUrl = "https://localhost:7231/SAML/ArtifactResolutionService",
                            LocalCertificates = new List<Certificate>()
                        {
                            new Certificate()
                            {
                                FileName = "certificates/sp.pfx",
                                Password = "password"
                            }
                        }
                        },
                        PartnerIdentityProviderConfigurations = new List<PartnerIdentityProviderConfiguration>()
                        {
                            new PartnerIdentityProviderConfiguration()
                            {
                                Id = model.Id,
                                Name = model.Name,
                                SignAuthnRequest = true,
                                SignLogoutRequest = true,
                                SignLogoutResponse = true,
                                WantLogoutRequestSigned = true,
                                WantLogoutResponseSigned = true,
                                SingleSignOnServiceUrl = model.SingleSignOnServiceUrl,
                                SingleLogoutServiceUrl = model.SingleLogoutServiceUrl,
                                PartnerCertificates = new List<Certificate>()
                                {
                                    new Certificate()
                                    {
                                        
                                        FileName = model.FileName
                                    }
                                }
                            }
                        }
                    };

                    _context.SamlConfigurations.Update(samlConfiguration);
                    await _context.SaveChangesAsync();
                    RedirectToAction("Index", "SamlConfigurations");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SamlConfigurationExists(model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                //return RedirectToAction(nameof(Index));
            //}
            //ViewData["ParnerIdentityProviderConfigurationId"] = new SelectList(_context.PartnerIdentityProviderConfigurations, "Id", "Id", model.Id);
            //ViewData["LocalServiceProviderConfigurationId"] = new SelectList(_context.PartnerIdentityProviderConfigurations, "Id", "Id", model.Id);
            return View(model);
        }

        // GET: SamlConfigurations/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int? id, SamlConfigurationsViewModel model)
        {
            if (id == null || _context.SamlConfigurations == null)
            {
                return NotFound();
            }
          
          
                await _context.SamlConfigurations
                .Include(p => p.PartnerIdentityProviderConfigurations)
                //.Include(p => p.PartnerIdentityProviderConfigurations)
                .FirstOrDefaultAsync(p => p.Id == id);

          
            return View(model);
        }

        // POST: SamlConfigurations/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
         
            //await _context.SaveChangesAsync();
            //return RedirectToAction(nameof(Index));

            return View();
        }

        private bool SamlConfigurationExists(int id)
        {
          return _context.SamlConfigurations.Any(e => e.Id == id);
        }
    }
}
