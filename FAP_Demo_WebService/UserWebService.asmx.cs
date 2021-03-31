﻿using System;
using System.Collections.Generic;
using System.Web.Services;
using eCert.Models.Entity;
using FAP_Demo_WebService.Models;

namespace FAP_Demo_WebService
{
    /// <summary>
    /// Summary description for UserWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class UserWebService : System.Web.Services.WebService
    {
        [WebMethod]
        public User GetUserByAcademicEmail(string academicEmail)
        {
            if(academicEmail == "hainnhe130585@fpt.edu.vn")
            {
                return new User() { AcademicEmail = "hainnhe130585@fpt.edu.vn", DOB = new DateTime(1999, 1, 11), Ethnicity = "Kinh", Gender = false, PhoneNumber = "0969043389", RollNumber = "HE130585", MemberCode = "hainnhe130585", FullName = "Nguyễn Ngọc Hải" };
            }else if(academicEmail == "hapthe130576@fpt.edu.vn")
            {
                return new User() { AcademicEmail = "hapthe130576@fpt.edu.vn", DOB = new DateTime(1999, 9, 26), Ethnicity = "Kinh", Gender = false, PhoneNumber = "0382181359", RollNumber = "HE130576", MemberCode = "hapthe130576", FullName = "Phạm Thanh Hà" };
            }else if(academicEmail == "tuannmhe130642@fpt.edu.vn")
            {
                return new User() { AcademicEmail = "tuannmhe130642@fpt.edu.vn", DOB = new DateTime(1999, 1, 11), Ethnicity = "Kinh", Gender = false, PhoneNumber = "0123456789", RollNumber = "HE130642", MemberCode = "tuannmhe130642", FullName = "Nguyễn Minh Tuấn" };
            }
            
            return null;
        }

        [WebMethod]
        public User GetUserByRollNumber(string rollNumber)
        {
            if (rollNumber == "he130585")
            {
                return new User() { AcademicEmail = "hainnhe130585@fpt.edu.vn", DOB = new DateTime(1999, 1, 11), Ethnicity = "Kinh", Gender = false, PhoneNumber = "0969043389", RollNumber = "HE130585", FullName = "Nguyễn Ngọc Hải" };
            }
            else if (rollNumber == "he130576")
            {
                return new User() { AcademicEmail = "hapthe130576@fpt.edu.vn", DOB = new DateTime(1999, 9, 26), Ethnicity = "Kinh", Gender = false, PhoneNumber = "0382181359", RollNumber = "HE130576", FullName = "Phạm Thanh Hà" };
            }
            else if (rollNumber == "he130642")
            {
                return new User() { AcademicEmail = "tuannmhe130642@fpt.edu.vn", DOB = new DateTime(1999, 1, 11), Ethnicity = "Kinh", Gender = false, PhoneNumber = "0123456789", RollNumber = "HE130642", FullName = "Nguyễn Minh Tuấn" };
            }
            return null;
        }

        //Get student's passed subject
        [WebMethod]
        public List<Subject> GetPassedSubjects(string rollNumber)
        {
            if(rollNumber == "HE130576")
            {
                return new List<Subject>()
                {
                    new Subject(){Semester = "Spring 2019", SubjectCode = "PRN292", SubjectName = ".NET and C#", Mark = 7.5f},
                    new Subject(){Semester = "Fall 2019", SubjectCode = "SWR301", SubjectName = "Software Requirements", Mark = 7.0f},
                    new Subject(){Semester = "Fall 2019", SubjectCode = "SWQ391", SubjectName = "Software Quality Assurance and Testing", Mark = 6.5f},
                };
            }
            else
            {
                return new List<Subject>()
                {
                    new Subject(){Semester = "Fall 2019", SubjectCode = "PRN292", SubjectName = ".NET and C#", Mark = 6.4f},
                    new Subject(){Semester = "Fall 2019", SubjectCode = "SWR301", SubjectName = "Software Requirements", Mark = 7.0f},
                    new Subject(){Semester = "Fall 2019", SubjectCode = "SWQ391", SubjectName = "Software Quality Assurance and Testing", Mark = 7.6f},
                    new Subject(){Semester = "Fall 2019", SubjectCode = "JPD131", SubjectName = "Elementary Japanese 2.1", Mark = 6.0f},
                    new Subject(){Semester = "Summer 2020", SubjectCode = "PRO192", SubjectName = "Mobile Programming", Mark = 5.8f},
                };
            }
            
        }

        //Get detail of a passed subject
        [WebMethod]
        public Subject GetDetailPassedSubject(string rollnumber, string subjectCode)
        {
            if(rollnumber == "HE130576" && subjectCode == "PRN292")
            {
                return new Subject() { Semester = "Spring 2019", SubjectCode = "PRN292", SubjectName = ".NET and C#", Mark = 7.5f, StudentFullName = "Phạm Thanh Hà" };
            }else if(rollnumber == "HE130576" && subjectCode == "SWR301")
            {
                return new Subject() { Semester = "Fall 2019", SubjectCode = "SWR301", SubjectName = "Software Requirements", Mark = 7.0f, StudentFullName = "Phạm Thanh Hà" };
            }else if(rollnumber == "HE130576" && subjectCode == "SWQ391")
            {
                return new Subject() { Semester = "Fall 2019", SubjectCode = "SWQ391", SubjectName = "Software Quality Assurance and Testing", Mark = 6.5f, StudentFullName = "Phạm Thanh Hà đổi tên" };
            }
            else if (rollnumber == "HE130576" && subjectCode == "JPD131")
            {
                return new Subject() { Semester = "Fall 2019", SubjectCode = "JPD131", SubjectName = "Elementary Japanese 2.1", Mark = 7.4f, StudentFullName = "Phạm Thanh Hà đổi tên 2" };
            }

            return null;
        }

    }
}


