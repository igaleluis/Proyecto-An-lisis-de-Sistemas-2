using Radzen;
using SistemaBecas.Client.Components;
using SistemaBecas.Client.Services;
using SistemaBecas.Client.Services.Login;
using SistemaBecas.Client.Services.Documentacion;
using Microsoft.AspNetCore.Components.Authorization;
using SistemaBecas.Client.Services.Authentication;
using SistemaBecas.Client.Services.RegistroService;
using SistemaBecas.Client.Services.Reportes;


var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<CustomAuthenticationStateProvider>();

builder.Services.AddScoped<AuthenticationStateProvider>(
    provider =>
        provider.GetRequiredService<CustomAuthenticationStateProvider>());

builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<JwtAuthorizationHandler>();
builder.Services.AddHttpClient<GestionApiClient>(client =>
{
    var apiBaseUrl = builder.Configuration["ApiBaseUrl"];
    if (string.IsNullOrWhiteSpace(apiBaseUrl))
        throw new InvalidOperationException("Debe configurar ApiBaseUrl para conectar el cliente con la API.");

    client.BaseAddress = new Uri(apiBaseUrl);
});


builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddScoped<ContextMenuService>();


//Servicios API

builder.Services.AddHttpClient("Api", client =>
{
    client.BaseAddress = new Uri("https://localhost:7088/");
});
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IRegistroService, RegistroService>();
builder.Services.AddScoped<DocumentacionService>();
builder.Services.AddScoped<ReportesService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);

    // The default HSTS value is 30 days.
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute(
    "/not-found",
    createScopeForStatusCodePages: true
);

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
