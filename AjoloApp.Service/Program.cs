using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using AjoloApp.Repository.AjoloAppRepository;
using AjoloApp.Repository.AjoloAppRepository.Entities;
using AjoloApp.RuleEngine.AssignationRoomService;
using AjoloApp.RuleEngine.AssignationTutorService;
using AjoloApp.RuleEngine.AuthenticationService;
using AjoloApp.RuleEngine.CollaboratorService;
using AjoloApp.RuleEngine.DashboardService;
using AjoloApp.RuleEngine.EnrolledChildrenService;
using AjoloApp.RuleEngine.KidService;
using AjoloApp.RuleEngine.ParentService;
using AjoloApp.RuleEngine.PaymentService;
using AjoloApp.RuleEngine.TypeBusinessService;
using System.Security.Principal;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<IPrincipal>(provider => provider.GetService<IHttpContextAccessor>().HttpContext.User);

#region ServiceType Based
builder.Services.AddScoped<ITypeBusinessService<Turn>, TypeBusinessService<Turn>>();
builder.Services.AddScoped<ITypeBusinessService<Modality>, TypeBusinessService<Modality>>();
builder.Services.AddScoped<ITypeBusinessService<Room>, TypeBusinessService<Room>>();
builder.Services.AddScoped<ITypeBusinessService<City>, TypeBusinessService<City>>();
builder.Services.AddScoped<ITypeBusinessService<BranchOffice>, TypeBusinessService<BranchOffice>>();
builder.Services.AddScoped<ITypeBusinessService<PaymentType>, TypeBusinessService<PaymentType>>();
builder.Services.AddScoped<ITypeBusinessService<PaymentOperation>, TypeBusinessService<PaymentOperation>>();
builder.Services.AddScoped<ITypeBusinessService<AuditPaymentType>, TypeBusinessService<AuditPaymentType>>();
builder.Services.AddScoped<ITypeBusinessService<Relationship>, TypeBusinessService<Relationship>>();
builder.Services.AddScoped<ITypeBusinessService<DocumentType>, TypeBusinessService<DocumentType>>();
builder.Services.AddScoped<ITypeBusinessService<SexType>, TypeBusinessService<SexType>>();
builder.Services.AddScoped<ITypeBusinessService<BloodType>, TypeBusinessService<BloodType>>();
builder.Services.AddScoped<ITypeBusinessService<MaritalStatus>, TypeBusinessService<MaritalStatus>>();
#endregion

builder.Services.AddScoped<IKidService, KidService>();
builder.Services.AddScoped<IParentService, ParentService>();
builder.Services.AddScoped<ICollaboratorService, CollaboratorService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();

builder.Services.AddScoped<IAssignationRoomService, AssignationRoomService>();
builder.Services.AddScoped<IAssignationTutorService, AssignationTutorService>();
builder.Services.AddScoped<IEnrolledChildrenService, EnrolledChildrenService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IRolesBusinessService, RolesBusinessService>();

string mySqlConnectionStr = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AjoloAppContext>(options => options.UseNpgsql(mySqlConnectionStr));
builder.Services.AddIdentityCore<User>(
        //options =>
        //{
        //    options.SignIn.RequireConfirmedAccount = false;
        //}
        )
     .AddRoles<Role>()
     .AddEntityFrameworkStores<AjoloAppContext>();
//jwt
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
// Adding Jwt Bearer  
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
    };
});
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(setup =>
{
    // Include 'SecurityScheme' to use JWT Authentication
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name = "JWT Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    setup.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

    setup.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true) // allow any origin 
    .AllowCredentials());

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
