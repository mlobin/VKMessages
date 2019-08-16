using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using VkNet.Model;
using System.Drawing;

namespace VKMessages
{
    class Program
    {
        public static int LevenshteinDistance(string src, string dest)
        {
            int[,] d = new int[src.Length + 1, dest.Length + 1];
            int i, j, cost;
            char[] str1 = src.ToCharArray();
            char[] str2 = dest.ToCharArray();

            for (i = 0; i <= str1.Length; i++)
                d[i, 0] = i;
            for (j = 0; j <= str2.Length; j++)
                d[0, j] = j;
            for (i = 1; i <= str1.Length; i++)
                for (j = 1; j <= str2.Length; j++)
                {
                    if (str1[i - 1] == str2[j - 1])
                        cost = 0;
                    else
                        cost = 1;
                    d[i, j] =
                        Math.Min(
                            d[i - 1, j] + 1,              // Deletion
                            Math.Min(
                                d[i, j - 1] + 1,          // Insertion
                                d[i - 1, j - 1] + cost)); // Substitution
                    if ((i > 1) && (j > 1) && (str1[i - 1] ==
                        str2[j - 2]) && (str1[i - 2] == str2[j - 1]))
                        d[i, j] = Math.Min(d[i, j], d[i - 2, j - 2] + cost);
                }
            return d[str1.Length, str2.Length];
        }


        static void Main(string[] args)
        {
            Dictionary<string, int> words = new Dictionary<string, int>();
            Dictionary<string, string> keys = new Dictionary<string, string>();


            VkNet.VkApi vkApi = new VkNet.VkApi();
            vkApi.Authorize(new ApiAuthParams
            {
                ApplicationId = 000000,
                Login = "Login",
                Password = "PassWord",
                Settings = VkNet.Enums.Filters.Settings.All
            });
            Random r = new Random();
            List<User> users = new List<User>();
            List<long> aaa = new List<long>();
            Test yyyy = Test.Load("msg_sc.json");
            Regex rr = new Regex("\\)+");
            //           yyyy.i -= 101032;//Хомяки
            //ulong hor = (ulong)vkApi.Messages.GetHistory(new VkNet.Model.RequestParams.MessagesGetHistoryParams { PeerId = 2000000107 }).Messages[0].OutRead;
            List<Message> msg = new List<Message>();
            MessageGetHistoryObject aa = new MessageGetHistoryObject();
            while (yyyy.i < 50980)
            {
                try
                {
                    int y = r.Next(21);
                    aa = vkApi.Messages.GetHistory(new VkNet.Model.RequestParams.MessagesGetHistoryParams
                    {
                        PeerId = 2000000168,   //для Аколитской флудилки цифра - 148, для студсовета - 168
                        Count = 180 + y,
                        Offset = yyyy.i,
                        Reversed = true
                    });
                    yyyy.i += 180 + y;
                }
                catch (Exception e)
                {
                    yyyy.i += 200;
                    continue;
                }
                Thread.Sleep(250 + r.Next(750));
                msg.AddRange(aa.Messages);
            }

            foreach (Message s in msg)
            {
                //int day = (((DateTime)s.Date).Year - 2016) * 365 + ((DateTime)s.Date).DayOfYear;
                if (yyyy.users.ContainsKey((long)s.FromId))
                    yyyy.users[(long)s.FromId]++;
                else yyyy.users.Add((long)s.FromId, 1);
                if (s.Emoji.HasValue || rr.Match(s.Text).Value == s.Text)
                    if (yyyy.smiles.ContainsKey((long)s.FromId))
                        yyyy.smiles[(long)s.FromId]++;
                    else yyyy.smiles.Add((long)s.FromId, 1);
                if (s.Attachments.Count > 0)
                    if (yyyy.pictures.ContainsKey((long)s.FromId))
                        yyyy.pictures[(long)s.FromId]++;
                    else yyyy.pictures.Add((long)s.FromId, 1);
                if (s.Attachments.Count > 0 || s.Emoji.HasValue || rr.Match(s.Text).Value == s.Text)
                    if (yyyy.flood.ContainsKey((long)s.FromId))
                        yyyy.flood[(long)s.FromId]++;
                    else yyyy.flood.Add((long)s.FromId, 1);
                /*if (yyyy.dates.ContainsKey(day))
                    yyyy.dates[day]++;
                else yyyy.dates.Add(day, 1);*/
            }

            List<long> a = new List<long>();
            foreach (var s in yyyy.users)
            {
                a.Add(s.Key);
            }
            var gusers = vkApi.Users.Get(a);
            var lusers = gusers.OrderBy(x =>
            {
                return yyyy.users[x.Id];
            }
            ).Reverse();
            int tst = 0;
            int flood = 0;
   //         yyyy.i += 101032;

            foreach (var s in lusers)
            {
                string t = s.FirstName + " " + s.LastName;
                tst += yyyy.users[s.Id];
                if (!yyyy.pictures.ContainsKey(s.Id))
                    yyyy.pictures.Add(s.Id, 0);
                if (!yyyy.smiles.ContainsKey(s.Id))
                    yyyy.smiles.Add(s.Id, 0);
                if (!yyyy.flood.ContainsKey(s.Id))
                    yyyy.flood.Add(s.Id, 0);
                flood += yyyy.flood[s.Id];
                Console.WriteLine(t + " нафлудил: " + yyyy.users[s.Id].ToString() +
                    " из них " + yyyy.pictures[s.Id].ToString() + " с картинками, и " + yyyy.smiles[s.Id].ToString() + " со смайлами");
            }
            //Console.WriteLine("Сергей написал " + (ser + seroff).ToString() + " сообщений только из скобочек, из них " + seroff + " не из трех. Злых (><) смайлов было "+serang+". Эл написал "+l+" косящихся глаз.");
            yyyy.Save("msg_sc.json");
            /*using (StreamWriter sw = new StreamWriter("int.txt", false))
            {
                foreach (var t in yyyy.dates)
                {
                    sw.Write(t.Value.ToString() + " ");
                }
            }*/
            Console.ReadLine();
        }


    }

    public class Test
    {
        public int i;
        public Dictionary<long, int> users;
        public Dictionary<long, int> smiles = new Dictionary<long, int>();
        public Dictionary<long, int> pictures = new Dictionary<long, int>();
        public Dictionary<long, int> flood = new Dictionary<long, int>();
        public Dictionary<int, int> dates = new Dictionary<int, int>();
        public static Test Load(string str)
        {
            if (!File.Exists(str))
            {
                return new Test { i = 0, users = new Dictionary<long, int>() };
            }
            using (StreamReader r = new StreamReader(str))
            {
                string json = r.ReadToEnd();
                Test items = JsonConvert.DeserializeObject<Test>(json);
                return items;
            }
        }

        public void Save(string str)
        {
            using (StreamWriter r = new StreamWriter(str))
            {
                r.Write(JsonConvert.SerializeObject(this));
            }
        }
    }
}
