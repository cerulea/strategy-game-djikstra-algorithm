﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StrategyGameDjikstraAlgo;

namespace StrategyGameDjikstraAlgo
{
    /// <summary>
    /// This MinHeap does not support duplicates;
    /// each key's hashCode must be unique.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MinHeap<T> : Heap<T> where T : IComparable<T>
    {
        public MinHeap() : base()
        {
        }

        public MinHeap(IEnumerable<T> collection) : base(collection)
        {
        }

        /// <summary>
        /// Assumes that arr and length are set, but arr might not be a min heap.
        /// Makes arr a min heap.
        /// The running time of this heapify algorithm is O(n).
        /// </summary>
        protected override void BuildHeap()
        {
            int firstNonLeafIndex = (int)Math.Floor((double)HeapSize() / 2);

            for (int heapIndex = firstNonLeafIndex; heapIndex >= 1; --heapIndex)
                Heapify(heapIndex);
        }

        /// <summary>
        /// Assumes that the binary trees rooted at
        /// Left(minHeapIndex) and Right(minHeapIndex)
        /// are min heaps, but that minHeap(minHeapIndex)
        /// might be greater than its children.
        /// MinHeapify lets the value at minHeap(minHeapIndex) "float down"
        /// so that the minHeap obeys the minHeap property.
        /// Running time of this algorithm is O(lg n) or O(h).
        /// </summary>
        /// <param name="heapIndex"></param>
        protected override void Heapify(int heapIndex)
        {
            const string errorMessage
                = "MinHeap::Heapify(): Specified minHeapIndex must be >= 1";

            if (heapIndex <= 0)
                throw new HeapException(errorMessage);

            Debug.Assert(heapIndex >= 1, errorMessage);

            int l = Left(heapIndex);
            int r = Right(heapIndex);

            int smallest;

            if (l <= HeapSize() && Get(l).CompareTo(Get(heapIndex)) < 0)
                smallest = l;
            else
                smallest = heapIndex;

            if (r <= HeapSize() && Get(r).CompareTo(Get(smallest)) < 0)
                smallest = r;

            if (smallest != heapIndex)
            {
                // check these two lines
                keyToHeapIndex[Get(heapIndex)] = smallest;
                keyToHeapIndex[Get(smallest)] = heapIndex;

                Util.Swap(heap, listIndex(heapIndex), listIndex(smallest));

                Heapify(smallest);
            }
        }

        public void Insert(T key)
        {
            heap.Add(key);

            keyToHeapIndex.Add(key, HeapSize());

            int tempHeapIndex = HeapSize();

            while (tempHeapIndex > 1
                && Get(Parent(tempHeapIndex)).CompareTo(Get(tempHeapIndex)) > 0)
            {
                keyToHeapIndex[Get(tempHeapIndex)] = Parent(tempHeapIndex);
                keyToHeapIndex[Get(Parent(tempHeapIndex))] = tempHeapIndex;

                Util.Swap(heap,
                    listIndex(tempHeapIndex),
                    listIndex(Parent(tempHeapIndex)));

                tempHeapIndex = Parent(tempHeapIndex);
            }
        }

        /// <summary>
        /// Public interface method of decreaseKey.
        /// </summary>
        /// <param name="oldKey"></param>
        /// <param name="newKey"></param>
        public void DecreaseKey(T oldKey, T newKey)
        {
            // first check if key in dict

            const string errorMessage
                = "MinHeap::DecreaseKey(): oldKey not in dict";

            if (!keyToHeapIndex.ContainsKey(oldKey))
                throw new HeapException(errorMessage);

            Debug.Assert(keyToHeapIndex.ContainsKey(oldKey), errorMessage);

            DecreaseKey(keyToHeapIndex[oldKey], newKey);
        }

        /// <summary>
        /// Helper method of the public method DecreaseKey.
        /// </summary>
        /// <param name="heapIndex"></param>
        /// <param name="key"></param>
        private void DecreaseKey(int heapIndex, T key)
        {
            CheckHeapIndex(heapIndex);

            const string errorMessage
                = "MinHeap::DecreaseKey(): New key is larger than old key";

            if (key.CompareTo(Get(heapIndex)) > 0)
                throw new HeapException(errorMessage);

            Debug.Assert(key.CompareTo(Get(heapIndex)) <= 0, errorMessage);

            // possible time savings tweak: check for unncessary steps
            // would also need to add oldKey param
            /*
            if (key != oldKey)
            {
                keyToHeapIndex.Remove(Get(heapIndex));
                keyToHeapIndex.Add(key, heapIndex);

                Set(heapIndex, key);
            }
            */

            // need this?
            /*
            keyToHeapIndex.Remove(Get(heapIndex));
            keyToHeapIndex.Add(key, heapIndex);
            */

            Set(heapIndex, key);

            while (heapIndex > 1
                && Get(Parent(heapIndex)).CompareTo(Get(heapIndex)) > 0) {

                keyToHeapIndex[Get(heapIndex)] = Parent(heapIndex);
                keyToHeapIndex[Get(Parent(heapIndex))] = heapIndex;

                Util.Swap(heap, listIndex(heapIndex), listIndex(Parent(heapIndex)));

                heapIndex = Parent(heapIndex);
            }
        }
    }
}
