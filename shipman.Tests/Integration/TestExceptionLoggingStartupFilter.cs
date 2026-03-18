namespace shipman.Tests.Integration;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Diagnostics;

public class TestExceptionLoggingStartupFilter : IStartupFilter
{
    public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
    {
        return app =>
        {
            app.Use(async (context, nextMiddleware) =>
            {
                try
                {
                    await nextMiddleware();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("SERVER EXCEPTION:");
                    Debug.WriteLine(ex.ToString());
                    throw;
                }
            });

            next(app);
        };
    }
}
