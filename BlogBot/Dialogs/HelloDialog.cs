namespace BlogBot.Dialogs
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Connector;

    [Serializable]
    public class HelloDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Hi, I am Blog Bot");
            await this.Respond(context);
            // Continue conversation using the following method.
            context.Wait(this.ProcessConversation);
        }

        private async Task ProcessConversation(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var userInput = await argument;

            // Try retrieving the name of the user from state.
            context.UserData.TryGetValue("userName", out string nameOfUser);

            // We don't want to process the first message that the user sent to the bot to initiate a conversation (the 'Hi' message). This flag will only be set when our bot requests the user to enter his name. Therefore, we will check the value of this flag and only then set the state with the name of the user.
            context.UserData.TryGetValue("nameRequired", out bool nameRequired);
            if (nameRequired)
            {
                nameOfUser = userInput.Text;

                // Save the name and set the flag.
                context.UserData.SetValue("userName", nameOfUser);
                context.UserData.SetValue("nameRequired", false);
            }

            //await Respond(context);
            context.Done(userInput);
        }

        private async Task Respond(IDialogContext context)
        {
            var userName = string.Empty;
            context.UserData.TryGetValue("userName", out userName);
            if (string.IsNullOrWhiteSpace(userName))
            {
                await context.PostAsync("What is your name?");
                context.UserData.SetValue("nameRequired", true);
            }
            else
            {
                await context.PostAsync($"Hi! {userName}, This conversation will conclude now.");
            }
        }
    }
}