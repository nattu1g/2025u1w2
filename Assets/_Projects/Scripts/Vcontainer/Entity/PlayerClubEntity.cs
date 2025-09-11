using System.Collections.Generic;
using BBSim.Models;

namespace BBSim.Vcontainer.Entity
{
    public class PlayerClubEntity
    {
        private List<Student> students = new List<Student>();
        public List<Student> Students => students;
    }
}
