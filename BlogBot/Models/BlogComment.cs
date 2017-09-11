namespace BlogBot.Models
{
    using System;

    using Microsoft.Bot.Builder.FormFlow;

    public enum BlogAspectOptions
    {
        Article,

        Profile
    }

    [Serializable]
    public class BlogComment
    {
        public BlogAspectOptions BlogAspect;

        [Prompt("What message would you like to post?")]
        public string MessageForRahul;

        public static IForm<BlogComment> BuildForm()
        {
            return new FormBuilder<BlogComment>().Message("Hi, I am Blog Bot. We will populate entities now.").Build();
        }
    }
}