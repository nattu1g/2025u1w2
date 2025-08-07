using System.Collections.Generic;
using Scripts.Models;
using UnityEngine;

namespace Scripts.Vcontainer.Entity
{
    public class PlayerClubEntity
    {
        private List<Student> students = new List<Student>();
        public List<Student> Students => students;
    }
}
