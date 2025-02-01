using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Mvc;
using Project_1.Models;

namespace Project_1.Controllers
{
    public class PlacementController : Controller
    {
        string connectionString = @"Server=3.6.100.92;Database=Placement;User Id=Intern1;Password=Hilip@123;TrustServerCertificate=True;";

        // Register a new student for a placement (GET)
        public ActionResult RegisterStudent(int placementId)
        {
            var registration = new StudentRegistration
            {
                PlacementId = placementId,
                RegisteredOn = DateTime.Now,
                Status = 1 // Default status as 'New'
            };

            return View(registration);
        }

        // Register student for the placement (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegisterStudent(StudentRegistration registration, int studentId, int placementId)
        {
            // Validate placementId
            if (placementId == 0)
            {
                TempData["Message"] = "Placement ID is required!";
                return RedirectToAction("RegisterStudent", new { placementId });
            }

            if (studentId == 0)
            {
                TempData["Message"] = "Student ID is required!";
                return RedirectToAction("RegisterStudent", new { placementId });
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                // Check if PlacementId exists in PlacementDetails
                string checkPlacementQuery = "SELECT COUNT(*) FROM PlacementDetails WHERE PlacementId = @PlacementId";
                using (SqlCommand checkPlacementCmd = new SqlCommand(checkPlacementQuery, con))
                {
                    checkPlacementCmd.Parameters.AddWithValue("@PlacementId", placementId);
                    int placementCount = (int)checkPlacementCmd.ExecuteScalar();

                    if (placementCount == 0)
                    {
                        TempData["Message"] = "Invalid Placement ID!";
                        return RedirectToAction("RegisterStudent", new { placementId });
                    }
                }

                // Check if Student is already registered for this Placement
                string checkDuplicateQuery = "SELECT COUNT(*) FROM StudentRegistrations WHERE StudentId = @StudentId AND PlacementId = @PlacementId";
                using (SqlCommand checkDuplicateCmd = new SqlCommand(checkDuplicateQuery, con))
                {
                    checkDuplicateCmd.Parameters.AddWithValue("@StudentId", studentId);
                    checkDuplicateCmd.Parameters.AddWithValue("@PlacementId", placementId);
                    int duplicateCount = (int)checkDuplicateCmd.ExecuteScalar();

                    if (duplicateCount > 0)
                    {
                        TempData["Message"] = "Student ID is already registered for this Placement!";
                        return RedirectToAction("RegisterStudent", new { placementId });
                    }
                }

                // Insert the new student registration
                string query = "INSERT INTO StudentRegistrations (StudentId, PlacementId, RegisteredOn, Status) VALUES (@StudentId, @PlacementId, @RegisteredOn, @Status)";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@StudentId", studentId);
                    cmd.Parameters.AddWithValue("@PlacementId", placementId);
                    cmd.Parameters.AddWithValue("@RegisteredOn", DateTime.Now);
                    cmd.Parameters.AddWithValue("@Status", 1); // Default status as 'New'
                    cmd.ExecuteNonQuery();
                }

                TempData["Message"] = "Student registered successfully!";
            }

            return RedirectToAction("Student_Registration", new { placementId });
        }

