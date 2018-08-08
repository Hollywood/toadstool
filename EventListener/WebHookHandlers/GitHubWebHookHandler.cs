using Microsoft.AspNet.WebHooks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Configuration;
using System.Threading.Tasks;
using Toadstool.Utility;
using Toadstool.WebhookEvents;

namespace Toadstool.Handlers
{
    public class GitHubWebHookHandler : WebHookHandler
    {
        public override Task ExecuteAsync(string receiver, WebHookHandlerContext context)
        {
            if(receiver.ToLower() == Properties.Settings.Default.WebhookRecieverClient.ToLower())
            {
                JObject content = context.GetDataOrDefault<JObject>();
                var serializedContent = JsonConvert.SerializeObject(content);
                var util = new JsonUtilities();
                var action = util.GetFirstInstance<string>("action", serializedContent);
                var actionUser = content["sender"]["login"].Value<string>(); //Get the user performing the action
                var owner = content["repository"]["owner"]["login"].Value<string>();
                var repositoryName = util.GetFirstInstance<string>("name", serializedContent);
                var taggedUser = ConfigurationManager.AppSettings["Assignees"];
                IWebhookEvent webhookEvent = null;

                switch(action.ToLower())
                {
                    case "deleted":
                        webhookEvent = new DeleteEvent(repositoryName, actionUser, action, taggedUser);
                        break;
                    case "created":
                        webhookEvent = new CreatedEvent(repositoryName, actionUser, action, taggedUser);
                        break;
                    case "added":
                        webhookEvent = new RemovedEvent(repositoryName, actionUser, action, taggedUser);
                        webhookEvent.EditRepository(repositoryName, owner, actionUser);
                        break;
                    default:
                        break;
                }

                if(webhookEvent != null)
                    return webhookEvent.CreateRepositoryIssue();
            }

            return Task.FromResult(true);
        }

    }
}