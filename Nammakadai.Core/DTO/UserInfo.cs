namespace Nammakadai.Core.DTO
{
    public class UserInfo
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserMail { get; set; }
        public string AlternativeMail { get; set; }
        public string PhoneNumber { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set;}
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;

    }
}
