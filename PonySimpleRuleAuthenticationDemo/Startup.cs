using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using PonySimpleRuleAuthentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PonySimpleRuleAuthenticationDemo
{

    public enum AuthTypeEnum { COOKIEWITHRULE, COOKIEWITHUSERS, JWTWITHRULE_WITHCONFIG, JWTWITHRULE_WITHOUTCONFIG,
        JWTWITHRULE_WITHOUTCONFIG_FULL, JWTWITHUSER
    };
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();


            AuthTypeEnum AuthDemotype = AuthTypeEnum.JWTWITHUSER;


            switch (AuthDemotype)
            {
                case AuthTypeEnum.COOKIEWITHRULE:
                    services.AddPonySimpleRuleCookieAuthentication(options => { options.ExpireTimeSpan = TimeSpan.FromSeconds(200); },
                        (uid, pwd) => pwd == "b" ? "admin" : null);
                    break;
                case AuthTypeEnum.COOKIEWITHUSERS:
                    services.AddPonySimpleRuleCookieAuthentication(options => { options.ExpireTimeSpan = TimeSpan.FromSeconds(200); },
                          Configuration, "Users");
                    break;



                case AuthTypeEnum.JWTWITHRULE_WITHCONFIG:
                    services.AddPonySimpleRuleJWTAuthentication ( Configuration, "jwt", (uid, pwd) => pwd == "b" ? "admin" : null );
                    break;


                case AuthTypeEnum.JWTWITHRULE_WITHOUTCONFIG:
                    services.AddPonySimpleRuleJWTAuthentication(new PonySimpleRuleOptions() { SigningKey = Configuration.GetSection("jwt:SigningKey").Value, ExpiredInMinutes = 60 * 24 * 3, AuthFunc = (uid, pwd) => pwd == "b" ? "admin" : null  });
                    break;

                case AuthTypeEnum.JWTWITHRULE_WITHOUTCONFIG_FULL:
                    services.AddPonySimpleRuleJWTAuthentication((options => {


                        options.RequireHttpsMetadata = false;
                        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,


                            //ValidateIssuer = true,
                            //ValidateAudience = true,
                            //ValidIssuer = "http://localhost:44300",
                            //ValidAudience = "http://localhost:44300",


                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetSection("jwt:SigningKey").Value))
                            //IssuerSigningKey = new SymmetricSecurityKey( Encoding.UTF8.GetBytes ("YVBy0OLlMQG6VVVp1OH7Xzyr7gHuw1qvUC5dcGt3SBM=)"))
                        };




                    }),


                    ((uid, pwd) => pwd == "b" ? "admin" : null)

                    );
                    break;


                case AuthTypeEnum.JWTWITHUSER:
                    services.AddPonySimpleRuleJWTAuthentication(Configuration, "jwt", "users");
                    break;



            }


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseStrictSameSiteMode();


            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
