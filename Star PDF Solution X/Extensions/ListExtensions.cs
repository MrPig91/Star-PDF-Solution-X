﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Star_PDF_Solution_X.Extensions
{
    public static class ListExtensions
    {
        public static void Sort<T>(this ObservableCollection<T> collection) where T : IComparable<T>
        {
            List<T> sorted = collection.OrderBy(x => x).ToList();
            for (int i = 0; i < sorted.Count(); i++)
                collection.Move(collection.IndexOf(sorted[i]), i);
        }
    }
}
