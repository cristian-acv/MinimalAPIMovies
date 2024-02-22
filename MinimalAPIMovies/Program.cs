using MinimalAPIMovies.Entities;

var builder = WebApplication.CreateBuilder(args);

var allowedOrigins = builder.Configuration.GetValue<string>("allowedOrigins")!;

//Inicio de area de los servicios


builder.Services.AddCors(opc =>
{
     opc.AddDefaultPolicy(conf =>
     {
        conf.WithOrigins(allowedOrigins).AllowAnyHeader().AllowAnyMethod();
     });

    opc.AddPolicy("free",conf =>
    {
        conf.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });

});


builder.Services.AddOutputCache();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();

//Inicio de area de los middleware

if (builder.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}

app.UseCors();

app.UseOutputCache();


app.MapGet("/", () => "Hello World!");

app.MapGet("/generos", () =>
{
    var generos = new List<Gender>{ 
      
        new Gender
        {
            Id = 1, 
            Name = "Dama"
        },
        new Gender
        {
            Id = 2,
            Name = "Accion"
        },
        new Gender
        {
            Id = 1,
            Name = "Comedia"
        }
    };

    return generos; 
}).CacheOutput( c => c.Expire(TimeSpan.FromSeconds(15)));

app.Run();
