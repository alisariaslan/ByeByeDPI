using ByeByeDPI.Utils;

namespace ByeByeDPI.Html
{
    internal class AboutHtml
    {
        public static string GetTitle()
        {
            return "About";
        }
        public static string GetHtml(float fontSize = 12)
        {

            string aboutHtml = $@"
    <!doctype html>
    <html>
    <head>
      <meta charset='utf-8'/>
      <style>
            body {{ font-size: {fontSize}px; font-family: Segoe UI, Tahoma, Arial; padding: 16px; }}
            h2 {{ margin-top:0; }}
            a {{ text-decoration:none; }}
            a:hover {{ text-decoration:underline; }}
            .meta {{ margin-top:12px; }}
            .footer {{ margin-top:20px; font-size:90%;  }}
      </style>
    </head>
    <body>
      <h1>ByeByeDPI GUI</h1>
  <div class='meta'>Version: {AppVersionHelper.GetAppVersion()}</div>
      <p>A lightweight graphical interface for controlling GoodbyeDPI.</p>
      <p>Developed by <strong>Ali SARIASLAN</strong></p>
      <p>Contact: <a href='mailto:dev@alisariaslan.com'>dev@alisariaslan.com</a></p>
      <div class='footer'>
        GitHub: <a href='https://github.com/alisariaslan/ByeByeDPI' target='_blank'>https://github.com/alisariaslan/ByeByeDPI</a>
      </div>
    </body>
    </html>";

            return aboutHtml;
        }
    }
}
