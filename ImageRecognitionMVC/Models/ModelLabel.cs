namespace DefectsMVC.Models
{
    public class ModelLabel
    {
        public int Id { get; set; }

        public string Label { get; set; }

        public string Name { get; set; }

        public int Score { get; set; }

        public static List<ModelLabel> GetLabels()
        {
            return new List<ModelLabel>()
                {
                    new ModelLabel { Id = 0, Label = "Поясная складка ", Name = "waist folding" },
                    new ModelLabel { Id = 1, Label = "Прокатная ямка", Name = "rolled_pit" },

                };
        }
    }
}
