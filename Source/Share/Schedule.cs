//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Share
{
    using System;
    using System.Collections.Generic;
    
    public partial class Schedule
    {
        public Schedule()
        {
            this.Interviewers = new HashSet<Interviewer>();
        }
    
        public int Schedule_Id { get; set; }
        public string Schedule_Date { get; set; }
        public string Schedule_Time { get; set; }
        public Nullable<int> Vacancy_Id { get; set; }
        public Nullable<int> Admin_Id { get; set; }
    
        public virtual Admin Admin { get; set; }
        public virtual ICollection<Interviewer> Interviewers { get; set; }
        public virtual Vacancy Vacancy { get; set; }
    }
}
