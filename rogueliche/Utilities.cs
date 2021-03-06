﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rogueliche
{
    public class Utilities
    {
        private static Random random = new Random();

        public static int RandomNumber(int min, int max)
        {
            return random.Next(min, max + 1);
        }

        public static bool PassPercentileRoll(int testedValue)
        {
            return RandomNumber(1, 100) < testedValue;
        }

        public static int Clamp(int value, int min, int max)
        {
            return (value < min) ? min : (value > max) ? max : value;
        }

        public static T GetRandomFromList<T>(List<T> list)
        {
            return list[RandomNumber(0, list.Count - 1)];
        }
    }
}
