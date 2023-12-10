using Serilog;
using Time.Commerce.GraphQl.Extensions;
using Time.Commerce.Infras.Extensions;

var builder = WebApplication.CreateBuilder(args);

//use graphql
builder.Services.ConfigureGraphQlServices();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

//Add Serilog
builder.Host.UseSerilog((ctx, lc) => lc
      .WriteTo.Console()
      .ReadFrom.Configuration(ctx.Configuration));

builder.Services.ConfigureApplicationServices(builder.Configuration);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
await app.ConfigureRequestPipelineAsync(builder.Configuration);
