﻿using Octokit;
using System.Threading.Tasks;

namespace Toadstool.WebhookEvents
{
    public class CreatedEvent : WebhookEvent, IWebhookEvent
    {
        public CreatedEvent(string repositoryName, string actionUser, string action, string taggedUser)
        {
            RepositoryName = repositoryName;
            ActionUser = actionUser;
            Action = action;
            TaggedUser = taggedUser;
        }

        public async Task<Issue> CreateRepositoryIssue()
        {
            return await CreateIssue();
        }
    }
}