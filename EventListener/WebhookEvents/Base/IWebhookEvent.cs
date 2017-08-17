using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toadstool.WebhookEvents
{
    public interface IWebhookEvent
    {
        Task<Issue> CreateRepositoryIssue();
    }
}
