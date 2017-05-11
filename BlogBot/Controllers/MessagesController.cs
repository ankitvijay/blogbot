using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace BlogBot
{
    using System;
    using System.Workflow.ComponentModel.Design;

    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity.Type == ActivityTypes.Message)
            {
                //// The client is responsible for getting\setting state of the bot.
                //StateClient stateClient = activity.GetStateClient();
                //// Retrieve User Data based on ChannelId and UserId (unique combination)
                //BotData userData = await stateClient.BotState.GetUserDataAsync(activity.ChannelId, activity.From.Id);
                //// Set a property in the retrieved state.
                //userData.SetProperty<string>("sampleProperty", "sampleValue");
                //// Request stateClient to save data.
                //await stateClient.BotState.SetUserDataAsync(activity.ChannelId, activity.From.Id, userData);

                // **Set State Data
                // The client is responsible for getting\setting state of the bot.
                StateClient stateClient = activity.GetStateClient();
                // We will store an instance of this class in the state store.
                CustomUserData customUserData = new CustomUserData("message");
                // The eTag in BotData specifies that we want to update the latest instance of CustomUserData in the state store.
                BotData botData = new BotData(eTag: "*");
                // Set the property as usual.
                botData.SetProperty("UserData", customUserData);
                // Request the StateClient instance to save this state.
                BotData response1 = await stateClient.BotState.SetUserDataAsync(activity.ChannelId, activity.From.Id, botData);

                // **Get State Data
                BotData fetchedStateData = await stateClient.BotState.GetUserDataAsync(activity.ChannelId, activity.From.Id);
                CustomUserData fetchedCustomUserData = fetchedStateData.GetProperty<CustomUserData>("UserData");
          
                ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                int length = (activity.Text ?? string.Empty).Length;

                Activity replyActivity = activity.CreateReply($"Length of message is {length}");
                await connector.Conversations.ReplyToActivityAsync(replyActivity);
            }
            else
            {
                HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }

    public class CustomUserData
    {
        public string Message { get; }

        public CustomUserData(string message)
        {
            this.Message = message;
        }
    }
}