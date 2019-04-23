// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.AI.QnA;
using Microsoft.Bot.Schema;

namespace TestQnaBot
{
    /// <summary>
    /// Main entry point and orchestration for bot.
    /// </summary>
    public class TestQnaBotBot : IBot
    {
        public static readonly string QnAMakerKey = "Office365QnA";
        private readonly BotServices _services;

        /// <summary>
        /// Initializes a new instance of the <see cref="CoreBot"/> class.
        /// <param name="botServices">Bot services.</param>
        /// <param name="accessors">Bot State Accessors.</param>
        /// </summary>
        public TestQnaBotBot(BotServices services)
        {
            _services = services ?? throw new ArgumentNullException(nameof(services));

            if (!_services.QnAServices.ContainsKey(QnAMakerKey))
            {
                throw new System.ArgumentException(
                    $"Invalid configuration. Please check your '.bot' file for a QnA service named '{QnAMakerKey}'.");
            }
        }

        /// <summary>
        /// Run every turn of the conversation. Handles orchestration of messages.
        /// <param name="turnContext">Bot Turn Context.</param>
        /// <param name="cancellationToken">Task CancellationToken.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// </summary>
        public async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            var activity = turnContext.Activity;

            if (activity.Type == ActivityTypes.Message)
            {
                var options1 = new QnAMakerOptions
                {
                    Top = 10,
                    ScoreThreshold = 0.6f,
                };
                var response = await _services.QnAServices[QnAMakerKey].GetAnswersAsync(turnContext, options1);
                if (response != null && response.Length > 0)
                {
                    await turnContext.SendActivityAsync($"QnA Results: {response.Length}");
                }

                var options2 = new QnAMakerOptions
                {
                    Top = 10,
                    ScoreThreshold = 0.6f,
                    StrictFilters = new Metadata[] { new Metadata() { Name = "Topic", Value = "sharepoint" } },
                };
                var response2 = await _services.QnAServices[QnAMakerKey].GetAnswersAsync(turnContext, options2);
                if (response2 != null && response2.Length > 0)
                {
                    await turnContext.SendActivityAsync($"QnA Results: {response2.Length}");
                }

                var options3 = new QnAMakerOptions
                {
                    Top = 10,
                    ScoreThreshold = 0.6f,
                };
                var response3 = await _services.QnAServices[QnAMakerKey].GetAnswersAsync(turnContext, options3);
                if (response3 != null && response3.Length > 0)
                {
                    await turnContext.SendActivityAsync($"QnA Results: {response3.Length}");
                }
            }
            else if (activity.Type == ActivityTypes.ConversationUpdate)
            {
            }
        }
    }
}
