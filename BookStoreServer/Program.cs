using BookStoreServer.Context;
using BookStoreServer.Services;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddCors(cfr =>
{
    cfr.AddDefaultPolicy(p => p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

#region Dependency Injection
builder.Services.TryAddScoped<JwtService>();
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddScoped<BookwormDbContext>();
#endregion


#region Authentication
builder.Services.AddAuthentication().AddJwtBearer(options =>
{
    options.TokenValidationParameters = new()
    {
        ValidateIssuer = true, //token? g?nderen ki?i bilgisi
        ValidateAudience = true, //token? kullanacak site ya da ki?i bilgisi
        ValidateIssuerSigningKey = true, //token?n g?venlik anahtar? ?retmesini sa?layan g?venlik s?zc???
        ValidateLifetime = true, //tokenun ya?am s?resini kontrol etmek istiyor musunuz
        ValidIssuer = "Oguzhan Muratoglu",
        ValidAudience = "Book Store Web Project",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("my secret key my secret key my secret key 1234 ... my secret key my secret key my secret key 1234 ..."))
    };
});
#endregion


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors();

app.MapControllers();

app.Run();
