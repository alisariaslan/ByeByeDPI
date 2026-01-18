using ByeByeDPI.Utils;

namespace ByeByeDPI.Html
{
    internal class HelpHTML
    {
        public static string GetTitle()
        {
            return "Help";
        }

        public static string GetHtml(float fontSize = 12)
        {
            string helpHtml = $@"
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
        .footer {{ margin-top:20px; font-size:90%; }}
  </style>
</head>
<body>
  <h1>ByeByeDPI GUI</h1>
  <div class='meta'>Version: {AppVersionHelper.GetAppVersion()}</div>
  <p>ByeByeDPI GUI is a lightweight graphical interface for controlling GoodbyeDPI.</p>

  <h2>How to Use</h2>
  <ol>
    <li>First, if your country and ISP are listed in presets, try applying a preset via: <strong>Appbar → App → Presets</strong>.</li>
    <li>Next, click <strong>Install</strong> to start testing parameters. Each parameter will be checked against specific domains to verify connectivity. You can view parameters via <strong>Appbar → Settings → Open Parameters</strong>.</li>
    <li>
        After testing a parameter:
        <ul>
            <li>If all desired domains are reachable, click <strong>Apply</strong> to make this parameter permanent.</li>
            <li>If you want to skip, click <strong>Next</strong> to test the next parameter.</li>
            <li>To start over, click <strong>Reset</strong>.</li>
        </ul>
    </li>
    <li>If no parameter works, try flushing or resetting your DNS using: <strong>Appbar → App → DNS → ...</strong>.
        <ul>
            <li>If you reset DNS, Windows will automatically use the DNS that was applied.</li>
            <li>You can flush DNS manually if needed.</li>
        </ul>
    </li>
  </ol>

  <p>Contact: <a href='mailto:dev@alisariaslan.com'>dev@alisariaslan.com</a></p>
  <div class='footer'>
    GitHub: <a href='https://github.com/alisariaslan/ByeByeDPI' target='_blank'>https://github.com/alisariaslan/ByeByeDPI</a>
  </div>
</body>
</html>";

            return helpHtml;
        }
    }
}
