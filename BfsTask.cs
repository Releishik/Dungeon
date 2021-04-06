using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon
{
	public class BfsTask
	{


	    public static IEnumerable<SinglyLinkedList<Point>> FindPaths(Map map, Point start, Point[] chests)
        {
			if (map.Dungeon != null && map.Dungeon.Length > 0)
			{
				List<LinkedList<Point>> list = new List<LinkedList<Point>>();
				for (int i = 0; i < chests.Length; i++)
				{
					list.Add(AStar(map, chests[i], start));
				}
			}
            yield break;
		}		

		static LinkedList<Point> AStar(Map map, Point from, Point to)
		{
			Dictionary<Point, Tuple<int, int>> weights = new Dictionary<Point, Tuple<int, int>>();
			LinkedList<Point> closed = new LinkedList<Point>();
			//HashSet<Point> closed = new HashSet<Point>();
			HashSet<Point> open = new HashSet<Point>();
			weights.Add(from, Tuple.Create(0, 0));
			open.Add(from);
			while(open.Count!=0)
			{
				var point = MinimumPoint(open, weights);
				open.Remove(point);
				closed.AddLast(point);
				foreach(var next in GetNeighbours(map, point, weights, to).Where(p=>!closed.Contains(p)))
				{
					if (next == to) { closed.AddLast(next); break; }
					open.Add(next);
				}
			}
			return closed;
		}

		static IEnumerable<Point> GetNeighbours(Map map, Point point, Dictionary<Point, Tuple<int, int>> weights, Point destination)
		{
			int[] coords = { -1, 0, 1 };
			var permuts = coords.SelectMany(e => coords, (x, y) => Tuple.Create(x, y)).Where(t => !t.Equals(Tuple.Create(0, 0))).Distinct().Select(t=>Tuple.Create(point.X+t.Item1, point.Y+t.Item2));
			foreach (var t in permuts
				.Where(t=> point.X + t.Item1 >= 0 && point.X + t.Item1 < map.Dungeon.GetLength(0) &&
				point.Y + t.Item2 >= 0 && point.Y + t.Item2 < map.Dungeon.GetLength(1))
				.Where(t => map.Dungeon[t.Item1, t.Item2] == MapCell.Empty))
			{
				var nextPoint = new Point(t.Item1, t.Item2);
				int h = SimpleDist(nextPoint, destination);
				int g = weights[point].Item1 + 10;
				weights.Add(nextPoint, Tuple.Create(g, h));
				yield return nextPoint;
			}
		}

		static int SimpleDist(Point a, Point b)
		{
			int x = b.X - a.X;
			int y = b.Y - a.Y;
			return (int)Math.Abs(Math.Sqrt(x * x + y * y) * 100);
		}

		static Point MinimumPoint(HashSet<Point> open, Dictionary<Point, Tuple<int, int>> weights) => open.OrderBy(p=>weights[p].Item1+weights[p].Item2).First();
	}



	/*
	 public class PathPoint
	{
		public int X;
		public int Y;
		public int[] Point;
		public PathPoint Previous;
		public int G;
		public int H;
		public PathPoint(int x, int y)
		{
			X = x;
			Y = y;
			Point = new int[] { x, y };
			Previous = null;
		}
		public override string ToString() => string.Format("[{0}, {1}]", X, Y);
	}

	public class Monster : ICreature
	{
		public int Priority = -1;

		CreatureCommand PathFind(int x, int y)
		{
			var player = GetPlayer();
			var map = MapMirror();
			List<PathPoint> closed = new List<PathPoint>();
			List<PathPoint> open = new List<PathPoint>();
			open.Add(new PathPoint(x, y) { G = 0 });
			map[x, y] = null;
			while (open.Count != 0 && player != null)
			{
				var topElem = MinimumPoint(open);
				closed.Add(topElem);
				open.Remove(topElem);
				if (topElem.X == player.X && topElem.Y == player.Y)
				{
					var path = PathConstruction(closed, topElem);
					var pX = path[path.Count - 2].X;
					var pY = path[path.Count - 2].Y;
					return new CreatureCommand() { DeltaX = pX - x, DeltaY = pY - y };
				}
				GetNeighbours(topElem, player, map, open, new int[] { 1, 0 }); //horiz neighbours
				GetNeighbours(topElem, player, map, open, new int[] { 0, 1 }); //vert neighbours
			}
			return new CreatureCommand();
		} 
	 */
}