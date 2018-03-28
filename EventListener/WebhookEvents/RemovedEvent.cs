using Octokit;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Toadstool.Utility;

namespace Toadstool.WebhookEvents
{
    public class RemovedEvent : WebhookEvent, IWebhookEvent
    {
        public RemovedEvent(string repositoryName, string actionUser, string action, string taggedUser)
        {
            RepositoryName = repositoryName;
            ActionUser = actionUser;
            Action = action;
            TaggedUser = taggedUser;
        }

        public async Task<Issue> CreateRepositoryIssue()
        {
            return await CreateRepositoryIssue(RepositoryName, $"An Outside Collaborator was added to {RepositoryName}!", $"{ActionUser} was added as an outside collaborator to {RepositoryName}. They have been removed.");
        }

        public async Task<bool> EditRepository(string owner, string repository, string user)
        {
            return await RemoveOutsideCollaborator(RepositoryName, owner, ActionUser);
        }
    }
}