//Lấy danh sách sinh viên của mỗi kì tuyển sinh mới, ví dụ K13, K14. Sau đó gen password cho mỗi account và lưu vào database eCert
/*
 * - Nên chia ra First, Middle, Last hay lấy toàn bộ tên dưới dạng full name?
 * - Có cần lấy các thông tin như
 */
//[WebMethod]
//public List<User> GetUserList(string khoá)
//{
//    return new List<User>()
//    {
//        new User(){FirstName = "Pham", MiddleName = "Thanh", LastName = "Ha", Gender = true, DOB = new DateTime(1999, 9, 26), PhoneNumber = "0382181359", PersonalEmail = "ha@mail.com", AcademicEmail = "hapthe130576@fpt.edu.vn", RollNumber = "HE130576"},
//        new User(){FirstName = "Ha", MiddleName = "Phuc", LastName = "Tri", Gender = true, DOB = new DateTime(1999, 6, 9), PhoneNumber = "0948989687", PersonalEmail = "tri@mail.com", AcademicEmail = "trihphe130589@fpt.edu.vn", RollNumber = "HE130589"},
//        new User(){FirstName = "Hoang", MiddleName = "Viet", LastName = "Bach", Gender = true, DOB = new DateTime(1999, 12, 1), PhoneNumber = "0969043389", PersonalEmail = "bach@mail.com", AcademicEmail = "bachhvhe130603@fpt.edu.vn", RollNumber = "HE130603"},
//        new User(){FirstName = "Nguyen", MiddleName = "Minh", LastName = "Tuan", Gender = true, DOB = new DateTime(1999, 1, 11), PhoneNumber = "0343143697", PersonalEmail = "tuan@mail.com", AcademicEmail = "tuannmhe130642@fpt.edu.vn", RollNumber = "HE130642"},
//        new User(){FirstName = "Nguyen", MiddleName = "Ngoc", LastName = "Hai", Gender = true, DOB = new DateTime(1999, 8, 3), PhoneNumber = "0334530595", PersonalEmail = "hai@mail.com", AcademicEmail = "hainnhe130585@fpt.edu.vn", RollNumber = "HE130585"},
//        new User(){FirstName = "Le", MiddleName = "Phuong", LastName = "Ngoc", Gender = false, DOB = new DateTime(1999, 7, 14), PhoneNumber = "0957314600", PersonalEmail = "ngoc@mail.com", AcademicEmail = "ngoclphe130001@fpt.edu.vn", RollNumber = "HE130001"},
//        new User(){FirstName = "Tran", MiddleName = "Binh", LastName = "Minh", Gender = true, DOB = new DateTime(1999, 1, 22), PhoneNumber = "0364419247", PersonalEmail = "minh@mail.com", AcademicEmail = "minhtbhe130493@fpt.edu.vn", RollNumber = "HE130493"},
//        new User(){FirstName = "Hoang", MiddleName = "Lan", LastName = "Nhi", Gender = false, DOB = new DateTime(1999, 4, 8), PhoneNumber = "0983371084", PersonalEmail = "nhi@mail.com", AcademicEmail = "nhihlhe130712@fpt.edu.vn", RollNumber = "HE130712"},
//        new User(){FirstName = "Phan", MiddleName = "Lac", LastName = "Duong", Gender = true, DOB = new DateTime(1999, 5, 24), PhoneNumber = "0365509704", PersonalEmail = "duong@mail.com", AcademicEmail = "duongplhe130313@fpt.edu.vn", RollNumber = "HE130313"},
//        new User(){FirstName = "Vo", MiddleName = "Dai", LastName = "Ton", Gender = true, DOB = new DateTime(1999, 10, 19), PhoneNumber = "0984437810", PersonalEmail = "ton@mail.com", AcademicEmail = "tonvdhe130247@fpt.edu.vn", RollNumber = "HE130247"},
//        new User(){FirstName = "Le", MiddleName = "Huy", LastName = "Hoang", Gender = true, DOB = new DateTime(1999, 2, 28), PhoneNumber = "0913299087", PersonalEmail = "hoang@mail.com", AcademicEmail = "hoanglhhe130547@fpt.edu.vn", RollNumber = "HE130547"},
//        new User(){FirstName = "Nguyen", MiddleName = "Quynh", LastName = "Anh", Gender = false, DOB = new DateTime(1999, 4, 18), PhoneNumber = "0346257921", PersonalEmail = "quynhanh@mail.com", AcademicEmail = "anhnqhe130648@fpt.edu.vn", RollNumber = "HE130648"},
//        new User(){FirstName = "Pham", MiddleName = "Cam", LastName = "Ly", Gender = false, DOB = new DateTime(1999, 4, 30), PhoneNumber = "0913019832", PersonalEmail = "ly@mail.com", AcademicEmail = "lypche130241@fpt.edu.vn", RollNumber = "HE130241"},
//        new User(){FirstName = "Le", MiddleName = "Dai", LastName = "Nghia", Gender = true, DOB = new DateTime(1999, 5, 22), PhoneNumber = "0384513790", PersonalEmail = "nghia@mail.com", AcademicEmail = "nghialdhe130024@fpt.edu.vn", RollNumber = "HE130024"},
//        new User(){FirstName = "Nguyen", MiddleName = "Trong", LastName = "Phung", Gender = true, DOB = new DateTime(1999, 3, 12), PhoneNumber = "0371648209", PersonalEmail = "phung@mail.com", AcademicEmail = "phungnthe130517@fpt.edu.vn", RollNumber = "HE130517"},
//        new User(){FirstName = "Ha", MiddleName = "Lan", LastName = "Ngoc", Gender = false, DOB = new DateTime(1999, 9, 1), PhoneNumber = "0941687300", PersonalEmail = "lanngoc@mail.com", AcademicEmail = "ngochlhe130367@fpt.edu.vn", RollNumber = "HE130367"},
//        new User(){FirstName = "Tran", MiddleName = "Xuan", LastName = "Bach", Gender = true, DOB = new DateTime(1999, 2, 1), PhoneNumber = "0361054763", PersonalEmail = "xuanbach@mail.com", AcademicEmail = "bachtxhe130198@fpt.edu.vn", RollNumber = "HE130198"},
//        new User(){FirstName = "Vu", MiddleName = "Dinh", LastName = "Trong", Gender = true, DOB = new DateTime(1999, 11, 3), PhoneNumber = "0324516873", PersonalEmail = "trongdz@mail.com", AcademicEmail = "trongvdhe130240fpt.edu.vn", RollNumber = "HE130240"},
//        new User(){FirstName = "Nguyen", MiddleName = "Phuong", LastName = "Anh", Gender = false, DOB = new DateTime(1999, 11, 8), PhoneNumber = "0341387601", PersonalEmail = "panh@mail.com", AcademicEmail = "anhnphe130045@fpt.edu.vn", RollNumber = "HE130045"},
//        new User(){FirstName = "Hoang", MiddleName = "Nam", LastName = "Long", Gender = true, DOB = new DateTime(1999, 6, 29), PhoneNumber = "0984137081", PersonalEmail = "longpro@mail.com", AcademicEmail = "longhnhe130069@fpt.edu.vn", RollNumber = "HE130069"}
//    };

//}