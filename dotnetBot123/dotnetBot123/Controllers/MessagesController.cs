using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Dialogs;
using InBytesBot;




namespace dotnetBot123
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            
            if (activity.Type == ActivityTypes.Message)
            {
                if (activity.Type == ActivityTypes.Message)
                {
                    await Conversation.SendAsync(activity, () => new SampleLuisDialog());
                }
               
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
            return null;
        }
    }
}

   