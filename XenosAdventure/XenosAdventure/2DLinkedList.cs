using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XenosAdventure
{
    class _2DLinkedListNode<T>
    {
        public _2DLinkedListNode<T> left, right, up, down;
        public T item;

        public _2DLinkedListNode(T item)
        {
            this.item = item;
        }

        public _2DLinkedListNode<T> SetLeft(_2DLinkedListNode<T> left)
        {
            this.left = left;
            return this;
        }

        public _2DLinkedListNode<T> SetRight(_2DLinkedListNode<T> right)
        {
            this.right = right;
            return this;
        }

        public _2DLinkedListNode<T> SetUp(_2DLinkedListNode<T> up)
        {
            this.up = up;
            return this;
        }

        public _2DLinkedListNode<T> SetDown(_2DLinkedListNode<T> down)
        {
            this.down = down;
            return this;
        }
    }
    class _2DLinkedList<T>
    {
        public _2DLinkedList()
        {

        }
    }
}
