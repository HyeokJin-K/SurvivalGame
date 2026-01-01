
using System.Collections.Generic;
using UnityEngine;

public class CommandBuffer
{
    private readonly Queue<IPlayerActionCommand> _commandQueue = new Queue<IPlayerActionCommand>();
    private readonly float _bufferDuration;

    public CommandBuffer(float bufferDuration = 0f)
    {
        _bufferDuration = bufferDuration;
    }
    
    public void AddCommand(IPlayerActionCommand command)
    {
        _commandQueue.Enqueue(command);
    }

    public bool HasCommands()
    {
        return _commandQueue.Count > 0;
    }

    public void ProcessCommands(PlayerHandlerContext handlerContext)
    {
        while (_commandQueue.Count > 0)
        {
            var command = _commandQueue.Peek();

            if (Time.time - command.TimeStamp > _bufferDuration)
            {
                _commandQueue.Dequeue();
                continue;
            }

            if (command.CanExecute(handlerContext))
            {
                _commandQueue.Dequeue();
                command.Execute(handlerContext);
                break;
            }
            else
            {
                break;
            }
        }
    }

    public void ClearBuffer()
    {
        _commandQueue.Clear();
    }
}

