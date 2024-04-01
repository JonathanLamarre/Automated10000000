using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Automated10000000
{
    public class Program
    {
        private static readonly Color WAND = Color.FromArgb(255, 106, 75, 21);
        private static readonly Color SWORD = Color.FromArgb(255, 247, 247, 247);
        private static readonly Color KEY = Color.FromArgb(255, 147, 147, 147);
        private static readonly Color ROCHE = Color.FromArgb(255, 37, 43, 52);
        private static readonly Color BOIS = Color.FromArgb(255, 190, 122, 76);
        private static readonly Color SHIELD = Color.FromArgb(255, 189, 189, 186);
        private static readonly Color BOX = Color.FromArgb(255, 189, 189, 186);

        public static void Main(string[] arg)
        {
            HashSet<Color> actionsToFind;

            if (arg.Length > 0 && arg[0] == "attack")
            {
                actionsToFind = new HashSet<Color> {WAND, SWORD};
            }
            else if (arg.Length > 0 && arg[0] == "key")
            {
                actionsToFind = new HashSet<Color> {KEY};
            }
            else
            {
                actionsToFind = new HashSet<Color> {ROCHE, BOIS, SHIELD, BOX};
            }

            string runningLocation = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string runningLocationDirectory = Path.GetDirectoryName(runningLocation) ?? throw new InvalidOperationException();
            string iconLocation = Path.Combine(runningLocationDirectory, "Icon10000000.bmp");
            Bitmap iconOf10000000Program = (Bitmap)Image.FromFile(iconLocation);
            Bitmap screenshot = Tools.GetFullScreenshot();
            Point? point = ImageFinder.Find(screenshot, iconOf10000000Program);

            if (point == null) return;

            var grid = new Color[8,7];
            int xMove = 0;

            for (int i = 0; i < 8; i++)
            {
                int yMove = 0;

                for (int j = 0; j < 7; j++)
                {
                    Rectangle rectangle = new Rectangle(point.Value.X + 287 + i*84 + xMove, point.Value.Y + 159 + j*84 + yMove, 84, 84);
                    Bitmap tile = screenshot.Clone(rectangle, screenshot.PixelFormat );
                    //tile.Save($"tile{i}{j}.jpg", ImageFormat.Jpeg);
                    grid[i,j] = tile.GetPixel(42, 42);
                    yMove += j == 1 || j == 6 ? 3 : 4;
                }

                xMove += i == 0 || i == 6 ? 3 : 4;
            }

            (bool, int startx, int starty, int endx, int endy) FindHorizontalMatch()
            {
                for (int i = 0; i < 6; i++)
                {
                    for (int j = 0; j < 7; j++)
                    {
                        if (actionsToFind.Contains(grid[i, j]) && grid[i, j] == grid[i + 1, j])
                        {
                            for (int k = 0; k < 7; k++)
                            {
                                if (k == j) continue;

                                if (grid[i, j] == grid[i + 2, k]) return (true, i + 2, k, i + 2, j);
                            }
                        }
                        else if (actionsToFind.Contains(grid[i, j]) && grid[i, j] == grid[i + 2, j])
                        {
                            for (int k = 0; k < 7; k++)
                            {
                                if (k == j) continue;

                                if (grid[i, j] == grid[i + 1, k]) return (true, i + 1, k, i + 1, j);
                            }
                        }
                        else if (actionsToFind.Contains(grid[i + 1, j]) && grid[i + 1, j] == grid[i + 2, j])
                        {
                            for (int k = 0; k < 7; k++)
                            {
                                if (k == j) continue;

                                if (grid[i + 1, j] == grid[i, k]) return (true, i, k, i, j);
                            }
                        }
                    }
                }

                return (false, 0, 0, 0, 0);
            }

            (bool, int startx, int starty, int endx, int endy) FindVerticalMatch()
               {
                   for (int j = 0; j < 5; j++)
                   {
                       for (int i = 0; i < 8; i++)
                       {
                           if (actionsToFind.Contains(grid[i, j]) && grid[i, j] == grid[i, j + 1])
                           {
                               for (int k = 0; k < 8; k++)
                               {
                                   if (k == i) continue;

                                   if (grid[i, j] == grid[k, j + 2]) return (true, k, j + 2, i, j + 2);
                               }
                           }
                           else if (actionsToFind.Contains(grid[i, j]) && grid[i, j] == grid[i, j + 2])
                           {
                               for (int k = 0; k < 8; k++)
                               {
                                   if (k == i) continue;

                                   if (grid[i, j] == grid[k, j + 1]) return (true, k, j + 1, i, j + 1);
                               }
                           }
                           else if (actionsToFind.Contains(grid[i, j + 1]) && grid[i, j + 1] == grid[i, j + 2])
                           {
                               for (int k = 0; k < 8; k++)
                               {
                                   if (k == i) continue;

                                   if (grid[i, j + 1] == grid[k, j]) return (true, k, j, i, j);
                               }
                           }
                       }
                   }

                   return (false, 0, 0, 0, 0);
               }

            (bool foundMatch, int startx, int starty, int endx, int endy) = FindHorizontalMatch();

            if (foundMatch)
            {
                DragAndDrop.Drag
                (
                    point.Value.X + 287 + startx * 88 + 40,
                    point.Value.Y + 159 + starty*88 + 40,
                    point.Value.X + 287 + endx * 88 + 40,
                    point.Value.Y + 159 + endy*88 + 40,
                    TimeSpan.FromMilliseconds(5)
                );

                return;
            }

            (foundMatch, startx, starty, endx, endy) = FindVerticalMatch();

            if (foundMatch)
            {
                DragAndDrop.Drag
                (
                    point.Value.X + 287 + startx * 88 + 40,
                    point.Value.Y + 159 + starty*88 + 40,
                    point.Value.X + 287 + endx * 88 + 40,
                    point.Value.Y + 159 + endy*88 + 40,
                    TimeSpan.FromMilliseconds(5)
                );
            }
        }
    }
}