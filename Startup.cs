using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;

namespace http
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {  

                 endpoints.MapPost("/body-data", async context =>
                {
                    //  context.Response.Headers.Add("Content-Type", "application/json; charset=utf-8");
                    context.Response.Headers.Add("Content-Type", "text/html; charset=utf-8");

                    using (StreamReader stream = new StreamReader(context.Request.Body))
                    {
                        string body = await stream.ReadToEndAsync();
                        // ObjBody data = JsonConvert.DeserializeObject<ObjBody>(body);
                        dynamic data = JsonConvert.DeserializeObject<dynamic>(body);
                        await context.Response.WriteAsync($"<h1>Parametros no http</h1>");
                        await context.Response.WriteAsync($"Parametro joao = {data.joao}<br>");
                        await context.Response.WriteAsync($"Parametro wife = {data.wife}<br>");
                    }
                });
                 
                  endpoints.MapPost("/form-data", async context =>
                {
                    context.Response.Headers.Add("Content-Type", "text/html; charset=utf-8");
                    // var dict = context.Request.Form.ToDictionary(x => x.Key, x => x.Value.ToString());
                    string teste = context.Request.Form["joao"].ToString();
                    string teste2 = context.Request.Form["wife"].ToString();
                    await context.Response.WriteAsync($"<h1>Parametros no http</h1>");
                    await context.Response.WriteAsync($"Parametro joao = {teste}<br>");
                    await context.Response.WriteAsync($"Parametro parceiro = {teste2}<br>");
                });

                //https://localhost:5001/query-string?danilo=teste&parceiro=natalia
                endpoints.MapGet("/query-string", async context =>
                {
                    context.Response.Headers.Add("Content-Type", "text/html; charset=utf-8");
                    string teste = context.Request.Query["joao"].ToString();
                    string teste2 = context.Request.Query["wife"].ToString();
                    await context.Response.WriteAsync($"<h1>Parametros no http</h1>");
                    await context.Response.WriteAsync($"Parametro danilo = {teste}<br>");
                    await context.Response.WriteAsync($"Parametro wife = {teste2}<br>");
                });

                endpoints.MapGet("/", async context =>
                {

                    /*Teste de criação de cookies*/
                    // CookieOptions cookieOptions = new CookieOptions();
                    // cookieOptions.Expires = DateTime.Now.AddDays(2);
                    // context.Response.Cookies.Append("Nome", "João Paulo", cookieOptions);
                    // context.Response.Cookies.Append("Sistema", "Windows", cookieOptions);
                    // context.Response.Cookies.Append("Estado", "São Paulo ", cookieOptions);

                    context.Response.Headers.Add("Content-Type", "text/html; charset=utf-8");
                    /********Mudando o status**********/
                    //context.Response.StatusCode = 200;

                    await context.Response.WriteAsync($"<h1>Valores do cookie localhost</h1>");
                    string teste = context.Request.Headers["Cookie"];
                    string[] chaveValor = teste.Split(',');
                    await context.Response.WriteAsync($"<ul>");
                    foreach (var item in chaveValor)
                    {
                        string[] cv = item.Split('=');
                        // if(cv[0].Trim().ToLower() == "_manobra_session")
                        // {
                        await context.Response.WriteAsync($"<li>");

                        await context.Response.WriteAsync($"<b>Chave:</b> {cv[0]}<br>");
                        await context.Response.WriteAsync($"<b>Valor:</b> {cv[1]}<br>");

                        await context.Response.WriteAsync($"</li>");
                        // }
                    }
                    await context.Response.WriteAsync($"</ul>");
                });

                 endpoints.MapGet("/pdf", async context =>
                {
                    context.Response.Headers.Add("Content-Type", "application/pdf; charset=utf-8");

                    PdfDocument document = new PdfDocument();
                    document.Info.Title = "Created with PDFsharp";
                    PdfPage page = document.AddPage();
                    XGraphics gfx = XGraphics.FromPdfPage(page);
                    XFont font = new XFont("Verdana", 20, XFontStyle.BoldItalic);
                    gfx.DrawString("Hello, World!", font, XBrushes.Black, new XRect(0, 0, page.Width, page.Height), XStringFormats.Center);
                    const string filename = "/tmp/HelloWorld.pdf";
                    document.Save(filename);
                    string doc = File.ReadAllText(filename);

                    await context.Response.WriteAsync(doc);
                });
                
                 endpoints.MapGet("/arquivo.csv", async context =>
                {
                    context.Response.Headers.Add("Content-Type", "text/csv; charset=utf-8");

                    await context.Response.WriteAsync($"Chave; Valor\n");
                    string teste = context.Request.Headers["Cookie"];
                    string[] chaveValor = teste.Split(',');
                    foreach (var item in chaveValor)
                    {
                        string[] cv = item.Split('=');
                        await context.Response.WriteAsync($"{cv[0]};{cv[1]}\n");
                    }

                });

               /* endpoints.MapGet("/", async context =>
                {
                    #region Teste Cookies
                     /*Teste de criação de cookies*/
                    // CookieOptions cookieOptions = new CookieOptions();
                    // cookieOptions.Expires = DateTime.Now.AddDays(2);
                    // context.Response.Cookies.Append("Nome", "João Paulo", cookieOptions);
                    // context.Response.Cookies.Append("Sistema", "Windows", cookieOptions);
                    // context.Response.Cookies.Append("Estado", "São Paulo ", cookieOptions);
                    // #endregion

                    #region html
                    
                    // context.Response.Headers.Add("Content-Type", "text/html; charset=utf-8");
                    

                    // string teste = context.Request.Headers["User-Agent"];
                    // string teste = context.Request.Headers["Cookie"];
                    // await context.Response.WriteAsync($"<h1>Chave; Valor</h1>");
                    // string[] chaveValor = teste.Split(',');
                    // await context.Response.WriteAsync($"<ul>");
                    // foreach (var item in chaveValor)
                    // {
                    //     await context.Response.WriteAsync($"<li>");
                    //     string[] cv = item.Split('=');
                    //     await context.Response.WriteAsync($"<b>Chave:</b> {cv[0]}</br>");
                    //     await context.Response.WriteAsync($"<b>Valor:</b> {cv[1]}</br>");
                    // }
                    //  await context.Response.WriteAsync($"</ul>");

                    #endregion
                    #region csv
                    // context.Response.Headers.Add("Content-Type", "text/csv; charset=utf-8");

                    // await context.Response.WriteAsync($"Chave; Valor\n");
                    // string teste = context.Request.Headers["Cookie"];
                    // string[] chaveValor = teste.Split(',');
                    // foreach (var item in chaveValor)
                    // {
                    //     string[] cv = item.Split('=');
                    //     await context.Response.WriteAsync($"{cv[0]};{cv[1]}\n");
                    // }

                    #endregion
                   });
            //});
        }
    }
}
