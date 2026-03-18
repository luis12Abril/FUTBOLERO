  using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;            // ESTA LINEA SE AGREGO


namespace FUTBOLERO.Client.Service
{
    public class Autenticacion : AuthenticationStateProvider
    {

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {

            // Esto equivale a cerrar sesion
            var identity = new ClaimsIdentity();
            var user = new ClaimsPrincipal(identity);
            //return Task.FromResult(new AuthenticationState(user));
            return Task.FromResult(new AuthenticationState(user));

        }

        public void Entrar(string iiduser, string torneo, string idtorneo)
        {
            // Para tener una sesion activa solo agregamos esta linea

            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name,iiduser),
                new Claim(ClaimTypes.Role,torneo),
                new Claim("mitorneo",idtorneo)
            }, "auth");

            //identity.AddClaim(new Claim(ClaimTypes.Role, "LUIS"));
            //identity.AddClaim(new string Roless, "LUIS");

            var user = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        public void cerrarSession()
        {

            // Esto equivale a cerrar sesion
            var identity = new ClaimsIdentity();

            var user = new ClaimsPrincipal();
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));

        }


    }
}
