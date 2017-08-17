using Octokit;
using System.Threading.Tasks;

namespace Toadstool.WebhookEvents
{
    public class DeleteEvent : WebhookEvent, IWebhookEvent
    {
        public DeleteEvent(string repositoryName, string actionUser, string action)
        {
            RepositoryName = repositoryName;
            ActionUser = actionUser;
            Action = action;
        }

        public async Task<Issue> CreateRepositoryIssue()
        {
            return await CreateIssue();
        }

    }
}