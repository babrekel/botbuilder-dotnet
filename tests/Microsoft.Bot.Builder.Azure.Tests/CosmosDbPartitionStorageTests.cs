﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Bot.Builder.Adapters;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Tests;
<<<<<<< HEAD
using Xunit;
=======
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
>>>>>>> Enhanced tests

namespace Microsoft.Bot.Builder.Azure.Tests
{
    [Trait("TestCategory", "Storage")]
    [Trait("TestCategory", "Storage - CosmosDB Partitioned")]
    public class CosmosDbPartitionStorageTests : StorageBaseTests, IDisposable, IClassFixture<CosmosDbPartitionStorageFixture>
    {
        // Endpoint and Authkey for the CosmosDB Emulator running locally
        private const string CosmosServiceEndpoint = "https://localhost:8081";
        private const string CosmosAuthKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
        private const string CosmosDatabaseName = "test-CosmosDbPartitionStorageTests";
        private const string CosmosCollectionName = "bot-storage";
        private IStorage _storage;

        public CosmosDbPartitionStorageTests()
        {
            _storage = new CosmosDbPartitionedStorage(
                new CosmosDbPartitionedStorageOptions
                {
                    AuthKey = CosmosAuthKey,
                    ContainerId = CosmosCollectionName,
                    CosmosDbEndpoint = CosmosServiceEndpoint,
                    DatabaseId = CosmosDatabaseName,
                });
        }

        public async void Dispose()
        {
            _storage = null;
        }

        [Fact]
        public void Constructor_Should_Throw_On_InvalidOptions()
        {
            // No Options. Should throw.
            Assert.Throws<ArgumentNullException>(() => new CosmosDbPartitionedStorage(null));

            // No Endpoint. Should throw.
            Assert.Throws<ArgumentException>(() => new CosmosDbPartitionedStorage(new CosmosDbPartitionedStorageOptions()
            {
                AuthKey = "test",
                ContainerId = "testId",
                DatabaseId = "testDb",
                CosmosDbEndpoint = null,
            }));

            // No Auth Key. Should throw.
            Assert.Throws<ArgumentException>(() => new CosmosDbPartitionedStorage(new CosmosDbPartitionedStorageOptions()
            {
                AuthKey = null,
                ContainerId = "testId",
                DatabaseId = "testDb",
                CosmosDbEndpoint = "testEndpoint",
            }));

            // No Database Id. Should throw.
            Assert.Throws<ArgumentException>(() => new CosmosDbPartitionedStorage(new CosmosDbPartitionedStorageOptions()
            {
                AuthKey = "testAuthKey",
                ContainerId = "testId",
                DatabaseId = null,
                CosmosDbEndpoint = "testEndpoint",
            }));

            // No Container Id. Should throw.
            Assert.Throws<ArgumentException>(() => new CosmosDbPartitionedStorage(new CosmosDbPartitionedStorageOptions()
            {
                AuthKey = "testAuthKey",
                ContainerId = null,
                DatabaseId = "testDb",
                CosmosDbEndpoint = "testEndpoint",
            }));

            // Invalid Row Key characters in KeySuffix
            Assert.Throws<ArgumentException>(() => new CosmosDbPartitionedStorage(new CosmosDbPartitionedStorageOptions()
            {
                AuthKey = "testAuthKey",
                ContainerId = "testId",
                DatabaseId = "testDb",
                CosmosDbEndpoint = "testEndpoint",
                KeySuffix = "?#*test",
                CompatibilityMode = false
            }));

            Assert.Throws<ArgumentException>(() => new CosmosDbPartitionedStorage(new CosmosDbPartitionedStorageOptions()
            {
                AuthKey = "testAuthKey",
                ContainerId = "testId",
                DatabaseId = "testDb",
                CosmosDbEndpoint = "testEndpoint",
                KeySuffix = "thisisatest",
                CompatibilityMode = true
            }));
        }

        // NOTE: THESE TESTS REQUIRE THAT THE COSMOS DB EMULATOR IS INSTALLED AND STARTED !!!!!!!!!!!!!!!!!
        [IgnoreOnNoEmulatorFact]
        public async Task CreateObjectCosmosDBPartitionTest()
        {
            await CreateObjectTest(_storage);
        }

        // NOTE: THESE TESTS REQUIRE THAT THE COSMOS DB EMULATOR IS INSTALLED AND STARTED !!!!!!!!!!!!!!!!!
        [IgnoreOnNoEmulatorFact]
        public async Task ReadUnknownCosmosDBPartitionTest()
        {
            await ReadUnknownTest(_storage);
        }

