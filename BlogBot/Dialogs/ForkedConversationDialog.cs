namespace BlogBot.Dialogs
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Connector;

    [Serializable]
    public class ForkedConversationDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync($"You can fork off conversation from within a dialog. Enter any message to continue.");
            context.Wait(this.Resume);
        }

        private async Task Resume(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var userInput = await result;
            context.Done(userInput.Text);
        }
    }
}