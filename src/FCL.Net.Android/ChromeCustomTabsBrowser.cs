using Android.Content;
using Android.Graphics;
using Android.Support.CustomTabs;
using Uri = Android.Net.Uri;

namespace FCL.Net.Android
{
    public class ChromeCustomTabsBrowser : AndroidBrowserBase
    {
        protected override void OpenBrowser(Uri uri)
        {
            var manager = new CustomTabsActivityManager(_context);
            var builder = new CustomTabsIntent.Builder(manager.Session)
               .SetToolbarColor(Color.Argb(255, 52, 152, 219))
               .SetShowTitle(true)
               .EnableUrlBarHiding();

            var customTabsIntent = builder.Build();
            customTabsIntent.Intent.AddFlags(ActivityFlags.NoHistory);

            customTabsIntent.LaunchUrl(_context, uri);
        }
    }
}