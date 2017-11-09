namespace BlogBot.Dialogs
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using BlogBot.Models;

    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Builder.FormFlow;
    using Microsoft.Bot.Builder.Luis;
    using Microsoft.Bot.Builder.Luis.Models;

    [Serializable]
    [LuisModel("", "")]
    public class LUISTestDialog : LuisDialog<BlogComment>
    {
        [LuisIntent("Blog Aspects")]
        public async Task CanCommentOn(IDialogContext context, LuisResult result)
        {
            foreach (var entity in result.Entities.Where(e => e.Type == "Blog Aspect Entity"))
            {
                var name = entity.Entity.ToLowerInvariant();
                if (name == "blog" || name == "profile")
                {
                    await context.PostAsync($"Yes you can comment on {name}. Launching the form now...");
                    var blogCommentForm = FormDialog.FromForm(BlogComment.BuildForm, FormOptions.PromptInStart);
                    context.Call(blogCommentForm, Continue);

                    async Task Continue(IDialogContext dialogContext, IAwaitable<BlogComment> dialogResult)
                    {
                        await dialogContext.PostAsync($"Thank you for submitting your request.");
                        dialogContext.Wait(this.MessageReceived);
                    }

                    return;
                }
            }

            await context.PostAsync("Not an available option");
            context.Wait(this.MessageReceived);
        }

        [LuisIntent("Hello Intent")]
        public async Task HelloIntent(IDialogContext context, LuisResult result)
        {
            context.Call(new HelloDialog(), Callback);

            async Task Callback(IDialogContext dialogContext, IAwaitable<object> dialogResult)
            {
                await dialogContext.PostAsync("Hello dialog concludes.");
                dialogContext.Wait(this.MessageReceived);
            }
        }

        [LuisIntent("")]
        public async Task NoIntentFound(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("LUIS could not find a matching intent.");
            context.Wait(this.MessageReceived);
        }
    }
}