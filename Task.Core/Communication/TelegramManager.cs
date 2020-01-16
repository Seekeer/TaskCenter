using System;
using System.Linq;
using System.Collections.Generic;
using Task.Core.Model;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Task.Core.Data;
using System.Diagnostics;

namespace Task.Core.Communication
{
    public class TelegramSettings
    {
        public string APIKey { get; set; }
    }

    public enum TaskMessageButtons 
    { 
        Accept,
        Complete
    }

    public class TelegramManager
    {
        public TelegramManager( AppDbContext dbContext, ITaskRepository taskRepository, 
            IExecutorRepository executorRepository)
        {
            TelegramSettings telegramSettings = new TelegramSettings { APIKey = "1013744878:AAF5qFktlgyBpXeVp-KXpWyb4dnTqUbSPN4" };
            _botClient = new Telegram.Bot.TelegramBotClient(telegramSettings.APIKey);
            NLog.LogManager.GetCurrentClassLogger().Info($"Telegram bot created:{telegramSettings.APIKey}");
            _taskRepository = taskRepository;
            _executorRepository = executorRepository;
            _dbContext = dbContext;

            _botClient.OnCallbackQuery += _botClient_OnCallbackQuery;
            _botClient.OnReceiveError += _botClient_OnReceiveError;
            _botClient.OnReceiveGeneralError += _botClient_OnReceiveGeneralError;
            _botClient.OnMessage += _botClient_OnMessage;
            _botClient.OnInlineResultChosen += _botClient_OnInlineResultChosen;
            _botClient.StartReceiving(Array.Empty<UpdateType>());
        }

        public async System.Threading.Tasks.Task CheckTasks()
        {
            var tasks = _dbContext.ExecutorTasks.Where(x => x.LastStatus == TaskStatus.Created);
            await SendTask(tasks);
        }

        public async System.Threading.Tasks.Task SendTask(IEnumerable<ExecutorTask> executorTasks)
        {
            await System.Threading.Tasks.Task.WhenAll(executorTasks.Select(x => SendTask(x)).ToArray());
        }

        public async System.Threading.Tasks.Task CancelTask(GlobalTask task)
        {
            await System.Threading.Tasks.Task.WhenAll(task.ExecutorTasks.Select(x => CancelTask(x)).ToArray());
        }

        public async System.Threading.Tasks.Task CancelTask(ExecutorTask task)
        {
            await _botClient.DeleteMessageAsync(new Telegram.Bot.Types.ChatId(task.Executor.TelegramId), task.TelegramMessageId);
            _taskRepository.TaskCanceled(task);
        }

        private void _botClient_OnInlineResultChosen(object sender, Telegram.Bot.Args.ChosenInlineResultEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void _botClient_OnCallbackQuery1(object sender, Telegram.Bot.Args.CallbackQueryEventArgs e)
        {
            throw new NotImplementedException();
        }

        private async void _botClient_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            var user = _executorRepository.FindExecutor(e.Message.From.Id);
            if(user == null)
            {
                user = _executorRepository.RegisterExecutor(e.Message.From.Id, e.Message.From.FirstName, e.Message.From.LastName);

                if(string.IsNullOrEmpty(e.Message.From.LastName))
                {
                    await _botClient.SendTextMessageAsync(user.TelegramId, "Введите свою фамилию", ParseMode.Default, true, false, 0);
                    return;
                }
            }

            if (string.IsNullOrEmpty(user.LastName))
            {
                user.LastName = e.Message.Text;
                _executorRepository.Update(user);

                if(string.IsNullOrEmpty(e.Message.From.FirstName))
                {
                    await _botClient.SendTextMessageAsync(user.TelegramId, "Введите своё имя", ParseMode.Default, true, false, 0);
                    return;
                }
            }

            if (string.IsNullOrEmpty(e.Message.From.FirstName))
            {
                user.FirstName = e.Message.Text;
                _executorRepository.Update(user);
            }
        }

        public async System.Threading.Tasks.Task SendTask(ExecutorTask task)
        {
            var message = await _botClient.SendTextMessageAsync(task.Executor.TelegramId, GetTaskText(task), ParseMode.Default, true, false, 0, GetTaskMarkup(task, TaskMessageButtons.Accept));
            _taskRepository.TaskSent(task, message);
        }

        private string GetTaskText(ExecutorTask task)
        {
            if(task is ExecutorCommentsTask commentsTask)
                return $"Здравствуй, товарищ! Необходимо оставить комментарии под этими постами: {Environment.NewLine}" +
                    string.Join(Environment.NewLine, commentsTask.Groups);

            Debug.Assert(false, "New ExecutorTask type has been added");
            return string.Empty;
        }

        private InlineKeyboardMarkup GetTaskMarkup(ExecutorTask task, TaskMessageButtons buttonType)
        {
            var list = new List<InlineKeyboardButton>();

            switch (buttonType)
            {
                case TaskMessageButtons.Accept:
                    list.Add(InlineKeyboardButton.WithCallbackData("Задача принята", $"{ACCEPT_KEYWORD};{task.Task.Id}"));
                    break;
                case TaskMessageButtons.Complete:
                    list.Add(InlineKeyboardButton.WithCallbackData("Задача выполнена", $"{DONE_KEYWORD};{task.Task.Id}"));
                    break;
                default:
                    break;
            }

            return new InlineKeyboardMarkup(list.ToArray());
        }

        private void _botClient_OnCallbackQuery(object sender, Telegram.Bot.Args.CallbackQueryEventArgs e)
        {
            var messageData = e.CallbackQuery.Data;
            var parts = messageData.Split(';').ToList();
            switch (parts[0])
            {
                case ACCEPT_KEYWORD:
                    var task = _taskRepository.TaskAccepted(parts[1]);
                    _botClient.EditMessageReplyMarkupAsync(task.Executor.TelegramId, task.TelegramMessageId, GetTaskMarkup(task, TaskMessageButtons.Complete));
                    break;
                case DONE_KEYWORD:
                    var task2 = _taskRepository.TaskDone(parts[1]);
                    _botClient.EditMessageReplyMarkupAsync(task2.Executor.TelegramId, task2.TelegramMessageId);
                    break;
                default:
                    break;
            }
        }

        private void _botClient_OnReceiveError(object sender, Telegram.Bot.Args.ReceiveErrorEventArgs e)
        {
            NLog.LogManager.GetCurrentClassLogger().Fatal(e.ApiRequestException);
        }

        private void _botClient_OnReceiveGeneralError(object sender, Telegram.Bot.Args.ReceiveGeneralErrorEventArgs e)
        {
            NLog.LogManager.GetCurrentClassLogger().Fatal(e.Exception);
        }

        private const string ACCEPT_KEYWORD = "accept";
        private const string DONE_KEYWORD = "done";
        private readonly ITaskRepository _taskRepository;
        private readonly IExecutorRepository _executorRepository;
        private readonly AppDbContext _dbContext;
        private readonly TelegramBotClient _botClient;
    }
}
