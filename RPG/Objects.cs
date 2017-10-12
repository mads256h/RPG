﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RPG.Extensions;
using RPG.Properties;

namespace RPG.Objects
{
    public sealed class Wall : PictureBox
    {
        public static List<Wall> Walls = new List<Wall>();
        public Wall(int x1, int y1, int x2, int y2) : base()
        {
            BackColor = Color.White;
            Location = new Point(x1, y1);
            Size = new Size(x2 - x1, y2 - y1);
            Walls.Add(this);
        }

        public bool Intersects(Rectangle bounds)
        {
            return Bounds.IntersectsWith(bounds);
        }
    }

    public sealed class Grass : PictureBox
    {
        public static List<Grass> Grasses = new List<Grass>();

        public int EncounterRate;

        public Grass(int x1, int y1, int x2, int y2, int encounterRate) : base()
        {
            BackColor = Color.DarkGreen;
            Location = new Point(x1, y1);
            Size = new Size(x2 - x1, y2 - y1);
            this.SetImage(Resources.Grass);
            EncounterRate = encounterRate;
            Grasses.Add(this);
        }

        public bool Intersects(Rectangle bounds)
        {
            if (Bounds.IntersectsWith(bounds))
            {
                if (EncounterRate != 0)
                {
                    if (Program.Random.Next(0, EncounterRate) == 0)
                    {
                        //Start battle
                    }
                }
                return true;
            }
            return false;
        }
    }

    public sealed class Entrance
    {
        public static List<Entrance> Entrances = new List<Entrance>();

        public int LevelID;
        public Rectangle Bounds;

        public Entrance(int x1, int y1, int x2, int y2, int levelID)
        {
            LevelID = levelID;
            Bounds.Location = new Point(x1, y1);
            Bounds.Size = new Size(x2 - x1, y2 - y1);
            Entrances.Add(this);
        }

        public bool Intersects(Rectangle bounds)
        {
            if (Bounds.IntersectsWith(bounds))
            {
                Level.LoadLevel(LevelID);
                return true;
            }
            return false;

        }
    }

    public sealed class Door : PictureBox
    {
        public static List<Door> Doors = new List<Door>();

        public int ID;

        public bool Open {
            get => _open;
            set
            {
                Logger.WriteLine($"Door {ID} open state changed to: {value}");
                Visible = !value;
                _open = value;
            }
        }

        private bool _open = false;

        public Door(int x1,int y1, int x2, int y2, int id) : base()
        {
            BackColor = Color.SaddleBrown;
            Location = new Point(x1, y1);
            Size = new Size(x2 - x1, y2 - y1);
            ID = id;
            Doors.Add(this);
        }


        public bool Intersects(Rectangle bounds)
        {
            return Bounds.IntersectsWith(bounds);
        }
    }

    public sealed class Key : PictureBox
    {
        public static List<Key> Keys = new List<Key>();

        public int ID;

        public Key(int x1, int y1, int x2, int y2, int id) : base()
        {
            BackColor = Color.Transparent;
            Image = Properties.Resources.Key;
            Location = new Point(x1, y1);
            Size = new Size(x2 - x1, y2 - y1);
            ID = id;
            Keys.Add(this);
        }

        public bool Intersects(Rectangle bounds)
        {
            if (Bounds.IntersectsWith(bounds) && Visible)
            {
                foreach (var door in Door.Doors)
                {
                    if (door.ID == ID)
                        door.Open = true;
                }
                FormOverworld.SoundPlayer.Stream = Resources.testsound;
                FormOverworld.SoundPlayer.Play();
                Visible = false;
                return true;
            }
            return false;
            
        }
    }

    public class Floor : PictureBox
    {
        private static readonly Image Dirt = Resources.Dirt;
        private static readonly Image Tiles = Resources.Tiles;
        public enum FloorTexture : int
        {
            Dirt = 0,
            Tiles = 1
        }

        public static List<Floor> Floors = new List<Floor>();

        public FloorTexture floorTexture;

        public Floor(int x1, int y1, int x2, int y2, FloorTexture floor) : base()
        {
            BackColor = Color.Brown;
            Location = new Point(x1, y1);
            Size = new Size(x2 - x1, y2 - y1);
            System.Drawing.Image temp;
            floorTexture = floor;
            switch (floorTexture)
            {
                case FloorTexture.Dirt:
                    temp = Dirt;
                    break;
                case FloorTexture.Tiles:
                    temp = Tiles;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(floor), floor, null);
            }
            this.SetImage(temp);
            Floors.Add(this);
        }

    }
}