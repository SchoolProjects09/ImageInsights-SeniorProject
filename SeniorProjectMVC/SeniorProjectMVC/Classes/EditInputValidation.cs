namespace SeniorProjectMVC
{
	public class EditInputValidation
	{
        public bool Validated { get; set; }
        public bool NameFail { get; set; }
        public bool DescriptionFail { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? ErrorMsg { get; set; }
        public int ID { get; set; }

        public EditInputValidation(FileEditFormInput input, int iD)
        {
            Name = input.Name;
            Description = input.Description;
            ID = iD;
        }

        public bool Validate()
        {
            Validated = true;

            if (String.IsNullOrWhiteSpace(Name)
                || 0 >= Name.Length || Name.Length > 50)
            {
                NameFail = true;
                Validated = false;
            }
            if (String.IsNullOrWhiteSpace(Description)
                || 0 >= Description.Length || Description.Length > 250)
            {
                DescriptionFail = true;
                Validated = false;
            }

            return Validated;
        }
    }
}
