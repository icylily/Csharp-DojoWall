using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DojoWall.Models;
using  Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace DojoWall.Controllers
{
    public class HomeController : Controller
    {
        private WallContext dbContext;
        public HomeController(WallContext context)
        {
            dbContext = context;
        }
        
        [Route("")]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("Register")]
        public IActionResult Register(NewUser newUser)
        {
            if (!ModelState.IsValid)
            {
                return View("Index");
            }
            else
            {
                if (dbContext.Users.Any(u => u.Email == newUser.Email))
                {
                    ModelState.AddModelError("Email", "Email already in use!");
                    return View("Index");
                }
                else
                {
                    PasswordHasher<NewUser> Hasher = new PasswordHasher<NewUser>();
                    newUser.Password = Hasher.HashPassword(newUser, newUser.Password);
                    User addUser = newUser.GetNewuser();
                    dbContext.Users.Add(addUser);
                    dbContext.SaveChanges();
                    Loginuser.SetLogin(HttpContext, addUser.UserId);
                    return Redirect("Dashboard");
                }
            }
        }

        [HttpPost("Login")]
        public IActionResult Login(Loginuser newLoginUser)
        {
            if (!ModelState.IsValid)
            {
                return View("Index");
            }
            else
            {
                User needLogin = dbContext.Users.FirstOrDefault(u => u.Email == newLoginUser.LogEmail);
                if (needLogin == null)
                {
                    ModelState.AddModelError("LogEmail", "This email didn't exist.Please rigester first!");
                    return View("Index");
                }
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                var verifyPass = Hasher.VerifyHashedPassword(needLogin, needLogin.Password, newLoginUser.LogPassword);
                if (verifyPass == 0)
                {
                    ModelState.AddModelError("LogPassword", "Password is wrong!");
                    return View("Index");
                }
                else
                {
                    Loginuser.SetLogin(HttpContext, needLogin.UserId);
                    return Redirect("Dashboard");
                }
            }
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return View("Index");
        }

        [HttpGet("Dashboard")]
        public IActionResult Dashboard()
        {

            int UserId = Loginuser.GetUserID(HttpContext);
            if (UserId == 0)
            {
                ViewBag.message = " Need Register or login first!";
                return View("Warning");
            }
            Wall DojoWall = SetupWall(UserId);
            return View(DojoWall);
        }

        public Wall SetupWall(int userId)
        {
            Wall setupWall = new Wall();
            setupWall.User = dbContext.Users.FirstOrDefault(user => user.UserId == userId);
            setupWall.AllMessages = dbContext.Messages
            .Include(message => message.Creator)
            .Include(message => message.OwnedComments).ToList();
            return setupWall;
        }

        [HttpPost("/NewMessage")]
        public IActionResult NewMessage(Wall wall)
        {
            int UserId = Loginuser.GetUserID(HttpContext);
            if (UserId == 0)
            {
                ViewBag.message = " Need Register or login first!";
                return View("Warning");
            }
            Message message = new Message();
            message.UserId = wall.UserId;
            message.MessageContent = wall.NewMessage;
            dbContext.Messages.Add(message);
            dbContext.SaveChanges();
            return Redirect("/Dashboard");
        }

        [HttpPost("/NewComment")]
        public IActionResult NewComment(Wall wall)
        {
            int UserId = Loginuser.GetUserID(HttpContext);
            if (UserId == 0)
            {
                ViewBag.message = " Need Register or login first!";
                return View("Warning");
            }
            Comment comment = new Comment();
            comment.UserId = wall.UserId;
            comment.MessageId = wall.MessageId;
            comment.CommentContent = wall.NewComment;
            dbContext.Comments.Add(comment);
            dbContext.SaveChanges();
            return Redirect("/Dashboard");
        }

        [HttpGet("/deleteComment/{commentId}")]
        public IActionResult DeleteComment(int commentId)
        {
            int UserId = Loginuser.GetUserID(HttpContext);
            if (UserId == 0)
            {
                ViewBag.message = " Need Register or login first!";
                return View("Warning");
            }
            Comment thisComment = dbContext.Comments.FirstOrDefault(com => com.CommentId == commentId);
            if(thisComment.UserId != UserId)
            {
                ViewBag.message = " You only could delete the commet that created by yourself!";
                return View("Warning");
            }
            if(DateTime.Now.Subtract(thisComment.CreatedAt).TotalMinutes >=30)
            {
                ViewBag.message = " You only could delete the commet that  was made in the last 30 minutes.!";
                return View("Warning");
            }
            dbContext.Comments.Remove(thisComment);
            dbContext.SaveChanges();
            return Redirect("/Dashboard");
        }


    }
}
