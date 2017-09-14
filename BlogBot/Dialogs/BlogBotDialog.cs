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
                    (context, text) => new HelloDialog().ContinueWith(ContinueHelloConversation)),
                new DefaultCase<string, IDialog<string>>(
                    (context, text) => (IDialog<string>)FormDialog.FromForm(BlogComment.BuildForm, FormOptions.PromptInStart)
                        .ContinueWith(ContinueBlogConversation))).Unwrap().PostToUser();

        // This method fires off after the Blog comment dialog to conclude the conversation.
        private static async Task<IDialog<string>> ContinueBlogConversation(IBotContext context, IAwaitable<BlogComment> item)
        {
            // This will contain a BlogComment object with entities populated.
            var blogComment = await item;

            return new ForkedConversationDialog().ContinueWith(
                async (c, r) =>
                      {
                          await c.PostAsync("Carrying out conversation based on user input!");
                          return Chain.Return($"You entered {await r}.End of forked conversation");
                      });
        }

        // This method fires off after the Hello dialog to conclude the conversation.
        private static async Task<IDialog<string>> ContinueHelloConversation(IBotContext context, IAwaitable<object> item)
        {
            var message = await item;
            context.UserData.TryGetValue("userName", out string name);
            return Chain.Return($"Thank you for using the Blog Bot: {name}");
        }
    }
}