﻿using Craftify.DesignAutomationToolkit.Executions.Results;

namespace Craftify.DesignAutomationToolkit.Models;

public record WorkItemExecutedArgs(IReadOnlyCollection<PublishedArgument> PublishedArguments, TimedExecutionResult<WorkItemResult> WorkItemResult);