using System;

namespace Sitecore.Support.Shell.Framework.Commands.System
{
    using Configuration;
    using Sitecore.Shell.Framework.Commands;
    using Sitecore.Shell.Framework;

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
            Items.Preview();
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
    }
}