        // Fetch and display registered students for a placement
        public ActionResult Student_Registration(int placementId)
        {
            List<StudentRegistration> registeredStudents = new List<StudentRegistration>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = "SELECT StudentId, PlacementId, RegisteredOn, Status FROM StudentRegistrations WHERE PlacementId = @PlacementId";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@PlacementId", placementId);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    registeredStudents.Add(new StudentRegistration
                    {
                        StudentId = Convert.ToInt32(reader["StudentId"]),
                        PlacementId = Convert.ToInt32(reader["PlacementId"]),
                        RegisteredOn = Convert.ToDateTime(reader["RegisteredOn"]),
                        Status = Convert.ToInt32(reader["Status"])
                    });
                }
            }

            return View(registeredStudents);
        }
        public ActionResult ScheduleInterview()
        {
            return View();
        }

        // POST: Placement/ScheduleInterview
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ScheduleInterview(InterviewSchedule interview)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    // Check if StudentId exists in StudentRegistration table
                    string checkStudentQuery = "SELECT COUNT(*) FROM StudentRegistrations WHERE StudentId = @StudentId";
                    using (SqlCommand checkStudentCmd = new SqlCommand(checkStudentQuery, con))
                    {
                        checkStudentCmd.Parameters.AddWithValue("@StudentId", interview.StudentId);
                        int studentCount = (int)checkStudentCmd.ExecuteScalar();

                        if (studentCount == 0)
                        {
                            // Student ID not found, show popup
                            TempData["ErrorMessage"] = "Invalid Student ID!";
                            return View(interview);
                        }
                    }

                    // Check if ScheduleDate is valid (not in the past)
                    if (interview.ScheduleDate < DateTime.Now)
                    {
                        TempData["ErrorMessage"] = "ScheduleDate cannot be in the past!";
                        return View(interview);
                    }

                    // Insert new interview with only InterviewScheduleId, StudentId, and ScheduleDate
                    string insertQuery = @"
                INSERT INTO InterviewSchedule (
                    StudentId,
                    ScheduleDate
                )
                VALUES (
                    @StudentId,
                    @ScheduleDate
                );";

                    using (SqlCommand cmd = new SqlCommand(insertQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@StudentId", interview.StudentId);
                        cmd.Parameters.AddWithValue("@ScheduleDate", interview.ScheduleDate);
                        cmd.ExecuteNonQuery();
                    }

                    TempData["Message"] = "Interview scheduled successfully!";
                }

                return RedirectToAction("ScheduleInterview");
            }

            return View(interview);
        }
        public ActionResult UpdateInterview(int? interviewId)
        {
            if (!interviewId.HasValue)
            {
                TempData["ErrorMessage"] = "Interview ID is required!";
                return RedirectToAction("ViewInterviews");
            }

            InterviewSchedule interview = GetInterviewById(interviewId.Value);

            if (interview == null)
            {
                TempData["ErrorMessage"] = "Interview not found!";
                return RedirectToAction("ViewInterviews");
            }

            return View(interview); // Pass a single InterviewSchedule object
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateInterview(InterviewSchedule interview)
        {
            if (interview == null || interview.InterviewScheduleId <= 0)
            {
                TempData["ErrorMessage"] = "Invalid interview data!";
                return RedirectToAction("ViewInterviews");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        con.Open();

                        // Update all fields for the interview
                        string query = @"
                    UPDATE InterviewSchedule 
                    SET 
                        ScheduleDate = @ScheduleDate, 
                        IsAppeared = @IsAppeared, 
                        IsSelected = @IsSelected, 
                        Package = @Package, 
                        JoiningDate = @JoiningDate 
                    WHERE InterviewScheduleId = @InterviewScheduleId;";

                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.Parameters.AddWithValue("@ScheduleDate", interview.ScheduleDate);
                            cmd.Parameters.AddWithValue("@IsAppeared", interview.IsAppeared);
                            cmd.Parameters.AddWithValue("@IsSelected", interview.IsSelected);
                            cmd.Parameters.AddWithValue("@Package", interview.Package ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@JoiningDate", interview.JoiningDate ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@InterviewScheduleId", interview.InterviewScheduleId);

                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                TempData["Message"] = "Interview updated successfully!";
                            }
                            else
                            {
                                TempData["ErrorMessage"] = "Failed to update interview!";
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log the exception (ex) here
                    TempData["ErrorMessage"] = "An error occurred while updating the interview.";
                }

                return RedirectToAction("ViewInterviews");
            }

            return View(interview); // Pass a single InterviewSchedule object
        }

        // GET: ViewInterviews
        public ActionResult ViewInterviews()
        {
            List<InterviewSchedule> interviews = new List<InterviewSchedule>();

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    // Query to fetch all interviews
                    string query = @"
                SELECT 
                    InterviewScheduleId, 
                    StudentId, 
                    ScheduleDate, 
                    IsAppeared, 
                    IsSelected, 
                    Package, 
                    JoiningDate 
                FROM InterviewSchedule;";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                interviews.Add(new InterviewSchedule
                                {
                                    InterviewScheduleId = reader.GetInt32(0),
                                    StudentId = reader.GetInt32(1),
                                    ScheduleDate = reader.GetDateTime(2),
                                    IsAppeared = reader.GetBoolean(3),
                                    IsSelected = reader.GetBoolean(4),
                                    Package = reader.IsDBNull(5) ? (decimal?)null : reader.GetDecimal(5),
                                    JoiningDate = reader.IsDBNull(6) ? (DateTime?)null : reader.GetDateTime(6)
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception (ex) here
                TempData["ErrorMessage"] = "An error occurred while fetching interviews.";
            }

            return View(interviews); // Pass a list of InterviewSchedule objects
        }

        private InterviewSchedule GetInterviewById(int interviewId)
        {
            InterviewSchedule interview = null;

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    string query = @"
                SELECT 
                    InterviewScheduleId, 
                    StudentId, 
                    ScheduleDate, 
                    IsAppeared, 
                    IsSelected, 
                    Package, 
                    JoiningDate 
                FROM InterviewSchedule 
                WHERE InterviewScheduleId = @InterviewScheduleId;";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@InterviewScheduleId", interviewId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                interview = new InterviewSchedule
                                {
                                    InterviewScheduleId = reader.GetInt32(0),
                                    StudentId = reader.GetInt32(1),
                                    ScheduleDate = reader.GetDateTime(2),
                                    IsAppeared = reader.GetBoolean(3),
                                    IsSelected = reader.GetBoolean(4),
                                    Package = reader.IsDBNull(5) ? (decimal?)null : reader.GetDecimal(5),
                                    JoiningDate = reader.IsDBNull(6) ? (DateTime?)null : reader.GetDateTime(6)
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception (ex) here
                TempData["ErrorMessage"] = "An error occurred while fetching the interview.";
            }

            return interview; // Return a single InterviewSchedule object
        }
    }
}
