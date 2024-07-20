using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using trainingTask2.Models;

namespace trainingTask2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private static List<User> users = new List<User>
        {
            new User { Id = 1, FirstName = "Layan", LastName = "Alfar", DateOfBirth = new DateTime(2003, 1, 7), Email = "layanalfar0@gmail.com" }
        };

        [HttpGet(Name = "GetUsers")]
        public ActionResult<IEnumerable<User>> GetUsers()
        {
            return Ok(users);
        }

        [HttpGet("{id}", Name = "GetUser")]
        public ActionResult<User> GetUser(int id)
        {
            User user = null;
            foreach (var u in users)
            {
                if (u.Id == id)
                {
                    user = u;
                    break;
                }
            }

            if (user == null)
            {
                return NotFound(new { message = $"User with ID {id} not found." });
            }
            return Ok(user);
        }

        [HttpPost(Name = "CreateUser")]
        public ActionResult<User> PostUser(User user)
        {
            if (user == null || string.IsNullOrWhiteSpace(user.FirstName) || string.IsNullOrWhiteSpace(user.LastName))
            {
                return BadRequest(new { message = "Invalid user data. First name and last name are required." });
            }

            user.Id = GetNextId();
            users.Add(user);
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        [HttpPut("{id}", Name = "UpdateUser")]
        public IActionResult PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest(new { message = "User ID mismatch." });
            }

            User existingUser = null;
            foreach (var u in users)
            {
                if (u.Id == id)
                {
                    existingUser = u;
                    break;
                }
            }

            if (existingUser == null)
            {
                return NotFound(new { message = $"User with ID {id} not found." });
            }

            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.DateOfBirth = user.DateOfBirth;
            existingUser.Email = user.Email;

            return NoContent();
        }

        [HttpDelete("{id}", Name = "DeleteUser")]
        public IActionResult DeleteUser(int id)
        {
            User user = null;
            foreach (var u in users)
            {
                if (u.Id == id)
                {
                    user = u;
                    break;
                }
            }

            if (user == null)
            {
                return NotFound(new { message = $"User with ID {id} not found." });
            }

            users.Remove(user);
            return NoContent();
        }

        private int GetNextId()
        {
            int maxId = 0;
            foreach (var u in users)
            {
                if (u.Id > maxId)
                {
                    maxId = u.Id;
                }
            }
            return maxId + 1;
        }
    }
}