        // NOTE: THESE TESTS REQUIRE THAT THE COSMOS DB EMULATOR IS INSTALLED AND STARTED !!!!!!!!!!!!!!!!!
        [IgnoreOnNoEmulatorFact]
        public async Task UpdateObjectCosmosDBPartitionTest()
        {
            await UpdateObjectTest<CosmosException>(_storage);
        }

        // NOTE: THESE TESTS REQUIRE THAT THE COSMOS DB EMULATOR IS INSTALLED AND STARTED !!!!!!!!!!!!!!!!!
        [IgnoreOnNoEmulatorFact]
        public async Task DeleteObjectCosmosDBPartitionTest()
        {
            await DeleteObjectTest(_storage);
        }

        // NOTE: THESE TESTS REQUIRE THAT THE COSMOS DB EMULATOR IS INSTALLED AND STARTED !!!!!!!!!!!!!!!!!
        [IgnoreOnNoEmulatorFact]
        public async Task DeleteUnknownObjectCosmosDBPartitionTest()
        {
            await _storage.DeleteAsync(new[] { "unknown_delete" });
        }

        // NOTE: THESE TESTS REQUIRE THAT THE COSMOS DB EMULATOR IS INSTALLED AND STARTED !!!!!!!!!!!!!!!!!
        [IgnoreOnNoEmulatorFact]
        public async Task HandleCrazyKeysCosmosDBPartition()
        {
            await HandleCrazyKeys(_storage);
        }

        // NOTE: THESE TESTS REQUIRE THAT THE COSMOS DB EMULATOR IS INSTALLED AND STARTED !!!!!!!!!!!!!!!!!
        [IgnoreOnNoEmulatorFact]
        public async Task ReadingEmptyKeysReturnsEmptyDictionary()
        {
            var state = await _storage.ReadAsync(new string[] { });
            Assert.IsType<Dictionary<string, object>>(state);
            Assert.Equal(0, state.Count);
        }

