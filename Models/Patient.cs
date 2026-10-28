namespace JournalSys.Models;

public class Patient
{
        public int Id { get; set; }
        public string Fornavn { get; set; } = "";
        public string Etternavn { get; set; } = "";
        public DateTime Fodselsdato { get; set; }
        public string Telefon { get; set; } = "";
}
