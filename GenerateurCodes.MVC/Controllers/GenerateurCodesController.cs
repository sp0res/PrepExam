using GenerateurCodes.MVC.Data;
using GenerateurCodes.MVC.Interfaces;
using GenerateurCodes.MVC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GenerateurCodes.MVC.Controllers
{
    public class GenerateurCodesController : Controller
    {
        private readonly DataContext _context;

        public GenerateurCodesController(DataContext context)
        {
          _context = context;
        }
        // GET: GenerateurCodesController
        public async Task<ActionResult> Index()
        {
            //Retirer les commentaires pour vider la base de données si besoin
            //await _context.ReInitialiserBaseDeDonnees();
            return View();
        }


        // GET: GenerateurCodesController/Create
        public ActionResult ObtenirCode()
        {
            return View();
        }

        // POST: GenerateurCodesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ObtenirCode(DemandeCodeAcces demandeCodeAcces)
        {
            if(ModelState.IsValid)
            {
                //Sauvegarde de la demande dans la base de données
                _ = _context.InsererDemande(demandeCodeAcces);

                //Génération du code de sécurité du demandeur
                var code = GenererCode(demandeCodeAcces);

                var dateExpiration = DateTime.Now.AddMonths(6);

                //Sauvegarde du code de securité
                _ = _context.InsererCodeAcces(new CodeAcces
                {
                    DateExpiration = dateExpiration,
                    NomDemandeur = demandeCodeAcces.NomDemandeur,
                    Code = code
                });


                ViewBag.Resultat = $"Votre code d'accès est {code}. Il expire le {dateExpiration.ToLongDateString()}";
                
            }
            return View(demandeCodeAcces);
        }

        
        public ActionResult ValiderCode()
        {
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ValiderCode(string nom, string code)
        {
            var codeAccess = await _context.ObtenirCodeAccess(nom);

            if (codeAccess.Any(c => c.Code == code && c.DateExpiration >= DateTime.Now))
                ViewBag.Message = "Votre code est valide";
            else
               ViewBag.Message = "Les informations fournies ne permettent pas d'obtenir un code valide"; 

          return View();
        }

        public async Task<ActionResult> ListerDemandes()
        {
            return View(await _context.ObtenirDemandeCodeAccess());

        }

        private string GenererCode(DemandeCodeAcces demandeCodeAcces)
        {
            //Premier caractère NAS
            var rnd = new Random();
            return  $"{demandeCodeAcces.NAS.ToString()[0]}-{demandeCodeAcces.NomDemandeur.Substring(0, 2)}-{demandeCodeAcces.DateNaissanceDemandeur.Year}-{rnd.Next(0000,9999)}-{demandeCodeAcces.NAS.ToString()[8]}";
        }
    }
}
