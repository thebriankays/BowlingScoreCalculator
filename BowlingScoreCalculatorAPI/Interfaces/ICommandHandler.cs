﻿namespace BowlingScoreCalculatorAPI.Interfaces
{
    public interface ICommandHandler<TCommand, TResult>
    {
        Task<TResult> HandleAsync(TCommand command);
    }
}
