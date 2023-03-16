using Microsoft.AspNetCore.SignalR;
using TRYOUT.Hubs;
using TRYOUT.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();
builder.Services.AddTransient<IConverterService, Base64Service>();
builder.Services.AddSingleton<ILoopProcess, LoopCharactersProcess>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapHub<ConversionHub>("/encode");


app.Run();

