using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using GerenciadorAlunosV2.Contexts;
using GerenciadorAlunosV2.Repositories;

var builder = WebApplication.CreateBuilder(args);

// adição da configuração da connection, meu dbcontext e o mysql
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<GerenciadorAlunosDbContext>(options => options.UseMySQL(connectionString));

// agora com o Identity, tenho que builder os seus servicos. Faço de usuário padrão e perfis.
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => 
{
    // aproveitei pra já passar uns parâmetros
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 8;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedAccount = false;
    options.SignIn.RequireConfirmedEmail = false;
})
    .AddEntityFrameworkStores<GerenciadorAlunosDbContext>()
    .AddDefaultTokenProviders(); 

// daí agora preciso do cookie de autenticação
builder.Services.ConfigureApplicationCookie(options =>
{
    // se um usuário não logado tentar acessar uma página bloqueada, volta pro login
    options.LoginPath = "/Conta/Login";
    // se um usuário tentar acessar uma página q não tem privilégios suficientes, é direcionado p acesso negado
    options.AccessDeniedPath = "/Conta/AcessoNegado";
    
});

// preciso registrar meus repositorios na injeção de dependência do aspnet
// o addscoped cria uma instância do repositório por requisição web
builder.Services.AddScoped<AlunoRepository>();
builder.Services.AddScoped<MensalidadeRepository>();

// preciso incluir os servicos do padrão mvc, q seriam minhas controllers e minhas views
builder.Services.AddControllersWithViews();

var app = builder.Build();

// configuração de ambiente - já veio automaticamente com a criação do projeto
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

// definição da minha rota padrão
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
