using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TechnicBirthdayAgeService
{
    public class BirthdayCelebration
    {
        private List<string> thoughtsCollection = new List<string>();
        private string exceptionMessage;
        private DateTime birthday;
        private int age;
        public BirthdayCelebration(DateTime birthday)
        {
            this.birthday = birthday;
            PopulateThoughts();
        }

        public BirthdayCelebration(string exceptionMessage)
        {
            this.exceptionMessage = exceptionMessage;
            PopulateThoughts();
        }
        public string BirthdayOfWeek { get { return birthday.DayOfWeek.ToString(); } }
        public string AgeOfPerson
        {
            get
            {
                Process();
                return age.ToString();
            }
        }

      
        public int CountOfGoodThought { get; set; }
        public string GoodThought
        {
            get
            {
                thoughtsCollection.Shuffle();
                return thoughtsCollection.Random().ToString();
            }
        }
        public string ExceptionMessage { get { return exceptionMessage; } }
        private void Process()
        {
            age = DateTime.Now.Year-birthday.Year ;
            if (birthday > DateTime.Now.AddYears(-age))
            {
                age--;
            }

           // thoughtsCollection.Shuffle();

            //if (age < 10)
            //{
            //    AgeGroup = AgeGroupCategory.Tween;
            //}
            //else if (age >= 10 && age < 20)
            //{
            //    AgeGroup = AgeGroupCategory.Teenager;
            //}
            //else if (age >= 20 && age < 35)
            //{
            //    AgeGroup = AgeGroupCategory.Young;
            //}
            //else if (age >= 35 && age < 50)
            //{
            //    AgeGroup = AgeGroupCategory.Middle;
            //}
            //else
            //{
            //    AgeGroup = AgeGroupCategory.Good;
            //}
        }
        private void PopulateThoughts()
        {
            thoughtsCollection.Add("Life has many ways of testing a person’s will, either by having nothing happen at all or by having everything happen all at once.");
            thoughtsCollection.Add("There are two ways of spreading light: to be the candle, or the mirror that reflects it");
            thoughtsCollection.Add("With everything that has happened to you, you can either feel sorry for yourself or treat what has happened as a gift. Everything is either an opportunity to grow or an obstacle to keep you from growing. You get to choose.");
            thoughtsCollection.Add("Believe in yourself! Have faith in your abilities! Without a humble but reasonable confidence in your own powers you cannot be successful or happy.");
            thoughtsCollection.Add("Hate. It has caused a lot of problems in this world but has not solved one yet.");
            thoughtsCollection.Add("If opportunity doesn’t knock, build a door.");
            thoughtsCollection.Add("An attitude of positive expectation is the mark of the superior personality.");
            thoughtsCollection.Add("Success consists of going from failure to failure without loss of enthusiasm.");
            thoughtsCollection.Add("If you can dream it, then you can achieve it. You will get all you want in life if you help enough other people get what they want.");
            thoughtsCollection.Add("The only place where your dream becomes impossible is in your own thinking");
            thoughtsCollection.Add("We don’t see things as they are, we see them as we are.");
            thoughtsCollection.Add("Our greatest weakness lies in giving up. The most certain way to succeed is always to try just one more time.");
            thoughtsCollection.Add("I may not have gone where I intended to go, but I think I have ended up where I needed to be.");
            thoughtsCollection.Add("Learning is a gift. Even when pain is your teacher.");
            thoughtsCollection.Add("I’ve had a lot of worries in my life, most of which never happened");
            thoughtsCollection.Add("You yourself, as much as anybody in the entire universe, deserve your love and affection.");
            thoughtsCollection.Add("Hope is a waking dream.");
            thoughtsCollection.Add("Happiness is an attitude. We either make ourselves miserable, or happy and strong. The amount of work is the same.");
            thoughtsCollection.Add("I do believe we’re all connected. I do believe in positive energy. I do believe in the power of prayer. I do believe in putting good out into the world. And I believe in taking care of each other.");
            thoughtsCollection.Add("If you want light to come into your life, you need to stand where it is shining.");
            thoughtsCollection.Add("Today is a new beginning, a chance to turn your failures into achievements & your sorrows into so goods. No room for excuses.");
            thoughtsCollection.Add("Life is a gift, and it offers us the privilege, opportunity, and responsibility to give something back by becoming more.");
            thoughtsCollection.Add("We are all here for some special reason. Stop being a prisoner of your past. Become the architect of your future.");
            thoughtsCollection.Add("If you think you can do a thing or think you can’t do a thing, you’re right.");
            thoughtsCollection.Add("No matter what the situation, remind yourself I have a choice.");
            thoughtsCollection.Add("All you can change is yourself, but sometimes that changes everything!");
            thoughtsCollection.Add("The will to win, the desire to succeed, the urge to reach your full potential… these are the keys that will unlock the door to personal excellence.");
            thoughtsCollection.Add("If we’re growing, we’re always going to be out of our comfort zone.");
            thoughtsCollection.Add("Take chances, make mistakes. That’s how you grow. Pain nourishes your courage. You have to fail in order to practice being brave.");
            thoughtsCollection.Add("I am the greatest, I said that even before I knew I was.");
            thoughtsCollection.Add("We are responsible for what we are, and whatever we wish ourselves to be, we have the power to make ourselves.");
            thoughtsCollection.Add("The difference between stumbling blocks and stepping stones is how you use them.");
            thoughtsCollection.Add("If someone tells you, “You can’t” they really mean, “I can’t.");
        }
    }

    public static class Helper
    {
        private static Random rng = new Random();
        ////public static List<T> ShuffleElements<T>(this IList<T> list)
        ////{
        ////    Random rng = new Random();
        ////    int n = list.Count;
        ////    while (n > 1)
        ////    {
        ////        n--;
        ////        int k = rng.Next(n + 1);
        ////        T value = list[k];
        ////        list[k] = list[n];
        ////        list[n] = value;
        ////    }

        ////    return list;
        ////}

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }

        }
        public static T Random<T>(this IEnumerable<T> input)
        {
            return input.ElementAt(rng.Next(input.Count()));
        }

        //public static List<T> Random<T>(this IList<T> list, int takeNumber)
        //{
        //    return (list.Shuffle()).Take(takeNumber);
        //}
    }
}
