using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wms.infrastructure.Middlewares
{
    public class RequestResponseLoggingMiddleware
    {
        private const int MaxLenghtAllowLog = 2000;
        private readonly RequestDelegate _next;

        public RequestResponseLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                var URI = Microsoft.AspNetCore.Http.Extensions.UriHelper.GetDisplayUrl(context.Request);
                if (!URI.Contains("/api"))
                {
                    await _next(context);
                    return;
                }

                StringValues RequestID = string.Empty;
                StringValues UserName = string.Empty;
                StringValues Origin = string.Empty;
                StringValues SessionID = string.Empty;
                StringValues token = string.Empty;
                DateTime reqDate = DateTime.Now;
                string traceID = System.Diagnostics.Activity.Current?.TraceId.ToString();

                if (context.Request.Headers.TryGetValue("ContextID", out RequestID))
                {
                    RequestID = RequestID.ToString();
                }
                else
                {
                    RequestID = Guid.NewGuid().ToString().Replace("-", "");
                    context.Items.Add("ContextID", RequestID);
                }

                if (string.IsNullOrEmpty(traceID))
                {
                    traceID = RequestID;
                }

                if (context.Request.Headers.TryGetValue("Authorization", out token))
                {
                    token = token.ToString().Replace("Bearer ", "");
                }

                if (context.Request.Headers.TryGetValue("UserID", out UserName))
                {
                    UserName = UserName.ToString();
                }
                else if (!string.IsNullOrEmpty(token))
                {
                    //UserName = GetUsernameFromToken(token);
                }

                if (context.Request.Headers.TryGetValue("Origin", out Origin))
                {
                    Origin = Origin.ToString();
                }

                if (context.Request.Headers.TryGetValue("SessionID", out SessionID))
                {
                    SessionID = SessionID.ToString();
                }

                await FormatRequest(context.Request, URI, RequestID, UserName, Origin, SessionID, traceID);//run asyn
                string bodyText = string.Empty;
                int codestatus = 200;
                int contentLenght = 0;
                var originalBodyStream = context.Response.Body;

                using (var responseBody = new MemoryStream())
                {
                    context.Response.Body = responseBody;
                    await _next(context);
                    context.Response.Body.Seek(0, SeekOrigin.Begin);
                    //...and copy it into a string
                    bodyText = await new StreamReader(context.Response.Body).ReadToEndAsync();
                    context.Response.Body.Seek(0, SeekOrigin.Begin);
                    codestatus = context.Response.StatusCode;
                    await responseBody.CopyToAsync(originalBodyStream);
                }
                _ = FormatResponse(bodyText, codestatus, contentLenght, RequestID, URI, UserName, Origin, SessionID, reqDate, traceID);//run asyn
            }
            catch (Exception ex)
            {
                Console.Write($"Log middleware reading error {ex.Message} stack:{ex.StackTrace}");
            }

        }

        private async Task FormatRequest(HttpRequest request, string uri, string requestId, string UserName, string origin, string sessionId, string traceID)
        {
            if (string.IsNullOrEmpty(uri) || uri.ToLower().Contains("health"))
            {
                return;
            }

            request.EnableBuffering();

            StringBuilder sb = new StringBuilder("{ContextID} {ClientID} {TraceID} {Method} {Uri} {ContentLength}");
            List<object> propertyValues = new List<object>() { requestId, "", traceID, request.Method, uri, request.ContentLength };


            if (request.ContentLength > 0 && request.ContentLength <= 131072)
            {
                // We now need to read the request stream.First, we create a new byte[] with the same length as the request stream...
                var buffer = new byte[Convert.ToInt32(request.ContentLength)];

                //...Then we copy the entire request stream into the new buffer.
                await request.Body.ReadAsync(buffer, 0, buffer.Length);

                //We convert the byte[] into a string using UTF8 encoding...
                var bodyAsText = Encoding.UTF8.GetString(buffer);

                //..and finally, assign the read body back to the request body, which is allowed because of EnableRewind()
                request.Body.Seek(0, SeekOrigin.Begin);

                sb.Append(" {Body}"); propertyValues.Add(bodyAsText);
            }
            if (!string.IsNullOrEmpty(UserName))
            {
                sb.Append(" {UserName}"); propertyValues.Add(UserName);
            }
            if (!string.IsNullOrEmpty(origin))
            {
                sb.Append(" {Origin}"); propertyValues.Add(origin);
            }
            if (!string.IsNullOrEmpty(sessionId))
            {
                sb.Append(" {SessionID}"); propertyValues.Add(sessionId);
            }
        }

        private async Task FormatResponse(string bodyText, int statusCode, int contentLenght, string requestId, string uri, string UserName, string origin, string sessionId, DateTime reqDate, string traceID)
        {
            try
            {
                await Task.Run(() =>
                {
                    //no log api check health
                    if (string.IsNullOrEmpty(uri) || uri.ToLower().Contains("health"))
                    {
                        return;
                    }
                    contentLenght = bodyText.Length;
                    StringBuilder sb = new StringBuilder("{ContextID} {ClientID} {TraceID} {Uri} {Status} {ReasonPhrase} {ContentLength}");
                    List<object> propertyValues = new List<object>() { requestId, "", traceID, uri, statusCode, "", contentLenght };
                    if (bodyText.Length > 0 && bodyText.Length <= 131072)
                    {
                        sb.Append(" {Response}"); propertyValues.Add(bodyText);
                    }
                    if (!string.IsNullOrEmpty(UserName))
                    {
                        sb.Append(" {UserName}"); propertyValues.Add(UserName);
                    }
                    if (!string.IsNullOrEmpty(origin))
                    {
                        sb.Append(" {Origin}"); propertyValues.Add(origin);
                    }
                    if (!string.IsNullOrEmpty(sessionId))
                    {
                        sb.Append(" {SessionID}"); propertyValues.Add(sessionId);
                    }
                    sb.Append(" {ResponseTime}"); propertyValues.Add(DateTime.Now.Subtract(reqDate).TotalMilliseconds);
                    switch (statusCode)
                    {
                        case 500:
                            Console.WriteLine(sb.ToString());
                            break;
                        case 204:
                        case 406:
                        case 400:
                            Console.WriteLine(sb.ToString());
                            break;
                        default:
                            Console.WriteLine(sb.ToString());
                            break;
                    }
                });

            }
            catch (Exception ex)
            {
               Console.WriteLine(ex.Message.ToString());
            }

        }
    }
}
