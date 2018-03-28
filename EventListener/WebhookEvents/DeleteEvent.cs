using Octokit;
using System.Threading.Tasks;

namespace Toadstool.WebhookEvents
{
    public class DeleteEvent : WebhookEvent, IWebhookEvent
    {
        public DeleteEvent(string repositoryName, string actionUser, string action, string taggedUser)
        {
            RepositoryName = repositoryName;
            ActionUser = actionUser;
            Action = action;
            TaggedUser = taggedUser;
        }

        public async Task<Issue> CreateRepositoryIssue()
        {
            return await CreateRepositoryIssue(RepositoryName, $"{ActionUser} has {Action} {RepositoryName}!", $"@{TaggedUser}, {RepositoryName} has been {Action} by {ActionUser}!");
        }

        public Task<bool> EditRepository(string owner, string repository, string user)
        {
            throw new System.NotImplementedException();
        }
    }
}