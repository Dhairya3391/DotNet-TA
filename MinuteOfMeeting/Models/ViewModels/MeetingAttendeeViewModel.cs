namespace MinuteOfMeeting.Models.ViewModels
{
    public class MeetingAttendeeViewModel
    {
        public int StaffID { get; set; }
        public string StaffName { get; set; }
        public string EmailAddress { get; set; }
        public bool IsInvited { get; set; }
        public bool IsPresent { get; set; }
        public string Remarks { get; set; }
    }
}
