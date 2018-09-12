using System;
using System.Collections.Generic;

namespace Sitecore.Support.Shell.Framework.Commands.System
{
    using Sitecore.Configuration;
    using Sitecore.Data.Items;
    using Sitecore.Globalization;
    using Sitecore.Links;
    using Sitecore.Publishing;
    using Sitecore.Shell.DeviceSimulation;
    using Sitecore.Shell.Framework.Commands;
    using Sitecore.Sites;
    using Sitecore.Text;
    using Sitecore.Web;
    using Sitecore.Web.UI.Sheer;

    /// <summary>
    /// Represents the Preview command.
    /// </summary>
    [Serializable]
    public class Preview : Command
    {
        /// <summary>
        /// Executes the command in the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        public override void Execute(CommandContext context)
        {
            this.CustomPreview();
        }

        /// <summary>
        /// Queries the state of the command.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>The state of the command.</returns>
        public override CommandState QueryState(CommandContext context)
        {
            if (!Settings.Preview.Enabled)
            {
                return CommandState.Hidden;
            }

            return CommandState.Enabled;
        }

        public void CustomPreview()
        {

            UrlString preview = Preview.GetPreview();
            SiteContext site = Factory.GetSite(Settings.Preview.DefaultSite);
            if (site == null || preview == null)
            {
                SheerResponse.Alert(Translate.Text("Site \"{0}\" not found", new object[]
                {
                    Settings.Preview.DefaultSite
                }), new string[0]);
                return;
            }

            preview["sc_site"] = site.Name;
            preview["sc_mode"] = "preview";
            SheerResponse.Eval("window.open('" + preview + "', '_blank')");

        }

        private static UrlString GetPreview()
        {
            return Preview.GetPreview(null);
        }

        private static UrlString GetPreview(Item item)
        {
            SheerResponse.CheckModified(false);
            SiteContext siteContext = (item == null)
              ? Factory.GetSite(Settings.Preview.DefaultSite)
              : LinkManager.GetPreviewSiteContext(item);
            if (siteContext == null)
            {
                return null;
            }

            string cookieKey = siteContext.GetCookieKey("sc_date");
            WebUtil.SetCookieValue(cookieKey, string.Empty);
            PreviewManager.StoreShellUser(Settings.Preview.AsAnonymous);
            DeviceSimulationUtil.DeactivateSimulators();
            return new UrlString("/");
        }
    }
}