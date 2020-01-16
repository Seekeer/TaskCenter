using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Task.Core.Model;

namespace Task.Core
{
    public static class Solver
    {
        public static CommentsTask CreateCommentTask(IEnumerable<Executor> executors, string groupsList, string caption)
        {
            var links = new Stack<string>(groupsList.SplitByNewLine());
            var task = new CommentsTask() { Caption = caption };

            foreach (var executor in executors)
            {
                if (!links.Any())
                    break;

                var taskLinks = links.GetN(executor.LastPower);
                task.AddExecutorTasks(new ExecutorCommentsTask { Groups = string.Join(Environment.NewLine, taskLinks), Executor = executor });
            }

            return task;
        }

        private static IEnumerable<T> GetN<T>(this Stack<T> stack, int number)
        {
            var result = new List<T>();

            for (int i = 0; i < number; i++)
            {
                if (stack.Any())
                    result.Add(stack.Pop());
            }

            return result;
        }

        private static IEnumerable<string> SplitByNewLine(this string input)
        {
            var lines = input.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Where(x => !string.IsNullOrWhiteSpace(x));
            return lines;
        }

    }
}
