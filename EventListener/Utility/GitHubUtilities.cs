using Octokit;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace Toadstool.Utility
{
    public class GitHubUtilities
    {
        private GitHubClient client { get; set; }

        public GitHubUtilities()
        {
            var tokenAuth = new Credentials(ConfigurationManager.AppSettings["GitHubAccessToken"]);
            var connection = new Connection(new ProductHeaderValue("EventListener"));
            client = new GitHubClient(connection);
            client.Credentials = tokenAuth;
        }

        /// <summary>
        /// Create an issue in a specified repository based on a webhook request
        /// </summary>
        /// <param name="repositoryName"></param>
        /// <param name="title"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public async Task<Issue> CreateIssue(string repositoryName, string title, string body)
        {
            var issueDraft = new NewIssue(title);
            issueDraft.Body = body;

            //Get the users that the issue will tag/
            var issueAssignees = ConfigurationManager.AppSettings["Assignees"].Split(',').ToList();
            issueAssignees.ForEach(user =>
            {
                issueDraft.Assignees.Add(user);
            });

            //Get the targeted organization that holds the repository we will be creating the issue in.
            var organization = ConfigurationManager.AppSettings["Organization"];
            var issueRepository = ConfigurationManager.AppSettings["IssueRepository"];

            var issue = await client.Issue.Create(organization, issueRepository, issueDraft);
            return issue;
        }

    }
}