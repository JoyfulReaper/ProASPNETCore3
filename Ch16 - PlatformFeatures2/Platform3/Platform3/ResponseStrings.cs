using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Platform3
{
    public static class Responses
    {
        public static string DefaultResponse = @"
            <!DOCTYPE html>
            <html lang=""en"">
            <head>
                <link href = ""https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/css/bootstrap.min.css"" rel=""stylesheet"" integrity=""sha384-1BmE4kWBq78iYhFldvKuhfTAU6auU8tT94WrHftjDbrCEXSU1oBoqyl2QvZ6jIW3"" crossorigin=""anonymous"">
                <meta charset = ""utf-8"" />
                <title>Error {0}</title>
            </head>
            <body class=""text-center"">
                <h3 class=""p-2"">{0}: Something went wrong...</h3>
                <h6>You can go back to the<a href= ""/"" > homepage </ a > and try again</h6>
            </body>
            </html>";
    }
}
