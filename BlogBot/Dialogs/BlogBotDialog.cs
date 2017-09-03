namespace BlogBot.Dialogs
{
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    using BlogBot.Models;

    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Builder.FormFlow;

    // We won't inherit from IDialog here but use IDialog internally to create a dialog chain.
    public class BlogBotDialog
    {
        // This is a dialog chain. This gets triggered with user message as argument.
        public static readonly IDialog<string> dialog = Chain.PostToChain().Select(msg => msg.Text)
            // We will start with the Hello Dialog to greet user. Let's check whether user said "Hi"
            .Switch(
                new RegexCase<IDialog<string>>(
                    new Regex("^hi", RegexOptions.IgnoreCase),
                    (context, text) => new HelloDialog().ContinueWith(ContinueConversation)),
                new DefaultCase<string, IDialog<string>>(
                    (context, text) => FormDialog.FromForm(BlogSearch.BuildForm, FormOptions.PromptInStart)
                        .ContinueWith(ContinueConversation))).Unwrap().PostToUser();

        // This method fires off after either of the dialogs to conclude the conversation.
        private static async Task<IDialog<string>> ContinueConversation(IBotContext context, IAwaitable<object> item)
        {
            var message = await item;
            // use the message object to send an email to me.

            context.UserData.TryGetValue("userName", out string name);
            return Chain.Return($"Thank you for using the Blog Bot: {name}");
        }
    }
}