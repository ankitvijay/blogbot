using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogBot.Dialogs
{
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    using BlogBot.Models;

    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Builder.FormFlow;

    public class BlogBotDialog
    {
        public static readonly IDialog<string> dialog = Chain.PostToChain().Select(msg => msg.Text)
            .Switch(
            new RegexCase<IDialog<string>>(new Regex("^hi", RegexOptions.IgnoreCase),
                (context, text) =>
                    {
                        return Chain.ContinueWith(new HelloDialog(), ContinueConversation);
                        } ),
            new DefaultCase<string, IDialog<string>>(
                (context, text) =>
                    {
                        return Chain.ContinueWith(
                            FormDialog.FromForm<BlogSearch>(BlogSearch.BuildForm(), FormOptions.PromptInStart),
                            ContinueConversation);
                    })
            );

        private static Task<IDialog<object>> ContinueConversation(IBotContext context, IAwaitable<object> item)
        {
            throw new NotImplementedException();
        }
    }
}