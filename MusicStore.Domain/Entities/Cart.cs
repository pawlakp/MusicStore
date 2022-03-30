using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Domain.Entities
{
    public class CartLine
    {
        public AlbumAllDetails Product { get; set; }
    }
    public class Cart
    {

        private List<CartLine> lineCollection = new List<CartLine>();

        public void AddItem(AlbumAllDetails product)
        {
            CartLine line = lineCollection.Where(p => p.Product.album.AlbumId == product.album.AlbumId).FirstOrDefault();
          
            if(lineCollection.Contains(line)==false)lineCollection.Add(new CartLine { Product = product });
            
           
        }

        public void RemoveLine(AlbumAllDetails product)
        {
            lineCollection.RemoveAll(p => p.Product.album.AlbumId == product.album.AlbumId);
        }

        public decimal ComputeTotalValue()
        {
            return lineCollection.Sum(e => e.Product.album.Price );
        }

        public void Clear()
        {
            lineCollection.Clear();
        }

        public IEnumerable<CartLine> Lines
        {
            get { return lineCollection; }
        }

    }
}
