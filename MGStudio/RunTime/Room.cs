using MGStudio.BaseObjects;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGStudio.RunTime
{
    public class Room : BaseRoom, IGameOverload
    {
        public int TileWidth { get; set; } = 32;
        public int TileHeight { get; set; } = 32;

        public List<Entity> ActivatedEntities = new List<Entity>();

        public Tile[,] Tiles;

        public EntityService MovedObjects = new EntityService();
        public EntityService VisibleObjects = new EntityService();

        public Color BackColor;

        private ProjectGame Parent;

        public Entity MoveObject(Entity entity, int x, int y)
        {
            if (entity == null)
                return null;

            var tile = Tiles[x, y];
            if(tile.Items == null)
            {
                tile.Items = new List<Entity>();
            }
            else
            {
                if (tile.Items.Contains(entity))
                    return entity;
            }

            if (!entity.__New)
            {
                var previousTile = GetTileIndex(entity.__X, entity.__Y);
                var prevTile = Tiles[previousTile.x, previousTile.y];
                if(prevTile.Items != null && prevTile.Items.Contains(entity))
                {
                    prevTile.Items.Remove(entity);
                }
            }
            else
            {
                entity.__New = false;
            }

            tile.Items.Add(entity);

            entity.__X = entity.X;
            entity.__Y = entity.Y;

            return entity;
        }

        public Room(ProjectGame parent)
        {
            Parent = parent;

            Tiles = new Tile[Width, Height];

            Entity.MovedService = MovedObjects;
        }

        public virtual IEnumerable<Entity> GetEntities(float x, float y)
        {

            var tileVector = GetTileIndex(x, y);
            
            var tiles = Tiles[tileVector.x, tileVector.y];

            if (tiles.Items != null)
                yield return null;
            else
            {
                foreach (var entity in tiles.Items)
                {
                    yield return entity;
                }
            }        
        }

        public void ValidateScreenPosistions(ref float _x, ref float _y)
        {
            if (_x < 0)
                _x = 0;
            if (_y < 0)
                _y = 0;

            if (_x > Width)
                _x = Width;

            if (_y > Height)
                _y = Height;
        }

        public (int x, int y) GetTileIndex(float _x, float _y)
        {
            ValidateScreenPosistions(ref _x, ref _y);

            if (_x != 0)
                _x /= TileWidth;

            if (_y != 0)
                _y /= TileHeight;

            return ((int)_x, (int)_y);
        }

        public Entity CreateEntity(Type type, float _x, float _y)
        {
            var newEntity = Activator.CreateInstance(type) as Entity;

            ValidateScreenPosistions(ref _x, ref _y);

            ActivatedEntities.Add(newEntity);

            newEntity.X = _x;
            newEntity.Y= _y;

            return newEntity;
        }

        public Entity DeleteEntity(Entity entity)
        {
            if (entity == null)
                return null;

            if (ActivatedEntities.Contains(entity))
                ActivatedEntities.Remove(entity);

            if(!entity.__New)
            {
                var tileVector = GetTileIndex(entity.__X, entity.__Y);

                var tile = Tiles[tileVector.x, tileVector.y];
                if (tile.Items != null && tile.Items.Contains(entity))
                    tile.Items.Remove(entity);
                entity.__Deleted = true;
                entity.Delete();
            }

            return entity;
        }

        public void Update(GameTime gameTime)
        {
            int length = ActivatedEntities.Count;
            for (int i = 0; i < length; i++)
            {
                ActivatedEntities[i].Step(gameTime);
            }
        }

        public void Draw(GameTime gameTime)
        {
            
        }
    }
}
