using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TMA
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("********TMA********");
            Console.WriteLine("please enter the number of pairs you want to form: ");

            int pairsCount = Int32.Parse(Console.ReadLine());

            Console.WriteLine("Now enter the names of all {0} boys: ", pairsCount);
            Dictionary<string, List<string>> boys = new Dictionary<string, List<string>>();

            for (int i = 0; i < pairsCount; i++)
            {
                boys.Add(Console.ReadLine(), new List<string>());
            }

            Console.WriteLine("Now enter the names of all {0} girls: ", pairsCount);
            Dictionary<string, List<string>> girls = new Dictionary<string, List<string>>();

            for (int i = 0; i < pairsCount; i++)
            {
                girls.Add(Console.ReadLine(), new List<string>());
            }

            Console.Clear();
            Console.WriteLine("********TMA********");

            for (int i = 0; i < pairsCount; i++)
            {
                Console.Write("Enter the name of all the girls liked by {0} in the order of his liking: ", boys.ElementAt(i).Key);
                boys[boys.ElementAt(i).Key].AddRange(Console.ReadLine().Split(' '));
            }

            for (int i = 0; i < pairsCount; i++)
            {
                Console.Write("Enter the name of all the boys liked by {0} in the order of her liking: ", girls.ElementAt(i).Key);
                girls[girls.ElementAt(i).Key].AddRange(Console.ReadLine().Split(' '));
            }
            var stablePairs = TraditionalMarriageAlgorithm(boys, girls);
            Console.Clear();
            Console.WriteLine("The most stable pairs are: ");
            foreach (var item in stablePairs)
            {
                Console.WriteLine(item);
            }
            Console.ReadKey();
        }

        public static dynamic TraditionalMarriageAlgorithm(Dictionary<string, List<string>> boys, Dictionary<string, List<string>> girls)
        {
            Dictionary<string, List<string>> dictOfGirls = new Dictionary<string, List<string>>();
            for (int i = 0; i < boys.Count; i++)
            {
                dictOfGirls.Add(girls.ElementAt(i).Key, new List<string>());
            }

            for (int i = 0; i < boys.Count; i++)
            {
                dictOfGirls[boys.ElementAt(i).Value[0]].Add(boys.ElementAt(i).Key);
            }

            while(isNotStable(dictOfGirls))
            {
                List<string> rejectedBoys = makeGirlsChoose(ref dictOfGirls, ref girls, ref boys);
                makeRejectedBoysChoose(rejectedBoys, ref dictOfGirls, ref boys, ref girls);
            }

            return dictOfGirls.Select(s => s.Key + "<-->" + s.Value[0]);
        }

        private static List<string> makeGirlsChoose(ref Dictionary<string, List<string>> dictOfGirls, ref Dictionary<string, List<string>> girls, ref Dictionary<string, List<string>> boys)
        {
            List<string> rejectedBoys = new List<string>();
            for (int i = 0; i < dictOfGirls.Count; i++)
            {
                if (dictOfGirls.ElementAt(i).Value.Count>1)
                {
                    var lstAvailible = dictOfGirls.ElementAt(i).Value.ToDictionary(s => s);
                    var lstLiking = girls[dictOfGirls.ElementAt(i).Key];
                    string bestChoice = string.Empty;
                    for (int j = 0; j < lstLiking.Count; j++)
                    {
                        if (lstAvailible.ContainsKey(lstLiking[j]))
                        {
                            bestChoice = lstLiking[j];
                            break;
                        }
                    }
                    for (int j = 0; j < dictOfGirls.ElementAt(i).Value.Count; j++)
                    {
                        if (dictOfGirls.ElementAt(i).Value[j]!=bestChoice)
                        {
                            boys[dictOfGirls.ElementAt(i).Value[j]].Remove(dictOfGirls.ElementAt(i).Key);
                            rejectedBoys.Add(dictOfGirls.ElementAt(i).Value[j]);
                            dictOfGirls.ElementAt(i).Value.RemoveAt(j);
                            j--;
                        }
                    }
                }
            }
            return rejectedBoys;
        }

        private static void makeRejectedBoysChoose(List<string> rejectedBoys, ref Dictionary<string, List<string>> dictOfGirls, ref Dictionary<string, List<string>> boys, ref Dictionary<string, List<string>> girls)
        {
            for (int i = 0; i < rejectedBoys.Count; i++)
            {
                dictOfGirls[boys[rejectedBoys[i]][0]].Add(rejectedBoys[i]);
            }
        }

        private static bool isNotStable(Dictionary<string, List<string>> dict)
        {
            bool result = false;
            for (int i = 0; i < dict.Count; i++)
            {
                if (dict.ElementAt(i).Value.Count!=1)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }
    }
}
