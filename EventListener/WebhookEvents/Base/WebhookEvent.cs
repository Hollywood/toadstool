using Toadstool.Utility;
using Octokit;
using System.Threading.Tasks;

namespace Toadstool.WebhookEvents
{
    public abstract class WebhookEvent
    {
        public string RepositoryName;
        public string ActionUser;
        public string Action;

        public virtual Task<Issue> CreateIssue()
        {
            var issue = new GitHubUtilities();
            return issue.CreateIssue(RepositoryName, $"Toadstool reports that {ActionUser} has {Action} {RepositoryName}!", $"{RepositoryName} has been {Action} by {ActionUser}!");
        }
    }
}