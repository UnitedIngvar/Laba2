using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSTEK_parser.Model
{
    class ThreatModel
    {
        //General Info
        private int _id;
        private string _name;
        private string _description;
        private string _source;
        private string _object;

        //Additional info
        private string _conf;
        private string _sust;
        private string _access;

        //Dates of edit and adding
        private DateTime _editDate;
        private DateTime _inDate;



        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public string Source
        {
            get { return _source; }
            set { _source = value; }
        }

        public string Object
        {
            get { return _object; }
            set { _object = value; }
        }

        public string Conf
        {
            get { if (_conf == "1") return "Да"; else return "Нет" ; }
            set { _conf = value; }
        }

        public string Sust
        {
            get { if (_sust == "1") return "Да"; else return "Нет"; }
            set { _sust = value; }
        }

        public string Access
        {
            get { if (_access == "1") return "Да"; else return "Нет"; }
            set { _access = value; }
        }
        public DateTime InDate
        {
            get { return _inDate; }
            set { _inDate = value; }
        }

        public DateTime EditDate
        {
            get { return _editDate; }
            set { _editDate = value; }
        }
    }
}
