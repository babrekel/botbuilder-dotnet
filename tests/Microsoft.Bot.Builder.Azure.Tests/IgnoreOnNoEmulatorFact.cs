﻿using System;
using System.Diagnostics;
using System.IO;
using Xunit;

namespace Microsoft.Bot.Builder.Azure.Tests
{
    public sealed class IgnoreOnNoEmulatorFact : FactAttribute
    {
        public IgnoreOnNoEmulatorFact()
        {
            if (!CosmosDbEmulatorStatus.HasEmulator)
            {
                Skip = CosmosDbEmulatorStatus.NoEmulatorMessage;
            }

            if (Debugger.IsAttached)
            {
                Assert.True(CosmosDbEmulatorStatus.HasEmulator, CosmosDbEmulatorStatus.NoEmulatorMessage);
            }
        }
    }
}
