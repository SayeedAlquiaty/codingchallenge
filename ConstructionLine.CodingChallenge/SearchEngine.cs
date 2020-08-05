using System.Collections.Generic;
using System.Linq;

namespace ConstructionLine.CodingChallenge
{
    public class OptionKey
    {
        public string Size { get; set; }

        public string Color { get; set; }

    }
    public class SearchEngine
    {
        private readonly List<Shirt> _shirts;
        private readonly Dictionary<string, List<Shirt>> _optionShirts;

        private List<SizeCount>  sizeCounts = new List<SizeCount>();
        private List<ColorCount> colorCounts = new List<ColorCount>();

        public SearchEngine(List<Shirt> shirts)
        {
            _shirts = shirts;
            // TODO: data preparation and initialisation of additional data structures to improve performance goes here.
            _optionShirts = new Dictionary<string, List<Shirt>>();
            foreach (var shirt in shirts)
            {
                var sizeColor = shirt.Size.Name + shirt.Color.Name;
                if(_optionShirts.ContainsKey(sizeColor))
                {
                    _optionShirts.First(x => x.Key == sizeColor).Value.Add(shirt);
                }
                else
                {
                    _optionShirts.Add(sizeColor, new List<Shirt> { shirt });

                }
            }

            foreach(var size in Size.All)
            {
                sizeCounts.Add(new SizeCount { Size = size, Count = 0 });
            }

            foreach(var color in Color.All)
            {
                colorCounts.Add(new ColorCount { Color = color, Count = 0 });
            }

        }


        public SearchResults Search(SearchOptions options)
        {
            // TODO: search logic goes here.
            var optionSizes = options.Sizes.Count > 0 ? options.Sizes : Size.All;
            var optionColors = options.Colors.Count > 0 ? options.Colors : Color.All;

            var shirts = new List<Shirt>();

            foreach (var size in optionSizes)
            {
                foreach (var color in optionColors)
                {
                    int count = 0;
                    var key = size.Name + color.Name;


                    var list_shirt = _optionShirts.SingleOrDefault(x => x.Key == key).Value;
                    if(list_shirt != null)
                    {
                        count = list_shirt.Count();
                        shirts.AddRange(list_shirt);
                    }

                    var tempSizeCount = sizeCounts.SingleOrDefault(x => x.Size.Name == size.Name);
                    if (tempSizeCount != null)
                    {
                        tempSizeCount.Count += count;
                    }

                    var tempColorCount = colorCounts.SingleOrDefault(x => x.Color.Name == color.Name);
                    if (tempColorCount != null)
                    {
                        tempColorCount.Count += count;
                    }

                }
            }
            return new SearchResults
            {
                Shirts = shirts, SizeCounts = sizeCounts, ColorCounts = colorCounts
            };
        }
    }
}