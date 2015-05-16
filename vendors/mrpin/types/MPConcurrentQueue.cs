using System;
using System.Collections.Generic;

public class MPConcurrentQueue <T>
{

	/*
	 * Fields
	 */ 
	private Queue<T> _queue;
	private Object _locker = new Object ();

	/*
	 * Properties
	 */

	public bool isEmpty {
		get {
			bool result;

			lock (_locker) {
				result = _queue.Count == 0;
			}

			return result;
		}
	}

	/*
	 * Methods
	 */ 

	public MPConcurrentQueue ()
	{
		_queue = new Queue<T> ();
	}

	public void push (T item)
	{
		lock (_locker) {
			_queue.Enqueue (item);
		}
	}

	public T pop ()
	{
		T result = default(T);

		lock (_locker) {
			result = _queue.Dequeue ();
		}

		return result;
	}
}
