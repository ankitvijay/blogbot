namespace BlogBot.Models
{
    using System;

    using Microsoft.Bot.Builder.FormFlow;

    public enum BlogAspectOptions
    {
        Article,

        Profile,

        Subscribe
    }

    [Serializable]
    public class BlogSearch
    {
        public BlogAspectOptions? BlogAspect;

        public string MessageForRahul;

        public static IForm<BlogSearch> BuildForm()
        {
            return new FormBuilder<BlogSearch>().Message("Hi, I am Blog Bot. We will populate entities now.").Build();
        }
    }
}