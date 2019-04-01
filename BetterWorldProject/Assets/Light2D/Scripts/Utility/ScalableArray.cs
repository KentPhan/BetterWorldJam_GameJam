using UnityEngine.Assertions;

namespace Light2D
{
	public class ScalableArray<T>
	{
		private T[] m_Array = null;
		public T[] array {
			get { return m_Array; }
			private set { m_Array = value; }
		}

		public T this[int index] {
			get { return m_Array[index]; }
			set { m_Array[index] = value; }
		}

		public int capacity {
			get { return m_Array != null ? m_Array.Length : 0; }
		}

		private int m_Legnth = 0;
		public int length {
			get { return m_Legnth; }
			private set { m_Legnth = value; }
		}

		public ScalableArray(int defaultCapacity)
		{
			Assert.IsTrue(defaultCapacity > 0, "Length must be bigger than zero.");

			m_Array = new T[defaultCapacity];
		}

		public void Clear()
		{
			length = 0;
		}

		public void SetLength(int length)
		{
			this.length = length;
		}

		public void Rescale(int capacity, bool copyOriginals = false)
		{
			Assert.IsTrue(capacity > 0, "Length must be bigger than zero.");

			if(capacity != length)
			{
				T[] newArray = m_Array = new T[capacity];

				if(copyOriginals)
				{
					for(int i = 0; i < length && i < newArray.Length; i++)
						newArray[i] = m_Array[i];
				}

				m_Array = newArray;
			}
		}

		public void Add(T item)
		{
			if(length >= capacity)
				Rescale(capacity * 2, true);

			m_Array[length++] = item;
		}

		public void Insert(T item, int index)
		{
			if(index >= array.Length)
				Rescale(index * 2, true);

			m_Array[index] = item;

			if(length <= index)
				length = index + 1;
		}
	}
}