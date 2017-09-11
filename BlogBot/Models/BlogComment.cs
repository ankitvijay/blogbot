namespace BlogBot.Models
{
    using System;

    using Microsoft.Bot.Builder.FormFlow;

    public enum BlogAspectOptions
    {
        //[Terms("a", "A")]
        Article,

        //[Terms("p", "P")]
        Profile,

        //[Terms("s", "S")]
        Subscribe
    }

    [Serializable]
    public class BlogComment
    {
        //[Prompt("What do you like to comment on?")]
        public BlogAspectOptions BlogAspect;

        [Prompt("What message would you like to post?")]
        public string MessageForRahul;

        public static IForm<BlogComment> BuildForm()
        {
            return new FormBuilder<BlogComment>().Message("Hi, I am Blog Bot. We will populate entities now.").Build();
        }
    }
}