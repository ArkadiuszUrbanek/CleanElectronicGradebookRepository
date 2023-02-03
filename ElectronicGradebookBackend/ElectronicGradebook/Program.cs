using ElectronicGradebook.Converters;
using ElectronicGradebook.Models;
using ElectronicGradebook.Repositories;
using ElectronicGradebook.Repositories.Interfaces;
using ElectronicGradebook.Services;
using ElectronicGradebook.Services.Interfaces;
using ElectronicGradebook.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(opts =>
    {
        opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        opts.JsonSerializerOptions.Converters.Add(new ISO8601DateTimeConverter());
    }
);

builder.Services.AddDateOnlyTimeOnlyStringConverters();

//EMail Settings
builder.Services.Configure<EMailSettings>(builder.Configuration.GetSection("EMailSettings"));

//JWT Settings
builder.Services.Configure<JWTSettings>(builder.Configuration.GetSection("JWT"));

//JWT Authentication
builder.Services.AddAuthentication(opts =>
        {
            opts.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }
    )
    .AddJwtBearer(opts =>
    {
        opts.RequireHttpsMetadata = false;
        opts.SaveToken = false;
        opts.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["JWT:Audience"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecurityKey"])),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    }
);

//Repositories
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IAnnouncementRepository, AnnouncementRepository>();
builder.Services.AddTransient<IMessageRepository, MessageRepository>();
builder.Services.AddTransient<ISurveyRepository, SurveyRepository>();
builder.Services.AddTransient<IAnswerRepository, AnswerRepository>();
builder.Services.AddTransient<IClassRepository, ClassRepository>();
builder.Services.AddTransient<ILessonRepository, LessonRepository>();
builder.Services.AddTransient<ITeachingHourRepository, TeachingHourRepository>();
builder.Services.AddTransient<IClassroomRepository, ClassroomRepository>();
builder.Services.AddTransient<ISubjectRepository, SubjectRepository>();
builder.Services.AddTransient<IMarkRepository, MarkRepository>();
builder.Services.AddTransient<IAttendanceRepository, AttendanceRepository>();
builder.Services.AddTransient<IPostRepository, PostRepository>();

//Services
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<IAnnouncementService, AnnouncementService>();
builder.Services.AddTransient<IMessageService, MessageService>();
builder.Services.AddTransient<ISurveyService, SurveyService>();
builder.Services.AddTransient<IClassService, ClassService>();
builder.Services.AddTransient<ILessonService, LessonService>();
builder.Services.AddTransient<IClassroomService, ClassroomService>();
builder.Services.AddTransient<ISubjectService, SubjectService>();
builder.Services.AddTransient<IMarkService, MarkService>();
builder.Services.AddTransient<IAttendanceService, AttendanceService>();
builder.Services.AddTransient<IEMailService, EMailService>();
builder.Services.AddTransient<IPostService, PostService>();

//Database connection
builder.Services.AddDbContext<ElectronicGradebookDatabaseContext>(
    opts =>
    {
        opts.UseSqlServer(builder.Configuration.GetConnectionString("ElectronicGradebookDatabase"));
    }
);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(opts =>
{
        opts.SwaggerDoc("v1",
            new OpenApiInfo()
            {
                Title = "Electronic gradebook Web API",
                Version = "version 1.0",
                Description = "Engineering project's endpoints' definitions."
            }
        );
        opts.AddSecurityDefinition("jwt_auth",
            new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                BearerFormat = "JWT",
                Scheme = "bearer",
                Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http
            }
        );
        opts.AddSecurityRequirement(
            new OpenApiSecurityRequirement() 
            {
                {
                    new OpenApiSecurityScheme()
                    {
                        Reference = new OpenApiReference()
                        {
                            Id = "jwt_auth",
                            Type = ReferenceType.SecurityScheme
                        }
                    },
                    new string[] { }
                }
            }
        );
        opts.UseDateOnlyTimeOnlyStringConverters();
    }
);

//CORS
builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(CORSbuilder =>
        CORSbuilder
            .WithOrigins(builder.Configuration["JWT:Audience"])
            .WithMethods("GET", "POST", "PATCH", "DELETE")
            .AllowAnyHeader()
);

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
