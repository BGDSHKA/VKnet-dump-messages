using System;
using VkNet;
using VkNet.Enums.Filters;
using VkNet.Model;
using VkNet.Model.RequestParams;
using VkNet.AudioBypassService.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

namespace ConsoleApp15
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();
            services.AddAudioBypass();

            var api = new VkApi(services);

            api.Authorize(new ApiAuthParams
            {
                ApplicationId = PUT_APPLICATION_ID_HERE, // Id Standalone-приложения
                Login = "PUT_LOGIN_HERE", //Логин
                Password = "PUT_PASSWORD_HERE", //Пароль
                Settings = Settings.All //Полный доступ
            });

          

            Console.WriteLine(api.Token);
            var res = api.Groups.Get(new GroupsGetParams());

            Console.WriteLine(res.TotalCount);

            string writePath = @"E:\Messages.txt"; //Директория дампа сообщений
            try
            {
                using (StreamWriter sw = new StreamWriter(writePath, false, System.Text.Encoding.UTF8))
                {

                    for (int i = 0; i < 200000; i = i + 200)
                    {
                        var getHistory = api.Messages.GetHistory(new MessagesGetHistoryParams
                        {
                            Offset = i,
                            Count = 200,
                            PeerId = PUT_ID_DIALOGUE, //Id беседы (2000000000 +), паблик(-), диалога

                        });



                        foreach (Message p in getHistory.Messages)
                        {
                            if (p.FromId == PUT_USER_ID) //Если нужно выбрать сообщения определённого пользователя из беседы по Id
                            {
                                sw.WriteLine(p.Text);
                                Console.WriteLine(p.Text);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();

        }
    }
}
