namespace Program
{
    internal class Program
    {
        public class Circle
        {
            public double X { get; set; }
            public double Y { get; set; }
            public double R { get; set; }
            public Circle(double x, double y, double r)
            {
                X = x;
                Y = y;
                R = r;
            }
            public static List<Circle> CreateCircleList()
            {
                return new List<Circle>
                {
                    new Circle(0,0,3),
                    new Circle(4,0,3),
                    new Circle(4,2,3),
                    new Circle(8,0,3),
                    new Circle(12,0,3),
                    new Circle(100,2,2),
                    new Circle(20,5,5),
                    new Circle(99,4,2),
                };
            }
            public static bool Intersects(Circle A, Circle B)
            {
                //double dist = Math.Pow(Math.Pow(A.X - B.X, 2) + Math.Pow(A.Y - B.Y, 2), 1 / 2);
                double distance = Math.Sqrt(Math.Pow(A.X - B.X, 2) + Math.Pow(A.Y - B.Y, 2));

                return distance <= A.R + B.R;
            }

        }
        static List<Circle> ChoiceCircles()
        {
            Console.WriteLine("Choose an option:");
            Console.WriteLine("1. Use the predefined list of circles");
            Console.WriteLine("2. Enter your own circles");
            string choiceNumber = Console.ReadLine();

            List<Circle> mCircles = new List<Circle>();

            if (choiceNumber == "1")
            {
                mCircles = Circle.CreateCircleList();
            }
            else if (choiceNumber == "2")
            {
                Console.WriteLine("How many circles would you like to enter?");
                int numberOfCircles = int.Parse(Console.ReadLine());

                for (int i = 0; i < numberOfCircles; i++)
                {
                    Console.WriteLine($"Enter details for circle {i + 1}:");

                    Console.Write("Enter X: ");
                    double x = double.Parse(Console.ReadLine());

                    Console.Write("Enter Y: ");
                    double y = double.Parse(Console.ReadLine());

                    Console.Write("Enter R: ");
                    double r = double.Parse(Console.ReadLine());

                    mCircles.Add(new Circle(x, y, r));
                }
            }
            else
            {
                Console.WriteLine("Invalid choice.");
            }

            return mCircles;
        }

        static List<Circle>[] SearchForIntersections(List<Circle> circles) 
        {
            List<Circle>[] circleClusters = new List<Circle>[circles.Count];
            for (int i = 0; i < circles.Count; i++) { circleClusters[i] = new List<Circle>(); }
            for (int i = 0; i < circles.Count; i++)
            {
                circleClusters[i].Add(circles[i]);
            }
            for (int i = 0; i < circles.Count; i++)
            {
                for (int j = i + 1; j < circles.Count; j++)
                {
                    if (Circle.Intersects(circles[i], circles[j]) == true && circles[i] != circles[j])
                    {
                        circleClusters[i].Add(circles[j]);
                    }
                }
            }
//-------------------------------------------------------------------------------------------------------------------------------------------------------------
            for (int clusterIndexA = 0; clusterIndexA < circleClusters.Length; clusterIndexA++)
            {
                for (int clusterIndexB = clusterIndexA + 1; clusterIndexB < circleClusters.Length; clusterIndexB++)
                {
                    bool shouldMergeClusters = false;


                    foreach (var circleA in circleClusters[clusterIndexA])
                    {
                        foreach (var circleB in circleClusters[clusterIndexB])
                        {
                            if (Circle.Intersects(circleA, circleB))
                            {
                                shouldMergeClusters = true;
                                break;
                            }
                        }
                        if (shouldMergeClusters) break;
                    }


                    if (shouldMergeClusters)
                    {
                        HashSet<Circle> mergedGroup = new HashSet<Circle>(circleClusters[clusterIndexA]);
                        mergedGroup.UnionWith(circleClusters[clusterIndexB]);

                        circleClusters[clusterIndexA] = mergedGroup.ToList();
                        circleClusters[clusterIndexB].Clear();
                    }
                }
            }
            return circleClusters;
        }
        static void PrintResult(List<Circle>[] circleGraph) 
        {
            Console.WriteLine("----------------------");
            foreach (var i in circleGraph)
            {
                foreach (var j in i)
                {
                    Console.WriteLine($"x={j.X} y={j.Y} r={j.R}");
                }
                Console.WriteLine("----------------------");
            }
            Console.ReadLine();
        }
        static void Main(string[] args)
        {
            List<Circle> circles = new List<Circle>();
            circles= ChoiceCircles();
            List<Circle>[] circleGraph = new List<Circle>[circles.Count];
            circleGraph = SearchForIntersections(circles);
            circleGraph = circleGraph.Where(g => g.Count > 0).ToArray();
            PrintResult(circleGraph);
        }
    }
}