namespace ByeByeDPI.Html
{
    internal class UnderDevelopmentHtml
    {
        public static string GetTitle()
        {
            return "Under Development";
        }
        public static string GetHtml(string featureName, float fontsize = 12)
        {
            string html = $@"
<!doctype html>
<html>
<head>
  <meta charset='utf-8'/>
  <title>{featureName}</title>
  <style>
    body {{ font-size: {fontsize}px; font-family: Segoe UI, Tahoma, Arial; padding: 16px;  }}
    h2 {{ margin-top:0; font-size: 20px; }}
    p {{ margin-top:12px;  }}
    a {{  text-decoration:none; }}
    a:hover {{ text-decoration:underline; }}
    .footer {{ margin-top:20px; font-size:90%;  }}
  </style>
</head>
<body>
  <h1>{featureName}</h1>
  <p>This feature is currently under development and will be available in a future update.</p>
  <div class='footer'>
    Contact: <a href='mailto:dev@alisariaslan.com'>dev@alisariaslan.com</a><br/><br/>
    GitHub: <a href='https://github.com/alisariaslan/ByeByeDPI' target='_blank'>https://github.com/alisariaslan/ByeByeDPI</a>
  </div>
</body>
</html>";
            return html;
        }
    }
}
