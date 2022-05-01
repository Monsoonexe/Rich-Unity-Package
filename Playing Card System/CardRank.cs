using System;

namespace RichPackage.PlayingCards
{
	public struct CardRank : IComparable<CardRank>
	{
		public ECardRank rank;
		public int Value => RankToValue(this.rank);

		#region Constructors

		public CardRank(ECardRank rank)
		{
			this.rank = rank;
		}

		#endregion

		#region IComparable<CardRank>

		public int CompareTo(CardRank other)
		{
			//none and joker can't be compared
			if (this.rank == ECardRank.None || this.rank == ECardRank.Joker || other.rank == ECardRank.None || other.rank == ECardRank.Joker)
				throw new NotSupportedException("Cannot compare None or Joker");

			//compare the rank
			if (this.Value > other.Value)
				return 1;
			else if (this.Value < other.Value)
				return -1;
			else
				return 0;
		}

		#endregion IComparable<CardRank>

		public static int RankToValue(ECardRank rank)
		{
			int value; //return value

			switch (rank)
			{
				//unsupported (maybe return int.Min instead of throwing exception)
				case ECardRank.Joker:
				case ECardRank.None:
					throw new NotSupportedException($"{rank} cannot be converted to a value.");

				case ECardRank.Ace:
					value = 11;
					break;

				case ECardRank.Jack:
				case ECardRank.Queen:
				case ECardRank.King:
					value = 10;
					break;

				default:
					value = (int)rank;
					break;
			}

			return value;
		}

		public static implicit operator ECardRank(CardRank a) => a.rank;
		public static implicit operator int (CardRank a) => RankToValue(a.rank);
	}
}
