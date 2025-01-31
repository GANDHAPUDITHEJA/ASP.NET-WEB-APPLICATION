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

                // Check if StudentId exists in the Students table
                //string checkStudentQuery = "SELECT COUNT(*) FROM StudentRegistrations WHERE StudentId = @StudentId";
                //using (SqlCommand checkStudentCmd = new SqlCommand(checkStudentQuery, con))
                //{
                //    checkStudentCmd.Parameters.AddWithValue("@StudentId", studentId);
                //    int studentCount = (int)checkStudentCmd.ExecuteScalar();

                //    if (studentCount == 0)
                //    {
                //        TempData["Message"] = "Invalid Student ID!";
                //        return RedirectToAction("RegisterStudent", new { placementId });
                //    }
                //}

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
    }
}
