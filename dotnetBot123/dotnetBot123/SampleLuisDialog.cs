using FootballData;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;


namespace InBytesBot
{
    [LuisModel("", "")]
    [Serializable]
    public class SampleLuisDialog : LuisDialog<object>
    {
        [LuisIntent("Hello")]
        public async Task greet(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Hello You have reached LUIS For the Assistance, I can help you finding in total teams, Best team && worst team");
            context.Wait(MessageReceived);
        }
        [LuisIntent("Help")]
        public async Task Help(IDialogContext context, LuisResult result)
        {
            var reply = context.MakeMessage();
            reply.Attachments = new List<Attachment>();
            List<CardImage> images = new List<CardImage>();
            CardImage ci = new CardImage("http://intelligentlabs.co.uk/images/IntelligentLabs-White-Small.png");
            images.Add(ci);
            CardAction ca = new CardAction()
            {
                Title = "Visit Support",
                Type = "openUrl",
                Value = "https://www.luis.ai/home/help"
            };
            ThumbnailCard tc = new ThumbnailCard()
            {
                Title = "Need help?",
                Subtitle = "Go to our main site support.",
                Images = images,
                Tap = ca
            };
            reply.Attachments.Add(tc.ToAttachment());
            await context.PostAsync(reply);
            context.Wait(MessageReceived);
        }

        public static Championships champs = new Championships();
        [LuisIntent("TeamCount")]
        public async Task GetTeamCount(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"There are { champs.GetTeamCount() } teams.");
            context.Wait(MessageReceived);
        }

        [LuisIntent("")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("I have no idea what you are talking about.");
            context.Wait(MessageReceived);
        }

        [LuisIntent("TopTeam")]
        public async Task BestTeam(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"{ champs.GetHighestRatedTeam()} is the best team in the championships.");
            context.Wait(MessageReceived);
        }

        [LuisIntent("BottomTeam")]
        public async Task BottomTeam(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"{ champs.GetLowestRatedTeam()} is the worst team in the championships.");
            context.Wait(MessageReceived);
        }
        [LuisIntent("Thank You")]
        public async Task thankyou(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Thank you for visiting us, Hope we found your solutioins, See you soon ");
            context.Wait(MessageReceived);
        }
        [LuisIntent("Gif")]
        private async Task GifDialog(IDialogContext context, LuisResult result)
        {
                 var outboundMessage = context.MakeMessage();
                // get a random gif from giphy.com and send it as a card
                var client = new HttpClient() { BaseAddress = new Uri("http://api.giphy.com") };
                var results = client.GetStringAsync("/v1/gifs/trending?api_key=dc6zaTOxFJmzC").Result;
                var data = ((dynamic)JObject.Parse(results)).data;
                var gif = data[(int)Math.Floor(new Random().NextDouble() * data.Count)];
                var gifUrl = gif.images.fixed_height.url.Value;
                var slug = gif.slug.Value;

                outboundMessage.Attachments = new List<Attachment>();
                outboundMessage.Attachments.Add(new Attachment()
                {
                    ContentUrl = gifUrl,
                    ContentType = "image/gif",
                    Name = slug + ".gif"
                });
            await context.PostAsync(outboundMessage);
            context.Wait(MessageReceived);
        }
        [LuisIntent("RemoveTeam")]
        private async Task Removeteam(IDialogContext context, LuisResult result)
        {
            string teamname = "";
            EntityRecommendation rec;
            if (result.TryFindEntity("TeamName", out rec))
            {
                teamname = rec.Entity;
                champs.RemoveTeam(teamname);
                await context.PostAsync($"we have removed{teamname}");
            }
            else
            {
                await context.PostAsync("There is no team with that name");
            }

            context.Wait(MessageReceived);

        }

        }

    }
