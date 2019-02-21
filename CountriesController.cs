using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using KeenHub.Models;
using log4net;
using System.Security.Claims;
using Microsoft.AspNet.Identity;

namespace KeenHub.Controllers
{
    [RoutePrefix("api/Countries")]
    public class CountriesController : ApiController
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ApplicationDbContext db = new ApplicationDbContext();
        private const string LocalLoginProvider = "Local";
        
        [ActionName("GetCountry")]
        [HttpGet]
        public async Task<IHttpActionResult> GetCountry(string code)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //var identity = (ClaimsIdentity)User.Identity;
            //var appUser = db.Users.Find(identity.GetUserId());
            //if (appUser == null)
            //    return Unauthorized();

            Country country = await db.Countries.FindAsync(code);
            if (country == null)
                return NotFound();

            return Ok(country);
        }
        
        [ActionName("GetCountries")]
        [HttpGet]
        public async Task<IHttpActionResult> GetCountries(int nPage = 0, int nResults = Constants.NumericConstants.DefaultPageSize)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //var identity = (ClaimsIdentity)User.Identity;
            //var appUser = db.Users.Find(identity.GetUserId());
            //if (appUser == null)
            //    return Unauthorized();

            Country[] countries = await db.Countries.
                OrderBy(country => country.Code).Skip(nPage * nResults).Take(nResults).ToArrayAsync();

            if (!countries.Any())
                return NotFound();

            return Ok(countries);
        }

        [Authorize]
        [ActionName("PostCountry")]
        [HttpPost]
        // METODO DI AMMINISTRAZIONE DEL SISTEMA
        public async Task<IHttpActionResult> PostCountry([FromBody]Country country)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var identity = (ClaimsIdentity)User.Identity;
            var appUser = db.Users.Find(identity.GetUserId());
            if (appUser == null || !appUser.IsAdmin)
                return Unauthorized();

            if ((await db.Countries.FindAsync(country.Code)) != null)
                return BadRequest(Resources.Errors.CountryAlreadyPresent);

            db.Countries.Add(country);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Log.Error(Resources.Errors.SaveChangesFailed, e);
                return InternalServerError(e);
            }

            return Ok();
        }

        [Authorize]
        [ActionName("PutCountry")]
        [HttpPut]
        // METODO DI AMMINISTRAZIONE DEL SISTEMA
        public async Task<IHttpActionResult> PutCountry(string code, string newName)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var identity = (ClaimsIdentity)User.Identity;
            var appUser = db.Users.Find(identity.GetUserId());
            if (appUser == null || !appUser.IsAdmin)
                return Unauthorized();

            Country country = await db.Countries.FindAsync(code);

            if (country == null)
                return NotFound();

            country.Name = newName;

            db.Entry(country).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Log.Error(Resources.Errors.SaveChangesFailed, e);
                return InternalServerError(e);
            }

            return Ok();
        }

        [Authorize]
        [ActionName("DeleteCountry")]
        [HttpDelete]
        // METODO DI AMMINISTRAZIONE DEL SISTEMA
        public async Task<IHttpActionResult> DeleteCountry(string code)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var identity = (ClaimsIdentity)User.Identity;
            var appUser = db.Users.Find(identity.GetUserId());
            if (appUser == null || !appUser.IsAdmin)
                return Unauthorized();

            Country country = await db.Countries.FindAsync(code);

            if (country == null)
                return NotFound();

            db.Entry(country).State = EntityState.Deleted;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Log.Error(Resources.Errors.SaveChangesFailed, e);
                return InternalServerError(e);
            }

            return Ok();
        }

        #region DefaultMethods

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CountryExists(string id)
        {
            return db.Countries.Count(e => e.Code.Equals(id)) > 0;
        }

        #endregion
    }
}