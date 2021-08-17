
This project demonstrate asp.net core app that use my PonySimpleRuleAuthentication nuget package

PonySimpleRuleAuthentication is a simplified asp.net core Token or JWT authentication that does not authenticate user by lookup from a user database.  Instead the users is either lookup from a appsettings.json file, or a function that convert a uid/pwd pair into a role.  For example, the rule could be that anyone enter the current date in MMdd format is consider valid user.  The use case is for small hobby website that people can contact you to get uid/pwd or to get to know the rule.  Please note PonySimpleRuleAuthentication is designed with ease of use in mind, not security, and is to be used by website that is of very low value like personal blog, it's security level is low. Don't use this package anywhere security is of concern.

How to run the demo app

1. Clone the repro
2. Look for 
              AuthTypeEnum AuthDemotype = AuthTypeEnum.????;
   in ConfigureServices.  Change the enum to whatever you want to test

3. For Cookie auth, go to "Private" action. The will bring up login screen if not already logged in
4. To test JWT auth, click Get JWT Token.  This will request a JWT token with uid : b and pwd :.  Don't use login action because it is for Cookie auth.
5. Click Make JWT Token request, this will return current datetime from a controller action with [Authorize] attribute











How to use PonySimpleRuleAuthentication package:


1. Create a asp.net core mvc app WITHOUT authentication.
2. Add project reference to PonySimpleRuleAuthentication nuget package
3. Add the following code to ConfigureServices

    Sample 1 :
                   services.AddPonySimpleRuleCookieAuthentication(options => { options.ExpireTimeSpan = TimeSpan.FromSeconds(200); },
                        (uid, pwd) => pwd == "b" ? "admin" : null);
    

    Sample 2 :

                   services.AddPonySimpleRuleJWTAuthentication ( Configuration, "jwt", (uid, pwd) => pwd == "b" ? "admin" : null );
 

    Please refere to Startup.cs to see more sample usage.


 
 4. Add the following to Configure after app.UseRouting();

          app.UseAuthentication();
          app.UseStrictSameSiteMode(); //optional


5. Add this line to Views/Shared/_Layout.cshtml  to show the login/logout ui

    <partial name="_LoginPartialSimpleRule" />


PonyGoogleOnlyAuthentication require MVC controller service enabled, and is implemented as Razor class library, this is the mechanism that allows your to only add code in 3 places and have a fully functional Google Authentication. Reference link:

https://docs.microsoft.com/en-us/aspnet/core/razor-pages/ui-class?view=aspnetcore-5.0&tabs=visual-studio