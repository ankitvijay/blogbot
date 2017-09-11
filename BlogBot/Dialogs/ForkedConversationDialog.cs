namespace BlogBot.Dialogs
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.Bot.Builder.Dialogs;

    [Serializable]
    public class ForkedConversationDialog : IDialog<string>
    {
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync($"You can fork off conversation from within a dialog.");
            context.Done("ok");
        }
    }
}