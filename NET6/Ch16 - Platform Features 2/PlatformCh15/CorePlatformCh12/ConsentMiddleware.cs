using Microsoft.AspNetCore.Http.Features;

namespace PlatformCh16
{
    public class ConsentMiddleware
    {
        private readonly RequestDelegate _next;

        public ConsentMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if(context.Request.Path == "/consent")
            {
                ITrackingConsentFeature consentFeature = 
                    context.Features.Get<ITrackingConsentFeature>();
                if(!consentFeature.HasConsent)
                {
                    consentFeature.GrantConsent();
                } else
                {
                    consentFeature.WithdrawConsent();
                }

                await context.Response
                    .WriteAsync(consentFeature.HasConsent ? "Consent Granted\n" : "Consent Withdrawn\n");
            }
            await _next(context);
        }
    }
}
