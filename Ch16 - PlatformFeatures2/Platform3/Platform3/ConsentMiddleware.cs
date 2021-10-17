using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Platform3
{
    public class ConsentMiddleware
    {
        private readonly RequestDelegate _nextDelegate;

        public ConsentMiddleware(RequestDelegate nextDelegate)
        {
            _nextDelegate = nextDelegate;
        }

        public async Task Invoke(HttpContext context)
        {
            if(context.Request.Path == "/consent")
            {
                ITrackingConsentFeature consentFeature
                    = context.Features.Get<ITrackingConsentFeature>();
                if(!consentFeature.HasConsent)
                {
                    consentFeature.GrantConsent();
                } else
                {
                    consentFeature.WithdrawConsent();
                }
                await context.Response.WriteAsync(consentFeature.HasConsent ? "Consent Granted \n" : "Consent Withdrawn\n");
            }
            await _nextDelegate(context);
        }
    }
}
