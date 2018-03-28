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
        public string TaggedUser;

        public virtual Task<Issue> CreateRepositoryIssue(string repositoryName, string title, string body )
        {
            var utility = new GitHubUtilities();
            return utility.CreateIssue(repositoryName, title, body);
        }

        public virtual Task<bool> RemoveOutsideCollaborator(string owner, string repositoryName, string user )
        {
            var utility = new GitHubUtilities();
            return utility.RemoveCollaborator(owner, repositoryName, user);
        }
    }
}