using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace SistemaBecas.Client.Services.Authentication
{
    public class CustomAuthenticationStateProvider
        : AuthenticationStateProvider
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly TokenService _tokenService;

        private ClaimsPrincipal _usuarioActual =
            new ClaimsPrincipal(new ClaimsIdentity());

        public CustomAuthenticationStateProvider(
            IJSRuntime jsRuntime,
            TokenService tokenService)
        {
            _jsRuntime = jsRuntime;
            _tokenService = tokenService;
        }

        public override Task<AuthenticationState>
            GetAuthenticationStateAsync()
        {
            return Task.FromResult(
                new AuthenticationState(_usuarioActual));
        }

        public async Task CargarSesionAsync()
        {
            try
            {
                var token = await _jsRuntime.InvokeAsync<string?>(
                    "localStorage.getItem",
                    "token");

                if (string.IsNullOrWhiteSpace(token))
                {
                    _usuarioActual =
                        new ClaimsPrincipal(
                            new ClaimsIdentity());

                    _tokenService.EliminarToken();

                    NotifyAuthenticationStateChanged(
                        Task.FromResult(
                            new AuthenticationState(
                                _usuarioActual)));

                    return;
                }

                // Guardar el token también en memoria
                _tokenService.GuardarToken(token);

                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                var claims = jwtToken.Claims.ToList();

                var identity = new ClaimsIdentity(
                    claims,
                    authenticationType: "jwt",
                    ClaimTypes.Name,
                    ClaimTypes.Role);

                _usuarioActual =
                    new ClaimsPrincipal(identity);

                NotifyAuthenticationStateChanged(
                    Task.FromResult(
                        new AuthenticationState(
                            _usuarioActual)));
            }
            catch
            {
                _usuarioActual =
                    new ClaimsPrincipal(
                        new ClaimsIdentity());

                _tokenService.EliminarToken();

                NotifyAuthenticationStateChanged(
                    Task.FromResult(
                        new AuthenticationState(
                            _usuarioActual)));
            }
        }

        public async Task IniciarSesion(string token)
        {
            // Guardar token en localStorage
            await _jsRuntime.InvokeVoidAsync(
                "localStorage.setItem",
                "token",
                token);

            // Guardar token en memoria
            _tokenService.GuardarToken(token);

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var claims = jwtToken.Claims.ToList();

            var identity = new ClaimsIdentity(
                claims,
                authenticationType: "jwt",
                ClaimTypes.Name,
                ClaimTypes.Role);

            _usuarioActual =
                new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(
                Task.FromResult(
                    new AuthenticationState(
                        _usuarioActual)));
        }

        public async Task CerrarSesion()
        {
            await _jsRuntime.InvokeVoidAsync(
                "localStorage.removeItem",
                "token");

            await _jsRuntime.InvokeVoidAsync(
                "localStorage.removeItem",
                "usuario");

            // Eliminar token de memoria
            _tokenService.EliminarToken();

            _usuarioActual =
                new ClaimsPrincipal(
                    new ClaimsIdentity());

            NotifyAuthenticationStateChanged(
                Task.FromResult(
                    new AuthenticationState(
                        _usuarioActual)));
        }
    }
}