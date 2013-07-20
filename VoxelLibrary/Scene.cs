using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VoxelLand
{
    public class Scene : IEnumerable<Entity>
    {
        public Scene()
        {
            entities = new List<Entity>();
        }

        public void Add(Entity e)
        {
            entities.Add(e);
        }

        public IEnumerator<Entity> GetEnumerator()
        {
            return entities.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private List<Entity> entities;
    }
}
