using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CommandController : SingletonInstance<CommandController>
{
    // List of commands
    private List<ICommand> _commandsToExecute = new List<ICommand>();

    public void Add(ICommand command)
    {
        _commandsToExecute.Add(command);
    }

    public void Play()
    {
        StartCoroutine(PlayCoroutine());
    }

    public void Rewind()
    {
        StartCoroutine(RewindCoroutine());
    }

    private IEnumerator PlayCoroutine()
    {
        foreach (var command in _commandsToExecute)
        {
            Debug.Log(nameof(PlayCoroutine) + " running...");

            command.Execute();

            yield return new WaitForEndOfFrame();
        }

        yield return null;
    }

    private IEnumerator RewindCoroutine()
    {
        foreach (var command in Enumerable.Reverse(_commandsToExecute))
        {
            Debug.Log(nameof(RewindCoroutine) + " running...");

            command.Undo();

            yield return new WaitForEndOfFrame();
        }

        yield return null;
    }

    public void Clear()
    {
        _commandsToExecute.Clear();
    }
}
