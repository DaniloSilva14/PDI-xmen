using ApiLambda.Services;
using Microsoft.OpenApi.Models;
using Amazon.S3;
using Amazon.Runtime;
using Amazon.Extensions.NETCore.Setup;
using Microsoft.AspNetCore.Cors.Infrastructure;

namespace ApiLambda;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "API Lambda", Version = "v1" });
        });

        services.AddScoped<IPortfolioService, PortfolioService>();

        services.AddAWSService<IAmazonS3>(new AWSOptions
        {
            Credentials = new BasicAWSCredentials
                (
                    accessKey: Environment.GetEnvironmentVariable("AwsAccessKey"),
                    secretKey: Environment.GetEnvironmentVariable("AwsSecretKey")
                ),
            Region = Amazon.RegionEndpoint.GetBySystemName(Environment.GetEnvironmentVariable("AwsRegion"))
        });

        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder => 
                builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
            );
        });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseCors("CorsPolicy");

        app.UseSwagger(swagger => 
        {
            swagger.RouteTemplate = "swagger/{documentName}/swagger.json";
        });

        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("v1/swagger.json", "API Lambda v1");
            c.RoutePrefix = "swagger";
        });

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapGet("/", async context =>
            {
                await context.Response.WriteAsync("Welcome to running ASP.NET Core on AWS Lambda");
            });
        }); 
    }
}