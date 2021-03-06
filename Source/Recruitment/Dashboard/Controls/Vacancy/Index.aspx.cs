﻿
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Action;

namespace Recruitment.Dashboard.Controls.Vacancy
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Index : System.Web.UI.Page
    {
        private readonly Vacancys _vacancy = new Vacancys();
        private readonly Applicant _applicant = new Applicant();
        private readonly Action.Schedule _schedule = new Action.Schedule();
        protected int Count = 1;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FetchAllListVancancys();

                // bin data for dropdownlist interviewer
                dropdownListInterviewer.DataSource = FetchListRoleInterviewer();

                dropdownListInterviewer.DataTextField = "Admin_Account";
                dropdownListInterviewer.DataValueField = "Admin_Id";
                dropdownListInterviewer.DataBind();
            }
        }

        /// <summary>
        /// Fetches all list vancancys.
        /// </summary>
        public void FetchAllListVancancys()
        {
            RadGridListVacancys.DataSource = _vacancy.FetchAllListVancancys();
            RadGridListVacancys.DataBind();
        }

        /// <summary>
        /// Handles the Click event of the lbtnSubmitAddnew control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void lbtnSubmitAddnew_Click(object sender, EventArgs e)
        {
            var vacancys = new Share.Vacancy
                {
                    Vacancy_Name = txtName.Text,
                    Vacancy_WorkAddress = txtAddress.Text,
                    Vacancy_Salary = txtsalary.Text,
                    Vacancy_Positions = txtposition.Text,
                    Vacancy_NumberPeople = Convert.ToInt32(txtnumberpeople.Text),
                    Vacancy_Skill = txtskill.Text,
                    Vacancy_Exp = txtxexp.Text,
                    Vacancy_TypeTime = dropTypeTime.SelectedValue,
                    Vacancy_Gender = dropGender.SelectedValue,
                    Vacancy_Age = txtage.Text,
                    Vacancy_Description = txtDescription.Text,
                    Vacancy_DateUp = DateTime.Now.ToString(),
                    Vacancy_DateStart = rdDate_Start.SelectedDate.ToString().Replace("12:00:00 AM", ""),
                    Vacancy_DateEnd = rdDate_End.SelectedDate.ToString().Replace("12:00:00 AM", "")
                };
            // create
            Panel_ContentAddnew.Visible = false;

            _vacancy.CreateNewVacancys(vacancys);
            // reaload datasource
            FetchAllListVancancys();
            Ltr_action.Text = "<script>CloseAddnewVacancys();" + "</script>";
        }

        /// <summary>
        /// LBTN_s the addnew_ click.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void lbtn_Addnew_Click(object sender, EventArgs e)
        {
            Panel_ContentAddnew.Visible = true;
            panelCreateInterviewer.Visible = false;
        }

        /// <summary>
        /// Fetches the schedule id by vacancys id.
        /// </summary>
        /// <param name="vacancysId">The vacancys id.</param>
        /// <returns></returns>
        public int FetchScheduleIdByVacancysId(int vacancysId)
        {
            return _vacancy.FetchScheduleIdByVacancysId(vacancysId);
        }

        public static int ResultVacancysBySchedule = 0;

        /// <summary>
        /// Filters the vacancys by schedule.
        /// </summary>
        /// <returns></returns>
        public string FilterVacancysBySchedule(int vacancyId)
        {
            ResultVacancysBySchedule = FetchScheduleIdByVacancysId(vacancyId);
            return FetchCountApplicantAdminConfirm(vacancyId) +
                   FetchCountApplicantAdminNotConfirm(vacancyId) +
                   "<a class='btn' href='CheckApplicant.aspx?AppliId=" + vacancyId + "'>View</a>";
        }

        /// <summary>
        /// Filters the vacancys by schedule.
        /// </summary>
        /// <returns></returns>
        public string FilterVacancysHaveOrNotByvacancyId(int vacancyId, int scheduleId, int adminId)
        {
            ResultVacancysBySchedule = FetchScheduleIdByVacancysId(vacancyId);
            if (ResultVacancysBySchedule == 0)
            {
                return "<span class='label'>not scheduling" + "</span>";
            }
            return FetchDateScheduleByScheduleId(scheduleId) + ""
                + FetchTimeOnlyByVacancyId(vacancyId) + FetchUserNameAdminByAdminId(adminId);
        }

        public string FetchUserNameAdminByAdminId(int adminId)
        {
            var admin = new Admin();
            return "&nbsp;[<a style='color:#205D80' href='#'>" + admin.FetchUserNameAdminByAdminId(adminId) + "</a>]";
        }

        /// <summary>
        /// Fetches the time only by vacancy id.
        /// </summary>
        /// <param name="vacancysId">The vacancys id.</param>
        /// <returns></returns>
        public string FetchTimeOnlyByVacancyId(int vacancysId)
        {
            return _vacancy.FetchTimeOnlyByVacancyId(vacancysId);
        }

        /// <summary>
        /// Fetches the date schedule by schedule id.
        /// </summary>
        /// <param name="scheduleId">The schedule id.</param>
        /// <returns></returns>
        public String FetchDateScheduleByScheduleId(int scheduleId)
        {
            return _schedule.FetchDateScheduleByScheduleId(scheduleId);
        }

        /// <summary>
        /// Filters the vacancys by schedule show create interviewer.
        /// </summary>
        /// <param name="vacancyId">The vacancy id.</param>
        /// <returns></returns>
        public bool FilterVacancysByScheduleShowCreateInterviewer(int vacancyId)
        {
            if (FetchScheduleIdByVacancysId(vacancyId) == 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Fetches the count applicant admin confirm.
        /// </summary>
        /// <param name="vacancysId">The vacancys id.</param>
        /// <returns></returns>
        public string FetchCountApplicantAdminConfirm(int vacancysId)
        {
            int st = _applicant.FetchCountApplicantAdminConfirm(vacancysId);
            if (st != 0)
            {
                return "<span class='label label-success' title='has confirm'>"
                       + st + "</span>&nbsp;";
            }
            return string.Empty;
        }

        /// <summary>
        /// Fetches the count applicant admin not confirm.
        /// </summary>
        /// <param name="vacancysId">The vacancys id.</param>
        /// <returns></returns>
        public string FetchCountApplicantAdminNotConfirm(int vacancysId)
        {
            var st = _applicant.FetchCountApplicantAdminNotConfirm(vacancysId);
            if (st != 0)
            {
                return "<span class='label label-warning' title='waiting confirm'>"
                       + st + "</span>&nbsp;";
            }
            return string.Empty;
        }

        /// <summary>
        /// Creates the inter viewer.
        /// </summary>
        /// <param name="vacancys">The vacancys.</param>
        /// <param name="dateInterViewer">The date inter viewer.</param>
        public void CreateInterViewer(Share.Vacancy vacancys, string dateInterViewer)
        {
            var interviewer = new Interview();
            interviewer.CreateInterViewer(vacancys, dateInterViewer);
        }

        private static int _idVacancys;
        /// <summary>
        /// Handles the Click event of the lbtn_IntervierThis control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        public void lbtn_IntervierThis_Click(object sender, EventArgs e)
        {
            Panel_ContentAddnew.Visible = false;
            panelCreateInterviewer.Visible = true;

            var lbtn = (LinkButton)sender;
            _idVacancys = Convert.ToInt32(lbtn.CommandArgument);
        }

        /// <summary>
        /// Handles the Click event of the btnCreateInterViewer control.
        /// </summary>
        /// <param name="sendr">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnCreateInterViewer_Click(object sendr, EventArgs e)
        {
            var vacancys = new Share.Vacancy
                {
                    Vacancy_Id = _idVacancys,
                    Vacancy_DateInterViewer = RadDatePicker_DateInter.SelectedDate.ToString().Replace("12:00:00 AM", "").Replace("23:00:00", ""),
                    Vacancy_TimeInterViewer = RadTimePicker_TimeInter.SelectedTime.ToString(),
                    Admin_Id = Convert.ToInt32(dropdownListInterviewer.SelectedValue)
                };

            CreateInterViewer(vacancys, RadDatePicker_DateInter.SelectedDate.ToString().Replace("23:00:00", "").Replace("12:00:00 AM", ""));
            panelCreateInterviewer.Visible = false;

            // reload datasource
            FetchAllListVancancys();
        }

        /// <summary>
        /// Handles the Click event of the hidePanelCreateInter control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void hidePanelCreateInter_Click(object sender, EventArgs e)
        {
            panelCreateInterviewer.Visible = false;
        }

        /// <summary>
        /// Fetches the list role interviewer.
        /// </summary>
        /// <returns></returns>
        public List<Share.Admin> FetchListRoleInterviewer()
        {
            var admin = new Admin();
            return admin.FetchListRoleInterviewer();
        }

        /// <summary>
        /// Deletes the vacancys by vacancys id.
        /// </summary>
        /// <param name="vacancysId">The vacancys id.</param>
        public void DeleteVacancysByVacancysId(int vacancysId)
        {
            _vacancy.DeleteVacancysByVacancysId(vacancysId);
        }

        /// <summary>
        /// Handles the Click event of the lbtnRemove control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        public void lbtnRemove_Click(object sender, EventArgs e)
        {
            var lbtn = (LinkButton)sender;
            DeleteVacancysByVacancysId(Convert.ToInt32(lbtn.CommandArgument));
            // reload
            FetchAllListVancancys();
        }
    }
}