        // NOTE: THESE TESTS REQUIRE THAT THE COSMOS DB EMULATOR IS INSTALLED AND STARTED !!!!!!!!!!!!!!!!!
        [IgnoreOnNoEmulatorFact]
        public async Task ReadingNullKeysThrowException()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _storage.ReadAsync(null));
        }

        // NOTE: THESE TESTS REQUIRE THAT THE COSMOS DB EMULATOR IS INSTALLED AND STARTED !!!!!!!!!!!!!!!!!
        [IgnoreOnNoEmulatorFact]
        public async Task WritingNullStoreItemsThrowException()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _storage.WriteAsync(null));
        }

        // NOTE: THESE TESTS REQUIRE THAT THE COSMOS DB EMULATOR IS INSTALLED AND STARTED !!!!!!!!!!!!!!!!!
        [IgnoreOnNoEmulatorFact]
        public async Task WritingNoStoreItemsDoesntThrow()
        {
<<<<<<< HEAD
            var changes = new Dictionary<string, object>();
            await _storage.WriteAsync(changes);
=======
            if (CheckEmulator())
            {
                var changes = new Dictionary<string, object>();
                await _storage.WriteAsync(changes);
            }
        }

        // NOTE: THESE TESTS REQUIRE THAT THE COSMOS DB EMULATOR IS INSTALLED AND STARTED !!!!!!!!!!!!!!!!!
        [TestMethod]
        public async Task WritingWithEtagThrowsExceptionWithPreconditionFailure()
        {
            const string concurrencyKey = "concurrent-value";

            if (CheckEmulator())
            {
                _storageOptions.OptimisticConcurrencyEnabled = true;
                var dict = new Dictionary<string, object>
                {
                    [concurrencyKey] = new
                    {
                        etag = (string)null,
                        value = "xyz",
                    }
                };

                await _storage.WriteAsync(dict, CancellationToken.None);
                var dictFromStorage = await _storage.ReadAsync(new[] { concurrencyKey }, CancellationToken.None);

                await _storage.WriteAsync(dictFromStorage, CancellationToken.None);

                var result = await Assert.ThrowsExceptionAsync<CosmosException>(() => _storage.WriteAsync(dictFromStorage, CancellationToken.None));
                Assert.AreEqual(HttpStatusCode.PreconditionFailed, result.StatusCode, "expected conflict writing concurrently");

                // should be able to write with null ETag.
                await _storage.WriteAsync(dict, CancellationToken.None);

                // should be able to write with "*" ETag.
                var dict2 = new Dictionary<string, object>
                {
                    [concurrencyKey] = new
                    {
                        etag = "*",
                        value = "xyz",
                    }
                };

                await _storage.WriteAsync(dict2, CancellationToken.None);

                dictFromStorage = await _storage.ReadAsync(new[] { concurrencyKey }, CancellationToken.None);
                await _storage.WriteAsync(dictFromStorage);
            }
        }

        // NOTE: THESE TESTS REQUIRE THAT THE COSMOS DB EMULATOR IS INSTALLED AND STARTED !!!!!!!!!!!!!!!!!
        [TestMethod]
        public async Task WritingWithEtagThrowsExceptionWhenWritingInvalidEtagType()
        {
            if (CheckEmulator())
            {
                _storageOptions.OptimisticConcurrencyEnabled = true;

                var dict = new Dictionary<string, object>
                {
                    ["key"] = new { etag = 5 }
                };

                var result = await Assert.ThrowsExceptionAsync<ArgumentException>(() => _storage.WriteAsync(dict, CancellationToken.None));
                Assert.IsTrue(result.Message.Contains("Expected a string or no value."), $"{result.Message} did not contain 'Expected a string or no value.'");
            }
>>>>>>> Enhanced tests
        }

        // NOTE: THESE TESTS REQUIRE THAT THE COSMOS DB EMULATOR IS INSTALLED AND STARTED !!!!!!!!!!!!!!!!!
        // For issue https://github.com/Microsoft/botbuilder-dotnet/issues/871
        // See the linked issue for details. This issue was happening when using the CosmosDB
        // data store for state. The stepIndex, which was an object being cast to an Int64
        // after deserialization, was throwing an exception for not being Int32 datatype.
        // This test checks to make sure that this error is no longer thrown.
        //
        // The problem was reintroduced when the prompt retry count feature was implemented:
        // https://github.com/microsoft/botbuilder-dotnet/issues/1859
        // The waterfall in this test has been modified to include a prompt.
        [IgnoreOnNoEmulatorFact]
        public async Task WaterfallCosmos()
        {
            var convoState = new ConversationState(_storage);

            var adapter = new TestAdapter()
                .Use(new AutoSaveStateMiddleware(convoState));

            var dialogState = convoState.CreateProperty<DialogState>("dialogState");
            var dialogs = new DialogSet(dialogState);

            dialogs.Add(new TextPrompt(nameof(TextPrompt), async (promptContext, cancellationToken) =>
            {
                var result = promptContext.Recognized.Value;
                if (result.Length > 3)
                {
                    var succeededMessage = MessageFactory.Text($"You got it at the {promptContext.AttemptCount}th try!");
                    await promptContext.Context.SendActivityAsync(succeededMessage, cancellationToken);
                    return true;
                }

                var reply = MessageFactory.Text($"Please send a name that is longer than 3 characters. {promptContext.AttemptCount}");
                await promptContext.Context.SendActivityAsync(reply, cancellationToken);

                return false;
            }));

            var steps = new WaterfallStep[]
                {
                    async (stepContext, ct) =>
                    {
                        Assert.Equal(typeof(int), stepContext.ActiveDialog.State["stepIndex"].GetType());
                        await stepContext.Context.SendActivityAsync("step1");
                        return Dialog.EndOfTurn;
                    },
                    async (stepContext, ct) =>
                    {
                        Assert.Equal(typeof(int), stepContext.ActiveDialog.State["stepIndex"].GetType());
                        return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = MessageFactory.Text("Please type your name.") }, ct);
                    },
                    async (stepContext, ct) =>
                    {
                        Assert.Equal(typeof(int), stepContext.ActiveDialog.State["stepIndex"].GetType());
                        await stepContext.Context.SendActivityAsync("step3");
                        return Dialog.EndOfTurn;
                    },
                };

            dialogs.Add(new WaterfallDialog(nameof(WaterfallDialog), steps));

            await new TestFlow(adapter, async (turnContext, cancellationToken) =>
            {
                var dc = await dialogs.CreateContextAsync(turnContext);

                await dc.ContinueDialogAsync();
                if (!turnContext.Responded)
                {
                    await dc.BeginDialogAsync(nameof(WaterfallDialog));
                }
            })
                .Send("hello")
                .AssertReply("step1")
                .Send("hello")
                .AssertReply("Please type your name.")
                .Send("hi")
                .AssertReply("Please send a name that is longer than 3 characters. 1")
                .Send("hi")
                .AssertReply("Please send a name that is longer than 3 characters. 2")
                .Send("hi")
                .AssertReply("Please send a name that is longer than 3 characters. 3")
                .Send("Kyle")
                .AssertReply("You got it at the 4th try!")
                .AssertReply("step3")
                .StartTestAsync();
        }
    }
}
